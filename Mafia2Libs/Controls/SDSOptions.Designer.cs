namespace Forms.OptionControls
{
    partial class SDSOptions
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
            this.groupSDS = new System.Windows.Forms.GroupBox();
            this.CompressionDropdownBox = new System.Windows.Forms.ComboBox();
            this.M2Label = new System.Windows.Forms.Label();
            this.AddTimeDateBackupsBox = new System.Windows.Forms.CheckBox();
            this.UnpackLUABox = new System.Windows.Forms.CheckBox();
            this.groupSDS.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupSDS
            // 
            this.groupSDS.AutoSize = true;
            this.groupSDS.Controls.Add(this.UnpackLUABox);
            this.groupSDS.Controls.Add(this.AddTimeDateBackupsBox);
            this.groupSDS.Controls.Add(this.CompressionDropdownBox);
            this.groupSDS.Controls.Add(this.M2Label);
            this.groupSDS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupSDS.Location = new System.Drawing.Point(0, 0);
            this.groupSDS.Name = "groupSDS";
            this.groupSDS.Size = new System.Drawing.Size(213, 150);
            this.groupSDS.TabIndex = 0;
            this.groupSDS.TabStop = false;
            this.groupSDS.Text = "SDS Options";
            // 
            // CompressionDropdownBox
            // 
            this.CompressionDropdownBox.FormattingEnabled = true;
            this.CompressionDropdownBox.Items.AddRange(new object[] {
            "Uncompressed",
            "Compression"});
            this.CompressionDropdownBox.Location = new System.Drawing.Point(9, 32);
            this.CompressionDropdownBox.Name = "CompressionDropdownBox";
            this.CompressionDropdownBox.Size = new System.Drawing.Size(121, 21);
            this.CompressionDropdownBox.TabIndex = 3;
            this.CompressionDropdownBox.SelectedIndexChanged += new System.EventHandler(this.SDSCompress_IndexChanged);
            // 
            // M2Label
            // 
            this.M2Label.AutoSize = true;
            this.M2Label.Location = new System.Drawing.Point(6, 16);
            this.M2Label.Name = "M2Label";
            this.M2Label.Size = new System.Drawing.Size(154, 13);
            this.M2Label.TabIndex = 2;
            this.M2Label.Text = "$SDS_COMPRESSION_TYPE";
            // 
            // AddTimeDateBackupsBox
            // 
            this.AddTimeDateBackupsBox.AutoSize = true;
            this.AddTimeDateBackupsBox.Location = new System.Drawing.Point(9, 59);
            this.AddTimeDateBackupsBox.Name = "AddTimeDateBackupsBox";
            this.AddTimeDateBackupsBox.Size = new System.Drawing.Size(175, 17);
            this.AddTimeDateBackupsBox.TabIndex = 4;
            this.AddTimeDateBackupsBox.Text = "Add Time and Date to Backups";
            this.AddTimeDateBackupsBox.UseVisualStyleBackColor = true;
            this.AddTimeDateBackupsBox.CheckedChanged += new System.EventHandler(this.AddTimeDateBackupsBox_CheckedChanged);
            // 
            // UnpackLUABox
            // 
            this.UnpackLUABox.AutoSize = true;
            this.UnpackLUABox.Location = new System.Drawing.Point(9, 82);
            this.UnpackLUABox.Name = "UnpackLUABox";
            this.UnpackLUABox.Size = new System.Drawing.Size(182, 17);
            this.UnpackLUABox.TabIndex = 5;
            this.UnpackLUABox.Text = "Decompile LUA when unpacking";
            this.UnpackLUABox.UseVisualStyleBackColor = true;
            this.UnpackLUABox.CheckedChanged += new System.EventHandler(this.UnpackLUABox_CheckedChanged);
            // 
            // SDSOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupSDS);
            this.Name = "SDSOptions";
            this.Size = new System.Drawing.Size(213, 150);
            this.groupSDS.ResumeLayout(false);
            this.groupSDS.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupSDS;
        private System.Windows.Forms.ComboBox CompressionDropdownBox;
        private System.Windows.Forms.Label M2Label;
        private System.Windows.Forms.CheckBox UnpackLUABox;
        private System.Windows.Forms.CheckBox AddTimeDateBackupsBox;
    }
}
