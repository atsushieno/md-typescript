using System;
using ICSharpCode.NRefactory.TypeSystem;
using TypeScriptServiceBridge.TypeScript;

namespace MonoDevelop.TypeScriptBinding.NRefactory.TypeSystem
{
	public class TypeScriptUnresolvedTypeDefinition : IUnresolvedTypeDefinition
	{
		TypeScriptService service;
		AST path;
		public TypeScriptUnresolvedTypeDefinition (TypeScriptService service, AST path)
		{
			this.service = service;
			this.path = path;
		}

		#region IUnresolvedTypeDefinition implementation

		public IType Resolve (ITypeResolveContext context)
		{
			throw new NotImplementedException ();
		}

		public ITypeResolveContext CreateResolveContext (ITypeResolveContext parentContext)
		{
			throw new NotImplementedException ();
		}

		public TypeKind Kind {
			get {
				throw new NotImplementedException ();
			}
		}
		
		public SymbolKind SymbolKind {
			get { throw new NotImplementedException (); }
		}

		public System.Collections.Generic.IList<ITypeReference> BaseTypes {
			get {
				throw new NotImplementedException ();
			}
		}

		public System.Collections.Generic.IList<IUnresolvedTypeParameter> TypeParameters {
			get {
				throw new NotImplementedException ();
			}
		}

		public System.Collections.Generic.IList<IUnresolvedTypeDefinition> NestedTypes {
			get {
				throw new NotImplementedException ();
			}
		}

		public System.Collections.Generic.IList<IUnresolvedMember> Members {
			get {
				throw new NotImplementedException ();
			}
		}

		public System.Collections.Generic.IEnumerable<IUnresolvedMethod> Methods {
			get {
				throw new NotImplementedException ();
			}
		}

		public System.Collections.Generic.IEnumerable<IUnresolvedProperty> Properties {
			get {
				throw new NotImplementedException ();
			}
		}

		public System.Collections.Generic.IEnumerable<IUnresolvedField> Fields {
			get {
				throw new NotImplementedException ();
			}
		}

		public System.Collections.Generic.IEnumerable<IUnresolvedEvent> Events {
			get {
				throw new NotImplementedException ();
			}
		}

		public bool? HasExtensionMethods {
			get {
				throw new NotImplementedException ();
			}
		}

		public bool AddDefaultConstructorIfRequired {
			get {
				throw new NotImplementedException ();
			}
		}

		public FullTypeName FullTypeName {
			get {
				throw new NotImplementedException ();
			}
		}

		#endregion

		#region ITypeReference implementation

		IType ITypeReference.Resolve (ITypeResolveContext context)
		{
			throw new NotImplementedException ();
		}

		#endregion

		#region IUnresolvedEntity implementation

		public EntityType EntityType {
			get {
				throw new NotImplementedException ();
			}
		}

		public DomRegion Region {
			get {
				throw new NotImplementedException ();
			}
		}

		public DomRegion BodyRegion {
			get {
				throw new NotImplementedException ();
			}
		}

		public IUnresolvedTypeDefinition DeclaringTypeDefinition {
			get {
				throw new NotImplementedException ();
			}
		}

		public IUnresolvedFile UnresolvedFile {
			get {
				throw new NotImplementedException ();
			}
		}

		public System.Collections.Generic.IList<IUnresolvedAttribute> Attributes {
			get {
				throw new NotImplementedException ();
			}
		}

		public bool IsStatic {
			get {
				throw new NotImplementedException ();
			}
		}

		public bool IsAbstract {
			get {
				throw new NotImplementedException ();
			}
		}

		public bool IsSealed {
			get {
				throw new NotImplementedException ();
			}
		}

		public bool IsShadowing {
			get {
				throw new NotImplementedException ();
			}
		}

		public bool IsSynthetic {
			get {
				throw new NotImplementedException ();
			}
		}

		#endregion

		#region IHasAccessibility implementation

		public Accessibility Accessibility {
			get {
				throw new NotImplementedException ();
			}
		}

		public bool IsPrivate {
			get {
				throw new NotImplementedException ();
			}
		}

		public bool IsPublic {
			get {
				throw new NotImplementedException ();
			}
		}

		public bool IsProtected {
			get {
				throw new NotImplementedException ();
			}
		}

		public bool IsInternal {
			get {
				throw new NotImplementedException ();
			}
		}

		public bool IsProtectedOrInternal {
			get {
				throw new NotImplementedException ();
			}
		}

		public bool IsProtectedAndInternal {
			get {
				throw new NotImplementedException ();
			}
		}

		#endregion

		#region INamedElement implementation

		public string FullName {
			get {
				throw new NotImplementedException ();
			}
		}

		public string Name {
			get {
				throw new NotImplementedException ();
			}
		}

		public string ReflectionName {
			get {
				throw new NotImplementedException ();
			}
		}

		public string Namespace {
			get {
				throw new NotImplementedException ();
			}
		}

		#endregion
	}
}

