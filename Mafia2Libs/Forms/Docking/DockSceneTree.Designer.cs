
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
            this.treeView1 = new Mafia2Tool.Controls.MTreeView();
            this.Tab_Explorer = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.Label_SearchByName = new System.Windows.Forms.Label();
            this.TextBox_Search = new System.Windows.Forms.TextBox();
            this.Button_Search = new System.Windows.Forms.Button();
            this.mTreeView1 = new Mafia2Tool.Controls.MTreeView();
            this.SearchContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Button_JumpToTreeView = new System.Windows.Forms.ToolStripMenuItem();
            this.EntryMenuStrip.SuspendLayout();
            this.Tab_Explorer.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SearchContextMenu.SuspendLayout();
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
            // treeView1
            // 
            this.treeView1.CheckBoxes = true;
            this.treeView1.ContextMenuStrip = this.EntryMenuStrip;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.HideSelection = false;
            this.treeView1.ImageIndex = 3;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.Location = new System.Drawing.Point(3, 3);
            this.treeView1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(271, 485);
            this.treeView1.TabIndex = 0;
            this.treeView1.DoubleClick += new System.EventHandler(this.OnDoubleClick);
            // 
            // Tab_Explorer
            // 
            this.Tab_Explorer.Controls.Add(this.tabPage1);
            this.Tab_Explorer.Controls.Add(this.tabPage2);
            this.Tab_Explorer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tab_Explorer.Location = new System.Drawing.Point(0, 0);
            this.Tab_Explorer.Name = "Tab_Explorer";
            this.Tab_Explorer.SelectedIndex = 0;
            this.Tab_Explorer.Size = new System.Drawing.Size(285, 519);
            this.Tab_Explorer.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.treeView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(277, 491);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.splitContainer1);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(277, 491);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.Label_SearchByName);
            this.splitContainer1.Panel1.Controls.Add(this.TextBox_Search);
            this.splitContainer1.Panel1.Controls.Add(this.Button_Search);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.mTreeView1);
            this.splitContainer1.Size = new System.Drawing.Size(271, 485);
            this.splitContainer1.SplitterDistance = 48;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 2;
            // 
            // Label_SearchByName
            // 
            this.Label_SearchByName.AutoSize = true;
            this.Label_SearchByName.Location = new System.Drawing.Point(4, 6);
            this.Label_SearchByName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label_SearchByName.Name = "Label_SearchByName";
            this.Label_SearchByName.Size = new System.Drawing.Size(93, 15);
            this.Label_SearchByName.TabIndex = 4;
            this.Label_SearchByName.Text = "Search By Name";
            // 
            // TextBox_Search
            // 
            this.TextBox_Search.Location = new System.Drawing.Point(4, 24);
            this.TextBox_Search.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TextBox_Search.Name = "TextBox_Search";
            this.TextBox_Search.Size = new System.Drawing.Size(226, 23);
            this.TextBox_Search.TabIndex = 3;
            // 
            // Button_Search
            // 
            this.Button_Search.Location = new System.Drawing.Point(237, 22);
            this.Button_Search.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Button_Search.Name = "Button_Search";
            this.Button_Search.Size = new System.Drawing.Size(33, 27);
            this.Button_Search.TabIndex = 0;
            this.Button_Search.Text = ">>";
            this.Button_Search.UseVisualStyleBackColor = true;
            this.Button_Search.Click += new System.EventHandler(this.Button_Search_OnClick);
            // 
            // mTreeView1
            // 
            this.mTreeView1.CheckBoxes = true;
            this.mTreeView1.ContextMenuStrip = this.SearchContextMenu;
            this.mTreeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mTreeView1.HideSelection = false;
            this.mTreeView1.ImageIndex = 3;
            this.mTreeView1.ImageList = this.imageList1;
            this.mTreeView1.Location = new System.Drawing.Point(0, 0);
            this.mTreeView1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.mTreeView1.Name = "mTreeView1";
            this.mTreeView1.SelectedImageIndex = 0;
            this.mTreeView1.Size = new System.Drawing.Size(271, 432);
            this.mTreeView1.TabIndex = 0;
            // 
            // SearchContextMenu
            // 
            this.SearchContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Button_JumpToTreeView});
            this.SearchContextMenu.Name = "SearchContextMenu";
            this.SearchContextMenu.Size = new System.Drawing.Size(187, 26);
            this.SearchContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.SearchContextMenu_Opening);
            // 
            // Button_JumpToTreeView
            // 
            this.Button_JumpToTreeView.Name = "Button_JumpToTreeView";
            this.Button_JumpToTreeView.Size = new System.Drawing.Size(186, 22);
            this.Button_JumpToTreeView.Text = "$JUMP_TO_TREEVIEW";
            this.Button_JumpToTreeView.Click += new System.EventHandler(this.Button_JumpToTreeView_OnClick);
            // 
            // DockSceneTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(285, 519);
            this.Controls.Add(this.Tab_Explorer);
            this.HideOnClose = true;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MinimumSize = new System.Drawing.Size(301, 39);
            this.Name = "DockSceneTree";
            this.TabText = "Scene Outliner";
            this.Text = "DockSceneTree";
            this.EntryMenuStrip.ResumeLayout(false);
            this.Tab_Explorer.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.SearchContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip EntryMenuStrip;
        public System.Windows.Forms.ToolStripMenuItem JumpToButton;
        public System.Windows.Forms.ToolStripMenuItem DeleteButton;
        public System.Windows.Forms.ToolStripMenuItem DuplicateButton;
        public System.Windows.Forms.ToolStripMenuItem Export3DButton;
        private Mafia2Tool.Controls.MTreeView treeView1;
        public System.Windows.Forms.ToolStripMenuItem UpdateParent1Button;
        public System.Windows.Forms.ToolStripMenuItem UpdateParent2Button;
        private System.Windows.Forms.ToolStripMenuItem FrameActions;
        public System.Windows.Forms.ToolStripMenuItem ExportFrameButton;
        public System.Windows.Forms.ToolStripMenuItem LinkToActorButton;
        private System.Windows.Forms.TabControl Tab_Explorer;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label Label_SearchByName;
        private System.Windows.Forms.TextBox TextBox_Search;
        private System.Windows.Forms.Button Button_Search;
        private Mafia2Tool.Controls.MTreeView mTreeView1;
        private System.Windows.Forms.ContextMenuStrip SearchContextMenu;
        private System.Windows.Forms.ToolStripMenuItem Button_JumpToTreeView;
    }
}