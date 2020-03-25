using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Gibbed.Illusion.FileFormats.Hashing;
using Forms.EditorControls;
using ResourceTypes.Materials;
using Utils.Lang;
using Utils.Settings;
using System.Linq;

namespace Mafia2Tool
{
    public partial class MaterialTool : Form
    {
        private MaterialLibrary mtl;
        private List<string> debugShader = new List<string>();

        public MaterialTool(FileInfo file)
        {
            InitializeComponent();
            Localise();

            if (MaterialsManager.MaterialLibraries.ContainsKey(file.FullName))
            {
                mtl = MaterialsManager.MaterialLibraries[file.FullName];
            }
            else
            {
                mtl = new MaterialLibrary();
                mtl.ReadMatFile(file.FullName);
            }

            FetchMaterials();
            Show();
            MainPanel.Visible = true;
            MergePanel.Visible = false;
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
        }

        public void FetchMaterials(bool searchMode = false, string text = null)
        {
            dataGridView1.Rows.Clear();

            foreach (KeyValuePair<ulong, Material> mat in mtl.Materials)
            {
                if (!string.IsNullOrEmpty(text) && searchMode)
                {
                    if (mat.Value.MaterialName.Contains(text) || mat.Value.MaterialHash.ToString().Contains(text))
                        dataGridView1.Rows.Add(BuildRowData(mat));
                    else
                        continue;
                }
                else
                {
                    dataGridView1.Rows.Add(BuildRowData(mat));
                }
            }
        }

        public void WriteMaterialsFile()
        {
            DialogResult result = MessageBox.Show("Do you want to save your changes?", "Save Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
                SaveButton_Click(null, null);
        }

        private void OnKeyPressed(object sender, KeyPressEventArgs e)
        {
            FetchMaterials(true, MaterialSearch.Text);
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            MaterialData.Load();
            Dispose();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (!MainPanel.Visible)
            {
                MessageBox.Show("Complete the merge to save!");
                return;
            }

            mtl.WriteMatFile(mtl.Name);
        }

        private void AddMaterial(object sender, EventArgs e)
        {
            if (!MainPanel.Visible)
                return;

            //ask user for material name.
            NewObjectForm form = new NewObjectForm(true);
            form.SetLabel(Language.GetString("$QUESTION_NAME_OF_MAT"));
            form.LoadOption(new MaterialAddOption());
            
            if(form.ShowDialog() == DialogResult.OK)
            {
                if (mtl.Materials.ContainsKey(FNV64.Hash(form.GetInputText())))
                {
                    MessageBox.Show("Found duplicate material. Will not be adding new material!");
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
            if (dataGridView1.SelectedCells[0] == null || !MainPanel.Visible)
                return;

            int index = dataGridView1.SelectedCells[0].RowIndex;
            mtl.Materials.Remove((dataGridView1.Rows[index].Tag as Material).MaterialHash);
            dataGridView1.Rows.RemoveAt(index);
        }

        private void MaterialSearch_TextChanged(object sender, EventArgs e)
        {
            FetchMaterials(true, MaterialSearch.Text);
        }

        private void UpdateList(object sender, EventArgs e)
        {
            if (!MainPanel.Visible)
                return;

            FetchMaterials(false, null);
        }

        private void OnMaterialSelected(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.RowIndex > -1) && (e.ColumnIndex > -1))
            {
                MaterialGrid.SelectedObject = dataGridView1.Rows[e.RowIndex].Tag;
                Material mat = (dataGridView1.Rows[e.RowIndex].Tag as Material);
                Console.WriteLine(string.Format("{0} {1}", mat.MaterialName, (int)mat.Flags));
            }
        }

        private DataGridViewRow BuildRowData(KeyValuePair<ulong, Material> mat)
        {
            DataGridViewRow row = new DataGridViewRow();
            row.Tag = mat.Value;
            row.CreateCells(dataGridView1, new object[] { mat.Value.MaterialName, mat.Key });
            return row;
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
            if (!MainPanel.Visible)
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
            MainPanel.Visible = false;
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
                MainPanel.Visible = true;
                MergePanel.Visible = false;
                OverwriteListBox.Items.Clear();
                NewMatListBox.Items.Clear();
            }
        }

        private void MergeButton_Click(object sender, EventArgs e)
        {
            if(MergePanel.Visible)
            {
                MainPanel.Visible = true;
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
                    OverwriteListBox.SetItemChecked(i, true);
            }
        }

        private void SelectAllNewButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < NewMatListBox.Items.Count; i++)
                NewMatListBox.SetItemChecked(i, true);
        }
    }
}
