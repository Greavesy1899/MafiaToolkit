using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using ResourceTypes.City;
using Utils.Language;
using MessageBox = System.Windows.MessageBox;

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
            BuildData(true);
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
            Context_DuplicateMetaInfoItem.Text = Language.GetString("$DUPLICATE_ITEM");
        }

        private void BuildData(bool fromFile)
        {
            TreeView_ShopMenu2.Nodes.Clear();

            if (fromFile)
            {
                menuData = new ShopMenu2();
                menuData.ReadFromFile(menuFile.FullName);
            }

            ShopFolder = new TreeNode("Shop Types");
            foreach (ShopMenu2.Shop Shop in menuData.Shops)
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
                foreach (ShopMenu2.ItemConfig Item in MetaInfo.Items)
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

            // Mark as not edited
            Text = Language.GetString("$SHOPMENU2_EDITOR_TITLE");
            bIsFileEdited = false;
        }

        private void PreSave()
        {
            // Clear existing lists
            menuData.Shops = new ShopMenu2.Shop[ShopFolder.Nodes.Count];
            menuData.ShopItems = new ShopMenu2.ShopMenu[ShopMetaInfoFolder.Nodes.Count];

            // Repopulate lists
            for (int i = 0; i < ShopFolder.Nodes.Count; i++)
            {
                menuData.Shops[i] = (ShopMenu2.Shop)ShopFolder.Nodes[i].Tag;
            }

            for (int i = 0; i < ShopMetaInfoFolder.Nodes.Count; i++)
            {
                var MetaInfo = ShopMetaInfoFolder.Nodes[i];
                ShopMenu2.ShopMenu CurrentMetaInfo = (ShopMenu2.ShopMenu)MetaInfo.Tag;
                menuData.ShopItems[i] = CurrentMetaInfo;

                // Clear for new entries
                CurrentMetaInfo.Items = new ShopMenu2.ItemConfig[MetaInfo.Nodes.Count];
                for (int j = 0; j < MetaInfo.Nodes.Count; j++)
                {
                    CurrentMetaInfo.Items[j] = (ShopMenu2.ItemConfig)MetaInfo.Nodes[j].Tag;
                }
            }
        }

        private void Reload()
        {
            PropertyGrid_ShopMenu2.SelectedObject = null;
            TreeView_ShopMenu2.SelectedNode = null;
            BuildData(true);

            Text = Language.GetString("$SHOPMENU2_EDITOR_TITLE");
            bIsFileEdited = false;
        }

        private void Delete()
        {
            TreeNode SelNode = TreeView_ShopMenu2.SelectedNode;
            if (SelNode != null)
            {
                SelNode.Remove();

                Text = Language.GetString("$SHOPMENU2_EDITOR_TITLE") + "*";
                bIsFileEdited = true;
            }
        }

        private void AddType()
        {
            // Create the new shop
            ShopMenu2.Shop NewShop = new ShopMenu2.Shop();
            NewShop.Name = "New Shop";

            // Create a new node
            TreeNode NewNode = new TreeNode("Shop" + (ShopFolder.Nodes.Count + 1));
            NewNode.Tag = NewShop;
            ShopFolder.Nodes.Add(NewNode);

            // Mark as edited
            MarkAsEdited();
        }

        private void AddMetaInfo()
        {
            // Create the new MetaInfo
            ShopMenu2.ShopMenu NewMenu = new ShopMenu2.ShopMenu();
            NewMenu.Path = "New Shop Menu";

            // Create a new node
            TreeNode NewNode = new TreeNode("ShopMenu" + (ShopMetaInfoFolder.Nodes.Count + 1));
            NewNode.Tag = NewMenu;
            ShopMetaInfoFolder.Nodes.Add(NewNode);

            // Mark as edited
            MarkAsEdited();
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

            MarkAsEdited();
        }

        private void DuplicateMetaInfoItem()
        {
            if (TreeView_ShopMenu2.SelectedNode == null || TreeView_ShopMenu2.SelectedNode.Tag == null)
            {
                // Skip, nothing selected
                return;
            }

            ShopMenu2.ItemConfig SelectedConfig = (TreeView_ShopMenu2.SelectedNode.Tag as ShopMenu2.ItemConfig);
            if (SelectedConfig == null)
            {
                // Skip, not correct type
                return;
            }

            // Copy
            ShopMenu2.ItemConfig CopiedConfig = new ShopMenu2.ItemConfig(SelectedConfig);

            // Create a new TreeNode
            TreeNode NewMetaInfoNode = new TreeNode(CopiedConfig.Name.ToString());
            NewMetaInfoNode.Tag = CopiedConfig;

            // Add to same parent
            TreeNode ParentNode = TreeView_ShopMenu2.SelectedNode.Parent;
            ParentNode.Nodes.Add(NewMetaInfoNode);

            // Mark as modified
            MarkAsEdited();
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

            MarkAsEdited();
        }

        private void ShopMenu2Editor_Closing(object sender, FormClosingEventArgs e)
        {
            if (bIsFileEdited)
            {
                MessageBoxResult SaveChanges = MessageBox.Show(Language.GetString("$SAVE_PROMPT"), "Toolkit", MessageBoxButton.YesNoCancel);

                if (SaveChanges == MessageBoxResult.Yes)
                {
                    Save();
                }
                else if (SaveChanges == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void Context_Menu_OnOpening(object sender, CancelEventArgs e)
        {
            Context_DuplicateMetaInfo.Enabled = false;
            Context_DuplicateMetaInfoItem.Enabled = false;

            if (TreeView_ShopMenu2.SelectedNode == null || TreeView_ShopMenu2.SelectedNode.Tag == null)
            {
                // Skip, nothing selected
                return;
            }

            // If a MetaInfo, enable DuplicateMetaInfo button
            ShopMenu2.ShopMenu MetaInfo = (TreeView_ShopMenu2.SelectedNode.Tag as ShopMenu2.ShopMenu);
            if (MetaInfo != null)
            {
                Context_DuplicateMetaInfo.Enabled = true;
            }

            // If a ItemConfig, enable DuplicateMetaInfoItem button
            ShopMenu2.ItemConfig ItemConfig = (TreeView_ShopMenu2.SelectedNode.Tag as ShopMenu2.ItemConfig);
            if (ItemConfig != null)
            {
                Context_DuplicateMetaInfoItem.Enabled = true;
            }
        }

        private void MarkAsEdited()
        {
            Text = Language.GetString("$SHOPMENU2_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

        private void Button_Save_OnClick(object sender, EventArgs e) => Save();
        private void Button_Reload_OnClick(object sender, EventArgs e) => Reload();
        private void Button_Exit_OnClick(object sender, EventArgs e) => Close();
        private void Button_Delete_OnClick(object sender, EventArgs e) => Delete();
        private void Button_AddType_OnClick(object sender, EventArgs e) => AddType();
        private void Button_AddMetaInfo_OnClick(object sender, EventArgs e) => AddMetaInfo();
        private void Button_ExportXml_OnClick(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "XML|*.XML";

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                menuData.ConvertToXML(saveFile.FileName);
            }
        }

        private void Button_ImportXml_OnClick(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML|*.XML";
            openFileDialog.Multiselect = false;
            openFileDialog.CheckFileExists = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string FileToOpen = openFileDialog.FileName;
                if (File.Exists(FileToOpen))
                {
                    menuData.ConvertFromXML(FileToOpen);

                    BuildData(false);
                }
            }
        }

        private void Context_Delete_OnClick(object sender, EventArgs e) => Delete();
        private void Context_AddType_OnClick(object sender, EventArgs e) => AddType();
        private void Context_AddMetaInfo_OnClick(object sender, EventArgs e) => AddMetaInfo();
        private void Context_DupeMetaInfo_Clicked(object sender, EventArgs e) => DuplicateMetaInfo();
        private void Context_DupeMetaInfoItem_Clicked(object sender, EventArgs e) => DuplicateMetaInfoItem();
    }
}