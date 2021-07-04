using System.IO;
using System.Windows.Forms;
using ResourceTypes.OC3.FaceFX;
using Utils.Language;

namespace Toolkit.Forms
{
    public partial class Editor_FxActors : Form
    {
        private FileInfo FxActorFile;

        private FxContainer<FxActor> ActorContainer;

        private object clipboard;

        public Editor_FxActors(FileInfo InOriginFile)
        {
            InitializeComponent();

            FxActorFile = InOriginFile;
            ActorContainer = null;

            Localise();

            BuildData();

            Show();
        }

        private void Localise()
        {
            Text = Language.GetString("$FXACTOR_EDITOR");
            Button_File.Text = Language.GetString("$FILE");
            Button_Save.Text = Language.GetString("$SAVE");
            Button_Reload.Text = Language.GetString("$RELOAD");
            Button_Exit.Text = Language.GetString("$EXIT");
        }

        private void BuildData()
        {
            using (BinaryReader Reader = new BinaryReader(File.Open(FxActorFile.FullName, FileMode.Open)))
            {
                ActorContainer = new FxContainer<FxActor>();
                ActorContainer.ReadFromFile(Reader);
            }

            foreach (FxArchive archive in ActorContainer.Archives)
            {
                FxActor actor = archive.GetObjectAs<FxActor>();

                TreeNode actorNode = new TreeNode(actor.Name.ToString());

                actorNode.Tag = actor;

                TreeView_FxActors.Nodes.Add(actorNode);
            }
        }

        private void Save()
        {
            File.Copy(FxActorFile.FullName, FxActorFile.FullName + "_old", true);
            using (BinaryWriter writer = new BinaryWriter(File.Open(FxActorFile.FullName, FileMode.Create)))
            {
                ActorContainer.WriteToFile(writer);
            }
        }

        private void Reload()
        {
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
            }
        }

        private void Delete()
        {
            int Index = TreeView_FxActors.SelectedNode.Index;

            ActorContainer.Archives.RemoveAt(Index);
            TreeView_FxActors.Nodes.Remove(TreeView_FxActors.SelectedNode);
        }

        private void TreeView_FxActors_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Grid_Actors.SelectedObject = e.Node.Tag;
        }

        private void TreeView_FxActors_OnKeyUp(object sender, KeyEventArgs e)
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
                Grid_Actors.SelectedObject = null;
                TreeView_FxActors.SelectedNode = null;
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

        private void Button_Exit_Click(object sender, System.EventArgs e)
        {
            System.Windows.MessageBoxResult SaveChanges = System.Windows.MessageBox.Show("Save before closing?", "", System.Windows.MessageBoxButton.YesNo);

            if (SaveChanges == System.Windows.MessageBoxResult.Yes)
            {
                Save();
            }

            Close();
        }

        private void Button_Save_Click(object sender, System.EventArgs e) => Save();
        private void Button_Reload_Click(object sender, System.EventArgs e) => Reload();
        private void Button_Copy_Click(object sender, System.EventArgs e) => Copy();
        private void Button_Paste_Click(object sender, System.EventArgs e) => Paste();
        private void Button_Delete_Click(object sender, System.EventArgs e) => Delete();
        private void Context_Copy_Click(object sender, System.EventArgs e) => Copy();
        private void Context_Paste_Click(object sender, System.EventArgs e) => Paste();
        private void Context_Delete_Click(object sender, System.EventArgs e) => Delete();
    }
}