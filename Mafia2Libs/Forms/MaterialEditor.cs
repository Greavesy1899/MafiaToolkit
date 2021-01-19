using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Gibbed.Illusion.FileFormats.Hashing;
using Forms.EditorControls;
using ResourceTypes.Materials;
using Utils.Language;
using Utils.Settings;
using System.Linq;
using Utils.Extensions;

namespace Mafia2Tool
{
    public partial class MaterialEditor : Form
    {
        private MaterialLibrary mtl;
        private int currentSearchType;

        public MaterialEditor(FileInfo file)
        {
            InitializeComponent();
            Localise();

            // We try and grab the library from our storage.
            mtl = MaterialsManager.MaterialLibraries.TryGet(file.FullName);

            // If it doesn't exist, then we should try and read it as a fallback.
            if(mtl == null)
            {
                // Version will be replaced when reading file
                mtl = new MaterialLibrary(VersionsEnumerator.V_57);
                mtl.ReadMatFile(file.FullName);
            }

            FetchMaterials();
            Show();
            Panel_Main.Visible = true;
            MergePanel.Visible = false;
            ComboBox_SearchType.SelectedIndex = currentSearchType = 0;
            ToolkitSettings.UpdateRichPresence("Using the Material Library editor.");
        }

        private void Localise()
        {
            contextFileButton.Text = Language.GetString("$FILE");
            contextOpenButton.Text = Language.GetString("$OPEN");
            contextSaveButton.Text = Language.GetString("$SAVE");
            contextExitButton.Text = Language.GetString("$EXIT");
            toolButton.Text = Language.GetString("$TOOLS");
            addMaterialToolStripMenuItem.Text = Language.GetString("$MATERIAL_ADD");
            Text = Language.GetString("$MATERIAL_EDITOR_TITLE");
            DeleteSelectedMaterialButton.Text = Language.GetString("$MATERIAL_DELETE");
            CancelButton.Text = Language.GetString("$CANCEL");
            MergeButton.Text = Language.GetString("$MERGE");
            MergeMTLButton.Text = Language.GetString("$MERGE_MTL");
            OverWriteLabel.Text = Language.GetString("$CONFLICTING_MATS");
            NewMaterialLabel.Text = Language.GetString("$NEW_MATS");
            SelectAllNewButton.Text = SelectAllOverwriteButton.Text = Language.GetString("$SELECT_ALL");
            Button_ExportSelected.Text = Language.GetString("$EXPORT_MATS");
            Label_SearchType.Text = Language.GetString("$LABEL_SEARCHTYPE");
            Button_Search.Text = Language.GetString("$SEARCH");

            for(int i = 0; i < ComboBox_SearchType.Items.Count; i++)
            {
                var text = (ComboBox_SearchType.Items[i] as string);
                text = Language.GetString(text);
                ComboBox_SearchType.Items[i] = text;
            }
        }

        public void FetchMaterials()
        {
            dataGridView1.Rows.Clear();

            foreach(var Pair in mtl.Materials)
            {
                dataGridView1.Rows.Add(BuildRowData(Pair.Value));
            }
        }

        private void SearchForMaterials(string text = null)
        {
            dataGridView1.Rows.Clear();

            IMaterial[] Filtered = mtl.SelectSearchTypeAndProceedSearch(text, currentSearchType);

            foreach (IMaterial Mat in Filtered)
            {
                dataGridView1.Rows.Add(BuildRowData(Mat));
            }
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            MaterialData.Load();
            Dispose();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (!Panel_Main.Visible)
            {
                MessageBox.Show("Complete the merge to save!");
                return;
            }

            mtl.WriteMatFile(mtl.Name);
        }

        private void AddMaterial(object sender, EventArgs e)
        {
            if (!Panel_Main.Visible)
            {
                return;
            }

            // Ask user for material name.
            NewObjectForm form = new NewObjectForm(true);
            form.SetLabel(Language.GetString("$QUESTION_NAME_OF_MAT"));
            form.LoadOption(new MaterialAddOption());
            
            if(form.ShowDialog() == DialogResult.OK)
            {
                if (mtl.Materials.ContainsKey(FNV64.Hash(form.GetInputText())))
                {
                    MessageBox.Show("Found duplicate material. Will not be adding new material!", "Toolkit");
                    return;
                }

                // Create material with new name.
                IMaterial mat = MaterialFactory.ConstructMaterial(mtl.Version);
                mat.SetName(form.GetInputText());

                mtl.Materials.Add(mat.GetMaterialHash(), mat);
                dataGridView1.Rows.Add(BuildRowData(mat));
            }

            // Cleanup and reload.
            form.Dispose();
        }

        private void DeleteMaterial(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells[0] == null || !Panel_Main.Visible)
            {
                return;
            }

            int index = dataGridView1.SelectedCells[0].RowIndex;
            mtl.Materials.Remove((dataGridView1.Rows[index].Tag as IMaterial).GetMaterialHash());
            dataGridView1.Rows.RemoveAt(index);
        }

        private void UpdateList(object sender, EventArgs e)
        {
            if (!Panel_Main.Visible)
            {
                return;
            }

            FetchMaterials();
        }

        private void OnMaterialSelected(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.RowIndex > -1) && (e.ColumnIndex > -1))
            {
                MaterialGrid.SelectedObject = dataGridView1.Rows[e.RowIndex].Tag;
                IMaterial mat = (dataGridView1.Rows[e.RowIndex].Tag as IMaterial);
            }
        }

        private DataGridViewRow BuildRowData(IMaterial mat)
        {
            DataGridViewRow row = new DataGridViewRow();
            row.Tag = mat;
            row.CreateCells(dataGridView1, new object[] { mat.MaterialName, mat.GetMaterialHash() });
            return row;
        }

        private void MergeMTLButton_Click(object sender, EventArgs e)
        {
            if (!Panel_Main.Visible)
            {
                return;
            }

            // Version will be replaced when loaded
            MaterialLibrary matLib = new MaterialLibrary(VersionsEnumerator.V_57);
            if(MTLBrowser.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    matLib.ReadMatFile(MTLBrowser.FileName);
                }
                catch
                {
                    MessageBox.Show("Failed to load the selected .MTL!");
                }
            }
            else
            {
                return;
            }

            if (matLib.Materials.Count == 0)
            {
                MessageBox.Show("Failed to load the selected .MTL!");
                return;
            }

            MergePanel.Visible = true;
            Panel_Main.Visible = false;
            OverwriteListBox.Items.Clear();
            NewMatListBox.Items.Clear();

            for(int i = 0; i < matLib.Materials.Count; i++)
            {
                var mat = matLib.Materials.ElementAt(i).Value;
                if (mtl.Materials.ContainsKey(mat.GetMaterialHash()))
                {
                    OverwriteListBox.Items.Add(mat);
                }
                else
                {
                    NewMatListBox.Items.Add(mat);
                }
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            if(MergePanel.Visible)
            {
                Panel_Main.Visible = true;
                MergePanel.Visible = false;
                OverwriteListBox.Items.Clear();
                NewMatListBox.Items.Clear();
            }
        }

        private void MergeButton_Click(object sender, EventArgs e)
        {
            if(MergePanel.Visible)
            {
                Panel_Main.Visible = true;
                MergePanel.Visible = false;

                for(int i = 0; i < NewMatListBox.CheckedItems.Count; i++)
                {
                    var mat = (NewMatListBox.CheckedItems[i] as IMaterial);
                    mtl.Materials.Add(mat.GetMaterialHash(), mat);
                }

                for(int i = 0; i < OverwriteListBox.CheckedItems.Count; i++)
                {
                    var mat = (OverwriteListBox.CheckedItems[i] as IMaterial);
                    mtl.Materials[mat.GetMaterialHash()] = mat;
                }

                OverwriteListBox.Items.Clear();
                NewMatListBox.Items.Clear();
            }
        }

        private void SelectAllOverwriteButton_Click(object sender, EventArgs e)
        {
            if(MergePanel.Visible)
            {
                for (int i = 0; i < OverwriteListBox.Items.Count; i++)
                {
                    OverwriteListBox.SetItemChecked(i, true);
                }
            }
        }

        private void SelectAllNewButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < NewMatListBox.Items.Count; i++)
            {
                NewMatListBox.SetItemChecked(i, true);
            }
        }

        private void SearchType_OnIndexChanged(object sender, EventArgs e)
        {
            currentSearchType = ComboBox_SearchType.SelectedIndex;
        }

        private void Button_ExportedSelected_Clicked(object sender, EventArgs e)
        {
            MaterialLibrary library = new MaterialLibrary(mtl.Version);

            foreach(DataGridViewCell cell in dataGridView1.SelectedCells)
            {
                if(cell.ColumnIndex == 0)
                {
                    var material = (cell.OwningRow.Tag as IMaterial);

                    if (material != null)
                    {
                        library.Materials.Add(material.GetMaterialHash(), material);
                    }
                }
            }
            
            if(MTLSaveDialog.ShowDialog() == DialogResult.OK)
            {
                library.WriteMatFile(MTLSaveDialog.FileName);
            }
        }

        private void Button_Search_Click(object sender, EventArgs e)
        {
            string Filtered = MaterialSearch.Text;

            if (!string.IsNullOrEmpty(Filtered))
            {
                SearchForMaterials(MaterialSearch.Text);
            }
            else
            {
                FetchMaterials();
            }
        }

        private void MaterialGrid_OnPropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            MaterialGrid.Refresh();
        }

        private void Button_DumpTextures_Click(object sender, EventArgs e)
        {
            if (mtl.Version != VersionsEnumerator.V_63)
            {
                return;
            }

            Dictionary<ulong, bool> CurrentTextures = new Dictionary<ulong, bool>();

            using (StreamWriter Writer = new StreamWriter(File.Open("TextureDump.txt", FileMode.Create)))
            {
                foreach (var Material in mtl.Materials)
                {
                    if (Material.Value is Material_v63)
                    {
                        Material_v63 Mat = (Material.Value as Material_v63);

                        foreach(var Texture in Mat.Textures)
                        {
                            if (!CurrentTextures.ContainsKey(Texture.TextureName.Hash))
                            {
                                Writer.WriteLine(Texture.TextureName.String);
                                CurrentTextures.Add(Texture.TextureName.Hash, true);
                            }
                        }
                    }
                }
            }
        }

        private void MaterialSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Button_Search.PerformClick();
            }
        }
    }
}
