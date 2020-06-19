using System;
using Utils.Language;
using Utils.Models;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Forms.EditorControls
{
    public partial class FrameResourceModelOptions : Form
    {
        private Dictionary<string, bool> options;
        public Dictionary<string, bool> Options {
            get { return options; }
            set { options = value; }
        }

        public FrameResourceModelOptions(VertexFlags flags, int i, bool is32bit)
        {
            InitializeComponent();
            Label_BufferType.Visible = is32bit;
            Init(flags, i);
            Localise();
        }

        private void Localise()
        {
            buttonCancel.Text = Language.GetString("$CANCEL");
            buttonContinue.Text = Language.GetString("$CONTINUE");
            Text = Language.GetString("$MODEL_OPTIONS_TITLE");
            ModelOptionsText.Text = Language.GetString("$MODEL_OPTIONS_TEXT");
            ImportNormalBox.Text = Language.GetString("$IMPORT_NORMAL");
            ImportTangentBox.Text = Language.GetString("$IMPORT_TANGENT");
            ImportDiffuseBox.Text = Language.GetString("$IMPORT_DIFFUSE");
            ImportUV1Box.Text = Language.GetString("$IMPORT_UV1");
            ImportUV2Box.Text = Language.GetString("$IMPORT_UV2");
            ImportAOBox.Text = Language.GetString("$IMPORT_AO");
            ImportColor0Box.Text = Language.GetString("$IMPORT_COLOR0");
            ImportColor1Box.Text = Language.GetString("$IMPORT_COLOR1");
            FlipUVBox.Text = Language.GetString("$FLIP_UV");
        }

        private void Init(VertexFlags flags, int i)
        {
            string text = string.Format("{0} LOD: {1}", Language.GetString("$MODEL_OPTIONS_TEXT"), i.ToString());
            ModelOptionsText.Text = text;

            options = new Dictionary<string, bool>();
            options.Add("NORMALS", false);
            options.Add("TANGENTS", false);
            options.Add("DIFFUSE", false);
            options.Add("UV1", false);
            options.Add("UV2", false);
            options.Add("AO", false);
            options.Add("FLIP_UV", false);
            options.Add("COLOR0", false);
            options.Add("COLOR1", false);

            ImportNormalBox.Enabled = flags.HasFlag(VertexFlags.Normals);
            ImportTangentBox.Enabled = flags.HasFlag(VertexFlags.Tangent);
            ImportDiffuseBox.Enabled = flags.HasFlag(VertexFlags.TexCoords0);
            ImportUV1Box.Enabled = flags.HasFlag(VertexFlags.TexCoords1);
            ImportUV2Box.Enabled = flags.HasFlag(VertexFlags.TexCoords2);
            ImportAOBox.Enabled = flags.HasFlag(VertexFlags.ShadowTexture);
            ImportColor0Box.Enabled = flags.HasFlag(VertexFlags.Color);
            ImportColor1Box.Enabled = flags.HasFlag(VertexFlags.Color1);
            FlipUVBox.Enabled = false;
        }

        public void OnButtonClickContinue(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            options["NORMALS"] = ImportNormalBox.Checked;
            options["TANGENTS"] = ImportTangentBox.Checked;
            options["DIFFUSE"] = ImportDiffuseBox.Checked;
            options["UV1"] = ImportUV1Box.Checked;
            options["UV2"] = ImportUV2Box.Checked;
            options["AO"] = ImportAOBox.Checked;
            options["FLIP_UV"] = FlipUVBox.Checked;
            options["COLOR0"] = ImportColor0Box.Checked;
            options["COLOR1"] = ImportColor1Box.Checked;
            DialogResult = DialogResult.OK;
            Close();
        }

        public void OnButtonClickCancel(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
