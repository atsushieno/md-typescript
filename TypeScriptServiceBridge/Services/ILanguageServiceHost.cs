using System;
using Jurassic.Library;
using TypeScriptServiceBridge.Hosting;
using TypeScriptServiceBridge.TypeScript;

namespace TypeScriptServiceBridge
{
	public interface ILanguageServiceHost : ILogger
	{
		ObjectInstance GetCompilationSettings ();
        double GetScriptCount ();
        string GetScriptId(double scriptIndex);
		string GetScriptSourceText (double scriptIndex, double start, double end);
        double GetScriptSourceLength (double scriptIndex);
        bool GetScriptIsResident (double scriptIndex);
        double GetScriptVersion (double scriptIndex);
        ObjectInstance GetScriptEditRangeSinceVersion (double scriptIndex, double scriptVersion);
	}

	public class LanguageServiceHost_Impl : Logger_Impl, ILanguageServiceHost
	{
		ObjectInstance instance;

		public LanguageServiceHost_Impl (ObjectInstance instance)
			: base (instance)
		{
			this.instance = instance;
		}

		#region ILanguageServiceHost implementation
		public ObjectInstance GetCompilationSettings ()
		{
			return (ObjectInstance) instance.CallMemberFunction ("getCompilationSettings");
		}

		public double GetScriptCount ()
		{
			return (double) instance.CallMemberFunction ("getScriptCount");
		}

		public string GetScriptId (double scriptIndex)
		{
			return (string) instance.CallMemberFunction ("getScriptId", scriptIndex);
		}

		public string GetScriptSourceText (double scriptIndex, double start, double end)
		{
			return (string) instance.CallMemberFunction ("getScriptSourceText", scriptIndex, start, end);
		}

		public double GetScriptSourceLength (double scriptIndex)
		{
			return (double) instance.CallMemberFunction ("GetScriptSourceLength", scriptIndex);
		}

		public bool GetScriptIsResident (double scriptIndex)
		{
			return (bool) instance.CallMemberFunction ("getScriptIsResident", scriptIndex);
		}

		public double GetScriptVersion (double scriptIndex)
		{
			return (double) instance.CallMemberFunction ("getScriptVersion", scriptIndex);
		}

		public ObjectInstance GetScriptEditRangeSinceVersion (double scriptIndex, double scriptVersion)
		{
			return (ObjectInstance) instance.CallMemberFunction ("getScriptEditRangeSinceVersion", scriptIndex, scriptVersion);
		}
		#endregion
	}
}

