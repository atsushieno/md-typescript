using System;
using System.IO;
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

		public override Jurassic.Library.ObjectInstance CallConstructor (string module, string className, params object[] args)
		{
			// jQuery-like create()->apply() construction instead of unsupported "new" operation.
			var mod = (ObjectInstance) JurassicTypeHosting.Engine.GetGlobalValue (module);
			var cls = (FunctionInstance) mod.GetPropertyValue (className);
			var inst = ObjectConstructor.Create (cls.Engine, cls.InstancePrototype);
			var ctor = (FunctionInstance) inst ["constructor"];
			ctor.Apply (inst, cls.Engine.Array.Construct (args));
			return inst;
		}

		public override object CallMemberFunction (ITypeScriptObject instance, string functionName, params object [] args)
		{
			return instance.Instance.CallMemberFunction (functionName, args);
		}

		public override object GetPropertyValue (ITypeScriptObject instance, string propertyName)
		{
			return instance.Instance.GetPropertyValue (propertyName);
		}

		public override void SetPropertyValue (ITypeScriptObject instance, string propertyName, object value)
		{
			instance.Instance.SetPropertyValue (propertyName, value, true);
		}

		public override object GetArrayItem (ITypeScriptObject instance, int index)
		{
			return ((ArrayInstance) instance.Instance) [index];
		}
		
		public override void SetArrayItem (ITypeScriptObject instance, int index, object value)
		{
			((ArrayInstance) instance.Instance) [index] = value;
		}

		public override void AddReference (ITypeScriptObject instance)
		{
		}

		public override void Release (ITypeScriptObject instance)
		{
		}
	}
}

