using System;
using System.Windows.Forms;

namespace Mafia2Tool
{
    public partial class FrameResourceModelOptions : Form
    {
        public int type = -1;
        public bool[] data = new bool[6];
        public Control control;

        public FrameResourceModelOptions()
        {
            InitializeComponent();
            Localise();
        }

        private void Localise()
        {
            buttonCancel.Text = Language.GetString("$CANCEL");
            buttonContinue.Text = Language.GetString("$CONTINUE");
            Text = Language.GetString("$NEWOBJFORM_TITLE");
        }

        public void OnButtonClickContinue(object sender, EventArgs e)
        {
            type = 1;
            data[0] = ImportNormalBox.Checked;
            data[1] = ImportUV0Box.Checked;
            data[2] = ImportUV1Box.Checked;
            data[3] = ImportUV2Box.Checked;
            data[4] = ImportUV7Box.Checked;
            data[5] = FlipUVBox.Checked;
            Close();
        }

        public void OnButtonClickCancel(object sender, EventArgs e)
        {
            type = -1;
            Close();
        }
    }
}
