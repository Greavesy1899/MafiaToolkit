using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.XPath;
using Gibbed.Mafia2.FileFormats;
using Gibbed.Mafia2.FileFormats.Archive;

namespace Mafia2Tool
{
    public partial class GameExplorer : Form
    {
        public GameExplorer()
        {
            InitializeComponent();
            BuildTreeView();
        }

        /// <summary>
        /// Build TreeView from Mafia II's main directory.
        /// </summary>
        public void BuildTreeView()
        {
            IniFile ini = new IniFile();
            TreeNode rootTreeNode;

            DirectoryInfo dirInfo = new DirectoryInfo(ini.Read("MafiaII", "Directories"));

            //check if directory exists.
            if (!dirInfo.Exists)
                return;

            //check if launcher.exe exists.
            bool hasLauncher = false;
            foreach (FileInfo file in dirInfo.GetFiles())
            {
                if (file.Name == "launcher.exe" || file.Name == "launcher")
                    hasLauncher = true;
            }

            if (!hasLauncher)
                return;

            //build treeView.
            rootTreeNode = new TreeNode(dirInfo.Name);
            rootTreeNode.Tag = dirInfo;
            GetSubFolders(dirInfo.GetDirectories(), rootTreeNode);
            folderView.Nodes.Add(rootTreeNode);
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
                if(dirs.Length != 0)
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
        }

        /// <summary>
        /// TEST:: Open an SDS from the FileInfo given.
        /// </summary>
        /// <param name="file">location of SDS.</param>
        private void OpenSDS(FileInfo file)
        {
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

            List<string> itemNames = new List<string>();
            if (string.IsNullOrEmpty(archiveFile.ResourceInfoXml) == false)
            {
                using (var reader = new StringReader(archiveFile.ResourceInfoXml))
                {
                    var doc = new XPathDocument(reader);
                    var nav = doc.CreateNavigator();
                    var nodes = nav.Select("/xml/ResourceInfo/SourceDataDescription");
                    while (nodes.MoveNext() == true)
                    {
                        itemNames.Add(nodes.Current.Value);
                    }
                }
            }

            string extractedPath = file.Directory.FullName + "/extracted/";

            //todo: need to save files.
            if (!Directory.Exists(extractedPath))
                Directory.CreateDirectory(extractedPath);

            Directory.CreateDirectory(extractedPath + file.Name);

            //TODO Cleanup this code. It's awful.
            for(int i = 0; i != archiveFile.ResourceEntries.Count; i++)
            {
                ResourceEntry entry = archiveFile.ResourceEntries[i];

                foreach (ResourceType type in archiveFile.ResourceTypes)
                {
                    if (entry.TypeId == type.Id)
                    {
                        ListViewItem item;

                        if (itemNames[i] != "not available")
                            item = new ListViewItem(itemNames[i], 1);
                        else
                            item = new ListViewItem(type.Name, 1);

                        ListViewItem.ListViewSubItem[] subItems;
                        item.Tag = file;

                        subItems = new ListViewItem.ListViewSubItem[]
                        {
                            new ListViewItem.ListViewSubItem(item, type.Name),
                            new ListViewItem.ListViewSubItem(item, entry.Data.Length.ToString()),
                            new ListViewItem.ListViewSubItem(item,
                                "")
                        };

                        item.SubItems.AddRange(subItems);
                        fileListView.Items.Add(item);

                        string path = "";
                    }
                }
            }
            fileListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
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

        void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode selectedNode = e.Node;
            OpenDirectory((DirectoryInfo)selectedNode.Tag);
        }

        private void listView1_ItemActivate(object sender, EventArgs e)
        {
            ListViewItem item = fileListView.SelectedItems[0];
            MaterialTool mTool;

            if (item.SubItems[1].Text == "Directory")
                OpenDirectory((DirectoryInfo)item.Tag);
            else if (item.SubItems[1].Text == "Material Library")
                mTool = new MaterialTool((FileInfo)item.Tag);
            else if (item.SubItems[1].Text == "SDS Archive")
                OpenSDS((FileInfo)item.Tag);
        }
    }
}
