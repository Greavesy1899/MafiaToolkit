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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SDSContentEditor));
            this.ResourceTreeView = new Mafia2Tool.Controls.MTreeView();
            this.Context_Menu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Context_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.Tool_Strip = new System.Windows.Forms.ToolStrip();
            this.Button_File = new System.Windows.Forms.ToolStripDropDownButton();
            this.Button_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Reload = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Tools = new System.Windows.Forms.ToolStripDropDownButton();
            this.Button_AutoAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_BatchImportTextures = new System.Windows.Forms.ToolStripMenuItem();
            this.FileDialog_Generic = new System.Windows.Forms.OpenFileDialog();
            this.FolderBrowser_Generic = new System.Windows.Forms.FolderBrowserDialog();
            this.Context_Menu.SuspendLayout();
            this.Tool_Strip.SuspendLayout();
            this.SuspendLayout();
            // 
            // ResourceTreeView
            // 
            this.ResourceTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ResourceTreeView.ContextMenuStrip = this.Context_Menu;
            this.ResourceTreeView.Location = new System.Drawing.Point(14, 32);
            this.ResourceTreeView.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ResourceTreeView.Name = "ResourceTreeView";
            this.ResourceTreeView.Size = new System.Drawing.Size(463, 383);
            this.ResourceTreeView.TabIndex = 0;
            // 
            // Context_Menu
            // 
            this.Context_Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Context_Delete});
            this.Context_Menu.Name = "Context_Menu";
            this.Context_Menu.Size = new System.Drawing.Size(170, 26);
            // 
            // Context_Delete
            // 
            this.Context_Delete.Name = "Context_Delete";
            this.Context_Delete.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete)));
            this.Context_Delete.Size = new System.Drawing.Size(169, 22);
            this.Context_Delete.Text = "$DELETE";
            this.Context_Delete.Click += new System.EventHandler(this.Context_Delete_Click);
            // 
            // Tool_Strip
            // 
            this.Tool_Strip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Button_File,
            this.Button_Tools});
            this.Tool_Strip.Location = new System.Drawing.Point(0, 0);
            this.Tool_Strip.Name = "Tool_Strip";
            this.Tool_Strip.Size = new System.Drawing.Size(490, 25);
            this.Tool_Strip.TabIndex = 15;
            this.Tool_Strip.Text = "toolStrip1";
            // 
            // Button_File
            // 
            this.Button_File.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Button_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Button_Save,
            this.Button_Reload,
            this.Button_Exit});
            this.Button_File.Image = ((System.Drawing.Image)(resources.GetObject("Button_File.Image")));
            this.Button_File.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_File.Name = "Button_File";
            this.Button_File.Size = new System.Drawing.Size(47, 22);
            this.Button_File.Text = "$FILE";
            // 
            // Button_Save
            // 
            this.Button_Save.Name = "Button_Save";
            this.Button_Save.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.Button_Save.Size = new System.Drawing.Size(165, 22);
            this.Button_Save.Text = "$SAVE";
            this.Button_Save.Click += new System.EventHandler(this.Button_Save_Click);
            // 
            // Button_Reload
            // 
            this.Button_Reload.Name = "Button_Reload";
            this.Button_Reload.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.Button_Reload.Size = new System.Drawing.Size(165, 22);
            this.Button_Reload.Text = "$RELOAD";
            this.Button_Reload.Click += new System.EventHandler(this.Button_Reload_Click);
            // 
            // Button_Exit
            // 
            this.Button_Exit.Name = "Button_Exit";
            this.Button_Exit.Size = new System.Drawing.Size(165, 22);
            this.Button_Exit.Text = "$EXIT";
            this.Button_Exit.Click += new System.EventHandler(this.Button_Exit_Click);
            // 
            // Button_Tools
            // 
            this.Button_Tools.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Button_Tools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Button_AutoAdd,
            this.Button_Delete,
            this.Button_BatchImportTextures});
            this.Button_Tools.Image = ((System.Drawing.Image)(resources.GetObject("Button_Tools.Image")));
            this.Button_Tools.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_Tools.Name = "Button_Tools";
            this.Button_Tools.Size = new System.Drawing.Size(61, 22);
            this.Button_Tools.Text = "$TOOLS";
            // 
            // Button_AutoAdd
            // 
            this.Button_AutoAdd.Name = "Button_AutoAdd";
            this.Button_AutoAdd.Size = new System.Drawing.Size(187, 22);
            this.Button_AutoAdd.Text = "$AUTO-ADD_FILES";
            this.Button_AutoAdd.Click += new System.EventHandler(this.Button_AutoAdd_Click);
            // 
            // Button_Delete
            // 
            this.Button_Delete.Name = "Button_Delete";
            this.Button_Delete.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete)));
            this.Button_Delete.Size = new System.Drawing.Size(187, 22);
            this.Button_Delete.Text = "$DELETE";
            this.Button_Delete.Click += new System.EventHandler(this.Button_Delete_Click);
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
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 430);
            this.Controls.Add(this.Tool_Strip);
            this.Controls.Add(this.ResourceTreeView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "SDSContentEditor";
            this.Text = "$SDSCONTENT_EDITOR_TITLE";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SDSContentEditor_Closing);
            this.Context_Menu.ResumeLayout(false);
            this.Tool_Strip.ResumeLayout(false);
            this.Tool_Strip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip Tool_Strip;
        private System.Windows.Forms.ToolStripDropDownButton Button_File;
        private System.Windows.Forms.ToolStripMenuItem Button_Save;
        private System.Windows.Forms.ToolStripMenuItem Button_Reload;
        private System.Windows.Forms.ToolStripMenuItem Button_Exit;
        private System.Windows.Forms.ToolStripDropDownButton Button_Tools;
        private System.Windows.Forms.ToolStripMenuItem Button_AutoAdd;
        private System.Windows.Forms.ToolStripMenuItem Button_BatchImportTextures;
        private System.Windows.Forms.OpenFileDialog FileDialog_Generic;
        private System.Windows.Forms.FolderBrowserDialog FolderBrowser_Generic;
        private Controls.MTreeView ResourceTreeView;
        private System.Windows.Forms.ContextMenuStrip Context_Menu;
        private System.Windows.Forms.ToolStripMenuItem Context_Delete;
        private System.Windows.Forms.ToolStripMenuItem Button_Delete;
    }
}