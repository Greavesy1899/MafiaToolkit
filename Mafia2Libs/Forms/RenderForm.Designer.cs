namespace Mafia2Tool
{
    partial class D3DForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(D3DForm));
            this.RenderPanel = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.FileButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.SaveButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitButton = new System.Windows.Forms.ToolStripMenuItem();
            this.MainTabControl = new System.Windows.Forms.TabControl();
            this.SceneTab = new System.Windows.Forms.TabPage();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.EditEntryTab = new System.Windows.Forms.TabPage();
            this.CurrentEntryType = new System.Windows.Forms.Label();
            this.EntryApplyChanges = new System.Windows.Forms.Button();
            this.HideInViewerCheckBox = new System.Windows.Forms.CheckBox();
            this.CurrentEntry = new System.Windows.Forms.Label();
            this.RotationZBox = new System.Windows.Forms.TextBox();
            this.RotationZLabel = new System.Windows.Forms.Label();
            this.RotationYBox = new System.Windows.Forms.TextBox();
            this.RotationYLabel = new System.Windows.Forms.Label();
            this.RotationXBox = new System.Windows.Forms.TextBox();
            this.RotationXLabel = new System.Windows.Forms.Label();
            this.PositionZBox = new System.Windows.Forms.TextBox();
            this.PositionZLabel = new System.Windows.Forms.Label();
            this.PositionYBox = new System.Windows.Forms.TextBox();
            this.PositionYLabel = new System.Windows.Forms.Label();
            this.PositionXBox = new System.Windows.Forms.TextBox();
            this.PositionXLabel = new System.Windows.Forms.Label();
            this.ViewButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.ToggleWireFrameButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ToggleCullingBottle = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            this.MainTabControl.SuspendLayout();
            this.SceneTab.SuspendLayout();
            this.EditEntryTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // RenderPanel
            // 
            this.RenderPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RenderPanel.Location = new System.Drawing.Point(224, 28);
            this.RenderPanel.Name = "RenderPanel";
            this.RenderPanel.Size = new System.Drawing.Size(564, 410);
            this.RenderPanel.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileButton,
            this.ViewButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // FileButton
            // 
            this.FileButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.FileButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveButton,
            this.ExitButton});
            this.FileButton.Image = ((System.Drawing.Image)(resources.GetObject("FileButton.Image")));
            this.FileButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.FileButton.Name = "FileButton";
            this.FileButton.Size = new System.Drawing.Size(38, 22);
            this.FileButton.Text = "File";
            // 
            // SaveButton
            // 
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(180, 22);
            this.SaveButton.Text = "Save";
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // ExitButton
            // 
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(180, 22);
            this.ExitButton.Text = "Exit";
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // MainTabControl
            // 
            this.MainTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.MainTabControl.Controls.Add(this.SceneTab);
            this.MainTabControl.Controls.Add(this.EditEntryTab);
            this.MainTabControl.Location = new System.Drawing.Point(6, 28);
            this.MainTabControl.Name = "MainTabControl";
            this.MainTabControl.SelectedIndex = 0;
            this.MainTabControl.Size = new System.Drawing.Size(212, 410);
            this.MainTabControl.TabIndex = 5;
            // 
            // SceneTab
            // 
            this.SceneTab.Controls.Add(this.treeView1);
            this.SceneTab.Location = new System.Drawing.Point(4, 22);
            this.SceneTab.Name = "SceneTab";
            this.SceneTab.Padding = new System.Windows.Forms.Padding(3);
            this.SceneTab.Size = new System.Drawing.Size(204, 384);
            this.SceneTab.TabIndex = 0;
            this.SceneTab.Text = "Scene";
            this.SceneTab.UseVisualStyleBackColor = true;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(3, 3);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(198, 378);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnAfterSelect);
            // 
            // EditEntryTab
            // 
            this.EditEntryTab.Controls.Add(this.CurrentEntryType);
            this.EditEntryTab.Controls.Add(this.EntryApplyChanges);
            this.EditEntryTab.Controls.Add(this.HideInViewerCheckBox);
            this.EditEntryTab.Controls.Add(this.CurrentEntry);
            this.EditEntryTab.Controls.Add(this.RotationZBox);
            this.EditEntryTab.Controls.Add(this.RotationZLabel);
            this.EditEntryTab.Controls.Add(this.RotationYBox);
            this.EditEntryTab.Controls.Add(this.RotationYLabel);
            this.EditEntryTab.Controls.Add(this.RotationXBox);
            this.EditEntryTab.Controls.Add(this.RotationXLabel);
            this.EditEntryTab.Controls.Add(this.PositionZBox);
            this.EditEntryTab.Controls.Add(this.PositionZLabel);
            this.EditEntryTab.Controls.Add(this.PositionYBox);
            this.EditEntryTab.Controls.Add(this.PositionYLabel);
            this.EditEntryTab.Controls.Add(this.PositionXBox);
            this.EditEntryTab.Controls.Add(this.PositionXLabel);
            this.EditEntryTab.Location = new System.Drawing.Point(4, 22);
            this.EditEntryTab.Name = "EditEntryTab";
            this.EditEntryTab.Padding = new System.Windows.Forms.Padding(3);
            this.EditEntryTab.Size = new System.Drawing.Size(204, 384);
            this.EditEntryTab.TabIndex = 1;
            this.EditEntryTab.Text = "Edit Entry";
            this.EditEntryTab.UseVisualStyleBackColor = true;
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
            this.EntryApplyChanges.Click += new System.EventHandler(this.EntryApplyChanges_OnClick);
            // 
            // HideInViewerCheckBox
            // 
            this.HideInViewerCheckBox.AutoSize = true;
            this.HideInViewerCheckBox.Location = new System.Drawing.Point(10, 204);
            this.HideInViewerCheckBox.Name = "HideInViewerCheckBox";
            this.HideInViewerCheckBox.Size = new System.Drawing.Size(135, 17);
            this.HideInViewerCheckBox.TabIndex = 13;
            this.HideInViewerCheckBox.Text = "Hide in Viewer (Debug)";
            this.HideInViewerCheckBox.UseVisualStyleBackColor = true;
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
            // RotationZBox
            // 
            this.RotationZBox.Location = new System.Drawing.Point(67, 161);
            this.RotationZBox.Name = "RotationZBox";
            this.RotationZBox.Size = new System.Drawing.Size(131, 20);
            this.RotationZBox.TabIndex = 11;
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
            // RotationYBox
            // 
            this.RotationYBox.Location = new System.Drawing.Point(67, 135);
            this.RotationYBox.Name = "RotationYBox";
            this.RotationYBox.Size = new System.Drawing.Size(131, 20);
            this.RotationYBox.TabIndex = 9;
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
            // RotationXBox
            // 
            this.RotationXBox.Location = new System.Drawing.Point(67, 109);
            this.RotationXBox.Name = "RotationXBox";
            this.RotationXBox.Size = new System.Drawing.Size(131, 20);
            this.RotationXBox.TabIndex = 7;
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
            // PositionZBox
            // 
            this.PositionZBox.Location = new System.Drawing.Point(67, 83);
            this.PositionZBox.Name = "PositionZBox";
            this.PositionZBox.Size = new System.Drawing.Size(131, 20);
            this.PositionZBox.TabIndex = 5;
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
            // PositionYBox
            // 
            this.PositionYBox.Location = new System.Drawing.Point(67, 57);
            this.PositionYBox.Name = "PositionYBox";
            this.PositionYBox.Size = new System.Drawing.Size(131, 20);
            this.PositionYBox.TabIndex = 3;
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
            // PositionXBox
            // 
            this.PositionXBox.Location = new System.Drawing.Point(67, 31);
            this.PositionXBox.Name = "PositionXBox";
            this.PositionXBox.Size = new System.Drawing.Size(131, 20);
            this.PositionXBox.TabIndex = 1;
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
            // ViewButton
            // 
            this.ViewButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ViewButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToggleWireFrameButton,
            this.ToggleCullingBottle});
            this.ViewButton.Image = ((System.Drawing.Image)(resources.GetObject("ViewButton.Image")));
            this.ViewButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ViewButton.Name = "ViewButton";
            this.ViewButton.Size = new System.Drawing.Size(45, 22);
            this.ViewButton.Text = "View";
            // 
            // ToggleWireFrameButton
            // 
            this.ToggleWireFrameButton.Name = "ToggleWireFrameButton";
            this.ToggleWireFrameButton.Size = new System.Drawing.Size(180, 22);
            this.ToggleWireFrameButton.Text = "Toggle Wireframe";
            this.ToggleWireFrameButton.Click += new System.EventHandler(this.FillModeButton_Click);
            // 
            // ToggleCullingBottle
            // 
            this.ToggleCullingBottle.Name = "ToggleCullingBottle";
            this.ToggleCullingBottle.Size = new System.Drawing.Size(180, 22);
            this.ToggleCullingBottle.Text = "Toggle Culling";
            this.ToggleCullingBottle.Click += new System.EventHandler(this.CullModeButton_Click);
            // 
            // D3DForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.MainTabControl);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.RenderPanel);
            this.Name = "D3DForm";
            this.Text = "TestForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.MainTabControl.ResumeLayout(false);
            this.SceneTab.ResumeLayout(false);
            this.EditEntryTab.ResumeLayout(false);
            this.EditEntryTab.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel RenderPanel;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.TabControl MainTabControl;
        private System.Windows.Forms.TabPage SceneTab;
        private System.Windows.Forms.TabPage EditEntryTab;
        private System.Windows.Forms.Label PositionXLabel;
        private System.Windows.Forms.TextBox PositionXBox;
        private System.Windows.Forms.TextBox PositionZBox;
        private System.Windows.Forms.Label PositionZLabel;
        private System.Windows.Forms.TextBox PositionYBox;
        private System.Windows.Forms.Label PositionYLabel;
        private System.Windows.Forms.TextBox RotationZBox;
        private System.Windows.Forms.Label RotationZLabel;
        private System.Windows.Forms.TextBox RotationYBox;
        private System.Windows.Forms.Label RotationYLabel;
        private System.Windows.Forms.TextBox RotationXBox;
        private System.Windows.Forms.Label RotationXLabel;
        private System.Windows.Forms.Label CurrentEntry;
        private System.Windows.Forms.CheckBox HideInViewerCheckBox;
        private System.Windows.Forms.Button EntryApplyChanges;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Label CurrentEntryType;
        private System.Windows.Forms.ToolStripDropDownButton FileButton;
        private System.Windows.Forms.ToolStripMenuItem SaveButton;
        private System.Windows.Forms.ToolStripMenuItem ExitButton;
        private System.Windows.Forms.ToolStripDropDownButton ViewButton;
        private System.Windows.Forms.ToolStripMenuItem ToggleWireFrameButton;
        private System.Windows.Forms.ToolStripMenuItem ToggleCullingBottle;
    }
}