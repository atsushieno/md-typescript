using System;
using MonoDevelop.Core;
using MonoDevelop.Ide.Gui.Dialogs;
using Gtk;


namespace MonoDevelop.TypeScriptBinding.Projects.Gui
{

	public class TypeScriptOutputOptionsPanel : MultiConfigItemOptionsPanel
	{
		private TypeScriptOutputOptionsWidget mWidget;


		public override Gtk.Widget CreatePanelWidget ()
		{
			return (mWidget = new TypeScriptOutputOptionsWidget ());
		}


		public override void LoadConfigData ()
		{
			mWidget.Load ((TypeScriptProjectConfiguration)CurrentConfiguration);
		}


		public override void ApplyChanges ()
		{
			mWidget.Store ();
		}
	}

	[System.ComponentModel.Category("TypeScriptBinding")]
    [System.ComponentModel.ToolboxItem(true)]
    public partial class TypeScriptOutputOptionsWidget : Gtk.Bin
	{
		TypeScriptProjectConfiguration mConfiguration;


		public TypeScriptOutputOptionsWidget ()
		{
			//this.Build ();
			this.Show ();
		}


		public void Load (TypeScriptProjectConfiguration configuration)
		{
			mConfiguration = configuration;
			
			AdditionalArgumentsEntry.Text = configuration.AdditionalArguments;
		}


		public void Store ()
		{
			if (mConfiguration == null)
				return;
			
			mConfiguration.AdditionalArguments = AdditionalArgumentsEntry.Text.Trim ();
		}
		
	}
	
}