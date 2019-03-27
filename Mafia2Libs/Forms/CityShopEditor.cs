using System;
using System.IO;
using System.Windows.Forms;
using Mafia2;
using ResourceTypes.City;
using Utils.Lang;
using Utils.Settings;

namespace Mafia2Tool
{
    public partial class CityShopEditor : Form
    {
        private FileInfo cityShopsFile;
        private CityShops shopsData;

        public CityShopEditor(FileInfo file)
        {
            InitializeComponent();
            Localise();
            cityShopsFile = file;
            BuildData();
            ShowDialog();
            ToolkitSettings.UpdateRichPresence("Editing City Shops.");
        }

        private void Localise()
        {
            Text = Language.GetString("$CITY_SHOP_EDITOR_TITLE");
            fileToolButton.Text = Language.GetString("$FILE");
            SaveButton.Text = Language.GetString("$SAVE");
            ReloadButton.Text = Language.GetString("$RELOAD");
            ExitButton.Text = Language.GetString("$EXIT");
            toolButton.Text = Language.GetString("$TOOLS");
            AddAreaButton.Text = Language.GetString("$ADD_AREA");
            SaveButton.Text = Language.GetString("$SAVE");
            ReloadButton.Text = Language.GetString("$RELOAD");
            PopulateTranslokatorButton.Text = Language.GetString("$POPULATE_TRANSLOKATORS");

        }

        private void BuildData()
        {
            treeView1.Nodes.Clear();
            propertyGrid1.SelectedObject = null;
            shopsData = new CityShops(cityShopsFile.FullName);
            TreeNode areaNode = new TreeNode("Areas");
            TreeNode dataNode = new TreeNode("Data");

            for (int i = 0; i != shopsData.AreaDatas.Count; i++)
            {
                TreeNode node = new TreeNode(shopsData.AreaDatas[i].Name);
                node.Tag = shopsData.AreaDatas[i];
                dataNode.Nodes.Add(node);
            }

            for (int i = 0; i != shopsData.Areas.Count; i++)
            {
                TreeNode node = new TreeNode(shopsData.Areas[i].Name);
                node.Tag = shopsData.Areas[i];
                areaNode.Nodes.Add(node);
            }

            treeView1.Nodes.Add(areaNode);
            treeView1.Nodes.Add(dataNode);
        }

        private void AddAreaButton_Click(object sender, EventArgs e)
        {
            CityShops.Area area = new CityShops.Area();
            shopsData.Areas.Add(area);
            TreeNode node = new TreeNode("New Area");
            node.Tag = area;
            treeView1.Nodes[0].Nodes.Add(node);
            shopsData.PopulateTranslokatorEntities();
            MessageBox.Show("A new area was added succesfully! Check under the 'Areas' node.", "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(cityShopsFile.FullName, FileMode.Create)))
                shopsData.WriteToFile(writer);
        }

        private void ReloadButton_Click(object sender, EventArgs e)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(cityShopsFile.FullName, FileMode.Open)))
                shopsData.ReadFromFile(reader);

            BuildData();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void OnSelect(object sender, TreeViewEventArgs e)
        {
            propertyGrid1.SelectedObject = e.Node.Tag;
        }

        private void PopulateTranslokatorButton_Click(object sender, EventArgs e)
        {
            shopsData.PopulateTranslokatorEntities();
            MessageBox.Show("All translokators were checked for errors.", "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}