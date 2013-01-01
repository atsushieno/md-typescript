using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;

using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Gui.Content;
using MonoDevelop.Ide.CodeCompletion;
using MonoDevelop.Components;
using MonoDevelop.Components.Commands;

using Mono.TextEditor;
using MonoDevelop.Ide.TypeSystem;
using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.NRefactory.Completion;

using MonoDevelop.TypeScriptBinding.Projects;
using TypeScriptServiceBridge.TypeScript;
using TypeScriptServiceBridge.Services;

namespace MonoDevelop.TypeScriptBinding.Languages.Gui
{
	public class TypeScriptCompletionTextEditorExtension : CompletionTextEditorExtension//, IPathedDocument
	{
		TypeScriptFacade service;

		public TypeScriptCompletionTextEditorExtension ()
		{
		}

		public override void Initialize ()
		{
			base.Initialize ();
			var tp = this.Document.Project as TypeScriptProject;
			if (tp != null)
				service = tp.TypeScriptService;
		}
		
		public override void Dispose ()
		{
			base.Dispose ();
		}

		public override bool CanRunCompletionCommand ()
		{
			return service != null;
		}

		public override bool CanRunParameterCompletionCommand ()
		{
			return service != null;
		}

		public override int GetCurrentParameterIndex (int startOffset)
		{
			var ast = ls.GetScriptAST (service.GetFilePath (Document.FileName));
			var ap = ls.GetAstPathToPosition (ast, startOffset, GetAstPathOptions.Default);
			if (/*ap.IsArgumentListOfCall || ap.IsArgumentListOfFunction || ap.IsArgumentListOfNew
			    ||*/ ap.IsArgumentOfClassConstructor () || ap.IsArgumentOfFunction ()) {
				var doc = Document.ToString ();
				int idx = doc.LastIndexOf ('(', startOffset);
				Console.WriteLine ("  idx: " + idx);
				if (idx >= 0)
					return doc.Substring (idx, startOffset - idx).Count (c => c == ',');
			}
			return base.GetCurrentParameterIndex (startOffset);
		}

		ILanguageService ls {
			get { return service.LanguageService; }
		}

		public override ICompletionDataList HandleCodeCompletion (CodeCompletionContext completionContext, char completionChar, ref int triggerWordLength)
		{
			if (completionContext != null) {
				var ret = new CompletionDataList ();
				var list = ls.GetCompletionsAtPosition (service.GetFilePath (Document.FileName), completionContext.TriggerOffset, true);
				foreach (var entry in list.Entries) {
					ret.Add (new CompletionData (entry.Name, MonoDevelop.Core.IconId.Null, entry.Name + " : " + entry.Type));
				}
				return ret;
			}
			return base.HandleCodeCompletion (completionContext, completionChar, ref triggerWordLength);
		}
		
		public override IParameterDataProvider HandleParameterCompletion (CodeCompletionContext completionContext, char completionChar)
		{
			return base.HandleParameterCompletion (completionContext, completionChar);
		}

		public override bool GetParameterCompletionCommandOffset (out int cpos)
		{
			return base.GetParameterCompletionCommandOffset (out cpos);
		}

		/*
		#region IPathedDocument implementation

		public event EventHandler<DocumentPathChangedEventArgs> PathChanged;

		public Gtk.Widget CreatePathWidget (int index)
		{
			throw new NotImplementedException ();
		}

		public PathEntry[] CurrentPath {
			get {
					throw new NotImplementedException ();
			}
		}

		#endregion
		*/
	}
}
