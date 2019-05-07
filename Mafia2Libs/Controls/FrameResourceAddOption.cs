using System.Windows.Forms;
using Utils.Lang;

namespace Forms.EditorControls
{
    public partial class FrameResourceAddOption : UserControl
    {
        public FrameResourceAddOption()
        {
            InitializeComponent();
            Localise();
            FraddTypeCombo.SelectedIndex = 0;
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
