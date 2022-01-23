namespace Mafia2Tool
{
    partial class PrefabEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrefabEditor));
            this.Grid_Prefabs = new System.Windows.Forms.PropertyGrid();
            this.TreeView_Prefabs = new Mafia2Tool.Controls.MTreeView();
            this.Context_Menu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Context_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.Browser_ImportPRB = new System.Windows.Forms.OpenFileDialog();
            this.ToolStrip_Main = new System.Windows.Forms.ToolStrip();
            this.Button_File = new System.Windows.Forms.ToolStripDropDownButton();
            this.Button_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Reload = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Tools = new System.Windows.Forms.ToolStripDropDownButton();
            this.Button_Import = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Export = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.Browser_ExportPRB = new System.Windows.Forms.SaveFileDialog();
            this.Context_ImportXML = new System.Windows.Forms.ToolStripMenuItem();
            this.Context_ExportXML = new System.Windows.Forms.ToolStripMenuItem();
            this.Context_Menu.SuspendLayout();
            this.ToolStrip_Main.SuspendLayout();
            this.SuspendLayout();
            // 
            // Grid_Prefabs
            // 
            this.Grid_Prefabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Grid_Prefabs.Location = new System.Drawing.Point(469, 32);
            this.Grid_Prefabs.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Grid_Prefabs.Name = "Grid_Prefabs";
            this.Grid_Prefabs.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.Grid_Prefabs.Size = new System.Drawing.Size(450, 473);
            this.Grid_Prefabs.TabIndex = 10;
            // 
            // TreeView_Prefabs
            // 
            this.TreeView_Prefabs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.TreeView_Prefabs.ContextMenuStrip = this.Context_Menu;
            this.TreeView_Prefabs.Location = new System.Drawing.Point(14, 32);
            this.TreeView_Prefabs.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TreeView_Prefabs.Name = "TreeView_Prefabs";
            this.TreeView_Prefabs.Size = new System.Drawing.Size(429, 472);
            this.TreeView_Prefabs.TabIndex = 11;
            this.TreeView_Prefabs.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnNodeSelectSelect);
            // 
            // Context_Menu
            // 
            this.Context_Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Context_ImportXML,
            this.Context_ExportXML,
            this.Context_Delete});
            this.Context_Menu.Name = "Context_Menu";
            this.Context_Menu.Size = new System.Drawing.Size(181, 92);
            this.Context_Menu.Opening += new System.ComponentModel.CancelEventHandler(this.Context_Menu_OnOpening);
            // 
            // Context_Delete
            // 
            this.Context_Delete.Name = "Context_Delete";
            this.Context_Delete.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete)));
            this.Context_Delete.Size = new System.Drawing.Size(180, 22);
            this.Context_Delete.Text = "$DELETE";
            this.Context_Delete.Click += new System.EventHandler(this.Context_Delete_Click);
            // 
            // Browser_ImportPRB
            // 
            this.Browser_ImportPRB.FileName = "Select Singular Prefab file";
            this.Browser_ImportPRB.Filter = "Prefab File|*.prb|All Files|*.*";
            this.Browser_ImportPRB.Tag = "";
            // 
            // ToolStrip_Main
            // 
            this.ToolStrip_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Button_File,
            this.Button_Tools});
            this.ToolStrip_Main.Location = new System.Drawing.Point(0, 0);
            this.ToolStrip_Main.Name = "ToolStrip_Main";
            this.ToolStrip_Main.Size = new System.Drawing.Size(933, 25);
            this.ToolStrip_Main.TabIndex = 15;
            this.ToolStrip_Main.Text = "toolStrip1";
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
            this.Button_Save.Size = new System.Drawing.Size(180, 22);
            this.Button_Save.Text = "$SAVE";
            this.Button_Save.Click += new System.EventHandler(this.Button_Save_Click);
            // 
            // Button_Reload
            // 
            this.Button_Reload.Name = "Button_Reload";
            this.Button_Reload.Size = new System.Drawing.Size(180, 22);
            this.Button_Reload.Text = "$RELOAD";
            // 
            // Button_Exit
            // 
            this.Button_Exit.Name = "Button_Exit";
            this.Button_Exit.Size = new System.Drawing.Size(180, 22);
            this.Button_Exit.Text = "$EXIT";
            // 
            // Button_Tools
            // 
            this.Button_Tools.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Button_Tools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Button_Import,
            this.Button_Export,
            this.Button_Delete});
            this.Button_Tools.Image = ((System.Drawing.Image)(resources.GetObject("Button_Tools.Image")));
            this.Button_Tools.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_Tools.Name = "Button_Tools";
            this.Button_Tools.Size = new System.Drawing.Size(61, 22);
            this.Button_Tools.Text = "$TOOLS";
            // 
            // Button_Import
            // 
            this.Button_Import.Name = "Button_Import";
            this.Button_Import.Size = new System.Drawing.Size(214, 22);
            this.Button_Import.Text = "$IMPORT_PREFAB";
            this.Button_Import.Click += new System.EventHandler(this.Button_Import_Click);
            // 
            // Button_Export
            // 
            this.Button_Export.Name = "Button_Export";
            this.Button_Export.Size = new System.Drawing.Size(214, 22);
            this.Button_Export.Text = "$EXPORT_PREFAB";
            this.Button_Export.Click += new System.EventHandler(this.Button_Export_Click);
            // 
            // Button_Delete
            // 
            this.Button_Delete.Name = "Button_Delete";
            this.Button_Delete.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete)));
            this.Button_Delete.Size = new System.Drawing.Size(214, 22);
            this.Button_Delete.Text = "$DELETE_PREFAB";
            this.Button_Delete.Click += new System.EventHandler(this.Button_Delete_Click);
            // 
            // Browser_ExportPRB
            // 
            this.Browser_ExportPRB.FileName = "Save Singular Prefab file";
            this.Browser_ExportPRB.Filter = "Prefab File|*.prb|All Files|*.*";
            // 
            // Context_ImportXML
            // 
            this.Context_ImportXML.Name = "Context_ImportXML";
            this.Context_ImportXML.Size = new System.Drawing.Size(180, 22);
            this.Context_ImportXML.Text = "$IMPORT_XML";
            this.Context_ImportXML.Click += new System.EventHandler(this.Context_ImportAsXML_Clicked);
            // 
            // Context_ExportXML
            // 
            this.Context_ExportXML.Name = "Context_ExportXML";
            this.Context_ExportXML.Size = new System.Drawing.Size(180, 22);
            this.Context_ExportXML.Text = "$EXPORT_XML";
            this.Context_ExportXML.Click += new System.EventHandler(this.Context_ExportAsXML_Clicked);
            // 
            // PrefabEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 519);
            this.Controls.Add(this.ToolStrip_Main);
            this.Controls.Add(this.Grid_Prefabs);
            this.Controls.Add(this.TreeView_Prefabs);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "PrefabEditor";
            this.Text = "$PREFAB_EDITOR_TITLE";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PrefabEditor_Closing);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.PrefabEditor_OnKeyUp);
            this.Context_Menu.ResumeLayout(false);
            this.ToolStrip_Main.ResumeLayout(false);
            this.ToolStrip_Main.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PropertyGrid Grid_Prefabs;
        private System.Windows.Forms.OpenFileDialog Browser_ImportPRB;
        private System.Windows.Forms.ToolStrip ToolStrip_Main;
        private System.Windows.Forms.ToolStripDropDownButton Button_File;
        private System.Windows.Forms.ToolStripMenuItem Button_Save;
        private System.Windows.Forms.ToolStripMenuItem Button_Reload;
        private System.Windows.Forms.ToolStripMenuItem Button_Exit;
        private System.Windows.Forms.ToolStripDropDownButton Button_Tools;
        private System.Windows.Forms.ToolStripMenuItem Button_Import;
        private System.Windows.Forms.ToolStripMenuItem Button_Export;
        private System.Windows.Forms.ToolStripMenuItem Button_Delete;
        private System.Windows.Forms.SaveFileDialog Browser_ExportPRB;
        private System.Windows.Forms.ContextMenuStrip Context_Menu;
        private System.Windows.Forms.ToolStripMenuItem Context_Delete;
        private Controls.MTreeView TreeView_Prefabs;
        private System.Windows.Forms.ToolStripMenuItem Context_ImportXML;
        private System.Windows.Forms.ToolStripMenuItem Context_ExportXML;
    }
}