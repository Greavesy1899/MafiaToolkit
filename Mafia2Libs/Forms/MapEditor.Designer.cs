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
            components = new System.ComponentModel.Container();
            System.Windows.Forms.StatusStrip StatusStrip;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapEditor));
            CurrentModeButton = new System.Windows.Forms.ToolStripSplitButton();
            PositionXTool = new NumericUpDownToolStrip();
            PositionYTool = new NumericUpDownToolStrip();
            PositionZTool = new NumericUpDownToolStrip();
            CameraSpeedTool = new NumericUpDownToolStrip();
            Label_FPS = new System.Windows.Forms.ToolStripStatusLabel();
            Label_MemoryUsage = new System.Windows.Forms.ToolStripStatusLabel();
            Label_StatusBar = new System.Windows.Forms.ToolStripStatusLabel();
            ToolbarStrip = new System.Windows.Forms.ToolStrip();
            FileButton = new System.Windows.Forms.ToolStripDropDownButton();
            SaveButton = new System.Windows.Forms.ToolStripMenuItem();
            ExitButton = new System.Windows.Forms.ToolStripMenuItem();
            EditButton = new System.Windows.Forms.ToolStripDropDownButton();
            AddButton = new System.Windows.Forms.ToolStripMenuItem();
            Button_ImportFrame = new System.Windows.Forms.ToolStripMenuItem();
            Button_ImportBundle = new System.Windows.Forms.ToolStripMenuItem();
            AddSceneFolderButton = new System.Windows.Forms.ToolStripMenuItem();
            ViewButton = new System.Windows.Forms.ToolStripDropDownButton();
            ViewTopButton = new System.Windows.Forms.ToolStripMenuItem();
            ViewFrontButton = new System.Windows.Forms.ToolStripMenuItem();
            ViewSideButton = new System.Windows.Forms.ToolStripMenuItem();
            ViewBottomButton = new System.Windows.Forms.ToolStripMenuItem();
            ViewSide2Button = new System.Windows.Forms.ToolStripMenuItem();
            OptionsButton = new System.Windows.Forms.ToolStripDropDownButton();
            ToggleWireframeButton = new System.Windows.Forms.ToolStripMenuItem();
            ToggleCullingButton = new System.Windows.Forms.ToolStripMenuItem();
            EditLighting = new System.Windows.Forms.ToolStripMenuItem();
            ToggleTranslokatorTint = new System.Windows.Forms.ToolStripMenuItem();
            Button_TestConvert32 = new System.Windows.Forms.ToolStripMenuItem();
            Button_TestConvert16 = new System.Windows.Forms.ToolStripMenuItem();
            Button_DumpTexture = new System.Windows.Forms.ToolStripMenuItem();
            WindowButton = new System.Windows.Forms.ToolStripDropDownButton();
            SceneTreeButton = new System.Windows.Forms.ToolStripMenuItem();
            ObjectPropertiesButton = new System.Windows.Forms.ToolStripMenuItem();
            ViewOptionProperties = new System.Windows.Forms.ToolStripMenuItem();
            imageList1 = new System.Windows.Forms.ImageList(components);
            RenderPanel = new System.Windows.Forms.Panel();
            MeshBrowser = new System.Windows.Forms.OpenFileDialog();
            TxtBrowser = new System.Windows.Forms.OpenFileDialog();
            dockPanel1 = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            FrameBrowser = new System.Windows.Forms.OpenFileDialog();
            SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            AnimFileDialog = new System.Windows.Forms.OpenFileDialog();
            StatusStrip = new System.Windows.Forms.StatusStrip();
            StatusStrip.SuspendLayout();
            ToolbarStrip.SuspendLayout();
            SuspendLayout();
            // 
            // StatusStrip
            // 
            StatusStrip.AutoSize = false;
            StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { CurrentModeButton, PositionXTool, PositionYTool, PositionZTool, CameraSpeedTool, Label_FPS, Label_MemoryUsage, Label_StatusBar });
            StatusStrip.Location = new System.Drawing.Point(0, 692);
            StatusStrip.Name = "StatusStrip";
            StatusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            StatusStrip.Size = new System.Drawing.Size(1420, 28);
            StatusStrip.TabIndex = 6;
            StatusStrip.Text = "statusStrip1";
            // 
            // CurrentModeButton
            // 
            CurrentModeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            CurrentModeButton.Image = (System.Drawing.Image)resources.GetObject("CurrentModeButton.Image");
            CurrentModeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            CurrentModeButton.Name = "CurrentModeButton";
            CurrentModeButton.Size = new System.Drawing.Size(128, 26);
            CurrentModeButton.Text = "$CurrentModeLabel";
            CurrentModeButton.ButtonClick += CurrentModeButton_ButtonClick;
            // 
            // PositionXTool
            // 
            PositionXTool.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            PositionXTool.AutoSize = false;
            PositionXTool.DecimalPlaces = 5;
            PositionXTool.Increment = new decimal(new int[] { 1, 0, 0, 0 });
            PositionXTool.Margin = new System.Windows.Forms.Padding(3, 0, 1, 0);
            PositionXTool.Maximum = new decimal(new int[] { 9999999, 0, 0, 0 });
            PositionXTool.Minimum = new decimal(new int[] { 9999999, 0, 0, int.MinValue });
            PositionXTool.Name = "PositionXTool";
            PositionXTool.Overflow = System.Windows.Forms.ToolStripItemOverflow.Always;
            PositionXTool.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            PositionXTool.Size = new System.Drawing.Size(84, 28);
            PositionXTool.Text = "0.00000";
            PositionXTool.Value = new decimal(new int[] { 0, 0, 0, 0 });
            PositionXTool.ValueChanged += CameraToolsOnValueChanged;
            // 
            // PositionYTool
            // 
            PositionYTool.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            PositionYTool.AutoSize = false;
            PositionYTool.DecimalPlaces = 5;
            PositionYTool.Increment = new decimal(new int[] { 1, 0, 0, 0 });
            PositionYTool.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            PositionYTool.Maximum = new decimal(new int[] { 9999999, 0, 0, 0 });
            PositionYTool.Minimum = new decimal(new int[] { 9999999, 0, 0, int.MinValue });
            PositionYTool.Name = "PositionYTool";
            PositionYTool.Overflow = System.Windows.Forms.ToolStripItemOverflow.Always;
            PositionYTool.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            PositionYTool.Size = new System.Drawing.Size(84, 28);
            PositionYTool.Text = "0.00000";
            PositionYTool.Value = new decimal(new int[] { 0, 0, 0, 0 });
            PositionYTool.ValueChanged += CameraToolsOnValueChanged;
            // 
            // PositionZTool
            // 
            PositionZTool.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            PositionZTool.AutoSize = false;
            PositionZTool.DecimalPlaces = 5;
            PositionZTool.Increment = new decimal(new int[] { 1, 0, 0, 0 });
            PositionZTool.Margin = new System.Windows.Forms.Padding(1, 0, 3, 0);
            PositionZTool.Maximum = new decimal(new int[] { 9999999, 0, 0, 0 });
            PositionZTool.Minimum = new decimal(new int[] { 9999999, 0, 0, int.MinValue });
            PositionZTool.Name = "PositionZTool";
            PositionZTool.Overflow = System.Windows.Forms.ToolStripItemOverflow.Always;
            PositionZTool.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            PositionZTool.Size = new System.Drawing.Size(84, 28);
            PositionZTool.Text = "0.00000";
            PositionZTool.Value = new decimal(new int[] { 0, 0, 0, 0 });
            PositionZTool.ValueChanged += CameraToolsOnValueChanged;
            // 
            // CameraSpeedTool
            // 
            CameraSpeedTool.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            CameraSpeedTool.AutoSize = false;
            CameraSpeedTool.DecimalPlaces = 5;
            CameraSpeedTool.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            CameraSpeedTool.Margin = new System.Windows.Forms.Padding(1, 0, 3, 0);
            CameraSpeedTool.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            CameraSpeedTool.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
            CameraSpeedTool.Name = "CameraSpeedTool";
            CameraSpeedTool.Overflow = System.Windows.Forms.ToolStripItemOverflow.Always;
            CameraSpeedTool.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            CameraSpeedTool.Size = new System.Drawing.Size(84, 28);
            CameraSpeedTool.Text = "0.00000";
            CameraSpeedTool.Value = new decimal(new int[] { 0, 0, 0, 0 });
            CameraSpeedTool.ValueChanged += CameraSpeedUpdate;
            // 
            // Label_FPS
            // 
            Label_FPS.Name = "Label_FPS";
            Label_FPS.Overflow = System.Windows.Forms.ToolStripItemOverflow.Always;
            Label_FPS.Padding = new System.Windows.Forms.Padding(41, 0, 0, 0);
            Label_FPS.Size = new System.Drawing.Size(100, 23);
            Label_FPS.Text = "Label_FPS";
            // 
            // Label_MemoryUsage
            // 
            Label_MemoryUsage.Name = "Label_MemoryUsage";
            Label_MemoryUsage.Size = new System.Drawing.Size(117, 23);
            Label_MemoryUsage.Text = "Label_MemoryUsage";
            // 
            // Label_StatusBar
            // 
            Label_StatusBar.Name = "Label_StatusBar";
            Label_StatusBar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            Label_StatusBar.Size = new System.Drawing.Size(677, 23);
            Label_StatusBar.Spring = true;
            Label_StatusBar.Text = "Label_StatusBar";
            Label_StatusBar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ToolbarStrip
            // 
            ToolbarStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { FileButton, EditButton, ViewButton, OptionsButton, WindowButton });
            ToolbarStrip.Location = new System.Drawing.Point(0, 0);
            ToolbarStrip.Name = "ToolbarStrip";
            ToolbarStrip.Size = new System.Drawing.Size(1420, 25);
            ToolbarStrip.TabIndex = 1;
            ToolbarStrip.Text = "toolStrip1";
            // 
            // FileButton
            // 
            FileButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            FileButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { SaveButton, ExitButton });
            FileButton.Image = (System.Drawing.Image)resources.GetObject("FileButton.Image");
            FileButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            FileButton.Name = "FileButton";
            FileButton.Size = new System.Drawing.Size(47, 22);
            FileButton.Text = "$FILE";
            // 
            // SaveButton
            // 
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new System.Drawing.Size(106, 22);
            SaveButton.Text = "$SAVE";
            SaveButton.Click += SaveButton_Click;
            // 
            // ExitButton
            // 
            ExitButton.Name = "ExitButton";
            ExitButton.Size = new System.Drawing.Size(106, 22);
            ExitButton.Text = "$EXIT";
            ExitButton.Click += ExitButton_Click;
            // 
            // EditButton
            // 
            EditButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            EditButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { AddButton, Button_ImportFrame, Button_ImportBundle, AddSceneFolderButton });
            EditButton.Image = (System.Drawing.Image)resources.GetObject("EditButton.Image");
            EditButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            EditButton.Name = "EditButton";
            EditButton.Size = new System.Drawing.Size(66, 22);
            EditButton.Text = "$CREATE";
            // 
            // AddButton
            // 
            AddButton.Name = "AddButton";
            AddButton.Size = new System.Drawing.Size(191, 22);
            AddButton.Text = "$ADD";
            AddButton.Click += AddButtonOnClick;
            // 
            // Button_ImportFrame
            // 
            Button_ImportFrame.Name = "Button_ImportFrame";
            Button_ImportFrame.Size = new System.Drawing.Size(191, 22);
            Button_ImportFrame.Text = "$IMPORT_FRAME";
            Button_ImportFrame.Click += Button_ImportFrame_OnClicked;
            // 
            // Button_ImportBundle
            // 
            Button_ImportBundle.Name = "Button_ImportBundle";
            Button_ImportBundle.Size = new System.Drawing.Size(191, 22);
            Button_ImportBundle.Text = "$IMPORT_BUNDLE";
            Button_ImportBundle.Click += Button_ImportBundle_OnClick;
            // 
            // AddSceneFolderButton
            // 
            AddSceneFolderButton.Name = "AddSceneFolderButton";
            AddSceneFolderButton.Size = new System.Drawing.Size(191, 22);
            AddSceneFolderButton.Text = "$ADD_SCENE_FOLDER";
            AddSceneFolderButton.Click += AddSceneFolderButton_Click;
            // 
            // ViewButton
            // 
            ViewButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            ViewButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { ViewTopButton, ViewFrontButton, ViewSideButton, ViewBottomButton, ViewSide2Button });
            ViewButton.Image = (System.Drawing.Image)resources.GetObject("ViewButton.Image");
            ViewButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            ViewButton.Name = "ViewButton";
            ViewButton.Size = new System.Drawing.Size(53, 22);
            ViewButton.Text = "$VIEW";
            // 
            // ViewTopButton
            // 
            ViewTopButton.Name = "ViewTopButton";
            ViewTopButton.Size = new System.Drawing.Size(126, 22);
            ViewTopButton.Text = "$TOP";
            ViewTopButton.Click += OnViewTopButtonClicked;
            // 
            // ViewFrontButton
            // 
            ViewFrontButton.Name = "ViewFrontButton";
            ViewFrontButton.Size = new System.Drawing.Size(126, 22);
            ViewFrontButton.Text = "$FRONT";
            ViewFrontButton.Click += OnViewFrontButtonClicked;
            // 
            // ViewSideButton
            // 
            ViewSideButton.Enabled = false;
            ViewSideButton.Name = "ViewSideButton";
            ViewSideButton.Size = new System.Drawing.Size(126, 22);
            ViewSideButton.Text = "$SIDE";
            ViewSideButton.Visible = false;
            ViewSideButton.Click += OnViewSideButtonClicked;
            // 
            // ViewBottomButton
            // 
            ViewBottomButton.Enabled = false;
            ViewBottomButton.Name = "ViewBottomButton";
            ViewBottomButton.Size = new System.Drawing.Size(126, 22);
            ViewBottomButton.Text = "$BOTTOM";
            ViewBottomButton.Visible = false;
            ViewBottomButton.Click += OnViewBottomButtonClicked;
            // 
            // ViewSide2Button
            // 
            ViewSide2Button.Enabled = false;
            ViewSide2Button.Name = "ViewSide2Button";
            ViewSide2Button.Size = new System.Drawing.Size(126, 22);
            ViewSide2Button.Text = "$SIDE 2";
            ViewSide2Button.Visible = false;
            ViewSide2Button.Click += OnViewSide2ButtonClicked;
            // 
            // OptionsButton
            // 
            OptionsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            OptionsButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { ToggleWireframeButton, ToggleCullingButton, EditLighting, ToggleTranslokatorTint, Button_TestConvert32, Button_TestConvert16, Button_DumpTexture });
            OptionsButton.Image = (System.Drawing.Image)resources.GetObject("OptionsButton.Image");
            OptionsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            OptionsButton.Name = "OptionsButton";
            OptionsButton.Size = new System.Drawing.Size(75, 22);
            OptionsButton.Text = "$OPTIONS";
            // 
            // ToggleWireframeButton
            // 
            ToggleWireframeButton.Name = "ToggleWireframeButton";
            ToggleWireframeButton.Size = new System.Drawing.Size(242, 22);
            ToggleWireframeButton.Text = "$TOGGLE_WIREFRAME";
            ToggleWireframeButton.Click += FillModeButton_Click;
            // 
            // ToggleCullingButton
            // 
            ToggleCullingButton.Name = "ToggleCullingButton";
            ToggleCullingButton.Size = new System.Drawing.Size(242, 22);
            ToggleCullingButton.Text = "$TOGGLE_CULLING";
            ToggleCullingButton.Click += CullModeButton_Click;
            // 
            // EditLighting
            // 
            EditLighting.Name = "EditLighting";
            EditLighting.Size = new System.Drawing.Size(242, 22);
            EditLighting.Text = "$EDIT_LIGHTING";
            EditLighting.Click += EditLighting_Click;
            // 
            // ToggleTranslokatorTint
            // 
            ToggleTranslokatorTint.Enabled = false;
            ToggleTranslokatorTint.Name = "ToggleTranslokatorTint";
            ToggleTranslokatorTint.Size = new System.Drawing.Size(242, 22);
            ToggleTranslokatorTint.Text = "$TOGGLE_TRANSLOKATOR_TINT";
            ToggleTranslokatorTint.Click += TranslokatorTint_Click;
            // 
            // Button_TestConvert32
            // 
            Button_TestConvert32.Name = "Button_TestConvert32";
            Button_TestConvert32.Size = new System.Drawing.Size(242, 22);
            Button_TestConvert32.Text = "$TEST_CONVERT_32BIT";
            Button_TestConvert32.Click += Button_TestConvert32_Click;
            // 
            // Button_TestConvert16
            // 
            Button_TestConvert16.Name = "Button_TestConvert16";
            Button_TestConvert16.Size = new System.Drawing.Size(242, 22);
            Button_TestConvert16.Text = "$TEST_CONVERT_16BIT";
            Button_TestConvert16.Click += Button_TestConvert_Click;
            // 
            // Button_DumpTexture
            // 
            Button_DumpTexture.Name = "Button_DumpTexture";
            Button_DumpTexture.Size = new System.Drawing.Size(242, 22);
            Button_DumpTexture.Text = "$DUMP_TEXTURES";
            Button_DumpTexture.Click += Button_DumpTexture_Click;
            // 
            // WindowButton
            // 
            WindowButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            WindowButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { SceneTreeButton, ObjectPropertiesButton, ViewOptionProperties });
            WindowButton.Image = (System.Drawing.Image)resources.GetObject("WindowButton.Image");
            WindowButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            WindowButton.Name = "WindowButton";
            WindowButton.Size = new System.Drawing.Size(83, 22);
            WindowButton.Text = "$WINDOWS";
            // 
            // SceneTreeButton
            // 
            SceneTreeButton.Name = "SceneTreeButton";
            SceneTreeButton.Size = new System.Drawing.Size(198, 22);
            SceneTreeButton.Text = "$VIEW_SCENE_TREE";
            SceneTreeButton.Click += SceneTreeOnClicked;
            // 
            // ObjectPropertiesButton
            // 
            ObjectPropertiesButton.Name = "ObjectPropertiesButton";
            ObjectPropertiesButton.Size = new System.Drawing.Size(198, 22);
            ObjectPropertiesButton.Text = "$VIEW_PROPERTY_GRID";
            ObjectPropertiesButton.Click += PropertyGridOnClicked;
            // 
            // ViewOptionProperties
            // 
            ViewOptionProperties.Name = "ViewOptionProperties";
            ViewOptionProperties.Size = new System.Drawing.Size(198, 22);
            ViewOptionProperties.Text = "$VIEW_OPTIONS";
            ViewOptionProperties.Click += ViewOptionProperties_Click;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            imageList1.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = System.Drawing.Color.Transparent;
            imageList1.Images.SetKeyName(0, "StaticIcon");
            imageList1.Images.SetKeyName(1, "LightIcon");
            // 
            // RenderPanel
            // 
            RenderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            RenderPanel.Location = new System.Drawing.Point(0, 25);
            RenderPanel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RenderPanel.Name = "RenderPanel";
            RenderPanel.Size = new System.Drawing.Size(1420, 667);
            RenderPanel.TabIndex = 0;
            // 
            // MeshBrowser
            // 
            MeshBrowser.Filter = "Bundle|*.mtb|FBX|*.fbx";
            // 
            // TxtBrowser
            // 
            TxtBrowser.Filter = "Text Document|*txt";
            // 
            // dockPanel1
            // 
            dockPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            dockPanel1.Location = new System.Drawing.Point(0, 25);
            dockPanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            dockPanel1.Name = "dockPanel1";
            dockPanel1.Size = new System.Drawing.Size(1420, 667);
            dockPanel1.TabIndex = 0;
            // 
            // FrameBrowser
            // 
            FrameBrowser.Filter = "FrameResource|*.fr|Toolkit Frame Data|*.framedata";
            // 
            // AnimFileDialog
            // 
            AnimFileDialog.Filter = "Animation2|*an2";
            // 
            // MapEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1420, 720);
            Controls.Add(dockPanel1);
            Controls.Add(RenderPanel);
            Controls.Add(StatusStrip);
            Controls.Add(ToolbarStrip);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "MapEditor";
            Text = "Map Editor";
            FormClosing += OnFormClosing;
            StatusStrip.ResumeLayout(false);
            StatusStrip.PerformLayout();
            ToolbarStrip.ResumeLayout(false);
            ToolbarStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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
        private System.Windows.Forms.ToolStripMenuItem ViewOptionProperties;
        private System.Windows.Forms.OpenFileDialog TxtBrowser;
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
        private System.Windows.Forms.ToolStripMenuItem ToggleTranslokatorTint;
        private System.Windows.Forms.ToolStripMenuItem Button_TestConvert16;
        private System.Windows.Forms.ToolStripMenuItem Button_TestConvert32;
        private System.Windows.Forms.ToolStripStatusLabel Label_MemoryUsage;
        private System.Windows.Forms.ToolStripMenuItem Button_ImportFrame;
        private System.Windows.Forms.OpenFileDialog FrameBrowser;
        private System.Windows.Forms.ToolStripMenuItem Button_DumpTexture;
        private System.Windows.Forms.ToolStripMenuItem Button_ImportBundle;
        private System.Windows.Forms.SaveFileDialog SaveFileDialog;
        private System.Windows.Forms.OpenFileDialog AnimFileDialog;
        private System.Windows.Forms.ToolStripStatusLabel Label_StatusBar;
    }
}