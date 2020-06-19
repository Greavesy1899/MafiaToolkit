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
            this.ActorDefinitionLabel = new System.Windows.Forms.Label();
            this.DefinitionBox = new System.Windows.Forms.TextBox();
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
            this.groupGeneral.Controls.Add(this.DefinitionBox);
            this.groupGeneral.Controls.Add(this.ActorDefinitionLabel);
            this.groupGeneral.Controls.Add(this.ActorTypeLabel);
            this.groupGeneral.Controls.Add(this.TypeCombo);
            this.groupGeneral.Location = new System.Drawing.Point(0, 0);
            this.groupGeneral.Name = "groupGeneral";
            this.groupGeneral.Size = new System.Drawing.Size(340, 88);
            this.groupGeneral.TabIndex = 2;
            this.groupGeneral.TabStop = false;
            this.groupGeneral.Text = "$GENERAL";
            // 
            // ActorTypeLabel
            // 
            this.ActorTypeLabel.AutoSize = true;
            this.ActorTypeLabel.Location = new System.Drawing.Point(6, 51);
            this.ActorTypeLabel.Name = "ActorTypeLabel";
            this.ActorTypeLabel.Size = new System.Drawing.Size(84, 13);
            this.ActorTypeLabel.TabIndex = 1;
            this.ActorTypeLabel.Text = "$ACTOR_TYPE";
            // 
            // TypeCombo
            // 
            this.TypeCombo.FormattingEnabled = true;
            this.TypeCombo.Location = new System.Drawing.Point(182, 48);
            this.TypeCombo.Name = "TypeCombo";
            this.TypeCombo.Size = new System.Drawing.Size(152, 21);
            this.TypeCombo.TabIndex = 0;
            // 
            // ActorDefinitionLabel
            // 
            this.ActorDefinitionLabel.AutoSize = true;
            this.ActorDefinitionLabel.Location = new System.Drawing.Point(6, 24);
            this.ActorDefinitionLabel.Name = "ActorDefinitionLabel";
            this.ActorDefinitionLabel.Size = new System.Drawing.Size(117, 13);
            this.ActorDefinitionLabel.TabIndex = 3;
            this.ActorDefinitionLabel.Text = "$ACTOR_DEFINITION";
            // 
            // DefinitionBox
            // 
            this.DefinitionBox.Location = new System.Drawing.Point(182, 20);
            this.DefinitionBox.Name = "DefinitionBox";
            this.DefinitionBox.Size = new System.Drawing.Size(152, 20);
            this.DefinitionBox.TabIndex = 4;
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
        private System.Windows.Forms.Label ActorDefinitionLabel;
        private System.Windows.Forms.TextBox DefinitionBox;
    }
}
