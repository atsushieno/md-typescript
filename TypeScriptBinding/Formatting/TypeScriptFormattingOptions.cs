// 
// TypeScriptFormattingOptions.cs
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
using System.Reflection;
using System.Linq;

namespace MonoDevelop.TypeScriptBinding.Formatting
{
	public enum BraceStyle
	{
		DoNotChange,
		EndOfLine,
		EndOfLineWithoutSpace,
		NextLine,
		NextLineShifted,
		NextLineShifted2,
		BannerStyle
	}

	public enum BraceForcement
	{
		DoNotChange,
		RemoveBraces,
		AddBraces
	}

	public enum PropertyFormatting
	{
		AllowOneLine,
		ForceOneLine,
		ForceNewLine
	}

	public enum Wrapping {
		DoNotChange,
		DoNotWrap,
		WrapAlways,
		WrapIfTooLong
	}

	public enum NewLinePlacement {
		DoNotCare,
		NewLine,
		SameLine
	}

	public class TypeScriptFormattingOptions
	{
		public string Name {
			get;
			set;
		}

		public bool IsBuiltIn {
			get;
			set;
		}

		public TypeScriptFormattingOptions Clone ()
		{
			return (TypeScriptFormattingOptions)MemberwiseClone ();
		}

		#region Indentation
		public bool IndentModuleBody { // tested
			get;
			set;
		}

		public bool IndentClassBody { // tested
			get;
			set;
		}

		public bool IndentInterfaceBody { // tested
			get;
			set;
		}

		public bool IndentEnumBody { // tested
			get;
			set;
		}

		public bool IndentMethodBody { // tested
			get;
			set;
		}

		public bool IndentPropertyBody { // tested
			get;
			set;
		}

		public bool IndentBlocks { // tested
			get;
			set;
		}

		public bool IndentSwitchBody { // tested
			get;
			set;
		}

		public bool IndentCaseBody { // tested
			get;
			set;
		}

		public bool IndentBreakStatements { // tested
			get;
			set;
		}

		public bool AlignEmbeddedIfStatements { // tested
			get;
			set;
		}

		public PropertyFormatting PropertyFormatting { // tested
			get;
			set;
		}

		#endregion
		
		#region Braces
		public BraceStyle ModuleBraceStyle { // tested
			get;
			set;
		}

		public BraceStyle ClassBraceStyle { // tested
			get;
			set;
		}

		public BraceStyle InterfaceBraceStyle { // tested
			get;
			set;
		}

		public BraceStyle EnumBraceStyle { // tested
			get;
			set;
		}

		public BraceStyle MethodBraceStyle { // tested
			get;
			set;
		}

		public BraceStyle AnonymousMethodBraceStyle {
			get;
			set;
		}

		public BraceStyle ConstructorBraceStyle {  // tested
			get;
			set;
		}

		public BraceStyle PropertyBraceStyle { // tested
			get;
			set;
		}

		public BraceStyle PropertyGetBraceStyle { // tested
			get;
			set;
		}

		public BraceStyle PropertySetBraceStyle { // tested
			get;
			set;
		}

		public bool AllowPropertyGetBlockInline { // tested
			get;
			set;
		}

		public bool AllowPropertySetBlockInline { // tested
			get;
			set;
		}

		public BraceStyle StatementBraceStyle { // tested
			get;
			set;
		}

		public bool AllowIfBlockInline {
			get;
			set;
		}

		#endregion
		
		#region Force Braces
		public BraceForcement IfElseBraceForcement { // tested
			get;
			set;
		}

		public BraceForcement ForBraceForcement { // tested
			get;
			set;
		}

		public BraceForcement WhileBraceForcement { // tested
			get;
			set;
		}

		#endregion
		
		#region NewLines
		public NewLinePlacement ElseNewLinePlacement { // tested
			get;
			set;
		}

		public NewLinePlacement ElseIfNewLinePlacement { // tested
			get;
			set;
		}

		public NewLinePlacement CatchNewLinePlacement { // tested
			get;
			set;
		}

		public NewLinePlacement FinallyNewLinePlacement { // tested
			get;
			set;
		}

		public NewLinePlacement WhileNewLinePlacement { // tested
			get;
			set;
		}
		#endregion
		
		#region Spaces
		// Methods
		public bool SpaceBeforeMethodDeclarationParentheses { // tested
			get;
			set;
		}

		public bool SpaceBetweenEmptyMethodDeclarationParentheses {
			get;
			set;
		}

		public bool SpaceBeforeMethodDeclarationParameterComma { // tested
			get;
			set;
		}

		public bool SpaceAfterMethodDeclarationParameterComma { // tested
			get;
			set;
		}

		public bool SpaceWithinMethodDeclarationParentheses { // tested
			get;
			set;
		}
		
		// Method calls
		public bool SpaceBeforeMethodCallParentheses { // tested
			get;
			set;
		}

		public bool SpaceBetweenEmptyMethodCallParentheses { // tested
			get;
			set;
		}

		public bool SpaceBeforeMethodCallParameterComma { // tested
			get;
			set;
		}

		public bool SpaceAfterMethodCallParameterComma { // tested
			get;
			set;
		}

		public bool SpaceWithinMethodCallParentheses { // tested
			get;
			set;
		}
		
		// fields
		
		public bool SpaceBeforeFieldDeclarationComma { // tested
			get;
			set;
		}

		public bool SpaceAfterFieldDeclarationComma { // tested
			get;
			set;
		}
		
		// local variables
		
		public bool SpaceBeforeLocalVariableDeclarationComma { // tested
			get;
			set;
		}

		public bool SpaceAfterLocalVariableDeclarationComma { // tested
			get;
			set;
		}
		
		// constructors
		
		public bool SpaceBeforeConstructorDeclarationParentheses { // tested
			get;
			set;
		}

		public bool SpaceBetweenEmptyConstructorDeclarationParentheses { // tested
			get;
			set;
		}

		public bool SpaceBeforeConstructorDeclarationParameterComma { // tested
			get;
			set;
		}

		public bool SpaceAfterConstructorDeclarationParameterComma { // tested
			get;
			set;
		}

		public bool SpaceWithinConstructorDeclarationParentheses { // tested
			get;
			set;
		}
		
		// indexer
		public bool SpaceBeforeIndexerDeclarationBracket { // tested
			get;
			set;
		}

		public bool SpaceWithinIndexerDeclarationBracket { // tested
			get;
			set;
		}

		public bool SpaceBeforeIndexerDeclarationParameterComma {
			get;
			set;
		}

		public bool SpaceAfterIndexerDeclarationParameterComma {
			get;
			set;
		}

		public bool SpaceBeforeNewParentheses { // tested
			get;
			set;
		}

		public bool SpaceBeforeIfParentheses { // tested
			get;
			set;
		}

		public bool SpaceBeforeWhileParentheses { // tested
			get;
			set;
		}

		public bool SpaceBeforeForParentheses { // tested
			get;
			set;
		}

		public bool SpaceBeforeCatchParentheses { // tested
			get;
			set;
		}

		public bool SpaceBeforeSwitchParentheses { // tested
			get;
			set;
		}

		public bool SpaceAroundAssignment { // tested
			get;
			set;
		}

		public bool SpaceAroundLogicalOperator { // tested
			get;
			set;
		}

		public bool SpaceAroundEqualityOperator { // tested
			get;
			set;
		}

		public bool SpaceAroundRelationalOperator { // tested
			get;
			set;
		}

		public bool SpaceAroundBitwiseOperator { // tested
			get;
			set;
		}

		public bool SpaceAroundAdditiveOperator { // tested
			get;
			set;
		}

		public bool SpaceAroundMultiplicativeOperator { // tested
			get;
			set;
		}

		public bool SpaceAroundShiftOperator { // tested
			get;
			set;
		}

		public bool SpacesWithinParentheses { // tested
			get;
			set;
		}

		public bool SpacesWithinIfParentheses { // tested
			get;
			set;
		}

		public bool SpacesWithinWhileParentheses { // tested
			get;
			set;
		}

		public bool SpacesWithinForParentheses { // tested
			get;
			set;
		}

		public bool SpacesWithinCatchParentheses { // tested
			get;
			set;
		}

		public bool SpacesWithinSwitchParentheses { // tested
			get;
			set;
		}

		public bool SpacesWithinUsingParentheses { // tested
			get;
			set;
		}

		public bool SpacesWithinCastParentheses { // tested
			get;
			set;
		}

		public bool SpacesWithinSizeOfParentheses { // tested
			get;
			set;
		}

		public bool SpaceBeforeSizeOfParentheses { // tested
			get;
			set;
		}

		public bool SpacesWithinTypeOfParentheses { // tested
			get;
			set;
		}

		public bool SpacesWithinNewParentheses { // tested
			get;
			set;
		}

		public bool SpacesBetweenEmptyNewParentheses { // tested
			get;
			set;
		}

		public bool SpaceBeforeNewParameterComma { // tested
			get;
			set;
		}

		public bool SpaceAfterNewParameterComma { // tested
			get;
			set;
		}

		public bool SpaceBeforeTypeOfParentheses { // tested
			get;
			set;
		}

		public bool SpaceBeforeConditionalOperatorCondition { // tested
			get;
			set;
		}

		public bool SpaceAfterConditionalOperatorCondition { // tested
			get;
			set;
		}

		public bool SpaceBeforeConditionalOperatorSeparator { // tested
			get;
			set;
		}

		public bool SpaceAfterConditionalOperatorSeparator { // tested
			get;
			set;
		}
		
		// brackets
		public bool SpacesWithinBrackets { // tested
			get;
			set;
		}

		public bool SpacesBeforeBrackets { // tested
			get;
			set;
		}

		public bool SpaceBeforeBracketComma { // tested
			get;
			set;
		}

		public bool SpaceAfterBracketComma { // tested
			get;
			set;
		}

		public bool SpaceBeforeForSemicolon { // tested
			get;
			set;
		}

		public bool SpaceAfterForSemicolon { // tested
			get;
			set;
		}

		public bool SpaceAfterTypecast { // tested
			get;
			set;
		}

		public bool SpaceBeforeArrayDeclarationBrackets { // tested
			get;
			set;
		}

		#endregion
		
		#region Blank Lines
		public int BlankLinesBeforeImports {
			get;
			set;
		}

		public int BlankLinesAfterImports {
			get;
			set;
		}

		public int BlankLinesBeforeFirstDeclaration {
			get;
			set;
		}

		public int BlankLinesBetweenTypes {
			get;
			set;
		}

		public int BlankLinesBetweenFields {
			get;
			set;
		}

		public int BlankLinesBetweenMembers {
			get;
			set;
		}

		#endregion


		#region Keep formatting
		public bool KeepCommentsAtFirstColumn {
			get;
			set;
		}
		#endregion

		#region Wrapping

		public Wrapping ArrayInitializerWrapping {
			get;
			set;
		}

		public BraceStyle ArrayInitializerBraceStyle {
			get;
			set;
		}

		public Wrapping ChainedMethodCallWrapping {
			get;
			set;
		}

		public Wrapping MethodCallArgumentWrapping {
			get;
			set;
		}

		public bool NewLineAferMethodCallOpenParentheses {
			get;
			set;
		}

		public bool MethodCallClosingParenthesesOnNewLine {
			get;
			set;
		}

		public Wrapping IndexerArgumentWrapping {
			get;
			set;
		}

		public bool NewLineAferIndexerOpenBracket {
			get;
			set;
		}

		public bool IndexerClosingBracketOnNewLine {
			get;
			set;
		}

		public Wrapping MethodDeclarationParameterWrapping {
			get;
			set;
		}

		public bool NewLineAferMethodDeclarationOpenParentheses {
			get;
			set;
		}

		public bool MethodDeclarationClosingParenthesesOnNewLine {
			get;
			set;
		}

		public Wrapping IndexerDeclarationParameterWrapping {
			get;
			set;
		}

		public bool NewLineAferIndexerDeclarationOpenBracket {
			get;
			set;
		}

		public bool IndexerDeclarationClosingBracketOnNewLine {
			get;
			set;
		}
		#endregion

		internal TypeScriptFormattingOptions()
		{
		}
	}
}
