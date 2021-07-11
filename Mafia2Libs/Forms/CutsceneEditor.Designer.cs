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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CutsceneEditor));
            this.Tool_Strip = new System.Windows.Forms.ToolStrip();
            this.Button_File = new System.Windows.Forms.ToolStripDropDownButton();
            this.Button_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Reload = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Edit = new System.Windows.Forms.ToolStripDropDownButton();
            this.AddItemButton = new System.Windows.Forms.ToolStripMenuItem();
            this.AddDefinitionButton = new System.Windows.Forms.ToolStripMenuItem();
            this.PropertyGrid_Cutscene = new System.Windows.Forms.PropertyGrid();
            this.TreeView_Cutscene = new Mafia2Tool.Controls.MTreeView();
            this.Tool_Strip.SuspendLayout();
            this.SuspendLayout();
            // 
            // Tool_Strip
            // 
            this.Tool_Strip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Button_File,
            this.Button_Edit});
            this.Tool_Strip.Location = new System.Drawing.Point(0, 0);
            this.Tool_Strip.Name = "Tool_Strip";
            this.Tool_Strip.Size = new System.Drawing.Size(933, 25);
            this.Tool_Strip.TabIndex = 18;
            this.Tool_Strip.Text = "ToolStrip_Main";
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
            // Button_Edit
            // 
            this.Button_Edit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Button_Edit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddItemButton,
            this.AddDefinitionButton});
            this.Button_Edit.Enabled = false;
            this.Button_Edit.Image = ((System.Drawing.Image)(resources.GetObject("Button_Edit.Image")));
            this.Button_Edit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_Edit.Name = "Button_Edit";
            this.Button_Edit.Size = new System.Drawing.Size(49, 22);
            this.Button_Edit.Text = "$EDIT";
            // 
            // AddItemButton
            // 
            this.AddItemButton.Enabled = false;
            this.AddItemButton.Name = "AddItemButton";
            this.AddItemButton.Size = new System.Drawing.Size(171, 22);
            this.AddItemButton.Text = "$ADD_ITEM";
            // 
            // AddDefinitionButton
            // 
            this.AddDefinitionButton.Enabled = false;
            this.AddDefinitionButton.Name = "AddDefinitionButton";
            this.AddDefinitionButton.Size = new System.Drawing.Size(171, 22);
            this.AddDefinitionButton.Text = "$ADD_DEFINITION";
            // 
            // PropertyGrid_Cutscene
            // 
            this.PropertyGrid_Cutscene.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PropertyGrid_Cutscene.Location = new System.Drawing.Point(469, 39);
            this.PropertyGrid_Cutscene.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.PropertyGrid_Cutscene.Name = "PropertyGrid_Cutscene";
            this.PropertyGrid_Cutscene.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.PropertyGrid_Cutscene.Size = new System.Drawing.Size(450, 473);
            this.PropertyGrid_Cutscene.TabIndex = 16;
            this.PropertyGrid_Cutscene.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.PropertyGrid_Cutscene_PropertyChanged);
            // 
            // TreeView_Cutscene
            // 
            this.TreeView_Cutscene.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.TreeView_Cutscene.Location = new System.Drawing.Point(14, 39);
            this.TreeView_Cutscene.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TreeView_Cutscene.Name = "TreeView_Cutscene";
            this.TreeView_Cutscene.Size = new System.Drawing.Size(429, 472);
            this.TreeView_Cutscene.TabIndex = 17;
            this.TreeView_Cutscene.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeView_Cutscene_AfterSelect);
            // 
            // CutsceneEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 519);
            this.Controls.Add(this.Tool_Strip);
            this.Controls.Add(this.PropertyGrid_Cutscene);
            this.Controls.Add(this.TreeView_Cutscene);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "CutsceneEditor";
            this.Text = "$CUTSCENE_EDITOR";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CutsceneEditor_Closing);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CutsceneEditor_OnKeyUp);
            this.Tool_Strip.ResumeLayout(false);
            this.Tool_Strip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}