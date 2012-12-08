using System;
using System.Collections.Generic;
using TypeScriptServiceBridge.Hosting;

namespace TypeScriptServiceBridge
{
	public abstract class TypeScriptObject : IDisposable
	{
		static int object_created;

		public TypeScriptObject ()
		{
		}

		public abstract void Dispose ();

		public T Eval<T> (string script)
		{
			return LanguageServiceHost.Instance.Eval<T> (script);
		}

		public string AllocateVariable (object obj)
		{
			string name = "__tsbridge_v" + object_created++;
			LanguageServiceHost.Instance.SetGlobalVariable (name, obj);
			return name;
		}

		public void ReleaseVariable (string label)
		{
			LanguageServiceHost.Instance.SetGlobalVariable (label, null);
		}
	}
}

