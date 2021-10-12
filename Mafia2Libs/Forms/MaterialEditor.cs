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

namespace Toolkit
{
    public partial class MaterialEditor : Form
    {
        private FileInfo MaterialFile;
        private MaterialLibrary mtl;
        private int currentSearchType;

        private bool bIsFileEdited = false;

        public MaterialEditor(FileInfo file)
        {
            InitializeComponent();
            MaterialFile = file;
            BuildData();
            Localise();
            Show();
            Panel_Main.Visible = true;
            MergePanel.Visible = false;
            ComboBox_SearchType.SelectedIndex = currentSearchType = 0;
            ToolkitSettings.UpdateRichPresence("Using the Material Library editor.");
        }

        private void Localise()
        {
            Button_File.Text = Language.GetString("$FILE");
            Button_Open.Text = Language.GetString("$OPEN");
            Button_Save.Text = Language.GetString("$SAVE");
            Button_Exit.Text = Language.GetString("$EXIT");
            toolButton.Text = Language.GetString("$TOOLS");
            Button_AddMaterial.Text = Language.GetString("$MATERIAL_ADD");
            Text = Language.GetString("$MATERIAL_EDITOR_TITLE");
            Button_Delete.Text = Language.GetString("$MATERIAL_DELETE");
            CancelButton.Text = Language.GetString("$CANCEL");
            MergeButton.Text = Language.GetString("$MERGE");
            Button_MergeMTL.Text = Language.GetString("$MERGE_MTL");
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

        public void BuildData()
        {
            // We try and grab the library from our storage.
            mtl = MaterialsManager.MaterialLibraries.TryGet(MaterialFile.FullName);

            // If it doesn't exist, then we should try and read it as a fallback.
            if (mtl == null)
            {
                // Version will be replaced when reading file
                mtl = new MaterialLibrary(VersionsEnumerator.V_57);
                mtl.ReadMatFile(MaterialFile.FullName);
            }

            FetchMaterials();
        }

        public void FetchMaterials()
        {
            GirdView_Materials.Rows.Clear();

            foreach(var Pair in mtl.Materials)
            {
                GirdView_Materials.Rows.Add(BuildRowData(Pair.Value));
            }
        }

        private void SearchForMaterials(string text = null)
        {
            GirdView_Materials.Rows.Clear();

            IMaterial[] Filtered = mtl.SelectSearchTypeAndProceedSearch(text, currentSearchType);

            foreach (IMaterial Mat in Filtered)
            {
                GirdView_Materials.Rows.Add(BuildRowData(Mat));
            }
        }

        private void FileIsEdited()
        {
            Text = Language.GetString("$MATERIAL_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

        private void FileIsNotEdited()
        {
            Text = Language.GetString("$MATERIAL_EDITOR_TITLE");
            bIsFileEdited = false;
        }

        private void Save()
        {
            if (!Panel_Main.Visible)
            {
                MessageBox.Show("Complete the merge to save!");
                return;
            }

            mtl.WriteMatFile(mtl.Name);

            FileIsNotEdited();
        }

        private void Reload()
        {
            if (!Panel_Main.Visible)
            {
                return;
            }

            GirdView_Materials.ClearSelection();
            MaterialGrid.SelectedObject = null;

            BuildData();

            FileIsNotEdited();
        }

        private void Delete()
        {
            if (GirdView_Materials.SelectedCells[0] == null || !Panel_Main.Visible)
            {
                return;
            }

            int index = GirdView_Materials.SelectedCells[0].RowIndex;
            mtl.Materials.Remove((GirdView_Materials.Rows[index].Tag as IMaterial).GetMaterialHash());
            GirdView_Materials.Rows.RemoveAt(index);

            FileIsEdited();
        }

        private void AddMaterial()
        {
            if (!Panel_Main.Visible)
            {
                return;
            }

            // Ask user for material name.
            NewObjectForm form = new NewObjectForm(true);
            form.SetLabel(Language.GetString("$QUESTION_NAME_OF_MAT"));
            form.LoadOption(new MaterialAddOption());

            if (form.ShowDialog() == DialogResult.OK)
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
                GirdView_Materials.Rows.Add(BuildRowData(mat));
            }

            // Cleanup and reload.
            form.Dispose();

            FileIsEdited();
        }

        private DataGridViewRow BuildRowData(IMaterial mat)
        {
            DataGridViewRow row = new DataGridViewRow();
            row.Tag = mat;
            row.CreateCells(GirdView_Materials, new object[] { mat.MaterialName, mat.GetMaterialHash() });
            return row;
        }

        private void OnMaterialSelected(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.RowIndex > -1) && (e.ColumnIndex > -1))
            {
                MaterialGrid.SelectedObject = GirdView_Materials.Rows[e.RowIndex].Tag;
                IMaterial mat = (GirdView_Materials.Rows[e.RowIndex].Tag as IMaterial);
            }
        }

        private void MaterialGrid_OnPropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            MaterialGrid.Refresh();

            FileIsEdited();
        }

        private void Button_MergeMTL_Click(object sender, EventArgs e)
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

            FileIsEdited();
        }

        private void Button_Cancel_Click(object sender, EventArgs e)
        {
            if(MergePanel.Visible)
            {
                Panel_Main.Visible = true;
                MergePanel.Visible = false;
                OverwriteListBox.Items.Clear();
                NewMatListBox.Items.Clear();
            }
        }

        private void Button_Merge_Click(object sender, EventArgs e)
        {
            if(MergePanel.Visible)
            {
                Panel_Main.Visible = true;
                MergePanel.Visible = false;

                for(int i = 0; i < NewMatListBox.CheckedItems.Count; i++)
                {
                    var mat = (NewMatListBox.CheckedItems[i] as IMaterial);
                    if(mat.GetMTLVersion() != mtl.Version)
                    {
                        mat = MaterialFactory.ConvertMaterial(mtl.Version, mat);
                    }

                    mtl.Materials.Add(mat.GetMaterialHash(), mat);
                }

                for(int i = 0; i < OverwriteListBox.CheckedItems.Count; i++)
                {
                    var mat = (OverwriteListBox.CheckedItems[i] as IMaterial);
                    if (mat.GetMTLVersion() != mtl.Version)
                    {
                        mat = MaterialFactory.ConvertMaterial(mtl.Version, mat);
                    }

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

        private void Button_ExportedSelected_Click(object sender, EventArgs e)
        {
            MaterialLibrary library = new MaterialLibrary(mtl.Version);

            foreach(DataGridViewCell cell in GirdView_Materials.SelectedCells)
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

        private void MaterialEditor_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                Save();
            }
            else if (e.Control && e.KeyCode == Keys.R)
            {
                Reload();
            }
            else if (e.Control && e.KeyCode == Keys.D)
            {
                GirdView_Materials.ClearSelection();
                MaterialGrid.SelectedObject = null;
            }
            else if (e.Control && e.KeyCode == Keys.F)
            {
                MaterialSearch.Focus();
            }
        }

        private void Button_Exit_Click(object sender, EventArgs e)
        {
            if (bIsFileEdited)
            {
                System.Windows.MessageBoxResult SaveChanges = System.Windows.MessageBox.Show("Save before closing?", "", System.Windows.MessageBoxButton.YesNoCancel);

                if (SaveChanges == System.Windows.MessageBoxResult.Yes)
                {
                    Save();
                    MaterialData.Load();
                    Dispose();
                }
                else if (SaveChanges == System.Windows.MessageBoxResult.No)
                {
                    MaterialData.Load();
                    Dispose();
                }
            }
            else
            {
                MaterialData.Load();
                Dispose();
            }
        }

        private void Button_Save_Click(object sender, EventArgs e) => Save();
        private void Button_AddMaterial_Click(object sender, EventArgs e) => AddMaterial();
        private void Button_Delete_Click(object sender, EventArgs e) => Delete();
        private void Button_Reload_Click(object sender, EventArgs e) => Reload();

        private void MaterialEditor_Closing(object sender, FormClosingEventArgs e)
        {
            if (bIsFileEdited)
            {
                System.Windows.MessageBoxResult SaveChanges = System.Windows.MessageBox.Show(Language.GetString("$SAVE_PROMPT"), "Toolkit", System.Windows.MessageBoxButton.YesNoCancel);

                if (SaveChanges == System.Windows.MessageBoxResult.Yes)
                {
                    Save();
                }
                else if (SaveChanges == System.Windows.MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
