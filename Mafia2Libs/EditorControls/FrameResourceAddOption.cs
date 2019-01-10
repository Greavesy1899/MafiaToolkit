using System.Windows.Forms;
using Mafia2;

namespace Mafia2Tool.EditorControls
{
    public partial class FrameResourceAddOption : UserControl
    {
        public FrameResourceAddOption()
        {
            InitializeComponent();
            Localise();
        }

        private void Localise()
        {
            groupGeneral.Text = Language.GetString("$GENERAL");
            MafiaIIBrowser.Description = Language.GetString("$SELECT_MII_FOLDER");
        }

        public int GetSelectedType()
        {
            return FraddTypeCombo.SelectedIndex;
        }
    }
}
