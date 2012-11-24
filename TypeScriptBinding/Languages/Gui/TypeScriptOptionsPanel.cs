using System;
using MonoDevelop.Core;
using MonoDevelop.Ide.Gui.Dialogs;
using Gtk;


// This file was taken from the old "FlexBinding" add-in and has not been adapted yet


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

        /*protected virtual void OnWSdkPathButtonClicked (object sender, System.EventArgs e)
        {
            Gtk.FileChooserDialog fc =
                new Gtk.FileChooserDialog("Flex SDK Path", this.Toplevel as Gtk.Window, FileChooserAction.SelectFolder,
                    "Cancel", ResponseType.Cancel,
                    "Select", ResponseType.Accept);
            if (!String.IsNullOrEmpty(wSdkPathEntry.Text))
                fc.SetFilename(wSdkPathEntry.Text);

            if (fc.Run() == (int)ResponseType.Accept) {
                wSdkPathEntry.Text = fc.Filename;
            }

            fc.Destroy();
        }

        protected virtual void OnWPlayerPathButtonClicked (object sender, System.EventArgs e)
        {
            Gtk.FileChooserDialog fc =
                new Gtk.FileChooserDialog("Standalone Player Path", this.Toplevel as Gtk.Window, FileChooserAction.Open,
                    "Cancel", ResponseType.Cancel,
                    "Select", ResponseType.Accept);
            if (!String.IsNullOrEmpty(wSdkPathEntry.Text))
                fc.SetFilename(wPlayerPathEntry.Text);

            if (fc.Run() == (int)ResponseType.Accept) {
                wPlayerPathEntry.Text = fc.Filename;
            }

            fc.Destroy();
        }

        protected virtual void OnWBrowserPathButtonClicked (object sender, System.EventArgs e)
        {
            Gtk.FileChooserDialog fc =
                new Gtk.FileChooserDialog("Browser Path", this.Toplevel as Gtk.Window, FileChooserAction.Open,
                    "Cancel", ResponseType.Cancel,
                    "Select", ResponseType.Accept);
            if (!String.IsNullOrEmpty(wBrowserPathEntry.Text))
                fc.SetFilename(wBrowserPathEntry.Text);

            if (fc.Run() == (int)ResponseType.Accept) {
                wBrowserPathEntry.Text = fc.Filename;
            }

            fc.Destroy();
        }*/
    }
}
