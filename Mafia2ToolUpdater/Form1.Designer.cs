namespace Mafia2ToolUpdater
{
    partial class updateForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.infoTextLabel1 = new System.Windows.Forms.Label();
            this.percentageLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.Location = new System.Drawing.Point(0, 131);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(560, 23);
            this.progressBar1.TabIndex = 0;
            // 
            // infoTextLabel1
            // 
            this.infoTextLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoTextLabel1.Location = new System.Drawing.Point(0, 0);
            this.infoTextLabel1.Name = "infoTextLabel1";
            this.infoTextLabel1.Size = new System.Drawing.Size(560, 131);
            this.infoTextLabel1.TabIndex = 1;
            this.infoTextLabel1.Text = "Updating toolset. This should not take too long!";
            this.infoTextLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // percentageLabel
            // 
            this.percentageLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.percentageLabel.Location = new System.Drawing.Point(0, 112);
            this.percentageLabel.Margin = new System.Windows.Forms.Padding(6);
            this.percentageLabel.Name = "percentageLabel";
            this.percentageLabel.Size = new System.Drawing.Size(560, 19);
            this.percentageLabel.TabIndex = 2;
            this.percentageLabel.Text = "Percentage";
            this.percentageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // updateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 154);
            this.Controls.Add(this.percentageLabel);
            this.Controls.Add(this.infoTextLabel1);
            this.Controls.Add(this.progressBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "updateForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Update Tool";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label infoTextLabel1;
        private System.Windows.Forms.Label percentageLabel;
    }
}

