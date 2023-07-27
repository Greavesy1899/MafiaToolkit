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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShopMenu2Editor));
            PropertyGrid_ShopMenu2 = new System.Windows.Forms.PropertyGrid();
            TreeView_ShopMenu2 = new Controls.MTreeView();
            Context_Menu = new System.Windows.Forms.ContextMenuStrip(components);
            Context_AddType = new System.Windows.Forms.ToolStripMenuItem();
            Context_AddMetaInfo = new System.Windows.Forms.ToolStripMenuItem();
            Context_DuplicateMetaInfo = new System.Windows.Forms.ToolStripMenuItem();
            Context_DuplicateMetaInfoItem = new System.Windows.Forms.ToolStripMenuItem();
            Context_Delete = new System.Windows.Forms.ToolStripMenuItem();
            ToolStrip_Main = new System.Windows.Forms.ToolStrip();
            Button_File = new System.Windows.Forms.ToolStripDropDownButton();
            Button_Save = new System.Windows.Forms.ToolStripMenuItem();
            Button_Reload = new System.Windows.Forms.ToolStripMenuItem();
            Button_Exit = new System.Windows.Forms.ToolStripMenuItem();
            Button_Tools = new System.Windows.Forms.ToolStripDropDownButton();
            Button_AddType = new System.Windows.Forms.ToolStripMenuItem();
            Button_AddMetaInfo = new System.Windows.Forms.ToolStripMenuItem();
            Button_Delete = new System.Windows.Forms.ToolStripMenuItem();
            Context_Menu.SuspendLayout();
            ToolStrip_Main.SuspendLayout();
            SuspendLayout();
            // 
            // PropertyGrid_ShopMenu2
            // 
            PropertyGrid_ShopMenu2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            PropertyGrid_ShopMenu2.Location = new System.Drawing.Point(469, 32);
            PropertyGrid_ShopMenu2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PropertyGrid_ShopMenu2.Name = "PropertyGrid_ShopMenu2";
            PropertyGrid_ShopMenu2.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            PropertyGrid_ShopMenu2.Size = new System.Drawing.Size(450, 473);
            PropertyGrid_ShopMenu2.TabIndex = 10;
            PropertyGrid_ShopMenu2.PropertyValueChanged += Grid_ShopMenu2_PropertyChanged;
            // 
            // TreeView_ShopMenu2
            // 
            TreeView_ShopMenu2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            TreeView_ShopMenu2.ContextMenuStrip = Context_Menu;
            TreeView_ShopMenu2.Location = new System.Drawing.Point(14, 32);
            TreeView_ShopMenu2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TreeView_ShopMenu2.Name = "TreeView_ShopMenu2";
            TreeView_ShopMenu2.Size = new System.Drawing.Size(429, 472);
            TreeView_ShopMenu2.TabIndex = 11;
            TreeView_ShopMenu2.AfterSelect += OnNodeSelectSelect;
            // 
            // Context_Menu
            // 
            Context_Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { Context_AddType, Context_AddMetaInfo, Context_DuplicateMetaInfo, Context_DuplicateMetaInfoItem, Context_Delete });
            Context_Menu.Name = "SDSContext";
            Context_Menu.Size = new System.Drawing.Size(181, 136);
            Context_Menu.Opening += Context_Menu_OnOpening;
            // 
            // Context_AddType
            // 
            Context_AddType.Name = "Context_AddType";
            Context_AddType.Size = new System.Drawing.Size(180, 22);
            Context_AddType.Text = "$ADD_SHOPTYPE";
            Context_AddType.Click += Context_AddType_OnClick;
            // 
            // Context_AddMetaInfo
            // 
            Context_AddMetaInfo.Name = "Context_AddMetaInfo";
            Context_AddMetaInfo.Size = new System.Drawing.Size(180, 22);
            Context_AddMetaInfo.Text = "$ADD_METAINFO";
            Context_AddMetaInfo.Click += Context_AddMetaInfo_OnClick;
            // 
            // Context_DuplicateMetaInfo
            // 
            Context_DuplicateMetaInfo.Name = "Context_DuplicateMetaInfo";
            Context_DuplicateMetaInfo.Size = new System.Drawing.Size(180, 22);
            Context_DuplicateMetaInfo.Text = "$DUPE_METAINFO";
            Context_DuplicateMetaInfo.Click += Context_DupeMetaInfo_Clicked;
            // 
            // Context_DuplicateMetaInfoItem
            // 
            Context_DuplicateMetaInfoItem.Name = "Context_DuplicateMetaInfoItem";
            Context_DuplicateMetaInfoItem.Size = new System.Drawing.Size(180, 22);
            Context_DuplicateMetaInfoItem.Text = "$DUPLICATE_ITEM";
            Context_DuplicateMetaInfoItem.Click += Context_DupeMetaInfoItem_Clicked;
            // 
            // Context_Delete
            // 
            Context_Delete.Name = "Context_Delete";
            Context_Delete.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete;
            Context_Delete.Size = new System.Drawing.Size(180, 22);
            Context_Delete.Text = "$DELETE";
            Context_Delete.Click += Context_Delete_OnClick;
            // 
            // ToolStrip_Main
            // 
            ToolStrip_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { Button_File, Button_Tools });
            ToolStrip_Main.Location = new System.Drawing.Point(0, 0);
            ToolStrip_Main.Name = "ToolStrip_Main";
            ToolStrip_Main.Size = new System.Drawing.Size(933, 25);
            ToolStrip_Main.TabIndex = 15;
            ToolStrip_Main.Text = "toolStrip1";
            // 
            // Button_File
            // 
            Button_File.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            Button_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { Button_Save, Button_Reload, Button_Exit });
            Button_File.Image = (System.Drawing.Image)resources.GetObject("Button_File.Image");
            Button_File.ImageTransparentColor = System.Drawing.Color.Magenta;
            Button_File.Name = "Button_File";
            Button_File.Size = new System.Drawing.Size(47, 22);
            Button_File.Text = "$FILE";
            // 
            // Button_Save
            // 
            Button_Save.Name = "Button_Save";
            Button_Save.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S;
            Button_Save.Size = new System.Drawing.Size(165, 22);
            Button_Save.Text = "$SAVE";
            Button_Save.Click += Button_Save_OnClick;
            // 
            // Button_Reload
            // 
            Button_Reload.Name = "Button_Reload";
            Button_Reload.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R;
            Button_Reload.Size = new System.Drawing.Size(165, 22);
            Button_Reload.Text = "$RELOAD";
            Button_Reload.Click += Button_Reload_OnClick;
            // 
            // Button_Exit
            // 
            Button_Exit.Name = "Button_Exit";
            Button_Exit.Size = new System.Drawing.Size(165, 22);
            Button_Exit.Text = "$EXIT";
            Button_Exit.Click += Button_Exit_OnClick;
            // 
            // Button_Tools
            // 
            Button_Tools.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            Button_Tools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { Button_AddType, Button_AddMetaInfo, Button_Delete });
            Button_Tools.Image = (System.Drawing.Image)resources.GetObject("Button_Tools.Image");
            Button_Tools.ImageTransparentColor = System.Drawing.Color.Magenta;
            Button_Tools.Name = "Button_Tools";
            Button_Tools.Size = new System.Drawing.Size(61, 22);
            Button_Tools.Text = "$TOOLS";
            Button_Tools.ToolTipText = "$TOOLS";
            // 
            // Button_AddType
            // 
            Button_AddType.Name = "Button_AddType";
            Button_AddType.Size = new System.Drawing.Size(169, 22);
            Button_AddType.Text = "$ADD_SHOPTYPE";
            Button_AddType.Click += Button_AddType_OnClick;
            // 
            // Button_AddMetaInfo
            // 
            Button_AddMetaInfo.Name = "Button_AddMetaInfo";
            Button_AddMetaInfo.Size = new System.Drawing.Size(169, 22);
            Button_AddMetaInfo.Text = "$ADD_METAINFO";
            Button_AddMetaInfo.Click += Button_AddMetaInfo_OnClick;
            // 
            // Button_Delete
            // 
            Button_Delete.Name = "Button_Delete";
            Button_Delete.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete;
            Button_Delete.Size = new System.Drawing.Size(169, 22);
            Button_Delete.Text = "$DELETE";
            Button_Delete.Click += Button_Delete_OnClick;
            // 
            // ShopMenu2Editor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(933, 519);
            Controls.Add(ToolStrip_Main);
            Controls.Add(PropertyGrid_ShopMenu2);
            Controls.Add(TreeView_ShopMenu2);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "ShopMenu2Editor";
            Text = "$SHOPMENU2_EDITOR";
            FormClosing += ShopMenu2Editor_Closing;
            Context_Menu.ResumeLayout(false);
            ToolStrip_Main.ResumeLayout(false);
            ToolStrip_Main.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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
        private System.Windows.Forms.ToolStripMenuItem Context_DuplicateMetaInfo;
        private System.Windows.Forms.ToolStripMenuItem Context_DuplicateMetaInfoItem;
    }
}