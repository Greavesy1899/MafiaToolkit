namespace Mafia2Tool
{
    partial class ActorEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ActorEditor));
            this.ActorGrid = new System.Windows.Forms.PropertyGrid();
            this.ActorTreeView = new Mafia2Tool.Controls.MTreeView();
            this.ActorContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ContextDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.FileButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.SaveButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ReloadButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitButton = new System.Windows.Forms.ToolStripMenuItem();
            this.EditButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.AddItemButton = new System.Windows.Forms.ToolStripMenuItem();
            this.AddDefinitionButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ActorContext.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ActorGrid
            // 
            this.ActorGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ActorGrid.Location = new System.Drawing.Point(469, 32);
            this.ActorGrid.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ActorGrid.Name = "ActorGrid";
            this.ActorGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.ActorGrid.Size = new System.Drawing.Size(450, 473);
            this.ActorGrid.TabIndex = 10;
            this.ActorGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.ActorGrid_OnPropertyValueChanged);
            // 
            // ActorTreeView
            // 
            this.ActorTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ActorTreeView.ContextMenuStrip = this.ActorContext;
            this.ActorTreeView.Location = new System.Drawing.Point(14, 32);
            this.ActorTreeView.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ActorTreeView.Name = "ActorTreeView";
            this.ActorTreeView.Size = new System.Drawing.Size(429, 472);
            this.ActorTreeView.TabIndex = 11;
            this.ActorTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnNodeSelectSelect);
            this.ActorTreeView.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ActorTreeView_OnKeyUp);
            // 
            // ActorContext
            // 
            this.ActorContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ContextDelete,
            this.ContextCopy,
            this.ContextPaste});
            this.ActorContext.Name = "SDSContext";
            this.ActorContext.Size = new System.Drawing.Size(159, 70);
            this.ActorContext.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenu_OnOpening);
            // 
            // ContextDelete
            // 
            this.ContextDelete.Name = "ContextDelete";
            this.ContextDelete.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete)));
            this.ContextDelete.Size = new System.Drawing.Size(158, 22);
            this.ContextDelete.Text = "Delete";
            this.ContextDelete.Click += new System.EventHandler(this.ContextDelete_Click);
            // 
            // ContextCopy
            // 
            this.ContextCopy.Name = "ContextCopy";
            this.ContextCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.ContextCopy.Size = new System.Drawing.Size(158, 22);
            this.ContextCopy.Text = "$COPY";
            this.ContextCopy.Click += new System.EventHandler(this.ContextCopy_Click);
            // 
            // ContextPaste
            // 
            this.ContextPaste.Name = "ContextPaste";
            this.ContextPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.ContextPaste.Size = new System.Drawing.Size(158, 22);
            this.ContextPaste.Text = "$PASTE";
            this.ContextPaste.Click += new System.EventHandler(this.ContextPaste_Click);
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
            this.AddItemButton,
            this.AddDefinitionButton});
            this.EditButton.Image = ((System.Drawing.Image)(resources.GetObject("EditButton.Image")));
            this.EditButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.EditButton.Name = "EditButton";
            this.EditButton.Size = new System.Drawing.Size(49, 22);
            this.EditButton.Text = "$EDIT";
            // 
            // AddItemButton
            // 
            this.AddItemButton.Name = "AddItemButton";
            this.AddItemButton.Size = new System.Drawing.Size(171, 22);
            this.AddItemButton.Text = "$ADD_ITEM";
            this.AddItemButton.Click += new System.EventHandler(this.AddItemButton_Click);
            // 
            // AddDefinitionButton
            // 
            this.AddDefinitionButton.Name = "AddDefinitionButton";
            this.AddDefinitionButton.Size = new System.Drawing.Size(171, 22);
            this.AddDefinitionButton.Text = "$ADD_DEFINITION";
            this.AddDefinitionButton.Click += new System.EventHandler(this.AddDefinitionButton_Click);
            // 
            // ActorEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 519);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.ActorGrid);
            this.Controls.Add(this.ActorTreeView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "ActorEditor";
            this.Text = "$ACTOR_EDITOR_TITLE";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ActorEditor_Closing);
            this.ActorContext.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PropertyGrid ActorGrid;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton FileButton;
        private System.Windows.Forms.ToolStripMenuItem SaveButton;
        private System.Windows.Forms.ToolStripMenuItem ReloadButton;
        private System.Windows.Forms.ToolStripMenuItem ExitButton;
        private System.Windows.Forms.ContextMenuStrip ActorContext;
        private System.Windows.Forms.ToolStripMenuItem ContextDelete;
        private System.Windows.Forms.ToolStripDropDownButton EditButton;
        private System.Windows.Forms.ToolStripMenuItem AddItemButton;
        private System.Windows.Forms.ToolStripMenuItem AddDefinitionButton;
        private System.Windows.Forms.ToolStripMenuItem ContextCopy;
        private System.Windows.Forms.ToolStripMenuItem ContextPaste;
        private Mafia2Tool.Controls.MTreeView ActorTreeView;
    }
}