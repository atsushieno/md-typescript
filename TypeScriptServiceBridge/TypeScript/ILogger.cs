using System;
using Jurassic.Library;
using TypeScriptServiceBridge.TypeSystem;

namespace TypeScriptServiceBridge.TypeScript
{
	public interface ILogger
	{
        bool Information ();
		bool Debug ();
		bool Warning ();
		bool Error ();
        bool Fatal ();
		void Log (string s);
	}

	public class Logger_Impl : TypeScriptObject, ILogger
	{
		ObjectInstance instance;

		public Logger_Impl (ObjectInstance instance)
		{
			this.instance = instance;
		}

		public override void Dispose ()
		{
		}

		#region ILogger implementation
		public bool Information ()
		{
			return (bool) instance.CallMemberFunction ("information");
		}

		public bool Debug ()
		{
			return (bool) instance.CallMemberFunction ("debug");
		}

		public bool Warning ()
		{
			return (bool) instance.CallMemberFunction ("warning");
		}

		public bool Error ()
		{
			return (bool) instance.CallMemberFunction ("error");
		}

		public bool Fatal ()
		{
			return (bool) instance.CallMemberFunction ("fatal");
		}

		public void Log (string s)
		{
			instance.CallMemberFunction ("log", s);
		}
		#endregion
	}
}

