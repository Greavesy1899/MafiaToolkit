using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            mtl = new MaterialLibrary();
            mtl.ReadMatFile(file.FullName);
            FetchMaterials();
            ShowDialog();
            ToolkitSettings.UpdateRichPresence("Using the Material Library editor.");
        }

        public void FetchMaterials(bool searchMode = false, string text = null)
        {
            MaterialListBox.Items.Clear();
            foreach (KeyValuePair<ulong, Material> mat in mtl.Materials)
            {
                if (!string.IsNullOrEmpty(text) && searchMode)
                {
                    if (mat.Value.MaterialName.Contains(text) || mat.Value.MaterialHash.ToString().Contains(text))
                        MaterialListBox.Items.Add(mat);
                    else
                        continue;
                }
                else
                {
                    MaterialListBox.Items.Add(mat);
                }
            }
        }

        public void WriteMaterialsFile()
        {
            DialogResult result = MessageBox.Show("Do you want to save your changes?", "Save Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                //MaterialsManager.WriteMatFile(name, materials.ToArray());
                MessageBox.Show("Your saved file has been stored in the same folder as the executable.", "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void OnMaterialSelected(object sender, EventArgs e)
        {
            MaterialGrid.SelectedObject = MaterialListBox.SelectedItem;
        }

        private void OnKeyPressed(object sender, KeyPressEventArgs e)
        {
            ulong result;
            MaterialListBox.Items.Clear();
            foreach (KeyValuePair<ulong, Material> mat in mtl.Materials)
            {
                ulong.TryParse(MaterialSearch.Text, out result);

                if (mat.Value.MaterialName.Contains(MaterialSearch.Text))
                    MaterialListBox.Items.Add(mat);
                else if (mat.Value.MaterialHash == result)
                    MaterialListBox.Items.Add(mat);
            }
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            mtl.WriteMatFile(mtl.Name);
        }

        private void AddMaterial(object sender, EventArgs e)
        {
            Material mat = new Material();
            //erm, temporary lol.
            mtl.Materials.Add((ulong)Functions.RandomGenerator.Next(), mat);
            FetchMaterials();
        }

        private void MaterialSearch_TextChanged(object sender, EventArgs e)
        {
            FetchMaterials(true, MaterialSearch.Text);
        }
    }
}
