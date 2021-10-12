using System.Diagnostics;
using System;
using System.IO;
using System.Windows.Forms;
using ResourceTypes.Wwise;
using Utils.Language;
using Utils.Settings;
using Forms.EditorControls;
using System.Collections.Generic;

namespace MafiaToolkit
{
    public partial class BNKEditor : Form
    {
        private FileInfo BnkFile;
        private BNK bnk;

        private bool bIsFileEdited = false;


        public BNKEditor(FileInfo file)
        {
            InitializeComponent();
            Localise();
            BnkFile = file;
            BuildData();
            Show();
            ToolkitSettings.UpdateRichPresence("Using the Wwise BNK editor.");
        }

        private void Localise()
        {
            Text = Language.GetString("$BNK_EDITOR_TITLE");
            FileButton.Text = Language.GetString("$FILE");
            SaveButton.Text = Language.GetString("$SAVE");
            ReloadButton.Text = Language.GetString("$RELOAD");
            ExitButton.Text = Language.GetString("$EXIT");
            EditButton.Text = Language.GetString("$EDIT");
            Button_ReplaceWem.Text = Language.GetString("$REPLACE_WEM");
            Button_ImportWem.Text = Language.GetString("$IMPORT_WEM");
            Button_ExportWem.Text = Language.GetString("$EXPORT_WEM");
            Button_ExportAll.Text = Language.GetString("$EXPORT_ALL_WEMS");
            Button_DeleteWem.Text = Language.GetString("$DELETE_WEM");
            ContextReplace.Text = Language.GetString("$REPLACE_WEM");
            ContextEdit.Text = Language.GetString("$EDIT_HIRC");
            ContextExport.Text = Language.GetString("$EXPORT_WEM");
            ContextDelete.Text = Language.GetString("$DELETE_WEM");
            Checkbox_Trim.Text = Language.GetString("$TRIM_WEMS");
        }

        private void BuildData()
        {
            bnk = new BNK(BnkFile.FullName);

            for (int i = 0; i != bnk.WemList.Count; i++)
            {
                TreeNode node = new TreeNode(bnk.WemList[i].Name);
                node.Name = bnk.WemList[i].ID.ToString();
                node.Tag = bnk.WemList[i];
                TreeView_Wems.Nodes.Add(node);
            }
        }

        private void Save()
        {
            File.Copy(BnkFile.FullName, BnkFile.FullName + "_old", true);
            using (BinaryWriter writer = new BinaryWriter(File.Open(BnkFile.FullName, FileMode.Create)))
            {
                bnk.WriteToFile(writer);
            }

            Text = Language.GetString("$BNK_EDITOR_TITLE");
            bIsFileEdited = false;
        }

        private void Reload()
        {
            TreeView_Wems.Nodes.Clear();
            BuildData();

            WemGrid.SelectedObject = null;
            TreeView_Wems.SelectedNode = null;

            Text = Language.GetString("$BNK_EDITOR_TITLE");
            bIsFileEdited = false;
        }

        private void Delete()
        {
            Wem SelWem = (Wem)TreeView_Wems.SelectedNode.Tag;
            bnk.WemList.Remove(SelWem);
            TreeView_Wems.Nodes.Remove(TreeView_Wems.SelectedNode);

            Text = Language.GetString("$BNK_EDITOR_TITLE") + "*";
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
                bw.Write(wem.File);
            }
        }

        private void OnNodeSelectSelect(object sender, TreeViewEventArgs e)
        {
            WemGrid.SelectedObject = e.Node.Tag;
        }

        private void Button_ImportWem_Click(object sender, System.EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();

            if (BnkFile.DirectoryName != null)
            {
                openFile.InitialDirectory = BnkFile.DirectoryName;
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

                    foreach (Wem wem in bnk.WemList)
                    {
                        if (wem.ID == newWem.ID) //Check if Wem exists
                        {
                            MessageBox.Show(Language.GetString("$WEM_EXIST_SKIP"), "Toolkit");
                            hasConflict = true;
                            break;
                        }
                    }

                    if (hasConflict)
                    {
                        continue;
                    }

                    TreeNode node = new TreeNode(newWem.Name);
                    node.Name = newWem.ID.ToString();
                    node.Tag = newWem;
                    TreeView_Wems.Nodes.Add(node);
                    bnk.WemList.Add(newWem);

                    Text = Language.GetString("$BNK_EDITOR_TITLE") + "*";
                    bIsFileEdited = true;
                }
            }
        }

        private void Button_ReplaceWem_Click(object sender, System.EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();

            if (BnkFile.DirectoryName != null)
            {
                openFile.InitialDirectory = BnkFile.DirectoryName;
            }

            openFile.Multiselect = true;
            openFile.Filter = "WWise Wem files (*.wem)|*.wem";

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                foreach (string fileName in openFile.FileNames)
                {
                    if (bnk != null)
                    {
                        int itemIndex = bnk.WemList.IndexOf((Wem)WemGrid.SelectedObject);
                        Wem selWem = bnk.WemList[itemIndex];
                        Wem newWem;

                        using (BinaryReader br = new BinaryReader(File.Open(fileName, FileMode.Open)))
                        {
                            newWem = new Wem(fileName, "", br, selWem.Offset);
                        }

                        if (Checkbox_Trim.Checked)
                        {
                            byte[] tempArray = newWem.File;
                            Array.Resize(ref tempArray, selWem.File.Length);
                            newWem.File = tempArray;
                        }

                        newWem.ID = selWem.ID;
                        newWem.LanguageEnum = selWem.LanguageEnum;

                        if (!(selWem.Name == ("Imported_Wem_" + itemIndex)))
                        {
                            newWem.Name = selWem.Name;
                        }

                        bnk.WemList[itemIndex] = newWem;
                        TreeView_Wems.SelectedNode.Name = newWem.ID.ToString();
                        TreeView_Wems.SelectedNode.Tag = newWem;
                        bnk.Wems.Data = new List<DIDXChunk>();

                        Text = Language.GetString("$BNK_EDITOR_TITLE") + "*";
                        bIsFileEdited = true;
                    }
                }
            }
        }

        private void Button_ExportWem_Click(object sender, System.EventArgs e)
        {
            OpenFileDialog exportFile = new OpenFileDialog();
            exportFile.InitialDirectory = BnkFile.DirectoryName;
            exportFile.CheckFileExists = false;
            exportFile.FileName = "Save Here";

            if (exportFile.ShowDialog() == DialogResult.OK)
            {
                DialogResult exportIds = MessageBox.Show(Language.GetString("$EXPORT_WEM_WITH_NAME"), "Toolkit", MessageBoxButtons.YesNo);
                int itemIndex = bnk.WemList.IndexOf((Wem)WemGrid.SelectedObject);

                if (itemIndex != -1)
                {
                    Wem wem = bnk.WemList[itemIndex];
                    Export(exportFile, exportIds, wem);
                }
            }
        }

        private void Button_ExportAll_Click(object sender, System.EventArgs e)
        {
            OpenFileDialog exportFile = new OpenFileDialog();
            exportFile.InitialDirectory = BnkFile.DirectoryName;
            exportFile.CheckFileExists = false;
            exportFile.FileName = "Save Here";

            if (exportFile.ShowDialog() == DialogResult.OK)
            {
                DialogResult exportIds = MessageBox.Show(Language.GetString("$EXPORT_WEM_WITH_NAME"), "Toolkit", MessageBoxButtons.YesNo);
                foreach (Wem wem in bnk.WemList)
                {
                    Export(exportFile, exportIds, wem);
                }
            }
        }

        private void ContextEdit_Click(object sender, System.EventArgs e)
        {
            int itemIndex = bnk.WemList.IndexOf((Wem)WemGrid.SelectedObject);

            if (itemIndex != -1)
            {
                HIRCEditor HircEditor = new HIRCEditor(bnk.Objects, bnk.WemList[itemIndex]);
                HircEditor.Show();
            }

            Text = Language.GetString("$BNK_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

        private void BnkTreeView_OnKeyUp(object sender, KeyEventArgs e)
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
            {
                TreeView_Wems.SelectedNode.Text = e.ChangedItem.Value.ToString();
            }

            Text = Language.GetString("$BNK_EDITOR_TITLE") + "*";
            bIsFileEdited = true;

            WemGrid.Refresh();
        }

        private void BnkEditor_Closing(object sender, FormClosingEventArgs e)
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

        private void ContextDelete_Click(object sender, System.EventArgs e) => Delete();
        private void Button_DeleteWem_Click(object sender, System.EventArgs e) => Delete();
        private void SaveButton_OnClick(object sender, System.EventArgs e) => Save();
        private void ReloadButton_OnClick(object sender, System.EventArgs e) => Reload();
        private void ExitButton_OnClick(object sender, System.EventArgs e) => Close();
    }
}