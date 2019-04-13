namespace Mafia2Tool
{
    partial class TableEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TableEditor));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.FileButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.SaveButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ReloadButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitButton = new System.Windows.Forms.ToolStripMenuItem();
            this.DataGrid = new System.Windows.Forms.DataGridView();
            this.EditButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.AddRowButton = new System.Windows.Forms.ToolStripMenuItem();
            this.AddColumnButton = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileButton,
            this.EditButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 25);
            this.toolStrip1.TabIndex = 15;
            this.toolStrip1.Text = "MainStrip";
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
            this.SaveButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.ShortcutKeyDisplayString = "";
            this.SaveButton.Size = new System.Drawing.Size(180, 22);
            this.SaveButton.Text = "$SAVE";
            this.SaveButton.Click += new System.EventHandler(this.SaveOnClick);
            // 
            // ReloadButton
            // 
            this.ReloadButton.Name = "ReloadButton";
            this.ReloadButton.Size = new System.Drawing.Size(180, 22);
            this.ReloadButton.Text = "$RELOAD";
            this.ReloadButton.Click += new System.EventHandler(this.ReloadOnClick);
            // 
            // ExitButton
            // 
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(180, 22);
            this.ExitButton.Text = "$EXIT";
            this.ExitButton.Click += new System.EventHandler(this.ExitButtonOnClick);
            // 
            // DataGrid
            // 
            this.DataGrid.AllowUserToAddRows = false;
            this.DataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataGrid.Location = new System.Drawing.Point(0, 25);
            this.DataGrid.Name = "DataGrid";
            this.DataGrid.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.DataGrid.Size = new System.Drawing.Size(800, 425);
            this.DataGrid.TabIndex = 16;
            // 
            // EditButton
            // 
            this.EditButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.EditButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddRowButton,
            this.AddColumnButton});
            this.EditButton.Image = ((System.Drawing.Image)(resources.GetObject("EditButton.Image")));
            this.EditButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.EditButton.Name = "EditButton";
            this.EditButton.Size = new System.Drawing.Size(50, 22);
            this.EditButton.Text = "$EDIT";
            // 
            // AddRowButton
            // 
            this.AddRowButton.Name = "AddRowButton";
            this.AddRowButton.Size = new System.Drawing.Size(180, 22);
            this.AddRowButton.Text = "$ADD_ROW";
            this.AddRowButton.Click += new System.EventHandler(this.AddRowOnClick);
            // 
            // AddColumnButton
            // 
            this.AddColumnButton.Name = "AddColumnButton";
            this.AddColumnButton.Size = new System.Drawing.Size(180, 22);
            this.AddColumnButton.Text = "$ADD_COLUMN";
            this.AddColumnButton.Click += new System.EventHandler(this.AddColumnOnClick);
            // 
            // TableEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.DataGrid);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TableEditor";
            this.Text = "$TABLE_EDITOR";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton FileButton;
        private System.Windows.Forms.ToolStripMenuItem SaveButton;
        private System.Windows.Forms.ToolStripMenuItem ReloadButton;
        private System.Windows.Forms.ToolStripMenuItem ExitButton;
        private System.Windows.Forms.DataGridView DataGrid;
        private System.Windows.Forms.ToolStripDropDownButton EditButton;
        private System.Windows.Forms.ToolStripMenuItem AddRowButton;
        private System.Windows.Forms.ToolStripMenuItem AddColumnButton;
    }
}