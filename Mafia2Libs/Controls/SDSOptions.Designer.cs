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
            components = new System.ComponentModel.Container();
            groupSDS = new System.Windows.Forms.GroupBox();
            Checkbox_EnableLuaHelper = new System.Windows.Forms.CheckBox();
            NumericUpDown_Ratio = new Utils.Extensions.MNumericUpDown();
            NumericBox_VBSize = new Utils.Extensions.MNumericUpDown();
            Label_VBSize = new System.Windows.Forms.Label();
            Label_VertexBufferSize = new System.Windows.Forms.Label();
            NumericBox_IBSize = new Utils.Extensions.MNumericUpDown();
            Label_IBSize = new System.Windows.Forms.Label();
            Label_IndexBufferSize = new System.Windows.Forms.Label();
            CheckBox_BackupSDS = new System.Windows.Forms.CheckBox();
            CheckBox_UseOodle = new System.Windows.Forms.CheckBox();
            CookCollisionsBox = new System.Windows.Forms.CheckBox();
            SDSToolFormat = new System.Windows.Forms.CheckBox();
            UnpackLUABox = new System.Windows.Forms.CheckBox();
            AddTimeDateBackupsBox = new System.Windows.Forms.CheckBox();
            M2Label = new System.Windows.Forms.Label();
            ToolTips = new System.Windows.Forms.ToolTip(components);
            groupSDS.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)NumericUpDown_Ratio).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NumericBox_VBSize).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NumericBox_IBSize).BeginInit();
            SuspendLayout();
            // 
            // groupSDS
            // 
            groupSDS.AutoSize = true;
            groupSDS.Controls.Add(Checkbox_EnableLuaHelper);
            groupSDS.Controls.Add(NumericUpDown_Ratio);
            groupSDS.Controls.Add(NumericBox_VBSize);
            groupSDS.Controls.Add(Label_VBSize);
            groupSDS.Controls.Add(Label_VertexBufferSize);
            groupSDS.Controls.Add(NumericBox_IBSize);
            groupSDS.Controls.Add(Label_IBSize);
            groupSDS.Controls.Add(Label_IndexBufferSize);
            groupSDS.Controls.Add(CheckBox_BackupSDS);
            groupSDS.Controls.Add(CheckBox_UseOodle);
            groupSDS.Controls.Add(CookCollisionsBox);
            groupSDS.Controls.Add(SDSToolFormat);
            groupSDS.Controls.Add(UnpackLUABox);
            groupSDS.Controls.Add(AddTimeDateBackupsBox);
            groupSDS.Controls.Add(M2Label);
            groupSDS.Dock = System.Windows.Forms.DockStyle.Fill;
            groupSDS.Location = new System.Drawing.Point(0, 0);
            groupSDS.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupSDS.Name = "groupSDS";
            groupSDS.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupSDS.Size = new System.Drawing.Size(393, 272);
            groupSDS.TabIndex = 0;
            groupSDS.TabStop = false;
            groupSDS.Text = "SDS Options";
            // 
            // Checkbox_EnableLuaHelper
            // 
            Checkbox_EnableLuaHelper.AutoSize = true;
            Checkbox_EnableLuaHelper.Location = new System.Drawing.Point(260, 70);
            Checkbox_EnableLuaHelper.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Checkbox_EnableLuaHelper.Name = "Checkbox_EnableLuaHelper";
            Checkbox_EnableLuaHelper.Size = new System.Drawing.Size(163, 19);
            Checkbox_EnableLuaHelper.TabIndex = 18;
            Checkbox_EnableLuaHelper.Text = "Lua Decompile Assistance";
            Checkbox_EnableLuaHelper.UseVisualStyleBackColor = true;
            Checkbox_EnableLuaHelper.CheckedChanged += Checkbox_EnableLuaHelper_CheckedChanged;
            // 
            // NumericUpDown_Ratio
            // 
            NumericUpDown_Ratio.DecimalPlaces = 3;
            NumericUpDown_Ratio.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            NumericUpDown_Ratio.Location = new System.Drawing.Point(7, 41);
            NumericUpDown_Ratio.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            NumericUpDown_Ratio.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            NumericUpDown_Ratio.Name = "NumericUpDown_Ratio";
            NumericUpDown_Ratio.Size = new System.Drawing.Size(113, 23);
            NumericUpDown_Ratio.TabIndex = 17;
            NumericUpDown_Ratio.ValueChanged += NumericUpDown_Ratio_ValueChanged;
            // 
            // NumericBox_VBSize
            // 
            NumericBox_VBSize.Location = new System.Drawing.Point(191, 230);
            NumericBox_VBSize.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            NumericBox_VBSize.Maximum = new decimal(new int[] { 99999999, 0, 0, 0 });
            NumericBox_VBSize.Name = "NumericBox_VBSize";
            NumericBox_VBSize.Size = new System.Drawing.Size(128, 23);
            NumericBox_VBSize.TabIndex = 16;
            NumericBox_VBSize.ValueChanged += NumericBox_VBSize_ValueChanged;
            // 
            // Label_VBSize
            // 
            Label_VBSize.AutoSize = true;
            Label_VBSize.Location = new System.Drawing.Point(327, 232);
            Label_VBSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_VBSize.Name = "Label_VBSize";
            Label_VBSize.Size = new System.Drawing.Size(35, 15);
            Label_VBSize.TabIndex = 15;
            Label_VBSize.Text = "$SIZE";
            // 
            // Label_VertexBufferSize
            // 
            Label_VertexBufferSize.AutoSize = true;
            Label_VertexBufferSize.Location = new System.Drawing.Point(7, 232);
            Label_VertexBufferSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_VertexBufferSize.Name = "Label_VertexBufferSize";
            Label_VertexBufferSize.Size = new System.Drawing.Size(148, 15);
            Label_VertexBufferSize.TabIndex = 14;
            Label_VertexBufferSize.Text = "$VERTEX_SIZE_PER_BUFFER";
            // 
            // NumericBox_IBSize
            // 
            NumericBox_IBSize.Location = new System.Drawing.Point(191, 201);
            NumericBox_IBSize.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            NumericBox_IBSize.Maximum = new decimal(new int[] { 99999999, 0, 0, 0 });
            NumericBox_IBSize.Name = "NumericBox_IBSize";
            NumericBox_IBSize.Size = new System.Drawing.Size(128, 23);
            NumericBox_IBSize.TabIndex = 13;
            NumericBox_IBSize.ValueChanged += NumericBox_IBSize_ValueChanged;
            // 
            // Label_IBSize
            // 
            Label_IBSize.AutoSize = true;
            Label_IBSize.Location = new System.Drawing.Point(327, 203);
            Label_IBSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_IBSize.Name = "Label_IBSize";
            Label_IBSize.Size = new System.Drawing.Size(35, 15);
            Label_IBSize.TabIndex = 12;
            Label_IBSize.Text = "$SIZE";
            // 
            // Label_IndexBufferSize
            // 
            Label_IndexBufferSize.AutoSize = true;
            Label_IndexBufferSize.Location = new System.Drawing.Point(7, 203);
            Label_IndexBufferSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_IndexBufferSize.Name = "Label_IndexBufferSize";
            Label_IndexBufferSize.Size = new System.Drawing.Size(143, 15);
            Label_IndexBufferSize.TabIndex = 11;
            Label_IndexBufferSize.Text = "$INDEX_SIZE_PER_BUFFER";
            // 
            // CheckBox_BackupSDS
            // 
            CheckBox_BackupSDS.AutoSize = true;
            CheckBox_BackupSDS.Location = new System.Drawing.Point(7, 70);
            CheckBox_BackupSDS.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CheckBox_BackupSDS.Name = "CheckBox_BackupSDS";
            CheckBox_BackupSDS.Size = new System.Drawing.Size(140, 19);
            CheckBox_BackupSDS.TabIndex = 9;
            CheckBox_BackupSDS.Text = "$BACKUP_SDS_LABEL";
            CheckBox_BackupSDS.UseVisualStyleBackColor = true;
            CheckBox_BackupSDS.CheckedChanged += CheckBox_BackupSDS_CheckedChanged;
            // 
            // CheckBox_UseOodle
            // 
            CheckBox_UseOodle.AutoSize = true;
            CheckBox_UseOodle.Location = new System.Drawing.Point(126, 43);
            CheckBox_UseOodle.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CheckBox_UseOodle.Name = "CheckBox_UseOodle";
            CheckBox_UseOodle.Size = new System.Drawing.Size(80, 19);
            CheckBox_UseOodle.TabIndex = 8;
            CheckBox_UseOodle.Text = "Use Oodle";
            CheckBox_UseOodle.UseVisualStyleBackColor = true;
            CheckBox_UseOodle.CheckedChanged += CheckBox_UseOodle_CheckedChanged;
            // 
            // CookCollisionsBox
            // 
            CookCollisionsBox.AutoSize = true;
            CookCollisionsBox.Location = new System.Drawing.Point(7, 175);
            CookCollisionsBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CookCollisionsBox.Name = "CookCollisionsBox";
            CookCollisionsBox.Size = new System.Drawing.Size(129, 19);
            CookCollisionsBox.TabIndex = 7;
            CookCollisionsBox.Text = "$COOK_COLLISION";
            CookCollisionsBox.UseVisualStyleBackColor = true;
            CookCollisionsBox.CheckedChanged += CookCollisionsBox_CheckedChanged;
            // 
            // SDSToolFormat
            // 
            SDSToolFormat.AutoSize = true;
            SDSToolFormat.Location = new System.Drawing.Point(7, 150);
            SDSToolFormat.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            SDSToolFormat.Name = "SDSToolFormat";
            SDSToolFormat.Size = new System.Drawing.Size(134, 19);
            SDSToolFormat.TabIndex = 6;
            SDSToolFormat.Text = "Use SDS Tool Format";
            SDSToolFormat.UseVisualStyleBackColor = true;
            SDSToolFormat.CheckedChanged += SDSToolFormat_CheckedChanged;
            // 
            // UnpackLUABox
            // 
            UnpackLUABox.AutoSize = true;
            UnpackLUABox.Location = new System.Drawing.Point(7, 123);
            UnpackLUABox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            UnpackLUABox.Name = "UnpackLUABox";
            UnpackLUABox.Size = new System.Drawing.Size(199, 19);
            UnpackLUABox.TabIndex = 5;
            UnpackLUABox.Text = "Decompile LUA when unpacking";
            UnpackLUABox.UseVisualStyleBackColor = true;
            UnpackLUABox.CheckedChanged += UnpackLUABox_CheckedChanged;
            // 
            // AddTimeDateBackupsBox
            // 
            AddTimeDateBackupsBox.AutoSize = true;
            AddTimeDateBackupsBox.Location = new System.Drawing.Point(7, 97);
            AddTimeDateBackupsBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            AddTimeDateBackupsBox.Name = "AddTimeDateBackupsBox";
            AddTimeDateBackupsBox.Size = new System.Drawing.Size(188, 19);
            AddTimeDateBackupsBox.TabIndex = 4;
            AddTimeDateBackupsBox.Text = "Add Time and Date to Backups";
            AddTimeDateBackupsBox.UseVisualStyleBackColor = true;
            AddTimeDateBackupsBox.CheckedChanged += AddTimeDateBackupsBox_CheckedChanged;
            // 
            // M2Label
            // 
            M2Label.AutoSize = true;
            M2Label.Location = new System.Drawing.Point(7, 18);
            M2Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            M2Label.Name = "M2Label";
            M2Label.Size = new System.Drawing.Size(156, 15);
            M2Label.TabIndex = 2;
            M2Label.Text = "$SDS_COMPRESSION_RATIO";
            // 
            // ToolTips
            // 
            ToolTips.AutomaticDelay = 100;
            // 
            // SDSOptions
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(groupSDS);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "SDSOptions";
            Size = new System.Drawing.Size(393, 272);
            groupSDS.ResumeLayout(false);
            groupSDS.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)NumericUpDown_Ratio).EndInit();
            ((System.ComponentModel.ISupportInitialize)NumericBox_VBSize).EndInit();
            ((System.ComponentModel.ISupportInitialize)NumericBox_IBSize).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.GroupBox groupSDS;
        private System.Windows.Forms.Label M2Label;
        private System.Windows.Forms.CheckBox UnpackLUABox;
        private System.Windows.Forms.CheckBox AddTimeDateBackupsBox;
        private System.Windows.Forms.CheckBox SDSToolFormat;
        private System.Windows.Forms.CheckBox CookCollisionsBox;
        private System.Windows.Forms.CheckBox CheckBox_UseOodle;
        private System.Windows.Forms.CheckBox CheckBox_BackupSDS;
        private System.Windows.Forms.Label Label_IndexBufferSize;
        private System.Windows.Forms.Label Label_IBSize;
        private Utils.Extensions.MNumericUpDown NumericBox_IBSize;
        private Utils.Extensions.MNumericUpDown NumericBox_VBSize;
        private System.Windows.Forms.Label Label_VBSize;
        private System.Windows.Forms.Label Label_VertexBufferSize;
        private Utils.Extensions.MNumericUpDown NumericUpDown_Ratio;
        private System.Windows.Forms.ToolTip ToolTips;
        private System.Windows.Forms.CheckBox Checkbox_EnableLuaHelper;
    }
}
