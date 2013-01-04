using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Projects;
using TypeScriptServiceBridge.Harness;
using TypeScriptServiceBridge.Services;

namespace MonoDevelop.TypeScriptBinding
{
	public class TypeScriptFacade
	{
		Project project;

		public TypeScriptFacade (Project project)
		{
			this.project = project;
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
		
		DateTimeOffset last_script_updated_time = DateTimeOffset.MinValue;
		Dictionary<string,DateTimeOffset> last_updated_time_for_docs = new Dictionary<string, DateTimeOffset> ();
		
		public void UpdateScripts ()
		{
			foreach (var doc in IdeApp.Workbench.Documents.Where (d => d.Project == project && d.IsCompileableInProject)) {
				DateTimeOffset last;
				var path = GetFilePath (doc.FileName);
				if (!last_updated_time_for_docs.TryGetValue (path, out last) || last > last_script_updated_time) {
					last_updated_time_for_docs [path] = DateTimeOffset.UtcNow;
					ShimHost.UpdateScript (path, doc.Editor.Text);
				}
			}
			last_script_updated_time = DateTimeOffset.UtcNow;
		}

		public void UpdateLastEditTime (FilePath file)
		{
			last_updated_time_for_docs [GetFilePath (file)] = DateTimeOffset.UtcNow;
		}
	}
}

