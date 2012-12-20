using System;
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Editor;
using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.NRefactory.Documentation;

namespace MonoDevelop.TypeScriptBinding
{
	public class TypeScriptUnresolvedFile : IUnresolvedFile, IUnresolvedDocumentationProvider
	{
		string fileName;
		IList<IUnresolvedTypeDefinition> topLevelTypeDefinitions = new List<IUnresolvedTypeDefinition>();
		IList<IUnresolvedAttribute> assemblyAttributes = new List<IUnresolvedAttribute>();
		IList<IUnresolvedAttribute> moduleAttributes = new List<IUnresolvedAttribute>();
		IList<Error> errors = new List<Error> ();
		Dictionary<IUnresolvedEntity, string> documentation;

		public TypeScriptUnresolvedFile (string fileName)
		{
			if (fileName == null)
				throw new ArgumentNullException("fileName");
			this.fileName = fileName;
			//this.rootUsingScope = new UsingScope();
		}
		
		static T FindEntity<T>(IList<T> list, TextLocation location) where T : class, IUnresolvedEntity
		{
			// This could be improved using a binary search
			foreach (T entity in list) {
				if (entity.Region.IsInside(location.Line, location.Column))
					return entity;
			}
			return null;
		}

		#region IUnresolvedFile implementation

		public IUnresolvedTypeDefinition GetTopLevelTypeDefinition (TextLocation location)
		{
			return FindEntity(topLevelTypeDefinitions, location);
		}

		public IUnresolvedTypeDefinition GetInnermostTypeDefinition (ICSharpCode.NRefactory.TextLocation location)
		{
			IUnresolvedTypeDefinition parent = null;
			IUnresolvedTypeDefinition type = GetTopLevelTypeDefinition(location);
			while (type != null) {
				parent = type;
				type = FindEntity(parent.NestedTypes, location);
			}
			return parent;
		}

		public IUnresolvedMember GetMember (TextLocation location)
		{
			IUnresolvedTypeDefinition type = GetInnermostTypeDefinition(location);
			if (type == null)
				return null;
			return FindEntity(type.Members, location);
		}

		public ITypeResolveContext GetTypeResolveContext (ICompilation compilation, TextLocation loc)
		{
			ITypeResolveContext rctx = new TypeScriptTypeResolveContext (compilation.MainAssembly);
			//rctx = rctx.WithUsingScope (GetUsingScope (loc).Resolve (compilation));
			var curDef = GetInnermostTypeDefinition (loc);
			if (curDef != null) {
				var resolvedDef = curDef.Resolve (rctx).GetDefinition ();
				if (resolvedDef == null)
					return rctx;
				rctx = rctx.WithCurrentTypeDefinition (resolvedDef);
				
				var curMember = resolvedDef.Members.FirstOrDefault (m => m.Region.FileName == FileName && m.Region.Begin <= loc && loc < m.BodyRegion.End);
				if (curMember != null)
					rctx = rctx.WithCurrentMember (curMember);
			}
			
			return rctx;
		}

		public string FileName {
			get { return fileName; }
		}
		
		DateTime? lastWriteTime;
		
		public DateTime? LastWriteTime {
			get { return lastWriteTime; }
			set {
				//FreezableHelper.ThrowIfFrozen(this);
				lastWriteTime = value;
			}
		}

		public IList<IUnresolvedTypeDefinition> TopLevelTypeDefinitions {
			get { return topLevelTypeDefinitions; }
		}

		public IList<IUnresolvedAttribute> AssemblyAttributes {
			get { return assemblyAttributes; }
		}

		public IList<IUnresolvedAttribute> ModuleAttributes {
			get { return moduleAttributes; }
		}
		
		public IList<Error> Errors {
			get { return errors; }
			internal set { errors = (List<Error>) value; }
		}

		#endregion

		#region IUnresolvedDocumentationProvider implementation

		public string GetDocumentation (IUnresolvedEntity entity)
		{
			if (entity == null)
				throw new ArgumentNullException("entity");
			if (documentation == null)
				return null;
			string xmlDoc;
			if (documentation.TryGetValue(entity, out xmlDoc))
				return xmlDoc;
			else
				return null;
		}

		public DocumentationComment GetDocumentation (IUnresolvedEntity entity, IEntity resolvedEntity)
		{
			if (entity == null)
				throw new ArgumentNullException("entity");
			if (resolvedEntity == null)
				throw new ArgumentNullException("resolvedEntity");
			string xmlDoc = GetDocumentation(entity);
			if (xmlDoc == null)
				return null;
			var unresolvedTypeDef = entity as IUnresolvedTypeDefinition ?? entity.DeclaringTypeDefinition;
			var resolvedTypeDef = resolvedEntity as ITypeDefinition ?? resolvedEntity.DeclaringTypeDefinition;
			/*if (unresolvedTypeDef != null && resolvedTypeDef != null) {
				// Strictly speaking, we would have to pass the parent context into CreateResolveContext,
				// then transform the result using WithTypeDefinition().
				// However, we can simplify this here because we know this is a C# type definition.
				var context = unresolvedTypeDef.CreateResolveContext(new SimpleTypeResolveContext(resolvedTypeDef));
				if (resolvedEntity is IMember)
					context = context.WithCurrentMember((IMember)resolvedEntity);
				return new CSharpDocumentationComment(new StringTextSource(xmlDoc), context);
			} else*/ {
				return new DocumentationComment(new StringTextSource(xmlDoc), new SimpleTypeResolveContext(resolvedEntity));
			}
		}

		#endregion
	}
}
