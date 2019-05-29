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
            this.MaterialGrid = new System.Windows.Forms.PropertyGrid();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.contextFileButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.contextOpenButton = new System.Windows.Forms.ToolStripMenuItem();
            this.contextReloadButton = new System.Windows.Forms.ToolStripMenuItem();
            this.contextSaveButton = new System.Windows.Forms.ToolStripMenuItem();
            this.contextExitButton = new System.Windows.Forms.ToolStripMenuItem();
            this.toolButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.addMaterialToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteSelectedMaterialButton = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.columnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnHash = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dumpInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // MaterialSearch
            // 
            this.MaterialSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.MaterialSearch.Location = new System.Drawing.Point(13, 28);
            this.MaterialSearch.Name = "MaterialSearch";
            this.MaterialSearch.Size = new System.Drawing.Size(368, 20);
            this.MaterialSearch.TabIndex = 0;
            this.MaterialSearch.TextChanged += new System.EventHandler(this.MaterialSearch_TextChanged);
            this.MaterialSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPressed);
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
            this.toolButton});
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
            this.contextReloadButton,
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
            this.contextOpenButton.Size = new System.Drawing.Size(110, 22);
            this.contextOpenButton.Text = "Open";
            // 
            // contextReloadButton
            // 
            this.contextReloadButton.Name = "contextReloadButton";
            this.contextReloadButton.Size = new System.Drawing.Size(110, 22);
            this.contextReloadButton.Text = "Reload";
            this.contextReloadButton.Click += new System.EventHandler(this.UpdateList);
            // 
            // contextSaveButton
            // 
            this.contextSaveButton.Name = "contextSaveButton";
            this.contextSaveButton.Size = new System.Drawing.Size(110, 22);
            this.contextSaveButton.Text = "Save";
            this.contextSaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // contextExitButton
            // 
            this.contextExitButton.Name = "contextExitButton";
            this.contextExitButton.Size = new System.Drawing.Size(110, 22);
            this.contextExitButton.Text = "Exit";
            this.contextExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // toolButton
            // 
            this.toolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addMaterialToolStripMenuItem,
            this.DeleteSelectedMaterialButton,
            this.dumpInfoToolStripMenuItem});
            this.toolButton.Image = ((System.Drawing.Image)(resources.GetObject("toolButton.Image")));
            this.toolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButton.Name = "toolButton";
            this.toolButton.Size = new System.Drawing.Size(48, 22);
            this.toolButton.Text = "Tools";
            // 
            // addMaterialToolStripMenuItem
            // 
            this.addMaterialToolStripMenuItem.Name = "addMaterialToolStripMenuItem";
            this.addMaterialToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.addMaterialToolStripMenuItem.Text = "Add Material";
            this.addMaterialToolStripMenuItem.Click += new System.EventHandler(this.AddMaterial);
            // 
            // DeleteSelectedMaterialButton
            // 
            this.DeleteSelectedMaterialButton.Name = "DeleteSelectedMaterialButton";
            this.DeleteSelectedMaterialButton.Size = new System.Drawing.Size(180, 22);
            this.DeleteSelectedMaterialButton.Text = "$DELETE_SEL_MAT";
            this.DeleteSelectedMaterialButton.Click += new System.EventHandler(this.DeleteMaterial);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnName,
            this.columnHash});
            this.dataGridView1.Location = new System.Drawing.Point(13, 54);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(369, 367);
            this.dataGridView1.TabIndex = 3;
            this.dataGridView1.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnMaterialSelected);
            // 
            // columnName
            // 
            this.columnName.HeaderText = "Name";
            this.columnName.Name = "columnName";
            this.columnName.ReadOnly = true;
            // 
            // columnHash
            // 
            this.columnHash.HeaderText = "Hash";
            this.columnHash.Name = "columnHash";
            this.columnHash.ReadOnly = true;
            // 
            // dumpInfoToolStripMenuItem
            // 
            this.dumpInfoToolStripMenuItem.Name = "dumpInfoToolStripMenuItem";
            this.dumpInfoToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.dumpInfoToolStripMenuItem.Text = "Dump Info";
            this.dumpInfoToolStripMenuItem.Click += new System.EventHandler(this.DumpInfo_Clicked);
            // 
            // MaterialTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(785, 431);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.MaterialGrid);
            this.Controls.Add(this.MaterialSearch);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MaterialTool";
            this.Text = "Material Library Editor";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox MaterialSearch;
        private System.Windows.Forms.PropertyGrid MaterialGrid;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton contextFileButton;
        private System.Windows.Forms.ToolStripMenuItem contextOpenButton;
        private System.Windows.Forms.ToolStripMenuItem contextSaveButton;
        private System.Windows.Forms.ToolStripMenuItem contextExitButton;
        private System.Windows.Forms.ToolStripDropDownButton toolButton;
        private System.Windows.Forms.ToolStripMenuItem addMaterialToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DeleteSelectedMaterialButton;
        private System.Windows.Forms.ToolStripMenuItem contextReloadButton;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnHash;
        private System.Windows.Forms.ToolStripMenuItem dumpInfoToolStripMenuItem;
    }
}