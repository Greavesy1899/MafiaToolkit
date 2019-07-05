using System;
using Utils.Lang;
using Utils.Models;
using System.Windows.Forms;

namespace Mafia2Tool
{
    public partial class FrameResourceModelOptions : Form
    {
        public bool[] data = new bool[6];
        public Control control;

        public FrameResourceModelOptions(VertexFlags flags)
        {
            InitializeComponent();
            Init(flags);
            Localise();
        }

        private void Localise()
        {
            buttonCancel.Text = Language.GetString("$CANCEL");
            buttonContinue.Text = Language.GetString("$CONTINUE");
            Text = Language.GetString("$NEWOBJFORM_TITLE");
        }

        private void Init(VertexFlags flags)
        {
            ImportNormalBox.Enabled = flags.HasFlag(VertexFlags.Normals);
            ImportUV0Box.Enabled = flags.HasFlag(VertexFlags.TexCoords0);
            ImportUV1Box.Enabled = flags.HasFlag(VertexFlags.TexCoords1);
            ImportUV2Box.Enabled = flags.HasFlag(VertexFlags.TexCoords2);
            ImportUV7Box.Enabled = flags.HasFlag(VertexFlags.ShadowTexture);
            FlipUVBox.Enabled = false;
        }

        public void OnButtonClickContinue(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
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
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
