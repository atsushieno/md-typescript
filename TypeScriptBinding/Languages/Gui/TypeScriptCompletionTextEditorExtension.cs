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
using TypeScriptServiceBridge.Harness;
using TypeScriptServiceBridge.Services;
using TypeScriptServiceBridge.TypeScript;

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
				LoggingService.LogDebug ("  idx: " + idx);
				if (idx >= 0)
					return doc.Substring (idx, startOffset - idx).Count (c => c == ',');
			}
			return base.GetCurrentParameterIndex (startOffset);
		}
		
		public override bool KeyPress (Gdk.Key key, char keyChar, Gdk.ModifierType modifier)
		{
			bool result = base.KeyPress (key, keyChar, modifier);
			
			if (EnableParameterInsight && (keyChar == ',' || keyChar == ')') && CanRunParameterCompletionCommand ())
				base.RunParameterCompletionCommand ();
			
			return result;
		}

		TypeScriptLS shimHost {
			get { return service.ShimHost; }
		}

		ILanguageService ls {
			get { return service.LanguageService; }
		}

		public override ICompletionDataList HandleCodeCompletion (CodeCompletionContext completionContext, char completionChar, ref int triggerWordLength)
		{
			if (!EnableCodeCompletion)
				return null;
			if (!EnableAutoCodeCompletion && char.IsLetter (completionChar))
				return null;
			
			try {
				if (char.IsLetterOrDigit (completionChar) || completionChar == '_') {
					if (completionContext.TriggerOffset > 1 && char.IsLetterOrDigit (document.Editor.GetCharAt (completionContext.TriggerOffset - 2)))
						return null;
					triggerWordLength = 1;
				}
				return InternalHandleCodeCompletion (completionContext, completionChar, false, ref triggerWordLength);
			} catch (Exception e) {
				LoggingService.LogError ("Unexpected code completion exception." + Environment.NewLine + 
				                         "FileName: " + Document.FileName + Environment.NewLine + 
				                         "Position: line=" + completionContext.TriggerLine + " col=" + completionContext.TriggerLineOffset + Environment.NewLine + 
				                         "Line text: " + Document.Editor.GetLineText (completionContext.TriggerLine), 
				                         e);
				return null;
			}
		}

		DateTimeOffset last_script_updated_time = DateTimeOffset.MinValue;
		Dictionary<string,DateTimeOffset> lastUpdatedTime = new Dictionary<string, DateTimeOffset> ();

		void UpdateScripts ()
		{
			foreach (var doc in IdeApp.Workbench.Documents.Where (d => d.Project == this.Document.Project && d.IsCompileableInProject)) {
				DateTimeOffset last;
				var path = service.GetFilePath (doc.FileName);
				if (!lastUpdatedTime.TryGetValue (path, out last) || last > last_script_updated_time) {
					lastUpdatedTime [path] = DateTimeOffset.UtcNow;
					shimHost.UpdateScript (path, doc.Editor.Text);
				}
			}
			last_script_updated_time = DateTimeOffset.UtcNow;
		}

		ICompletionDataList InternalHandleCodeCompletion (CodeCompletionContext completionContext, char completionChar, bool ctrlSpace, ref int triggerWordLength)
		{
			if (completionContext != null) {
				UpdateScripts ();
				string file = service.GetFilePath (Document.FileName);
				var ret = new CompletionDataList ();
				var list = ls.GetCompletionsAtPosition (file, (int) completionContext.TriggerOffset, true);
				foreach (var entry in list.Entries) {
					IconId icon = IconId.Null;
					// FIXME: fill icons
					/*
					if (entry.Kind == ScriptElementKind.MemberFunctionElement
					    || entry.Kind == ScriptElementKind.FunctionElement)
						icon = methodIcon;
					else if (entry.Kind == ScriptElementKind.MemberVariableElement
					    || entry.Kind == ScriptElementKind.VariableElement)
						icon = fieldIcon;
					*/
					ret.Add (new CompletionData (entry.Name, icon, string.Format ("({0}) {1} : {2}", entry.Kind, entry.Name, entry.Type)));
				}
				return ret;
			}
			return base.HandleCodeCompletion (completionContext, completionChar, ref triggerWordLength);
		}
		
		internal Mono.TextEditor.TextEditorData TextEditorData {
			get {
				var doc = Document;
				if (doc == null)
					return null;
				return doc.Editor;
			}
		}

		public override ICompletionDataList CodeCompletionCommand (CodeCompletionContext completionContext)
		{
			int triggerWordLength = 0;
			char ch = completionContext.TriggerOffset > 0 ? TextEditorData.GetCharAt (completionContext.TriggerOffset - 1) : '\0';
			return InternalHandleCodeCompletion (completionContext, ch, true, ref triggerWordLength);
		}

		public override IParameterDataProvider HandleParameterCompletion (CodeCompletionContext completionContext, char completionChar)
		{
			LoggingService.LogDebug ("HandleParameterCompletion: ({0},{1}) {2}",
			                   completionContext.TriggerLine, completionContext.TriggerLineOffset, completionContext.TriggerOffset);
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
