namespace Mafia2Tool.OptionControls
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
            this.ScreenFarUpDown = new System.Windows.Forms.NumericUpDown();
            this.ScreenNearUpDown = new System.Windows.Forms.NumericUpDown();
            this.ScreenFarLabel = new System.Windows.Forms.Label();
            this.ScreenNearLabel = new System.Windows.Forms.Label();
            this.ExportPathButton = new System.Windows.Forms.FolderBrowserDialog();
            this.CameraSpeedUpDown = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.RenderGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ScreenFarUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScreenNearUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CameraSpeedUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // RenderGroup
            // 
            this.RenderGroup.Controls.Add(this.CameraSpeedUpDown);
            this.RenderGroup.Controls.Add(this.label1);
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
            // ExportPathButton
            // 
            this.ExportPathButton.Description = "$EXPORT_PATH_DESC";
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
            65536});
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 110);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(146, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "$RENDER_CAMERASPEED";
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
            ((System.ComponentModel.ISupportInitialize)(this.ScreenFarUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScreenNearUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CameraSpeedUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox RenderGroup;
        private System.Windows.Forms.Label ScreenNearLabel;
        private System.Windows.Forms.Label ScreenFarLabel;
        private System.Windows.Forms.FolderBrowserDialog ExportPathButton;
        private System.Windows.Forms.NumericUpDown ScreenFarUpDown;
        private System.Windows.Forms.NumericUpDown ScreenNearUpDown;
        private System.Windows.Forms.NumericUpDown CameraSpeedUpDown;
        private System.Windows.Forms.Label label1;
    }
}
