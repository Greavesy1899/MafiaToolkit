namespace Mafia2Tool
{
    partial class CCDBEditor
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
            this.Grid_CCDB = new System.Windows.Forms.PropertyGrid();
            this.TreeView_CCDB = new Mafia2Tool.Controls.MTreeView();
            this.ToolStrip_Main = new System.Windows.Forms.ToolStrip();
            this.Button_File = new System.Windows.Forms.ToolStripDropDownButton();
            this.Button_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Reload = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_ExpandAll = new System.Windows.Forms.ToolStripButton();
            this.Button_CollapseAll = new System.Windows.Forms.ToolStripButton();
            this.ToolStrip_Main.SuspendLayout();
            this.SuspendLayout();
            //
            // Grid_CCDB
            //
            this.Grid_CCDB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Grid_CCDB.Location = new System.Drawing.Point(469, 32);
            this.Grid_CCDB.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Grid_CCDB.Name = "Grid_CCDB";
            this.Grid_CCDB.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.Grid_CCDB.Size = new System.Drawing.Size(450, 473);
            this.Grid_CCDB.TabIndex = 10;
            this.Grid_CCDB.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.OnPropertyValueChanged);
            //
            // TreeView_CCDB
            //
            this.TreeView_CCDB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.TreeView_CCDB.Location = new System.Drawing.Point(14, 32);
            this.TreeView_CCDB.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TreeView_CCDB.Name = "TreeView_CCDB";
            this.TreeView_CCDB.Size = new System.Drawing.Size(429, 472);
            this.TreeView_CCDB.TabIndex = 11;
            this.TreeView_CCDB.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnNodeSelectSelect);
            //
            // ToolStrip_Main
            //
            this.ToolStrip_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Button_File,
            this.Button_ExpandAll,
            this.Button_CollapseAll});
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
            this.Button_Save.Click += new System.EventHandler(this.Button_Save_Click);
            //
            // Button_Reload
            //
            this.Button_Reload.Name = "Button_Reload";
            this.Button_Reload.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.Button_Reload.Size = new System.Drawing.Size(165, 22);
            this.Button_Reload.Text = "$RELOAD";
            this.Button_Reload.Click += new System.EventHandler(this.Button_Reload_Click);
            //
            // Button_Exit
            //
            this.Button_Exit.Name = "Button_Exit";
            this.Button_Exit.Size = new System.Drawing.Size(165, 22);
            this.Button_Exit.Text = "$EXIT";
            this.Button_Exit.Click += new System.EventHandler(this.Button_Exit_Click);
            //
            // Button_ExpandAll
            //
            this.Button_ExpandAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Button_ExpandAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_ExpandAll.Name = "Button_ExpandAll";
            this.Button_ExpandAll.Size = new System.Drawing.Size(70, 22);
            this.Button_ExpandAll.Text = "Expand All";
            this.Button_ExpandAll.Click += new System.EventHandler(this.Button_ExpandAll_Click);
            //
            // Button_CollapseAll
            //
            this.Button_CollapseAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Button_CollapseAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_CollapseAll.Name = "Button_CollapseAll";
            this.Button_CollapseAll.Size = new System.Drawing.Size(73, 22);
            this.Button_CollapseAll.Text = "Collapse All";
            this.Button_CollapseAll.Click += new System.EventHandler(this.Button_CollapseAll_Click);
            //
            // CCDBEditor
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 519);
            this.Controls.Add(this.ToolStrip_Main);
            this.Controls.Add(this.Grid_CCDB);
            this.Controls.Add(this.TreeView_CCDB);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "CCDBEditor";
            this.Text = "$CCDB_EDITOR";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CCDBEditor_Closing);
            this.ToolStrip_Main.ResumeLayout(false);
            this.ToolStrip_Main.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PropertyGrid Grid_CCDB;
        private System.Windows.Forms.ToolStrip ToolStrip_Main;
        private System.Windows.Forms.ToolStripDropDownButton Button_File;
        private System.Windows.Forms.ToolStripMenuItem Button_Save;
        private System.Windows.Forms.ToolStripMenuItem Button_Reload;
        private System.Windows.Forms.ToolStripMenuItem Button_Exit;
        private System.Windows.Forms.ToolStripButton Button_ExpandAll;
        private System.Windows.Forms.ToolStripButton Button_CollapseAll;
        private Controls.MTreeView TreeView_CCDB;
    }
}
