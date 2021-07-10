using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using ResourceTypes.OC3.FaceFX;
using Utils.Language;
using Utils.Settings;

namespace Toolkit.Forms
{
    public partial class FxActorEditor : Form
    {
        private FileInfo FxActorFile;

        private FxContainer<FxActor> ActorContainer;

        private object clipboard;

        private bool bIsFileEdited = false;

        public FxActorEditor(FileInfo InOriginFile)
        {
            InitializeComponent();

            FxActorFile = InOriginFile;
            ActorContainer = null;

            Localise();

            BuildData();

            Show();

            ToolkitSettings.UpdateRichPresence("Using the FxActor editor.");
        }

        private void Localise()
        {
            Text = Language.GetString("$FXACTOR_EDITOR");
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
            using (BinaryReader Reader = new BinaryReader(File.Open(FxActorFile.FullName, FileMode.Open)))
            {
                ActorContainer = new FxContainer<FxActor>();
                ActorContainer.ReadFromFile(Reader);
            }

            foreach (FxArchive Archive in ActorContainer.Archives)
            {
                FxActor Actor = Archive.GetObjectAs<FxActor>();

                TreeNode ActorNode = new TreeNode(Actor.Name.ToString());

                ActorNode.Tag = Actor;

                TreeView_FxActors.Nodes.Add(ActorNode);
            }
        }

        private void Save()
        {
            File.Copy(FxActorFile.FullName, FxActorFile.FullName + "_old", true);
            using (BinaryWriter writer = new BinaryWriter(File.Open(FxActorFile.FullName, FileMode.Create)))
            {
                ActorContainer.WriteToFile(writer);
            }

            Text = Language.GetString("$FXACTOR_EDITOR");
            bIsFileEdited = false;
        }

        private void Reload()
        {
            Text = Language.GetString("$FXACTOR_EDITOR");
            bIsFileEdited = false;

            Grid_Actors.SelectedObject = null;
            TreeView_FxActors.SelectedNode = null;
            TreeView_FxActors.Nodes.Clear();
            BuildData();
        }

        private void Copy()
        {
            int index = TreeView_FxActors.SelectedNode.Index;
            clipboard = ActorContainer.Archives[index];
        }

        private void Paste()
        {
            if (clipboard is FxArchive)
            {
                FxArchive Archive = (clipboard as FxArchive);
                ActorContainer.Archives.Add(Archive);

                FxActor Actor = Archive.GetObjectAs<FxActor>();

                TreeNode ActorNode = new TreeNode(Actor.Name.ToString());

                ActorNode.Tag = Actor;

                TreeView_FxActors.Nodes.Add(ActorNode);

                Text = Language.GetString("$FXACTOR_EDITOR") + "*";
                bIsFileEdited = true;
            }
        }

        private void Delete()
        {
            int Index = TreeView_FxActors.SelectedNode.Index;

            ActorContainer.Archives.RemoveAt(Index);
            TreeView_FxActors.Nodes.Remove(TreeView_FxActors.SelectedNode);

            Text = Language.GetString("$FXACTOR_EDITOR") + "*";
            bIsFileEdited = true;
        }

        private void Import()
        {
            Microsoft.Win32.OpenFileDialog ImportActor = new Microsoft.Win32.OpenFileDialog();
            ImportActor.InitialDirectory = FxActorFile.DirectoryName;
            ImportActor.Multiselect = false;
            ImportActor.Filter = "FxActor file (*.fxa)|*.fxa";

            if (ImportActor.ShowDialog() == true)
            {
                FxContainer<FxActor> ImportActorContainer;

                using (BinaryReader Reader = new BinaryReader(File.Open(ImportActor.FileName, FileMode.Open)))
                {
                    ImportActorContainer = new FxContainer<FxActor>();
                    ImportActorContainer.ReadFromFile(Reader);
                }

                ActorContainer.Archives.AddRange(ImportActorContainer.Archives);
                TreeView_FxActors.Nodes.Clear();

                foreach (FxArchive Archive in ActorContainer.Archives)
                {
                    FxActor Actor = Archive.GetObjectAs<FxActor>();

                    TreeNode ActorNode = new TreeNode(Actor.Name.ToString());

                    ActorNode.Tag = Actor;

                    TreeView_FxActors.Nodes.Add(ActorNode);
                }

                Text = Language.GetString("$FXACTOR_EDITOR") + "*";
                bIsFileEdited = true;
            }
        }

        private void Export()
        {
            int index = TreeView_FxActors.SelectedNode.Index;
            FxArchive SelectedArchive = ActorContainer.Archives[index];

            FxActor Actor = SelectedArchive.GetObjectAs<FxActor>();

            Microsoft.Win32.SaveFileDialog ExportActor = new Microsoft.Win32.SaveFileDialog();
            ExportActor.InitialDirectory = FxActorFile.DirectoryName;
            ExportActor.FileName = Actor.Name.ToString();
            ExportActor.Filter = "FxActor file (*.fxa)|*.fxa";

            if (ExportActor.ShowDialog() == true)
            {
                FxContainer<FxActor> ExportActorContainer = new FxContainer<FxActor>();
                ExportActorContainer.Archives = new List<FxArchive>();
                ExportActorContainer.Archives.Add(SelectedArchive);

                using (BinaryWriter Writer = new BinaryWriter(File.Open(ExportActor.FileName, FileMode.Create)))
                {
                    ExportActorContainer.WriteToFile(Writer);
                }
            }
        }

        private void TreeView_FxActors_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Grid_Actors.SelectedObject = e.Node.Tag;
        }

        private void FxActorEditor_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.D)
            {
                Grid_Actors.SelectedObject = null;
                TreeView_FxActors.SelectedNode = null;
            }
        }

        private void Grid_Actors_PropertyChanged(object sender, PropertyValueChangedEventArgs e)
        {
            if (e.ChangedItem.Label == "Name")
                TreeView_FxActors.SelectedNode.Text = e.ChangedItem.Value.ToString();

            Text = Language.GetString("$FXACTOR_EDITOR") + "*";
            bIsFileEdited = true;
        }

        private void FxActorEditor_Closing(object sender, FormClosingEventArgs e)
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