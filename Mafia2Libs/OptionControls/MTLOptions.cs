using System;
using System.Windows.Forms;

namespace Mafia2Tool.OptionControls
{
    public partial class MTLOptions : UserControl
    {
        public MTLOptions()
        {
            InitializeComponent();
            Localise();
            LoadSettings();
        }

        private void Localise()
        {
            groupMTL.Text = Language.GetString("$MATERIAL_LIBS");
            removeSelectedButton.Text = Language.GetString("$MATERIAL_LIB_REMOVE");
            addLibraryButton.Text = Language.GetString("$MATERIAL_LIB_ADD");
            MTLsToLoadText.Text = Language.GetString("$MATERIAL_LIB_SELECTED");
        }

        private void LoadSettings()
        {
            string[] files = ToolkitSettings.MaterialLibs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach(string file in files)
            {
                MTLListBox.Items.Add(file);
            }
        }

        private void addLibrary_Click(object sender, EventArgs e)
        {
            if(MTLBrowser.ShowDialog() == DialogResult.OK)
            {
                foreach(string file in MTLBrowser.FileNames)
                {
                    MTLListBox.Items.Add(file);
                }
            }
            UpdateINIKey();
        }

        private void removeSelected_Click(object sender, EventArgs e)
        {
            if (MTLListBox.SelectedItem == null && MTLListBox.SelectedIndex > 0)
                return;

            MTLListBox.Items.Remove(MTLListBox.SelectedItem);
            UpdateINIKey();
        }

        private void UpdateINIKey()
        {
            string value = "";

            foreach(string file in MTLListBox.Items)
            {
                value += file;
                value += ",";
            }
            ToolkitSettings.MaterialLibs = value;
            ToolkitSettings.WriteKey("MaterialLibs", "Materials", value);
        }
    }
}
