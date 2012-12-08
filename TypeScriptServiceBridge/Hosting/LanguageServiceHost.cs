using System;
using System.Reflection;

namespace TypeScriptServiceBridge.Hosting
{
	public abstract class LanguageServiceHost
	{
		static readonly LanguageServiceHost host = Create ();

		static LanguageServiceHost Create ()
		{
			/*
			var ca = typeof (object).Assembly.GetCustomAttributes (typeof (AssemblyCompanyAttribute), false) [0] as AssemblyCompanyAttribute;
			return ca.Company.IndexOf ("Mono", StringComparison.OrdinalIgnoreCase) >= 0 ?
				(LanguageServiceHost) new NodeLanguageServiceHost () :
				new JurassicLanguageServiceHost ();
			*/
			return new JurassicLanguageServiceHost ();
		}

		public static LanguageServiceHost Instance {
			get { return host; }
		}

		// FIXME: async!
		public static T Evaluate<T> (string command)
		{
			return host.Eval<T> (command);
		}

		// FIXME: async!
		public abstract T Eval<T> (string command);

		// FIXME: async!
		public abstract void SetGlobalVariable (string label, object obj);
	}
}
