using System;
using System.IO;
using System.Windows.Forms;
using Mafia2;

namespace Mafia2Tool
{
    public partial class MaterialTool : Form
    {

        public MaterialTool()
        {
            InitializeComponent();
            FetchMaterials();
        }

        public void FetchMaterials()
        {
            foreach (Material mat in MaterialsParse.GetMaterials())
            {
                MaterialListBox.Items.Add(mat);
            }
        }

        public void WriteMaterialsFile()
        {
            DialogResult result = MessageBox.Show("Do you want to save your changes?", "Save Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                MaterialsParse.WriteMatFile("default.mtl");
                MessageBox.Show("Your saved file has been stored in the same folder as the executable.", "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void OnMaterialSelected(object sender, EventArgs e)
        {
            MaterialGrid.SelectedObject = MaterialListBox.SelectedItem;

        }

        private void OnClose(object sender, FormClosingEventArgs e)
        {
            WriteMaterialsFile();
        }

        private void OnKeyPressed(object sender, KeyPressEventArgs e)
        {
            MaterialListBox.Items.Clear();
            foreach (Material mat in MaterialsParse.GetMaterials())
            {
                if (mat.MaterialNumStr.Contains(MaterialSearch.Text))
                {
                    MaterialListBox.Items.Add(mat);
                }
                else if (mat.MaterialName.Contains(MaterialSearch.Text))
                {
                    MaterialListBox.Items.Add(mat);
                }
            }
        }
    }
}
