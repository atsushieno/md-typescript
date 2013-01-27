// Not likely to work with nodejs.
// Anyways using ServiceBridge is totally wrong, it's not running on top of other host.
using System;
using TypeScriptServiceBridge.V8Debugger;
using TypeScriptServiceBridge;
using Jurassic.Library;
using System.Net.Sockets;
using System.IO;
using Jurassic;
using System.Threading;

namespace Mono.JavaScript.Node.Debugger
{
	public class V8DebuggerProtocolClient : IDisposable
	{
		public const int DefaultNodeDebuggerPort = 5858;

		public V8DebuggerProtocolClient (ScriptEngine engine)
			: this (engine, DefaultNodeDebuggerPort)
		{
		}

		public V8DebuggerProtocolClient (ScriptEngine engine, int port)
		{
			this.engine = engine;
			client = new TcpClient ("localhost", port);
			stream = client.GetStream ();
			reader = new StreamReader (stream);
			writer = new StreamWriter (stream);
		}

		public event Action<DebuggerEvent> Break;
		public event Action<DebuggerEvent> UncaughtException;

		public void Start ()
		{
			reader_loop_thread = new Thread (EventLoop);
			reader_loop_thread.Start ();
		}

		protected void OnBreak (DebuggerEvent evt)
		{
			if (Break != null)
				Break (evt);
		}

		protected void OnUncaughtException (DebuggerEvent evt)
		{
			if (UncaughtException != null)
				UncaughtException (evt);
			else
				throw new InvalidOperationException ("V8 debugger returned uncaught exception message", new JavaScriptException (evt.Instance, 0, null));
		}

		Thread reader_loop_thread;
		ManualResetEventSlim reader_finished;
		bool reader_loop = true;
		object reader_lock = new object ();
		ScriptEngine engine;
		TcpClient client;
		NetworkStream stream;
		StreamWriter writer;
		StreamReader reader;

		public void Dispose ()
		{
			reader_finished = new ManualResetEventSlim (false);
			reader_loop = false;
			reader_finished.Wait (5000);
			client.Close ();
		}

		public void EventLoop ()
		{
			while (reader_loop) {
				Thread.Sleep (50);
				if (!stream.DataAvailable)
					continue;
				lock (reader_lock) {
					if (!reader_loop)
						break;
					var line = reader.ReadLine ();
					if (line == null) {
						Console.WriteLine ("no input");
						continue;
					}
					try {
						var obj = (ObjectInstance) JSONObject.Parse (engine, line);
						if (obj.HasProperty ("uncaught"))
							OnUncaughtException (new DebuggerEvent (obj));
						else
							OnBreak (new DebuggerEvent (obj));
					} catch (JavaScriptException ex) {
						Console.WriteLine ("Error on parsing : " + line + Environment.NewLine + "Details: " + Environment.NewLine + ex);
					}
				}
			}
			if (reader_finished != null)
				reader_finished.Set ();
		}

		string InternalProcess (string request)
		{
			writer.WriteLine (request);

			return reader.ReadLine ();
		}

		public DebuggerResponse Process (DebuggerRequest request)
		{
			var res = InternalProcess (JSONObject.Stringify (engine, request.Instance));
			return new DebuggerResponse ((ObjectInstance) JSONObject.Parse (engine, res));
		}

		public void Continue (ContinueRequestArguments args)
		{
			Process (new DebuggerRequest () { Arguments = args, Command = "continue" });
		}

		public object Evaluate (EvaluateArguments args)
		{
			return Process (new DebuggerRequest () { Arguments = args, Command = "evaluate" }).Body;
		}

		public object lookup (LookupArguments args)
		{
			return Process (new DebuggerRequest () { Arguments = args, Command = "lookup" }).Body;
		}

		public BackTraceResponseBody Backtrace (BackTraceRequestArguments args)
		{
			return (BackTraceResponseBody) Process (new DebuggerRequest () { Arguments = args, Command = "backtrace" }).Body;
		}

		public Frame Frame (FrameRequestArguments args)
		{
			return (Frame) Process (new DebuggerRequest () { Arguments = args, Command = "frame" }).Body;
		}

		public Scope Scope (ScopeRequestArguments args)
		{
			return (Scope) Process (new DebuggerRequest () { Arguments = args, Command = "scope" }).Body;
		}

		public ScopesResponseBody Scopes (ScopesRequestArguments args)
		{
			return (ScopesResponseBody) Process (new DebuggerRequest () { Arguments = args, Command = "scopes" }).Body;
		}

		public ScriptsResponseBodyElement [] Scripts (ScriptsRequestArguments args)
		{
			return (ScriptsResponseBodyElement []) Process (new DebuggerRequest () { Arguments = args, Command = "scripts" }).Body;
		}

		public SourceResponseBody Source (SourceRequestArguments args)
		{
			return (SourceResponseBody) Process (new DebuggerRequest () { Arguments = args, Command = "source" }).Body;
		}

		public SetBreakpointResponseBody SetBreakpoint (SetBreakpointRequestArguments args)
		{
			return (SetBreakpointResponseBody) Process (new DebuggerRequest () { Arguments = args, Command = "setBreakpoint" }).Body;
		}

		public void ChangeBreakpointRequestArguments (ChangeBreakpointRequestArguments args)
		{
			Process (new DebuggerRequest () { Arguments = args, Command = "changeBreakpoint" });
		}

		public ClearBreakpointResponseBody ClearBreakpoint (ClearBreakpointRequestArguments args)
		{
			return (ClearBreakpointResponseBody) Process (new DebuggerRequest () { Arguments = args, Command = "clearBreakpoint" }).Body;
		}

		public SetExceptionBreakResponseBody SetExceptionBreak (SetExceptionBreakRequestArguments args)
		{
			return (SetExceptionBreakResponseBody) Process (new DebuggerRequest () { Arguments = args, Command = "setExceptionBreak" }).Body;
		}

		public void V8flags (V8FlagsRequestArguments args)
		{
			Process (new DebuggerRequest () { Arguments = args, Command = "v8flags" });
		}

		public VersionResponseBody Version ()
		{
			return (VersionResponseBody) Process (new DebuggerRequest () { Command = "version" }).Body;
		}

		public void Profile (ProfileRequestArguments args)
		{
			Process (new DebuggerRequest () { Arguments = args, Command = "profile" });
		}

		public void Disconnect ()
		{
			Process (new DebuggerRequest () { Command = "disconnect" });
		}

		public GcResoponseBody Gc (GcRequestArguments args)
		{
			return (GcResoponseBody) Process (new DebuggerRequest () { Arguments = args, Command = "gc" }).Body;
		}
	}
}

