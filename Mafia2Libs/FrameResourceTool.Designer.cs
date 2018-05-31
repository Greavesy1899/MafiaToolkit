namespace Mafia2Tool {
    partial class FrameResourceTool {
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
            this.FrameResourceButton = new System.Windows.Forms.Button();
            this.FrameResourceGrid = new System.Windows.Forms.PropertyGrid();
            this.FrameResourceListBox = new System.Windows.Forms.ListBox();
            this.FrameResourceSearch = new System.Windows.Forms.TextBox();
            this.Load3D = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // FrameResourceButton
            // 
            this.FrameResourceButton.Location = new System.Drawing.Point(226, 11);
            this.FrameResourceButton.Name = "FrameResourceButton";
            this.FrameResourceButton.Size = new System.Drawing.Size(75, 23);
            this.FrameResourceButton.TabIndex = 7;
            this.FrameResourceButton.Text = "button1";
            this.FrameResourceButton.UseVisualStyleBackColor = true;
            this.FrameResourceButton.Click += new System.EventHandler(this.LoadMaterialTool);
            // 
            // FrameResourceGrid
            // 
            this.FrameResourceGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FrameResourceGrid.Location = new System.Drawing.Point(386, 38);
            this.FrameResourceGrid.Name = "FrameResourceGrid";
            this.FrameResourceGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.FrameResourceGrid.Size = new System.Drawing.Size(386, 381);
            this.FrameResourceGrid.TabIndex = 6;
            // 
            // FrameResourceListBox
            // 
            this.FrameResourceListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.FrameResourceListBox.FormattingEnabled = true;
            this.FrameResourceListBox.Location = new System.Drawing.Point(35, 185);
            this.FrameResourceListBox.Name = "FrameResourceListBox";
            this.FrameResourceListBox.Size = new System.Drawing.Size(289, 173);
            this.FrameResourceListBox.TabIndex = 5;
            this.FrameResourceListBox.SelectedIndexChanged += new System.EventHandler(this.OnSelectedChanged);
            // 
            // FrameResourceSearch
            // 
            this.FrameResourceSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.FrameResourceSearch.Location = new System.Drawing.Point(12, 11);
            this.FrameResourceSearch.Name = "FrameResourceSearch";
            this.FrameResourceSearch.Size = new System.Drawing.Size(207, 20);
            this.FrameResourceSearch.TabIndex = 4;
            // 
            // Load3D
            // 
            this.Load3D.Location = new System.Drawing.Point(305, 11);
            this.Load3D.Name = "Load3D";
            this.Load3D.Size = new System.Drawing.Size(75, 23);
            this.Load3D.TabIndex = 8;
            this.Load3D.Text = "Load 3D";
            this.Load3D.UseVisualStyleBackColor = true;
            this.Load3D.Click += new System.EventHandler(this.OnClickLoad3D);
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(12, 40);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(368, 379);
            this.treeView1.TabIndex = 9;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnNodeSelect);
            // 
            // FrameResourceTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(785, 431);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.Load3D);
            this.Controls.Add(this.FrameResourceButton);
            this.Controls.Add(this.FrameResourceGrid);
            this.Controls.Add(this.FrameResourceSearch);
            this.Controls.Add(this.FrameResourceListBox);
            this.Name = "FrameResourceTool";
            this.Text = "FrameResourceTool";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button FrameResourceButton;
        private System.Windows.Forms.PropertyGrid FrameResourceGrid;
        private System.Windows.Forms.ListBox FrameResourceListBox;
        private System.Windows.Forms.TextBox FrameResourceSearch;
        private System.Windows.Forms.Button Load3D;
        private System.Windows.Forms.TreeView treeView1;
    }
}