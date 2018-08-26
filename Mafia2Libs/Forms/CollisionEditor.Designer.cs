namespace Mafia2Tool
{
    partial class CollisionEditor
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
            this.FrameResourceGrid = new System.Windows.Forms.PropertyGrid();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.buttonLoadMesh = new System.Windows.Forms.Button();
            this.openM2T = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // FrameResourceGrid
            // 
            this.FrameResourceGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FrameResourceGrid.Location = new System.Drawing.Point(402, 57);
            this.FrameResourceGrid.Name = "FrameResourceGrid";
            this.FrameResourceGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.FrameResourceGrid.Size = new System.Drawing.Size(386, 381);
            this.FrameResourceGrid.TabIndex = 10;
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeView1.Location = new System.Drawing.Point(12, 57);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(368, 381);
            this.treeView1.TabIndex = 11;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // buttonLoadMesh
            // 
            this.buttonLoadMesh.Location = new System.Drawing.Point(12, 12);
            this.buttonLoadMesh.Name = "buttonLoadMesh";
            this.buttonLoadMesh.Size = new System.Drawing.Size(75, 23);
            this.buttonLoadMesh.TabIndex = 12;
            this.buttonLoadMesh.Text = "Load Mesh";
            this.buttonLoadMesh.UseVisualStyleBackColor = true;
            this.buttonLoadMesh.Click += new System.EventHandler(this.buttonLoadMesh_Click);
            // 
            // openM2T
            // 
            this.openM2T.FileName = "Select M2T file.";
            this.openM2T.Filter = "\"Model File|*.m2t|All Files|*.*\"";
            this.openM2T.Tag = "";
            // 
            // CollisionEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buttonLoadMesh);
            this.Controls.Add(this.FrameResourceGrid);
            this.Controls.Add(this.treeView1);
            this.Name = "CollisionEditor";
            this.Text = "CollisionEditor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnClose);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid FrameResourceGrid;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button buttonLoadMesh;
        private System.Windows.Forms.OpenFileDialog openM2T;
    }
}