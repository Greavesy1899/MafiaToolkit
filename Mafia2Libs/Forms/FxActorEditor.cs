using System.IO;
using System.Windows.Forms;
using ResourceTypes.OC3.FaceFX;
using Utils.Language;

namespace Toolkit.Forms
{
    public partial class FxActorEditor : Form
    {
        private FileInfo OriginFile;

        private FxActorContainer ActorContainer;

        public FxActorEditor(FileInfo InOriginFile)
        {
            InitializeComponent();

            OriginFile = InOriginFile;
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
            using(BinaryReader Reader = new BinaryReader(File.Open(OriginFile.FullName, FileMode.Open)))
            {
                ActorContainer = new FxActorContainer();
                ActorContainer.ReadFromFile(Reader);
            }

            // TODO: On a succesfull read, we should then propagate the TreeView.
        }

        private void TreeView_Actors_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Grid_Actors.SelectedObject = e.Node;
        }

        private void Button_Save_Click(object sender, System.EventArgs e)
        {
            // TODO: Idea for this is to 'sanitize' - effectively run through the data and update any indexes/offesets etc.
            // Then save to file as usual.
        }

        private void Button_Reload_Click(object sender, System.EventArgs e)
        {
            // TODO: File should be reloaded from origin file, and editor should be wiped
        }

        private void Button_Exit_Click(object sender, System.EventArgs e)
        {
            // TODO: Message Box - Do you want to lose unsaved changes?
            Close();
        }
    }
}