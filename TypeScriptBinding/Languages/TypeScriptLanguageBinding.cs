using System;
using System.IO;
using MonoDevelop.Core;
using MonoDevelop.Projects;

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
	}
	
}
