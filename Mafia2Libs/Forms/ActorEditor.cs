using System.IO;
using System.Windows.Forms;
using ResourceTypes.Actors;
using Utils.Lang;
using Utils.Settings;

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

            for (int i = 0; i != actors.Definitions.Length; i++)
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
                child.Tag = actors.TempUnks[actors.Items[i].DataID];
                node.Nodes.Add(child);

                TreeNode[] nodes = treeView1.Nodes.Find(actors.Items[i].ItemType.ToString(), true);

                if (nodes.Length == 0)
                    treeView1.Nodes.Add(node);
                else
                    nodes[0].Nodes.Add(node);


                string folder = "actors_unks/" + (ActorTypes)actors.Items[i].ActorType + "/";
                string filename = actors.Items[i].FrameUnk + ".dat";

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                using (BinaryWriter writer = new BinaryWriter(File.Open(Path.Combine(folder, filename), FileMode.Create)))
                {
                    if (actors.TempUnks[actors.Items[i].DataID].Data == null)
                        writer.Write(actors.TempUnks[actors.Items[i].DataID].Buffer);
                    else
                        actors.TempUnks[actors.Items[i].DataID].Data.WriteToFile(writer);
                }
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
            using (BinaryWriter writer = new BinaryWriter(File.Open(actorFile.FullName + "EDIT", FileMode.Create)))
            {
                actors.WriteToFile(writer);
            }
        }
    }
}