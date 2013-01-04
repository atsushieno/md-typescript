#if HAVE_NREFACTORY_TYPESYSTEM
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.NRefactory.TypeSystem.Implementation;
using MonoDevelop.Ide.TypeSystem;

namespace MonoDevelop.TypeScriptBinding
{
	public class TypeScriptAmbience : Ambience
	{
		static Dictionary<TypeKind, string> classTypes = new Dictionary<TypeKind, string> ();
		static TypeScriptAmbience ()
		{
			
			classTypes [TypeKind.Class] = "class";
			classTypes [TypeKind.Enum] = "enum";
			classTypes [TypeKind.Interface] = "interface";
		}

		public TypeScriptAmbience ()
			: base ("TypeScript")
		{
		}

		#region implemented abstract members of MonoDevelop.Ide.TypeSystem.Ambience
		public override string GetIntrinsicTypeName (string reflectionName)
		{
			// nothing like System.Int32 -> int in TypeScript (so far)
			return reflectionName;
		}

		public override string SingleLineComment (string text)
		{
			return "// " + text;
		}

		public override string GetString (string nameSpace, OutputSettings settings)
		{
			var result = new StringBuilder ();
			if (settings.IncludeKeywords)
				result.Append (settings.EmitKeyword ("module"));
			result.Append (Format (nameSpace));
			return result.ToString ();
		}

		protected override string GetTypeReferenceString (ICSharpCode.NRefactory.TypeSystem.IType reference, OutputSettings settings)
		{
			if (reference == null)
				return "null";
			var type = reference;
			if (type.Kind == TypeKind.Unknown) {
				return settings.IncludeMarkup ? settings.Markup (reference.Name) : reference.Name;
			}

			// There is no generics in TypeScript yet...
			//if (reference.Kind == TypeKind.TypeParameter)
			//	return settings.IncludeMarkup ? settings.Markup (reference.Name) : reference.FullName;
			
			var sb = new StringBuilder ();
			// I don't think we map anything to $Anonymous in TypeScript.
			/*
			if (type is ITypeDefinition && ((ITypeDefinition)type).IsSynthetic && ((ITypeDefinition)type).Name == "$Anonymous$") {
				sb.Append ("new {");
				foreach (var property in ((ITypeDefinition)type).Properties) {
					sb.AppendLine ();
					sb.Append ("\t");
					sb.Append (GetTypeReferenceString (property.ReturnType, settings) ?? "?");
					sb.Append (" ");
					sb.Append (settings.IncludeMarkup ? settings.Markup (property.Name) : property.Name);
					sb.Append (";");
				}
				sb.AppendLine ();
				sb.Append ("}");
				return sb.ToString ();
			}
			*/
			
			AppendType (sb, type, settings);
			return sb.ToString ();
		}

		protected override string GetTypeString (ICSharpCode.NRefactory.TypeSystem.IType t, OutputSettings settings)
		{
			if (t.Kind == TypeKind.Unknown) {
				return settings.IncludeMarkup ? settings.Markup (t.Name) : t.Name;
			}
			
			if (t.Kind == TypeKind.TypeParameter)
				return settings.IncludeMarkup ? settings.Markup (t.FullName) : t.FullName;

			var typeWithElementType = t as TypeWithElementType;
			if (typeWithElementType != null) {
				var sb = new StringBuilder ();
			
				if (typeWithElementType is PointerType) {
					sb.Append (settings.Markup ("*"));
				} 
				AppendType (sb, typeWithElementType.ElementType, settings);
				
				if (typeWithElementType is ArrayType) {
					sb.Append (settings.Markup ("["));
					sb.Append (settings.Markup (new string (',', ((ArrayType)t).Dimensions - 1)));
					sb.Append (settings.Markup ("]"));
				}
				return sb.ToString ();
			}
			
			ITypeDefinition type = t.GetDefinition ();
			if (type == null)
				return "";

			// No short names in TypeScript
			/*
			if (!settings.UseNETTypeNames && type.Namespace == "System" && type.TypeParameterCount == 0) {
				switch (type.Name) {
				case "Object":
					return "object";
				case "Boolean":
					return "bool";
				case "Char":
					return "char";
				case "SByte":
					return "sbyte";
				case "Byte":
					return "byte";
				case "Int16":
					return "short";
				case "UInt16":
					return "ushort";
				case "Int32":
					return "int";
				case "UInt32":
					return "uint";
				case "Int64":
					return "long";
				case "UInt64":
					return "ulong";
				case "Single":
					return "float";
				case "Double":
					return "double";
				case "Decimal":
					return "decimal";
				case "String":
					return "string";
				case "Void":
					return "void";
				}
			}
			*/
			
			// output anonymous type
			if (type.IsSynthetic && type.Name == "$Anonymous$")
				return GetTypeReferenceString (type, settings);
			
			var result = new StringBuilder ();


			var def = type;
			AppendModifiers (result, settings, def);
			if (settings.IncludeKeywords)
				result.Append (GetString (def.Kind));
			if (result.Length > 0 && !result.ToString ().EndsWith (" "))
				result.Append (settings.Markup (" "));
			
			
			if (type.Kind == TypeKind.Delegate && settings.ReformatDelegates && settings.IncludeReturnType) {
				var invoke = type.GetDelegateInvokeMethod ();
				result.Append (GetTypeReferenceString (invoke.ReturnType, settings));
				result.Append (settings.Markup (" "));
			}
			
			if (settings.UseFullName && !string.IsNullOrEmpty (type.Namespace)) 
				result.Append ((settings.IncludeMarkup ? settings.Markup (t.Namespace) : type.Namespace) + ".");
			
			if (settings.UseFullInnerTypeName && type.DeclaringTypeDefinition != null) {
				bool includeGenerics = settings.IncludeGenerics;
				settings.OutputFlags |= OutputFlags.IncludeGenerics;
				string typeString = GetTypeReferenceString (type.DeclaringTypeDefinition, settings);
				if (!includeGenerics)
					settings.OutputFlags &= ~OutputFlags.IncludeGenerics;
				result.Append (typeString);
				result.Append (settings.Markup ("."));
			}
			result.Append (settings.EmitName (type, settings.IncludeMarkup ? settings.Markup (t.Name) : type.Name));
			// No generics in TypeScript so far.
			/*
			if (settings.IncludeGenerics && type.TypeParameterCount > 0) {
				result.Append (settings.Markup ("<"));
				for (int i = 0; i < type.TypeParameterCount; i++) {
					if (i > 0)
						result.Append (settings.Markup (settings.HideGenericParameterNames ? "," : ", "));
					if (!settings.HideGenericParameterNames) {
						if (t is ParameterizedType) {
							result.Append (GetTypeReferenceString (((ParameterizedType)t).TypeArguments [i], settings));
						} else {
							AppendVariance (result, type.TypeParameters [i].Variance);
							result.Append (NetToCSharpTypeName (type.TypeParameters [i].FullName));
						}
					}
				}
				result.Append (settings.Markup (">"));
			}
			*/

			// No delegate in TypeScript so far.
			/*
			if (t.Kind == TypeKind.Delegate && settings.ReformatDelegates) {
//				var policy = GetPolicy (settings);
//				if (policy.BeforeMethodCallParentheses)
//					result.Append (settings.Markup (" "));
				result.Append (settings.Markup ("("));
				var invoke = type.GetDelegateInvokeMethod ();
				if (invoke != null) 
					AppendParameterList (result, settings, invoke.Parameters);
				result.Append (settings.Markup (")"));
				return result.ToString ();
			}
			*/
			
			if (settings.IncludeBaseTypes && type.DirectBaseTypes.Any ()) {
				bool first = true;
				foreach (var baseType in type.DirectBaseTypes) {
//				if (baseType.FullName == "System.Object" || baseType.FullName == "System.Enum")
//					continue;
					result.Append (settings.Markup (first ? " : " : ", "));
					first = false;
					result.Append (GetTypeReferenceString (baseType, settings));	
				}
				
			}
//		OutputConstraints (result, settings, type.TypeParameters);
			return result.ToString ();
		}

		protected override string GetMethodString (ICSharpCode.NRefactory.TypeSystem.IMethod method, OutputSettings settings)
		{
			throw new System.NotImplementedException ();
		}

		protected override string GetConstructorString (ICSharpCode.NRefactory.TypeSystem.IMethod constructor, OutputSettings settings)
		{
			throw new System.NotImplementedException ();
		}

		protected override string GetDestructorString (ICSharpCode.NRefactory.TypeSystem.IMethod destructor, OutputSettings settings)
		{
			throw new System.NotImplementedException ();
		}

		protected override string GetOperatorString (ICSharpCode.NRefactory.TypeSystem.IMethod op, OutputSettings settings)
		{
			throw new System.NotSupportedException ("No operator overload in TypeScript");
		}

		protected override string GetFieldString (ICSharpCode.NRefactory.TypeSystem.IField field, OutputSettings settings)
		{
			if (field == null)
				return "";
			var result = new StringBuilder ();
			bool isEnum = field.DeclaringTypeDefinition != null && field.DeclaringTypeDefinition.Kind == TypeKind.Enum;
			AppendModifiers (result, settings, field);
			
			if (!settings.CompletionListFomat && settings.IncludeReturnType && !isEnum) {
				result.Append (GetTypeReferenceString (field.ReturnType, settings));
				result.Append (settings.Markup (" "));
			}
			
			if (!settings.IncludeReturnType && settings.UseFullName) {
				result.Append (GetTypeReferenceString (field.DeclaringTypeDefinition, settings));
				result.Append (settings.Markup ("."));
			}
			result.Append (settings.EmitName (field, FilterName (Format (field.Name))));
			
			if (settings.CompletionListFomat && settings.IncludeReturnType && !isEnum) {
				result.Append (settings.Markup (" : "));
				result.Append (GetTypeReferenceString (field.ReturnType, settings));
			}
			return result.ToString ();
		}

		protected override string GetEventString (ICSharpCode.NRefactory.TypeSystem.IEvent evt, OutputSettings settings)
		{
			throw new System.NotSupportedException ("No event in TypeScript");
		}

		protected override string GetPropertyString (ICSharpCode.NRefactory.TypeSystem.IProperty property, OutputSettings settings)
		{
			if (property == null)
				return "";
			var result = new StringBuilder ();
			AppendModifiers (result, settings, property);
			if (!settings.CompletionListFomat && settings.IncludeReturnType) {
				result.Append (GetTypeReferenceString (property.ReturnType, settings));
				result.Append (settings.Markup (" "));
			}
			
			if (!settings.IncludeReturnType && settings.UseFullName) {
				result.Append (GetTypeReferenceString (property.DeclaringTypeDefinition, new OutputSettings (OutputFlags.UseFullName)));
				result.Append (settings.Markup ("."));
			}

			// TypeScript does not support explicit interface implementation.
			//AppendExplicitInterfaces (result, property, settings);
			
			if (property.EntityType == EntityType.Indexer) {
				result.Append (settings.EmitName (property, "this"));
			} else {
				result.Append (settings.EmitName (property, Format (FilterName (property.Name))));
			}
			
			if (settings.IncludeParameters && property.Parameters.Count > 0) {
				result.Append (settings.Markup ("["));
				AppendParameterList (result, settings, property.Parameters);
				result.Append (settings.Markup ("]"));
			}
						
			if (settings.CompletionListFomat && settings.IncludeReturnType) {
				result.Append (settings.Markup (" : "));
				result.Append (GetTypeReferenceString (property.ReturnType, settings));
			}
			
			if (settings.IncludeAccessor) {
				result.Append (settings.Markup (" {"));
				if (property.CanGet)
					result.Append (settings.Markup (" get;"));
				if (property.CanSet)
					result.Append (settings.Markup (" set;"));
				result.Append (settings.Markup (" }"));
			}
			
			return result.ToString ();
		}

		protected override string GetIndexerString (ICSharpCode.NRefactory.TypeSystem.IProperty property, OutputSettings settings)
		{
			throw new System.NotSupportedException ("No indexer in TypeScript");
		}

		protected override string GetParameterString (ICSharpCode.NRefactory.TypeSystem.IParameterizedMember member, ICSharpCode.NRefactory.TypeSystem.IParameter parameter, OutputSettings settings)
		{
			if (parameter == null)
				return "";
			var result = new StringBuilder ();
			if (settings.IncludeParameterName) {
				if (settings.IncludeModifiers) {
					// No in/out parameter modifiers
					/*
					if (parameter.IsOut) {
						result.Append (settings.EmitKeyword ("out"));
					}
					if (parameter.IsRef) {
						result.Append (settings.EmitKeyword ("ref"));
					}
					*/
					if (parameter.IsParams) {
						result.Append (settings.EmitKeyword ("..."));
					}
				}
				
				result.Append (GetTypeReferenceString (parameter.Type, settings));
				result.Append (" ");

				if (settings.HighlightName) {
					result.Append (settings.EmitName (parameter, settings.Highlight (Format (FilterName (parameter.Name)))));
				} else {
					result.Append (settings.EmitName (parameter, Format (FilterName (parameter.Name))));
				}
			} else {
				result.Append (GetTypeReferenceString (parameter.Type, settings));
			}
			return result.ToString ();
		}
		#endregion
		
		void AppendType (StringBuilder sb, IType type, OutputSettings settings)
		{
			if (type.Kind == TypeKind.Unknown) {
				sb.Append (settings.IncludeMarkup ? settings.Markup (type.Name) : type.Name);
				return;
			}
			// no generics in TypeScript so far.
			/*
			if (type.Kind == TypeKind.TypeParameter) {
				sb.Append (settings.IncludeMarkup ? settings.Markup (type.Name) : type.Name);
				return;
			}
			*/
			// no nested type in TypeScript so far.
			/*
			if (type.DeclaringType != null) {
				AppendType (sb, type.DeclaringType, settings);
				sb.Append (settings.Markup ("."));
			}
			*/

			var typeWithElementType = type as TypeWithElementType;
			if (typeWithElementType != null) {
				AppendType (sb, typeWithElementType.ElementType, settings);
				
				if (typeWithElementType is PointerType) {
					sb.Append (settings.Markup ("*"));
				} 
				
				if (typeWithElementType is ArrayType) {
					sb.Append (settings.Markup ("["));
					sb.Append (settings.Markup (new string (',', ((ArrayType)type).Dimensions - 1)));
					sb.Append (settings.Markup ("]"));
				}
				return;
			}

			// no generics in TypeScript so far.
			/*
			var pt = type as ParameterizedType;
			if (pt != null) {
				if (pt.Name == "Nullable" && pt.Namespace == "System" && pt.TypeParameterCount == 1) {
					AppendType (sb, pt.TypeArguments [0], settings);
					sb.Append (settings.Markup ("?"));
					return;
				}
				sb.Append (pt.Name);
				if (pt.TypeParameterCount > 0) {
					sb.Append (settings.Markup ("<"));
					for (int i = 0; i < pt.TypeParameterCount; i++) {
						if (i > 0)
							sb.Append (settings.Markup (", "));
						AppendType (sb, pt.TypeArguments [i], settings);
					}
					sb.Append (settings.Markup (">"));
				}
				return;
			}
			*/
			
			var typeDef = type as ITypeDefinition ?? type.GetDefinition ();
			if (typeDef != null) {
				if (settings.UseFullName) {
					sb.Append (settings.IncludeMarkup ? settings.Markup (typeDef.FullName) : typeDef.FullName);
				} else {
					sb.Append (settings.IncludeMarkup ? settings.Markup (typeDef.Name) : typeDef.Name);
				}

				// No generics in TypeScript so far.
				/*
				if (typeDef.TypeParameterCount > 0) {
					sb.Append (settings.Markup ("<"));
					for (int i = 0; i < typeDef.TypeParameterCount; i++) {
						if (i > 0)
							sb.Append (settings.Markup (", "));
						AppendVariance (sb, typeDef.TypeParameters [i].Variance);
						AppendType (sb, typeDef.TypeParameters [i], settings);
					}
					sb.Append (settings.Markup (">"));
				}
				*/
			}
		}
		
		void AppendModifiers (StringBuilder result, OutputSettings settings, IEntity entity)
		{
			if (!settings.IncludeModifiers)
				return;
			if (entity.IsStatic)
				result.Append (settings.EmitModifiers ("static"));
			// TypeScript does not support them.
			/*
			if (entity.IsSealed)
				result.Append (settings.EmitModifiers ("sealed"));
			if (entity.IsAbstract)
				result.Append (settings.EmitModifiers ("abstract"));
			if (entity.IsShadowing)
				result.Append (settings.EmitModifiers ("new"));
			*/

			// TypeScript does not support most of them.
			switch (entity.Accessibility) {
			/*
			case Accessibility.Internal:
				result.Append (settings.EmitModifiers ("internal"));
				break;
			case Accessibility.ProtectedAndInternal:
				result.Append (settings.EmitModifiers ("protected internal"));
				break;
			case Accessibility.ProtectedOrInternal:
				result.Append (settings.EmitModifiers ("internal protected"));
				break;
			case Accessibility.Protected:
				result.Append (settings.EmitModifiers ("protected"));
				break;
			*/
			case Accessibility.Private:
				result.Append (settings.EmitModifiers ("private"));
				break;
			case Accessibility.Public:
				result.Append (settings.EmitModifiers ("public"));
				break;
			}
		}

		static string GetString (TypeKind classType)
		{
			string res;
			if (classTypes.TryGetValue (classType, out res))
				return res;
			return string.Empty;
		}
		
		internal static string FilterName (string name)
		{
			// TypeScript does not support name escaping...
			return name;
		}

		void AppendParameterList (StringBuilder result, OutputSettings settings, IEnumerable<IParameter> parameterList)
		{
			if (parameterList == null)
				return;
			
			bool first = true;
			foreach (var parameter in parameterList) {
				if (!first)
					result.Append (settings.Markup (", "));
				AppendParameter (settings, result, parameter);
				first = false;
			}
		}
		
		void AppendParameter (OutputSettings settings, StringBuilder result, IParameter parameter)
		{
			if (parameter == null)
				return;
			result.Append (GetParameterString (null, parameter, settings));
			if (parameter.IsParams) {
				result.Append (settings.Markup ("params"));
				result.Append (settings.Markup (" "));
			}
		}
	}
}
#endif
