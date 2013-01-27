using System;
using TypeScriptServiceBridge.V8Debugger;

namespace Mono.JavaScript.Node.Debugger
{
	public static class V8DebuggerProtocolClientExtensions
	{
		public static void Continue (this V8DebuggerProtocolClient debugger, string stepAction = null, int stepCount = 0)
		{
			var arg = new ContinueRequestArguments ().CreateLocalCache<ContinueRequestArguments> ();
			arg.Stepaction = stepAction;
			arg.Stepcount = stepCount;
			debugger.Continue (arg);
		}

		public static SetBreakpointResponseBody SetBreakpoint (this V8DebuggerProtocolClient debugger, string type, string target, bool enabled, int line, int column = 0, string condition = null, int ignoreCount = 0)
		{
			var arg = new SetBreakpointRequestArguments ().CreateLocalCache<SetBreakpointRequestArguments> ();
			arg.Type = type;
			arg.Target = target;
			arg.Enabled = enabled;
			arg.Line = line;
			arg.Column = column;
			arg.Condition = condition;
			arg.IgnoreCount = ignoreCount;
			return debugger.SetBreakpoint (arg);
		}
	}
}

