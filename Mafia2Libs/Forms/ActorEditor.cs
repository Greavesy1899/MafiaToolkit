using System.Diagnostics;
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

        private TreeNode definitions;
        private TreeNode items;


        public ActorEditor(FileInfo file)
        {
            InitializeComponent();
            Localise();
            actorFile = file;
            BuildData();
            Show();
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
            definitions = new TreeNode("Definitions");
            items = new TreeNode("Entities");

            for (int i = 0; i != actors.Definitions.Length; i++)
            {
                TreeNode node = new TreeNode(actors.Definitions[i].name);
                node.Name = actors.Definitions[i].Hash.ToString();
                node.Tag = actors.Definitions[i];
                definitions.Nodes.Add(node);
            }

            for (int i = 0; i != actors.Items.Length; i++)
            {
                TreeNode node = new TreeNode(actors.Items[i].EntityType);
                node.Tag = actors.Items[i];

                TreeNode child = new TreeNode("Extra Data");
                child.Tag = actors.ExtraData[actors.Items[i].DataID];
                node.Nodes.Add(child);
                items.Nodes.Add(node);
                //TreeNode[] nodes = treeView1.Nodes.Find(actors.Items[i].Hash2.ToString(), true);

                //if (nodes.Length == 0)
                //    items.Nodes.Add(node);
                //else
                //    nodes[0].Nodes.Add(node);

                if (Debugger.IsAttached)
                {
                    string folder = "actors_unks/" + (ActorTypes)actors.Items[i].ActorTypeID + "1" + "/";
                    string filename = actors.Items[i].EntityType + ".dat";

                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    using (BinaryWriter writer = new BinaryWriter(File.Open(Path.Combine(folder, filename), FileMode.Create)))
                    {
                        if (actors.ExtraData[actors.Items[i].DataID].Data == null)
                        {
                            writer.Write(actors.ExtraData[actors.Items[i].DataID].Buffer);
                        }
                        else
                        {
                            using (MemoryStream stream = new MemoryStream())
                            {
                                actors.ExtraData[actors.Items[i].DataID].Data.WriteToFile(stream, false);
                                writer.Write(stream.GetBuffer());
                            }
                        }
                    }
                }
            }
            treeView1.Nodes.Add(definitions);
            treeView1.Nodes.Add(items);
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

        private void ContextDelete_Click(object sender, System.EventArgs e)
        {

        }
    }
}