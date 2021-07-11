
namespace Mafia2Tool
{
    partial class PckEditor
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
            this.Grid_Pck = new System.Windows.Forms.PropertyGrid();
            this.TreeView_Pck = new Mafia2Tool.Controls.MTreeView();
            this.Context_Menu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStrip_Pck = new System.Windows.Forms.ToolStrip();
            this.Button_File = new System.Windows.Forms.ToolStripDropDownButton();
            this.Button_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Reload = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Edit = new System.Windows.Forms.ToolStripDropDownButton();
            this.ToolStrip_Pck.SuspendLayout();
            this.SuspendLayout();
            // 
            // Grid_Pck
            // 
            this.Grid_Pck.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Grid_Pck.Location = new System.Drawing.Point(469, 32);
            this.Grid_Pck.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Grid_Pck.Name = "Grid_Pck";
            this.Grid_Pck.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.Grid_Pck.Size = new System.Drawing.Size(450, 473);
            this.Grid_Pck.TabIndex = 10;
            // 
            // TreeView_Pck
            // 
            this.TreeView_Pck.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.TreeView_Pck.ContextMenuStrip = this.Context_Menu;
            this.TreeView_Pck.Location = new System.Drawing.Point(14, 32);
            this.TreeView_Pck.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TreeView_Pck.Name = "TreeView_Pck";
            this.TreeView_Pck.Size = new System.Drawing.Size(429, 472);
            this.TreeView_Pck.TabIndex = 11;
            this.TreeView_Pck.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.PckEditor_OnNodeSelect);
            // 
            // Context_Menu
            // 
            this.Context_Menu.Name = "SDSContext";
            this.Context_Menu.Size = new System.Drawing.Size(61, 4);
            // 
            // ToolStrip_Pck
            // 
            this.ToolStrip_Pck.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Button_File,
            this.Button_Edit});
            this.ToolStrip_Pck.Location = new System.Drawing.Point(0, 0);
            this.ToolStrip_Pck.Name = "ToolStrip_Pck";
            this.ToolStrip_Pck.Size = new System.Drawing.Size(933, 25);
            this.ToolStrip_Pck.TabIndex = 15;
            this.ToolStrip_Pck.Text = "ToolStrip_Pck";
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
            // Button_Edit
            // 
            this.Button_Edit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Button_Edit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_Edit.Name = "Button_Edit";
            this.Button_Edit.Size = new System.Drawing.Size(49, 22);
            this.Button_Edit.Text = "$EDIT";
            // 
            // PckEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 519);
            this.Controls.Add(this.ToolStrip_Pck);
            this.Controls.Add(this.Grid_Pck);
            this.Controls.Add(this.TreeView_Pck);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "PckEditor";
            this.Text = "$PCK_EDITOR_TITLE";
            this.ToolStrip_Pck.ResumeLayout(false);
            this.ToolStrip_Pck.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PropertyGrid Grid_Pck;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton Button_File;
        private System.Windows.Forms.ToolStripMenuItem SaveButton;
        private System.Windows.Forms.ToolStripMenuItem ReloadButton;
        private System.Windows.Forms.ToolStripMenuItem ExitButton;
        private System.Windows.Forms.ContextMenuStrip Context_Menu;
        private System.Windows.Forms.ToolStripDropDownButton EditButton;
        private Controls.MTreeView TreeView_Pck;
        private System.Windows.Forms.ToolStrip ToolStrip_Pck;
        private System.Windows.Forms.ToolStripMenuItem Button_Save;
        private System.Windows.Forms.ToolStripMenuItem Button_Reload;
        private System.Windows.Forms.ToolStripMenuItem Button_Exit;
        private System.Windows.Forms.ToolStripDropDownButton Button_Edit;
    }
}