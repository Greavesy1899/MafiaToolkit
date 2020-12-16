using System;
using Utils.Extensions;

namespace Mafia2Tool
{
    partial class MapEditor
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
            System.Windows.Forms.StatusStrip StatusStrip;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapEditor));
            this.CurrentModeButton = new System.Windows.Forms.ToolStripSplitButton();
            this.PositionXTool = new Utils.Extensions.NumericUpDownToolStrip();
            this.PositionYTool = new Utils.Extensions.NumericUpDownToolStrip();
            this.PositionZTool = new Utils.Extensions.NumericUpDownToolStrip();
            this.CameraSpeedTool = new Utils.Extensions.NumericUpDownToolStrip();
            this.Label_FPS = new System.Windows.Forms.ToolStripStatusLabel();
            this.Label_MemoryUsage = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolbarStrip = new System.Windows.Forms.ToolStrip();
            this.FileButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.SaveButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitButton = new System.Windows.Forms.ToolStripMenuItem();
            this.EditButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.AddButton = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_ImportFrame = new System.Windows.Forms.ToolStripMenuItem();
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
            this.ViewTopButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewFrontButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewSideButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewBottomButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewSide2Button = new System.Windows.Forms.ToolStripMenuItem();
            this.OptionsButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.ToggleWireframeButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ToggleCullingButton = new System.Windows.Forms.ToolStripMenuItem();
            this.EditLighting = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_TestConvert32 = new System.Windows.Forms.ToolStripMenuItem();
            this.Button_TestConvert16 = new System.Windows.Forms.ToolStripMenuItem();
            this.WindowButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.SceneTreeButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ObjectPropertiesButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewOptionProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.RenderPanel = new System.Windows.Forms.Panel();
            this.MeshBrowser = new System.Windows.Forms.OpenFileDialog();
            this.TxtBrowser = new System.Windows.Forms.OpenFileDialog();
            this.dockPanel1 = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.FrameBrowser = new System.Windows.Forms.OpenFileDialog();
            this.Button_DumpTexture = new System.Windows.Forms.ToolStripMenuItem();
            StatusStrip = new System.Windows.Forms.StatusStrip();
            StatusStrip.SuspendLayout();
            this.ToolbarStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // StatusStrip
            // 
            StatusStrip.AutoSize = false;
            StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CurrentModeButton,
            this.PositionXTool,
            this.PositionYTool,
            this.PositionZTool,
            this.CameraSpeedTool,
            this.Label_FPS,
            this.Label_MemoryUsage});
            StatusStrip.Location = new System.Drawing.Point(0, 600);
            StatusStrip.Name = "StatusStrip";
            StatusStrip.Size = new System.Drawing.Size(1217, 24);
            StatusStrip.TabIndex = 6;
            StatusStrip.Text = "statusStrip1";
            // 
            // CurrentModeButton
            // 
            this.CurrentModeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.CurrentModeButton.Image = ((System.Drawing.Image)(resources.GetObject("CurrentModeButton.Image")));
            this.CurrentModeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CurrentModeButton.Name = "CurrentModeButton";
            this.CurrentModeButton.Size = new System.Drawing.Size(128, 22);
            this.CurrentModeButton.Text = "$CurrentModeLabel";
            this.CurrentModeButton.ButtonClick += new System.EventHandler(this.CurrentModeButton_ButtonClick);
            // 
            // PositionXTool
            // 
            this.PositionXTool.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.PositionXTool.AutoSize = false;
            this.PositionXTool.DecimalPlaces = 5;
            this.PositionXTool.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.PositionXTool.Margin = new System.Windows.Forms.Padding(3, 0, 1, 0);
            this.PositionXTool.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.PositionXTool.Minimum = new decimal(new int[] {
            9999999,
            0,
            0,
            -2147483648});
            this.PositionXTool.Name = "PositionXTool";
            this.PositionXTool.Overflow = System.Windows.Forms.ToolStripItemOverflow.Always;
            this.PositionXTool.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PositionXTool.Size = new System.Drawing.Size(76, 24);
            this.PositionXTool.Text = "0.00000";
            this.PositionXTool.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.PositionXTool.ValueChanged += new System.EventHandler(this.CameraToolsOnValueChanged);
            // 
            // PositionYTool
            // 
            this.PositionYTool.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.PositionYTool.AutoSize = false;
            this.PositionYTool.DecimalPlaces = 5;
            this.PositionYTool.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.PositionYTool.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.PositionYTool.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.PositionYTool.Minimum = new decimal(new int[] {
            9999999,
            0,
            0,
            -2147483648});
            this.PositionYTool.Name = "PositionYTool";
            this.PositionYTool.Overflow = System.Windows.Forms.ToolStripItemOverflow.Always;
            this.PositionYTool.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PositionYTool.Size = new System.Drawing.Size(76, 24);
            this.PositionYTool.Text = "0.00000";
            this.PositionYTool.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.PositionYTool.ValueChanged += new System.EventHandler(this.CameraToolsOnValueChanged);
            // 
            // PositionZTool
            // 
            this.PositionZTool.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.PositionZTool.AutoSize = false;
            this.PositionZTool.DecimalPlaces = 5;
            this.PositionZTool.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.PositionZTool.Margin = new System.Windows.Forms.Padding(1, 0, 3, 0);
            this.PositionZTool.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.PositionZTool.Minimum = new decimal(new int[] {
            9999999,
            0,
            0,
            -2147483648});
            this.PositionZTool.Name = "PositionZTool";
            this.PositionZTool.Overflow = System.Windows.Forms.ToolStripItemOverflow.Always;
            this.PositionZTool.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PositionZTool.Size = new System.Drawing.Size(76, 24);
            this.PositionZTool.Text = "0.00000";
            this.PositionZTool.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.PositionZTool.ValueChanged += new System.EventHandler(this.CameraToolsOnValueChanged);
            // 
            // CameraSpeedTool
            // 
            this.CameraSpeedTool.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.CameraSpeedTool.AutoSize = false;
            this.CameraSpeedTool.DecimalPlaces = 5;
            this.CameraSpeedTool.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.CameraSpeedTool.Margin = new System.Windows.Forms.Padding(1, 0, 3, 0);
            this.CameraSpeedTool.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.CameraSpeedTool.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.CameraSpeedTool.Name = "CameraSpeedTool";
            this.CameraSpeedTool.Overflow = System.Windows.Forms.ToolStripItemOverflow.Always;
            this.CameraSpeedTool.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.CameraSpeedTool.Size = new System.Drawing.Size(76, 24);
            this.CameraSpeedTool.Text = "0.00000";
            this.CameraSpeedTool.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.CameraSpeedTool.ValueChanged += new System.EventHandler(this.CameraSpeedUpdate);
            // 
            // Label_FPS
            // 
            this.Label_FPS.Name = "Label_FPS";
            this.Label_FPS.Overflow = System.Windows.Forms.ToolStripItemOverflow.Always;
            this.Label_FPS.Padding = new System.Windows.Forms.Padding(41, 0, 0, 0);
            this.Label_FPS.Size = new System.Drawing.Size(100, 19);
            this.Label_FPS.Text = "Label_FPS";
            // 
            // Label_MemoryUsage
            // 
            this.Label_MemoryUsage.Name = "Label_MemoryUsage";
            this.Label_MemoryUsage.Size = new System.Drawing.Size(117, 19);
            this.Label_MemoryUsage.Text = "Label_MemoryUsage";
            // 
            // ToolbarStrip
            // 
            this.ToolbarStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileButton,
            this.EditButton,
            this.ViewButton,
            this.OptionsButton,
            this.WindowButton});
            this.ToolbarStrip.Location = new System.Drawing.Point(0, 0);
            this.ToolbarStrip.Name = "ToolbarStrip";
            this.ToolbarStrip.Size = new System.Drawing.Size(1217, 25);
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
            this.Button_ImportFrame,
            this.AddSceneFolderButton,
            this.AddCollisionButton,
            this.roadDebuggingToolStripMenuItem});
            this.EditButton.Image = ((System.Drawing.Image)(resources.GetObject("EditButton.Image")));
            this.EditButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.EditButton.Name = "EditButton";
            this.EditButton.Size = new System.Drawing.Size(66, 22);
            this.EditButton.Text = "$CREATE";
            // 
            // AddButton
            // 
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(191, 22);
            this.AddButton.Text = "$ADD";
            this.AddButton.Click += new System.EventHandler(this.AddButtonOnClick);
            // 
            // Button_ImportFrame
            // 
            this.Button_ImportFrame.Name = "Button_ImportFrame";
            this.Button_ImportFrame.Size = new System.Drawing.Size(191, 22);
            this.Button_ImportFrame.Text = "$IMPORT_FRAME";
            this.Button_ImportFrame.Click += new System.EventHandler(this.Button_ImportFrame_OnClicked);
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
            this.ViewTopButton,
            this.ViewFrontButton,
            this.ViewSideButton,
            this.ViewBottomButton,
            this.ViewSide2Button});
            this.ViewButton.Image = ((System.Drawing.Image)(resources.GetObject("ViewButton.Image")));
            this.ViewButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ViewButton.Name = "ViewButton";
            this.ViewButton.Size = new System.Drawing.Size(53, 22);
            this.ViewButton.Text = "$VIEW";
            // 
            // ViewTopButton
            // 
            this.ViewTopButton.Name = "ViewTopButton";
            this.ViewTopButton.Size = new System.Drawing.Size(180, 22);
            this.ViewTopButton.Text = "$TOP";
            this.ViewTopButton.Click += new System.EventHandler(this.OnViewTopButtonClicked);
            // 
            // ViewFrontButton
            // 
            this.ViewFrontButton.Name = "ViewFrontButton";
            this.ViewFrontButton.Size = new System.Drawing.Size(180, 22);
            this.ViewFrontButton.Text = "$FRONT";
            this.ViewFrontButton.Click += new System.EventHandler(this.OnViewFrontButtonClicked);
            // 
            // ViewSideButton
            // 
            this.ViewSideButton.Enabled = false;
            this.ViewSideButton.Name = "ViewSideButton";
            this.ViewSideButton.Size = new System.Drawing.Size(180, 22);
            this.ViewSideButton.Text = "$SIDE";
            this.ViewSideButton.Visible = false;
            this.ViewSideButton.Click += new System.EventHandler(this.OnViewSideButtonClicked);
            // 
            // ViewBottomButton
            // 
            this.ViewBottomButton.Enabled = false;
            this.ViewBottomButton.Name = "ViewBottomButton";
            this.ViewBottomButton.Size = new System.Drawing.Size(180, 22);
            this.ViewBottomButton.Text = "$BOTTOM";
            this.ViewBottomButton.Visible = false;
            this.ViewBottomButton.Click += new System.EventHandler(this.OnViewBottomButtonClicked);
            // 
            // ViewSide2Button
            // 
            this.ViewSide2Button.Enabled = false;
            this.ViewSide2Button.Name = "ViewSide2Button";
            this.ViewSide2Button.Size = new System.Drawing.Size(180, 22);
            this.ViewSide2Button.Text = "$SIDE 2";
            this.ViewSide2Button.Visible = false;
            this.ViewSide2Button.Click += new System.EventHandler(this.OnViewSide2ButtonClicked);
            // 
            // OptionsButton
            // 
            this.OptionsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.OptionsButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToggleWireframeButton,
            this.ToggleCullingButton,
            this.EditLighting,
            this.Button_TestConvert32,
            this.Button_TestConvert16,
            this.Button_DumpTexture});
            this.OptionsButton.Image = ((System.Drawing.Image)(resources.GetObject("OptionsButton.Image")));
            this.OptionsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.OptionsButton.Name = "OptionsButton";
            this.OptionsButton.Size = new System.Drawing.Size(75, 22);
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
            // EditLighting
            // 
            this.EditLighting.Name = "EditLighting";
            this.EditLighting.Size = new System.Drawing.Size(193, 22);
            this.EditLighting.Text = "$EDIT_LIGHTING";
            this.EditLighting.Click += new System.EventHandler(this.EditLighting_Click);
            // 
            // Button_TestConvert32
            // 
            this.Button_TestConvert32.Name = "Button_TestConvert32";
            this.Button_TestConvert32.Size = new System.Drawing.Size(193, 22);
            this.Button_TestConvert32.Text = "$TEST_CONVERT_32BIT";
            this.Button_TestConvert32.Click += new System.EventHandler(this.Button_TestConvert32_Click);
            // 
            // Button_TestConvert16
            // 
            this.Button_TestConvert16.Name = "Button_TestConvert16";
            this.Button_TestConvert16.Size = new System.Drawing.Size(193, 22);
            this.Button_TestConvert16.Text = "$TEST_CONVERT_16BIT";
            this.Button_TestConvert16.Click += new System.EventHandler(this.Button_TestConvert_Click);
            // 
            // WindowButton
            // 
            this.WindowButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.WindowButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SceneTreeButton,
            this.ObjectPropertiesButton,
            this.ViewOptionProperties});
            this.WindowButton.Image = ((System.Drawing.Image)(resources.GetObject("WindowButton.Image")));
            this.WindowButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.WindowButton.Name = "WindowButton";
            this.WindowButton.Size = new System.Drawing.Size(83, 22);
            this.WindowButton.Text = "$WINDOWS";
            // 
            // SceneTreeButton
            // 
            this.SceneTreeButton.Name = "SceneTreeButton";
            this.SceneTreeButton.Size = new System.Drawing.Size(198, 22);
            this.SceneTreeButton.Text = "$VIEW_SCENE_TREE";
            this.SceneTreeButton.Click += new System.EventHandler(this.SceneTreeOnClicked);
            // 
            // ObjectPropertiesButton
            // 
            this.ObjectPropertiesButton.Name = "ObjectPropertiesButton";
            this.ObjectPropertiesButton.Size = new System.Drawing.Size(198, 22);
            this.ObjectPropertiesButton.Text = "$VIEW_PROPERTY_GRID";
            this.ObjectPropertiesButton.Click += new System.EventHandler(this.PropertyGridOnClicked);
            // 
            // ViewOptionProperties
            // 
            this.ViewOptionProperties.Name = "ViewOptionProperties";
            this.ViewOptionProperties.Size = new System.Drawing.Size(198, 22);
            this.ViewOptionProperties.Text = "$VIEW_OPTIONS";
            this.ViewOptionProperties.Click += new System.EventHandler(this.ViewOptionProperties_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "StaticIcon");
            this.imageList1.Images.SetKeyName(1, "LightIcon");
            // 
            // RenderPanel
            // 
            this.RenderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RenderPanel.Location = new System.Drawing.Point(0, 25);
            this.RenderPanel.Name = "RenderPanel";
            this.RenderPanel.Size = new System.Drawing.Size(1217, 575);
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
            // dockPanel1
            // 
            this.dockPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel1.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingWindow;
            this.dockPanel1.Location = new System.Drawing.Point(0, 25);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.Size = new System.Drawing.Size(1217, 575);
            this.dockPanel1.TabIndex = 0;
            // 
            // FrameBrowser
            // 
            this.FrameBrowser.Filter = "Toolkit Frame Data|*framedata";
            // 
            // Button_DumpTexture
            // 
            this.Button_DumpTexture.Name = "Button_DumpTexture";
            this.Button_DumpTexture.Size = new System.Drawing.Size(193, 22);
            this.Button_DumpTexture.Text = "$DUMP_TEXTURES";
            this.Button_DumpTexture.Click += new System.EventHandler(this.Button_DumpTexture_Click);
            // 
            // MapEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1217, 624);
            this.Controls.Add(this.dockPanel1);
            this.Controls.Add(this.RenderPanel);
            this.Controls.Add(StatusStrip);
            this.Controls.Add(this.ToolbarStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MapEditor";
            this.Text = "Map Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            StatusStrip.ResumeLayout(false);
            StatusStrip.PerformLayout();
            this.ToolbarStrip.ResumeLayout(false);
            this.ToolbarStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip ToolbarStrip;
        private System.Windows.Forms.ToolStripDropDownButton FileButton;
        private System.Windows.Forms.ToolStripMenuItem SaveButton;
        private System.Windows.Forms.ToolStripMenuItem ExitButton;
        private System.Windows.Forms.ToolStripDropDownButton WindowButton;
        private System.Windows.Forms.ToolStripDropDownButton OptionsButton;
        private System.Windows.Forms.Panel RenderPanel;
        private System.Windows.Forms.ToolStripDropDownButton EditButton;
        private System.Windows.Forms.ToolStripMenuItem AddButton;
        private System.Windows.Forms.OpenFileDialog MeshBrowser;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripMenuItem SceneTreeButton;
        private System.Windows.Forms.ToolStripMenuItem ObjectPropertiesButton;
        private System.Windows.Forms.ToolStripMenuItem ToggleWireframeButton;
        private System.Windows.Forms.ToolStripMenuItem ToggleCullingButton;
        private System.Windows.Forms.ToolStripMenuItem AddSceneFolderButton;
        private System.Windows.Forms.ToolStripMenuItem AddRoadSplineButton;
        private System.Windows.Forms.ToolStripMenuItem ViewOptionProperties;
        private System.Windows.Forms.ToolStripMenuItem AddSplineTxT;
        private System.Windows.Forms.OpenFileDialog TxtBrowser;
        private System.Windows.Forms.ToolStripMenuItem AddJunctionButton;
        private System.Windows.Forms.ToolStripMenuItem EditUnkSet3;
        private System.Windows.Forms.ToolStripMenuItem AddBackward;
        private System.Windows.Forms.ToolStripMenuItem AddToward;
        private System.Windows.Forms.ToolStripMenuItem AddCollisionButton;
        private System.Windows.Forms.ToolStripMenuItem roadDebuggingToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel Label_FPS;
        private NumericUpDownToolStrip PositionYTool;
        private NumericUpDownToolStrip PositionZTool;
        private NumericUpDownToolStrip PositionXTool;
        private NumericUpDownToolStrip CameraSpeedTool;
        private System.Windows.Forms.ToolStripDropDownButton ViewButton;
        private System.Windows.Forms.ToolStripMenuItem ViewTopButton;
        private System.Windows.Forms.ToolStripMenuItem ViewFrontButton;
        private System.Windows.Forms.ToolStripMenuItem ViewSideButton;
        private System.Windows.Forms.ToolStripMenuItem ViewBottomButton;
        private System.Windows.Forms.ToolStripMenuItem ViewSide2Button;
        private System.Windows.Forms.ToolStripSplitButton CurrentModeButton;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel1;
        private System.Windows.Forms.ToolStripMenuItem EditLighting;
        private System.Windows.Forms.ToolStripMenuItem Button_TestConvert16;
        private System.Windows.Forms.ToolStripMenuItem Button_TestConvert32;
        private System.Windows.Forms.ToolStripStatusLabel Label_MemoryUsage;
        private System.Windows.Forms.ToolStripMenuItem Button_ImportFrame;
        private System.Windows.Forms.OpenFileDialog FrameBrowser;
        private System.Windows.Forms.ToolStripMenuItem Button_DumpTexture;
    }
}