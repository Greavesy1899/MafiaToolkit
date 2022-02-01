namespace Forms.EditorControls
{
    partial class FrameResourceModelOptions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrameResourceModelOptions));
            this.ImportAOBox = new System.Windows.Forms.CheckBox();
            this.SplitContainer_Root = new System.Windows.Forms.SplitContainer();
            this.TreeView_Objects = new Mafia2Tool.Controls.MTreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.PropertyGrid_Test = new System.Windows.Forms.PropertyGrid();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
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
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer_Root)).BeginInit();
            this.SplitContainer_Root.Panel1.SuspendLayout();
            this.SplitContainer_Root.Panel2.SuspendLayout();
            this.SplitContainer_Root.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.Tab_Root.SuspendLayout();
            this.TabPage_Validation.SuspendLayout();
            this.TabPage_ConvertLogs.SuspendLayout();
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
            // SplitContainer_Root
            // 
            this.SplitContainer_Root.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainer_Root.Location = new System.Drawing.Point(4, 32);
            this.SplitContainer_Root.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.SplitContainer_Root.Name = "SplitContainer_Root";
            // 
            // SplitContainer_Root.Panel1
            // 
            this.SplitContainer_Root.Panel1.Controls.Add(this.TreeView_Objects);
            // 
            // SplitContainer_Root.Panel2
            // 
            this.SplitContainer_Root.Panel2.Controls.Add(this.PropertyGrid_Test);
            this.SplitContainer_Root.Size = new System.Drawing.Size(720, 302);
            this.SplitContainer_Root.SplitterDistance = 240;
            this.SplitContainer_Root.SplitterWidth = 5;
            this.SplitContainer_Root.TabIndex = 16;
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
            this.TreeView_Objects.Size = new System.Drawing.Size(240, 302);
            this.TreeView_Objects.TabIndex = 0;
            this.TreeView_Objects.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.TreeView_OnBeforeSelect);
            this.TreeView_Objects.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeView_OnAfterSelect);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "cross.png");
            this.imageList1.Images.SetKeyName(1, "tick.png");
            // 
            // PropertyGrid_Test
            // 
            this.PropertyGrid_Test.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PropertyGrid_Test.Location = new System.Drawing.Point(0, 0);
            this.PropertyGrid_Test.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.PropertyGrid_Test.Name = "PropertyGrid_Test";
            this.PropertyGrid_Test.Size = new System.Drawing.Size(475, 302);
            this.PropertyGrid_Test.TabIndex = 16;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.SplitContainer_Root, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.toolStrip1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.Tab_Root, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.602151F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 91.39785F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 171F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(728, 509);
            this.tableLayoutPanel1.TabIndex = 19;
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
            this.Button_Validate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Button_Validate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_Validate.Name = "Button_Validate";
            this.Button_Validate.Size = new System.Drawing.Size(52, 22);
            this.Button_Validate.Text = "Validate";
            this.Button_Validate.Click += new System.EventHandler(this.Button_Validate_Click);
            // 
            // Button_Continue
            // 
            this.Button_Continue.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Button_Continue.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_Continue.Name = "Button_Continue";
            this.Button_Continue.Size = new System.Drawing.Size(60, 22);
            this.Button_Continue.Text = "Continue";
            this.Button_Continue.Click += new System.EventHandler(this.Button_Continue_Click);
            // 
            // Button_StopImport
            // 
            this.Button_StopImport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Button_StopImport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_StopImport.Name = "Button_StopImport";
            this.Button_StopImport.Size = new System.Drawing.Size(74, 22);
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
            this.Tab_Root.Location = new System.Drawing.Point(4, 340);
            this.Tab_Root.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Tab_Root.Name = "Tab_Root";
            this.Tab_Root.SelectedIndex = 0;
            this.Tab_Root.Size = new System.Drawing.Size(720, 166);
            this.Tab_Root.TabIndex = 19;
            // 
            // TabPage_Validation
            // 
            this.TabPage_Validation.Controls.Add(this.ListBox_Validation);
            this.TabPage_Validation.Location = new System.Drawing.Point(4, 24);
            this.TabPage_Validation.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TabPage_Validation.Name = "TabPage_Validation";
            this.TabPage_Validation.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TabPage_Validation.Size = new System.Drawing.Size(712, 138);
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
            this.ListBox_Validation.Size = new System.Drawing.Size(704, 132);
            this.ListBox_Validation.TabIndex = 0;
            // 
            // TabPage_ConvertLogs
            // 
            this.TabPage_ConvertLogs.Controls.Add(this.ListBox_ImportLog);
            this.TabPage_ConvertLogs.Location = new System.Drawing.Point(4, 24);
            this.TabPage_ConvertLogs.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TabPage_ConvertLogs.Name = "TabPage_ConvertLogs";
            this.TabPage_ConvertLogs.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TabPage_ConvertLogs.Size = new System.Drawing.Size(712, 138);
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
            this.ListBox_ImportLog.Size = new System.Drawing.Size(704, 132);
            this.ListBox_ImportLog.TabIndex = 0;
            // 
            // FrameResourceModelOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(728, 509);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrameResourceModelOptions";
            this.Text = "$MODEL_OPTIONS_TITLE";
            this.SplitContainer_Root.Panel1.ResumeLayout(false);
            this.SplitContainer_Root.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer_Root)).EndInit();
            this.SplitContainer_Root.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.Tab_Root.ResumeLayout(false);
            this.TabPage_Validation.ResumeLayout(false);
            this.TabPage_ConvertLogs.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.CheckBox ImportAOBox;
        private System.Windows.Forms.SplitContainer SplitContainer_Root;
        private Mafia2Tool.Controls.MTreeView TreeView_Objects;
        private System.Windows.Forms.PropertyGrid PropertyGrid_Test;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
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
    }
}