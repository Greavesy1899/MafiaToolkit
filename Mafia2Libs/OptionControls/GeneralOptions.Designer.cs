namespace Mafia2Tool.OptionControls
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
            this.browseButton = new System.Windows.Forms.Button();
            this.M2DirectoryBox = new System.Windows.Forms.TextBox();
            this.M2Label = new System.Windows.Forms.Label();
            this.MafiaIIBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBoxSplitter = new System.Windows.Forms.SplitContainer();
            this.groupDiscordRPC = new System.Windows.Forms.GroupBox();
            this.DiscordElapsedCheckBox = new System.Windows.Forms.CheckBox();
            this.DiscordStateCheckBox = new System.Windows.Forms.CheckBox();
            this.DiscordDetailsCheckBox = new System.Windows.Forms.CheckBox();
            this.DiscordEnabledCheckBox = new System.Windows.Forms.CheckBox();
            this.debugLoggingCheckbox = new System.Windows.Forms.CheckBox();
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
            this.groupGeneral.Text = "General Options";
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(345, 37);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(71, 20);
            this.browseButton.TabIndex = 2;
            this.browseButton.Text = "Browse";
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
            this.M2Label.Size = new System.Drawing.Size(90, 13);
            this.M2Label.TabIndex = 0;
            this.M2Label.Text = "Mafia II Directory:";
            // 
            // MafiaIIBrowser
            // 
            this.MafiaIIBrowser.Description = "Select your MafiaII folder. The folder should contain \"launcher.exe\"";
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
            this.groupDiscordRPC.Text = "Discord Rich Presence";
            // 
            // DiscordElapsedCheckBox
            // 
            this.DiscordElapsedCheckBox.AutoSize = true;
            this.DiscordElapsedCheckBox.Location = new System.Drawing.Point(6, 88);
            this.DiscordElapsedCheckBox.Name = "DiscordElapsedCheckBox";
            this.DiscordElapsedCheckBox.Size = new System.Drawing.Size(132, 17);
            this.DiscordElapsedCheckBox.TabIndex = 3;
            this.DiscordElapsedCheckBox.Text = "Elapsed Time Enabled";
            this.DiscordElapsedCheckBox.UseVisualStyleBackColor = true;
            this.DiscordElapsedCheckBox.CheckedChanged += new System.EventHandler(this.DiscordElapsedCheckBox_CheckedChanged);
            // 
            // DiscordStateCheckBox
            // 
            this.DiscordStateCheckBox.AutoSize = true;
            this.DiscordStateCheckBox.Location = new System.Drawing.Point(7, 65);
            this.DiscordStateCheckBox.Name = "DiscordStateCheckBox";
            this.DiscordStateCheckBox.Size = new System.Drawing.Size(93, 17);
            this.DiscordStateCheckBox.TabIndex = 2;
            this.DiscordStateCheckBox.Text = "State Enabled";
            this.DiscordStateCheckBox.UseVisualStyleBackColor = true;
            this.DiscordStateCheckBox.CheckedChanged += new System.EventHandler(this.DiscordStateCheckBox_CheckedChanged);
            // 
            // DiscordDetailsCheckBox
            // 
            this.DiscordDetailsCheckBox.AutoSize = true;
            this.DiscordDetailsCheckBox.Location = new System.Drawing.Point(7, 42);
            this.DiscordDetailsCheckBox.Name = "DiscordDetailsCheckBox";
            this.DiscordDetailsCheckBox.Size = new System.Drawing.Size(100, 17);
            this.DiscordDetailsCheckBox.TabIndex = 1;
            this.DiscordDetailsCheckBox.Text = "Details Enabled";
            this.DiscordDetailsCheckBox.UseVisualStyleBackColor = true;
            this.DiscordDetailsCheckBox.CheckedChanged += new System.EventHandler(this.DiscordDetailsCheckBox_CheckedChanged);
            // 
            // DiscordEnabledCheckBox
            // 
            this.DiscordEnabledCheckBox.AutoSize = true;
            this.DiscordEnabledCheckBox.Location = new System.Drawing.Point(7, 19);
            this.DiscordEnabledCheckBox.Name = "DiscordEnabledCheckBox";
            this.DiscordEnabledCheckBox.Size = new System.Drawing.Size(138, 17);
            this.DiscordEnabledCheckBox.TabIndex = 0;
            this.DiscordEnabledCheckBox.Text = "Rich Presence Enabled";
            this.DiscordEnabledCheckBox.UseVisualStyleBackColor = true;
            this.DiscordEnabledCheckBox.CheckedChanged += new System.EventHandler(this.DiscordEnabledCheckBox_CheckedChanged);
            // 
            // debugLoggingCheckbox
            // 
            this.debugLoggingCheckbox.AutoSize = true;
            this.debugLoggingCheckbox.Location = new System.Drawing.Point(7, 63);
            this.debugLoggingCheckbox.Name = "debugLoggingCheckbox";
            this.debugLoggingCheckbox.Size = new System.Drawing.Size(129, 17);
            this.debugLoggingCheckbox.TabIndex = 4;
            this.debugLoggingCheckbox.Text = "Enable debug logging";
            this.debugLoggingCheckbox.UseVisualStyleBackColor = true;
            this.debugLoggingCheckbox.CheckedChanged += new System.EventHandler(this.DebugLoggingCheckBox_CheckedChanged);
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
    }
}
