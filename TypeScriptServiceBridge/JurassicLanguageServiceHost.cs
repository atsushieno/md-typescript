using System;
using System.IO;
using Jurassic;

namespace TypeScriptServiceBridge
{
	public class JurassicLanguageServiceHost : LanguageServiceHost
	{
		const string typeScriptService = "ls-bridge.js";
		ScriptEngine engine;

		public JurassicLanguageServiceHost ()
		{
			engine = new ScriptEngine ();
			engine.Evaluate (new StreamReader (GetType ().Assembly.GetManifestResourceStream (typeScriptService)).ReadToEnd ());
		}

		public override T Eval<T> (string command)
		{
			return engine.Evaluate<T> (command);
		}
	}
}

