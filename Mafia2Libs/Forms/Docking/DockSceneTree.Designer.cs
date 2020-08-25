
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
            this.Split_Main = new System.Windows.Forms.SplitContainer();
            this.Label_FilterFrame = new System.Windows.Forms.Label();
            this.TextBox_FilterFrame = new System.Windows.Forms.TextBox();
            this.Button_Search = new System.Windows.Forms.Button();
            this.treeView1 = new Utils.Extensions.MTreeView();
            this.EntryMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Split_Main)).BeginInit();
            this.Split_Main.Panel1.SuspendLayout();
            this.Split_Main.Panel2.SuspendLayout();
            this.Split_Main.SuspendLayout();
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
            this.EntryMenuStrip.Size = new System.Drawing.Size(181, 136);
            this.EntryMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.OpenEntryContext);
            // 
            // JumpToButton
            // 
            this.JumpToButton.Name = "JumpToButton";
            this.JumpToButton.Size = new System.Drawing.Size(180, 22);
            this.JumpToButton.Text = "Jump To Position";
            // 
            // DeleteButton
            // 
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(180, 22);
            this.DeleteButton.Text = "Delete";
            // 
            // DuplicateButton
            // 
            this.DuplicateButton.Name = "DuplicateButton";
            this.DuplicateButton.Size = new System.Drawing.Size(180, 22);
            this.DuplicateButton.Text = "Duplicate";
            // 
            // Export3DButton
            // 
            this.Export3DButton.Name = "Export3DButton";
            this.Export3DButton.Size = new System.Drawing.Size(180, 22);
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
            this.FrameActions.Size = new System.Drawing.Size(180, 22);
            this.FrameActions.Text = "Frame Actions";
            // 
            // UpdateParent1Button
            // 
            this.UpdateParent1Button.Name = "UpdateParent1Button";
            this.UpdateParent1Button.Size = new System.Drawing.Size(180, 22);
            this.UpdateParent1Button.Text = "Update Parent 1";
            // 
            // UpdateParent2Button
            // 
            this.UpdateParent2Button.Name = "UpdateParent2Button";
            this.UpdateParent2Button.Size = new System.Drawing.Size(180, 22);
            this.UpdateParent2Button.Text = "Update Parent 2";
            // 
            // ExportFrameButton
            // 
            this.ExportFrameButton.Name = "ExportFrameButton";
            this.ExportFrameButton.Size = new System.Drawing.Size(180, 22);
            this.ExportFrameButton.Text = "Export Frame";
            // 
            // LinkToActorButton
            // 
            this.LinkToActorButton.Name = "LinkToActorButton";
            this.LinkToActorButton.Size = new System.Drawing.Size(180, 22);
            this.LinkToActorButton.Text = "$LINK_TO_ACTOR";
            // 
            // imageList1
            // 
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
            // Split_Main
            // 
            this.Split_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Split_Main.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.Split_Main.Location = new System.Drawing.Point(0, 0);
            this.Split_Main.Name = "Split_Main";
            this.Split_Main.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // Split_Main.Panel1
            // 
            this.Split_Main.Panel1.Controls.Add(this.Label_FilterFrame);
            this.Split_Main.Panel1.Controls.Add(this.TextBox_FilterFrame);
            this.Split_Main.Panel1.Controls.Add(this.Button_Search);
            // 
            // Split_Main.Panel2
            // 
            this.Split_Main.Panel2.Controls.Add(this.treeView1);
            this.Split_Main.Size = new System.Drawing.Size(244, 450);
            this.Split_Main.SplitterDistance = 48;
            this.Split_Main.TabIndex = 1;
            // 
            // Label_FilterFrame
            // 
            this.Label_FilterFrame.AutoSize = true;
            this.Label_FilterFrame.Location = new System.Drawing.Point(3, 5);
            this.Label_FilterFrame.Name = "Label_FilterFrame";
            this.Label_FilterFrame.Size = new System.Drawing.Size(105, 13);
            this.Label_FilterFrame.TabIndex = 4;
            this.Label_FilterFrame.Text = "Filter By Name/Hash";
            // 
            // TextBox_FilterFrame
            // 
            this.TextBox_FilterFrame.Enabled = false;
            this.TextBox_FilterFrame.Location = new System.Drawing.Point(3, 21);
            this.TextBox_FilterFrame.Name = "TextBox_FilterFrame";
            this.TextBox_FilterFrame.Size = new System.Drawing.Size(194, 20);
            this.TextBox_FilterFrame.TabIndex = 3;
            // 
            // Button_Search
            // 
            this.Button_Search.Enabled = false;
            this.Button_Search.Location = new System.Drawing.Point(203, 19);
            this.Button_Search.Name = "Button_Search";
            this.Button_Search.Size = new System.Drawing.Size(28, 23);
            this.Button_Search.TabIndex = 0;
            this.Button_Search.Text = ">>";
            this.Button_Search.UseVisualStyleBackColor = true;
            this.Button_Search.Click += new System.EventHandler(this.button1_Click);
            // 
            // treeView1
            // 
            this.treeView1.CheckBoxes = true;
            this.treeView1.ContextMenuStrip = this.EntryMenuStrip;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.HideSelection = false;
            this.treeView1.ImageIndex = 3;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(244, 398);
            this.treeView1.TabIndex = 0;
            this.treeView1.DoubleClick += new System.EventHandler(this.OnDoubleClick);
            // 
            // DockSceneTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(244, 450);
            this.Controls.Add(this.Split_Main);
            this.HideOnClose = true;
            this.MinimumSize = new System.Drawing.Size(260, 39);
            this.Name = "DockSceneTree";
            this.TabText = "Scene Outliner";
            this.Text = "DockSceneTree";
            this.EntryMenuStrip.ResumeLayout(false);
            this.Split_Main.Panel1.ResumeLayout(false);
            this.Split_Main.Panel1.PerformLayout();
            this.Split_Main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Split_Main)).EndInit();
            this.Split_Main.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip EntryMenuStrip;
        public System.Windows.Forms.ToolStripMenuItem JumpToButton;
        public System.Windows.Forms.ToolStripMenuItem DeleteButton;
        public System.Windows.Forms.ToolStripMenuItem DuplicateButton;
        public System.Windows.Forms.ToolStripMenuItem Export3DButton;
        private Utils.Extensions.MTreeView treeView1;
        public System.Windows.Forms.ToolStripMenuItem UpdateParent1Button;
        public System.Windows.Forms.ToolStripMenuItem UpdateParent2Button;
        private System.Windows.Forms.ToolStripMenuItem FrameActions;
        public System.Windows.Forms.ToolStripMenuItem ExportFrameButton;
        public System.Windows.Forms.ToolStripMenuItem LinkToActorButton;
        private System.Windows.Forms.SplitContainer Split_Main;
        private System.Windows.Forms.TextBox TextBox_FilterFrame;
        private System.Windows.Forms.Button Button_Search;
        private System.Windows.Forms.Label Label_FilterFrame;
    }
}