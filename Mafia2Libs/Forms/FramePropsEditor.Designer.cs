namespace Mafia2Tool
{
    partial class FramePropsEditor
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
            Button_AddEntry = new System.Windows.Forms.ToolStripMenuItem();
            Button_DeleteEntry = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            Button_AddProperty = new System.Windows.Forms.ToolStripMenuItem();
            Button_DeleteProperty = new System.Windows.Forms.ToolStripMenuItem();
            StatusStrip_Main = new System.Windows.Forms.StatusStrip();
            StatusLabel_EntryCount = new System.Windows.Forms.ToolStripStatusLabel();
            StatusLabel_PropertyCount = new System.Windows.Forms.ToolStripStatusLabel();
            StatusLabel_Selection = new System.Windows.Forms.ToolStripStatusLabel();
            SplitContainer_Main = new System.Windows.Forms.SplitContainer();
            ContextMenu_Tree = new System.Windows.Forms.ContextMenuStrip(components);
            Context_AddEntry = new System.Windows.Forms.ToolStripMenuItem();
            Context_DeleteEntry = new System.Windows.Forms.ToolStripMenuItem();
            Context_Separator1 = new System.Windows.Forms.ToolStripSeparator();
            Context_AddProperty = new System.Windows.Forms.ToolStripMenuItem();
            Context_DeleteProperty = new System.Windows.Forms.ToolStripMenuItem();
            Context_Separator2 = new System.Windows.Forms.ToolStripSeparator();
            Context_AddValue = new System.Windows.Forms.ToolStripMenuItem();
            Context_DeleteValue = new System.Windows.Forms.ToolStripMenuItem();
            Context_Separator3 = new System.Windows.Forms.ToolStripSeparator();
            Context_CopyHash = new System.Windows.Forms.ToolStripMenuItem();
            ToolStrip_Main.SuspendLayout();
            StatusStrip_Main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)SplitContainer_Main).BeginInit();
            SplitContainer_Main.Panel1.SuspendLayout();
            SplitContainer_Main.Panel2.SuspendLayout();
            SplitContainer_Main.SuspendLayout();
            ContextMenu_Tree.SuspendLayout();
            SuspendLayout();
            //
            // PropertyGrid_Main
            //
            PropertyGrid_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            PropertyGrid_Main.Location = new System.Drawing.Point(0, 0);
            PropertyGrid_Main.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
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
            TreeView_Main.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TreeView_Main.Name = "TreeView_Main";
            TreeView_Main.Size = new System.Drawing.Size(475, 469);
            TreeView_Main.TabIndex = 11;
            TreeView_Main.HideSelection = false;
            TreeView_Main.ContextMenuStrip = ContextMenu_Tree;
            TreeView_Main.AfterSelect += OnNodeSelectSelect;
            TreeView_Main.NodeMouseDoubleClick += OnNodeDoubleClick;
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
            Button_Tools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { Button_ExportXml, Button_ImportXml, toolStripSeparator1, Button_ExpandAll, Button_CollapseAll, toolStripSeparator2, Button_AddEntry, Button_DeleteEntry, toolStripSeparator3, Button_AddProperty, Button_DeleteProperty });
            Button_Tools.ImageTransparentColor = System.Drawing.Color.Magenta;
            Button_Tools.Name = "Button_Tools";
            Button_Tools.Size = new System.Drawing.Size(61, 22);
            Button_Tools.Text = "$TOOLS";
            //
            // Button_ExportXml
            //
            Button_ExportXml.Name = "Button_ExportXml";
            Button_ExportXml.Size = new System.Drawing.Size(250, 22);
            Button_ExportXml.Text = "$EXPORT_XML";
            Button_ExportXml.Click += Button_ExportXml_OnClick;
            //
            // Button_ImportXml
            //
            Button_ImportXml.Name = "Button_ImportXml";
            Button_ImportXml.Size = new System.Drawing.Size(250, 22);
            Button_ImportXml.Text = "$IMPORT_XML";
            Button_ImportXml.Click += Button_ImportXml_OnClick;
            //
            // toolStripSeparator1
            //
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(247, 6);
            //
            // Button_ExpandAll
            //
            Button_ExpandAll.Name = "Button_ExpandAll";
            Button_ExpandAll.Size = new System.Drawing.Size(250, 22);
            Button_ExpandAll.Text = "$EXPAND_ALL";
            Button_ExpandAll.Click += Button_ExpandAll_OnClick;
            //
            // Button_CollapseAll
            //
            Button_CollapseAll.Name = "Button_CollapseAll";
            Button_CollapseAll.Size = new System.Drawing.Size(250, 22);
            Button_CollapseAll.Text = "$COLLAPSE_ALL";
            Button_CollapseAll.Click += Button_CollapseAll_OnClick;
            //
            // toolStripSeparator2
            //
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(247, 6);
            //
            // Button_AddEntry
            //
            Button_AddEntry.Name = "Button_AddEntry";
            Button_AddEntry.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N;
            Button_AddEntry.Size = new System.Drawing.Size(250, 22);
            Button_AddEntry.Text = "$FRAMEPROPS_ADD_ENTRY";
            Button_AddEntry.Click += Button_AddEntry_OnClick;
            //
            // Button_DeleteEntry
            //
            Button_DeleteEntry.Name = "Button_DeleteEntry";
            Button_DeleteEntry.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            Button_DeleteEntry.Size = new System.Drawing.Size(250, 22);
            Button_DeleteEntry.Text = "$FRAMEPROPS_DELETE_ENTRY";
            Button_DeleteEntry.Enabled = false;
            Button_DeleteEntry.Click += Button_DeleteEntry_OnClick;
            //
            // toolStripSeparator3
            //
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(247, 6);
            //
            // Button_AddProperty
            //
            Button_AddProperty.Name = "Button_AddProperty";
            Button_AddProperty.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P;
            Button_AddProperty.Size = new System.Drawing.Size(250, 22);
            Button_AddProperty.Text = "$FRAMEPROPS_ADD_PROPERTY";
            Button_AddProperty.Enabled = false;
            Button_AddProperty.Click += Button_AddProperty_OnClick;
            //
            // Button_DeleteProperty
            //
            Button_DeleteProperty.Name = "Button_DeleteProperty";
            Button_DeleteProperty.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete;
            Button_DeleteProperty.Size = new System.Drawing.Size(250, 22);
            Button_DeleteProperty.Text = "$FRAMEPROPS_DELETE_PROPERTY";
            Button_DeleteProperty.Enabled = false;
            Button_DeleteProperty.Click += Button_DeleteProperty_OnClick;
            //
            // StatusStrip_Main
            //
            StatusStrip_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { StatusLabel_EntryCount, StatusLabel_PropertyCount, StatusLabel_Selection });
            StatusStrip_Main.Location = new System.Drawing.Point(0, 497);
            StatusStrip_Main.Name = "StatusStrip_Main";
            StatusStrip_Main.Size = new System.Drawing.Size(933, 22);
            StatusStrip_Main.TabIndex = 16;
            //
            // StatusLabel_EntryCount
            //
            StatusLabel_EntryCount.Name = "StatusLabel_EntryCount";
            StatusLabel_EntryCount.Size = new System.Drawing.Size(55, 17);
            StatusLabel_EntryCount.Text = "Entries: 0";
            //
            // StatusLabel_PropertyCount
            //
            StatusLabel_PropertyCount.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            StatusLabel_PropertyCount.Name = "StatusLabel_PropertyCount";
            StatusLabel_PropertyCount.Size = new System.Drawing.Size(77, 17);
            StatusLabel_PropertyCount.Text = "Properties: 0";
            //
            // StatusLabel_Selection
            //
            StatusLabel_Selection.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            StatusLabel_Selection.Name = "StatusLabel_Selection";
            StatusLabel_Selection.Size = new System.Drawing.Size(786, 17);
            StatusLabel_Selection.Spring = true;
            StatusLabel_Selection.Text = "";
            StatusLabel_Selection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // SplitContainer_Main
            //
            SplitContainer_Main.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            SplitContainer_Main.Location = new System.Drawing.Point(4, 28);
            SplitContainer_Main.Name = "SplitContainer_Main";
            //
            // SplitContainer_Main.Panel1
            //
            SplitContainer_Main.Panel1.Controls.Add(TreeView_Main);
            //
            // SplitContainer_Main.Panel2
            //
            SplitContainer_Main.Panel2.Controls.Add(PropertyGrid_Main);
            SplitContainer_Main.Size = new System.Drawing.Size(929, 469);
            SplitContainer_Main.SplitterDistance = 475;
            SplitContainer_Main.TabIndex = 17;
            //
            // ContextMenu_Tree
            //
            ContextMenu_Tree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { Context_AddEntry, Context_DeleteEntry, Context_Separator1, Context_AddProperty, Context_DeleteProperty, Context_Separator2, Context_AddValue, Context_DeleteValue, Context_Separator3, Context_CopyHash });
            ContextMenu_Tree.Name = "ContextMenu_Tree";
            ContextMenu_Tree.Size = new System.Drawing.Size(200, 176);
            ContextMenu_Tree.Opening += ContextMenu_Opening;
            //
            // Context_AddEntry
            //
            Context_AddEntry.Name = "Context_AddEntry";
            Context_AddEntry.Size = new System.Drawing.Size(199, 22);
            Context_AddEntry.Text = "Add Entry";
            Context_AddEntry.Click += Button_AddEntry_OnClick;
            //
            // Context_DeleteEntry
            //
            Context_DeleteEntry.Name = "Context_DeleteEntry";
            Context_DeleteEntry.Size = new System.Drawing.Size(199, 22);
            Context_DeleteEntry.Text = "Delete Entry";
            Context_DeleteEntry.Click += Button_DeleteEntry_OnClick;
            //
            // Context_Separator1
            //
            Context_Separator1.Name = "Context_Separator1";
            Context_Separator1.Size = new System.Drawing.Size(196, 6);
            //
            // Context_AddProperty
            //
            Context_AddProperty.Name = "Context_AddProperty";
            Context_AddProperty.Size = new System.Drawing.Size(199, 22);
            Context_AddProperty.Text = "Add Property";
            Context_AddProperty.Click += Button_AddProperty_OnClick;
            //
            // Context_DeleteProperty
            //
            Context_DeleteProperty.Name = "Context_DeleteProperty";
            Context_DeleteProperty.Size = new System.Drawing.Size(199, 22);
            Context_DeleteProperty.Text = "Delete Property";
            Context_DeleteProperty.Click += Button_DeleteProperty_OnClick;
            //
            // Context_Separator2
            //
            Context_Separator2.Name = "Context_Separator2";
            Context_Separator2.Size = new System.Drawing.Size(196, 6);
            //
            // Context_AddValue
            //
            Context_AddValue.Name = "Context_AddValue";
            Context_AddValue.Size = new System.Drawing.Size(199, 22);
            Context_AddValue.Text = "Add Value";
            Context_AddValue.Click += Context_AddValue_OnClick;
            //
            // Context_DeleteValue
            //
            Context_DeleteValue.Name = "Context_DeleteValue";
            Context_DeleteValue.Size = new System.Drawing.Size(199, 22);
            Context_DeleteValue.Text = "Delete Value";
            Context_DeleteValue.Click += Context_DeleteValue_OnClick;
            //
            // Context_Separator3
            //
            Context_Separator3.Name = "Context_Separator3";
            Context_Separator3.Size = new System.Drawing.Size(196, 6);
            //
            // Context_CopyHash
            //
            Context_CopyHash.Name = "Context_CopyHash";
            Context_CopyHash.Size = new System.Drawing.Size(199, 22);
            Context_CopyHash.Text = "Copy Hash to Clipboard";
            Context_CopyHash.Click += Context_CopyHash_OnClick;
            //
            // FramePropsEditor
            //
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(933, 519);
            Controls.Add(SplitContainer_Main);
            Controls.Add(StatusStrip_Main);
            Controls.Add(ToolStrip_Main);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FramePropsEditor";
            Text = "$FRAMEPROPS_EDITOR_TITLE";
            FormClosing += FramePropsEditor_Closing;
            ToolStrip_Main.ResumeLayout(false);
            ToolStrip_Main.PerformLayout();
            StatusStrip_Main.ResumeLayout(false);
            StatusStrip_Main.PerformLayout();
            SplitContainer_Main.Panel1.ResumeLayout(false);
            SplitContainer_Main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)SplitContainer_Main).EndInit();
            SplitContainer_Main.ResumeLayout(false);
            ContextMenu_Tree.ResumeLayout(false);
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
        private System.Windows.Forms.ToolStripMenuItem Button_AddEntry;
        private System.Windows.Forms.ToolStripMenuItem Button_DeleteEntry;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem Button_AddProperty;
        private System.Windows.Forms.ToolStripMenuItem Button_DeleteProperty;
        private System.Windows.Forms.StatusStrip StatusStrip_Main;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel_EntryCount;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel_PropertyCount;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel_Selection;
        private System.Windows.Forms.SplitContainer SplitContainer_Main;
        private System.Windows.Forms.ContextMenuStrip ContextMenu_Tree;
        private System.Windows.Forms.ToolStripMenuItem Context_AddEntry;
        private System.Windows.Forms.ToolStripMenuItem Context_DeleteEntry;
        private System.Windows.Forms.ToolStripSeparator Context_Separator1;
        private System.Windows.Forms.ToolStripMenuItem Context_AddProperty;
        private System.Windows.Forms.ToolStripMenuItem Context_DeleteProperty;
        private System.Windows.Forms.ToolStripSeparator Context_Separator2;
        private System.Windows.Forms.ToolStripMenuItem Context_AddValue;
        private System.Windows.Forms.ToolStripMenuItem Context_DeleteValue;
        private System.Windows.Forms.ToolStripSeparator Context_Separator3;
        private System.Windows.Forms.ToolStripMenuItem Context_CopyHash;
    }
}
