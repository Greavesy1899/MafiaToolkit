namespace Mafia2Tool.Forms
{
    partial class TranslokatorEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TranslokatorEditor));
            this.PropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.TranslokatorTree = new System.Windows.Forms.TreeView();
            this.CollisionContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ContextDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.deletePlacementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openM2T = new System.Windows.Forms.OpenFileDialog();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.fileToolButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.SaveToolButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ReloadButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitButton = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.CollisionContext.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // PropertyGrid
            // 
            this.PropertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PropertyGrid.Location = new System.Drawing.Point(386, 28);
            this.PropertyGrid.Name = "PropertyGrid";
            this.PropertyGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.PropertyGrid.Size = new System.Drawing.Size(402, 416);
            this.PropertyGrid.TabIndex = 16;
            // 
            // TranslokatorTree
            // 
            this.TranslokatorTree.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.TranslokatorTree.ContextMenuStrip = this.CollisionContext;
            this.TranslokatorTree.Location = new System.Drawing.Point(12, 28);
            this.TranslokatorTree.Name = "TranslokatorTree";
            this.TranslokatorTree.Size = new System.Drawing.Size(368, 416);
            this.TranslokatorTree.TabIndex = 17;
            this.TranslokatorTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TranslokatorTree_AfterSelect);
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
            this.fileToolButton,
            this.toolsButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 25);
            this.toolStrip1.TabIndex = 18;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // fileToolButton
            // 
            this.fileToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.fileToolButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveToolButton,
            this.ReloadButton,
            this.ExitButton});
            this.fileToolButton.Image = ((System.Drawing.Image)(resources.GetObject("fileToolButton.Image")));
            this.fileToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.fileToolButton.Name = "fileToolButton";
            this.fileToolButton.Size = new System.Drawing.Size(38, 22);
            this.fileToolButton.Text = "File";
            // 
            // SaveToolButton
            // 
            this.SaveToolButton.Name = "SaveToolButton";
            this.SaveToolButton.Size = new System.Drawing.Size(180, 22);
            this.SaveToolButton.Text = "Save";
            this.SaveToolButton.Click += new System.EventHandler(this.SaveToolButton_Click);
            // 
            // ReloadButton
            // 
            this.ReloadButton.Name = "ReloadButton";
            this.ReloadButton.Size = new System.Drawing.Size(180, 22);
            this.ReloadButton.Text = "Reload";
            this.ReloadButton.Click += new System.EventHandler(this.ReloadButton_Click);
            // 
            // ExitButton
            // 
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(180, 22);
            this.ExitButton.Text = "Exit";
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // toolsButton
            // 
            this.toolsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolsButton.Image = ((System.Drawing.Image)(resources.GetObject("toolsButton.Image")));
            this.toolsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolsButton.Name = "toolsButton";
            this.toolsButton.Size = new System.Drawing.Size(48, 22);
            this.toolsButton.Text = "Tools";
            // 
            // TranslokatorEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.PropertyGrid);
            this.Controls.Add(this.TranslokatorTree);
            this.Controls.Add(this.toolStrip1);
            this.Name = "TranslokatorEditor";
            this.Text = "TranslokatorEditor";
            this.CollisionContext.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PropertyGrid PropertyGrid;
        private System.Windows.Forms.TreeView TranslokatorTree;
        private System.Windows.Forms.ContextMenuStrip CollisionContext;
        private System.Windows.Forms.ToolStripMenuItem ContextDelete;
        private System.Windows.Forms.ToolStripMenuItem deletePlacementToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openM2T;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton fileToolButton;
        private System.Windows.Forms.ToolStripMenuItem SaveToolButton;
        private System.Windows.Forms.ToolStripMenuItem ReloadButton;
        private System.Windows.Forms.ToolStripMenuItem ExitButton;
        private System.Windows.Forms.ToolStripDropDownButton toolsButton;
    }
}