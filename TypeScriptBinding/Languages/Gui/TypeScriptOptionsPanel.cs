using System;
using MonoDevelop.Core;
using MonoDevelop.Ide.Gui.Dialogs;
using Gtk;

namespace MonoDevelop.TypeScriptBinding.Languages.Gui
{
    public class TypeScriptOptionsPanel : OptionsPanel
    {
        TypeScriptOptionsWidget mWidget;

        public override Gtk.Widget CreatePanelWidget()
        {
            return mWidget = new TypeScriptOptionsWidget();
        }

        public override void ApplyChanges()
        {
            mWidget.Store();
        }
    }

    [System.ComponentModel.Category("TypeScriptBinding")]
    [System.ComponentModel.ToolboxItem(true)]
    public partial class TypeScriptOptionsWidget : Gtk.Bin
    {
        public TypeScriptOptionsWidget()
        {
            this.Build();

			TscLocationEntry.Text = PropertyService.Get<string> ("TypeScriptBinding.TscLocation");
			NodeLocationEntry.Text = PropertyService.Get<string> ("TypeScriptBinding.NodeLocation");
        }

        public bool Store()
        {
			PropertyService.Set ("TypeScriptBinding.TscLocation", TscLocationEntry.Text);
			PropertyService.Set ("TypeScriptBinding.NodeLocation", NodeLocationEntry.Text);
            PropertyService.SaveProperties();
            return true;
        }
    }
}
