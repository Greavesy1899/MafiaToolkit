using Utils.Lang;

namespace Mafia2Tool.OptionControls
{
    partial class ModelOptions
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupGeneral = new System.Windows.Forms.GroupBox();
            this.exportPathBrowser = new System.Windows.Forms.Button();
            this.exportPathTextBox = new System.Windows.Forms.TextBox();
            this.exportPathLabel = new System.Windows.Forms.Label();
            this.modelFormatDropdownBox = new System.Windows.Forms.ComboBox();
            this.M2Label = new System.Windows.Forms.Label();
            this.ExportPathButton = new System.Windows.Forms.FolderBrowserDialog();
            this.groupGeneral.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupGeneral
            // 
            this.groupGeneral.Controls.Add(this.exportPathBrowser);
            this.groupGeneral.Controls.Add(this.exportPathTextBox);
            this.groupGeneral.Controls.Add(this.exportPathLabel);
            this.groupGeneral.Controls.Add(this.modelFormatDropdownBox);
            this.groupGeneral.Controls.Add(this.M2Label);
            this.groupGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupGeneral.Location = new System.Drawing.Point(0, 0);
            this.groupGeneral.Name = "groupGeneral";
            this.groupGeneral.Size = new System.Drawing.Size(390, 193);
            this.groupGeneral.TabIndex = 2;
            this.groupGeneral.TabStop = false;
            this.groupGeneral.Text = Language.GetString("$MODEL_EXPORTING");
            // 
            // exportPathBrowser
            // 
            this.exportPathBrowser.Location = new System.Drawing.Point(358, 82);
            this.exportPathBrowser.Name = "exportPathBrowser";
            this.exportPathBrowser.Size = new System.Drawing.Size(26, 20);
            this.exportPathBrowser.TabIndex = 4;
            this.exportPathBrowser.Text = "...";
            this.exportPathBrowser.UseVisualStyleBackColor = true;
            this.exportPathBrowser.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // exportPathTextBox
            // 
            this.exportPathTextBox.Location = new System.Drawing.Point(7, 82);
            this.exportPathTextBox.Name = "exportPathTextBox";
            this.exportPathTextBox.Size = new System.Drawing.Size(347, 20);
            this.exportPathTextBox.TabIndex = 3;
            this.exportPathTextBox.TextChanged += new System.EventHandler(this.ExportPath_TextChanged);
            // 
            // exportPathLabel
            // 
            this.exportPathLabel.AutoSize = true;
            this.exportPathLabel.Location = new System.Drawing.Point(4, 65);
            this.exportPathLabel.Name = "exportPathLabel";
            this.exportPathLabel.Size = new System.Drawing.Size(65, 13);
            this.exportPathLabel.TabIndex = 2;
            this.exportPathLabel.Text = Language.GetString("$EXPORT_PATH_TITLE");
            // 
            // modelFormatDropdownBox
            // 
            this.modelFormatDropdownBox.FormattingEnabled = true;
            this.modelFormatDropdownBox.Items.AddRange(new object[] {
            "FBX Ascii",
            "FBX Binary",
            "M2T"});
            this.modelFormatDropdownBox.Location = new System.Drawing.Point(7, 37);
            this.modelFormatDropdownBox.Name = "modelFormatDropdownBox";
            this.modelFormatDropdownBox.Size = new System.Drawing.Size(121, 21);
            this.modelFormatDropdownBox.TabIndex = 1;
            this.modelFormatDropdownBox.SelectedIndexChanged += new System.EventHandler(this.ExportModelFormat_IndexChanged);
            // 
            // M2Label
            // 
            this.M2Label.AutoSize = true;
            this.M2Label.Location = new System.Drawing.Point(4, 21);
            this.M2Label.Name = "M2Label";
            this.M2Label.Size = new System.Drawing.Size(107, 13);
            this.M2Label.TabIndex = 0;
            this.M2Label.Text = Language.GetString("$EXPORT_MODELTYPE_TITLE");
            // 
            // ExportPathButton
            // 
            this.ExportPathButton.Description = Language.GetString("$EXPORT_PATH_DESC");
            // 
            // ModelOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupGeneral);
            this.Name = "ModelOptions";
            this.Size = new System.Drawing.Size(390, 193);
            this.groupGeneral.ResumeLayout(false);
            this.groupGeneral.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupGeneral;
        private System.Windows.Forms.Label M2Label;
        private System.Windows.Forms.ComboBox modelFormatDropdownBox;
        private System.Windows.Forms.Label exportPathLabel;
        private System.Windows.Forms.TextBox exportPathTextBox;
        private System.Windows.Forms.Button exportPathBrowser;
        private System.Windows.Forms.FolderBrowserDialog ExportPathButton;
    }
}
