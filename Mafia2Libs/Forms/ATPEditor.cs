using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Xml.Linq;
using ResourceTypes.Navigation;
using Utils.Helpers.Reflection;
using Utils.Language;
using Utils.Settings;
using MessageBox = System.Windows.MessageBox;

namespace Toolkit.Forms
{
    public partial class ATPEditor : Form
    {
        private FileInfo ATPFile;
        private AnimalTrafficLoader ATPLoader;

        private TreeNode AnimalTypesRootNode;
        private TreeNode AnimalPathsRootNode;

        private object OurClipboard;

        private bool bIsFileEdited = false;

        public ATPEditor(FileInfo file)
        {
            InitializeComponent();
            Localise();
            ATPFile = file;
            LoadAndBuildData();
            Show();
            ToolkitSettings.UpdateRichPresence("Using the ATP editor.");
        }

        private void Localise()
        {
            Text = Language.GetString("$ATP_EDITOR_TITLE");
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
            ToolStrip_Add.Text = Language.GetString("$ADD_ATP");
            ToolStrip_Delete.Text = Language.GetString("$DELETE_ATP");
            FileDialog_Open.Title = Language.GetString("$OPEN_TITLE");
            FileDialog_Save.Title = Language.GetString("$SAVE_TITLE");
        }

        private void LoadAndBuildData()
        {
            ATPLoader = new AnimalTrafficLoader(ATPFile);

            CreateTable();
        }

        private void CreateTable()
        {
            // Add Animal Types
            AnimalTypesRootNode = new TreeNode("Animal Types");
            foreach(AnimalTrafficLoader.AnimalTrafficType ATPType in ATPLoader.AnimalTypes)
            {
                TreeNode AnimalType = new TreeNode();
                AnimalType.Text = ATPType.Name.String;
                AnimalType.Name = ATPType.Name.Hash.ToString();
                AnimalType.Tag = ATPType;

                AnimalTypesRootNode.Nodes.Add(AnimalType);
            }

            // Add Animal Paths
            AnimalPathsRootNode = new TreeNode("Animal Path");
            foreach(AnimalTrafficLoader.AnimalTrafficPath ATPPath in ATPLoader.Paths)
            {
                TreeNode AnimalPath = new TreeNode();
                AnimalPath.Name = String.Format("ATPATH_{0}", AnimalPathsRootNode.Nodes.Count);
                AnimalPath.Text = String.Format("Path: [{1}] || Animal: [{0}]", ATPLoader.AnimalTypes[ATPPath.AnimalTypeIdx].Name.ToString(), AnimalPathsRootNode.Nodes.Count);
                AnimalPath.Tag = ATPPath;

                AnimalPathsRootNode.Nodes.Add(AnimalPath);
            }

            // Add the root nodes
            TreeView_Tables.Nodes.Add(AnimalTypesRootNode);
            TreeView_Tables.Nodes.Add(AnimalPathsRootNode);
        }

        private void FileIsEdited()
        {
            Text = Language.GetString("$ATP_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

        private void FileIsNotEdited()
        {
            Text = Language.GetString("$ATP_EDITOR_TITLE");
            bIsFileEdited = false;
        }

        private void Save()
        {
            if(ATPLoader == null)
            {   
                // No ATPLoader?
                return;
            }

            // Setup new arrays
            AnimalTrafficLoader.AnimalTrafficType[] NewTypeArray = new AnimalTrafficLoader.AnimalTrafficType[AnimalTypesRootNode.Nodes.Count];
            AnimalTrafficLoader.AnimalTrafficPath[] NewPathsArray = new AnimalTrafficLoader.AnimalTrafficPath[AnimalPathsRootNode.Nodes.Count];

            for(int i = 0; i < NewTypeArray.Length; i++)
            {
                NewTypeArray[i] = (AnimalTypesRootNode.Nodes[i].Tag as AnimalTrafficLoader.AnimalTrafficType);
            }

            for (int i = 0; i < NewPathsArray.Length; i++)
            {
                NewPathsArray[i] = (AnimalPathsRootNode.Nodes[i].Tag as AnimalTrafficLoader.AnimalTrafficPath);
            }

            // Store new arrays
            ATPLoader.AnimalTypes = NewTypeArray;
            ATPLoader.Paths = NewPathsArray;

            // Write the file
            ATPLoader.WriteToFile();

            // Reset editor state
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
                if (IsTypeofInterface(SelectedNode.Tag, typeof(AnimalTrafficLoader.AnimalTrafficPath)))
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
                if (IsTypeofInterface(SelectedNode.Tag, typeof(AnimalTrafficLoader.AnimalTrafficPath)))
                {
                    object ObjectToCopy = OurClipboard;
                    object NewObject = Activator.CreateInstance(ObjectToCopy.GetType());
                    ReflectionHelpers.Copy(ObjectToCopy, ref NewObject);
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

        private void ATPEditor_OnKeyUp(object sender, KeyEventArgs e)
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
        private void ContextMenu_Opening(object sender, CancelEventArgs e)
        {
            TreeNode SelectedNode = TreeView_Tables.SelectedNode;
            if (SelectedNode != null && SelectedNode.Tag != null)
            {
                if (IsTypeofInterface(SelectedNode.Tag, typeof(AnimalTrafficLoader.AnimalTrafficPath))
                    || IsTypeofInterface(SelectedNode.Tag, typeof(AnimalTrafficLoader.AnimalTrafficType)))
                {
                    ToolStrip_Copy.Enabled = true;
                    ToolStrip_Paste.Enabled = true;
                    ToolStrip_Delete.Enabled = true;
                    ToolStrip_Add.Enabled = false;
                    return;
                }
            }
            else if(SelectedNode == AnimalTypesRootNode || SelectedNode == AnimalPathsRootNode)
            {
                ToolStrip_Copy.Enabled = false;
                ToolStrip_Paste.Enabled = false;
                ToolStrip_Delete.Enabled = false;
                ToolStrip_Add.Enabled = true;
                return;
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
                XElement Root = ReflectionHelpers.ConvertPropertyToXML(ATPLoader);
                Root.Save(FileDialog_Save.FileName);
            }
        }

        private void Button_ImportXML_Click(object sender, EventArgs e)
        {
            if(FileDialog_Open.ShowDialog() == DialogResult.OK)
            {
                string FileToOpen = FileDialog_Open.FileName;
                if(File.Exists(FileToOpen))
                {
                   // tables.ConvertFromXML(FileToOpen);

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

            if(PropertyGrid_Item.SelectedObject is AnimalTrafficLoader.AnimalTrafficPath)
            {
                TreeNode ActiveNode = TreeView_Tables.SelectedNode;
                AnimalTrafficLoader.AnimalTrafficPath ATPPath = (ActiveNode.Tag as AnimalTrafficLoader.AnimalTrafficPath);
                ActiveNode.Text = String.Format("Path: [{1}] || Animal: [{0}]", ATPLoader.AnimalTypes[ATPPath.AnimalTypeIdx].Name.ToString(), AnimalPathsRootNode.Nodes.Count);
            }
            else if(PropertyGrid_Item.SelectedObject is AnimalTrafficLoader.AnimalTrafficType)
            {
                TreeNode ActiveNode = TreeView_Tables.SelectedNode;
                AnimalTrafficLoader.AnimalTrafficType ATPType = (ActiveNode.Tag as AnimalTrafficLoader.AnimalTrafficType);
                ActiveNode.Text = ATPType.Name.String;
            }
        }

        private void ATPEditor_Closing(object sender, FormClosingEventArgs e)
        {
            if (bIsFileEdited)
            {
                MessageBoxResult SaveChanges = MessageBox.Show(Language.GetString("$SAVE_PROMPT"), "Toolkit", MessageBoxButton.YesNoCancel);

                if (SaveChanges == MessageBoxResult.Yes)
                {
                    Save();
                }
                else if (SaveChanges == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void ATPTreeView_OnAdd(object sender, EventArgs e)
        {
            TreeNode SelectedNode = TreeView_Tables.SelectedNode;
            if(SelectedNode == AnimalTypesRootNode)
            {
                AnimalTrafficLoader.AnimalTrafficType AnimalType = new AnimalTrafficLoader.AnimalTrafficType();
                AnimalType.Name.Set("NewAnimalType");

                TreeNode AnimalTypeNode = new TreeNode();
                AnimalTypeNode.Text = AnimalType.Name.ToString();
                AnimalTypeNode.Name = AnimalType.Name.Hash.ToString();
                AnimalTypeNode.Tag = AnimalType;

                AnimalTypesRootNode.Nodes.Add(AnimalTypeNode);
                TreeView_Tables.SelectedNode = AnimalTypeNode;
            }
            else if(SelectedNode == AnimalPathsRootNode)
            {
                AnimalTrafficLoader.AnimalTrafficPath ATPPath = new AnimalTrafficLoader.AnimalTrafficPath();

                TreeNode AnimalPath = new TreeNode();
                AnimalPath.Name = String.Format("ATPATH_{0}", AnimalPathsRootNode.Nodes.Count);
                AnimalPath.Text = AnimalPath.Name = String.Format("Path: [{1}] || Animal: [{0}]", ATPLoader.AnimalTypes[ATPPath.AnimalTypeIdx].Name.ToString(), AnimalPathsRootNode.Nodes.Count);
                AnimalPath.Tag = ATPPath;

                AnimalPathsRootNode.Nodes.Add(AnimalPath);
                TreeView_Tables.SelectedNode = AnimalPath;
            }
        }

        private void ATPTreeView_OnDelete(object sender, EventArgs e)
        {
            bool bRemovedNode = false;

            TreeNode SelectedNode = TreeView_Tables.SelectedNode;
            if (SelectedNode.Tag is AnimalTrafficLoader.AnimalTrafficType)
            {
                if(AnimalTypesRootNode.Nodes.Contains(SelectedNode))
                {
                    AnimalTypesRootNode.Nodes.Remove(SelectedNode);
                    bRemovedNode = true;
                }
            }
            else if(SelectedNode.Tag is AnimalTrafficLoader.AnimalTrafficPath)
            {
                if (AnimalPathsRootNode.Nodes.Contains(SelectedNode))
                {
                    AnimalPathsRootNode.Nodes.Remove(SelectedNode);
                    bRemovedNode = true;
                }
            }

            if(bRemovedNode)
            {
                PropertyGrid_Item.SelectedObject = null;
            }
        }
    }
}
