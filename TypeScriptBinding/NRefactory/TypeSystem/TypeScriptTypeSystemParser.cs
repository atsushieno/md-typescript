/*
using System;
using System.IO;
using System.Linq;
using MonoDevelop.Ide.TypeSystem;
using MonoDevelop.Projects;
using MonoDevelop.TypeScriptBinding.Projects;
using Jurassic.Library;
using TypeScriptServiceBridge.Harness;

namespace MonoDevelop.TypeScriptBinding.NRefactory.TypeSystem
{
	public class TypeScriptTypeSystemParser : TypeSystemParser
	{
		public TypeScriptTypeSystemParser ()
		{
		}
		
		TypeScriptService GetService (Project project)
		{
			var tp = project as TypeScriptProject;
			return tp != null ? tp.TypeScriptService : null;
		}

		#region ITypeSystemParser implementation

		public override ParsedDocument Parse (bool storeAst, string fileName, TextReader content, Project project = null)
		{
			var service = GetService (project);
			if (service == null)
				return null;

			var file = service.GetFilePath (fileName);
			bool contains = service.LanguageService.Host.GetScriptFileNames ().Contains (file);
			if (!contains)
				service.ShimHost.AddScript (file, content.ReadToEnd ());

			var doc = new ParsedDocumentDecorator (new TypeScriptUnresolvedFile (service, file));

			return doc;
		}

		public ParsedDocument Parse (bool storeAst, string fileName, Project project = null)
		{
			using (var sr = new StreamReader (fileName))
				return Parse (storeAst, fileName, sr, project);
		}

		#endregion
	}
}
*/
