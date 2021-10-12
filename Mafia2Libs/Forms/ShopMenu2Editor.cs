using System.IO;
using System.Windows.Forms;
using Utils.Helpers;
using Utils.Language;
using ResourceTypes.City;

namespace MafiaToolkit
{
    public partial class ShopMenu2Editor : Form
    {
        private FileInfo menuFile;
        private ShopMenu2 menuData;

        private bool bIsFileEdited = false;

        public ShopMenu2Editor(FileInfo file)
        {
            InitializeComponent();
            Localise();
            menuFile = file;
            BuildData();
            Show();
        }

        private void Localise()
        {
            Text = Language.GetString("$SHOPMENU2_EDITOR_TITLE");
            Button_File.Text = Language.GetString("$FILE");
            Button_Save.Text = Language.GetString("$SAVE");
            Button_Reload.Text = Language.GetString("$RELOAD");
            Button_Exit.Text = Language.GetString("$EXIT");
            Button_Delete.Text = Language.GetString("$DELETE");
            Button_AddType.Text = Language.GetString("$ADD_SHOPTYPE");
            Button_AddMetaInfo.Text = Language.GetString("$ADD_METAINFO");
            Context_Delete.Text = Language.GetString("$DELETE");
            Context_AddType.Text = Language.GetString("$ADD_SHOPTYPE");
            Context_AddMetaInfo.Text = Language.GetString("$ADD_METAINFO");
        }

        private void BuildData()
        {
            TreeView_ShopMenu2.Nodes.Clear();

            menuData = new ShopMenu2();
            menuData.ReadFromFile(menuFile.FullName);

            var node = new TreeNode("Shop Types");
            for(int i = 0; i < menuData.shops.Count; i++)
            {
                var shop = menuData.shops[i];
                var child = new TreeNode("Shop"+shop.ID.ToString());
                child.Text = shop.Name;
                child.Tag = shop;
                node.Nodes.Add(child);
            }

            var metaNode = new TreeNode("Shop MetaInfo");
            for (int i = 0; i < menuData.shopItems.Count; i++)
            {
                var metaInfo = menuData.shopItems[i];
                var meta = new TreeNode("Meta" + metaInfo.ID.ToString());
                meta.Text = metaInfo.Path;
                meta.Tag = metaInfo;
                metaNode.Nodes.Add(meta);
            }
            TreeView_ShopMenu2.Nodes.Add(node);
            TreeView_ShopMenu2.Nodes.Add(metaNode);
        }

        private void Save()
        {
            File.Copy(menuFile.FullName, menuFile.FullName + "_old", true);
            menuData.WriteToFile(menuFile.FullName);

            Text = Language.GetString("$SHOPMENU2_EDITOR_TITLE");
            bIsFileEdited = false;
        }

        private void Reload()
        {
            PropertyGrid_ShopMenu2.SelectedObject = null;
            TreeView_ShopMenu2.SelectedNode = null;
            BuildData();

            Text = Language.GetString("$SHOPMENU2_EDITOR_TITLE");
            bIsFileEdited = false;
        }

        private void Delete()
        {
            TreeNode SelNode = TreeView_ShopMenu2.SelectedNode;

            if (SelNode.Parent != null)
            {
                if (SelNode.Parent.Text == "Shop Types")
                {
                    var SelShop = new ShopMenu2.Shop();
                    foreach (var shop in menuData.shops)
                    {
                        if (SelNode.Text == shop.Name)
                        {
                            SelShop = shop;
                            break;
                        }
                    }

                    if (menuData.shops.Contains(SelShop))
                    {
                        menuData.shops.Remove(SelShop);
                        TreeView_ShopMenu2.Nodes.Remove(SelNode);

                        Text = Language.GetString("$SHOPMENU2_EDITOR_TITLE") + "*";
                        bIsFileEdited = true;
                    }
                }
                else if (SelNode.Parent.Text == "Shop MetaInfo")
                {
                    var SelItem = new ShopMenu2.ShopMenu();
                    foreach (var item in menuData.shopItems)
                    {
                        if (SelNode.Text == item.Path)
                        {
                            SelItem = item;
                            break;
                        }
                    }

                    if (menuData.shopItems.Contains(SelItem))
                    {
                        menuData.shopItems.Remove(SelItem);
                        TreeView_ShopMenu2.Nodes.Remove(SelNode);

                        Text = Language.GetString("$SHOPMENU2_EDITOR_TITLE") + "*";
                        bIsFileEdited = true;
                    }
                }
            }
        }

        private void AddType()
        {
            var shop = new ShopMenu2.Shop();
            var child = new TreeNode("Shop" + shop.ID.ToString());
            shop.Name = "New Shop " + menuData.shops.Count.ToString();
            menuData.shops.Add(shop);

            var NewShop = menuData.shops[menuData.shops.Count - 1];
            child.Text = NewShop.Name;
            child.Tag = NewShop;
            TreeView_ShopMenu2.Nodes[0].Nodes.Add(child);

            Text = Language.GetString("$SHOPMENU2_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

        private void AddMetaInfo()
        {
            var metaInfo = new ShopMenu2.ShopMenu();
            var meta = new TreeNode("Meta" + metaInfo.ID.ToString());
            metaInfo.Path = "New MetaInfo " + menuData.shopItems.Count.ToString();
            menuData.shopItems.Add(metaInfo);

            var newInfo = menuData.shopItems[menuData.shopItems.Count - 1];
            meta.Text = newInfo.Path;
            meta.Tag = newInfo;
            TreeView_ShopMenu2.Nodes[1].Nodes.Add(meta);

            Text = Language.GetString("$SHOPMENU2_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

        private void OnNodeSelectSelect(object sender, TreeViewEventArgs e)
        {
            PropertyGrid_ShopMenu2.SelectedObject = e.Node.Tag;
        }

        private void Grid_ShopMenu2_PropertyChanged(object sender, PropertyValueChangedEventArgs e)
        {
            if (e.ChangedItem.Label == "Name" || e.ChangedItem.Label == "Path")
                TreeView_ShopMenu2.SelectedNode.Text = e.ChangedItem.Value.ToString();

            Text = Language.GetString("$SHOPMENU2_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

        private void ShopMenu2Editor_Closing(object sender, FormClosingEventArgs e)
        {
            if (bIsFileEdited)
            {
                System.Windows.MessageBoxResult SaveChanges = System.Windows.MessageBox.Show(Language.GetString("$SAVE_PROMPT"), "Toolkit", System.Windows.MessageBoxButton.YesNoCancel);

                if (SaveChanges == System.Windows.MessageBoxResult.Yes)
                {
                    Save();
                }
                else if (SaveChanges == System.Windows.MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void Button_Save_OnClick(object sender, System.EventArgs e) => Save();
        private void Button_Reload_OnClick(object sender, System.EventArgs e) => Reload();
        private void Button_Exit_OnClick(object sender, System.EventArgs e) => Close();
        private void Button_Delete_OnClick(object sender, System.EventArgs e) => Delete();
        private void Button_AddType_OnClick(object sender, System.EventArgs e) => AddType();
        private void Button_AddMetaInfo_OnClick(object sender, System.EventArgs e) => AddMetaInfo();
        private void Context_Delete_OnClick(object sender, System.EventArgs e) => Delete();
        private void Context_AddType_OnClick(object sender, System.EventArgs e) => AddType();
        private void Context_AddMetaInfo_OnClick(object sender, System.EventArgs e) => AddMetaInfo();
    }
}