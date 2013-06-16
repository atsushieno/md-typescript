// 
// TypeScriptFormattingOptionsFactory.cs
//  
// Author:
//      Carlos Alberto Cortez <calberto.cortez@gmail.com>
//  
// Copyright (c) 2013 Carlos Alberto Cortez
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

namespace MonoDevelop.TypeScriptBinding.Formatting
{
	//
	// This a class modeled after the NRefactory one.
	//
	public static class TypeScriptFormattingOptionsFactory
	{
		public static TypeScriptFormattingOptions CreateEmpty ()
		{
			return new TypeScriptFormattingOptions ();
		}

		public static TypeScriptFormattingOptions CreateDefault ()
		{
			return new TypeScriptFormattingOptions () {
				IndentModuleBody = true,
				IndentClassBody = true,
				IndentInterfaceBody = true,
				IndentEnumBody = true,
				IndentMethodBody = true,
				IndentPropertyBody = true,
				IndentBlocks = true,
				IndentSwitchBody = true,
				IndentCaseBody = true,
				IndentBreakStatements = true,

				ModuleBraceStyle = BraceStyle.NextLine,
				ClassBraceStyle = BraceStyle.NextLine,
				InterfaceBraceStyle = BraceStyle.NextLine,
				EnumBraceStyle = BraceStyle.NextLine,
				MethodBraceStyle = BraceStyle.NextLine,
				ConstructorBraceStyle = BraceStyle.NextLine,
				AnonymousMethodBraceStyle = BraceStyle.EndOfLine,

				PropertyGetBraceStyle = BraceStyle.EndOfLine,
				PropertySetBraceStyle = BraceStyle.EndOfLine,
				AllowPropertyGetBlockInline = true,
				AllowPropertySetBlockInline = true,

				ElseIfNewLinePlacement = NewLinePlacement.SameLine,
				CatchNewLinePlacement = NewLinePlacement.SameLine,
				FinallyNewLinePlacement = NewLinePlacement.SameLine,
				WhileNewLinePlacement = NewLinePlacement.SameLine,

				SpaceBeforeMethodCallParentheses = true,
				SpaceBeforeMethodDeclarationParentheses = true,
				SpaceBeforeConstructorDeclarationParentheses = true,
				SpaceAfterMethodCallParameterComma = true,
				SpaceAfterConstructorDeclarationParameterComma = true,

				SpaceBeforeNewParentheses = true,
				SpacesWithinNewParentheses = false,
				SpacesBetweenEmptyNewParentheses = false,
				SpaceBeforeNewParameterComma = false,
				SpaceAfterNewParameterComma = true,

				SpaceBeforeIfParentheses = true,
				SpaceBeforeWhileParentheses = true,
				SpaceBeforeForParentheses = true,
				SpaceBeforeCatchParentheses = true,
				SpaceBeforeSwitchParentheses = true,
				SpaceAroundAssignment = true,
				SpaceAroundLogicalOperator = true,
				SpaceAroundEqualityOperator = true,
				SpaceAroundRelationalOperator = true,
				SpaceAroundBitwiseOperator = true,
				SpaceAroundAdditiveOperator = true,
				SpaceAroundMultiplicativeOperator = true,
				SpaceAroundShiftOperator = true,
				SpacesWithinParentheses = false,
				SpaceWithinMethodCallParentheses = false,
				SpaceWithinMethodDeclarationParentheses = false,
				SpacesWithinIfParentheses = false,
				SpacesWithinWhileParentheses = false,
				SpacesWithinForParentheses = false,
				SpacesWithinCatchParentheses = false,
				SpacesWithinSwitchParentheses = false,
				SpacesWithinCastParentheses = false,
				SpacesWithinSizeOfParentheses = false,
				SpacesWithinTypeOfParentheses = false,
				SpaceBeforeConditionalOperatorCondition = true,
				SpaceAfterConditionalOperatorCondition = true,
				SpaceBeforeConditionalOperatorSeparator = true,
				SpaceAfterConditionalOperatorSeparator = true,

				SpacesWithinBrackets = false,
				SpacesBeforeBrackets = true,
				SpaceBeforeBracketComma = false,
				SpaceAfterBracketComma = true,
				
				SpaceBeforeForSemicolon = false,
				SpaceAfterForSemicolon = true,
				SpaceAfterTypecast = false,

				AlignEmbeddedIfStatements = true,
				PropertyFormatting = PropertyFormatting.AllowOneLine,
				SpaceBeforeMethodDeclarationParameterComma = false,
				SpaceAfterMethodDeclarationParameterComma = true,
				SpaceBeforeFieldDeclarationComma = false,
				SpaceAfterFieldDeclarationComma = true,
				SpaceBeforeLocalVariableDeclarationComma = false,
				SpaceAfterLocalVariableDeclarationComma = true,

				SpaceBeforeIndexerDeclarationBracket = true,
				SpaceWithinIndexerDeclarationBracket = false,
				SpaceBeforeIndexerDeclarationParameterComma = false,

				SpaceAfterIndexerDeclarationParameterComma = true,
				
				BlankLinesBeforeImports = 0,
				BlankLinesAfterImports = 1,

				BlankLinesBeforeFirstDeclaration = 0,
				BlankLinesBetweenTypes = 1,
				BlankLinesBetweenFields = 0,
				BlankLinesBetweenMembers = 1,

				KeepCommentsAtFirstColumn = true,
				ChainedMethodCallWrapping = Wrapping.DoNotChange,
				MethodCallArgumentWrapping = Wrapping.DoNotChange,
				NewLineAferMethodCallOpenParentheses = true,
				MethodCallClosingParenthesesOnNewLine = true,
				
				IndexerArgumentWrapping = Wrapping.DoNotChange,
				NewLineAferIndexerOpenBracket = false,
				IndexerClosingBracketOnNewLine = false,
				
				IfElseBraceForcement = BraceForcement.DoNotChange,
				ForBraceForcement = BraceForcement.DoNotChange,
				WhileBraceForcement = BraceForcement.DoNotChange,
			};
		}
	}
}

