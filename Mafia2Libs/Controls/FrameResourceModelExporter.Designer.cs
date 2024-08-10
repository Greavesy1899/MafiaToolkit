namespace Forms.EditorControls
{
    partial class FrameResourceModelExporter
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrameResourceModelExporter));
            ImportAOBox = new System.Windows.Forms.CheckBox();
            Split_ModelPage = new System.Windows.Forms.SplitContainer();
            TreeView_Objects = new Mafia2Tool.Controls.MTreeView();
            HelperContextMenu = new System.Windows.Forms.ContextMenuStrip(components);
            imageList1 = new System.Windows.Forms.ImageList(components);
            PropertyGrid_Model = new System.Windows.Forms.PropertyGrid();
            Panel_Material = new System.Windows.Forms.TableLayoutPanel();
            Label_ChoosePreset = new System.Windows.Forms.Label();
            ComboBox_ChoosePreset = new System.Windows.Forms.ComboBox();
            Label_ChooseLibrary = new System.Windows.Forms.Label();
            ComboBox_ChooseLibrary = new System.Windows.Forms.ComboBox();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            Button_Validate = new System.Windows.Forms.ToolStripButton();
            Button_Continue = new System.Windows.Forms.ToolStripButton();
            Button_StopImport = new System.Windows.Forms.ToolStripButton();
            Label_DebugMessage = new System.Windows.Forms.ToolStripLabel();
            Tab_Root = new System.Windows.Forms.TabControl();
            TabPage_Validation = new System.Windows.Forms.TabPage();
            ListBox_Validation = new System.Windows.Forms.ListBox();
            TabPage_ConvertLogs = new System.Windows.Forms.TabPage();
            ListBox_ImportLog = new System.Windows.Forms.ListBox();
            TabControl_Editors = new System.Windows.Forms.TabControl();
            TabPage_Model = new System.Windows.Forms.TabPage();
            TabPage_Material = new System.Windows.Forms.TabPage();
            Split_MaterialPage = new System.Windows.Forms.SplitContainer();
            ListView_Materials = new Mafia2Tool.Controls.MListView();
            Split_MaterialEditor = new System.Windows.Forms.SplitContainer();
            PropertyGrid_Material = new System.Windows.Forms.PropertyGrid();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            AnimFileDialog = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)Split_ModelPage).BeginInit();
            Split_ModelPage.Panel1.SuspendLayout();
            Split_ModelPage.Panel2.SuspendLayout();
            Split_ModelPage.SuspendLayout();
            Panel_Material.SuspendLayout();
            toolStrip1.SuspendLayout();
            Tab_Root.SuspendLayout();
            TabPage_Validation.SuspendLayout();
            TabPage_ConvertLogs.SuspendLayout();
            TabControl_Editors.SuspendLayout();
            TabPage_Model.SuspendLayout();
            TabPage_Material.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Split_MaterialPage).BeginInit();
            Split_MaterialPage.Panel1.SuspendLayout();
            Split_MaterialPage.Panel2.SuspendLayout();
            Split_MaterialPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Split_MaterialEditor).BeginInit();
            Split_MaterialEditor.Panel1.SuspendLayout();
            Split_MaterialEditor.Panel2.SuspendLayout();
            Split_MaterialEditor.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // ImportAOBox
            // 
            ImportAOBox.AutoSize = true;
            ImportAOBox.Location = new System.Drawing.Point(19, 147);
            ImportAOBox.Name = "ImportAOBox";
            ImportAOBox.Size = new System.Drawing.Size(95, 17);
            ImportAOBox.TabIndex = 9;
            ImportAOBox.Text = "$IMPORT_AO";
            ImportAOBox.UseVisualStyleBackColor = true;
            // 
            // Split_ModelPage
            // 
            Split_ModelPage.Cursor = System.Windows.Forms.Cursors.VSplit;
            Split_ModelPage.Dock = System.Windows.Forms.DockStyle.Fill;
            Split_ModelPage.Location = new System.Drawing.Point(3, 3);
            Split_ModelPage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Split_ModelPage.Name = "Split_ModelPage";
            // 
            // Split_ModelPage.Panel1
            // 
            Split_ModelPage.Panel1.Controls.Add(TreeView_Objects);
            // 
            // Split_ModelPage.Panel2
            // 
            Split_ModelPage.Panel2.Controls.Add(PropertyGrid_Model);
            Split_ModelPage.Size = new System.Drawing.Size(708, 297);
            Split_ModelPage.SplitterDistance = 236;
            Split_ModelPage.SplitterWidth = 5;
            Split_ModelPage.TabIndex = 16;
            // 
            // TreeView_Objects
            // 
            TreeView_Objects.ContextMenuStrip = HelperContextMenu;
            TreeView_Objects.Dock = System.Windows.Forms.DockStyle.Fill;
            TreeView_Objects.ImageIndex = 0;
            TreeView_Objects.ImageList = imageList1;
            TreeView_Objects.Location = new System.Drawing.Point(0, 0);
            TreeView_Objects.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TreeView_Objects.Name = "TreeView_Objects";
            TreeView_Objects.SelectedImageIndex = 0;
            TreeView_Objects.Size = new System.Drawing.Size(236, 297);
            TreeView_Objects.TabIndex = 0;
            TreeView_Objects.BeforeSelect += TreeView_OnBeforeSelect;
            TreeView_Objects.AfterSelect += TreeView_OnAfterSelect;
            // 
            // HelperContextMenu
            // 
            HelperContextMenu.Name = "EntryMenuStrip";
            HelperContextMenu.Size = new System.Drawing.Size(181, 26);
            HelperContextMenu.Opening += HelperContextMenu_Opening;
            HelperContextMenu.ItemClicked += HelperContextMenu_OnItemClicked;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            imageList1.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = System.Drawing.Color.Transparent;
            imageList1.Images.SetKeyName(0, "StatusInvalid_16x.png");
            imageList1.Images.SetKeyName(1, "StatusOK_16x.png");
            imageList1.Images.SetKeyName(2, "MaterialGroup_16x.png");
            imageList1.Images.SetKeyName(3, "Model3D_16x.png");
            imageList1.Images.SetKeyName(4, "Log_16x.png");
            imageList1.Images.SetKeyName(5, "TestResultFile_16x.png");
            // 
            // PropertyGrid_Model
            // 
            PropertyGrid_Model.Dock = System.Windows.Forms.DockStyle.Fill;
            PropertyGrid_Model.Location = new System.Drawing.Point(0, 0);
            PropertyGrid_Model.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PropertyGrid_Model.Name = "PropertyGrid_Model";
            PropertyGrid_Model.Size = new System.Drawing.Size(467, 297);
            PropertyGrid_Model.TabIndex = 16;
            PropertyGrid_Model.PropertyValueChanged += PropertyGrid_Model_OnPropertyValueChanged;
            // 
            // Panel_Material
            // 
            Panel_Material.AutoSize = true;
            Panel_Material.ColumnCount = 2;
            Panel_Material.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 39.75904F));
            Panel_Material.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.24096F));
            Panel_Material.Controls.Add(Label_ChoosePreset, 0, 1);
            Panel_Material.Controls.Add(ComboBox_ChoosePreset, 1, 1);
            Panel_Material.Controls.Add(Label_ChooseLibrary, 0, 0);
            Panel_Material.Controls.Add(ComboBox_ChooseLibrary, 1, 0);
            Panel_Material.Dock = System.Windows.Forms.DockStyle.Fill;
            Panel_Material.Location = new System.Drawing.Point(0, 0);
            Panel_Material.Name = "Panel_Material";
            Panel_Material.RowCount = 2;
            Panel_Material.RowStyles.Add(new System.Windows.Forms.RowStyle());
            Panel_Material.RowStyles.Add(new System.Windows.Forms.RowStyle());
            Panel_Material.Size = new System.Drawing.Size(467, 57);
            Panel_Material.TabIndex = 0;
            // 
            // Label_ChoosePreset
            // 
            Label_ChoosePreset.Dock = System.Windows.Forms.DockStyle.Fill;
            Label_ChoosePreset.Location = new System.Drawing.Point(3, 29);
            Label_ChoosePreset.Name = "Label_ChoosePreset";
            Label_ChoosePreset.Size = new System.Drawing.Size(179, 29);
            Label_ChoosePreset.TabIndex = 2;
            Label_ChoosePreset.Text = "$MT_CHOOSEPRESET";
            Label_ChoosePreset.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ComboBox_ChoosePreset
            // 
            ComboBox_ChoosePreset.Dock = System.Windows.Forms.DockStyle.Fill;
            ComboBox_ChoosePreset.FormattingEnabled = true;
            ComboBox_ChoosePreset.Location = new System.Drawing.Point(188, 32);
            ComboBox_ChoosePreset.Name = "ComboBox_ChoosePreset";
            ComboBox_ChoosePreset.Size = new System.Drawing.Size(276, 23);
            ComboBox_ChoosePreset.TabIndex = 3;
            // 
            // Label_ChooseLibrary
            // 
            Label_ChooseLibrary.AutoSize = true;
            Label_ChooseLibrary.Dock = System.Windows.Forms.DockStyle.Fill;
            Label_ChooseLibrary.Location = new System.Drawing.Point(3, 0);
            Label_ChooseLibrary.Name = "Label_ChooseLibrary";
            Label_ChooseLibrary.Size = new System.Drawing.Size(179, 29);
            Label_ChooseLibrary.TabIndex = 0;
            Label_ChooseLibrary.Text = "$MT_CHOOSELIBRARY";
            Label_ChooseLibrary.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ComboBox_ChooseLibrary
            // 
            ComboBox_ChooseLibrary.Dock = System.Windows.Forms.DockStyle.Fill;
            ComboBox_ChooseLibrary.FormattingEnabled = true;
            ComboBox_ChooseLibrary.Location = new System.Drawing.Point(188, 3);
            ComboBox_ChooseLibrary.Name = "ComboBox_ChooseLibrary";
            ComboBox_ChooseLibrary.Size = new System.Drawing.Size(276, 23);
            ComboBox_ChooseLibrary.TabIndex = 1;
            ComboBox_ChooseLibrary.SelectionChangeCommitted += ComboBox_Preset_SelectionChangeCommitted;
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { Button_Validate, Button_Continue, Button_StopImport, Label_DebugMessage });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(728, 25);
            toolStrip1.TabIndex = 18;
            toolStrip1.Text = "toolStrip1";
            // 
            // Button_Validate
            // 
            Button_Validate.Image = (System.Drawing.Image)resources.GetObject("Button_Validate.Image");
            Button_Validate.ImageTransparentColor = System.Drawing.Color.Magenta;
            Button_Validate.Name = "Button_Validate";
            Button_Validate.Size = new System.Drawing.Size(68, 22);
            Button_Validate.Text = "Validate";
            Button_Validate.Click += Button_Validate_Click;
            // 
            // Button_Continue
            // 
            Button_Continue.Image = (System.Drawing.Image)resources.GetObject("Button_Continue.Image");
            Button_Continue.ImageTransparentColor = System.Drawing.Color.Magenta;
            Button_Continue.Name = "Button_Continue";
            Button_Continue.Size = new System.Drawing.Size(76, 22);
            Button_Continue.Text = "Continue";
            Button_Continue.Click += Button_Continue_Click;
            // 
            // Button_StopImport
            // 
            Button_StopImport.Image = (System.Drawing.Image)resources.GetObject("Button_StopImport.Image");
            Button_StopImport.ImageTransparentColor = System.Drawing.Color.Magenta;
            Button_StopImport.Name = "Button_StopImport";
            Button_StopImport.Size = new System.Drawing.Size(90, 22);
            Button_StopImport.Text = "Stop Import";
            Button_StopImport.Click += Button_StopImport_Click;
            // 
            // Label_DebugMessage
            // 
            Label_DebugMessage.Name = "Label_DebugMessage";
            Label_DebugMessage.Size = new System.Drawing.Size(86, 22);
            Label_DebugMessage.Text = "toolStripLabel1";
            // 
            // Tab_Root
            // 
            Tab_Root.Controls.Add(TabPage_Validation);
            Tab_Root.Controls.Add(TabPage_ConvertLogs);
            Tab_Root.Dock = System.Windows.Forms.DockStyle.Fill;
            Tab_Root.ImageList = imageList1;
            Tab_Root.Location = new System.Drawing.Point(4, 365);
            Tab_Root.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Tab_Root.Name = "Tab_Root";
            Tab_Root.SelectedIndex = 0;
            Tab_Root.Size = new System.Drawing.Size(720, 141);
            Tab_Root.TabIndex = 19;
            // 
            // TabPage_Validation
            // 
            TabPage_Validation.Controls.Add(ListBox_Validation);
            TabPage_Validation.ImageIndex = 5;
            TabPage_Validation.Location = new System.Drawing.Point(4, 24);
            TabPage_Validation.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TabPage_Validation.Name = "TabPage_Validation";
            TabPage_Validation.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TabPage_Validation.Size = new System.Drawing.Size(712, 113);
            TabPage_Validation.TabIndex = 0;
            TabPage_Validation.Text = "Validation";
            TabPage_Validation.UseVisualStyleBackColor = true;
            // 
            // ListBox_Validation
            // 
            ListBox_Validation.Dock = System.Windows.Forms.DockStyle.Fill;
            ListBox_Validation.FormattingEnabled = true;
            ListBox_Validation.ItemHeight = 15;
            ListBox_Validation.Location = new System.Drawing.Point(4, 3);
            ListBox_Validation.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ListBox_Validation.Name = "ListBox_Validation";
            ListBox_Validation.ScrollAlwaysVisible = true;
            ListBox_Validation.Size = new System.Drawing.Size(704, 107);
            ListBox_Validation.TabIndex = 0;
            // 
            // TabPage_ConvertLogs
            // 
            TabPage_ConvertLogs.Controls.Add(ListBox_ImportLog);
            TabPage_ConvertLogs.ImageIndex = 4;
            TabPage_ConvertLogs.Location = new System.Drawing.Point(4, 24);
            TabPage_ConvertLogs.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TabPage_ConvertLogs.Name = "TabPage_ConvertLogs";
            TabPage_ConvertLogs.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TabPage_ConvertLogs.Size = new System.Drawing.Size(712, 113);
            TabPage_ConvertLogs.TabIndex = 1;
            TabPage_ConvertLogs.Text = "Import Log";
            TabPage_ConvertLogs.UseVisualStyleBackColor = true;
            // 
            // ListBox_ImportLog
            // 
            ListBox_ImportLog.Dock = System.Windows.Forms.DockStyle.Fill;
            ListBox_ImportLog.FormattingEnabled = true;
            ListBox_ImportLog.ItemHeight = 15;
            ListBox_ImportLog.Location = new System.Drawing.Point(4, 3);
            ListBox_ImportLog.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ListBox_ImportLog.Name = "ListBox_ImportLog";
            ListBox_ImportLog.ScrollAlwaysVisible = true;
            ListBox_ImportLog.Size = new System.Drawing.Size(704, 107);
            ListBox_ImportLog.TabIndex = 0;
            // 
            // TabControl_Editors
            // 
            TabControl_Editors.Controls.Add(TabPage_Model);
            TabControl_Editors.Controls.Add(TabPage_Material);
            TabControl_Editors.Dock = System.Windows.Forms.DockStyle.Fill;
            TabControl_Editors.ImageList = imageList1;
            TabControl_Editors.Location = new System.Drawing.Point(3, 28);
            TabControl_Editors.Name = "TabControl_Editors";
            TabControl_Editors.SelectedIndex = 0;
            TabControl_Editors.Size = new System.Drawing.Size(722, 331);
            TabControl_Editors.TabIndex = 1;
            TabControl_Editors.SelectedIndexChanged += TabControl_Editors_TabIndexChanged;
            TabControl_Editors.TabIndexChanged += TabControl_Editors_TabIndexChanged;
            // 
            // TabPage_Model
            // 
            TabPage_Model.Controls.Add(Split_ModelPage);
            TabPage_Model.ImageIndex = 3;
            TabPage_Model.Location = new System.Drawing.Point(4, 24);
            TabPage_Model.Name = "TabPage_Model";
            TabPage_Model.Padding = new System.Windows.Forms.Padding(3);
            TabPage_Model.Size = new System.Drawing.Size(714, 303);
            TabPage_Model.TabIndex = 0;
            TabPage_Model.Text = "$TAB_MODEL";
            TabPage_Model.UseVisualStyleBackColor = true;
            // 
            // TabPage_Material
            // 
            TabPage_Material.Controls.Add(Split_MaterialPage);
            TabPage_Material.ImageIndex = 2;
            TabPage_Material.Location = new System.Drawing.Point(4, 24);
            TabPage_Material.Name = "TabPage_Material";
            TabPage_Material.Padding = new System.Windows.Forms.Padding(3);
            TabPage_Material.Size = new System.Drawing.Size(714, 303);
            TabPage_Material.TabIndex = 1;
            TabPage_Material.Text = "$TAB_MATERIAL";
            TabPage_Material.UseVisualStyleBackColor = true;
            // 
            // Split_MaterialPage
            // 
            Split_MaterialPage.Cursor = System.Windows.Forms.Cursors.VSplit;
            Split_MaterialPage.Dock = System.Windows.Forms.DockStyle.Fill;
            Split_MaterialPage.Location = new System.Drawing.Point(3, 3);
            Split_MaterialPage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Split_MaterialPage.Name = "Split_MaterialPage";
            // 
            // Split_MaterialPage.Panel1
            // 
            Split_MaterialPage.Panel1.Controls.Add(ListView_Materials);
            // 
            // Split_MaterialPage.Panel2
            // 
            Split_MaterialPage.Panel2.Controls.Add(Split_MaterialEditor);
            Split_MaterialPage.Size = new System.Drawing.Size(708, 297);
            Split_MaterialPage.SplitterDistance = 236;
            Split_MaterialPage.SplitterWidth = 5;
            Split_MaterialPage.TabIndex = 17;
            // 
            // ListView_Materials
            // 
            ListView_Materials.Dock = System.Windows.Forms.DockStyle.Fill;
            ListView_Materials.FullRowSelect = true;
            ListView_Materials.GridLines = true;
            ListView_Materials.GroupImageList = imageList1;
            ListView_Materials.LargeImageList = imageList1;
            ListView_Materials.Location = new System.Drawing.Point(0, 0);
            ListView_Materials.Name = "ListView_Materials";
            ListView_Materials.Size = new System.Drawing.Size(236, 297);
            ListView_Materials.SmallImageList = imageList1;
            ListView_Materials.TabIndex = 0;
            ListView_Materials.UseCompatibleStateImageBehavior = false;
            ListView_Materials.View = System.Windows.Forms.View.List;
            ListView_Materials.SelectedIndexChanged += ListView_Materials_SelectedIndexChanged;
            // 
            // Split_MaterialEditor
            // 
            Split_MaterialEditor.Cursor = System.Windows.Forms.Cursors.HSplit;
            Split_MaterialEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            Split_MaterialEditor.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            Split_MaterialEditor.IsSplitterFixed = true;
            Split_MaterialEditor.Location = new System.Drawing.Point(0, 0);
            Split_MaterialEditor.Name = "Split_MaterialEditor";
            Split_MaterialEditor.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // Split_MaterialEditor.Panel1
            // 
            Split_MaterialEditor.Panel1.Controls.Add(Panel_Material);
            // 
            // Split_MaterialEditor.Panel2
            // 
            Split_MaterialEditor.Panel2.Controls.Add(PropertyGrid_Material);
            Split_MaterialEditor.Size = new System.Drawing.Size(467, 297);
            Split_MaterialEditor.SplitterDistance = 57;
            Split_MaterialEditor.TabIndex = 1;
            // 
            // PropertyGrid_Material
            // 
            PropertyGrid_Material.Dock = System.Windows.Forms.DockStyle.Fill;
            PropertyGrid_Material.Location = new System.Drawing.Point(0, 0);
            PropertyGrid_Material.Name = "PropertyGrid_Material";
            PropertyGrid_Material.Size = new System.Drawing.Size(467, 236);
            PropertyGrid_Material.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(TabControl_Editors, 0, 1);
            tableLayoutPanel1.Controls.Add(toolStrip1, 0, 0);
            tableLayoutPanel1.Controls.Add(Tab_Root, 0, 2);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 147F));
            tableLayoutPanel1.Size = new System.Drawing.Size(728, 509);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // AnimFileDialog
            // 
            AnimFileDialog.Filter = "Animation2|*an2";
            AnimFileDialog.Multiselect = true;
            // 
            // FrameResourceModelExporter
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new System.Drawing.Size(728, 509);
            Controls.Add(tableLayoutPanel1);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrameResourceModelExporter";
            Text = "$MODEL_OPTIONS_TITLE";
            Split_ModelPage.Panel1.ResumeLayout(false);
            Split_ModelPage.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)Split_ModelPage).EndInit();
            Split_ModelPage.ResumeLayout(false);
            Panel_Material.ResumeLayout(false);
            Panel_Material.PerformLayout();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            Tab_Root.ResumeLayout(false);
            TabPage_Validation.ResumeLayout(false);
            TabPage_ConvertLogs.ResumeLayout(false);
            TabControl_Editors.ResumeLayout(false);
            TabPage_Model.ResumeLayout(false);
            TabPage_Material.ResumeLayout(false);
            Split_MaterialPage.Panel1.ResumeLayout(false);
            Split_MaterialPage.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)Split_MaterialPage).EndInit();
            Split_MaterialPage.ResumeLayout(false);
            Split_MaterialEditor.Panel1.ResumeLayout(false);
            Split_MaterialEditor.Panel1.PerformLayout();
            Split_MaterialEditor.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)Split_MaterialEditor).EndInit();
            Split_MaterialEditor.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
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
        private System.Windows.Forms.OpenFileDialog AnimFileDialog;
        private System.Windows.Forms.ContextMenuStrip HelperContextMenu;
    }
}