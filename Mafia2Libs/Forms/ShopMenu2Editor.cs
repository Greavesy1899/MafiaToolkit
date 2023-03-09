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

        private TreeNode ShopFolder = null;
        private TreeNode ShopMetaInfoFolder = null;

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

            ShopFolder = new TreeNode("Shop Types");
            for (int i = 0; i < menuData.shops.Count; i++)
            {
                var shop = menuData.shops[i];
                var child = new TreeNode("Shop" + shop.ID.ToString());
                child.Text = shop.Name;
                child.Tag = shop;
                ShopFolder.Nodes.Add(child);
            }

            ShopMetaInfoFolder = new TreeNode("Shop MetaInfo");
            for (int i = 0; i < menuData.shopItems.Count; i++)
            {
                var metaInfo = menuData.shopItems[i];
                var meta = new TreeNode("Meta" + metaInfo.ID.ToString());
                meta.Text = metaInfo.Path;
                meta.Tag = metaInfo;
                ShopMetaInfoFolder.Nodes.Add(meta);
            }

            // Add all nodes
            TreeView_ShopMenu2.Nodes.Add(ShopFolder);
            TreeView_ShopMenu2.Nodes.Add(ShopMetaInfoFolder);
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
            menuData.shopItems.Add(CopiedMetaInfo);
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