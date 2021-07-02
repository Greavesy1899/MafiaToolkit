namespace Toolkit.Forms
{
    partial class FxAnimSetEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FxAnimSetEditor));
            this.Grid_AnimSet = new System.Windows.Forms.PropertyGrid();
            this.TreeView_AnimSet = new Mafia2Tool.Controls.MTreeView();
            this.ToolStrip_Top = new System.Windows.Forms.ToolStrip();
            this.Button_File = new System.Windows.Forms.ToolStripDropDownButton();
            this.Button_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Reload = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStrip_Top.SuspendLayout();
            this.SuspendLayout();
            // 
            // Grid_AnimSet
            // 
            this.Grid_AnimSet.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Grid_AnimSet.Location = new System.Drawing.Point(402, 28);
            this.Grid_AnimSet.Name = "Grid_AnimSet";
            this.Grid_AnimSet.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.Grid_AnimSet.Size = new System.Drawing.Size(386, 410);
            this.Grid_AnimSet.TabIndex = 10;
            // 
            // TreeView_AnimSet
            // 
            this.TreeView_AnimSet.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.TreeView_AnimSet.Location = new System.Drawing.Point(12, 28);
            this.TreeView_AnimSet.Name = "TreeView_AnimSet";
            this.TreeView_AnimSet.Size = new System.Drawing.Size(368, 410);
            this.TreeView_AnimSet.TabIndex = 11;
            // 
            // ToolStrip_Top
            // 
            this.ToolStrip_Top.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Button_File});
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
            this.Button_Save.Size = new System.Drawing.Size(180, 22);
            this.Button_Save.Text = "$SAVE";
            // 
            // Button_Reload
            // 
            this.Button_Reload.Name = "Button_Reload";
            this.Button_Reload.Size = new System.Drawing.Size(180, 22);
            this.Button_Reload.Text = "$RELOAD";
            // 
            // Button_Exit
            // 
            this.Button_Exit.Name = "Button_Exit";
            this.Button_Exit.Size = new System.Drawing.Size(180, 22);
            this.Button_Exit.Text = "$EXIT";
            // 
            // FxAnimSetEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ToolStrip_Top);
            this.Controls.Add(this.Grid_AnimSet);
            this.Controls.Add(this.TreeView_AnimSet);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FxAnimSetEditor";
            this.Text = "$FXANIMSET_EDITOR";
            this.ToolStrip_Top.ResumeLayout(false);
            this.ToolStrip_Top.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PropertyGrid Grid_AnimSet;
        private System.Windows.Forms.ToolStrip ToolStrip_Top;
        private System.Windows.Forms.ToolStripDropDownButton Button_File;
        private System.Windows.Forms.ToolStripMenuItem Button_Save;
        private System.Windows.Forms.ToolStripMenuItem Button_Reload;
        private System.Windows.Forms.ToolStripMenuItem Button_Exit;
        private Mafia2Tool.Controls.MTreeView TreeView_AnimSet;
    }
}