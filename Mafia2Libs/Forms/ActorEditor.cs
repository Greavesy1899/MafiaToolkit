﻿using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using ResourceTypes.Actors;
using Utils.Language;
using Utils.Settings;
using Utils.Helpers.Reflection;
using Forms.EditorControls;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace Mafia2Tool
{
    public partial class ActorEditor : Form
    {
        private FileInfo actorFile;
        private Actor actors;

        private TreeNode definitions;
        private TreeNode items;

        private object clipboard;

        private bool bIsFileEdited = false;


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
            ContextCopy.Text = Language.GetString("$COPY");
            ContextPaste.Text = Language.GetString("$PASTE");
            Button_MoveDown.Text = Language.GetString("$MOVE_DOWN");
            Button_MoveUp.Text = Language.GetString("$MOVE_UP");
        }

        private void BuildData()
        {
            actors = new Actor(actorFile);
            definitions = new TreeNode("Definitions");
            items = new TreeNode("Entities");

            for (int i = 0; i != actors.Definitions.Count; i++)
            {
                TreeNode node = new TreeNode(actors.Definitions[i].Name);
                node.Name = actors.Definitions[i].FrameNameHash.ToString();
                node.Tag = actors.Definitions[i];
                definitions.Nodes.Add(node);
            }

            for (int i = 0; i != actors.Items.Count; i++)
            {
                TreeNode node = new TreeNode(actors.Items[i].EntityName);
                node.Tag = actors.Items[i];

                if (actors.Items[i].DataID != -1)
                {
                    TreeNode child = new TreeNode("Extra Data");
                    child.Tag = actors.ExtraData[actors.Items[i].DataID];
                    node.Nodes.Add(child);

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

                items.Nodes.Add(node);
            }
            ActorTreeView.Nodes.Add(definitions);
            ActorTreeView.Nodes.Add(items);
        }

        private void Save()
        {
            File.Copy(actorFile.FullName, actorFile.FullName + "_old", true);
            using (BinaryWriter writer = new BinaryWriter(File.Open(actorFile.FullName, FileMode.Create)))
            {
                actors.WriteToFile(writer);
            }

            Text = Language.GetString("$ACTOR_EDITOR_TITLE");
            bIsFileEdited = false;
        }

        private void Reload()
        {
            ActorTreeView.Nodes.Clear();
            BuildData();

            ActorGrid.SelectedObject = null;
            ActorTreeView.SelectedNode = null;

            Text = Language.GetString("$ACTOR_EDITOR_TITLE");
            bIsFileEdited = false;
        }

        private void Copy()
        {
            TreeNode SelectedNode = ActorTreeView.SelectedNode;
            if (ActorTreeView.SelectedNode.Text.Equals("Extra Data"))
            {
                ActorExtraData ExtraData = (SelectedNode.Tag as ActorExtraData);
                clipboard = ExtraData;
            }
        }

        private void Paste()
        {
            TreeNode SelectedNode = ActorTreeView.SelectedNode;
            if (!ActorTreeView.SelectedNode.Text.Equals("Extra Data"))
            {
                // Not on the correct node type
                return;
            }

            ActorExtraData ExtraDataToCopy = (clipboard as ActorExtraData);
            ActorExtraData ExtaDataToEdit = (SelectedNode.Tag as ActorExtraData);
            if (ExtraDataToCopy.BufferType != ExtaDataToEdit.BufferType)
            {
                // Dont accept if types are not the same.
                // Do not allow replacement of types.
                return;
            }

            // Copy contents and then assign the new pasted data into the selected extra data.
            object ObjectToCopy = ExtraDataToCopy.Data;
            object NewObject = Activator.CreateInstance(ObjectToCopy.GetType());
            ReflectionHelpers.Copy(ObjectToCopy, ref NewObject);
            ExtaDataToEdit.Data = (NewObject as IActorExtraDataInterface);

            // Force reload
            ActorGrid.SelectedObject = SelectedNode.Tag;

            // Mark as edited
            Text = Language.GetString("$ACTOR_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

        private bool IsTypeofInterface(object ObjectToCheck, Type InterfaceType)
        {
            Type TypeOfObject = ObjectToCheck.GetType();
            return InterfaceType.IsAssignableFrom(TypeOfObject);
        }

        private void Delete()
        {
            object data = ActorTreeView.SelectedNode.Tag;
            bool isDeleted = false;
            if (data is ActorEntry)
            {
                actors.Items.Remove((ActorEntry)data);
                isDeleted = true;
            }
            else if (data is ActorDefinition)
            {
                actors.Definitions.Remove((ActorDefinition)data);
                isDeleted = true;
            }

            if (isDeleted)
            {
                ActorTreeView.Nodes.Remove(ActorTreeView.SelectedNode);

                Text = Language.GetString("$ACTOR_EDITOR_TITLE") + "*";
                bIsFileEdited = true;
            }
        }

        private void OnNodeSelectSelect(object sender, TreeViewEventArgs e)
        {
            ActorGrid.SelectedObject = e.Node.Tag;
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

                if (entry.DataID != -1)
                {
                    TreeNode child = new TreeNode("Extra Data");
                    child.Tag = actors.ExtraData[entry.DataID];
                    node.Nodes.Add(child);
                }

                items.Nodes.Add(node);
            }

            Text = Language.GetString("$ACTOR_EDITOR_TITLE") + "*";
            bIsFileEdited = true;

            objectForm.Dispose();
        }

        private void AddDefinitionButton_Click(object sender, System.EventArgs e)
        {
            ListWindow window = new ListWindow();
            window.PopulateForm(actors.Items);

            if (window.ShowDialog() == DialogResult.OK)
            {
                ActorDefinition definition = actors.CreateActorDefinition((window.chosenObject as ActorEntry));
                TreeNode node = new TreeNode(definition.Name);
                node.Name = definition.FrameNameHash.ToString();
                node.Tag = definition;
                definitions.Nodes.Add(node);

                Text = Language.GetString("$ACTOR_EDITOR_TITLE") + "*";
                bIsFileEdited = true;
            }
        }

        private void ActorTreeView_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.D)
            {
                ActorGrid.SelectedObject = null;
                ActorTreeView.SelectedNode = null;
            }
            else if(e.Control && e.KeyCode == Keys.PageUp)
            {
                MoveItemUp();
            }
            else if(e.Control && e.KeyCode == Keys.PageDown)
            {
                MoveItemDown();
            }
        }

        private void ActorGrid_OnPropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (e.ChangedItem.Label == "Name" || e.ChangedItem.Label == "EntityName")
                ActorTreeView.SelectedNode.Text = e.ChangedItem.Value.ToString();

            Text = Language.GetString("$ACTOR_EDITOR_TITLE") + "*";
            bIsFileEdited = true;

            ActorGrid.Refresh();
        }

        private void ActorEditor_Closing(object sender, FormClosingEventArgs e)
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

        private void ContextDelete_Click(object sender, System.EventArgs e) => Delete();
        private void SaveButton_OnClick(object sender, System.EventArgs e) => Save();
        private void ReloadButton_OnClick(object sender, System.EventArgs e) => Reload();
        private void ExitButton_OnClick(object sender, System.EventArgs e) => Close();
        private void ContextCopy_Click(object sender, System.EventArgs e) => Copy();
        private void ContextPaste_Click(object sender, System.EventArgs e) => Paste();
        private void Button_MoveUp_Clicked(object sender, EventArgs e) => MoveItemUp();
        private void Button_MoveDown_Clicked(object sender, EventArgs e) => MoveItemDown();
        private void ContextMenu_OnOpening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ContextCopy.Visible = false;
            ContextPaste.Visible = false;
            Button_MoveDown.Visible = false;
            Button_MoveUp.Visible = false;

            TreeNode SelectedNode = ActorTreeView.SelectedNode;
            if (SelectedNode != null && SelectedNode.Tag != null)
            {
                if (SelectedNode.Text.Equals("Extra Data") || SelectedNode.Tag is ActorExtraData)
                {
                    ContextCopy.Visible = true;
                    ContextPaste.Visible = true;
                }

                // For now, Move Up/Down only active for ActorEntry.
                ActorEntry Item = (SelectedNode.Tag as ActorEntry);
                if(Item != null)
                {
                    Button_MoveDown.Visible = true;
                    Button_MoveUp.Visible = true;
                }
            }
        }

        private void MoveItemDown()
        {
            TreeNode SelectedNode = ActorTreeView.SelectedNode;
            if (SelectedNode == null || SelectedNode.Tag == null)
            {
                return;
            }

            ActorEntry Item = (SelectedNode.Tag as ActorEntry);
            if (Item == null)
            {
                // Only works for ActorEntry for now
                return;
            }

            int Index = actors.Items.IndexOf(Item);
            int NextIndex = (actors.Items.Count != Index ? Index + 1 : -1);
            if (NextIndex == -1)
            {
                return;
            }

            // Can move down, start by swapping entires
            ActorEntry ItemBelow = actors.Items[NextIndex];
            actors.Items[Index] = ItemBelow;
            actors.Items[NextIndex] = Item;

            // Now move down in TreeView
            TreeNode ParentNode = SelectedNode.Parent;
            int NodeIndex = ParentNode.Nodes.IndexOf(SelectedNode);
            ParentNode.Nodes.RemoveAt(NodeIndex);
            ParentNode.Nodes.Insert(NodeIndex + 1, SelectedNode);
            ActorTreeView.SelectedNode = SelectedNode;

            // Update UI
            Text = Language.GetString("$STREAM_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

        private void MoveItemUp()
        {
            TreeNode SelectedNode = ActorTreeView.SelectedNode;
            if (SelectedNode == null || SelectedNode.Tag == null)
            {
                return;
            }

            ActorEntry Item = (SelectedNode.Tag as ActorEntry);
            if (Item == null)
            {
                // Only works for ActorEntry for now
                return;
            }

            int Index = actors.Items.IndexOf(Item);
            int NextIndex = (Index != 0 ? Index - 1 : -1);
            if (NextIndex == -1)
            {
                return;
            }

            // Can move up, start by swapping entires
            ActorEntry ItemAbove = actors.Items[NextIndex];
            actors.Items[Index] = ItemAbove;
            actors.Items[NextIndex] = Item;

            // Now move up in TreeView
            TreeNode ParentNode = SelectedNode.Parent;
            int NodeIndex = ParentNode.Nodes.IndexOf(SelectedNode);
            ParentNode.Nodes.RemoveAt(NodeIndex);
            ParentNode.Nodes.Insert(NodeIndex - 1, SelectedNode);
            ActorTreeView.SelectedNode = SelectedNode;

            // Update UI
            Text = Language.GetString("$STREAM_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }
    }
}
