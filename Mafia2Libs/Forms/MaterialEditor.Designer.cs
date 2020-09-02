namespace Mafia2Tool {
    partial class MaterialEditor {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MaterialEditor));
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
            this.MergeMTLButton = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_ExportSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.columnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnHash = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MTLBrowser = new System.Windows.Forms.OpenFileDialog();
            this.Panel_Main = new System.Windows.Forms.Panel();
            this.Button_Search = new System.Windows.Forms.Button();
            this.Label_SearchType = new System.Windows.Forms.Label();
            this.ComboBox_SearchType = new System.Windows.Forms.ComboBox();
            this.MergePanel = new System.Windows.Forms.Panel();
            this.SelectAllNewButton = new System.Windows.Forms.Button();
            this.SelectAllOverwriteButton = new System.Windows.Forms.Button();
            this.NewMaterialLabel = new System.Windows.Forms.Label();
            this.OverWriteLabel = new System.Windows.Forms.Label();
            this.NewMatListBox = new System.Windows.Forms.CheckedListBox();
            this.CancelButton = new System.Windows.Forms.Button();
            this.MergeButton = new System.Windows.Forms.Button();
            this.OverwriteListBox = new System.Windows.Forms.CheckedListBox();
            this.MTLSaveDialog = new System.Windows.Forms.SaveFileDialog();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.Panel_Main.SuspendLayout();
            this.MergePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MaterialSearch
            // 
            this.MaterialSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.MaterialSearch.Location = new System.Drawing.Point(3, 28);
            this.MaterialSearch.Name = "MaterialSearch";
            this.MaterialSearch.Size = new System.Drawing.Size(209, 20);
            this.MaterialSearch.TabIndex = 0;
            // 
            // MaterialGrid
            // 
            this.MaterialGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MaterialGrid.Location = new System.Drawing.Point(338, 3);
            this.MaterialGrid.Name = "MaterialGrid";
            this.MaterialGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.MaterialGrid.Size = new System.Drawing.Size(447, 400);
            this.MaterialGrid.TabIndex = 2;
            this.MaterialGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.MaterialGrid_OnPropertyValueChanged);
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
            this.MergeMTLButton,
            this.Button_ExportSelected});
            this.toolButton.Image = ((System.Drawing.Image)(resources.GetObject("toolButton.Image")));
            this.toolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButton.Name = "toolButton";
            this.toolButton.Size = new System.Drawing.Size(47, 22);
            this.toolButton.Text = "Tools";
            // 
            // addMaterialToolStripMenuItem
            // 
            this.addMaterialToolStripMenuItem.Name = "addMaterialToolStripMenuItem";
            this.addMaterialToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.addMaterialToolStripMenuItem.Text = "Add Material";
            this.addMaterialToolStripMenuItem.Click += new System.EventHandler(this.AddMaterial);
            // 
            // DeleteSelectedMaterialButton
            // 
            this.DeleteSelectedMaterialButton.Name = "DeleteSelectedMaterialButton";
            this.DeleteSelectedMaterialButton.Size = new System.Drawing.Size(178, 22);
            this.DeleteSelectedMaterialButton.Text = "$DELETE_SEL_MAT";
            this.DeleteSelectedMaterialButton.Click += new System.EventHandler(this.DeleteMaterial);
            // 
            // MergeMTLButton
            // 
            this.MergeMTLButton.Name = "MergeMTLButton";
            this.MergeMTLButton.Size = new System.Drawing.Size(178, 22);
            this.MergeMTLButton.Text = "$MERGE_MTL";
            this.MergeMTLButton.Click += new System.EventHandler(this.MergeMTLButton_Click);
            // 
            // Button_ExportSelected
            // 
            this.Button_ExportSelected.Name = "Button_ExportSelected";
            this.Button_ExportSelected.Size = new System.Drawing.Size(178, 22);
            this.Button_ExportSelected.Text = "$EXPORT_SELECTED";
            this.Button_ExportSelected.Click += new System.EventHandler(this.Button_ExportedSelected_Clicked);
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
            this.dataGridView1.Location = new System.Drawing.Point(3, 54);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(329, 346);
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
            // MTLBrowser
            // 
            this.MTLBrowser.Filter = "Material Library|*.mtl";
            // 
            // Panel_Main
            // 
            this.Panel_Main.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Panel_Main.Controls.Add(this.Button_Search);
            this.Panel_Main.Controls.Add(this.Label_SearchType);
            this.Panel_Main.Controls.Add(this.ComboBox_SearchType);
            this.Panel_Main.Controls.Add(this.dataGridView1);
            this.Panel_Main.Controls.Add(this.MaterialSearch);
            this.Panel_Main.Controls.Add(this.MaterialGrid);
            this.Panel_Main.Location = new System.Drawing.Point(0, 28);
            this.Panel_Main.Name = "Panel_Main";
            this.Panel_Main.Size = new System.Drawing.Size(785, 403);
            this.Panel_Main.TabIndex = 4;
            // 
            // Button_Search
            // 
            this.Button_Search.Location = new System.Drawing.Point(218, 25);
            this.Button_Search.Name = "Button_Search";
            this.Button_Search.Size = new System.Drawing.Size(114, 23);
            this.Button_Search.TabIndex = 6;
            this.Button_Search.Text = "$SEARCH";
            this.Button_Search.UseVisualStyleBackColor = true;
            this.Button_Search.Click += new System.EventHandler(this.Button_Search_Click);
            // 
            // Label_SearchType
            // 
            this.Label_SearchType.AutoSize = true;
            this.Label_SearchType.Location = new System.Drawing.Point(8, 7);
            this.Label_SearchType.Name = "Label_SearchType";
            this.Label_SearchType.Size = new System.Drawing.Size(124, 13);
            this.Label_SearchType.TabIndex = 5;
            this.Label_SearchType.Text = "$LABEL_SEARCHTYPE";
            // 
            // ComboBox_SearchType
            // 
            this.ComboBox_SearchType.FormattingEnabled = true;
            this.ComboBox_SearchType.Items.AddRange(new object[] {
            "$LABEL_MATERIALNAME",
            "$LABEL_TEXTURENAME",
            "$LABEL_MATERIALHASH",
            "$LABEL_SHADERID",
            "$LABEL_SHADERHASH"});
            this.ComboBox_SearchType.Location = new System.Drawing.Point(148, 3);
            this.ComboBox_SearchType.Name = "ComboBox_SearchType";
            this.ComboBox_SearchType.Size = new System.Drawing.Size(184, 21);
            this.ComboBox_SearchType.TabIndex = 4;
            this.ComboBox_SearchType.SelectedIndexChanged += new System.EventHandler(this.SearchType_OnIndexChanged);
            // 
            // MergePanel
            // 
            this.MergePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MergePanel.Controls.Add(this.SelectAllNewButton);
            this.MergePanel.Controls.Add(this.SelectAllOverwriteButton);
            this.MergePanel.Controls.Add(this.NewMaterialLabel);
            this.MergePanel.Controls.Add(this.OverWriteLabel);
            this.MergePanel.Controls.Add(this.NewMatListBox);
            this.MergePanel.Controls.Add(this.CancelButton);
            this.MergePanel.Controls.Add(this.MergeButton);
            this.MergePanel.Controls.Add(this.OverwriteListBox);
            this.MergePanel.Location = new System.Drawing.Point(0, 28);
            this.MergePanel.Name = "MergePanel";
            this.MergePanel.Padding = new System.Windows.Forms.Padding(5);
            this.MergePanel.Size = new System.Drawing.Size(785, 403);
            this.MergePanel.TabIndex = 4;
            // 
            // SelectAllNewButton
            // 
            this.SelectAllNewButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectAllNewButton.Location = new System.Drawing.Point(424, 28);
            this.SelectAllNewButton.Name = "SelectAllNewButton";
            this.SelectAllNewButton.Size = new System.Drawing.Size(100, 23);
            this.SelectAllNewButton.TabIndex = 8;
            this.SelectAllNewButton.Text = "$SELECT_ALL";
            this.SelectAllNewButton.UseVisualStyleBackColor = true;
            this.SelectAllNewButton.Click += new System.EventHandler(this.SelectAllNewButton_Click);
            // 
            // SelectAllOverwriteButton
            // 
            this.SelectAllOverwriteButton.Location = new System.Drawing.Point(251, 28);
            this.SelectAllOverwriteButton.Name = "SelectAllOverwriteButton";
            this.SelectAllOverwriteButton.Size = new System.Drawing.Size(100, 23);
            this.SelectAllOverwriteButton.TabIndex = 7;
            this.SelectAllOverwriteButton.Text = "$SELECT_ALL";
            this.SelectAllOverwriteButton.UseVisualStyleBackColor = true;
            this.SelectAllOverwriteButton.Click += new System.EventHandler(this.SelectAllOverwriteButton_Click);
            // 
            // NewMaterialLabel
            // 
            this.NewMaterialLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NewMaterialLabel.AutoSize = true;
            this.NewMaterialLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NewMaterialLabel.Location = new System.Drawing.Point(527, 5);
            this.NewMaterialLabel.Name = "NewMaterialLabel";
            this.NewMaterialLabel.Size = new System.Drawing.Size(96, 17);
            this.NewMaterialLabel.TabIndex = 6;
            this.NewMaterialLabel.Text = "New Materials";
            // 
            // OverWriteLabel
            // 
            this.OverWriteLabel.AutoSize = true;
            this.OverWriteLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OverWriteLabel.Location = new System.Drawing.Point(8, 5);
            this.OverWriteLabel.Name = "OverWriteLabel";
            this.OverWriteLabel.Size = new System.Drawing.Size(134, 17);
            this.OverWriteLabel.TabIndex = 5;
            this.OverWriteLabel.Text = "Conflicting Materials";
            // 
            // NewMatListBox
            // 
            this.NewMatListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NewMatListBox.FormattingEnabled = true;
            this.NewMatListBox.Location = new System.Drawing.Point(530, 23);
            this.NewMatListBox.Name = "NewMatListBox";
            this.NewMatListBox.Size = new System.Drawing.Size(243, 364);
            this.NewMatListBox.TabIndex = 4;
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CancelButton.Location = new System.Drawing.Point(251, 364);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(100, 23);
            this.CancelButton.TabIndex = 3;
            this.CancelButton.Text = "$CANCEL";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // MergeButton
            // 
            this.MergeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.MergeButton.Location = new System.Drawing.Point(424, 364);
            this.MergeButton.Name = "MergeButton";
            this.MergeButton.Size = new System.Drawing.Size(100, 23);
            this.MergeButton.TabIndex = 2;
            this.MergeButton.Text = "$MERGE";
            this.MergeButton.UseVisualStyleBackColor = true;
            this.MergeButton.Click += new System.EventHandler(this.MergeButton_Click);
            // 
            // OverwriteListBox
            // 
            this.OverwriteListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.OverwriteListBox.FormattingEnabled = true;
            this.OverwriteListBox.Location = new System.Drawing.Point(8, 23);
            this.OverwriteListBox.Name = "OverwriteListBox";
            this.OverwriteListBox.Size = new System.Drawing.Size(237, 364);
            this.OverwriteListBox.TabIndex = 0;
            // 
            // MTLSaveDialog
            // 
            this.MTLSaveDialog.Filter = "Material Library|*.mtl";
            // 
            // MaterialEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(785, 431);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.Panel_Main);
            this.Controls.Add(this.MergePanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MaterialEditor";
            this.Text = "Material Library Editor";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.Panel_Main.ResumeLayout(false);
            this.Panel_Main.PerformLayout();
            this.MergePanel.ResumeLayout(false);
            this.MergePanel.PerformLayout();
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
        private System.Windows.Forms.ToolStripMenuItem MergeMTLButton;
        private System.Windows.Forms.OpenFileDialog MTLBrowser;
        private System.Windows.Forms.Panel Panel_Main;
        private System.Windows.Forms.Panel MergePanel;
        private System.Windows.Forms.CheckedListBox OverwriteListBox;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button MergeButton;
        private System.Windows.Forms.CheckedListBox NewMatListBox;
        private System.Windows.Forms.Label OverWriteLabel;
        private System.Windows.Forms.Label NewMaterialLabel;
        private System.Windows.Forms.Button SelectAllNewButton;
        private System.Windows.Forms.Button SelectAllOverwriteButton;
        private System.Windows.Forms.Label Label_SearchType;
        private System.Windows.Forms.ComboBox ComboBox_SearchType;
        private System.Windows.Forms.ToolStripMenuItem Button_ExportSelected;
        private System.Windows.Forms.SaveFileDialog MTLSaveDialog;
        private System.Windows.Forms.Button Button_Search;
    }
}