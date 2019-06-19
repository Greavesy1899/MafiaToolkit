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
            this.AddSceneFolderButton = new System.Windows.Forms.ToolStripMenuItem();
            this.AddCollisionButton = new System.Windows.Forms.ToolStripMenuItem();
            this.roadDebuggingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AddBackward = new System.Windows.Forms.ToolStripMenuItem();
            this.AddToward = new System.Windows.Forms.ToolStripMenuItem();
            this.AddRoadSplineButton = new System.Windows.Forms.ToolStripMenuItem();
            this.AddSplineTxT = new System.Windows.Forms.ToolStripMenuItem();
            this.AddJunctionButton = new System.Windows.Forms.ToolStripMenuItem();
            this.EditUnkSet3 = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.SceneTreeButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ObjectPropertiesButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewOptionProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.OptionsButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.ToggleWireframeButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ToggleCullingButton = new System.Windows.Forms.ToolStripMenuItem();
            this.TEMPCameraSpeed = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.StatusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.dockPanel1 = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.RenderPanel = new System.Windows.Forms.Panel();
            this.MeshBrowser = new System.Windows.Forms.OpenFileDialog();
            this.TxtBrowser = new System.Windows.Forms.OpenFileDialog();
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
            this.toolStripTextBox1});
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
            this.FileButton.Size = new System.Drawing.Size(47, 22);
            this.FileButton.Text = "$FILE";
            // 
            // SaveButton
            // 
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(106, 22);
            this.SaveButton.Text = "$SAVE";
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // ExitButton
            // 
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(106, 22);
            this.ExitButton.Text = "$EXIT";
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // EditButton
            // 
            this.EditButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.EditButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddButton,
            this.AddSceneFolderButton,
            this.AddCollisionButton,
            this.roadDebuggingToolStripMenuItem});
            this.EditButton.Image = ((System.Drawing.Image)(resources.GetObject("EditButton.Image")));
            this.EditButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.EditButton.Name = "EditButton";
            this.EditButton.Size = new System.Drawing.Size(67, 22);
            this.EditButton.Text = "$CREATE";
            // 
            // AddButton
            // 
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(191, 22);
            this.AddButton.Text = "$ADD";
            this.AddButton.Click += new System.EventHandler(this.AddButtonOnClick);
            // 
            // AddSceneFolderButton
            // 
            this.AddSceneFolderButton.Name = "AddSceneFolderButton";
            this.AddSceneFolderButton.Size = new System.Drawing.Size(191, 22);
            this.AddSceneFolderButton.Text = "$ADD_SCENE_FOLDER";
            this.AddSceneFolderButton.Click += new System.EventHandler(this.AddSceneFolderButton_Click);
            // 
            // AddCollisionButton
            // 
            this.AddCollisionButton.Name = "AddCollisionButton";
            this.AddCollisionButton.Size = new System.Drawing.Size(191, 22);
            this.AddCollisionButton.Text = "$ADD_COLLISION";
            this.AddCollisionButton.Click += new System.EventHandler(this.AddCollisionButton_Click);
            // 
            // roadDebuggingToolStripMenuItem
            // 
            this.roadDebuggingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddBackward,
            this.AddToward,
            this.AddRoadSplineButton,
            this.AddSplineTxT,
            this.AddJunctionButton,
            this.EditUnkSet3});
            this.roadDebuggingToolStripMenuItem.Name = "roadDebuggingToolStripMenuItem";
            this.roadDebuggingToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.roadDebuggingToolStripMenuItem.Text = "Road Debugging";
            // 
            // AddBackward
            // 
            this.AddBackward.Name = "AddBackward";
            this.AddBackward.Size = new System.Drawing.Size(183, 22);
            this.AddBackward.Text = "$ADD_BACKWARD";
            this.AddBackward.Click += new System.EventHandler(this.AddBackwardClick);
            // 
            // AddToward
            // 
            this.AddToward.Name = "AddToward";
            this.AddToward.Size = new System.Drawing.Size(183, 22);
            this.AddToward.Text = "$ADD_TOWARD";
            this.AddToward.Click += new System.EventHandler(this.AddTowardClick);
            // 
            // AddRoadSplineButton
            // 
            this.AddRoadSplineButton.Name = "AddRoadSplineButton";
            this.AddRoadSplineButton.Size = new System.Drawing.Size(183, 22);
            this.AddRoadSplineButton.Text = "$ADD_ROAD_SPLINE";
            this.AddRoadSplineButton.Click += new System.EventHandler(this.AddRoadSplineButton_Click);
            // 
            // AddSplineTxT
            // 
            this.AddSplineTxT.Name = "AddSplineTxT";
            this.AddSplineTxT.Size = new System.Drawing.Size(183, 22);
            this.AddSplineTxT.Text = "$ADD_SPLINE_TXT";
            this.AddSplineTxT.Click += new System.EventHandler(this.AddSplineTxT_Click);
            // 
            // AddJunctionButton
            // 
            this.AddJunctionButton.Name = "AddJunctionButton";
            this.AddJunctionButton.Size = new System.Drawing.Size(183, 22);
            this.AddJunctionButton.Text = "$ADD_JUNCTION";
            this.AddJunctionButton.Click += new System.EventHandler(this.AddJunctionOnClick);
            // 
            // EditUnkSet3
            // 
            this.EditUnkSet3.Name = "EditUnkSet3";
            this.EditUnkSet3.Size = new System.Drawing.Size(183, 22);
            this.EditUnkSet3.Text = "$EDIT_UNKSET3";
            this.EditUnkSet3.Click += new System.EventHandler(this.EditUnkSet3Click);
            // 
            // ViewButton
            // 
            this.ViewButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ViewButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SceneTreeButton,
            this.ObjectPropertiesButton,
            this.ViewOptionProperties});
            this.ViewButton.Image = ((System.Drawing.Image)(resources.GetObject("ViewButton.Image")));
            this.ViewButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ViewButton.Name = "ViewButton";
            this.ViewButton.Size = new System.Drawing.Size(53, 22);
            this.ViewButton.Text = "$VIEW";
            // 
            // SceneTreeButton
            // 
            this.SceneTreeButton.Name = "SceneTreeButton";
            this.SceneTreeButton.Size = new System.Drawing.Size(199, 22);
            this.SceneTreeButton.Text = "$VIEW_SCENE_TREE";
            this.SceneTreeButton.Click += new System.EventHandler(this.SceneTreeOnClicked);
            // 
            // ObjectPropertiesButton
            // 
            this.ObjectPropertiesButton.Name = "ObjectPropertiesButton";
            this.ObjectPropertiesButton.Size = new System.Drawing.Size(199, 22);
            this.ObjectPropertiesButton.Text = "$VIEW_PROPERTY_GRID";
            this.ObjectPropertiesButton.Click += new System.EventHandler(this.PropertyGridOnClicked);
            // 
            // ViewOptionProperties
            // 
            this.ViewOptionProperties.Name = "ViewOptionProperties";
            this.ViewOptionProperties.Size = new System.Drawing.Size(199, 22);
            this.ViewOptionProperties.Text = "$VIEW_OPTIONS";
            this.ViewOptionProperties.Click += new System.EventHandler(this.ViewOptionProperties_Click);
            // 
            // OptionsButton
            // 
            this.OptionsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.OptionsButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToggleWireframeButton,
            this.ToggleCullingButton});
            this.OptionsButton.Image = ((System.Drawing.Image)(resources.GetObject("OptionsButton.Image")));
            this.OptionsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.OptionsButton.Name = "OptionsButton";
            this.OptionsButton.Size = new System.Drawing.Size(76, 22);
            this.OptionsButton.Text = "$OPTIONS";
            // 
            // ToggleWireframeButton
            // 
            this.ToggleWireframeButton.Name = "ToggleWireframeButton";
            this.ToggleWireframeButton.Size = new System.Drawing.Size(193, 22);
            this.ToggleWireframeButton.Text = "$TOGGLE_WIREFRAME";
            this.ToggleWireframeButton.Click += new System.EventHandler(this.FillModeButton_Click);
            // 
            // ToggleCullingButton
            // 
            this.ToggleCullingButton.Name = "ToggleCullingButton";
            this.ToggleCullingButton.Size = new System.Drawing.Size(193, 22);
            this.ToggleCullingButton.Text = "$TOGGLE_CULLING";
            this.ToggleCullingButton.Click += new System.EventHandler(this.CullModeButton_Click);
            // 
            // TEMPCameraSpeed
            // 
            this.TEMPCameraSpeed.Name = "TEMPCameraSpeed";
            this.TEMPCameraSpeed.Size = new System.Drawing.Size(100, 25);
            this.TEMPCameraSpeed.Leave += new System.EventHandler(this.CameraSpeedUpdate);
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(100, 25);
            // 
            // StatusStrip
            // 
            this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel2});
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
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(118, 19);
            this.toolStripStatusLabel3.Text = "toolStripStatusLabel3";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(118, 19);
            this.toolStripStatusLabel2.Text = "toolStripStatusLabel2";
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
            // TxtBrowser
            // 
            this.TxtBrowser.Filter = "Text Document|*txt";
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
        private System.Windows.Forms.ToolStripDropDownButton EditButton;
        private System.Windows.Forms.ToolStripMenuItem AddButton;
        private System.Windows.Forms.OpenFileDialog MeshBrowser;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ImageList imageList1;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel1;
        private System.Windows.Forms.ToolStripMenuItem SceneTreeButton;
        private System.Windows.Forms.ToolStripMenuItem ObjectPropertiesButton;
        private System.Windows.Forms.ToolStripMenuItem ToggleWireframeButton;
        private System.Windows.Forms.ToolStripMenuItem ToggleCullingButton;
        private System.Windows.Forms.ToolStripMenuItem AddSceneFolderButton;
        private System.Windows.Forms.ToolStripMenuItem AddRoadSplineButton;
        private System.Windows.Forms.ToolStripMenuItem ViewOptionProperties;
        private System.Windows.Forms.ToolStripMenuItem AddSplineTxT;
        private System.Windows.Forms.OpenFileDialog TxtBrowser;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.ToolStripMenuItem AddJunctionButton;
        private System.Windows.Forms.ToolStripMenuItem EditUnkSet3;
        private System.Windows.Forms.ToolStripMenuItem AddBackward;
        private System.Windows.Forms.ToolStripMenuItem AddToward;
        private System.Windows.Forms.ToolStripMenuItem AddCollisionButton;
        private System.Windows.Forms.ToolStripMenuItem roadDebuggingToolStripMenuItem;
    }
}