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
            this.CheckBox_UseOodle = new System.Windows.Forms.CheckBox();
            this.CookCollisionsBox = new System.Windows.Forms.CheckBox();
            this.SDSToolFormat = new System.Windows.Forms.CheckBox();
            this.UnpackLUABox = new System.Windows.Forms.CheckBox();
            this.AddTimeDateBackupsBox = new System.Windows.Forms.CheckBox();
            this.CompressionDropdownBox = new System.Windows.Forms.ComboBox();
            this.M2Label = new System.Windows.Forms.Label();
            this.groupSDS.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupSDS
            // 
            this.groupSDS.AutoSize = true;
            this.groupSDS.Controls.Add(this.CheckBox_UseOodle);
            this.groupSDS.Controls.Add(this.CookCollisionsBox);
            this.groupSDS.Controls.Add(this.SDSToolFormat);
            this.groupSDS.Controls.Add(this.UnpackLUABox);
            this.groupSDS.Controls.Add(this.AddTimeDateBackupsBox);
            this.groupSDS.Controls.Add(this.CompressionDropdownBox);
            this.groupSDS.Controls.Add(this.M2Label);
            this.groupSDS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupSDS.Location = new System.Drawing.Point(0, 0);
            this.groupSDS.Name = "groupSDS";
            this.groupSDS.Size = new System.Drawing.Size(300, 150);
            this.groupSDS.TabIndex = 0;
            this.groupSDS.TabStop = false;
            this.groupSDS.Text = "SDS Options";
            // 
            // CheckBox_UseOodle
            // 
            this.CheckBox_UseOodle.AutoSize = true;
            this.CheckBox_UseOodle.Location = new System.Drawing.Point(164, 34);
            this.CheckBox_UseOodle.Name = "CheckBox_UseOodle";
            this.CheckBox_UseOodle.Size = new System.Drawing.Size(76, 17);
            this.CheckBox_UseOodle.TabIndex = 8;
            this.CheckBox_UseOodle.Text = "Use Oodle";
            this.CheckBox_UseOodle.UseVisualStyleBackColor = true;
            this.CheckBox_UseOodle.CheckedChanged += new System.EventHandler(this.CheckBox_UseOodle_CheckedChanged);
            // 
            // CookCollisionsBox
            // 
            this.CookCollisionsBox.AutoSize = true;
            this.CookCollisionsBox.Location = new System.Drawing.Point(9, 127);
            this.CookCollisionsBox.Name = "CookCollisionsBox";
            this.CookCollisionsBox.Size = new System.Drawing.Size(124, 17);
            this.CookCollisionsBox.TabIndex = 7;
            this.CookCollisionsBox.Text = "$COOK_COLLISION";
            this.CookCollisionsBox.UseVisualStyleBackColor = true;
            this.CookCollisionsBox.CheckedChanged += new System.EventHandler(this.CookCollisionsBox_CheckedChanged);
            // 
            // SDSToolFormat
            // 
            this.SDSToolFormat.AutoSize = true;
            this.SDSToolFormat.Location = new System.Drawing.Point(9, 105);
            this.SDSToolFormat.Name = "SDSToolFormat";
            this.SDSToolFormat.Size = new System.Drawing.Size(129, 17);
            this.SDSToolFormat.TabIndex = 6;
            this.SDSToolFormat.Text = "Use SDS Tool Format";
            this.SDSToolFormat.UseVisualStyleBackColor = true;
            this.SDSToolFormat.CheckedChanged += new System.EventHandler(this.SDSToolFormat_CheckedChanged);
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
            // SDSOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupSDS);
            this.Name = "SDSOptions";
            this.Size = new System.Drawing.Size(300, 150);
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
        private System.Windows.Forms.CheckBox SDSToolFormat;
        private System.Windows.Forms.CheckBox CookCollisionsBox;
        private System.Windows.Forms.CheckBox CheckBox_UseOodle;
    }
}
