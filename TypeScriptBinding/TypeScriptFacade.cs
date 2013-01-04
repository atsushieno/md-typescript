using System;
using System.IO;
using MonoDevelop.Core;
using MonoDevelop.Projects;
using TypeScriptServiceBridge.Harness;
using TypeScriptServiceBridge.Services;

namespace MonoDevelop.TypeScriptBinding
{
	public class TypeScriptFacade
	{
		public TypeScriptFacade ()
		{
			ShimHost = new TypeScriptLS ();
			LanguageService = new TypeScriptServicesFactory ()
				.CreateLanguageService (new LanguageServiceShimHostAdapter (ShimHost));
		}

		public TypeScriptLS ShimHost { get; private set; }
		public ILanguageService LanguageService { get; private set; }
		
		public string GetFilePath (ProjectFile file)
		{
			return GetFilePath (file.FilePath);
		}

		public string GetFilePath (FilePath path)
		{
			return path.CanonicalPath.FullPath;
		}

		public string GetFilePath (string file)
		{
			return Path.GetFullPath (file);
		}
	}
}

