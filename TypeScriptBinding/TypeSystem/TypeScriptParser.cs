#if ADVANCED_TYPE_SYSTEM_PARSER
using System;
using System.IO;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Documentation;
using ICSharpCode.NRefactory.Editor;
using TypeScriptServiceBridge;
using TypeScriptServiceBridge.Harness;
using TypeScriptServiceBridge.Services;
using MonoDevelop.TypeScriptBinding.NRefactory;

namespace MonoDevelop.TypeScriptBinding.NRefactory
{
	public class TypeScriptParser
	{
		internal static object parseLock = new object ();

		public TypeScriptParser ()
		{
		}

		TypeScriptServicesFactory factory;
		TypeScriptLS tsls;
		ILanguageService ls;

		// FIXME: make use of it if neccessary.
		public bool GenerateTypeSystemMode { get; set; }

		public TypeScriptSyntaxTree Parse (TextReader reader, string fileName)
		{
			if (factory == null)
				factory = new TypeScriptServicesFactory ();
			if (tsls == null)
				tsls = new TypeScriptLS ();
			if (ls == null)
				ls = factory.CreateLanguageService (new LanguageServiceShimHostAdapter (tsls));
			throw new NotImplementedException ();
		}
	}
}

#endif
