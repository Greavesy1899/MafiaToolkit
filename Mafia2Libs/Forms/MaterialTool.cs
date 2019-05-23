using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Gibbed.Illusion.FileFormats.Hashing;
using Forms.EditorControls;
using ResourceTypes.Materials;
using Utils.Lang;
using Utils.Settings;

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

            if (MaterialsManager.MTLs.ContainsKey(file.FullName))
            {
                mtl = MaterialsManager.MTLs[file.FullName];
            }
            else
            {
                mtl = new MaterialLibrary();
                mtl.ReadMatFile(file.FullName);
            }

            FetchMaterials();
            Show();
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
            NewObjectForm form = new NewObjectForm(true);
            form.SetLabel(Language.GetString("$QUESTION_NAME_OF_MAT"));
            form.LoadOption(new MaterialAddOption());
            form.ShowDialog();
            if (form.type == -1)
                return;

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
            if ((e.RowIndex > -1) && (e.ColumnIndex > -1))
            {
                MaterialGrid.SelectedObject = dataGridView1.Rows[e.RowIndex].Tag;
                Material mat = (dataGridView1.Rows[e.RowIndex].Tag as Material);
                Console.WriteLine(string.Format("{0} {1}", mat.MaterialName, (int)mat.Flags));
            }
        }

        private void DumpSpecificMaterialNames()
        {
            int countForShaderID = 0;
            int countForShaderHash = 0;
            int countForBoth = 0;
            foreach (KeyValuePair<ulong, Material> mat in mtl.Materials)
            {
                if (mat.Value.ShaderID == 3854590933660942049)
                {
                    Console.WriteLine("Material {0} has ShaderID", mat.Value.MaterialName);
                    countForShaderID++;
                }

                if (mat.Value.ShaderHash == 601151254)
                {
                    Console.WriteLine("Material {0} has ShaderHash", mat.Value.MaterialName);
                    countForShaderHash++;
                }

                if (mat.Value.ShaderID == 3854590933660942049 && mat.Value.ShaderHash == 601151254)
                {
                    Console.WriteLine("Material {0} has both ShaderID and ShaderHash", mat.Value.MaterialName);
                    countForBoth++;
                }
            }
            Console.WriteLine(countForShaderID);
            Console.WriteLine(countForShaderHash);
            Console.WriteLine(countForBoth);
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

        private void DumpInfo_Clicked(object sender, EventArgs e)
        {
            DumpSpecificMaterialNames();
        }
    }
}
