using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using ResourceTypes.Actors;
using Utils.Language;
using Utils.Settings;
using Forms.EditorControls;

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
            EditButton.Text = Language.GetString("$EDIT");
            AddDefinitionButton.Text = Language.GetString("$ADD_DEFINITION");
            AddItemButton.Text = Language.GetString("$ADD_ITEM");
        }

        private void BuildData()
        {
            actors = new Actor(actorFile.FullName);
            definitions = new TreeNode("Definitions");
            items = new TreeNode("Entities");

            for (int i = 0; i != actors.Definitions.Count; i++)
            {
                TreeNode node = new TreeNode(actors.Definitions[i].Name);
                node.Name = actors.Definitions[i].DefinitionHash.ToString();
                node.Tag = actors.Definitions[i];
                definitions.Nodes.Add(node);
            }

            for (int i = 0; i != actors.Items.Count; i++)
            {
                TreeNode node = new TreeNode(actors.Items[i].EntityName);
                node.Tag = actors.Items[i];

                TreeNode child = new TreeNode("Extra Data");
                child.Tag = actors.ExtraData[actors.Items[i].DataID];
                node.Nodes.Add(child);
                items.Nodes.Add(node);

                if (Debugger.IsAttached)
                {
                    string folder = "actors_unks/" + (ActorTypes)actors.Items[i].ActorTypeID + "/";
                    string filename = actors.Items[i].EntityName + ".dat";

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
            object data = ActorTreeView.SelectedNode.Tag;
            bool isDeleted = false;
            if (data is ActorEntry)
            {
                actors.Items.Remove((ActorEntry)data);
                isDeleted = true;
            }
            else if(data is ActorDefinition)
            {
                actors.Definitions.Remove((ActorDefinition)data);
                isDeleted = true;
            }

            if(isDeleted)
            {
                ActorTreeView.Nodes.Remove(ActorTreeView.SelectedNode);
            }
        }

        private void SaveButton_OnClick(object sender, System.EventArgs e)
        {
            File.Copy(actorFile.FullName, actorFile.FullName + "_old", true);
            using (BinaryWriter writer = new BinaryWriter(File.Open(actorFile.FullName, FileMode.Create)))
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

        private void AddItemButton_Click(object sender, System.EventArgs e)
        {
            NewObjectForm objectForm = new NewObjectForm(true);
            objectForm.SetLabel("$SELECT_TYPE_AND_NAME");
            ActorItemAddOption optionControl = new ActorItemAddOption();
            objectForm.LoadOption(optionControl);

            if (objectForm.ShowDialog() == DialogResult.OK)
            {
                ActorTypes type = optionControl.GetSelectedType();
                string def = optionControl.GetDefinitionName();
                ActorEntry entry = actors.CreateActorEntry(type, objectForm.GetInputText());
                entry.DefinitionName = def;

                TreeNode node = new TreeNode(entry.EntityName);
                node.Text = entry.EntityName;
                node.Tag = entry;

                TreeNode child = new TreeNode("Extra Data");
                child.Tag = actors.ExtraData[entry.DataID];
                node.Nodes.Add(child);
                items.Nodes.Add(node);
            }

            objectForm.Dispose();
        }

        private void AddDefinitionButton_Click(object sender, System.EventArgs e)
        {
            ListWindow window = new ListWindow();
            window.PopulateForm(actors.Items);

            if(window.ShowDialog() == DialogResult.OK)
            {
                ActorDefinition definition = actors.CreateActorDefinition((window.chosenObject as ActorEntry));
                TreeNode node = new TreeNode(definition.Name);
                node.Name = definition.DefinitionHash.ToString();
                node.Tag = definition;
                definitions.Nodes.Add(node);
            }
        }

        private void ActorGrid_OnPropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            ActorGrid.Refresh();
        }
    }
}