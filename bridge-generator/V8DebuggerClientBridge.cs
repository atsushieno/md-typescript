using System;
using System.Collections.Generic;
using Jurassic;
using Jurassic.Library;
using TypeScriptServiceBridge;

using TypeScriptServiceBridge.TypeScript.Formatting;
using TypeScriptServiceBridge;
using TypeScriptServiceBridge.TypeScript;
using TypeScriptServiceBridge.SymbolDisplay;
using TypeScriptServiceBridge.V8Debugger;


namespace TypeScriptServiceBridge {
	public class TypeScriptBridgeAttribute : Attribute
	{
		public TypeScriptBridgeAttribute (string name)
		{
			Name = name;
		}
		
		public string Name { get; set; }
	}
}

namespace TypeScriptServiceBridge.TypeScript.Formatting
{
	public class RulesProvider : TypeScriptObject
			
	{
		public RulesProvider (ObjectInstance instance) : base (instance) {}
		public RulesProvider ()
			 : base (CallConstructor ("TypeScript.Formatting", "RulesProvider"))
		{
		}
	}
}
namespace TypeScriptServiceBridge
{
	public class any : TypeScriptObject
			
	{
		public any (ObjectInstance instance) : base (instance) {}
		public any ()
			 : base (CallConstructor ("", "any"))
		{
		}
	}
	public class unknown : TypeScriptObject
			
	{
		public unknown (ObjectInstance instance) : base (instance) {}
		public unknown ()
			 : base (CallConstructor ("", "unknown"))
		{
		}
	}
	public class someanonymoustype : TypeScriptObject
			
	{
		public someanonymoustype (ObjectInstance instance) : base (instance) {}
		public someanonymoustype ()
			 : base (CallConstructor ("", "someanonymoustype"))
		{
		}
	}
	public class typescriptfunctionargument : TypeScriptObject
			
	{
		public typescriptfunctionargument (ObjectInstance instance) : base (instance) {}
		public typescriptfunctionargument ()
			 : base (CallConstructor ("", "typescriptfunctionargument"))
		{
		}
	}
	public class MaskBitSize : TypeScriptObject
			
	{
		public MaskBitSize (ObjectInstance instance) : base (instance) {}
		public MaskBitSize ()
			 : base (CallConstructor ("", "MaskBitSize"))
		{
		}
	}
	public class RegExp : TypeScriptObject
			
	{
		public RegExp (ObjectInstance instance) : base (instance) {}
		public RegExp ()
			 : base (CallConstructor ("", "RegExp"))
		{
		}
	}
	public class Error : TypeScriptObject
			
	{
		public Error (ObjectInstance instance) : base (instance) {}
		public Error ()
			 : base (CallConstructor ("", "Error"))
		{
		}
	}
}
namespace TypeScriptServiceBridge.TypeScript
{
	public interface ILocation : ITypeScriptObject
			
	{
	}
	public class ILocation_Impl : TypeScriptObject, ILocation
	{
		public ILocation_Impl (ObjectInstance instance) : base (instance) {}
	}
	public class Location : TypeScriptObject
			
	{
		public Location (ObjectInstance instance) : base (instance) {}
		public Location ()
			 : base (CallConstructor ("TypeScript", "Location"))
		{
		}
	}
	public interface ITextWriter : ITypeScriptObject
			
	{
	}
	public class ITextWriter_Impl : TypeScriptObject, ITextWriter
	{
		public ITextWriter_Impl (ObjectInstance instance) : base (instance) {}
	}
	public interface IAstWalkCallback : ITypeScriptObject
			
	{
	}
	public class IAstWalkCallback_Impl : TypeScriptObject, IAstWalkCallback
	{
		public IAstWalkCallback_Impl (ObjectInstance instance) : base (instance) {}
	}
	public interface IAstWalkChildren : ITypeScriptObject
			
	{
	}
	public class IAstWalkChildren_Impl : TypeScriptObject, IAstWalkChildren
	{
		public IAstWalkChildren_Impl (ObjectInstance instance) : base (instance) {}
	}
	public class Scanner : TypeScriptObject
			
	{
		public Scanner (ObjectInstance instance) : base (instance) {}
		public Scanner ()
			 : base (CallConstructor ("TypeScript", "Scanner"))
		{
		}
	}
}
namespace TypeScriptServiceBridge.SymbolDisplay
{
	public class Format : TypeScriptObject
			
	{
		public Format (ObjectInstance instance) : base (instance) {}
		public Format ()
			 : base (CallConstructor ("SymbolDisplay", "Format"))
		{
		}
	}
	public class Part : TypeScriptObject
			
	{
		public Part (ObjectInstance instance) : base (instance) {}
		public Part ()
			 : base (CallConstructor ("SymbolDisplay", "Part"))
		{
		}
	}
}
namespace TypeScriptServiceBridge.V8Debugger
{
	public class DebuggerPacket : TypeScriptObject
			
	{
		public DebuggerPacket (ObjectInstance instance) : base (instance) {}
		public DebuggerPacket ()
			 : base (CallConstructor ("V8Debugger", "DebuggerPacket"))
		{
		}
		[TypeScriptBridge ("seq")]
		public double Seq {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("seq")); }
			set { SetPropertyValue ("seq", value); }
		}
		[TypeScriptBridge ("type")]
		public string Type {
			get { return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, GetPropertyValue ("type")); }
			set { SetPropertyValue ("type", value); }
		}
		[TypeScriptBridge ("get_seq")]
		public double Get_seq ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_seq"));
		}
		[TypeScriptBridge ("set_seq")]
		public void Set_seq (double @value)
		{
			CallMemberFunction ("set_seq", @value);
		}
		[TypeScriptBridge ("get_type")]
		public string Get_type ()
		{
			return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, CallMemberFunction ("get_type"));
		}
		[TypeScriptBridge ("set_type")]
		public void Set_type (string @value)
		{
			CallMemberFunction ("set_type", @value);
		}
	}
	public class DebuggerRequest : TypeScriptObject
			
	{
		public DebuggerRequest (ObjectInstance instance) : base (instance) {}
		public DebuggerRequest ()
			 : base (CallConstructor ("V8Debugger", "DebuggerRequest"))
		{
		}
		[TypeScriptBridge ("command")]
		public string Command {
			get { return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, GetPropertyValue ("command")); }
			set { SetPropertyValue ("command", value); }
		}
		[TypeScriptBridge ("arguments")]
		public object Arguments {
			get { return TypeConverter.ConvertTo<object> (JurassicTypeHosting.Engine, GetPropertyValue ("arguments")); }
			set { SetPropertyValue ("arguments", value); }
		}
		[TypeScriptBridge ("get_command")]
		public string Get_command ()
		{
			return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, CallMemberFunction ("get_command"));
		}
		[TypeScriptBridge ("set_command")]
		public void Set_command (string @value)
		{
			CallMemberFunction ("set_command", @value);
		}
		[TypeScriptBridge ("get_arguments")]
		public object Get_arguments ()
		{
			return TypeConverter.ConvertTo<object> (JurassicTypeHosting.Engine, CallMemberFunction ("get_arguments"));
		}
		[TypeScriptBridge ("set_arguments")]
		public void Set_arguments (object @value)
		{
			CallMemberFunction ("set_arguments", @value);
		}
	}
	public class DebuggerResponse : TypeScriptObject
			
	{
		public DebuggerResponse (ObjectInstance instance) : base (instance) {}
		public DebuggerResponse ()
			 : base (CallConstructor ("V8Debugger", "DebuggerResponse"))
		{
		}
		[TypeScriptBridge ("request_seq")]
		public double Request_seq {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("request_seq")); }
			set { SetPropertyValue ("request_seq", value); }
		}
		[TypeScriptBridge ("command")]
		public string Command {
			get { return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, GetPropertyValue ("command")); }
			set { SetPropertyValue ("command", value); }
		}
		[TypeScriptBridge ("body")]
		public object Body {
			get { return TypeConverter.ConvertTo<object> (JurassicTypeHosting.Engine, GetPropertyValue ("body")); }
			set { SetPropertyValue ("body", value); }
		}
		[TypeScriptBridge ("running")]
		public bool Running {
			get { return TypeConverter.ConvertTo<bool> (JurassicTypeHosting.Engine, GetPropertyValue ("running")); }
			set { SetPropertyValue ("running", value); }
		}
		[TypeScriptBridge ("success")]
		public bool Success {
			get { return TypeConverter.ConvertTo<bool> (JurassicTypeHosting.Engine, GetPropertyValue ("success")); }
			set { SetPropertyValue ("success", value); }
		}
		[TypeScriptBridge ("message")]
		public string Message {
			get { return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, GetPropertyValue ("message")); }
			set { SetPropertyValue ("message", value); }
		}
		[TypeScriptBridge ("get_request_seq")]
		public double Get_request_seq ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_request_seq"));
		}
		[TypeScriptBridge ("set_request_seq")]
		public void Set_request_seq (double @value)
		{
			CallMemberFunction ("set_request_seq", @value);
		}
		[TypeScriptBridge ("get_command")]
		public string Get_command ()
		{
			return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, CallMemberFunction ("get_command"));
		}
		[TypeScriptBridge ("set_command")]
		public void Set_command (string @value)
		{
			CallMemberFunction ("set_command", @value);
		}
		[TypeScriptBridge ("get_body")]
		public object Get_body ()
		{
			return TypeConverter.ConvertTo<object> (JurassicTypeHosting.Engine, CallMemberFunction ("get_body"));
		}
		[TypeScriptBridge ("set_body")]
		public void Set_body (object @value)
		{
			CallMemberFunction ("set_body", @value);
		}
		[TypeScriptBridge ("get_running")]
		public bool Get_running ()
		{
			return TypeConverter.ConvertTo<bool> (JurassicTypeHosting.Engine, CallMemberFunction ("get_running"));
		}
		[TypeScriptBridge ("set_running")]
		public void Set_running (bool @value)
		{
			CallMemberFunction ("set_running", @value);
		}
		[TypeScriptBridge ("get_success")]
		public bool Get_success ()
		{
			return TypeConverter.ConvertTo<bool> (JurassicTypeHosting.Engine, CallMemberFunction ("get_success"));
		}
		[TypeScriptBridge ("set_success")]
		public void Set_success (bool @value)
		{
			CallMemberFunction ("set_success", @value);
		}
		[TypeScriptBridge ("get_message")]
		public string Get_message ()
		{
			return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, CallMemberFunction ("get_message"));
		}
		[TypeScriptBridge ("set_message")]
		public void Set_message (string @value)
		{
			CallMemberFunction ("set_message", @value);
		}
	}
	public class DebuggerEvent : TypeScriptObject
			
	{
		public DebuggerEvent (ObjectInstance instance) : base (instance) {}
		public DebuggerEvent ()
			 : base (CallConstructor ("V8Debugger", "DebuggerEvent"))
		{
		}
		[TypeScriptBridge ("event")]
		public string Event {
			get { return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, GetPropertyValue ("event")); }
			set { SetPropertyValue ("event", value); }
		}
		[TypeScriptBridge ("body")]
		public object Body {
			get { return TypeConverter.ConvertTo<object> (JurassicTypeHosting.Engine, GetPropertyValue ("body")); }
			set { SetPropertyValue ("body", value); }
		}
		[TypeScriptBridge ("get_event")]
		public string Get_event ()
		{
			return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, CallMemberFunction ("get_event"));
		}
		[TypeScriptBridge ("set_event")]
		public void Set_event (string @value)
		{
			CallMemberFunction ("set_event", @value);
		}
		[TypeScriptBridge ("get_body")]
		public object Get_body ()
		{
			return TypeConverter.ConvertTo<object> (JurassicTypeHosting.Engine, CallMemberFunction ("get_body"));
		}
		[TypeScriptBridge ("set_body")]
		public void Set_body (object @value)
		{
			CallMemberFunction ("set_body", @value);
		}
	}
	public class HandleReference : TypeScriptObject
			
	{
		public HandleReference (ObjectInstance instance) : base (instance) {}
		public HandleReference ()
			 : base (CallConstructor ("V8Debugger", "HandleReference"))
		{
		}
		[TypeScriptBridge ("ref")]
		public double Ref {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("ref")); }
			set { SetPropertyValue ("ref", value); }
		}
		[TypeScriptBridge ("get_ref")]
		public double Get_ref ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_ref"));
		}
		[TypeScriptBridge ("set_ref")]
		public void Set_ref (double @value)
		{
			CallMemberFunction ("set_ref", @value);
		}
	}
	public class HandleByValue : TypeScriptObject
			
	{
		public HandleByValue (ObjectInstance instance) : base (instance) {}
		public HandleByValue ()
			 : base (CallConstructor ("V8Debugger", "HandleByValue"))
		{
		}
		[TypeScriptBridge ("handle")]
		public double Handle {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("handle")); }
			set { SetPropertyValue ("handle", value); }
		}
		[TypeScriptBridge ("type")]
		public string Type {
			get { return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, GetPropertyValue ("type")); }
			set { SetPropertyValue ("type", value); }
		}
		[TypeScriptBridge ("get_handle")]
		public double Get_handle ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_handle"));
		}
		[TypeScriptBridge ("set_handle")]
		public void Set_handle (double @value)
		{
			CallMemberFunction ("set_handle", @value);
		}
		[TypeScriptBridge ("get_type")]
		public string Get_type ()
		{
			return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, CallMemberFunction ("get_type"));
		}
		[TypeScriptBridge ("set_type")]
		public void Set_type (string @value)
		{
			CallMemberFunction ("set_type", @value);
		}
	}
	public class HandlePrimitive : TypeScriptObject
			
	{
		public HandlePrimitive (ObjectInstance instance) : base (instance) {}
		public HandlePrimitive ()
			 : base (CallConstructor ("V8Debugger", "HandlePrimitive"))
		{
		}
		[TypeScriptBridge ("value")]
		public object Value {
			get { return TypeConverter.ConvertTo<object> (JurassicTypeHosting.Engine, GetPropertyValue ("value")); }
			set { SetPropertyValue ("value", value); }
		}
		[TypeScriptBridge ("get_value")]
		public object Get_value ()
		{
			return TypeConverter.ConvertTo<object> (JurassicTypeHosting.Engine, CallMemberFunction ("get_value"));
		}
		[TypeScriptBridge ("set_value")]
		public void Set_value (object @value)
		{
			CallMemberFunction ("set_value", @value);
		}
	}
	public class HandleObject : TypeScriptObject
			
	{
		public HandleObject (ObjectInstance instance) : base (instance) {}
		public HandleObject ()
			 : base (CallConstructor ("V8Debugger", "HandleObject"))
		{
		}
		[TypeScriptBridge ("className")]
		public string ClassName {
			get { return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, GetPropertyValue ("className")); }
			set { SetPropertyValue ("className", value); }
		}
		[TypeScriptBridge ("constructorFunction")]
		public TypeScriptServiceBridge.V8Debugger.HandleReference ConstructorFunction {
			get { return TypeScriptObject.Create<TypeScriptServiceBridge.V8Debugger.HandleReference> ((ObjectInstance) GetPropertyValue ("constructorFunction")); }
			set { SetPropertyValue ("constructorFunction", value != null ? value.Instance : null); }
		}
		[TypeScriptBridge ("protoObject")]
		public TypeScriptServiceBridge.V8Debugger.HandleReference ProtoObject {
			get { return TypeScriptObject.Create<TypeScriptServiceBridge.V8Debugger.HandleReference> ((ObjectInstance) GetPropertyValue ("protoObject")); }
			set { SetPropertyValue ("protoObject", value != null ? value.Instance : null); }
		}
		[TypeScriptBridge ("prototypeObject")]
		public TypeScriptServiceBridge.V8Debugger.HandleReference PrototypeObject {
			get { return TypeScriptObject.Create<TypeScriptServiceBridge.V8Debugger.HandleReference> ((ObjectInstance) GetPropertyValue ("prototypeObject")); }
			set { SetPropertyValue ("prototypeObject", value != null ? value.Instance : null); }
		}
		[TypeScriptBridge ("properties")]
		public TypeScriptArray<TypeScriptServiceBridge.V8Debugger.NameHandlePair> Properties {
			get { return new TypeScriptArray<TypeScriptServiceBridge.V8Debugger.NameHandlePair> ((ArrayInstance) GetPropertyValue ("properties")); }
			set { SetPropertyValue ("properties", value != null ? value.Instance : null); }
		}
		[TypeScriptBridge ("get_className")]
		public string Get_className ()
		{
			return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, CallMemberFunction ("get_className"));
		}
		[TypeScriptBridge ("set_className")]
		public void Set_className (string @value)
		{
			CallMemberFunction ("set_className", @value);
		}
		[TypeScriptBridge ("get_constructorFunction")]
		public TypeScriptServiceBridge.V8Debugger.HandleReference Get_constructorFunction ()
		{
			return TypeScriptObject.Create<TypeScriptServiceBridge.V8Debugger.HandleReference>  ((ObjectInstance) CallMemberFunction ("get_constructorFunction"));
		}
		[TypeScriptBridge ("set_constructorFunction")]
		public void Set_constructorFunction (TypeScriptServiceBridge.V8Debugger.HandleReference @value)
		{
			CallMemberFunction ("set_constructorFunction", @value);
		}
		[TypeScriptBridge ("get_protoObject")]
		public TypeScriptServiceBridge.V8Debugger.HandleReference Get_protoObject ()
		{
			return TypeScriptObject.Create<TypeScriptServiceBridge.V8Debugger.HandleReference>  ((ObjectInstance) CallMemberFunction ("get_protoObject"));
		}
		[TypeScriptBridge ("set_protoObject")]
		public void Set_protoObject (TypeScriptServiceBridge.V8Debugger.HandleReference @value)
		{
			CallMemberFunction ("set_protoObject", @value);
		}
		[TypeScriptBridge ("get_prototypeObject")]
		public TypeScriptServiceBridge.V8Debugger.HandleReference Get_prototypeObject ()
		{
			return TypeScriptObject.Create<TypeScriptServiceBridge.V8Debugger.HandleReference>  ((ObjectInstance) CallMemberFunction ("get_prototypeObject"));
		}
		[TypeScriptBridge ("set_prototypeObject")]
		public void Set_prototypeObject (TypeScriptServiceBridge.V8Debugger.HandleReference @value)
		{
			CallMemberFunction ("set_prototypeObject", @value);
		}
		[TypeScriptBridge ("get_properties")]
		public TypeScriptArray<TypeScriptServiceBridge.V8Debugger.NameHandlePair> Get_properties ()
		{
			return new TypeScriptArray<TypeScriptServiceBridge.V8Debugger.NameHandlePair> ((ArrayInstance) CallMemberFunction ("get_properties"));
		}
		[TypeScriptBridge ("set_properties")]
		public void Set_properties (TypeScriptArray<TypeScriptServiceBridge.V8Debugger.NameHandlePair> @value)
		{
			CallMemberFunction ("set_properties", @value);
		}
	}
	public class HandleFunction : TypeScriptObject
			
	{
		public HandleFunction (ObjectInstance instance) : base (instance) {}
		public HandleFunction ()
			 : base (CallConstructor ("V8Debugger", "HandleFunction"))
		{
		}
		[TypeScriptBridge ("name")]
		public string Name {
			get { return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, GetPropertyValue ("name")); }
			set { SetPropertyValue ("name", value); }
		}
		[TypeScriptBridge ("inferredName")]
		public string InferredName {
			get { return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, GetPropertyValue ("inferredName")); }
			set { SetPropertyValue ("inferredName", value); }
		}
		[TypeScriptBridge ("source")]
		public string Source {
			get { return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, GetPropertyValue ("source")); }
			set { SetPropertyValue ("source", value); }
		}
		[TypeScriptBridge ("script")]
		public TypeScriptServiceBridge.V8Debugger.HandleReference Script {
			get { return TypeScriptObject.Create<TypeScriptServiceBridge.V8Debugger.HandleReference> ((ObjectInstance) GetPropertyValue ("script")); }
			set { SetPropertyValue ("script", value != null ? value.Instance : null); }
		}
		[TypeScriptBridge ("scriptId")]
		public double ScriptId {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("scriptId")); }
			set { SetPropertyValue ("scriptId", value); }
		}
		[TypeScriptBridge ("position")]
		public double Position {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("position")); }
			set { SetPropertyValue ("position", value); }
		}
		[TypeScriptBridge ("line")]
		public double Line {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("line")); }
			set { SetPropertyValue ("line", value); }
		}
		[TypeScriptBridge ("column")]
		public double Column {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("column")); }
			set { SetPropertyValue ("column", value); }
		}
		[TypeScriptBridge ("get_name")]
		public string Get_name ()
		{
			return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, CallMemberFunction ("get_name"));
		}
		[TypeScriptBridge ("set_name")]
		public void Set_name (string @value)
		{
			CallMemberFunction ("set_name", @value);
		}
		[TypeScriptBridge ("get_inferredName")]
		public string Get_inferredName ()
		{
			return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, CallMemberFunction ("get_inferredName"));
		}
		[TypeScriptBridge ("set_inferredName")]
		public void Set_inferredName (string @value)
		{
			CallMemberFunction ("set_inferredName", @value);
		}
		[TypeScriptBridge ("get_source")]
		public string Get_source ()
		{
			return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, CallMemberFunction ("get_source"));
		}
		[TypeScriptBridge ("set_source")]
		public void Set_source (string @value)
		{
			CallMemberFunction ("set_source", @value);
		}
		[TypeScriptBridge ("get_script")]
		public TypeScriptServiceBridge.V8Debugger.HandleReference Get_script ()
		{
			return TypeScriptObject.Create<TypeScriptServiceBridge.V8Debugger.HandleReference>  ((ObjectInstance) CallMemberFunction ("get_script"));
		}
		[TypeScriptBridge ("set_script")]
		public void Set_script (TypeScriptServiceBridge.V8Debugger.HandleReference @value)
		{
			CallMemberFunction ("set_script", @value);
		}
		[TypeScriptBridge ("get_scriptId")]
		public double Get_scriptId ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_scriptId"));
		}
		[TypeScriptBridge ("set_scriptId")]
		public void Set_scriptId (double @value)
		{
			CallMemberFunction ("set_scriptId", @value);
		}
		[TypeScriptBridge ("get_position")]
		public double Get_position ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_position"));
		}
		[TypeScriptBridge ("set_position")]
		public void Set_position (double @value)
		{
			CallMemberFunction ("set_position", @value);
		}
		[TypeScriptBridge ("get_line")]
		public double Get_line ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_line"));
		}
		[TypeScriptBridge ("set_line")]
		public void Set_line (double @value)
		{
			CallMemberFunction ("set_line", @value);
		}
		[TypeScriptBridge ("get_column")]
		public double Get_column ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_column"));
		}
		[TypeScriptBridge ("set_column")]
		public void Set_column (double @value)
		{
			CallMemberFunction ("set_column", @value);
		}
	}
	public class NameHandlePair : TypeScriptObject
			
	{
		public NameHandlePair (ObjectInstance instance) : base (instance) {}
		public NameHandlePair ()
			 : base (CallConstructor ("V8Debugger", "NameHandlePair"))
		{
		}
		[TypeScriptBridge ("name")]
		public string Name {
			get { return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, GetPropertyValue ("name")); }
			set { SetPropertyValue ("name", value); }
		}
		[TypeScriptBridge ("handle")]
		public double Handle {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("handle")); }
			set { SetPropertyValue ("handle", value); }
		}
		[TypeScriptBridge ("get_name")]
		public string Get_name ()
		{
			return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, CallMemberFunction ("get_name"));
		}
		[TypeScriptBridge ("set_name")]
		public void Set_name (string @value)
		{
			CallMemberFunction ("set_name", @value);
		}
		[TypeScriptBridge ("get_handle")]
		public double Get_handle ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_handle"));
		}
		[TypeScriptBridge ("set_handle")]
		public void Set_handle (double @value)
		{
			CallMemberFunction ("set_handle", @value);
		}
	}
	public class ContinueRequestArguments : TypeScriptObject
			
	{
		public ContinueRequestArguments (ObjectInstance instance) : base (instance) {}
		public ContinueRequestArguments ()
			 : base (CallConstructor ("V8Debugger", "ContinueRequestArguments"))
		{
		}
		[TypeScriptBridge ("stepaction")]
		public string Stepaction {
			get { return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, GetPropertyValue ("stepaction")); }
			set { SetPropertyValue ("stepaction", value); }
		}
		[TypeScriptBridge ("stepcount")]
		public double Stepcount {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("stepcount")); }
			set { SetPropertyValue ("stepcount", value); }
		}
		[TypeScriptBridge ("get_stepaction")]
		public string Get_stepaction ()
		{
			return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, CallMemberFunction ("get_stepaction"));
		}
		[TypeScriptBridge ("set_stepaction")]
		public void Set_stepaction (string @value)
		{
			CallMemberFunction ("set_stepaction", @value);
		}
		[TypeScriptBridge ("get_stepcount")]
		public double Get_stepcount ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_stepcount"));
		}
		[TypeScriptBridge ("set_stepcount")]
		public void Set_stepcount (double @value)
		{
			CallMemberFunction ("set_stepcount", @value);
		}
	}
	public class EvaluateAdditionalContext : TypeScriptObject
			
	{
		public EvaluateAdditionalContext (ObjectInstance instance) : base (instance) {}
		public EvaluateAdditionalContext ()
			 : base (CallConstructor ("V8Debugger", "EvaluateAdditionalContext"))
		{
		}
		[TypeScriptBridge ("name")]
		public string Name {
			get { return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, GetPropertyValue ("name")); }
			set { SetPropertyValue ("name", value); }
		}
		[TypeScriptBridge ("handle")]
		public string Handle {
			get { return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, GetPropertyValue ("handle")); }
			set { SetPropertyValue ("handle", value); }
		}
		[TypeScriptBridge ("get_name")]
		public string Get_name ()
		{
			return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, CallMemberFunction ("get_name"));
		}
		[TypeScriptBridge ("set_name")]
		public void Set_name (string @value)
		{
			CallMemberFunction ("set_name", @value);
		}
		[TypeScriptBridge ("get_handle")]
		public string Get_handle ()
		{
			return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, CallMemberFunction ("get_handle"));
		}
		[TypeScriptBridge ("set_handle")]
		public void Set_handle (string @value)
		{
			CallMemberFunction ("set_handle", @value);
		}
	}
	public class EvaluateArguments : TypeScriptObject
			
	{
		public EvaluateArguments (ObjectInstance instance) : base (instance) {}
		public EvaluateArguments ()
			 : base (CallConstructor ("V8Debugger", "EvaluateArguments"))
		{
		}
		[TypeScriptBridge ("expression")]
		public string Expression {
			get { return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, GetPropertyValue ("expression")); }
			set { SetPropertyValue ("expression", value); }
		}
		[TypeScriptBridge ("bvraframe")]
		public double Bvraframe {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("bvraframe")); }
			set { SetPropertyValue ("bvraframe", value); }
		}
		[TypeScriptBridge ("global")]
		public bool Global {
			get { return TypeConverter.ConvertTo<bool> (JurassicTypeHosting.Engine, GetPropertyValue ("global")); }
			set { SetPropertyValue ("global", value); }
		}
		[TypeScriptBridge ("disable_break")]
		public bool Disable_break {
			get { return TypeConverter.ConvertTo<bool> (JurassicTypeHosting.Engine, GetPropertyValue ("disable_break")); }
			set { SetPropertyValue ("disable_break", value); }
		}
		[TypeScriptBridge ("additional_context")]
		public TypeScriptArray<TypeScriptServiceBridge.V8Debugger.EvaluateAdditionalContext> Additional_context {
			get { return new TypeScriptArray<TypeScriptServiceBridge.V8Debugger.EvaluateAdditionalContext> ((ArrayInstance) GetPropertyValue ("additional_context")); }
			set { SetPropertyValue ("additional_context", value != null ? value.Instance : null); }
		}
		[TypeScriptBridge ("get_expression")]
		public string Get_expression ()
		{
			return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, CallMemberFunction ("get_expression"));
		}
		[TypeScriptBridge ("set_expression")]
		public void Set_expression (string @value)
		{
			CallMemberFunction ("set_expression", @value);
		}
		[TypeScriptBridge ("get_bvraframe")]
		public double Get_bvraframe ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_bvraframe"));
		}
		[TypeScriptBridge ("set_bvraframe")]
		public void Set_bvraframe (double @value)
		{
			CallMemberFunction ("set_bvraframe", @value);
		}
		[TypeScriptBridge ("get_global")]
		public bool Get_global ()
		{
			return TypeConverter.ConvertTo<bool> (JurassicTypeHosting.Engine, CallMemberFunction ("get_global"));
		}
		[TypeScriptBridge ("set_global")]
		public void Set_global (bool @value)
		{
			CallMemberFunction ("set_global", @value);
		}
		[TypeScriptBridge ("get_disable_break")]
		public bool Get_disable_break ()
		{
			return TypeConverter.ConvertTo<bool> (JurassicTypeHosting.Engine, CallMemberFunction ("get_disable_break"));
		}
		[TypeScriptBridge ("set_disable_break")]
		public void Set_disable_break (bool @value)
		{
			CallMemberFunction ("set_disable_break", @value);
		}
		[TypeScriptBridge ("get_additional_context")]
		public TypeScriptArray<TypeScriptServiceBridge.V8Debugger.EvaluateAdditionalContext> Get_additional_context ()
		{
			return new TypeScriptArray<TypeScriptServiceBridge.V8Debugger.EvaluateAdditionalContext> ((ArrayInstance) CallMemberFunction ("get_additional_context"));
		}
		[TypeScriptBridge ("set_additional_context")]
		public void Set_additional_context (TypeScriptArray<TypeScriptServiceBridge.V8Debugger.EvaluateAdditionalContext> @value)
		{
			CallMemberFunction ("set_additional_context", @value);
		}
	}
	public class LookupArguments : TypeScriptObject
			
	{
		public LookupArguments (ObjectInstance instance) : base (instance) {}
		public LookupArguments ()
			 : base (CallConstructor ("V8Debugger", "LookupArguments"))
		{
		}
		[TypeScriptBridge ("handles")]
		public TypeScriptArray<string> Handles {
			get { return TypeConverter.ConvertTo<TypeScriptArray<string>> (JurassicTypeHosting.Engine, GetPropertyValue ("handles")); }
			set { SetPropertyValue ("handles", value); }
		}
		[TypeScriptBridge ("includeSource")]
		public bool IncludeSource {
			get { return TypeConverter.ConvertTo<bool> (JurassicTypeHosting.Engine, GetPropertyValue ("includeSource")); }
			set { SetPropertyValue ("includeSource", value); }
		}
		[TypeScriptBridge ("get_handles")]
		public TypeScriptArray<string> Get_handles ()
		{
			return TypeConverter.ConvertTo<TypeScriptArray<string>> (JurassicTypeHosting.Engine, CallMemberFunction ("get_handles"));
		}
		[TypeScriptBridge ("set_handles")]
		public void Set_handles (TypeScriptArray<string> @value)
		{
			CallMemberFunction ("set_handles", @value);
		}
		[TypeScriptBridge ("get_includeSource")]
		public bool Get_includeSource ()
		{
			return TypeConverter.ConvertTo<bool> (JurassicTypeHosting.Engine, CallMemberFunction ("get_includeSource"));
		}
		[TypeScriptBridge ("set_includeSource")]
		public void Set_includeSource (bool @value)
		{
			CallMemberFunction ("set_includeSource", @value);
		}
	}
	public class BackTraceRequestArguments : TypeScriptObject
			
	{
		public BackTraceRequestArguments (ObjectInstance instance) : base (instance) {}
		public BackTraceRequestArguments ()
			 : base (CallConstructor ("V8Debugger", "BackTraceRequestArguments"))
		{
		}
		[TypeScriptBridge ("fromFrame")]
		public double FromFrame {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("fromFrame")); }
			set { SetPropertyValue ("fromFrame", value); }
		}
		[TypeScriptBridge ("toFrame")]
		public double ToFrame {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("toFrame")); }
			set { SetPropertyValue ("toFrame", value); }
		}
		[TypeScriptBridge ("bottom")]
		public bool Bottom {
			get { return TypeConverter.ConvertTo<bool> (JurassicTypeHosting.Engine, GetPropertyValue ("bottom")); }
			set { SetPropertyValue ("bottom", value); }
		}
		[TypeScriptBridge ("get_fromFrame")]
		public double Get_fromFrame ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_fromFrame"));
		}
		[TypeScriptBridge ("set_fromFrame")]
		public void Set_fromFrame (double @value)
		{
			CallMemberFunction ("set_fromFrame", @value);
		}
		[TypeScriptBridge ("get_toFrame")]
		public double Get_toFrame ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_toFrame"));
		}
		[TypeScriptBridge ("set_toFrame")]
		public void Set_toFrame (double @value)
		{
			CallMemberFunction ("set_toFrame", @value);
		}
		[TypeScriptBridge ("get_bottom")]
		public bool Get_bottom ()
		{
			return TypeConverter.ConvertTo<bool> (JurassicTypeHosting.Engine, CallMemberFunction ("get_bottom"));
		}
		[TypeScriptBridge ("set_bottom")]
		public void Set_bottom (bool @value)
		{
			CallMemberFunction ("set_bottom", @value);
		}
	}
	public class BackTraceResponseBody : TypeScriptObject
			
	{
		public BackTraceResponseBody (ObjectInstance instance) : base (instance) {}
		public BackTraceResponseBody ()
			 : base (CallConstructor ("V8Debugger", "BackTraceResponseBody"))
		{
		}
		[TypeScriptBridge ("fromFrame")]
		public double FromFrame {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("fromFrame")); }
			set { SetPropertyValue ("fromFrame", value); }
		}
		[TypeScriptBridge ("toFrame")]
		public double ToFrame {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("toFrame")); }
			set { SetPropertyValue ("toFrame", value); }
		}
		[TypeScriptBridge ("totalFrames")]
		public double TotalFrames {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("totalFrames")); }
			set { SetPropertyValue ("totalFrames", value); }
		}
		[TypeScriptBridge ("frames")]
		public TypeScriptArray<TypeScriptServiceBridge.V8Debugger.Frame> Frames {
			get { return new TypeScriptArray<TypeScriptServiceBridge.V8Debugger.Frame> ((ArrayInstance) GetPropertyValue ("frames")); }
			set { SetPropertyValue ("frames", value != null ? value.Instance : null); }
		}
		[TypeScriptBridge ("get_fromFrame")]
		public double Get_fromFrame ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_fromFrame"));
		}
		[TypeScriptBridge ("set_fromFrame")]
		public void Set_fromFrame (double @value)
		{
			CallMemberFunction ("set_fromFrame", @value);
		}
		[TypeScriptBridge ("get_toFrame")]
		public double Get_toFrame ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_toFrame"));
		}
		[TypeScriptBridge ("set_toFrame")]
		public void Set_toFrame (double @value)
		{
			CallMemberFunction ("set_toFrame", @value);
		}
		[TypeScriptBridge ("get_totalFrames")]
		public double Get_totalFrames ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_totalFrames"));
		}
		[TypeScriptBridge ("set_totalFrames")]
		public void Set_totalFrames (double @value)
		{
			CallMemberFunction ("set_totalFrames", @value);
		}
		[TypeScriptBridge ("get_frames")]
		public TypeScriptArray<TypeScriptServiceBridge.V8Debugger.Frame> Get_frames ()
		{
			return new TypeScriptArray<TypeScriptServiceBridge.V8Debugger.Frame> ((ArrayInstance) CallMemberFunction ("get_frames"));
		}
		[TypeScriptBridge ("set_frames")]
		public void Set_frames (TypeScriptArray<TypeScriptServiceBridge.V8Debugger.Frame> @value)
		{
			CallMemberFunction ("set_frames", @value);
		}
	}
	public class FrameRequestArguments : TypeScriptObject
			
	{
		public FrameRequestArguments (ObjectInstance instance) : base (instance) {}
		public FrameRequestArguments ()
			 : base (CallConstructor ("V8Debugger", "FrameRequestArguments"))
		{
		}
		[TypeScriptBridge ("number")]
		public double Number {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("number")); }
			set { SetPropertyValue ("number", value); }
		}
		[TypeScriptBridge ("get_number")]
		public double Get_number ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_number"));
		}
		[TypeScriptBridge ("set_number")]
		public void Set_number (double @value)
		{
			CallMemberFunction ("set_number", @value);
		}
	}
	public class Frame : TypeScriptObject
			
	{
		public Frame (ObjectInstance instance) : base (instance) {}
		public Frame ()
			 : base (CallConstructor ("V8Debugger", "Frame"))
		{
		}
		[TypeScriptBridge ("index")]
		public double Index {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("index")); }
			set { SetPropertyValue ("index", value); }
		}
		[TypeScriptBridge ("receiver")]
		public System.Object Receiver {
			get { return TypeConverter.ConvertTo<System.Object> (JurassicTypeHosting.Engine, GetPropertyValue ("receiver")); }
			set { SetPropertyValue ("receiver", value); }
		}
		[TypeScriptBridge ("func")]
		public System.Object Func {
			get { return TypeConverter.ConvertTo<System.Object> (JurassicTypeHosting.Engine, GetPropertyValue ("func")); }
			set { SetPropertyValue ("func", value); }
		}
		[TypeScriptBridge ("script")]
		public string Script {
			get { return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, GetPropertyValue ("script")); }
			set { SetPropertyValue ("script", value); }
		}
		[TypeScriptBridge ("constructCall")]
		public bool ConstructCall {
			get { return TypeConverter.ConvertTo<bool> (JurassicTypeHosting.Engine, GetPropertyValue ("constructCall")); }
			set { SetPropertyValue ("constructCall", value); }
		}
		[TypeScriptBridge ("debuggerFrame")]
		public bool DebuggerFrame {
			get { return TypeConverter.ConvertTo<bool> (JurassicTypeHosting.Engine, GetPropertyValue ("debuggerFrame")); }
			set { SetPropertyValue ("debuggerFrame", value); }
		}
		[TypeScriptBridge ("arguments")]
		public TypeScriptArray<TypeScriptServiceBridge.V8Debugger.NameValuePair> Arguments {
			get { return new TypeScriptArray<TypeScriptServiceBridge.V8Debugger.NameValuePair> ((ArrayInstance) GetPropertyValue ("arguments")); }
			set { SetPropertyValue ("arguments", value != null ? value.Instance : null); }
		}
		[TypeScriptBridge ("locals")]
		public TypeScriptArray<TypeScriptServiceBridge.V8Debugger.NameValuePair> Locals {
			get { return new TypeScriptArray<TypeScriptServiceBridge.V8Debugger.NameValuePair> ((ArrayInstance) GetPropertyValue ("locals")); }
			set { SetPropertyValue ("locals", value != null ? value.Instance : null); }
		}
		[TypeScriptBridge ("position")]
		public double Position {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("position")); }
			set { SetPropertyValue ("position", value); }
		}
		[TypeScriptBridge ("line")]
		public double Line {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("line")); }
			set { SetPropertyValue ("line", value); }
		}
		[TypeScriptBridge ("column")]
		public double Column {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("column")); }
			set { SetPropertyValue ("column", value); }
		}
		[TypeScriptBridge ("sourceLineText")]
		public string SourceLineText {
			get { return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, GetPropertyValue ("sourceLineText")); }
			set { SetPropertyValue ("sourceLineText", value); }
		}
		[TypeScriptBridge ("scopes")]
		public TypeScriptArray<TypeScriptServiceBridge.V8Debugger.Scope> Scopes {
			get { return new TypeScriptArray<TypeScriptServiceBridge.V8Debugger.Scope> ((ArrayInstance) GetPropertyValue ("scopes")); }
			set { SetPropertyValue ("scopes", value != null ? value.Instance : null); }
		}
		[TypeScriptBridge ("get_index")]
		public double Get_index ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_index"));
		}
		[TypeScriptBridge ("set_index")]
		public void Set_index (double @value)
		{
			CallMemberFunction ("set_index", @value);
		}
		[TypeScriptBridge ("get_receiver")]
		public System.Object Get_receiver ()
		{
			return TypeConverter.ConvertTo<System.Object> (JurassicTypeHosting.Engine, CallMemberFunction ("get_receiver"));
		}
		[TypeScriptBridge ("set_receiver")]
		public void Set_receiver (System.Object @value)
		{
			CallMemberFunction ("set_receiver", @value);
		}
		[TypeScriptBridge ("get_func")]
		public System.Object Get_func ()
		{
			return TypeConverter.ConvertTo<System.Object> (JurassicTypeHosting.Engine, CallMemberFunction ("get_func"));
		}
		[TypeScriptBridge ("set_func")]
		public void Set_func (System.Object @value)
		{
			CallMemberFunction ("set_func", @value);
		}
		[TypeScriptBridge ("get_script")]
		public string Get_script ()
		{
			return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, CallMemberFunction ("get_script"));
		}
		[TypeScriptBridge ("set_script")]
		public void Set_script (string @value)
		{
			CallMemberFunction ("set_script", @value);
		}
		[TypeScriptBridge ("get_constructCall")]
		public bool Get_constructCall ()
		{
			return TypeConverter.ConvertTo<bool> (JurassicTypeHosting.Engine, CallMemberFunction ("get_constructCall"));
		}
		[TypeScriptBridge ("set_constructCall")]
		public void Set_constructCall (bool @value)
		{
			CallMemberFunction ("set_constructCall", @value);
		}
		[TypeScriptBridge ("get_debuggerFrame")]
		public bool Get_debuggerFrame ()
		{
			return TypeConverter.ConvertTo<bool> (JurassicTypeHosting.Engine, CallMemberFunction ("get_debuggerFrame"));
		}
		[TypeScriptBridge ("set_debuggerFrame")]
		public void Set_debuggerFrame (bool @value)
		{
			CallMemberFunction ("set_debuggerFrame", @value);
		}
		[TypeScriptBridge ("get_arguments")]
		public TypeScriptArray<TypeScriptServiceBridge.V8Debugger.NameValuePair> Get_arguments ()
		{
			return new TypeScriptArray<TypeScriptServiceBridge.V8Debugger.NameValuePair> ((ArrayInstance) CallMemberFunction ("get_arguments"));
		}
		[TypeScriptBridge ("set_arguments")]
		public void Set_arguments (TypeScriptArray<TypeScriptServiceBridge.V8Debugger.NameValuePair> @value)
		{
			CallMemberFunction ("set_arguments", @value);
		}
		[TypeScriptBridge ("get_locals")]
		public TypeScriptArray<TypeScriptServiceBridge.V8Debugger.NameValuePair> Get_locals ()
		{
			return new TypeScriptArray<TypeScriptServiceBridge.V8Debugger.NameValuePair> ((ArrayInstance) CallMemberFunction ("get_locals"));
		}
		[TypeScriptBridge ("set_locals")]
		public void Set_locals (TypeScriptArray<TypeScriptServiceBridge.V8Debugger.NameValuePair> @value)
		{
			CallMemberFunction ("set_locals", @value);
		}
		[TypeScriptBridge ("get_position")]
		public double Get_position ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_position"));
		}
		[TypeScriptBridge ("set_position")]
		public void Set_position (double @value)
		{
			CallMemberFunction ("set_position", @value);
		}
		[TypeScriptBridge ("get_line")]
		public double Get_line ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_line"));
		}
		[TypeScriptBridge ("set_line")]
		public void Set_line (double @value)
		{
			CallMemberFunction ("set_line", @value);
		}
		[TypeScriptBridge ("get_column")]
		public double Get_column ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_column"));
		}
		[TypeScriptBridge ("set_column")]
		public void Set_column (double @value)
		{
			CallMemberFunction ("set_column", @value);
		}
		[TypeScriptBridge ("get_sourceLineText")]
		public string Get_sourceLineText ()
		{
			return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, CallMemberFunction ("get_sourceLineText"));
		}
		[TypeScriptBridge ("set_sourceLineText")]
		public void Set_sourceLineText (string @value)
		{
			CallMemberFunction ("set_sourceLineText", @value);
		}
		[TypeScriptBridge ("get_scopes")]
		public TypeScriptArray<TypeScriptServiceBridge.V8Debugger.Scope> Get_scopes ()
		{
			return new TypeScriptArray<TypeScriptServiceBridge.V8Debugger.Scope> ((ArrayInstance) CallMemberFunction ("get_scopes"));
		}
		[TypeScriptBridge ("set_scopes")]
		public void Set_scopes (TypeScriptArray<TypeScriptServiceBridge.V8Debugger.Scope> @value)
		{
			CallMemberFunction ("set_scopes", @value);
		}
	}
	public class NameValuePair : TypeScriptObject
			
	{
		public NameValuePair (ObjectInstance instance) : base (instance) {}
		public NameValuePair ()
			 : base (CallConstructor ("V8Debugger", "NameValuePair"))
		{
		}
		[TypeScriptBridge ("name")]
		public string Name {
			get { return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, GetPropertyValue ("name")); }
			set { SetPropertyValue ("name", value); }
		}
		[TypeScriptBridge ("value")]
		public object Value {
			get { return TypeConverter.ConvertTo<object> (JurassicTypeHosting.Engine, GetPropertyValue ("value")); }
			set { SetPropertyValue ("value", value); }
		}
		[TypeScriptBridge ("get_name")]
		public string Get_name ()
		{
			return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, CallMemberFunction ("get_name"));
		}
		[TypeScriptBridge ("set_name")]
		public void Set_name (string @value)
		{
			CallMemberFunction ("set_name", @value);
		}
		[TypeScriptBridge ("get_value")]
		public object Get_value ()
		{
			return TypeConverter.ConvertTo<object> (JurassicTypeHosting.Engine, CallMemberFunction ("get_value"));
		}
		[TypeScriptBridge ("set_value")]
		public void Set_value (object @value)
		{
			CallMemberFunction ("set_value", @value);
		}
	}
	public class ScopeRequestArguments : TypeScriptObject
			
	{
		public ScopeRequestArguments (ObjectInstance instance) : base (instance) {}
		public ScopeRequestArguments ()
			 : base (CallConstructor ("V8Debugger", "ScopeRequestArguments"))
		{
		}
		[TypeScriptBridge ("number")]
		public double Number {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("number")); }
			set { SetPropertyValue ("number", value); }
		}
		[TypeScriptBridge ("frameNumber")]
		public double FrameNumber {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("frameNumber")); }
			set { SetPropertyValue ("frameNumber", value); }
		}
		[TypeScriptBridge ("get_number")]
		public double Get_number ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_number"));
		}
		[TypeScriptBridge ("set_number")]
		public void Set_number (double @value)
		{
			CallMemberFunction ("set_number", @value);
		}
		[TypeScriptBridge ("get_frameNumber")]
		public double Get_frameNumber ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_frameNumber"));
		}
		[TypeScriptBridge ("set_frameNumber")]
		public void Set_frameNumber (double @value)
		{
			CallMemberFunction ("set_frameNumber", @value);
		}
	}
	public class ScopeType : TypeScriptObject
			
	{
		public ScopeType (ObjectInstance instance) : base (instance) {}
		public ScopeType ()
			 : base (CallConstructor ("V8Debugger", "ScopeType"))
		{
		}
		[TypeScriptBridge ("Global")]
		public double Global {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("Global")); }
			set { SetPropertyValue ("Global", value); }
		}
		[TypeScriptBridge ("Local")]
		public double Local {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("Local")); }
			set { SetPropertyValue ("Local", value); }
		}
		[TypeScriptBridge ("With")]
		public double With {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("With")); }
			set { SetPropertyValue ("With", value); }
		}
		[TypeScriptBridge ("Closure")]
		public double Closure {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("Closure")); }
			set { SetPropertyValue ("Closure", value); }
		}
		[TypeScriptBridge ("Catch")]
		public double Catch {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("Catch")); }
			set { SetPropertyValue ("Catch", value); }
		}
		[TypeScriptBridge ("get_Global")]
		public double Get_Global ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_Global"));
		}
		[TypeScriptBridge ("set_Global")]
		public void Set_Global (double @value)
		{
			CallMemberFunction ("set_Global", @value);
		}
		[TypeScriptBridge ("get_Local")]
		public double Get_Local ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_Local"));
		}
		[TypeScriptBridge ("set_Local")]
		public void Set_Local (double @value)
		{
			CallMemberFunction ("set_Local", @value);
		}
		[TypeScriptBridge ("get_With")]
		public double Get_With ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_With"));
		}
		[TypeScriptBridge ("set_With")]
		public void Set_With (double @value)
		{
			CallMemberFunction ("set_With", @value);
		}
		[TypeScriptBridge ("get_Closure")]
		public double Get_Closure ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_Closure"));
		}
		[TypeScriptBridge ("set_Closure")]
		public void Set_Closure (double @value)
		{
			CallMemberFunction ("set_Closure", @value);
		}
		[TypeScriptBridge ("get_Catch")]
		public double Get_Catch ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_Catch"));
		}
		[TypeScriptBridge ("set_Catch")]
		public void Set_Catch (double @value)
		{
			CallMemberFunction ("set_Catch", @value);
		}
	}
	public class Scope : TypeScriptObject
			
	{
		public Scope (ObjectInstance instance) : base (instance) {}
		public Scope ()
			 : base (CallConstructor ("V8Debugger", "Scope"))
		{
		}
		[TypeScriptBridge ("index")]
		public double Index {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("index")); }
			set { SetPropertyValue ("index", value); }
		}
		[TypeScriptBridge ("frameIndex")]
		public double FrameIndex {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("frameIndex")); }
			set { SetPropertyValue ("frameIndex", value); }
		}
		[TypeScriptBridge ("type")]
		public double Type {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("type")); }
			set { SetPropertyValue ("type", value); }
		}
		[TypeScriptBridge ("object")]
		public string Object {
			get { return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, GetPropertyValue ("object")); }
			set { SetPropertyValue ("object", value); }
		}
		[TypeScriptBridge ("get_index")]
		public double Get_index ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_index"));
		}
		[TypeScriptBridge ("set_index")]
		public void Set_index (double @value)
		{
			CallMemberFunction ("set_index", @value);
		}
		[TypeScriptBridge ("get_frameIndex")]
		public double Get_frameIndex ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_frameIndex"));
		}
		[TypeScriptBridge ("set_frameIndex")]
		public void Set_frameIndex (double @value)
		{
			CallMemberFunction ("set_frameIndex", @value);
		}
		[TypeScriptBridge ("get_type")]
		public double Get_type ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_type"));
		}
		[TypeScriptBridge ("set_type")]
		public void Set_type (double @value)
		{
			CallMemberFunction ("set_type", @value);
		}
		[TypeScriptBridge ("get_object")]
		public string Get_object ()
		{
			return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, CallMemberFunction ("get_object"));
		}
		[TypeScriptBridge ("set_object")]
		public void Set_object (string @value)
		{
			CallMemberFunction ("set_object", @value);
		}
	}
	public class ScopesRequestArguments : TypeScriptObject
			
	{
		public ScopesRequestArguments (ObjectInstance instance) : base (instance) {}
		public ScopesRequestArguments ()
			 : base (CallConstructor ("V8Debugger", "ScopesRequestArguments"))
		{
		}
		[TypeScriptBridge ("frameNumber")]
		public double FrameNumber {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("frameNumber")); }
			set { SetPropertyValue ("frameNumber", value); }
		}
		[TypeScriptBridge ("get_frameNumber")]
		public double Get_frameNumber ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_frameNumber"));
		}
		[TypeScriptBridge ("set_frameNumber")]
		public void Set_frameNumber (double @value)
		{
			CallMemberFunction ("set_frameNumber", @value);
		}
	}
	public class ScopesResponseBody : TypeScriptObject
			
	{
		public ScopesResponseBody (ObjectInstance instance) : base (instance) {}
		public ScopesResponseBody ()
			 : base (CallConstructor ("V8Debugger", "ScopesResponseBody"))
		{
		}
		[TypeScriptBridge ("fromScope")]
		public double FromScope {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("fromScope")); }
			set { SetPropertyValue ("fromScope", value); }
		}
		[TypeScriptBridge ("toScope")]
		public double ToScope {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("toScope")); }
			set { SetPropertyValue ("toScope", value); }
		}
		[TypeScriptBridge ("totalScopes")]
		public double TotalScopes {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("totalScopes")); }
			set { SetPropertyValue ("totalScopes", value); }
		}
		[TypeScriptBridge ("scopes")]
		public TypeScriptArray<TypeScriptServiceBridge.V8Debugger.Scope> Scopes {
			get { return new TypeScriptArray<TypeScriptServiceBridge.V8Debugger.Scope> ((ArrayInstance) GetPropertyValue ("scopes")); }
			set { SetPropertyValue ("scopes", value != null ? value.Instance : null); }
		}
		[TypeScriptBridge ("get_fromScope")]
		public double Get_fromScope ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_fromScope"));
		}
		[TypeScriptBridge ("set_fromScope")]
		public void Set_fromScope (double @value)
		{
			CallMemberFunction ("set_fromScope", @value);
		}
		[TypeScriptBridge ("get_toScope")]
		public double Get_toScope ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_toScope"));
		}
		[TypeScriptBridge ("set_toScope")]
		public void Set_toScope (double @value)
		{
			CallMemberFunction ("set_toScope", @value);
		}
		[TypeScriptBridge ("get_totalScopes")]
		public double Get_totalScopes ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_totalScopes"));
		}
		[TypeScriptBridge ("set_totalScopes")]
		public void Set_totalScopes (double @value)
		{
			CallMemberFunction ("set_totalScopes", @value);
		}
		[TypeScriptBridge ("get_scopes")]
		public TypeScriptArray<TypeScriptServiceBridge.V8Debugger.Scope> Get_scopes ()
		{
			return new TypeScriptArray<TypeScriptServiceBridge.V8Debugger.Scope> ((ArrayInstance) CallMemberFunction ("get_scopes"));
		}
		[TypeScriptBridge ("set_scopes")]
		public void Set_scopes (TypeScriptArray<TypeScriptServiceBridge.V8Debugger.Scope> @value)
		{
			CallMemberFunction ("set_scopes", @value);
		}
	}
	public class ScriptsRequestArguments : TypeScriptObject
			
	{
		public ScriptsRequestArguments (ObjectInstance instance) : base (instance) {}
		public ScriptsRequestArguments ()
			 : base (CallConstructor ("V8Debugger", "ScriptsRequestArguments"))
		{
		}
		[TypeScriptBridge ("types")]
		public double Types {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("types")); }
			set { SetPropertyValue ("types", value); }
		}
		[TypeScriptBridge ("ids")]
		public TypeScriptArray<double> Ids {
			get { return TypeConverter.ConvertTo<TypeScriptArray<double>> (JurassicTypeHosting.Engine, GetPropertyValue ("ids")); }
			set { SetPropertyValue ("ids", value); }
		}
		[TypeScriptBridge ("includeSource")]
		public bool IncludeSource {
			get { return TypeConverter.ConvertTo<bool> (JurassicTypeHosting.Engine, GetPropertyValue ("includeSource")); }
			set { SetPropertyValue ("includeSource", value); }
		}
		[TypeScriptBridge ("filter")]
		public object Filter {
			get { return TypeConverter.ConvertTo<object> (JurassicTypeHosting.Engine, GetPropertyValue ("filter")); }
			set { SetPropertyValue ("filter", value); }
		}
		[TypeScriptBridge ("get_types")]
		public double Get_types ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_types"));
		}
		[TypeScriptBridge ("set_types")]
		public void Set_types (double @value)
		{
			CallMemberFunction ("set_types", @value);
		}
		[TypeScriptBridge ("get_ids")]
		public TypeScriptArray<double> Get_ids ()
		{
			return TypeConverter.ConvertTo<TypeScriptArray<double>> (JurassicTypeHosting.Engine, CallMemberFunction ("get_ids"));
		}
		[TypeScriptBridge ("set_ids")]
		public void Set_ids (TypeScriptArray<double> @value)
		{
			CallMemberFunction ("set_ids", @value);
		}
		[TypeScriptBridge ("get_includeSource")]
		public bool Get_includeSource ()
		{
			return TypeConverter.ConvertTo<bool> (JurassicTypeHosting.Engine, CallMemberFunction ("get_includeSource"));
		}
		[TypeScriptBridge ("set_includeSource")]
		public void Set_includeSource (bool @value)
		{
			CallMemberFunction ("set_includeSource", @value);
		}
		[TypeScriptBridge ("get_filter")]
		public object Get_filter ()
		{
			return TypeConverter.ConvertTo<object> (JurassicTypeHosting.Engine, CallMemberFunction ("get_filter"));
		}
		[TypeScriptBridge ("set_filter")]
		public void Set_filter (object @value)
		{
			CallMemberFunction ("set_filter", @value);
		}
	}
	public class SourceLocation : TypeScriptObject
			
	{
		public SourceLocation (ObjectInstance instance) : base (instance) {}
		public SourceLocation ()
			 : base (CallConstructor ("V8Debugger", "SourceLocation"))
		{
		}
		[TypeScriptBridge ("line")]
		public double Line {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("line")); }
			set { SetPropertyValue ("line", value); }
		}
		[TypeScriptBridge ("column")]
		public double Column {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("column")); }
			set { SetPropertyValue ("column", value); }
		}
		[TypeScriptBridge ("get_line")]
		public double Get_line ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_line"));
		}
		[TypeScriptBridge ("set_line")]
		public void Set_line (double @value)
		{
			CallMemberFunction ("set_line", @value);
		}
		[TypeScriptBridge ("get_column")]
		public double Get_column ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_column"));
		}
		[TypeScriptBridge ("set_column")]
		public void Set_column (double @value)
		{
			CallMemberFunction ("set_column", @value);
		}
	}
	public class ScriptsResponseBodyElement : TypeScriptObject
			
	{
		public ScriptsResponseBodyElement (ObjectInstance instance) : base (instance) {}
		public ScriptsResponseBodyElement ()
			 : base (CallConstructor ("V8Debugger", "ScriptsResponseBodyElement"))
		{
		}
		[TypeScriptBridge ("name")]
		public string Name {
			get { return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, GetPropertyValue ("name")); }
			set { SetPropertyValue ("name", value); }
		}
		[TypeScriptBridge ("id")]
		public double Id {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("id")); }
			set { SetPropertyValue ("id", value); }
		}
		[TypeScriptBridge ("lineOffset")]
		public double LineOffset {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("lineOffset")); }
			set { SetPropertyValue ("lineOffset", value); }
		}
		[TypeScriptBridge ("columnOffset")]
		public double ColumnOffset {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("columnOffset")); }
			set { SetPropertyValue ("columnOffset", value); }
		}
		[TypeScriptBridge ("lineCount")]
		public double LineCount {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("lineCount")); }
			set { SetPropertyValue ("lineCount", value); }
		}
		[TypeScriptBridge ("data")]
		public System.Object Data {
			get { return TypeConverter.ConvertTo<System.Object> (JurassicTypeHosting.Engine, GetPropertyValue ("data")); }
			set { SetPropertyValue ("data", value); }
		}
		[TypeScriptBridge ("source")]
		public string Source {
			get { return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, GetPropertyValue ("source")); }
			set { SetPropertyValue ("source", value); }
		}
		[TypeScriptBridge ("sourceStart")]
		public string SourceStart {
			get { return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, GetPropertyValue ("sourceStart")); }
			set { SetPropertyValue ("sourceStart", value); }
		}
		[TypeScriptBridge ("sourceLength")]
		public double SourceLength {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("sourceLength")); }
			set { SetPropertyValue ("sourceLength", value); }
		}
		[TypeScriptBridge ("scriptType")]
		public double ScriptType {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("scriptType")); }
			set { SetPropertyValue ("scriptType", value); }
		}
		[TypeScriptBridge ("compilationType")]
		public double CompilationType {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("compilationType")); }
			set { SetPropertyValue ("compilationType", value); }
		}
		[TypeScriptBridge ("evalFromScript")]
		public string EvalFromScript {
			get { return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, GetPropertyValue ("evalFromScript")); }
			set { SetPropertyValue ("evalFromScript", value); }
		}
		[TypeScriptBridge ("evalFromLocation")]
		public TypeScriptServiceBridge.V8Debugger.SourceLocation EvalFromLocation {
			get { return TypeScriptObject.Create<TypeScriptServiceBridge.V8Debugger.SourceLocation> ((ObjectInstance) GetPropertyValue ("evalFromLocation")); }
			set { SetPropertyValue ("evalFromLocation", value != null ? value.Instance : null); }
		}
		[TypeScriptBridge ("get_name")]
		public string Get_name ()
		{
			return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, CallMemberFunction ("get_name"));
		}
		[TypeScriptBridge ("set_name")]
		public void Set_name (string @value)
		{
			CallMemberFunction ("set_name", @value);
		}
		[TypeScriptBridge ("get_id")]
		public double Get_id ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_id"));
		}
		[TypeScriptBridge ("set_id")]
		public void Set_id (double @value)
		{
			CallMemberFunction ("set_id", @value);
		}
		[TypeScriptBridge ("get_lineOffset")]
		public double Get_lineOffset ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_lineOffset"));
		}
		[TypeScriptBridge ("set_lineOffset")]
		public void Set_lineOffset (double @value)
		{
			CallMemberFunction ("set_lineOffset", @value);
		}
		[TypeScriptBridge ("get_columnOffset")]
		public double Get_columnOffset ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_columnOffset"));
		}
		[TypeScriptBridge ("set_columnOffset")]
		public void Set_columnOffset (double @value)
		{
			CallMemberFunction ("set_columnOffset", @value);
		}
		[TypeScriptBridge ("get_lineCount")]
		public double Get_lineCount ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_lineCount"));
		}
		[TypeScriptBridge ("set_lineCount")]
		public void Set_lineCount (double @value)
		{
			CallMemberFunction ("set_lineCount", @value);
		}
		[TypeScriptBridge ("get_data")]
		public System.Object Get_data ()
		{
			return TypeConverter.ConvertTo<System.Object> (JurassicTypeHosting.Engine, CallMemberFunction ("get_data"));
		}
		[TypeScriptBridge ("set_data")]
		public void Set_data (System.Object @value)
		{
			CallMemberFunction ("set_data", @value);
		}
		[TypeScriptBridge ("get_source")]
		public string Get_source ()
		{
			return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, CallMemberFunction ("get_source"));
		}
		[TypeScriptBridge ("set_source")]
		public void Set_source (string @value)
		{
			CallMemberFunction ("set_source", @value);
		}
		[TypeScriptBridge ("get_sourceStart")]
		public string Get_sourceStart ()
		{
			return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, CallMemberFunction ("get_sourceStart"));
		}
		[TypeScriptBridge ("set_sourceStart")]
		public void Set_sourceStart (string @value)
		{
			CallMemberFunction ("set_sourceStart", @value);
		}
		[TypeScriptBridge ("get_sourceLength")]
		public double Get_sourceLength ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_sourceLength"));
		}
		[TypeScriptBridge ("set_sourceLength")]
		public void Set_sourceLength (double @value)
		{
			CallMemberFunction ("set_sourceLength", @value);
		}
		[TypeScriptBridge ("get_scriptType")]
		public double Get_scriptType ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_scriptType"));
		}
		[TypeScriptBridge ("set_scriptType")]
		public void Set_scriptType (double @value)
		{
			CallMemberFunction ("set_scriptType", @value);
		}
		[TypeScriptBridge ("get_compilationType")]
		public double Get_compilationType ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_compilationType"));
		}
		[TypeScriptBridge ("set_compilationType")]
		public void Set_compilationType (double @value)
		{
			CallMemberFunction ("set_compilationType", @value);
		}
		[TypeScriptBridge ("get_evalFromScript")]
		public string Get_evalFromScript ()
		{
			return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, CallMemberFunction ("get_evalFromScript"));
		}
		[TypeScriptBridge ("set_evalFromScript")]
		public void Set_evalFromScript (string @value)
		{
			CallMemberFunction ("set_evalFromScript", @value);
		}
		[TypeScriptBridge ("get_evalFromLocation")]
		public TypeScriptServiceBridge.V8Debugger.SourceLocation Get_evalFromLocation ()
		{
			return TypeScriptObject.Create<TypeScriptServiceBridge.V8Debugger.SourceLocation>  ((ObjectInstance) CallMemberFunction ("get_evalFromLocation"));
		}
		[TypeScriptBridge ("set_evalFromLocation")]
		public void Set_evalFromLocation (TypeScriptServiceBridge.V8Debugger.SourceLocation @value)
		{
			CallMemberFunction ("set_evalFromLocation", @value);
		}
	}
	public class SourceRequestArguments : TypeScriptObject
			
	{
		public SourceRequestArguments (ObjectInstance instance) : base (instance) {}
		public SourceRequestArguments ()
			 : base (CallConstructor ("V8Debugger", "SourceRequestArguments"))
		{
		}
		[TypeScriptBridge ("frame")]
		public double Frame {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("frame")); }
			set { SetPropertyValue ("frame", value); }
		}
		[TypeScriptBridge ("fromLine")]
		public double FromLine {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("fromLine")); }
			set { SetPropertyValue ("fromLine", value); }
		}
		[TypeScriptBridge ("toLine")]
		public double ToLine {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("toLine")); }
			set { SetPropertyValue ("toLine", value); }
		}
		[TypeScriptBridge ("get_frame")]
		public double Get_frame ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_frame"));
		}
		[TypeScriptBridge ("set_frame")]
		public void Set_frame (double @value)
		{
			CallMemberFunction ("set_frame", @value);
		}
		[TypeScriptBridge ("get_fromLine")]
		public double Get_fromLine ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_fromLine"));
		}
		[TypeScriptBridge ("set_fromLine")]
		public void Set_fromLine (double @value)
		{
			CallMemberFunction ("set_fromLine", @value);
		}
		[TypeScriptBridge ("get_toLine")]
		public double Get_toLine ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_toLine"));
		}
		[TypeScriptBridge ("set_toLine")]
		public void Set_toLine (double @value)
		{
			CallMemberFunction ("set_toLine", @value);
		}
	}
	public class SourceResponseBody : TypeScriptObject
			
	{
		public SourceResponseBody (ObjectInstance instance) : base (instance) {}
		public SourceResponseBody ()
			 : base (CallConstructor ("V8Debugger", "SourceResponseBody"))
		{
		}
		[TypeScriptBridge ("source")]
		public string Source {
			get { return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, GetPropertyValue ("source")); }
			set { SetPropertyValue ("source", value); }
		}
		[TypeScriptBridge ("fromLine")]
		public double FromLine {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("fromLine")); }
			set { SetPropertyValue ("fromLine", value); }
		}
		[TypeScriptBridge ("toLine")]
		public double ToLine {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("toLine")); }
			set { SetPropertyValue ("toLine", value); }
		}
		[TypeScriptBridge ("fromPosition")]
		public double FromPosition {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("fromPosition")); }
			set { SetPropertyValue ("fromPosition", value); }
		}
		[TypeScriptBridge ("toPosition")]
		public double ToPosition {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("toPosition")); }
			set { SetPropertyValue ("toPosition", value); }
		}
		[TypeScriptBridge ("totalLines")]
		public double TotalLines {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("totalLines")); }
			set { SetPropertyValue ("totalLines", value); }
		}
		[TypeScriptBridge ("get_source")]
		public string Get_source ()
		{
			return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, CallMemberFunction ("get_source"));
		}
		[TypeScriptBridge ("set_source")]
		public void Set_source (string @value)
		{
			CallMemberFunction ("set_source", @value);
		}
		[TypeScriptBridge ("get_fromLine")]
		public double Get_fromLine ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_fromLine"));
		}
		[TypeScriptBridge ("set_fromLine")]
		public void Set_fromLine (double @value)
		{
			CallMemberFunction ("set_fromLine", @value);
		}
		[TypeScriptBridge ("get_toLine")]
		public double Get_toLine ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_toLine"));
		}
		[TypeScriptBridge ("set_toLine")]
		public void Set_toLine (double @value)
		{
			CallMemberFunction ("set_toLine", @value);
		}
		[TypeScriptBridge ("get_fromPosition")]
		public double Get_fromPosition ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_fromPosition"));
		}
		[TypeScriptBridge ("set_fromPosition")]
		public void Set_fromPosition (double @value)
		{
			CallMemberFunction ("set_fromPosition", @value);
		}
		[TypeScriptBridge ("get_toPosition")]
		public double Get_toPosition ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_toPosition"));
		}
		[TypeScriptBridge ("set_toPosition")]
		public void Set_toPosition (double @value)
		{
			CallMemberFunction ("set_toPosition", @value);
		}
		[TypeScriptBridge ("get_totalLines")]
		public double Get_totalLines ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_totalLines"));
		}
		[TypeScriptBridge ("set_totalLines")]
		public void Set_totalLines (double @value)
		{
			CallMemberFunction ("set_totalLines", @value);
		}
	}
	public class SetBreakpointRequestArguments : TypeScriptObject
			
	{
		public SetBreakpointRequestArguments (ObjectInstance instance) : base (instance) {}
		public SetBreakpointRequestArguments ()
			 : base (CallConstructor ("V8Debugger", "SetBreakpointRequestArguments"))
		{
		}
		[TypeScriptBridge ("type")]
		public string Type {
			get { return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, GetPropertyValue ("type")); }
			set { SetPropertyValue ("type", value); }
		}
		[TypeScriptBridge ("target")]
		public object Target {
			get { return TypeConverter.ConvertTo<object> (JurassicTypeHosting.Engine, GetPropertyValue ("target")); }
			set { SetPropertyValue ("target", value); }
		}
		[TypeScriptBridge ("line")]
		public double Line {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("line")); }
			set { SetPropertyValue ("line", value); }
		}
		[TypeScriptBridge ("column")]
		public double Column {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("column")); }
			set { SetPropertyValue ("column", value); }
		}
		[TypeScriptBridge ("enabled")]
		public bool Enabled {
			get { return TypeConverter.ConvertTo<bool> (JurassicTypeHosting.Engine, GetPropertyValue ("enabled")); }
			set { SetPropertyValue ("enabled", value); }
		}
		[TypeScriptBridge ("condition")]
		public string Condition {
			get { return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, GetPropertyValue ("condition")); }
			set { SetPropertyValue ("condition", value); }
		}
		[TypeScriptBridge ("ignoreCount")]
		public double IgnoreCount {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("ignoreCount")); }
			set { SetPropertyValue ("ignoreCount", value); }
		}
		[TypeScriptBridge ("get_type")]
		public string Get_type ()
		{
			return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, CallMemberFunction ("get_type"));
		}
		[TypeScriptBridge ("set_type")]
		public void Set_type (string @value)
		{
			CallMemberFunction ("set_type", @value);
		}
		[TypeScriptBridge ("get_target")]
		public object Get_target ()
		{
			return TypeConverter.ConvertTo<object> (JurassicTypeHosting.Engine, CallMemberFunction ("get_target"));
		}
		[TypeScriptBridge ("set_target")]
		public void Set_target (object @value)
		{
			CallMemberFunction ("set_target", @value);
		}
		[TypeScriptBridge ("get_line")]
		public double Get_line ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_line"));
		}
		[TypeScriptBridge ("set_line")]
		public void Set_line (double @value)
		{
			CallMemberFunction ("set_line", @value);
		}
		[TypeScriptBridge ("get_column")]
		public double Get_column ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_column"));
		}
		[TypeScriptBridge ("set_column")]
		public void Set_column (double @value)
		{
			CallMemberFunction ("set_column", @value);
		}
		[TypeScriptBridge ("get_enabled")]
		public bool Get_enabled ()
		{
			return TypeConverter.ConvertTo<bool> (JurassicTypeHosting.Engine, CallMemberFunction ("get_enabled"));
		}
		[TypeScriptBridge ("set_enabled")]
		public void Set_enabled (bool @value)
		{
			CallMemberFunction ("set_enabled", @value);
		}
		[TypeScriptBridge ("get_condition")]
		public string Get_condition ()
		{
			return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, CallMemberFunction ("get_condition"));
		}
		[TypeScriptBridge ("set_condition")]
		public void Set_condition (string @value)
		{
			CallMemberFunction ("set_condition", @value);
		}
		[TypeScriptBridge ("get_ignoreCount")]
		public double Get_ignoreCount ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_ignoreCount"));
		}
		[TypeScriptBridge ("set_ignoreCount")]
		public void Set_ignoreCount (double @value)
		{
			CallMemberFunction ("set_ignoreCount", @value);
		}
	}
	public class SetBreakpointResponseBody : TypeScriptObject
			
	{
		public SetBreakpointResponseBody (ObjectInstance instance) : base (instance) {}
		public SetBreakpointResponseBody ()
			 : base (CallConstructor ("V8Debugger", "SetBreakpointResponseBody"))
		{
		}
		[TypeScriptBridge ("type")]
		public string Type {
			get { return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, GetPropertyValue ("type")); }
			set { SetPropertyValue ("type", value); }
		}
		[TypeScriptBridge ("breakpoint")]
		public double Breakpoint {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("breakpoint")); }
			set { SetPropertyValue ("breakpoint", value); }
		}
		[TypeScriptBridge ("get_type")]
		public string Get_type ()
		{
			return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, CallMemberFunction ("get_type"));
		}
		[TypeScriptBridge ("set_type")]
		public void Set_type (string @value)
		{
			CallMemberFunction ("set_type", @value);
		}
		[TypeScriptBridge ("get_breakpoint")]
		public double Get_breakpoint ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_breakpoint"));
		}
		[TypeScriptBridge ("set_breakpoint")]
		public void Set_breakpoint (double @value)
		{
			CallMemberFunction ("set_breakpoint", @value);
		}
	}
	public class ChangeBreakpointRequestArguments : TypeScriptObject
			
	{
		public ChangeBreakpointRequestArguments (ObjectInstance instance) : base (instance) {}
		public ChangeBreakpointRequestArguments ()
			 : base (CallConstructor ("V8Debugger", "ChangeBreakpointRequestArguments"))
		{
		}
		[TypeScriptBridge ("breakpoint")]
		public double Breakpoint {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("breakpoint")); }
			set { SetPropertyValue ("breakpoint", value); }
		}
		[TypeScriptBridge ("enabled")]
		public bool Enabled {
			get { return TypeConverter.ConvertTo<bool> (JurassicTypeHosting.Engine, GetPropertyValue ("enabled")); }
			set { SetPropertyValue ("enabled", value); }
		}
		[TypeScriptBridge ("condition")]
		public string Condition {
			get { return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, GetPropertyValue ("condition")); }
			set { SetPropertyValue ("condition", value); }
		}
		[TypeScriptBridge ("ignoreCount")]
		public double IgnoreCount {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("ignoreCount")); }
			set { SetPropertyValue ("ignoreCount", value); }
		}
		[TypeScriptBridge ("get_breakpoint")]
		public double Get_breakpoint ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_breakpoint"));
		}
		[TypeScriptBridge ("set_breakpoint")]
		public void Set_breakpoint (double @value)
		{
			CallMemberFunction ("set_breakpoint", @value);
		}
		[TypeScriptBridge ("get_enabled")]
		public bool Get_enabled ()
		{
			return TypeConverter.ConvertTo<bool> (JurassicTypeHosting.Engine, CallMemberFunction ("get_enabled"));
		}
		[TypeScriptBridge ("set_enabled")]
		public void Set_enabled (bool @value)
		{
			CallMemberFunction ("set_enabled", @value);
		}
		[TypeScriptBridge ("get_condition")]
		public string Get_condition ()
		{
			return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, CallMemberFunction ("get_condition"));
		}
		[TypeScriptBridge ("set_condition")]
		public void Set_condition (string @value)
		{
			CallMemberFunction ("set_condition", @value);
		}
		[TypeScriptBridge ("get_ignoreCount")]
		public double Get_ignoreCount ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_ignoreCount"));
		}
		[TypeScriptBridge ("set_ignoreCount")]
		public void Set_ignoreCount (double @value)
		{
			CallMemberFunction ("set_ignoreCount", @value);
		}
	}
	public class ClearBreakpointRequestArguments : TypeScriptObject
			
	{
		public ClearBreakpointRequestArguments (ObjectInstance instance) : base (instance) {}
		public ClearBreakpointRequestArguments ()
			 : base (CallConstructor ("V8Debugger", "ClearBreakpointRequestArguments"))
		{
		}
		[TypeScriptBridge ("breakpoint")]
		public double Breakpoint {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("breakpoint")); }
			set { SetPropertyValue ("breakpoint", value); }
		}
		[TypeScriptBridge ("get_breakpoint")]
		public double Get_breakpoint ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_breakpoint"));
		}
		[TypeScriptBridge ("set_breakpoint")]
		public void Set_breakpoint (double @value)
		{
			CallMemberFunction ("set_breakpoint", @value);
		}
	}
	public class ClearBreakpointResponseBody : TypeScriptObject
			
	{
		public ClearBreakpointResponseBody (ObjectInstance instance) : base (instance) {}
		public ClearBreakpointResponseBody ()
			 : base (CallConstructor ("V8Debugger", "ClearBreakpointResponseBody"))
		{
		}
		[TypeScriptBridge ("type")]
		public string Type {
			get { return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, GetPropertyValue ("type")); }
			set { SetPropertyValue ("type", value); }
		}
		[TypeScriptBridge ("breakpoint")]
		public double Breakpoint {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("breakpoint")); }
			set { SetPropertyValue ("breakpoint", value); }
		}
		[TypeScriptBridge ("get_type")]
		public string Get_type ()
		{
			return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, CallMemberFunction ("get_type"));
		}
		[TypeScriptBridge ("set_type")]
		public void Set_type (string @value)
		{
			CallMemberFunction ("set_type", @value);
		}
		[TypeScriptBridge ("get_breakpoint")]
		public double Get_breakpoint ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_breakpoint"));
		}
		[TypeScriptBridge ("set_breakpoint")]
		public void Set_breakpoint (double @value)
		{
			CallMemberFunction ("set_breakpoint", @value);
		}
	}
	public class SetExceptionBreakRequestArguments : TypeScriptObject
			
	{
		public SetExceptionBreakRequestArguments (ObjectInstance instance) : base (instance) {}
		public SetExceptionBreakRequestArguments ()
			 : base (CallConstructor ("V8Debugger", "SetExceptionBreakRequestArguments"))
		{
		}
		[TypeScriptBridge ("type")]
		public string Type {
			get { return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, GetPropertyValue ("type")); }
			set { SetPropertyValue ("type", value); }
		}
		[TypeScriptBridge ("enabled")]
		public bool Enabled {
			get { return TypeConverter.ConvertTo<bool> (JurassicTypeHosting.Engine, GetPropertyValue ("enabled")); }
			set { SetPropertyValue ("enabled", value); }
		}
		[TypeScriptBridge ("get_type")]
		public string Get_type ()
		{
			return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, CallMemberFunction ("get_type"));
		}
		[TypeScriptBridge ("set_type")]
		public void Set_type (string @value)
		{
			CallMemberFunction ("set_type", @value);
		}
		[TypeScriptBridge ("get_enabled")]
		public bool Get_enabled ()
		{
			return TypeConverter.ConvertTo<bool> (JurassicTypeHosting.Engine, CallMemberFunction ("get_enabled"));
		}
		[TypeScriptBridge ("set_enabled")]
		public void Set_enabled (bool @value)
		{
			CallMemberFunction ("set_enabled", @value);
		}
	}
	public class SetExceptionBreakResponseBody : TypeScriptObject
			
	{
		public SetExceptionBreakResponseBody (ObjectInstance instance) : base (instance) {}
		public SetExceptionBreakResponseBody ()
			 : base (CallConstructor ("V8Debugger", "SetExceptionBreakResponseBody"))
		{
		}
		[TypeScriptBridge ("type")]
		public string Type {
			get { return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, GetPropertyValue ("type")); }
			set { SetPropertyValue ("type", value); }
		}
		[TypeScriptBridge ("enabled")]
		public bool Enabled {
			get { return TypeConverter.ConvertTo<bool> (JurassicTypeHosting.Engine, GetPropertyValue ("enabled")); }
			set { SetPropertyValue ("enabled", value); }
		}
		[TypeScriptBridge ("get_type")]
		public string Get_type ()
		{
			return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, CallMemberFunction ("get_type"));
		}
		[TypeScriptBridge ("set_type")]
		public void Set_type (string @value)
		{
			CallMemberFunction ("set_type", @value);
		}
		[TypeScriptBridge ("get_enabled")]
		public bool Get_enabled ()
		{
			return TypeConverter.ConvertTo<bool> (JurassicTypeHosting.Engine, CallMemberFunction ("get_enabled"));
		}
		[TypeScriptBridge ("set_enabled")]
		public void Set_enabled (bool @value)
		{
			CallMemberFunction ("set_enabled", @value);
		}
	}
	public class V8FlagsRequestArguments : TypeScriptObject
			
	{
		public V8FlagsRequestArguments (ObjectInstance instance) : base (instance) {}
		public V8FlagsRequestArguments ()
			 : base (CallConstructor ("V8Debugger", "V8FlagsRequestArguments"))
		{
		}
		[TypeScriptBridge ("flags")]
		public string Flags {
			get { return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, GetPropertyValue ("flags")); }
			set { SetPropertyValue ("flags", value); }
		}
		[TypeScriptBridge ("get_flags")]
		public string Get_flags ()
		{
			return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, CallMemberFunction ("get_flags"));
		}
		[TypeScriptBridge ("set_flags")]
		public void Set_flags (string @value)
		{
			CallMemberFunction ("set_flags", @value);
		}
	}
	public class VersionResponseBody : TypeScriptObject
			
	{
		public VersionResponseBody (ObjectInstance instance) : base (instance) {}
		public VersionResponseBody ()
			 : base (CallConstructor ("V8Debugger", "VersionResponseBody"))
		{
		}
		[TypeScriptBridge ("V8Version")]
		public string V8Version {
			get { return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, GetPropertyValue ("V8Version")); }
			set { SetPropertyValue ("V8Version", value); }
		}
		[TypeScriptBridge ("get_V8Version")]
		public string Get_V8Version ()
		{
			return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, CallMemberFunction ("get_V8Version"));
		}
		[TypeScriptBridge ("set_V8Version")]
		public void Set_V8Version (string @value)
		{
			CallMemberFunction ("set_V8Version", @value);
		}
	}
	public class ProfileRequestArguments : TypeScriptObject
			
	{
		public ProfileRequestArguments (ObjectInstance instance) : base (instance) {}
		public ProfileRequestArguments ()
			 : base (CallConstructor ("V8Debugger", "ProfileRequestArguments"))
		{
		}
		[TypeScriptBridge ("command")]
		public string Command {
			get { return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, GetPropertyValue ("command")); }
			set { SetPropertyValue ("command", value); }
		}
		[TypeScriptBridge ("get_command")]
		public string Get_command ()
		{
			return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, CallMemberFunction ("get_command"));
		}
		[TypeScriptBridge ("set_command")]
		public void Set_command (string @value)
		{
			CallMemberFunction ("set_command", @value);
		}
	}
	public class GcRequestArguments : TypeScriptObject
			
	{
		public GcRequestArguments (ObjectInstance instance) : base (instance) {}
		public GcRequestArguments ()
			 : base (CallConstructor ("V8Debugger", "GcRequestArguments"))
		{
		}
		[TypeScriptBridge ("type")]
		public string Type {
			get { return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, GetPropertyValue ("type")); }
			set { SetPropertyValue ("type", value); }
		}
		[TypeScriptBridge ("get_type")]
		public string Get_type ()
		{
			return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, CallMemberFunction ("get_type"));
		}
		[TypeScriptBridge ("set_type")]
		public void Set_type (string @value)
		{
			CallMemberFunction ("set_type", @value);
		}
	}
	public class GcResoponseBody : TypeScriptObject
			
	{
		public GcResoponseBody (ObjectInstance instance) : base (instance) {}
		public GcResoponseBody ()
			 : base (CallConstructor ("V8Debugger", "GcResoponseBody"))
		{
		}
		[TypeScriptBridge ("before")]
		public double Before {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("before")); }
			set { SetPropertyValue ("before", value); }
		}
		[TypeScriptBridge ("after")]
		public double After {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("after")); }
			set { SetPropertyValue ("after", value); }
		}
		[TypeScriptBridge ("get_before")]
		public double Get_before ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_before"));
		}
		[TypeScriptBridge ("set_before")]
		public void Set_before (double @value)
		{
			CallMemberFunction ("set_before", @value);
		}
		[TypeScriptBridge ("get_after")]
		public double Get_after ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_after"));
		}
		[TypeScriptBridge ("set_after")]
		public void Set_after (double @value)
		{
			CallMemberFunction ("set_after", @value);
		}
	}
	public class ListBreakpointResponseBody : TypeScriptObject
			
	{
		public ListBreakpointResponseBody (ObjectInstance instance) : base (instance) {}
		public ListBreakpointResponseBody ()
			 : base (CallConstructor ("V8Debugger", "ListBreakpointResponseBody"))
		{
		}
		[TypeScriptBridge ("breakpoints")]
		public TypeScriptArray<TypeScriptServiceBridge.V8Debugger.BreakpointInfo> Breakpoints {
			get { return new TypeScriptArray<TypeScriptServiceBridge.V8Debugger.BreakpointInfo> ((ArrayInstance) GetPropertyValue ("breakpoints")); }
			set { SetPropertyValue ("breakpoints", value != null ? value.Instance : null); }
		}
		[TypeScriptBridge ("breakOnExceptions")]
		public bool BreakOnExceptions {
			get { return TypeConverter.ConvertTo<bool> (JurassicTypeHosting.Engine, GetPropertyValue ("breakOnExceptions")); }
			set { SetPropertyValue ("breakOnExceptions", value); }
		}
		[TypeScriptBridge ("breakOnUncaughtExceptions")]
		public bool BreakOnUncaughtExceptions {
			get { return TypeConverter.ConvertTo<bool> (JurassicTypeHosting.Engine, GetPropertyValue ("breakOnUncaughtExceptions")); }
			set { SetPropertyValue ("breakOnUncaughtExceptions", value); }
		}
		[TypeScriptBridge ("get_breakpoints")]
		public TypeScriptArray<TypeScriptServiceBridge.V8Debugger.BreakpointInfo> Get_breakpoints ()
		{
			return new TypeScriptArray<TypeScriptServiceBridge.V8Debugger.BreakpointInfo> ((ArrayInstance) CallMemberFunction ("get_breakpoints"));
		}
		[TypeScriptBridge ("set_breakpoints")]
		public void Set_breakpoints (TypeScriptArray<TypeScriptServiceBridge.V8Debugger.BreakpointInfo> @value)
		{
			CallMemberFunction ("set_breakpoints", @value);
		}
		[TypeScriptBridge ("get_breakOnExceptions")]
		public bool Get_breakOnExceptions ()
		{
			return TypeConverter.ConvertTo<bool> (JurassicTypeHosting.Engine, CallMemberFunction ("get_breakOnExceptions"));
		}
		[TypeScriptBridge ("set_breakOnExceptions")]
		public void Set_breakOnExceptions (bool @value)
		{
			CallMemberFunction ("set_breakOnExceptions", @value);
		}
		[TypeScriptBridge ("get_breakOnUncaughtExceptions")]
		public bool Get_breakOnUncaughtExceptions ()
		{
			return TypeConverter.ConvertTo<bool> (JurassicTypeHosting.Engine, CallMemberFunction ("get_breakOnUncaughtExceptions"));
		}
		[TypeScriptBridge ("set_breakOnUncaughtExceptions")]
		public void Set_breakOnUncaughtExceptions (bool @value)
		{
			CallMemberFunction ("set_breakOnUncaughtExceptions", @value);
		}
	}
	public class BreakpointInfo : TypeScriptObject
			
	{
		public BreakpointInfo (ObjectInstance instance) : base (instance) {}
		public BreakpointInfo ()
			 : base (CallConstructor ("V8Debugger", "BreakpointInfo"))
		{
		}
		[TypeScriptBridge ("type")]
		public string Type {
			get { return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, GetPropertyValue ("type")); }
			set { SetPropertyValue ("type", value); }
		}
		[TypeScriptBridge ("script_id")]
		public double Script_id {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("script_id")); }
			set { SetPropertyValue ("script_id", value); }
		}
		[TypeScriptBridge ("script_name")]
		public string Script_name {
			get { return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, GetPropertyValue ("script_name")); }
			set { SetPropertyValue ("script_name", value); }
		}
		[TypeScriptBridge ("number")]
		public double Number {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("number")); }
			set { SetPropertyValue ("number", value); }
		}
		[TypeScriptBridge ("line")]
		public double Line {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("line")); }
			set { SetPropertyValue ("line", value); }
		}
		[TypeScriptBridge ("column")]
		public double Column {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("column")); }
			set { SetPropertyValue ("column", value); }
		}
		[TypeScriptBridge ("groupId")]
		public double GroupId {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("groupId")); }
			set { SetPropertyValue ("groupId", value); }
		}
		[TypeScriptBridge ("hit_count")]
		public double Hit_count {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("hit_count")); }
			set { SetPropertyValue ("hit_count", value); }
		}
		[TypeScriptBridge ("active")]
		public bool Active {
			get { return TypeConverter.ConvertTo<bool> (JurassicTypeHosting.Engine, GetPropertyValue ("active")); }
			set { SetPropertyValue ("active", value); }
		}
		[TypeScriptBridge ("ignoreCount")]
		public double IgnoreCount {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("ignoreCount")); }
			set { SetPropertyValue ("ignoreCount", value); }
		}
		[TypeScriptBridge ("actual_locations")]
		public TypeScriptArray<TypeScriptServiceBridge.V8Debugger.SourceLocation> Actual_locations {
			get { return new TypeScriptArray<TypeScriptServiceBridge.V8Debugger.SourceLocation> ((ArrayInstance) GetPropertyValue ("actual_locations")); }
			set { SetPropertyValue ("actual_locations", value != null ? value.Instance : null); }
		}
		[TypeScriptBridge ("get_type")]
		public string Get_type ()
		{
			return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, CallMemberFunction ("get_type"));
		}
		[TypeScriptBridge ("set_type")]
		public void Set_type (string @value)
		{
			CallMemberFunction ("set_type", @value);
		}
		[TypeScriptBridge ("get_script_id")]
		public double Get_script_id ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_script_id"));
		}
		[TypeScriptBridge ("set_script_id")]
		public void Set_script_id (double @value)
		{
			CallMemberFunction ("set_script_id", @value);
		}
		[TypeScriptBridge ("get_script_name")]
		public string Get_script_name ()
		{
			return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, CallMemberFunction ("get_script_name"));
		}
		[TypeScriptBridge ("set_script_name")]
		public void Set_script_name (string @value)
		{
			CallMemberFunction ("set_script_name", @value);
		}
		[TypeScriptBridge ("get_number")]
		public double Get_number ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_number"));
		}
		[TypeScriptBridge ("set_number")]
		public void Set_number (double @value)
		{
			CallMemberFunction ("set_number", @value);
		}
		[TypeScriptBridge ("get_line")]
		public double Get_line ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_line"));
		}
		[TypeScriptBridge ("set_line")]
		public void Set_line (double @value)
		{
			CallMemberFunction ("set_line", @value);
		}
		[TypeScriptBridge ("get_column")]
		public double Get_column ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_column"));
		}
		[TypeScriptBridge ("set_column")]
		public void Set_column (double @value)
		{
			CallMemberFunction ("set_column", @value);
		}
		[TypeScriptBridge ("get_groupId")]
		public double Get_groupId ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_groupId"));
		}
		[TypeScriptBridge ("set_groupId")]
		public void Set_groupId (double @value)
		{
			CallMemberFunction ("set_groupId", @value);
		}
		[TypeScriptBridge ("get_hit_count")]
		public double Get_hit_count ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_hit_count"));
		}
		[TypeScriptBridge ("set_hit_count")]
		public void Set_hit_count (double @value)
		{
			CallMemberFunction ("set_hit_count", @value);
		}
		[TypeScriptBridge ("get_active")]
		public bool Get_active ()
		{
			return TypeConverter.ConvertTo<bool> (JurassicTypeHosting.Engine, CallMemberFunction ("get_active"));
		}
		[TypeScriptBridge ("set_active")]
		public void Set_active (bool @value)
		{
			CallMemberFunction ("set_active", @value);
		}
		[TypeScriptBridge ("get_ignoreCount")]
		public double Get_ignoreCount ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_ignoreCount"));
		}
		[TypeScriptBridge ("set_ignoreCount")]
		public void Set_ignoreCount (double @value)
		{
			CallMemberFunction ("set_ignoreCount", @value);
		}
		[TypeScriptBridge ("get_actual_locations")]
		public TypeScriptArray<TypeScriptServiceBridge.V8Debugger.SourceLocation> Get_actual_locations ()
		{
			return new TypeScriptArray<TypeScriptServiceBridge.V8Debugger.SourceLocation> ((ArrayInstance) CallMemberFunction ("get_actual_locations"));
		}
		[TypeScriptBridge ("set_actual_locations")]
		public void Set_actual_locations (TypeScriptArray<TypeScriptServiceBridge.V8Debugger.SourceLocation> @value)
		{
			CallMemberFunction ("set_actual_locations", @value);
		}
	}
	public class BreakEventBody : TypeScriptObject
			
	{
		public BreakEventBody (ObjectInstance instance) : base (instance) {}
		public BreakEventBody ()
			 : base (CallConstructor ("V8Debugger", "BreakEventBody"))
		{
		}
		[TypeScriptBridge ("invocationText")]
		public string InvocationText {
			get { return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, GetPropertyValue ("invocationText")); }
			set { SetPropertyValue ("invocationText", value); }
		}
		[TypeScriptBridge ("sourceLine")]
		public double SourceLine {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("sourceLine")); }
			set { SetPropertyValue ("sourceLine", value); }
		}
		[TypeScriptBridge ("sourceColumn")]
		public double SourceColumn {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("sourceColumn")); }
			set { SetPropertyValue ("sourceColumn", value); }
		}
		[TypeScriptBridge ("sourceLineText")]
		public string SourceLineText {
			get { return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, GetPropertyValue ("sourceLineText")); }
			set { SetPropertyValue ("sourceLineText", value); }
		}
		[TypeScriptBridge ("script")]
		public TypeScriptServiceBridge.V8Debugger.ScriptInfo Script {
			get { return TypeScriptObject.Create<TypeScriptServiceBridge.V8Debugger.ScriptInfo> ((ObjectInstance) GetPropertyValue ("script")); }
			set { SetPropertyValue ("script", value != null ? value.Instance : null); }
		}
		[TypeScriptBridge ("breakpoints")]
		public TypeScriptArray<double> Breakpoints {
			get { return TypeConverter.ConvertTo<TypeScriptArray<double>> (JurassicTypeHosting.Engine, GetPropertyValue ("breakpoints")); }
			set { SetPropertyValue ("breakpoints", value); }
		}
		[TypeScriptBridge ("get_invocationText")]
		public string Get_invocationText ()
		{
			return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, CallMemberFunction ("get_invocationText"));
		}
		[TypeScriptBridge ("set_invocationText")]
		public void Set_invocationText (string @value)
		{
			CallMemberFunction ("set_invocationText", @value);
		}
		[TypeScriptBridge ("get_sourceLine")]
		public double Get_sourceLine ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_sourceLine"));
		}
		[TypeScriptBridge ("set_sourceLine")]
		public void Set_sourceLine (double @value)
		{
			CallMemberFunction ("set_sourceLine", @value);
		}
		[TypeScriptBridge ("get_sourceColumn")]
		public double Get_sourceColumn ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_sourceColumn"));
		}
		[TypeScriptBridge ("set_sourceColumn")]
		public void Set_sourceColumn (double @value)
		{
			CallMemberFunction ("set_sourceColumn", @value);
		}
		[TypeScriptBridge ("get_sourceLineText")]
		public string Get_sourceLineText ()
		{
			return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, CallMemberFunction ("get_sourceLineText"));
		}
		[TypeScriptBridge ("set_sourceLineText")]
		public void Set_sourceLineText (string @value)
		{
			CallMemberFunction ("set_sourceLineText", @value);
		}
		[TypeScriptBridge ("get_script")]
		public TypeScriptServiceBridge.V8Debugger.ScriptInfo Get_script ()
		{
			return TypeScriptObject.Create<TypeScriptServiceBridge.V8Debugger.ScriptInfo>  ((ObjectInstance) CallMemberFunction ("get_script"));
		}
		[TypeScriptBridge ("set_script")]
		public void Set_script (TypeScriptServiceBridge.V8Debugger.ScriptInfo @value)
		{
			CallMemberFunction ("set_script", @value);
		}
		[TypeScriptBridge ("get_breakpoints")]
		public TypeScriptArray<double> Get_breakpoints ()
		{
			return TypeConverter.ConvertTo<TypeScriptArray<double>> (JurassicTypeHosting.Engine, CallMemberFunction ("get_breakpoints"));
		}
		[TypeScriptBridge ("set_breakpoints")]
		public void Set_breakpoints (TypeScriptArray<double> @value)
		{
			CallMemberFunction ("set_breakpoints", @value);
		}
	}
	public class ScriptInfo : TypeScriptObject
			
	{
		public ScriptInfo (ObjectInstance instance) : base (instance) {}
		public ScriptInfo ()
			 : base (CallConstructor ("V8Debugger", "ScriptInfo"))
		{
		}
		[TypeScriptBridge ("name")]
		public string Name {
			get { return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, GetPropertyValue ("name")); }
			set { SetPropertyValue ("name", value); }
		}
		[TypeScriptBridge ("lineOffset")]
		public double LineOffset {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("lineOffset")); }
			set { SetPropertyValue ("lineOffset", value); }
		}
		[TypeScriptBridge ("columnOffset")]
		public double ColumnOffset {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("columnOffset")); }
			set { SetPropertyValue ("columnOffset", value); }
		}
		[TypeScriptBridge ("lineCount")]
		public double LineCount {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("lineCount")); }
			set { SetPropertyValue ("lineCount", value); }
		}
		[TypeScriptBridge ("get_name")]
		public string Get_name ()
		{
			return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, CallMemberFunction ("get_name"));
		}
		[TypeScriptBridge ("set_name")]
		public void Set_name (string @value)
		{
			CallMemberFunction ("set_name", @value);
		}
		[TypeScriptBridge ("get_lineOffset")]
		public double Get_lineOffset ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_lineOffset"));
		}
		[TypeScriptBridge ("set_lineOffset")]
		public void Set_lineOffset (double @value)
		{
			CallMemberFunction ("set_lineOffset", @value);
		}
		[TypeScriptBridge ("get_columnOffset")]
		public double Get_columnOffset ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_columnOffset"));
		}
		[TypeScriptBridge ("set_columnOffset")]
		public void Set_columnOffset (double @value)
		{
			CallMemberFunction ("set_columnOffset", @value);
		}
		[TypeScriptBridge ("get_lineCount")]
		public double Get_lineCount ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_lineCount"));
		}
		[TypeScriptBridge ("set_lineCount")]
		public void Set_lineCount (double @value)
		{
			CallMemberFunction ("set_lineCount", @value);
		}
	}
	public class ExceptionEventBody : TypeScriptObject
			
	{
		public ExceptionEventBody (ObjectInstance instance) : base (instance) {}
		public ExceptionEventBody ()
			 : base (CallConstructor ("V8Debugger", "ExceptionEventBody"))
		{
		}
		[TypeScriptBridge ("uncaught")]
		public bool Uncaught {
			get { return TypeConverter.ConvertTo<bool> (JurassicTypeHosting.Engine, GetPropertyValue ("uncaught")); }
			set { SetPropertyValue ("uncaught", value); }
		}
		[TypeScriptBridge ("exception")]
		public object Exception {
			get { return TypeConverter.ConvertTo<object> (JurassicTypeHosting.Engine, GetPropertyValue ("exception")); }
			set { SetPropertyValue ("exception", value); }
		}
		[TypeScriptBridge ("sourceLine")]
		public double SourceLine {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("sourceLine")); }
			set { SetPropertyValue ("sourceLine", value); }
		}
		[TypeScriptBridge ("sourceColumn")]
		public double SourceColumn {
			get { return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, GetPropertyValue ("sourceColumn")); }
			set { SetPropertyValue ("sourceColumn", value); }
		}
		[TypeScriptBridge ("sourceLineText")]
		public string SourceLineText {
			get { return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, GetPropertyValue ("sourceLineText")); }
			set { SetPropertyValue ("sourceLineText", value); }
		}
		[TypeScriptBridge ("script")]
		public TypeScriptServiceBridge.V8Debugger.ScriptInfo Script {
			get { return TypeScriptObject.Create<TypeScriptServiceBridge.V8Debugger.ScriptInfo> ((ObjectInstance) GetPropertyValue ("script")); }
			set { SetPropertyValue ("script", value != null ? value.Instance : null); }
		}
		[TypeScriptBridge ("get_uncaught")]
		public bool Get_uncaught ()
		{
			return TypeConverter.ConvertTo<bool> (JurassicTypeHosting.Engine, CallMemberFunction ("get_uncaught"));
		}
		[TypeScriptBridge ("set_uncaught")]
		public void Set_uncaught (bool @value)
		{
			CallMemberFunction ("set_uncaught", @value);
		}
		[TypeScriptBridge ("get_exception")]
		public object Get_exception ()
		{
			return TypeConverter.ConvertTo<object> (JurassicTypeHosting.Engine, CallMemberFunction ("get_exception"));
		}
		[TypeScriptBridge ("set_exception")]
		public void Set_exception (object @value)
		{
			CallMemberFunction ("set_exception", @value);
		}
		[TypeScriptBridge ("get_sourceLine")]
		public double Get_sourceLine ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_sourceLine"));
		}
		[TypeScriptBridge ("set_sourceLine")]
		public void Set_sourceLine (double @value)
		{
			CallMemberFunction ("set_sourceLine", @value);
		}
		[TypeScriptBridge ("get_sourceColumn")]
		public double Get_sourceColumn ()
		{
			return TypeConverter.ConvertTo<double> (JurassicTypeHosting.Engine, CallMemberFunction ("get_sourceColumn"));
		}
		[TypeScriptBridge ("set_sourceColumn")]
		public void Set_sourceColumn (double @value)
		{
			CallMemberFunction ("set_sourceColumn", @value);
		}
		[TypeScriptBridge ("get_sourceLineText")]
		public string Get_sourceLineText ()
		{
			return TypeConverter.ConvertTo<string> (JurassicTypeHosting.Engine, CallMemberFunction ("get_sourceLineText"));
		}
		[TypeScriptBridge ("set_sourceLineText")]
		public void Set_sourceLineText (string @value)
		{
			CallMemberFunction ("set_sourceLineText", @value);
		}
		[TypeScriptBridge ("get_script")]
		public TypeScriptServiceBridge.V8Debugger.ScriptInfo Get_script ()
		{
			return TypeScriptObject.Create<TypeScriptServiceBridge.V8Debugger.ScriptInfo>  ((ObjectInstance) CallMemberFunction ("get_script"));
		}
		[TypeScriptBridge ("set_script")]
		public void Set_script (TypeScriptServiceBridge.V8Debugger.ScriptInfo @value)
		{
			CallMemberFunction ("set_script", @value);
		}
	}
}
