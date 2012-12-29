using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using MonoDevelop.Core;
using MonoDevelop.Core.Execution;
using MonoDevelop.Core.ProgressMonitoring;
using MonoDevelop.Projects;
using MonoDevelop.TypeScriptBinding.Projects;
using TypeScriptServiceBridge.TypeScript;

namespace MonoDevelop.TypeScriptBinding.Tools
{
	public class TypeScriptBuilder
	{
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

		public static BuildResult Compile (TypeScriptProject project, TypeScriptProjectConfiguration configuration, IProgressMonitor monitor)
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
		
		public static void Execute (TypeScriptProject project, TypeScriptProjectConfiguration configuration, IProgressMonitor monitor, ExecutionContext context)
		{
			string exe = PropertyService.Get<string> ("TypeScriptBinding.NodeLocation");
			exe = exe ?? FindToolPath ("node");

			NativeExecutionCommand cmd = new NativeExecutionCommand (exe);
			cmd.Arguments = project.TargetJavaScriptFile + " " + configuration.CommandLineParameters;
			cmd.WorkingDirectory = Path.GetDirectoryName (project.GetTargetJavascriptFilePath ());
			IConsole console;
			if (configuration.ExternalConsole)
				console = context.ExternalConsoleFactory.CreateConsole (false);
			else
				console = context.ConsoleFactory.CreateConsole (false);
			
			AggregatedOperationMonitor operationMonitor = new AggregatedOperationMonitor (monitor);
			
			try {
				if (!context.ExecutionHandler.CanExecute (cmd)) {
					monitor.ReportError (String.Format ("Cannot execute '{0}'.", cmd.CommandString), null);
					return;
				}
				
				IProcessAsyncOperation operation = context.ExecutionHandler.Execute (cmd, console);
				
				operationMonitor.AddOperation (operation);
				operation.WaitForCompleted ();
				
				monitor.Log.WriteLine ("Player exited with code {0}.", operation.ExitCode);
			} catch (Exception) {
				monitor.ReportError (String.Format ("Error while executing '{0}'.", cmd.CommandString), null);
			} finally {
				operationMonitor.Dispose ();
				console.Dispose ();
			}
		}
	}
}

