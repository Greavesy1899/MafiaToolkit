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
            this.RenderGroup = new System.Windows.Forms.GroupBox();
            this.FieldOfViewNumDown = new System.Windows.Forms.NumericUpDown();
            this.RenderFieldOfView = new System.Windows.Forms.Label();
            this.UseMIPsBox = new System.Windows.Forms.CheckBox();
            this.ExperimentalBox = new System.Windows.Forms.CheckBox();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.TexDirectoryBox = new System.Windows.Forms.TextBox();
            this.TexLabel = new System.Windows.Forms.Label();
            this.CameraSpeedUpDown = new System.Windows.Forms.NumericUpDown();
            this.CameraSpeedLabel = new System.Windows.Forms.Label();
            this.ScreenFarUpDown = new System.Windows.Forms.NumericUpDown();
            this.ScreenNearUpDown = new System.Windows.Forms.NumericUpDown();
            this.ScreenFarLabel = new System.Windows.Forms.Label();
            this.ScreenNearLabel = new System.Windows.Forms.Label();
            this.TexBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.CheckBox_VSync = new System.Windows.Forms.CheckBox();
            this.RenderGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FieldOfViewNumDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CameraSpeedUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScreenFarUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScreenNearUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // RenderGroup
            // 
            this.RenderGroup.Controls.Add(this.CheckBox_VSync);
            this.RenderGroup.Controls.Add(this.FieldOfViewNumDown);
            this.RenderGroup.Controls.Add(this.RenderFieldOfView);
            this.RenderGroup.Controls.Add(this.UseMIPsBox);
            this.RenderGroup.Controls.Add(this.ExperimentalBox);
            this.RenderGroup.Controls.Add(this.BrowseButton);
            this.RenderGroup.Controls.Add(this.TexDirectoryBox);
            this.RenderGroup.Controls.Add(this.TexLabel);
            this.RenderGroup.Controls.Add(this.CameraSpeedUpDown);
            this.RenderGroup.Controls.Add(this.CameraSpeedLabel);
            this.RenderGroup.Controls.Add(this.ScreenFarUpDown);
            this.RenderGroup.Controls.Add(this.ScreenNearUpDown);
            this.RenderGroup.Controls.Add(this.ScreenFarLabel);
            this.RenderGroup.Controls.Add(this.ScreenNearLabel);
            this.RenderGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RenderGroup.Location = new System.Drawing.Point(0, 0);
            this.RenderGroup.Name = "RenderGroup";
            this.RenderGroup.Size = new System.Drawing.Size(390, 193);
            this.RenderGroup.TabIndex = 2;
            this.RenderGroup.TabStop = false;
            this.RenderGroup.Text = "$RENDER_OPTIONS";
            // 
            // FieldOfViewNumDown
            // 
            this.FieldOfViewNumDown.DecimalPlaces = 1;
            this.FieldOfViewNumDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.FieldOfViewNumDown.Location = new System.Drawing.Point(235, 126);
            this.FieldOfViewNumDown.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.FieldOfViewNumDown.Minimum = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.FieldOfViewNumDown.Name = "FieldOfViewNumDown";
            this.FieldOfViewNumDown.Size = new System.Drawing.Size(120, 20);
            this.FieldOfViewNumDown.TabIndex = 13;
            this.FieldOfViewNumDown.Value = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.FieldOfViewNumDown.ValueChanged += new System.EventHandler(this.FieldOfViewNumDown_ValueChanged);
            // 
            // RenderFieldOfView
            // 
            this.RenderFieldOfView.AutoSize = true;
            this.RenderFieldOfView.Location = new System.Drawing.Point(232, 110);
            this.RenderFieldOfView.Name = "RenderFieldOfView";
            this.RenderFieldOfView.Size = new System.Drawing.Size(137, 13);
            this.RenderFieldOfView.TabIndex = 12;
            this.RenderFieldOfView.Text = "$RENDER_FIELDOFVIEW";
            // 
            // UseMIPsBox
            // 
            this.UseMIPsBox.AutoSize = true;
            this.UseMIPsBox.Location = new System.Drawing.Point(235, 61);
            this.UseMIPsBox.Name = "UseMIPsBox";
            this.UseMIPsBox.Size = new System.Drawing.Size(72, 17);
            this.UseMIPsBox.TabIndex = 11;
            this.UseMIPsBox.Text = "Use MIPs";
            this.UseMIPsBox.UseVisualStyleBackColor = true;
            this.UseMIPsBox.CheckedChanged += new System.EventHandler(this.UseMIPsBox_CheckedChanged);
            // 
            // ExperimentalBox
            // 
            this.ExperimentalBox.AutoSize = true;
            this.ExperimentalBox.Location = new System.Drawing.Point(235, 38);
            this.ExperimentalBox.Name = "ExperimentalBox";
            this.ExperimentalBox.Size = new System.Drawing.Size(86, 17);
            this.ExperimentalBox.TabIndex = 10;
            this.ExperimentalBox.Text = "Experimental";
            this.ExperimentalBox.UseVisualStyleBackColor = true;
            this.ExperimentalBox.CheckedChanged += new System.EventHandler(this.ExperimentalBox_CheckedChanged);
            // 
            // BrowseButton
            // 
            this.BrowseButton.Location = new System.Drawing.Point(345, 167);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(26, 20);
            this.BrowseButton.TabIndex = 9;
            this.BrowseButton.Text = "...";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // TexDirectoryBox
            // 
            this.TexDirectoryBox.Location = new System.Drawing.Point(7, 167);
            this.TexDirectoryBox.Name = "TexDirectoryBox";
            this.TexDirectoryBox.Size = new System.Drawing.Size(332, 20);
            this.TexDirectoryBox.TabIndex = 8;
            this.TexDirectoryBox.TextChanged += new System.EventHandler(this.TexDirectoryBox_TextChanged);
            // 
            // TexLabel
            // 
            this.TexLabel.AutoSize = true;
            this.TexLabel.Location = new System.Drawing.Point(4, 151);
            this.TexLabel.Name = "TexLabel";
            this.TexLabel.Size = new System.Drawing.Size(133, 13);
            this.TexLabel.TabIndex = 7;
            this.TexLabel.Text = "$TEXTURE_DIRECTORY";
            // 
            // CameraSpeedUpDown
            // 
            this.CameraSpeedUpDown.DecimalPlaces = 1;
            this.CameraSpeedUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.CameraSpeedUpDown.Location = new System.Drawing.Point(7, 126);
            this.CameraSpeedUpDown.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.CameraSpeedUpDown.Name = "CameraSpeedUpDown";
            this.CameraSpeedUpDown.Size = new System.Drawing.Size(120, 20);
            this.CameraSpeedUpDown.TabIndex = 6;
            this.CameraSpeedUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.CameraSpeedUpDown.ValueChanged += new System.EventHandler(this.CameraSpeedUpDown_Changed);
            // 
            // CameraSpeedLabel
            // 
            this.CameraSpeedLabel.AutoSize = true;
            this.CameraSpeedLabel.Location = new System.Drawing.Point(4, 110);
            this.CameraSpeedLabel.Name = "CameraSpeedLabel";
            this.CameraSpeedLabel.Size = new System.Drawing.Size(146, 13);
            this.CameraSpeedLabel.TabIndex = 5;
            this.CameraSpeedLabel.Text = "$RENDER_CAMERASPEED";
            // 
            // ScreenFarUpDown
            // 
            this.ScreenFarUpDown.Location = new System.Drawing.Point(7, 81);
            this.ScreenFarUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.ScreenFarUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ScreenFarUpDown.Name = "ScreenFarUpDown";
            this.ScreenFarUpDown.Size = new System.Drawing.Size(120, 20);
            this.ScreenFarUpDown.TabIndex = 4;
            this.ScreenFarUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ScreenFarUpDown.ValueChanged += new System.EventHandler(this.ScreenDepth_Changed);
            // 
            // ScreenNearUpDown
            // 
            this.ScreenNearUpDown.Location = new System.Drawing.Point(7, 37);
            this.ScreenNearUpDown.Maximum = new decimal(new int[] {
            2500,
            0,
            0,
            0});
            this.ScreenNearUpDown.Name = "ScreenNearUpDown";
            this.ScreenNearUpDown.Size = new System.Drawing.Size(120, 20);
            this.ScreenNearUpDown.TabIndex = 3;
            this.ScreenNearUpDown.ValueChanged += new System.EventHandler(this.ScreenNear_Changed);
            // 
            // ScreenFarLabel
            // 
            this.ScreenFarLabel.AutoSize = true;
            this.ScreenFarLabel.Location = new System.Drawing.Point(4, 65);
            this.ScreenFarLabel.Name = "ScreenFarLabel";
            this.ScreenFarLabel.Size = new System.Drawing.Size(130, 13);
            this.ScreenFarLabel.TabIndex = 2;
            this.ScreenFarLabel.Text = "$RENDER_SCREENFAR";
            // 
            // ScreenNearLabel
            // 
            this.ScreenNearLabel.AutoSize = true;
            this.ScreenNearLabel.Location = new System.Drawing.Point(4, 21);
            this.ScreenNearLabel.Name = "ScreenNearLabel";
            this.ScreenNearLabel.Size = new System.Drawing.Size(139, 13);
            this.ScreenNearLabel.TabIndex = 0;
            this.ScreenNearLabel.Text = "$RENDER_SCREENNEAR";
            // 
            // TexBrowser
            // 
            this.TexBrowser.Description = "$SELECT_TEX_FOLDER";
            // 
            // CheckBox_VSync
            // 
            this.CheckBox_VSync.AutoSize = true;
            this.CheckBox_VSync.Location = new System.Drawing.Point(235, 84);
            this.CheckBox_VSync.Name = "CheckBox_VSync";
            this.CheckBox_VSync.Size = new System.Drawing.Size(57, 17);
            this.CheckBox_VSync.TabIndex = 14;
            this.CheckBox_VSync.Text = "VSync";
            this.CheckBox_VSync.UseVisualStyleBackColor = true;
            this.CheckBox_VSync.CheckedChanged += new System.EventHandler(this.CheckBox_VSync_OnChecked);
            // 
            // RenderOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.RenderGroup);
            this.Name = "RenderOptions";
            this.Size = new System.Drawing.Size(390, 193);
            this.RenderGroup.ResumeLayout(false);
            this.RenderGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FieldOfViewNumDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CameraSpeedUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScreenFarUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScreenNearUpDown)).EndInit();
            this.ResumeLayout(false);

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
    }
}
