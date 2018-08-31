using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Gibbed.Mafia2.FileFormats;
using Gibbed.Mafia2.FileFormats.Archive;
using Mafia2;

namespace Mafia2Tool
{
    public partial class GameExplorer : Form
    {
        private DirectoryInfo currentDirectory;
        private DirectoryInfo originalPath;

        public GameExplorer()
        {
            InitializeComponent();
            infoText.Text = "Loading..";
            BuildTreeView();
            infoText.Text = "Ready..";
        }

        /// <summary>
        /// Build TreeView from Mafia II's main directory.
        /// </summary>
        public void BuildTreeView()
        {
            TreeNode rootTreeNode;


            if(string.IsNullOrEmpty(ToolkitSettings.M2Directory))
                GetPath();

            originalPath = new DirectoryInfo(ToolkitSettings.M2Directory);

            //check if directory exists.
            if (!originalPath.Exists)
            {
                MessageBox.Show("Could not find MafiaII 'launcher.exe', please correct the path!", "Error!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            //check if launcher.exe exists.
            bool hasLauncher = false;
            foreach (FileInfo file in originalPath.GetFiles())
            {
                if (file.Name == "launcher.exe" || file.Name == "launcher")
                    hasLauncher = true;
            }

            if (!hasLauncher)
            {
                MessageBox.Show("Could not find MafiaII 'launcher.exe', please correct the path!", "Error!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            infoText.Text = "Building folders..";
            //build treeView.
            rootTreeNode = new TreeNode(originalPath.Name);
            rootTreeNode.Tag = originalPath;
            GetSubFolders(originalPath.GetDirectories(), rootTreeNode);
            folderView.Nodes.Add(rootTreeNode);
            infoText.Text = "Done builidng folders..";
            OpenDirectory(originalPath);
        }

        /// <summary>
        /// If the program has errored it will run this.. It's to get a new path.
        /// </summary>
        private void GetPath()
        {
            MafiaIIBrowser.SelectedPath = "";
            if (MafiaIIBrowser.ShowDialog() == DialogResult.OK)
            {
                ToolkitSettings.M2Directory = MafiaIIBrowser.SelectedPath;
                ToolkitSettings.WriteKey("MafiaII", "Directories", MafiaIIBrowser.SelectedPath);
            }
            else
                return;
        }

        /// <summary>
        /// Build tree by adding sub folders to treeView1.
        /// </summary>
        /// <param name="directories">sub directories of root.</param>
        /// <param name="rootTreeNode">Node to apply the children</param>
        public void GetSubFolders(DirectoryInfo[] directories, TreeNode rootTreeNode)
        {
            TreeNode node;
            DirectoryInfo[] dirs;

            foreach (DirectoryInfo directory in directories)
            {
                node = new TreeNode(directory.Name);
                node.Tag = directory;
                node.ImageIndex = 0;
                dirs = directory.GetDirectories();
                if (dirs.Length != 0)
                    GetSubFolders(dirs, node);

                rootTreeNode.Nodes.Add(node);
            }
        }

        /// <summary>
        /// Clears listView1 and displays the current directory.
        /// </summary>
        /// <param name="directory">directory to show</param>
        private void OpenDirectory(DirectoryInfo directory)
        {
            infoText.Text = "Loading Directory..";
            fileListView.Items.Clear();
            ListViewItem.ListViewSubItem[] subItems;
            ListViewItem item = null;

            foreach (DirectoryInfo dir in directory.GetDirectories())
            {
                item = new ListViewItem(dir.Name, 0);
                item.Tag = dir;
                subItems = new ListViewItem.ListViewSubItem[]
                {
                    new ListViewItem.ListViewSubItem(item, "Directory"),
                    new ListViewItem.ListViewSubItem(item, ""),
                    new ListViewItem.ListViewSubItem(item,
                        dir.LastAccessTime.ToShortDateString())
                };
                item.SubItems.AddRange(subItems);
                fileListView.Items.Add(item);
            }

            foreach (FileInfo file in directory.GetFiles())
            {
                item = new ListViewItem(file.Name, DetermineFileIcon(file.Extension));
                item.Tag = file;

                subItems = new ListViewItem.ListViewSubItem[]
                {
                    new ListViewItem.ListViewSubItem(item, DetermineFileType(file.Extension)),
                    new ListViewItem.ListViewSubItem(item, file.Length.ToString()),
                    new ListViewItem.ListViewSubItem(item,
                        file.LastAccessTime.ToShortDateString())
                };

                item.SubItems.AddRange(subItems);
                fileListView.Items.Add(item);
            }
            fileListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            infoText.Text = "Done loading directory.";
            textStripFolderPath.Text = directory.FullName;
            currentDirectory = directory;
        }

        /// <summary>
        /// Pack an SDS from the FileInfo given.
        /// </summary>
        /// <param name="file">location of SDS.</param>
        private void PackSDS(FileInfo file)
        {

            if (file == null)
                MessageBox.Show("File is null");

            if (file.Name == "ingame.sds" || file.Name == "tables.sds")
            {
                MessageBox.Show("Packing " + file.Name + " is temporarily disabled due to game crashing.", "Toolkit",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            //backup file before repacking..
            if (!Directory.Exists(file.Directory.FullName + "/BackupSDS"))
                Directory.CreateDirectory(file.Directory.FullName + "/BackupSDS");

            //place copy in new folder.
            File.Copy(file.FullName, file.Directory.FullName + "/BackupSDS/"+file.Name, true);

            //begin..
            infoText.Text = "Saving SDS..";
            ArchiveFile archiveFile = new ArchiveFile
            {
                Platform = Platform.PC,
                Unknown20 = new byte[16] {55, 51, 57, 55, 57, 43, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
            };
            archiveFile.BuildResources(file.Directory.FullName + "/extracted/" + file.Name);

            foreach (ResourceEntry entry in archiveFile.ResourceEntries)
            {
                if(entry.Data == null)
                    throw new FormatException();
            }

            using (var output = File.Create(file.FullName))
            {
                archiveFile.Serialize(output, ArchiveSerializeOptions.Compress);
            }
        }

        /// <summary>
        /// Open an SDS from the FileInfo given.
        /// </summary>
        /// <param name="file">info of SDS.</param>
        private void OpenSDS(FileInfo file)
        {
            Log.WriteLine("Opening SDS: " + file.Name);

            infoText.Text = "Opening SDS..";
            fileListView.Items.Clear();
            ArchiveFile archiveFile;
            using (var input = File.OpenRead(file.FullName))
            {
                using (Stream data = ArchiveEncryption.Unwrap(input))
                {
                    archiveFile = new ArchiveFile();
                    archiveFile.Deserialize(data ?? input);
                }
            }

            Log.WriteLine("Succesfully unwrapped compressed data");

            archiveFile.SaveResources(file);

            OpenDirectory(new DirectoryInfo(file.Directory.FullName + "/extracted/" + file.Name));
            infoText.Text = "Opened SDS..";
        }

        /// <summary>
        /// Open a PATCH file from the FileInfo given.
        /// </summary>
        /// <param name="file"></param>
        private void OpenPATCH(FileInfo file)
        {
            Log.WriteLine("Opening PATCH: " + file.Name);

            infoText.Text = "Opening PATCH..";

            PatchFile patch;
            using (BinaryReader reader = new BinaryReader(File.Open(file.FullName, FileMode.Open)))
            {
                patch = new PatchFile();
                patch.Deserialize(reader, Gibbed.IO.Endian.Little);
            }
        }

        /// <summary>
        /// Check extension and return file type string.
        /// </summary>
        /// <param name="extension">extension of file.</param>
        private string DetermineFileType(string extension)
        {
            //TODO. Sort extensions with localisations.
            switch (extension)
            {
                case ".sds":
                    return "SDS Archive";
                case ".patch":
                    return "PATCH Archive";
                case ".dds":
                    return "Direct-Draw Surface";
                case ".exe":
                    return "Executable";
                case ".dll":
                    return "Dynamic-Link Library";
                case ".mtl":
                    return "Material Library";
                case "": //fix for content files.
                    return "File";
                default:
                    return extension.Remove(0, 1).ToUpper();
            }
        }

        /// <summary>
        /// Use the file extension to determine to file icon.
        /// </summary>
        /// <param name="extension">file extension</param>
        /// <returns></returns>
        private int DetermineFileIcon(string extension)
        {
            switch (extension)
            {
                case ".exe":
                    return 3;
                case ".dll":
                    return 2;
                default:
                    return 1;
            }
        }

        private void toolStrip1_Resize(object sender, EventArgs e)
        {
            int width = toolStrip2.DisplayRectangle.Width;

            foreach (ToolStripItem tsi in toolStrip2.Items)
            {
                if (!(tsi == textStripFolderPath))
                {
                    width -= tsi.Width;
                    width -= tsi.Margin.Horizontal;
                }
            }

            textStripFolderPath.Width = Math.Max(0, width - textStripFolderPath.Margin.Horizontal);
        }
        protected override void OnLoad(EventArgs e)
        {
            toolStrip1_Resize(this, e);
        }
        void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode selectedNode = e.Node;
            OpenDirectory((DirectoryInfo) selectedNode.Tag);
        }
        private void listView1_ItemActivate(object sender, EventArgs e)
        {
            ListViewItem item = fileListView.SelectedItems[0];
            MaterialTool mTool;
            FrameResourceTool fTool;
            CollisionEditor cTool;

            if (item.SubItems[1].Text == "Directory")
                OpenDirectory((DirectoryInfo)item.Tag);
            else if (item.SubItems[1].Text == "Material Library")
                mTool = new MaterialTool((FileInfo)item.Tag);
            else if (item.SubItems[1].Text == "SDS Archive")
                OpenSDS((FileInfo)item.Tag);
            //else if (item.SubItems[1].Text == "PATCH Archive")
            //    OpenPATCH((FileInfo)item.Tag);
            else if (item.SubItems[1].Text == "FR")
                fTool = new FrameResourceTool((FileInfo)item.Tag);
            else if (item.SubItems[1].Text == "COL")
                cTool = new CollisionEditor((FileInfo)item.Tag);
        }
        private void ContextSDSPack_Click(object sender, EventArgs e)
        {
            if (fileListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select an item.", "Toolkit", MessageBoxButtons.OK);
                return;
            }

            PackSDS(fileListView.SelectedItems[0].Tag as FileInfo);
        }
        private void ContextSDSUnpack_Click(object sender, EventArgs e)
        {
            OpenSDS(fileListView.SelectedItems[0].Tag as FileInfo);
        }
        private void openMafiaIIToolStripMenuItem_Click(object sender, EventArgs e)
        {
           folderView.Nodes.Clear();
           BuildTreeView();
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void runMafiaIIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string exe = Path.Combine(ToolkitSettings.M2Directory + "launcher.exe");

            if (!File.Exists(exe))
            {
                MessageBox.Show("Launcher.exe was not found.", "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Process.Start(exe);
        }
        private void buttonStripUp_Click(object sender, EventArgs e)
        {
            if (currentDirectory.Name == originalPath.Name)
                return;
            OpenDirectory(currentDirectory.Parent);
        }
        private void buttonStripRefresh_Click(object sender, EventArgs e)
        {
            currentDirectory.Refresh();
            OpenDirectory(currentDirectory);
        }
        private void onPathChange(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == '\r')
            {
                if(Directory.Exists(textStripFolderPath.Text) && textStripFolderPath.Text.Contains(currentDirectory.Name))
                    OpenDirectory(new DirectoryInfo(textStripFolderPath.Text));
                else
                    MessageBox.Show("Game Explorer cannot find path '" + textStripFolderPath + "'. Make sure the path exists and try again.", "Game Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ContextOpenFolder_Click(object sender, EventArgs e)
        {
            Process.Start(currentDirectory.FullName);
        }
        private void OnOpening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (fileListView.SelectedItems.Count == 0)
            {
                e.Cancel = true;
                return;
            }

            SDSContext.Items[0].Visible = false;
            SDSContext.Items[1].Visible = false;

            if (fileListView.SelectedItems[0].Tag.GetType() == typeof(FileInfo))
            {
                string extension = (fileListView.SelectedItems[0].Tag as FileInfo).Extension;

                if (extension == ".sds")
                {
                    SDSContext.Items[0].Visible = true;
                    SDSContext.Items[1].Visible = true;
                }
            }
        }
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionsForm options = new OptionsForm();
            options.ShowDialog();
        }
    }
}
