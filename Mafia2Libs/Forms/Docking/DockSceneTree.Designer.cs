
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
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.treeView1 = new Utils.Extensions.MTreeView();
            this.LinkToActorButton = new System.Windows.Forms.ToolStripMenuItem();
            this.EntryMenuStrip.SuspendLayout();
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
            this.treeView1.Size = new System.Drawing.Size(266, 450);
            this.treeView1.TabIndex = 0;
            this.treeView1.DoubleClick += new System.EventHandler(this.OnDoubleClick);
            // 
            // LinkToActorButton
            // 
            this.LinkToActorButton.Name = "LinkToActorButton";
            this.LinkToActorButton.Size = new System.Drawing.Size(166, 22);
            this.LinkToActorButton.Text = "$LINK_TO_ACTOR";
            // 
            // DockSceneTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(266, 450);
            this.Controls.Add(this.treeView1);
            this.HideOnClose = true;
            this.Name = "DockSceneTree";
            this.TabText = "Scene Outliner";
            this.Text = "DockSceneTree";
            this.EntryMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip EntryMenuStrip;
        public System.Windows.Forms.ToolStripMenuItem JumpToButton;
        public System.Windows.Forms.ToolStripMenuItem DeleteButton;
        public System.Windows.Forms.ToolStripMenuItem DuplicateButton;
        public System.Windows.Forms.ToolStripMenuItem Export3DButton;
        public Utils.Extensions.MTreeView treeView1;
        public System.Windows.Forms.ToolStripMenuItem UpdateParent1Button;
        public System.Windows.Forms.ToolStripMenuItem UpdateParent2Button;
        private System.Windows.Forms.ToolStripMenuItem FrameActions;
        public System.Windows.Forms.ToolStripMenuItem ExportFrameButton;
        public System.Windows.Forms.ToolStripMenuItem LinkToActorButton;
    }
}