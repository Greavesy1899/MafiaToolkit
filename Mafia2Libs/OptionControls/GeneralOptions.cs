using System;
using System.Windows.Forms;

namespace Mafia2Tool.OptionControls
{
    public partial class GeneralOptions : UserControl
    {
        public GeneralOptions()
        {
            InitializeComponent();
            LoadSettings();
        }

        /// <summary>
        /// Read Settings from INI and populate controls.
        /// </summary>
        private void LoadSettings()
        {
            M2DirectoryBox.Text = ToolkitSettings.M2Directory;
            DiscordEnabledCheckBox.Checked = ToolkitSettings.DiscordEnabled;
            DiscordDetailsCheckBox.Checked = ToolkitSettings.DiscordDetailsEnabled;
            DiscordStateCheckBox.Checked = ToolkitSettings.DiscordStateEnabled;
            DiscordElapsedCheckBox.Checked = ToolkitSettings.DiscordElapsedTimeEnabled;
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            MafiaIIBrowser.SelectedPath = "";
            if (MafiaIIBrowser.ShowDialog() == DialogResult.OK)
            {
                M2DirectoryBox.Text = MafiaIIBrowser.SelectedPath;
                M2Directory_TextChanged(null, null);
            }
            else
                return;
        }

        private void M2Directory_TextChanged(object sender, EventArgs e)
        {
            ToolkitSettings.M2Directory = M2DirectoryBox.Text;
            ToolkitSettings.WriteKey("MafiaII", "Directories", M2DirectoryBox.Text);
        }

        private void DiscordEnabledCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ToolkitSettings.DiscordEnabled = DiscordEnabledCheckBox.Checked;
            ToolkitSettings.WriteKey("Enabled", "Discord", DiscordEnabledCheckBox.Checked.ToString());
            DiscordDetailsCheckBox.Enabled = DiscordEnabledCheckBox.Checked ? true : false;
            DiscordStateCheckBox.Enabled = DiscordEnabledCheckBox.Checked ? true : false;
            DiscordElapsedCheckBox.Enabled = DiscordEnabledCheckBox.Checked ? true : false;
            ToolkitSettings.UpdateRichPresence();
        }

        private void DiscordDetailsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ToolkitSettings.DiscordDetailsEnabled = DiscordDetailsCheckBox.Checked;
            ToolkitSettings.WriteKey("DetailsEnabled", "Discord", DiscordDetailsCheckBox.Checked.ToString());
            ToolkitSettings.UpdateRichPresence();
        }

        private void DiscordStateCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ToolkitSettings.DiscordStateEnabled = DiscordStateCheckBox.Checked;
            ToolkitSettings.WriteKey("StateEmabled", "Discord", DiscordStateCheckBox.Checked.ToString());
            ToolkitSettings.UpdateRichPresence();
        }

        private void DiscordElapsedCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ToolkitSettings.DiscordElapsedTimeEnabled = DiscordElapsedCheckBox.Checked;
            ToolkitSettings.WriteKey("ElapsedTimeEnabled", "Discord", DiscordElapsedCheckBox.Checked.ToString());
            ToolkitSettings.UpdateRichPresence();
        }
    }
}
