using System;
using System.Collections.Generic;
using System.Linq;
using Jurassic;
using Jurassic.Library;
using TypeScriptServiceBridge.Hosting;

namespace TypeScriptServiceBridge
{
	public class TypeScriptObject : ITypeScriptObject, IDisposable
	{
		public static T Create<T> (ObjectInstance instance) where T : ITypeScriptObject
		{
			return instance == null ? default (T) : (T) Activator.CreateInstance (typeof (T), instance);
		}

		public static ObjectInstance CallConstructor (string module, string className, params object[] args)
		{
			return LanguageServiceHost.Instance.CallConstructor (module, className, args);
		}

		public object CallMemberFunction (string functionName, params object[] args)
		{
			return LanguageServiceHost.Instance.CallMemberFunction (this, functionName, args);
		}

		public object GetPropertyValue (string propertyName)
		{
			return LanguageServiceHost.Instance.GetPropertyValue (this, propertyName);
		}

		public void SetPropertyValue (string propertyName, object value)
		{
			LanguageServiceHost.Instance.SetPropertyValue (this, propertyName, value);
		}
		
		public static object GetStaticPropertyValue (Type type, string propertyName)
		{
			return LanguageServiceHost.Instance.GetStaticPropertyValue (type, propertyName);
		}
		
		public static void SetStaticPropertyValue (Type type, string propertyName, object value)
		{
			LanguageServiceHost.Instance.SetStaticPropertyValue (type, propertyName, value);
		}

		public static object ToNative (object wrapped)
		{
			var o = wrapped as TypeScriptObject;
			if (o != null)
				return o.Instance;
			return wrapped;
		}

		public static T ConvertTo<T> (object native)
		{
			if (native is T)
				return (T) native;
			if (typeof (ITypeScriptObject).IsAssignableFrom (typeof (T)))
				return (T) Activator.CreateInstance (typeof (T), native);
			return (T) native;
		}

		public ObjectInstance Instance { get; set; }

		public TypeScriptObject (ObjectInstance instance)
		{
			this.Instance = instance;
			LanguageServiceHost.Instance.AddReference (this);
		}

		public virtual void Dispose ()
		{
			LanguageServiceHost.Instance.Release (this);
		}

		public ITypeScriptObject CreateLocalCache ()
		{
			return LanguageServiceHost.Instance.Cached (this);
		}

		public override string ToString ()
		{
			return Instance != null ? Instance.ToString () : base.ToString ();
		}
	}
}

