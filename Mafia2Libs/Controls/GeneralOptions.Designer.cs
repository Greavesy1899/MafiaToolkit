namespace Forms.OptionControls
{
    partial class GeneralOptions
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupGeneral = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.languageComboBox = new System.Windows.Forms.ComboBox();
            this.debugLoggingCheckbox = new System.Windows.Forms.CheckBox();
            this.browseButton = new System.Windows.Forms.Button();
            this.M2DirectoryBox = new System.Windows.Forms.TextBox();
            this.M2Label = new System.Windows.Forms.Label();
            this.MafiaIIBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBoxSplitter = new System.Windows.Forms.SplitContainer();
            this.groupDiscordRPC = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.DiscordStateTextBox = new System.Windows.Forms.TextBox();
            this.DiscordElapsedCheckBox = new System.Windows.Forms.CheckBox();
            this.DiscordStateCheckBox = new System.Windows.Forms.CheckBox();
            this.DiscordDetailsCheckBox = new System.Windows.Forms.CheckBox();
            this.DiscordEnabledCheckBox = new System.Windows.Forms.CheckBox();
            this.groupGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSplitter)).BeginInit();
            this.groupBoxSplitter.Panel1.SuspendLayout();
            this.groupBoxSplitter.Panel2.SuspendLayout();
            this.groupBoxSplitter.SuspendLayout();
            this.groupDiscordRPC.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupGeneral
            // 
            this.groupGeneral.Controls.Add(this.label1);
            this.groupGeneral.Controls.Add(this.languageComboBox);
            this.groupGeneral.Controls.Add(this.debugLoggingCheckbox);
            this.groupGeneral.Controls.Add(this.browseButton);
            this.groupGeneral.Controls.Add(this.M2DirectoryBox);
            this.groupGeneral.Controls.Add(this.M2Label);
            this.groupGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupGeneral.Location = new System.Drawing.Point(0, 0);
            this.groupGeneral.Name = "groupGeneral";
            this.groupGeneral.Size = new System.Drawing.Size(506, 157);
            this.groupGeneral.TabIndex = 1;
            this.groupGeneral.TabStop = false;
            this.groupGeneral.Text = "$GENERAL";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "$LANGUAGE_OPTION";
            // 
            // languageComboBox
            // 
            this.languageComboBox.FormattingEnabled = true;
            this.languageComboBox.Items.AddRange(new object[] {
            "$LANGUAGE_ENGLISH",
            "$LANGUAGE_RUSSIAN",
            "$LANGUAGE_CZECH",
            "$LANGUAGE_POLISH",
            "French"});
            this.languageComboBox.Location = new System.Drawing.Point(7, 76);
            this.languageComboBox.Name = "languageComboBox";
            this.languageComboBox.Size = new System.Drawing.Size(121, 21);
            this.languageComboBox.TabIndex = 5;
            this.languageComboBox.SelectedIndexChanged += new System.EventHandler(this.IndexChange);
            // 
            // debugLoggingCheckbox
            // 
            this.debugLoggingCheckbox.AutoSize = true;
            this.debugLoggingCheckbox.Location = new System.Drawing.Point(6, 103);
            this.debugLoggingCheckbox.Name = "debugLoggingCheckbox";
            this.debugLoggingCheckbox.Size = new System.Drawing.Size(173, 17);
            this.debugLoggingCheckbox.TabIndex = 4;
            this.debugLoggingCheckbox.Text = "$ENABLE_DEBUG_LOGGING";
            this.debugLoggingCheckbox.UseVisualStyleBackColor = true;
            this.debugLoggingCheckbox.CheckedChanged += new System.EventHandler(this.DebugLoggingCheckBox_CheckedChanged);
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(345, 37);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(26, 20);
            this.browseButton.TabIndex = 2;
            this.browseButton.Text = "...";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // M2DirectoryBox
            // 
            this.M2DirectoryBox.Location = new System.Drawing.Point(7, 37);
            this.M2DirectoryBox.Name = "M2DirectoryBox";
            this.M2DirectoryBox.Size = new System.Drawing.Size(332, 20);
            this.M2DirectoryBox.TabIndex = 1;
            this.M2DirectoryBox.TextChanged += new System.EventHandler(this.M2Directory_TextChanged);
            // 
            // M2Label
            // 
            this.M2Label.AutoSize = true;
            this.M2Label.Location = new System.Drawing.Point(4, 21);
            this.M2Label.Name = "M2Label";
            this.M2Label.Size = new System.Drawing.Size(97, 13);
            this.M2Label.TabIndex = 0;
            this.M2Label.Text = "$MII_DIRECTORY";
            this.M2Label.Click += new System.EventHandler(this.M2Label_Click);
            // 
            // MafiaIIBrowser
            // 
            this.MafiaIIBrowser.Description = "$SELECT_MII_FOLDER";
            // 
            // groupBoxSplitter
            // 
            this.groupBoxSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxSplitter.Location = new System.Drawing.Point(0, 0);
            this.groupBoxSplitter.Name = "groupBoxSplitter";
            this.groupBoxSplitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // groupBoxSplitter.Panel1
            // 
            this.groupBoxSplitter.Panel1.Controls.Add(this.groupGeneral);
            // 
            // groupBoxSplitter.Panel2
            // 
            this.groupBoxSplitter.Panel2.Controls.Add(this.groupDiscordRPC);
            this.groupBoxSplitter.Size = new System.Drawing.Size(506, 310);
            this.groupBoxSplitter.SplitterDistance = 157;
            this.groupBoxSplitter.TabIndex = 2;
            // 
            // groupDiscordRPC
            // 
            this.groupDiscordRPC.Controls.Add(this.label2);
            this.groupDiscordRPC.Controls.Add(this.DiscordStateTextBox);
            this.groupDiscordRPC.Controls.Add(this.DiscordElapsedCheckBox);
            this.groupDiscordRPC.Controls.Add(this.DiscordStateCheckBox);
            this.groupDiscordRPC.Controls.Add(this.DiscordDetailsCheckBox);
            this.groupDiscordRPC.Controls.Add(this.DiscordEnabledCheckBox);
            this.groupDiscordRPC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupDiscordRPC.Location = new System.Drawing.Point(0, 0);
            this.groupDiscordRPC.Name = "groupDiscordRPC";
            this.groupDiscordRPC.Size = new System.Drawing.Size(506, 149);
            this.groupDiscordRPC.TabIndex = 0;
            this.groupDiscordRPC.TabStop = false;
            this.groupDiscordRPC.Text = "$DISCORD_RICH_PRESENCE";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "$DISCORDSTATELABEL";
            // 
            // DiscordStateTextBox
            // 
            this.DiscordStateTextBox.Location = new System.Drawing.Point(6, 123);
            this.DiscordStateTextBox.Name = "DiscordStateTextBox";
            this.DiscordStateTextBox.Size = new System.Drawing.Size(365, 20);
            this.DiscordStateTextBox.TabIndex = 7;
            this.DiscordStateTextBox.TextChanged += new System.EventHandler(this.DiscordStateTextBox_TextChanged);
            // 
            // DiscordElapsedCheckBox
            // 
            this.DiscordElapsedCheckBox.AutoSize = true;
            this.DiscordElapsedCheckBox.Location = new System.Drawing.Point(6, 88);
            this.DiscordElapsedCheckBox.Name = "DiscordElapsedCheckBox";
            this.DiscordElapsedCheckBox.Size = new System.Drawing.Size(218, 17);
            this.DiscordElapsedCheckBox.TabIndex = 3;
            this.DiscordElapsedCheckBox.Text = "$DISCORD_TOGGLE_ELAPSED_TIME";
            this.DiscordElapsedCheckBox.UseVisualStyleBackColor = true;
            this.DiscordElapsedCheckBox.CheckedChanged += new System.EventHandler(this.DiscordElapsedCheckBox_CheckedChanged);
            // 
            // DiscordStateCheckBox
            // 
            this.DiscordStateCheckBox.AutoSize = true;
            this.DiscordStateCheckBox.Location = new System.Drawing.Point(7, 65);
            this.DiscordStateCheckBox.Name = "DiscordStateCheckBox";
            this.DiscordStateCheckBox.Size = new System.Drawing.Size(172, 17);
            this.DiscordStateCheckBox.TabIndex = 2;
            this.DiscordStateCheckBox.Text = "$DISCORD_TOGGLE_STATE";
            this.DiscordStateCheckBox.UseVisualStyleBackColor = true;
            this.DiscordStateCheckBox.CheckedChanged += new System.EventHandler(this.DiscordStateCheckBox_CheckedChanged);
            // 
            // DiscordDetailsCheckBox
            // 
            this.DiscordDetailsCheckBox.AutoSize = true;
            this.DiscordDetailsCheckBox.Location = new System.Drawing.Point(7, 42);
            this.DiscordDetailsCheckBox.Name = "DiscordDetailsCheckBox";
            this.DiscordDetailsCheckBox.Size = new System.Drawing.Size(182, 17);
            this.DiscordDetailsCheckBox.TabIndex = 1;
            this.DiscordDetailsCheckBox.Text = "$DISCORD_TOGGLE_DETAILS";
            this.DiscordDetailsCheckBox.UseVisualStyleBackColor = true;
            this.DiscordDetailsCheckBox.CheckedChanged += new System.EventHandler(this.DiscordDetailsCheckBox_CheckedChanged);
            // 
            // DiscordEnabledCheckBox
            // 
            this.DiscordEnabledCheckBox.AutoSize = true;
            this.DiscordEnabledCheckBox.Location = new System.Drawing.Point(7, 19);
            this.DiscordEnabledCheckBox.Name = "DiscordEnabledCheckBox";
            this.DiscordEnabledCheckBox.Size = new System.Drawing.Size(227, 17);
            this.DiscordEnabledCheckBox.TabIndex = 0;
            this.DiscordEnabledCheckBox.Text = "$DISCORD_TOGGLE_RICH_PRESENCE";
            this.DiscordEnabledCheckBox.UseVisualStyleBackColor = true;
            this.DiscordEnabledCheckBox.CheckedChanged += new System.EventHandler(this.DiscordEnabledCheckBox_CheckedChanged);
            // 
            // GeneralOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxSplitter);
            this.Name = "GeneralOptions";
            this.Size = new System.Drawing.Size(506, 310);
            this.groupGeneral.ResumeLayout(false);
            this.groupGeneral.PerformLayout();
            this.groupBoxSplitter.Panel1.ResumeLayout(false);
            this.groupBoxSplitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxSplitter)).EndInit();
            this.groupBoxSplitter.ResumeLayout(false);
            this.groupDiscordRPC.ResumeLayout(false);
            this.groupDiscordRPC.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupGeneral;
        private System.Windows.Forms.TextBox M2DirectoryBox;
        private System.Windows.Forms.Label M2Label;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.FolderBrowserDialog MafiaIIBrowser;
        private System.Windows.Forms.SplitContainer groupBoxSplitter;
        private System.Windows.Forms.GroupBox groupDiscordRPC;
        private System.Windows.Forms.CheckBox DiscordEnabledCheckBox;
        private System.Windows.Forms.CheckBox DiscordElapsedCheckBox;
        private System.Windows.Forms.CheckBox DiscordStateCheckBox;
        private System.Windows.Forms.CheckBox DiscordDetailsCheckBox;
        private System.Windows.Forms.CheckBox debugLoggingCheckbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox languageComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox DiscordStateTextBox;
    }
}
