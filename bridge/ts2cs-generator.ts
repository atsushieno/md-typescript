
module BridgeGenerator {

	export enum ContainerKind {
		None,
		Class,
		Interface,
		Enum,
	}

	export class Driver {

		private shimHost: Harness.TypeScriptLS;
		private hostAdapter: Services.LanguageServiceShimHostAdapter;
		private compiler : Services.CompilerState;
		private languageService : Services.ILanguageService;
		
		constructor ()
		{
			this.shimHost = new Harness.TypeScriptLS ();
			this.hostAdapter = new Services.LanguageServiceShimHostAdapter (this.shimHost);
			this.compiler = new Services.CompilerState (this.hostAdapter);
			this.languageService = new Services.TypeScriptServicesFactory ().createPullLanguageService (this.hostAdapter);
		}

		public run ()
		{
			IO.printLine ("using System;");
			IO.printLine ("using TypeScript;");
			IO.printLine ("using Services;");
			IO.printLine ("namespace TypeScript.Formatting { class RulesProvider {} }");
			IO.printLine ("public class any {}");
			IO.printLine ("public class unknown {}");
			IO.printLine ("public class someanonymoustype {}");
			IO.printLine ("public class typescriptfunctionargument {}");
			IO.printLine ("// TypeScript types");
			IO.printLine ("namespace TypeScript {");
			IO.printLine ("\tpublic interface ILocation {}");
			IO.printLine ("\tpublic class Location {}");
			IO.printLine ("\tpublic interface ITextWriter {}");
			IO.printLine ("\tpublic interface IAstWalkCallback {} // delegate interface not supported yet...");
			IO.printLine ("\tpublic interface IAstWalkChildren {}");
			IO.printLine ("\tpublic class Scanner {}");
			IO.printLine ("}");
			IO.printLine ("namespace SymbolDisplay {");
			IO.printLine ("\tpublic class Format {}");
			IO.printLine ("\tpublic class Part {}");
			IO.printLine ("}");
			IO.printLine ("public class MaskBitSize {}");
			IO.printLine ("// Language service types");
			IO.printLine ("namespace Services {");
			IO.printLine ("}");
			IO.printLine ("// JavaScript standard types");
			IO.printLine ("public class RegExp {}");
			IO.printLine ("public class Error {}");
			IO.printLine ("[AttributeUsage (AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true)] public class ImplementsAttribute : Attribute { public ImplementsAttribute (string interfaceType) { Type = interfaceType; } public string Type { get; set; } }");

			//var files : string[] = IO.dir ("../external/typescript/src", null, { recursive: true });
			var files : string[] = IO.readFile (IO.arguments [0]).contents.split ("\n");

			for (var i in files)
				if (files [i] != "")
					this.shimHost.addScript (files [i], IO.readFile (files [i]).contents);

			this.compiler.refresh ();

			for (var i in files) {
				if (files [i] != "") {
					IO.printLine ("// " + files [i]);
					var script = this.compiler.getScript (files [i]);
					for (var i in script.moduleElements.members)
						this.processAST (script.moduleElements.members [i]);
				}
			}
		}
		
		private is_type_name : boolean = false;
		private current_type_name : string = null;
		private container_kind : ContainerKind = ContainerKind.None;
		private in_vardecl_or_retval : boolean = false;
		private constructors = {};
		
		private processAST (ast : TypeScript.AST)
		{
			if (ast == null || ast === undefined)
				return;

			var isTypeNameBak : boolean = false;
			var currentTypeBak : string = null;
			var containerKindBak : ContainerKind = ContainerKind.None;

			switch (ast.nodeType ()) {

			case TypeScript.NodeType.TrueLiteral: IO.print ("true"); break;
			case TypeScript.NodeType.FalseLiteral: IO.print ("false"); break;
			case TypeScript.NodeType.StringLiteral: IO.print ("@\"" + (<TypeScript.StringLiteral> ast).text + "\""); break;
			case TypeScript.NodeType.RegularExpressionLiteral: IO.print ("new System.Text.RegularExpressions.Regex (" + (<TypeScript.RegexLiteral> ast).text + ")"); break;
			case TypeScript.NodeType.NumericLiteral: IO.print ((<TypeScript.NumberLiteral> ast).text ()); break;
			case TypeScript.NodeType.NullLiteral: IO.print ("null"); break;
			
			case TypeScript.NodeType.ThisExpression:
				IO.print ("this");
				break;

			case TypeScript.NodeType.Name:
				var name = (<TypeScript.Identifier> ast).text ();
				if (this.is_type_name) {
					switch (name) {
					case "number":
						name = "double";
						break;
					case "boolean":
						name = "bool";
						break;
					}
				}
				if (name == "$")
					IO.print ("DUMMY_DOLLAR"); // C# does not allow this.
				else
					IO.print (name);
				break;

			case TypeScript.NodeType.NegateExpression:
				var unary = <TypeScript.UnaryExpression> ast;
				IO.print ("-");
				this.processAST (unary.operand);
				break;

			case TypeScript.NodeType.ArrayLiteralExpression:
				IO.print ("null /*Dummy default value!!! ArrayLiteralExpression*/");
				break;
			case TypeScript.NodeType.ObjectCreationExpression:
				IO.print ("null /*Dummy default value!!! ObjectCreationExpression*/");
				break;
			case TypeScript.NodeType.InvocationExpression:
				IO.print ("null /*Dummy default value!!! InvocationExpression*/");
				break;
			
			case TypeScript.NodeType.ElementAccessExpression:
				var binary = <TypeScript.BinaryExpression> ast;
				this.processAST (binary.operand1);
				IO.print ("[");
				this.processAST (binary.operand2);
				IO.print ("]");
				break;
			case TypeScript.NodeType.MemberAccessExpression:
			case TypeScript.NodeType.BitwiseOrExpression:
			case TypeScript.NodeType.BitwiseExclusiveOrExpression:
			case TypeScript.NodeType.BitwiseAndExpression:
			case TypeScript.NodeType.AddExpression:
			case TypeScript.NodeType.SubtractExpression:
			case TypeScript.NodeType.MultiplyExpression:
			case TypeScript.NodeType.DivideExpression:
			case TypeScript.NodeType.ModuloExpression:
			case TypeScript.NodeType.LeftShiftExpression:
			case TypeScript.NodeType.SignedRightShiftExpression:
			case TypeScript.NodeType.UnsignedRightShiftExpression:
				binary = <TypeScript.BinaryExpression> ast;

				// cannot support "this" in parameter default value expression.
				if (binary.operand1.nodeType () == TypeScript.NodeType.ThisExpression) {
					IO.print ("null");
					break;
				}

				this.processAST (binary.operand1);
				switch (ast.nodeType ()) {
				case TypeScript.NodeType.MemberAccessExpression:
					IO.print ("."); break;
				case TypeScript.NodeType.BitwiseOrExpression:
					IO.print (" | "); break;
				case TypeScript.NodeType.BitwiseExclusiveOrExpression:
					IO.print (" ^ "); break;
				case TypeScript.NodeType.BitwiseAndExpression:
					IO.print (" & "); break;
				case TypeScript.NodeType.AddExpression:
					IO.print (" + "); break;
				case TypeScript.NodeType.SubtractExpression:
					IO.print (" - "); break;
				case TypeScript.NodeType.MultiplyExpression:
					IO.print (" * "); break;
				case TypeScript.NodeType.DivideExpression:
					IO.print (" / "); break;
				case TypeScript.NodeType.ModuloExpression:
					IO.print (" % "); break;
				case TypeScript.NodeType.LeftShiftExpression:
					IO.print (" << "); break;
				case TypeScript.NodeType.SignedRightShiftExpression:
					IO.print (" >> "); break;
				case TypeScript.NodeType.UnsignedRightShiftExpression:
					IO.print (" >> "); break; // no difference between >> and >>> in C#
				}
				this.processAST (binary.operand2);
				break;

			case TypeScript.NodeType.GenericType:
				var genericType = <TypeScript.GenericType> ast;
				this.processAST (genericType.name);
				this.processTypeParameters (genericType.typeArguments);
				break;

			case TypeScript.NodeType.TypeParameter:
				var typeParam = <TypeScript.TypeParameter> ast;
				this.processAST (typeParam.name);
				break;

			case TypeScript.NodeType.TypeRef:
				var typeRef = <TypeScript.TypeReference> ast;
				isTypeNameBak = this.is_type_name;
				this.is_type_name = true;
				this.processAST (typeRef.term);
				this.is_type_name = isTypeNameBak;
				if (typeRef.arrayCount) {
					IO.print ("[");
					for (var i = 1; i < typeRef.arrayCount; i++) // [] or [,] or [,,] ...
						IO.print (",");
					IO.print ("]");
				}
				break;
			
			// modules can be either a namespace or an enum
			case TypeScript.NodeType.ModuleDeclaration:
				var mod = <TypeScript.ModuleDeclaration> ast;
				if (mod.isEnum ()) {
					currentTypeBak = this.current_type_name;
					containerKindBak = this.container_kind;
					this.current_type_name = mod.name.actualText;
					this.container_kind = ContainerKind.Enum;
					IO.print ("\tpublic " + this.getModuleModifierString (mod.getModuleFlags ()));
					IO.printLine ("enum " + mod.name.actualText);
					IO.printLine ("\t{");
					for (var i in mod.members.members)
						this.processAST (mod.members.members [i]);
					IO.printLine ("\t}");
					this.container_kind = containerKindBak;
					this.current_type_name = currentTypeBak;
				} else {
					IO.printLine ("namespace " + mod.name.actualText);
					IO.printLine ("{");
					for (var i in mod.members.members)
						this.processAST (mod.members.members [i]);
					IO.printLine ("}");
				}
				break;

			case TypeScript.NodeType.ClassDeclaration:
			case TypeScript.NodeType.InterfaceDeclaration:
				var decl = <TypeScript.TypeDeclaration> ast;

				// I skipped ambient classes as they don't seem in use.
				if (decl.getVarFlags () & TypeScript.VariableFlags.Ambient) {
					break;
				}

				// in parameters there could be anonymous types, but we can't handle them.
				if (this.in_vardecl_or_retval) {
					IO.print ("someanonymoustype");
					break;
				}

				currentTypeBak = this.current_type_name;
				this.current_type_name = decl.name.actualText;
				containerKindBak = this.container_kind;
				this.container_kind =
					ast.nodeType () == TypeScript.NodeType.InterfaceDeclaration ?
					ContainerKind.Interface : ContainerKind.Class;

				if (this.container_kind == ContainerKind.Interface &&
				    decl.members.members.length == 1 &&
				    decl.members.members [0].nodeType () == TypeScript.NodeType.FunctionDeclaration &&
				    (<TypeScript.FunctionDeclaration> decl.members.members [0]).getNameText () == "_call") {
				    /*
					IO.print ("delegate " + decl.name.actualText);
					this.processFunctionArguments (<TypeScript.FunctionDeclaration> ast);
					IO.printLine (";");
					*/
					// dunno what we can do.
					IO.printLine ("// interface " + decl.name.actualText + " should become delegate, but we ignore them so far.");
					IO.printLine ("");
				} else {
					if (decl.implementsList != null && decl.implementsList.members.length > 0) {
						for (var i in decl.implementsList.members) {
							IO.print ("[Implements (\"");
							this.processAST (decl.implementsList.members [i]);
							IO.printLine ("\")]");
						}
					}

					// TypeScript non-export does not always mean it's non-public in C# context...
					// they could be referenced in public methods as a parameter or return type.
					// Hence it's always "public" here.
					IO.print ("\tpublic " + this.getVariableModifierString (decl.getVarFlags (), true));

					if (decl.nodeType () == TypeScript.NodeType.InterfaceDeclaration)
						IO.print ("interface ");
					else
						IO.print ("class ");
					IO.print (decl.name.actualText);
					this.processTypeParameters (decl.typeParameters);
					if (decl.extendsList != null && decl.extendsList.members.length > 0) {
						IO.printLine ("");
						IO.print ("\t\t : ");
						for (var i in decl.extendsList.members) {
							this.processAST (decl.extendsList.members [i]);
							if (i < decl.extendsList.members.length - 1)
								IO.print (", ");
						}
					}
					IO.printLine (" {");

					if (decl.members != null)
						for (var i in decl.members.members)
							this.processAST (decl.members.members [i]);

					var hasDefaultCtor : boolean = false;
					var constructor = <TypeScript.FunctionDeclaration>this.constructors [this.current_type_name];
					if (constructor) {
						var inVarDeclBakCtor = this.in_vardecl_or_retval;
						this.in_vardecl_or_retval = true;
						hasDefaultCtor = hasDefaultCtor || constructor.arguments.members.length == 0;
						for (var i = 0; i < constructor.arguments.members.length; i++) {
							var cparam = <TypeScript.Parameter>constructor.arguments.members [i];
							if (cparam.getVarFlags () & TypeScript.VariableFlags.Public) {
								IO.print ("\t\tpublic ");
								if (cparam.typeExpr == null)
									IO.print ("object/*unknown*/");
								else
									this.processAST (cparam.typeExpr);
								IO.print (" ");
								this.processAST (cparam.id);
								IO.printLine (" { get; set; }");
							}
						}
						this.in_vardecl_or_retval = inVarDeclBakCtor;
					}
					if (decl.nodeType () == TypeScript.NodeType.ClassDeclaration && !hasDefaultCtor) {
						// create dummy constructor to not error out inheritance without default ctor.
						IO.printLine ("public " + decl.name.actualText + " () {}");
					}

					IO.printLine ("\t}");
				}
				
				this.current_type_name = currentTypeBak;
				this.container_kind = containerKindBak;
				break;

			// type members
			case TypeScript.NodeType.VariableStatement:
				this.processAST ((<TypeScript.VariableStatement> ast).declaration);
				break;
			case TypeScript.NodeType.VariableDeclaration:
				if (this.current_type_name == null) {
					IO.printLine ("\t// skip global variable declaration");
					break; // skip global variables
				}
				
				/*
				currentTypeBak = this.current_type_name;
				if (this.current_type_name == null) {
					IO.printLine ("\tpublic static partial class ModuleMembers");
					IO.printLine ("\t{");
					this.current_type_name = "ModuleMembers";
				}
				*/
				
				var vardecl = <TypeScript.VariableDeclaration> ast;
				if (vardecl.declarators == null || vardecl.declarators.members == null)
					break; // makes no sense...
				for (var i in vardecl.declarators.members)
					this.processAST (vardecl.declarators.members [i]);

				/*
				if (currentTypeBak == null) {
					IO.printLine ("\t}");
					this.current_type_name = null;
				}
				*/

				break;

			case TypeScript.NodeType.VariableDeclarator:
				var inVarDeclBak = this.in_vardecl_or_retval;
				this.in_vardecl_or_retval = true;
				var variable = <TypeScript.VariableDeclarator> ast;
				
				if (this.container_kind == ContainerKind.Enum) {
					// unlike other fields, emit "init" expression (should be only int and some expressions)
					IO.print ("\t\t@");
					this.processAST (variable.id);
					if (variable.init != null)
						this.processDefaultValue (variable.typeExpr, variable.init);
					IO.printLine (",");
				} else {
					//IO.printLine ("\t\t[TypeScriptField]");
					IO.print ("\t\t" + this.getVariableModifierString (variable.getVarFlags ()));
					IO.print (" ");
					if (variable.typeExpr == null)
						IO.print ("object/*unknown*/");
					else
						this.processAST (variable.typeExpr);
					IO.print (" @");
					this.processAST (variable.id);
					/*
					// for stubs, no need to assign values. Just generate property.
					// Interfaces also have them, so they can't be declared as fields anyways.
					if (variable.init != null)
						this.processDefaultValue (variable.typeExpr, variable.init);
					IO.printLine (";");
					*/
					IO.printLine (" { get; set; }");

				}
				this.in_vardecl_or_retval = inVarDeclBak;
				break;

			case TypeScript.NodeType.FunctionDeclaration:
				if (this.current_type_name == null) {
					IO.printLine ("\t// skip global functions");
					break; // skip global functions
				}
				
				if (this.in_vardecl_or_retval) {
					// can't really output this yet.
					IO.printLine ("typescriptfunctionargument");
					break;
				}
				var func = <TypeScript.FunctionDeclaration> ast;

				if (func.getFunctionFlags () & TypeScript.FunctionFlags.Private) // skip private functions
					break;

				/*
				currentTypeBak = this.current_type_name;
				if (this.current_type_name == null) {
					IO.printLine ("\tpublic static partial class ModuleMembers");
					IO.printLine ("\t{");
					this.current_type_name = "ModuleMembers";
				}
				*/
				
				IO.print ("\t\t" + this.getFunctionModifierString (func.getFunctionFlags ()));
				IO.print (" ");
				if (func.getNameText () == null) {
					// constructor
					IO.print ("public " + this.current_type_name);
					this.constructors [this.current_type_name] = func;
				} else {
					var inVarDeclOrRetValBak = this.in_vardecl_or_retval;
					this.in_vardecl_or_retval = true;
					if (func.returnTypeAnnotation != null)
						this.processAST (func.returnTypeAnnotation);
					else
						IO.print ("object"); // could be void or untyped object
					this.in_vardecl_or_retval = inVarDeclOrRetValBak;
					IO.print (" @");
					this.processAST (func.name);
					this.processTypeParameters (func.typeArguments);
				}
				this.processFunctionArguments (func);

				if (this.container_kind == ContainerKind.Interface) {
					IO.printLine (";");
				} else {
					IO.printLine ("");
					IO.printLine ("\t\t{");
					IO.printLine ("\t\t\tthrow new NotImplementedException ();");
					IO.printLine ("\t\t}");
				}

				/*
				if (currentTypeBak == null) {
					IO.printLine ("\t}");
					this.current_type_name = null;
				}
				*/

				break;

			case TypeScript.NodeType.Parameter:
				var inVarDeclBak = this.in_vardecl_or_retval;
				this.in_vardecl_or_retval = true;

				var param = <TypeScript.Parameter> ast;
				IO.print (this.getVariableModifierString (param.getVarFlags (), true));
				IO.print (" ");
				if (param.typeExpr == null)
					IO.print ("object/*unknown*/");
				else
					this.processAST (param.typeExpr);
				IO.print (" @");
				this.processAST (param.id);
				if (param.init != null)
					this.processDefaultValue (param.typeExpr, param.init);
				else if (param.isOptional) {
					// the parameter is optional but has no init value - need to give default
					if (param.typeExpr == null) {
						IO.print (" = null /* unknown type */");
					} else {
						IO.print (" = default (");
						this.processAST (param.typeExpr);
						IO.print (")");
					}
				}

				this.in_vardecl_or_retval = inVarDeclBak;

				break;

			default:
				if (this.current_type_name == null) {
					IO.printLine ("\t// skip global unsupported node type " + ast.nodeType ());
				} else {
					IO.printLine ("\t\t/* !!!!! UNBOUND NODE TYPE " + ast.nodeType () + " */");
				}
				break;
			}
		}
		
		private processDefaultValue (typeExpr : TypeScript.AST, init : TypeScript.AST)
		{
			IO.print (" = ");
			if (typeExpr == null)
				IO.print ("null /* unknown type */");
			else
				this.processAST (init);
		}
		
		private processFunctionArguments (func : TypeScript.FunctionDeclaration)
		{
			IO.print ("(");
			for (var i in func.arguments.members) {
				this.processAST (func.arguments.members [i]);
				if (i < func.arguments.members.length - 1)
					IO.print (", ");
			}
			IO.print (")");
		}

		// so far only used for enums. And for the same reason as that of class
		// or interface, accessibility isn't taken from this flag into consideration.
		private getModuleModifierString (flag : TypeScript.ModuleFlags) : string
		{
			var s = "";
			if (false) {
				if (flag & TypeScript.ModuleFlags.Exported)
					s += "public ";
				if (flag & TypeScript.ModuleFlags.Private)
					s += "private ";
				if (flag & TypeScript.ModuleFlags.Public)
					s += "public ";
			}
			if (flag & TypeScript.ModuleFlags.Ambient)
				s += "static "; // extension methods container
			if (flag & TypeScript.ModuleFlags.Static)
				s += "static ";
			return s;
		}
		
		private getVariableModifierString (flag : TypeScript.VariableFlags, ignoreAccessibility : boolean = false) : string
		{
			var s = "";
			if (!ignoreAccessibility) {
				if (flag & TypeScript.VariableFlags.Exported)
					s += "public ";
				if (flag & TypeScript.VariableFlags.Private)
					s += "private ";
				if (flag & TypeScript.VariableFlags.Public)
					s += "public ";
			}
			if (flag & TypeScript.VariableFlags.Ambient)
				s += "static "; // extension methods container
			if (flag & TypeScript.VariableFlags.Static)
				s += "static ";
			return s;
		}
		
		private getFunctionModifierString (flag : TypeScript.FunctionFlags) : string
		{
			var s = "";
			if (flag & TypeScript.FunctionFlags.Exported)
				s += "public ";
			if (flag & TypeScript.FunctionFlags.Private)
				s += "private ";
			if (flag & TypeScript.FunctionFlags.Public)
				s += "public ";
			if (flag & TypeScript.FunctionFlags.Ambient)
				s += "static "; // extension methods container
			if (flag & TypeScript.FunctionFlags.Static)
				s += "static ";
			return s;
		}

		private processTypeParameters (typeParams: TypeScript.ASTList)
		{
			if (typeParams != null && typeParams.members.length > 0) {
				IO.print ("<");
				for (var i in typeParams.members) {
					this.processAST (typeParams.members [i]);
					if (i < typeParams.members.length - 1)
						IO.printLine (", ");
				}
				IO.print (">");
			}
		}
	}
}

new BridgeGenerator.Driver ().run ();
