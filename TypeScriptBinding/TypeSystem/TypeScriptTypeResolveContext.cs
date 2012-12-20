using System;
using ICSharpCode.NRefactory.TypeSystem;

namespace MonoDevelop.TypeScriptBinding.NRefactory.Resolver
{
	public class TypeScriptTypeResolveContext : ITypeResolveContext
	{
		readonly IAssembly assembly;
		//readonly ResolvedUsingScope currentUsingScope;
		readonly ITypeDefinition currentTypeDefinition;
		readonly IMember currentMember;
		readonly string[] methodTypeParameterNames;
		
		public TypeScriptTypeResolveContext (IAssembly assembly, /*ResolvedUsingScope usingScope = null,*/ ITypeDefinition typeDefinition = null, IMember member = null)
		{
			if (assembly == null)
				throw new ArgumentNullException("assembly");
			this.assembly = assembly;
			//this.currentUsingScope = usingScope;
			this.currentTypeDefinition = typeDefinition;
			this.currentMember = member;
		}

		private TypeScriptTypeResolveContext (IAssembly assembly, /*ResolvedUsingScope usingScope,*/ ITypeDefinition typeDefinition, IMember member, string[] methodTypeParameterNames)
		{
			this.assembly = assembly;
			//this.currentUsingScope = usingScope;
			this.currentTypeDefinition = typeDefinition;
			this.currentMember = member;
			this.methodTypeParameterNames = methodTypeParameterNames;
		}

		#region ITypeResolveContext implementation

		public ITypeResolveContext WithCurrentTypeDefinition (ITypeDefinition typeDefinition)
		{
			return new TypeScriptTypeResolveContext(assembly, /*currentUsingScope,*/ typeDefinition, currentMember, methodTypeParameterNames);
		}

		public ITypeResolveContext WithCurrentMember (IMember member)
		{
			return new TypeScriptTypeResolveContext(assembly, /*currentUsingScope,*/ currentTypeDefinition, member, methodTypeParameterNames);
		}

		public ICompilation Compilation {
			get { return assembly.Compilation; }
		}

		public IAssembly CurrentAssembly {
			get { return assembly; }
		}

		public ITypeDefinition CurrentTypeDefinition {
			get { return currentTypeDefinition; }
		}

		public IMember CurrentMember {
			get { return currentMember; }
		}

		#endregion
	}
}

