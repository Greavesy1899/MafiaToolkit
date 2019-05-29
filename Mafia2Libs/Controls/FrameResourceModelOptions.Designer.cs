namespace Mafia2Tool
{
    partial class FrameResourceModelOptions
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
            this.buttonContinue = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ImportNormalBox = new System.Windows.Forms.CheckBox();
            this.ImportUV1Box = new System.Windows.Forms.CheckBox();
            this.ImportUV2Box = new System.Windows.Forms.CheckBox();
            this.ImportUV7Box = new System.Windows.Forms.CheckBox();
            this.FlipUVBox = new System.Windows.Forms.CheckBox();
            this.ImportUV0Box = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // buttonContinue
            // 
            this.buttonContinue.Location = new System.Drawing.Point(259, 163);
            this.buttonContinue.Name = "buttonContinue";
            this.buttonContinue.Size = new System.Drawing.Size(93, 23);
            this.buttonContinue.TabIndex = 2;
            this.buttonContinue.Text = "$CONTINUE";
            this.buttonContinue.UseVisualStyleBackColor = true;
            this.buttonContinue.Click += new System.EventHandler(this.OnButtonClickContinue);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(16, 163);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(93, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "$CANCEL";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.OnButtonClickCancel);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(235, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Select what options you would like to import with";
            // 
            // ImportNormalBox
            // 
            this.ImportNormalBox.AutoSize = true;
            this.ImportNormalBox.Location = new System.Drawing.Point(16, 39);
            this.ImportNormalBox.Name = "ImportNormalBox";
            this.ImportNormalBox.Size = new System.Drawing.Size(96, 17);
            this.ImportNormalBox.TabIndex = 6;
            this.ImportNormalBox.Text = "Import Normals";
            this.ImportNormalBox.UseVisualStyleBackColor = true;
            // 
            // ImportUV1Box
            // 
            this.ImportUV1Box.AutoSize = true;
            this.ImportUV1Box.Location = new System.Drawing.Point(16, 83);
            this.ImportUV1Box.Name = "ImportUV1Box";
            this.ImportUV1Box.Size = new System.Drawing.Size(79, 17);
            this.ImportUV1Box.TabIndex = 7;
            this.ImportUV1Box.Text = "Import UV1";
            this.ImportUV1Box.UseVisualStyleBackColor = true;
            // 
            // ImportUV2Box
            // 
            this.ImportUV2Box.AutoSize = true;
            this.ImportUV2Box.Location = new System.Drawing.Point(16, 106);
            this.ImportUV2Box.Name = "ImportUV2Box";
            this.ImportUV2Box.Size = new System.Drawing.Size(79, 17);
            this.ImportUV2Box.TabIndex = 8;
            this.ImportUV2Box.Text = "Import UV2";
            this.ImportUV2Box.UseVisualStyleBackColor = true;
            // 
            // ImportUV7Box
            // 
            this.ImportUV7Box.AutoSize = true;
            this.ImportUV7Box.Location = new System.Drawing.Point(16, 129);
            this.ImportUV7Box.Name = "ImportUV7Box";
            this.ImportUV7Box.Size = new System.Drawing.Size(79, 17);
            this.ImportUV7Box.TabIndex = 9;
            this.ImportUV7Box.Text = "Import UV7";
            this.ImportUV7Box.UseVisualStyleBackColor = true;
            // 
            // FlipUVBox
            // 
            this.FlipUVBox.AutoSize = true;
            this.FlipUVBox.Location = new System.Drawing.Point(181, 39);
            this.FlipUVBox.Name = "FlipUVBox";
            this.FlipUVBox.Size = new System.Drawing.Size(119, 17);
            this.FlipUVBox.TabIndex = 10;
            this.FlipUVBox.Text = "Flip UV Coordinates";
            this.FlipUVBox.UseVisualStyleBackColor = true;
            // 
            // ImportUV0Box
            // 
            this.ImportUV0Box.AutoSize = true;
            this.ImportUV0Box.Location = new System.Drawing.Point(16, 60);
            this.ImportUV0Box.Name = "ImportUV0Box";
            this.ImportUV0Box.Size = new System.Drawing.Size(79, 17);
            this.ImportUV0Box.TabIndex = 11;
            this.ImportUV0Box.Text = "Import UV0";
            this.ImportUV0Box.UseVisualStyleBackColor = true;
            // 
            // FrameResourceModelOptions
            // 
            this.AcceptButton = this.buttonContinue;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(364, 193);
            this.ControlBox = false;
            this.Controls.Add(this.ImportUV0Box);
            this.Controls.Add(this.FlipUVBox);
            this.Controls.Add(this.ImportUV7Box);
            this.Controls.Add(this.ImportUV2Box);
            this.Controls.Add(this.ImportUV1Box);
            this.Controls.Add(this.ImportNormalBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonContinue);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrameResourceModelOptions";
            this.Text = "NewObjectEntry";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonContinue;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox ImportNormalBox;
        private System.Windows.Forms.CheckBox ImportUV1Box;
        private System.Windows.Forms.CheckBox ImportUV2Box;
        private System.Windows.Forms.CheckBox ImportUV7Box;
        private System.Windows.Forms.CheckBox FlipUVBox;
        private System.Windows.Forms.CheckBox ImportUV0Box;
    }
}