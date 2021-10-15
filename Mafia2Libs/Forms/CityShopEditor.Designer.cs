namespace Toolkit
{
    partial class CityShopEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CityShopEditor));
            this.CollisionContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ContextDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.deletePlacementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openM2T = new System.Windows.Forms.OpenFileDialog();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.fileToolButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.Button_SaveNonDLC = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_SaveDLC = new System.Windows.Forms.ToolStripMenuItem();
            this.ReloadButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitButton = new System.Windows.Forms.ToolStripMenuItem();
            this.toolButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.AddAreaButton = new System.Windows.Forms.ToolStripMenuItem();
            this.AddDataButton = new System.Windows.Forms.ToolStripMenuItem();
            this.DuplicateDataButton = new System.Windows.Forms.ToolStripMenuItem();
            this.PopulateTranslokatorButton = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteAreaButton = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteDataButton = new System.Windows.Forms.ToolStripMenuItem();
            this.TreeView_CityShop = new Toolkit.Controls.MTreeView();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.TabControl = new System.Windows.Forms.TabControl();
            this.PropertyGridTab = new System.Windows.Forms.TabPage();
            this.DataGridTab = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.CollisionContext.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.TabControl.SuspendLayout();
            this.PropertyGridTab.SuspendLayout();
            this.DataGridTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
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
            this.toolStrip1.Size = new System.Drawing.Size(933, 25);
            this.toolStrip1.TabIndex = 15;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // fileToolButton
            // 
            this.fileToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.fileToolButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Button_SaveNonDLC,
            this.Button_SaveDLC,
            this.ReloadButton,
            this.ExitButton});
            this.fileToolButton.Image = ((System.Drawing.Image)(resources.GetObject("fileToolButton.Image")));
            this.fileToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.fileToolButton.Name = "fileToolButton";
            this.fileToolButton.Size = new System.Drawing.Size(47, 22);
            this.fileToolButton.Text = "$FILE";
            // 
            // Button_SaveNonDLC
            // 
            this.Button_SaveNonDLC.Name = "Button_SaveNonDLC";
            this.Button_SaveNonDLC.Size = new System.Drawing.Size(133, 22);
            this.Button_SaveNonDLC.Text = "$SAVE";
            this.Button_SaveNonDLC.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // Button_SaveDLC
            // 
            this.Button_SaveDLC.Name = "Button_SaveDLC";
            this.Button_SaveDLC.Size = new System.Drawing.Size(133, 22);
            this.Button_SaveDLC.Text = "$SAVE_DLC";
            this.Button_SaveDLC.Click += new System.EventHandler(this.SaveButtonDLC_Click);
            // 
            // ReloadButton
            // 
            this.ReloadButton.Name = "ReloadButton";
            this.ReloadButton.Size = new System.Drawing.Size(133, 22);
            this.ReloadButton.Text = "$RELOAD";
            this.ReloadButton.Click += new System.EventHandler(this.ReloadButton_Click);
            // 
            // ExitButton
            // 
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(133, 22);
            this.ExitButton.Text = "$EXIT";
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // toolButton
            // 
            this.toolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddAreaButton,
            this.AddDataButton,
            this.DuplicateDataButton,
            this.PopulateTranslokatorButton,
            this.DeleteAreaButton,
            this.DeleteDataButton});
            this.toolButton.Image = ((System.Drawing.Image)(resources.GetObject("toolButton.Image")));
            this.toolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButton.Name = "toolButton";
            this.toolButton.Size = new System.Drawing.Size(61, 22);
            this.toolButton.Text = "$TOOLS";
            // 
            // AddAreaButton
            // 
            this.AddAreaButton.Name = "AddAreaButton";
            this.AddAreaButton.Size = new System.Drawing.Size(233, 22);
            this.AddAreaButton.Text = "$ADD_AREA";
            this.AddAreaButton.Click += new System.EventHandler(this.AddAreaButton_Click);
            // 
            // AddDataButton
            // 
            this.AddDataButton.Name = "AddDataButton";
            this.AddDataButton.Size = new System.Drawing.Size(233, 22);
            this.AddDataButton.Text = "$ADD_DATA";
            this.AddDataButton.Click += new System.EventHandler(this.AddDataButton_Click);
            // 
            // DuplicateDataButton
            // 
            this.DuplicateDataButton.Name = "DuplicateDataButton";
            this.DuplicateDataButton.Size = new System.Drawing.Size(233, 22);
            this.DuplicateDataButton.Text = "$DUPLICATE_DATA";
            this.DuplicateDataButton.Click += new System.EventHandler(this.DuplicateData_OnClick);
            // 
            // PopulateTranslokatorButton
            // 
            this.PopulateTranslokatorButton.Name = "PopulateTranslokatorButton";
            this.PopulateTranslokatorButton.Size = new System.Drawing.Size(233, 22);
            this.PopulateTranslokatorButton.Text = "$POPULATE_TRANSLOKATORS";
            this.PopulateTranslokatorButton.Click += new System.EventHandler(this.PopulateTranslokatorButton_Click);
            // 
            // DeleteAreaButton
            // 
            this.DeleteAreaButton.Name = "DeleteAreaButton";
            this.DeleteAreaButton.Size = new System.Drawing.Size(233, 22);
            this.DeleteAreaButton.Text = "$DELETE_AREA";
            this.DeleteAreaButton.Click += new System.EventHandler(this.DeleteArea_Click);
            // 
            // DeleteDataButton
            // 
            this.DeleteDataButton.Name = "DeleteDataButton";
            this.DeleteDataButton.Size = new System.Drawing.Size(233, 22);
            this.DeleteDataButton.Text = "$DELETE_DATA";
            this.DeleteDataButton.Click += new System.EventHandler(this.DeleteData_Click);
            // 
            // TreeView_CityShop
            // 
            this.TreeView_CityShop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.TreeView_CityShop.Location = new System.Drawing.Point(14, 32);
            this.TreeView_CityShop.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TreeView_CityShop.Name = "TreeView_CityShop";
            this.TreeView_CityShop.Size = new System.Drawing.Size(360, 472);
            this.TreeView_CityShop.TabIndex = 16;
            this.TreeView_CityShop.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnAfterSelect);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(4, 3);
            this.propertyGrid1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(522, 439);
            this.propertyGrid1.TabIndex = 17;
            this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.OnPropertyChanged);
            // 
            // TabControl
            // 
            this.TabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TabControl.Controls.Add(this.PropertyGridTab);
            this.TabControl.Controls.Add(this.DataGridTab);
            this.TabControl.Location = new System.Drawing.Point(382, 32);
            this.TabControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            this.TabControl.Size = new System.Drawing.Size(538, 473);
            this.TabControl.TabIndex = 18;
            this.TabControl.Selected += new System.Windows.Forms.TabControlEventHandler(this.OnTabSelected);
            // 
            // PropertyGridTab
            // 
            this.PropertyGridTab.Controls.Add(this.propertyGrid1);
            this.PropertyGridTab.Location = new System.Drawing.Point(4, 24);
            this.PropertyGridTab.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.PropertyGridTab.Name = "PropertyGridTab";
            this.PropertyGridTab.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.PropertyGridTab.Size = new System.Drawing.Size(530, 445);
            this.PropertyGridTab.TabIndex = 0;
            this.PropertyGridTab.Text = "Properties";
            this.PropertyGridTab.UseVisualStyleBackColor = true;
            // 
            // DataGridTab
            // 
            this.DataGridTab.Controls.Add(this.dataGridView1);
            this.DataGridTab.Location = new System.Drawing.Point(4, 24);
            this.DataGridTab.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.DataGridTab.Name = "DataGridTab";
            this.DataGridTab.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.DataGridTab.Size = new System.Drawing.Size(530, 445);
            this.DataGridTab.TabIndex = 1;
            this.DataGridTab.Text = "Entity Grid";
            this.DataGridTab.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(4, 3);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(522, 439);
            this.dataGridView1.TabIndex = 0;
            // 
            // CityShopEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 519);
            this.Controls.Add(this.TreeView_CityShop);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.TabControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "CityShopEditor";
            this.Text = "$ACTOR_EDITOR_TITLE";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CityShopEditor_Closing);
            this.CollisionContext.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.TabControl.ResumeLayout(false);
            this.PropertyGridTab.ResumeLayout(false);
            this.DataGridTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openM2T;
        private System.Windows.Forms.TabControl TabControl;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton fileToolButton;
        private System.Windows.Forms.ToolStripMenuItem Button_SaveNonDLC;
        private System.Windows.Forms.ToolStripMenuItem ReloadButton;
        private System.Windows.Forms.ToolStripMenuItem ExitButton;
        private System.Windows.Forms.ContextMenuStrip CollisionContext;
        private System.Windows.Forms.ToolStripMenuItem ContextDelete;
        private System.Windows.Forms.ToolStripMenuItem deletePlacementToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolButton;
        private System.Windows.Forms.ToolStripMenuItem AddAreaButton;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.ToolStripMenuItem PopulateTranslokatorButton;
        private System.Windows.Forms.ToolStripMenuItem AddDataButton;
        private System.Windows.Forms.TabPage PropertyGridTab;
        private System.Windows.Forms.TabPage DataGridTab;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ToolStripMenuItem DuplicateDataButton;
        private System.Windows.Forms.ToolStripMenuItem DeleteAreaButton;
        private System.Windows.Forms.ToolStripMenuItem DeleteDataButton;
        private System.Windows.Forms.ToolStripMenuItem Button_SaveDLC;
        private Controls.MTreeView TreeView_CityShop;
    }
}