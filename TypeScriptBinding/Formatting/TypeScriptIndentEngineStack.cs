//
// TypeScriptIndentEngineStack.cs
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
using System.Text;

namespace MonoDevelop.TypeScriptBinding.Formatting
{
	public partial class TypeScriptIndentEngine : ICloneable {
		public enum Inside {
			Empty              = 0,
			
			MultiLineComment   = (1 << 1),
			LineComment        = (1 << 2),
			DocComment         = (1 << 11),
			Comment            = (MultiLineComment | LineComment | DocComment),

			DoubleQuotedString = (1 << 4),
			SingleQuotedString = (1 << 5),
			String             = (DoubleQuotedString | SingleQuotedString),

			ParenList          = (1 << 7),
			
			FoldedStatement    = (1 << 8),
			Block              = (1 << 9),
			Case               = (1 << 10),
			
			FoldedOrBlock      = (FoldedStatement | Block),
			FoldedBlockOrCase  = (FoldedStatement | Block | Case)
		}
		
		private class IndentStack : ICloneable {
			readonly static int InitialCapacity = 16;
			
			struct Node {
				public Inside inside;
				public string keyword;
				public string indent;
				public int nSpaces;
				public int lineNumber;
				
				public override string ToString ()
				{
					return string.Format ("[Node: inside={0}, keyword={1}, indent={2}, nSpaces={3}, lineNr={4}]", inside, keyword, indent, nSpaces, lineNumber);
				}
			};
			
			Node[] stack;
			int count;
			TypeScriptIndentEngine engine;
			
			public IndentStack (TypeScriptIndentEngine engine) : this (engine, InitialCapacity)
			{
			}
			
			public IndentStack (TypeScriptIndentEngine engine, int capacity)
			{
				this.engine = engine;
				if (capacity < InitialCapacity)
					capacity = InitialCapacity;
				
				this.stack = new Node [capacity];
				this.count = 0;
			}
			
			public bool IsEmpty {
				get { return count == 0; }
			}
			
			public int Count {
				get { return count; }
			}
			
			public object Clone ()
			{
				IndentStack clone = new IndentStack (engine, stack.Length);
				
				clone.stack = (Node[]) stack.Clone ();
				clone.count = count;
				
				return clone;
			}
			
			public void Reset ()
			{
				for (int i = 0; i < count; i++) {
					stack[i].keyword = null;
					stack[i].indent = null;
				}
				
				count = 0;
			}
			
			public void Push (Inside inside, string keyword, int lineNumber, int nSpaces)
			{
				StringBuilder indentBuilder;
				int sp = count - 1;
				Node node;
				int n = 0;
				
				indentBuilder = new StringBuilder ();
				if ((inside & Inside.ParenList) != 0) {
					if (count > 0 && stack[sp].inside == inside) {

						while (sp >= 0) {
							if ((stack[sp].inside & Inside.FoldedOrBlock) != 0)
								break;
							sp--;
						}

						if (sp >= 0) {
							indentBuilder.Append (stack[sp].indent);
							if (stack[sp].lineNumber == lineNumber)
								n = stack[sp].nSpaces;
						}
					} else {
						while (sp >= 0) {
							if ((stack[sp].inside & Inside.FoldedBlockOrCase) != 0) {
								indentBuilder.Append (stack[sp].indent);
								break;
							}
							
							sp--;
						}
					}

					if (nSpaces - n <= 0) {
						indentBuilder.Append ('\t');
					} else {
						indentBuilder.Append (' ', nSpaces - n);
					}
				} else if (inside == Inside.MultiLineComment) {
					if (count > 0) {
						indentBuilder.Append (stack[sp].indent);
						if (stack[sp].lineNumber == lineNumber)
							n = stack[sp].nSpaces;
					}
					
					indentBuilder.Append (' ', nSpaces - n);
				} else if (inside == Inside.Case) {
					while (sp >= 0) {
						if ((stack[sp].inside & Inside.FoldedOrBlock) != 0) {
							indentBuilder.Append (stack[sp].indent);
							break;
						}
						
						sp--;
					}
					
					if (engine.policy.IndentSwitchBody)
						indentBuilder.Append ('\t');
					
					nSpaces = 0;
				} else if ((inside & (Inside.FoldedOrBlock)) != 0) {
					while (sp >= 0) {
						if ((stack[sp].inside & Inside.FoldedBlockOrCase) != 0) {
							indentBuilder.Append (stack[sp].indent);
							break;
						}
						
						sp--;
					}
					
					Inside parent = count > 0 ? stack[count - 1].inside : Inside.Empty;
					
					// This is a workaround to make anonymous methods indent nicely
					if (parent == Inside.ParenList)
						stack[count - 1].indent = indentBuilder.ToString ();
					
					if (inside == Inside.FoldedStatement) {
						indentBuilder.Append ('\t');
					} else if (inside == Inside.Block) {
						if (parent != Inside.Case || nSpaces != -1)
							indentBuilder.Append ('\t');
					}
					
					nSpaces = 0;
				} else if ((inside & (Inside.String)) != 0) {
					// if these fold, do not indent
					nSpaces = 0;
					
					//pop regions back out
					if (keyword == "region" || keyword == "endregion") {
						for (; sp >= 0; sp--) {
							if ((stack[sp].inside & Inside.FoldedBlockOrCase) != 0) {
								indentBuilder.Append (stack[sp].indent);
								break;
							}
						}
					}
				} else if (inside == Inside.LineComment || inside == Inside.DocComment) {
					// can't actually fold, but we still want to push it onto the stack
					nSpaces = 0;
				} else {
					// not a valid argument?
					throw new ArgumentOutOfRangeException ();
				}
				
				node.indent = indentBuilder.ToString ();
				node.keyword = keyword;
				node.nSpaces = nSpaces;
				node.lineNumber = lineNumber;
				node.inside = inside;
				
				if (count == stack.Length)
					Array.Resize <Node> (ref stack, 2 * count);
				
				stack[count++] = node;
			}
			
			public void Push (Inside inside, string keyword, int lineNumber, int nSpaces, string indent)
			{
				Node node = new Node ();

				node.indent = indent;
				node.keyword = keyword;
				node.nSpaces = nSpaces;
				node.lineNumber = lineNumber;
				node.inside = inside;
				
				if (count == stack.Length)
					Array.Resize <Node> (ref stack, 2 * count);
				
				stack[count++] = node;
			}
			
			public void Pop ()
			{
				if (count == 0)
					throw new InvalidOperationException ();
				
				int sp = count - 1;
				stack[sp].keyword = null;
				stack[sp].indent = null;
				count = sp;
			}
			
			public Inside PeekInside (int up)
			{
				if (up < 0)
					throw new ArgumentOutOfRangeException ();
				
				if (up >= count)
					return Inside.Empty;
				
				return stack[count - up - 1].inside;
			}
			
			public string PeekKeyword (int up)
			{
				if (up < 0)
					throw new ArgumentOutOfRangeException ();
				
				if (up >= count)
					return String.Empty;
				
				return stack[count - up - 1].keyword;
			}
			
			public string PeekIndent (int up)
			{
				if (up < 0)
					throw new ArgumentOutOfRangeException ();
				
				if (up >= count)
					return String.Empty;
				
				return stack[count - up - 1].indent;
			}
			
			public int PeekLineNumber (int up)
			{
				if (up < 0)
					throw new ArgumentOutOfRangeException ();
				
				if (up >= count)
					return -1;
				
				return stack[count - up - 1].lineNumber;
			}
		}
	}
}
