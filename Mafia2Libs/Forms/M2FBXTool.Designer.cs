namespace Mafia2Tool.Forms
{
    partial class M2FBXTool
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(M2FBXTool));
            this.ImportBox = new System.Windows.Forms.TextBox();
            this.MeshBrowser = new System.Windows.Forms.OpenFileDialog();
            this.M2FBXGroup = new System.Windows.Forms.GroupBox();
            this.ImportButton = new System.Windows.Forms.Button();
            this.ExportButton = new System.Windows.Forms.Button();
            this.M2FBXLabel = new System.Windows.Forms.Label();
            this.ConvertButton = new System.Windows.Forms.Button();
            this.ExportBox = new System.Windows.Forms.TextBox();
            this.ExportLabel = new System.Windows.Forms.Label();
            this.ImportLabel = new System.Windows.Forms.Label();
            this.M2FBXGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // ImportBox
            // 
            this.ImportBox.Location = new System.Drawing.Point(105, 72);
            this.ImportBox.Name = "ImportBox";
            this.ImportBox.Size = new System.Drawing.Size(311, 20);
            this.ImportBox.TabIndex = 3;
            // 
            // MeshBrowser
            // 
            this.MeshBrowser.Filter = "Meshes|*.m2t|FBX|*.fbx";
            // 
            // M2FBXGroup
            // 
            this.M2FBXGroup.AutoSize = true;
            this.M2FBXGroup.Controls.Add(this.ImportButton);
            this.M2FBXGroup.Controls.Add(this.ExportButton);
            this.M2FBXGroup.Controls.Add(this.M2FBXLabel);
            this.M2FBXGroup.Controls.Add(this.ConvertButton);
            this.M2FBXGroup.Controls.Add(this.ExportBox);
            this.M2FBXGroup.Controls.Add(this.ExportLabel);
            this.M2FBXGroup.Controls.Add(this.ImportLabel);
            this.M2FBXGroup.Controls.Add(this.ImportBox);
            this.M2FBXGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.M2FBXGroup.Location = new System.Drawing.Point(0, 0);
            this.M2FBXGroup.Name = "M2FBXGroup";
            this.M2FBXGroup.Size = new System.Drawing.Size(428, 188);
            this.M2FBXGroup.TabIndex = 4;
            this.M2FBXGroup.TabStop = false;
            this.M2FBXGroup.Text = "M2 FBX ";
            // 
            // ImportButton
            // 
            this.ImportButton.Location = new System.Drawing.Point(9, 72);
            this.ImportButton.Name = "ImportButton";
            this.ImportButton.Size = new System.Drawing.Size(93, 20);
            this.ImportButton.TabIndex = 9;
            this.ImportButton.Text = "Select Mesh";
            this.ImportButton.UseVisualStyleBackColor = true;
            this.ImportButton.Click += new System.EventHandler(this.ImportButton_Click);
            // 
            // ExportButton
            // 
            this.ExportButton.Location = new System.Drawing.Point(9, 122);
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.Size = new System.Drawing.Size(93, 20);
            this.ExportButton.TabIndex = 8;
            this.ExportButton.Text = "Select Mesh";
            this.ExportButton.UseVisualStyleBackColor = true;
            this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // M2FBXLabel
            // 
            this.M2FBXLabel.Location = new System.Drawing.Point(6, 21);
            this.M2FBXLabel.Name = "M2FBXLabel";
            this.M2FBXLabel.Size = new System.Drawing.Size(410, 35);
            this.M2FBXLabel.TabIndex = 7;
            this.M2FBXLabel.Text = "$M2FBX_INSTRUCTIONS";
            // 
            // ConvertButton
            // 
            this.ConvertButton.Location = new System.Drawing.Point(9, 158);
            this.ConvertButton.Name = "ConvertButton";
            this.ConvertButton.Size = new System.Drawing.Size(93, 23);
            this.ConvertButton.TabIndex = 6;
            this.ConvertButton.Text = "Convert";
            this.ConvertButton.UseVisualStyleBackColor = true;
            this.ConvertButton.Click += new System.EventHandler(this.ConvertButton_Click);
            // 
            // ExportBox
            // 
            this.ExportBox.Location = new System.Drawing.Point(105, 122);
            this.ExportBox.Name = "ExportBox";
            this.ExportBox.Size = new System.Drawing.Size(311, 20);
            this.ExportBox.TabIndex = 5;
            // 
            // ExportLabel
            // 
            this.ExportLabel.AutoSize = true;
            this.ExportLabel.Location = new System.Drawing.Point(6, 106);
            this.ExportLabel.Name = "ExportLabel";
            this.ExportLabel.Size = new System.Drawing.Size(96, 13);
            this.ExportLabel.TabIndex = 4;
            this.ExportLabel.Text = "$EXPORT_LABEL";
            // 
            // ImportLabel
            // 
            this.ImportLabel.AutoSize = true;
            this.ImportLabel.Location = new System.Drawing.Point(6, 56);
            this.ImportLabel.Name = "ImportLabel";
            this.ImportLabel.Size = new System.Drawing.Size(94, 13);
            this.ImportLabel.TabIndex = 2;
            this.ImportLabel.Text = "$IMPORT_LABEL";
            // 
            // M2FBXTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 188);
            this.Controls.Add(this.M2FBXGroup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "M2FBXTool";
            this.Text = "M2FBXTool";
            this.M2FBXGroup.ResumeLayout(false);
            this.M2FBXGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ImportBox;
        private System.Windows.Forms.OpenFileDialog MeshBrowser;
        private System.Windows.Forms.GroupBox M2FBXGroup;
        private System.Windows.Forms.TextBox ExportBox;
        private System.Windows.Forms.Label ExportLabel;
        private System.Windows.Forms.Label ImportLabel;
        private System.Windows.Forms.Label M2FBXLabel;
        private System.Windows.Forms.Button ConvertButton;
        private System.Windows.Forms.Button ImportButton;
        private System.Windows.Forms.Button ExportButton;
    }
}