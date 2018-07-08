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
            this.Load3D = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.collisionEditor = new System.Windows.Forms.Button();
            this.overwriteBuffer = new System.Windows.Forms.Button();
            this.edmBrowser = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // FrameResourceButton
            // 
            this.FrameResourceButton.Location = new System.Drawing.Point(12, 11);
            this.FrameResourceButton.Name = "FrameResourceButton";
            this.FrameResourceButton.Size = new System.Drawing.Size(91, 23);
            this.FrameResourceButton.TabIndex = 7;
            this.FrameResourceButton.Text = "View Materials";
            this.FrameResourceButton.UseVisualStyleBackColor = true;
            this.FrameResourceButton.Click += new System.EventHandler(this.LoadMaterialTool);
            // 
            // FrameResourceGrid
            // 
            this.FrameResourceGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FrameResourceGrid.Location = new System.Drawing.Point(387, 40);
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
            this.FrameResourceListBox.Location = new System.Drawing.Point(12, 40);
            this.FrameResourceListBox.Name = "FrameResourceListBox";
            this.FrameResourceListBox.Size = new System.Drawing.Size(368, 381);
            this.FrameResourceListBox.TabIndex = 5;
            this.FrameResourceListBox.Visible = false;
            this.FrameResourceListBox.SelectedIndexChanged += new System.EventHandler(this.OnSelectedChanged);
            // 
            // Load3D
            // 
            this.Load3D.Location = new System.Drawing.Point(109, 11);
            this.Load3D.Name = "Load3D";
            this.Load3D.Size = new System.Drawing.Size(75, 23);
            this.Load3D.TabIndex = 8;
            this.Load3D.Text = "Save 3D";
            this.Load3D.UseVisualStyleBackColor = true;
            this.Load3D.Click += new System.EventHandler(this.OnClickLoad3D);
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeView1.Location = new System.Drawing.Point(12, 40);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(368, 381);
            this.treeView1.TabIndex = 9;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnNodeSelect);
            // 
            // folderBrowser
            // 
            this.folderBrowser.Description = "Select folder which contains extracted SDS content.";
            this.folderBrowser.ShowNewFolderButton = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(190, 11);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "Switch View";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.SwitchView);
            // 
            // collisionEditor
            // 
            this.collisionEditor.Location = new System.Drawing.Point(271, 11);
            this.collisionEditor.Name = "collisionEditor";
            this.collisionEditor.Size = new System.Drawing.Size(81, 23);
            this.collisionEditor.TabIndex = 11;
            this.collisionEditor.Text = "Edit Collisions";
            this.collisionEditor.UseVisualStyleBackColor = true;
            this.collisionEditor.Click += new System.EventHandler(this.collisionEditor_Click);
            // 
            // overwriteBuffer
            // 
            this.overwriteBuffer.Location = new System.Drawing.Point(358, 11);
            this.overwriteBuffer.Name = "overwriteBuffer";
            this.overwriteBuffer.Size = new System.Drawing.Size(94, 23);
            this.overwriteBuffer.TabIndex = 13;
            this.overwriteBuffer.Text = "Overwrite Buffer";
            this.overwriteBuffer.UseVisualStyleBackColor = true;
            this.overwriteBuffer.Click += new System.EventHandler(this.overwriteBuffer_Click);
            // 
            // edmBrowser
            // 
            this.edmBrowser.FileName = "edmBrowser";
            // 
            // FrameResourceTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(785, 431);
            this.Controls.Add(this.overwriteBuffer);
            this.Controls.Add(this.collisionEditor);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Load3D);
            this.Controls.Add(this.FrameResourceButton);
            this.Controls.Add(this.FrameResourceGrid);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.FrameResourceListBox);
            this.Name = "FrameResourceTool";
            this.Text = "FrameResourceTool";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button FrameResourceButton;
        private System.Windows.Forms.PropertyGrid FrameResourceGrid;
        private System.Windows.Forms.ListBox FrameResourceListBox;
        private System.Windows.Forms.Button Load3D;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowser;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button collisionEditor;
        private System.Windows.Forms.Button overwriteBuffer;
        private System.Windows.Forms.OpenFileDialog edmBrowser;
    }
}