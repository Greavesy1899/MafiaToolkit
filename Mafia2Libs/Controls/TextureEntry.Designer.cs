namespace MafiaToolkit
{
    partial class TextureEntry
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
            this.Container = new System.Windows.Forms.SplitContainer();
            this.TextureImage = new System.Windows.Forms.PictureBox();
            this.MaterialName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Container)).BeginInit();
            this.Container.Panel1.SuspendLayout();
            this.Container.Panel2.SuspendLayout();
            this.Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TextureImage)).BeginInit();
            this.SuspendLayout();
            // 
            // Container
            // 
            this.Container.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Container.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Container.IsSplitterFixed = true;
            this.Container.Location = new System.Drawing.Point(0, 0);
            this.Container.Name = "Container";
            this.Container.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // Container.Panel1
            // 
            this.Container.Panel1.Controls.Add(this.TextureImage);
            // 
            // Container.Panel2
            // 
            this.Container.Panel2.Controls.Add(this.MaterialName);
            this.Container.Size = new System.Drawing.Size(150, 150);
            this.Container.SplitterDistance = 120;
            this.Container.TabIndex = 1;
            // 
            // TextureImage
            // 
            this.TextureImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextureImage.Location = new System.Drawing.Point(0, 0);
            this.TextureImage.Name = "TextureImage";
            this.TextureImage.Size = new System.Drawing.Size(150, 120);
            this.TextureImage.TabIndex = 0;
            this.TextureImage.TabStop = false;
            // 
            // MaterialName
            // 
            this.MaterialName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MaterialName.Location = new System.Drawing.Point(0, 0);
            this.MaterialName.Name = "MaterialName";
            this.MaterialName.Size = new System.Drawing.Size(150, 26);
            this.MaterialName.TabIndex = 0;
            this.MaterialName.Text = "$Material_Name";
            // 
            // TextureEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.Container);
            this.Name = "TextureEntry";
            this.Load += new System.EventHandler(this.OnLoad);
            this.Container.Panel1.ResumeLayout(false);
            this.Container.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Container)).EndInit();
            this.Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TextureImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer Container;
        private System.Windows.Forms.Label MaterialName;
        private System.Windows.Forms.PictureBox TextureImage;
    }
}
