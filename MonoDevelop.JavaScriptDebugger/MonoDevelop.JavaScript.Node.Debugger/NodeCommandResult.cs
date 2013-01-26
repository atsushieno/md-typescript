using System;

namespace MonoDevelop.JavaScript.Node.Debugger
{
	class NodeCommandResult: ResultData
	{
		public CommandStatus Status;
		public string ErrorMessage;
		
		public NodeCommandResult (string line)
		{
			if (line.StartsWith ("^done")) {
				Status = CommandStatus.Done;
				ReadResults (line, 6);
			} else if (line.StartsWith ("^error")) {
				Status = CommandStatus.Error;
				if (line.Length > 7) {
					ReadResults (line, 7);
					ErrorMessage = GetValue ("msg");
				}
			} else if (line.StartsWith ("^running")) {
				Status = CommandStatus.Running;
			}
		}
	}
}
