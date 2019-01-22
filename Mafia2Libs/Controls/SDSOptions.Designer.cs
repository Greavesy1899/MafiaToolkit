namespace Mafia2Tool.OptionControls
{
    partial class SDSOptions
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
            this.groupSDS = new System.Windows.Forms.GroupBox();
            this.SuspendLayout();
            // 
            // groupSDS
            // 
            this.groupSDS.AutoSize = true;
            this.groupSDS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupSDS.Location = new System.Drawing.Point(0, 0);
            this.groupSDS.Name = "groupSDS";
            this.groupSDS.Size = new System.Drawing.Size(150, 150);
            this.groupSDS.TabIndex = 0;
            this.groupSDS.TabStop = false;
            this.groupSDS.Text = "SDS Options";
            // 
            // SDSOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupSDS);
            this.Name = "SDSOptions";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupSDS;
    }
}
