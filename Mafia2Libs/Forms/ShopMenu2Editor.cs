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

        private void OnNodeSelectSelect(object sender, TreeViewEventArgs e)
        {
            PropertyGrid_ShopMenu2.SelectedObject = e.Node.Tag;
        }

        private void Button_Save_OnClick(object sender, System.EventArgs e)
        {
            File.Copy(menuFile.FullName, menuFile.FullName + "_old", true);
            menuData.WriteToFile(menuFile.FullName);
        }

        private void Button_Reload_OnClick(object sender, System.EventArgs e)
        {
            PropertyGrid_ShopMenu2.SelectedObject = null;
            BuildData();
        }

        private void Button_Exit_OnClick(object sender, System.EventArgs e)
        {
            Close();
        }
    }
}