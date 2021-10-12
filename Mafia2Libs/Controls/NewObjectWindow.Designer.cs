namespace MafiaToolkit
{
    partial class NewObjectForm
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
            this.label = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.buttonContinue = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Location = new System.Drawing.Point(13, 13);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(46, 13);
            this.label.TabIndex = 0;
            this.label.Text = "$LABEL";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(16, 30);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(340, 20);
            this.textBox1.TabIndex = 1;
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
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Location = new System.Drawing.Point(16, 57);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(340, 100);
            this.panel1.TabIndex = 4;
            // 
            // NewObjectForm
            // 
            this.AcceptButton = this.buttonContinue;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(364, 193);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonContinue);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewObjectForm";
            this.Text = "NewObjectEntry";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button buttonContinue;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Panel panel1;
    }
}