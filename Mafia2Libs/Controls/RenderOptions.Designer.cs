namespace Forms.OptionControls
{
    partial class RenderOptions
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
            RenderGroup = new System.Windows.Forms.GroupBox();
            Checkbox_EnableNavigation = new System.Windows.Forms.CheckBox();
            Checkbox_EnableTranslokatorTint = new System.Windows.Forms.CheckBox();
            CheckBox_VSync = new System.Windows.Forms.CheckBox();
            FieldOfViewNumDown = new System.Windows.Forms.NumericUpDown();
            RenderFieldOfView = new System.Windows.Forms.Label();
            UseMIPsBox = new System.Windows.Forms.CheckBox();
            ExperimentalBox = new System.Windows.Forms.CheckBox();
            BrowseButton = new System.Windows.Forms.Button();
            TexDirectoryBox = new System.Windows.Forms.TextBox();
            TexLabel = new System.Windows.Forms.Label();
            CameraSpeedUpDown = new System.Windows.Forms.NumericUpDown();
            CameraSpeedLabel = new System.Windows.Forms.Label();
            ScreenFarUpDown = new System.Windows.Forms.NumericUpDown();
            ScreenNearUpDown = new System.Windows.Forms.NumericUpDown();
            ScreenFarLabel = new System.Windows.Forms.Label();
            ScreenNearLabel = new System.Windows.Forms.Label();
            TexBrowser = new System.Windows.Forms.FolderBrowserDialog();
            RenderGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)FieldOfViewNumDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)CameraSpeedUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ScreenFarUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ScreenNearUpDown).BeginInit();
            SuspendLayout();
            // 
            // RenderGroup
            // 
            RenderGroup.Controls.Add(Checkbox_EnableNavigation);
            RenderGroup.Controls.Add(Checkbox_EnableTranslokatorTint);
            RenderGroup.Controls.Add(CheckBox_VSync);
            RenderGroup.Controls.Add(FieldOfViewNumDown);
            RenderGroup.Controls.Add(RenderFieldOfView);
            RenderGroup.Controls.Add(UseMIPsBox);
            RenderGroup.Controls.Add(ExperimentalBox);
            RenderGroup.Controls.Add(BrowseButton);
            RenderGroup.Controls.Add(TexDirectoryBox);
            RenderGroup.Controls.Add(TexLabel);
            RenderGroup.Controls.Add(CameraSpeedUpDown);
            RenderGroup.Controls.Add(CameraSpeedLabel);
            RenderGroup.Controls.Add(ScreenFarUpDown);
            RenderGroup.Controls.Add(ScreenNearUpDown);
            RenderGroup.Controls.Add(ScreenFarLabel);
            RenderGroup.Controls.Add(ScreenNearLabel);
            RenderGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            RenderGroup.Location = new System.Drawing.Point(0, 0);
            RenderGroup.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RenderGroup.Name = "RenderGroup";
            RenderGroup.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RenderGroup.Size = new System.Drawing.Size(455, 223);
            RenderGroup.TabIndex = 2;
            RenderGroup.TabStop = false;
            RenderGroup.Text = "$RENDER_OPTIONS";
            // 
            // Checkbox_EnableNavigation
            // 
            Checkbox_EnableNavigation.AutoSize = true;
            Checkbox_EnableNavigation.Location = new System.Drawing.Point(274, 19);
            Checkbox_EnableNavigation.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Checkbox_EnableNavigation.Name = "Checkbox_EnableNavigation";
            Checkbox_EnableNavigation.Size = new System.Drawing.Size(147, 19);
            Checkbox_EnableNavigation.TabIndex = 15;
            Checkbox_EnableNavigation.Text = "$ENABLE_NAVIGATION";
            Checkbox_EnableNavigation.UseVisualStyleBackColor = true;
            Checkbox_EnableNavigation.CheckedChanged += Button_EnableNavigation_CheckedChanged;
            // 
            // Checkbox_EnableTranslokatorTint
            // 
            Checkbox_EnableTranslokatorTint.AutoSize = true;
            Checkbox_EnableTranslokatorTint.Location = new System.Drawing.Point(274, 124);
            Checkbox_EnableTranslokatorTint.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Checkbox_EnableTranslokatorTint.Name = "Checkbox_EnableTranslokatorTint";
            Checkbox_EnableTranslokatorTint.Size = new System.Drawing.Size(147, 19);
            Checkbox_EnableTranslokatorTint.TabIndex = 16;
            Checkbox_EnableTranslokatorTint.Text = "$TOGGLE_TRANSLOKATOR_TINT";
            Checkbox_EnableTranslokatorTint.UseVisualStyleBackColor = true;
            Checkbox_EnableTranslokatorTint.CheckedChanged += Button_EnableTranslokatorTint_CheckedChanged;
            // 
            // CheckBox_VSync
            // 
            CheckBox_VSync.AutoSize = true;
            CheckBox_VSync.Location = new System.Drawing.Point(274, 97);
            CheckBox_VSync.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CheckBox_VSync.Name = "CheckBox_VSync";
            CheckBox_VSync.Size = new System.Drawing.Size(58, 19);
            CheckBox_VSync.TabIndex = 14;
            CheckBox_VSync.Text = "VSync";
            CheckBox_VSync.UseVisualStyleBackColor = true;
            CheckBox_VSync.CheckedChanged += CheckBox_VSync_OnChecked;
            // 
            // FieldOfViewNumDown
            // 
            FieldOfViewNumDown.DecimalPlaces = 1;
            FieldOfViewNumDown.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            FieldOfViewNumDown.Location = new System.Drawing.Point(274, 145);
            FieldOfViewNumDown.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            FieldOfViewNumDown.Maximum = new decimal(new int[] { 120, 0, 0, 0 });
            FieldOfViewNumDown.Minimum = new decimal(new int[] { 40, 0, 0, 0 });
            FieldOfViewNumDown.Name = "FieldOfViewNumDown";
            FieldOfViewNumDown.Size = new System.Drawing.Size(140, 23);
            FieldOfViewNumDown.TabIndex = 13;
            FieldOfViewNumDown.Value = new decimal(new int[] { 120, 0, 0, 0 });
            FieldOfViewNumDown.ValueChanged += FieldOfViewNumDown_ValueChanged;
            // 
            // RenderFieldOfView
            // 
            RenderFieldOfView.AutoSize = true;
            RenderFieldOfView.Location = new System.Drawing.Point(271, 127);
            RenderFieldOfView.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            RenderFieldOfView.Name = "RenderFieldOfView";
            RenderFieldOfView.Size = new System.Drawing.Size(132, 15);
            RenderFieldOfView.TabIndex = 12;
            RenderFieldOfView.Text = "$RENDER_FIELDOFVIEW";
            // 
            // UseMIPsBox
            // 
            UseMIPsBox.AutoSize = true;
            UseMIPsBox.Location = new System.Drawing.Point(274, 70);
            UseMIPsBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            UseMIPsBox.Name = "UseMIPsBox";
            UseMIPsBox.Size = new System.Drawing.Size(74, 19);
            UseMIPsBox.TabIndex = 11;
            UseMIPsBox.Text = "Use MIPs";
            UseMIPsBox.UseVisualStyleBackColor = true;
            UseMIPsBox.CheckedChanged += UseMIPsBox_CheckedChanged;
            // 
            // ExperimentalBox
            // 
            ExperimentalBox.AutoSize = true;
            ExperimentalBox.Location = new System.Drawing.Point(274, 44);
            ExperimentalBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ExperimentalBox.Name = "ExperimentalBox";
            ExperimentalBox.Size = new System.Drawing.Size(95, 19);
            ExperimentalBox.TabIndex = 10;
            ExperimentalBox.Text = "Experimental";
            ExperimentalBox.UseVisualStyleBackColor = true;
            ExperimentalBox.CheckedChanged += ExperimentalBox_CheckedChanged;
            // 
            // BrowseButton
            // 
            BrowseButton.Location = new System.Drawing.Point(402, 193);
            BrowseButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            BrowseButton.Name = "BrowseButton";
            BrowseButton.Size = new System.Drawing.Size(30, 23);
            BrowseButton.TabIndex = 9;
            BrowseButton.Text = "...";
            BrowseButton.UseVisualStyleBackColor = true;
            BrowseButton.Click += BrowseButton_Click;
            // 
            // TexDirectoryBox
            // 
            TexDirectoryBox.Location = new System.Drawing.Point(8, 193);
            TexDirectoryBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TexDirectoryBox.Name = "TexDirectoryBox";
            TexDirectoryBox.Size = new System.Drawing.Size(387, 23);
            TexDirectoryBox.TabIndex = 8;
            TexDirectoryBox.TextChanged += TexDirectoryBox_TextChanged;
            // 
            // TexLabel
            // 
            TexLabel.AutoSize = true;
            TexLabel.Location = new System.Drawing.Point(5, 174);
            TexLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            TexLabel.Name = "TexLabel";
            TexLabel.Size = new System.Drawing.Size(124, 15);
            TexLabel.TabIndex = 7;
            TexLabel.Text = "$TEXTURE_DIRECTORY";
            // 
            // CameraSpeedUpDown
            // 
            CameraSpeedUpDown.DecimalPlaces = 1;
            CameraSpeedUpDown.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            CameraSpeedUpDown.Location = new System.Drawing.Point(8, 145);
            CameraSpeedUpDown.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CameraSpeedUpDown.Maximum = new decimal(new int[] { 500, 0, 0, 0 });
            CameraSpeedUpDown.Name = "CameraSpeedUpDown";
            CameraSpeedUpDown.Size = new System.Drawing.Size(140, 23);
            CameraSpeedUpDown.TabIndex = 6;
            CameraSpeedUpDown.Value = new decimal(new int[] { 1, 0, 0, 0 });
            CameraSpeedUpDown.ValueChanged += CameraSpeedUpDown_Changed;
            // 
            // CameraSpeedLabel
            // 
            CameraSpeedLabel.AutoSize = true;
            CameraSpeedLabel.Location = new System.Drawing.Point(5, 127);
            CameraSpeedLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            CameraSpeedLabel.Name = "CameraSpeedLabel";
            CameraSpeedLabel.Size = new System.Drawing.Size(142, 15);
            CameraSpeedLabel.TabIndex = 5;
            CameraSpeedLabel.Text = "$RENDER_CAMERASPEED";
            // 
            // ScreenFarUpDown
            // 
            ScreenFarUpDown.DecimalPlaces = 2;
            ScreenFarUpDown.Location = new System.Drawing.Point(8, 93);
            ScreenFarUpDown.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ScreenFarUpDown.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            ScreenFarUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            ScreenFarUpDown.Name = "ScreenFarUpDown";
            ScreenFarUpDown.Size = new System.Drawing.Size(140, 23);
            ScreenFarUpDown.TabIndex = 4;
            ScreenFarUpDown.Value = new decimal(new int[] { 1, 0, 0, 0 });
            ScreenFarUpDown.ValueChanged += ScreenDepth_Changed;
            // 
            // ScreenNearUpDown
            // 
            ScreenNearUpDown.DecimalPlaces = 2;
            ScreenNearUpDown.Location = new System.Drawing.Point(8, 43);
            ScreenNearUpDown.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ScreenNearUpDown.Maximum = new decimal(new int[] { 2500, 0, 0, 0 });
            ScreenNearUpDown.Name = "ScreenNearUpDown";
            ScreenNearUpDown.Size = new System.Drawing.Size(140, 23);
            ScreenNearUpDown.TabIndex = 3;
            ScreenNearUpDown.ValueChanged += ScreenNear_Changed;
            // 
            // ScreenFarLabel
            // 
            ScreenFarLabel.AutoSize = true;
            ScreenFarLabel.Location = new System.Drawing.Point(5, 75);
            ScreenFarLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            ScreenFarLabel.Name = "ScreenFarLabel";
            ScreenFarLabel.Size = new System.Drawing.Size(123, 15);
            ScreenFarLabel.TabIndex = 2;
            ScreenFarLabel.Text = "$RENDER_SCREENFAR";
            // 
            // ScreenNearLabel
            // 
            ScreenNearLabel.AutoSize = true;
            ScreenNearLabel.Location = new System.Drawing.Point(5, 24);
            ScreenNearLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            ScreenNearLabel.Name = "ScreenNearLabel";
            ScreenNearLabel.Size = new System.Drawing.Size(133, 15);
            ScreenNearLabel.TabIndex = 0;
            ScreenNearLabel.Text = "$RENDER_SCREENNEAR";
            // 
            // TexBrowser
            // 
            TexBrowser.Description = "$SELECT_TEX_FOLDER";
            // 
            // RenderOptions
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(RenderGroup);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "RenderOptions";
            Size = new System.Drawing.Size(455, 223);
            RenderGroup.ResumeLayout(false);
            RenderGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)FieldOfViewNumDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)CameraSpeedUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)ScreenFarUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)ScreenNearUpDown).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox RenderGroup;
        private System.Windows.Forms.Label ScreenNearLabel;
        private System.Windows.Forms.Label ScreenFarLabel;
        private System.Windows.Forms.NumericUpDown ScreenFarUpDown;
        private System.Windows.Forms.NumericUpDown ScreenNearUpDown;
        private System.Windows.Forms.NumericUpDown CameraSpeedUpDown;
        private System.Windows.Forms.Label CameraSpeedLabel;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.TextBox TexDirectoryBox;
        private System.Windows.Forms.Label TexLabel;
        private System.Windows.Forms.FolderBrowserDialog TexBrowser;
        private System.Windows.Forms.CheckBox ExperimentalBox;
        private System.Windows.Forms.CheckBox UseMIPsBox;
        private System.Windows.Forms.NumericUpDown FieldOfViewNumDown;
        private System.Windows.Forms.Label RenderFieldOfView;
        private System.Windows.Forms.CheckBox CheckBox_VSync;
        private System.Windows.Forms.CheckBox Checkbox_EnableNavigation;
        private System.Windows.Forms.CheckBox Checkbox_EnableTranslokatorTint;
    }
}
