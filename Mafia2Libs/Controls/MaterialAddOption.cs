using System.Windows.Forms;
using Utils.Language;

namespace Forms.EditorControls
{
    public partial class MaterialAddOption : UserControl
    {
        public MaterialAddOption()
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
