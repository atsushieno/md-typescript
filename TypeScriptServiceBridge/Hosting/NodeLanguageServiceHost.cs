using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Jurassic;

namespace TypeScriptServiceBridge.Hosting
{

	// Since mono is not capable of processing Jurassic in full CLR mode https://bugzilla.xamarin.com/show_bug.cgi?id=7829
	// we practically need non-Jurassic host. It is not ideal though.
	public class NodeLanguageServiceHost : LanguageServiceHost
	{
		internal const string NodeServerAppFullPath = "mdTypeScriptBridge.js";

		int port;
		WebClient client;
		Process node;
		ScriptEngine engine;

		static int FindAvailablePort ()
		{
			var rnd = new Random ();
			do {
				int port = rnd.Next (1024, 65535);
				TcpListener l = null;
				try {
					l = new TcpListener (port);
					l.Start ();
					return port;
				} catch (SocketException) {
				} finally {
					if (l != null)
						l.Stop ();
				}
			} while (true);
		}

		public NodeLanguageServiceHost ()
		{
			this.port = FindAvailablePort ();
			client = new WebClient ();
			node = Process.Start ("node", NodeServerAppFullPath);
			engine = new ScriptEngine ();
		}

		public override T Eval<T> (string command)
		{
			var result = EvalNode (command);
			return Jurassic.TypeConverter.ConvertTo<T> (engine, result);
		}

		string EvalNode (string command)
		{
			var c = new NameValueCollection ();
			c ["command"] = "eval";
			c ["expr"] = command;
			return Encoding.UTF8.GetString (client.UploadValues ("http://localhost:" + port, "POST", c));
		}

		// never known to work.
		public override void SetGlobalVariable (string label, object value)
		{
			var c = new NameValueCollection ();
			c ["command"] = "set_global_variable";
			c ["assign.assignee"] = label;
			c ["assign.target"] = value.ToString ();
			client.UploadValues ("http://localhost:" + port, "POST", c);
		}
	}
}

