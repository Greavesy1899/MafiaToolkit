namespace Mafia2Tool
{
    partial class ShopMenu2Editor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShopMenu2Editor));
            this.FrameResourceGrid = new System.Windows.Forms.PropertyGrid();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.CollisionContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ContextDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.deletePlacementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openM2T = new System.Windows.Forms.OpenFileDialog();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.fileToolButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CollisionContext.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // FrameResourceGrid
            // 
            this.FrameResourceGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FrameResourceGrid.Location = new System.Drawing.Point(402, 28);
            this.FrameResourceGrid.Name = "FrameResourceGrid";
            this.FrameResourceGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.FrameResourceGrid.Size = new System.Drawing.Size(386, 410);
            this.FrameResourceGrid.TabIndex = 10;
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeView1.ContextMenuStrip = this.CollisionContext;
            this.treeView1.Location = new System.Drawing.Point(12, 28);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(368, 410);
            this.treeView1.TabIndex = 11;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnNodeSelectSelect);
            // 
            // CollisionContext
            // 
            this.CollisionContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ContextDelete,
            this.deletePlacementToolStripMenuItem});
            this.CollisionContext.Name = "SDSContext";
            this.CollisionContext.Size = new System.Drawing.Size(167, 48);
            // 
            // ContextDelete
            // 
            this.ContextDelete.Name = "ContextDelete";
            this.ContextDelete.Size = new System.Drawing.Size(166, 22);
            this.ContextDelete.Text = "Delete Collision";
            // 
            // deletePlacementToolStripMenuItem
            // 
            this.deletePlacementToolStripMenuItem.Name = "deletePlacementToolStripMenuItem";
            this.deletePlacementToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.deletePlacementToolStripMenuItem.Text = "Delete Placement";
            // 
            // openM2T
            // 
            this.openM2T.FileName = "Select M2T file.";
            this.openM2T.Filter = "Model File|*.m2t|All Files|*.*|FBX Model|*.fbx";
            this.openM2T.Tag = "";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolButton});
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
            this.saveToolStripMenuItem,
            this.reloadToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolButton.Image = ((System.Drawing.Image)(resources.GetObject("fileToolButton.Image")));
            this.fileToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.fileToolButton.Name = "fileToolButton";
            this.fileToolButton.Size = new System.Drawing.Size(47, 22);
            this.fileToolButton.Text = "$FILE";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.saveToolStripMenuItem.Text = "$SAVE";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // reloadToolStripMenuItem
            // 
            this.reloadToolStripMenuItem.Name = "reloadToolStripMenuItem";
            this.reloadToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.reloadToolStripMenuItem.Text = "$RELOAD";
            this.reloadToolStripMenuItem.Click += new System.EventHandler(this.reloadToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.exitToolStripMenuItem.Text = "$EXIT";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // ShopMenu2Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.FrameResourceGrid);
            this.Controls.Add(this.treeView1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ShopMenu2Editor";
            this.Text = "$SPEECH_EDITOR_TITLE";
            this.CollisionContext.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PropertyGrid FrameResourceGrid;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.OpenFileDialog openM2T;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton fileToolButton;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reloadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip CollisionContext;
        private System.Windows.Forms.ToolStripMenuItem ContextDelete;
        private System.Windows.Forms.ToolStripMenuItem deletePlacementToolStripMenuItem;
    }
}