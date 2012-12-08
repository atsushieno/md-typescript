using System;
using Jurassic.Library;
using TypeScriptServiceBridge.Hosting;

namespace TypeScriptServiceBridge.Services
{
	public interface ILanguageService : ITypeScriptObject
	{
		ILanguageServiceHost Host { get; set; }

		void Refresh ();

		void LogAST (string filename);
		void LogSyntaxAST (string filename);

		ArrayInstance GetErrors (double maxCount);

		ObjectInstance GetScriptAST (string fileName);
		ArrayInstance GetScriptErrors (string fileName, double maxCount);
		ObjectInstance GetCompletionsAtPosition (string fileName, double pos, bool isMemberCompletion);
		ObjectInstance GetTypeAtPosition (string fileName, double pos);
		ObjectInstance GetSignatureAtPosition (string fileName, double pos);
		ObjectInstance GetDefinitionAtPosition (string fileName, double pos);
		ArrayInstance GetReferencesAtPosition (string fileName, double pos);
		ArrayInstance GetOccurrencesAtPosition (string fileName, double pos);
		ArrayInstance GetImplementorsAtPosition (string fileName, double pos);
		ArrayInstance GetNavigateToItems (string searchValue);
		ArrayInstance GetScriptLexicalStructure (string fileName);
		ArrayInstance GetOutliningRegions (string fileName);

		ArrayInstance GetScriptSyntaxAST (string fileName);
		ArrayInstance GetFormattingEditsForRange (string fileName, double minChar, double limChar, ObjectInstance options);
		ArrayInstance GetFormattingEditsAfterKeystroke (string fileName, double position, string key, ObjectInstance options);
		ArrayInstance GetBraceMatchingAtPosition (string fileName, double position);
		double GetSmartIndentAtLineNumber (string fileName, double lineNumber, ObjectInstance options);

		ObjectInstance GetAstPathToPosition (ObjectInstance script, double pos, ObjectInstance options);
		ObjectInstance GetIdentifierPathToPosition (ObjectInstance script, double pos);
		ObjectInstance GetSymbolAtPosition (ObjectInstance script, double pos);

		ObjectInstance GetSymbolTree ();
	}

	public class LanguageService_Impl : TypeScriptObject, ILanguageService
	{
		static ILanguageService ls_instance;

		public static ILanguageService Create ()
		{
			return new LanguageService_Impl (LanguageServiceHost.Evaluate<ObjectInstance> (
				@"new Services.TypeScriptServicesFactory ().createLanguageService (
        			new Services.LanguageServiceShimHostAdapter (
						new Harness.TypeScriptLS ()));"));
		}

		public static ILanguageService Get ()
		{
			if (ls_instance == null)
				ls_instance = Create ();
			return ls_instance;
		}

		internal LanguageService_Impl (ObjectInstance instance)
			: base (instance)
		{
		}

		#region ts public members
		public ILanguageServiceHost Host {
			get { return new LanguageServiceHost_Impl ((ObjectInstance) Instance.GetPropertyValue ("host")); }
			set { Instance.SetPropertyValue ("host", value.Instance, true); }
		}

		public void Refresh ()
		{
			Instance.CallMemberFunction ("refresh");
		}

		public void LogAST (string filename)
		{
			Instance.CallMemberFunction ("logAST", filename);
		}
		public void LogSyntaxAST (string filename)
		{
			Instance.CallMemberFunction ("logSyntaxAST", filename);
		}

		public ArrayInstance GetErrors (double maxCount)
		{
			return (ArrayInstance) Instance.CallMemberFunction ("getErrors", maxCount);
		}

		public ObjectInstance GetScriptAST (string fileName)
		{
			return (ObjectInstance) Instance.CallMemberFunction ("getScriptAST", fileName);
		}
		public ArrayInstance GetScriptErrors (string fileName, double maxCount)
		{
			return (ArrayInstance) Instance.CallMemberFunction ("getScriptErrors", fileName, maxCount);
		}
		public ObjectInstance GetCompletionsAtPosition (string fileName, double pos, bool isMemberCompletion)
		{
			return (ObjectInstance) Instance.CallMemberFunction ("getCompletionsAtPosition", fileName, pos, isMemberCompletion);
		}
		public ObjectInstance GetTypeAtPosition (string fileName, double pos)
		{
			return (ObjectInstance) Instance.CallMemberFunction ("getTypeAtPosition", fileName, pos);
		}
		public ObjectInstance GetSignatureAtPosition (string fileName, double pos)
		{
			return (ObjectInstance) Instance.CallMemberFunction ("getSignatureAtPosition", fileName, pos);
		}
		public ObjectInstance GetDefinitionAtPosition (string fileName, double pos)
		{
			return (ObjectInstance) Instance.CallMemberFunction ("getDefinitionAtPosition", fileName, pos);
		}
		public ArrayInstance GetReferencesAtPosition (string fileName, double pos)
		{
			return (ArrayInstance) Instance.CallMemberFunction ("getReferencesAtPosition", fileName, pos);
		}
		public ArrayInstance GetOccurrencesAtPosition (string fileName, double pos)
		{
			return (ArrayInstance) Instance.CallMemberFunction ("getOccurrencesAtPosition", fileName, pos);
		}
		public ArrayInstance GetImplementorsAtPosition (string fileName, double pos)
		{
			return (ArrayInstance) Instance.CallMemberFunction ("getImplementorsAtPosition", fileName, pos);
		}
		public ArrayInstance GetNavigateToItems (string searchValue)
		{
			return (ArrayInstance) Instance.CallMemberFunction ("getNavigateToItems", searchValue);
		}
		public ArrayInstance GetScriptLexicalStructure (string fileName)
		{
			return (ArrayInstance) Instance.CallMemberFunction ("getScriptLexicalStructure", fileName);
		}
		public ArrayInstance GetOutliningRegions (string fileName)
		{
			return (ArrayInstance) Instance.CallMemberFunction ("getOutliningRegions", fileName);
		}

		public ArrayInstance GetScriptSyntaxAST (string fileName)
		{
			return (ArrayInstance) Instance.CallMemberFunction ("getScriptSyntaxAST", fileName);
		}
		public ArrayInstance GetFormattingEditsForRange (string fileName, double minChar, double limChar, ObjectInstance options)
		{
			return (ArrayInstance) Instance.CallMemberFunction ("getFormattingEditsForRange", fileName, minChar, limChar, options);
		}
		public ArrayInstance GetFormattingEditsAfterKeystroke (string fileName, double position, string key, ObjectInstance options)
		{
			return (ArrayInstance) Instance.CallMemberFunction ("getFormattingEditsAfterKeystroke", fileName, position, key, options);
		}
		public ArrayInstance GetBraceMatchingAtPosition (string fileName, double position)
		{
			return (ArrayInstance) Instance.CallMemberFunction ("getBraceMatchingAtPosition", fileName, position);
		}
		public double GetSmartIndentAtLineNumber (string fileName, double lineNumber, ObjectInstance options)
		{
			return (double) Instance.CallMemberFunction ("getSmartIndentAtLineNumber", fileName, lineNumber, options);
		}

		public ObjectInstance GetAstPathToPosition (ObjectInstance script, double pos, ObjectInstance options)
		{
			return (ObjectInstance) Instance.CallMemberFunction ("getAstPathToPosition", script, pos, options);
		}
		public ObjectInstance GetIdentifierPathToPosition (ObjectInstance script, double pos)
		{
			return (ObjectInstance) Instance.CallMemberFunction ("getIdentifierPathToPosition", script, pos);
		}
		public ObjectInstance GetSymbolAtPosition (ObjectInstance script, double pos)
		{
			return (ObjectInstance) Instance.CallMemberFunction ("getSymbolAtPosition", script, pos);
		}

		public ObjectInstance GetSymbolTree ()
		{
			return (ObjectInstance) Instance.CallMemberFunction ("getSymbolTree");
		}
		#endregion
	}
}

