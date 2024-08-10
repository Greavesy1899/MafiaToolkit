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
            HelperContextMenu = new System.Windows.Forms.ContextMenuStrip(components);
            imageList1 = new System.Windows.Forms.ImageList(components);
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
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            AnimFileDialog = new System.Windows.Forms.OpenFileDialog();
            TabPage_Model = new System.Windows.Forms.TabPage();
            Split_ModelPage = new System.Windows.Forms.SplitContainer();
            PropertyGrid_Model = new System.Windows.Forms.PropertyGrid();
            TreeView_Objects = new Mafia2Tool.Controls.MTreeView();
            TabControl_Editors = new System.Windows.Forms.TabControl();
            toolStrip1.SuspendLayout();
            Tab_Root.SuspendLayout();
            TabPage_Validation.SuspendLayout();
            TabPage_ConvertLogs.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            TabPage_Model.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Split_ModelPage).BeginInit();
            Split_ModelPage.Panel1.SuspendLayout();
            Split_ModelPage.Panel2.SuspendLayout();
            Split_ModelPage.SuspendLayout();
            TabControl_Editors.SuspendLayout();
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
            // HelperContextMenu
            // 
            HelperContextMenu.Name = "EntryMenuStrip";
            HelperContextMenu.Size = new System.Drawing.Size(61, 4);
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
            // TabControl_Editors
            // 
            TabControl_Editors.Controls.Add(TabPage_Model);
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
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            Tab_Root.ResumeLayout(false);
            TabPage_Validation.ResumeLayout(false);
            TabPage_ConvertLogs.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            TabPage_Model.ResumeLayout(false);
            Split_ModelPage.Panel1.ResumeLayout(false);
            Split_ModelPage.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)Split_ModelPage).EndInit();
            Split_ModelPage.ResumeLayout(false);
            TabControl_Editors.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.CheckBox ImportAOBox;
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
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.OpenFileDialog AnimFileDialog;
        private System.Windows.Forms.ContextMenuStrip HelperContextMenu;
        private System.Windows.Forms.TabControl TabControl_Editors;
        private System.Windows.Forms.TabPage TabPage_Model;
        private System.Windows.Forms.SplitContainer Split_ModelPage;
        private Mafia2Tool.Controls.MTreeView TreeView_Objects;
        private System.Windows.Forms.PropertyGrid PropertyGrid_Model;
    }
}