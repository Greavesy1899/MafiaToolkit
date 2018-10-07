namespace Mafia2Tool {
    partial class MaterialTool {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MaterialTool));
            this.MaterialSearch = new System.Windows.Forms.TextBox();
            this.MaterialListBox = new System.Windows.Forms.ListBox();
            this.MaterialGrid = new System.Windows.Forms.PropertyGrid();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.contextFileButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.contextOpenButton = new System.Windows.Forms.ToolStripMenuItem();
            this.contextSaveButton = new System.Windows.Forms.ToolStripMenuItem();
            this.contextExitButton = new System.Windows.Forms.ToolStripMenuItem();
            this.toolButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.addMaterialToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UpdateListButton = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MaterialSearch
            // 
            this.MaterialSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.MaterialSearch.Location = new System.Drawing.Point(13, 28);
            this.MaterialSearch.Name = "MaterialSearch";
            this.MaterialSearch.Size = new System.Drawing.Size(207, 20);
            this.MaterialSearch.TabIndex = 0;
            this.MaterialSearch.TextChanged += new System.EventHandler(this.MaterialSearch_TextChanged);
            this.MaterialSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPressed);
            // 
            // MaterialListBox
            // 
            this.MaterialListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.MaterialListBox.DisplayMember = "MaterialName";
            this.MaterialListBox.FormattingEnabled = true;
            this.MaterialListBox.Location = new System.Drawing.Point(13, 53);
            this.MaterialListBox.Name = "MaterialListBox";
            this.MaterialListBox.Size = new System.Drawing.Size(368, 368);
            this.MaterialListBox.TabIndex = 1;
            this.MaterialListBox.ValueMember = "MaterialName";
            this.MaterialListBox.SelectedIndexChanged += new System.EventHandler(this.OnMaterialSelected);
            // 
            // MaterialGrid
            // 
            this.MaterialGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MaterialGrid.Location = new System.Drawing.Point(387, 40);
            this.MaterialGrid.Name = "MaterialGrid";
            this.MaterialGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.MaterialGrid.Size = new System.Drawing.Size(386, 381);
            this.MaterialGrid.TabIndex = 2;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextFileButton,
            this.toolButton,
            this.UpdateListButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(785, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // contextFileButton
            // 
            this.contextFileButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.contextFileButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextOpenButton,
            this.contextSaveButton,
            this.contextExitButton});
            this.contextFileButton.Image = ((System.Drawing.Image)(resources.GetObject("contextFileButton.Image")));
            this.contextFileButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.contextFileButton.Name = "contextFileButton";
            this.contextFileButton.Size = new System.Drawing.Size(38, 22);
            this.contextFileButton.Text = "File";
            // 
            // contextOpenButton
            // 
            this.contextOpenButton.Enabled = false;
            this.contextOpenButton.Name = "contextOpenButton";
            this.contextOpenButton.Size = new System.Drawing.Size(103, 22);
            this.contextOpenButton.Text = "Open";
            // 
            // contextSaveButton
            // 
            this.contextSaveButton.Name = "contextSaveButton";
            this.contextSaveButton.Size = new System.Drawing.Size(103, 22);
            this.contextSaveButton.Text = "Save";
            this.contextSaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // contextExitButton
            // 
            this.contextExitButton.Name = "contextExitButton";
            this.contextExitButton.Size = new System.Drawing.Size(103, 22);
            this.contextExitButton.Text = "Exit";
            this.contextExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // toolButton
            // 
            this.toolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addMaterialToolStripMenuItem});
            this.toolButton.Image = ((System.Drawing.Image)(resources.GetObject("toolButton.Image")));
            this.toolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButton.Name = "toolButton";
            this.toolButton.Size = new System.Drawing.Size(48, 22);
            this.toolButton.Text = "Tools";
            // 
            // addMaterialToolStripMenuItem
            // 
            this.addMaterialToolStripMenuItem.Name = "addMaterialToolStripMenuItem";
            this.addMaterialToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.addMaterialToolStripMenuItem.Text = "Add Material";
            this.addMaterialToolStripMenuItem.Click += new System.EventHandler(this.AddMaterial);
            // 
            // UpdateListButton
            // 
            this.UpdateListButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.UpdateListButton.Image = ((System.Drawing.Image)(resources.GetObject("UpdateListButton.Image")));
            this.UpdateListButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.UpdateListButton.Name = "UpdateListButton";
            this.UpdateListButton.Size = new System.Drawing.Size(70, 22);
            this.UpdateListButton.Text = "Update List";
            this.UpdateListButton.Click += new System.EventHandler(this.UpdateList);
            // 
            // MaterialTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(785, 431);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.MaterialGrid);
            this.Controls.Add(this.MaterialListBox);
            this.Controls.Add(this.MaterialSearch);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MaterialTool";
            this.Text = "Material Library Editor";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox MaterialSearch;
        private System.Windows.Forms.ListBox MaterialListBox;
        private System.Windows.Forms.PropertyGrid MaterialGrid;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton contextFileButton;
        private System.Windows.Forms.ToolStripMenuItem contextOpenButton;
        private System.Windows.Forms.ToolStripMenuItem contextSaveButton;
        private System.Windows.Forms.ToolStripMenuItem contextExitButton;
        private System.Windows.Forms.ToolStripDropDownButton toolButton;
        private System.Windows.Forms.ToolStripMenuItem addMaterialToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton UpdateListButton;
    }
}