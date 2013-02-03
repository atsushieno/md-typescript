// Not likely to work with nodejs.
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using Jurassic;
using Jurassic.Library;
using V8DebuggerClientBridge;
using V8DebuggerClientBridge.V8Debugger;

namespace Mono.JavaScript.Node.Debugger
{
	public class V8DebuggerProtocolClient : IDisposable
	{
		public const int DefaultNodeDebuggerPort = 5858;

		public V8DebuggerProtocolClient ()
			: this (DefaultNodeDebuggerPort)
		{
		}

		public V8DebuggerProtocolClient (int port)
		{
			this.engine = JavaScriptObject.Engine;
#if TCP_CLIENT
			client = new TcpClient ("localhost", port);
			stream = client.GetStream ();
			reader = new StreamReader (stream);
			writer = new StreamWriter (stream);
#else
			websocket = new ClientWebSocket ();
			websocket.ConnectAsync (new Uri ("ws://localhost:" + port), CancellationToken.None);
#endif
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
#if TCP_CLIENT
		TcpClient client;
		NetworkStream stream;
		StreamWriter writer;
		StreamReader reader;
#else
		ClientWebSocket websocket = new ClientWebSocket ();
#endif

		public void Dispose ()
		{
			reader_finished = new ManualResetEventSlim (false);
			reader_loop = false;
			reader_finished.Wait (5000);
#if TCP_CLIENT
			client.Close ();
#else
			websocket.CloseAsync (WebSocketCloseStatus.NormalClosure, "quit", CancellationToken.None);
#endif
		}

		public void EventLoop ()
		{
			var buffer = new ArraySegment<byte> (new byte [0x10000]);
			while (reader_loop) {
				var task = websocket.ReceiveAsync (buffer, CancellationToken.None);
				task.Wait ();
				task.ContinueWith (t => {
					string line = Encoding.UTF8.GetString (buffer.Array, buffer.Offset, task.Result.Count);
					if (line == null) {
						Console.WriteLine ("no input");
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
				});
			}
			if (reader_finished != null)
				reader_finished.Set ();
		}

		Queue<ObjectInstance> pending_messages = new Queue<ObjectInstance> ();

		ObjectInstance InternalProcess (string request)
		{
			return websocket.SendAsync (new ArraySegment<byte> (Encoding.UTF8.GetBytes (request)), WebSocketMessageType.Text, true, CancellationToken.None)
				.ContinueWith<ObjectInstance> (t => {
					if (t.IsFaulted)
						throw t.Exception;
					while (pending_messages.Count == 0)
						Thread.Sleep (50);
					return pending_messages.Dequeue ();
				}).Result;
		}

		public DebuggerResponse Process (DebuggerRequest request)
		{
			var res = InternalProcess (JSONObject.Stringify (engine, request.Instance));
			return new DebuggerResponse (res);
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
