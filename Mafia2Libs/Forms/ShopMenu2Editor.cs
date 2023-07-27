using System.IO;
using System.Windows.Forms;
using Utils.Helpers;
using Utils.Language;
using ResourceTypes.City;

namespace Mafia2Tool
{
    public partial class ShopMenu2Editor : Form
    {
        private FileInfo menuFile;
        private ShopMenu2 menuData;

        private TreeNode ShopFolder;
        private TreeNode ShopMetaInfoFolder;

        private bool bIsFileEdited;

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

            ShopFolder = new TreeNode("Shop Types");
            foreach(ShopMenu2.Shop Shop in menuData.Shops)
            {
                var child = new TreeNode("Shop" + Shop.ID.ToString());
                child.Text = Shop.Name;
                child.Tag = Shop;
                ShopFolder.Nodes.Add(child);
            }

            ShopMetaInfoFolder = new TreeNode("Shop MetaInfo");
            foreach (ShopMenu2.ShopMenu MetaInfo in menuData.ShopItems)
            {
                var meta = new TreeNode("Meta" + MetaInfo.ID.ToString());
                meta.Text = MetaInfo.Path;
                meta.Tag = MetaInfo;
                ShopMetaInfoFolder.Nodes.Add(meta);

                // Add items as sub-nodes
                foreach(ShopMenu2.ItemConfig Item in MetaInfo.Items)
                {
                    TreeNode ItemConfigNode = new TreeNode(Item.Name.ToString());
                    ItemConfigNode.Tag = Item;
                    meta.Nodes.Add(ItemConfigNode);
                }
            }

            // Add all nodes
            TreeView_ShopMenu2.Nodes.Add(ShopFolder);
            TreeView_ShopMenu2.Nodes.Add(ShopMetaInfoFolder);
        }

        private void Save()
        {
            PreSave();

            File.Copy(menuFile.FullName, menuFile.FullName + "_old", true);
            menuData.WriteToFile(menuFile.FullName);

            Text = Language.GetString("$SHOPMENU2_EDITOR_TITLE");
            bIsFileEdited = false;
        }

        private void PreSave()
        {
            // Clear existing lists
            menuData.Shops.Clear();
            menuData.ShopItems.Clear();

            // Repopulate lists
            foreach(TreeNode Shop in ShopFolder.Nodes)
            {
                menuData.Shops.Add((ShopMenu2.Shop)Shop.Tag);
            }

            foreach (TreeNode MetaInfo in ShopMetaInfoFolder.Nodes)
            {
                ShopMenu2.ShopMenu CurrentMetaInfo = (ShopMenu2.ShopMenu)MetaInfo.Tag;
                menuData.ShopItems.Add(CurrentMetaInfo);

                // Clear for new entries
                CurrentMetaInfo.Items.Clear();
                foreach (TreeNode Item in MetaInfo.Nodes)
                {
                    CurrentMetaInfo.Items.Add((ShopMenu2.ItemConfig)Item.Tag);
                }
            }
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
                    foreach (var shop in menuData.Shops)
                    {
                        if (SelNode.Text == shop.Name)
                        {
                            SelShop = shop;
                            break;
                        }
                    }

                    if (menuData.Shops.Contains(SelShop))
                    {
                        menuData.Shops.Remove(SelShop);
                        TreeView_ShopMenu2.Nodes.Remove(SelNode);

                        Text = Language.GetString("$SHOPMENU2_EDITOR_TITLE") + "*";
                        bIsFileEdited = true;
                    }
                }
                else if (SelNode.Parent.Text == "Shop MetaInfo")
                {
                    var SelItem = new ShopMenu2.ShopMenu();
                    foreach (var item in menuData.ShopItems)
                    {
                        if (SelNode.Text == item.Path)
                        {
                            SelItem = item;
                            break;
                        }
                    }

                    if (menuData.ShopItems.Contains(SelItem))
                    {
                        menuData.ShopItems.Remove(SelItem);
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
            shop.Name = "New Shop " + menuData.Shops.Count.ToString();
            menuData.Shops.Add(shop);

            var NewShop = menuData.Shops[menuData.Shops.Count - 1];
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
            metaInfo.Path = "New MetaInfo " + menuData.ShopItems.Count.ToString();
            menuData.ShopItems.Add(metaInfo);

            var newInfo = menuData.ShopItems[menuData.ShopItems.Count - 1];
            meta.Text = newInfo.Path;
            meta.Tag = newInfo;
            TreeView_ShopMenu2.Nodes[1].Nodes.Add(meta);

            Text = Language.GetString("$SHOPMENU2_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

        private void DuplicateMetaInfo()
        {
            if (TreeView_ShopMenu2.SelectedNode == null || TreeView_ShopMenu2.SelectedNode.Tag == null)
            {
                // Skip, nothing selected
                return;
            }

            ShopMenu2.ShopMenu MetaInfo = (TreeView_ShopMenu2.SelectedNode.Tag as ShopMenu2.ShopMenu);
            if (MetaInfo == null)
            {
                // Skip, not a meta info
                return;
            }

            // Copy
            ShopMenu2.ShopMenu CopiedMetaInfo = new ShopMenu2.ShopMenu(MetaInfo);

            // Create a new TreeNode
            var NewMetaInfoNode = new TreeNode("Meta" + CopiedMetaInfo.ID.ToString());
            NewMetaInfoNode.Text = CopiedMetaInfo.Path;
            NewMetaInfoNode.Tag = CopiedMetaInfo;
            menuData.ShopItems.Add(CopiedMetaInfo);
            ShopMetaInfoFolder.Nodes.Add(NewMetaInfoNode);

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
            {
                TreeView_ShopMenu2.SelectedNode.Text = e.ChangedItem.Value.ToString();
            }

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

        private void Context_Menu_OnOpening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Context_DuplicateMetaInfo.Enabled = false;

            if (TreeView_ShopMenu2.SelectedNode == null || TreeView_ShopMenu2.SelectedNode.Tag == null)
            {
                // Skip, nothing selected
                return;
            }

            // If a MetaInfo, enable DuplicateMetaInfo button
            ShopMenu2.ShopMenu MetaInfo = (TreeView_ShopMenu2.SelectedNode.Tag as ShopMenu2.ShopMenu);
            if(MetaInfo != null)
            {
                Context_DuplicateMetaInfo.Enabled = true;
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
        private void Context_DupeMetaInfo_Clicked(object sender, System.EventArgs e) => DuplicateMetaInfo();
    }
}