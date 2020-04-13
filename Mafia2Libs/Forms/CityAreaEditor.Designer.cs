namespace Mafia2Tool
{
    partial class CityAreaEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CityAreaEditor));
            this.CollisionContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ContextDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.deletePlacementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openM2T = new System.Windows.Forms.OpenFileDialog();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.fileToolButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.SaveButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ReloadButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitButton = new System.Windows.Forms.ToolStripMenuItem();
            this.toolButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.AddAreaButton = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteArea = new System.Windows.Forms.ToolStripMenuItem();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.AreaNameLabel = new System.Windows.Forms.Label();
            this.AreaNameBox = new System.Windows.Forms.TextBox();
            this.Area1Label = new System.Windows.Forms.Label();
            this.Area2Label = new System.Windows.Forms.Label();
            this.UnkByteLabel = new System.Windows.Forms.Label();
            this.UnkByteBox = new System.Windows.Forms.CheckBox();
            this.AreaGroupBox = new System.Windows.Forms.GroupBox();
            this.SaveAreaButton = new System.Windows.Forms.Button();
            this.ReloadAreaButton = new System.Windows.Forms.Button();
            this.Area2Box = new System.Windows.Forms.TextBox();
            this.Area1Box = new System.Windows.Forms.TextBox();
            this.CollisionContext.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.AreaGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // CollisionContext
            // 
            this.CollisionContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ContextDelete,
            this.deletePlacementToolStripMenuItem});
            this.CollisionContext.Name = "SDSContext";
            this.CollisionContext.Size = new System.Drawing.Size(167, 48);
            // 
            // ContextDelete
            // 
            this.ContextDelete.Name = "ContextDelete";
            this.ContextDelete.Size = new System.Drawing.Size(166, 22);
            this.ContextDelete.Text = "Delete Collision";
            // 
            // deletePlacementToolStripMenuItem
            // 
            this.deletePlacementToolStripMenuItem.Name = "deletePlacementToolStripMenuItem";
            this.deletePlacementToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.deletePlacementToolStripMenuItem.Text = "Delete Placement";
            // 
            // openM2T
            // 
            this.openM2T.FileName = "Select M2T file.";
            this.openM2T.Filter = "Model File|*.m2t|All Files|*.*|FBX Model|*.fbx";
            this.openM2T.Tag = "";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolButton,
            this.toolButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 25);
            this.toolStrip1.TabIndex = 15;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // fileToolButton
            // 
            this.fileToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.fileToolButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveButton,
            this.ReloadButton,
            this.ExitButton});
            this.fileToolButton.Image = ((System.Drawing.Image)(resources.GetObject("fileToolButton.Image")));
            this.fileToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.fileToolButton.Name = "fileToolButton";
            this.fileToolButton.Size = new System.Drawing.Size(47, 22);
            this.fileToolButton.Text = "$FILE";
            // 
            // SaveButton
            // 
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(124, 22);
            this.SaveButton.Text = "$SAVE";
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // ReloadButton
            // 
            this.ReloadButton.Name = "ReloadButton";
            this.ReloadButton.Size = new System.Drawing.Size(124, 22);
            this.ReloadButton.Text = "$RELOAD";
            this.ReloadButton.Click += new System.EventHandler(this.ReloadButton_Click);
            // 
            // ExitButton
            // 
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(124, 22);
            this.ExitButton.Text = "$EXIT";
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // toolButton
            // 
            this.toolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddAreaButton,
            this.DeleteArea});
            this.toolButton.Image = ((System.Drawing.Image)(resources.GetObject("toolButton.Image")));
            this.toolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButton.Name = "toolButton";
            this.toolButton.Size = new System.Drawing.Size(61, 22);
            this.toolButton.Text = "$TOOLS";
            // 
            // AddAreaButton
            // 
            this.AddAreaButton.Name = "AddAreaButton";
            this.AddAreaButton.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.A)));
            this.AddAreaButton.Size = new System.Drawing.Size(190, 22);
            this.AddAreaButton.Text = "$ADD_AREA";
            this.AddAreaButton.Click += new System.EventHandler(this.AddAreaButton_Click);
            // 
            // DeleteArea
            // 
            this.DeleteArea.Name = "DeleteArea";
            this.DeleteArea.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D)));
            this.DeleteArea.Size = new System.Drawing.Size(190, 22);
            this.DeleteArea.Text = "$DELETE_AREA";
            this.DeleteArea.Click += new System.EventHandler(this.DeleteArea_Click);
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 28);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(384, 407);
            this.listBox1.TabIndex = 16;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.UpdateAreaData);
            // 
            // AreaNameLabel
            // 
            this.AreaNameLabel.AutoSize = true;
            this.AreaNameLabel.Location = new System.Drawing.Point(15, 25);
            this.AreaNameLabel.Name = "AreaNameLabel";
            this.AreaNameLabel.Size = new System.Drawing.Size(79, 13);
            this.AreaNameLabel.TabIndex = 17;
            this.AreaNameLabel.Text = "$AREA_NAME";
            // 
            // AreaNameBox
            // 
            this.AreaNameBox.Location = new System.Drawing.Point(142, 22);
            this.AreaNameBox.Name = "AreaNameBox";
            this.AreaNameBox.Size = new System.Drawing.Size(229, 20);
            this.AreaNameBox.TabIndex = 18;
            // 
            // Area1Label
            // 
            this.Area1Label.AutoSize = true;
            this.Area1Label.Location = new System.Drawing.Point(15, 51);
            this.Area1Label.Name = "Area1Label";
            this.Area1Label.Size = new System.Drawing.Size(54, 13);
            this.Area1Label.TabIndex = 21;
            this.Area1Label.Text = "$AREA_1";
            // 
            // Area2Label
            // 
            this.Area2Label.AutoSize = true;
            this.Area2Label.Location = new System.Drawing.Point(15, 78);
            this.Area2Label.Name = "Area2Label";
            this.Area2Label.Size = new System.Drawing.Size(54, 13);
            this.Area2Label.TabIndex = 22;
            this.Area2Label.Text = "$AREA_2";
            // 
            // UnkByteLabel
            // 
            this.UnkByteLabel.AutoSize = true;
            this.UnkByteLabel.Location = new System.Drawing.Point(15, 105);
            this.UnkByteLabel.Name = "UnkByteLabel";
            this.UnkByteLabel.Size = new System.Drawing.Size(70, 13);
            this.UnkByteLabel.TabIndex = 24;
            this.UnkByteLabel.Text = "$UNK_BYTE";
            // 
            // UnkByteBox
            // 
            this.UnkByteBox.AutoSize = true;
            this.UnkByteBox.Location = new System.Drawing.Point(142, 105);
            this.UnkByteBox.Name = "UnkByteBox";
            this.UnkByteBox.Size = new System.Drawing.Size(15, 14);
            this.UnkByteBox.TabIndex = 25;
            this.UnkByteBox.UseVisualStyleBackColor = true;
            // 
            // AreaGroupBox
            // 
            this.AreaGroupBox.Controls.Add(this.SaveAreaButton);
            this.AreaGroupBox.Controls.Add(this.ReloadAreaButton);
            this.AreaGroupBox.Controls.Add(this.Area2Box);
            this.AreaGroupBox.Controls.Add(this.Area1Box);
            this.AreaGroupBox.Controls.Add(this.AreaNameLabel);
            this.AreaGroupBox.Controls.Add(this.UnkByteBox);
            this.AreaGroupBox.Controls.Add(this.AreaNameBox);
            this.AreaGroupBox.Controls.Add(this.UnkByteLabel);
            this.AreaGroupBox.Controls.Add(this.Area2Label);
            this.AreaGroupBox.Controls.Add(this.Area1Label);
            this.AreaGroupBox.Location = new System.Drawing.Point(402, 28);
            this.AreaGroupBox.Name = "AreaGroupBox";
            this.AreaGroupBox.Size = new System.Drawing.Size(386, 407);
            this.AreaGroupBox.TabIndex = 26;
            this.AreaGroupBox.TabStop = false;
            this.AreaGroupBox.Text = "$AREA_DATA";
            // 
            // SaveAreaButton
            // 
            this.SaveAreaButton.Location = new System.Drawing.Point(18, 147);
            this.SaveAreaButton.Name = "SaveAreaButton";
            this.SaveAreaButton.Size = new System.Drawing.Size(91, 23);
            this.SaveAreaButton.TabIndex = 29;
            this.SaveAreaButton.Text = "$SAVE_AREA";
            this.SaveAreaButton.UseVisualStyleBackColor = true;
            this.SaveAreaButton.Click += new System.EventHandler(this.SaveArea_Clicked);
            // 
            // ReloadAreaButton
            // 
            this.ReloadAreaButton.Location = new System.Drawing.Point(271, 147);
            this.ReloadAreaButton.Name = "ReloadAreaButton";
            this.ReloadAreaButton.Size = new System.Drawing.Size(100, 23);
            this.ReloadAreaButton.TabIndex = 28;
            this.ReloadAreaButton.Text = "$RELOAD_AREA";
            this.ReloadAreaButton.UseVisualStyleBackColor = true;
            this.ReloadAreaButton.Click += new System.EventHandler(this.ReloadArea_Click);
            // 
            // Area2Box
            // 
            this.Area2Box.Location = new System.Drawing.Point(142, 74);
            this.Area2Box.Name = "Area2Box";
            this.Area2Box.Size = new System.Drawing.Size(229, 20);
            this.Area2Box.TabIndex = 27;
            // 
            // Area1Box
            // 
            this.Area1Box.Location = new System.Drawing.Point(142, 48);
            this.Area1Box.Name = "Area1Box";
            this.Area1Box.Size = new System.Drawing.Size(229, 20);
            this.Area1Box.TabIndex = 26;
            // 
            // CityAreaEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.AreaGroupBox);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CityAreaEditor";
            this.Text = "$ACTOR_EDITOR_TITLE";
            this.CollisionContext.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.AreaGroupBox.ResumeLayout(false);
            this.AreaGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openM2T;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton fileToolButton;
        private System.Windows.Forms.ToolStripMenuItem SaveButton;
        private System.Windows.Forms.ToolStripMenuItem ReloadButton;
        private System.Windows.Forms.ToolStripMenuItem ExitButton;
        private System.Windows.Forms.ContextMenuStrip CollisionContext;
        private System.Windows.Forms.ToolStripMenuItem ContextDelete;
        private System.Windows.Forms.ToolStripMenuItem deletePlacementToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolButton;
        private System.Windows.Forms.ToolStripMenuItem AddAreaButton;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label AreaNameLabel;
        private System.Windows.Forms.TextBox AreaNameBox;
        private System.Windows.Forms.Label Area1Label;
        private System.Windows.Forms.Label Area2Label;
        private System.Windows.Forms.Label UnkByteLabel;
        private System.Windows.Forms.CheckBox UnkByteBox;
        private System.Windows.Forms.GroupBox AreaGroupBox;
        private System.Windows.Forms.TextBox Area1Box;
        private System.Windows.Forms.TextBox Area2Box;
        private System.Windows.Forms.Button SaveAreaButton;
        private System.Windows.Forms.Button ReloadAreaButton;
        private System.Windows.Forms.ToolStripMenuItem DeleteArea;
    }
}