using System;
using ResourceTypes.Materials;
using System.Windows.Forms;
using System.Linq;
using System.Diagnostics;

namespace Mafia2Tool.Forms
{
    public partial class MatBrowser : Form
    {
        MaterialLibrary SelectedLibrary = null;
        TextureEntry SelectedEntry = null;
        Material SelectedMaterial = null;

        public MatBrowser()
        {
            InitializeComponent();
            Init();
            ShowDialog();
        }

        private void Init()
        {
            // Populate Material Libraries;
            for (int i = 0; i < MaterialsManager.MaterialLibraries.Count; i++)
            {
                var lib = MaterialsManager.MaterialLibraries.ElementAt(i).Value;
                ComboBox_Materials.Items.Add(lib.Name);
            }

            ComboBox_Materials.SelectedIndex = 0;
            Label_MaterialCount.Text = "";
        }

        public Material GetSelectedMaterial()
        {
            return SelectedMaterial;
        }

        private void PopulateBrowser(Material[] materials)
        {
            FlowPanel_Materials.Controls.Clear();

            for (int x = 0; x < materials.Length; x++)
            {
                var mat = materials[x];
                TextureEntry textEntry = new TextureEntry();
                textEntry.SetMaterial(mat);
                textEntry.WasClicked += TextureEntry_WasClicked;
                FlowPanel_Materials.Controls.Add(textEntry);
            }
        }

        private void TextureEntry_WasClicked(object sender, EventArgs e)
        {
            // Add the new selected one
            TextureEntry Entry = (sender as TextureEntry);

            // Remove the previous entry, if it is the same then we select this and exit the browser
            if (SelectedEntry != null)
            {
                SelectedEntry.IsSelected = false;

                if (SelectedEntry == Entry)
                {
                    Debug.Assert(sender == SelectedEntry, "The sent control should be the same as SelectedMaterial, but it is not!");
                    SelectedMaterial = SelectedEntry.GetMaterial();
                    Close();
                }
            }

            SelectedEntry = Entry;
        }

        private void Button_SearchOnClicked(object sender, EventArgs e)
        {
            string text = TextBox_SearchBar.Text;
            Material[] filtered = SelectedLibrary.SearchForMaterialsByName(text);
            PopulateBrowser(filtered);
            Label_MaterialCount.Text = string.Format("(Found: {0} Materials)", filtered.Length);
        }

        private void ComboBox_MaterialsSelectedIndexChanged(object sender, EventArgs e)
        {
            int index = ComboBox_Materials.SelectedIndex;
            SelectedLibrary = MaterialsManager.MaterialLibraries.ElementAt(index).Value;
        }
    }
}
