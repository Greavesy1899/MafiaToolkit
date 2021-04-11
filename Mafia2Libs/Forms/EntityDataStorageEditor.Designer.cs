namespace Mafia2Tool
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EntityDataStorageEditor));
            this.PropertyGrid_Item = new System.Windows.Forms.PropertyGrid();
            this.TreeView_Tables = new Controls.MTreeView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.Button_File = new System.Windows.Forms.ToolStripDropDownButton();
            this.Button_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Reload = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Tools = new System.Windows.Forms.ToolStripDropDownButton();
            this.Button_CopyData = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_PasteData = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // PropertyGrid_Item
            // 
            this.PropertyGrid_Item.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PropertyGrid_Item.Location = new System.Drawing.Point(402, 28);
            this.PropertyGrid_Item.Name = "PropertyGrid_Item";
            this.PropertyGrid_Item.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.PropertyGrid_Item.Size = new System.Drawing.Size(386, 410);
            this.PropertyGrid_Item.TabIndex = 10;
            // 
            // TreeView_Tables
            // 
            this.TreeView_Tables.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.TreeView_Tables.Location = new System.Drawing.Point(12, 28);
            this.TreeView_Tables.Name = "TreeView_Tables";
            this.TreeView_Tables.Size = new System.Drawing.Size(368, 410);
            this.TreeView_Tables.TabIndex = 11;
            this.TreeView_Tables.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnNodeSelectSelect);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Button_File,
            this.Button_Tools});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 25);
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
            this.Button_Save.Size = new System.Drawing.Size(124, 22);
            this.Button_Save.Text = "$SAVE";
            this.Button_Save.Click += new System.EventHandler(this.Button_Save_OnClick);
            // 
            // Button_Reload
            // 
            this.Button_Reload.Name = "Button_Reload";
            this.Button_Reload.Size = new System.Drawing.Size(124, 22);
            this.Button_Reload.Text = "$RELOAD";
            this.Button_Reload.Click += new System.EventHandler(this.Button_Reload_OnClick);
            // 
            // Button_Exit
            // 
            this.Button_Exit.Name = "Button_Exit";
            this.Button_Exit.Size = new System.Drawing.Size(124, 22);
            this.Button_Exit.Text = "$EXIT";
            this.Button_Exit.Click += new System.EventHandler(this.Button_Exit_OnClick);
            // 
            // Button_Tools
            // 
            this.Button_Tools.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Button_Tools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Button_CopyData,
            this.Button_PasteData});
            this.Button_Tools.Image = ((System.Drawing.Image)(resources.GetObject("Button_Tools.Image")));
            this.Button_Tools.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_Tools.Name = "Button_Tools";
            this.Button_Tools.Size = new System.Drawing.Size(61, 22);
            this.Button_Tools.Text = "$TOOLS";
            // 
            // Button_CopyData
            // 
            this.Button_CopyData.Name = "Button_CopyData";
            this.Button_CopyData.Size = new System.Drawing.Size(180, 22);
            this.Button_CopyData.Text = "$COPY";
            this.Button_CopyData.Click += new System.EventHandler(this.Button_CopyData_Click);
            // 
            // Button_PasteData
            // 
            this.Button_PasteData.Name = "Button_PasteData";
            this.Button_PasteData.Size = new System.Drawing.Size(180, 22);
            this.Button_PasteData.Text = "$PASTE";
            this.Button_PasteData.Click += new System.EventHandler(this.Button_Paste_Click);
            // 
            // EntityDataStorageEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.PropertyGrid_Item);
            this.Controls.Add(this.TreeView_Tables);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EntityDataStorageEditor";
            this.Text = "$ACTOR_EDITOR_TITLE";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PropertyGrid PropertyGrid_Item;
        private System.Windows.Forms.TreeView TreeView_Tables;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton Button_File;
        private System.Windows.Forms.ToolStripMenuItem Button_Save;
        private System.Windows.Forms.ToolStripMenuItem Button_Reload;
        private System.Windows.Forms.ToolStripMenuItem Button_Exit;
        private System.Windows.Forms.ToolStripDropDownButton Button_Tools;
        private System.Windows.Forms.ToolStripMenuItem Button_CopyData;
        private System.Windows.Forms.ToolStripMenuItem Button_PasteData;
    }
}