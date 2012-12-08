using System;
using Jurassic.Library;

namespace TypeScriptServiceBridge.TypeScript
{
	public interface ILogger : ITypeScriptObject
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
		public Logger_Impl (ObjectInstance instance)
			: base (instance)
		{
		}

		public override void Dispose ()
		{
		}

		#region ILogger implementation
		public bool Information ()
		{
			return (bool) Instance.CallMemberFunction ("information");
		}

		public bool Debug ()
		{
			return (bool) Instance.CallMemberFunction ("debug");
		}

		public bool Warning ()
		{
			return (bool) Instance.CallMemberFunction ("warning");
		}

		public bool Error ()
		{
			return (bool) Instance.CallMemberFunction ("error");
		}

		public bool Fatal ()
		{
			return (bool) Instance.CallMemberFunction ("fatal");
		}

		public void Log (string s)
		{
			Instance.CallMemberFunction ("log", s);
		}
		#endregion
	}
}

