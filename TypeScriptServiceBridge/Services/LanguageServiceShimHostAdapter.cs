using System;
using Jurassic.Library;
using TypeScriptServiceBridge.TypeScript;
using TypeScriptServiceBridge.Harness;

namespace TypeScriptServiceBridge.Services
{
	public class LanguageServiceShimHostAdapter : TypeScriptObject, ILanguageServiceHost
	{
		public LanguageServiceShimHostAdapter (ObjectInstance instance)
			: base (instance)
		{
		}

		public LanguageServiceShimHostAdapter (ILanguageServiceShimHost shimHost)
			: this (TypeScriptObject.CallConstructor ("Services", "LanguageServiceShimHostAdapter", shimHost.Instance))
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

		#region ILanguageServiceHost implementation
		public ObjectInstance GetCompilationSettings ()
		{
			return (ObjectInstance) Instance.CallMemberFunction ("getCompilationSettings");
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
			return (double) Instance.CallMemberFunction ("GetScriptSourceLength", scriptIndex);
		}

		public bool GetScriptIsResident (double scriptIndex)
		{
			return (bool) Instance.CallMemberFunction ("getScriptIsResident", scriptIndex);
		}

		public double GetScriptVersion (double scriptIndex)
		{
			return (double) Instance.CallMemberFunction ("getScriptVersion", scriptIndex);
		}

		public ObjectInstance GetScriptEditRangeSinceVersion (double scriptIndex, double scriptVersion)
		{
			return (ObjectInstance) Instance.CallMemberFunction ("getScriptEditRangeSinceVersion", scriptIndex, scriptVersion);
		}
		#endregion
	}
}

