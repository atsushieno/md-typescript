using System;
using System.Reflection;

namespace TypeScriptServiceBridge
{
	public abstract class LanguageServiceHost
	{
		static readonly LanguageServiceHost host = Create ();

		public static LanguageServiceHost Create ()
		{
			/*
			var ca = typeof (object).Assembly.GetCustomAttributes (typeof (AssemblyCompanyAttribute), false) [0] as AssemblyCompanyAttribute;
			return ca.Company.IndexOf ("Mono", StringComparison.OrdinalIgnoreCase) >= 0 ?
				(LanguageServiceHost) new NodeLanguageServiceHost () :
				new JurassicLanguageServiceHost ();
			*/
			return new JurassicLanguageServiceHost ();
		}

		// FIXME: async!
		public static T Evaluate<T> (string command)
		{
			return host.Eval<T> (command);
		}

		// FIXME: async!
		public abstract T Eval<T> (string command);
	}
}
