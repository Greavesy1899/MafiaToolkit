using System;
using System.Windows.Forms;
using Utils.Language;
using Utils.Logging;
using Utils.Settings;

namespace Forms.OptionControls
{
    public partial class GeneralOptions : UserControl
    {
        public GeneralOptions()
        {
            InitializeComponent();
            Localise();
            LoadSettings();
        }

        private void Localise()
        {
            groupGeneral.Text = Language.GetString("$GENERAL");
            debugLoggingCheckbox.Text = Language.GetString("$ENABLE_DEBUG_LOGGING");
            CheckForUpdatesBox.Text = Language.GetString("$CHECK_FOR_UPDATES");
            M2Label.Text = Language.GetString("$MII_DIRECTORY");
            MafiaIIBrowser.Description = Language.GetString("$SELECT_MII_FOLDER");
            groupDiscordRPC.Text = Language.GetString("$DISCORD_RICH_PRESENCE");
            DiscordElapsedCheckBox.Text = Language.GetString("$DISCORD_TOGGLE_ELAPSED_TIME");
            DiscordStateCheckBox.Text = Language.GetString("$DISCORD_TOGGLE_STATE");
            DiscordDetailsCheckBox.Text = Language.GetString("$DISCORD_TOGGLE_DETAILS");
            DiscordEnabledCheckBox.Text = Language.GetString("$DISCORD_TOGGLE_RICH_PRESENCE");
            languageComboBox.Items[0] = Language.GetString("$LANGUAGE_ENGLISH");
            languageComboBox.Items[1] = Language.GetString("$LANGUAGE_RUSSIAN");
            languageComboBox.Items[2] = Language.GetString("$LANGUAGE_CZECH");
            languageComboBox.Items[3] = Language.GetString("$LANGUAGE_POLISH");
            languageComboBox.Items[5] = Language.GetString("$LANGUAGE_SLOVAK");
            languageComboBox.Items[6] = Language.GetString("$LANGUAGE_ARABIC");
            label1.Text = Language.GetString("$LANGUAGE_OPTION");
            label2.Text = Language.GetString("$DISCORDSTATELABEL");
        }

        /// <summary>
        /// Read Settings from INI and populate controls.
        /// </summary>
        private void LoadSettings()
        {
            M2DirectoryBox.Text = GameStorage.Instance.GetSelectedGame().Directory;
            DiscordStateTextBox.Text = ToolkitSettings.CustomStateText;
            DiscordEnabledCheckBox.Checked = ToolkitSettings.DiscordEnabled;
            DiscordDetailsCheckBox.Checked = ToolkitSettings.DiscordDetailsEnabled;
            DiscordStateCheckBox.Checked = ToolkitSettings.DiscordStateEnabled;
            DiscordElapsedCheckBox.Checked = ToolkitSettings.DiscordElapsedTimeEnabled;
            debugLoggingCheckbox.Checked = ToolkitSettings.LoggingEnabled;
            languageComboBox.SelectedIndex = ToolkitSettings.Language;

            //handle discord area
            DiscordDetailsCheckBox.Enabled = DiscordEnabledCheckBox.Checked ? true : false;
            DiscordStateCheckBox.Enabled = DiscordEnabledCheckBox.Checked ? true : false;
            DiscordElapsedCheckBox.Enabled = DiscordEnabledCheckBox.Checked ? true : false;
            DiscordStateTextBox.Enabled = DiscordEnabledCheckBox.Checked ? true : false;
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            MafiaIIBrowser.SelectedPath = "";
            if (MafiaIIBrowser.ShowDialog() == DialogResult.OK)
            {
                M2DirectoryBox.Text = MafiaIIBrowser.SelectedPath;
                M2Directory_TextChanged(null, null);
            }
            else return;
        }

        private void M2Directory_TextChanged(object sender, EventArgs e)
        {
            GameStorage.Instance.GetSelectedGame().Directory = M2DirectoryBox.Text;
        }

        private void DiscordEnabledCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ToolkitSettings.DiscordEnabled = DiscordEnabledCheckBox.Checked;
            ToolkitSettings.WriteKey("Enabled", "Discord", DiscordEnabledCheckBox.Checked.ToString());
            DiscordDetailsCheckBox.Enabled = DiscordEnabledCheckBox.Checked ? true : false;
            DiscordStateCheckBox.Enabled = DiscordEnabledCheckBox.Checked ? true : false;
            DiscordElapsedCheckBox.Enabled = DiscordEnabledCheckBox.Checked ? true : false;
            DiscordStateTextBox.Enabled = DiscordEnabledCheckBox.Checked ? true : false;
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

        private void DebugLoggingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ToolkitSettings.LoggingEnabled = debugLoggingCheckbox.Checked;
            ToolkitSettings.WriteKey("Logging", "Misc", debugLoggingCheckbox.Checked.ToString());
            Log.LoggingEnabled = debugLoggingCheckbox.Checked;
        }

        private void IndexChange(object sender, EventArgs e)
        {
            ToolkitSettings.Language = languageComboBox.SelectedIndex;
            ToolkitSettings.WriteKey("Language", "Misc", ToolkitSettings.Language.ToString());
            Language.ReadLanguageXML();
        }

        private void DiscordStateTextBox_TextChanged(object sender, EventArgs e)
        {
            ToolkitSettings.CustomStateText = DiscordStateTextBox.Text;
            ToolkitSettings.WriteKey("CustomStateText", "Discord", DiscordStateTextBox.Text);
        }

        private void CheckForUpdatesBoxChanged(object sender, EventArgs e)
        {
            ToolkitSettings.CheckForUpdates = CheckForUpdatesBox.Checked;
            ToolkitSettings.WriteKey("CheckForUpdates", "Misc", CheckForUpdatesBox.Checked.ToString());
        }
    }
}
