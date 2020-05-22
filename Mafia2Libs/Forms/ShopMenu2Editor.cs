using System.IO;
using System.Windows.Forms;
using ResourceTypes.Speech;
using Utils.Language;
using Utils.Settings;
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
            Text = Language.GetString("$SPEECH_EDITOR_TITLE");
            fileToolButton.Text = Language.GetString("$FILE");
            saveToolStripMenuItem.Text = Language.GetString("$SAVE");
            reloadToolStripMenuItem.Text = Language.GetString("$RELOAD");
            exitToolStripMenuItem.Text = Language.GetString("$EXIT");
        }

        private void BuildData()
        {
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
            treeView1.Nodes.Add(node);
            treeView1.Nodes.Add(metaNode);
        }

        private void OnNodeSelectSelect(object sender, TreeViewEventArgs e)
        {
            FrameResourceGrid.SelectedObject = e.Node.Tag;
        }

        private void exitToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void reloadToolStripMenuItem_Click(object sender, System.EventArgs e)
        {

        }

        private void saveToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            menuData.WriteToFile(menuFile.FullName);
        }
    }
}