using System;
using System.IO;
using MonoDevelop.Core;
using MonoDevelop.Ide.Gui.Dialogs;
using Gtk;


namespace MonoDevelop.TypeScriptBinding.Projects.Gui
{

	public class TypeScriptProjectOptionsPanel : ItemOptionsPanel
	{
		TypeScriptProjectOptionsWidget mWidget;


		public override Gtk.Widget CreatePanelWidget ()
		{	
			mWidget = new TypeScriptProjectOptionsWidget ();
			mWidget.Load ((TypeScriptProject)ConfiguredProject);
			return mWidget;
		}


		public override void ApplyChanges ()
		{
			mWidget.Store ();
		}
	}

	[System.ComponentModel.Category("TypeScriptBinding")]
    [System.ComponentModel.ToolboxItem(true)]
    public partial class TypeScriptProjectOptionsWidget : Gtk.Bin
	{
		TypeScriptProject mProject;


		public TypeScriptProjectOptionsWidget ()
		{
			this.Build ();
		}


		public void Load (TypeScriptProject project)
		{
			mProject = project;
			
			TargetJavaScriptFileEntry.Text = mProject.TargetJavaScriptFile;
			AdditionalArgumentsEntry.Text = mProject.AdditionalArguments;
		}


		public void Store ()
		{
			if (mProject == null)
				return;
			
			mProject.TargetJavaScriptFile = TargetJavaScriptFileEntry.Text.Trim ();
			mProject.AdditionalArguments = AdditionalArgumentsEntry.Text.Trim ();
		}
	}
	
}
