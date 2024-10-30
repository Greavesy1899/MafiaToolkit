
namespace Forms.Docking
{
    partial class DockSceneTree
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DockSceneTree));
            this.EntryMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.JumpToButton = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteButton = new System.Windows.Forms.ToolStripMenuItem();
            this.DuplicateButton = new System.Windows.Forms.ToolStripMenuItem();
            this.Export3DButton = new System.Windows.Forms.ToolStripMenuItem();
            this.FrameActions = new System.Windows.Forms.ToolStripMenuItem();
            this.UpdateParent1Button = new System.Windows.Forms.ToolStripMenuItem();
            this.UpdateParent2Button = new System.Windows.Forms.ToolStripMenuItem();
            this.ExportFrameButton = new System.Windows.Forms.ToolStripMenuItem();
            this.LinkToActorButton = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.TreeView_Explorer = new Mafia2Tool.Controls.MTreeView();
            this.Tab_Explorer = new System.Windows.Forms.TabControl();
            this.TabPage_Explorer = new System.Windows.Forms.TabPage();
            this.TabPage_Searcher = new System.Windows.Forms.TabPage();
            this.Split_Searcher_Root = new System.Windows.Forms.SplitContainer();
            this.Split_Searcher_TextButton = new System.Windows.Forms.SplitContainer();
            this.TextBox_Search = new System.Windows.Forms.TextBox();
            this.Button_Search = new System.Windows.Forms.Button();
            this.TreeView_Searcher = new Mafia2Tool.Controls.MTreeView();
            this.tooltipPanel = new System.Windows.Forms.Panel();
            this.tooltipText = new System.Windows.Forms.Label();
            this.EntryMenuStrip.SuspendLayout();
            this.Tab_Explorer.SuspendLayout();
            this.TabPage_Explorer.SuspendLayout();
            this.TabPage_Searcher.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Split_Searcher_Root)).BeginInit();
            this.Split_Searcher_Root.Panel1.SuspendLayout();
            this.Split_Searcher_Root.Panel2.SuspendLayout();
            this.Split_Searcher_Root.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Split_Searcher_TextButton)).BeginInit();
            this.Split_Searcher_TextButton.Panel1.SuspendLayout();
            this.Split_Searcher_TextButton.Panel2.SuspendLayout();
            this.Split_Searcher_TextButton.SuspendLayout();
            this.tooltipPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // EntryMenuStrip
            // 
            this.EntryMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.JumpToButton,
            this.DeleteButton,
            this.DuplicateButton,
            this.Export3DButton,
            this.FrameActions});
            this.EntryMenuStrip.Name = "EntryMenuStrip";
            this.EntryMenuStrip.Size = new System.Drawing.Size(165, 114);
            this.EntryMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.OpenEntryContext);
            // 
            // JumpToButton
            // 
            this.JumpToButton.Name = "JumpToButton";
            this.JumpToButton.Size = new System.Drawing.Size(164, 22);
            this.JumpToButton.Text = "Jump To Position";
            // 
            // DeleteButton
            // 
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(164, 22);
            this.DeleteButton.Text = "Delete";
            // 
            // DuplicateButton
            // 
            this.DuplicateButton.Name = "DuplicateButton";
            this.DuplicateButton.Size = new System.Drawing.Size(164, 22);
            this.DuplicateButton.Text = "Duplicate";
            // 
            // Export3DButton
            // 
            this.Export3DButton.Name = "Export3DButton";
            this.Export3DButton.Size = new System.Drawing.Size(164, 22);
            this.Export3DButton.Text = "Export 3D";
            // 
            // FrameActions
            // 
            this.FrameActions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.UpdateParent1Button,
            this.UpdateParent2Button,
            this.ExportFrameButton,
            this.LinkToActorButton});
            this.FrameActions.Name = "FrameActions";
            this.FrameActions.Size = new System.Drawing.Size(164, 22);
            this.FrameActions.Text = "Frame Actions";
            // 
            // UpdateParent1Button
            // 
            this.UpdateParent1Button.Name = "UpdateParent1Button";
            this.UpdateParent1Button.Size = new System.Drawing.Size(166, 22);
            this.UpdateParent1Button.Text = "Update Parent 1";
            // 
            // UpdateParent2Button
            // 
            this.UpdateParent2Button.Name = "UpdateParent2Button";
            this.UpdateParent2Button.Size = new System.Drawing.Size(166, 22);
            this.UpdateParent2Button.Text = "Update Parent 2";
            // 
            // ExportFrameButton
            // 
            this.ExportFrameButton.Name = "ExportFrameButton";
            this.ExportFrameButton.Size = new System.Drawing.Size(166, 22);
            this.ExportFrameButton.Text = "Export Frame";
            // 
            // LinkToActorButton
            // 
            this.LinkToActorButton.Name = "LinkToActorButton";
            this.LinkToActorButton.Size = new System.Drawing.Size(166, 22);
            this.LinkToActorButton.Text = "$LINK_TO_ACTOR";
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "ActorFrame.png");
            this.imageList1.Images.SetKeyName(1, "AreaFrame.png");
            this.imageList1.Images.SetKeyName(2, "CameraFrame.png");
            this.imageList1.Images.SetKeyName(3, "CollisionFrame.png");
            this.imageList1.Images.SetKeyName(4, "CollisionObject.png");
            this.imageList1.Images.SetKeyName(5, "LightFrame.png");
            this.imageList1.Images.SetKeyName(6, "MeshFrame.png");
            this.imageList1.Images.SetKeyName(7, "Placeholder.png");
            this.imageList1.Images.SetKeyName(8, "SceneObject.png");
            this.imageList1.Images.SetKeyName(9, "SkinnedFrame.png");
            this.imageList1.Images.SetKeyName(10, "DummyFrame.png");
            // 
            // tooltipPanel
            // 
            this.tooltipPanel.BackColor = System.Drawing.Color.Silver;
            this.tooltipPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tooltipPanel.Dock = System.Windows.Forms.DockStyle.Bottom;            
            this.tooltipPanel.Controls.Add(tooltipText);
            this.tooltipPanel.Location = new System.Drawing.Point(3, 438);
            this.tooltipPanel.Name = "tooltipPanel";
            this.tooltipPanel.Size = new System.Drawing.Size(316, 50);
            this.tooltipPanel.TabIndex = 1;
            // 
            // tooltipText
            // 
            this.tooltipText.AutoSize = true;
            this.tooltipText.Location = new System.Drawing.Point(-2, 0);
            this.tooltipText.Name = "tooltipText";
            this.tooltipText.Size = new System.Drawing.Size(266, 45);
            this.tooltipText.TabIndex = 0;
            this.tooltipText.Text = "Drag with Left Mouse Button to set Parent1\r\nDrag with Right Mouse Button to set Parent2\r\nDrag with Middle Mouse Button to switch frames";
            // 
            // TreeView_Explorer
            // 
            this.TreeView_Explorer.CheckBoxes = true;
            this.TreeView_Explorer.ContextMenuStrip = this.EntryMenuStrip;
            this.TreeView_Explorer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TreeView_Explorer.HideSelection = false;
            this.TreeView_Explorer.ImageIndex = 3;
            this.TreeView_Explorer.ImageList = this.imageList1;
            this.TreeView_Explorer.Location = new System.Drawing.Point(3, 3);
            this.TreeView_Explorer.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TreeView_Explorer.Name = "TreeView_Explorer";
            this.TreeView_Explorer.SelectedImageIndex = 0;
            this.TreeView_Explorer.Size = new System.Drawing.Size(316, 485);
            this.TreeView_Explorer.TabIndex = 0;
            this.TreeView_Explorer.DoubleClick += new System.EventHandler(this.OnDoubleClick);
            this.TreeView_Explorer.AllowDrop = true;
            this.TreeView_Explorer.ItemDrag += TreeView_Explorer_ItemDrag;
            this.TreeView_Explorer.DragEnter += TreeView_Explorer_DragEnter;
            this.TreeView_Explorer.DragDrop += TreeView_Explorer_DragDrop;
            this.TreeView_Explorer.MouseDown += TreeView_Explorer_MouseDown;
            this.TreeView_Explorer.DragOver += TreeView_Explorer_DragOver;
            this.TreeView_Explorer.DragLeave += TreeView_Explorer_DragLeave;
            // 
            // Tab_Explorer
            // 
            this.Tab_Explorer.Controls.Add(this.TabPage_Explorer);
            this.Tab_Explorer.Controls.Add(this.TabPage_Searcher);
            this.Tab_Explorer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tab_Explorer.Location = new System.Drawing.Point(0, 0);
            this.Tab_Explorer.Name = "Tab_Explorer";
            this.Tab_Explorer.SelectedIndex = 0;
            this.Tab_Explorer.Size = new System.Drawing.Size(330, 519);
            this.Tab_Explorer.TabIndex = 1;
            // 
            // TabPage_Explorer
            // 
            this.TabPage_Explorer.Controls.Add(this.TreeView_Explorer);
            this.TabPage_Explorer.Controls.Add(this.tooltipPanel);
            this.TabPage_Explorer.Location = new System.Drawing.Point(4, 24);
            this.TabPage_Explorer.Name = "TabPage_Explorer";
            this.TabPage_Explorer.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage_Explorer.Size = new System.Drawing.Size(322, 491);
            this.TabPage_Explorer.TabIndex = 0;
            this.TabPage_Explorer.Text = "tabPage1";
            this.TabPage_Explorer.UseVisualStyleBackColor = true;
            // 
            // TabPage_Searcher
            // 
            this.TabPage_Searcher.Controls.Add(this.Split_Searcher_Root);
            this.TabPage_Searcher.Location = new System.Drawing.Point(4, 24);
            this.TabPage_Searcher.Name = "TabPage_Searcher";
            this.TabPage_Searcher.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage_Searcher.Size = new System.Drawing.Size(322, 491);
            this.TabPage_Searcher.TabIndex = 1;
            this.TabPage_Searcher.Text = "tabPage2";
            this.TabPage_Searcher.UseVisualStyleBackColor = true;
            // 
            // Split_Searcher_Root
            // 
            this.Split_Searcher_Root.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.Split_Searcher_Root.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Split_Searcher_Root.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.Split_Searcher_Root.IsSplitterFixed = true;
            this.Split_Searcher_Root.Location = new System.Drawing.Point(3, 3);
            this.Split_Searcher_Root.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Split_Searcher_Root.Name = "Split_Searcher_Root";
            this.Split_Searcher_Root.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // Split_Searcher_Root.Panel1
            // 
            this.Split_Searcher_Root.Panel1.Controls.Add(this.Split_Searcher_TextButton);
            // 
            // Split_Searcher_Root.Panel2
            // 
            this.Split_Searcher_Root.Panel2.Controls.Add(this.TreeView_Searcher);
            this.Split_Searcher_Root.Size = new System.Drawing.Size(316, 485);
            this.Split_Searcher_Root.SplitterDistance = 25;
            this.Split_Searcher_Root.SplitterWidth = 5;
            this.Split_Searcher_Root.TabIndex = 2;
            // 
            // Split_Searcher_TextButton
            // 
            this.Split_Searcher_TextButton.Cursor = System.Windows.Forms.Cursors.VSplit;
            this.Split_Searcher_TextButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Split_Searcher_TextButton.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.Split_Searcher_TextButton.IsSplitterFixed = true;
            this.Split_Searcher_TextButton.Location = new System.Drawing.Point(0, 0);
            this.Split_Searcher_TextButton.Name = "Split_Searcher_TextButton";
            // 
            // Split_Searcher_TextButton.Panel1
            // 
            this.Split_Searcher_TextButton.Panel1.Controls.Add(this.TextBox_Search);
            // 
            // Split_Searcher_TextButton.Panel2
            // 
            this.Split_Searcher_TextButton.Panel2.Controls.Add(this.Button_Search);
            this.Split_Searcher_TextButton.Size = new System.Drawing.Size(316, 25);
            this.Split_Searcher_TextButton.SplitterDistance = 269;
            this.Split_Searcher_TextButton.TabIndex = 1;
            // 
            // TextBox_Search
            // 
            this.TextBox_Search.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextBox_Search.Location = new System.Drawing.Point(0, 0);
            this.TextBox_Search.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TextBox_Search.Name = "TextBox_Search";
            this.TextBox_Search.Size = new System.Drawing.Size(269, 23);
            this.TextBox_Search.TabIndex = 3;
            this.TextBox_Search.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TextBox_Search_OnKeyUp);
            // 
            // Button_Search
            // 
            this.Button_Search.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Button_Search.Location = new System.Drawing.Point(0, 0);
            this.Button_Search.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Button_Search.Name = "Button_Search";
            this.Button_Search.Size = new System.Drawing.Size(43, 25);
            this.Button_Search.TabIndex = 0;
            this.Button_Search.Text = ">>";
            this.Button_Search.UseVisualStyleBackColor = true;
            this.Button_Search.Click += new System.EventHandler(this.Button_Search_OnClick);
            // 
            // TreeView_Searcher
            // 
            this.TreeView_Searcher.CheckBoxes = true;
            this.TreeView_Searcher.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TreeView_Searcher.HideSelection = false;
            this.TreeView_Searcher.ImageIndex = 3;
            this.TreeView_Searcher.ImageList = this.imageList1;
            this.TreeView_Searcher.Location = new System.Drawing.Point(0, 0);
            this.TreeView_Searcher.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TreeView_Searcher.Name = "TreeView_Searcher";
            this.TreeView_Searcher.SelectedImageIndex = 0;
            this.TreeView_Searcher.Size = new System.Drawing.Size(316, 455);
            this.TreeView_Searcher.TabIndex = 0;
            this.TreeView_Searcher.DoubleClick += new System.EventHandler(this.TreeView_Searcher_OnDoubleClick);
            this.TreeView_Searcher.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TreeView_Searcher_OnKeyUp);
            // 
            // DockSceneTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(330, 519);
            this.Controls.Add(this.Tab_Explorer);
            this.HideOnClose = true;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MinimumSize = new System.Drawing.Size(301, 39);
            this.Name = "DockSceneTree";
            this.TabText = "Scene Outliner";
            this.Text = "DockSceneTree";
            this.EntryMenuStrip.ResumeLayout(false);
            this.Tab_Explorer.ResumeLayout(false);
            this.TabPage_Explorer.ResumeLayout(false);
            this.TabPage_Searcher.ResumeLayout(false);
            this.Split_Searcher_Root.Panel1.ResumeLayout(false);
            this.Split_Searcher_Root.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Split_Searcher_Root)).EndInit();
            this.Split_Searcher_Root.ResumeLayout(false);
            this.Split_Searcher_TextButton.Panel1.ResumeLayout(false);
            this.Split_Searcher_TextButton.Panel1.PerformLayout();
            this.Split_Searcher_TextButton.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Split_Searcher_TextButton)).EndInit();
            this.Split_Searcher_TextButton.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip EntryMenuStrip;
        public System.Windows.Forms.ToolStripMenuItem JumpToButton;
        public System.Windows.Forms.ToolStripMenuItem DeleteButton;
        public System.Windows.Forms.ToolStripMenuItem DuplicateButton;
        public System.Windows.Forms.ToolStripMenuItem Export3DButton;
        private Mafia2Tool.Controls.MTreeView TreeView_Explorer;
        public System.Windows.Forms.ToolStripMenuItem UpdateParent1Button;
        public System.Windows.Forms.ToolStripMenuItem UpdateParent2Button;
        private System.Windows.Forms.ToolStripMenuItem FrameActions;
        public System.Windows.Forms.ToolStripMenuItem ExportFrameButton;
        public System.Windows.Forms.ToolStripMenuItem LinkToActorButton;
        private System.Windows.Forms.TabControl Tab_Explorer;
        private System.Windows.Forms.TabPage TabPage_Explorer;
        private System.Windows.Forms.TabPage TabPage_Searcher;
        private System.Windows.Forms.SplitContainer Split_Searcher_Root;
        private System.Windows.Forms.TextBox TextBox_Search;
        private System.Windows.Forms.Button Button_Search;
        private Mafia2Tool.Controls.MTreeView TreeView_Searcher;
        private System.Windows.Forms.SplitContainer Split_Searcher_TextButton;
        private System.Windows.Forms.Panel tooltipPanel;
        private System.Windows.Forms.Label tooltipText;
    }
}