namespace Forms.Docking
{
    partial class DockViewProperties
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DockViewProperties));
            this.EntryMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.PreviewButton = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteButton = new System.Windows.Forms.ToolStripMenuItem();
            this.DuplicateButton = new System.Windows.Forms.ToolStripMenuItem();
            this.Export3DButton = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.Label_PickIntersection = new System.Windows.Forms.Label();
            this.TextBox_PickWSLocation = new System.Windows.Forms.TextBox();
            this.EntryMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // EntryMenuStrip
            // 
            this.EntryMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PreviewButton,
            this.DeleteButton,
            this.DuplicateButton,
            this.Export3DButton});
            this.EntryMenuStrip.Name = "EntryMenuStrip";
            this.EntryMenuStrip.Size = new System.Drawing.Size(126, 92);
            // 
            // PreviewButton
            // 
            this.PreviewButton.Name = "PreviewButton";
            this.PreviewButton.Size = new System.Drawing.Size(125, 22);
            this.PreviewButton.Text = "Preview";
            // 
            // DeleteButton
            // 
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(125, 22);
            this.DeleteButton.Text = "Delete";
            // 
            // DuplicateButton
            // 
            this.DuplicateButton.Name = "DuplicateButton";
            this.DuplicateButton.Size = new System.Drawing.Size(125, 22);
            this.DuplicateButton.Text = "Duplicate";
            // 
            // Export3DButton
            // 
            this.Export3DButton.Name = "Export3DButton";
            this.Export3DButton.Size = new System.Drawing.Size(125, 22);
            this.Export3DButton.Text = "Export 3D";
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
            // Label_PickIntersection
            // 
            this.Label_PickIntersection.AutoSize = true;
            this.Label_PickIntersection.Location = new System.Drawing.Point(10, 10);
            this.Label_PickIntersection.Name = "Label_PickIntersection";
            this.Label_PickIntersection.Size = new System.Drawing.Size(118, 15);
            this.Label_PickIntersection.TabIndex = 1;
            this.Label_PickIntersection.Text = "Last Pick Intersection";
            // 
            // TextBox_PickWSLocation
            // 
            this.TextBox_PickWSLocation.Location = new System.Drawing.Point(10, 28);
            this.TextBox_PickWSLocation.Name = "TextBox_PickWSLocation";
            this.TextBox_PickWSLocation.Size = new System.Drawing.Size(288, 23);
            this.TextBox_PickWSLocation.TabIndex = 3;
            // 
            // DockViewProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(310, 519);
            this.Controls.Add(this.TextBox_PickWSLocation);
            this.Controls.Add(this.Label_PickIntersection);
            this.HideOnClose = true;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "DockViewProperties";
            this.TabText = "View Properties";
            this.Text = "DockViewProperties";
            this.Resize += new System.EventHandler(this.OnResize);
            this.EntryMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip EntryMenuStrip;
        public System.Windows.Forms.ToolStripMenuItem PreviewButton;
        public System.Windows.Forms.ToolStripMenuItem DeleteButton;
        public System.Windows.Forms.ToolStripMenuItem DuplicateButton;
        public System.Windows.Forms.ToolStripMenuItem Export3DButton;
        private System.Windows.Forms.Label Label_PickIntersection;
        private System.Windows.Forms.TextBox TextBox_PickWSLocation;
    }
}