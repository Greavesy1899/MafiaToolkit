namespace Forms.EditorControls
{
    partial class FrameResourceModelImporter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrameResourceModelImporter));
            this.ImportAOBox = new System.Windows.Forms.CheckBox();
            this.Split_ModelPage = new System.Windows.Forms.SplitContainer();
            this.TreeView_Objects = new Mafia2Tool.Controls.MTreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.PropertyGrid_Model = new System.Windows.Forms.PropertyGrid();
            this.Panel_Material = new System.Windows.Forms.TableLayoutPanel();
            this.Label_ChoosePreset = new System.Windows.Forms.Label();
            this.ComboBox_ChoosePreset = new System.Windows.Forms.ComboBox();
            this.Label_ChooseLibrary = new System.Windows.Forms.Label();
            this.ComboBox_ChooseLibrary = new System.Windows.Forms.ComboBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.Button_Validate = new System.Windows.Forms.ToolStripButton();
            this.Button_Continue = new System.Windows.Forms.ToolStripButton();
            this.Button_StopImport = new System.Windows.Forms.ToolStripButton();
            this.Label_DebugMessage = new System.Windows.Forms.ToolStripLabel();
            this.Tab_Root = new System.Windows.Forms.TabControl();
            this.TabPage_Validation = new System.Windows.Forms.TabPage();
            this.ListBox_Validation = new System.Windows.Forms.ListBox();
            this.TabPage_ConvertLogs = new System.Windows.Forms.TabPage();
            this.ListBox_ImportLog = new System.Windows.Forms.ListBox();
            this.TabControl_Editors = new System.Windows.Forms.TabControl();
            this.TabPage_Model = new System.Windows.Forms.TabPage();
            this.TabPage_Material = new System.Windows.Forms.TabPage();
            this.Split_MaterialPage = new System.Windows.Forms.SplitContainer();
            this.ListView_Materials = new Mafia2Tool.Controls.MListView();
            this.Split_MaterialEditor = new System.Windows.Forms.SplitContainer();
            this.PropertyGrid_Material = new System.Windows.Forms.PropertyGrid();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.Split_ModelPage)).BeginInit();
            this.Split_ModelPage.Panel1.SuspendLayout();
            this.Split_ModelPage.Panel2.SuspendLayout();
            this.Split_ModelPage.SuspendLayout();
            this.Panel_Material.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.Tab_Root.SuspendLayout();
            this.TabPage_Validation.SuspendLayout();
            this.TabPage_ConvertLogs.SuspendLayout();
            this.TabControl_Editors.SuspendLayout();
            this.TabPage_Model.SuspendLayout();
            this.TabPage_Material.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Split_MaterialPage)).BeginInit();
            this.Split_MaterialPage.Panel1.SuspendLayout();
            this.Split_MaterialPage.Panel2.SuspendLayout();
            this.Split_MaterialPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Split_MaterialEditor)).BeginInit();
            this.Split_MaterialEditor.Panel1.SuspendLayout();
            this.Split_MaterialEditor.Panel2.SuspendLayout();
            this.Split_MaterialEditor.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ImportAOBox
            // 
            this.ImportAOBox.AutoSize = true;
            this.ImportAOBox.Location = new System.Drawing.Point(19, 147);
            this.ImportAOBox.Name = "ImportAOBox";
            this.ImportAOBox.Size = new System.Drawing.Size(95, 17);
            this.ImportAOBox.TabIndex = 9;
            this.ImportAOBox.Text = "$IMPORT_AO";
            this.ImportAOBox.UseVisualStyleBackColor = true;
            // 
            // Split_ModelPage
            // 
            this.Split_ModelPage.Cursor = System.Windows.Forms.Cursors.VSplit;
            this.Split_ModelPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Split_ModelPage.Location = new System.Drawing.Point(3, 3);
            this.Split_ModelPage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Split_ModelPage.Name = "Split_ModelPage";
            // 
            // Split_ModelPage.Panel1
            // 
            this.Split_ModelPage.Panel1.Controls.Add(this.TreeView_Objects);
            // 
            // Split_ModelPage.Panel2
            // 
            this.Split_ModelPage.Panel2.Controls.Add(this.PropertyGrid_Model);
            this.Split_ModelPage.Size = new System.Drawing.Size(708, 297);
            this.Split_ModelPage.SplitterDistance = 236;
            this.Split_ModelPage.SplitterWidth = 5;
            this.Split_ModelPage.TabIndex = 16;
            // 
            // TreeView_Objects
            // 
            this.TreeView_Objects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TreeView_Objects.ImageIndex = 0;
            this.TreeView_Objects.ImageList = this.imageList1;
            this.TreeView_Objects.Location = new System.Drawing.Point(0, 0);
            this.TreeView_Objects.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TreeView_Objects.Name = "TreeView_Objects";
            this.TreeView_Objects.SelectedImageIndex = 0;
            this.TreeView_Objects.Size = new System.Drawing.Size(236, 297);
            this.TreeView_Objects.TabIndex = 0;
            this.TreeView_Objects.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.TreeView_OnBeforeSelect);
            this.TreeView_Objects.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeView_OnAfterSelect);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "StatusInvalid_16x.png");
            this.imageList1.Images.SetKeyName(1, "StatusOK_16x.png");
            this.imageList1.Images.SetKeyName(2, "MaterialGroup_16x.png");
            this.imageList1.Images.SetKeyName(3, "Model3D_16x.png");
            this.imageList1.Images.SetKeyName(4, "Log_16x.png");
            this.imageList1.Images.SetKeyName(5, "TestResultFile_16x.png");
            // 
            // PropertyGrid_Model
            // 
            this.PropertyGrid_Model.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PropertyGrid_Model.Location = new System.Drawing.Point(0, 0);
            this.PropertyGrid_Model.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.PropertyGrid_Model.Name = "PropertyGrid_Model";
            this.PropertyGrid_Model.Size = new System.Drawing.Size(467, 297);
            this.PropertyGrid_Model.TabIndex = 16;
            this.PropertyGrid_Model.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.PropertyGrid_Model_OnPropertyValueChanged);
            // 
            // Panel_Material
            // 
            this.Panel_Material.AutoSize = true;
            this.Panel_Material.ColumnCount = 2;
            this.Panel_Material.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 39.75904F));
            this.Panel_Material.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.24096F));
            this.Panel_Material.Controls.Add(this.Label_ChoosePreset, 0, 1);
            this.Panel_Material.Controls.Add(this.ComboBox_ChoosePreset, 1, 1);
            this.Panel_Material.Controls.Add(this.Label_ChooseLibrary, 0, 0);
            this.Panel_Material.Controls.Add(this.ComboBox_ChooseLibrary, 1, 0);
            this.Panel_Material.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel_Material.Location = new System.Drawing.Point(0, 0);
            this.Panel_Material.Name = "Panel_Material";
            this.Panel_Material.RowCount = 2;
            this.Panel_Material.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.Panel_Material.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.Panel_Material.Size = new System.Drawing.Size(467, 57);
            this.Panel_Material.TabIndex = 0;
            // 
            // Label_ChoosePreset
            // 
            this.Label_ChoosePreset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Label_ChoosePreset.Location = new System.Drawing.Point(3, 29);
            this.Label_ChoosePreset.Name = "Label_ChoosePreset";
            this.Label_ChoosePreset.Size = new System.Drawing.Size(179, 29);
            this.Label_ChoosePreset.TabIndex = 2;
            this.Label_ChoosePreset.Text = "$MT_CHOOSEPRESET";
            this.Label_ChoosePreset.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ComboBox_ChoosePreset
            // 
            this.ComboBox_ChoosePreset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComboBox_ChoosePreset.FormattingEnabled = true;
            this.ComboBox_ChoosePreset.Location = new System.Drawing.Point(188, 32);
            this.ComboBox_ChoosePreset.Name = "ComboBox_ChoosePreset";
            this.ComboBox_ChoosePreset.Size = new System.Drawing.Size(276, 23);
            this.ComboBox_ChoosePreset.TabIndex = 3;
            // 
            // Label_ChooseLibrary
            // 
            this.Label_ChooseLibrary.AutoSize = true;
            this.Label_ChooseLibrary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Label_ChooseLibrary.Location = new System.Drawing.Point(3, 0);
            this.Label_ChooseLibrary.Name = "Label_ChooseLibrary";
            this.Label_ChooseLibrary.Size = new System.Drawing.Size(179, 29);
            this.Label_ChooseLibrary.TabIndex = 0;
            this.Label_ChooseLibrary.Text = "$MT_CHOOSELIBRARY";
            this.Label_ChooseLibrary.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ComboBox_ChooseLibrary
            // 
            this.ComboBox_ChooseLibrary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComboBox_ChooseLibrary.FormattingEnabled = true;
            this.ComboBox_ChooseLibrary.Location = new System.Drawing.Point(188, 3);
            this.ComboBox_ChooseLibrary.Name = "ComboBox_ChooseLibrary";
            this.ComboBox_ChooseLibrary.Size = new System.Drawing.Size(276, 23);
            this.ComboBox_ChooseLibrary.TabIndex = 1;
            this.ComboBox_ChooseLibrary.SelectionChangeCommitted += new System.EventHandler(this.ComboBox_Preset_SelectionChangeCommitted);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Button_Validate,
            this.Button_Continue,
            this.Button_StopImport,
            this.Label_DebugMessage});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(728, 25);
            this.toolStrip1.TabIndex = 18;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // Button_Validate
            // 
            this.Button_Validate.Image = ((System.Drawing.Image)(resources.GetObject("Button_Validate.Image")));
            this.Button_Validate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_Validate.Name = "Button_Validate";
            this.Button_Validate.Size = new System.Drawing.Size(68, 22);
            this.Button_Validate.Text = "Validate";
            this.Button_Validate.Click += new System.EventHandler(this.Button_Validate_Click);
            // 
            // Button_Continue
            // 
            this.Button_Continue.Image = ((System.Drawing.Image)(resources.GetObject("Button_Continue.Image")));
            this.Button_Continue.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_Continue.Name = "Button_Continue";
            this.Button_Continue.Size = new System.Drawing.Size(76, 22);
            this.Button_Continue.Text = "Continue";
            this.Button_Continue.Click += new System.EventHandler(this.Button_Continue_Click);
            // 
            // Button_StopImport
            // 
            this.Button_StopImport.Image = ((System.Drawing.Image)(resources.GetObject("Button_StopImport.Image")));
            this.Button_StopImport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_StopImport.Name = "Button_StopImport";
            this.Button_StopImport.Size = new System.Drawing.Size(90, 22);
            this.Button_StopImport.Text = "Stop Import";
            this.Button_StopImport.Click += new System.EventHandler(this.Button_StopImport_Click);
            // 
            // Label_DebugMessage
            // 
            this.Label_DebugMessage.Name = "Label_DebugMessage";
            this.Label_DebugMessage.Size = new System.Drawing.Size(86, 22);
            this.Label_DebugMessage.Text = "toolStripLabel1";
            // 
            // Tab_Root
            // 
            this.Tab_Root.Controls.Add(this.TabPage_Validation);
            this.Tab_Root.Controls.Add(this.TabPage_ConvertLogs);
            this.Tab_Root.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tab_Root.ImageList = this.imageList1;
            this.Tab_Root.Location = new System.Drawing.Point(4, 365);
            this.Tab_Root.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Tab_Root.Name = "Tab_Root";
            this.Tab_Root.SelectedIndex = 0;
            this.Tab_Root.Size = new System.Drawing.Size(720, 141);
            this.Tab_Root.TabIndex = 19;
            // 
            // TabPage_Validation
            // 
            this.TabPage_Validation.Controls.Add(this.ListBox_Validation);
            this.TabPage_Validation.ImageIndex = 5;
            this.TabPage_Validation.Location = new System.Drawing.Point(4, 24);
            this.TabPage_Validation.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TabPage_Validation.Name = "TabPage_Validation";
            this.TabPage_Validation.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TabPage_Validation.Size = new System.Drawing.Size(712, 113);
            this.TabPage_Validation.TabIndex = 0;
            this.TabPage_Validation.Text = "Validation";
            this.TabPage_Validation.UseVisualStyleBackColor = true;
            // 
            // ListBox_Validation
            // 
            this.ListBox_Validation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListBox_Validation.FormattingEnabled = true;
            this.ListBox_Validation.ItemHeight = 15;
            this.ListBox_Validation.Location = new System.Drawing.Point(4, 3);
            this.ListBox_Validation.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ListBox_Validation.Name = "ListBox_Validation";
            this.ListBox_Validation.ScrollAlwaysVisible = true;
            this.ListBox_Validation.Size = new System.Drawing.Size(704, 107);
            this.ListBox_Validation.TabIndex = 0;
            // 
            // TabPage_ConvertLogs
            // 
            this.TabPage_ConvertLogs.Controls.Add(this.ListBox_ImportLog);
            this.TabPage_ConvertLogs.ImageIndex = 4;
            this.TabPage_ConvertLogs.Location = new System.Drawing.Point(4, 24);
            this.TabPage_ConvertLogs.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TabPage_ConvertLogs.Name = "TabPage_ConvertLogs";
            this.TabPage_ConvertLogs.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TabPage_ConvertLogs.Size = new System.Drawing.Size(712, 113);
            this.TabPage_ConvertLogs.TabIndex = 1;
            this.TabPage_ConvertLogs.Text = "Import Log";
            this.TabPage_ConvertLogs.UseVisualStyleBackColor = true;
            // 
            // ListBox_ImportLog
            // 
            this.ListBox_ImportLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListBox_ImportLog.FormattingEnabled = true;
            this.ListBox_ImportLog.ItemHeight = 15;
            this.ListBox_ImportLog.Location = new System.Drawing.Point(4, 3);
            this.ListBox_ImportLog.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ListBox_ImportLog.Name = "ListBox_ImportLog";
            this.ListBox_ImportLog.ScrollAlwaysVisible = true;
            this.ListBox_ImportLog.Size = new System.Drawing.Size(704, 107);
            this.ListBox_ImportLog.TabIndex = 0;
            // 
            // TabControl_Editors
            // 
            this.TabControl_Editors.Controls.Add(this.TabPage_Model);
            this.TabControl_Editors.Controls.Add(this.TabPage_Material);
            this.TabControl_Editors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabControl_Editors.ImageList = this.imageList1;
            this.TabControl_Editors.Location = new System.Drawing.Point(3, 28);
            this.TabControl_Editors.Name = "TabControl_Editors";
            this.TabControl_Editors.SelectedIndex = 0;
            this.TabControl_Editors.Size = new System.Drawing.Size(722, 331);
            this.TabControl_Editors.TabIndex = 1;
            this.TabControl_Editors.SelectedIndexChanged += new System.EventHandler(this.TabControl_Editors_TabIndexChanged);
            this.TabControl_Editors.TabIndexChanged += new System.EventHandler(this.TabControl_Editors_TabIndexChanged);
            // 
            // TabPage_Model
            // 
            this.TabPage_Model.Controls.Add(this.Split_ModelPage);
            this.TabPage_Model.ImageIndex = 3;
            this.TabPage_Model.Location = new System.Drawing.Point(4, 24);
            this.TabPage_Model.Name = "TabPage_Model";
            this.TabPage_Model.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage_Model.Size = new System.Drawing.Size(714, 303);
            this.TabPage_Model.TabIndex = 0;
            this.TabPage_Model.Text = "$TAB_MODEL";
            this.TabPage_Model.UseVisualStyleBackColor = true;
            // 
            // TabPage_Material
            // 
            this.TabPage_Material.Controls.Add(this.Split_MaterialPage);
            this.TabPage_Material.ImageIndex = 2;
            this.TabPage_Material.Location = new System.Drawing.Point(4, 24);
            this.TabPage_Material.Name = "TabPage_Material";
            this.TabPage_Material.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage_Material.Size = new System.Drawing.Size(714, 303);
            this.TabPage_Material.TabIndex = 1;
            this.TabPage_Material.Text = "$TAB_MATERIAL";
            this.TabPage_Material.UseVisualStyleBackColor = true;
            // 
            // Split_MaterialPage
            // 
            this.Split_MaterialPage.Cursor = System.Windows.Forms.Cursors.VSplit;
            this.Split_MaterialPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Split_MaterialPage.Location = new System.Drawing.Point(3, 3);
            this.Split_MaterialPage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Split_MaterialPage.Name = "Split_MaterialPage";
            // 
            // Split_MaterialPage.Panel1
            // 
            this.Split_MaterialPage.Panel1.Controls.Add(this.ListView_Materials);
            // 
            // Split_MaterialPage.Panel2
            // 
            this.Split_MaterialPage.Panel2.Controls.Add(this.Split_MaterialEditor);
            this.Split_MaterialPage.Size = new System.Drawing.Size(708, 297);
            this.Split_MaterialPage.SplitterDistance = 236;
            this.Split_MaterialPage.SplitterWidth = 5;
            this.Split_MaterialPage.TabIndex = 17;
            // 
            // ListView_Materials
            // 
            this.ListView_Materials.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListView_Materials.FullRowSelect = true;
            this.ListView_Materials.GridLines = true;
            this.ListView_Materials.GroupImageList = this.imageList1;
            this.ListView_Materials.HideSelection = false;
            this.ListView_Materials.LargeImageList = this.imageList1;
            this.ListView_Materials.Location = new System.Drawing.Point(0, 0);
            this.ListView_Materials.Name = "ListView_Materials";
            this.ListView_Materials.Size = new System.Drawing.Size(236, 297);
            this.ListView_Materials.SmallImageList = this.imageList1;
            this.ListView_Materials.TabIndex = 0;
            this.ListView_Materials.UseCompatibleStateImageBehavior = false;
            this.ListView_Materials.View = System.Windows.Forms.View.List;
            this.ListView_Materials.SelectedIndexChanged += new System.EventHandler(this.ListView_Materials_SelectedIndexChanged);
            // 
            // Split_MaterialEditor
            // 
            this.Split_MaterialEditor.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.Split_MaterialEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Split_MaterialEditor.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.Split_MaterialEditor.IsSplitterFixed = true;
            this.Split_MaterialEditor.Location = new System.Drawing.Point(0, 0);
            this.Split_MaterialEditor.Name = "Split_MaterialEditor";
            this.Split_MaterialEditor.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // Split_MaterialEditor.Panel1
            // 
            this.Split_MaterialEditor.Panel1.Controls.Add(this.Panel_Material);
            // 
            // Split_MaterialEditor.Panel2
            // 
            this.Split_MaterialEditor.Panel2.Controls.Add(this.PropertyGrid_Material);
            this.Split_MaterialEditor.Size = new System.Drawing.Size(467, 297);
            this.Split_MaterialEditor.SplitterDistance = 57;
            this.Split_MaterialEditor.TabIndex = 1;
            // 
            // PropertyGrid_Material
            // 
            this.PropertyGrid_Material.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PropertyGrid_Material.Location = new System.Drawing.Point(0, 0);
            this.PropertyGrid_Material.Name = "PropertyGrid_Material";
            this.PropertyGrid_Material.Size = new System.Drawing.Size(467, 236);
            this.PropertyGrid_Material.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.TabControl_Editors, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.toolStrip1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.Tab_Root, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 147F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(728, 509);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // FrameResourceModelOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(728, 509);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrameResourceModelOptions";
            this.Text = "$MODEL_OPTIONS_TITLE";
            this.Split_ModelPage.Panel1.ResumeLayout(false);
            this.Split_ModelPage.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Split_ModelPage)).EndInit();
            this.Split_ModelPage.ResumeLayout(false);
            this.Panel_Material.ResumeLayout(false);
            this.Panel_Material.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.Tab_Root.ResumeLayout(false);
            this.TabPage_Validation.ResumeLayout(false);
            this.TabPage_ConvertLogs.ResumeLayout(false);
            this.TabControl_Editors.ResumeLayout(false);
            this.TabPage_Model.ResumeLayout(false);
            this.TabPage_Material.ResumeLayout(false);
            this.Split_MaterialPage.Panel1.ResumeLayout(false);
            this.Split_MaterialPage.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Split_MaterialPage)).EndInit();
            this.Split_MaterialPage.ResumeLayout(false);
            this.Split_MaterialEditor.Panel1.ResumeLayout(false);
            this.Split_MaterialEditor.Panel1.PerformLayout();
            this.Split_MaterialEditor.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Split_MaterialEditor)).EndInit();
            this.Split_MaterialEditor.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.CheckBox ImportAOBox;
        private System.Windows.Forms.SplitContainer Split_ModelPage;
        private Mafia2Tool.Controls.MTreeView TreeView_Objects;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.TabControl Tab_Root;
        private System.Windows.Forms.TabPage TabPage_Validation;
        private System.Windows.Forms.TabPage TabPage_ConvertLogs;
        private System.Windows.Forms.ListBox ListBox_Validation;
        private System.Windows.Forms.ListBox ListBox_ImportLog;
        private System.Windows.Forms.ToolStripButton Button_Validate;
        private System.Windows.Forms.ToolStripButton Button_Continue;
        private System.Windows.Forms.ToolStripButton Button_StopImport;
        private System.Windows.Forms.ToolStripLabel Label_DebugMessage;
        private System.Windows.Forms.TabControl TabControl_Editors;
        private System.Windows.Forms.TabPage TabPage_Model;
        private System.Windows.Forms.TabPage TabPage_Material;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.SplitContainer Split_MaterialPage;
        private System.Windows.Forms.PropertyGrid PropertyGrid_Model;
        private Mafia2Tool.Controls.MListView ListView_Materials;
        private System.Windows.Forms.TableLayoutPanel Panel_Material;
        private System.Windows.Forms.Label Label_ChooseLibrary;
        private System.Windows.Forms.ComboBox ComboBox_ChooseLibrary;
        private System.Windows.Forms.Label Label_ChoosePreset;
        private System.Windows.Forms.ComboBox ComboBox_ChoosePreset;
        private System.Windows.Forms.SplitContainer Split_MaterialEditor;
        private System.Windows.Forms.PropertyGrid PropertyGrid_Material;
    }
}