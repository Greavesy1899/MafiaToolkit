namespace Mafia2Tool.Forms.Docking
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
            this.MainTabControl = new System.Windows.Forms.TabControl();
            this.SceneTab = new System.Windows.Forms.TabPage();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.EditEntryTab = new System.Windows.Forms.TabPage();
            this.RotationZNumeric = new System.Windows.Forms.NumericUpDown();
            this.RotationYNumeric = new System.Windows.Forms.NumericUpDown();
            this.RotationXNumeric = new System.Windows.Forms.NumericUpDown();
            this.PositionZNumeric = new System.Windows.Forms.NumericUpDown();
            this.PositionYNumeric = new System.Windows.Forms.NumericUpDown();
            this.PositionXNumeric = new System.Windows.Forms.NumericUpDown();
            this.OnFrameNameTable = new System.Windows.Forms.CheckBox();
            this.CurrentEntryType = new System.Windows.Forms.Label();
            this.EntryApplyChanges = new System.Windows.Forms.Button();
            this.CurrentEntry = new System.Windows.Forms.Label();
            this.RotationZLabel = new System.Windows.Forms.Label();
            this.RotationYLabel = new System.Windows.Forms.Label();
            this.RotationXLabel = new System.Windows.Forms.Label();
            this.PositionZLabel = new System.Windows.Forms.Label();
            this.PositionYLabel = new System.Windows.Forms.Label();
            this.PositionXLabel = new System.Windows.Forms.Label();
            this.FrameNameTableFlags = new Utils.Extensions.FlagCheckedListBox();
            this.MainTabControl.SuspendLayout();
            this.SceneTab.SuspendLayout();
            this.EditEntryTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RotationZNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotationYNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotationXNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionZNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionYNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionXNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // MainTabControl
            // 
            this.MainTabControl.Controls.Add(this.SceneTab);
            this.MainTabControl.Controls.Add(this.EditEntryTab);
            this.MainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTabControl.Location = new System.Drawing.Point(0, 0);
            this.MainTabControl.Name = "MainTabControl";
            this.MainTabControl.SelectedIndex = 0;
            this.MainTabControl.Size = new System.Drawing.Size(255, 450);
            this.MainTabControl.TabIndex = 6;
            // 
            // SceneTab
            // 
            this.SceneTab.Controls.Add(this.treeView1);
            this.SceneTab.Location = new System.Drawing.Point(4, 22);
            this.SceneTab.Name = "SceneTab";
            this.SceneTab.Padding = new System.Windows.Forms.Padding(3);
            this.SceneTab.Size = new System.Drawing.Size(247, 424);
            this.SceneTab.TabIndex = 0;
            this.SceneTab.Text = "Scene";
            this.SceneTab.UseVisualStyleBackColor = true;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.ImageIndex = 3;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.Location = new System.Drawing.Point(3, 3);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(241, 418);
            this.treeView1.TabIndex = 0;
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
            // EditEntryTab
            // 
            this.EditEntryTab.Controls.Add(this.RotationZNumeric);
            this.EditEntryTab.Controls.Add(this.RotationYNumeric);
            this.EditEntryTab.Controls.Add(this.RotationXNumeric);
            this.EditEntryTab.Controls.Add(this.PositionZNumeric);
            this.EditEntryTab.Controls.Add(this.PositionYNumeric);
            this.EditEntryTab.Controls.Add(this.PositionXNumeric);
            this.EditEntryTab.Controls.Add(this.OnFrameNameTable);
            this.EditEntryTab.Controls.Add(this.CurrentEntryType);
            this.EditEntryTab.Controls.Add(this.EntryApplyChanges);
            this.EditEntryTab.Controls.Add(this.CurrentEntry);
            this.EditEntryTab.Controls.Add(this.RotationZLabel);
            this.EditEntryTab.Controls.Add(this.RotationYLabel);
            this.EditEntryTab.Controls.Add(this.RotationXLabel);
            this.EditEntryTab.Controls.Add(this.PositionZLabel);
            this.EditEntryTab.Controls.Add(this.PositionYLabel);
            this.EditEntryTab.Controls.Add(this.PositionXLabel);
            this.EditEntryTab.Controls.Add(this.FrameNameTableFlags);
            this.EditEntryTab.Location = new System.Drawing.Point(4, 22);
            this.EditEntryTab.Name = "EditEntryTab";
            this.EditEntryTab.Padding = new System.Windows.Forms.Padding(3);
            this.EditEntryTab.Size = new System.Drawing.Size(247, 424);
            this.EditEntryTab.TabIndex = 1;
            this.EditEntryTab.Text = "Edit Entry";
            this.EditEntryTab.UseVisualStyleBackColor = true;
            // 
            // RotationZNumeric
            // 
            this.RotationZNumeric.DecimalPlaces = 5;
            this.RotationZNumeric.Location = new System.Drawing.Point(67, 162);
            this.RotationZNumeric.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.RotationZNumeric.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.RotationZNumeric.Name = "RotationZNumeric";
            this.RotationZNumeric.Size = new System.Drawing.Size(185, 20);
            this.RotationZNumeric.TabIndex = 23;
            // 
            // RotationYNumeric
            // 
            this.RotationYNumeric.DecimalPlaces = 5;
            this.RotationYNumeric.Location = new System.Drawing.Point(67, 136);
            this.RotationYNumeric.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.RotationYNumeric.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.RotationYNumeric.Name = "RotationYNumeric";
            this.RotationYNumeric.Size = new System.Drawing.Size(185, 20);
            this.RotationYNumeric.TabIndex = 22;
            // 
            // RotationXNumeric
            // 
            this.RotationXNumeric.DecimalPlaces = 5;
            this.RotationXNumeric.Location = new System.Drawing.Point(67, 110);
            this.RotationXNumeric.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.RotationXNumeric.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.RotationXNumeric.Name = "RotationXNumeric";
            this.RotationXNumeric.Size = new System.Drawing.Size(185, 20);
            this.RotationXNumeric.TabIndex = 21;
            // 
            // PositionZNumeric
            // 
            this.PositionZNumeric.DecimalPlaces = 5;
            this.PositionZNumeric.Location = new System.Drawing.Point(67, 84);
            this.PositionZNumeric.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.PositionZNumeric.Minimum = new decimal(new int[] {
            999999999,
            0,
            0,
            -2147483648});
            this.PositionZNumeric.Name = "PositionZNumeric";
            this.PositionZNumeric.Size = new System.Drawing.Size(185, 20);
            this.PositionZNumeric.TabIndex = 20;
            // 
            // PositionYNumeric
            // 
            this.PositionYNumeric.DecimalPlaces = 5;
            this.PositionYNumeric.Location = new System.Drawing.Point(67, 58);
            this.PositionYNumeric.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.PositionYNumeric.Minimum = new decimal(new int[] {
            999999999,
            0,
            0,
            -2147483648});
            this.PositionYNumeric.Name = "PositionYNumeric";
            this.PositionYNumeric.Size = new System.Drawing.Size(185, 20);
            this.PositionYNumeric.TabIndex = 19;
            // 
            // PositionXNumeric
            // 
            this.PositionXNumeric.DecimalPlaces = 5;
            this.PositionXNumeric.Location = new System.Drawing.Point(67, 32);
            this.PositionXNumeric.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.PositionXNumeric.Minimum = new decimal(new int[] {
            999999999,
            0,
            0,
            -2147483648});
            this.PositionXNumeric.Name = "PositionXNumeric";
            this.PositionXNumeric.Size = new System.Drawing.Size(185, 20);
            this.PositionXNumeric.TabIndex = 18;
            // 
            // OnFrameNameTable
            // 
            this.OnFrameNameTable.AutoSize = true;
            this.OnFrameNameTable.Location = new System.Drawing.Point(13, 199);
            this.OnFrameNameTable.Name = "OnFrameNameTable";
            this.OnFrameNameTable.Size = new System.Drawing.Size(133, 17);
            this.OnFrameNameTable.TabIndex = 17;
            this.OnFrameNameTable.Text = "On FrameNameTable?";
            this.OnFrameNameTable.UseVisualStyleBackColor = true;
            // 
            // CurrentEntryType
            // 
            this.CurrentEntryType.AutoSize = true;
            this.CurrentEntryType.Location = new System.Drawing.Point(10, 339);
            this.CurrentEntryType.Name = "CurrentEntryType";
            this.CurrentEntryType.Size = new System.Drawing.Size(108, 13);
            this.CurrentEntryType.TabIndex = 15;
            this.CurrentEntryType.Text = "FRAME TYPE HERE";
            // 
            // EntryApplyChanges
            // 
            this.EntryApplyChanges.Location = new System.Drawing.Point(10, 355);
            this.EntryApplyChanges.Name = "EntryApplyChanges";
            this.EntryApplyChanges.Size = new System.Drawing.Size(188, 23);
            this.EntryApplyChanges.TabIndex = 14;
            this.EntryApplyChanges.Text = "Apply Changes";
            this.EntryApplyChanges.UseVisualStyleBackColor = true;
            // 
            // CurrentEntry
            // 
            this.CurrentEntry.AutoSize = true;
            this.CurrentEntry.Location = new System.Drawing.Point(10, 7);
            this.CurrentEntry.Name = "CurrentEntry";
            this.CurrentEntry.Size = new System.Drawing.Size(111, 13);
            this.CurrentEntry.TabIndex = 12;
            this.CurrentEntry.Text = "FRAME NAME HERE";
            // 
            // RotationZLabel
            // 
            this.RotationZLabel.AutoSize = true;
            this.RotationZLabel.Location = new System.Drawing.Point(7, 164);
            this.RotationZLabel.Name = "RotationZLabel";
            this.RotationZLabel.Size = new System.Drawing.Size(57, 13);
            this.RotationZLabel.TabIndex = 10;
            this.RotationZLabel.Text = "Rotation Z";
            // 
            // RotationYLabel
            // 
            this.RotationYLabel.AutoSize = true;
            this.RotationYLabel.Location = new System.Drawing.Point(7, 138);
            this.RotationYLabel.Name = "RotationYLabel";
            this.RotationYLabel.Size = new System.Drawing.Size(57, 13);
            this.RotationYLabel.TabIndex = 8;
            this.RotationYLabel.Text = "Rotation Y";
            // 
            // RotationXLabel
            // 
            this.RotationXLabel.AutoSize = true;
            this.RotationXLabel.Location = new System.Drawing.Point(7, 112);
            this.RotationXLabel.Name = "RotationXLabel";
            this.RotationXLabel.Size = new System.Drawing.Size(57, 13);
            this.RotationXLabel.TabIndex = 6;
            this.RotationXLabel.Text = "Rotation X";
            // 
            // PositionZLabel
            // 
            this.PositionZLabel.AutoSize = true;
            this.PositionZLabel.Location = new System.Drawing.Point(7, 86);
            this.PositionZLabel.Name = "PositionZLabel";
            this.PositionZLabel.Size = new System.Drawing.Size(54, 13);
            this.PositionZLabel.TabIndex = 4;
            this.PositionZLabel.Text = "Position Z";
            // 
            // PositionYLabel
            // 
            this.PositionYLabel.AutoSize = true;
            this.PositionYLabel.Location = new System.Drawing.Point(7, 60);
            this.PositionYLabel.Name = "PositionYLabel";
            this.PositionYLabel.Size = new System.Drawing.Size(54, 13);
            this.PositionYLabel.TabIndex = 2;
            this.PositionYLabel.Text = "Position Y";
            // 
            // PositionXLabel
            // 
            this.PositionXLabel.AutoSize = true;
            this.PositionXLabel.Location = new System.Drawing.Point(7, 34);
            this.PositionXLabel.Name = "PositionXLabel";
            this.PositionXLabel.Size = new System.Drawing.Size(54, 13);
            this.PositionXLabel.TabIndex = 0;
            this.PositionXLabel.Text = "Position X";
            // 
            // FrameNameTableFlags
            // 
            this.FrameNameTableFlags.CheckOnClick = true;
            this.FrameNameTableFlags.FormattingEnabled = true;
            this.FrameNameTableFlags.Location = new System.Drawing.Point(10, 222);
            this.FrameNameTableFlags.Name = "FrameNameTableFlags";
            this.FrameNameTableFlags.Size = new System.Drawing.Size(188, 94);
            this.FrameNameTableFlags.TabIndex = 16;
            // 
            // DockSceneTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(255, 450);
            this.Controls.Add(this.MainTabControl);
            this.Name = "DockSceneTree";
            this.TabText = "Scene Tree";
            this.Text = "DockSceneTree";
            this.MainTabControl.ResumeLayout(false);
            this.SceneTab.ResumeLayout(false);
            this.EditEntryTab.ResumeLayout(false);
            this.EditEntryTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RotationZNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotationYNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotationXNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionZNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionYNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionXNumeric)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl MainTabControl;
        private System.Windows.Forms.TabPage SceneTab;
        public System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TabPage EditEntryTab;
        private System.Windows.Forms.NumericUpDown RotationZNumeric;
        private System.Windows.Forms.NumericUpDown RotationYNumeric;
        private System.Windows.Forms.NumericUpDown RotationXNumeric;
        private System.Windows.Forms.NumericUpDown PositionZNumeric;
        private System.Windows.Forms.NumericUpDown PositionYNumeric;
        private System.Windows.Forms.NumericUpDown PositionXNumeric;
        private System.Windows.Forms.CheckBox OnFrameNameTable;
        private System.Windows.Forms.Label CurrentEntryType;
        private System.Windows.Forms.Button EntryApplyChanges;
        private System.Windows.Forms.Label CurrentEntry;
        private System.Windows.Forms.Label RotationZLabel;
        private System.Windows.Forms.Label RotationYLabel;
        private System.Windows.Forms.Label RotationXLabel;
        private System.Windows.Forms.Label PositionZLabel;
        private System.Windows.Forms.Label PositionYLabel;
        private System.Windows.Forms.Label PositionXLabel;
        private Utils.Extensions.FlagCheckedListBox FrameNameTableFlags;
        private System.Windows.Forms.ImageList imageList1;
    }
}