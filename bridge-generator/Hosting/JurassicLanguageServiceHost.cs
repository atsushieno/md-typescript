using System;
using System.IO;
using System.Linq;
using Jurassic;
using Jurassic.Library;

namespace TypeScriptServiceBridge.Hosting
{
	public class JurassicLanguageServiceHost : LanguageServiceHost
	{
		const string TypeScriptService = "ls-bridge.js";

		ScriptEngine engine;

		public JurassicLanguageServiceHost ()
		{
			engine = JurassicTypeHosting.Engine;
			engine.Evaluate (new StreamReader (GetType ().Assembly.GetManifestResourceStream (TypeScriptService)).ReadToEnd ());
		}

		public override void Execute (string command)
		{
			engine.Execute (command);
		}

		public override object Eval (string command)
		{
			return engine.Evaluate (command);
		}

		public override T Eval<T> (string command)
		{
			return engine.Evaluate<T> (command);
		}

		static object [] MarshalArguments (object [] args)
		{
			return args.Select (o => MarshalArgument (o)).ToArray ();
		}

		static object MarshalArgument (object o)
		{
			var to = o as ITypeScriptObject;
			if (to != null)
				return to.Instance;
			return o;
		}

		FunctionInstance GetFunctionForClass (string module, string className)
		{
			var mod = (ObjectInstance) JurassicTypeHosting.Engine.GetGlobalValue (module);
			return (FunctionInstance) mod.GetPropertyValue (className);
		}

		public override ObjectInstance CallConstructor (string module, string className, params object[] args)
		{
			// jQuery-like create()->apply() construction instead of unsupported "new" operation.
			var cls = GetFunctionForClass (module, className);
			var inst = ObjectConstructor.Create (cls.Engine, cls.InstancePrototype);
			var ctor = (FunctionInstance) inst ["constructor"];
			ctor.Apply (inst, cls.Engine.Array.Construct (MarshalArguments (args)));
			return inst;
		}

		public override object CallMemberFunction (ITypeScriptObject instance, string functionName, params object [] args)
		{
			return ((ObjectInstance) instance.Instance).CallMemberFunction (functionName, MarshalArguments (args));
		}

		public override object GetPropertyValue (ITypeScriptObject instance, string propertyName)
		{
			return ((ObjectInstance) instance.Instance).GetPropertyValue (propertyName);
		}

		public override void SetPropertyValue (ITypeScriptObject instance, string propertyName, object value)
		{
			((ObjectInstance) instance.Instance).SetPropertyValue (propertyName, MarshalArgument (value), true);
		}

		public override object GetStaticPropertyValue (Type typeScriptObjectType, string propertyName)
		{
			var cls = GetFunctionForClass (typeScriptObjectType.Namespace, typeScriptObjectType.Name);
			return cls.GetPropertyValue (propertyName);
		}

		public override void SetStaticPropertyValue (Type typeScriptObjectType, string propertyName, object value)
		{
			var cls = GetFunctionForClass (typeScriptObjectType.Namespace, typeScriptObjectType.Name);
			cls.SetPropertyValue (propertyName, MarshalArgument (value), true);
		}

		public override object GetArrayItem (ITypeScriptObject instance, int index)
		{
			return ((ArrayInstance) instance.Instance) [index];
		}
		
		public override void SetArrayItem (ITypeScriptObject instance, int index, object value)
		{
			((ArrayInstance) instance.Instance) [index] = MarshalArgument (value);
		}

		public override void AddReference (ITypeScriptObject instance)
		{
		}

		public override void Release (ITypeScriptObject instance)
		{
		}

		public override ITypeScriptObject Cached (ITypeScriptObject instance)
		{
			return instance;
		}
	}
}

