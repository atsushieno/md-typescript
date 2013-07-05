using System;
using System.Reflection;
using Jurassic.Library;

namespace TypeScriptServiceBridge.Hosting
{
	public abstract class LanguageServiceHost : IDisposable
	{
		static LanguageServiceHost host;

		static LanguageServiceHost Create ()
		{
			AppDomain.CurrentDomain.DomainUnload += delegate {
				if (host != null)
					host.Dispose (); 
			};
			var ca = typeof (object).Assembly.GetCustomAttributes (typeof (AssemblyCompanyAttribute), false) [0] as AssemblyCompanyAttribute;
			return ca.Company.IndexOf ("Mono", StringComparison.OrdinalIgnoreCase) >= 0 ?
				(LanguageServiceHost) new NodeLanguageServiceHost () :
				new JurassicLanguageServiceHost ();
		}

		public static LanguageServiceHost Instance {
			get {
				if (host == null) {
					host = Create ();
				}
				return host;
			}
			set { host = value; }
		}

		/*
		// FIXME: async!
		public static T Evaluate<T> (string command)
		{
			return host.Eval<T> (command);
		}
		*/

		public virtual void Dispose ()
		{
		}

		public abstract void Execute (string command);
		
		// FIXME: async!
		public abstract object Eval (string command);

		// FIXME: async!
		public abstract T Eval<T> (string command);

		public abstract ObjectInstance CallConstructor (string module, string className, params object [] args);

		public abstract object CallMemberFunction (ITypeScriptObject instance, string functionName, params object [] args);

		public abstract object GetPropertyValue (ITypeScriptObject instance, string propertyName);
		public abstract void SetPropertyValue (ITypeScriptObject instance, string propertyName, object value);
		
		public abstract object GetStaticPropertyValue (Type typeScriptObjectType, string propertyName);
		public abstract void SetStaticPropertyValue (Type typeScriptObjectType, string propertyName, object value);

		public abstract object GetArrayItem (ITypeScriptObject instance, int index);
		public abstract void SetArrayItem (ITypeScriptObject instance, int index, object value);

		public abstract void AddReference (ITypeScriptObject instance);

		public abstract void Release (ITypeScriptObject instance);

		public abstract ITypeScriptObject Cached (ITypeScriptObject instance);
	}
}
