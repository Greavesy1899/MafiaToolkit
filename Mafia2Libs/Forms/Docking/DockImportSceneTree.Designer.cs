
using System;

namespace Forms.Docking
{
    partial class DockImportSceneTree
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DockImportSceneTree));
            imageList1 = new System.Windows.Forms.ImageList(components);
            TreeView_Explorer = new Mafia2Tool.Controls.MTreeView();
            Tab_Explorer = new System.Windows.Forms.TabControl();
            TabPage_Explorer = new System.Windows.Forms.TabPage();
            TabPage_Searcher = new System.Windows.Forms.TabPage();
            Split_Searcher_Root = new System.Windows.Forms.SplitContainer();
            Split_Searcher_TextButton = new System.Windows.Forms.SplitContainer();
            TextBox_Search = new System.Windows.Forms.TextBox();
            Button_Search = new System.Windows.Forms.Button();
            TreeView_Searcher = new Mafia2Tool.Controls.MTreeView();
            importButton = new System.Windows.Forms.Button();
            cancelButton = new System.Windows.Forms.Button();
            Tab_Explorer.SuspendLayout();
            TabPage_Explorer.SuspendLayout();
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
            // 
            // TreeView_Explorer
            // 
            //TreeView_Explorer.CheckBoxes = true; until batch import is done
            TreeView_Explorer.Dock = System.Windows.Forms.DockStyle.Top;
            TreeView_Explorer.HideSelection = false;
            TreeView_Explorer.ImageIndex = 3;
            TreeView_Explorer.ImageList = imageList1;
            TreeView_Explorer.Location = new System.Drawing.Point(3, 3);
            TreeView_Explorer.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TreeView_Explorer.Name = "TreeView_Explorer";
            TreeView_Explorer.SelectedImageIndex = 0;
            TreeView_Explorer.Size = new System.Drawing.Size(316, 463);
            TreeView_Explorer.TabIndex = 0;
            TreeView_Explorer.DoubleClick += OnDoubleClick;
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
            TabPage_Explorer.Controls.Add(cancelButton);
            TabPage_Explorer.Controls.Add(importButton);
            TabPage_Explorer.Controls.Add(TreeView_Explorer);
            TabPage_Explorer.Location = new System.Drawing.Point(4, 24);
            TabPage_Explorer.Name = "TabPage_Explorer";
            TabPage_Explorer.Padding = new System.Windows.Forms.Padding(3);
            TabPage_Explorer.Size = new System.Drawing.Size(322, 491);
            TabPage_Explorer.TabIndex = 0;
            TabPage_Explorer.Text = "tabPage1";
            TabPage_Explorer.UseVisualStyleBackColor = true;
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
            //TreeView_Searcher.CheckBoxes = true; until batch import is done
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
            // importButton
            // 
            importButton.Location = new System.Drawing.Point(244, 468);
            importButton.Name = "importButton";
            importButton.Size = new System.Drawing.Size(75, 23);
            importButton.TabIndex = 1;
            importButton.Text = "Import Selected";
            importButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            cancelButton.Location = new System.Drawing.Point(3, 468);
            cancelButton.Name = "Cancel";
            cancelButton.Size = new System.Drawing.Size(75, 23);
            cancelButton.TabIndex = 2;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += CancelButton_OnClick;
            // 
            // DockImportSceneTree
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(330, 519);
            Controls.Add(Tab_Explorer);
            HideOnClose = true;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MinimumSize = new System.Drawing.Size(301, 39);
            Name = "DockImportSceneTree";
            TabText = "Scene Outliner";
            Text = "DockImportSceneTree";
            Tab_Explorer.ResumeLayout(false);
            TabPage_Explorer.ResumeLayout(false);
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
        private Mafia2Tool.Controls.MTreeView TreeView_Explorer;
        private System.Windows.Forms.TabControl Tab_Explorer;
        private System.Windows.Forms.TabPage TabPage_Explorer;
        private System.Windows.Forms.TabPage TabPage_Searcher;
        private System.Windows.Forms.SplitContainer Split_Searcher_Root;
        private System.Windows.Forms.TextBox TextBox_Search;
        private System.Windows.Forms.Button Button_Search;
        private Mafia2Tool.Controls.MTreeView TreeView_Searcher;
        private System.Windows.Forms.SplitContainer Split_Searcher_TextButton;
        public System.Windows.Forms.Button cancelButton;
        public System.Windows.Forms.Button importButton;
    }
}