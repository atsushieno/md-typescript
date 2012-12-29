using System;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.CodeDom.Compiler;
using System.Diagnostics;
using MonoDevelop.Core;
using MonoDevelop.Core.Execution;
using MonoDevelop.Core.ProgressMonitoring;
using MonoDevelop.Core.Serialization;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Projects;
using MonoDevelop.TypeScriptBinding.Tools;


namespace MonoDevelop.TypeScriptBinding.Projects
{

	[DataInclude (typeof (TypeScriptProjectConfiguration))]
    public class TypeScriptProject : Project
	{
		
		[ItemProperty("AdditionalArguments", DefaultValue="")]
		string mAdditionalArguments = string.Empty;
		
		public string AdditionalArguments {
			get { return mAdditionalArguments;  }
			set { mAdditionalArguments = value; }
		}
		
		[ItemProperty("TargetJavaScriptFile", DefaultValue="")]
		string mTargetJavaScriptFile = string.Empty;
		
		public string TargetJavaScriptFile {
			get { return mTargetJavaScriptFile;  }
			set { mTargetJavaScriptFile = value; }
		}

		public string GetTargetJavascriptFilePath ()
		{
			return Path.Combine (BaseDirectory, TargetJavaScriptFile);
		}

		public TypeScriptProject ()
		{
		}

		public TypeScriptProject (ProjectCreateInformation info, XmlElement projectOptions) : base()
		{
			if (projectOptions.Attributes ["TargetJavaScriptFile"] != null)
			{
				
				TargetJavaScriptFile = GetOptionAttribute (info, projectOptions, "TargetJavaScriptFile");
				
			}
			
			if (projectOptions.Attributes ["AdditionalArguments"] != null)
			{
				
				AdditionalArguments = GetOptionAttribute (info, projectOptions, "AdditionalArguments");
				
			}
			
			TypeScriptProjectConfiguration configuration;
			
			configuration = (TypeScriptProjectConfiguration)CreateConfiguration ("Debug");
			configuration.DebugMode = true;
			//configuration.Platform = target;
			Configurations.Add (configuration);
			
			configuration = (TypeScriptProjectConfiguration)CreateConfiguration ("Release");
			configuration.DebugMode = false;
			//configuration.Platform = target;
			Configurations.Add (configuration);
		}
		
		
		public override SolutionItemConfiguration CreateConfiguration (string name)
		{
			TypeScriptProjectConfiguration conf = new TypeScriptProjectConfiguration ();
			conf.Name = name;
			return conf;
		}

		protected override BuildResult DoBuild (IProgressMonitor monitor, ConfigurationSelector configurationSelector)
		{
			TypeScriptProjectConfiguration TypeScriptConfig = (TypeScriptProjectConfiguration)GetConfiguration (configurationSelector);
			return TypeScriptBuilder.Compile (this, TypeScriptConfig, monitor);
		}

		protected override void DoClean (IProgressMonitor monitor, ConfigurationSelector configuration)
		{
			base.DoClean (monitor, configuration);
		}		
		
		protected override void DoExecute (IProgressMonitor monitor, ExecutionContext context, ConfigurationSelector configurationSelector)
		{
			TypeScriptProjectConfiguration TypeScriptConfig = (TypeScriptProjectConfiguration)GetConfiguration (configurationSelector);
			TypeScriptBuilder.Execute (this, TypeScriptConfig, monitor, context);
		}

		protected string GetOptionAttribute (ProjectCreateInformation info, XmlElement projectOptions, string attributeName)
		{
			string value = projectOptions.Attributes [attributeName].InnerText;
			value = value.Replace ("${ProjectName}", info.ProjectName);
			return value;
		}

		public override bool IsCompileable (string fileName)
		{
			return !string.IsNullOrEmpty (this.TargetJavaScriptFile);
		}

		protected override bool OnGetCanExecute (ExecutionContext context, ConfigurationSelector configurationSelector)
		{
			return !string.IsNullOrEmpty (this.TargetJavaScriptFile);
		}

		public override string ProjectType {
			get { return "TypeScript"; }
		}

		public override string[] SupportedLanguages {
			get { return new string[] { "", "TypeScript" }; }
		}
		
	}
	
}