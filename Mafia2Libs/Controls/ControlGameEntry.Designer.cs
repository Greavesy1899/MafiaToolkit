using System;

namespace MafiaToolkit.Controls
{
    partial class ControlGameEntry
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
            this.Picture_GameIcon = new System.Windows.Forms.PictureBox();
            this.Label_GameName = new System.Windows.Forms.Label();
            this.TextBox_FolderPath = new System.Windows.Forms.TextBox();
            this.Label_FolderPath = new System.Windows.Forms.Label();
            this.Picture_Status = new System.Windows.Forms.PictureBox();
            this.Button_Start = new System.Windows.Forms.Button();
            this.Button_SelectFolder = new System.Windows.Forms.Button();
            this.Label_GameDescription = new System.Windows.Forms.Label();
            this.Label_GameType = new System.Windows.Forms.Label();
            this.FolderDialog_MafiaFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.Label_MissingImage = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Picture_GameIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Picture_Status)).BeginInit();
            this.SuspendLayout();
            // 
            // Picture_GameIcon
            // 
            this.Picture_GameIcon.InitialImage = null;
            this.Picture_GameIcon.Location = new System.Drawing.Point(3, 12);
            this.Picture_GameIcon.Name = "Picture_GameIcon";
            this.Picture_GameIcon.Size = new System.Drawing.Size(300, 150);
            this.Picture_GameIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Picture_GameIcon.TabIndex = 1;
            this.Picture_GameIcon.TabStop = false;
            // 
            // Label_GameName
            // 
            this.Label_GameName.AutoSize = true;
            this.Label_GameName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.Label_GameName.Location = new System.Drawing.Point(309, 12);
            this.Label_GameName.Name = "Label_GameName";
            this.Label_GameName.Size = new System.Drawing.Size(127, 15);
            this.Label_GameName.TabIndex = 2;
            this.Label_GameName.Text = "Label_GameName";
            // 
            // TextBox_FolderPath
            // 
            this.TextBox_FolderPath.Location = new System.Drawing.Point(312, 111);
            this.TextBox_FolderPath.Name = "TextBox_FolderPath";
            this.TextBox_FolderPath.Size = new System.Drawing.Size(426, 20);
            this.TextBox_FolderPath.TabIndex = 4;
            this.TextBox_FolderPath.TextChanged += new System.EventHandler(this.TextBox_FolderPath_OnTextChanged);
            // 
            // Label_FolderPath
            // 
            this.Label_FolderPath.AutoSize = true;
            this.Label_FolderPath.Location = new System.Drawing.Point(312, 92);
            this.Label_FolderPath.Name = "Label_FolderPath";
            this.Label_FolderPath.Size = new System.Drawing.Size(159, 13);
            this.Label_FolderPath.TabIndex = 5;
            this.Label_FolderPath.Text = "$GAMEENTRY_FOLDERPATH";
            // 
            // Picture_Status
            // 
            this.Picture_Status.Location = new System.Drawing.Point(674, 41);
            this.Picture_Status.Name = "Picture_Status";
            this.Picture_Status.Size = new System.Drawing.Size(64, 64);
            this.Picture_Status.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Picture_Status.TabIndex = 6;
            this.Picture_Status.TabStop = false;
            // 
            // Button_Start
            // 
            this.Button_Start.Location = new System.Drawing.Point(614, 139);
            this.Button_Start.Name = "Button_Start";
            this.Button_Start.Size = new System.Drawing.Size(124, 23);
            this.Button_Start.TabIndex = 7;
            this.Button_Start.Text = "$START_TOOLKIT";
            this.Button_Start.UseVisualStyleBackColor = true;
            // 
            // Button_SelectFolder
            // 
            this.Button_SelectFolder.Location = new System.Drawing.Point(315, 137);
            this.Button_SelectFolder.Name = "Button_SelectFolder";
            this.Button_SelectFolder.Size = new System.Drawing.Size(124, 23);
            this.Button_SelectFolder.TabIndex = 10;
            this.Button_SelectFolder.Text = "$SELECT_FOLDER";
            this.Button_SelectFolder.UseVisualStyleBackColor = true;
            this.Button_SelectFolder.Click += new System.EventHandler(this.Button_SelectFolder_OnClick);
            // 
            // Label_GameDescription
            // 
            this.Label_GameDescription.AutoSize = true;
            this.Label_GameDescription.Location = new System.Drawing.Point(309, 41);
            this.Label_GameDescription.Name = "Label_GameDescription";
            this.Label_GameDescription.Size = new System.Drawing.Size(120, 13);
            this.Label_GameDescription.TabIndex = 11;
            this.Label_GameDescription.Text = "Label_GameDescription";
            // 
            // Label_GameType
            // 
            this.Label_GameType.AutoSize = true;
            this.Label_GameType.Location = new System.Drawing.Point(671, 25);
            this.Label_GameType.Name = "Label_GameType";
            this.Label_GameType.Size = new System.Drawing.Size(59, 13);
            this.Label_GameType.TabIndex = 12;
            this.Label_GameType.Text = "GameType";
            // 
            // FolderDialog_MafiaFolder
            // 
            this.FolderDialog_MafiaFolder.Description = "$SELECT_MII_FOLDER";
            // 
            // Label_MissingImage
            // 
            this.Label_MissingImage.AutoSize = true;
            this.Label_MissingImage.Location = new System.Drawing.Point(103, 79);
            this.Label_MissingImage.Name = "Label_MissingImage";
            this.Label_MissingImage.Size = new System.Drawing.Size(89, 13);
            this.Label_MissingImage.TabIndex = 13;
            this.Label_MissingImage.Text = "MISSING IMAGE";
            // 
            // ControlGameEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.Label_MissingImage);
            this.Controls.Add(this.Label_GameType);
            this.Controls.Add(this.Label_GameDescription);
            this.Controls.Add(this.Button_SelectFolder);
            this.Controls.Add(this.Button_Start);
            this.Controls.Add(this.Picture_Status);
            this.Controls.Add(this.Label_FolderPath);
            this.Controls.Add(this.TextBox_FolderPath);
            this.Controls.Add(this.Label_GameName);
            this.Controls.Add(this.Picture_GameIcon);
            this.Name = "ControlGameEntry";
            this.Size = new System.Drawing.Size(748, 173);
            ((System.ComponentModel.ISupportInitialize)(this.Picture_GameIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Picture_Status)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox Picture_GameIcon;
        private System.Windows.Forms.Label Label_GameName;
        private System.Windows.Forms.TextBox TextBox_FolderPath;
        private System.Windows.Forms.Label Label_FolderPath;
        private System.Windows.Forms.PictureBox Picture_Status;
        private System.Windows.Forms.Button Button_Start;
        private System.Windows.Forms.Button Button_SelectFolder;
        private System.Windows.Forms.Label Label_GameDescription;
        private System.Windows.Forms.Label Label_GameType;
        private System.Windows.Forms.FolderBrowserDialog FolderDialog_MafiaFolder;
        private System.Windows.Forms.Label Label_MissingImage;
    }
}
