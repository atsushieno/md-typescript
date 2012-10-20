using System;
using System.IO;
using MonoDevelop.Core;
using MonoDevelop.Projects;
//using MonoDevelop.Projects.CodeGeneration;
//using MonoDevelop.Projects.Dom;
//using MonoDevelop.Projects.Dom.Parser;


namespace MonoDevelop.TypeScriptBinding.Languages
{

	public class TypeScriptLanguageBinding : ILanguageBinding
	{

		public string SingleLineCommentTag { get { return "//"; } }
		public string BlockCommentStartTag { get { return "/*"; } }
		public string BlockCommentEndTag { get { return "*/"; } }

		public string Language { get { return "TypeScript"; } }

		
		public FilePath GetFileName (FilePath baseName)
		{
			return new FilePath (baseName.FileNameWithoutExtension + ".ts");
		}
		
		
		public bool IsSourceCodeFile (FilePath fileName)
		{
			return fileName.Extension == "ts";
		}
		
		
		/*public IParser Parser {
			get { return null; }
		}
		
		
		public IRefactorer Refactorer {
			get { return null; }
		}*/
		
	}
	
}
