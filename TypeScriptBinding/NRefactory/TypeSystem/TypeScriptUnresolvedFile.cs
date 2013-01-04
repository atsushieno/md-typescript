using System;
using System.Linq;
using ICSharpCode.NRefactory.TypeSystem;
using TypeScriptServiceBridge.TypeScript;
using MonoDevelop.TypeScriptBinding.NRefactory.TypeSystem;
using ICSharpCode.NRefactory;
using MonoDevelop.Ide.TypeSystem;
using System.Collections.Generic;

namespace MonoDevelop.TypeScriptBinding
{
	public class TypeScriptUnresolvedFile : IUnresolvedFile
	{
		TypeScriptService service;
		string file;
		
		public TypeScriptUnresolvedFile (TypeScriptService service, string file)
		{
			this.service = service;
			this.file = file;
		}

		#region IUnresolvedFile implementation

		AstPath GetAstPath (TextLocation location)
		{
			var ast = service.LanguageService.GetScriptAST (file);
			var pos = service.ShimHost.LineColToPosition (file, location.Line, location.Column);
			var path = service.LanguageService.GetAstPathToPosition (ast, pos, GetAstPathOptions.Default);
			return path;
		}

		public IUnresolvedTypeDefinition GetInnermostTypeDefinition (ICSharpCode.NRefactory.TextLocation location)
		{
			var path = GetAstPath (location);
			foreach (var ap in path.Asts.Reverse ())
				if (ap.Type != null)
					return new TypeScriptUnresolvedTypeDefinition (service, ap);
			return null;
		}
		
		public IUnresolvedMember GetMember (TextLocation location)
		{
			throw new NotImplementedException ();
		}
		
		TextLocation ToTextLocation (double pos)
		{
			var lc = service.ShimHost.PositionToLineCol (file, pos);
			return new TextLocation ((int) lc.Line, (int) lc.Col);
		}

		public ITypeResolveContext GetTypeResolveContext (ICompilation compilation, TextLocation loc)
		{
			throw new NotImplementedException ();
		}

		public string FileName {
			get { return file; }
		}

		public DateTime? LastWriteTime { get; set; }

		public System.Collections.Generic.IList<IUnresolvedTypeDefinition> TopLevelTypeDefinitions {
			get {
				throw new NotImplementedException ();
			}
		}

		public System.Collections.Generic.IList<IUnresolvedAttribute> AssemblyAttributes {
			get { return new IUnresolvedAttribute [0]; }
		}

		public System.Collections.Generic.IList<IUnresolvedAttribute> ModuleAttributes {
			get { return new IUnresolvedAttribute [0]; }
		}

		public IUnresolvedTypeDefinition GetTopLevelTypeDefinition (TextLocation location)
		{
			throw new NotImplementedException ();
		}

		public IList<Error> Errors {
			get {
				var errors = service.LanguageService.GetScriptErrors (file, short.MaxValue);
				return errors.Select (e => new Error (ErrorType.Unknown, e.Message)).ToArray ();
			}
		}

		#endregion
	}
}

