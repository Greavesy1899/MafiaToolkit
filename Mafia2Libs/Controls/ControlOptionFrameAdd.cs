using System.Windows.Forms;
using Utils.Language;

namespace Forms.EditorControls
{
    public partial class ControlOptionFrameAdd : UserControl
    {
        public ControlOptionFrameAdd()
        {
            InitializeComponent();
            Localise();
            ComboBox_Type.SelectedIndex = 0;
            CheckBox_AddToNameTable.Checked = false;
        }

        private void Localise()
        {
            Group_General.Text = Language.GetString("$GENERAL");
            Label_Type.Text = Language.GetString("$FRADD_TYPE");
            Label_AddToNameTable.Text = Language.GetString("$FRADD_NAME_TABLE");
        }

        public int GetSelectedType()
        {
            return ComboBox_Type.SelectedIndex;
        }

        public bool GetAddToNameTable()
        {
            return CheckBox_AddToNameTable.Checked;
        }
    }
}
