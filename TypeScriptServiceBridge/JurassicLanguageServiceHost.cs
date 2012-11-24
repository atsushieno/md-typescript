using System;
using System.IO;
using Jurassic;

namespace TypeScriptServiceBridge
{
	public class JurassicLanguageServiceHost : LanguageServiceHost
	{
		const string typeScriptService = "ts-all.js";
		ScriptEngine engine;

		public JurassicLanguageServiceHost ()
		{
			engine = new ScriptEngine ();
			engine.Evaluate (File.ReadAllText (typeScriptService));
		}

		public override T Eval<T> (string command)
		{
			return engine.Evaluate<T> (command);
		}
	}
}

