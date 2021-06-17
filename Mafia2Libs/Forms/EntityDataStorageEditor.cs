using System;
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
            TreeNode entityNode = new TreeNode("Entity");
            entityNode.Tag = tables;
            TreeView_Tables.Nodes.Add(entityNode);

            if (tables.Tables != null)
            {
                for (int i = 0; i < tables.Tables.Length; i++)
                {
                    TreeNode node = new TreeNode("Table_" + i);
                    node.Tag = tables.Tables[i];
                    entityNode.Nodes.Add(node);
                }
            }
        }

        private void OnNodeSelectSelect(object sender, TreeViewEventArgs e)
        {
            PropertyGrid_Item.SelectedObject = e.Node.Tag;
        }

        private void Button_Save_OnClick(object sender, EventArgs e)
        {
            TreeNode TableRoot = TreeView_Tables.Nodes[0];

            // Update tables from treeview
            tables.Tables = new IActorExtraDataInterface[TableRoot.Nodes.Count];
            for(int i = 0; i < tables.Tables.Length; i++)
            {
                tables.Tables[i] = (IActorExtraDataInterface)TableRoot.Nodes[i].Tag;
            }

            // Write to file
            File.Copy(edsFile.FullName, edsFile.FullName + "_old", true);
            tables.WriteToFile(edsFile.FullName, false);
        }

        private void Button_Reload_OnClick(object sender, EventArgs e)
        {
            TreeView_Tables.Nodes.Clear();
            LoadAndBuildData();
        }

        private void Button_Exit_OnClick(object sender, EventArgs e)
        {
            Close();
        }

        private void Form_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (TreeView_Tables.Focused)
            {
                if (e.Control && e.KeyCode == Keys.C)
                {
                    CopyTagData();
                }
                else if (e.Control && e.KeyCode == Keys.V)
                {
                    PasteTagData();
                }
            }
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
            }
        }

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
                }
            }
        }
    }
}