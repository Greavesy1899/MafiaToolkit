﻿namespace Toolkit.Forms
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
            this.Button_Tools = new System.Windows.Forms.ToolStripDropDownButton();
            this.Button_Import = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Export = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Copy = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Paste = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.Context_Menu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Context_Export = new System.Windows.Forms.ToolStripMenuItem();
            this.Context_Copy = new System.Windows.Forms.ToolStripMenuItem();
            this.Context_Paste = new System.Windows.Forms.ToolStripMenuItem();
            this.Context_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.TreeView_FxActors = new MafiaToolkit.Controls.MTreeView();
            this.ToolStrip_Top.SuspendLayout();
            this.Context_Menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // Grid_Actors
            // 
            this.Grid_Actors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Grid_Actors.Location = new System.Drawing.Point(469, 32);
            this.Grid_Actors.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Grid_Actors.Name = "Grid_Actors";
            this.Grid_Actors.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.Grid_Actors.Size = new System.Drawing.Size(450, 473);
            this.Grid_Actors.TabIndex = 10;
            this.Grid_Actors.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.Grid_Actors_PropertyChanged);
            // 
            // ToolStrip_Top
            // 
            this.ToolStrip_Top.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Button_File,
            this.Button_Tools});
            this.ToolStrip_Top.Location = new System.Drawing.Point(0, 0);
            this.ToolStrip_Top.Name = "ToolStrip_Top";
            this.ToolStrip_Top.Size = new System.Drawing.Size(933, 25);
            this.ToolStrip_Top.TabIndex = 15;
            this.ToolStrip_Top.Text = "ToolStrip";
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
            // Button_Tools
            // 
            this.Button_Tools.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Button_Tools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Button_Import,
            this.Button_Export,
            this.Button_Copy,
            this.Button_Paste,
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
            this.Button_Import.Size = new System.Drawing.Size(169, 22);
            this.Button_Import.Text = "$IMPORT";
            this.Button_Import.Click += new System.EventHandler(this.Button_Import_Click);
            // 
            // Button_Export
            // 
            this.Button_Export.Name = "Button_Export";
            this.Button_Export.Size = new System.Drawing.Size(169, 22);
            this.Button_Export.Text = "$EXPORT";
            this.Button_Export.Click += new System.EventHandler(this.Button_Export_Click);
            // 
            // Button_Copy
            // 
            this.Button_Copy.Name = "Button_Copy";
            this.Button_Copy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.Button_Copy.Size = new System.Drawing.Size(169, 22);
            this.Button_Copy.Text = "$COPY";
            this.Button_Copy.Click += new System.EventHandler(this.Button_Copy_Click);
            // 
            // Button_Paste
            // 
            this.Button_Paste.Name = "Button_Paste";
            this.Button_Paste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.Button_Paste.Size = new System.Drawing.Size(169, 22);
            this.Button_Paste.Text = "$PASTE";
            this.Button_Paste.Click += new System.EventHandler(this.Button_Paste_Click);
            // 
            // Button_Delete
            // 
            this.Button_Delete.Name = "Button_Delete";
            this.Button_Delete.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete)));
            this.Button_Delete.Size = new System.Drawing.Size(169, 22);
            this.Button_Delete.Text = "$DELETE";
            this.Button_Delete.Click += new System.EventHandler(this.Button_Delete_Click);
            // 
            // Context_Menu
            // 
            this.Context_Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Context_Export,
            this.Context_Copy,
            this.Context_Paste,
            this.Context_Delete});
            this.Context_Menu.Name = "Context_Menu";
            this.Context_Menu.Size = new System.Drawing.Size(170, 92);
            // 
            // Context_Export
            // 
            this.Context_Export.Name = "Context_Export";
            this.Context_Export.Size = new System.Drawing.Size(169, 22);
            this.Context_Export.Text = "$EXPORT";
            this.Context_Export.Click += new System.EventHandler(this.Context_Export_Click);
            // 
            // Context_Copy
            // 
            this.Context_Copy.Name = "Context_Copy";
            this.Context_Copy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.Context_Copy.Size = new System.Drawing.Size(169, 22);
            this.Context_Copy.Text = "$COPY";
            this.Context_Copy.Click += new System.EventHandler(this.Context_Copy_Click);
            // 
            // Context_Paste
            // 
            this.Context_Paste.Name = "Context_Paste";
            this.Context_Paste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.Context_Paste.Size = new System.Drawing.Size(169, 22);
            this.Context_Paste.Text = "$PASTE";
            this.Context_Paste.Click += new System.EventHandler(this.Context_Paste_Click);
            // 
            // Context_Delete
            // 
            this.Context_Delete.Name = "Context_Delete";
            this.Context_Delete.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete)));
            this.Context_Delete.Size = new System.Drawing.Size(169, 22);
            this.Context_Delete.Text = "$DELETE";
            this.Context_Delete.Click += new System.EventHandler(this.Context_Delete_Click);
            // 
            // TreeView_FxActors
            // 
            this.TreeView_FxActors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.TreeView_FxActors.ContextMenuStrip = this.Context_Menu;
            this.TreeView_FxActors.Location = new System.Drawing.Point(14, 32);
            this.TreeView_FxActors.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TreeView_FxActors.Name = "TreeView_FxActors";
            this.TreeView_FxActors.Size = new System.Drawing.Size(429, 472);
            this.TreeView_FxActors.TabIndex = 11;
            this.TreeView_FxActors.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeView_FxActors_AfterSelect);
            // 
            // FxActorEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 519);
            this.Controls.Add(this.ToolStrip_Top);
            this.Controls.Add(this.Grid_Actors);
            this.Controls.Add(this.TreeView_FxActors);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "FxActorEditor";
            this.Text = "$FXACTOR_EDITOR";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FxActorEditor_Closing);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FxActorEditor_OnKeyUp);
            this.ToolStrip_Top.ResumeLayout(false);
            this.ToolStrip_Top.PerformLayout();
            this.Context_Menu.ResumeLayout(false);
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
        private MafiaToolkit.Controls.MTreeView TreeView_FxActors;
        private System.Windows.Forms.ContextMenuStrip Context_Menu;
        private System.Windows.Forms.ToolStripMenuItem Context_Copy;
        private System.Windows.Forms.ToolStripMenuItem Context_Paste;
        private System.Windows.Forms.ToolStripMenuItem Context_Delete;
        private System.Windows.Forms.ToolStripDropDownButton Button_Tools;
        private System.Windows.Forms.ToolStripMenuItem Button_Delete;
        private System.Windows.Forms.ToolStripMenuItem Button_Copy;
        private System.Windows.Forms.ToolStripMenuItem Button_Paste;
        private System.Windows.Forms.ToolStripMenuItem Button_Import;
        private System.Windows.Forms.ToolStripMenuItem Button_Export;
        private System.Windows.Forms.ToolStripMenuItem Context_Export;
    }
}