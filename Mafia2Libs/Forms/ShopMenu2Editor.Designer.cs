namespace MafiaToolkit
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
            this.TreeView_ShopMenu2 = new MafiaToolkit.Controls.MTreeView();
            this.Context_Menu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Context_AddType = new System.Windows.Forms.ToolStripMenuItem();
            this.Context_AddMetaInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.Context_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStrip_Main = new System.Windows.Forms.ToolStrip();
            this.Button_File = new System.Windows.Forms.ToolStripDropDownButton();
            this.Button_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Reload = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Tools = new System.Windows.Forms.ToolStripDropDownButton();
            this.Button_AddType = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_AddMetaInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.Context_Menu.SuspendLayout();
            this.ToolStrip_Main.SuspendLayout();
            this.SuspendLayout();
            // 
            // PropertyGrid_ShopMenu2
            // 
            this.PropertyGrid_ShopMenu2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PropertyGrid_ShopMenu2.Location = new System.Drawing.Point(469, 32);
            this.PropertyGrid_ShopMenu2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.PropertyGrid_ShopMenu2.Name = "PropertyGrid_ShopMenu2";
            this.PropertyGrid_ShopMenu2.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.PropertyGrid_ShopMenu2.Size = new System.Drawing.Size(450, 473);
            this.PropertyGrid_ShopMenu2.TabIndex = 10;
            this.PropertyGrid_ShopMenu2.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.Grid_ShopMenu2_PropertyChanged);
            // 
            // TreeView_ShopMenu2
            // 
            this.TreeView_ShopMenu2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.TreeView_ShopMenu2.ContextMenuStrip = this.Context_Menu;
            this.TreeView_ShopMenu2.Location = new System.Drawing.Point(14, 32);
            this.TreeView_ShopMenu2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TreeView_ShopMenu2.Name = "TreeView_ShopMenu2";
            this.TreeView_ShopMenu2.Size = new System.Drawing.Size(429, 472);
            this.TreeView_ShopMenu2.TabIndex = 11;
            this.TreeView_ShopMenu2.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnNodeSelectSelect);
            // 
            // Context_Menu
            // 
            this.Context_Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Context_AddType,
            this.Context_AddMetaInfo,
            this.Context_Delete});
            this.Context_Menu.Name = "SDSContext";
            this.Context_Menu.Size = new System.Drawing.Size(170, 70);
            // 
            // Context_AddType
            // 
            this.Context_AddType.Name = "Context_AddType";
            this.Context_AddType.Size = new System.Drawing.Size(169, 22);
            this.Context_AddType.Text = "$ADD_SHOPTYPE";
            this.Context_AddType.Click += new System.EventHandler(this.Context_AddType_OnClick);
            // 
            // Context_AddMetaInfo
            // 
            this.Context_AddMetaInfo.Name = "Context_AddMetaInfo";
            this.Context_AddMetaInfo.Size = new System.Drawing.Size(169, 22);
            this.Context_AddMetaInfo.Text = "$ADD_METAINFO";
            this.Context_AddMetaInfo.Click += new System.EventHandler(this.Context_AddMetaInfo_OnClick);
            // 
            // Context_Delete
            // 
            this.Context_Delete.Name = "Context_Delete";
            this.Context_Delete.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete)));
            this.Context_Delete.Size = new System.Drawing.Size(169, 22);
            this.Context_Delete.Text = "$DELETE";
            this.Context_Delete.Click += new System.EventHandler(this.Context_Delete_OnClick);
            // 
            // ToolStrip_Main
            // 
            this.ToolStrip_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Button_File,
            this.Button_Tools});
            this.ToolStrip_Main.Location = new System.Drawing.Point(0, 0);
            this.ToolStrip_Main.Name = "ToolStrip_Main";
            this.ToolStrip_Main.Size = new System.Drawing.Size(933, 25);
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
            this.Button_Save.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.Button_Save.Size = new System.Drawing.Size(165, 22);
            this.Button_Save.Text = "$SAVE";
            this.Button_Save.Click += new System.EventHandler(this.Button_Save_OnClick);
            // 
            // Button_Reload
            // 
            this.Button_Reload.Name = "Button_Reload";
            this.Button_Reload.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.Button_Reload.Size = new System.Drawing.Size(165, 22);
            this.Button_Reload.Text = "$RELOAD";
            this.Button_Reload.Click += new System.EventHandler(this.Button_Reload_OnClick);
            // 
            // Button_Exit
            // 
            this.Button_Exit.Name = "Button_Exit";
            this.Button_Exit.Size = new System.Drawing.Size(165, 22);
            this.Button_Exit.Text = "$EXIT";
            this.Button_Exit.Click += new System.EventHandler(this.Button_Exit_OnClick);
            // 
            // Button_Tools
            // 
            this.Button_Tools.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Button_Tools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Button_AddType,
            this.Button_AddMetaInfo,
            this.Button_Delete});
            this.Button_Tools.Image = ((System.Drawing.Image)(resources.GetObject("Button_Tools.Image")));
            this.Button_Tools.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_Tools.Name = "Button_Tools";
            this.Button_Tools.Size = new System.Drawing.Size(61, 22);
            this.Button_Tools.Text = "$TOOLS";
            this.Button_Tools.ToolTipText = "$TOOLS";
            // 
            // Button_AddType
            // 
            this.Button_AddType.Name = "Button_AddType";
            this.Button_AddType.Size = new System.Drawing.Size(169, 22);
            this.Button_AddType.Text = "$ADD_SHOPTYPE";
            this.Button_AddType.Click += new System.EventHandler(this.Button_AddType_OnClick);
            // 
            // Button_AddMetaInfo
            // 
            this.Button_AddMetaInfo.Name = "Button_AddMetaInfo";
            this.Button_AddMetaInfo.Size = new System.Drawing.Size(169, 22);
            this.Button_AddMetaInfo.Text = "$ADD_METAINFO";
            this.Button_AddMetaInfo.Click += new System.EventHandler(this.Button_AddMetaInfo_OnClick);
            // 
            // Button_Delete
            // 
            this.Button_Delete.Name = "Button_Delete";
            this.Button_Delete.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete)));
            this.Button_Delete.Size = new System.Drawing.Size(169, 22);
            this.Button_Delete.Text = "$DELETE";
            this.Button_Delete.Click += new System.EventHandler(this.Button_Delete_OnClick);
            // 
            // ShopMenu2Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 519);
            this.Controls.Add(this.ToolStrip_Main);
            this.Controls.Add(this.PropertyGrid_ShopMenu2);
            this.Controls.Add(this.TreeView_ShopMenu2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "ShopMenu2Editor";
            this.Text = "$SHOPMENU2_EDITOR";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ShopMenu2Editor_Closing);
            this.Context_Menu.ResumeLayout(false);
            this.ToolStrip_Main.ResumeLayout(false);
            this.ToolStrip_Main.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PropertyGrid PropertyGrid_ShopMenu2;
        private System.Windows.Forms.ToolStrip ToolStrip_Main;
        private System.Windows.Forms.ToolStripDropDownButton Button_File;
        private System.Windows.Forms.ToolStripMenuItem Button_Save;
        private System.Windows.Forms.ToolStripMenuItem Button_Reload;
        private System.Windows.Forms.ToolStripMenuItem Button_Exit;
        private System.Windows.Forms.ContextMenuStrip CollisionContext;
        private System.Windows.Forms.ToolStripMenuItem Context_Delete;
        private Controls.MTreeView TreeView_ShopMenu2;
        private System.Windows.Forms.ContextMenuStrip Context_Menu;
        private System.Windows.Forms.ToolStripDropDownButton Button_Tools;
        private System.Windows.Forms.ToolStripMenuItem Button_Delete;
        private System.Windows.Forms.ToolStripMenuItem Button_AddType;
        private System.Windows.Forms.ToolStripMenuItem Button_AddMetaInfo;
        private System.Windows.Forms.ToolStripMenuItem Context_AddType;
        private System.Windows.Forms.ToolStripMenuItem Context_AddMetaInfo;
    }
}