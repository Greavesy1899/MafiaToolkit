using System;
using System.IO;
using System.Windows.Forms;
using Utils.Helpers;
using Utils.Types;
using Utils.Language;

namespace Mafia2Tool
{
    public partial class SDSContentEditor : Form
    {
        FileInfo OriginalInfo;
        SDSContentFile file;

        private bool bIsFileEdited = false;

        public SDSContentEditor()
        {
            InitializeComponent();
            Localise();
        }

        public SDSContentEditor(FileInfo info)
        {
            InitializeComponent();
            Localise();
            OriginalInfo = info;
            file = new SDSContentFile();
            file.ReadFromFile(OriginalInfo);
            PopulateTree();
            ShowDialog();
        }

        private void Localise()
        {
            Text = Language.GetString("$SDSCONTENT_EDITOR_TITLE");
            Button_File.Text = Language.GetString("$FILE");
            Button_Save.Text = Language.GetString("$SAVE");
            Button_Reload.Text = Language.GetString("$RELOAD");
            Button_Exit.Text = Language.GetString("$EXIT");
            Button_Delete.Text = Language.GetString("$DELETE");
            Context_Delete.Text = Language.GetString("$DELETE");
        }

        private void PopulateTree()
        {
            ResourceTreeView.Nodes.Clear();
            foreach(var resource in file.Resources)
            {
                TreeNode parent = SDSContentFile.BuildResourceTreeNode(resource.Key);
                parent.Nodes.AddRange(resource.Value.ToArray());
                ResourceTreeView.Nodes.Add(parent);
            }
        }

        private void Save()
        {
            file.WriteToFile();

            Text = Language.GetString("$SDSCONTENT_EDITOR_TITLE");
            bIsFileEdited = false;
        }

        private void Reload()
        {
            file = new SDSContentFile();
            file.ReadFromFile(OriginalInfo);
            PopulateTree();

            Text = Language.GetString("$SDSCONTENT_EDITOR_TITLE");
            bIsFileEdited = false;
        }

        private void Delete()
        {
            TreeNode SelNode = ResourceTreeView.SelectedNode;

            if (SelNode.Parent != null)
            {
                string ParentNodeName = SelNode.Parent.Text;

                file.Resources[ParentNodeName].Remove(SelNode);
                ResourceTreeView.Nodes.Remove(ResourceTreeView.SelectedNode);

                Text = Language.GetString("$SDSCONTENT_EDITOR_TITLE") + "*";
                bIsFileEdited = true;
            }
            else
            {
                file.Resources.Remove(SelNode.Text);
                ResourceTreeView.Nodes.Remove(ResourceTreeView.SelectedNode);

                Text = Language.GetString("$SDSCONTENT_EDITOR_TITLE") + "*";
                bIsFileEdited = true;
            }
        }

        private void Button_AutoAdd_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("This will only automatically add any buffer pools or textures into the SDSContent.xml. Are you sure you want to do this?", "Toolkit", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                file.CreateFileFromFolder();
                PopulateTree();

                Text = Language.GetString("$SDSCONTENT_EDITOR_TITLE") + "*";
                bIsFileEdited = true;
            }
        }

        private void Button_BatchImportTextures_Click(object sender, EventArgs e)
        {
            // Ask the user if they want to delete all existing texture/mipmap entries.
            // This gives us a clean slate for adding new entries without conflict.
            DialogResult MBResult = MessageBox.Show("Would you like to delete all Texture and Mipmap entries in the SDSContent?", "Toolkit", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if(MBResult == DialogResult.Yes)
            {
                file.WipeResourceType("Texture");
                file.WipeResourceType("Mipmap");
            }

            DialogResult Result = FileDialog_Generic.ShowDialog();
            if(Result != DialogResult.OK || string.IsNullOrEmpty(FileDialog_Generic.FileName))
            {
                // Fail
                return;
            }

            if(!File.Exists(FileDialog_Generic.FileName))
            {
                // Fail, file doesn't exist
                return;
            }

            Result = FolderBrowser_Generic.ShowDialog();
            if(Result != DialogResult.OK || string.IsNullOrEmpty(FolderBrowser_Generic.SelectedPath))
            {
                // Fail;
                return;
            }

            // Cache folder and file name
            string TextureFolder = FolderBrowser_Generic.SelectedPath;
            string FileName = FileDialog_Generic.FileName;
            string SDSPath = file.GetParentFolder();
            string[] AllTextures = File.ReadAllLines(FileName);

            foreach(string TextureEntry in AllTextures)
            {
                // Check if the texture exists in the provided path
                string ConnectedPath = TextureFolder + "//" + TextureEntry;
                if (File.Exists(ConnectedPath))
                {
                    // Copy over the texture
                    string NewSDSPath = SDSPath + "//" + TextureEntry;
                    File.Copy(ConnectedPath, NewSDSPath, true);

                    // CreateTextureResource() checks if MIP exists.
                    file.CreateTextureResource(TextureEntry);
                }
            }

            // Update tree
            PopulateTree();

            Text = Language.GetString("$SDSCONTENT_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

        private void SDSContentEditor_Closing(object sender, FormClosingEventArgs e)
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

        private void Button_Save_Click(object sender, EventArgs e) => Save();
        private void Button_Reload_Click(object sender, EventArgs e) => Reload();
        private void Button_Exit_Click(object sender, EventArgs e) => Close();
        private void Button_Delete_Click(object sender, EventArgs e) => Delete();
        private void Context_Delete_Click(object sender, EventArgs e) => Delete();
    }
}