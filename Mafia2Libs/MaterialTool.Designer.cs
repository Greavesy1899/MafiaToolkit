namespace Mafia2Tool {
    partial class MaterialTool {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.MaterialSearch = new System.Windows.Forms.TextBox();
            this.MaterialListBox = new System.Windows.Forms.ListBox();
            this.MaterialGrid = new System.Windows.Forms.PropertyGrid();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // MaterialSearch
            // 
            this.MaterialSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.MaterialSearch.Location = new System.Drawing.Point(13, 13);
            this.MaterialSearch.Name = "MaterialSearch";
            this.MaterialSearch.Size = new System.Drawing.Size(207, 20);
            this.MaterialSearch.TabIndex = 0;
            this.MaterialSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPressed);
            // 
            // MaterialListBox
            // 
            this.MaterialListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.MaterialListBox.FormattingEnabled = true;
            this.MaterialListBox.Location = new System.Drawing.Point(13, 40);
            this.MaterialListBox.Name = "MaterialListBox";
            this.MaterialListBox.Size = new System.Drawing.Size(368, 381);
            this.MaterialListBox.TabIndex = 1;
            this.MaterialListBox.SelectedIndexChanged += new System.EventHandler(this.OnMaterialSelected);
            // 
            // MaterialGrid
            // 
            this.MaterialGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MaterialGrid.Location = new System.Drawing.Point(387, 40);
            this.MaterialGrid.Name = "MaterialGrid";
            this.MaterialGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.MaterialGrid.Size = new System.Drawing.Size(386, 381);
            this.MaterialGrid.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(227, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // MaterialTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(785, 431);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.MaterialGrid);
            this.Controls.Add(this.MaterialListBox);
            this.Controls.Add(this.MaterialSearch);
            this.Name = "MaterialTool";
            this.Text = "MaterialTool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnClose);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox MaterialSearch;
        private System.Windows.Forms.ListBox MaterialListBox;
        private System.Windows.Forms.PropertyGrid MaterialGrid;
        private System.Windows.Forms.Button button1;
    }
}