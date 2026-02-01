namespace Mafia2Tool
{
    partial class FrameNameTableEditor
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
            DataGrid = new System.Windows.Forms.PropertyGrid();
            DataTreeView = new Controls.MTreeView();
            ContextMenu = new System.Windows.Forms.ContextMenuStrip(components);
            ContextDelete = new System.Windows.Forms.ToolStripMenuItem();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            FileButton = new System.Windows.Forms.ToolStripDropDownButton();
            SaveButton = new System.Windows.Forms.ToolStripMenuItem();
            ReloadButton = new System.Windows.Forms.ToolStripMenuItem();
            ExitButton = new System.Windows.Forms.ToolStripMenuItem();
            EditButton = new System.Windows.Forms.ToolStripDropDownButton();
            AddEntryButton = new System.Windows.Forms.ToolStripMenuItem();
            DeleteEntryButton = new System.Windows.Forms.ToolStripMenuItem();
            ToolsButton = new System.Windows.Forms.ToolStripDropDownButton();
            ExportXMLButton = new System.Windows.Forms.ToolStripMenuItem();
            ContextMenu.SuspendLayout();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            //
            // DataGrid
            //
            DataGrid.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            DataGrid.Location = new System.Drawing.Point(469, 32);
            DataGrid.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            DataGrid.Name = "DataGrid";
            DataGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            DataGrid.Size = new System.Drawing.Size(450, 473);
            DataGrid.TabIndex = 10;
            DataGrid.PropertyValueChanged += DataGrid_OnPropertyValueChanged;
            //
            // DataTreeView
            //
            DataTreeView.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            DataTreeView.ContextMenuStrip = ContextMenu;
            DataTreeView.Location = new System.Drawing.Point(14, 32);
            DataTreeView.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            DataTreeView.Name = "DataTreeView";
            DataTreeView.Size = new System.Drawing.Size(429, 472);
            DataTreeView.TabIndex = 11;
            DataTreeView.AfterSelect += OnNodeSelect;
            //
            // ContextMenu
            //
            ContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ContextDelete });
            ContextMenu.Name = "ContextMenu";
            ContextMenu.Size = new System.Drawing.Size(181, 48);
            //
            // ContextDelete
            //
            ContextDelete.Name = "ContextDelete";
            ContextDelete.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete;
            ContextDelete.Size = new System.Drawing.Size(180, 22);
            ContextDelete.Text = "Delete";
            ContextDelete.Click += DeleteEntryButton_Click;
            //
            // toolStrip1
            //
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { FileButton, EditButton, ToolsButton });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(933, 25);
            toolStrip1.TabIndex = 15;
            toolStrip1.Text = "toolStrip1";
            //
            // FileButton
            //
            FileButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            FileButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { SaveButton, ReloadButton, ExitButton });
            FileButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            FileButton.Name = "FileButton";
            FileButton.Size = new System.Drawing.Size(47, 22);
            FileButton.Text = "$FILE";
            //
            // SaveButton
            //
            SaveButton.Name = "SaveButton";
            SaveButton.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S;
            SaveButton.Size = new System.Drawing.Size(165, 22);
            SaveButton.Text = "$SAVE";
            SaveButton.Click += SaveButton_OnClick;
            //
            // ReloadButton
            //
            ReloadButton.Name = "ReloadButton";
            ReloadButton.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R;
            ReloadButton.Size = new System.Drawing.Size(165, 22);
            ReloadButton.Text = "$RELOAD";
            ReloadButton.Click += ReloadButton_OnClick;
            //
            // ExitButton
            //
            ExitButton.Name = "ExitButton";
            ExitButton.Size = new System.Drawing.Size(165, 22);
            ExitButton.Text = "$EXIT";
            ExitButton.Click += ExitButton_OnClick;
            //
            // EditButton
            //
            EditButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            EditButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { AddEntryButton, DeleteEntryButton });
            EditButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            EditButton.Name = "EditButton";
            EditButton.Size = new System.Drawing.Size(49, 22);
            EditButton.Text = "$EDIT";
            //
            // AddEntryButton
            //
            AddEntryButton.Name = "AddEntryButton";
            AddEntryButton.Size = new System.Drawing.Size(180, 22);
            AddEntryButton.Text = "$ADD_ITEM";
            AddEntryButton.Click += AddEntryButton_Click;
            //
            // DeleteEntryButton
            //
            DeleteEntryButton.Name = "DeleteEntryButton";
            DeleteEntryButton.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete;
            DeleteEntryButton.Size = new System.Drawing.Size(180, 22);
            DeleteEntryButton.Text = "$DELETE";
            DeleteEntryButton.Click += DeleteEntryButton_Click;
            //
            // ToolsButton
            //
            ToolsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            ToolsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            ToolsButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { ExportXMLButton });
            ToolsButton.Name = "ToolsButton";
            ToolsButton.Size = new System.Drawing.Size(61, 22);
            ToolsButton.Text = "$TOOLS";
            //
            // ExportXMLButton
            //
            ExportXMLButton.Name = "ExportXMLButton";
            ExportXMLButton.Size = new System.Drawing.Size(180, 22);
            ExportXMLButton.Text = "$EXPORT_XML";
            ExportXMLButton.Click += ExportXMLButton_Click;
            //
            // FrameNameTableEditor
            //
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(933, 519);
            Controls.Add(toolStrip1);
            Controls.Add(DataGrid);
            Controls.Add(DataTreeView);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FrameNameTableEditor";
            Text = "$FNT_EDITOR_TITLE";
            FormClosing += FrameNameTableEditor_Closing;
            ContextMenu.ResumeLayout(false);
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.PropertyGrid DataGrid;
        private Mafia2Tool.Controls.MTreeView DataTreeView;
        private System.Windows.Forms.ContextMenuStrip ContextMenu;
        private System.Windows.Forms.ToolStripMenuItem ContextDelete;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton FileButton;
        private System.Windows.Forms.ToolStripMenuItem SaveButton;
        private System.Windows.Forms.ToolStripMenuItem ReloadButton;
        private System.Windows.Forms.ToolStripMenuItem ExitButton;
        private System.Windows.Forms.ToolStripDropDownButton EditButton;
        private System.Windows.Forms.ToolStripMenuItem AddEntryButton;
        private System.Windows.Forms.ToolStripMenuItem DeleteEntryButton;
        private System.Windows.Forms.ToolStripDropDownButton ToolsButton;
        private System.Windows.Forms.ToolStripMenuItem ExportXMLButton;
    }
}
