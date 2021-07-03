namespace Toolkit.Forms
{
    partial class FxActorEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FxActorEditor));
            this.Grid_Actors = new System.Windows.Forms.PropertyGrid();
            this.ToolStrip_Top = new System.Windows.Forms.ToolStrip();
            this.Button_File = new System.Windows.Forms.ToolStripDropDownButton();
            this.Button_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Reload = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.FxActorToolsDrop = new System.Windows.Forms.ToolStripDropDownButton();
            this.FxActToolCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.FxActToolPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.FxActToolDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.FxActContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ContextMenu_Copy = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextMenu_Paste = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextMenu_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.FxActorTreeView = new Mafia2Tool.Controls.MTreeView();
            this.ToolStrip_Top.SuspendLayout();
            this.FxActContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // Grid_Actors
            // 
            this.Grid_Actors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Grid_Actors.Location = new System.Drawing.Point(402, 28);
            this.Grid_Actors.Name = "Grid_Actors";
            this.Grid_Actors.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.Grid_Actors.Size = new System.Drawing.Size(386, 410);
            this.Grid_Actors.TabIndex = 10;
            this.Grid_Actors.Click += new System.EventHandler(this.Grid_Actors_Click);
            // 
            // ToolStrip_Top
            // 
            this.ToolStrip_Top.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Button_File,
            this.FxActorToolsDrop});
            this.ToolStrip_Top.Location = new System.Drawing.Point(0, 0);
            this.ToolStrip_Top.Name = "ToolStrip_Top";
            this.ToolStrip_Top.Size = new System.Drawing.Size(800, 25);
            this.ToolStrip_Top.TabIndex = 15;
            this.ToolStrip_Top.Text = "toolStrip1";
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
            this.Button_Save.Click += new System.EventHandler(this.Button_Save_Click);
            // 
            // Button_Reload
            // 
            this.Button_Reload.Name = "Button_Reload";
            this.Button_Reload.Size = new System.Drawing.Size(124, 22);
            this.Button_Reload.Text = "$RELOAD";
            this.Button_Reload.Click += new System.EventHandler(this.Button_Reload_Click);
            // 
            // Button_Exit
            // 
            this.Button_Exit.Name = "Button_Exit";
            this.Button_Exit.Size = new System.Drawing.Size(124, 22);
            this.Button_Exit.Text = "$EXIT";
            this.Button_Exit.Click += new System.EventHandler(this.Button_Exit_Click);
            // 
            // FxActorToolsDrop
            // 
            this.FxActorToolsDrop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.FxActorToolsDrop.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FxActToolCopy,
            this.FxActToolPaste,
            this.FxActToolDelete});
            this.FxActorToolsDrop.Image = ((System.Drawing.Image)(resources.GetObject("FxActorToolsDrop.Image")));
            this.FxActorToolsDrop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.FxActorToolsDrop.Name = "FxActorToolsDrop";
            this.FxActorToolsDrop.Size = new System.Drawing.Size(61, 22);
            this.FxActorToolsDrop.Text = "$TOOLS";
            // 
            // FxActToolCopy
            // 
            this.FxActToolCopy.Name = "FxActToolCopy";
            this.FxActToolCopy.Size = new System.Drawing.Size(118, 22);
            this.FxActToolCopy.Text = "$COPY";
            this.FxActToolCopy.Click += new System.EventHandler(this.FxActToolCopy_Click);
            // 
            // FxActToolPaste
            // 
            this.FxActToolPaste.Name = "FxActToolPaste";
            this.FxActToolPaste.Size = new System.Drawing.Size(118, 22);
            this.FxActToolPaste.Text = "$PASTE";
            this.FxActToolPaste.Click += new System.EventHandler(this.FxActToolPaste_Click);
            // 
            // FxActToolDelete
            // 
            this.FxActToolDelete.Name = "FxActToolDelete";
            this.FxActToolDelete.Size = new System.Drawing.Size(118, 22);
            this.FxActToolDelete.Text = "$DELETE";
            this.FxActToolDelete.Click += new System.EventHandler(this.FxActToolDelete_Click);
            // 
            // FxActContextMenu
            // 
            this.FxActContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ContextMenu_Copy,
            this.ContextMenu_Paste,
            this.ContextMenu_Delete});
            this.FxActContextMenu.Name = "FxActContextMenu";
            this.FxActContextMenu.Size = new System.Drawing.Size(119, 70);
            this.FxActContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.FxActContextMenu_Opening);
            // 
            // ContextMenu_Copy
            // 
            this.ContextMenu_Copy.Name = "ContextMenu_Copy";
            this.ContextMenu_Copy.Size = new System.Drawing.Size(118, 22);
            this.ContextMenu_Copy.Text = "$COPY";
            this.ContextMenu_Copy.Click += new System.EventHandler(this.FxActContextCopy_Click);
            // 
            // ContextMenu_Paste
            // 
            this.ContextMenu_Paste.Name = "ContextMenu_Paste";
            this.ContextMenu_Paste.Size = new System.Drawing.Size(118, 22);
            this.ContextMenu_Paste.Text = "$PASTE";
            this.ContextMenu_Paste.Click += new System.EventHandler(this.FxActContextPaste_Click);
            // 
            // ContextMenu_Delete
            // 
            this.ContextMenu_Delete.Name = "ContextMenu_Delete";
            this.ContextMenu_Delete.Size = new System.Drawing.Size(118, 22);
            this.ContextMenu_Delete.Text = "$DELETE";
            this.ContextMenu_Delete.Click += new System.EventHandler(this.FxActContextDelete_Click);
            // 
            // FxActorTreeView
            // 
            this.FxActorTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.FxActorTreeView.ContextMenuStrip = this.FxActContextMenu;
            this.FxActorTreeView.Location = new System.Drawing.Point(12, 28);
            this.FxActorTreeView.Name = "FxActorTreeView";
            this.FxActorTreeView.Size = new System.Drawing.Size(368, 410);
            this.FxActorTreeView.TabIndex = 11;
            this.FxActorTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.FxActorTreeView_AfterSelect);
            this.FxActorTreeView.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FxActorTreeView_OnKeyUp);
            // 
            // FxActorEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ToolStrip_Top);
            this.Controls.Add(this.Grid_Actors);
            this.Controls.Add(this.FxActorTreeView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FxActorEditor";
            this.Text = "$FXACTOR_EDITOR";
            this.ToolStrip_Top.ResumeLayout(false);
            this.ToolStrip_Top.PerformLayout();
            this.FxActContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PropertyGrid Grid_Actors;
        private System.Windows.Forms.ToolStrip ToolStrip_Top;
        private System.Windows.Forms.ToolStripDropDownButton Button_File;
        private System.Windows.Forms.ToolStripMenuItem Button_Save;
        private System.Windows.Forms.ToolStripMenuItem Button_Reload;
        private System.Windows.Forms.ToolStripMenuItem Button_Exit;
        private Mafia2Tool.Controls.MTreeView FxActorTreeView;
        private System.Windows.Forms.ContextMenuStrip FxActContextMenu;
        private System.Windows.Forms.ToolStripMenuItem ContextMenu_Copy;
        private System.Windows.Forms.ToolStripMenuItem ContextMenu_Paste;
        private System.Windows.Forms.ToolStripMenuItem ContextMenu_Delete;
        private System.Windows.Forms.ToolStripDropDownButton FxActorToolsDrop;
        private System.Windows.Forms.ToolStripMenuItem FxActToolDelete;
        private System.Windows.Forms.ToolStripMenuItem FxActToolCopy;
        private System.Windows.Forms.ToolStripMenuItem FxActToolPaste;
    }
}