using System.IO;
using System.Linq;
using System.Windows.Forms;
using ResourceTypes.Misc;
using ResourceTypes.Speech;
using Utils.Lang;
using Utils.Settings;

namespace Mafia2Tool
{
    public partial class StreamEditor : Form
    {
        private FileInfo file;
        private StreamMapLoader stream;

        public StreamEditor(FileInfo file)
        {
            InitializeComponent();
            Localise();
            this.file = file;
            BuildData();
            Show();
            ToolkitSettings.UpdateRichPresence("Using the Speech editor.");
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
            stream = new StreamMapLoader(file);

            for(int i = 0; i < stream.groupHeaders.Length; i++)
            {
                TreeNode node = new TreeNode("group" + i);
                node.Text = stream.groupHeaders[i];
                treeView1.Nodes.Add(node);
            }
            for (int i = 0; i != stream.lines.Length; i++)
            {
                var line = stream.lines[i];
                TreeNode node = new TreeNode();
                node.Name = line.Name;
                node.Text = line.Name;
                node.Tag = line;
                treeView1.Nodes[line.groupID].Nodes.Add(node);
            }
            for(int i = 0; i < stream.groups.Length; i++)
            {
                var line = stream.groups[i];
                TreeNode node = new TreeNode();
                node.Name = "GroupLoader" + i;
                node.Text = line.name;

                for(int x = line.startOffset; x < line.startOffset+line.endOffset; x++)
                {
                    var loader = stream.loaders[x];
                    TreeNode child = new TreeNode();
                    child.Name = "Loader" + x;
                    child.Text = loader.path;
                    node.Nodes.Add(child);
                }

                mTreeView1.Nodes.Add(node);
            }

        }

        public void UncheckAllNodes(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                node.Checked = false;
                CheckChildren(node, false);
            }
        }

        private void CheckChildren(TreeNode rootNode, bool isChecked)
        {
            foreach (TreeNode node in rootNode.Nodes)
            {
                CheckChildren(node, isChecked);
                node.Checked = isChecked;
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