namespace Mafia2Tool.EditorControls
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
            this.FSceneOffsetLabel = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
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
            this.groupGeneral.Controls.Add(this.textBox3);
            this.groupGeneral.Controls.Add(this.textBox2);
            this.groupGeneral.Controls.Add(this.textBox1);
            this.groupGeneral.Controls.Add(this.FSceneOffsetLabel);
            this.groupGeneral.Location = new System.Drawing.Point(0, 0);
            this.groupGeneral.Name = "groupGeneral";
            this.groupGeneral.Size = new System.Drawing.Size(340, 70);
            this.groupGeneral.TabIndex = 2;
            this.groupGeneral.TabStop = false;
            this.groupGeneral.Text = "$GENERAL";
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
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(135, 24);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(61, 20);
            this.textBox1.TabIndex = 2;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(202, 24);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(61, 20);
            this.textBox2.TabIndex = 3;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(269, 24);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(61, 20);
            this.textBox3.TabIndex = 4;
            // 
            // FrameResourceSceneOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupGeneral);
            this.Name = "FrameResourceSceneOption";
            this.Size = new System.Drawing.Size(340, 70);
            this.groupGeneral.ResumeLayout(false);
            this.groupGeneral.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.FolderBrowserDialog MafiaIIBrowser;
        private System.Windows.Forms.GroupBox groupGeneral;
        private System.Windows.Forms.Label FSceneOffsetLabel;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
    }
}
