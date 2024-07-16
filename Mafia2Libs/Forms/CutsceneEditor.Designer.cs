namespace Mafia2Tool.Forms
{
    partial class CutsceneEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CutsceneEditor));
            Tool_Strip = new System.Windows.Forms.ToolStrip();
            Button_File = new System.Windows.Forms.ToolStripDropDownButton();
            Button_Save = new System.Windows.Forms.ToolStripMenuItem();
            Button_Reload = new System.Windows.Forms.ToolStripMenuItem();
            Button_Exit = new System.Windows.Forms.ToolStripMenuItem();
            Button_Edit = new System.Windows.Forms.ToolStripDropDownButton();
            AddItemButton = new System.Windows.Forms.ToolStripMenuItem();
            AddDefinitionButton = new System.Windows.Forms.ToolStripMenuItem();
            PropertyGrid_Cutscene = new System.Windows.Forms.PropertyGrid();
            TreeView_Cutscene = new Controls.MTreeView();
            TreeViewContextMenu = new System.Windows.Forms.ContextMenuStrip(components);
            ContextMenu_Duplicate = new System.Windows.Forms.ToolStripMenuItem();
            Tool_Strip.SuspendLayout();
            TreeViewContextMenu.SuspendLayout();
            SuspendLayout();
            // 
            // Tool_Strip
            // 
            Tool_Strip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { Button_File, Button_Edit });
            Tool_Strip.Location = new System.Drawing.Point(0, 0);
            Tool_Strip.Name = "Tool_Strip";
            Tool_Strip.Size = new System.Drawing.Size(933, 25);
            Tool_Strip.TabIndex = 18;
            Tool_Strip.Text = "ToolStrip_Main";
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
            // Button_Edit
            // 
            Button_Edit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            Button_Edit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { AddItemButton, AddDefinitionButton });
            Button_Edit.Enabled = false;
            Button_Edit.Image = (System.Drawing.Image)resources.GetObject("Button_Edit.Image");
            Button_Edit.ImageTransparentColor = System.Drawing.Color.Magenta;
            Button_Edit.Name = "Button_Edit";
            Button_Edit.Size = new System.Drawing.Size(49, 22);
            Button_Edit.Text = "$EDIT";
            // 
            // AddItemButton
            // 
            AddItemButton.Enabled = false;
            AddItemButton.Name = "AddItemButton";
            AddItemButton.Size = new System.Drawing.Size(171, 22);
            AddItemButton.Text = "$ADD_ITEM";
            // 
            // AddDefinitionButton
            // 
            AddDefinitionButton.Enabled = false;
            AddDefinitionButton.Name = "AddDefinitionButton";
            AddDefinitionButton.Size = new System.Drawing.Size(171, 22);
            AddDefinitionButton.Text = "$ADD_DEFINITION";
            // 
            // PropertyGrid_Cutscene
            // 
            PropertyGrid_Cutscene.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            PropertyGrid_Cutscene.Location = new System.Drawing.Point(469, 39);
            PropertyGrid_Cutscene.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PropertyGrid_Cutscene.Name = "PropertyGrid_Cutscene";
            PropertyGrid_Cutscene.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            PropertyGrid_Cutscene.Size = new System.Drawing.Size(450, 473);
            PropertyGrid_Cutscene.TabIndex = 16;
            PropertyGrid_Cutscene.PropertyValueChanged += PropertyGrid_Cutscene_PropertyChanged;
            // 
            // TreeView_Cutscene
            // 
            TreeView_Cutscene.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            TreeView_Cutscene.ContextMenuStrip = TreeViewContextMenu;
            TreeView_Cutscene.Location = new System.Drawing.Point(14, 39);
            TreeView_Cutscene.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TreeView_Cutscene.Name = "TreeView_Cutscene";
            TreeView_Cutscene.Size = new System.Drawing.Size(429, 472);
            TreeView_Cutscene.TabIndex = 17;
            TreeView_Cutscene.AfterSelect += TreeView_Cutscene_AfterSelect;
            // 
            // TreeViewContextMenu
            // 
            TreeViewContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ContextMenu_Duplicate });
            TreeViewContextMenu.Name = "TreeViewContextMenu";
            TreeViewContextMenu.Size = new System.Drawing.Size(244, 48);
            TreeViewContextMenu.Opening += TreeViewContextMenu_Opening;
            // 
            // ContextMenu_Duplicate
            // 
            ContextMenu_Duplicate.Name = "ContextMenu_Duplicate";
            ContextMenu_Duplicate.Size = new System.Drawing.Size(243, 22);
            ContextMenu_Duplicate.Text = "$CUTSCENE_DUPLICATE_ENTITY";
            ContextMenu_Duplicate.Click += ContextMenu_Duplicate_Click;
            // 
            // CutsceneEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(933, 519);
            Controls.Add(Tool_Strip);
            Controls.Add(PropertyGrid_Cutscene);
            Controls.Add(TreeView_Cutscene);
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "CutsceneEditor";
            Text = "$CUTSCENE_EDITOR";
            FormClosing += CutsceneEditor_Closing;
            KeyUp += CutsceneEditor_OnKeyUp;
            Tool_Strip.ResumeLayout(false);
            Tool_Strip.PerformLayout();
            TreeViewContextMenu.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ToolStrip Tool_Strip;
        private System.Windows.Forms.ToolStripDropDownButton Button_File;
        private System.Windows.Forms.ToolStripMenuItem Button_Save;
        private System.Windows.Forms.ToolStripMenuItem ReloadButton;
        private System.Windows.Forms.ToolStripMenuItem ExitButton;
        private System.Windows.Forms.ToolStripDropDownButton Button_Edit;
        private System.Windows.Forms.ToolStripMenuItem AddItemButton;
        private System.Windows.Forms.ToolStripMenuItem AddDefinitionButton;
        private System.Windows.Forms.PropertyGrid PropertyGrid_Cutscene;
        private Controls.MTreeView TreeView_Cutscene;
        private System.Windows.Forms.ToolStripMenuItem Button_Reload;
        private System.Windows.Forms.ToolStripMenuItem Button_Exit;
        private System.Windows.Forms.ContextMenuStrip TreeViewContextMenu;
        private System.Windows.Forms.ToolStripMenuItem ContextMenu_Duplicate;
    }
}