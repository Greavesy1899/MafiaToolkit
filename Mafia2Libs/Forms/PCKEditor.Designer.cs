namespace Mafia2Tool
{
    partial class PCKEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PCKEditor));
            this.WemGrid = new System.Windows.Forms.PropertyGrid();
            this.TreeView_Wems = new Mafia2Tool.Controls.MTreeView();
            this.PckContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ContextDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.FileButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.SaveButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ReloadButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitButton = new System.Windows.Forms.ToolStripMenuItem();
            this.EditButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.Button_ImportWem = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_DeleteWem = new System.Windows.Forms.ToolStripMenuItem();
            this.PckContext.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // WemGrid
            // 
            this.WemGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WemGrid.Location = new System.Drawing.Point(469, 32);
            this.WemGrid.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.WemGrid.Name = "WemGrid";
            this.WemGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.WemGrid.Size = new System.Drawing.Size(450, 473);
            this.WemGrid.TabIndex = 10;
            this.WemGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.WemGrid_OnPropertyValueChanged);
            // 
            // TreeView_Wems
            // 
            this.TreeView_Wems.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.TreeView_Wems.ContextMenuStrip = this.PckContext;
            this.TreeView_Wems.Location = new System.Drawing.Point(14, 32);
            this.TreeView_Wems.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TreeView_Wems.Name = "TreeView_Wems";
            this.TreeView_Wems.Size = new System.Drawing.Size(429, 472);
            this.TreeView_Wems.TabIndex = 11;
            this.TreeView_Wems.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnNodeSelectSelect);
            this.TreeView_Wems.KeyUp += new System.Windows.Forms.KeyEventHandler(this.PckTreeView_OnKeyUp);
            // 
            // PckContext
            // 
            this.PckContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ContextDelete});
            this.PckContext.Name = "SDSContext";
            this.PckContext.Size = new System.Drawing.Size(159, 26);
            // 
            // ContextDelete
            // 
            this.ContextDelete.Name = "ContextDelete";
            this.ContextDelete.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete)));
            this.ContextDelete.Size = new System.Drawing.Size(158, 22);
            this.ContextDelete.Text = "Delete";
            this.ContextDelete.Click += new System.EventHandler(this.ContextDelete_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileButton,
            this.EditButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(933, 25);
            this.toolStrip1.TabIndex = 15;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // FileButton
            // 
            this.FileButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.FileButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveButton,
            this.ReloadButton,
            this.ExitButton});
            this.FileButton.Image = ((System.Drawing.Image)(resources.GetObject("FileButton.Image")));
            this.FileButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.FileButton.Name = "FileButton";
            this.FileButton.Size = new System.Drawing.Size(47, 22);
            this.FileButton.Text = "$FILE";
            // 
            // SaveButton
            // 
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.SaveButton.Size = new System.Drawing.Size(165, 22);
            this.SaveButton.Text = "$SAVE";
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_OnClick);
            // 
            // ReloadButton
            // 
            this.ReloadButton.Name = "ReloadButton";
            this.ReloadButton.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.ReloadButton.Size = new System.Drawing.Size(165, 22);
            this.ReloadButton.Text = "$RELOAD";
            this.ReloadButton.Click += new System.EventHandler(this.ReloadButton_OnClick);
            // 
            // ExitButton
            // 
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(165, 22);
            this.ExitButton.Text = "$EXIT";
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_OnClick);
            // 
            // EditButton
            // 
            this.EditButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.EditButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Button_ImportWem,
            this.Button_DeleteWem});
            this.EditButton.Image = ((System.Drawing.Image)(resources.GetObject("EditButton.Image")));
            this.EditButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.EditButton.Name = "EditButton";
            this.EditButton.Size = new System.Drawing.Size(49, 22);
            this.EditButton.Text = "$EDIT";
            // 
            // Button_ImportWem
            // 
            this.Button_ImportWem.Name = "Button_ImportWem";
            this.Button_ImportWem.Size = new System.Drawing.Size(155, 22);
            this.Button_ImportWem.Text = "$IMPORT_WEM";
            this.Button_ImportWem.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.Button_ImportWem.Click += new System.EventHandler(this.Button_ImportWem_Click);
            // 
            // Button_DeleteWem
            // 
            this.Button_DeleteWem.Name = "Button_DeleteWem";
            this.Button_DeleteWem.Size = new System.Drawing.Size(155, 22);
            this.Button_DeleteWem.Text = "$DELETE_WEM";
            this.Button_DeleteWem.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.Button_DeleteWem.Click += new System.EventHandler(this.Button_DeleteWem_Click);
            // 
            // PCKEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 519);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.WemGrid);
            this.Controls.Add(this.TreeView_Wems);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "PCKEditor";
            this.Text = "$PCK_EDITOR_TITLE";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PckEditor_Closing);
            this.PckContext.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PropertyGrid WemGrid;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton FileButton;
        private System.Windows.Forms.ToolStripMenuItem SaveButton;
        private System.Windows.Forms.ToolStripMenuItem ReloadButton;
        private System.Windows.Forms.ToolStripMenuItem ExitButton;
        private System.Windows.Forms.ContextMenuStrip PckContext;
        private System.Windows.Forms.ToolStripMenuItem ContextDelete;
        private System.Windows.Forms.ToolStripDropDownButton EditButton;
        private System.Windows.Forms.ToolStripMenuItem Button_ImportWem;
        private Controls.MTreeView TreeView_Wems;
        private System.Windows.Forms.ToolStripMenuItem Button_DeleteWem;
    }
}