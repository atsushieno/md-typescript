using System;
using System.IO;
using MonoDevelop.Ide.TypeSystem;
using MonoDevelop.Projects;
using MonoDevelop.TypeScriptBinding.Projects;

namespace MonoDevelop.TypeScriptBinding.NRefactory.TypeSystem
{
	public class TypeScriptTypeSystemParser : ITypeSystemParser
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

		public ParsedDocument Parse (bool storeAst, string fileName, TextReader content, Project project = null)
		{
			var service = GetService (project);
			if (service == null)
				return null;

			var file = service.GetFilePath (fileName);
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

