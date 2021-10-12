namespace Toolkit.Forms
{
    partial class GameSelector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameSelector));
            this.FlowPanel_GamesList = new System.Windows.Forms.FlowLayoutPanel();
            this.CheckBox_SelectAsDefault = new System.Windows.Forms.CheckBox();
            this.Label_ToolkitVersion = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // FlowPanel_GamesList
            // 
            this.FlowPanel_GamesList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FlowPanel_GamesList.AutoScroll = true;
            this.FlowPanel_GamesList.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.FlowPanel_GamesList.Location = new System.Drawing.Point(14, 36);
            this.FlowPanel_GamesList.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.FlowPanel_GamesList.Name = "FlowPanel_GamesList";
            this.FlowPanel_GamesList.Size = new System.Drawing.Size(905, 470);
            this.FlowPanel_GamesList.TabIndex = 0;
            this.FlowPanel_GamesList.WrapContents = false;
            this.FlowPanel_GamesList.Paint += new System.Windows.Forms.PaintEventHandler(this.FlowPanel_GamesList_Paint);
            // 
            // CheckBox_SelectAsDefault
            // 
            this.CheckBox_SelectAsDefault.AutoSize = true;
            this.CheckBox_SelectAsDefault.Location = new System.Drawing.Point(723, 14);
            this.CheckBox_SelectAsDefault.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.CheckBox_SelectAsDefault.Name = "CheckBox_SelectAsDefault";
            this.CheckBox_SelectAsDefault.Size = new System.Drawing.Size(140, 19);
            this.CheckBox_SelectAsDefault.TabIndex = 1;
            this.CheckBox_SelectAsDefault.Text = "$SELECT_AS_DEFAULT";
            this.CheckBox_SelectAsDefault.UseVisualStyleBackColor = true;
            this.CheckBox_SelectAsDefault.CheckedChanged += new System.EventHandler(this.CheckBox_SelectAsDefault_OnChecked);
            // 
            // Label_ToolkitVersion
            // 
            this.Label_ToolkitVersion.AutoSize = true;
            this.Label_ToolkitVersion.Location = new System.Drawing.Point(14, 15);
            this.Label_ToolkitVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label_ToolkitVersion.Name = "Label_ToolkitVersion";
            this.Label_ToolkitVersion.Size = new System.Drawing.Size(104, 15);
            this.Label_ToolkitVersion.TabIndex = 2;
            this.Label_ToolkitVersion.Text = "TOOLKIT_VERSION";
            // 
            // GameSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 519);
            this.Controls.Add(this.Label_ToolkitVersion);
            this.Controls.Add(this.CheckBox_SelectAsDefault);
            this.Controls.Add(this.FlowPanel_GamesList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GameSelector";
            this.Text = "GameSelector";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel FlowPanel_GamesList;
        private System.Windows.Forms.CheckBox CheckBox_SelectAsDefault;
        private System.Windows.Forms.Label Label_ToolkitVersion;
    }
}