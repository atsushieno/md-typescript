using System;
using System.IO;
using MonoDevelop.Core.Execution;

namespace Mono.JavaScriptDebugger
{
	public class NodeExecutionCommand : NativeExecutionCommand
	{
		public NodeExecutionCommand (string nodePath, string scriptPath)
			: base (nodePath)
		{
			ScriptPath = scriptPath;
		}

		bool debug;
		public bool Debug {
			get { return debug; }
			set {
				debug = value;
				UpdateArguments ();
			}
		}

		public string ScriptPath { get; private set; }

		string additional_arguments;

		public string AdditionalArguments {
			get { return additional_arguments; }
			set {
				additional_arguments = value;
				UpdateArguments ();
			}
		}

		void UpdateArguments ()
		{
			Arguments = Debug ? "debug " : "";
			Arguments += ScriptPath + " " + AdditionalArguments;
		}
	}
}

