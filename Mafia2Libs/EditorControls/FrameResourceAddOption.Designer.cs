namespace Mafia2Tool.EditorControls
{
    partial class FrameResourceAddOption
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
            this.MafiaIIBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.groupGeneral = new System.Windows.Forms.GroupBox();
            this.FraddTypeLabel = new System.Windows.Forms.Label();
            this.FraddTypeCombo = new System.Windows.Forms.ComboBox();
            this.groupGeneral.SuspendLayout();
            this.SuspendLayout();
            // 
            // MafiaIIBrowser
            // 
            this.MafiaIIBrowser.Description = "$SELECT_MII_FOLDER";
            // 
            // groupGeneral
            // 
            this.groupGeneral.AutoSize = true;
            this.groupGeneral.Controls.Add(this.FraddTypeLabel);
            this.groupGeneral.Controls.Add(this.FraddTypeCombo);
            this.groupGeneral.Location = new System.Drawing.Point(0, 0);
            this.groupGeneral.Name = "groupGeneral";
            this.groupGeneral.Size = new System.Drawing.Size(340, 70);
            this.groupGeneral.TabIndex = 2;
            this.groupGeneral.TabStop = false;
            this.groupGeneral.Text = "$GENERAL";
            // 
            // FraddTypeLabel
            // 
            this.FraddTypeLabel.AutoSize = true;
            this.FraddTypeLabel.Location = new System.Drawing.Point(6, 27);
            this.FraddTypeLabel.Name = "FraddTypeLabel";
            this.FraddTypeLabel.Size = new System.Drawing.Size(84, 13);
            this.FraddTypeLabel.TabIndex = 1;
            this.FraddTypeLabel.Text = "$FRADD_TYPE";
            // 
            // FraddTypeCombo
            // 
            this.FraddTypeCombo.FormattingEnabled = true;
            this.FraddTypeCombo.Items.AddRange(new object[] {
            "Single Mesh",
            "Frame",
            "Light",
            "Camera",
            "Component_U005",
            "Sector",
            "Dummy",
            "Deflector",
            "Area",
            "Target",
            "Model",
            "Collision"});
            this.FraddTypeCombo.Location = new System.Drawing.Point(182, 24);
            this.FraddTypeCombo.Name = "FraddTypeCombo";
            this.FraddTypeCombo.Size = new System.Drawing.Size(152, 21);
            this.FraddTypeCombo.TabIndex = 0;
            // 
            // FrameResourceAddOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupGeneral);
            this.Name = "FrameResourceAddOption";
            this.Size = new System.Drawing.Size(340, 70);
            this.groupGeneral.ResumeLayout(false);
            this.groupGeneral.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.FolderBrowserDialog MafiaIIBrowser;
        private System.Windows.Forms.GroupBox groupGeneral;
        private System.Windows.Forms.Label FraddTypeLabel;
        private System.Windows.Forms.ComboBox FraddTypeCombo;
    }
}
