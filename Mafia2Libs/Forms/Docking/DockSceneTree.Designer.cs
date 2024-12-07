
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DockSceneTree));
            EntryMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            JumpToButton = new System.Windows.Forms.ToolStripMenuItem();
            DeleteButton = new System.Windows.Forms.ToolStripMenuItem();
            DuplicateButton = new System.Windows.Forms.ToolStripMenuItem();
            Export3DButton = new System.Windows.Forms.ToolStripMenuItem();
            FrameActions = new System.Windows.Forms.ToolStripMenuItem();
            UpdateParent1Button = new System.Windows.Forms.ToolStripMenuItem();
            UpdateParent2Button = new System.Windows.Forms.ToolStripMenuItem();
            ExportFrameButton = new System.Windows.Forms.ToolStripMenuItem();
            LinkToActorButton = new System.Windows.Forms.ToolStripMenuItem();
            TranslokatorNewInstanceButton = new System.Windows.Forms.ToolStripMenuItem();
            ActorEntryNewTRObjectButton = new System.Windows.Forms.ToolStripMenuItem();
            TRRebuildObjectButton = new System.Windows.Forms.ToolStripMenuItem();
            imageList1 = new System.Windows.Forms.ImageList(components);
            TreeView_Explorer = new Mafia2Tool.Controls.MTreeView();
            Tab_Explorer = new System.Windows.Forms.TabControl();
            TabPage_Explorer = new System.Windows.Forms.TabPage();
            tooltipPanel = new System.Windows.Forms.Panel();
            tooltipText = new System.Windows.Forms.Label();
            TabPage_Searcher = new System.Windows.Forms.TabPage();
            Split_Searcher_Root = new System.Windows.Forms.SplitContainer();
            Split_Searcher_TextButton = new System.Windows.Forms.SplitContainer();
            TextBox_Search = new System.Windows.Forms.TextBox();
            Button_Search = new System.Windows.Forms.Button();
            TreeView_Searcher = new Mafia2Tool.Controls.MTreeView();
            EntryMenuStrip.SuspendLayout();
            Tab_Explorer.SuspendLayout();
            TabPage_Explorer.SuspendLayout();
            tooltipPanel.SuspendLayout();
            TabPage_Searcher.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Split_Searcher_Root).BeginInit();
            Split_Searcher_Root.Panel1.SuspendLayout();
            Split_Searcher_Root.Panel2.SuspendLayout();
            Split_Searcher_Root.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Split_Searcher_TextButton).BeginInit();
            Split_Searcher_TextButton.Panel1.SuspendLayout();
            Split_Searcher_TextButton.Panel2.SuspendLayout();
            Split_Searcher_TextButton.SuspendLayout();
            SuspendLayout();
            // 
            // EntryMenuStrip
            // 
            EntryMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { JumpToButton, DeleteButton, DuplicateButton, Export3DButton, FrameActions, TranslokatorNewInstanceButton, ActorEntryNewTRObjectButton,TRRebuildObjectButton });
            EntryMenuStrip.Name = "EntryMenuStrip";
            EntryMenuStrip.Size = new System.Drawing.Size(204, 158);
            EntryMenuStrip.Opening += OpenEntryContext;
            // 
            // JumpToButton
            // 
            JumpToButton.Name = "JumpToButton";
            JumpToButton.Size = new System.Drawing.Size(203, 22);
            JumpToButton.Text = "Jump To Position";
            // 
            // DeleteButton
            // 
            DeleteButton.Name = "DeleteButton";
            DeleteButton.Size = new System.Drawing.Size(203, 22);
            DeleteButton.Text = "Delete";
            // 
            // DuplicateButton
            // 
            DuplicateButton.Name = "DuplicateButton";
            DuplicateButton.Size = new System.Drawing.Size(203, 22);
            DuplicateButton.Text = "Duplicate";
            // 
            // Export3DButton
            // 
            Export3DButton.Name = "Export3DButton";
            Export3DButton.Size = new System.Drawing.Size(203, 22);
            Export3DButton.Text = "Export 3D";
            // 
            // FrameActions
            // 
            FrameActions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { UpdateParent1Button, UpdateParent2Button, ExportFrameButton, LinkToActorButton });
            FrameActions.Name = "FrameActions";
            FrameActions.Size = new System.Drawing.Size(203, 22);
            FrameActions.Text = "Frame Actions";
            // 
            // UpdateParent1Button
            // 
            UpdateParent1Button.Name = "UpdateParent1Button";
            UpdateParent1Button.Size = new System.Drawing.Size(166, 22);
            UpdateParent1Button.Text = "Update Parent 1";
            // 
            // UpdateParent2Button
            // 
            UpdateParent2Button.Name = "UpdateParent2Button";
            UpdateParent2Button.Size = new System.Drawing.Size(166, 22);
            UpdateParent2Button.Text = "Update Parent 2";
            // 
            // ExportFrameButton
            // 
            ExportFrameButton.Name = "ExportFrameButton";
            ExportFrameButton.Size = new System.Drawing.Size(166, 22);
            ExportFrameButton.Text = "Export Frame";
            // 
            // LinkToActorButton
            // 
            LinkToActorButton.Name = "LinkToActorButton";
            LinkToActorButton.Size = new System.Drawing.Size(166, 22);
            LinkToActorButton.Text = "$LINK_TO_ACTOR";
            // 
            // TranslokatorNewInstanceButton
            // 
            TranslokatorNewInstanceButton.Name = "TranslokatorNewInstanceButton";
            TranslokatorNewInstanceButton.Size = new System.Drawing.Size(203, 22);
            TranslokatorNewInstanceButton.Text = "New Instance";
            // 
            // TRRebuildObjectButton
            // 
            TRRebuildObjectButton.Name = "TRRebuildObjectButton";
            TRRebuildObjectButton.Size = new System.Drawing.Size(203, 22);
            TRRebuildObjectButton.Text = "Rebuild Object";
            // 
            // ActorEntryNewTRObjectButton
            // 
            ActorEntryNewTRObjectButton.Name = "ActorEntryNewTRObjectButton";
            ActorEntryNewTRObjectButton.Size = new System.Drawing.Size(203, 22);
            ActorEntryNewTRObjectButton.Text = "New Translokator Object";
            // 
            // imageList1
            // 
            imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            imageList1.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = System.Drawing.Color.Transparent;
            imageList1.Images.SetKeyName(0, "ActorFrame.png");
            imageList1.Images.SetKeyName(1, "AreaFrame.png");
            imageList1.Images.SetKeyName(2, "CameraFrame.png");
            imageList1.Images.SetKeyName(3, "CollisionFrame.png");
            imageList1.Images.SetKeyName(4, "CollisionObject.png");
            imageList1.Images.SetKeyName(5, "LightFrame.png");
            imageList1.Images.SetKeyName(6, "MeshFrame.png");
            imageList1.Images.SetKeyName(7, "Placeholder.png");
            imageList1.Images.SetKeyName(8, "SceneObject.png");
            imageList1.Images.SetKeyName(9, "SkinnedFrame.png");
            imageList1.Images.SetKeyName(10, "DummyFrame.png");
            imageList1.Images.SetKeyName(11, "grid.png");
            imageList1.Images.SetKeyName(12, "instance.png");
            imageList1.Images.SetKeyName(13, "object.png");
            // 
            // TreeView_Explorer
            // 
            TreeView_Explorer.AllowDrop = true;
            TreeView_Explorer.CheckBoxes = true;
            TreeView_Explorer.ContextMenuStrip = EntryMenuStrip;
            TreeView_Explorer.Dock = System.Windows.Forms.DockStyle.Fill;
            TreeView_Explorer.HideSelection = false;
            TreeView_Explorer.ImageIndex = 3;
            TreeView_Explorer.ImageList = imageList1;
            TreeView_Explorer.Location = new System.Drawing.Point(3, 3);
            TreeView_Explorer.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TreeView_Explorer.Name = "TreeView_Explorer";
            TreeView_Explorer.SelectedImageIndex = 0;
            TreeView_Explorer.Size = new System.Drawing.Size(316, 435);
            TreeView_Explorer.TabIndex = 0;
            TreeView_Explorer.ItemDrag += TreeView_Explorer_ItemDrag;
            TreeView_Explorer.DragDrop += TreeView_Explorer_DragDrop;
            TreeView_Explorer.DragEnter += TreeView_Explorer_DragEnter;
            TreeView_Explorer.DragOver += TreeView_Explorer_DragOver;
            TreeView_Explorer.DragLeave += TreeView_Explorer_DragLeave;
            TreeView_Explorer.DoubleClick += OnDoubleClick;
            TreeView_Explorer.MouseDown += TreeView_Explorer_MouseDown;
            // 
            // Tab_Explorer
            // 
            Tab_Explorer.Controls.Add(TabPage_Explorer);
            Tab_Explorer.Controls.Add(TabPage_Searcher);
            Tab_Explorer.Dock = System.Windows.Forms.DockStyle.Fill;
            Tab_Explorer.Location = new System.Drawing.Point(0, 0);
            Tab_Explorer.Name = "Tab_Explorer";
            Tab_Explorer.SelectedIndex = 0;
            Tab_Explorer.Size = new System.Drawing.Size(330, 519);
            Tab_Explorer.TabIndex = 1;
            // 
            // TabPage_Explorer
            // 
            TabPage_Explorer.Controls.Add(TreeView_Explorer);
            TabPage_Explorer.Controls.Add(tooltipPanel);
            TabPage_Explorer.Location = new System.Drawing.Point(4, 24);
            TabPage_Explorer.Name = "TabPage_Explorer";
            TabPage_Explorer.Padding = new System.Windows.Forms.Padding(3);
            TabPage_Explorer.Size = new System.Drawing.Size(322, 491);
            TabPage_Explorer.TabIndex = 0;
            TabPage_Explorer.Text = "tabPage1";
            TabPage_Explorer.UseVisualStyleBackColor = true;
            // 
            // tooltipPanel
            // 
            tooltipPanel.BackColor = System.Drawing.Color.Silver;
            tooltipPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            tooltipPanel.Controls.Add(tooltipText);
            tooltipPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            tooltipPanel.Location = new System.Drawing.Point(3, 438);
            tooltipPanel.Name = "tooltipPanel";
            tooltipPanel.Size = new System.Drawing.Size(316, 50);
            tooltipPanel.TabIndex = 1;
            // 
            // tooltipText
            // 
            tooltipText.AutoSize = true;
            tooltipText.Location = new System.Drawing.Point(-2, 0);
            tooltipText.Name = "tooltipText";
            tooltipText.Size = new System.Drawing.Size(266, 45);
            tooltipText.TabIndex = 0;
            tooltipText.Text = "Drag with Left Mouse Button to set Parent1\r\nDrag with Right Mouse Button to set Parent2\r\nDrag with Middle Mouse Button to switch frames";
            // 
            // TabPage_Searcher
            // 
            TabPage_Searcher.Controls.Add(Split_Searcher_Root);
            TabPage_Searcher.Location = new System.Drawing.Point(4, 24);
            TabPage_Searcher.Name = "TabPage_Searcher";
            TabPage_Searcher.Padding = new System.Windows.Forms.Padding(3);
            TabPage_Searcher.Size = new System.Drawing.Size(322, 491);
            TabPage_Searcher.TabIndex = 1;
            TabPage_Searcher.Text = "tabPage2";
            TabPage_Searcher.UseVisualStyleBackColor = true;
            // 
            // Split_Searcher_Root
            // 
            Split_Searcher_Root.Cursor = System.Windows.Forms.Cursors.HSplit;
            Split_Searcher_Root.Dock = System.Windows.Forms.DockStyle.Fill;
            Split_Searcher_Root.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            Split_Searcher_Root.IsSplitterFixed = true;
            Split_Searcher_Root.Location = new System.Drawing.Point(3, 3);
            Split_Searcher_Root.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Split_Searcher_Root.Name = "Split_Searcher_Root";
            Split_Searcher_Root.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // Split_Searcher_Root.Panel1
            // 
            Split_Searcher_Root.Panel1.Controls.Add(Split_Searcher_TextButton);
            // 
            // Split_Searcher_Root.Panel2
            // 
            Split_Searcher_Root.Panel2.Controls.Add(TreeView_Searcher);
            Split_Searcher_Root.Size = new System.Drawing.Size(316, 485);
            Split_Searcher_Root.SplitterDistance = 25;
            Split_Searcher_Root.SplitterWidth = 5;
            Split_Searcher_Root.TabIndex = 2;
            // 
            // Split_Searcher_TextButton
            // 
            Split_Searcher_TextButton.Cursor = System.Windows.Forms.Cursors.VSplit;
            Split_Searcher_TextButton.Dock = System.Windows.Forms.DockStyle.Fill;
            Split_Searcher_TextButton.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            Split_Searcher_TextButton.IsSplitterFixed = true;
            Split_Searcher_TextButton.Location = new System.Drawing.Point(0, 0);
            Split_Searcher_TextButton.Name = "Split_Searcher_TextButton";
            // 
            // Split_Searcher_TextButton.Panel1
            // 
            Split_Searcher_TextButton.Panel1.Controls.Add(TextBox_Search);
            // 
            // Split_Searcher_TextButton.Panel2
            // 
            Split_Searcher_TextButton.Panel2.Controls.Add(Button_Search);
            Split_Searcher_TextButton.Size = new System.Drawing.Size(316, 25);
            Split_Searcher_TextButton.SplitterDistance = 269;
            Split_Searcher_TextButton.TabIndex = 1;
            // 
            // TextBox_Search
            // 
            TextBox_Search.Dock = System.Windows.Forms.DockStyle.Fill;
            TextBox_Search.Location = new System.Drawing.Point(0, 0);
            TextBox_Search.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBox_Search.Name = "TextBox_Search";
            TextBox_Search.Size = new System.Drawing.Size(269, 23);
            TextBox_Search.TabIndex = 3;
            TextBox_Search.KeyUp += TextBox_Search_OnKeyUp;
            // 
            // Button_Search
            // 
            Button_Search.Dock = System.Windows.Forms.DockStyle.Fill;
            Button_Search.Location = new System.Drawing.Point(0, 0);
            Button_Search.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Button_Search.Name = "Button_Search";
            Button_Search.Size = new System.Drawing.Size(43, 25);
            Button_Search.TabIndex = 0;
            Button_Search.Text = ">>";
            Button_Search.UseVisualStyleBackColor = true;
            Button_Search.Click += Button_Search_OnClick;
            // 
            // TreeView_Searcher
            // 
            TreeView_Searcher.CheckBoxes = true;
            TreeView_Searcher.Dock = System.Windows.Forms.DockStyle.Fill;
            TreeView_Searcher.HideSelection = false;
            TreeView_Searcher.ImageIndex = 3;
            TreeView_Searcher.ImageList = imageList1;
            TreeView_Searcher.Location = new System.Drawing.Point(0, 0);
            TreeView_Searcher.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TreeView_Searcher.Name = "TreeView_Searcher";
            TreeView_Searcher.SelectedImageIndex = 0;
            TreeView_Searcher.Size = new System.Drawing.Size(316, 455);
            TreeView_Searcher.TabIndex = 0;
            TreeView_Searcher.DoubleClick += TreeView_Searcher_OnDoubleClick;
            TreeView_Searcher.KeyUp += TreeView_Searcher_OnKeyUp;
            // 
            // DockSceneTree
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(330, 519);
            Controls.Add(Tab_Explorer);
            HideOnClose = true;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MinimumSize = new System.Drawing.Size(301, 39);
            Name = "DockSceneTree";
            TabText = "Scene Outliner";
            Text = "DockSceneTree";
            EntryMenuStrip.ResumeLayout(false);
            Tab_Explorer.ResumeLayout(false);
            TabPage_Explorer.ResumeLayout(false);
            tooltipPanel.ResumeLayout(false);
            tooltipPanel.PerformLayout();
            TabPage_Searcher.ResumeLayout(false);
            Split_Searcher_Root.Panel1.ResumeLayout(false);
            Split_Searcher_Root.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)Split_Searcher_Root).EndInit();
            Split_Searcher_Root.ResumeLayout(false);
            Split_Searcher_TextButton.Panel1.ResumeLayout(false);
            Split_Searcher_TextButton.Panel1.PerformLayout();
            Split_Searcher_TextButton.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)Split_Searcher_TextButton).EndInit();
            Split_Searcher_TextButton.ResumeLayout(false);
            ResumeLayout(false);
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
        public System.Windows.Forms.ToolStripMenuItem TranslokatorNewInstanceButton;
        public System.Windows.Forms.ToolStripMenuItem ActorEntryNewTRObjectButton;
        public System.Windows.Forms.ToolStripMenuItem TRRebuildObjectButton;
    }
}