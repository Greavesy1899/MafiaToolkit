using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using ResourceTypes.OC3.FaceFX;
using Utils.Language;
using Utils.Settings;

namespace Toolkit.Forms
{
    public partial class FxAnimSetEditor : Form
    {
        private FileInfo FxAnimSetFile;

        private FxContainer<FxAnimSet> AnimSetContainer;

        private object clipboard;

        private bool bIsFileEdited = false;

        public FxAnimSetEditor(FileInfo InOriginFile)
        {
            InitializeComponent();

            FxAnimSetFile = InOriginFile;
            AnimSetContainer = null;

            Localise();

            BuildData();

            Show();

            ToolkitSettings.UpdateRichPresence("Using the FxAnimSet editor.");
        }

        private void Localise()
        {
            Text = Language.GetString("$FXANIMSET_EDITOR");
            Button_File.Text = Language.GetString("$FILE");
            Button_Save.Text = Language.GetString("$SAVE");
            Button_Reload.Text = Language.GetString("$RELOAD");
            Button_Exit.Text = Language.GetString("$EXIT");
            Button_Copy.Text = Language.GetString("$COPY");
            Button_Paste.Text = Language.GetString("$PASTE");
            Button_Delete.Text = Language.GetString("$DELETE");
            Button_Import.Text = Language.GetString("$IMPORT");
            Button_Export.Text = Language.GetString("$EXPORT");
            Context_Copy.Text = Language.GetString("$COPY");
            Context_Paste.Text = Language.GetString("$PASTE");
            Context_Delete.Text = Language.GetString("$DELETE");
            Context_Export.Text = Language.GetString("$EXPORT");
        }

        private void BuildData()
        {
            using (BinaryReader Reader = new BinaryReader(File.Open(FxAnimSetFile.FullName, FileMode.Open)))
            {
                AnimSetContainer = new FxContainer<FxAnimSet>();
                AnimSetContainer.ReadFromFile(Reader);
            }

            foreach (FxArchive Archive in AnimSetContainer.Archives)
            {
                FxAnimSet AnimSet = Archive.GetObjectAs<FxAnimSet>();

                TreeNode AnimSetNode = new TreeNode(AnimSet.Name.ToString());

                AnimSetNode.Tag = AnimSet;

                TreeView_FxAnimSets.Nodes.Add(AnimSetNode);
            }
        }

        private void Save()
        {
            File.Copy(FxAnimSetFile.FullName, FxAnimSetFile.FullName + "_old", true);
            using (BinaryWriter writer = new BinaryWriter(File.Open(FxAnimSetFile.FullName, FileMode.Create)))
            {
                AnimSetContainer.WriteToFile(writer);
            }

            Text = Language.GetString("$FXANIMSET_EDITOR");
            bIsFileEdited = false;
        }

        private void Reload()
        {
            Text = Language.GetString("$FXANIMSET_EDITOR");
            bIsFileEdited = false;
            Grid_AnimSet.SelectedObject = null;
            TreeView_FxAnimSets.SelectedNode = null;
            TreeView_FxAnimSets.Nodes.Clear();
            BuildData();
        }

        private void Copy()
        {
            int index = TreeView_FxAnimSets.SelectedNode.Index;
            clipboard = AnimSetContainer.Archives[index];
        }

        private void Paste()
        {
            if (clipboard is FxArchive)
            {
                FxArchive Archive = (clipboard as FxArchive);
                AnimSetContainer.Archives.Add(Archive);

                FxAnimSet AnimSet = Archive.GetObjectAs<FxAnimSet>();

                TreeNode AnimSetNode = new TreeNode(AnimSet.Name.ToString());

                AnimSetNode.Tag = AnimSet;

                TreeView_FxAnimSets.Nodes.Add(AnimSetNode);

                Text = Language.GetString("$FXANIMSET_EDITOR") + "*";
                bIsFileEdited = true;
            }
        }

        private void Delete()
        {
            int Index = TreeView_FxAnimSets.SelectedNode.Index;

            AnimSetContainer.Archives.RemoveAt(Index);
            TreeView_FxAnimSets.Nodes.Remove(TreeView_FxAnimSets.SelectedNode);

            Text = Language.GetString("$FXANIMSET_EDITOR") + "*";
            bIsFileEdited = true;
        }

        private void Import()
        {
            Microsoft.Win32.OpenFileDialog ImportSet = new Microsoft.Win32.OpenFileDialog();
            ImportSet.InitialDirectory = FxAnimSetFile.DirectoryName;
            ImportSet.Multiselect = false;
            ImportSet.Filter = "FxAnimSet file (*.fas)|*.fas";

            if (ImportSet.ShowDialog() == true)
            {
                FxContainer<FxAnimSet> ImportAnimSetContainer;

                using (BinaryReader Reader = new BinaryReader(File.Open(ImportSet.FileName, FileMode.Open)))
                {
                    ImportAnimSetContainer = new FxContainer<FxAnimSet>();
                    ImportAnimSetContainer.ReadFromFile(Reader);
                }

                AnimSetContainer.Archives.AddRange(ImportAnimSetContainer.Archives);
                TreeView_FxAnimSets.Nodes.Clear();

                foreach (FxArchive Archive in AnimSetContainer.Archives)
                {
                    FxAnimSet AnimSet = Archive.GetObjectAs<FxAnimSet>();

                    TreeNode AnimSetNode = new TreeNode(AnimSet.Name.ToString());

                    AnimSetNode.Tag = AnimSet;

                    TreeView_FxAnimSets.Nodes.Add(AnimSetNode);
                }

                Text = Language.GetString("$FXANIMSET_EDITOR") + "*";
                bIsFileEdited = true;
            }
        }

        private void Export()
        {
            int index = TreeView_FxAnimSets.SelectedNode.Index;
            FxArchive SelectedArchive = AnimSetContainer.Archives[index];

            FxAnimSet AnimSet = SelectedArchive.GetObjectAs<FxAnimSet>();

            Microsoft.Win32.SaveFileDialog ExportSet = new Microsoft.Win32.SaveFileDialog();
            ExportSet.InitialDirectory = FxAnimSetFile.DirectoryName;
            ExportSet.FileName = AnimSet.Name.ToString();
            ExportSet.Filter = "FxAnimSet file (*.fas)|*.fas";

            if (ExportSet.ShowDialog() == true)
            {
                FxContainer<FxAnimSet> ExportAnimSetContainer = new FxContainer<FxAnimSet>();
                ExportAnimSetContainer.Archives = new List<FxArchive>();
                ExportAnimSetContainer.Archives.Add(SelectedArchive);

                using (BinaryWriter Writer = new BinaryWriter(File.Open(ExportSet.FileName, FileMode.Create)))
                {
                    ExportAnimSetContainer.WriteToFile(Writer);
                }
            }
        }

        private void TreeView_FxAnimSets_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Grid_AnimSet.SelectedObject = e.Node.Tag;
        }

        private void TreeView_FxAnimSets_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                Copy();
            }
            else if (e.Control && e.KeyCode == Keys.V)
            {
                Paste();
            }
            else if (e.Control && e.KeyCode == Keys.Delete)
            {
                Delete();
            }
            else if (e.Control && e.KeyCode == Keys.D)
            {
                Grid_AnimSet.SelectedObject = null;
                TreeView_FxAnimSets.SelectedNode = null;
            }
            else if (e.Control && e.KeyCode == Keys.S)
            {
                Save();
            }
            else if (e.Control && e.KeyCode == Keys.R)
            {
                Reload();
            }
        }

        private void Grid_AnimSet_PropertyChanged(object sender, PropertyValueChangedEventArgs e)
        {
            if (e.ChangedItem.Label == "Name")
                TreeView_FxAnimSets.SelectedNode.Text = e.ChangedItem.Value.ToString();

            Text = Language.GetString("$FXANIMSET_EDITOR") + "*";
            bIsFileEdited = true;
        }

        private void FxAnimSetEditor_Closing(object sender, FormClosingEventArgs e)
        {
            if (bIsFileEdited)
            {
                System.Windows.MessageBoxResult SaveChanges = System.Windows.MessageBox.Show("Save before closing?", "", System.Windows.MessageBoxButton.YesNoCancel);

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

        private void Button_Exit_Click(object sender, System.EventArgs e) => Close();
        private void Button_Save_Click(object sender, System.EventArgs e) => Save();
        private void Button_Reload_Click(object sender, System.EventArgs e) => Reload();
        private void Button_Copy_Click(object sender, System.EventArgs e) => Copy();
        private void Button_Paste_Click(object sender, System.EventArgs e) => Paste();
        private void Button_Delete_Click(object sender, System.EventArgs e) => Delete();
        private void Button_Import_Click(object sender, System.EventArgs e) => Import();
        private void Button_Export_Click(object sender, System.EventArgs e) => Export();
        private void Context_Copy_Click(object sender, System.EventArgs e) => Copy();
        private void Context_Paste_Click(object sender, System.EventArgs e) => Paste();
        private void Context_Delete_Click(object sender, System.EventArgs e) => Delete();
        private void Context_Export_Click(object sender, System.EventArgs e) => Export();
    }
}