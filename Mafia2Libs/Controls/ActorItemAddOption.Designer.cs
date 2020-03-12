namespace Forms.EditorControls
{
    partial class ActorItemAddOption
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
            this.ActorTypeLabel = new System.Windows.Forms.Label();
            this.TypeCombo = new System.Windows.Forms.ComboBox();
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
            this.groupGeneral.Controls.Add(this.ActorTypeLabel);
            this.groupGeneral.Controls.Add(this.TypeCombo);
            this.groupGeneral.Location = new System.Drawing.Point(0, 0);
            this.groupGeneral.Name = "groupGeneral";
            this.groupGeneral.Size = new System.Drawing.Size(340, 75);
            this.groupGeneral.TabIndex = 2;
            this.groupGeneral.TabStop = false;
            this.groupGeneral.Text = "$GENERAL";
            // 
            // ActorTypeLabel
            // 
            this.ActorTypeLabel.AutoSize = true;
            this.ActorTypeLabel.Location = new System.Drawing.Point(6, 27);
            this.ActorTypeLabel.Name = "ActorTypeLabel";
            this.ActorTypeLabel.Size = new System.Drawing.Size(84, 13);
            this.ActorTypeLabel.TabIndex = 1;
            this.ActorTypeLabel.Text = "$ACTOR_TYPE";
            // 
            // TypeCombo
            // 
            this.TypeCombo.FormattingEnabled = true;
            this.TypeCombo.Location = new System.Drawing.Point(182, 24);
            this.TypeCombo.Name = "TypeCombo";
            this.TypeCombo.Size = new System.Drawing.Size(152, 21);
            this.TypeCombo.TabIndex = 0;
            // 
            // ActorItemAddOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupGeneral);
            this.Name = "ActorItemAddOption";
            this.Size = new System.Drawing.Size(340, 78);
            this.groupGeneral.ResumeLayout(false);
            this.groupGeneral.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.FolderBrowserDialog MafiaIIBrowser;
        private System.Windows.Forms.GroupBox groupGeneral;
        private System.Windows.Forms.Label ActorTypeLabel;
        private System.Windows.Forms.ComboBox TypeCombo;
    }
}
