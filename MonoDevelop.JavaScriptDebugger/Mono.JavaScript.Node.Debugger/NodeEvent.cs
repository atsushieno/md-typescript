using System;

namespace Mono.JavaScript.Node.Debugger
{
	class NodeEvent: ResultData
	{
		public string Name;
		public string Reason;
		
		public NodeEvent (string line)
		{
			int i = line.IndexOf (',');
			if (i == -1)
				i = line.Length;
			Name = line.Substring (1, i - 1);
			ReadResults (line, i+1);
			object[] reasons = GetAllValues ("reason");
			if (reasons.Length > 0)
				Reason = (string) reasons [0]; 
		}
	}
}
