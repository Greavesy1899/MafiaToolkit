namespace Toolkit {
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
            this.Button_File = new System.Windows.Forms.ToolStripDropDownButton();
            this.Button_Open = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Reload = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.Button_AddMaterial = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_MergeMTL = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_ExportSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Debug = new System.Windows.Forms.ToolStripDropDownButton();
            this.Button_DumpTextures = new System.Windows.Forms.ToolStripMenuItem();
            this.GirdView_Materials = new System.Windows.Forms.DataGridView();
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
            ((System.ComponentModel.ISupportInitialize)(this.GirdView_Materials)).BeginInit();
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
            this.MaterialSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MaterialSearch_KeyDown);
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
            this.Button_File,
            this.toolButton,
            this.Button_Debug});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(785, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // Button_File
            // 
            this.Button_File.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Button_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Button_Open,
            this.Button_Reload,
            this.Button_Save,
            this.Button_Exit});
            this.Button_File.Image = ((System.Drawing.Image)(resources.GetObject("Button_File.Image")));
            this.Button_File.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_File.Name = "Button_File";
            this.Button_File.Size = new System.Drawing.Size(38, 22);
            this.Button_File.Text = "File";
            // 
            // Button_Open
            // 
            this.Button_Open.Enabled = false;
            this.Button_Open.Name = "Button_Open";
            this.Button_Open.Size = new System.Drawing.Size(180, 22);
            this.Button_Open.Text = "Open";
            // 
            // Button_Reload
            // 
            this.Button_Reload.Name = "Button_Reload";
            this.Button_Reload.Size = new System.Drawing.Size(180, 22);
            this.Button_Reload.Text = "Reload";
            this.Button_Reload.Click += new System.EventHandler(this.Button_Reload_Click);
            // 
            // Button_Save
            // 
            this.Button_Save.Name = "Button_Save";
            this.Button_Save.Size = new System.Drawing.Size(180, 22);
            this.Button_Save.Text = "Save";
            this.Button_Save.Click += new System.EventHandler(this.Button_Save_Click);
            // 
            // Button_Exit
            // 
            this.Button_Exit.Name = "Button_Exit";
            this.Button_Exit.Size = new System.Drawing.Size(180, 22);
            this.Button_Exit.Text = "Exit";
            this.Button_Exit.Click += new System.EventHandler(this.Button_Exit_Click);
            // 
            // toolButton
            // 
            this.toolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Button_AddMaterial,
            this.Button_Delete,
            this.Button_MergeMTL,
            this.Button_ExportSelected});
            this.toolButton.Image = ((System.Drawing.Image)(resources.GetObject("toolButton.Image")));
            this.toolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButton.Name = "toolButton";
            this.toolButton.Size = new System.Drawing.Size(47, 22);
            this.toolButton.Text = "Tools";
            // 
            // Button_AddMaterial
            // 
            this.Button_AddMaterial.Name = "Button_AddMaterial";
            this.Button_AddMaterial.Size = new System.Drawing.Size(180, 22);
            this.Button_AddMaterial.Text = "Add Material";
            this.Button_AddMaterial.Click += new System.EventHandler(this.Button_AddMaterial_Click);
            // 
            // Button_Delete
            // 
            this.Button_Delete.Name = "Button_Delete";
            this.Button_Delete.Size = new System.Drawing.Size(180, 22);
            this.Button_Delete.Text = "$DELETE_SEL_MAT";
            this.Button_Delete.Click += new System.EventHandler(this.Button_Delete_Click);
            // 
            // Button_MergeMTL
            // 
            this.Button_MergeMTL.Name = "Button_MergeMTL";
            this.Button_MergeMTL.Size = new System.Drawing.Size(180, 22);
            this.Button_MergeMTL.Text = "$MERGE_MTL";
            this.Button_MergeMTL.Click += new System.EventHandler(this.Button_MergeMTL_Click);
            // 
            // Button_ExportSelected
            // 
            this.Button_ExportSelected.Name = "Button_ExportSelected";
            this.Button_ExportSelected.Size = new System.Drawing.Size(180, 22);
            this.Button_ExportSelected.Text = "$EXPORT_SELECTED";
            this.Button_ExportSelected.Click += new System.EventHandler(this.Button_ExportedSelected_Click);
            // 
            // Button_Debug
            // 
            this.Button_Debug.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Button_Debug.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Button_DumpTextures});
            this.Button_Debug.Image = ((System.Drawing.Image)(resources.GetObject("Button_Debug.Image")));
            this.Button_Debug.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_Debug.Name = "Button_Debug";
            this.Button_Debug.Size = new System.Drawing.Size(85, 22);
            this.Button_Debug.Text = "Debug Tools";
            // 
            // Button_DumpTextures
            // 
            this.Button_DumpTextures.Name = "Button_DumpTextures";
            this.Button_DumpTextures.Size = new System.Drawing.Size(188, 22);
            this.Button_DumpTextures.Text = "Dump Texture Names";
            this.Button_DumpTextures.Click += new System.EventHandler(this.Button_DumpTextures_Click);
            // 
            // GirdView_Materials
            // 
            this.GirdView_Materials.AllowUserToAddRows = false;
            this.GirdView_Materials.AllowUserToDeleteRows = false;
            this.GirdView_Materials.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.GirdView_Materials.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.GirdView_Materials.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GirdView_Materials.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnName,
            this.columnHash});
            this.GirdView_Materials.Location = new System.Drawing.Point(3, 54);
            this.GirdView_Materials.Name = "GirdView_Materials";
            this.GirdView_Materials.ReadOnly = true;
            this.GirdView_Materials.Size = new System.Drawing.Size(329, 346);
            this.GirdView_Materials.TabIndex = 3;
            this.GirdView_Materials.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnMaterialSelected);
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
            this.Panel_Main.Controls.Add(this.GirdView_Materials);
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
            this.CancelButton.Click += new System.EventHandler(this.Button_Cancel_Click);
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
            this.MergeButton.Click += new System.EventHandler(this.Button_Merge_Click);
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
            this.KeyPreview = true;
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
            ((System.ComponentModel.ISupportInitialize)(this.GirdView_Materials)).EndInit();
            this.Panel_Main.ResumeLayout(false);
            this.Panel_Main.PerformLayout();
            this.MergePanel.ResumeLayout(false);
            this.MergePanel.PerformLayout();
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MaterialEditor_Closing);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MaterialEditor_OnKeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox MaterialSearch;
        private System.Windows.Forms.PropertyGrid MaterialGrid;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton Button_File;
        private System.Windows.Forms.ToolStripMenuItem Button_Open;
        private System.Windows.Forms.ToolStripMenuItem Button_Save;
        private System.Windows.Forms.ToolStripMenuItem Button_Exit;
        private System.Windows.Forms.ToolStripDropDownButton toolButton;
        private System.Windows.Forms.ToolStripMenuItem Button_AddMaterial;
        private System.Windows.Forms.ToolStripMenuItem Button_Delete;
        private System.Windows.Forms.ToolStripMenuItem Button_Reload;
        private System.Windows.Forms.DataGridView GirdView_Materials;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnHash;
        private System.Windows.Forms.ToolStripMenuItem Button_MergeMTL;
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
        private System.Windows.Forms.ToolStripDropDownButton Button_Debug;
        private System.Windows.Forms.ToolStripMenuItem Button_DumpTextures;
    }
}