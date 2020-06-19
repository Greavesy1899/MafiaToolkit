namespace Forms.EditorControls
{
    partial class ControlOptionFrameAdd
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
            this.Group_General = new System.Windows.Forms.GroupBox();
            this.Label_Type = new System.Windows.Forms.Label();
            this.ComboBox_Type = new System.Windows.Forms.ComboBox();
            this.Label_AddToNameTable = new System.Windows.Forms.Label();
            this.CheckBox_AddToNameTable = new System.Windows.Forms.CheckBox();
            this.Group_General.SuspendLayout();
            this.SuspendLayout();
            // 
            // Group_General
            // 
            this.Group_General.AutoSize = true;
            this.Group_General.Controls.Add(this.CheckBox_AddToNameTable);
            this.Group_General.Controls.Add(this.Label_AddToNameTable);
            this.Group_General.Controls.Add(this.Label_Type);
            this.Group_General.Controls.Add(this.ComboBox_Type);
            this.Group_General.Location = new System.Drawing.Point(0, 0);
            this.Group_General.Name = "Group_General";
            this.Group_General.Size = new System.Drawing.Size(340, 84);
            this.Group_General.TabIndex = 2;
            this.Group_General.TabStop = false;
            this.Group_General.Text = "$GENERAL";
            // 
            // Label_Type
            // 
            this.Label_Type.AutoSize = true;
            this.Label_Type.Location = new System.Drawing.Point(6, 27);
            this.Label_Type.Name = "Label_Type";
            this.Label_Type.Size = new System.Drawing.Size(84, 13);
            this.Label_Type.TabIndex = 1;
            this.Label_Type.Text = "$FRADD_TYPE";
            // 
            // ComboBox_Type
            // 
            this.ComboBox_Type.FormattingEnabled = true;
            this.ComboBox_Type.Items.AddRange(new object[] {
            "Single Mesh",
            "Frame",
            "Light",
            "Camera",
            "Component_U005",
            "Sector",
            "Dummy",
            "Deflector",
            "Area",
            "Target",
            "Model",
            "Collision"});
            this.ComboBox_Type.Location = new System.Drawing.Point(182, 24);
            this.ComboBox_Type.Name = "ComboBox_Type";
            this.ComboBox_Type.Size = new System.Drawing.Size(152, 21);
            this.ComboBox_Type.TabIndex = 0;
            // 
            // Label_AddToNameTable
            // 
            this.Label_AddToNameTable.AutoSize = true;
            this.Label_AddToNameTable.Location = new System.Drawing.Point(6, 52);
            this.Label_AddToNameTable.Name = "Label_AddToNameTable";
            this.Label_AddToNameTable.Size = new System.Drawing.Size(121, 13);
            this.Label_AddToNameTable.TabIndex = 2;
            this.Label_AddToNameTable.Text = "$FRADD_NAMETABLE";
            // 
            // CheckBox_AddToNameTable
            // 
            this.CheckBox_AddToNameTable.AutoSize = true;
            this.CheckBox_AddToNameTable.Location = new System.Drawing.Point(182, 51);
            this.CheckBox_AddToNameTable.Name = "CheckBox_AddToNameTable";
            this.CheckBox_AddToNameTable.Size = new System.Drawing.Size(15, 14);
            this.CheckBox_AddToNameTable.TabIndex = 3;
            this.CheckBox_AddToNameTable.UseVisualStyleBackColor = true;
            // 
            // FrameResourceAddOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Group_General);
            this.Name = "FrameResourceAddOption";
            this.Size = new System.Drawing.Size(340, 78);
            this.Group_General.ResumeLayout(false);
            this.Group_General.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox Group_General;
        private System.Windows.Forms.Label Label_Type;
        private System.Windows.Forms.ComboBox ComboBox_Type;
        private System.Windows.Forms.Label Label_AddToNameTable;
        private System.Windows.Forms.CheckBox CheckBox_AddToNameTable;
    }
}
