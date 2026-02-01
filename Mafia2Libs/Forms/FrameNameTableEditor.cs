using System;
using System.IO;
using System.Windows.Forms;
using ResourceTypes.FrameNameTable;
using Utils.Language;
using Utils.Settings;

namespace Mafia2Tool
{
    public partial class FrameNameTableEditor : Form
    {
        private FileInfo file;
        private FrameNameTable frameNameTable;
        private bool bIsFileEdited = false;

        public FrameNameTableEditor(FileInfo info)
        {
            InitializeComponent();
            file = info;
            Localise();
            LoadData();
            Show();
            ToolkitSettings.UpdateRichPresence("Using the FrameNameTable editor.");
        }

        private void Localise()
        {
            Text = Language.GetString("$FNT_EDITOR_TITLE");
            FileButton.Text = Language.GetString("$FILE");
            SaveButton.Text = Language.GetString("$SAVE");
            ReloadButton.Text = Language.GetString("$RELOAD");
            ExitButton.Text = Language.GetString("$EXIT");
            EditButton.Text = Language.GetString("$EDIT");
            AddEntryButton.Text = Language.GetString("$ADD_ITEM");
            DeleteEntryButton.Text = Language.GetString("$DELETE");
            ToolsButton.Text = Language.GetString("$TOOLS");
            ExportXMLButton.Text = Language.GetString("$EXPORT_XML");
        }

        private void LoadData()
        {
            DataTreeView.Nodes.Clear();

            frameNameTable = new FrameNameTable(file.FullName);

            TreeNode rootNode = new TreeNode("FrameNameTable");
            rootNode.Tag = frameNameTable;

            for (int i = 0; i < frameNameTable.FrameData.Length; i++)
            {
                FrameNameTable.Data data = frameNameTable.FrameData[i];
                string nodeName = !string.IsNullOrEmpty(data.Name) ? data.Name : $"Entry_{i}";
                TreeNode node = new TreeNode(nodeName);
                node.Tag = data;
                rootNode.Nodes.Add(node);
            }

            DataTreeView.Nodes.Add(rootNode);
            rootNode.Expand();

            Text = Language.GetString("$FNT_EDITOR_TITLE");
            bIsFileEdited = false;
        }

        private void Save()
        {
            File.Copy(file.FullName, file.FullName + "_old", true);
            using (BinaryWriter writer = new BinaryWriter(File.Open(file.FullName, FileMode.Create)))
            {
                frameNameTable.WriteToFile(writer);
            }

            Text = Language.GetString("$FNT_EDITOR_TITLE");
            bIsFileEdited = false;
        }

        private void Reload()
        {
            LoadData();
            DataGrid.SelectedObject = null;
            DataTreeView.SelectedNode = null;
        }

        private void Delete()
        {
            TreeNode selectedNode = DataTreeView.SelectedNode;
            if (selectedNode == null || selectedNode.Tag == null)
                return;

            if (selectedNode.Tag is FrameNameTable.Data)
            {
                FrameNameTable.Data[] currentData = frameNameTable.FrameData;
                FrameNameTable.Data[] newData = new FrameNameTable.Data[currentData.Length - 1];

                int index = 0;
                for (int i = 0; i < currentData.Length; i++)
                {
                    if (currentData[i] != selectedNode.Tag)
                    {
                        if (index < newData.Length)
                        {
                            newData[index] = currentData[i];
                            index++;
                        }
                    }
                }

                frameNameTable.FrameData = newData;
                DataTreeView.Nodes.Remove(selectedNode);

                Text = Language.GetString("$FNT_EDITOR_TITLE") + "*";
                bIsFileEdited = true;
            }
        }

        private void AddEntry()
        {
            FrameNameTable.Data newEntry = new FrameNameTable.Data();
            newEntry.Name = "NewEntry";
            newEntry.ParentName = "";
            newEntry.FrameIndex = 0;
            newEntry.Flags = NameTableFlags.flag_1;

            FrameNameTable.Data[] currentData = frameNameTable.FrameData;
            FrameNameTable.Data[] newData = new FrameNameTable.Data[currentData.Length + 1];
            currentData.CopyTo(newData, 0);
            newData[currentData.Length] = newEntry;
            frameNameTable.FrameData = newData;

            TreeNode node = new TreeNode(newEntry.Name);
            node.Tag = newEntry;

            if (DataTreeView.Nodes.Count > 0)
            {
                DataTreeView.Nodes[0].Nodes.Add(node);
            }

            Text = Language.GetString("$FNT_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

        private void ExportToXML()
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "XML Files (*.xml)|*.xml";
                saveDialog.FileName = Path.GetFileNameWithoutExtension(file.Name) + ".xml";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter writer = new StreamWriter(saveDialog.FileName))
                    {
                        writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                        writer.WriteLine("<FrameNameTable>");
                        writer.WriteLine($"  <EntryCount>{frameNameTable.FrameData.Length}</EntryCount>");
                        writer.WriteLine("  <Entries>");

                        for (int i = 0; i < frameNameTable.FrameData.Length; i++)
                        {
                            FrameNameTable.Data data = frameNameTable.FrameData[i];
                            writer.WriteLine("    <Entry>");
                            writer.WriteLine($"      <Index>{i}</Index>");
                            writer.WriteLine($"      <Name>{System.Security.SecurityElement.Escape(data.Name)}</Name>");
                            writer.WriteLine($"      <ParentName>{System.Security.SecurityElement.Escape(data.ParentName)}</ParentName>");
                            writer.WriteLine($"      <Parent>{data.Parent}</Parent>");
                            writer.WriteLine($"      <NamePos1>{data.NamePos1}</NamePos1>");
                            writer.WriteLine($"      <NamePos2>{data.NamePos2}</NamePos2>");
                            writer.WriteLine($"      <FrameIndex>{data.FrameIndex}</FrameIndex>");
                            writer.WriteLine($"      <Flags>{(int)data.Flags}</Flags>");
                            writer.WriteLine("    </Entry>");
                        }

                        writer.WriteLine("  </Entries>");
                        writer.WriteLine("</FrameNameTable>");
                    }
                }
            }
        }

        private void OnNodeSelect(object sender, TreeViewEventArgs e)
        {
            DataGrid.SelectedObject = e.Node.Tag;
        }

        private void DataGrid_OnPropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (e.ChangedItem.Label == "Name")
            {
                DataTreeView.SelectedNode.Text = e.ChangedItem.Value.ToString();
            }

            Text = Language.GetString("$FNT_EDITOR_TITLE") + "*";
            bIsFileEdited = true;

            DataGrid.Refresh();
        }

        private void FrameNameTableEditor_Closing(object sender, FormClosingEventArgs e)
        {
            if (bIsFileEdited)
            {
                System.Windows.MessageBoxResult saveChanges = System.Windows.MessageBox.Show(
                    Language.GetString("$SAVE_PROMPT"),
                    "Toolkit",
                    System.Windows.MessageBoxButton.YesNoCancel);

                if (saveChanges == System.Windows.MessageBoxResult.Yes)
                {
                    Save();
                }
                else if (saveChanges == System.Windows.MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void SaveButton_OnClick(object sender, EventArgs e) => Save();
        private void ReloadButton_OnClick(object sender, EventArgs e) => Reload();
        private void ExitButton_OnClick(object sender, EventArgs e) => Close();
        private void AddEntryButton_Click(object sender, EventArgs e) => AddEntry();
        private void DeleteEntryButton_Click(object sender, EventArgs e) => Delete();
        private void ExportXMLButton_Click(object sender, EventArgs e) => ExportToXML();
    }
}
