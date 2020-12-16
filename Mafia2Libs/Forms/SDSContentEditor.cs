using System;
using System.IO;
using System.Windows.Forms;
using Utils.Types;

namespace Mafia2Tool
{
    public partial class SDSContentEditor : Form
    {
        SDSContentFile file;
        public SDSContentEditor()
        {
            InitializeComponent();
            Localise();
        }

        public SDSContentEditor(FileInfo info)
        {
            InitializeComponent();
            Localise();
            file = new SDSContentFile();
            file.ReadFromFile(info);
            PopulateTree();
            ShowDialog();
        }

        private void Localise()
        {


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

        private void AutoAddFilesButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("This will only automatically add any buffer pools or textures into the SDSContent.xml. Are you sure you want to do this?", "Toolkit", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                file.CreateFileFromFolder();
                PopulateTree();
            }
        }

        private void SaveButtonOnClick(object sender, EventArgs e)
        {
            file.WriteToFile();
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
        }
    }
}