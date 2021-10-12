namespace Toolkit.Forms
{
    partial class EntityDataStorageEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EntityDataStorageEditor));
            this.PropertyGrid_Item = new System.Windows.Forms.PropertyGrid();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.Button_File = new System.Windows.Forms.ToolStripDropDownButton();
            this.Button_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Reload = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Tools = new System.Windows.Forms.ToolStripDropDownButton();
            this.Button_CopyData = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_PasteData = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_ExportXML = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_ImportXML = new System.Windows.Forms.ToolStripMenuItem();
            this.Context_Menu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStrip_Copy = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStrip_Paste = new System.Windows.Forms.ToolStripMenuItem();
            this.TreeView_Tables = new MafiaToolkit.Controls.MTreeView();
            this.FileDialog_Open = new System.Windows.Forms.OpenFileDialog();
            this.FileDialog_Save = new System.Windows.Forms.SaveFileDialog();
            this.toolStrip1.SuspendLayout();
            this.Context_Menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // PropertyGrid_Item
            // 
            this.PropertyGrid_Item.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PropertyGrid_Item.Location = new System.Drawing.Point(469, 32);
            this.PropertyGrid_Item.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.PropertyGrid_Item.Name = "PropertyGrid_Item";
            this.PropertyGrid_Item.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.PropertyGrid_Item.Size = new System.Drawing.Size(450, 473);
            this.PropertyGrid_Item.TabIndex = 10;
            this.PropertyGrid_Item.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.PropertyGrid_OnValueChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Button_File,
            this.Button_Tools});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(933, 25);
            this.toolStrip1.TabIndex = 15;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // Button_File
            // 
            this.Button_File.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Button_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Button_Save,
            this.Button_Reload,
            this.Button_Exit});
            this.Button_File.Image = ((System.Drawing.Image)(resources.GetObject("Button_File.Image")));
            this.Button_File.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_File.Name = "Button_File";
            this.Button_File.Size = new System.Drawing.Size(47, 22);
            this.Button_File.Text = "$FILE";
            // 
            // Button_Save
            // 
            this.Button_Save.Name = "Button_Save";
            this.Button_Save.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.Button_Save.Size = new System.Drawing.Size(165, 22);
            this.Button_Save.Text = "$SAVE";
            this.Button_Save.Click += new System.EventHandler(this.Button_Save_OnClick);
            // 
            // Button_Reload
            // 
            this.Button_Reload.Name = "Button_Reload";
            this.Button_Reload.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.Button_Reload.Size = new System.Drawing.Size(165, 22);
            this.Button_Reload.Text = "$RELOAD";
            this.Button_Reload.Click += new System.EventHandler(this.Button_Reload_OnClick);
            // 
            // Button_Exit
            // 
            this.Button_Exit.Name = "Button_Exit";
            this.Button_Exit.Size = new System.Drawing.Size(165, 22);
            this.Button_Exit.Text = "$EXIT";
            this.Button_Exit.Click += new System.EventHandler(this.Button_Exit_OnClick);
            // 
            // Button_Tools
            // 
            this.Button_Tools.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Button_Tools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Button_CopyData,
            this.Button_PasteData,
            this.Button_ExportXML,
            this.Button_ImportXML});
            this.Button_Tools.Image = ((System.Drawing.Image)(resources.GetObject("Button_Tools.Image")));
            this.Button_Tools.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_Tools.Name = "Button_Tools";
            this.Button_Tools.Size = new System.Drawing.Size(61, 22);
            this.Button_Tools.Text = "$TOOLS";
            // 
            // Button_CopyData
            // 
            this.Button_CopyData.Name = "Button_CopyData";
            this.Button_CopyData.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.Button_CopyData.Size = new System.Drawing.Size(153, 22);
            this.Button_CopyData.Text = "$COPY";
            this.Button_CopyData.Click += new System.EventHandler(this.Button_CopyData_Click);
            // 
            // Button_PasteData
            // 
            this.Button_PasteData.Name = "Button_PasteData";
            this.Button_PasteData.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.Button_PasteData.Size = new System.Drawing.Size(153, 22);
            this.Button_PasteData.Text = "$PASTE";
            this.Button_PasteData.Click += new System.EventHandler(this.Button_Paste_Click);
            // 
            // Button_ExportXML
            // 
            this.Button_ExportXML.Name = "Button_ExportXML";
            this.Button_ExportXML.Size = new System.Drawing.Size(153, 22);
            this.Button_ExportXML.Text = "$EXPORT_XML";
            this.Button_ExportXML.Click += new System.EventHandler(this.Button_ExportXML_Click);
            // 
            // Button_ImportXML
            // 
            this.Button_ImportXML.Name = "Button_ImportXML";
            this.Button_ImportXML.Size = new System.Drawing.Size(153, 22);
            this.Button_ImportXML.Text = "$IMPORT_XML";
            this.Button_ImportXML.Click += new System.EventHandler(this.Button_ImportXML_Click);
            // 
            // Context_Menu
            // 
            this.Context_Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStrip_Copy,
            this.ToolStrip_Paste});
            this.Context_Menu.Name = "Context_Menu";
            this.Context_Menu.Size = new System.Drawing.Size(154, 48);
            this.Context_Menu.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenu_Opening);
            // 
            // ToolStrip_Copy
            // 
            this.ToolStrip_Copy.Name = "ToolStrip_Copy";
            this.ToolStrip_Copy.ShortcutKeyDisplayString = "";
            this.ToolStrip_Copy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.ToolStrip_Copy.Size = new System.Drawing.Size(153, 22);
            this.ToolStrip_Copy.Text = "$COPY";
            this.ToolStrip_Copy.Click += new System.EventHandler(this.ToolStrip_Copy_Click);
            // 
            // ToolStrip_Paste
            // 
            this.ToolStrip_Paste.Name = "ToolStrip_Paste";
            this.ToolStrip_Paste.ShortcutKeyDisplayString = "";
            this.ToolStrip_Paste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.ToolStrip_Paste.Size = new System.Drawing.Size(153, 22);
            this.ToolStrip_Paste.Text = "$PASTE";
            this.ToolStrip_Paste.Click += new System.EventHandler(this.ToolStrip_Paste_Click);
            // 
            // TreeView_Tables
            // 
            this.TreeView_Tables.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.TreeView_Tables.ContextMenuStrip = this.Context_Menu;
            this.TreeView_Tables.Location = new System.Drawing.Point(14, 32);
            this.TreeView_Tables.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TreeView_Tables.Name = "TreeView_Tables";
            this.TreeView_Tables.Size = new System.Drawing.Size(429, 472);
            this.TreeView_Tables.TabIndex = 11;
            this.TreeView_Tables.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnNodeSelectSelect);
            // 
            // FileDialog_Open
            // 
            this.FileDialog_Open.DefaultExt = "xml";
            this.FileDialog_Open.Filter = "XML|*xml";
            this.FileDialog_Open.Title = "$OPEN_FILE";
            // 
            // FileDialog_Save
            // 
            this.FileDialog_Save.DefaultExt = "xml";
            this.FileDialog_Save.Filter = "XML|.xml";
            this.FileDialog_Save.Title = "$SAVE_FILE";
            // 
            // EntityDataStorageEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 519);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.PropertyGrid_Item);
            this.Controls.Add(this.TreeView_Tables);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "EntityDataStorageEditor";
            this.Text = "$EDS_EDITOR_TITLE";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EDSEditor_Closing);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.EDSEditor_OnKeyUp);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.Context_Menu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PropertyGrid PropertyGrid_Item;
        private MafiaToolkit.Controls.MTreeView TreeView_Tables;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton Button_File;
        private System.Windows.Forms.ToolStripMenuItem Button_Save;
        private System.Windows.Forms.ToolStripMenuItem Button_Reload;
        private System.Windows.Forms.ToolStripMenuItem Button_Exit;
        private System.Windows.Forms.ToolStripDropDownButton Button_Tools;
        private System.Windows.Forms.ToolStripMenuItem Button_CopyData;
        private System.Windows.Forms.ToolStripMenuItem Button_PasteData;
        private System.Windows.Forms.ContextMenuStrip Context_Menu;
        private System.Windows.Forms.ToolStripMenuItem ToolStrip_Copy;
        private System.Windows.Forms.ToolStripMenuItem ToolStrip_Paste;
        private System.Windows.Forms.ToolStripMenuItem Button_ExportXML;
        private System.Windows.Forms.ToolStripMenuItem Button_ImportXML;
        private System.Windows.Forms.OpenFileDialog FileDialog_Open;
        private System.Windows.Forms.SaveFileDialog FileDialog_Save;
    }
}