using System;
using System.Windows.Forms;
using Utils.Language;
using Utils.Settings;

namespace Forms.OptionControls
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

        private void AddMaterial(string MaterialPath, bool CheckExists = false)
        {
            if (!MTLListBox.Items.Contains(MaterialPath))
            {
                if (CheckExists == true)
                {
                    if (System.IO.File.Exists(MaterialPath))
                    {
                        MTLListBox.Items.Add(MaterialPath);
                    }
                    else
                    {
                        MessageBox.Show("The Material: " + MaterialPath + " is missing.");
                    }
                }
                else
                {
                    MTLListBox.Items.Add(MaterialPath);
                }
            }
        }

        private void LoadSettings()
        {
            string[] files = GameStorage.Instance.GetSelectedGame().Materials.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string file in files)
            {
                //Avoiding adding duplicate materials
                if (MTLListBox.Items.Contains(file) == true)
                {
                    break;
                }

                MTLListBox.Items.Add(file);
            }

            //Saving the loaded settings
            UpdateINIKey();
        }

        private void AddLibrary_Click(object sender, EventArgs e)
        {
            if (MTLBrowser.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in MTLBrowser.FileNames)
                {
                    MTLListBox.Items.Add(file);
                }
            }

            UpdateINIKey();
        }

        private void RemoveSelected_Click(object sender, EventArgs e)
        {
            if (MTLListBox.SelectedItem == null && MTLListBox.SelectedIndex > 0)
                return;

            MTLListBox.Items.Remove(MTLListBox.SelectedItem);
            UpdateINIKey();
        }

        private void UpdateINIKey()
        {
            string value = "";

            foreach (string file in MTLListBox.Items)
            {
                value += file;
                value += ",";
            }

            GameStorage.Instance.GetSelectedGame().Materials = value;
        }
    }
}
