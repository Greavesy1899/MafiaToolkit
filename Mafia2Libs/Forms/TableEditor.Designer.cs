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
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            FileButton = new System.Windows.Forms.ToolStripDropDownButton();
            SaveButton = new System.Windows.Forms.ToolStripMenuItem();
            ReloadButton = new System.Windows.Forms.ToolStripMenuItem();
            ExitButton = new System.Windows.Forms.ToolStripMenuItem();
            EditButton = new System.Windows.Forms.ToolStripDropDownButton();
            AddRowButton = new System.Windows.Forms.ToolStripMenuItem();
            DeleteRowButton = new System.Windows.Forms.ToolStripMenuItem();
            DataGrid = new System.Windows.Forms.DataGridView();
            statusStrip1 = new System.Windows.Forms.StatusStrip();
            ColumnIndexLabel = new System.Windows.Forms.ToolStripStatusLabel();
            RowIndexLabel = new System.Windows.Forms.ToolStripStatusLabel();
            DataTypeLabel = new System.Windows.Forms.ToolStripStatusLabel();
            Label_Version = new System.Windows.Forms.ToolStripLabel();
            toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)DataGrid).BeginInit();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { FileButton, EditButton, Label_Version });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(933, 25);
            toolStrip1.TabIndex = 15;
            toolStrip1.Text = "MainStrip";
            // 
            // FileButton
            // 
            FileButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            FileButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { SaveButton, ReloadButton, ExitButton });
            FileButton.Image = (System.Drawing.Image)resources.GetObject("FileButton.Image");
            FileButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            FileButton.Name = "FileButton";
            FileButton.Size = new System.Drawing.Size(47, 22);
            FileButton.Text = "$FILE";
            // 
            // SaveButton
            // 
            SaveButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            SaveButton.Name = "SaveButton";
            SaveButton.ShortcutKeyDisplayString = "";
            SaveButton.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S;
            SaveButton.Size = new System.Drawing.Size(165, 22);
            SaveButton.Text = "$SAVE";
            SaveButton.Click += SaveOnClick;
            // 
            // ReloadButton
            // 
            ReloadButton.Name = "ReloadButton";
            ReloadButton.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R;
            ReloadButton.Size = new System.Drawing.Size(165, 22);
            ReloadButton.Text = "$RELOAD";
            ReloadButton.Click += ReloadOnClick;
            // 
            // ExitButton
            // 
            ExitButton.Name = "ExitButton";
            ExitButton.Size = new System.Drawing.Size(165, 22);
            ExitButton.Text = "$EXIT";
            ExitButton.Click += ExitButtonOnClick;
            // 
            // EditButton
            // 
            EditButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            EditButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { AddRowButton, DeleteRowButton });
            EditButton.Image = (System.Drawing.Image)resources.GetObject("EditButton.Image");
            EditButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            EditButton.Name = "EditButton";
            EditButton.Size = new System.Drawing.Size(49, 22);
            EditButton.Text = "$EDIT";
            // 
            // AddRowButton
            // 
            AddRowButton.Name = "AddRowButton";
            AddRowButton.Size = new System.Drawing.Size(201, 22);
            AddRowButton.Text = "$ADD_ROW";
            AddRowButton.Click += AddRowOnClick;
            // 
            // DeleteRowButton
            // 
            DeleteRowButton.Name = "DeleteRowButton";
            DeleteRowButton.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete;
            DeleteRowButton.Size = new System.Drawing.Size(201, 22);
            DeleteRowButton.Text = "$DELETE_ROW";
            DeleteRowButton.Click += DeleteRowOnClick;
            // 
            // DataGrid
            // 
            DataGrid.AllowUserToAddRows = false;
            DataGrid.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            DataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            DataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DataGrid.Location = new System.Drawing.Point(0, 32);
            DataGrid.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            DataGrid.MultiSelect = false;
            DataGrid.Name = "DataGrid";
            DataGrid.RightToLeft = System.Windows.Forms.RightToLeft.No;
            DataGrid.Size = new System.Drawing.Size(933, 458);
            DataGrid.TabIndex = 16;
            DataGrid.CellValueChanged += CellContent_Changed;
            DataGrid.SelectionChanged += OnSelectedChange;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ColumnIndexLabel, RowIndexLabel, DataTypeLabel });
            statusStrip1.Location = new System.Drawing.Point(0, 497);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            statusStrip1.Size = new System.Drawing.Size(933, 22);
            statusStrip1.TabIndex = 17;
            statusStrip1.Text = "statusStrip1";
            // 
            // ColumnIndexLabel
            // 
            ColumnIndexLabel.Name = "ColumnIndexLabel";
            ColumnIndexLabel.Size = new System.Drawing.Size(79, 17);
            ColumnIndexLabel.Text = "ColumnIndex";
            // 
            // RowIndexLabel
            // 
            RowIndexLabel.Name = "RowIndexLabel";
            RowIndexLabel.Size = new System.Drawing.Size(59, 17);
            RowIndexLabel.Text = "RowIndex";
            // 
            // DataTypeLabel
            // 
            DataTypeLabel.Name = "DataTypeLabel";
            DataTypeLabel.Size = new System.Drawing.Size(118, 17);
            DataTypeLabel.Text = "toolStripStatusLabel1";
            // 
            // Label_Version
            // 
            Label_Version.Name = "Label_Version";
            Label_Version.Size = new System.Drawing.Size(60, 22);
            Label_Version.Text = "$VERSION";
            // 
            // TableEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(933, 519);
            Controls.Add(statusStrip1);
            Controls.Add(toolStrip1);
            Controls.Add(DataGrid);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "TableEditor";
            Text = "$TABLE_EDITOR";
            FormClosing += TableEditor_Closing;
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)DataGrid).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel ColumnIndexLabel;
        private System.Windows.Forms.ToolStripStatusLabel RowIndexLabel;
        private System.Windows.Forms.ToolStripStatusLabel DataTypeLabel;
        private System.Windows.Forms.ToolStripMenuItem DeleteRowButton;
        private System.Windows.Forms.ToolStripLabel Label_Version;
    }
}