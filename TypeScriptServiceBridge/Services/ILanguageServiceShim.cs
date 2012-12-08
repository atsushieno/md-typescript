using System;
using Jurassic.Library;
using TypeScriptServiceBridge.TypeScript;

namespace TypeScriptServiceBridge.Services
{
	public interface ILanguageServiceShim : ITypeScriptObject
	{
		ILanguageServiceShimHost Host { get; set; }
		ILanguageService LanguageService { get; set; }
		ILogger Logger { get; set; }

        void Dispose (object dummy);
        void Refresh (bool throwOnError);

        void LogAST (string fileName);
		void LogSyntaxAST (string fileName);

        string GetErrors(double maxCount);
        string GetScriptErrors(string fileName, double maxCount);
        string GetTypeAtPosition(string fileName, double pos);
        string GetSignatureAtPosition(string fileName, double pos);
        string GetDefinitionAtPosition(string fileName, double pos);
        string GetBraceMatchingAtPosition(string fileName, double pos);
        string GetSmartIndentAtLineNumber(string fileName, double lineNumber, string options/*Services.EditorOptions*/);
        string GetReferencesAtPosition(string fileName, double pos);
        string GetOccurrencesAtPosition(string fileName, double pos);
        void GetCompletionsAtPosition(string fileName, double pos, bool isMemberCompletion);
        string GetImplementorsAtPosition(string fileName, double pos);
        string GetFormattingEditsForRange(string fileName, double minChar, double limChar, string options/*Services.FormatCodeOptions*/);
        string GetFormattingEditsAfterKeystroke(string fileName, double position, string key, string options/*Services.FormatCodeOptions*/);
        string GetNavigateToItems(string searchValue);
        string GetScriptLexicalStructure(string fileName);
        string GetOutliningRegions(string fileName);
	}

	public class LanguageServiceShim_Impl : TypeScriptObject, ILanguageServiceShim
	{
		public LanguageServiceShim_Impl (ObjectInstance instance)
			: base (instance)
		{
		}

		#region ILanguageServiceShim implementation

        public void Dispose (object dummy)
		{
			Instance.CallMemberFunction ("dispose", TypeScriptObject.ToNative (dummy));
		}

		public void Refresh (bool throwOnError)
		{
			Instance.CallMemberFunction ("refresh", throwOnError);
		}

		public void LogAST (string fileName)
		{
			Instance.CallMemberFunction ("logAST", fileName);
		}

		public void LogSyntaxAST (string fileName)
		{
			Instance.CallMemberFunction ("logSyntaxAST", fileName);
		}

		public string GetErrors (double maxCount)
		{
			return (string) Instance.CallMemberFunction ("getErrors", maxCount);
		}

		public string GetScriptErrors (string fileName, double maxCount)
		{
			return (string) Instance.CallMemberFunction ("getScriptErrors", fileName, maxCount);
		}

		public string GetTypeAtPosition (string fileName, double pos)
		{
			return (string) Instance.CallMemberFunction ("getTypeAtPosition", fileName, pos);
		}

		public string GetSignatureAtPosition (string fileName, double pos)
		{
			return (string) Instance.CallMemberFunction ("getSignatureAtPosition", fileName, pos);
		}

		public string GetDefinitionAtPosition (string fileName, double pos)
		{
			return (string) Instance.CallMemberFunction ("getDefinitionAtPosition", fileName, pos);
		}

		public string GetBraceMatchingAtPosition (string fileName, double pos)
		{
			return (string) Instance.CallMemberFunction ("getBraceMatchingAtPosition", fileName, pos);
		}

		public string GetSmartIndentAtLineNumber (string fileName, double lineNumber, string options)
		{
			return (string) Instance.CallMemberFunction ("getSmartIndentAtLineNumber", fileName, lineNumber, options);
		}

		public string GetReferencesAtPosition (string fileName, double pos)
		{
			return (string) Instance.CallMemberFunction ("getReferencesAtPosition", fileName, pos);
		}

		public string GetOccurrencesAtPosition (string fileName, double pos)
		{
			return (string) Instance.CallMemberFunction ("getOccurrencesAtPosition", fileName, pos);
		}

		public void GetCompletionsAtPosition (string fileName, double pos, bool isMemberCompletion)
		{
			Instance.CallMemberFunction ("getCompletionsAtPosition", fileName, pos, isMemberCompletion);
		}

		public string GetImplementorsAtPosition (string fileName, double pos)
		{
			return (string) Instance.CallMemberFunction ("getImplementorsAtPosition", fileName, pos);
		}

		public string GetFormattingEditsForRange (string fileName, double minChar, double limChar, string options)
		{
			return (string) Instance.CallMemberFunction ("getFormattingEditsForRange", fileName, minChar, limChar, options);
		}

		public string GetFormattingEditsAfterKeystroke (string fileName, double position, string key, string options)
		{
			return (string) Instance.CallMemberFunction ("getFormattingEditsAfterKeystroke", fileName, position, key, options);
		}

		public string GetNavigateToItems (string searchValue)
		{
			return (string) Instance.CallMemberFunction ("getNavigateToItems", searchValue);
		}

		public string GetScriptLexicalStructure (string fileName)
		{
			return (string) Instance.CallMemberFunction ("getScriptLexicalStructure", fileName);
		}

		public string GetOutliningRegions (string fileName)
		{
			return (string) Instance.CallMemberFunction ("getOutliningRegions", fileName);
		}

		public ILanguageServiceShimHost Host {
			get { return new LanguageServiceShimHost_Impl ((ObjectInstance) Instance.GetPropertyValue ("host")); }
			set { Instance.SetPropertyValue ("host", value.Instance, true); }
		}

		public ILanguageService LanguageService {
			get { return new LanguageService_Impl ((ObjectInstance) Instance.GetPropertyValue ("languageService")); }
			set { Instance.SetPropertyValue ("languageService", value.Instance, true); }
		}

		public ILogger Logger {
			get { return new Logger_Impl ((ObjectInstance) Instance.GetPropertyValue ("logger")); }
			set { Instance.SetPropertyValue ("logger", value.Instance, true); }
		}
		#endregion
	}
}

