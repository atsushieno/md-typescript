using System;
using System.IO;
using System.Linq;
using System.Text;
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
using TypeScriptServiceBridge.Harness;
using TypeScriptServiceBridge.Services;

namespace MonoDevelop.TypeScriptBinding.Projects
{

	[DataInclude (typeof (TypeScriptProjectConfiguration))]
    public class TypeScriptProject : Project
	{
		TypeScriptLS shim_host;
		ILanguageService language_service;

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
			shim_host = new TypeScriptLS ();
			language_service = new TypeScriptServicesFactory ()
				.CreateLanguageService (new LanguageServiceShimHostAdapter (shim_host));
		}

		public TypeScriptProject (ProjectCreateInformation info, XmlElement projectOptions)
			: this ()
		{
			if (projectOptions.Attributes ["TargetJavaScriptFile"] != null)
				TargetJavaScriptFile = GetOptionAttribute (info, projectOptions, "TargetJavaScriptFile");

			if (projectOptions.Attributes ["AdditionalArguments"] != null)
				AdditionalArguments = GetOptionAttribute (info, projectOptions, "AdditionalArguments");

			TypeScriptProjectConfiguration configuration;
			
			configuration = (TypeScriptProjectConfiguration) CreateConfiguration ("Debug");
			configuration.DebugMode = true;
			Configurations.Add (configuration);
			
			configuration = (TypeScriptProjectConfiguration) CreateConfiguration ("Release");
			configuration.DebugMode = false;
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
			return CompileWithTsc (this, TypeScriptConfig, monitor);
		}

		protected override void DoClean (IProgressMonitor monitor, ConfigurationSelector configuration)
		{
			base.DoClean (monitor, configuration);
		}		
		
		protected override void DoExecute (IProgressMonitor monitor, ExecutionContext context, ConfigurationSelector configurationSelector)
		{
			TypeScriptProjectConfiguration TypeScriptConfig = (TypeScriptProjectConfiguration)GetConfiguration (configurationSelector);
			ExecuteWithNode (this, TypeScriptConfig, monitor, context);
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
			TypeScriptProjectConfiguration conf = (TypeScriptProjectConfiguration) GetConfiguration (configurationSelector);
			ExecutionCommand cmd = CreateExecutionCommand (conf);
			return !string.IsNullOrEmpty (this.TargetJavaScriptFile) && context.ExecutionHandler.CanExecute (cmd);
		}

		public override string ProjectType {
			get { return "TypeScript"; }
		}

		public override string[] SupportedLanguages {
			get { return new string[] { "", "TypeScript" }; }
		}


		static string FindToolPath (string tool)
		{
			var paths = Environment.GetEnvironmentVariable ("PATH").Split (Path.PathSeparator);
			foreach (var path in paths) {
				var p = Path.Combine (path, tool);
				if (File.Exists (p))
					return p;
			}
			return null;
		}
		
		public static BuildResult CompileWithTsc (TypeScriptProject project, TypeScriptProjectConfiguration configuration, IProgressMonitor monitor)
		{
			string exe = PropertyService.Get<string> ("TypeScriptBinding.TscLocation");
			exe = exe ?? FindToolPath ("tsc");
			
			var outfile = project.GetTargetJavascriptFilePath ();
			var outdir = Path.GetDirectoryName (outfile);
			if (!Directory.Exists (outdir))
				Directory.CreateDirectory (outdir);
			
			var argList = new List<string> ();
			argList.Add ("--out");
			argList.Add (outfile.FirstOrDefault () == '"' ? '"' + outfile + '"' : outfile);
			
			if (configuration.DebugMode)
				argList.Add ("-c");
			
			if (project.AdditionalArguments != "")
				argList.Add (project.AdditionalArguments);
			
			if (configuration.AdditionalArguments != "")
				argList.Add (configuration.AdditionalArguments);
			
			foreach (var fp in project.Files.Where (pf => pf.BuildAction == "Compile"))
				argList.Add (fp.FilePath.FullPath);
			
			var args = String.Join (" ", argList.ToArray ());
			
			var stdOutErr = new StringWriter ();
			int exitCode = RunTool (exe, args, project.BaseDirectory, monitor, stdOutErr);
			
			BuildResult result = ParseOutput (project, stdOutErr.ToString ());
			if (result.CompilerOutput.Trim ().Length != 0)
				monitor.Log.WriteLine (result.CompilerOutput);
			
			if (result.ErrorCount == 0 && exitCode != 0)
			{
				string errorMessage = stdOutErr.ToString ();
				if (!string.IsNullOrEmpty (errorMessage))
					result.AddError (errorMessage); 
				else
					result.AddError ("Build failed. Go to \"Build Output\" for more information");
			}
			
			return result;
		}
		
		private static int RunTool (string cmd, string args, string workingDirectory, IProgressMonitor monitor, TextWriter err)
		{
			int exitcode = 0;
			ProcessStartInfo pinfo = new ProcessStartInfo (cmd, args);
			pinfo.UseShellExecute = false;
			pinfo.RedirectStandardOutput = true;
			pinfo.RedirectStandardError = true;
			pinfo.WorkingDirectory = workingDirectory;
			
			using (ProcessWrapper pw = Runtime.ProcessService.StartProcess (pinfo, monitor.Log, err, null)) {
				pw.WaitForOutput ();
				exitcode = pw.ExitCode;
			}
			
			return exitcode;
		}
		
		static BuildResult ParseOutput (TypeScriptProject project, string stdOutAndErr)
		{
			BuildResult result = new BuildResult ();
			
			StringBuilder output = new StringBuilder ();
			foreach (var line in stdOutAndErr.Split ('\n').Select (l => l.Trim ())) {
				output.AppendLine (line);
				
				if (line.Length == 0 || line.StartsWith ("\t"))
					continue;
				
				// FIXME: implement
				BuildError error = new BuildError () { ErrorText = line };
				if (error != null)
					result.Append (error);
			}
			
			result.CompilerOutput = output.ToString ();
			
			return result;
		}		

		ExecutionCommand CreateExecutionCommand (TypeScriptProjectConfiguration conf)
		{
			NativeExecutionCommand cmd = new NativeExecutionCommand (GetNodePath ());
			cmd.Arguments = TargetJavaScriptFile + " " + conf.CommandLineParameters;
			cmd.WorkingDirectory = Path.GetDirectoryName (GetTargetJavascriptFilePath ());
			return cmd;
		}

		string GetNodePath ()
		{
			string exe = PropertyService.Get<string> ("TypeScriptBinding.NodeLocation");
			return exe ?? FindToolPath ("node");
		}
		
		public void ExecuteWithNode (TypeScriptProject project, TypeScriptProjectConfiguration conf, IProgressMonitor monitor, ExecutionContext context)
		{
			var exe = GetNodePath ();

			bool pause = conf.PauseConsoleOutput;
			IConsole console;
			
			monitor.Log.WriteLine ("Running project...");
			
			if (conf.ExternalConsole)
				console = context.ExternalConsoleFactory.CreateConsole (!pause);
			else
				console = context.ConsoleFactory.CreateConsole (!pause);
			
			AggregatedOperationMonitor operationMonitor = new AggregatedOperationMonitor (monitor);
			
			try {
				var cmd = CreateExecutionCommand (conf);

				if (!context.ExecutionHandler.CanExecute (cmd)) {
					monitor.ReportError ("Cannot execute \"" + exe + "\". The selected execution mode is not supported for TypeScript projects.", null);
					return;
				}
				
				IProcessAsyncOperation op = context.ExecutionHandler.Execute (cmd, console);
				
				operationMonitor.AddOperation (op);
				op.WaitForCompleted ();
				
				monitor.Log.WriteLine ("The operation exited with code: {0}", op.ExitCode);
			} catch (Exception ex) {
				monitor.ReportError ("Cannot execute \"" + exe + "\"", ex);
			} finally {			
				operationMonitor.Dispose ();			
				console.Dispose ();
			}
		}

		#region file change events

		protected override void OnFileRemovedFromProject (ProjectFileEventArgs e)
		{
			base.OnFileRemovedFromProject (e);
			// how to remove files???
			foreach (var item in e)
				shim_host.UpdateScript (item.ProjectFile.FilePath.CanonicalPath, null);
		}
		
		protected override void OnFileAddedToProject (ProjectFileEventArgs e)
		{
			base.OnFileAddedToProject (e);
			// FIXME: make sure that adding, removing and then adding the same file still works (as "remove" does not really remove it).
			foreach (var item in e)
				if (item.ProjectFile.BuildAction == BuildAction.Compile)
					shim_host.AddScript (item.ProjectFile.FilePath.FullPath, File.ReadAllText (item.ProjectFile.FilePath.CanonicalPath));
		}
		
		protected override void OnFileRenamedInProject (ProjectFileRenamedEventArgs e)
		{
			base.OnFileRenamedInProject (e);
			foreach (ProjectFileRenamedEventInfo item in e)
				if (item.ProjectFile.BuildAction == BuildAction.Compile)
					shim_host.UpdateScript (item.OldName.CanonicalPath, null);
			// FIXME: make sure that adding, removing and then adding the same file still works (as "remove" does not really remove it).
			foreach (ProjectFileRenamedEventInfo item in e)
				if (item.ProjectFile.BuildAction == BuildAction.Compile)
					shim_host.AddScript (item.NewName.FullPath, File.ReadAllText (item.NewName.CanonicalPath));
		}

		#endregion
	}
}