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
            Button_ImportWem.Text = Language.GetString("$IMPORT_WEM");
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
                    bnk.WemList.Add(newWem);
                }
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
                TreeView_Wems.SelectedNode.Text = e.ChangedItem.Value.ToString();

            Text = Language.GetString("$BNK_EDITOR_TITLE") + "*";
            bIsFileEdited = true;

            WemGrid.Refresh();
        }

        private void BnkEditor_Closing(object sender, FormClosingEventArgs e)
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