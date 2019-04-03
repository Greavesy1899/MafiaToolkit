using System;
using System.Windows.Forms;
using Utils.Lang;
using Utils.Settings;

namespace Mafia2Tool.OptionControls
{
    public partial class SDSOptions : UserControl
    {
        public SDSOptions()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void Localise()
        {
            M2Label.Text = Language.GetString("$SDS_COMPRESSION_TYPE");
        }

        private void LoadSettings()
        {
            CompressionDropdownBox.SelectedIndex = ToolkitSettings.SerializeSDSOption;
        }

        private void ExportModelFormat_IndexChanged(object sender, EventArgs e)
        {
            ToolkitSettings.Format = CompressionDropdownBox.SelectedIndex;
            ToolkitSettings.WriteKey("SerializeOption", "SDS", CompressionDropdownBox.SelectedIndex.ToString());
        }
    }
}
