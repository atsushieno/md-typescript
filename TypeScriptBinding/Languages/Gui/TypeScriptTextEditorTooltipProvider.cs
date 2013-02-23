//
// TypeScriptTextEditorTooltipProvider.cs
//
// Author:
// Atsushi Enomoto <atsushieno@veritas-vos-liberabit.com>
// Mike Krüger <mkrueger@novell.com>
//
// Copyright (c) 2010 Novell, Inc (http://www.novell.com)
// Copyright (c) 2013 Xamarin, Inc (http://xamarin.com)
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
using MonoDevelop.Ide.Gui.Content;
using MonoDevelop.Core;
using Mono.TextEditor;
using System.Text;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide;
using MonoDevelop.Ide.TypeSystem;
using MonoDevelop.Projects;
using MonoDevelop.SourceEditor;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using MonoDevelop.Refactoring;
using MonoDevelop.TypeScriptBinding;
using MonoDevelop.TypeScriptBinding.Languages.Gui;
using MonoDevelop.TypeScriptBinding.Projects;
using TypeScriptServiceBridge.Services;

namespace MonoDevelop.TypeScriptBinding.Languages.Gui
{
	public class TypeScriptTextEditorTooltipProvider : TooltipProvider
	{

		public TypeScriptTextEditorTooltipProvider ()
		{
		}

		TypeScriptService GetService (Project project)
		{
			var tp = project as TypeScriptProject;
			return tp != null ? tp.TypeScriptService : null;
		}

		
		#region ITooltipProvider implementation 
		
		public override TooltipItem GetItem (Mono.TextEditor.TextEditor editor, int offset)
		{
			var service = GetService (((ExtensibleTextEditor) editor).Project);
			if (service != null) {
				service.UpdateScripts ();
				var item = service.LanguageService.GetDefinitionAtPosition (service.GetFilePath (editor.FileName), offset);
				return new TooltipItem (item, (int)item.MinChar, (int)(item.LimChar - item.MinChar));
			}
			return null;
		}
		
		DefinitionInfo lastResult = null;
		TypeScriptLanguageItemWindow lastWindow = null;
		
		public Gtk.Window CreateTooltipWindow (Mono.TextEditor.TextEditor editor, int offset, Gdk.ModifierType modifierState, TooltipItem item)
		{
			var resolveResult = (DefinitionInfo)item.Item;
			var result = new TypeScriptLanguageItemWindow (editor, modifierState, resolveResult);
			lastWindow = result;
			lastResult = resolveResult;
			if (result.IsEmpty)
				return null;
			return result;
		}
		
		public void GetRequiredPosition (Mono.TextEditor.TextEditor editor, Gtk.Window tipWindow, out int requiredWidth, out double xalign)
		{
			TypeScriptLanguageItemWindow win = (TypeScriptLanguageItemWindow) tipWindow;
			requiredWidth = win.SetMaxWidth (win.Screen.Width);
			xalign = 0.5;
		}
		
		public bool IsInteractive (Mono.TextEditor.TextEditor editor, Gtk.Window tipWindow)
		{
			return false;
		}
		
		#endregion 
		
	}
}

