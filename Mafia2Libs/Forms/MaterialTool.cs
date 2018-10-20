using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Mafia2;

namespace Mafia2Tool
{
    public partial class MaterialTool : Form
    {
        private MaterialLibrary mtl;

        public MaterialTool(FileInfo file)
        {
            InitializeComponent();
            Localise();
            mtl = new MaterialLibrary();
            mtl.ReadMatFile(file.FullName);
            FetchMaterials();
            ShowDialog();
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
            mtl.WriteMatFile(mtl.Name);
        }

        private void AddMaterial(object sender, EventArgs e)
        {
            //ask user for material name.
            NewObjectForm form = new NewObjectForm();
            form.SetLabel(Language.GetString("$QUESTION_NAME_OF_MAT"));
            form.ShowDialog();
            if (form.type == -1)
                return;

            //create material with new name.
            Material mat = new Material();
            mat.SetName(form.GetInputText());
            mtl.Materials.Add(mat.MaterialHash, mat);
            dataGridView1.Rows.Add(BuildRowData(mat));
            //cleanup and reload.
            form.Dispose();
        }

        private void DeleteMaterial(object sender, EventArgs e)
        {

            if (dataGridView1.SelectedCells[0] == null)
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
            FetchMaterials(false, null);
        }

        private void OnMaterialSelected(object sender, DataGridViewCellEventArgs e)
        {
            if((e.RowIndex > -1) && (e.ColumnIndex > -1))
                MaterialGrid.SelectedObject = dataGridView1.Rows[e.RowIndex].Tag;
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
    }
}
