using System.Windows.Forms;
using Mafia2;

namespace Mafia2Tool.EditorControls
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

        public Vector3 GetOffset()
        {
            float x = 0.0f;
            float y = 0.0f;
            float z = 0.0f;

            float.TryParse(OffsetXBox.Text, out x);
            float.TryParse(OffsetYBox.Text, out y);
            float.TryParse(OffsetZBox.Text, out z);

            return new Vector3(x, y, z);
        }
    }
}
