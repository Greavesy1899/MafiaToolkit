namespace Forms.OptionControls
{
    partial class MTLOptions
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
            this.groupMTL = new System.Windows.Forms.GroupBox();
            this.removeSelectedButton = new System.Windows.Forms.Button();
            this.addLibraryButton = new System.Windows.Forms.Button();
            this.MTLListBox = new System.Windows.Forms.ListBox();
            this.MTLsToLoadText = new System.Windows.Forms.Label();
            this.MTLBrowser = new System.Windows.Forms.OpenFileDialog();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.groupMTL.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupMTL
            // 
            this.groupMTL.AutoSize = true;
            this.groupMTL.Controls.Add(this.removeSelectedButton);
            this.groupMTL.Controls.Add(this.addLibraryButton);
            this.groupMTL.Controls.Add(this.MTLListBox);
            this.groupMTL.Controls.Add(this.MTLsToLoadText);
            this.groupMTL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupMTL.Location = new System.Drawing.Point(0, 0);
            this.groupMTL.Name = "groupMTL";
            this.groupMTL.Size = new System.Drawing.Size(443, 357);
            this.groupMTL.TabIndex = 0;
            this.groupMTL.TabStop = false;
            this.groupMTL.Text = "$MATERIAL_LIBS";
            // 
            // removeSelectedButton
            // 
            this.removeSelectedButton.Location = new System.Drawing.Point(99, 218);
            this.removeSelectedButton.Name = "removeSelectedButton";
            this.removeSelectedButton.Size = new System.Drawing.Size(103, 23);
            this.removeSelectedButton.TabIndex = 3;
            this.removeSelectedButton.Text = "$MATERIAL_LIB_REMOVE";
            this.removeSelectedButton.UseVisualStyleBackColor = true;
            this.removeSelectedButton.Click += new System.EventHandler(this.removeSelected_Click);
            // 
            // addLibraryButton
            // 
            this.addLibraryButton.Location = new System.Drawing.Point(10, 218);
            this.addLibraryButton.Name = "addLibraryButton";
            this.addLibraryButton.Size = new System.Drawing.Size(83, 23);
            this.addLibraryButton.TabIndex = 2;
            this.addLibraryButton.Text = "$MATERIAL_LIB_ADD";
            this.addLibraryButton.UseVisualStyleBackColor = true;
            this.addLibraryButton.Click += new System.EventHandler(this.addLibrary_Click);
            // 
            // MTLListBox
            // 
            this.MTLListBox.FormattingEnabled = true;
            this.MTLListBox.HorizontalScrollbar = true;
            this.MTLListBox.Location = new System.Drawing.Point(10, 37);
            this.MTLListBox.Name = "MTLListBox";
            this.MTLListBox.Size = new System.Drawing.Size(411, 173);
            this.MTLListBox.TabIndex = 1;
            // 
            // MTLsToLoadText
            // 
            this.MTLsToLoadText.AutoSize = true;
            this.MTLsToLoadText.Location = new System.Drawing.Point(7, 20);
            this.MTLsToLoadText.Name = "MTLsToLoadText";
            this.MTLsToLoadText.Size = new System.Drawing.Size(151, 13);
            this.MTLsToLoadText.TabIndex = 0;
            this.MTLsToLoadText.Text = "$MATERIAL_LIB_SELECTED";
            // 
            // MTLBrowser
            // 
            this.MTLBrowser.Filter = "\"MTL Files|*.mtl|All files|*.*\"\"";
            this.MTLBrowser.Multiselect = true;
            // 
            // MTLOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupMTL);
            this.Name = "MTLOptions";
            this.Size = new System.Drawing.Size(443, 357);
            this.groupMTL.ResumeLayout(false);
            this.groupMTL.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupMTL;
        private System.Windows.Forms.Label MTLsToLoadText;
        private System.Windows.Forms.ListBox MTLListBox;
        private System.Windows.Forms.Button addLibraryButton;
        private System.Windows.Forms.Button removeSelectedButton;
        private System.Windows.Forms.OpenFileDialog MTLBrowser;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}
