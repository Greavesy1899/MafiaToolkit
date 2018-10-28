using System.IO;
using System.Windows.Forms;
using Mafia2;

namespace Mafia2Tool
{
    public partial class ActorEditor : Form
    {
        private FileInfo actorFile;
        private Actor actors;

        public ActorEditor(FileInfo file)
        {
            InitializeComponent();
            Localise();
            actorFile = file;
            BuildData();
            ShowDialog();
            ToolkitSettings.UpdateRichPresence("Using the Actor editor.");
        }

        private void Localise()
        {
            Text = Language.GetString("$ACTOR_EDITOR_TITLE");
            fileToolButton.Text = Language.GetString("$FILE");
            saveToolStripMenuItem.Text = Language.GetString("$SAVE");
            reloadToolStripMenuItem.Text = Language.GetString("$RELOAD");
            exitToolStripMenuItem.Text = Language.GetString("$EXIT");
        }

        private void BuildData()
        {
            actors = new Actor(actorFile.FullName);

            for(int i = 0; i != actors.Definitions.Length; i++)
            {
                TreeNode node = new TreeNode(actors.Definitions[i].name);
                node.Name = actors.Definitions[i].Hash.ToString();
                node.Tag = actors.Definitions[i];
                treeView1.Nodes.Add(node);
            }

            for (int i = 0; i != actors.Items.Length; i++)
            {
                TreeNode node = new TreeNode(actors.Items[i].EntityType);
                node.Tag = actors.Items[i];

                TreeNode child = new TreeNode("Extra Data");
                child.Tag = actors.UnkSector.TempUnks[actors.Items[i].PropID];
                node.Nodes.Add(child);

                TreeNode[] nodes = treeView1.Nodes.Find(actors.Items[i].Hash2.ToString(), true);

                if (nodes.Length == 0)
                    treeView1.Nodes.Add(node);
                else
                    nodes[0].Nodes.Add(node);

            }
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

        }
    }
}