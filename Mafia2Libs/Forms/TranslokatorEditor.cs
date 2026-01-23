using ResourceTypes.Translokator;
using System;
using System.IO;
using System.Windows.Forms;
using Utils.Language;

namespace Mafia2Tool.Forms
{
    public partial class TranslokatorEditor : Form
    {
        private FileInfo file;
        private TranslokatorLoader translokator;
        private object clipboard;

        private bool bIsFileEdited = false;

        public TranslokatorEditor(FileInfo info)
        {
            InitializeComponent();
            file = info;
            Localise();
            LoadFile();
            Show();
        }

        private void Localise()
        {
            ExitButton.Text = Language.GetString("$EXIT");
            fileToolButton.Text = Language.GetString("$FILE");
            SaveToolButton.Text = Language.GetString("$SAVE");
            ReloadButton.Text = Language.GetString("$RELOAD");
            AddInstance.Text = Language.GetString("$ADD_INSTANCE");
            AddObject.Text = Language.GetString("$ADD_OBJECT");
            AddGroup.Text = Language.GetString("$ADD_GROUP");
            Delete.Text = Language.GetString("$DELETE");
            Text = Language.GetString("$TRANSLOKATOR_EDITOR");
            CopyButton.Text = Language.GetString("$COPY");
            PasteButton.Text = Language.GetString("$PASTE");
            ToolsButton.Text = Language.GetString("$TOOLS");
            ViewNumInstButton.Text = Language.GetString("$VIEW_NUM_INST");
            Button_ExportXml.Text = Language.GetString("$EXPORT_XML");
            Button_ImportXml.Text = Language.GetString("$IMPORT_XML");
        }

        private void LoadFile()
        {
            translokator = new TranslokatorLoader(file);

            LoadData();
        }

        private void LoadData()
        {
            TranslokatorTree.Nodes.Clear();

            TreeNode headerData = new TreeNode("Header Data");
            headerData.Tag = translokator;

            TreeNode gridNode = new TreeNode("Grids");
            for (int i = 0; i < translokator.Grids.Length; i++)
            {
                Grid grid = translokator.Grids[i];
                TreeNode child = new TreeNode("Grid " + i);
                child.Tag = grid;
                gridNode.Nodes.Add(child);
            }
            TreeNode ogNode = new TreeNode("Objects Groups");
            ogNode.Tag = "OBJ_GROUPS";
            for (int i = 0; i < translokator.ObjectGroups.Length; i++)
            {
                ObjectGroup objectGroup = translokator.ObjectGroups[i];
                TreeNode objectGroupNode = new TreeNode(String.Format("Object Group: [{0}]", objectGroup.ActorType));
                objectGroupNode.Tag = objectGroup;

                for (int y = 0; y < objectGroup.Objects.Length; y++)
                {
                    ResourceTypes.Translokator.Object obj = objectGroup.Objects[y];
                    TreeNode objNode = new TreeNode(obj.Name.ToString());
                    objNode.Tag = obj;
                    objectGroupNode.Nodes.Add(objNode);

                    for (int x = 0; x < obj.Instances.Length; x++)
                    {
                        Instance instance = obj.Instances[x];
                        TreeNode instanceNode = new TreeNode(obj.Name + " " + x);
                        instanceNode.Tag = instance;
                        objNode.Nodes.Add(instanceNode);
                    }
                }

                ogNode.Nodes.Add(objectGroupNode);
            }
            TranslokatorTree.Nodes.Add(headerData);
            TranslokatorTree.Nodes.Add(gridNode);
            TranslokatorTree.Nodes.Add(ogNode);

            Text = Language.GetString("$TRANSLOKATOR_EDITOR");
            bIsFileEdited = false;
        }

        private void SaveFile()
        {
            translokator.Grids = new Grid[TranslokatorTree.Nodes[1].GetNodeCount(false)];
            for (int i = 0; i < translokator.Grids.Length; i++)
            {
                Grid grid = (TranslokatorTree.Nodes[1].Nodes[i].Tag as Grid);
                translokator.Grids[i] = grid;
            }

            translokator.ObjectGroups = new ObjectGroup[TranslokatorTree.Nodes[2].GetNodeCount(false)];
            for (int i = 0; i < translokator.ObjectGroups.Length; i++)
            {
                ObjectGroup objectGroup = (TranslokatorTree.Nodes[2].Nodes[i].Tag as ObjectGroup);
                objectGroup.Objects = new ResourceTypes.Translokator.Object[TranslokatorTree.Nodes[2].Nodes[i].GetNodeCount(false)];
                for (int y = 0; y < objectGroup.Objects.Length; y++)
                {
                    ResourceTypes.Translokator.Object obj = (TranslokatorTree.Nodes[2].Nodes[i].Nodes[y].Tag as ResourceTypes.Translokator.Object);
                    obj.Instances = new Instance[TranslokatorTree.Nodes[2].Nodes[i].Nodes[y].GetNodeCount(false)];
                    for (int z = 0; z < obj.Instances.Length; z++)
                    {
                        Instance instance = (TranslokatorTree.Nodes[2].Nodes[i].Nodes[y].Nodes[z].Tag as Instance);
                        obj.Instances[z] = instance;
                    }
                    objectGroup.Objects[y] = obj;
                }

                translokator.ObjectGroups[i] = objectGroup;
            }
            translokator.WriteToFile(file);

            Text = Language.GetString("$TRANSLOKATOR_EDITOR");
            bIsFileEdited = false;
        }

        private void AddInstanceNode()
        {
            if (TranslokatorTree.SelectedNode.Tag is ResourceTypes.Translokator.Object)
            {
                ResourceTypes.Translokator.Object obj = (TranslokatorTree.SelectedNode.Tag as ResourceTypes.Translokator.Object);
                Instance instance = new Instance();
                TreeNode instanceNode = new TreeNode(obj.Name + " " + TranslokatorTree.SelectedNode.GetNodeCount(false));
                instanceNode.Tag = instance;
                TranslokatorTree.SelectedNode.Nodes.Add(instanceNode);

                Text = Language.GetString("$TRANSLOKATOR_EDITOR") + "*";
                bIsFileEdited = true;
            }
            if (TranslokatorTree.SelectedNode.Tag is Instance && TranslokatorTree.SelectedNode.Parent.Tag is ResourceTypes.Translokator.Object)
            {
                var obj = (TranslokatorTree.SelectedNode.Parent.Tag as ResourceTypes.Translokator.Object);
                Instance instance = new Instance();
                TreeNode instanceNode = new TreeNode(obj.Name + " " + TranslokatorTree.SelectedNode.Parent.GetNodeCount(false));
                instanceNode.Tag = instance;
                TranslokatorTree.SelectedNode.Parent.Nodes.Add(instanceNode);

                Text = Language.GetString("$TRANSLOKATOR_EDITOR") + "*";
                bIsFileEdited = true;
            }
        }

        private void AddObjectNode()
        {
            if (TranslokatorTree.SelectedNode.Tag is ObjectGroup)
            {
                ObjectGroup group = (TranslokatorTree.SelectedNode.Tag as ObjectGroup);
                ResourceTypes.Translokator.Object obj = new ResourceTypes.Translokator.Object();
                obj.Name.Set("NewObject");
                TreeNode instanceNode = new TreeNode(obj.Name + " " + TranslokatorTree.SelectedNode.GetNodeCount(false));
                instanceNode.Tag = obj;
                TranslokatorTree.SelectedNode.Nodes.Add(instanceNode);

                Text = Language.GetString("$TRANSLOKATOR_EDITOR") + "*";
                bIsFileEdited = true;
            }
        }
        private void AddObjectGroupNode()
        {
            // If not correct node, leave early.
            string AsString = TranslokatorTree.SelectedNode.Tag.ToString();
            if(AsString.Equals("OBJ_GROUPS") == false)
            {
                return;
            }

            ObjectGroup NewObjectGroup = new ObjectGroup();
            TreeNode NewNode = new TreeNode(String.Format("Object Group: [{0}]", NewObjectGroup.ActorType));
            NewNode.Tag = NewObjectGroup;

            TranslokatorTree.Nodes[2].Nodes.Add(NewNode);
        }

        private void DeleteNode()
        {
            if (TranslokatorTree.SelectedNode != null && TranslokatorTree.SelectedNode.Tag != null)
            {
                TranslokatorTree.Nodes.Remove(TranslokatorTree.SelectedNode);

                Text = Language.GetString("$TRANSLOKATOR_EDITOR") + "*";
                bIsFileEdited = true;
            }
        }

        private void Copy()
        {
            if (TranslokatorTree.SelectedNode != null && TranslokatorTree.SelectedNode.Tag != null)
            {
                clipboard = TranslokatorTree.SelectedNode.Tag;
            }
        }

        private void Paste()
        {
            var data = clipboard;
            if (data != null)
            {
                if (TranslokatorTree.SelectedNode != null && TranslokatorTree.SelectedNode.Tag != null)
                {
                    var tag = TranslokatorTree.SelectedNode.Tag;
                    if (tag is ResourceTypes.Translokator.Object)
                    {
                        TranslokatorTree.SelectedNode.Tag = new ResourceTypes.Translokator.Object((ResourceTypes.Translokator.Object)clipboard);
                    }
                    else if (tag is Instance && data is Instance)
                    {
                        TranslokatorTree.SelectedNode.Tag = new Instance((Instance)clipboard);
                    }

                    Text = Language.GetString("$TRANSLOKATOR_EDITOR") + "*";
                    bIsFileEdited = true;
                }
            }
            PropertyGrid.SelectedObject = TranslokatorTree?.SelectedNode.Tag;
        }

        private void TranslokatorTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (TranslokatorTree?.SelectedNode.Tag != null)
            {
                PropertyGrid.SelectedObject = TranslokatorTree?.SelectedNode.Tag;
            }
        }

        private void TranslokatorContext_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Clear state
            AddInstance.Enabled = false;
            AddObject.Enabled = false;
            AddGroup.Enabled = false;
            Delete.Enabled = false;
            CopyButton.Enabled = false;
            PasteButton.Enabled = false;

            // Update state
            if (TranslokatorTree.SelectedNode != null && TranslokatorTree.SelectedNode.Tag != null)
            {
                object Tag = TranslokatorTree.SelectedNode.Tag;
                if (Tag.GetType() == typeof(ResourceTypes.Translokator.Object))
                {
                    AddInstance.Enabled = true;
                }
                if (Tag.GetType() == typeof(ObjectGroup))
                {
                    AddObject.Enabled = true;
                }
                if (Tag.GetType() == typeof(Instance) 
                    || Tag.GetType() == typeof(ResourceTypes.Translokator.Object)
                    || Tag.GetType() == typeof(ObjectGroup))
                {
                    Delete.Enabled = true;
                }
                if(Tag.GetType() == typeof(string))
                {
                    string AsString = (Tag as string);
                    AddGroup.Enabled = AsString.Equals("OBJ_GROUPS");
                }
                if (Tag.GetType() == typeof(ResourceTypes.Translokator.Object) ||
                    Tag.GetType() == typeof(Instance))
                {
                    CopyButton.Enabled = true;
                    PasteButton.Enabled = true;
                }
            }

            // Need to hide context if none are visible
            bool bAnyEnabled = false;
            for (int i = 0; i != TranslokatorContext.Items.Count; i++)
            {
                if (TranslokatorContext.Items[i].Enabled)
                {
                    bAnyEnabled = true;
                    break;
                }
            }

            if(!bAnyEnabled)
            {
                e.Cancel = true;
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if(e.Control && e.KeyCode == Keys.A)
            {
                object Tag = TranslokatorTree.SelectedNode.Tag;
                if (Tag is ObjectGroup)
                {
                    AddObjectNode();
                }
                else if (Tag is ResourceTypes.Translokator.Object || TranslokatorTree.SelectedNode.Tag is Instance)
                {
                    AddInstanceNode();
                }
                else if(Tag is string)
                {
                    string AsString = Tag.ToString();
                    if(AsString.Equals("OBJ_GROUPS"))
                    {
                        AddObjectGroupNode();
                    }
                }
            }
        }

        private void PropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (e.ChangedItem.Label == "Name")
            {
                TreeNode selected = TranslokatorTree.SelectedNode;
                TranslokatorTree.SelectedNode.Text = e.ChangedItem.Value.ToString();

                if(selected.Tag is ResourceTypes.Translokator.Object)
                {
                    for(int i = 0; i < selected.Nodes.Count; i++)
                    {
                        selected.Nodes[i].Text = selected.Text + " " + i;
                    }
                }
            }
            else if(e.ChangedItem.Label == "ActorType")
            {
                TreeNode selected = TranslokatorTree.SelectedNode;
                if(selected.Tag is ObjectGroup)
                {
                    ObjectGroup AsObjectGroup = (selected.Tag as ObjectGroup);
                    string NewName = String.Format("Object Group: [{0}]", AsObjectGroup.ActorType);
                    selected.Name = NewName;
                    selected.Text = NewName;
                }
            }

            PropertyGrid.Refresh();
            Cursor.Current = Cursors.Default;

            Text = Language.GetString("$TRANSLOKATOR_EDITOR") + "*";
            bIsFileEdited = true;
        }
        private void ViewNumInstButton_Click(object sender, EventArgs e)
        {
            var num = 0;

            for (int i = 0; i < TranslokatorTree.Nodes[2].GetNodeCount(false); i++)
            {
                for (int y = 0; y < TranslokatorTree.Nodes[2].Nodes[i].GetNodeCount(false); y++)
                {
                    for (int z = 0; z < (TranslokatorTree.Nodes[2].Nodes[i].Nodes[y].GetNodeCount(false)); z++)
                    {
                        if (TranslokatorTree.Nodes[2].Nodes[i].Nodes[y].Nodes[z].Tag is Instance)
                        {
                            num++;
                        }
                    }
                }
            }
            MessageBox.Show(string.Format("Number of Instances: {0}", num), "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void TranslocatorEditor_Closing(object sender, FormClosingEventArgs e)
        {
            if (bIsFileEdited)
            {
                System.Windows.MessageBoxResult SaveChanges = System.Windows.MessageBox.Show(Language.GetString("$SAVE_PROMPT"), "Toolkit", System.Windows.MessageBoxButton.YesNoCancel);

                if (SaveChanges == System.Windows.MessageBoxResult.Yes)
                {
                    SaveFile();
                }
                else if (SaveChanges == System.Windows.MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void ReloadButton_Click(object sender, EventArgs e)
        {
            LoadFile();

            TranslokatorTree.SelectedNode = null;
            PropertyGrid.SelectedObject = null;

        }

        private void AddObjectOnClick(object sender, EventArgs e) => AddObjectNode();
        private void AddInstance_Click(object sender, EventArgs e) => AddInstanceNode();
        private void OnAddGroupPressed(object sender, EventArgs e) => AddObjectGroupNode();
        private void Delete_Click(object sender, EventArgs e) => DeleteNode();
        private void CopyButton_Click(object sender, EventArgs e) => Copy();
        private void PasteButton_Click(object sender, EventArgs e) => Paste();
        private void SaveToolButton_Click(object sender, EventArgs e) => SaveFile();
        private void ExitButton_Click(object sender, EventArgs e) => Close();
        private void Button_ExportXml_OnClick(object sender, System.EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "XML|*.XML";

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                translokator.ConvertToXML(saveFile.FileName);
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
                    translokator.ConvertFromXML(FileToOpen);

                    LoadData();
                }
            }
        }
    }
}
