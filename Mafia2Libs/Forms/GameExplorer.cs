using Core.IO;
using Mafia2Tool.Forms;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Windows.Forms;
using Utils.Extensions;
using Utils.Language;
using Utils.Settings;
using Vortice.Mathematics;

namespace Mafia2Tool
{
    public partial class GameExplorer : Form
    {
        private DirectoryInfo currentDirectory;
        private DirectoryInfo rootDirectory;
        private DirectoryInfo pcDirectory;
        private FileInfo launcher;
        private Game game;

        private FileSystemWatcher DirectoryWatcher;

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
            InitFileWatcher();
            FileListViewTypeController(1);
            infoText.Text = "Ready..";

            // Add Attributes to System.Numerics and Vortice.Mathematics types so we can edit them in the PropertyGrid.
            TypeDescriptor.AddAttributes(typeof(Vector3), new TypeConverterAttribute(typeof(Vector3Converter)));
            TypeDescriptor.AddAttributes(typeof(Vector4), new TypeConverterAttribute(typeof(Vector4Converter)));
            TypeDescriptor.AddAttributes(typeof(Quaternion), new TypeConverterAttribute(typeof(QuaternionConverter)));
            TypeDescriptor.AddAttributes(typeof(Matrix4x4), new TypeConverterAttribute(typeof(ExpandableObjectConverter)));
            TypeDescriptor.AddAttributes(typeof(BoundingBox), new TypeConverterAttribute(typeof(ExpandableObjectConverter)));
        }

        private void Localise()
        {
            dropdownFile.Text = Language.GetString("$FILE");
            dropdownView.Text = Language.GetString("$VIEW");
            dropdownTools.Text = Language.GetString("$TOOLS");
            dropdownAbout.Text = Language.GetString("$ABOUT");

            AboutButton.Text = Language.GetString("$ABOUT");
            Text = Language.GetString("$MII_TK_GAME_EXPLORER");
            FolderUpButton.ToolTipText = Language.GetString("$UP_TOOLTIP");
            FolderPath.ToolTipText = Language.GetString("$FOLDER_PATH_TOOLTIP");
            FolderRefreshButton.Text = Language.GetString("$REFRESH");
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
            ContextDeleteSelectedFiles.Text = Language.GetString("$DELETE_SELECTED_OBJECTS");
            ContextUnpackSelectedSDS.Text = Language.GetString("$UNPACK_SELECTED_SDS");
            ContextPackSelectedSDS.Text = Language.GetString("$PACK_SELECTED_SDS");
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
            OpenGameFolderButton.Text = Language.GetString("$BTN_OPEN_MII");
            RunGameButton.Text = Language.GetString("$BTN_RUN_MII");
            ExitEditorButton.Text = Language.GetString("$EXIT");
            OptionsItem.Text = Language.GetString("$OPTIONS");
            PackCurrentSDSButton.Text = Language.GetString("$UNPACK_SELECTED_SDS");
            UnpackCurrentSDSButton.Text = Language.GetString("$PACK_SELECTED_SDS");
            UnpackAllSDSButton.Text = Language.GetString("$UNPACK_ALL_SDS");
            SelectGameButton.Text = Language.GetString("$SELECT_GAME");

            Button_UnpackSDS.Text = Language.GetString("$UNPACK");
            Button_UnpackSDS.ToolTipText = Language.GetString("$UNPACK");
            Button_PackSDS.Text = Language.GetString("$PACK");
            Button_PackSDS.ToolTipText = Language.GetString("$PACK");
            Button_Settings.Text = Language.GetString("$OPTIONS");
            Button_Settings.ToolTipText = Language.GetString("$OPTIONS");
        }

        public void InitExplorerSettings()
        {
            folderView.Nodes.Clear();

            game = GameStorage.Instance.GetSelectedGame();
            pcDirectory = new DirectoryInfo(game.Directory);
            launcher = new FileInfo(pcDirectory.FullName + "/" + GameStorage.GetExecutableName(game.GameType));

            if(!launcher.Exists)
            {
                DialogResult result = MessageBox.Show("Could not find executable! Would you like to change the selected game?", "Toolkit", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                if(result == DialogResult.OK)
                {
                    OpenGameSelectorWindow();
                }
                //Close();
                return;
            }

            InitTreeView();
        }

        private void InitTreeView()
        {
            infoText.Text = "Building folders..";

            if (game.GameType != GamesEnumerator.MafiaIII && game.GameType != GamesEnumerator.MafiaI_DE)
            {
                rootDirectory = pcDirectory.Parent;
            }
            else
            {
                rootDirectory = pcDirectory;
            }

            TreeNode rootTreeNode = new TreeNode(rootDirectory.Name);
            rootTreeNode.Tag = rootDirectory;
            folderView.Nodes.Add(rootTreeNode);
            infoText.Text = "Done building folders..";
            OpenDirectory(rootDirectory);
        }

        private void InitFileWatcher()
        {
            DirectoryWatcher = new FileSystemWatcher(rootDirectory.FullName);
            DirectoryWatcher.SynchronizingObject = this;
            DirectoryWatcher.IncludeSubdirectories = true;
            DirectoryWatcher.EnableRaisingEvents = false;
            DirectoryWatcher.Changed += DirectoryWatcher_OnAnyChange;
            DirectoryWatcher.Created += DirectoryWatcher_OnAnyChange;
            DirectoryWatcher.Created += DirectoryWatcher_OnAnyChange;
            DirectoryWatcher.Renamed += DirectoryWatcher_OnAnyChange;
            DirectoryWatcher.Deleted += DirectoryWatcher_OnAnyChange;
        }

        private void DirectoryWatcher_OnAnyChange(object sender, FileSystemEventArgs e)
        {
            if (e.FullPath.Contains(currentDirectory.FullName))
            {
                this.BeginInvoke((MethodInvoker)(() => OpenDirectory(currentDirectory)));
            }
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
            // Make sure toolstrip buttons are reset
            SetPackUnpackButtonEnabled(false);

            infoText.Text = "Loading Directory..";
            fileListView.Items.Clear();
            ListViewItem.ListViewSubItem[] subItems;
            ListViewItem item = null;

            if (!directory.Exists)
            {               
                string FolderPath = directory.FullName;
                int Index = FolderPath.LastIndexOf('\\');
                List<string> NewPath = new List<string>();
                while (Index != -1)
                {
                    NewPath.Add(FolderPath.Substring(0, Index));
                    FolderPath = FolderPath.Substring(0, Index);
                    Index = FolderPath.LastIndexOf('\\');
                }
                for (int i = 0; i < NewPath.Count; i++)
                {
                    if (Directory.Exists(NewPath[i]) == true)
                    {
                        DirectoryInfo NewDirectoryInfo = new DirectoryInfo(NewPath[i]);
                        OpenDirectory(NewDirectoryInfo);
                        infoText.Text = "Returned to the previous folder.";
                        return;
                    }
                }
            }

            DirectoryBase directoryInfo = new DirectoryBase(directory);

            foreach (DirectoryInfo dir in directory.GetDirectories())
            {
                DirectoryBase childInfo = new DirectoryBase(dir);

                if (searchMode && !string.IsNullOrEmpty(filename))
                {
                    if (!dir.Name.Contains(filename))
                    {
                        continue;
                    }
                }
                item = new ListViewItem(dir.Name, 0);
                item.Tag = childInfo;
                subItems = new ListViewItem.ListViewSubItem[]
                {
                    new ListViewItem.ListViewSubItem(item, "Directory"),
                    new ListViewItem.ListViewSubItem(item, (dir.GetDirectories().Length + dir.GetFiles().Length).ToString() + " items"),
                    new ListViewItem.ListViewSubItem(item, dir.LastWriteTime.ToShortDateString()),
                };
                item.SubItems.AddRange(subItems);
                fileListView.Items.Add(item);
            }

            foreach (FileInfo info in directory.GetFiles())
            {
                if (!imageBank.Images.ContainsKey(info.Extension))
                {
                    var icon = Icon.ExtractAssociatedIcon(info.FullName);
                    if (icon != null)
                    {
                        imageBank.Images.Add(info.Extension, icon);
                    }
                }

                if (searchMode && !string.IsNullOrEmpty(filename))
                {
                    if (!info.Name.Contains(filename))
                    {
                        continue;
                    }
                }

                var file = FileFactory.ConstructFromFileInfo(info);

                item = new ListViewItem(info.Name, imageBank.Images.IndexOfKey(info.Extension));
                item.Tag = file;
                subItems = new ListViewItem.ListViewSubItem[]
                {
                    new ListViewItem.ListViewSubItem(item, file.GetExtensionUpper()),
                    new ListViewItem.ListViewSubItem(item, file.GetFileSizeAsString()),
                    new ListViewItem.ListViewSubItem(item, file.GetLastTimeWrite()),
                };

                directoryInfo.AddLoadedFile(file);
                item.SubItems.AddRange(subItems);
                fileListView.Items.Add(item);
            }

            infoText.Text = "Done loading directory.";
            currentDirectory = directory;
            string directoryPath = directory.FullName.Remove(0, directory.FullName.IndexOf(rootDirectory.Name)).TrimEnd('\\');

            FolderPath.Text = directoryPath;
            fileListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

            // We have to remove the AfterExpand event before we expand the node.
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

        private void OpenSDSDirectory(FileInfo file, bool openDirectory = true)
        {
            string extractedFolder = Path.Combine(file.Directory.FullName, "extracted");

            if (openDirectory)
            {
                var directory = file.Directory;
                string path = directory.FullName.Remove(0, directory.FullName.IndexOf(rootDirectory.Name, StringComparison.InvariantCultureIgnoreCase)).TrimEnd('\\');
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
            {
                HandleFile(fileListView.SelectedItems[0]);
            }
        }

        private bool OpenFile(FileBase asset)
        {
            // cannot open files if this format is selected
            if ((asset as FileSDS) == null)
            {
                if (ToolkitSettings.UseSDSToolFormat)
                {
                    MessageBox.Show("These files are not supported with SDSTools format. Please navigate to the settings and de-select 'Use SDS Tool Format'", "Toolkit");
                    return false;
                }
            }

            if(!asset.Open())
            {
                return false;
            }

            if (asset is FileSDS)
            {           
                OpenSDSDirectory(asset.GetUnderlyingFileInfo());
            }
            else
            {
                OpenDirectory(currentDirectory);
            }

            return true;
        }

        private void HandleFile(ListViewItem item)
        {
            if (ToolkitSettings.UseSDSToolFormat)
            {
                switch (item.SubItems[1].Text)
                {
                    case "Directory":
                        var directory = (item.Tag as DirectoryBase);
                        OpenDirectory(directory.GetDirectoryInfo());
                        return;
                    case "SDS":
                        OpenFile(item.Tag as FileBase);
                        return;
                    default:
                        Process.Start(((FileInfo)item.Tag).FullName);
                        break;
                }
                return;
            }

            if(item.Tag is FileBase)
            {
                var asset = (item.Tag as FileBase);
                
                if(OpenFile(asset))
                {
                    return;
                }
            }
            else if(item.SubItems[1].Text.Equals("Directory"))
            {
                var directory = (item.Tag as DirectoryBase);
                OpenDirectory(directory.GetDirectoryInfo());
                return;
            }
        }

        private void ContextSDSPack_Click(object sender, EventArgs e)
        {
            if (fileListView.SelectedItems.Count == 0)
            {
                MessageBox.Show(Language.GetString("$ERROR_SELECT_ITEM"), "Toolkit", MessageBoxButtons.OK);
                return;
            }

            var file = fileListView.SelectedItems[0].Tag as FileBase;
            var info = file.GetUnderlyingFileInfo();

            if(info.Attributes.HasFlag(FileAttributes.ReadOnly))
            {
                DialogResult result = MessageBox.Show("Detected a read only file. Would you like to forcefully pack?", "Toolkit", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result != DialogResult.Yes)
                {
                    return;
                }

                info.Attributes -= FileAttributes.ReadOnly;
            }

            if(file is FileSDS)
            {
                (file as FileSDS).Save();
                infoText.Text = string.Format("Packed SDS: {0}", file.GetName());
            }
        }

        private void ContextSDSUnpack_Click(object sender, EventArgs e)
        {
            if (fileListView.SelectedItems.Count > 0)
            {
                object Tag = fileListView.SelectedItems[0].Tag;
                if (Tag is FileSDS)
                {
                    FileSDS SDSFile = (Tag as FileSDS);
                    SDSFile.Open();
                    OpenSDSDirectory(SDSFile.GetUnderlyingFileInfo());
                }
            }
        }

        private void onPathChange(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                string newDir = FolderPath.Text;
                int idx = newDir.IndexOf(rootDirectory.Name);
                if (newDir.IndexOf(rootDirectory.Name) == 0)
                {
                    newDir = Path.Combine(rootDirectory.Parent.FullName, FolderPath.Text);
                }

                if (Directory.Exists(newDir) && FolderPath.Text.Contains(currentDirectory.Name))
                {
                    OpenDirectory(new DirectoryInfo(newDir));
                }
                else
                {
                    MessageBox.Show("Game Explorer cannot find path '" + newDir + "'. Make sure the path exists and try again.", "Game Explorer", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }

        private void ContextOpenFolder_Click(object sender, EventArgs e)
        {
            ProcessStartInfo StartInfo = new ProcessStartInfo();
            StartInfo.UseShellExecute = true;
            StartInfo.FileName = currentDirectory.FullName;

            Process.Start(StartInfo);
        }

        private void OnOpening(object sender, CancelEventArgs e)
        {
            ContextSDSUnpack.Visible = false;
            ContextSDSPack.Visible = false;
            ContextForceBigEndian.Visible = false;
            ContextDeleteSelectedFiles.Visible = false;
            ContextUnpackSelectedSDS.Visible = false;
            ContextPackSelectedSDS.Visible = false;

            if (fileListView.SelectedItems.Count == 0)
            {
                return;
            }

            if (fileListView.SelectedItems[0].Tag is FileBase)
            {
                object Tag = fileListView.SelectedItems[0].Tag;

                if (Tag is FileSDS)
                {
                    ContextSDSUnpack.Visible = true;
                    ContextSDSPack.Visible = true;
                }
                else if(Tag is FileFrameResource)
                {
                    ContextForceBigEndian.Visible = true;
                }
            }

            foreach (ListViewItem item in fileListView.Items)
            {
                if(item.Tag is FileSDS)
                {
                    ContextPackSelectedSDS.Visible = true;
                    ContextUnpackSelectedSDS.Visible = true;
                }
            }

            ContextDeleteSelectedFiles.Visible = true;
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
            DialogResult Result = MessageBox.Show("Are you sure you want to unpack all SDS?", "Toolkit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Return if user said no
            if(Result == DialogResult.No)
            {
                return;
            }

            foreach (ListViewItem item in fileListView.Items)
            {
                if(item.Tag is FileSDS)
                {
                    (item.Tag as FileSDS).Open();
                }
            }
        }

        //'File' Button dropdown events.
        private void OpenMafiaIIClicked(object sender, EventArgs e) => InitExplorerSettings();
        private void ExitToolkitClicked(object sender, EventArgs e) => ExitProgram();
        private void RunMafiaIIClicked(object sender, EventArgs e) => Process.Start(launcher.FullName);
        private void SearchBarOnTextChanged(object sender, EventArgs e) => OpenDirectory(currentDirectory, true, SearchEntryText.Text);

        //FileListViewStrip events.
        private void OnUpButtonClicked(object sender, EventArgs e)
        {
            if (currentDirectory.Name == rootDirectory.Name)
                return;

            string directoryPath = currentDirectory.FullName.Remove(0, currentDirectory.FullName.IndexOf(rootDirectory.Name)).TrimEnd('\\');

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
        private void UnpackAllSDSButton_Click(object sender, EventArgs e)
        {
            DialogResult Result = MessageBox.Show("Are you sure you want to unpack all SDS Archives?", "Toolkit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Result == DialogResult.No)
            {
                return;
            }

            UnpackSDSRecurse(rootDirectory);
        }

        private void OnCredits_Pressed(object sender, EventArgs e)
        {
            var aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }

        private void ListView_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                OnUpButtonClicked(null, null);
            }
            else if(e.Control && e.KeyCode == Keys.P)
            {
                // TODO: Make this not use the function which is wired up to a delegate
                ContextSDSPack_Click(sender, null);
            }
            else if (e.Control && e.KeyCode == Keys.U)
            {
                // TODO: Make this not use the function which is wired up to a delegate
                ContextSDSUnpack_Click(sender, null);
            }
            else if(e.Control && e.KeyCode == Keys.Delete)
            {
                // TODO: Make this not use the function which is wired up to a delegate
                ContextDeleteSelectedFiles_OnClick(sender, null);
            }
        }

        private void FolderViewAfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null)
            {
                DirectoryInfo dir = (DirectoryInfo)e.Node.Tag;

                if (currentDirectory != dir)
                {
                    OpenDirectory(dir);
                }
            }
        }

        private void CheckValidSDS(FileInfo info)
        {
            var file = FileFactory.ConstructFromFileInfo(info);

            if(file is FileSDS)
            {
                var SDSFile = (file as FileSDS);
                Debug.WriteLine("Unpacking " + info.FullName);
                SDSFile.Open();
            }
        }

        private void OpenGameSelectorWindow()
        {
            GameSelector selector = new GameSelector();
            if (selector.ShowDialog() == DialogResult.OK)
            {
                InitExplorerSettings();
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
                if (!directory.Name.Contains("BackupSDS"))
                {
                    UnpackSDSRecurse(directory);
                }
            }

            Debug.WriteLine("Finished Unpack All SDS Function");
        }

        private void ContextForceBigEndian_Click(object sender, EventArgs e)
        {
            var file = (fileListView.SelectedItems[0].Tag as FileBase);
            if(file is FileFrameResource)
            {
                FileFrameResource frameResource = (file as FileFrameResource);
                frameResource.SetBigEndian(true);
                frameResource.Open();
            }
        }

        private void Button_SelectGame_OnClick(object sender, EventArgs e)
        {
            OpenGameSelectorWindow();
        }

        private void ListView_OnDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void ListView_OnDragLeave(object sender, EventArgs e)
        {
        }

        private void ListView_OnDragDrop(object sender, DragEventArgs e)
        {
            var formats = e.Data.GetFormats();
            if(e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

                if (s.Length > 0)
                {
                    FileInfo info = new FileInfo(s[0]);
                    FileBase file = FileFactory.ConstructFromFileInfo(info);
                    bool result = OpenFile(file);
                }

            }
        }

        private void ContextDeleteSelectedFiles_OnClick(object sender, EventArgs e)
        {
            DialogResult Result = MessageBox.Show("Are you sure? This will delete all selected files and folders.", "Toolkit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // If not yes, then end function
            if(Result != DialogResult.Yes)
            {
                return;
            }

            foreach(ListViewItem SelectedObject in fileListView.SelectedItems)
            {
                object ActualObject = SelectedObject.Tag;

                if(ActualObject is FileBase)
                {
                    (ActualObject as FileBase).Delete();
                }
                else if(ActualObject is DirectoryBase)
                {
                    (ActualObject as DirectoryBase).Delete();
                }
            }

            OpenDirectory(currentDirectory);
        }

        private void ContextUnpackSelectedSDS_OnClick(object sender, EventArgs e)
        {
            foreach (ListViewItem SelectedObject in fileListView.SelectedItems)
            {
                object ActualObject = SelectedObject.Tag;

                if (ActualObject is FileSDS)
                {
                    (ActualObject as FileSDS).Open();
                }
            }
        }

        private void ContextPackSelectedSDS_OnClick(object sender, EventArgs e)
        {
            foreach (ListViewItem SelectedObject in fileListView.SelectedItems)
            {
                object ActualObject = SelectedObject.Tag;

                if (ActualObject is FileSDS)
                {
                    (ActualObject as FileSDS).Save();
                }
            }
        }

        private void ListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            SetPackUnpackButtonEnabled(false);

            if (e.Item != null)
            {
                object Tag = e.Item.Tag;
                if(Tag is FileSDS)
                {
                    SetPackUnpackButtonEnabled(true);
                }
            }
        }

        private void SetPackUnpackButtonEnabled(bool enabled)
        {
            Button_UnpackSDS.Enabled = enabled;
            Button_PackSDS.Enabled = enabled;
            UnpackCurrentSDSButton.Enabled = enabled;
            PackCurrentSDSButton.Enabled = enabled;
        }
    }
}
