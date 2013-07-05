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
using System.IO;

namespace TypeScriptServiceBridge.Hosting
{

	// Since mono is not capable of processing Jurassic in full CLR mode https://bugzilla.xamarin.com/show_bug.cgi?id=7829
	// we practically need non-Jurassic host. It is not ideal though.
	public class NodeLanguageServiceHost : LanguageServiceHost
	{
		public static Func<string> NodeCommandLocator;

		static string FindFromPath (string cmd)
		{
			foreach (var env in Environment.GetEnvironmentVariable ("PATH").Split (Path.PathSeparator)) {
				var path = Path.Combine (env, cmd);
				if (File.Exists (path))
					return path;
			}
			return null;
		}

		internal const string NodeServerAppFullPath = "node-ts-server.js";

		int port;
		WebClient client;
		Process node;
		ScriptEngine engine;

		static bool UseExternalNodeService {
			get { return Environment.GetEnvironmentVariable ("USE_EXTERNAL_NODE_SERVICE") != null; }
		}

		static int FindAvailablePort ()
		{
			if (UseExternalNodeService)
				return 36140;
			var rnd = new Random ();
			for (int i = 0; i < 100; i++) {
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
			}
			throw new InvalidOperationException ("100 attempts of opening a listener port failed. It is likely that no listen priviledge is given");
		}

		// FIXME: Only externally hosted node is still stable somehow...
		public NodeLanguageServiceHost ()
			: this (FindAvailablePort (), UseExternalNodeService)
		{
		}

		public NodeLanguageServiceHost (int port, bool externalServer)
		{
			this.port = port;
			var nodeServer = Path.Combine (Path.GetDirectoryName (new Uri (GetType ().Assembly.CodeBase).LocalPath), NodeServerAppFullPath);
			string nodeCommand = NodeCommandLocator != null ? NodeCommandLocator () : null;
			nodeCommand = string.IsNullOrEmpty (nodeCommand) ? FindFromPath ("node") : nodeCommand;
			if (!externalServer) {
				node = new Process ();
				var psi = node.StartInfo;
				psi.CreateNoWindow = true;
				psi.FileName = nodeCommand;
				psi.Arguments = nodeServer + " " + port;
				psi.UseShellExecute = false;
				psi.RedirectStandardOutput = true;
				psi.RedirectStandardError = true;
				psi.WorkingDirectory = Path.GetDirectoryName (NodeServerAppFullPath);
				node.OutputDataReceived += (sender, e) => { if (StandardOutputReceived != null) StandardOutputReceived (e.Data); };
				node.ErrorDataReceived += (sender, e) => { if (StandardErrorReceived != null) StandardErrorReceived (e.Data); };
				node.Start ();
				node.BeginErrorReadLine ();
				node.BeginOutputReadLine ();
			}
			engine = new ScriptEngine ();
			Console.WriteLine ("ran node at port " + port);
			if (node != null && node.HasExited) {
				throw new Exception ("node already exited, must be some error. Check diagnostic error output");
			}
			System.Threading.Thread.Sleep (200);
			client = new MyWebClient ();
		}

		public Action<string> StandardOutputReceived;
		public Action<string> StandardErrorReceived;

		public override void Dispose ()
		{
			if (node == null)
				return;
			client.DownloadString ("http://localhost:" + port + "/?command=quit");
			int timeout = 3000;
			if (!node.HasExited && !node.WaitForExit (timeout))
				Console.Error.WriteLine ("Warning: node server did not finish within {0} milliseconds", timeout);
			node.Close ();
			node = null;
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
			var ret = engine.Evaluate<ObjectInstance> ("ret = " + Encoding.UTF8.GetString (client.UploadData ("http://localhost:" + port + "/?command=eval", Encoding.UTF8.GetBytes ((assignRet ? "ret = " : "") + command))));
			var err = ret ["error"];
			if (err != null && err != Undefined.Value) {
				var errmsg = JSONObject.Stringify (engine, err);
				throw new JavaScriptException (engine, "EvalError", "Error was returned from the server: " + errmsg);
			}
			engine.Execute ("ret = ret ['result'];");
		}

		public override ObjectInstance CallConstructor (string module, string className, params object[] args)
		{
			// it may or may not return the actual instance. It depends on the result.
			return Eval (string.Format (@"new {0}.{1} ({2})", module, className, string.Join (",", args.Select (a => AnyToString (a)).ToArray ()))) as ObjectInstance;
		}

		string AnyToString (object o)
		{
			var to = o as ITypeScriptObject;
			if (to != null) {
				var obj = to.Instance;
				if (o == null || o == Null.Value)
					return "null";
				if (o == Undefined.Value)
					return "undefined";
				Identifier id;
				if (refs.TryGetValue (to, out id))
					return id.Value;
				else
					return JSONObject.Stringify (engine, obj);
			}
			return JSONObject.Stringify (engine, o);
		}

		public override object CallMemberFunction (ITypeScriptObject instance, string functionName, params object [] args)
		{
			string jinst = GetInstanceRepresentation (instance, true);
			return Eval (string.Format ("{0}.{1} ({2})", jinst, functionName, string.Join (",", args.Select (a => AnyToString (a)).ToArray ())));
		}

		string GetInstanceRepresentation (ITypeScriptObject instance, bool invalidOnLocalCache)
		{
			Identifier jinst = null;
			refs.TryGetValue (instance, out jinst);
			if (jinst == local_cache_id && invalidOnLocalCache)
				throw new InvalidOperationException ("Invalid operation on local cache object: it could result in any kind of inconsistency.");
			return jinst != null ? jinst.Value : JSONObject.Stringify (engine, instance.Instance);
		}
		
		public override object GetPropertyValue (ITypeScriptObject instance, string propertyName)
		{
			string jinst = GetInstanceRepresentation (instance, false);
			if (jinst == local_cache_id_string)
				return instance.Instance.GetPropertyValue (propertyName);
			else
				return Eval (string.Format ("{0}.{1}", jinst, propertyName));
		}
		
		public override void SetPropertyValue (ITypeScriptObject instance, string propertyName, object value)
		{
			string jinst = GetInstanceRepresentation (instance, true);
			Execute (string.Format ("{0}.{1} = {2}", jinst, propertyName, AnyToString (value)));
		}

		public override object GetStaticPropertyValue (Type typeScriptObjectType, string propertyName)
		{
			return Eval (string.Format ("{0}.{1}.{2}", typeScriptObjectType.Namespace, typeScriptObjectType.Name, propertyName));
		}
		
		public override void SetStaticPropertyValue (Type typeScriptObjectType, string propertyName, object value)
		{
			Execute (string.Format ("{0}.{1}.{2} = {3}", typeScriptObjectType.Namespace, typeScriptObjectType.Name, propertyName, AnyToString (value)));
		}

		public override object GetArrayItem (ITypeScriptObject instance, int index)
		{
			string jinst = GetInstanceRepresentation (instance, false);
			if (jinst == local_cache_id_string)
				return ((ArrayInstance) instance.Instance) [index];
			return Eval (string.Format ("{0} [{1}]", jinst, index));
		}
		
		public override void SetArrayItem (ITypeScriptObject instance, int index, object value)
		{
			string jinst = GetInstanceRepresentation (instance, true);
			Execute (string.Format ("{0} [{1}] = {2}", jinst, index, AnyToString (value)));
		}

		object lock_obj = new object ();
		int serial = 0;
		Dictionary<ITypeScriptObject,Identifier> refs = new Dictionary<ITypeScriptObject,Identifier> ();
		
		public override void AddReference (ITypeScriptObject instance)
		{
			lock (lock_obj) {
				var id = new Identifier ("__proxy_" + serial);
				//instance.Instance = id;
				refs [instance] = id;
				Execute (id.Value + " = ret;");
				serial++;
			}
		}
		
		public override void Release (ITypeScriptObject instance)
		{
			Identifier id;
			refs.TryGetValue (instance, out id);
			if (id != null) {
				refs [instance] = null;
				Execute (id.Value + " = undefined;");
			}
		}

		const string local_cache_id_string = "__local_cache_id__";
		static readonly Identifier local_cache_id = new Identifier (local_cache_id_string);

		public override ITypeScriptObject Cached (ITypeScriptObject instance)
		{
			if (instance == null)
				return null;
			Identifier id;
			if (refs.TryGetValue (instance, out id) && id == local_cache_id)
				return instance; // already a cache
			var ret = (ITypeScriptObject) Activator.CreateInstance (instance.GetType (), instance.Instance);
			refs [ret] = local_cache_id; // assign a null key
			return ret;
		}
	}

	class Identifier
	{
		public Identifier (string value)
		{
			Value = value;
		}

		public string Value { get; private set; }
	}

	class MyWebClient : WebClient
	{
		protected override WebRequest GetWebRequest (Uri address)
		{
			var req = (HttpWebRequest) base.GetWebRequest (address);
			req.Timeout = 5000;
			return req;
		}
	}
}

