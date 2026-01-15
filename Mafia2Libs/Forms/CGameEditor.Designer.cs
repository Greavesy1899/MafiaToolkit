namespace Mafia2Tool
{
    partial class CGameEditor
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
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            Button_ExpandAll = new System.Windows.Forms.ToolStripMenuItem();
            Button_CollapseAll = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            Button_AddSlot = new System.Windows.Forms.ToolStripMenuItem();
            Button_DeleteSlot = new System.Windows.Forms.ToolStripMenuItem();
            ToolStrip_Main.SuspendLayout();
            SuspendLayout();
            //
            // PropertyGrid_Main
            //
            PropertyGrid_Main.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            PropertyGrid_Main.Location = new System.Drawing.Point(469, 32);
            PropertyGrid_Main.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PropertyGrid_Main.Name = "PropertyGrid_Main";
            PropertyGrid_Main.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            PropertyGrid_Main.Size = new System.Drawing.Size(450, 473);
            PropertyGrid_Main.TabIndex = 10;
            PropertyGrid_Main.PropertyValueChanged += PropertyGrid_PropertyChanged;
            //
            // TreeView_Main
            //
            TreeView_Main.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            TreeView_Main.Location = new System.Drawing.Point(14, 32);
            TreeView_Main.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TreeView_Main.Name = "TreeView_Main";
            TreeView_Main.Size = new System.Drawing.Size(449, 472);
            TreeView_Main.TabIndex = 11;
            TreeView_Main.AfterSelect += OnNodeSelectSelect;
            //
            // ToolStrip_Main
            //
            ToolStrip_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { Button_File, Button_Tools });
            ToolStrip_Main.Location = new System.Drawing.Point(0, 0);
            ToolStrip_Main.Name = "ToolStrip_Main";
            ToolStrip_Main.Size = new System.Drawing.Size(933, 25);
            ToolStrip_Main.TabIndex = 15;
            ToolStrip_Main.Text = "toolStrip1";
            //
            // Button_File
            //
            Button_File.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            Button_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { Button_Save, Button_Reload, Button_Exit });
            Button_File.ImageTransparentColor = System.Drawing.Color.Magenta;
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
            Button_Tools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { Button_ExportXml, Button_ImportXml, toolStripSeparator1, Button_ExpandAll, Button_CollapseAll, toolStripSeparator2, Button_AddSlot, Button_DeleteSlot });
            Button_Tools.ImageTransparentColor = System.Drawing.Color.Magenta;
            Button_Tools.Name = "Button_Tools";
            Button_Tools.Size = new System.Drawing.Size(61, 22);
            Button_Tools.Text = "$TOOLS";
            //
            // Button_ExportXml
            //
            Button_ExportXml.Name = "Button_ExportXml";
            Button_ExportXml.Size = new System.Drawing.Size(200, 22);
            Button_ExportXml.Text = "$EXPORT_XML";
            Button_ExportXml.Click += Button_ExportXml_OnClick;
            //
            // Button_ImportXml
            //
            Button_ImportXml.Name = "Button_ImportXml";
            Button_ImportXml.Size = new System.Drawing.Size(200, 22);
            Button_ImportXml.Text = "$IMPORT_XML";
            Button_ImportXml.Click += Button_ImportXml_OnClick;
            //
            // toolStripSeparator1
            //
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(197, 6);
            //
            // Button_ExpandAll
            //
            Button_ExpandAll.Name = "Button_ExpandAll";
            Button_ExpandAll.Size = new System.Drawing.Size(200, 22);
            Button_ExpandAll.Text = "$EXPAND_ALL";
            Button_ExpandAll.Click += Button_ExpandAll_OnClick;
            //
            // Button_CollapseAll
            //
            Button_CollapseAll.Name = "Button_CollapseAll";
            Button_CollapseAll.Size = new System.Drawing.Size(200, 22);
            Button_CollapseAll.Text = "$COLLAPSE_ALL";
            Button_CollapseAll.Click += Button_CollapseAll_OnClick;
            //
            // toolStripSeparator2
            //
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(197, 6);
            //
            // Button_AddSlot
            //
            Button_AddSlot.Name = "Button_AddSlot";
            Button_AddSlot.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N;
            Button_AddSlot.Size = new System.Drawing.Size(200, 22);
            Button_AddSlot.Text = "$CGAME_ADD_SLOT";
            Button_AddSlot.Click += Button_AddSlot_OnClick;
            //
            // Button_DeleteSlot
            //
            Button_DeleteSlot.Name = "Button_DeleteSlot";
            Button_DeleteSlot.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            Button_DeleteSlot.Size = new System.Drawing.Size(200, 22);
            Button_DeleteSlot.Text = "$CGAME_DELETE_SLOT";
            Button_DeleteSlot.Enabled = false;
            Button_DeleteSlot.Click += Button_DeleteSlot_OnClick;
            //
            // CGameEditor
            //
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(933, 519);
            Controls.Add(ToolStrip_Main);
            Controls.Add(PropertyGrid_Main);
            Controls.Add(TreeView_Main);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "CGameEditor";
            Text = "$CGAME_EDITOR_TITLE";
            FormClosing += CGameEditor_Closing;
            ToolStrip_Main.ResumeLayout(false);
            ToolStrip_Main.PerformLayout();
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
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem Button_ExpandAll;
        private System.Windows.Forms.ToolStripMenuItem Button_CollapseAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem Button_AddSlot;
        private System.Windows.Forms.ToolStripMenuItem Button_DeleteSlot;
    }
}
