using System;
using System.Windows.Forms;
using Mafia2;

namespace Mafia2Tool {
    public partial class MaterialTool : Form {

        public MaterialTool() {
            InitializeComponent();
            FetchMaterials();
        }

        public void FetchMaterials() {
            foreach(Material mat in MaterialsParse.GetMaterials()) {
                MaterialListBox.Items.Add(mat);
            }
        }

        public void WriteMaterialsFile() {
        //    using (FileStream stream = new FileStream("default.mtl", FileMode.Truncate)) {
        //        stream.WriteString("MTLB");
        //        stream.WriteValueS32(57);
        //        stream.WriteValueS32(materials.Length);
        //        stream.WriteValueS32(0);

        //        for (int i = 0; i != materials.Length; i++) {
        //            materials[i].WriteToFile(stream);
        //        }
        //    }
        }

        private void OnMaterialSelected(object sender, EventArgs e) {
            MaterialGrid.SelectedObject = MaterialListBox.SelectedItem;

        }

        private void OnClose(object sender, FormClosingEventArgs e) {
        //    WriteMaterialsFile();
        }

        private void OnKeyPressed(object sender, KeyPressEventArgs e) {
            MaterialListBox.Items.Clear();
            foreach(Material mat in MaterialsParse.GetMaterials()) {
                if(mat.MaterialNumStr.Contains(MaterialSearch.Text)) {
                    MaterialListBox.Items.Add(mat);
                }
                else if(mat.MaterialName.Contains(MaterialSearch.Text)) {
                    MaterialListBox.Items.Add(mat);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e) {
        //    string materialID = "";
        //    string[] file = File.ReadAllLines("Lokace_ElGreco_translocator_00.hlavni(0).obj.mtl");
        //    int index = 0;
        //    foreach(string line in file) {
        //        if(line.Contains("newmtl ")) {
        //            materialID = line.Split(' ')[1];
        //            foreach(Material mat in MaterialListBox.Items) {
        //                if(mat.MaterialNumStr == materialID) {
        //                    file[index] = string.Format("{0} {1}", "newmtl", mat.MaterialName);
        //                    file[index + 6] = string.Format("{0} {1}", "map_kd", mat.SPS[0].File);
        //                }
        //            }
        //        }
        //        index++;
        //    }
        //    File.WriteAllLines("Lokace_ElGreco_translocator_00.hlavni(0).obj.mtl", file);

        //    file = File.ReadAllLines("Lokace_ElGreco_translocator_00.hlavni(0).obj");
        //    index = 0;
        //    foreach (string line in file) {
        //        if (line.Contains("usemtl ")) {
        //            materialID = line.Split(' ')[1];
        //            foreach (Material mat in MaterialListBox.Items) {
        //                if (mat.MaterialNumStr == materialID) {
        //                    file[index] = string.Format("{0} {1}", "usemtl", mat.MaterialName);
        //                }
        //            }
        //        }
        //        index++;
        //    }
        //    File.WriteAllLines("Lokace_ElGreco_translocator_00.hlavni(0).obj", file);
        }
    }
}
