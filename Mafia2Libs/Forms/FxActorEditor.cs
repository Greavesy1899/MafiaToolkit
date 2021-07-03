using System.IO;
using System.Windows.Forms;
using ResourceTypes.OC3.FaceFX;
using Utils.Language;

namespace Toolkit.Forms
{
    public partial class FxActorEditor : Form
    {
        private FileInfo fxActorFile;

        private FxContainer<FxActor> ActorContainer;

        private object clipboard;

        public FxActorEditor(FileInfo InOriginFile)
        {
            InitializeComponent();

            fxActorFile = InOriginFile;
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
            using (BinaryReader Reader = new BinaryReader(File.Open(fxActorFile.FullName, FileMode.Open)))
            {
                ActorContainer = new FxContainer<FxActor>();
                ActorContainer.ReadFromFile(Reader);
            }

            foreach (FxArchive archive in ActorContainer.Archives)
            {
                FxActor actor = archive.GetObjectAs<FxActor>();

                TreeNode actorNode = new TreeNode(actor.Name.ToString());

                actorNode.Tag = actor;

                FxActorTreeView.Nodes.Add(actorNode);
            }


            // TODO: On a succesfull read, we should then propagate the TreeView.
        }

        private void Copy()
        {
            int index = FxActorTreeView.SelectedNode.Index;
            clipboard = ActorContainer.Archives[index];
        }

        private void Paste()
        {
            if (clipboard is FxArchive)
            {
                FxArchive archive = (clipboard as FxArchive);
                ActorContainer.Archives.Add(archive);

                FxActor actor = archive.GetObjectAs<FxActor>();

                TreeNode actorNode = new TreeNode(actor.Name.ToString());

                actorNode.Tag = actor;

                FxActorTreeView.Nodes.Add(actorNode);
            }
        }

        private void FxActorTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Grid_Actors.SelectedObject = e.Node.Tag;
        }

        private void FxActorTreeView_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                Copy();
            }
            else if (e.Control && e.KeyCode == Keys.V)
            {
                Paste();
            }
        }

        private void Button_Save_Click(object sender, System.EventArgs e)
        {
            // TODO: Idea for this is to 'sanitize' - effectively run through the data and update any indexes/offesets etc.
            // Then save to file as usual.

            File.Copy(fxActorFile.FullName, fxActorFile.FullName + "_old", true);
            using (BinaryWriter writer = new BinaryWriter(File.Open(fxActorFile.FullName, FileMode.Create)))
            {
                ActorContainer.WriteToFile(writer);
            }
        }

        private void Button_Reload_Click(object sender, System.EventArgs e)
        {
            // TODO: File should be reloaded from origin file, and editor should be wiped
            Grid_Actors.SelectedObject = null;
            FxActorTreeView.Nodes.Clear();
            BuildData();
        }

        private void Button_Exit_Click(object sender, System.EventArgs e)
        {
            // TODO: Message Box - Do you want to lose unsaved changes?
            Close();
        }

        private void Grid_Actors_Click(object sender, System.EventArgs e)
        {

        }

        private void FxActToolDelete_Click(object sender, System.EventArgs e)
        {
            int index = FxActorTreeView.SelectedNode.Index;

            ActorContainer.Archives.RemoveAt(index);
            FxActorTreeView.Nodes.Remove(FxActorTreeView.SelectedNode);
        }
        private void FxActToolCopy_Click(object sender, System.EventArgs e) => Copy();
        private void FxActToolPaste_Click(object sender, System.EventArgs e) => Paste();

        private void FxActContextDelete_Click(object sender, System.EventArgs e)
        {
            int index = FxActorTreeView.SelectedNode.Index;

            ActorContainer.Archives.RemoveAt(index);
            FxActorTreeView.Nodes.Remove(FxActorTreeView.SelectedNode);
        }
        private void FxActContextCopy_Click(object sender, System.EventArgs e) => Copy();
        private void FxActContextPaste_Click(object sender, System.EventArgs e) => Paste();

        private void FxActContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void TreeView_Actors_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }
    }
}