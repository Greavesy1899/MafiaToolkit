namespace Mafia2Tool
{
    partial class SpeechEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpeechEditor));
            Grid_Speech = new System.Windows.Forms.PropertyGrid();
            TreeView_Speech = new Controls.MTreeView();
            FileOpenDialog_SelectXML = new System.Windows.Forms.OpenFileDialog();
            Tool_Strip = new System.Windows.Forms.ToolStrip();
            Button_File = new System.Windows.Forms.ToolStripDropDownButton();
            Button_Save = new System.Windows.Forms.ToolStripMenuItem();
            Button_Reload = new System.Windows.Forms.ToolStripMenuItem();
            Button_Exit = new System.Windows.Forms.ToolStripMenuItem();
            Button_Edit = new System.Windows.Forms.ToolStripDropDownButton();
            Button_SaveToXML = new System.Windows.Forms.ToolStripMenuItem();
            Button_LoadFromXML = new System.Windows.Forms.ToolStripMenuItem();
            FileSaveDialog_SelectXML = new System.Windows.Forms.SaveFileDialog();
            Tool_Strip.SuspendLayout();
            SuspendLayout();
            // 
            // Grid_Speech
            // 
            Grid_Speech.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            Grid_Speech.Location = new System.Drawing.Point(469, 32);
            Grid_Speech.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Grid_Speech.Name = "Grid_Speech";
            Grid_Speech.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            Grid_Speech.Size = new System.Drawing.Size(450, 473);
            Grid_Speech.TabIndex = 10;
            Grid_Speech.PropertyValueChanged += Grid_Speech_PropertyChanged;
            // 
            // TreeView_Speech
            // 
            TreeView_Speech.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            TreeView_Speech.Location = new System.Drawing.Point(14, 32);
            TreeView_Speech.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TreeView_Speech.Name = "TreeView_Speech";
            TreeView_Speech.Size = new System.Drawing.Size(429, 472);
            TreeView_Speech.TabIndex = 11;
            TreeView_Speech.AfterSelect += OnNodeSelectSelect;
            // 
            // FileOpenDialog_SelectXML
            // 
            FileOpenDialog_SelectXML.Filter = "XML File|*.xml";
            FileOpenDialog_SelectXML.Tag = "";
            // 
            // Tool_Strip
            // 
            Tool_Strip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { Button_File, Button_Edit });
            Tool_Strip.Location = new System.Drawing.Point(0, 0);
            Tool_Strip.Name = "Tool_Strip";
            Tool_Strip.Size = new System.Drawing.Size(933, 25);
            Tool_Strip.TabIndex = 15;
            Tool_Strip.Text = "toolStrip1";
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
            Button_Save.Click += Button_Save_Click;
            // 
            // Button_Reload
            // 
            Button_Reload.Name = "Button_Reload";
            Button_Reload.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R;
            Button_Reload.Size = new System.Drawing.Size(165, 22);
            Button_Reload.Text = "$RELOAD";
            Button_Reload.Click += Button_Reload_Click;
            // 
            // Button_Exit
            // 
            Button_Exit.Name = "Button_Exit";
            Button_Exit.Size = new System.Drawing.Size(165, 22);
            Button_Exit.Text = "$EXIT";
            Button_Exit.Click += Button_Exit_Click;
            // 
            // Button_Edit
            // 
            Button_Edit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            Button_Edit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { Button_SaveToXML, Button_LoadFromXML });
            Button_Edit.Image = (System.Drawing.Image)resources.GetObject("Button_Edit.Image");
            Button_Edit.ImageTransparentColor = System.Drawing.Color.Magenta;
            Button_Edit.Name = "Button_Edit";
            Button_Edit.Size = new System.Drawing.Size(49, 22);
            Button_Edit.Text = "$EDIT";
            // 
            // Button_SaveToXML
            // 
            Button_SaveToXML.Name = "Button_SaveToXML";
            Button_SaveToXML.Size = new System.Drawing.Size(180, 22);
            Button_SaveToXML.Text = "$SAVE_TO_XML";
            Button_SaveToXML.Click += OnSaveToXMLClicked;
            // 
            // Button_LoadFromXML
            // 
            Button_LoadFromXML.Name = "Button_LoadFromXML";
            Button_LoadFromXML.Size = new System.Drawing.Size(180, 22);
            Button_LoadFromXML.Text = "$LOAD_FROM_XML";
            Button_LoadFromXML.Click += OnLoadFromXMLClicked;
            // 
            // FileSaveDialog_SelectXML
            // 
            FileSaveDialog_SelectXML.Filter = "XML File|*.xml";
            // 
            // SpeechEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(933, 519);
            Controls.Add(Tool_Strip);
            Controls.Add(Grid_Speech);
            Controls.Add(TreeView_Speech);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "SpeechEditor";
            Text = "$SPEECH_EDITOR_TITLE";
            FormClosing += SpeechEditor_Closing;
            Tool_Strip.ResumeLayout(false);
            Tool_Strip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.PropertyGrid Grid_Speech;
        private System.Windows.Forms.OpenFileDialog FileOpenDialog_SelectXML;
        private System.Windows.Forms.ToolStrip Tool_Strip;
        private System.Windows.Forms.ToolStripDropDownButton Button_Fille;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reloadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private Controls.MTreeView TreeView_Speech;
        private System.Windows.Forms.ToolStripDropDownButton Button_File;
        private System.Windows.Forms.ToolStripMenuItem Button_Save;
        private System.Windows.Forms.ToolStripMenuItem Button_Reload;
        private System.Windows.Forms.ToolStripMenuItem Button_Exit;
        private System.Windows.Forms.ToolStripDropDownButton Button_Edit;
        private System.Windows.Forms.ToolStripMenuItem Button_SaveToXML;
        private System.Windows.Forms.ToolStripMenuItem Button_LoadFromXML;
        private System.Windows.Forms.SaveFileDialog FileSaveDialog_SelectXML;
    }
}