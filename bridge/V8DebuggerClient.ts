
module V8Debugger
{
	class DebuggerPacket
	{
		public seq : number;
		public type : string;
	}

	class DebuggerRequest extends DebuggerPacket {
		public command : string;
		public arguments : any;
		
		constructor ()
		{
			super ();
			this.type = "request";
		}
	}
	
	class DebuggerResponse extends DebuggerPacket {
		public request_seq : number;
		public command : string;
		public body : any;
		public running : bool;
		public success : bool;
		public message : string;
		
		constructor ()
		{
			super ();
			this.type = "response";
		}
	}
	
	class DebuggerEvent extends DebuggerPacket {
		public event : string;
		public body : any;
		
		constructor ()
		{
			super ();
			this.type = "event";
		}
	}
	
	class HandleReference {
		public ref : number;
	}
	
	class HandleByValue {
		public handle : number;
		public type : string; // "undefined", "null", "boolean", "number", "string", "object", function" or "frame"
	}
	
	class HandlePrimitive extends HandleByValue {
		public value : any;
	}
	
	class HandleObject extends HandleByValue {
		public className : string;
		public constructorFunction : HandleReference;
		public protoObject : HandleReference;
		public prototypeObject : HandleReference;
		public properties : NameHandlePair [];
	}
	
	class HandleFunction extends HandleObject {
		public name : string;
		public inferredName : string;
		public source : string; // really?
		public script : HandleReference;
		public scriptId : number;
		public position : number;
		public line : number;
		public column : number;
	}
	
	class NameHandlePair {
		public name : string;
		public handle : number;
	}
		
	class ContinueRequestArguments {
		stepaction : string; // "in", "next" or "out"
		stepcount : number;
	}

	// no need to define type for continue response. no other body member.
	
	class EvaluateAdditionalContext {
		public name : string;
		public handle : string; // Handle
	}
	
	class EvaluateArguments {
		public expression : string;
		public bvraframe : number;
		public global : bool;
		public disable_break : bool;
		public additional_context : EvaluateAdditionalContext [];
	}
	
	// no need to define type for evaluate response. It could be anything.
	
	class LookupArguments {
		public handles : string []; // Handle
		public includeSource : bool;
	}
	
	// no need to define tyoe for lookup response. It is any[].
	
	class BackTraceRequestArguments {
		public fromFrame : number;
		public toFrame : number;
		public bottom : bool;
	}
	
	class BackTraceResponseBody {
		public fromFrame : number;
		public toFrame : number;
		public totalFrames : number;
		public frames : Frame [];
	}
	
	class FrameRequestArguments {
		// "number" is a keyword...
		public number : number;
	}

	// frame response body is a frame	
	class Frame {
		public index : number;
		public receiver : Object; // function
		public func : Object; // function
		public script : string;
		public constructCall : bool;
		public debuggerFrame : bool;
		public arguments : NameValuePair [];
		public locals : NameValuePair [];
		public position : number;
		public line : number;
		public column : number;
		public sourceLineText : string;
		public scopes : Scope [];
	}
	
	class NameValuePair {
		public name : string;
		public value : any;
	}
	
	class ScopeRequestArguments {
		// "number" is a keyword...
		public number : number;
		public frameNumber : number;
	}
	
	class ScopeType {
		public Global : number = 0;
		public Local : number = 1;
		public With : number = 2;
		public Closure : number = 3;
		public Catch : number = 4;
	}

	// scope response body is a Scope	
	class Scope {
		public index : number;
		public frameIndex : number;
		public type : number;
		public object : string; // Handle
	}
	
	class ScopesRequestArguments {
		public frameNumber : number;
	}
	
	class ScopesResponseBody {
		public fromScope : number;
		public toScope : number;
		public totalScopes : number;
		public scopes : Scope [];
	}
	
	class ScriptsRequestArguments {
		public types : number; // flags
		public ids : number [];
		public includeSource : bool;
		public filter : any; // string or number
	}
	
	class SourceLocation {
		public line : number;
		public column : number;
	}
	
	// body is array of this class
	class ScriptsResponseBodyElement {
		public name : string;
		public id : number;
		public lineOffset : number;
		public columnOffset : number;
		public lineCount : number;
		public data : Object;
		public source : string;
		public sourceStart : string;
		public sourceLength : number;
		public scriptType : number; // flags
		public compilationType : number; // 0 (API) or 1 (eval)
		public evalFromScript : string;
		public evalFromLocation : SourceLocation;
	}
	
	class SourceRequestArguments {
		public frame : number;
		public fromLine : number;
		public toLine : number;
	}
	
	class SourceResponseBody {
		public source : string;
		public fromLine : number;
		public toLine : number;
		public fromPosition : number;
		public toPosition : number;
		public totalLines : number;
	}
	
	class SetBreakpointRequestArguments {
		public type : string; // "function", "script", "scriptId" or "scriptRegExp"
		public target : any; // function expression or script identification
		public line : number;
		public column : number;
		public enabled : bool;
		public condition : string;
		public ignoreCount : number;
	}
	
	class SetBreakpointResponseBody {
		public type : string; // "function" or "script"
		public breakpoint : number; // of the new breakpoint
	}
	
	class ChangeBreakpointRequestArguments {
		public breakpoint : number;
		public enabled : bool;
		public condition : string;
		public ignoreCount : number;
	}
	
	// no description on response...
	
	class ClearBreakpointRequestArguments {
		public breakpoint : number;
	}
	
	class ClearBreakpointResponseBody {
		public type : string; // "function" or "script"
		public breakpoint : number;
	}
	
	class SetExceptionBreakRequestArguments {
		public type : string; // "all" or "uncaught"
		public enabled : bool;
	}
	
	class SetExceptionBreakResponseBody {
		public type : string; // "all" or "uncaught"
		public enabled : bool;
	}
	
	class V8FlagsRequestArguments {
		public flags : string;
	}
	
	// no response body for v8flags.

	// no arguments in version request.
	
	class VersionResponseBody {
		public V8Version : string;
	}
	
	class ProfileRequestArguments {
		public command : string; // "resume" or "pause"
	}
	
	// no body for profile response.
	
	// no arguments in disconnect request.
	
	// no body for disconnect response.
	
	class GcRequestArguments {
		public type : string; // so far only "all"
	}
	
	class GcResoponseBody {
		public before : number;
		public after : number;
	}
	
	// no arguments for listBreakpoint request.
	
	class ListBreakpointResponseBody {
		public breakpoints : BreakpointInfo [];
		public breakOnExceptions : bool;
		public breakOnUncaughtExceptions : bool;
	}
	
	class BreakpointInfo {
		public type : string; // "scriptId" or "scriptName"
		public script_id : number;
		public script_name : string;
		public number : number;
		public line : number;
		public column : number;
		public groupId : number;
		public hit_count : number;
		public active : bool;
		public ignoreCount : number;
		public actual_locations : SourceLocation []; // FIXME: not sure if it really is.
	}
	
	class BreakEventBody {
		public invocationText : string;
		public sourceLine : number;
		public sourceColumn : number;
		public sourceLineText : string;
		public script : ScriptInfo;
		public breakpoints : number [];
	}
	
	class ScriptInfo {
		public name : string;
		public lineOffset : number;
		public columnOffset : number;
		public lineCount : number;
	}
	
	class ExceptionEventBody {
		public uncaught : bool;
		public exception : any; // ?
		public sourceLine : number;
		public sourceColumn : number;
		public sourceLineText : string;
		public script : ScriptInfo;
	}
	
	/*	
	class DebuggerClient {
		// continue is a reserved keyword
		public continueDebug (request : DebuggerRequest) : DebuggerResponse;
		public evaluate (request : DebuggerRequest) : DebuggerResponse;
		public lookup (request : DebuggerRequest) : DebuggerResponse;
		public backtrace (request : DebuggerRequest) : DebuggerResponse;
		public frame (request : DebuggerRequest) : DebuggerResponse;
		public scope (request : DebuggerRequest) : DebuggerResponse;
		public scopes (request : DebuggerRequest) : DebuggerResponse;
		public scripts (request : DebuggerRequest) : DebuggerResponse;
		public source (request : DebuggerRequest) : DebuggerResponse;
		public setBreakpoint (request : DebuggerRequest) : DebuggerResponse;
		public changeBreakpoint (request : DebuggerRequest) : DebuggerResponse;
		public clearBreakpoint (request : DebuggerRequest) : DebuggerResponse;
		public setExceptionBreak (request : DebuggerRequest) : DebuggerResponse;
		public v8flags (request : DebuggerRequest) : DebuggerResponse;
		public version (request : DebuggerRequest) : DebuggerResponse;
		public profile (request : DebuggerRequest) : DebuggerResponse;
		public disconnect (request : DebuggerRequest) : DebuggerResponse;
		public gc (request : DebuggerRequest) : DebuggerResponse;
	}
	*/
}
