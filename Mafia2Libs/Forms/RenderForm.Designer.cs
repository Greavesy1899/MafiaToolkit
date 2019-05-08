using System;
using Utils.Extensions;

namespace Mafia2Tool
{
    partial class D3DForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(D3DForm));
            this.ToolbarStrip = new System.Windows.Forms.ToolStrip();
            this.FileButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.SaveButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitButton = new System.Windows.Forms.ToolStripMenuItem();
            this.EditButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.AddButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.SceneTreeButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ObjectPropertiesButton = new System.Windows.Forms.ToolStripMenuItem();
            this.OptionsButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.DisableLODButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ToggleModelsButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ToggleCollisionsButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ToggleWireframeButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ToggleCullingButton = new System.Windows.Forms.ToolStripMenuItem();
            this.TEMPCameraSpeed = new System.Windows.Forms.ToolStripTextBox();
            this.NameTableFlagLimit = new System.Windows.Forms.ToolStripTextBox();
            this.StatusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.dockPanel1 = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.RenderPanel = new System.Windows.Forms.Panel();
            this.MeshBrowser = new System.Windows.Forms.OpenFileDialog();
            this.AddSceneFolderButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolbarStrip.SuspendLayout();
            this.StatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // ToolbarStrip
            // 
            this.ToolbarStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileButton,
            this.EditButton,
            this.ViewButton,
            this.OptionsButton,
            this.TEMPCameraSpeed,
            this.NameTableFlagLimit});
            this.ToolbarStrip.Location = new System.Drawing.Point(0, 0);
            this.ToolbarStrip.Name = "ToolbarStrip";
            this.ToolbarStrip.Size = new System.Drawing.Size(800, 25);
            this.ToolbarStrip.TabIndex = 1;
            this.ToolbarStrip.Text = "toolStrip1";
            // 
            // FileButton
            // 
            this.FileButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.FileButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveButton,
            this.ExitButton});
            this.FileButton.Image = ((System.Drawing.Image)(resources.GetObject("FileButton.Image")));
            this.FileButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.FileButton.Name = "FileButton";
            this.FileButton.Size = new System.Drawing.Size(38, 22);
            this.FileButton.Text = "File";
            // 
            // SaveButton
            // 
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(180, 22);
            this.SaveButton.Text = "Save";
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // ExitButton
            // 
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(180, 22);
            this.ExitButton.Text = "Exit";
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // EditButton
            // 
            this.EditButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.EditButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddButton,
            this.AddSceneFolderButton});
            this.EditButton.Image = ((System.Drawing.Image)(resources.GetObject("EditButton.Image")));
            this.EditButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.EditButton.Name = "EditButton";
            this.EditButton.Size = new System.Drawing.Size(54, 22);
            this.EditButton.Text = "Create";
            // 
            // AddButton
            // 
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(180, 22);
            this.AddButton.Text = "$ADD";
            this.AddButton.Click += new System.EventHandler(this.AddButtonOnClick);
            // 
            // ViewButton
            // 
            this.ViewButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ViewButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SceneTreeButton,
            this.ObjectPropertiesButton});
            this.ViewButton.Image = ((System.Drawing.Image)(resources.GetObject("ViewButton.Image")));
            this.ViewButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ViewButton.Name = "ViewButton";
            this.ViewButton.Size = new System.Drawing.Size(45, 22);
            this.ViewButton.Text = "View";
            // 
            // SceneTreeButton
            // 
            this.SceneTreeButton.Name = "SceneTreeButton";
            this.SceneTreeButton.Size = new System.Drawing.Size(182, 22);
            this.SceneTreeButton.Text = "Scene Tree";
            this.SceneTreeButton.Click += new System.EventHandler(this.SceneTreeOnClicked);
            // 
            // ObjectPropertiesButton
            // 
            this.ObjectPropertiesButton.Name = "ObjectPropertiesButton";
            this.ObjectPropertiesButton.Size = new System.Drawing.Size(182, 22);
            this.ObjectPropertiesButton.Text = "Object Property Grid";
            this.ObjectPropertiesButton.Click += new System.EventHandler(this.PropertyGridOnClicked);
            // 
            // OptionsButton
            // 
            this.OptionsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.OptionsButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DisableLODButton,
            this.ToggleModelsButton,
            this.ToggleCollisionsButton,
            this.ToggleWireframeButton,
            this.ToggleCullingButton});
            this.OptionsButton.Image = ((System.Drawing.Image)(resources.GetObject("OptionsButton.Image")));
            this.OptionsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.OptionsButton.Name = "OptionsButton";
            this.OptionsButton.Size = new System.Drawing.Size(62, 22);
            this.OptionsButton.Text = "Options";
            // 
            // DisableLODButton
            // 
            this.DisableLODButton.Name = "DisableLODButton";
            this.DisableLODButton.Size = new System.Drawing.Size(168, 22);
            this.DisableLODButton.Text = "Disable LODs";
            // 
            // ToggleModelsButton
            // 
            this.ToggleModelsButton.Checked = true;
            this.ToggleModelsButton.CheckOnClick = true;
            this.ToggleModelsButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToggleModelsButton.Name = "ToggleModelsButton";
            this.ToggleModelsButton.Size = new System.Drawing.Size(168, 22);
            this.ToggleModelsButton.Text = "Toggle Models";
            // 
            // ToggleCollisionsButton
            // 
            this.ToggleCollisionsButton.Checked = true;
            this.ToggleCollisionsButton.CheckOnClick = true;
            this.ToggleCollisionsButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToggleCollisionsButton.Name = "ToggleCollisionsButton";
            this.ToggleCollisionsButton.Size = new System.Drawing.Size(168, 22);
            this.ToggleCollisionsButton.Text = "Toggle Collisions";
            // 
            // ToggleWireframeButton
            // 
            this.ToggleWireframeButton.Name = "ToggleWireframeButton";
            this.ToggleWireframeButton.Size = new System.Drawing.Size(168, 22);
            this.ToggleWireframeButton.Text = "Toggle Wireframe";
            this.ToggleWireframeButton.Click += new System.EventHandler(this.FillModeButton_Click);
            // 
            // ToggleCullingButton
            // 
            this.ToggleCullingButton.Name = "ToggleCullingButton";
            this.ToggleCullingButton.Size = new System.Drawing.Size(168, 22);
            this.ToggleCullingButton.Text = "Toggle Culling";
            this.ToggleCullingButton.Click += new System.EventHandler(this.CullModeButton_Click);
            // 
            // TEMPCameraSpeed
            // 
            this.TEMPCameraSpeed.Name = "TEMPCameraSpeed";
            this.TEMPCameraSpeed.Size = new System.Drawing.Size(100, 25);
            this.TEMPCameraSpeed.Leave += new System.EventHandler(this.CameraSpeedUpdate);
            // 
            // NameTableFlagLimit
            // 
            this.NameTableFlagLimit.Name = "NameTableFlagLimit";
            this.NameTableFlagLimit.Size = new System.Drawing.Size(100, 25);
            this.NameTableFlagLimit.Text = "0";
            // 
            // StatusStrip
            // 
            this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel4});
            this.StatusStrip.Location = new System.Drawing.Point(0, 426);
            this.StatusStrip.Name = "StatusStrip";
            this.StatusStrip.Size = new System.Drawing.Size(800, 24);
            this.StatusStrip.TabIndex = 6;
            this.StatusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(122, 19);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(122, 19);
            this.toolStripStatusLabel2.Text = "toolStripStatusLabel2";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(118, 19);
            this.toolStripStatusLabel3.Text = "toolStripStatusLabel3";
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(118, 19);
            this.toolStripStatusLabel4.Text = "toolStripStatusLabel4";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "StaticIcon");
            this.imageList1.Images.SetKeyName(1, "LightIcon");
            // 
            // dockPanel1
            // 
            this.dockPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel1.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingWindow;
            this.dockPanel1.Location = new System.Drawing.Point(0, 25);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.Size = new System.Drawing.Size(800, 401);
            this.dockPanel1.TabIndex = 0;
            // 
            // RenderPanel
            // 
            this.RenderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RenderPanel.Location = new System.Drawing.Point(0, 25);
            this.RenderPanel.Name = "RenderPanel";
            this.RenderPanel.Size = new System.Drawing.Size(800, 401);
            this.RenderPanel.TabIndex = 0;
            // 
            // MeshBrowser
            // 
            this.MeshBrowser.Filter = "Meshes|*.m2t|FBX|*.fbx";
            // 
            // AddSceneFolderButton
            // 
            this.AddSceneFolderButton.Name = "AddSceneFolderButton";
            this.AddSceneFolderButton.Size = new System.Drawing.Size(180, 22);
            this.AddSceneFolderButton.Text = "Add Scene Folder";
            this.AddSceneFolderButton.Click += new System.EventHandler(this.AddSceneFolderButton_Click);
            // 
            // D3DForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dockPanel1);
            this.Controls.Add(this.RenderPanel);
            this.Controls.Add(this.StatusStrip);
            this.Controls.Add(this.ToolbarStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "D3DForm";
            this.Text = "Map Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.ToolbarStrip.ResumeLayout(false);
            this.ToolbarStrip.PerformLayout();
            this.StatusStrip.ResumeLayout(false);
            this.StatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip ToolbarStrip;
        private System.Windows.Forms.ToolStripDropDownButton FileButton;
        private System.Windows.Forms.ToolStripMenuItem SaveButton;
        private System.Windows.Forms.ToolStripMenuItem ExitButton;
        private System.Windows.Forms.ToolStripDropDownButton ViewButton;
        private System.Windows.Forms.StatusStrip StatusStrip;
        private System.Windows.Forms.ToolStripDropDownButton OptionsButton;
        private System.Windows.Forms.Panel RenderPanel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripTextBox TEMPCameraSpeed;
        private System.Windows.Forms.ToolStripMenuItem DisableLODButton;
        private System.Windows.Forms.ToolStripTextBox NameTableFlagLimit;
        private System.Windows.Forms.ToolStripMenuItem ToggleCollisionsButton;
        private System.Windows.Forms.ToolStripMenuItem ToggleModelsButton;
        private System.Windows.Forms.ToolStripDropDownButton EditButton;
        private System.Windows.Forms.ToolStripMenuItem AddButton;
        private System.Windows.Forms.OpenFileDialog MeshBrowser;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ImageList imageList1;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel1;
        private System.Windows.Forms.ToolStripMenuItem SceneTreeButton;
        private System.Windows.Forms.ToolStripMenuItem ObjectPropertiesButton;
        private System.Windows.Forms.ToolStripMenuItem ToggleWireframeButton;
        private System.Windows.Forms.ToolStripMenuItem ToggleCullingButton;
        private System.Windows.Forms.ToolStripMenuItem AddSceneFolderButton;
    }
}