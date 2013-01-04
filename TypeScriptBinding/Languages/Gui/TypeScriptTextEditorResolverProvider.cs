using System;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Gui.Content;
using MonoDevelop.Projects;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using MonoDevelop.TypeScriptBinding;
using MonoDevelop.TypeScriptBinding.Projects;
using MonoDevelop.TypeScriptBinding.NRefactory.TypeSystem;
using TypeScriptServiceBridge.Services;

namespace MonoDevelop.TypeScriptBinding.Languages.Gui
{
	public class TypeScriptTextEditorResolverProvider : ITextEditorResolverProvider
	{
		public TypeScriptTextEditorResolverProvider ()
		{
		}

		TypeScriptService GetService (Project project)
		{
			var tp = project as TypeScriptProject;
			return tp != null ? tp.TypeScriptService : null;
		}

		#region ITextEditorResolverProvider implementation

		public ResolveResult GetLanguageItem (Document document, int offset, out DomRegion expressionRegion)
		{
			expressionRegion = default (DomRegion);

			var service = GetService (document.Project);
			if (service == null)
				return null;
			var file = service.GetFilePath (document.FileName);

			var def = service.LanguageService.GetDefinitionAtPosition (file, offset);
			var result = new ResolveResult (new TypeScriptType (def));
			return result;
		}

		public ResolveResult GetLanguageItem (Document document, int offset, string identifier)
		{
			var service = GetService (document.Project);
			if (service == null)
				return null;
			var file = service.GetFilePath (document.FileName);

			var def = service.LanguageService.GetDefinitionAtPosition (file, offset);
			var result = new ResolveResult (new TypeScriptType (def));
			return result;
		}
		
		string GetTooltopString (DefinitionInfo info)
		{
			return string.Format ("{0} {1} (in {2} {3})",
			                      info.Kind,
			                      info.Name,
			                      info.ContainerKind,
			                      info.ContainerName);
		}

		public string CreateTooltip (Document document, int offset, ResolveResult result, string errorInformations, Gdk.ModifierType modifierState)
		{
			var service = GetService (document.Project);
			if (service == null)
				return null;
			var file = service.GetFilePath (document.FileName);

			var def = service.LanguageService.GetDefinitionAtPosition (file, offset);
			return GetTooltopString (def);
		}

		#endregion
	}
}

