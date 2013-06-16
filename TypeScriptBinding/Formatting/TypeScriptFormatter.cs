// 
// CSharpFormatter.cs
//  
// Author:
//       Mike Krüger <mkrueger@novell.com>
//       Carlos Alberto Cortez <calberto.cortez@gmail.com>
// 
// Copyright (c) 2009 Novell, Inc (http://www.novell.com)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;

using Mono.TextEditor;
using MonoDevelop.Ide.Gui.Content;
using MonoDevelop.Projects.Policies;
using System.Linq;
using MonoDevelop.Ide.CodeFormatting;
using MonoDevelop.Core;

namespace MonoDevelop.TypeScriptBinding.Formatting
{
	public class TypeScriptFormatter : AbstractAdvancedFormatter
	{
		static internal readonly string MimeType = "text/x-typescript";

		public override bool SupportsOnTheFlyFormatting { get { return false; } }

		public override bool SupportsCorrectingIndent { get { return true; } }

		public override void CorrectIndenting (PolicyContainer policyParent, IEnumerable<string> mimeTypeChain, 
			TextEditorData data, int line)
		{
			var lineSegment = data.Document.GetLine (line);
			if (lineSegment == null)
				return;

			var policy = policyParent.Get<TypeScriptFormattingPolicy> (mimeTypeChain);
			var textPolicy = policyParent.Get<TextStylePolicy> (mimeTypeChain);

			var tracker = new DocumentStateTracker<TypeScriptIndentEngine> (new TypeScriptIndentEngine (policy, textPolicy), data);
			tracker.UpdateEngine (lineSegment.Offset);
			for (int i = lineSegment.Offset; i < lineSegment.Offset + lineSegment.Length; i++)
				tracker.Engine.Push (data.Document.GetCharAt (i));

			string currIndent = lineSegment.GetIndentation (data.Document);

			int nlwsp = currIndent.Length;
			if (!tracker.Engine.LineBeganInsideMultiLineComment ||
			    (nlwsp < lineSegment.LengthIncludingDelimiter && data.Document.GetCharAt (lineSegment.Offset + nlwsp) == '*')) {

				// Possibly replace the indent
				string newIndent = tracker.Engine.ThisLineIndent;
				if (newIndent != currIndent) 
					data.Replace (lineSegment.Offset, nlwsp, newIndent);
			}

			tracker.Dispose ();
		}

		public override void OnTheFlyFormat (MonoDevelop.Ide.Gui.Document doc, int startOffset, int endOffset)
		{
			// FIXME - Add this thing.
			//OnTheFlyFormatter.Format (doc, startOffset, endOffset);
		}

		public string FormatText (TypeScriptFormattingPolicy policy, TextStylePolicy textPolicy, string mimeType, string input, int startOffset, int endOffset)
		{
			return input;

			/*
			var data = new TextEditorData ();
			data.Document.SuppressHighlightUpdate = true;
			data.Document.MimeType = mimeType;
			data.Document.FileName = "toformat.ts";

			if (textPolicy != null) {
				data.Options.TabsToSpaces = textPolicy.TabsToSpaces;
				data.Options.TabSize = textPolicy.TabWidth;
				data.Options.IndentationSize = textPolicy.IndentWidth;
				data.Options.IndentStyle = textPolicy.RemoveTrailingWhitespace ? IndentStyle.Virtual : IndentStyle.Smart;
			}

			data.Text = input;

			var parser = new CSharpParser ();
			var compilationUnit = parser.Parse (data);
			bool hadErrors = parser.HasErrors;
			
			if (hadErrors) {
				//				foreach (var e in parser.ErrorReportPrinter.Errors)
				//					Console.WriteLine (e.Message);
				return input.Substring (startOffset, Math.Max (0, Math.Min (endOffset, input.Length) - startOffset));
			}

			var originalVersion = data.Document.Version;

			var textEditorOptions = data.CreateNRefactoryTextEditorOptions ();
			var formattingVisitor = new AstFormattingVisitor (
				policy.CreateOptions (),
				data.Document,
				textEditorOptions
			) {
				HadErrors = hadErrors,
				FormattingMode = FormattingMode.Intrusive
			};
			compilationUnit.AcceptVisitor (formattingVisitor);
			try {
				formattingVisitor.ApplyChanges (startOffset, endOffset - startOffset);
			} catch (Exception e) {
				LoggingService.LogError ("Error in code formatter", e);
				return input.Substring (startOffset, Math.Max (0, Math.Min (endOffset, input.Length) - startOffset));
			}

			// check if the formatter has produced errors
			parser = new CSharpParser ();
			parser.Parse (data);
			if (parser.HasErrors) {
				LoggingService.LogError ("C# formatter produced source code errors. See console for output.");
				return input.Substring (startOffset, Math.Max (0, Math.Min (endOffset, input.Length) - startOffset));
			}

			var currentVersion = data.Document.Version;

			string result = data.GetTextBetween (startOffset, originalVersion.MoveOffsetTo (currentVersion, endOffset, ICSharpCode.NRefactory.Editor.AnchorMovementType.Default));
			data.Dispose ();
			return result;
			*/
		}

		public override string FormatText (PolicyContainer policyParent, IEnumerable<string> mimeTypeChain, string input, int startOffset, int endOffset)
		{
			var policy = policyParent.Get<TypeScriptFormattingPolicy> (mimeTypeChain);
			var textPolicy = policyParent.Get<TextStylePolicy> (mimeTypeChain);

			return FormatText (policy, textPolicy, mimeTypeChain.First (), input, startOffset, endOffset);

		}
	}
}
