using System;
using System.Windows.Forms;
using Utils.Lang;
using Utils.Settings;

namespace Forms.OptionControls
{
    public partial class SDSOptions : UserControl
    {
        public SDSOptions()
        {
            InitializeComponent();
            Localise();
            LoadSettings();
        }

        private void Localise()
        {
            M2Label.Text = Language.GetString("$SDS_COMPRESSION_TYPE");
            CompressionDropdownBox.Items[0] = Language.GetString("$SDS_UNCOMPRESSED");
            CompressionDropdownBox.Items[0] = Language.GetString("$SDS_COMPRESSED");
            CompressionDropdownBox.Items[0] = Language.GetString("$SDS_ONEBLOCK");
        }

        private void LoadSettings()
        {
            CompressionDropdownBox.Enabled = false;
            CompressionDropdownBox.SelectedIndex = ToolkitSettings.SerializeSDSOption;
        }

        private void ExportModelFormat_IndexChanged(object sender, EventArgs e)
        {
            ToolkitSettings.Format = CompressionDropdownBox.SelectedIndex;
            ToolkitSettings.WriteKey("SerializeOption", "SDS", CompressionDropdownBox.SelectedIndex.ToString());
        }
    }
}
