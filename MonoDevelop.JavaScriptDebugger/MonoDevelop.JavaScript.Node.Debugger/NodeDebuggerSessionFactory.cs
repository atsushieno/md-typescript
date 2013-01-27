using System;
using System.Collections.Generic;
using System.IO;
using Mono.Debugging.Client;
using Mono.Debugging.Backend;
using MonoDevelop.Core;
using MonoDevelop.Core.Execution;
using MonoDevelop.Debugger;
using Mono.JavaScript.Debugger;
using Mono.JavaScript.Node.Debugger;
using MonoDevelop.JavaScript.Node;
using TypeScriptServiceBridge;

namespace MonoDevelop.JavaScript.Node.Debugger
{
	public class NodeDebuggerSessionFactory : IDebuggerEngine
	{
		public bool CanDebugCommand (ExecutionCommand command)
		{
			return command is NodeExecutionCommand;
		}

		class NodeDebuggerStartInfo : DebuggerStartInfo
		{
			public SourceMap SourceMap { get; set; }
		}
		
		public DebuggerStartInfo CreateDebuggerStartInfo (ExecutionCommand command)
		{
			NodeExecutionCommand pec = (NodeExecutionCommand) command;
			pec.Debug = true;
			NodeDebuggerStartInfo startInfo = new NodeDebuggerStartInfo ();
			startInfo.Command = pec.Command;
			startInfo.Arguments = pec.Arguments;
			startInfo.WorkingDirectory = pec.WorkingDirectory;
			if (pec.EnvironmentVariables.Count > 0) {
				foreach (KeyValuePair<string,string> val in pec.EnvironmentVariables)
					startInfo.EnvironmentVariables [val.Key] = val.Value;
			}

			// FIXME: use sourcemap.
			// FIXME: there is some issue in SourceMap parser or TypeScript map output that results in ArgumentOutOfRangeException...
			startInfo.SourceMap = new SourceMap (JurassicTypeHosting.Engine);
			startInfo.SourceMap.Read (File.ReadAllText (Path.Combine (pec.WorkingDirectory, Path.GetFileName (pec.ScriptPath + ".map"))));

			return startInfo;
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

		// Copy from MonoDevelop.TypeScriptBinding.Projects.TypeScriptProject.
		string GetNodePath ()
		{
			string exe = PropertyService.Get<string> ("TypeScriptBinding.NodeLocation");
			return string.IsNullOrEmpty (exe) ? FindToolPath ("node") : exe;
		}

		public DebuggerSession CreateSession ()
		{
			NodeDebuggerSession ds = new NodeDebuggerSession (GetNodePath ());
			/*
			ds.Disposing += delegate {
				if (console != null && !console.IsCompleted) {
					console.Cancel ();
					console = null;
				}
			};
			*/
			
			return ds;
		}
		
		public ProcessInfo[] GetAttachableProcesses ()
		{
			// we don't support it.
			return new ProcessInfo [0];
		}
		
		string FindFile (string cmd)
		{
			if (Path.IsPathRooted (cmd))
				return cmd;
			string pathVar = Environment.GetEnvironmentVariable ("PATH");
			string[] paths = pathVar.Split (Path.PathSeparator);
			foreach (string path in paths) {
				string file = Path.Combine (path, cmd);
				if (File.Exists (file))
					return file;
			}
			return cmd;
		}
	}
}
