using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;

using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Gui.Content;
using MonoDevelop.Ide.CodeCompletion;
using MonoDevelop.Components;
using MonoDevelop.Components.Commands;

using Mono.TextEditor;
using MonoDevelop.Ide.TypeSystem;
using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.NRefactory.Completion;

namespace MonoDevelop.TypeScriptBinding.Languages.Gui
{
	public class TypeScriptCompletionTextEditorExtension : CompletionTextEditorExtension//, IPathedDocument
	{
		public TypeScriptCompletionTextEditorExtension ()
		{
		}

		/*
		#region IPathedDocument implementation

		public event EventHandler<DocumentPathChangedEventArgs> PathChanged;

		public Gtk.Widget CreatePathWidget (int index)
		{
			throw new NotImplementedException ();
		}

		public PathEntry[] CurrentPath {
			get {
					throw new NotImplementedException ();
			}
		}

		#endregion
		*/
	}
}
