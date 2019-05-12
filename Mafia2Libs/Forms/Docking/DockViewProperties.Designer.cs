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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ModelComboBox = new System.Windows.Forms.ComboBox();
            this.CollisionComboBox = new System.Windows.Forms.ComboBox();
            this.BoxComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
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
            this.EntryMenuStrip.Size = new System.Drawing.Size(125, 92);
            // 
            // PreviewButton
            // 
            this.PreviewButton.Name = "PreviewButton";
            this.PreviewButton.Size = new System.Drawing.Size(124, 22);
            this.PreviewButton.Text = "Preview";
            // 
            // DeleteButton
            // 
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(124, 22);
            this.DeleteButton.Text = "Delete";
            // 
            // DuplicateButton
            // 
            this.DuplicateButton.Name = "DuplicateButton";
            this.DuplicateButton.Size = new System.Drawing.Size(124, 22);
            this.DuplicateButton.Text = "Duplicate";
            // 
            // Export3DButton
            // 
            this.Export3DButton.Name = "Export3DButton";
            this.Export3DButton.Size = new System.Drawing.Size(124, 22);
            this.Export3DButton.Text = "Export 3D";
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Toggle Models";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Toggle Collisions";
            // 
            // ModelComboBox
            // 
            this.ModelComboBox.FormattingEnabled = true;
            this.ModelComboBox.Items.AddRange(new object[] {
            "Visible",
            "Hidden"});
            this.ModelComboBox.Location = new System.Drawing.Point(119, 13);
            this.ModelComboBox.Name = "ModelComboBox";
            this.ModelComboBox.Size = new System.Drawing.Size(135, 21);
            this.ModelComboBox.TabIndex = 3;
            this.ModelComboBox.SelectedIndexChanged += new System.EventHandler(this.OnSelectedIndexChanged);
            // 
            // CollisionComboBox
            // 
            this.CollisionComboBox.FormattingEnabled = true;
            this.CollisionComboBox.Items.AddRange(new object[] {
            "Visible",
            "Hidden"});
            this.CollisionComboBox.Location = new System.Drawing.Point(119, 41);
            this.CollisionComboBox.Name = "CollisionComboBox";
            this.CollisionComboBox.Size = new System.Drawing.Size(135, 21);
            this.CollisionComboBox.TabIndex = 4;
            this.CollisionComboBox.SelectedIndexChanged += new System.EventHandler(this.OnSelectedIndexChanged);
            // 
            // BoxComboBox
            // 
            this.BoxComboBox.FormattingEnabled = true;
            this.BoxComboBox.Items.AddRange(new object[] {
            "Visible",
            "Hidden"});
            this.BoxComboBox.Location = new System.Drawing.Point(119, 68);
            this.BoxComboBox.Name = "BoxComboBox";
            this.BoxComboBox.Size = new System.Drawing.Size(135, 21);
            this.BoxComboBox.TabIndex = 6;
            this.BoxComboBox.SelectedIndexChanged += new System.EventHandler(this.OnSelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Toggle Boxes";
            // 
            // DockViewProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(266, 450);
            this.Controls.Add(this.BoxComboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.CollisionComboBox);
            this.Controls.Add(this.ModelComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.HideOnClose = true;
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ModelComboBox;
        private System.Windows.Forms.ComboBox CollisionComboBox;
        private System.Windows.Forms.ComboBox BoxComboBox;
        private System.Windows.Forms.Label label3;
    }
}