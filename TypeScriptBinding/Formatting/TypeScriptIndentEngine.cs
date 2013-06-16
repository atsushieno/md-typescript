//
// TypeScriptIndentEngine.cs
//
// Author: Jeffrey Stedfast <fejj@novell.com>
//         Carlos Alberto Cortez <calberto.cortez@gmail.com>
//
// Copyright (C) 2007 Novell, Inc (http://www.novell.com)
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
using System.Linq;
using System.Text;

using MonoDevelop.Ide.Gui.Content;

namespace MonoDevelop.TypeScriptBinding.Formatting
{
	public partial class TypeScriptIndentEngine :
		ICloneable, IDocumentStateEngine
	{
		IndentStack stack;
		
		// Ponder: should linebuf be dropped in favor of a
		// 'wordbuf' and a 'int curLineLen'? No real need to
		// keep a full line buffer.
		StringBuilder linebuf;
		
		string keyword;
		
		string currIndent;
		
		Inside beganInside;
		
		bool needsReindent;
		bool popVerbatim;
		bool canBeLabel;
		bool isEscaped;
		
		int firstNonLwsp;
		int lastNonLwsp;
		int wordStart;
		
		char lastChar;
		
		// previous char in the line
		char pc;
		
		// last significant (real) char in the line
		// (e.g. non-whitespace, not in a comment, etc)
		char rc;
		
		// previous last significant (real) char in the line
		char prc;
		
		int currLineNumber;
		int cursor;
		TypeScriptFormattingPolicy policy;
		TextStylePolicy textPolicy;

		//
		// Constructors
		//

		public TypeScriptIndentEngine (TypeScriptFormattingPolicy policy, TextStylePolicy textPolicy)
		{
			if (policy == null)
				throw new ArgumentNullException ("policy");

			if (textPolicy == null)
				throw new ArgumentNullException ("textPolicy");

			this.policy = policy;
			this.textPolicy = textPolicy;
			stack = new IndentStack (this);
			linebuf = new StringBuilder ();
			Reset ();
		}

		//
		// Properties
		//

		public int Position {
			get { return cursor; }
		}
		
		public int LineNumber {
			get { return currLineNumber; }
		}
		
		public int LineOffset {
			get { return linebuf.Length; }
		}
		
		public bool NeedsReindent {
			get { return needsReindent; }
		}
		
		public int StackDepth {
			get { return stack.Count; }
		}
		
		public bool IsInsideMultiLineComment {
			get { return stack.PeekInside (0) == Inside.MultiLineComment; }
		}
		
		public bool IsInsideDocLineComment {
			get { return stack.PeekInside (0) == Inside.DocComment; }
		}
		
		public bool LineBeganInsideMultiLineComment {
			get { return beganInside == Inside.MultiLineComment; }
		}
		
		public bool IsInsideOrdinaryCommentOrString {
			get { return (stack.PeekInside (0) & (Inside.LineComment | Inside.MultiLineComment | Inside.String)) != 0; }
		}
		
		public bool IsInsideComment {
			get { return (stack.PeekInside (0) & (Inside.LineComment | Inside.MultiLineComment | Inside.DocComment)) != 0; }
		}

		public bool IsInsideString {
			get { return (stack.PeekInside (0) & Inside.String) != 0; }
		}

		
		string TabsToSpaces (string indent)
		{
			if (indent == String.Empty)
				return String.Empty;
			
			var builder = new StringBuilder ();
			for (int i = 0; i < indent.Length; i++) {
				if (indent[i] == '\t')
					builder.Append (' ', textPolicy.IndentWidth);
				else
					builder.Append (indent[i]);
			}
			
			return builder.ToString ();
		}

		public string ThisLineIndent {
			get {
				if (textPolicy.TabsToSpaces)
					return TabsToSpaces (currIndent);
				
				return currIndent;
			}
		}
		
		public string NewLineIndent {
			get {
				if (textPolicy.TabsToSpaces)
					return TabsToSpaces (stack.PeekIndent (0));
				
				return stack.PeekIndent (0);
			}
		}
		
		// Methods
		
		// Resets the CSharpIndentEngine state machine
		public void Reset ()
		{
			stack.Reset ();

			linebuf.Length = 0;

			keyword = String.Empty;
			currIndent = String.Empty;

			needsReindent = false;
			popVerbatim = false;
			canBeLabel = true;
			isEscaped = false;

			firstNonLwsp = -1;
			lastNonLwsp = -1;
			wordStart = -1;

			prc = '\0';
			pc = '\0';
			rc = '\0';
			lastChar = '\0';
			currLineNumber = 1;
			cursor = 0;
		}
		
		// Clone the TypeScriptIndentEngine - useful if a consumer of this class wants
		// to test things w/o changing the real indent engine state
		public object Clone ()
		{
			var engine = new TypeScriptIndentEngine (policy, textPolicy);

			engine.stack = (IndentStack) stack.Clone ();
			engine.linebuf = new StringBuilder (linebuf.ToString (), linebuf.Capacity);
			
			engine.keyword = keyword;
			engine.currIndent = currIndent;
			
			engine.needsReindent = needsReindent;
			engine.popVerbatim = popVerbatim;
			engine.canBeLabel = canBeLabel;
			engine.isEscaped = isEscaped;
			
			engine.firstNonLwsp = firstNonLwsp;
			engine.lastNonLwsp = lastNonLwsp;
			engine.wordStart = wordStart;
			
			engine.prc = prc;
			engine.pc = pc;
			engine.rc = rc;
			
			engine.currLineNumber = currLineNumber;
			engine.cursor = cursor;
			
			return engine;
		}
		
		void TrimIndent ()
		{
			switch (stack.PeekInside (0)) {
			case Inside.FoldedStatement:
			case Inside.Block:
			case Inside.Case:
				if (currIndent == String.Empty)
					return;
				
				// chop off the last tab (e.g. unindent 1 level)
				currIndent = currIndent.Substring (0, currIndent.Length - 1);
				break;
			default:
				currIndent = stack.PeekIndent (0);
				break;
			}
		}
		
		// Check to see if @keyword is a "special" keyword - e.g. loop/if/else
		// constructs that we always want to indent if folded
		static bool KeywordIsSpecial (string keyword)
		{
			string[] specials = new string [] {
				"while",
				"for",
				"else",
				"if"
			};
			
			for (int i = 0; i < specials.Length; i++)
				if (keyword == specials[i])
					return true;
			
			return false;
		}
		
		static readonly string[] keywords = new string [] {
			"module",
			"interface",
			"class",
			"extends",
			"implements",
			"enum",
			"switch",
			"case",
			"while",
			"for",
			"do",
			"else",
			"if",
			"super",
			"this",
			"=",
			"return"
		};
		
		static readonly int maxKeywordLength = keywords.Max (word => word.Length);
		
		// Check to see if linebuf contains a keyword we're interested in (not all keywords)
		string WordIsKeyword ()
		{
			string str = linebuf.ToString (wordStart, Math.Min (linebuf.Length - wordStart, maxKeywordLength));
			
			for (int i = 0; i < keywords.Length; i++)
				if (str == keywords[i])
					return keywords[i];
			
			return null;
		}
		
		bool WordIsDefault ()
		{
			string str = linebuf.ToString (wordStart, linebuf.Length - wordStart).Trim ();
			return str == "default";
		}
		
		// directive keywords that we care about
		static string[] directiveKeywords = new string [] {"region", "endregion" };

		//
		// TODO: Does TypeScript even support regions?
		//
		string GetDirectiveKeyword (char currentChar)
		{
			if (!Char.IsWhiteSpace (currentChar) && currentChar != '\t' && currentChar != '\n')
				return null;

			string str = linebuf.ToString ().TrimStart ().Substring (1);
			
			if (str.Length == 0)
				return null;
			
			for (int i = 0; i < directiveKeywords.Length; i++) {
				if (directiveKeywords[i].StartsWith (str)) {
					if (str == directiveKeywords[i])
						return directiveKeywords[i];
					else
						return null;
				}
			}
			
			return String.Empty;
		}
		
		bool Folded2LevelsNonSpecial ()
		{
			return stack.PeekInside (0) == Inside.FoldedStatement &&
				stack.PeekInside (1) == Inside.FoldedStatement &&
				!KeywordIsSpecial (stack.PeekKeyword (0)) &&
				!KeywordIsSpecial (keyword);
		}

		// Maybe we need to check 'extends'/'implements' as keywords too?
		bool FoldedClassDeclaration ()
		{
			return stack.PeekInside (0) == Inside.FoldedStatement &&
				(keyword == "class" || keyword == "interface");
		}
		
		void PushFoldedStatement ()
		{
			string indent = null;
			
			// Note: nesting of folded statements stops after 2 unless a "special" folded
			// statement is introduced, in which case the cycle restarts
			//
			// Note: We also try to only fold class declarations once
			
			if (Folded2LevelsNonSpecial () || FoldedClassDeclaration ())
				indent = stack.PeekIndent (0);
			
			if (indent != null)
				stack.Push (Inside.FoldedStatement, keyword, currLineNumber, 0, indent);
			else
				stack.Push (Inside.FoldedStatement, keyword, currLineNumber, 0);
			
			keyword = String.Empty;
		}
		
		void PushSlash (Inside inside)
		{
			if ((inside & (Inside.String)) != 0)
				return;

			if (inside == Inside.LineComment) {
				stack.Pop (); // pop line comment
				stack.Push (Inside.DocComment, keyword, currLineNumber, 0);

			} else if (inside == Inside.MultiLineComment) {
				// check for end of multi-line comment block
				if (pc == '*') {
					// restore the keyword and pop the multiline comment
					keyword = stack.PeekKeyword (0);
					stack.Pop ();
				}

			} else {
				
				// FoldedStatement, Block, Attribute or ParenList
				// check for the start of a single-line comment
				if (pc == '/') {
					stack.Push (Inside.LineComment, keyword, currLineNumber, 0);
					
					// drop the previous '/': it belongs to this comment
					rc = prc;
				}
			}
		}
		
		void PushBackSlash (Inside inside)
		{
			// string and char literals can have \-escapes
			if ((inside & (Inside.String)) != 0)
				isEscaped = !isEscaped;
		}
		
		void PushStar (Inside inside)
		{
			if (pc != '/')
				return;
			
			// got a "/*" - might start a MultiLineComment
			if ((inside & (Inside.String | Inside.Comment)) != 0)
				return;
			
			// push a new multiline comment onto the stack
			int n = linebuf.Length - firstNonLwsp;

			stack.Push (Inside.MultiLineComment, keyword, currLineNumber, n);
			
			// drop the previous '/': it belongs to this comment block
			rc = prc;
		}
		
		void PushQuote (Inside inside)
		{
			Inside type;
			
			// ignore if in these
			if ((inside & (Inside.Comment | Inside.SingleQuotedString)) != 0)
				return;

			if (inside == Inside.DoubleQuotedString) {
				// check that it isn't escaped
				if (!isEscaped) {
					keyword = stack.PeekKeyword (0);
					stack.Pop ();
				}
			} else {
				// FoldedStatement, Block, Attribute or ParenList
				type = Inside.DoubleQuotedString;
				
				// push a new string onto the stack
				stack.Push (type, keyword, currLineNumber, 0);
			}
		}
		
		void PushSQuote (Inside inside)
		{
			if ((inside & (Inside.DoubleQuotedString | Inside.Comment)) != 0)
				return;
			
			if (inside == Inside.SingleQuotedString) {
				// check that it's not escaped
				if (isEscaped)
					return;
				
				keyword = stack.PeekKeyword (0);
				stack.Pop ();
				return;
			}

			// push a new char literal onto the stack 
			stack.Push (Inside.SingleQuotedString, keyword, currLineNumber, 0);
		}
	
		// LASTPOINT 2 - do we support labels?
		// if not, just wipe out the commented code above
		// (or was it commented already, in the original, anyway?)
		void PushColon (Inside inside)
		{
			if (inside != Inside.Block && inside != Inside.Case)
				return;
			
			// can't be a case/label if there's no preceeding text
			if (wordStart == -1)
				return;
			
			// goto-label or case statement
			if (keyword == "case" || keyword == "default") {
				// case (or default) statement
				if (stack.PeekKeyword (0) != "switch")
					return;
				
				if (inside == Inside.Case) {
					stack.Pop ();
					
					string newIndent = stack.PeekIndent (0);
					if (currIndent != newIndent) {
						currIndent = newIndent;
						needsReindent = true;
					}
				}
				
				if (!policy.IndentSwitchBody) {
					needsReindent = true;
					TrimIndent ();
				}
				
				stack.Push (Inside.Case, "switch", currLineNumber, 0);
			} else if (canBeLabel) {
				//GotoLabelIndentStyle style = FormattingProperties.GotoLabelIndentStyle;
				// FIXME TOO.
				//GotoLabelIndentStyle style = GotoLabelIndentStyle.OneLess;
				// indent goto labels as specified
				// FIXME
				/*switch (style) {
				case GotoLabelIndentStyle.LeftJustify:
					needsReindent = true;
			//		curIndent = " ";
					break;
				case GotoLabelIndentStyle.OneLess:
					needsReindent = true;
					TrimIndent ();
			//		curIndent += " ";
					break;
				default:
					break;
				}*/
				canBeLabel = false;
			} else if (pc == ':') {
				// :: operator, need to undo the "unindent label" operation we did for the previous ':'
				currIndent = stack.PeekIndent (0);
				needsReindent = true;
			}
		}
		
		void PushSemicolon (Inside inside)
		{
			if ((inside & (Inside.String | Inside.Comment)) != 0)
				return;
			
			if (inside == Inside.FoldedStatement) {
				// chain-pop folded statements
				while (stack.PeekInside (0) == Inside.FoldedStatement)
					stack.Pop ();
			}
			
			keyword = String.Empty;
		}
		
		void PushOpenParen (Inside inside)
		{
			int n = 1;
			
			if ((inside & (Inside.String | Inside.Comment)) != 0)
				return;
			
			// push a new paren list onto the stack
			if (firstNonLwsp != -1)
				n += linebuf.Length - firstNonLwsp;
			
			stack.Push (Inside.ParenList, keyword, currLineNumber, n);
			
			keyword = String.Empty;
		}
		
		void PushCloseParen (Inside inside)
		{
			if ((inside & (Inside.String | Inside.Comment)) != 0)
				return;
			
			if (inside != Inside.ParenList)				
				return;
			
			// pop this paren list off the stack
			keyword = stack.PeekKeyword (0);
			stack.Pop ();
		}
		
		void PushOpenBrace (Inside inside)
		{
			if ((inside & (Inside.String | Inside.Comment)) != 0)
				return;

			// push a new block onto the stack
			if (inside == Inside.FoldedStatement) {
				string pKeyword;
				
				if (firstNonLwsp == -1) {
					pKeyword = stack.PeekKeyword (0);
					stack.Pop ();
				} else {
					pKeyword = keyword;
				}
				
				while (true) {
					if (stack.PeekInside (0) != Inside.FoldedStatement)
						break;

					string kw = stack.PeekKeyword (0);
					stack.Pop ();
					TrimIndent ();

					if (!String.IsNullOrEmpty (kw)) {
						pKeyword = kw;
						break;
					}
				}
				
				if (firstNonLwsp == -1)
					currIndent = stack.PeekIndent (0);
				
				stack.Push (Inside.Block, pKeyword, currLineNumber, 0);
			} else if (inside == Inside.Case && (keyword == "default" || keyword == "case")) {
				if (currLineNumber == stack.PeekLineNumber (0) || firstNonLwsp == -1) {
					// e.g. "case 0: {" or "case 0:\n{"
					stack.Push (Inside.Block, keyword, currLineNumber, -1);
					
					if (firstNonLwsp == -1)
						TrimIndent ();
				} else {
					stack.Push (Inside.Block, keyword, currLineNumber, 0);
				}
			} else {
				stack.Push (Inside.Block, keyword, currLineNumber, 0);
// Destroys one lined expression block 'var s = "".Split (new char[] {' '});'
//				if (inside == Inside.ParenList)
//					TrimIndent ();
			}
			
			keyword = String.Empty;
			if (firstNonLwsp == -1)
				needsReindent = true;
		}
		
		void PushCloseBrace (Inside inside)
		{
			if ((inside & (Inside.String | Inside.Comment)) != 0)
				return;

			if (inside != Inside.Block && inside != Inside.Case) {
				if (stack.PeekInside (0) == Inside.FoldedStatement) {
					while (stack.PeekInside (0) == Inside.FoldedStatement) {
						stack.Pop ();
					}

					currIndent = stack.PeekIndent (0);
					keyword = stack.PeekKeyword (0);
					inside = stack.PeekInside (0);
				}

				if (inside != Inside.Block && inside != Inside.Case)
					return;
			}

			if (inside == Inside.Case) {
				currIndent = stack.PeekIndent (1);
				keyword = stack.PeekKeyword (0);
				inside = stack.PeekInside (1);
				stack.Pop ();
			}
			
			if (inside == Inside.ParenList) {
				currIndent = stack.PeekIndent (0);
				keyword = stack.PeekKeyword (0);
				inside = stack.PeekInside (0);
			}
			
			// pop this block off the stack
			keyword = stack.PeekKeyword (0);
			if (keyword != "case" && keyword != "default")
				keyword = String.Empty;

			stack.Pop ();

			while (stack.PeekInside (0) == Inside.FoldedStatement)
				stack.Pop ();

			if (firstNonLwsp == -1) {
				needsReindent = true;
				TrimIndent ();
			}
		}

		void PushNewLine (Inside inside)
		{
			top:
			switch (inside) {
			case Inside.MultiLineComment:
				// nothing to do
				break;
			case Inside.DocComment:
			case Inside.LineComment:
				// pop the line comment
				keyword = stack.PeekKeyword (0);
				stack.Pop ();

				inside = stack.PeekInside (0);
				goto top;
			case Inside.DoubleQuotedString:
				// Handle it somewhere else.
				break;
			case Inside.SingleQuotedString:
				// Handle it somewhere else.
				break;
			case Inside.ParenList:
				// nothing to do
				break;
			default:
				// Empty, FoldedStatement, and Block
				switch (rc) {
				case '\0':
					// nothing entered on this line
					break;
				case ':':
					canBeLabel = canBeLabel && inside != Inside.FoldedStatement;

					if ((keyword == "default" || keyword == "case") || canBeLabel)
						break;

					PushFoldedStatement ();
					break;
				case '[':
					// handled elsewhere
					break;
				case ']':
					// handled elsewhere
					break;
				case '(':
					// handled elsewhere
					break;
				case '{':
					// handled elsewhere
					break;
				case '}':
					// handled elsewhere
					break;
				case ';':
					// handled elsewhere
					break;
				case ',':
					// avoid indenting if we are in a list
					break;
				default:
					if (stack.PeekLineNumber (0) == currLineNumber) {
						// is this right? I don't remember why I did this...
						// (original note from Jeff)
						break;
					}

					if (inside == Inside.Block) {
						var k = stack.PeekKeyword (0);
						if (k == "enum" || k == "=")
							// just variable/value declarations
							break;
					}

					PushFoldedStatement ();
					break;
				}

				break;
			}
			
			linebuf.Length = 0;
			
			beganInside = stack.PeekInside (0);
			currIndent = stack.PeekIndent (0);
			
			canBeLabel = true;
			
			firstNonLwsp = -1;
			lastNonLwsp = -1;
			wordStart = -1;
			
			prc = '\0';
			pc = '\0';
			rc = '\0';
			
			currLineNumber++;
			cursor++;
		}
		
		void CheckForParentList ()
		{
			var after = stack.PeekInside (0);
			if ((after & Inside.ParenList) == Inside.ParenList && pc == '(') {
//				var indent = stack.PeekIndent (0);
				var kw = stack.PeekKeyword (0);
				var line = stack.PeekLineNumber (0);
				stack.Pop ();
				stack.Push (after, kw, line, 0);
			}
		}
		
		// This is the main logic of this class...
		public void Push (char c)
		{
			Inside inside, after;
			inside = stack.PeekInside (0);
			
			needsReindent = false;
			
			if ((inside & (Inside.String | Inside.Comment)) == 0 && wordStart != -1) {
				if (Char.IsWhiteSpace (c) || c == '(' || c == '{') {
					string tmp = WordIsKeyword ();
					if (tmp != null)
						keyword = tmp;

				} else if (c == ':' && WordIsDefault ()) {
					keyword = "default";
				}	
			} 
			
			//Console.WriteLine ("Pushing '{0}'/#{3}; wordStart = {1}; keyword = {2}", c, wordStart, keyword, (int)c);
			
			switch (c) {
			case '/':
				PushSlash (inside);
				break;
			case '\\':
				PushBackSlash (inside);
				break;
			case '*':
				PushStar (inside);
				break;
			case '"':
				PushQuote (inside);
				break;
			case '\'':
				PushSQuote (inside);
				break;
			case ':':
				PushColon (inside);
				break;
			case ';':
				PushSemicolon (inside);
				break;
			case '(':
				PushOpenParen (inside);
				break;
			case ')':
				PushCloseParen (inside);
				break;
			case '{':
				PushOpenBrace (inside);
				break;
			case '}':
				PushCloseBrace (inside);
				break;
			case '\r':
				CheckForParentList ();
				PushNewLine (inside);
				lastChar = c;
				return;
			case '\n':
				CheckForParentList ();
				
				if (lastChar == '\r')
					cursor++;
				else
					PushNewLine (inside);

				lastChar = c;
				return;
			default:
				break;
			}
			after = stack.PeekInside (0);
			
			if ((after & (Inside.String | Inside.Comment)) == 0) {
				if (!Char.IsWhiteSpace (c)) {
					if (firstNonLwsp == -1)
						firstNonLwsp = linebuf.Length;
					
					if (wordStart != -1 && c != ':' && Char.IsWhiteSpace (pc))
						// goto labels must be single word tokens
						canBeLabel = false;
					else if (wordStart == -1 && Char.IsDigit (c))
						// labels cannot start with a digit
						canBeLabel = false;
					
					lastNonLwsp = linebuf.Length;
					
					if (c != ':') {
						if (Char.IsWhiteSpace (pc) || rc == ':')
							wordStart = linebuf.Length;
						else if (pc == '\0')
							wordStart = 0;
					}
				}
			} else if (c != '\\' && (after & (Inside.String)) != 0) {
				// Note: PushBackSlash() will handle untoggling isEscaped if c == '\\'
				isEscaped = false;
			}
			
			pc = c;
			prc = rc;

			// Note: even though PreProcessor directive chars are insignificant, we do need to
			//       check for rc != '\\' at the end of a line.
			// (Original note from Jeff and the C# indent engine. Feel like removing it...)
			if ((inside & Inside.Comment) == 0 &&
			    (after & Inside.Comment) == 0 &&
			    !Char.IsWhiteSpace (c))
				rc = c;
			
			linebuf.Append (c);
			
			cursor++;
			lastChar = c;
		}
		
		public void Debug ()
		{
			Console.WriteLine ("\ncurLine = {0}", linebuf);
			Console.WriteLine ("curLineNr = {0}\ncursor = {1}\nneedsReindent = {2}",
			                   currLineNumber, cursor, needsReindent);
			Console.WriteLine ("stack:");
			for (int i = 0; i < stack.Count; i++) {
				switch (stack.PeekInside (i)) {
				case Inside.MultiLineComment:
					Console.WriteLine ("\t/* */ comment block");
					break;
				case Inside.LineComment:
					Console.WriteLine ("\t// comment");
					break;
				case Inside.DoubleQuotedString:
					Console.WriteLine ("\tdouble quoted string");
					break;
				case Inside.SingleQuotedString:
					Console.WriteLine ("\tsingle quoted string");
					break;
				case Inside.ParenList:
					Console.WriteLine ("\t( ) paren list");
					break;
				case Inside.FoldedStatement:
					if (stack.PeekKeyword (i) != String.Empty)
						Console.WriteLine ("\t{0}-statement", stack.PeekKeyword (i));
					else
						Console.WriteLine ("\tfolded statement?");
					break;
				case Inside.Case:
					Console.WriteLine ("\tcase statement");
					break;
				case Inside.Block:
					if (stack.PeekKeyword (i) != String.Empty)
						Console.WriteLine ("\t{0} {1} block", stack.PeekKeyword (i), "{ }");
					else
						Console.WriteLine ("\tmethod {0} block?", "{ }");
					break;
				}
			}
		}
	}
}
