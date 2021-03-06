using System;
using System.IO;
using System.Xml;
using MonoDevelop.Projects;


namespace MonoDevelop.TypeScriptBinding.Projects
{

	public class TypeScriptProjectBinding : IProjectBinding
	{
		
		public string Name { get { return "TypeScript"; } }

		
		public bool CanCreateSingleFileProject (string sourceFile)
		{
			return sourceFile.EndsWith (".ts", StringComparison.OrdinalIgnoreCase);
		}
		
		
		public Project CreateProject (ProjectCreateInformation info, XmlElement projectOptions)
		{
			return new TypeScriptProject (info, projectOptions);
		}


		public Project CreateSingleFileProject (string sourceFile)
		{
			ProjectCreateInformation info = new ProjectCreateInformation ();
			info.ProjectName = Path.GetFileNameWithoutExtension (sourceFile);
			info.SolutionPath = Path.GetDirectoryName (sourceFile);
			info.ProjectBasePath = Path.GetDirectoryName (sourceFile);

			Project project = null;
			project = new TypeScriptProject (info, null);
			project.Files.Add (new ProjectFile (sourceFile));

			return project;
		}
		
	}
	
}