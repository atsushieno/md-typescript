using System;
using ICSharpCode.NRefactory.TypeSystem;
using TypeScriptServiceBridge.Services;

namespace MonoDevelop.TypeScriptBinding.NRefactory.TypeSystem
{
	public class TypeScriptType : IType
	{
		public TypeScriptType (DefinitionInfo definition)
		{
			Definition = definition;
		}

		public DefinitionInfo Definition { get; private set; }

		#region IType implementation

		public ITypeDefinition GetDefinition ()
		{
			throw new NotImplementedException ();
		}

		public IType AcceptVisitor (TypeVisitor visitor)
		{
			throw new NotImplementedException ();
		}

		public IType VisitChildren (TypeVisitor visitor)
		{
			throw new NotImplementedException ();
		}

		public ITypeReference ToTypeReference ()
		{
			throw new NotImplementedException ();
		}

		public System.Collections.Generic.IEnumerable<IType> GetNestedTypes (Predicate<ITypeDefinition> filter = null, GetMemberOptions options = (GetMemberOptions)0)
		{
			throw new NotImplementedException ();
		}

		public System.Collections.Generic.IEnumerable<IType> GetNestedTypes (System.Collections.Generic.IList<IType> typeArguments, Predicate<ITypeDefinition> filter = null, GetMemberOptions options = (GetMemberOptions)0)
		{
			throw new NotImplementedException ();
		}

		public System.Collections.Generic.IEnumerable<IMethod> GetConstructors (Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = (GetMemberOptions)2)
		{
			throw new NotImplementedException ();
		}

		public System.Collections.Generic.IEnumerable<IMethod> GetMethods (Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = (GetMemberOptions)0)
		{
			throw new NotImplementedException ();
		}

		public System.Collections.Generic.IEnumerable<IMethod> GetMethods (System.Collections.Generic.IList<IType> typeArguments, Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = (GetMemberOptions)0)
		{
			throw new NotImplementedException ();
		}

		public System.Collections.Generic.IEnumerable<IProperty> GetProperties (Predicate<IUnresolvedProperty> filter = null, GetMemberOptions options = (GetMemberOptions)0)
		{
			throw new NotImplementedException ();
		}

		public System.Collections.Generic.IEnumerable<IField> GetFields (Predicate<IUnresolvedField> filter = null, GetMemberOptions options = (GetMemberOptions)0)
		{
			throw new NotImplementedException ();
		}

		public System.Collections.Generic.IEnumerable<IEvent> GetEvents (Predicate<IUnresolvedEvent> filter = null, GetMemberOptions options = (GetMemberOptions)0)
		{
			throw new NotImplementedException ();
		}

		public System.Collections.Generic.IEnumerable<IMember> GetMembers (Predicate<IUnresolvedMember> filter = null, GetMemberOptions options = (GetMemberOptions)0)
		{
			throw new NotImplementedException ();
		}

		public System.Collections.Generic.IEnumerable<IMethod> GetAccessors (Predicate<IUnresolvedMethod> filter = null, GetMemberOptions options = (GetMemberOptions)0)
		{
			throw new NotImplementedException ();
		}

		public TypeKind Kind {
			get {
				throw new NotImplementedException ();
			}
		}

		public bool? IsReferenceType {
			get {
				throw new NotImplementedException ();
			}
		}

		public IType DeclaringType {
			get {
				throw new NotImplementedException ();
			}
		}

		public int TypeParameterCount {
			get {
				throw new NotImplementedException ();
			}
		}

		public System.Collections.Generic.IEnumerable<IType> DirectBaseTypes {
			get {
				throw new NotImplementedException ();
			}
		}

		public TypeParameterSubstitution GetSubstitution ()
		{
			throw new NotImplementedException ();
		}

		public TypeParameterSubstitution GetSubstitution (System.Collections.Generic.IList<IType> methodTypeArguments)
		{
			throw new NotImplementedException ();
		}

		public System.Collections.Generic.IList<IType> TypeArguments {
			get {
				throw new NotImplementedException ();
			}
		}

		public bool IsParameterized {
			get {
				throw new NotImplementedException ();
			}
		}

		#endregion

		#region IEquatable implementation

		public bool Equals (IType other)
		{
			throw new NotImplementedException ();
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

