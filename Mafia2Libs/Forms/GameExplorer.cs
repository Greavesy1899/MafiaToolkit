using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Gibbed.Mafia2.FileFormats;
using Gibbed.Mafia2.FileFormats.Archive;
using Mafia2;
using ApexSDK;
using System.Drawing;

namespace Mafia2Tool
{
    public partial class GameExplorer : Form
    {
        private DirectoryInfo currentDirectory;
        private DirectoryInfo originalPath;

        public GameExplorer()
        {
            InitializeComponent();
        }

        public void LoadForm()
        {
            toolStrip1_Resize(this, null);
            Localise();
            infoText.Text = "Loading..";
            BuildTreeView();
            infoText.Text = "Ready..";
        }

        private bool Localise()
        {
            Text = Language.GetString("$MII_TK_GAME_EXPLORER");
            buttonStripUp.ToolTipText = Language.GetString("$UP_TOOLTIP");
            textStripFolderPath.ToolTipText = Language.GetString("$FOLDER_PATH_TOOLTIP");
            buttonStripRefresh.Text = Language.GetString("$REFRESH");
            textStripSearch.ToolTipText = Language.GetString("$SEARCH_TOOLTIP");
            columnName.Text = Language.GetString("$NAME");
            columnType.Text = Language.GetString("$TYPE");
            columnSize.Text = Language.GetString("$SIZE");
            columnLastModified.Text = Language.GetString("$LAST_MODIFIED");
            SDSContext.Text = Language.GetString("$VIEW");
            ContextSDSUnpack.Text = Language.GetString("$UNPACK");
            ContextSDSPack.Text = Language.GetString("$PACK");
            ContextOpenFolder.Text = Language.GetString("$OPEN_FOLDER_EXPLORER");
            ContextSDSUnpackAll.Text = Language.GetString("$UNPACK_ALL_SDS");
            ContextView.Text = Language.GetString("$VIEW");
            ContextViewIcon.Text = Language.GetString("$ICON");
            ContextViewDetails.Text = Language.GetString("$DETAILS");
            ContextViewSmallIcon.Text = Language.GetString("$SMALL_ICON");
            ContextViewList.Text = Language.GetString("$LIST");
            ContextViewTile.Text = Language.GetString("$TILE");
            dropdownFile.Text = Language.GetString("$FILE");
            openMafiaIIToolStripMenuItem.Text = Language.GetString("$BTN_OPEN_MII");
            runMafiaIIToolStripMenuItem.Text = Language.GetString("$BTN_RUN_MII");
            exitToolStripMenuItem.Text = Language.GetString("$EXIT");
            dropdownView.Text = Language.GetString("$VIEW");
            dropdownTools.Text = Language.GetString("$TOOLS");
            optionsToolStripMenuItem.Text = Language.GetString("$OPTIONS");
            MafiaIIBrowser.Description = Language.GetString("$SELECT_MII_FOLDER");
            return true;
        }

        /// <summary>
        /// Build TreeView from Mafia II's main directory.
        /// </summary>
        public void BuildTreeView()
        {
            TreeNode rootTreeNode;


            if (string.IsNullOrEmpty(ToolkitSettings.M2Directory))
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
                Log.WriteLine("Could not find MafiaII 'launcher.exe', please correct the path!", LoggingTypes.ERROR);
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
        private void OpenDirectory(DirectoryInfo directory, bool searchMode = false, string filename = null)
        {
            infoText.Text = "Loading Directory..";
            fileListView.Items.Clear();
            ListViewItem.ListViewSubItem[] subItems;
            ListViewItem item = null;

            foreach (DirectoryInfo dir in directory.GetDirectories())
            {
                if (searchMode && !string.IsNullOrEmpty(filename))
                {
                    if (!dir.Name.Contains(filename))
                        continue;
                }
                item = new ListViewItem(dir.Name, imageBank.Images.IndexOfKey("folderIcon"));
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
                if (!imageBank.Images.ContainsKey(file.Extension))
                    imageBank.Images.Add(file.Extension, Icon.ExtractAssociatedIcon(file.FullName));

                if (searchMode && !string.IsNullOrEmpty(filename))
                {
                    if (!file.Name.Contains(filename))
                        continue;
                }

                item = new ListViewItem(file.Name, imageBank.Images.IndexOfKey(file.Extension));
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

            //backup file before repacking..
            if (!Directory.Exists(file.Directory.FullName + "/BackupSDS"))
                Directory.CreateDirectory(file.Directory.FullName + "/BackupSDS");

            //place copy in new folder.
            File.Copy(file.FullName, file.Directory.FullName + "/BackupSDS/" + file.Name, true);

            //begin..
            infoText.Text = "Saving SDS..";
            ArchiveFile archiveFile = new ArchiveFile
            {
                Platform = Platform.PC,
                Unknown20 = new byte[16] { 55, 51, 57, 55, 57, 43, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
            };
            archiveFile.BuildResources(file.Directory.FullName + "/extracted/" + file.Name);

            foreach (ResourceEntry entry in archiveFile.ResourceEntries)
            {
                if (entry.Data == null)
                    throw new FormatException();
            }

            using (var output = File.Create(file.FullName))
            {
                archiveFile.Serialize(output, ArchiveSerializeOptions.Compress);
            }
            infoText.Text = "Saved SDS.";
        }

        /// <summary>
        /// Open an SDS from the FileInfo given.
        /// </summary>
        /// <param name="file">info of SDS.</param>
        private void OpenSDS(FileInfo file)
        {
            Log.WriteLine("Opening SDS: " + file.Name);
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
                patch.file = file;
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
                case ".spe":
                    return "Speech Data";
                case ".exe":
                    return "Executable";
                case ".dll":
                    return "Dynamic-Link Library";
                case ".mtl":
                    return "Material Library";
                case ".tbl":
                    return "Table";
                case "": //fix for content files.
                    return "File";
                default:
                    return extension.Remove(0, 1).ToUpper();
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
            LoadForm();
        }
        void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode selectedNode = e.Node;
            OpenDirectory((DirectoryInfo)selectedNode.Tag);
        }
        private void listView1_ItemActivate(object sender, EventArgs e)
        {
            HandleFile(fileListView.SelectedItems[0]);
        }

        private void HandleFile(ListViewItem item)
        {
            MaterialTool mTool;
            FrameResourceTool fTool;
            CollisionEditor cTool;
            //TODO: Build editor for speech.
            Speech speech;
            CutsceneFile cutscene;
            IOFxFile iofx;
            EmitterFile emitterFile;
            TableEditor tTool;

            switch (item.SubItems[1].Text)
            {
                case "Directory":
                    OpenDirectory((DirectoryInfo)item.Tag);
                    return;
                case "Material Library":
                    mTool = new MaterialTool((FileInfo)item.Tag);
                    return;
                case "Speech Data":
                    speech = new Speech((FileInfo)item.Tag);
                    return;
                case "CUT":
                    //cutscene = new CutsceneFile((FileInfo)item.Tag);
                    return;
                case "SDS Archive":
                    OpenSDS((FileInfo)item.Tag);
                    break;
                case "PATCH Archive":
                    OpenPATCH((FileInfo)item.Tag);
                    break;
                case "FR":
                    fTool = new FrameResourceTool((FileInfo)item.Tag);
                    return;
                case "COL":
                    cTool = new CollisionEditor((FileInfo)item.Tag);
                    return;
                case "IOFX":
                    iofx = new IOFxFile((FileInfo)item.Tag);
                    return;
                case "AEA":
                    emitterFile = new EmitterFile((FileInfo)item.Tag);
                    return;
                case "Table":
                    tTool = new TableEditor((FileInfo)item.Tag);
                    return;
            }
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
            foreach (ListViewItem item in fileListView.SelectedItems)
            {
                if (item.SubItems[1].Text == "SDS Archive")
                    HandleFile(item);
            }
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
            if (e.KeyChar == '\r')
            {
                if (Directory.Exists(textStripFolderPath.Text) && textStripFolderPath.Text.Contains(currentDirectory.Name))
                    OpenDirectory(new DirectoryInfo(textStripFolderPath.Text));
                else
                    MessageBox.Show("Game Explorer cannot find path '" + textStripFolderPath + "'. Make sure the path exists and try again.", "Game Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ContextOpenFolder_Click(object sender, EventArgs e)
        {
            Process.Start(currentDirectory.FullName);
        }
        private void OnOpening(object sender, CancelEventArgs e)
        {
            SDSContext.Items[0].Visible = false;
            SDSContext.Items[1].Visible = false;

            if (fileListView.SelectedItems.Count == 0)
                return;

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
            Controls.Clear();
            InitializeComponent();
            LoadForm();
        }

        private void SearchBarOnTextChanged(object sender, EventArgs e)
        {
            OpenDirectory(currentDirectory, true, textStripSearch.Text);
        }

        private void ContextSDSUnpackAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in fileListView.Items)
            {
                if (item.SubItems[1].Text == "SDS Archive")
                    HandleFile(item);
            }
        }

        private void ContextViewBtn_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            ContextViewIcon.Checked = false;
            ContextViewDetails.Checked = false;
            ContextViewSmallIcon.Checked = false;
            ContextViewList.Checked = false;
            ContextViewTile.Checked = false;

            switch (item.Name)
            {
                case "ContextViewIcon":
                    ContextViewIcon.Checked = true;
                    fileListView.View = View.LargeIcon;
                    break;
                case "ContextViewDetails":
                    ContextViewDetails.Checked = true;
                    fileListView.View = View.Details;
                    break;
                case "ContextViewSmallIcon":
                    ContextViewSmallIcon.Checked = true;
                    fileListView.View = View.SmallIcon;
                    break;
                case "ContextViewList":
                    ContextViewList.Checked = true;
                    fileListView.View = View.List;
                    break;
                case "ContextViewTile":
                    ContextViewTile.Checked = true;
                    fileListView.View = View.Tile;
                    break;
            }
        }
    }
}
