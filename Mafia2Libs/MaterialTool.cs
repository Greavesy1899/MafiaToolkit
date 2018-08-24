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
        private List<Material> materials;
        private string name;

        public MaterialTool(FileInfo file)
        {
            InitializeComponent();
            materials = MaterialsManager.ReadMatFile(file.FullName).ToList();
            name = file.FullName;
            FetchMaterials();
            ShowDialog();
            ToolkitSettings.UpdateRichPresence("Using the Material Library editor.");
        }

        public void FetchMaterials()
        {
            MaterialListBox.Items.Clear();
            Dictionary<ulong, string> hashes = new Dictionary<ulong, string>();
            foreach (Material mat in materials)
            {
                hashes.Add(mat.MaterialHash, mat.MaterialName);
                MaterialListBox.Items.Add(mat);
            }
        }

        public void WriteMaterialsFile()
        {
            DialogResult result = MessageBox.Show("Do you want to save your changes?", "Save Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                MaterialsManager.WriteMatFile(name, materials.ToArray());
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
            foreach (Material mat in materials)
            {
                ulong.TryParse(MaterialSearch.Text, out result);

                if (mat.MaterialName.Contains(MaterialSearch.Text))
                    MaterialListBox.Items.Add(mat);
                else if (mat.MaterialHash == result)
                    MaterialListBox.Items.Add(mat);
            }
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            MaterialsManager.WriteMatFile(name, materials.ToArray());
            MessageBox.Show("Your saved file has been stored in the same folder as the executable.", "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void AddMaterial(object sender, EventArgs e)
        {
            Material mat = new Material();
            materials.Add(mat);
            FetchMaterials();
        }
    }
}
