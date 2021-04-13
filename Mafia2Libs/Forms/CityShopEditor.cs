using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using ResourceTypes.City;
using Utils.Helpers;
using Utils.Language;
using Utils.Settings;

namespace Mafia2Tool
{
    public partial class CityShopEditor : Form
    {
        private FileInfo cityShopsFile;
        private CityShops shopsData;
        private CityShops.AreaData currentData;

        public CityShopEditor(FileInfo file)
        {
            InitializeComponent();
            TreeView_CityShop.SetDoubleBuffered();
            Localise();
            cityShopsFile = file;
            BuildData();
            Show();
            ToolkitSettings.UpdateRichPresence("Editing City Shops.");
        }

        private void Localise()
        {
            Text = Language.GetString("$CITY_SHOP_EDITOR_TITLE");
            fileToolButton.Text = Language.GetString("$FILE");
            Button_SaveNonDLC.Text = Language.GetString("$SAVE");
            ReloadButton.Text = Language.GetString("$RELOAD");
            ExitButton.Text = Language.GetString("$EXIT");
            toolButton.Text = Language.GetString("$TOOLS");
            AddAreaButton.Text = Language.GetString("$ADD_AREA");
            AddDataButton.Text = Language.GetString("$ADD_DATA");
            Button_SaveNonDLC.Text = Language.GetString("$SAVE") + " (Non DLC)";
            Button_SaveDLC.Text = Language.GetString("$SAVE") + " (DLC)";
            ReloadButton.Text = Language.GetString("$RELOAD");
            PopulateTranslokatorButton.Text = Language.GetString("$POPULATE_TRANSLOKATORS");
            DuplicateDataButton.Text = Language.GetString("$DUPLICATE_DATA");
            DeleteDataButton.Text = Language.GetString("$DELETE_DATA");
            DeleteAreaButton.Text = Language.GetString("$DELETE_AREA");
        }

        private void BuildData()
        {
            TreeView_CityShop.Nodes.Clear();
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

            TreeView_CityShop.Nodes.Add(areaNode);
            TreeView_CityShop.Nodes.Add(dataNode);
        }

        private void Save()
        {
            shopsData.Areas.Clear();
            shopsData.AreaDatas.Clear();

            foreach (TreeNode node in TreeView_CityShop.Nodes[0].Nodes)
            {
                shopsData.Areas.Add((CityShops.Area)node.Tag);
            }

            foreach (TreeNode node in TreeView_CityShop.Nodes[1].Nodes)
            {
                shopsData.AreaDatas.Add((CityShops.AreaData)node.Tag);
            }

            using (BinaryWriter writer = new BinaryWriter(File.Open(cityShopsFile.FullName, FileMode.Create)))
            {
                shopsData.WriteToFile(writer);
            }
        }

        private void AddAreaButton_Click(object sender, EventArgs e)
        {
            CityShops.Area area = new CityShops.Area();
            shopsData.Areas.Add(area);
            TreeNode node = new TreeNode("New Area");
            node.Tag = area;
            TreeView_CityShop.Nodes[0].Nodes.Add(node);
            shopsData.PopulateTranslokatorEntities();
            TreeView_CityShop.SelectedNode = node;
        }

        private void SaveButtonDLC_Click(object sender, EventArgs e)
        {
            shopsData.FileVersion = 9;
            Save();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            shopsData.FileVersion = 8;
            Save();
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

        private void OnAfterSelect(object sender, TreeViewEventArgs e)
        {
            propertyGrid1.SelectedObject = e.Node.Tag;

            if (e.Node.Tag != null)
            {
                if (e.Node.Tag.GetType() == typeof(CityShops.AreaData))
                    UpdateDataGrid((CityShops.AreaData)e.Node.Tag);
            }
        }

        private void SaveFromDataGrid()
        {
            List<string> entities = new List<string>();
            List<List<short>> translocators = new List<List<short>>();
            translocators = new List<List<short>>();

            for (int i = 1; i != dataGridView1.Rows[0].Cells.Count; i++)
                translocators.Add(new List<short>());

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].GetType() == typeof(DataGridViewTextBoxCell))
                {
                    if(row.Cells[0].Value != null)
                        entities.Add((row.Cells[0] as DataGridViewTextBoxCell).Value.ToString());
                }

                for (int i = 1; i != dataGridView1.Rows[0].Cells.Count; i++)
                {
                    if (row.Cells[0].Value != null)
                        translocators[i - 1].Add(short.Parse(row.Cells[i].Value.ToString()));
                }
            }
            currentData.Entries = entities.ToArray();

            for(int i = 0; i != currentData.Translokators.Count; i++)
            {
                CityShops.AreaData.TranslokatorData data = currentData.Translokators[i];
                data.EntityProperties = translocators[i];
            }
        }

        private void UpdateDataGrid(CityShops.AreaData areaData)
        {
            if(currentData == null)
            {
                currentData = areaData;
            }
            else
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();
                currentData = areaData;
            }
            List<List<object>> rows = new List<List<object>>();
            dataGridView1.Columns.Add("Entities", "Entities");
            foreach (var trans in currentData.Translokators)
                dataGridView1.Columns.Add(trans.Name, trans.Name);

            foreach (var entity in currentData.Entries)
            {
                List<object> row = new List<object>();
                row.Add(entity);
                rows.Add(row);
            }

            foreach (var trans in currentData.Translokators)
            {
                if (trans.EntityProperties != null)
                {
                    for (int i = 0; i != trans.EntityProperties.Count; i++)
                        rows[i].Add(trans.EntityProperties[i]);
                }
                else
                {
                    for (int i = 0; i != currentData.Entries.Length; i++)
                        rows[i].Add(1023);                      
                }
            }

            foreach(var row in rows)
                dataGridView1.Rows.Add(row.ToArray());

            dataGridView1.AutoResizeColumns();
        }

        private void PopulateTranslokatorButton_Click(object sender, EventArgs e)
        {
            shopsData.PopulateTranslokatorEntities();
            MessageBox.Show("All translokators were checked for errors.", "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void AddDataButton_Click(object sender, EventArgs e)
        {
            CityShops.AreaData areaData = new CityShops.AreaData();
            shopsData.AreaDatas.Add(areaData);
            TreeNode node = new TreeNode("New Area Data");
            node.Tag = areaData;
            TreeView_CityShop.Nodes[1].Nodes.Add(node);
            shopsData.PopulateTranslokatorEntities();
            TreeView_CityShop.SelectedNode = node;
        }

        private void OnPropertyChanged(object s, PropertyValueChangedEventArgs e)
        {
            if(e.ChangedItem.Label == "Name")
                TreeView_CityShop.SelectedNode.Text = e.ChangedItem.Value.ToString();
        }

        private void OnTabSelected(object sender, TabControlEventArgs e)
        {
            if(e.TabPage == DataGridTab)
            {
                if (TreeView_CityShop.SelectedNode.Tag != null)
                {
                    if (TreeView_CityShop.SelectedNode.Tag.GetType() == typeof(CityShops.AreaData))
                        UpdateDataGrid((CityShops.AreaData)TreeView_CityShop.SelectedNode.Tag);
                }
            }
            else if(e.TabPage == PropertyGridTab)
            {
                if(currentData != null)
                    SaveFromDataGrid();
            }
        }

        private void DuplicateData_OnClick(object sender, EventArgs e)
        {
            if (TreeView_CityShop.SelectedNode.Tag != null)
            {
                if (TreeView_CityShop.SelectedNode.Tag.GetType() == typeof(CityShops.AreaData))
                {
                    CityShops.AreaData data = new CityShops.AreaData((CityShops.AreaData)TreeView_CityShop.SelectedNode.Tag);
                    shopsData.AreaDatas.Add(data);
                    data.Name += "_dupe";
                    TreeNode node = new TreeNode(data.Name);
                    node.Tag = data;
                    TreeView_CityShop.Nodes[1].Nodes.Add(node);
                    TreeView_CityShop.SelectedNode = node;
                }
            }
        }

        private void DeleteArea_Click(object sender, EventArgs e)
        {
            if (TreeView_CityShop.SelectedNode.Tag != null)
            {
                if (TreeView_CityShop.SelectedNode.Tag.GetType() == typeof(CityShops.Area))
                {
                    TreeView_CityShop.Nodes.Remove(TreeView_CityShop.SelectedNode);
                    shopsData.Areas.Remove((CityShops.Area)TreeView_CityShop.SelectedNode.Tag);
                }
            }
        }

        private void DeleteData_Click(object sender, EventArgs e)
        {
            if (TreeView_CityShop.SelectedNode.Tag != null)
            {
                if (TreeView_CityShop.SelectedNode.Tag.GetType() == typeof(CityShops.AreaData))
                {
                    TreeView_CityShop.Nodes.Remove(TreeView_CityShop.SelectedNode);
                    shopsData.AreaDatas.Remove((CityShops.AreaData)TreeView_CityShop.SelectedNode.Tag);
                }
            }
        }
    }
}