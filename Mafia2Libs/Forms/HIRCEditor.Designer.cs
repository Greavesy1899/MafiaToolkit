namespace Toolkit
{
    partial class HIRCEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HIRCEditor));
            this.HircGrid = new System.Windows.Forms.PropertyGrid();
            this.BnkContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ContextExport = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.TreeView_HIRC = new System.Windows.Forms.TreeView();
            this.TreeView_HIRC.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnNodeSelectSelect);
            this.BnkContext.SuspendLayout();
            this.SuspendLayout();
            // 
            // HircGrid
            // 
            this.HircGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.HircGrid.Location = new System.Drawing.Point(469, 32);
            this.HircGrid.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.HircGrid.Name = "HircGrid";
            this.HircGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.HircGrid.Size = new System.Drawing.Size(450, 473);
            this.HircGrid.TabIndex = 10;
            this.HircGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.WemGrid_OnPropertyValueChanged);
            // 
            // BnkContext
            // 
            this.BnkContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ContextExport,
            this.ContextDelete});
            this.BnkContext.Name = "SDSContext";
            this.BnkContext.Size = new System.Drawing.Size(159, 48);
            // 
            // ContextExport
            // 
            this.ContextExport.Name = "ContextExport";
            this.ContextExport.Size = new System.Drawing.Size(158, 22);
            this.ContextExport.Text = "$EXPORT_WEM";
            // 
            // ContextDelete
            // 
            this.ContextDelete.Name = "ContextDelete";
            this.ContextDelete.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete)));
            this.ContextDelete.Size = new System.Drawing.Size(158, 22);
            this.ContextDelete.Text = "Delete";
            // 
            // TreeView_HIRC
            // 
            this.TreeView_HIRC.Location = new System.Drawing.Point(14, 32);
            this.TreeView_HIRC.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TreeView_HIRC.Name = "TreeView_HIRC";
            this.TreeView_HIRC.Size = new System.Drawing.Size(429, 472);
            this.TreeView_HIRC.TabIndex = 11;
            // 
            // HIRCEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 519);
            this.Controls.Add(this.TreeView_HIRC);
            this.Controls.Add(this.HircGrid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "HIRCEditor";
            this.Text = "$HIRC_EDITOR_TITLE";
            this.Load += new System.EventHandler(this.HIRCEditor_Load);
            this.BnkContext.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid HircGrid;
        private System.Windows.Forms.ContextMenuStrip BnkContext;
        private System.Windows.Forms.ToolStripMenuItem ContextDelete;
        private Controls.MTreeView TreeView_Wems;
        private System.Windows.Forms.ToolStripMenuItem ContextExport;
        private System.Windows.Forms.TreeView TreeView_HIRC;
    }
}