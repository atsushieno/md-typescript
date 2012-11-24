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

		
		protected void OnTargetJavaScriptFileButtonClicked (object sender, System.EventArgs e)
		{
			Gtk.FileChooserDialog fc =
                new Gtk.FileChooserDialog ("Target JavaScript file", this.Toplevel as Gtk.Window, FileChooserAction.Open,
                    "Cancel", ResponseType.Cancel,
                    "Select", ResponseType.Accept);
			
			Gtk.FileFilter filterJS = new Gtk.FileFilter ();
			filterJS.Name = "JavaScript Files";
			filterJS.AddPattern ("*.js");
			
			Gtk.FileFilter filterAll = new Gtk.FileFilter ();
			filterAll.Name = "All Files";
			filterAll.AddPattern ("*");
			
			fc.AddFilter (filterJS);
			fc.AddFilter (filterAll);
			
			if (mProject.TargetJavaScriptFile != "")
			{
				fc.SetFilename (mProject.TargetJavaScriptFile);
			}
			else
			{
				fc.SetFilename (mProject.BaseDirectory);
			}

			if (fc.Run () == (int)ResponseType.Accept)
			{
				string path = PathHelper.ToRelativePath (fc.Filename, mProject.BaseDirectory);
				
				TargetJavaScriptFileEntry.Text = path;
			}

			fc.Destroy ();
		}
		
	}
	
}
