namespace Mafia2Tool
{
    partial class SDSContentEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SDSContentEditor));
            this.ResourceTreeView = new System.Windows.Forms.TreeView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.fileToolButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.SaveButton = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolsButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.AutoAddFilesBUtton = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Button_BatchImportTextures = new System.Windows.Forms.ToolStripMenuItem();
            this.FileDialog_Generic = new System.Windows.Forms.OpenFileDialog();
            this.FolderBrowser_Generic = new System.Windows.Forms.FolderBrowserDialog();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ResourceTreeView
            // 
            this.ResourceTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ResourceTreeView.Location = new System.Drawing.Point(12, 28);
            this.ResourceTreeView.Name = "ResourceTreeView";
            this.ResourceTreeView.Size = new System.Drawing.Size(184, 156);
            this.ResourceTreeView.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolButton,
            this.ToolsButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 25);
            this.toolStrip1.TabIndex = 15;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // fileToolButton
            // 
            this.fileToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.fileToolButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveButton,
            this.reloadToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolButton.Image = ((System.Drawing.Image)(resources.GetObject("fileToolButton.Image")));
            this.fileToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.fileToolButton.Name = "fileToolButton";
            this.fileToolButton.Size = new System.Drawing.Size(38, 22);
            this.fileToolButton.Text = "File";
            // 
            // SaveButton
            // 
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(110, 22);
            this.SaveButton.Text = "Save";
            this.SaveButton.Click += new System.EventHandler(this.SaveButtonOnClick);
            // 
            // reloadToolStripMenuItem
            // 
            this.reloadToolStripMenuItem.Name = "reloadToolStripMenuItem";
            this.reloadToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.reloadToolStripMenuItem.Text = "Reload";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // ToolsButton
            // 
            this.ToolsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ToolsButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AutoAddFilesBUtton,
            this.Button_BatchImportTextures});
            this.ToolsButton.Image = ((System.Drawing.Image)(resources.GetObject("ToolsButton.Image")));
            this.ToolsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolsButton.Name = "ToolsButton";
            this.ToolsButton.Size = new System.Drawing.Size(47, 22);
            this.ToolsButton.Text = "Tools";
            // 
            // AutoAddFilesBUtton
            // 
            this.AutoAddFilesBUtton.Name = "AutoAddFilesBUtton";
            this.AutoAddFilesBUtton.Size = new System.Drawing.Size(187, 22);
            this.AutoAddFilesBUtton.Text = "Auto-Add Files";
            this.AutoAddFilesBUtton.Click += new System.EventHandler(this.AutoAddFilesButton_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.Location = new System.Drawing.Point(203, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(585, 156);
            this.panel1.TabIndex = 16;
            // 
            // Button_BatchImportTextures
            // 
            this.Button_BatchImportTextures.Name = "Button_BatchImportTextures";
            this.Button_BatchImportTextures.Size = new System.Drawing.Size(187, 22);
            this.Button_BatchImportTextures.Text = "$BATCH_IMPORT_TEX";
            this.Button_BatchImportTextures.Click += new System.EventHandler(this.Button_BatchImportTextures_Click);
            // 
            // FileDialog_Generic
            // 
            this.FileDialog_Generic.FileName = "AllTextures";
            this.FileDialog_Generic.Filter = "Text Files|*.txt";
            // 
            // FolderBrowser_Generic
            // 
            this.FolderBrowser_Generic.Description = "Select folder which contains textures.";
            this.FolderBrowser_Generic.ShowNewFolderButton = false;
            // 
            // SDSContentEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 196);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.ResourceTreeView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SDSContentEditor";
            this.Text = "SDSContentEditor";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView ResourceTreeView;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton fileToolButton;
        private System.Windows.Forms.ToolStripMenuItem SaveButton;
        private System.Windows.Forms.ToolStripMenuItem reloadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripDropDownButton ToolsButton;
        private System.Windows.Forms.ToolStripMenuItem AutoAddFilesBUtton;
        private System.Windows.Forms.ToolStripMenuItem Button_BatchImportTextures;
        private System.Windows.Forms.OpenFileDialog FileDialog_Generic;
        private System.Windows.Forms.FolderBrowserDialog FolderBrowser_Generic;
    }
}