using Mafia2;
using System;
using System.Windows.Forms;

namespace Mafia2Tool.OptionControls
{
    public partial class ModelOptions : UserControl
    {
        public ModelOptions()
        {
            InitializeComponent();
            LoadSettings();
        }

        /// <summary>
        /// Read Settings from INI and populate controls.
        /// </summary>
        private void LoadSettings()
        {
            exportPathTextBox.Text = ToolkitSettings.ExportPath;
            modelFormatDropdownBox.SelectedIndex = ToolkitSettings.Format;
        }

        private void ExportPath_TextChanged(object sender, EventArgs e)
        {
            ToolkitSettings.ExportPath = exportPathTextBox.Text;
            ToolkitSettings.WriteKey("ModelExportPath", "Directories", exportPathTextBox.Text);
        }

        private void ExportModelFormat_IndexChanged(object sender, EventArgs e)
        {
            ToolkitSettings.Format = modelFormatDropdownBox.SelectedIndex;
            ToolkitSettings.WriteKey("Format", "Exporting", modelFormatDropdownBox.SelectedIndex.ToString());
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            ExportPathButton.SelectedPath = "";
            if (ExportPathButton.ShowDialog() == DialogResult.OK)
            {
                exportPathTextBox.Text = ExportPathButton.SelectedPath;
                ExportPath_TextChanged(null, null);
            }
            else
                return;
        }
    }
}
