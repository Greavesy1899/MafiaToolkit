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
            FileButton.Text = Language.GetString("$FILE");
            SaveButton.Text = Language.GetString("$SAVE");
            ReloadButton.Text = Language.GetString("$RELOAD");
            ExitButton.Text = Language.GetString("$EXIT");
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

                if (Debugger.IsAttached)
                {
                    string folder = "actors_unks/" + (ActorTypes)actors.Items[i].ActorTypeID + "/";
                    string filename = actors.Items[i].EntityType + ".dat";

                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }

                    File.WriteAllBytes(Path.Combine(folder, filename), actors.Items[i].Data.GetDataInBytes());
                }
            }
            ActorTreeView.Nodes.Add(definitions);
            ActorTreeView.Nodes.Add(items);
        }

        private void OnNodeSelectSelect(object sender, TreeViewEventArgs e)
        {
            ActorGrid.SelectedObject = e.Node.Tag;
        }

        private void ContextDelete_Click(object sender, System.EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void SaveButton_OnClick(object sender, System.EventArgs e)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(actorFile.FullName + "EDIT", FileMode.Create)))
            {
                actors.WriteToFile(writer);
            }
        }

        private void ReloadButton_OnClick(object sender, System.EventArgs e)
        {
            ActorTreeView.Nodes.Clear();
            BuildData();
        }

        private void ExitButton_OnClick(object sender, System.EventArgs e)
        {
            Close();
        }
    }
}