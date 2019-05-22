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
            this.RenderGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CameraSpeedUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScreenFarUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScreenNearUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // RenderGroup
            // 
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
            10,
            0,
            0,
            0});
            this.CameraSpeedUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
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
            // RenderOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.RenderGroup);
            this.Name = "RenderOptions";
            this.Size = new System.Drawing.Size(390, 193);
            this.RenderGroup.ResumeLayout(false);
            this.RenderGroup.PerformLayout();
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
    }
}
