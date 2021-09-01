using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using ResourceTypes.Wwise;
using Utils.Language;
using Utils.Settings;
using Forms.EditorControls;
using System.Collections.Generic;

namespace Mafia2Tool
{
    public partial class PCKEditor : Form
    {
        private FileInfo PckFile;
        private PCK pck;

        private bool bIsFileEdited = false;


        public PCKEditor(FileInfo file)
        {
            InitializeComponent();
            Localise();
            PckFile = file;
            BuildData();
            Show();
            ToolkitSettings.UpdateRichPresence("Using the Wwise PCK editor.");
        }

        private void Localise()
        {
            Text = Language.GetString("$PCK_EDITOR_TITLE");
            FileButton.Text = Language.GetString("$FILE");
            SaveButton.Text = Language.GetString("$SAVE");
            ReloadButton.Text = Language.GetString("$RELOAD");
            ExitButton.Text = Language.GetString("$EXIT");
            EditButton.Text = Language.GetString("$EDIT");
            Button_ImportWem.Text = Language.GetString("$IMPORT_WEM");
        }

        private void BuildData()
        {
            pck = new PCK(PckFile.FullName);

            for (int i = 0; i != pck.WemList.Count; i++)
            {
                TreeNode node = new TreeNode(pck.WemList[i].Name);
                node.Name = pck.WemList[i].ID.ToString();
                node.Tag = pck.WemList[i];
                TreeView_Wems.Nodes.Add(node);
            }
        }

        private void Save()
        {
            File.Copy(PckFile.FullName, PckFile.FullName + "_old", true);
            using (BinaryWriter writer = new BinaryWriter(File.Open(PckFile.FullName, FileMode.Create)))
            {
                pck.WriteToFile(writer);
            }

            Text = Language.GetString("$PCK_EDITOR_TITLE");
            bIsFileEdited = false;
        }

        private void Reload()
        {
            TreeView_Wems.Nodes.Clear();
            BuildData();

            WemGrid.SelectedObject = null;
            TreeView_Wems.SelectedNode = null;

            Text = Language.GetString("$PCK_EDITOR_TITLE");
            bIsFileEdited = false;
        }

        private void Delete()
        {
            Wem SelWem = (Wem)TreeView_Wems.SelectedNode.Tag;
            pck.WemList.Remove(SelWem);
            TreeView_Wems.Nodes.Remove(TreeView_Wems.SelectedNode);

            Text = Language.GetString("$PCK_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

        private void Export(OpenFileDialog exportFile, DialogResult exportIds, Wem wem)
        {
            string fullPath = exportFile.FileName;
            string savePath = Path.GetDirectoryName(fullPath);
            string name;

            if (exportIds == DialogResult.Yes)
            {
                name = savePath + "\\" + wem.Name + ".wem";
            }
            else
            {
                name = savePath + "\\" + wem.ID + ".wem";
            }

            using (BinaryWriter bw = new BinaryWriter(new FileStream(name, FileMode.OpenOrCreate)))
            {
                bw.Write(wem.file);
                bw.Close();
            }
        }

        private void OnNodeSelectSelect(object sender, TreeViewEventArgs e)
        {
            WemGrid.SelectedObject = e.Node.Tag;
        }

        private void Button_ImportWem_Click(object sender, System.EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();

            if (PckFile.DirectoryName != null)
            {
                openFile.InitialDirectory = PckFile.DirectoryName;
            }

            openFile.Multiselect = true;
            openFile.Filter = "WWise Wem files (*.wem)|*.wem";

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                foreach (string fileName in openFile.FileNames)
                {
                    bool hasConflict = false;
                    Wem newWem = null;

                    using (BinaryReader br = new BinaryReader(File.Open(openFile.FileName, FileMode.Open)))
                    {
                        newWem = new Wem(openFile.FileName, openFile.FileName, br, 0);
                    }

                    foreach (Wem wem in pck.WemList)
                    {
                        if (wem.ID == newWem.ID) //Check if Wem exists
                        {
                            MessageBox.Show("A Wem with the same ID already exists, it will be skipped.", "Import");
                            hasConflict = true;
                            break;
                        }
                    }

                    if (hasConflict)
                        continue;

                    TreeNode node = new TreeNode(newWem.Name);
                    node.Name = newWem.ID.ToString();
                    node.Tag = newWem;
                    TreeView_Wems.Nodes.Add(node);
                    pck.WemList.Add(newWem);
                }
            }

            Text = Language.GetString("$PCK_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

        private void Button_ExportWem_Click(object sender, System.EventArgs e)
        {
            OpenFileDialog exportFile = new OpenFileDialog();
            exportFile.InitialDirectory = PckFile.DirectoryName;
            exportFile.CheckFileExists = false;
            exportFile.FileName = "Save Here";

            if (exportFile.ShowDialog() == DialogResult.OK)
            {
                DialogResult exportIds = MessageBox.Show("Export with name?", "Export Wem", MessageBoxButtons.YesNo);
                int itemIndex = pck.WemList.IndexOf((Wem)WemGrid.SelectedObject);

                if (itemIndex != -1)
                {
                    Wem wem = pck.WemList[itemIndex];
                    Export(exportFile, exportIds, wem);
                }
            }
        }

        private void Button_ExportAll_Click(object sender, System.EventArgs e)
        {
            OpenFileDialog exportFile = new OpenFileDialog();
            exportFile.InitialDirectory = PckFile.DirectoryName;
            exportFile.CheckFileExists = false;
            exportFile.FileName = "Save Here";

            if (exportFile.ShowDialog() == DialogResult.OK)
            {
                DialogResult exportIds = MessageBox.Show("Export with name?", "Export Wem", MessageBoxButtons.YesNo);
                foreach (Wem wem in pck.WemList)
                {
                    Export(exportFile, exportIds, wem);
                }
            }
        }

        private void PckTreeView_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.D)
            {
                WemGrid.SelectedObject = null;
                TreeView_Wems.SelectedNode = null;
            }
        }

        private void WemGrid_OnPropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (e.ChangedItem.Label == "Name")
                TreeView_Wems.SelectedNode.Text = e.ChangedItem.Value.ToString();

            Text = Language.GetString("$PCK_EDITOR_TITLE") + "*";
            bIsFileEdited = true;

            WemGrid.Refresh();
        }

        private void PckEditor_Closing(object sender, FormClosingEventArgs e)
        {
            if (bIsFileEdited)
            {
                System.Windows.MessageBoxResult SaveChanges = System.Windows.MessageBox.Show(Language.GetString("$SAVE_PROMPT"), "", System.Windows.MessageBoxButton.YesNoCancel);

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

        private void ContextDelete_Click(object sender, System.EventArgs e) => Delete();
        private void Button_DeleteWem_Click(object sender, System.EventArgs e) => Delete();
        private void SaveButton_OnClick(object sender, System.EventArgs e) => Save();
        private void ReloadButton_OnClick(object sender, System.EventArgs e) => Reload();
        private void ExitButton_OnClick(object sender, System.EventArgs e) => Close();
    }
}