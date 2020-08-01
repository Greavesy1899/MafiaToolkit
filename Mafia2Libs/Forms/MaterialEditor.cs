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
                mtl = new MaterialLibrary();
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

            Material[] Filtered = mtl.SelectSearchTypeAndProceedSearch(text, currentSearchType);

            foreach (Material Mat in Filtered)
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
                return;

            //ask user for material name.
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

                //create material with new name.
                Material mat = new Material();
                mat.SetName(form.GetInputText());

                mtl.Materials.Add(mat.MaterialHash, mat);
                dataGridView1.Rows.Add(BuildRowData(mat));
            }

            //cleanup and reload.
            form.Dispose();
        }

        private void DeleteMaterial(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells[0] == null || !Panel_Main.Visible)
            {
                return;
            }

            int index = dataGridView1.SelectedCells[0].RowIndex;
            mtl.Materials.Remove((dataGridView1.Rows[index].Tag as Material).MaterialHash);
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
                Material mat = (dataGridView1.Rows[e.RowIndex].Tag as Material);
            }
        }

        private DataGridViewRow BuildRowData(Material mat)
        {
            DataGridViewRow row = new DataGridViewRow();
            row.Tag = mat;
            row.CreateCells(dataGridView1, new object[] { mat.MaterialName, mat.MaterialHash });
            return row;
        }

        private void MergeMTLButton_Click(object sender, EventArgs e)
        {
            if (!Panel_Main.Visible)
                return;

            MaterialLibrary matLib = new MaterialLibrary();
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
                if (mtl.Materials.ContainsKey(mat.MaterialHash))
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
                    var mat = (NewMatListBox.CheckedItems[i] as Material);
                    mtl.Materials.Add(mat.MaterialHash, mat);
                }

                for(int i = 0; i < OverwriteListBox.CheckedItems.Count; i++)
                {
                    var mat = (OverwriteListBox.CheckedItems[i] as Material);
                    mtl.Materials[mat.MaterialHash] = mat;
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
            MaterialLibrary library = new MaterialLibrary();

            foreach(DataGridViewCell cell in dataGridView1.SelectedCells)
            {
                if(cell.ColumnIndex == 0)
                {
                    var material = (cell.OwningRow.Tag as Material);

                    if (material != null)
                    {
                        library.Materials.Add(material.MaterialHash, material);
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
    }
}
