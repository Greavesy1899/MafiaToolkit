using System.Windows.Forms;
using Utils.Language;

namespace Forms.EditorControls
{
    public partial class FrameResourceSceneOption : UserControl
    {
        public FrameResourceSceneOption()
        {
            InitializeComponent();
            Localise();
        }

        private void Localise()
        {
            groupGeneral.Text = Language.GetString("$SCENE_OPTIONS");
            FSceneOffsetLabel.Text = Language.GetString("$FSCENE_OFFSET");
            MafiaIIBrowser.Description = Language.GetString("$SELECT_MII_FOLDER");
        }
    }
}
