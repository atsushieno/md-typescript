using System;
using Jurassic;

namespace TypeScriptServiceBridge
{
	public static class JurassicTypeHosting
	{
		static ScriptEngine engine = new Jurassic.ScriptEngine ();

		public static ScriptEngine Engine {
			get { return engine; }
		}
	}
}

