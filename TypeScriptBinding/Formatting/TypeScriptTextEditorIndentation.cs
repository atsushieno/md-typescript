//
// CSharpTextEditorIndentation.cs
//
// Author:
//   Mike Krüger <mkrueger@novell.com>
//   Carlos Alberto Cortez <calberto.cortez@gmail.com>
//
// Copyright (C) 2008 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml;
using MonoDevelop.Core;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Gui.Content;

using MonoDevelop.Projects;
using MonoDevelop.Ide.CodeCompletion;

using Mono.TextEditor;
using MonoDevelop.Ide.CodeTemplates;
using MonoDevelop.Ide.TypeSystem;
using MonoDevelop.SourceEditor;

namespace MonoDevelop.TypeScriptBinding.Formatting
{
	public class TypeScriptTextEditorIndentation : TextEditorExtension
	{
		DocumentStateTracker<TypeScriptIndentEngine> stateTracker;
		int cursorPositionBeforeKeyPress;
		TypeScriptFormattingPolicy policy;
		TextStylePolicy textStylePolicy;

		char lastCharInserted;

		static TypeScriptTextEditorIndentation ()
		{
			CompletionWindowManager.WordCompleted += delegate (object sender, CodeCompletionContextEventArgs e) {
				IExtensibleTextEditor editor = e.Widget as IExtensibleTextEditor;
				if (editor == null)
					return;

				ITextEditorExtension textEditorExtension = editor.Extension;
				while (textEditorExtension != null && !(textEditorExtension is TypeScriptTextEditorIndentation))
					textEditorExtension = textEditorExtension.Next;

				TypeScriptTextEditorIndentation extension = textEditorExtension as TypeScriptTextEditorIndentation;
				if (extension == null)
					return;

				extension.stateTracker.UpdateEngine ();
				if (extension.stateTracker.Engine.NeedsReindent)
					extension.DoReSmartIndent ();
			};
		}


		void HandleTextPaste (int insertionOffset, string text, int insertedChars)
		{
			var documentLine = Editor.GetLineByOffset (insertionOffset + insertedChars);
			while (documentLine != null && insertionOffset < documentLine.EndOffset) {
				stateTracker.UpdateEngine (documentLine.Offset);
				DoReSmartIndent (documentLine.Offset);

				documentLine = documentLine.PreviousLine;
			}
		} 

		public static bool OnTheFlyFormatting {
			get {
				return PropertyService.Get ("OnTheFlyFormatting", true);
			}
			set {
				PropertyService.Set ("OnTheFlyFormatting", value);
			}
		}
		
		void RunFormatter (DocumentLocation location)
		{
			if (!OnTheFlyFormatting || Editor == null)
				return;

			var editMode = Editor.CurrentMode;
			if (editMode is TextLinkEditMode || editMode is InsertionCursorEditMode)
				return;

			// FIXME - Port a formatter for Typescript.
			//OnTheFlyFormatter.Format (Document, location);
		}

		public TypeScriptTextEditorIndentation ()
		{
			IEnumerable<string> types = MonoDevelop.Ide.DesktopService.GetMimeTypeInheritanceChain (TypeScriptFormatter.MimeType);
			policy = MonoDevelop.Projects.Policies.PolicyService.GetDefaultPolicy<TypeScriptFormattingPolicy> (types);
			textStylePolicy = MonoDevelop.Projects.Policies.PolicyService.GetDefaultPolicy<TextStylePolicy> (types);
		}

		public override void Initialize ()
		{
			base.Initialize ();

			IEnumerable<string> types = MonoDevelop.Ide.DesktopService.GetMimeTypeInheritanceChain (TypeScriptFormatter.MimeType);
			if (Document.Project != null && Document.Project.Policies != null) {
				policy = base.Document.Project.Policies.Get<TypeScriptFormattingPolicy> (types);
				textStylePolicy = base.Document.Project.Policies.Get<TextStylePolicy> (types);
			}

			if (Editor != null) {
				Editor.Options.Changed += (o, args) => {
					var project = Document.Project;
					if (project != null) {
						policy = project.Policies.Get<TypeScriptFormattingPolicy> (types);
						textStylePolicy = project.Policies.Get<TextStylePolicy> (types);
					}

					/*
					Editor.IndentationTracker = new IndentVirtualSpaceManager (
						Editor,
						new DocumentStateTracker<TypeScriptIndentEngine> (new TypeScriptIndentEngine (policy, textStylePolicy), Editor)
					);
					*/
				};

				/*
				Editor.IndentationTracker = new IndentVirtualSpaceManager (
					Editor,
					new DocumentStateTracker<TypeScriptIndentEngine> (new TypeScriptIndentEngine (policy, textStylePolicy), Editor)
				);*/
			}

			InitTracker ();
			Document.Editor.Paste += HandleTextPaste;
		}

		#region Sharing the tracker

		void InitTracker ()
		{
			stateTracker = new DocumentStateTracker<TypeScriptIndentEngine> (new TypeScriptIndentEngine (policy, textStylePolicy), Editor);
		}

		internal DocumentStateTracker<TypeScriptIndentEngine> StateTracker { get { return stateTracker; } }

		#endregion

		public bool DoInsertTemplate ()
		{
			string word = CodeTemplate.GetWordBeforeCaret (Editor);
			foreach (CodeTemplate template in CodeTemplateService.GetCodeTemplates (TypeScriptFormatter.MimeType))
				if (template.Shortcut == word) 
					return true;

			return false;
		}

		int lastInsertedSemicolon = -1;

		public override bool KeyPress (Gdk.Key key, char keyChar, Gdk.ModifierType modifier)
		{
			bool skipFormatting = StateTracker.Engine.IsInsideOrdinaryCommentOrString;
			bool isSomethingSelected = Editor.IsSomethingSelected;
			var editorMode = Editor.CurrentMode;
			var isTabReindent = DefaultSourceEditorOptions.Instance.TabIsReindent;
			var document = Editor.Document;

			cursorPositionBeforeKeyPress = Editor.Caret.Offset;

			if (key == Gdk.Key.BackSpace && Editor.Caret.Offset == lastInsertedSemicolon) {
				Editor.Document.Undo ();
				lastInsertedSemicolon = -1;
				return false;
			}

			lastInsertedSemicolon = -1;

			var smartSemicolonPlacement = PropertyService.Get ("SmartSemicolonPlacement", false);
			if (keyChar == ';' && smartSemicolonPlacement &&
			    !(editorMode is TextLinkEditMode) && !DoInsertTemplate () &&
			    !isSomethingSelected) {

				bool retval = base.KeyPress (key, keyChar, modifier);

				DocumentLine currLine = document.GetLine (Editor.Caret.Line);
				string text = document.GetTextAt (currLine);
				if (text.EndsWith (";") || text.Trim ().StartsWith ("for"))
					return retval;

				int guessedOffset = GuessSemicolonInsertionOffset (Editor, currLine, Editor.Caret.Offset);
				if (guessedOffset != Editor.Caret.Offset) {
					using (var undo = Editor.OpenUndoGroup ()) {
						Editor.Remove (Editor.Caret.Offset - 1, 1);
						Editor.Caret.Offset = guessedOffset;
						lastInsertedSemicolon = Editor.Caret.Offset + 1;

						retval = base.KeyPress (key, keyChar, modifier);
					}
				}

				return retval;
			}
			
			if (key == Gdk.Key.Tab) {
				stateTracker.UpdateEngine ();

				if (stateTracker.Engine.IsInsideString && !isSomethingSelected) {
					/*
					var lexer = new CSharpCompletionEngineBase.MiniLexer (textEditorData.Document.GetTextAt (0, textEditorData.Caret.Offset));
					lexer.Parse ();
					if (lexer.IsInString) {
						textEditorData.InsertAtCaret ("\\t");
						return false;
					}
					*/
				}
			}

			if (key == Gdk.Key.Tab && isTabReindent && !CompletionWindowManager.IsVisible &&
			    !(editorMode is TextLinkEditMode) && !DoInsertTemplate () &&
			    !isSomethingSelected) {

				int cursor = Editor.Caret.Offset;
				if (cursor >= 1) {

					if (Editor.Caret.Column > 1) {
						int delta = cursor - cursorPositionBeforeKeyPress;
						if (delta < 2 && delta > 0) {
							Editor.Remove (cursor - delta, delta);
							Editor.Caret.Offset = cursor - delta;
							document.CommitLineUpdate (Editor.Caret.Line);
						}
					}

					stateTracker.UpdateEngine ();
					DoReSmartIndent ();
				}

				return false;
			}

			//
			// Smart indent logic.
			//

			if (Editor.Options.IndentStyle == IndentStyle.Smart || Editor.Options.IndentStyle == IndentStyle.Virtual) {
				bool retval;

				// capture some of the current state
				int oldBufLen = Editor.Length;
				int oldLine = Editor.Caret.Line + 1;
				bool hadSelection = Editor.IsSomethingSelected;
				bool reIndent = false;

				// pass through to the base class, which actually inserts the character
				// and calls HandleCodeCompletion etc to handles completion
				using (var undo = Editor.OpenUndoGroup ())
					DoPreInsertionSmartIndent (key);

				bool automaticReindent;
				using (var undo = Editor.OpenUndoGroup ()) {

					retval = base.KeyPress (key, keyChar, modifier);

					// handle inserted characters
					if (Editor.Caret.Offset <= 0 || isSomethingSelected)
						return retval;

					lastCharInserted = TranslateKeyCharForIndenter (key, keyChar, Editor.GetCharAt (Editor.Caret.Offset - 1));
					if (lastCharInserted == '\0')
						return retval;

					stateTracker.UpdateEngine ();

					if (key == Gdk.Key.Return && modifier == Gdk.ModifierType.ControlMask)
						FixLineStart (Editor, stateTracker, Editor.Caret.Line + 1);
					else
						if (!(oldLine == Editor.Caret.Line + 1 && lastCharInserted == '\n') && (oldBufLen != Editor.Length || lastCharInserted != '\0'))
							DoPostInsertionSmartIndent (lastCharInserted, hadSelection, out reIndent);

					// reindent the line after the insertion, if needed
					// N.B. if the engine says we need to reindent, make sure that it's because a char was 
					// inserted rather than just updating the stack due to moving around
					stateTracker.UpdateEngine ();

					automaticReindent = (stateTracker.Engine.NeedsReindent && lastCharInserted != '\0');
					if (key == Gdk.Key.Return && (reIndent || automaticReindent))
						DoReSmartIndent ();
				}

				if (key != Gdk.Key.Return && (reIndent || automaticReindent)) {
					using (var undo = Editor.OpenUndoGroup ()) {
						DoReSmartIndent ();
					}
				}

				if (!skipFormatting && keyChar == '}') {
					using (var undo = Editor.OpenUndoGroup ()) {
						RunFormatter (new DocumentLocation (Editor.Caret.Location.Line, Editor.Caret.Location.Column));
					}
				}

				stateTracker.UpdateEngine ();
				lastCharInserted = '\0';
				return retval;
			}

			if (Editor.Options.IndentStyle == IndentStyle.Auto && isTabReindent && key == Gdk.Key.Tab) {
				bool retval = base.KeyPress (key, keyChar, modifier);
				DoReSmartIndent ();
				return retval;
			}

			//pass through to the base class, which actually inserts the character
			//and calls HandleCodeCompletion etc to handles completion
			var result = base.KeyPress (key, keyChar, modifier);

			if (!skipFormatting && keyChar == '}')
				RunFormatter (new DocumentLocation (Editor.Caret.Location.Line, Editor.Caret.Location.Column));

			return result;
		}

		// 
		// TODO - It seems the support for properties varies
		// depending on the ECMA script requested version.
		// Fix the code here.
		// 
		public static int GuessSemicolonInsertionOffset (TextEditorData data, DocumentLine currLine, int caretOffset)
		{
			int lastNonWsOffset = caretOffset;
			char lastNonWsChar = '\0';
			int max = currLine.EndOffset;

			// PORT NOTE: Honestly, this looks like a hack.
			if (caretOffset - 2 >= currLine.Offset && data.Document.GetCharAt (caretOffset - 2) == ')')
				return caretOffset;

			int end = caretOffset;
			while (end > 1 && Char.IsWhiteSpace (data.GetCharAt (end)))
				end--;

			int end2 = end;
			while (end2 > 1 && Char.IsLetter (data.GetCharAt (end2 - 1)))
				end2--;

			if (end != end2) {
				string token = data.GetTextBetween (end2, end + 1);
				// guess property context
				if (token == "get" || token == "set")
					return caretOffset;
			}

			bool isInDQuotedString = false, isInSQuotedString = false;
			bool isInLineComment = false , isInBlockComment = false;

			// Compare the first check against the original one.
			for (int pos = caretOffset; pos < max; pos++) {
				if (pos == caretOffset) {
					if (isInDQuotedString || isInSQuotedString || isInLineComment || isInBlockComment)
						return pos;
				}

				char ch = data.Document.GetCharAt (pos);
				switch (ch) {
				case '/':
					if (isInBlockComment) {
						if (pos > 0 && data.Document.GetCharAt (pos - 1) == '*') 
							isInBlockComment = false;

					} else if (!(isInDQuotedString || isInSQuotedString) && pos + 1 < max) {
						char nextChar = data.Document.GetCharAt (pos + 1);
						if (nextChar == '/') {
							isInLineComment = true;
							return lastNonWsOffset;
						}
						if (!isInLineComment && nextChar == '*') {
							isInBlockComment = true;
							return lastNonWsOffset;
						}
					}

					break;
				case '\\':
					if (isInSQuotedString || isInDQuotedString)
						pos++;

					break;
				case '"':
					if (!(isInSQuotedString || isInLineComment || isInBlockComment)) {
						if (isInDQuotedString && pos + 1 < max && data.Document.GetCharAt (pos + 1) == '"')
							pos++;
						else
							isInDQuotedString = !isInDQuotedString;
					}

					break;
				case '\'':
					if (!(isInDQuotedString || isInLineComment || isInBlockComment)) 
						isInSQuotedString = !isInSQuotedString;

					break;
				}

				if (!Char.IsWhiteSpace (ch)) {
					lastNonWsOffset = pos;
					lastNonWsChar = ch;
				}
			}

			// if the line ends with ';' the line end is not the correct place for a new semicolon.
			if (lastNonWsChar == ';')
				return caretOffset;

			return lastNonWsOffset;
		}

		static char TranslateKeyCharForIndenter (Gdk.Key key, char keyChar, char docChar)
		{
			switch (key) {
			case Gdk.Key.Return:
			case Gdk.Key.KP_Enter:
				return '\n';
			case Gdk.Key.Tab:
				return '\t';
			default:
				if (docChar == keyChar)
					return keyChar;
				break;
			}
			return '\0';
		}


		// removes "\s*\+\s*" patterns (used for special behaviour inside strings)
		void HandleStringConcatinationDeletion (int start, int end)
		{
			if (start < 0 || end >= Editor.Length || Editor.IsSomethingSelected)
				return;

			char ch = Editor.GetCharAt (start);
			if (ch == '"') {
				int sgn = Math.Sign (end - start);
				bool foundPlus = false;
				for (int max = start + sgn; max != end && max >= 0 && max < Editor.Length; max += sgn) {
					ch = Editor.GetCharAt (max);

					if (Char.IsWhiteSpace (ch))
						continue;

					if (ch == '+') {
						if (foundPlus)
							break;

						foundPlus = true;
					} else if (ch == '"') {
						if (!foundPlus)
							break;

						if (sgn < 0) {
							Editor.Remove (max, start - max);
							Editor.Caret.Offset = max + 1;
						} else {
							Editor.Remove (start + sgn, max - start);
							Editor.Caret.Offset = start;
						}

						break;

					} else
						break;
				}
			}
		}

		void DoPreInsertionSmartIndent (Gdk.Key key)
		{
			switch (key) {
			case Gdk.Key.BackSpace:
				stateTracker.UpdateEngine ();
				HandleStringConcatinationDeletion (Editor.Caret.Offset - 1, 0);
				break;
			case Gdk.Key.Delete:
				stateTracker.UpdateEngine ();
				HandleStringConcatinationDeletion (Editor.Caret.Offset, Editor.Length);
				break;
			}
		}

		// Special handling for certain characters just inserted , for comments etc
		void DoPostInsertionSmartIndent (char charInserted, bool hadSelection, out bool reIndent)
		{
			stateTracker.UpdateEngine ();
			reIndent = false;

			switch (charInserted) {
			case '}':
			case ';':
				reIndent = true;
				break;
			case '\n':
				if (FixLineStart (Editor, stateTracker, stateTracker.Engine.LineNumber)) 
					return;

				// newline always reindents unless it's had special handling
				reIndent = true;
				break;
			}
		}

		public static bool FixLineStart (TextEditorData textEditorData, DocumentStateTracker<TypeScriptIndentEngine> stateTracker, int lineNumber)
		{
			if (lineNumber <= DocumentLocation.MinLine)
				return false;

			var document = textEditorData.Document;

			var line = textEditorData.Document.GetLine (lineNumber);
			if (line == null)
				return false;

			var prevLine = document.GetLine (lineNumber - 1);
			if (prevLine == null)
				return false;

			string trimmedPreviousLine = document.GetTextAt (prevLine).TrimStart ();

			//xml doc comments
			//check previous line was a doc comment
			//check there's a following line?
			if (trimmedPreviousLine.StartsWith ("///")) {
				if (textEditorData.GetTextAt (line.Offset, line.Length).TrimStart ().StartsWith ("///"))
					return false;

				//check that the newline command actually inserted a newline
				textEditorData.EnsureCaretIsNotVirtual ();

				int insertionPoint = line.Offset + line.GetIndentation (document).Length;
				string nextLine = document.GetTextAt (document.GetLine (lineNumber + 1)).TrimStart ();

				if (trimmedPreviousLine.Length > "///".Length || nextLine.StartsWith ("///")) {
					textEditorData.Insert (insertionPoint, "/// ");
					return true;
				}
			} else if (stateTracker.Engine.IsInsideMultiLineComment) {
				//multi-line comments
				if (textEditorData.GetTextAt (line.Offset, line.Length).TrimStart ().StartsWith ("*"))
					return false;

				textEditorData.EnsureCaretIsNotVirtual ();
				string commentPrefix = string.Empty;

				if (trimmedPreviousLine.StartsWith ("* "))
					commentPrefix = "* ";
				else if (trimmedPreviousLine.StartsWith ("/**") || trimmedPreviousLine.StartsWith ("/*"))
					commentPrefix = " * ";
				else if (trimmedPreviousLine.StartsWith ("*"))
					commentPrefix = "*";

				int indentSize = line.GetIndentation (document).Length;
				var insertedText = prevLine.GetIndentation (document) + commentPrefix;

				textEditorData.Replace (line.Offset, indentSize, insertedText);
				textEditorData.Caret.Offset = line.Offset + insertedText.Length;
				return true;

			} else if (stateTracker.Engine.IsInsideString) {
				/* FIXME- ADD ME
				var lexer = new CSharpCompletionEngineBase.MiniLexer (textEditorData.Document.GetTextAt (0, prevLine.EndOffset));
				lexer.Parse ();
				if (!lexer.IsInString)
					return false;
					*/ 

				textEditorData.EnsureCaretIsNotVirtual ();
				textEditorData.Insert (prevLine.Offset + prevLine.Length, "\" +");

				int indentSize = line.GetIndentation (document).Length;
				var insertedText = prevLine.GetIndentation (document) + (trimmedPreviousLine.StartsWith ("\"") ? "" : "\t") + "\"";

				textEditorData.Replace (line.Offset, indentSize, insertedText);
				return true;
			}

			return false;
		}

		//does re-indenting and cursor positioning
		void DoReSmartIndent ()
		{
			DoReSmartIndent (Editor.Caret.Offset);
		}

		void DoReSmartIndent (int cursor)
		{
			if (stateTracker.Engine.LineBeganInsideMultiLineComment)
				return;

			var document = Editor.Document;

			string newIndent = String.Empty;
			var line = document.GetLineByOffset (cursor);

			// Get context to the end of the line w/o changing the main engine's state
			var ctx = (TypeScriptIndentEngine)stateTracker.Engine.Clone ();
			for (int i = cursor; i < line.EndOffset; i++)
				ctx.Push (document.GetCharAt (i));
				
			int pos = line.Offset;
			string currIndent = line.GetIndentation (document);
			int nlwsp = currIndent.Length;
			int offset = cursor > pos + nlwsp ? cursor - (pos + nlwsp) : 0;

			if (!stateTracker.Engine.LineBeganInsideMultiLineComment ||
			    (nlwsp < line.LengthIncludingDelimiter && document.GetCharAt (line.Offset + nlwsp) == '*')) {
				// Possibly replace the indent
				newIndent = ctx.ThisLineIndent;
				int newIndentLength = newIndent.Length;

				if (newIndent != currIndent) {
					if (CompletionWindowManager.IsVisible) {
						// GOD: Why do we need to have completion stuff in the simple indent thing?

						//if (pos < CompletionWindowManager.CodeCompletionContext.TriggerOffset)
							//CompletionWindowManager.CodeCompletionContext.TriggerOffset -= nlwsp;
					}

					newIndentLength = Editor.Replace (pos, nlwsp, newIndent);
					document.CommitLineUpdate (Editor.Caret.Line);

					// Engine state is now invalid
					stateTracker.ResetEngineToPosition (pos);
				}

				pos += newIndentLength;
			} else {
				pos += currIndent.Length;
			}

			pos += offset;

			Editor.FixVirtualIndentation ();
		}
	}
}
