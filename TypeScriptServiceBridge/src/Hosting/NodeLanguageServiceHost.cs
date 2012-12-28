using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Jurassic;
using Jurassic.Library;

namespace TypeScriptServiceBridge.Hosting
{

	// Since mono is not capable of processing Jurassic in full CLR mode https://bugzilla.xamarin.com/show_bug.cgi?id=7829
	// we practically need non-Jurassic host. It is not ideal though.
	public class NodeLanguageServiceHost : LanguageServiceHost
	{
		internal const string NodeServerAppFullPath = "node-ts-server.js";

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

		// FIXME: Only externally hosted node is still stable somehow...
		public NodeLanguageServiceHost ()
			//: this (FindAvailablePort (), false)
			: this (36140, true)
		{
		}

		public NodeLanguageServiceHost (int port, bool externalServer)
		{
			this.port = port;
			client = new WebClient ();
			Console.WriteLine (System.IO.Path.GetFullPath (NodeServerAppFullPath));
			if (!externalServer)
				node = Process.Start ("node", NodeServerAppFullPath + " " + port);
			engine = new ScriptEngine ();
			Console.WriteLine ("ran node at port " + port);
		}

		public override void Dispose ()
		{
			if (node == null)
				return;
			client.DownloadString ("http://localhost:" + port + "/?command=quit");
			int timeout = 3000;
			if (!node.HasExited && !node.WaitForExit (timeout))
				Console.Error.WriteLine ("Warning: node server did not finish within {0} milliseconds", timeout);
			node.Close ();
		}

		public override void Execute (string command)
		{
			ExecuteNode (command, false);
		}

		public override object Eval (string command)
		{
			ExecuteNode (command, true);
			return engine.Evaluate ("ret");
		}

		public override T Eval<T> (string command)
		{
			ExecuteNode (command, true);
			return engine.Evaluate<T> ("ret");
		}

		void ExecuteNode (string command, bool assignRet)
		{
			var res = Encoding.UTF8.GetString (client.UploadData ("http://localhost:" + port + "/?command=eval", Encoding.UTF8.GetBytes ((assignRet ? "ret = " : "") + command)));
			engine.Execute ("ret = " + (string.IsNullOrEmpty (res) ? "undefined" : res) + ";");
			var ret = engine.Evaluate<ObjectInstance> ("ret");
			var err = ret ["error"];
			if (err != null && err != Undefined.Value)
				throw new JavaScriptException (err, -1, null);
			engine.Execute ("ret = ret ['result'];");
		}

		public override ObjectInstance CallConstructor (string module, string className, params object[] args)
		{
			// jQuery-like create()->apply() construction instead of unsupported "new" operation.
			return Eval<ObjectInstance> (string.Format (@"new {0}.{1} ({2})", module, className, string.Join (",", args.Select (a => AnyToString (a)).ToArray ())));
		}

		string AnyToString (object o)
		{
			if (o == Undefined.Value)
				return "undefined";
			var oi = o as ObjectInstance;
			if (oi != null) {
				var kvp = refs.FirstOrDefault (p => p.Key.Instance == oi);
				if (kvp.Value != null)
					return kvp.Value;
			}
			return JSONObject.Stringify (engine, o);
		}

		public override object CallMemberFunction (ITypeScriptObject instance, string functionName, params object [] args)
		{
			string jinst = null;
			jinst = refs.TryGetValue (instance, out jinst) ? jinst : JSONObject.Stringify (engine, instance.Instance);
			return Eval (string.Format ("{0}.{1} ({2})", jinst, functionName, string.Join (",", args.Select (a => AnyToString (a)).ToArray ())));
		}

		string GetInstanceRepresentation (ITypeScriptObject instance)
		{
			string jinst = null;
			return refs.TryGetValue (instance, out jinst) ? jinst : JSONObject.Stringify (engine, instance.Instance);
		}
		
		public override object GetPropertyValue (ITypeScriptObject instance, string propertyName)
		{
			string jinst = GetInstanceRepresentation (instance);
			return Eval (string.Format ("{0}.{1}", jinst, propertyName));
		}
		
		public override void SetPropertyValue (ITypeScriptObject instance, string propertyName, object value)
		{
			string jinst = GetInstanceRepresentation (instance);
			Execute (string.Format ("{0}.{1} = {2}", jinst, propertyName, AnyToString (value)));
		}
		
		public override object GetArrayItem (ITypeScriptObject instance, int index)
		{
			string jinst = GetInstanceRepresentation (instance);
			return Eval (string.Format ("{0} [{1}]", jinst, index));
		}
		
		public override void SetArrayItem (ITypeScriptObject instance, int index, object value)
		{
			string jinst = GetInstanceRepresentation (instance);
			Execute (string.Format ("{0} [{1}] = {2}", jinst, index, AnyToString (value)));
		}

		int serial = 0;
		Dictionary<ITypeScriptObject,string> refs = new Dictionary<ITypeScriptObject,string> ();
		
		public override void AddReference (ITypeScriptObject instance)
		{
			var id = "__proxy_" + serial;
			refs [instance] = id;
			Execute (id + " = ret;");
			serial++;
		}
		
		public override void Release (ITypeScriptObject instance)
		{
			string id;
			refs.TryGetValue (instance, out id);
			if (id != null) {
				refs [instance] = null;
				Execute (id + " = undefined;");
			}
		}
	}
}

