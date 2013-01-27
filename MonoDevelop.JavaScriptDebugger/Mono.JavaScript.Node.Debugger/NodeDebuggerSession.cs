using System;
using System.Globalization;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Mono.Debugging.Client;
using Mono.JavaScript.Node.Debugger;
using TypeScriptServiceBridge;

namespace Mono.JavaScript.Node.Debugger
{
	class NodeDebuggerSession: DebuggerSession
	{
		Process proc;
		//StreamWriter sin;
		NodeCommandResult lastResult;
		bool running;
		// While there is no thread support, it is messy to remove all relevant code.
		long currentThread = -1;
		long activeThread = -1;
		//bool isMonoProcess;
		string currentProcessName;
		List<string> tempVariableObjects = new List<string> ();
		Dictionary<string,BreakEventInfo> breakpoints = new Dictionary<string,BreakEventInfo> ();
		List<BreakEventInfo> breakpointsWithHitCount = new List<BreakEventInfo> ();
		
		DateTime lastBreakEventUpdate = DateTime.Now;
		Dictionary<int, WaitCallback> breakUpdates = new Dictionary<int,WaitCallback> ();
		bool breakUpdateEventsQueued;
		const int BreakEventUpdateNotifyDelay = 500;
		
		bool internalStop;
		bool logNode;
		
		object syncLock = new object ();
		object eventLock = new object ();
		object nodeLock = new object ();
		ManualResetEventSlim bootstrap_lock = new ManualResetEventSlim (false);
		string node_path;
		V8DebuggerProtocolClient debugger;
		
		public NodeDebuggerSession (string nodePath)
		{
			node_path = nodePath;
			logNode = true;//!string.IsNullOrEmpty (Environment.GetEnvironmentVariable ("MONODEVELOP_NODE_LOG"));
		}
		
		protected override void OnRun (DebuggerStartInfo startInfo)
		{
			lock (nodeLock) {
				StartNodeDebugger (startInfo.Arguments);

				currentProcessName = startInfo.Command + " " + startInfo.Arguments;

				OnStarted ();

				debugger.Continue ();
			}
		}
		
		protected override void OnAttachToProcess (long processId)
		{
			throw new NotSupportedException ();
		}

		void StartNodeDebugger (string args)
		{
			proc = new Process ();
			proc.StartInfo.FileName = node_path;
			proc.StartInfo.Arguments = args;
			proc.StartInfo.UseShellExecute = false;
			proc.StartInfo.RedirectStandardInput = true;
			proc.StartInfo.RedirectStandardOutput = true;
			proc.StartInfo.RedirectStandardError = true;
			proc.StartInfo.EnvironmentVariables ["NODE_DISABLE_COLORS"] = "1";
			proc.OutputDataReceived += (sender, e) => ProcessOutput (e.Data);
			proc.ErrorDataReceived += (sender, e) => ProcessError (e.Data);

			lock (nodeLock) {
				proc.Start ();
			
//				sin = proc.StandardInput;
				proc.BeginOutputReadLine ();
				proc.BeginErrorReadLine ();

				if (!bootstrap_lock.Wait (8000))
					throw new InvalidOperationException ("Debugger did not start");
				if (logNode)
					LogWriter (false, "connected to node debugger");
			}

			debugger = new V8DebuggerProtocolClient (JurassicTypeHosting.Engine);
			debugger.Break += HandleBreak;
			debugger.UncaughtException += HandleUncaughtException;
			debugger.Start ();
		}

		void HandleUncaughtException (TypeScriptServiceBridge.V8Debugger.DebuggerEvent obj)
		{
			throw new NotSupportedException ();
		}

		void HandleBreak (TypeScriptServiceBridge.V8Debugger.DebuggerEvent obj)
		{
			throw new NotSupportedException ();
		}

		public event Action Disposing;
		
		public override void Dispose ()
		{
			if (Disposing != null)
				Disposing ();
			if (proc != null)
				proc.Kill ();
		}
		
		protected override void OnSetActiveThread (long processId, long threadId)
		{
			activeThread = threadId;
		}
		
		protected override void OnStop ()
		{
			if (!proc.HasExited)
				proc.Kill ();
			//Syscall.kill (proc.Id, Signum.SIGINT);
		}
		
		protected override void OnDetach ()
		{
			throw new NotSupportedException ();
		}
		
		protected override void OnExit ()
		{
			lock (nodeLock) {
				InternalStop ();
				proc.Kill ();
				TargetEventArgs args = new TargetEventArgs (TargetEventType.TargetExited);
				OnTargetEvent (args);
			}
		}
		
		protected override void OnStepLine ()
		{
			SelectThread (activeThread);
			RunCommand ("next");
		}
		
		protected override void OnNextLine ()
		{
			// can't differentiate from next on node.
			SelectThread (activeThread);
			RunCommand ("next");
		}
		
		protected override void OnStepInstruction ()
		{
			SelectThread (activeThread);
			RunCommand ("step");
		}
		
		protected override void OnNextInstruction ()
		{
			SelectThread (activeThread);
			RunCommand ("-exec-next-instruction");
		}
		
		protected override void OnFinish ()
		{
			SelectThread (activeThread);
			/*
			NodeCommandResult res = RunCommand ("-stack-info-depth", "2");
			if (res.GetValue ("depth") == "1") {
				RunCommand ("-exec-continue");
			} else {
				RunCommand ("-stack-select-frame", "0");
				RunCommand ("-exec-finish");
			}		
			*/
		}
		
		protected override BreakEventInfo OnInsertBreakEvent (BreakEvent be)
		{
			Breakpoint bp = be as Breakpoint;
			if (bp == null)
				throw new NotSupportedException ();
			
			BreakEventInfo bi = new BreakEventInfo ();
			
			lock (nodeLock) {
				bool dres = InternalStop ();
				try {
					string handle;
					try {
						if (bp is FunctionBreakpoint) {
							var bf = (FunctionBreakpoint) bp;
							handle = "function " + bf.FunctionName;
							var ret = debugger.SetBreakpoint ("function", bf.FunctionName, be.Enabled, -1, -1, bp.ConditionExpression, bp.HitCount);
						} else {
							handle = string.Format ("{0} ({1},{2})", bp.FileName, bp.Line, bp.Column);
							var ret = debugger.SetBreakpoint ("script", bp.FileName, be.Enabled, bp.Line, bp.Column, bp.ConditionExpression, bp.HitCount);
						}
					} catch (Exception ex) {
						bi.SetStatus (BreakEventStatus.Invalid, ex.Message);
						return bi;
					}
					bi.Handle = handle;
					bi.SetStatus (BreakEventStatus.Bound, null);
					return bi;
				} finally {
					InternalResume (dres);
				}
			}
		}
		
		bool CheckBreakpoint (string handle)
		{
			BreakEventInfo binfo;
			if (!breakpoints.TryGetValue (handle, out binfo))
				return true;
			
			Breakpoint bp = (Breakpoint) binfo.BreakEvent;

			// node debugger has no conditional breakpoints.
			/*
			if (!string.IsNullOrEmpty (bp.ConditionExpression) && bp.BreakIfConditionChanges) {
				// Update the condition expression
				NodeCommandResult res = RunCommand ("-data-evaluate-expression", Escape (bp.ConditionExpression));
				string val = res.GetValue ("value");
				RunCommand ("-break-condition", handle.ToString (), "(" + bp.ConditionExpression + ") != " + val);
			}
			*/

			if (bp.HitAction == HitAction.PrintExpression) {
				throw new NotSupportedException ();
				/*
				NodeCommandResult res = RunCommand ("repl", Escape (bp.TraceExpression));
				string val = res.GetValue ("value");
				NotifyBreakEventUpdate (binfo, 0, val);
				return false;
				*/
			}
			return true;
		}
		
		void NotifyBreakEventUpdate (BreakEventInfo binfo, int hitCount, string lastTrace)
		{
			bool notify = false;
			
			WaitCallback nc = delegate {
				if (hitCount != -1)
				binfo.UpdateHitCount (hitCount);
				if (lastTrace != null)
				binfo.UpdateLastTraceValue (lastTrace);
			};
			
			lock (breakUpdates)
			{
				int span = (int) (DateTime.Now - lastBreakEventUpdate).TotalMilliseconds;
				if (span >= BreakEventUpdateNotifyDelay && !breakUpdateEventsQueued) {
					// Last update was more than 0.5s ago. The update can be sent.
					lastBreakEventUpdate = DateTime.Now;
					notify = true;
				} else {
					// Queue the event notifications to avoid wasting too much time
					breakUpdates [(int)binfo.Handle] = nc;
					if (!breakUpdateEventsQueued) {
						breakUpdateEventsQueued = true;
						
						ThreadPool.QueueUserWorkItem (delegate {
							Thread.Sleep (BreakEventUpdateNotifyDelay - span);
							List<WaitCallback> copy;
							lock (breakUpdates) {
								copy = new List<WaitCallback> (breakUpdates.Values);
								breakUpdates.Clear ();
								breakUpdateEventsQueued = false;
								lastBreakEventUpdate = DateTime.Now;
							}
							foreach (WaitCallback wc in copy)
								wc (null);
						});
					}
				}
			}
			if (notify)
				nc (null);
		}
		
		void UpdateHitCountData ()
		{
			throw new NotSupportedException ();
			/*
			foreach (BreakEventInfo bp in breakpointsWithHitCount) {
				NodeCommandResult res = RunCommand ("-break-info", bp.Handle.ToString ());
				string val = res.GetObject ("BreakpointTable").GetObject ("body").GetObject (0).GetObject ("bkpt").GetValue ("ignore");
				if (val != null)
					NotifyBreakEventUpdate (bp, int.Parse (val), null);
				else
					NotifyBreakEventUpdate (bp, 0, null);
			}
			breakpointsWithHitCount.Clear ();
			*/
		}
		
		protected override void OnRemoveBreakEvent (BreakEventInfo binfo)
		{
			lock (nodeLock) {
				if (binfo.Handle == null)
					return;
				bool dres = InternalStop ();
				breakpointsWithHitCount.Remove (binfo);
				breakpoints.Remove ((string) binfo.Handle);
				try {
					RunCommand ("clearBreakpoint", binfo.Handle.ToString ());
				} finally {
					InternalResume (dres);
				}
			}
		}
		
		protected override void OnEnableBreakEvent (BreakEventInfo binfo, bool enable)
		{
			lock (nodeLock) {
				if (binfo.Handle == null)
					return;
				bool dres = InternalStop ();
				try {
					if (enable)
						RunCommand ("setBreakpoint", binfo.Handle.ToString ());
					else
						RunCommand ("clearBreakpoint", binfo.Handle.ToString ());
				} finally {
					InternalResume (dres);
				}
			}
		}
		
		protected override void OnUpdateBreakEvent (BreakEventInfo binfo)
		{
			Breakpoint bp = binfo.BreakEvent as Breakpoint;
			if (bp == null)
				throw new NotSupportedException ();
			
			if (binfo.Handle == null)
				return;
			
			bool ss = InternalStop ();
			
			try {
				if (bp.HitCount > 0) {
					RunCommand ("-break-after", binfo.Handle.ToString (), bp.HitCount.ToString ());
					breakpointsWithHitCount.Add (binfo);
				} else
					breakpointsWithHitCount.Remove (binfo);
				
				if (!string.IsNullOrEmpty (bp.ConditionExpression) && !bp.BreakIfConditionChanges)
					RunCommand ("-break-condition", binfo.Handle.ToString (), bp.ConditionExpression);
				else
					RunCommand ("-break-condition", binfo.Handle.ToString ());
			} finally {
				InternalResume (ss);
			}
		}
		
		protected override void OnContinue ()
		{
			SelectThread (activeThread);
			debugger.Continue ();
		}
		
		protected override ThreadInfo[] OnGetThreads (long processId)
		{
			List<ThreadInfo> list = new List<ThreadInfo> ();
			// node debugger has no threading operation, so allways return 0.
			list.Add (GetThread (0));
			/*
			ResultData data = RunCommand ("-thread-list-ids").GetObject ("thread-ids");
			foreach (string id in data.GetAllValues ("thread-id"))
				list.Add (GetThread (long.Parse (id)));
			*/
			return list.ToArray ();
		}
		
		protected override ProcessInfo[] OnGetProcesses ()
		{
			ProcessInfo p = new ProcessInfo (0, currentProcessName);
			return new ProcessInfo [] { p };
		}
		
		ThreadInfo GetThread (long id)
		{
			return new ThreadInfo (0, id, "Thread #" + id, null);
		}

		protected override Backtrace OnGetThreadBacktrace (long processId, long threadId)
		{
			ResultData data = SelectThread (threadId);
			NodeCommandResult res = RunCommand ("backtrace");
			int fcount = int.Parse (res.GetValue ("depth"));
			NodeBacktrace bt = new NodeBacktrace (this, threadId, fcount, data != null ? data.GetObject ("frame") : null);
			return new Backtrace (bt);
		}

		public ResultData SelectThread (long id)
		{
			if (id == currentThread)
				return null;
			throw new InvalidOperationException ("node debugger has no thread operation");
			/*
			currentThread = id;
			return RunCommand ("-thread-select", id.ToString ());
			*/
		}
		
		string Escape (string str)
		{
			if (str == null)
				return null;
			else if (str.IndexOf (' ') != -1 || str.IndexOf ('"') != -1) {
				str = str.Replace ("\"", "\\\"");
				return "\"" + str + "\"";
			}
			else
				return str;
		}
		
		public NodeCommandResult RunCommand (string command, params string[] args)
		{
			throw new NotImplementedException ();
			/*
			lock (nodeLock) {
				lock (syncLock) {
					lastResult = null;
					
					lock (eventLock) {
						running = true;
					}
					
					if (logNode)
						Console.WriteLine ("node debugger<: " + command + " " + string.Join (" ", args));

					sin.WriteLine (command + " " + string.Join (" ", args));
					sin.Flush ();

					if (!Monitor.Wait (syncLock, 10000))
						throw new InvalidOperationException ("Command execution timeout: " + command + " " + string.Join (" ", args));
					if (lastResult.Status == CommandStatus.Error)
						throw new InvalidOperationException (lastResult.Data);
					return lastResult;
				}
			}
			*/
		}
		
		bool InternalStop ()
		{
			// on gdb it would make some sense. on node no point of killing the debuggee.
			/*
			lock (eventLock) {
				if (!running)
					return false;
				internalStop = true;
				RunCommand ("kill");
				if (!Monitor.Wait (eventLock, 10000))
					throw new InvalidOperationException ("Target could not be interrupted.");
			}
			*/
			return true;
		}
		
		void InternalResume (bool resume)
		{
			if (resume)
				RunCommand ("cont");
		}
		
		void ProcessError (string line)
		{
			if (line == null)
				return;
			
			if (bootstrap_step < 2) {
				if (bootstrap_step < 1 && line == "debugger listening on port 5858") {
					bootstrap_step++;
					input_contd = null;
					bootstrap_lock.Set ();
					return;
				}
				/*
				if (bootstrap_step < 2 && line == "connecting... ok") {
					bootstrap_step++;
					input_contd = null;
					bootstrap_lock.Set ();
					return;
				}
				*/
				input_contd += line;
				return;
			}

			throw new InvalidOperationException (line);
		}

		bool output_contd = false;
		string stored_output = null;
		void ProcessOutput (string line)
		{
			try {
				DoProcessOutput (line);
			} catch (Exception ex) {
				if (logNode && LogWriter != null)
					LogWriter (false, ex.ToString ());
			}
		}

		int bootstrap_step;
		string input_contd;

		void DoProcessOutput (string line)
		{
			if (line == null)
				return;
			if (logNode && LogWriter != null)
				LogWriter (false, line);
			if (line.StartsWith ("debug> ")) {
				DoProcessOutput (line.Substring ("debug> ".Length));
				return;
			}
			if (line.StartsWith ("... ")) {
				DoProcessOutput (line.Substring ("... ".Length));
				return;
			}
			var idx = line.IndexOf ('\b');
			if (idx < 0) {
				// ignore them, extraneous information such as default stack trace.
				return;
			}
			if (idx != 0)
				throw new InvalidOperationException ("Unexpected response: " + line);
			line = input_contd + line.Substring (idx + 1);

			if (line.StartsWith ("... \b")) {
				line = line.Substring ("... \b".Length);
			} else {
				stored_output += line + "\n";
			}
			if (running) {
				lock (syncLock) {
					lastResult = new NodeCommandResult (line);
					running = false;
					Monitor.PulseAll (syncLock);
				}
			} else {
				NodeEvent ev;
				lock (eventLock) {
					running = false;
					ev = new NodeEvent (line.Substring (1));
					string ti = ev.GetValue ("thread-id");
					if (ti != null && ti != "all")
						currentThread = activeThread = int.Parse (ti);
					Monitor.PulseAll (eventLock);
					if (internalStop) {
						internalStop = false;
						return;
					}
				}
				ThreadPool.QueueUserWorkItem (delegate {
					try {
						HandleEvent (ev);
					} catch (Exception ex) {
						Console.WriteLine (ex);
					}
				});
			}
		}
		
		void HandleEvent (NodeEvent ev)
		{
			if (ev.Name != "stopped") {
				Console.WriteLine ("Unknown event: " + ev.Name);
				return;
			}
			
			CleanTempVariableObjects ();
			
			TargetEventType type;
			switch (ev.Reason) {
			case "breakpoint-hit":
				type = TargetEventType.TargetHitBreakpoint;
				if (!CheckBreakpoint (ev.GetValue ("bkptno"))) {
					RunCommand ("-exec-continue");
					return;
				}
				break;
			case "signal-received":
				if (ev.GetValue ("signal-name") == "SIGINT")
					type = TargetEventType.TargetInterrupted;
				else
					type = TargetEventType.TargetSignaled;
				break;
			case "exited":
			case "exited-signalled":
			case "exited-normally":
				type = TargetEventType.TargetExited;
				break;
			default:
				type = TargetEventType.TargetStopped;
				break;
			}
			
			ResultData curFrame = ev.GetObject ("frame");
			FireTargetEvent (type, curFrame);
		}
		
		void FireTargetEvent (TargetEventType type, ResultData curFrame)
		{
			UpdateHitCountData ();
			
			TargetEventArgs args = new TargetEventArgs (type);
			
			if (type != TargetEventType.TargetExited) {
				NodeCommandResult res = RunCommand ("backtrace");
				int fcount = int.Parse (res.GetValue ("depth"));
				
				NodeBacktrace bt = new NodeBacktrace (this, activeThread, fcount, curFrame);
				args.Backtrace = new Backtrace (bt);
				args.Thread = GetThread (activeThread);
			}
			OnTargetEvent (args);
		}
		
		internal void RegisterTempVariableObject (string var)
		{
			tempVariableObjects.Add (var);
		}
		
		void CleanTempVariableObjects ()
		{
			foreach (string s in tempVariableObjects)
				RunCommand ("-var-delete", s);
			tempVariableObjects.Clear ();
		}
	}
}
