using System;
using System.Linq;
using System.Windows.Forms;
using ResourceTypes.Materials;
using Utils.Language;
using Utils.Logging;

namespace Mafia2Tool.Forms
{
    public partial class MaterialBrowser : Form
    {
        MaterialLibrary SelectedLibrary = null;
        TextureEntry SelectedEntry = null;
        IMaterial SelectedMaterial = null;

        public MaterialBrowser(string materialName)
        {
            InitializeComponent();
            Init(materialName);
            Localise();
            ShowDialog();
        }

        private void Init(string materialName)
        {
            // Populate Material Libraries;
            for (int i = 0; i < MaterialsManager.MaterialLibraries.Count; i++)
            {
                var lib = MaterialsManager.MaterialLibraries.ElementAt(i).Value;
                ComboBox_Materials.Items.Add(lib.Name);
            }

            ComboBox_Materials.SelectedIndex = 0;
            ComboBox_SearchType.SelectedIndex = 0;
            Label_MaterialCount.Text = "";
            TextBox_SearchBar.Text = materialName;

            PerformSearch();
        }

        private void Localise()
        {
            Button_Search.Text = Language.GetString("$SEARCH");
            Label_SearchBar.Text = Language.GetString("$LABEL_SEARCHBAR");
            Label_SelectMatLib.Text = Language.GetString("$LABEL_SELECTMATLIBRARY");
            Label_SearchType.Text = Language.GetString("$LABEL_SEARCHTYPE");
            Text = Language.GetString("$TITLE_MATERIALBROWSER");

            for (int i = 0; i < ComboBox_SearchType.Items.Count; i++)
            {
                var text = (ComboBox_SearchType.Items[i] as string);
                text = Language.GetString(text);
                ComboBox_SearchType.Items[i] = text;
            }
        }

        public IMaterial GetSelectedMaterial()
        {
            return SelectedMaterial;
        }

        private void PopulateBrowser(IMaterial[] materials)
        {
            FlowPanel_Materials.Controls.Clear();

            for (int x = 0; x < materials.Length; x++)
            {
                var mat = materials[x];
                TextureEntry textEntry = new TextureEntry();
                textEntry.SetMaterial(mat);
                textEntry.OnEntrySingularClick += TextureEntry_OnSingularClick;
                textEntry.OnEntryDoubleClick += TextureEntry_OnDoubleClick;
                FlowPanel_Materials.Controls.Add(textEntry);
            }
        }

        private void PerformSearch()
        {
            string text = TextBox_SearchBar.Text;

            // We should not search if the search bar is empty, or we'll get some terrible results!
            if (!string.IsNullOrEmpty(text))
            {
                IMaterial[] filtered = SelectedLibrary.SelectSearchTypeAndProceedSearch(text, ComboBox_SearchType.SelectedIndex);
                PopulateBrowser(filtered);
                Label_MaterialCount.Text = string.Format("(Found: {0} Materials)", filtered.Length);
            }
        }

        private void TextureEntry_OnDoubleClick(object sender, EventArgs e)
        {
            // Add the new selected one
            ToolkitAssert.Ensure(sender == SelectedEntry, "The sent control should be the same as SelectedMaterial, but it is not!");
            SelectedMaterial = SelectedEntry.GetMaterial();
            Close();
        }

        private void TextureEntry_OnSingularClick(object sender, EventArgs e)
        {
            // Add the new selected one
            TextureEntry Entry = (sender as TextureEntry);

            // Remove the previous entry, if it is the same then we select this and exit the browser
            if (SelectedEntry != null)
            {
                SelectedEntry.IsSelected = false;
            }

            SelectedEntry = Entry;
        }

        private void Button_SearchOnClicked(object sender, EventArgs e)
        {
            PerformSearch();
        }

        private void ComboBox_MaterialsSelectedIndexChanged(object sender, EventArgs e)
        {
            int index = ComboBox_Materials.SelectedIndex;
            SelectedLibrary = MaterialsManager.MaterialLibraries.ElementAt(index).Value;
        }
    }
}
