using System;
using System.Collections.Generic;
using System.IO;
using Mono.Debugging.Client;
using Mono.Debugging.Backend;
using MonoDevelop.Core.Execution;
using MonoDevelop.Debugger;
using Mono.JavaScriptDebugger;
using TypeScriptServiceBridge;

namespace MonoDevelop.JavaScript.Debugger.Node
{
	public class NodeDebuggerSessionFactory : IDebuggerEngine
	{
		struct FileData {
			public DateTime LastCheck;
			public bool IsExe;
		}
		
		//Dictionary<string,FileData> fileCheckCache = new Dictionary<string, FileData> ();

		// FIXME: It is just a copy of GdbSessionFactory.
		// either create NodeExecutionCommand or check executable is "node".
		public bool CanDebugCommand (ExecutionCommand command)
		{
			return command is NodeExecutionCommand;
			/*
			return command is NativeExecutionCommand;
			if (cmd == null)
				return false;
			
			string file = FindFile (cmd.Command);
			if (!File.Exists (file)) {
				// The provided file is not guaranteed to exist. If it doesn't
				// we assume we can execute it because otherwise the run command
				// in the IDE will be disabled, and that's not good because that
				// command will build the project if the exec doesn't yet exist.
				return true;
			}
			
			file = Path.GetFullPath (file);
			DateTime currentTime = File.GetLastWriteTime (file);
			
			FileData data;
			if (fileCheckCache.TryGetValue (file, out data)) {
				if (data.LastCheck == currentTime)
					return data.IsExe;
			}
			data.LastCheck = currentTime;
			try {
				data.IsExe = IsExecutable (file);
			} catch {
				data.IsExe = false;
			}
			fileCheckCache [file] = data;
			return data.IsExe;
			*/
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

		/*
		public bool IsExecutable (string file)
		{
			// HACK: this is a quick but not very reliable way of checking if a file
			// is a native executable. Actually, we are interested in checking that
			// the file is not a script.
			using (StreamReader sr = new StreamReader (file)) {
				char[] chars = new char[3];
				int n = 0, nr = 0;
				while (n < chars.Length && (nr = sr.ReadBlock (chars, n, chars.Length - n)) != 0)
					n += nr;
				if (nr != chars.Length)
					return true;
				if (chars [0] == '#' && chars [1] == '!')
					return false;
			}
			return true;
		}
		*/
		
		public DebuggerSession CreateSession ()
		{
			NodeDebuggerSession ds = new NodeDebuggerSession ();
			return ds;
		}
		
		public ProcessInfo[] GetAttachableProcesses ()
		{
			List<ProcessInfo> procs = new List<ProcessInfo> ();
			foreach (string dir in Directory.GetDirectories ("/proc")) {
				int id;
				if (!int.TryParse (Path.GetFileName (dir), out id))
					continue;
				try {
					File.ReadAllText (Path.Combine (dir, "sessionid"));
				} catch {
					continue;
				}
				string cmdline = File.ReadAllText (Path.Combine (dir, "cmdline"));
				cmdline = cmdline.Replace ('\0',' ');
				var elems = cmdline.Split (' ');
				var idx = Array.IndexOf (elems, "debug");
				if (idx < 1 || elems [idx - 1].Contains ("node"))
					continue;
				ProcessInfo pi = new ProcessInfo (id, cmdline);
				procs.Add (pi);
			}
			return procs.ToArray ();
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
