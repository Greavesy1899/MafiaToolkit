namespace Mafia2Tool
{
    partial class ShopMenu2Editor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShopMenu2Editor));
            this.PropertyGrid_ShopMenu2 = new System.Windows.Forms.PropertyGrid();
            this.TreeView_ShopMenu2 = new System.Windows.Forms.TreeView();
            this.CollisionContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ContextDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.deletePlacementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStrip_Main = new System.Windows.Forms.ToolStrip();
            this.Button_File = new System.Windows.Forms.ToolStripDropDownButton();
            this.Button_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Reload = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.CollisionContext.SuspendLayout();
            this.ToolStrip_Main.SuspendLayout();
            this.SuspendLayout();
            // 
            // PropertyGrid_ShopMenu2
            // 
            this.PropertyGrid_ShopMenu2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PropertyGrid_ShopMenu2.Location = new System.Drawing.Point(402, 28);
            this.PropertyGrid_ShopMenu2.Name = "PropertyGrid_ShopMenu2";
            this.PropertyGrid_ShopMenu2.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.PropertyGrid_ShopMenu2.Size = new System.Drawing.Size(386, 410);
            this.PropertyGrid_ShopMenu2.TabIndex = 10;
            // 
            // TreeView_ShopMenu2
            // 
            this.TreeView_ShopMenu2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.TreeView_ShopMenu2.ContextMenuStrip = this.CollisionContext;
            this.TreeView_ShopMenu2.Location = new System.Drawing.Point(12, 28);
            this.TreeView_ShopMenu2.Name = "TreeView_ShopMenu2";
            this.TreeView_ShopMenu2.Size = new System.Drawing.Size(368, 410);
            this.TreeView_ShopMenu2.TabIndex = 11;
            this.TreeView_ShopMenu2.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnNodeSelectSelect);
            // 
            // CollisionContext
            // 
            this.CollisionContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ContextDelete,
            this.deletePlacementToolStripMenuItem});
            this.CollisionContext.Name = "SDSContext";
            this.CollisionContext.Size = new System.Drawing.Size(167, 48);
            // 
            // ContextDelete
            // 
            this.ContextDelete.Name = "ContextDelete";
            this.ContextDelete.Size = new System.Drawing.Size(166, 22);
            this.ContextDelete.Text = "Delete Collision";
            // 
            // deletePlacementToolStripMenuItem
            // 
            this.deletePlacementToolStripMenuItem.Name = "deletePlacementToolStripMenuItem";
            this.deletePlacementToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.deletePlacementToolStripMenuItem.Text = "Delete Placement";
            // 
            // ToolStrip_Main
            // 
            this.ToolStrip_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Button_File});
            this.ToolStrip_Main.Location = new System.Drawing.Point(0, 0);
            this.ToolStrip_Main.Name = "ToolStrip_Main";
            this.ToolStrip_Main.Size = new System.Drawing.Size(800, 25);
            this.ToolStrip_Main.TabIndex = 15;
            this.ToolStrip_Main.Text = "toolStrip1";
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
            this.Button_Save.Click += new System.EventHandler(this.Button_Save_OnClick);
            // 
            // Button_Reload
            // 
            this.Button_Reload.Name = "Button_Reload";
            this.Button_Reload.Size = new System.Drawing.Size(180, 22);
            this.Button_Reload.Text = "$RELOAD";
            this.Button_Reload.Click += new System.EventHandler(this.Button_Reload_OnClick);
            // 
            // Button_Exit
            // 
            this.Button_Exit.Name = "Button_Exit";
            this.Button_Exit.Size = new System.Drawing.Size(180, 22);
            this.Button_Exit.Text = "$EXIT";
            this.Button_Exit.Click += new System.EventHandler(this.Button_Exit_OnClick);
            // 
            // ShopMenu2Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ToolStrip_Main);
            this.Controls.Add(this.PropertyGrid_ShopMenu2);
            this.Controls.Add(this.TreeView_ShopMenu2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ShopMenu2Editor";
            this.Text = "$SHOPMENU2_EDITOR";
            this.CollisionContext.ResumeLayout(false);
            this.ToolStrip_Main.ResumeLayout(false);
            this.ToolStrip_Main.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PropertyGrid PropertyGrid_ShopMenu2;
        private System.Windows.Forms.TreeView TreeView_ShopMenu2;
        private System.Windows.Forms.ToolStrip ToolStrip_Main;
        private System.Windows.Forms.ToolStripDropDownButton Button_File;
        private System.Windows.Forms.ToolStripMenuItem Button_Save;
        private System.Windows.Forms.ToolStripMenuItem Button_Reload;
        private System.Windows.Forms.ToolStripMenuItem Button_Exit;
        private System.Windows.Forms.ContextMenuStrip CollisionContext;
        private System.Windows.Forms.ToolStripMenuItem ContextDelete;
        private System.Windows.Forms.ToolStripMenuItem deletePlacementToolStripMenuItem;
    }
}