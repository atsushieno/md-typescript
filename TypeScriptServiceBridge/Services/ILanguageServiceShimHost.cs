using System;
using Jurassic.Library;
using TypeScriptServiceBridge.TypeScript;

namespace TypeScriptServiceBridge.Services
{
	public interface ILanguageServiceShimHost : ILogger
	{
		string GetCompilationSettings ();
        double GetScriptCount();
		string GetScriptId (double scriptIndex);
        string GetScriptSourceText (double scriptIndex, double start, double end);
        double GetScriptSourceLength (double scriptIndex);
        bool GetScriptIsResident (double scriptIndex);
        double GetScriptVersion (double scriptIndex);
        string GetScriptEditRangeSinceVersion (double scriptIndex, double scriptVersion);
    }

	public class LanguageServiceShimHost_Impl : Logger_Impl, ILanguageServiceShimHost
	{
		public LanguageServiceShimHost_Impl (ObjectInstance instance)
			: base (instance)
		{
		}

		#region ILanguageServiceShimHost implementation
		public string GetCompilationSettings ()
		{
			return (string) Instance.CallMemberFunction ("getCompilationSettings");
		}

		public double GetScriptCount ()
		{
			return (double) Instance.CallMemberFunction ("getScriptCount");
		}

		public string GetScriptId (double scriptIndex)
		{
			return (string) Instance.CallMemberFunction ("getScriptId", scriptIndex);
		}

		public string GetScriptSourceText (double scriptIndex, double start, double end)
		{
			return (string) Instance.CallMemberFunction ("getScriptSourceText", scriptIndex, start, end);
		}

		public double GetScriptSourceLength (double scriptIndex)
		{
			return (double) Instance.CallMemberFunction ("getScriptSourceLength", scriptIndex);
		}

		public bool GetScriptIsResident (double scriptIndex)
		{
			return (bool) Instance.CallMemberFunction ("getScriptIsResident", scriptIndex);
		}

		public double GetScriptVersion (double scriptIndex)
		{
			return (double) Instance.CallMemberFunction ("getScriptVersion", scriptIndex);
		}

		public string GetScriptEditRangeSinceVersion (double scriptIndex, double scriptVersion)
		{
			return (string) Instance.CallMemberFunction ("getScriptEditRangeSinceVersion", scriptIndex, scriptVersion);
		}
		#endregion
	}
}

