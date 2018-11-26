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
            groupGeneral.Text = Language.GetString("$GENERAL");
            MafiaIIBrowser.Description = Language.GetString("$SELECT_MII_FOLDER");
        }
    }
}
