namespace Mafia2Tool {
    partial class FrameResourceTool {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrameResourceTool));
            this.FrameResourceGrid = new System.Windows.Forms.PropertyGrid();
            this.FrameResourceListBox = new System.Windows.Forms.ListBox();
            this.folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.m2tBrowser = new System.Windows.Forms.OpenFileDialog();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.fileToolButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.exportAllSubButton = new System.Windows.Forms.ToolStripMenuItem();
            this.farLodsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.collisionEditorSubButton = new System.Windows.Forms.ToolStripMenuItem();
            this.materialEditorSubButton = new System.Windows.Forms.ToolStripMenuItem();
            this.overwriteBufferSubButton = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.switchViewSubButton = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsButton = new System.Windows.Forms.ToolStripButton();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextExtract3D = new System.Windows.Forms.ToolStripMenuItem();
            this.contextDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.toolStrip1.SuspendLayout();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // FrameResourceGrid
            // 
            this.FrameResourceGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FrameResourceGrid.Location = new System.Drawing.Point(387, 28);
            this.FrameResourceGrid.Name = "FrameResourceGrid";
            this.FrameResourceGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.FrameResourceGrid.Size = new System.Drawing.Size(386, 393);
            this.FrameResourceGrid.TabIndex = 6;
            // 
            // FrameResourceListBox
            // 
            this.FrameResourceListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.FrameResourceListBox.FormattingEnabled = true;
            this.FrameResourceListBox.Location = new System.Drawing.Point(12, 27);
            this.FrameResourceListBox.Name = "FrameResourceListBox";
            this.FrameResourceListBox.Size = new System.Drawing.Size(368, 394);
            this.FrameResourceListBox.TabIndex = 5;
            this.FrameResourceListBox.Visible = false;
            this.FrameResourceListBox.SelectedIndexChanged += new System.EventHandler(this.OnSelectedChanged);
            // 
            // folderBrowser
            // 
            this.folderBrowser.ShowNewFolderButton = false;
            // 
            // m2tBrowser
            // 
            this.m2tBrowser.FileName = "m2tBrowser";
            this.m2tBrowser.Filter = "Meshes|*.m2t";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolButton,
            this.toolsButton,
            this.viewToolButton,
            this.optionsButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(785, 25);
            this.toolStrip1.TabIndex = 14;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // fileToolButton
            // 
            this.fileToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.fileToolButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.reloadToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolButton.Image = ((System.Drawing.Image)(resources.GetObject("fileToolButton.Image")));
            this.fileToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.fileToolButton.Name = "fileToolButton";
            this.fileToolButton.Size = new System.Drawing.Size(38, 22);
            this.fileToolButton.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenClick);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveClick);
            // 
            // reloadToolStripMenuItem
            // 
            this.reloadToolStripMenuItem.Name = "reloadToolStripMenuItem";
            this.reloadToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.reloadToolStripMenuItem.Text = "Reload";
            this.reloadToolStripMenuItem.Click += new System.EventHandler(this.ReloadClick);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitClick);
            // 
            // toolsButton
            // 
            this.toolsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolsButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportAllSubButton,
            this.collisionEditorSubButton,
            this.materialEditorSubButton,
            this.overwriteBufferSubButton});
            this.toolsButton.Image = ((System.Drawing.Image)(resources.GetObject("toolsButton.Image")));
            this.toolsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolsButton.Name = "toolsButton";
            this.toolsButton.Size = new System.Drawing.Size(48, 22);
            this.toolsButton.Text = "Tools";
            // 
            // exportAllSubButton
            // 
            this.exportAllSubButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.farLodsToolStripMenuItem,
            this.modelsToolStripMenuItem,
            this.allToolStripMenuItem});
            this.exportAllSubButton.Enabled = false;
            this.exportAllSubButton.Name = "exportAllSubButton";
            this.exportAllSubButton.Size = new System.Drawing.Size(180, 22);
            this.exportAllSubButton.Text = "Export All";
            this.exportAllSubButton.Click += new System.EventHandler(this.OnClickLoadAll);
            // 
            // farLodsToolStripMenuItem
            // 
            this.farLodsToolStripMenuItem.Name = "farLodsToolStripMenuItem";
            this.farLodsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.farLodsToolStripMenuItem.Text = "Far Lods";
            // 
            // modelsToolStripMenuItem
            // 
            this.modelsToolStripMenuItem.Name = "modelsToolStripMenuItem";
            this.modelsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.modelsToolStripMenuItem.Text = "Models";
            // 
            // allToolStripMenuItem
            // 
            this.allToolStripMenuItem.Name = "allToolStripMenuItem";
            this.allToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.allToolStripMenuItem.Text = "All";
            // 
            // collisionEditorSubButton
            // 
            this.collisionEditorSubButton.Enabled = false;
            this.collisionEditorSubButton.Name = "collisionEditorSubButton";
            this.collisionEditorSubButton.Size = new System.Drawing.Size(180, 22);
            this.collisionEditorSubButton.Text = "Collision Editor";
            this.collisionEditorSubButton.Click += new System.EventHandler(this.CollisionEditor_Click);
            // 
            // materialEditorSubButton
            // 
            this.materialEditorSubButton.Name = "materialEditorSubButton";
            this.materialEditorSubButton.Size = new System.Drawing.Size(180, 22);
            this.materialEditorSubButton.Text = "Material Editor";
            this.materialEditorSubButton.Click += new System.EventHandler(this.LoadMaterialTool);
            // 
            // overwriteBufferSubButton
            // 
            this.overwriteBufferSubButton.Name = "overwriteBufferSubButton";
            this.overwriteBufferSubButton.Size = new System.Drawing.Size(180, 22);
            this.overwriteBufferSubButton.Text = "Overwrite Buffer";
            this.overwriteBufferSubButton.Click += new System.EventHandler(this.OverwriteBuffer_Click);
            // 
            // viewToolButton
            // 
            this.viewToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.viewToolButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.switchViewSubButton});
            this.viewToolButton.Image = ((System.Drawing.Image)(resources.GetObject("viewToolButton.Image")));
            this.viewToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.viewToolButton.Name = "viewToolButton";
            this.viewToolButton.Size = new System.Drawing.Size(45, 22);
            this.viewToolButton.Text = "View";
            // 
            // switchViewSubButton
            // 
            this.switchViewSubButton.Name = "switchViewSubButton";
            this.switchViewSubButton.Size = new System.Drawing.Size(180, 22);
            this.switchViewSubButton.Text = "Switch View";
            this.switchViewSubButton.Click += new System.EventHandler(this.SwitchView);
            // 
            // optionsButton
            // 
            this.optionsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.optionsButton.Image = ((System.Drawing.Image)(resources.GetObject("optionsButton.Image")));
            this.optionsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.optionsButton.Name = "optionsButton";
            this.optionsButton.Size = new System.Drawing.Size(53, 22);
            this.optionsButton.Text = "Options";
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextExtract3D,
            this.contextDelete});
            this.contextMenu.Name = "contextMenuStrip1";
            this.contextMenu.ShowImageMargin = false;
            this.contextMenu.Size = new System.Drawing.Size(107, 48);
            this.contextMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ItemClicked);
            // 
            // contextExtract3D
            // 
            this.contextExtract3D.Name = "contextExtract3D";
            this.contextExtract3D.Size = new System.Drawing.Size(106, 22);
            this.contextExtract3D.Text = "Extract 3D";
            // 
            // contextDelete
            // 
            this.contextDelete.Name = "contextDelete";
            this.contextDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.contextDelete.Size = new System.Drawing.Size(106, 22);
            this.contextDelete.Text = "Delete";
            this.contextDelete.Click += new System.EventHandler(this.OnDelete);
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeView1.Location = new System.Drawing.Point(12, 27);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(368, 394);
            this.treeView1.TabIndex = 15;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnSelect);
            // 
            // FrameResourceTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(785, 431);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.FrameResourceGrid);
            this.Controls.Add(this.FrameResourceListBox);
            this.Name = "FrameResourceTool";
            this.Text = "Frame Resource Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnExit);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PropertyGrid FrameResourceGrid;
        private System.Windows.Forms.ListBox FrameResourceListBox;
        private System.Windows.Forms.FolderBrowserDialog folderBrowser;
        private System.Windows.Forms.OpenFileDialog m2tBrowser;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton fileToolButton;
        private System.Windows.Forms.ToolStripDropDownButton viewToolButton;
        private System.Windows.Forms.ToolStripMenuItem switchViewSubButton;
        private System.Windows.Forms.ToolStripDropDownButton toolsButton;
        private System.Windows.Forms.ToolStripMenuItem exportAllSubButton;
        private System.Windows.Forms.ToolStripMenuItem collisionEditorSubButton;
        private System.Windows.Forms.ToolStripMenuItem materialEditorSubButton;
        private System.Windows.Forms.ToolStripMenuItem overwriteBufferSubButton;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reloadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton optionsButton;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem contextExtract3D;
        private System.Windows.Forms.ToolStripMenuItem contextDelete;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ToolStripMenuItem farLodsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem modelsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem allToolStripMenuItem;
    }
}