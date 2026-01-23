namespace Mafia2Tool
{
    partial class XMLEditor
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
            this.Tool_Strip = new System.Windows.Forms.ToolStrip();
            this.Button_File = new System.Windows.Forms.ToolStripDropDownButton();
            this.Button_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Reload = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.Button_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Edit = new System.Windows.Forms.ToolStripDropDownButton();
            this.Button_Find = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Replace = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_GoToLine = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.Button_Format = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_Validate = new System.Windows.Forms.ToolStripMenuItem();
            this.TextBox_XML = new System.Windows.Forms.RichTextBox();
            this.StatusStrip = new System.Windows.Forms.StatusStrip();
            this.StatusLabel_Position = new System.Windows.Forms.ToolStripStatusLabel();
            this.Tool_Strip.SuspendLayout();
            this.StatusStrip.SuspendLayout();
            this.SuspendLayout();
            //
            // Tool_Strip
            //
            this.Tool_Strip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Button_File,
            this.Button_Edit});
            this.Tool_Strip.Location = new System.Drawing.Point(0, 0);
            this.Tool_Strip.Name = "Tool_Strip";
            this.Tool_Strip.Size = new System.Drawing.Size(1000, 25);
            this.Tool_Strip.TabIndex = 0;
            this.Tool_Strip.Text = "toolStrip1";
            //
            // Button_File
            //
            this.Button_File.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Button_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Button_Save,
            this.Button_Reload,
            this.toolStripSeparator1,
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
            this.Button_Save.Size = new System.Drawing.Size(180, 22);
            this.Button_Save.Text = "$SAVE";
            this.Button_Save.Click += new System.EventHandler(this.Button_Save_Click);
            //
            // Button_Reload
            //
            this.Button_Reload.Name = "Button_Reload";
            this.Button_Reload.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.Button_Reload.Size = new System.Drawing.Size(180, 22);
            this.Button_Reload.Text = "$RELOAD";
            this.Button_Reload.Click += new System.EventHandler(this.Button_Reload_Click);
            //
            // toolStripSeparator1
            //
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            //
            // Button_Exit
            //
            this.Button_Exit.Name = "Button_Exit";
            this.Button_Exit.Size = new System.Drawing.Size(180, 22);
            this.Button_Exit.Text = "$EXIT";
            this.Button_Exit.Click += new System.EventHandler(this.Button_Exit_Click);
            //
            // Button_Edit
            //
            this.Button_Edit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Button_Edit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Button_Find,
            this.Button_Replace,
            this.Button_GoToLine,
            this.toolStripSeparator2,
            this.Button_Format,
            this.Button_Validate});
            this.Button_Edit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_Edit.Name = "Button_Edit";
            this.Button_Edit.Size = new System.Drawing.Size(49, 22);
            this.Button_Edit.Text = "$EDIT";
            //
            // Button_Find
            //
            this.Button_Find.Name = "Button_Find";
            this.Button_Find.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.Button_Find.Size = new System.Drawing.Size(220, 22);
            this.Button_Find.Text = "$FIND";
            this.Button_Find.Click += new System.EventHandler(this.Button_Find_Click);
            //
            // Button_Replace
            //
            this.Button_Replace.Name = "Button_Replace";
            this.Button_Replace.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.Button_Replace.Size = new System.Drawing.Size(220, 22);
            this.Button_Replace.Text = "$REPLACE";
            this.Button_Replace.Click += new System.EventHandler(this.Button_Replace_Click);
            //
            // Button_GoToLine
            //
            this.Button_GoToLine.Name = "Button_GoToLine";
            this.Button_GoToLine.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.Button_GoToLine.Size = new System.Drawing.Size(220, 22);
            this.Button_GoToLine.Text = "$GO_TO_LINE";
            this.Button_GoToLine.Click += new System.EventHandler(this.Button_GoToLine_Click);
            //
            // toolStripSeparator2
            //
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(217, 6);
            //
            // Button_Format
            //
            this.Button_Format.Name = "Button_Format";
            this.Button_Format.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
            | System.Windows.Forms.Keys.F)));
            this.Button_Format.Size = new System.Drawing.Size(220, 22);
            this.Button_Format.Text = "$XML_FORMAT";
            this.Button_Format.Click += new System.EventHandler(this.Button_Format_Click);
            //
            // Button_Validate
            //
            this.Button_Validate.Name = "Button_Validate";
            this.Button_Validate.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.Button_Validate.Size = new System.Drawing.Size(220, 22);
            this.Button_Validate.Text = "$XML_VALIDATE";
            this.Button_Validate.Click += new System.EventHandler(this.Button_Validate_Click);
            //
            // TextBox_XML
            //
            this.TextBox_XML.AcceptsTab = true;
            this.TextBox_XML.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBox_XML.DetectUrls = false;
            this.TextBox_XML.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TextBox_XML.HideSelection = false;
            this.TextBox_XML.Location = new System.Drawing.Point(0, 28);
            this.TextBox_XML.Name = "TextBox_XML";
            this.TextBox_XML.Size = new System.Drawing.Size(1000, 522);
            this.TextBox_XML.TabIndex = 1;
            this.TextBox_XML.Text = "";
            this.TextBox_XML.WordWrap = false;
            this.TextBox_XML.TextChanged += new System.EventHandler(this.TextBox_XML_TextChanged);
            this.TextBox_XML.SelectionChanged += new System.EventHandler(this.TextBox_XML_SelectionChanged);
            //
            // StatusStrip
            //
            this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel_Position});
            this.StatusStrip.Location = new System.Drawing.Point(0, 553);
            this.StatusStrip.Name = "StatusStrip";
            this.StatusStrip.Size = new System.Drawing.Size(1000, 22);
            this.StatusStrip.TabIndex = 2;
            this.StatusStrip.Text = "statusStrip1";
            //
            // StatusLabel_Position
            //
            this.StatusLabel_Position.Name = "StatusLabel_Position";
            this.StatusLabel_Position.Size = new System.Drawing.Size(0, 17);
            //
            // XMLEditor
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 575);
            this.Controls.Add(this.StatusStrip);
            this.Controls.Add(this.TextBox_XML);
            this.Controls.Add(this.Tool_Strip);
            this.Name = "XMLEditor";
            this.Text = "$XML_EDITOR_TITLE";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.XMLEditor_FormClosing);
            this.Tool_Strip.ResumeLayout(false);
            this.Tool_Strip.PerformLayout();
            this.StatusStrip.ResumeLayout(false);
            this.StatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ToolStrip Tool_Strip;
        private System.Windows.Forms.ToolStripDropDownButton Button_File;
        private System.Windows.Forms.ToolStripMenuItem Button_Save;
        private System.Windows.Forms.ToolStripMenuItem Button_Reload;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem Button_Exit;
        private System.Windows.Forms.ToolStripDropDownButton Button_Edit;
        private System.Windows.Forms.ToolStripMenuItem Button_Find;
        private System.Windows.Forms.ToolStripMenuItem Button_Replace;
        private System.Windows.Forms.ToolStripMenuItem Button_GoToLine;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem Button_Format;
        private System.Windows.Forms.ToolStripMenuItem Button_Validate;
        private System.Windows.Forms.RichTextBox TextBox_XML;
        private System.Windows.Forms.StatusStrip StatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel_Position;
    }
}
