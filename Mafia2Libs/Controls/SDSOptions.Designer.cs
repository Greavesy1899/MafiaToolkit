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
            this.NumericBox_IBSize = new System.Windows.Forms.NumericUpDown();
            this.Label_IBSize = new System.Windows.Forms.Label();
            this.Label_IndexBufferSize = new System.Windows.Forms.Label();
            this.CheckBox_BackupSDS = new System.Windows.Forms.CheckBox();
            this.CheckBox_UseOodle = new System.Windows.Forms.CheckBox();
            this.CookCollisionsBox = new System.Windows.Forms.CheckBox();
            this.SDSToolFormat = new System.Windows.Forms.CheckBox();
            this.UnpackLUABox = new System.Windows.Forms.CheckBox();
            this.AddTimeDateBackupsBox = new System.Windows.Forms.CheckBox();
            this.CompressionDropdownBox = new System.Windows.Forms.ComboBox();
            this.M2Label = new System.Windows.Forms.Label();
            this.NumericBox_VBSize = new System.Windows.Forms.NumericUpDown();
            this.Label_VBSize = new System.Windows.Forms.Label();
            this.Label_VertexBufferSize = new System.Windows.Forms.Label();
            this.groupSDS.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericBox_IBSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericBox_VBSize)).BeginInit();
            this.SuspendLayout();
            // 
            // groupSDS
            // 
            this.groupSDS.AutoSize = true;
            this.groupSDS.Controls.Add(this.NumericBox_VBSize);
            this.groupSDS.Controls.Add(this.Label_VBSize);
            this.groupSDS.Controls.Add(this.Label_VertexBufferSize);
            this.groupSDS.Controls.Add(this.NumericBox_IBSize);
            this.groupSDS.Controls.Add(this.Label_IBSize);
            this.groupSDS.Controls.Add(this.Label_IndexBufferSize);
            this.groupSDS.Controls.Add(this.CheckBox_BackupSDS);
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
            this.groupSDS.Size = new System.Drawing.Size(337, 236);
            this.groupSDS.TabIndex = 0;
            this.groupSDS.TabStop = false;
            this.groupSDS.Text = "SDS Options";
            // 
            // NumericBox_IBSize
            // 
            this.NumericBox_IBSize.Location = new System.Drawing.Point(164, 174);
            this.NumericBox_IBSize.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.NumericBox_IBSize.Name = "NumericBox_IBSize";
            this.NumericBox_IBSize.Size = new System.Drawing.Size(110, 20);
            this.NumericBox_IBSize.TabIndex = 13;
            this.NumericBox_IBSize.ValueChanged += new System.EventHandler(this.NumericBox_IBSize_ValueChanged);
            // 
            // Label_IBSize
            // 
            this.Label_IBSize.AutoSize = true;
            this.Label_IBSize.Location = new System.Drawing.Point(280, 176);
            this.Label_IBSize.Name = "Label_IBSize";
            this.Label_IBSize.Size = new System.Drawing.Size(37, 13);
            this.Label_IBSize.TabIndex = 12;
            this.Label_IBSize.Text = "$SIZE";
            // 
            // Label_IndexBufferSize
            // 
            this.Label_IndexBufferSize.AutoSize = true;
            this.Label_IndexBufferSize.Location = new System.Drawing.Point(6, 176);
            this.Label_IndexBufferSize.Name = "Label_IndexBufferSize";
            this.Label_IndexBufferSize.Size = new System.Drawing.Size(152, 13);
            this.Label_IndexBufferSize.TabIndex = 11;
            this.Label_IndexBufferSize.Text = "$INDEX_SIZE_PER_BUFFER";
            // 
            // CheckBox_BackupSDS
            // 
            this.CheckBox_BackupSDS.AutoSize = true;
            this.CheckBox_BackupSDS.Location = new System.Drawing.Point(6, 61);
            this.CheckBox_BackupSDS.Name = "CheckBox_BackupSDS";
            this.CheckBox_BackupSDS.Size = new System.Drawing.Size(142, 17);
            this.CheckBox_BackupSDS.TabIndex = 9;
            this.CheckBox_BackupSDS.Text = "$BACKUP_SDS_LABEL";
            this.CheckBox_BackupSDS.UseVisualStyleBackColor = true;
            this.CheckBox_BackupSDS.CheckedChanged += new System.EventHandler(this.CheckBox_BackupSDS_CheckedChanged);
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
            this.CookCollisionsBox.Location = new System.Drawing.Point(6, 152);
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
            this.SDSToolFormat.Location = new System.Drawing.Point(6, 130);
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
            this.UnpackLUABox.Location = new System.Drawing.Point(6, 107);
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
            this.AddTimeDateBackupsBox.Location = new System.Drawing.Point(6, 84);
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
            // NumericBox_VBSize
            // 
            this.NumericBox_VBSize.Location = new System.Drawing.Point(164, 199);
            this.NumericBox_VBSize.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.NumericBox_VBSize.Name = "NumericBox_VBSize";
            this.NumericBox_VBSize.Size = new System.Drawing.Size(110, 20);
            this.NumericBox_VBSize.TabIndex = 16;
            this.NumericBox_VBSize.ValueChanged += new System.EventHandler(this.NumericBox_VBSize_ValueChanged);
            // 
            // Label_VBSize
            // 
            this.Label_VBSize.AutoSize = true;
            this.Label_VBSize.Location = new System.Drawing.Point(280, 201);
            this.Label_VBSize.Name = "Label_VBSize";
            this.Label_VBSize.Size = new System.Drawing.Size(37, 13);
            this.Label_VBSize.TabIndex = 15;
            this.Label_VBSize.Text = "$SIZE";
            // 
            // Label_VertexBufferSize
            // 
            this.Label_VertexBufferSize.AutoSize = true;
            this.Label_VertexBufferSize.Location = new System.Drawing.Point(6, 201);
            this.Label_VertexBufferSize.Name = "Label_VertexBufferSize";
            this.Label_VertexBufferSize.Size = new System.Drawing.Size(162, 13);
            this.Label_VertexBufferSize.TabIndex = 14;
            this.Label_VertexBufferSize.Text = "$VERTEX_SIZE_PER_BUFFER";
            // 
            // SDSOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupSDS);
            this.Name = "SDSOptions";
            this.Size = new System.Drawing.Size(337, 236);
            this.groupSDS.ResumeLayout(false);
            this.groupSDS.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericBox_IBSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericBox_VBSize)).EndInit();
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
        private System.Windows.Forms.CheckBox CheckBox_BackupSDS;
        private System.Windows.Forms.Label Label_IndexBufferSize;
        private System.Windows.Forms.Label Label_IBSize;
        private System.Windows.Forms.NumericUpDown NumericBox_IBSize;
        private System.Windows.Forms.NumericUpDown NumericBox_VBSize;
        private System.Windows.Forms.Label Label_VBSize;
        private System.Windows.Forms.Label Label_VertexBufferSize;
    }
}
