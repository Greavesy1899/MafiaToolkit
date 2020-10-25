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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.Button_File = new System.Windows.Forms.ToolStripDropDownButton();
            this.SaveButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ReloadButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitButton = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Edit = new System.Windows.Forms.ToolStripDropDownButton();
            this.AddItemButton = new System.Windows.Forms.ToolStripMenuItem();
            this.AddDefinitionButton = new System.Windows.Forms.ToolStripMenuItem();
            this.PropertyGrid_Cutscene = new System.Windows.Forms.PropertyGrid();
            this.TreeView_Cutscene = new System.Windows.Forms.TreeView();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Button_File,
            this.Button_Edit});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 25);
            this.toolStrip1.TabIndex = 18;
            this.toolStrip1.Text = "ToolStrip_Main";
            // 
            // Button_File
            // 
            this.Button_File.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Button_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveButton,
            this.ReloadButton,
            this.ExitButton});
            this.Button_File.Image = ((System.Drawing.Image)(resources.GetObject("Button_File.Image")));
            this.Button_File.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_File.Name = "Button_File";
            this.Button_File.Size = new System.Drawing.Size(47, 22);
            this.Button_File.Text = "$FILE";
            // 
            // SaveButton
            // 
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(180, 22);
            this.SaveButton.Text = "$SAVE";
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_OnClick);
            // 
            // ReloadButton
            // 
            this.ReloadButton.Name = "ReloadButton";
            this.ReloadButton.Size = new System.Drawing.Size(180, 22);
            this.ReloadButton.Text = "$RELOAD";
            this.ReloadButton.Click += new System.EventHandler(this.Reload_OnClick);
            // 
            // ExitButton
            // 
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(180, 22);
            this.ExitButton.Text = "$EXIT";
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_OnClick);
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
            this.PropertyGrid_Cutscene.Location = new System.Drawing.Point(402, 34);
            this.PropertyGrid_Cutscene.Name = "PropertyGrid_Cutscene";
            this.PropertyGrid_Cutscene.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.PropertyGrid_Cutscene.Size = new System.Drawing.Size(386, 410);
            this.PropertyGrid_Cutscene.TabIndex = 16;
            // 
            // TreeView_Cutscene
            // 
            this.TreeView_Cutscene.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.TreeView_Cutscene.Location = new System.Drawing.Point(12, 34);
            this.TreeView_Cutscene.Name = "TreeView_Cutscene";
            this.TreeView_Cutscene.Size = new System.Drawing.Size(368, 410);
            this.TreeView_Cutscene.TabIndex = 17;
            this.TreeView_Cutscene.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeView_Cutscene_AfterSelect);
            // 
            // CutsceneEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.PropertyGrid_Cutscene);
            this.Controls.Add(this.TreeView_Cutscene);
            this.Name = "CutsceneEditor";
            this.Text = "CutsceneEditor";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton Button_File;
        private System.Windows.Forms.ToolStripMenuItem SaveButton;
        private System.Windows.Forms.ToolStripMenuItem ReloadButton;
        private System.Windows.Forms.ToolStripMenuItem ExitButton;
        private System.Windows.Forms.ToolStripDropDownButton Button_Edit;
        private System.Windows.Forms.ToolStripMenuItem AddItemButton;
        private System.Windows.Forms.ToolStripMenuItem AddDefinitionButton;
        private System.Windows.Forms.PropertyGrid PropertyGrid_Cutscene;
        private System.Windows.Forms.TreeView TreeView_Cutscene;
    }
}