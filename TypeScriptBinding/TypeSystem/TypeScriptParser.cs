using System;
using System.IO;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Documentation;
using ICSharpCode.NRefactory.Editor;
using TypeScriptServiceBridge.TypeSystem;
using TypeScriptServiceBridge.Services;

namespace MonoDevelop.TypeScriptBinding
{
	public class TypeScriptParser
	{
		internal static object parseLock = new object ();

		public TypeScriptParser ()
		{
		}

		// FIXME: make use of it if neccessary.
		public bool GenerateTypeSystemMode { get; set; }

		public TypeScriptSyntaxTree Parse (TextReader reader, string fileName)
		{
			throw new NotImplementedException ();
		}
	}
}

