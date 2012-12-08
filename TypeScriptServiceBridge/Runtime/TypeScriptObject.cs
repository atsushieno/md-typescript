using System;
using System.Collections.Generic;
using Jurassic.Library;
using TypeScriptServiceBridge.Hosting;

namespace TypeScriptServiceBridge
{
	public abstract class TypeScriptObject : ITypeScriptObject, IDisposable
	{
		static ObjectInstance root = LanguageServiceHost.Evaluate<ObjectInstance> ("var MonoDevelopTypeScriptObject = function () {};");

		public static ObjectInstance CallConstructor (string module, string className, params object[] args)
		{
			var mod = (ObjectInstance) root.Engine.CallGlobalFunction (module);
			return (ObjectInstance) mod.CallMemberFunction (className, args);
		}

		public static object ToNative (object wrapped)
		{
			var o = wrapped as TypeScriptObject;
			if (o != null)
				return o.Instance;
			return wrapped;
		}

		public ObjectInstance Instance { get; private set; }

		public TypeScriptObject (ObjectInstance instance)
		{
			this.Instance = instance;
		}

		public virtual void Dispose ()
		{
		}
	}
}

