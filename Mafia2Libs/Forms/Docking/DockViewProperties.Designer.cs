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
            this.TextBox_WithOffset = new System.Windows.Forms.TextBox();
            this.Label_IntersectionWithOffset = new System.Windows.Forms.Label();
            this.Numeric_PosZ = new System.Windows.Forms.NumericUpDown();
            this.Numeric_PosY = new System.Windows.Forms.NumericUpDown();
            this.Numeric_PosX = new System.Windows.Forms.NumericUpDown();
            this.Label_PosZ = new System.Windows.Forms.Label();
            this.Label_PosY = new System.Windows.Forms.Label();
            this.Label_PosX = new System.Windows.Forms.Label();
            this.EntryMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Numeric_PosZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Numeric_PosY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Numeric_PosX)).BeginInit();
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
            // TextBox_WithOffset
            // 
            this.TextBox_WithOffset.Location = new System.Drawing.Point(9, 176);
            this.TextBox_WithOffset.Name = "TextBox_WithOffset";
            this.TextBox_WithOffset.Size = new System.Drawing.Size(288, 23);
            this.TextBox_WithOffset.TabIndex = 7;
            // 
            // Label_IntersectionWithOffset
            // 
            this.Label_IntersectionWithOffset.AutoSize = true;
            this.Label_IntersectionWithOffset.Location = new System.Drawing.Point(9, 158);
            this.Label_IntersectionWithOffset.Name = "Label_IntersectionWithOffset";
            this.Label_IntersectionWithOffset.Size = new System.Drawing.Size(136, 15);
            this.Label_IntersectionWithOffset.TabIndex = 6;
            this.Label_IntersectionWithOffset.Text = "Intersection WITH Offset";
            // 
            // Numeric_PosZ
            // 
            this.Numeric_PosZ.DecimalPlaces = 5;
            this.Numeric_PosZ.Location = new System.Drawing.Point(81, 123);
            this.Numeric_PosZ.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Numeric_PosZ.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.Numeric_PosZ.Minimum = new decimal(new int[] {
            999999999,
            0,
            0,
            -2147483648});
            this.Numeric_PosZ.Name = "Numeric_PosZ";
            this.Numeric_PosZ.Size = new System.Drawing.Size(216, 23);
            this.Numeric_PosZ.TabIndex = 26;
            this.Numeric_PosZ.ValueChanged += new System.EventHandler(this.Numeric_OnValueChanged);
            // 
            // Numeric_PosY
            // 
            this.Numeric_PosY.DecimalPlaces = 5;
            this.Numeric_PosY.Location = new System.Drawing.Point(81, 93);
            this.Numeric_PosY.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Numeric_PosY.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.Numeric_PosY.Minimum = new decimal(new int[] {
            999999999,
            0,
            0,
            -2147483648});
            this.Numeric_PosY.Name = "Numeric_PosY";
            this.Numeric_PosY.Size = new System.Drawing.Size(216, 23);
            this.Numeric_PosY.TabIndex = 25;
            this.Numeric_PosY.ValueChanged += new System.EventHandler(this.Numeric_OnValueChanged);
            // 
            // Numeric_PosX
            // 
            this.Numeric_PosX.DecimalPlaces = 5;
            this.Numeric_PosX.Location = new System.Drawing.Point(81, 63);
            this.Numeric_PosX.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Numeric_PosX.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.Numeric_PosX.Minimum = new decimal(new int[] {
            999999999,
            0,
            0,
            -2147483648});
            this.Numeric_PosX.Name = "Numeric_PosX";
            this.Numeric_PosX.Size = new System.Drawing.Size(216, 23);
            this.Numeric_PosX.TabIndex = 24;
            this.Numeric_PosX.ValueChanged += new System.EventHandler(this.Numeric_OnValueChanged);
            // 
            // Label_PosZ
            // 
            this.Label_PosZ.AutoSize = true;
            this.Label_PosZ.Location = new System.Drawing.Point(11, 125);
            this.Label_PosZ.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label_PosZ.Name = "Label_PosZ";
            this.Label_PosZ.Size = new System.Drawing.Size(49, 15);
            this.Label_PosZ.TabIndex = 23;
            this.Label_PosZ.Text = "Offset Z";
            // 
            // Label_PosY
            // 
            this.Label_PosY.AutoSize = true;
            this.Label_PosY.Location = new System.Drawing.Point(11, 95);
            this.Label_PosY.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label_PosY.Name = "Label_PosY";
            this.Label_PosY.Size = new System.Drawing.Size(49, 15);
            this.Label_PosY.TabIndex = 22;
            this.Label_PosY.Text = "Offset Y";
            // 
            // Label_PosX
            // 
            this.Label_PosX.AutoSize = true;
            this.Label_PosX.Location = new System.Drawing.Point(10, 65);
            this.Label_PosX.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label_PosX.Name = "Label_PosX";
            this.Label_PosX.Size = new System.Drawing.Size(49, 15);
            this.Label_PosX.TabIndex = 21;
            this.Label_PosX.Text = "Offset X";
            // 
            // DockViewProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(310, 519);
            this.Controls.Add(this.Numeric_PosZ);
            this.Controls.Add(this.Numeric_PosY);
            this.Controls.Add(this.Numeric_PosX);
            this.Controls.Add(this.Label_PosZ);
            this.Controls.Add(this.Label_PosY);
            this.Controls.Add(this.Label_PosX);
            this.Controls.Add(this.TextBox_WithOffset);
            this.Controls.Add(this.Label_IntersectionWithOffset);
            this.Controls.Add(this.TextBox_PickWSLocation);
            this.Controls.Add(this.Label_PickIntersection);
            this.HideOnClose = true;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "DockViewProperties";
            this.TabText = "View Properties";
            this.Text = "Utilities";
            this.Resize += new System.EventHandler(this.OnResize);
            this.EntryMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Numeric_PosZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Numeric_PosY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Numeric_PosX)).EndInit();
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
        private System.Windows.Forms.TextBox TextBox_WithOffset;
        private System.Windows.Forms.Label Label_IntersectionWithOffset;
        public System.Windows.Forms.NumericUpDown Numeric_PosZ;
        public System.Windows.Forms.NumericUpDown Numeric_PosY;
        public System.Windows.Forms.NumericUpDown Numeric_PosX;
        private System.Windows.Forms.Label Label_PosZ;
        private System.Windows.Forms.Label Label_PosY;
        private System.Windows.Forms.Label Label_PosX;
    }
}