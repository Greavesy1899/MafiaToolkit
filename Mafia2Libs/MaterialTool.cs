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
            MaterialsParse.ReadMatFile("default.mtl");
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
            MaterialsParse.WriteMatFile("default.mtl");
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
