using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Gibbed.Mafia2.FileFormats;
using Gibbed.Mafia2.FileFormats.Archive;
using Utils.Extensions;
using ApexSDK;
using System.Drawing;
using Utils.Lang;
using Utils.Logging;
using Utils.Settings;
using ResourceTypes.Cutscene;
using ResourceTypes.Navigation;
using ResourceTypes.Prefab;
using ResourceTypes.Sound;
using ResourceTypes.SDSConfig;
using Utils.Lua;

namespace Mafia2Tool
{
    public partial class GameExplorer : Form
    {
        private DirectoryInfo currentDirectory;
        private DirectoryInfo originalPath;
        private FileInfo launcher;

        public GameExplorer()
        {
            InitializeComponent();
        }

        public void PreloadData()
        {
            SplashForm splash = new SplashForm();
            splash.Show();
            splash.Refresh();
        }

        public void LoadForm()
        {
            toolStrip1_Resize(this, null);
            Localise();            
            infoText.Text = "Loading..";
            InitExplorerSettings();
            FileListViewTypeController(1);
            infoText.Text = "Ready..";
        }

        private bool Localise()
        {
            creditsToolStripMenuItem.Text = Language.GetString("$CREDITS");
            Text = Language.GetString("$MII_TK_GAME_EXPLORER");
            UpButton.ToolTipText = Language.GetString("$UP_TOOLTIP");
            FolderPath.ToolTipText = Language.GetString("$FOLDER_PATH_TOOLTIP");
            buttonStripRefresh.Text = Language.GetString("$REFRESH");
            SearchEntryText.ToolTipText = Language.GetString("$SEARCH_TOOLTIP");
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
            ViewStripMenuIcon.Text = Language.GetString("$ICON");
            ViewStripMenuDetails.Text = Language.GetString("$DETAILS");
            ViewStripMenuSmallIcon.Text = Language.GetString("$SMALL_ICON");
            ViewStripMenuList.Text = Language.GetString("$LIST");
            ViewStripMenuTile.Text = Language.GetString("$TILE");
            dropdownFile.Text = Language.GetString("$FILE");
            openMafiaIIToolStripMenuItem.Text = Language.GetString("$BTN_OPEN_MII");
            runMafiaIIToolStripMenuItem.Text = Language.GetString("$BTN_RUN_MII");
            exitToolStripMenuItem.Text = Language.GetString("$EXIT");
            dropdownView.Text = Language.GetString("$VIEW");
            dropdownTools.Text = Language.GetString("$TOOLS");
            OptionsItem.Text = Language.GetString("$OPTIONS");
            MafiaIIBrowser.Description = Language.GetString("$SELECT_MII_FOLDER");
            VersionLabel.Text += ToolkitSettings.Version;
            return true;
        }

        private void PrintErrorLauncher()
        {
            MessageBox.Show(Language.GetString("$ERROR_DID_NOT_FIND_LAUNCHER"), Language.GetString("$ERROR_TITLE"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            Log.WriteLine("$ERROR_DID_NOT_FIND_LAUNCHER", LoggingTypes.ERROR);
        }

        /// <summary>
        /// Build TreeView from Mafia II's main directory.
        /// </summary>
        public void InitExplorerSettings()
        {
            if (string.IsNullOrEmpty(ToolkitSettings.M2Directory))
                GetPath();

            originalPath = new DirectoryInfo(ToolkitSettings.M2Directory);

            //check if directory exists.
            if (!originalPath.Exists)
            {
                PrintErrorLauncher();
                GetPath();
                return;
            }

            bool hasLauncher = false;
            foreach (FileInfo file in originalPath.GetFiles())
            {
                //check for either steam or gog version.
                if ((file.Name.ToLower() == "launcher") || (file.Name.ToLower() == "launcher.exe") || (file.Name.ToLower() == "launch mafia ii") || (file.Name.ToLower() == "launch mafia ii.lnk"))
                {
                    hasLauncher = true;
                    launcher = file;
                }

                //stop early if needed
                if (hasLauncher)
                    break;
            }

            if (!hasLauncher)
            {
                PrintErrorLauncher();
                GetPath();
                return;
            }
            InitTreeView();
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
                InitTreeView();
            }
            else
                return;
        }

        private void InitTreeView()
        {
            infoText.Text = "Building folders..";
            //build treeView.
            TreeNode rootTreeNode = new TreeNode(originalPath.Name);
            rootTreeNode.Tag = originalPath;
            GetSubFolders(originalPath.GetDirectories(), rootTreeNode);
            folderView.Nodes.Add(rootTreeNode);
            infoText.Text = "Done builidng folders..";
            OpenDirectory(originalPath);
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
                item = new ListViewItem(dir.Name, 0);
                item.Tag = dir;
                subItems = new ListViewItem.ListViewSubItem[]
                {
                    new ListViewItem.ListViewSubItem(item, "Directory"),
                    new ListViewItem.ListViewSubItem(item, (dir.GetDirectories().Length + dir.GetFiles().Length).ToString() + " items"),
                    new ListViewItem.ListViewSubItem(item, dir.LastWriteTime.ToShortDateString()),
                };
                item.SubItems.AddRange(subItems);
                fileListView.Items.Add(item);
            }

            foreach (FileInfo file in directory.GetFiles())
            {
                if (!imageBank.Images.ContainsKey(file.Extension))
                    imageBank.Images.Add(file.Extension, Icon.ExtractAssociatedIcon(file.FullName));

                //Icon.ExtractAssociatedIcon(file.FullName).ToBitmap().Save(file.Name + ".bmp");

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
                    new ListViewItem.ListViewSubItem(item, file.CalculateFileSize()),
                    new ListViewItem.ListViewSubItem(item, file.LastWriteTime.ToShortDateString()),
                };

                item.SubItems.AddRange(subItems);
                fileListView.Items.Add(item);
            }

            infoText.Text = "Done loading directory.";
            FolderPath.Text = directory.FullName.Remove(0, directory.FullName.IndexOf(originalPath.Name));
            fileListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

            //sort out treeview stuff.
            currentDirectory = directory;
            string directoryPath = directory.FullName.Remove(0, directory.FullName.IndexOf(originalPath.Name)).TrimEnd('\\');

            TreeNode nodeToExpand = folderView.Nodes.FindTreeNodeByFullPath(directoryPath);
            if (nodeToExpand != null)
                nodeToExpand.Expand();
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

        private void HandleLuaFile(FileInfo file)
        {
            bool doDecompile = false;
            using (BinaryReader reader = new BinaryReader(File.Open(file.FullName, FileMode.Open)))
            {
                if (reader.ReadInt32() == 1635077147)
                    doDecompile = true;
            }

            if (doDecompile)
                LuaHelper.ReadFile(file);
            else Process.Start(file.FullName);

            OnRefreshButtonClicked(null, null);
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
                if (!(tsi == FolderPath))
                {
                    width -= tsi.Width;
                    width -= tsi.Margin.Horizontal;
                }
            }

            FolderPath.Width = Math.Max(0, width - FolderPath.Margin.Horizontal);
        }
        protected override void OnLoad(EventArgs e)
        {
            LoadForm();
        }
        void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node != null)
            {
                TreeNode selectedNode = e.Node;
                OpenDirectory((DirectoryInfo)selectedNode.Tag);
            }
        }
        private void listView1_ItemActivate(object sender, EventArgs e)
        {
            if(fileListView.SelectedItems.Count > 0)
                HandleFile(fileListView.SelectedItems[0]);
        }

        private void HandleFile(ListViewItem item)
        {
            MaterialTool mTool;
            CollisionEditor cTool;
            ActorEditor aTool;
            PrefabLoader prefabs;
            SpeechEditor sTool;
            CutsceneLoader cutscene;
            IOFxFile iofx;
            EmitterFile emitterFile;
            TableEditor tTool;
            NAVData nav;
            ApexRenderMesh mesh;
            ApexClothingAssetLoader aca;
            CityAreaEditor caEditor;
            CityShopEditor csEditor;
            SoundSectorLoader soundSector;

            //DEBUG
            D3DForm d3dForm;

            //special case:
            if (item.SubItems[0].Text.Contains("SDSContent") && item.SubItems[1].Text == "XML")
            {
                new SDSContentEditor((FileInfo)item.Tag);
                return;
            }
            else if (item.SubItems[0].Text.Contains("cityareas") && item.SubItems[1].Text == "BIN")
            {
                caEditor = new CityAreaEditor((FileInfo)item.Tag);
                return;
            }
            else if (item.SubItems[0].Text.Contains("cityshop") && item.SubItems[1].Text == "BIN")
            {
                csEditor = new CityShopEditor((FileInfo)item.Tag);
                return;
            }
            else if (item.SubItems[0].Text.Contains("roadmap") && item.SubItems[1].Text == "GSD")
            {
                Roadmap roadmap = new Roadmap((item.Tag as FileInfo));
                return;
            }
            else if (item.SubItems[0].Text.Contains("sdsconfig") && item.SubItems[1].Text == "BIN")
            {
                using (BinaryReader reader = new BinaryReader(File.Open((item.Tag as FileInfo).FullName, FileMode.Open)))
                {
                    SdsConfigFile sdsConfig = new SdsConfigFile();
                    sdsConfig.ReadFromFile(reader);
                }
                return;
            }

            switch (item.SubItems[1].Text)
            {

                case "ARM":
                    mesh = new ApexRenderMesh((FileInfo)item.Tag);
                    return;
                case "ATP":
                    AnimalTrafficLoader loader = new AnimalTrafficLoader((FileInfo)item.Tag);
                    return;
                case "ACA":
                    aca = new ApexClothingAssetLoader((FileInfo)item.Tag);
                    return;
                case "Directory":
                    OpenDirectory((DirectoryInfo)item.Tag);
                    return;
                case "Material Library":
                    mTool = new MaterialTool((FileInfo)item.Tag);
                    return;
                case "NAV":
                case "NOV":
                case "NHV":
                    nav = new NAVData((FileInfo)item.Tag);
                    return;
                case "Speech Data":
                    sTool = new SpeechEditor((FileInfo)item.Tag);
                    return;
                case "CUT":
                    cutscene = new CutsceneLoader((FileInfo)item.Tag);
                    return;
                case "SDS Archive":
                    OpenSDS((FileInfo)item.Tag);
                    break;
                case "PATCH Archive":
                    OpenPATCH((FileInfo)item.Tag);
                    break;
                case "FR":
                    //fTool = new FrameResourceTool((FileInfo)item.Tag);
                    d3dForm = new D3DForm((FileInfo)item.Tag);
                    d3dForm.Dispose();
                    return;
                case "COL":
                    cTool = new CollisionEditor((FileInfo)item.Tag);
                    return;
                case "IOFX":
                    //iofx = new IOFxFile((FileInfo)item.Tag);
                    return;
                case "AEA":
                    //emitterFile = new EmitterFile((FileInfo)item.Tag);
                    return;
                case "Table":
                    tTool = new TableEditor((FileInfo)item.Tag);
                    return;
                case "TRA":
                    ResourceTypes.Translokator.TranslokatorLoader trans = new ResourceTypes.Translokator.TranslokatorLoader((FileInfo)item.Tag);
                    return;
                case "ACT":
                    aTool = new ActorEditor((FileInfo)item.Tag);
                    break;
                case "PRF":
                    prefabs = new PrefabLoader((FileInfo)item.Tag);
                    return;
                case "BIN":
                    soundSector = new SoundSectorLoader((FileInfo)item.Tag);
                    return;
                case "LUA":
                case "AP":
                    HandleLuaFile((FileInfo)item.Tag);
                    return;
                case "IFL":
                    ResourceTypes.AnimatedTexture.AnimatedTextureLoader an = new ResourceTypes.AnimatedTexture.AnimatedTextureLoader((FileInfo)item.Tag);
                    return;
                default:
                    Process.Start(((FileInfo)item.Tag).FullName);
                    break;
            }
        }

        private void ContextSDSPack_Click(object sender, EventArgs e)
        {
            if (fileListView.SelectedItems.Count == 0)
            {
                MessageBox.Show(Language.GetString("$ERROR_SELECT_ITEM"), "Toolkit", MessageBoxButtons.OK);
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

        private void onPathChange(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                string newDir = FolderPath.Text;
                int idx = newDir.IndexOf(originalPath.Name);
                if (newDir.IndexOf(originalPath.Name) == 0)
                    newDir = Path.Combine(originalPath.Parent.FullName, FolderPath.Text);

               if (Directory.Exists(newDir) && FolderPath.Text.Contains(currentDirectory.Name))
                    OpenDirectory(new DirectoryInfo(newDir));
                else
                    MessageBox.Show("Game Explorer cannot find path '" + newDir + "'. Make sure the path exists and try again.", "Game Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private void OnOptionsItem_Clicked(object sender, EventArgs e)
        {
            OptionsForm options = new OptionsForm();
            options.ShowDialog();
            Localise();
            MaterialData.Load();
        }

        private void ContextSDSUnpackAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in fileListView.Items)
            {
                if (item.SubItems[1].Text == "SDS Archive")
                    HandleFile(item);
            }
        }

        //'File' Button dropdown events.
        private void OpenMafiaIIClicked(object sender, EventArgs e)
        {
            folderView.Nodes.Clear();
            InitExplorerSettings();
        }
        private void ExitToolkitClicked(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void RunMafiaIIClicked(object sender, EventArgs e)
        {
            Process.Start(launcher.FullName);
        }

        //FileListViewStrip events.
        private void OnUpButtonClicked(object sender, EventArgs e)
        {
            if (currentDirectory.Name == originalPath.Name)
                return;

            string directoryPath = currentDirectory.FullName.Remove(0, currentDirectory.FullName.IndexOf(originalPath.Name)).TrimEnd('\\');

            TreeNode nodeToCollapse = folderView.Nodes.FindTreeNodeByFullPath(directoryPath);
            if (nodeToCollapse != null)
                nodeToCollapse.Collapse();

            OpenDirectory(currentDirectory.Parent);
        }
        private void OnRefreshButtonClicked(object sender, EventArgs e)
        {
            if (currentDirectory != null)
            {
                currentDirectory.Refresh();
                OpenDirectory(currentDirectory);
            }
        }
        private void SearchBarOnTextChanged(object sender, EventArgs e)
        {
            OpenDirectory(currentDirectory, true, SearchEntryText.Text);
        }

        //View FileList handling.
        private void FileListViewTypeController(int type)
        {
            ContextViewIcon.Checked = false;
            ContextViewDetails.Checked = false;
            ContextViewSmallIcon.Checked = false;
            ContextViewList.Checked = false;
            ContextViewTile.Checked = false;
            ViewStripMenuIcon.Checked = false;
            ViewStripMenuDetails.Checked = false;
            ViewStripMenuSmallIcon.Checked = false;
            ViewStripMenuList.Checked = false;
            ViewStripMenuTile.Checked = false;

            switch (type)
            {
                case 0:
                    ContextViewIcon.Checked = true;
                    ViewStripMenuIcon.Checked = true;
                    fileListView.View = View.LargeIcon;
                    break;
                case 1:
                    ContextViewDetails.Checked = true;
                    ViewStripMenuDetails.Checked = true;
                    fileListView.View = View.Details;
                    break;
                case 2:
                    ContextViewSmallIcon.Checked = true;
                    ViewStripMenuSmallIcon.Checked = true;
                    fileListView.View = View.SmallIcon;
                    break;
                case 3:
                    ContextViewList.Checked = true;
                    ViewStripMenuList.Checked = true;
                    fileListView.View = View.List;
                    break;
                case 4:
                    ContextViewTile.Checked = true;
                    ViewStripMenuTile.Checked = true;
                    fileListView.View = View.Tile;
                    break;
            }
        }
        private void OnViewIconClicked(object sender, EventArgs e)
        {
            FileListViewTypeController(0);
        }
        private void OnViewDetailsClicked(object sender, EventArgs e)
        {
            FileListViewTypeController(1);
        }
        private void OnViewSmallIconClicked(object sender, EventArgs e)
        {
            FileListViewTypeController(2);
        }
        private void OnViewListClicked(object sender, EventArgs e)
        {
            FileListViewTypeController(3);
        }
        private void OnViewTileClicked(object sender, EventArgs e)
        {
            FileListViewTypeController(4);
        }

        private void OnCredits_Pressed(object sender, EventArgs e)
        {
            MessageBox.Show("Toolkit written by Greavesy. \n\n" +
                "Special thanks to: \nOleg @ ZModeler 3 \nRick 'Gibbed' \nFireboyd for UnluacNET" + 
                "\n\n" + 
                "Also, a very special thanks to donators: \nInlife \nT3mas1 \nJaqub \nxEptun \nL//oO//nyRider \nNemesis7675", 
                "Toolkit", 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Information);
        }
    }
}
