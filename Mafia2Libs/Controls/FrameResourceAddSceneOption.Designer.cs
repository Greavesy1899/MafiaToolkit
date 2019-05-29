namespace Forms.EditorControls
{
    partial class FrameResourceSceneOption
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
            this.OffsetZBox = new System.Windows.Forms.TextBox();
            this.OffsetYBox = new System.Windows.Forms.TextBox();
            this.OffsetXBox = new System.Windows.Forms.TextBox();
            this.FSceneOffsetLabel = new System.Windows.Forms.Label();
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
            this.groupGeneral.Controls.Add(this.OffsetZBox);
            this.groupGeneral.Controls.Add(this.OffsetYBox);
            this.groupGeneral.Controls.Add(this.OffsetXBox);
            this.groupGeneral.Controls.Add(this.FSceneOffsetLabel);
            this.groupGeneral.Location = new System.Drawing.Point(0, 0);
            this.groupGeneral.Name = "groupGeneral";
            this.groupGeneral.Size = new System.Drawing.Size(340, 65);
            this.groupGeneral.TabIndex = 2;
            this.groupGeneral.TabStop = false;
            this.groupGeneral.Text = "$GENERAL";
            // 
            // OffsetZBox
            // 
            this.OffsetZBox.Location = new System.Drawing.Point(269, 24);
            this.OffsetZBox.Name = "OffsetZBox";
            this.OffsetZBox.Size = new System.Drawing.Size(61, 20);
            this.OffsetZBox.TabIndex = 4;
            this.OffsetZBox.Text = "0";
            this.OffsetZBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // OffsetYBox
            // 
            this.OffsetYBox.Location = new System.Drawing.Point(202, 24);
            this.OffsetYBox.Name = "OffsetYBox";
            this.OffsetYBox.Size = new System.Drawing.Size(61, 20);
            this.OffsetYBox.TabIndex = 3;
            this.OffsetYBox.Text = "0";
            this.OffsetYBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // OffsetXBox
            // 
            this.OffsetXBox.Location = new System.Drawing.Point(135, 24);
            this.OffsetXBox.Name = "OffsetXBox";
            this.OffsetXBox.Size = new System.Drawing.Size(61, 20);
            this.OffsetXBox.TabIndex = 2;
            this.OffsetXBox.Text = "0";
            this.OffsetXBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FSceneOffsetLabel
            // 
            this.FSceneOffsetLabel.AutoSize = true;
            this.FSceneOffsetLabel.Location = new System.Drawing.Point(6, 27);
            this.FSceneOffsetLabel.Name = "FSceneOffsetLabel";
            this.FSceneOffsetLabel.Size = new System.Drawing.Size(110, 13);
            this.FSceneOffsetLabel.TabIndex = 1;
            this.FSceneOffsetLabel.Text = "$FRSCENE_OFFSET";
            // 
            // FrameResourceSceneOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupGeneral);
            this.Name = "FrameResourceSceneOption";
            this.Size = new System.Drawing.Size(340, 65);
            this.groupGeneral.ResumeLayout(false);
            this.groupGeneral.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.FolderBrowserDialog MafiaIIBrowser;
        private System.Windows.Forms.GroupBox groupGeneral;
        private System.Windows.Forms.Label FSceneOffsetLabel;
        private System.Windows.Forms.TextBox OffsetZBox;
        private System.Windows.Forms.TextBox OffsetYBox;
        private System.Windows.Forms.TextBox OffsetXBox;
    }
}
