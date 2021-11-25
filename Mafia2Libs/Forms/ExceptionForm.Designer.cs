
namespace Toolkit.Forms
{
    partial class ExceptionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExceptionForm));
            this.Label_ExceptionMsg = new System.Windows.Forms.Label();
            this.RichTextBox_StackTrace = new System.Windows.Forms.RichTextBox();
            this.Button_Continue = new System.Windows.Forms.Button();
            this.Button_Quit = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Label_ExceptionMsg
            // 
            this.Label_ExceptionMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Label_ExceptionMsg.Location = new System.Drawing.Point(51, 13);
            this.Label_ExceptionMsg.Name = "Label_ExceptionMsg";
            this.Label_ExceptionMsg.Size = new System.Drawing.Size(525, 53);
            this.Label_ExceptionMsg.TabIndex = 0;
            this.Label_ExceptionMsg.Text = resources.GetString("Label_ExceptionMsg.Text");
            // 
            // RichTextBox_StackTrace
            // 
            this.RichTextBox_StackTrace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RichTextBox_StackTrace.BackColor = System.Drawing.SystemColors.Window;
            this.RichTextBox_StackTrace.Location = new System.Drawing.Point(13, 102);
            this.RichTextBox_StackTrace.Name = "RichTextBox_StackTrace";
            this.RichTextBox_StackTrace.ReadOnly = true;
            this.RichTextBox_StackTrace.Size = new System.Drawing.Size(563, 352);
            this.RichTextBox_StackTrace.TabIndex = 1;
            this.RichTextBox_StackTrace.Text = "";
            // 
            // Button_Continue
            // 
            this.Button_Continue.Location = new System.Drawing.Point(370, 71);
            this.Button_Continue.Name = "Button_Continue";
            this.Button_Continue.Size = new System.Drawing.Size(100, 25);
            this.Button_Continue.TabIndex = 2;
            this.Button_Continue.Text = "Continue";
            this.Button_Continue.UseVisualStyleBackColor = true;
            this.Button_Continue.Click += new System.EventHandler(this.Button_Continue_Click);
            // 
            // Button_Quit
            // 
            this.Button_Quit.Location = new System.Drawing.Point(476, 71);
            this.Button_Quit.Name = "Button_Quit";
            this.Button_Quit.Size = new System.Drawing.Size(100, 25);
            this.Button_Quit.TabIndex = 3;
            this.Button_Quit.Text = "Quit";
            this.Button_Quit.UseVisualStyleBackColor = true;
            this.Button_Quit.Click += new System.EventHandler(this.Button_Quit_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.ErrorImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.ErrorImage")));
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(13, 13);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // ExceptionForm
            // 
            this.AcceptButton = this.Button_Continue;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Button_Quit;
            this.ClientSize = new System.Drawing.Size(588, 466);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.Button_Quit);
            this.Controls.Add(this.Button_Continue);
            this.Controls.Add(this.RichTextBox_StackTrace);
            this.Controls.Add(this.Label_ExceptionMsg);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExceptionForm";
            this.ShowIcon = false;
            this.Text = "Unhandled Exception";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label Label_ExceptionMsg;
        private System.Windows.Forms.RichTextBox RichTextBox_StackTrace;
        private System.Windows.Forms.Button Button_Continue;
        private System.Windows.Forms.Button Button_Quit;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}