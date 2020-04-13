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
using ResourceTypes.Misc;
using Mafia2Tool.Forms;
using ResourceTypes.FrameResource;
using SharpDX;
using Collision = ResourceTypes.Collisions.Collision;
using System.Text;

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
            LoadForm();
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
            TypeDescriptor.AddAttributes(typeof(Vector3), new TypeConverterAttribute(typeof(Vector3Converter)));
            TypeDescriptor.AddAttributes(typeof(Vector4), new TypeConverterAttribute(typeof(Vector4Converter)));
            TypeDescriptor.AddAttributes(typeof(Quaternion), new TypeConverterAttribute(typeof(QuaternionConverter)));
        }

        private void Localise()
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
            GEContext.Text = Language.GetString("$VIEW");
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
            UnpackAllSDSButton.Text = Language.GetString("$UNPACK_ALL_SDS");

            StringBuilder builder = new StringBuilder("Toolkit v");
            builder.Append(ToolkitSettings.Version);
            VersionLabel.Text = builder.ToString();
        }

        private void PrintErrorLauncher()
        {
            MessageBox.Show(Language.GetString("$ERROR_DID_NOT_FIND_LAUNCHER"), Language.GetString("$ERROR_TITLE"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            Log.WriteLine("$ERROR_DID_NOT_FIND_LAUNCHER", LoggingTypes.ERROR);
        }

        public void InitExplorerSettings()
        {
            folderView.Nodes.Clear();

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
                    break;
                }
            }

            if (!hasLauncher)
            {
                PrintErrorLauncher();
                GetPath();
                return;
            }

            InitTreeView();

            string path = originalPath.FullName + "/edit/tables/FrameProps.bin";
            if(File.Exists(path))
            {
                FileInfo info = new FileInfo(path);
                FrameProps props = new FrameProps(info);
                SceneData.FrameProperties = props;
            }

        }

        private void GetPath()
        {
            MafiaIIBrowser.SelectedPath = "";
            if (MafiaIIBrowser.ShowDialog() == DialogResult.OK)
            {
                ToolkitSettings.M2Directory = MafiaIIBrowser.SelectedPath;
                ToolkitSettings.WriteKey("MafiaII", "Directories", MafiaIIBrowser.SelectedPath);
                originalPath = new DirectoryInfo(ToolkitSettings.M2Directory);
                InitTreeView();
            }
            else
            {
                ExitProgram();
            }
        }

        private void InitTreeView()
        {
            infoText.Text = "Building folders..";
            TreeNode rootTreeNode = new TreeNode(originalPath.Name);
            rootTreeNode.Tag = originalPath;
            folderView.Nodes.Add(rootTreeNode);
            infoText.Text = "Done builidng folders..";
            OpenDirectory(originalPath);
        }

        private void GetSubdirectories(DirectoryInfo directory, TreeNode rootTreeNode)
        {
            foreach (DirectoryInfo subDirectory in directory.GetDirectories())
            {
                TreeNode node = new TreeNode(subDirectory.Name);
                node.Name = subDirectory.Name;
                node.Tag = subDirectory;
                node.ImageIndex = 0;

                if (subDirectory.GetDirectories().Length > 0)
                {
                    node.Nodes.Add("Dummy Node");
                }

                rootTreeNode.Nodes.Add(node);
            }
        }

        private void OpenDirectory(DirectoryInfo directory, bool searchMode = false, string filename = null)
        {
            infoText.Text = "Loading Directory..";
            fileListView.Items.Clear();
            ListViewItem.ListViewSubItem[] subItems;
            ListViewItem item = null;

            if (!directory.Exists)
            {
                MessageBox.Show("Could not find directory! Returning to original path..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                OpenDirectory(originalPath, false);
                return;
            }

            foreach (DirectoryInfo dir in directory.GetDirectories())
            {
                if (searchMode && !string.IsNullOrEmpty(filename))
                {
                    if (!dir.Name.Contains(filename))
                    {
                        continue;
                    }
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
                {
                    imageBank.Images.Add(file.Extension, Icon.ExtractAssociatedIcon(file.FullName));
                }

                if (searchMode && !string.IsNullOrEmpty(filename))
                {
                    if (!file.Name.Contains(filename))
                    {
                        continue;
                    }
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
            currentDirectory = directory;
            string directoryPath = directory.FullName.Remove(0, directory.FullName.IndexOf(originalPath.Name)).TrimEnd('\\');

            FolderPath.Text = directoryPath;
            fileListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

            //we have to remove the AfterExpand event before we expand the node.
            folderView.AfterExpand -= FolderViewAfterExpand;
            TreeNode folderNode = folderView.Nodes.FindTreeNodeByFullPath(directoryPath);

            if (folderNode != null)
            {
                folderNode.Nodes.Clear();
                GetSubdirectories(currentDirectory, folderNode);
                folderNode.Expand();
            }
            else
            {

                MessageBox.Show("Failed to find directory in FolderView!", "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            folderView.AfterExpand += FolderViewAfterExpand;
        }

        /// <summary>
        /// Pack an SDS from the FileInfo given.
        /// </summary>
        /// <param name="file">location of SDS.</param>
        private void PackSDS(FileInfo file)
        {

            if (file == null)
                MessageBox.Show("File is null");

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
                {
                    throw new FormatException();
                }
            }

            using (var output = File.Create(file.FullName))
            {
                archiveFile.Serialize(output, ToolkitSettings.SerializeSDSOption == 0 ? ArchiveSerializeOptions.OneBlock : ArchiveSerializeOptions.Compress);
            }
            infoText.Text = "Saved SDS.";
        }

        private void OpenSDS(FileInfo file, bool openDirectory = true)
        {
            string backupFolder = Path.Combine(file.Directory.FullName, "BackupSDS");
            string extractedFolder = Path.Combine(file.Directory.FullName, "extracted");

            //backup file before unpacking..
            if (!Directory.Exists(backupFolder))
            {
                Directory.CreateDirectory(backupFolder);
            }

            //place copy in new folder.
            string time = string.Format("{0}_{1}_{2}_{3}_{4}", DateTime.Now.TimeOfDay.Hours, DateTime.Now.TimeOfDay.Minutes, DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);
            string filename = ToolkitSettings.AddTimeDataBackup == true ? file.Name.Insert(file.Name.Length - 4, "_" + time) : file.Name;
            File.Copy(file.FullName, Path.Combine(backupFolder, filename), true);

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
            if (openDirectory)
            {
                var directory = file.Directory;
                string path = directory.FullName.Remove(0, directory.FullName.IndexOf(originalPath.Name, StringComparison.InvariantCultureIgnoreCase)).TrimEnd('\\');
                TreeNode node = folderView.Nodes.FindTreeNodeByFullPath(path);

                if(!node.Nodes.ContainsKey("extracted"))
                {
                    var extracted = new TreeNode("extracted");
                    extracted.Tag = extracted;
                    extracted.Name = "extracted";
                    extracted.Nodes.Add(file.Name);
                    node.Nodes.Add(extracted);
                }
                else
                {
                    node.Nodes["extracted"].Nodes.Add(file.Name);
                }

                OpenDirectory(new DirectoryInfo(Path.Combine(extractedFolder, file.Name)));
                infoText.Text = "Opened SDS..";
            }
        }

        private void OpenPATCH(FileInfo file)
        {
            Log.WriteLine("Opening PATCH: " + file.Name);

            infoText.Text = "Opening PATCH..";

            PatchFile patch;
            using (var input = File.OpenRead(file.FullName))
            {
                patch = new PatchFile();
                patch.file = file;
                patch.Deserialize(input, Gibbed.IO.Endian.Big);
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

        private void HandleSDSMap(FileInfo info, bool forceBigEndian = false)
        {
            //make sure to load materials.
            MaterialData.Load();


            //we now build scene data from GameExplorer rather than d3d viewer.
            SceneData.ScenePath = info.DirectoryName;
            SceneData.BuildData(forceBigEndian);

            //d3d viewer expects data inside scenedata.
            D3DForm d3dForm = new D3DForm(info);
            d3dForm.Dispose();
        }

        private bool HandleStreamMap(FileInfo file)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(file.FullName, FileMode.Open)))
            {
                if (reader.ReadInt32() == 1299346515)
                    return true;
            }
            return false;
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
                if (tsi != FolderPath)
                {
                    width -= tsi.Width;
                    width -= tsi.Margin.Horizontal;
                }
            }

            FolderPath.Width = Math.Max(0, width - FolderPath.Margin.Horizontal);
        }
        private void listView1_ItemActivate(object sender, EventArgs e)
        {
            if (fileListView.SelectedItems.Count > 0)
                HandleFile(fileListView.SelectedItems[0]);
        }


        //Improve this, its bad.
        private void HandleFile(ListViewItem item)
        {
            if (ToolkitSettings.UseSDSToolFormat)
            {
                switch (item.SubItems[1].Text)
                {
                    case "Directory":
                        OpenDirectory((DirectoryInfo)item.Tag);
                        return;
                    case "SDS Archive":
                        OpenSDS((FileInfo)item.Tag);
                        break;
                    default:
                        Process.Start(((FileInfo)item.Tag).FullName);
                        break;
                }
                return;
            }

            MaterialTool mTool;
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
            else if (item.SubItems[0].Text.Contains("FrameProps") && item.SubItems[1].Text == "BIN")
            {
                FrameProps fProps = new FrameProps((FileInfo)item.Tag);
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
            else if (item.SubItems[0].Text.Contains("shopmenu2") && item.SubItems[1].Text == "BIN")
            {
                ShopMenu2Editor editor = new ShopMenu2Editor((item.Tag as FileInfo));
                return;
            }
            else if (item.SubItems[1].Text == "BIN" && HandleStreamMap((item.Tag as FileInfo)))
            {
                StreamEditor editor = new StreamEditor((item.Tag as FileInfo));
                return;
            }
            else if (item.SubItems[1].Text == "BIN" && CGameData.CheckHeader((item.Tag as FileInfo)))
            {
                CGameData data = new CGameData((item.Tag as FileInfo));
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
                    HandleSDSMap((FileInfo)item.Tag);
                    return;
                case "COL":
                    MessageBox.Show("$COLLISION_EDITOR_REMOVED", "Toolkit", MessageBoxButtons.OK);
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
                case "TRA":
                    TranslokatorEditor editor = new TranslokatorEditor((FileInfo)item.Tag);
                    return;
                case "ACT":
                    aTool = new ActorEditor((FileInfo)item.Tag);
                    break;
                case "PRF":
                    prefabs = new PrefabLoader((FileInfo)item.Tag);
                    return;
                case "LUA":
                case "AP":
                case "SHP":
                    HandleLuaFile((FileInfo)item.Tag);
                    return;
                case "IFL":
                    ResourceTypes.AnimatedTexture.AnimatedTextureLoader an = new ResourceTypes.AnimatedTexture.AnimatedTextureLoader((FileInfo)item.Tag);
                    return;
                case "IDS":
                    ResourceTypes.ItemDesc.ItemDescLoader itemDesc = new ResourceTypes.ItemDesc.ItemDescLoader((item.Tag as FileInfo).FullName);
                    return;
                case "BIN":
                    SoundSectorLoader sLoader = new SoundSectorLoader(item.Tag as FileInfo);
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
            GEContext.Items[0].Visible = false;
            GEContext.Items[1].Visible = false;
            GEContext.Items[6].Visible = false;

            if (fileListView.SelectedItems.Count == 0)
            {
                return;
            }

            if (fileListView.SelectedItems[0].Tag.GetType() == typeof(FileInfo))
            {
                string extension = (fileListView.SelectedItems[0].Tag as FileInfo).Extension;

                if (extension == ".sds")
                {
                    GEContext.Items[0].Visible = true;
                    GEContext.Items[1].Visible = true;
                }
                else if(extension == ".fr")
                {
                    GEContext.Items[6].Visible = true;
                }
            }
        }
        private void OnOptionsItem_Clicked(object sender, EventArgs e)
        {
            OptionsForm options = new OptionsForm();
            options.ShowDialog();
            Localise();
        }

        private void ExitProgram()
        {
            Application.ExitThread();
            Application.Exit();
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
        private void OpenMafiaIIClicked(object sender, EventArgs e) => InitExplorerSettings();
        private void ExitToolkitClicked(object sender, EventArgs e)
        {
            ExitProgram();
        }
        private void RunMafiaIIClicked(object sender, EventArgs e) => Process.Start(launcher.FullName);
        private void SearchBarOnTextChanged(object sender, EventArgs e) => OpenDirectory(currentDirectory, true, SearchEntryText.Text);

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
        private void OnViewIconClicked(object sender, EventArgs e) => FileListViewTypeController(0);
        private void OnViewDetailsClicked(object sender, EventArgs e) => FileListViewTypeController(1);
        private void OnViewSmallIconClicked(object sender, EventArgs e) => FileListViewTypeController(2);
        private void OnViewListClicked(object sender, EventArgs e) => FileListViewTypeController(3);
        private void OnViewTileClicked(object sender, EventArgs e) => FileListViewTypeController(4);
        private void UnpackAllSDSButton_Click(object sender, EventArgs e) => UnpackSDSRecurse(originalPath);

        private void OnCredits_Pressed(object sender, EventArgs e)
        {
            MessageBox.Show("Toolkit developed by Greavesy. \n\n" +
                "Special thanks to: \nOleg @ ZModeler 3 \nRick 'Gibbed' \nFireboyd for developing UnluacNET" +
                "\n\n" +
                "Also, a very special thanks to PayPal donators: \nInlife \nT3mas1 \nJaqub \nxEptun \nL//oO//nyRider \nNemesis7675" +
                "\n\n" +
                "And Patreons: \nHamAndRock \nMelber", 
                "Toolkit",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void OnKeyPressed(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 0x8)
                OnUpButtonClicked(null, null);
        }

        private void FolderViewAfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null)
            {
                DirectoryInfo dir = (DirectoryInfo)e.Node.Tag;

                if (currentDirectory != dir)
                    OpenDirectory(dir);
            }
        }

        private void M2FBXButtonClicked(object sender, EventArgs e)
        {
            M2FBXTool tool = new M2FBXTool();
        }

        private void CheckValidSDS(FileInfo info)
        {
            if (info.Extension.Contains(".sds"))
            {
                Debug.WriteLine("Unpacking " + info.FullName);
                OpenSDS(info, false);
            }
        }
        private void UnpackSDSRecurse(DirectoryInfo info)
        {
            foreach (var file in info.GetFiles())
            {
                CheckValidSDS(file);
            }

            foreach (var directory in info.GetDirectories())
            {
                UnpackSDSRecurse(directory);
            }

            Debug.WriteLine("Finished Unpack All SDS Function");
        }

        private void CreateFrameResource_OnClick(object sender, EventArgs e)
        {
            FrameResource fr = new FrameResource();
            fr.WriteToFile(Path.Combine(currentDirectory.FullName, "FrameResource_0.fr"));
        }

        private void CreateSDSContentButton_Click(object sender, EventArgs e)
        {

        }

        private void CreateCollisionButton_Click(object sender, EventArgs e)
        {
            Collision collision = new Collision();
            collision.WriteToFile(Path.Combine(currentDirectory.FullName, "Collision_0.col"));
        }

        private void ContextForceBigEndian_Click(object sender, EventArgs e)
        {
            HandleSDSMap((FileInfo)fileListView.SelectedItems[0].Tag, true);
        }
    }
}
