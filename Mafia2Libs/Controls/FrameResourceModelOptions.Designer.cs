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
            this.ModelOptionsText = new System.Windows.Forms.Label();
            this.ImportNormalBox = new System.Windows.Forms.CheckBox();
            this.ImportUV1Box = new System.Windows.Forms.CheckBox();
            this.ImportUV2Box = new System.Windows.Forms.CheckBox();
            this.ImportAOBox = new System.Windows.Forms.CheckBox();
            this.FlipUVBox = new System.Windows.Forms.CheckBox();
            this.ImportDiffuseBox = new System.Windows.Forms.CheckBox();
            this.ImportTangentBox = new System.Windows.Forms.CheckBox();
            this.ImportColor1Box = new System.Windows.Forms.CheckBox();
            this.ImportColor0Box = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // buttonContinue
            // 
            this.buttonContinue.Location = new System.Drawing.Point(259, 190);
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
            this.buttonCancel.Location = new System.Drawing.Point(12, 190);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(93, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "$CANCEL";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.OnButtonClickCancel);
            // 
            // ModelOptionsText
            // 
            this.ModelOptionsText.Location = new System.Drawing.Point(13, 13);
            this.ModelOptionsText.Name = "ModelOptionsText";
            this.ModelOptionsText.Size = new System.Drawing.Size(290, 13);
            this.ModelOptionsText.TabIndex = 4;
            this.ModelOptionsText.Text = "$MODEL_OPTIONS_TEXT";
            // 
            // ImportNormalBox
            // 
            this.ImportNormalBox.AutoSize = true;
            this.ImportNormalBox.Location = new System.Drawing.Point(15, 40);
            this.ImportNormalBox.Name = "ImportNormalBox";
            this.ImportNormalBox.Size = new System.Drawing.Size(133, 17);
            this.ImportNormalBox.TabIndex = 6;
            this.ImportNormalBox.Text = "$IMPORT_NORMALS";
            this.ImportNormalBox.UseVisualStyleBackColor = true;
            // 
            // ImportUV1Box
            // 
            this.ImportUV1Box.AutoSize = true;
            this.ImportUV1Box.Location = new System.Drawing.Point(15, 100);
            this.ImportUV1Box.Name = "ImportUV1Box";
            this.ImportUV1Box.Size = new System.Drawing.Size(101, 17);
            this.ImportUV1Box.TabIndex = 7;
            this.ImportUV1Box.Text = "$IMPORT_UV1";
            this.ImportUV1Box.UseVisualStyleBackColor = true;
            // 
            // ImportUV2Box
            // 
            this.ImportUV2Box.AutoSize = true;
            this.ImportUV2Box.Location = new System.Drawing.Point(15, 120);
            this.ImportUV2Box.Name = "ImportUV2Box";
            this.ImportUV2Box.Size = new System.Drawing.Size(101, 17);
            this.ImportUV2Box.TabIndex = 8;
            this.ImportUV2Box.Text = "$IMPORT_UV2";
            this.ImportUV2Box.UseVisualStyleBackColor = true;
            // 
            // ImportAOBox
            // 
            this.ImportAOBox.AutoSize = true;
            this.ImportAOBox.Location = new System.Drawing.Point(15, 140);
            this.ImportAOBox.Name = "ImportAOBox";
            this.ImportAOBox.Size = new System.Drawing.Size(95, 17);
            this.ImportAOBox.TabIndex = 9;
            this.ImportAOBox.Text = "$IMPORT_AO";
            this.ImportAOBox.UseVisualStyleBackColor = true;
            // 
            // FlipUVBox
            // 
            this.FlipUVBox.AutoSize = true;
            this.FlipUVBox.Location = new System.Drawing.Point(180, 80);
            this.FlipUVBox.Name = "FlipUVBox";
            this.FlipUVBox.Size = new System.Drawing.Size(75, 17);
            this.FlipUVBox.TabIndex = 10;
            this.FlipUVBox.Text = "$FLIP_UV";
            this.FlipUVBox.UseVisualStyleBackColor = true;
            // 
            // ImportDiffuseBox
            // 
            this.ImportDiffuseBox.AutoSize = true;
            this.ImportDiffuseBox.Location = new System.Drawing.Point(15, 80);
            this.ImportDiffuseBox.Name = "ImportDiffuseBox";
            this.ImportDiffuseBox.Size = new System.Drawing.Size(125, 17);
            this.ImportDiffuseBox.TabIndex = 11;
            this.ImportDiffuseBox.Text = "$IMPORT_DIFFUSE";
            this.ImportDiffuseBox.UseVisualStyleBackColor = true;
            // 
            // ImportTangentBox
            // 
            this.ImportTangentBox.AutoSize = true;
            this.ImportTangentBox.Enabled = false;
            this.ImportTangentBox.Location = new System.Drawing.Point(15, 60);
            this.ImportTangentBox.Name = "ImportTangentBox";
            this.ImportTangentBox.Size = new System.Drawing.Size(139, 17);
            this.ImportTangentBox.TabIndex = 12;
            this.ImportTangentBox.Text = "$IMPORT_TANGENTS";
            this.ImportTangentBox.UseVisualStyleBackColor = true;
            // 
            // ImportColor1Box
            // 
            this.ImportColor1Box.AutoSize = true;
            this.ImportColor1Box.Location = new System.Drawing.Point(180, 60);
            this.ImportColor1Box.Name = "ImportColor1Box";
            this.ImportColor1Box.Size = new System.Drawing.Size(123, 17);
            this.ImportColor1Box.TabIndex = 14;
            this.ImportColor1Box.Text = "$IMPORT_COLOR1";
            this.ImportColor1Box.UseVisualStyleBackColor = true;
            // 
            // ImportColor0Box
            // 
            this.ImportColor0Box.AutoSize = true;
            this.ImportColor0Box.Location = new System.Drawing.Point(180, 40);
            this.ImportColor0Box.Name = "ImportColor0Box";
            this.ImportColor0Box.Size = new System.Drawing.Size(123, 17);
            this.ImportColor0Box.TabIndex = 13;
            this.ImportColor0Box.Text = "$IMPORT_COLOR0";
            this.ImportColor0Box.UseVisualStyleBackColor = true;
            // 
            // FrameResourceModelOptions
            // 
            this.AcceptButton = this.buttonContinue;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(364, 225);
            this.ControlBox = false;
            this.Controls.Add(this.ImportColor1Box);
            this.Controls.Add(this.ImportColor0Box);
            this.Controls.Add(this.ImportTangentBox);
            this.Controls.Add(this.ImportDiffuseBox);
            this.Controls.Add(this.FlipUVBox);
            this.Controls.Add(this.ImportAOBox);
            this.Controls.Add(this.ImportUV2Box);
            this.Controls.Add(this.ImportUV1Box);
            this.Controls.Add(this.ImportNormalBox);
            this.Controls.Add(this.ModelOptionsText);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonContinue);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrameResourceModelOptions";
            this.Text = "$MODEL_OPTIONS_TITLE";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonContinue;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label ModelOptionsText;
        private System.Windows.Forms.CheckBox ImportNormalBox;
        private System.Windows.Forms.CheckBox ImportUV1Box;
        private System.Windows.Forms.CheckBox ImportUV2Box;
        private System.Windows.Forms.CheckBox ImportAOBox;
        private System.Windows.Forms.CheckBox FlipUVBox;
        private System.Windows.Forms.CheckBox ImportDiffuseBox;
        private System.Windows.Forms.CheckBox ImportTangentBox;
        private System.Windows.Forms.CheckBox ImportColor1Box;
        private System.Windows.Forms.CheckBox ImportColor0Box;
    }
}