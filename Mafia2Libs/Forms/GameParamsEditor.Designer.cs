namespace Mafia2Tool
{
    partial class GameParamsEditor
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            PropertyGrid_Main = new System.Windows.Forms.PropertyGrid();
            TreeView_Main = new Controls.MTreeView();
            ToolStrip_Main = new System.Windows.Forms.ToolStrip();
            Button_File = new System.Windows.Forms.ToolStripDropDownButton();
            Button_Save = new System.Windows.Forms.ToolStripMenuItem();
            Button_Reload = new System.Windows.Forms.ToolStripMenuItem();
            Button_Exit = new System.Windows.Forms.ToolStripMenuItem();
            Button_Tools = new System.Windows.Forms.ToolStripDropDownButton();
            Button_ExportXml = new System.Windows.Forms.ToolStripMenuItem();
            Button_ImportXml = new System.Windows.Forms.ToolStripMenuItem();
            SplitContainer_Main = new System.Windows.Forms.SplitContainer();
            ToolStrip_Main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)SplitContainer_Main).BeginInit();
            SplitContainer_Main.Panel1.SuspendLayout();
            SplitContainer_Main.Panel2.SuspendLayout();
            SplitContainer_Main.SuspendLayout();
            SuspendLayout();
            //
            // PropertyGrid_Main
            //
            PropertyGrid_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            PropertyGrid_Main.Location = new System.Drawing.Point(0, 0);
            PropertyGrid_Main.Name = "PropertyGrid_Main";
            PropertyGrid_Main.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            PropertyGrid_Main.Size = new System.Drawing.Size(450, 469);
            PropertyGrid_Main.TabIndex = 10;
            PropertyGrid_Main.PropertyValueChanged += PropertyGrid_PropertyChanged;
            //
            // TreeView_Main
            //
            TreeView_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            TreeView_Main.Location = new System.Drawing.Point(0, 0);
            TreeView_Main.Name = "TreeView_Main";
            TreeView_Main.Size = new System.Drawing.Size(475, 469);
            TreeView_Main.TabIndex = 11;
            TreeView_Main.HideSelection = false;
            TreeView_Main.AfterSelect += OnNodeSelect;
            //
            // ToolStrip_Main
            //
            ToolStrip_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { Button_File, Button_Tools });
            ToolStrip_Main.Location = new System.Drawing.Point(0, 0);
            ToolStrip_Main.Name = "ToolStrip_Main";
            ToolStrip_Main.Size = new System.Drawing.Size(933, 25);
            ToolStrip_Main.TabIndex = 15;
            //
            // Button_File
            //
            Button_File.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            Button_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { Button_Save, Button_Reload, Button_Exit });
            Button_File.Name = "Button_File";
            Button_File.Size = new System.Drawing.Size(47, 22);
            Button_File.Text = "$FILE";
            //
            // Button_Save
            //
            Button_Save.Name = "Button_Save";
            Button_Save.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S;
            Button_Save.Size = new System.Drawing.Size(165, 22);
            Button_Save.Text = "$SAVE";
            Button_Save.Click += Button_Save_OnClick;
            //
            // Button_Reload
            //
            Button_Reload.Name = "Button_Reload";
            Button_Reload.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R;
            Button_Reload.Size = new System.Drawing.Size(165, 22);
            Button_Reload.Text = "$RELOAD";
            Button_Reload.Click += Button_Reload_OnClick;
            //
            // Button_Exit
            //
            Button_Exit.Name = "Button_Exit";
            Button_Exit.Size = new System.Drawing.Size(165, 22);
            Button_Exit.Text = "$EXIT";
            Button_Exit.Click += Button_Exit_OnClick;
            //
            // Button_Tools
            //
            Button_Tools.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            Button_Tools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { Button_ExportXml, Button_ImportXml });
            Button_Tools.Name = "Button_Tools";
            Button_Tools.Size = new System.Drawing.Size(61, 22);
            Button_Tools.Text = "$TOOLS";
            //
            // Button_ExportXml
            //
            Button_ExportXml.Name = "Button_ExportXml";
            Button_ExportXml.Size = new System.Drawing.Size(180, 22);
            Button_ExportXml.Text = "$EXPORT_XML";
            Button_ExportXml.Click += Button_ExportXml_OnClick;
            //
            // Button_ImportXml
            //
            Button_ImportXml.Name = "Button_ImportXml";
            Button_ImportXml.Size = new System.Drawing.Size(180, 22);
            Button_ImportXml.Text = "$IMPORT_XML";
            Button_ImportXml.Click += Button_ImportXml_OnClick;
            //
            // SplitContainer_Main
            //
            SplitContainer_Main.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            SplitContainer_Main.Location = new System.Drawing.Point(4, 28);
            SplitContainer_Main.Name = "SplitContainer_Main";
            SplitContainer_Main.Panel1.Controls.Add(TreeView_Main);
            SplitContainer_Main.Panel2.Controls.Add(PropertyGrid_Main);
            SplitContainer_Main.Size = new System.Drawing.Size(929, 469);
            SplitContainer_Main.SplitterDistance = 475;
            SplitContainer_Main.TabIndex = 17;
            //
            // GameParamsEditor
            //
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(933, 500);
            Controls.Add(SplitContainer_Main);
            Controls.Add(ToolStrip_Main);
            Name = "GameParamsEditor";
            Text = "$GAMEPARAMS_EDITOR_TITLE";
            FormClosing += GameParamsEditor_Closing;
            ToolStrip_Main.ResumeLayout(false);
            ToolStrip_Main.PerformLayout();
            SplitContainer_Main.Panel1.ResumeLayout(false);
            SplitContainer_Main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)SplitContainer_Main).EndInit();
            SplitContainer_Main.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.PropertyGrid PropertyGrid_Main;
        private Controls.MTreeView TreeView_Main;
        private System.Windows.Forms.ToolStrip ToolStrip_Main;
        private System.Windows.Forms.ToolStripDropDownButton Button_File;
        private System.Windows.Forms.ToolStripMenuItem Button_Save;
        private System.Windows.Forms.ToolStripMenuItem Button_Reload;
        private System.Windows.Forms.ToolStripMenuItem Button_Exit;
        private System.Windows.Forms.ToolStripDropDownButton Button_Tools;
        private System.Windows.Forms.ToolStripMenuItem Button_ExportXml;
        private System.Windows.Forms.ToolStripMenuItem Button_ImportXml;
        private System.Windows.Forms.SplitContainer SplitContainer_Main;
    }
}
