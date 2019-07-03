using System.Windows.Forms;

namespace Mafia2Tool
{
    partial class StreamEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StreamEditor));
            this.linesTree = new System.Windows.Forms.TreeView();
            this.LineContextStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStrip = new System.Windows.Forms.ToolStrip();
            this.fileToolButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.StreamLinesPage = new System.Windows.Forms.TabPage();
            this.StreamGroupPage = new System.Windows.Forms.TabPage();
            this.StreamBlocksPage = new System.Windows.Forms.TabPage();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.AddLineButton = new System.Windows.Forms.ToolStripMenuItem();
            this.groupTree = new Utils.Extensions.MTreeView();
            this.blockView = new Utils.Extensions.MTreeView();
            this.DeleteLineButton = new System.Windows.Forms.ToolStripMenuItem();
            this.LineContextStrip.SuspendLayout();
            this.ToolStrip.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.StreamLinesPage.SuspendLayout();
            this.StreamGroupPage.SuspendLayout();
            this.StreamBlocksPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // linesTree
            // 
            this.linesTree.ContextMenuStrip = this.LineContextStrip;
            this.linesTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.linesTree.Location = new System.Drawing.Point(0, 0);
            this.linesTree.Name = "linesTree";
            this.linesTree.Size = new System.Drawing.Size(238, 384);
            this.linesTree.TabIndex = 11;
            this.linesTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnNodeSelectSelect);
            // 
            // LineContextStrip
            // 
            this.LineContextStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddLineButton,
            this.DeleteLineButton});
            this.LineContextStrip.Name = "AddLineButton";
            this.LineContextStrip.Size = new System.Drawing.Size(181, 70);
            this.LineContextStrip.Text = "Context Strip";
            this.LineContextStrip.Opening += new System.ComponentModel.CancelEventHandler(this.OnContextMenuOpening);
            // 
            // ToolStrip
            // 
            this.ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolButton});
            this.ToolStrip.Location = new System.Drawing.Point(0, 0);
            this.ToolStrip.Name = "ToolStrip";
            this.ToolStrip.Size = new System.Drawing.Size(934, 25);
            this.ToolStrip.TabIndex = 15;
            this.ToolStrip.Text = "toolStrip1";
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
            // PropertyGrid
            // 
            this.PropertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PropertyGrid.Location = new System.Drawing.Point(260, 28);
            this.PropertyGrid.Name = "PropertyGrid";
            this.PropertyGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.PropertyGrid.Size = new System.Drawing.Size(662, 410);
            this.PropertyGrid.TabIndex = 10;
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tabControl.Controls.Add(this.StreamLinesPage);
            this.tabControl.Controls.Add(this.StreamGroupPage);
            this.tabControl.Controls.Add(this.StreamBlocksPage);
            this.tabControl.Location = new System.Drawing.Point(12, 28);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(246, 410);
            this.tabControl.TabIndex = 16;
            // 
            // StreamLinesPage
            // 
            this.StreamLinesPage.Controls.Add(this.linesTree);
            this.StreamLinesPage.Location = new System.Drawing.Point(4, 22);
            this.StreamLinesPage.Name = "StreamLinesPage";
            this.StreamLinesPage.Size = new System.Drawing.Size(238, 384);
            this.StreamLinesPage.TabIndex = 0;
            this.StreamLinesPage.Text = "Stream Lines";
            this.StreamLinesPage.UseVisualStyleBackColor = true;
            // 
            // StreamGroupPage
            // 
            this.StreamGroupPage.Controls.Add(this.groupTree);
            this.StreamGroupPage.Location = new System.Drawing.Point(4, 22);
            this.StreamGroupPage.Name = "StreamGroupPage";
            this.StreamGroupPage.Size = new System.Drawing.Size(238, 384);
            this.StreamGroupPage.TabIndex = 1;
            this.StreamGroupPage.Text = "Stream Groups";
            this.StreamGroupPage.UseVisualStyleBackColor = true;
            // 
            // StreamBlocksPage
            // 
            this.StreamBlocksPage.Controls.Add(this.blockView);
            this.StreamBlocksPage.Location = new System.Drawing.Point(4, 22);
            this.StreamBlocksPage.Name = "StreamBlocksPage";
            this.StreamBlocksPage.Size = new System.Drawing.Size(238, 384);
            this.StreamBlocksPage.TabIndex = 2;
            this.StreamBlocksPage.Text = "Stream Blocks";
            this.StreamBlocksPage.UseVisualStyleBackColor = true;
            // 
            // AddLineButton
            // 
            this.AddLineButton.Name = "AddLineButton";
            this.AddLineButton.Size = new System.Drawing.Size(180, 22);
            this.AddLineButton.Text = "Add Line";
            this.AddLineButton.Click += new System.EventHandler(this.AddLineButtonPressed);
            // 
            // groupTree
            // 
            this.groupTree.CheckBoxes = true;
            this.groupTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupTree.Location = new System.Drawing.Point(0, 0);
            this.groupTree.Name = "groupTree";
            this.groupTree.Size = new System.Drawing.Size(238, 384);
            this.groupTree.TabIndex = 13;
            this.groupTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnNodeSelectSelect);
            // 
            // blockView
            // 
            this.blockView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.blockView.Location = new System.Drawing.Point(0, 0);
            this.blockView.Name = "blockView";
            this.blockView.Size = new System.Drawing.Size(238, 384);
            this.blockView.TabIndex = 14;
            this.blockView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnNodeSelectSelect);
            // 
            // DeleteLineButton
            // 
            this.DeleteLineButton.Name = "DeleteLineButton";
            this.DeleteLineButton.Size = new System.Drawing.Size(180, 22);
            this.DeleteLineButton.Text = "Delete Line";
            this.DeleteLineButton.Click += new System.EventHandler(this.DeleteLineButtonPressed);
            // 
            // StreamEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(934, 450);
            this.Controls.Add(this.ToolStrip);
            this.Controls.Add(this.PropertyGrid);
            this.Controls.Add(this.tabControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "StreamEditor";
            this.Text = "$SPEECH_EDITOR_TITLE";
            this.LineContextStrip.ResumeLayout(false);
            this.ToolStrip.ResumeLayout(false);
            this.ToolStrip.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.StreamLinesPage.ResumeLayout(false);
            this.StreamGroupPage.ResumeLayout(false);
            this.StreamBlocksPage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TreeView linesTree;
        private System.Windows.Forms.ToolStrip ToolStrip;
        private System.Windows.Forms.ToolStripDropDownButton fileToolButton;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reloadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.PropertyGrid PropertyGrid;
        private System.Windows.Forms.TabControl tabControl;
        private TabPage StreamLinesPage;
        private TabPage StreamGroupPage;
        private TabPage StreamBlocksPage;
        private Utils.Extensions.MTreeView groupTree;
        private Utils.Extensions.MTreeView blockView;
        private ContextMenuStrip LineContextStrip;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private ToolStripMenuItem AddLineButton;
        private ToolStripMenuItem DeleteLineButton;
    }
}