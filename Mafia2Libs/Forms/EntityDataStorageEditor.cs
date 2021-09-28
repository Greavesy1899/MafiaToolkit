using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using ResourceTypes.Actors;
using ResourceTypes.EntityDataStorage;
using Utils.Helpers.Reflection;
using Utils.Language;
using Utils.Settings;

namespace Toolkit.Forms
{
    // TODO: IsTypeofInterface is going to be redundant once we merge branches.
    public partial class EntityDataStorageEditor : Form
    {
        private FileInfo edsFile;
        private EntityDataStorageLoader tables;

        private object OurClipboard;

        private bool bIsFileEdited = false;

        public EntityDataStorageEditor(FileInfo file)
        {
            InitializeComponent();
            Localise();
            edsFile = file;
            LoadAndBuildData();
            Show();
            ToolkitSettings.UpdateRichPresence("Using the EDS editor.");
        }

        private void Localise()
        {
            Text = Language.GetString("$EDS_EDITOR_TITLE");
            Button_File.Text = Language.GetString("$FILE");
            Button_Tools.Text = Language.GetString("$TOOLS");
            Button_Save.Text = Language.GetString("$SAVE");
            Button_Reload.Text = Language.GetString("$RELOAD");
            Button_Exit.Text = Language.GetString("$EXIT");
            Button_CopyData.Text = Language.GetString("$COPY");
            Button_PasteData.Text = Language.GetString("$PASTE");
            Button_ExportXML.Text = Language.GetString("$EXPORT_XML");
            Button_ImportXML.Text = Language.GetString("$IMPORT_XML");
            ToolStrip_Copy.Text = Language.GetString("$COPY");
            ToolStrip_Paste.Text = Language.GetString("$PASTE");
            FileDialog_Open.Title = Language.GetString("$OPEN_TITLE");
            FileDialog_Save.Title = Language.GetString("$SAVE_TITLE");
        }

        private void LoadAndBuildData()
        {
            tables = new EntityDataStorageLoader();
            tables.ReadFromFile(edsFile.FullName, false);

            CreateTable();
        }

        private void CreateTable()
        {
            string EntityName = string.Format("Entity [{0}]", tables.Hash);
            TreeNode entityNode = new TreeNode(EntityName);
            entityNode.Tag = tables;
            TreeView_Tables.Nodes.Add(entityNode);

            if (tables.Tables != null)
            {
                for (int i = 0; i < tables.Tables.Length; i++)
                {
                    string TableName = string.Format("Table [{0}]", tables.TableHashes[i]);
                    TreeNode node = new TreeNode(TableName);
                    node.Tag = tables.Tables[i];
                    entityNode.Nodes.Add(node);
                }
            }
        }

        private void FileIsEdited()
        {
            Text = Language.GetString("$EDS_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

        private void FileIsNotEdited()
        {
            Text = Language.GetString("$EDS_EDITOR_TITLE");
            bIsFileEdited = false;
        }

        private void Save()
        {
            TreeNode TableRoot = TreeView_Tables.Nodes[0];

            // Update tables from treeview
            tables.Tables = new IActorExtraDataInterface[TableRoot.Nodes.Count];
            for (int i = 0; i < tables.Tables.Length; i++)
            {
                tables.Tables[i] = (IActorExtraDataInterface)TableRoot.Nodes[i].Tag;
            }

            // Write to file
            File.Copy(edsFile.FullName, edsFile.FullName + "_old", true);
            tables.WriteToFile(edsFile.FullName, false);

            FileIsNotEdited();
        }

        private void Reload()
        {
            TreeView_Tables.Nodes.Clear();
            LoadAndBuildData();

            PropertyGrid_Item.SelectedObject = null;
            TreeView_Tables.SelectedNode = null;

            FileIsNotEdited();
        }

        private void CopyTagData()
        {
            TreeNode SelectedNode = TreeView_Tables.SelectedNode;
            if (SelectedNode != null && SelectedNode.Tag != null)
            {
                if (IsTypeofInterface(SelectedNode.Tag, typeof(IActorExtraDataInterface)))
                {
                    OurClipboard = SelectedNode.Tag;
                }
            }
        }

        private void PasteTagData()
        {
            if (OurClipboard == null)
            {
                // Nothing to copy
                return;
            }

            // Attempt to copy
            TreeNode SelectedNode = TreeView_Tables.SelectedNode;
            if (SelectedNode != null && SelectedNode.Tag != null)
            {
                if (IsTypeofInterface(SelectedNode.Tag, typeof(IActorExtraDataInterface)))
                {
                    object ObjectToCopy = OurClipboard;
                    object NewObject = Activator.CreateInstance(ObjectToCopy.GetType());
                    ReflectionHelpers.Copy(ObjectToCopy, NewObject);
                    SelectedNode.Tag = NewObject;

                    // Force reload
                    PropertyGrid_Item.SelectedObject = SelectedNode.Tag;
                }

                FileIsEdited();
            }
        }

        private void OnNodeSelectSelect(object sender, TreeViewEventArgs e)
        {
            PropertyGrid_Item.SelectedObject = e.Node.Tag;
        }

        private void EDSEditor_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.D)
            {
                PropertyGrid_Item.SelectedObject = null;
                TreeView_Tables.SelectedNode = null;
            }
        }

        private void Button_Save_OnClick(object sender, EventArgs e) => Save();
        private void Button_Reload_OnClick(object sender, EventArgs e) => Reload();
        private void Button_Exit_OnClick(object sender, EventArgs e) => Close();
        private void Button_CopyData_Click(object sender, EventArgs e) => CopyTagData();
        private void Button_Paste_Click(object sender, EventArgs e) => PasteTagData();
        private void ToolStrip_Copy_Click(object sender, EventArgs e) => CopyTagData();
        private void ToolStrip_Paste_Click(object sender, EventArgs e) => PasteTagData();
        private void ContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TreeNode SelectedNode = TreeView_Tables.SelectedNode;
            if (SelectedNode != null && SelectedNode.Tag != null)
            {
                if (IsTypeofInterface(SelectedNode.Tag, typeof(IActorExtraDataInterface)))
                {
                    ToolStrip_Copy.Enabled = true;
                    ToolStrip_Paste.Enabled = true;
                    return;
                }
            }

            e.Cancel = true;
        }

        // TODO: Should we move this into a util?
        private bool IsTypeofInterface(object ObjectToCheck, Type InterfaceType)
        {
            Type TypeOfObject = ObjectToCheck.GetType();
            return InterfaceType.IsAssignableFrom(TypeOfObject);
        }

        private void Button_ExportXML_Click(object sender, EventArgs e)
        {
            if(FileDialog_Save.ShowDialog() == DialogResult.OK)
            {
                tables.ConvertToXML(FileDialog_Save.FileName);
            }   
        }

        private void Button_ImportXML_Click(object sender, EventArgs e)
        {
            if(FileDialog_Open.ShowDialog() == DialogResult.OK)
            {
                string FileToOpen = FileDialog_Open.FileName;
                if(File.Exists(FileToOpen))
                {
                    tables.ConvertFromXML(FileToOpen);

                    // Reload TreeVieew and PropertyGrid, the import may mean our 'visual' part of the data is extremely outdated
                    TreeView_Tables.Nodes.Clear();
                    PropertyGrid_Item.SelectedObject = null;

                    CreateTable();

                    FileIsEdited();
                }
            }
        }

        private void PropertyGrid_OnValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            FileIsEdited();

            // Check if property 'Hash' needs to be updated
            if (PropertyGrid_Item.SelectedObject is EntityDataStorageLoader)
            {
                if (e.ChangedItem.Label.Equals("Hash"))
                {
                    string NewEntityName = string.Format("Entity [{0}]", e.ChangedItem.Value);
                    TreeView_Tables.SelectedNode.Text = NewEntityName;
                    TreeView_Tables.SelectedNode.Name = NewEntityName;
                }
                else
                {
                    EntityDataStorageLoader EDSLoader = (PropertyGrid_Item.SelectedObject as EntityDataStorageLoader);
                    string NewEntityName = string.Format("Entity [{0}]", EDSLoader.Hash);

                    Debug.Assert(EDSLoader.TableHashes.Length == TreeView_Tables.SelectedNode.Nodes.Count, "WARNING: This editor does not support deleting/adding new tables. " +
                        "The length of 'TableHashes' NEEDS to equal the same amount of child nodes attached to " + NewEntityName);

                    int Index = 0;
                    foreach (TreeNode ChildNode in TreeView_Tables.SelectedNode.Nodes)
                    {
                        string NewTableName = string.Format("Table [{0}]", EDSLoader.TableHashes[Index]);
                        ChildNode.Text = NewTableName;
                        ChildNode.Name = NewTableName;

                        Index++;
                    }
                }
            }
        }

        private void EDSEditor_Closing(object sender, FormClosingEventArgs e)
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
    }
}