namespace MafiaToolkit.Forms
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
            this.FlowPanel_GamesList.Location = new System.Drawing.Point(12, 31);
            this.FlowPanel_GamesList.Name = "FlowPanel_GamesList";
            this.FlowPanel_GamesList.Size = new System.Drawing.Size(776, 407);
            this.FlowPanel_GamesList.TabIndex = 0;
            this.FlowPanel_GamesList.WrapContents = false;
            // 
            // CheckBox_SelectAsDefault
            // 
            this.CheckBox_SelectAsDefault.AutoSize = true;
            this.CheckBox_SelectAsDefault.Location = new System.Drawing.Point(620, 12);
            this.CheckBox_SelectAsDefault.Name = "CheckBox_SelectAsDefault";
            this.CheckBox_SelectAsDefault.Size = new System.Drawing.Size(148, 17);
            this.CheckBox_SelectAsDefault.TabIndex = 1;
            this.CheckBox_SelectAsDefault.Text = "$SELECT_AS_DEFAULT";
            this.CheckBox_SelectAsDefault.UseVisualStyleBackColor = true;
            this.CheckBox_SelectAsDefault.CheckedChanged += new System.EventHandler(this.CheckBox_SelectAsDefault_OnChecked);
            // 
            // Label_ToolkitVersion
            // 
            this.Label_ToolkitVersion.AutoSize = true;
            this.Label_ToolkitVersion.Location = new System.Drawing.Point(12, 13);
            this.Label_ToolkitVersion.Name = "Label_ToolkitVersion";
            this.Label_ToolkitVersion.Size = new System.Drawing.Size(107, 13);
            this.Label_ToolkitVersion.TabIndex = 2;
            this.Label_ToolkitVersion.Text = "TOOLKIT_VERSION";
            // 
            // GameSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Label_ToolkitVersion);
            this.Controls.Add(this.CheckBox_SelectAsDefault);
            this.Controls.Add(this.FlowPanel_GamesList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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