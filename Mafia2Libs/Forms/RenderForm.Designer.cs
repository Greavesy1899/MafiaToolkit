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
            this.ToggleWireFrameButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ToggleCullingBottle = new System.Windows.Forms.ToolStripMenuItem();
            this.OptionsButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.DisableLODButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ToggleModelsButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ToggleCollisionsButton = new System.Windows.Forms.ToolStripMenuItem();
            this.TEMPCameraSpeed = new System.Windows.Forms.ToolStripTextBox();
            this.NameTableFlagLimit = new System.Windows.Forms.ToolStripTextBox();
            this.EntryMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.PreviewButton = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteButton = new System.Windows.Forms.ToolStripMenuItem();
            this.DuplicateButton = new System.Windows.Forms.ToolStripMenuItem();
            this.Export3DButton = new System.Windows.Forms.ToolStripMenuItem();
            this.StatusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.MainTabControl = new System.Windows.Forms.TabControl();
            this.SceneTab = new System.Windows.Forms.TabPage();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.EditEntryTab = new System.Windows.Forms.TabPage();
            this.RotationZNumeric = new System.Windows.Forms.NumericUpDown();
            this.RotationYNumeric = new System.Windows.Forms.NumericUpDown();
            this.RotationXNumeric = new System.Windows.Forms.NumericUpDown();
            this.PositionZNumeric = new System.Windows.Forms.NumericUpDown();
            this.PositionYNumeric = new System.Windows.Forms.NumericUpDown();
            this.PositionXNumeric = new System.Windows.Forms.NumericUpDown();
            this.OnFrameNameTable = new System.Windows.Forms.CheckBox();
            this.CurrentEntryType = new System.Windows.Forms.Label();
            this.EntryApplyChanges = new System.Windows.Forms.Button();
            this.CurrentEntry = new System.Windows.Forms.Label();
            this.RotationZLabel = new System.Windows.Forms.Label();
            this.RotationYLabel = new System.Windows.Forms.Label();
            this.RotationXLabel = new System.Windows.Forms.Label();
            this.PositionZLabel = new System.Windows.Forms.Label();
            this.PositionYLabel = new System.Windows.Forms.Label();
            this.PositionXLabel = new System.Windows.Forms.Label();
            this.DebugPG = new System.Windows.Forms.TabPage();
            this.DebugPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.RenderPanel = new System.Windows.Forms.Panel();
            this.MeshBrowser = new System.Windows.Forms.OpenFileDialog();
            this.FrameNameTableFlags = new Utils.Extensions.FlagCheckedListBox();
            this.ToolbarStrip.SuspendLayout();
            this.EntryMenuStrip.SuspendLayout();
            this.StatusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.MainTabControl.SuspendLayout();
            this.SceneTab.SuspendLayout();
            this.EditEntryTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RotationZNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotationYNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotationXNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionZNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionYNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionXNumeric)).BeginInit();
            this.DebugPG.SuspendLayout();
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
            this.SaveButton.Size = new System.Drawing.Size(98, 22);
            this.SaveButton.Text = "Save";
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // ExitButton
            // 
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(98, 22);
            this.ExitButton.Text = "Exit";
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // EditButton
            // 
            this.EditButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.EditButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddButton});
            this.EditButton.Image = ((System.Drawing.Image)(resources.GetObject("EditButton.Image")));
            this.EditButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.EditButton.Name = "EditButton";
            this.EditButton.Size = new System.Drawing.Size(40, 22);
            this.EditButton.Text = "Edit";
            // 
            // AddButton
            // 
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(104, 22);
            this.AddButton.Text = "$ADD";
            this.AddButton.Click += new System.EventHandler(this.AddButtonOnClick);
            // 
            // ViewButton
            // 
            this.ViewButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ViewButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToggleWireFrameButton,
            this.ToggleCullingBottle});
            this.ViewButton.Image = ((System.Drawing.Image)(resources.GetObject("ViewButton.Image")));
            this.ViewButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ViewButton.Name = "ViewButton";
            this.ViewButton.Size = new System.Drawing.Size(45, 22);
            this.ViewButton.Text = "View";
            // 
            // ToggleWireFrameButton
            // 
            this.ToggleWireFrameButton.Name = "ToggleWireFrameButton";
            this.ToggleWireFrameButton.Size = new System.Drawing.Size(168, 22);
            this.ToggleWireFrameButton.Text = "Toggle Wireframe";
            this.ToggleWireFrameButton.Click += new System.EventHandler(this.FillModeButton_Click);
            // 
            // ToggleCullingBottle
            // 
            this.ToggleCullingBottle.Name = "ToggleCullingBottle";
            this.ToggleCullingBottle.Size = new System.Drawing.Size(168, 22);
            this.ToggleCullingBottle.Text = "Toggle Culling";
            this.ToggleCullingBottle.Click += new System.EventHandler(this.CullModeButton_Click);
            // 
            // OptionsButton
            // 
            this.OptionsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.OptionsButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DisableLODButton,
            this.ToggleModelsButton,
            this.ToggleCollisionsButton});
            this.OptionsButton.Image = ((System.Drawing.Image)(resources.GetObject("OptionsButton.Image")));
            this.OptionsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.OptionsButton.Name = "OptionsButton";
            this.OptionsButton.Size = new System.Drawing.Size(62, 22);
            this.OptionsButton.Text = "Options";
            // 
            // DisableLODButton
            // 
            this.DisableLODButton.Name = "DisableLODButton";
            this.DisableLODButton.Size = new System.Drawing.Size(164, 22);
            this.DisableLODButton.Text = "Disable LODs";
            this.DisableLODButton.Click += new System.EventHandler(this.DisableLodButtonOnClick);
            // 
            // ToggleModelsButton
            // 
            this.ToggleModelsButton.Checked = true;
            this.ToggleModelsButton.CheckOnClick = true;
            this.ToggleModelsButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToggleModelsButton.Name = "ToggleModelsButton";
            this.ToggleModelsButton.Size = new System.Drawing.Size(164, 22);
            this.ToggleModelsButton.Text = "Toggle Models";
            this.ToggleModelsButton.Click += new System.EventHandler(this.ToggleModelOnClick);
            // 
            // ToggleCollisionsButton
            // 
            this.ToggleCollisionsButton.Checked = true;
            this.ToggleCollisionsButton.CheckOnClick = true;
            this.ToggleCollisionsButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToggleCollisionsButton.Name = "ToggleCollisionsButton";
            this.ToggleCollisionsButton.Size = new System.Drawing.Size(164, 22);
            this.ToggleCollisionsButton.Text = "Toggle Collisions";
            this.ToggleCollisionsButton.Click += new System.EventHandler(this.ToggleCollisionsOnClick);
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
            this.NameTableFlagLimit.TextChanged += new System.EventHandler(this.NameTableFlagValueChanged);
            // 
            // EntryMenuStrip
            // 
            this.EntryMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PreviewButton,
            this.DeleteButton,
            this.DuplicateButton,
            this.Export3DButton});
            this.EntryMenuStrip.Name = "EntryMenuStrip";
            this.EntryMenuStrip.Size = new System.Drawing.Size(125, 92);
            this.EntryMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.OpenEntryContext);
            // 
            // PreviewButton
            // 
            this.PreviewButton.Name = "PreviewButton";
            this.PreviewButton.Size = new System.Drawing.Size(124, 22);
            this.PreviewButton.Text = "Preview";
            this.PreviewButton.Click += new System.EventHandler(this.PreviewButton_Click);
            // 
            // DeleteButton
            // 
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(124, 22);
            this.DeleteButton.Text = "Delete";
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // DuplicateButton
            // 
            this.DuplicateButton.Name = "DuplicateButton";
            this.DuplicateButton.Size = new System.Drawing.Size(124, 22);
            this.DuplicateButton.Text = "Duplicate";
            this.DuplicateButton.Click += new System.EventHandler(this.DuplicateButton_Click);
            // 
            // Export3DButton
            // 
            this.Export3DButton.Name = "Export3DButton";
            this.Export3DButton.Size = new System.Drawing.Size(124, 22);
            this.Export3DButton.Text = "Export 3D";
            this.Export3DButton.Click += new System.EventHandler(this.Export3DButton_Click);
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
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.MainTabControl);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.RenderPanel);
            this.splitContainer1.Size = new System.Drawing.Size(800, 401);
            this.splitContainer1.SplitterDistance = 266;
            this.splitContainer1.TabIndex = 7;
            // 
            // MainTabControl
            // 
            this.MainTabControl.Controls.Add(this.SceneTab);
            this.MainTabControl.Controls.Add(this.EditEntryTab);
            this.MainTabControl.Controls.Add(this.DebugPG);
            this.MainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTabControl.Location = new System.Drawing.Point(0, 0);
            this.MainTabControl.Name = "MainTabControl";
            this.MainTabControl.SelectedIndex = 0;
            this.MainTabControl.Size = new System.Drawing.Size(266, 401);
            this.MainTabControl.TabIndex = 5;
            // 
            // SceneTab
            // 
            this.SceneTab.Controls.Add(this.treeView1);
            this.SceneTab.Location = new System.Drawing.Point(4, 22);
            this.SceneTab.Name = "SceneTab";
            this.SceneTab.Padding = new System.Windows.Forms.Padding(3);
            this.SceneTab.Size = new System.Drawing.Size(258, 375);
            this.SceneTab.TabIndex = 0;
            this.SceneTab.Text = "Scene";
            this.SceneTab.UseVisualStyleBackColor = true;
            // 
            // treeView1
            // 
            this.treeView1.ContextMenuStrip = this.EntryMenuStrip;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.Location = new System.Drawing.Point(3, 3);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(252, 369);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnAfterSelect);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "StaticIcon");
            this.imageList1.Images.SetKeyName(1, "LightIcon");
            // 
            // EditEntryTab
            // 
            this.EditEntryTab.Controls.Add(this.RotationZNumeric);
            this.EditEntryTab.Controls.Add(this.RotationYNumeric);
            this.EditEntryTab.Controls.Add(this.RotationXNumeric);
            this.EditEntryTab.Controls.Add(this.PositionZNumeric);
            this.EditEntryTab.Controls.Add(this.PositionYNumeric);
            this.EditEntryTab.Controls.Add(this.PositionXNumeric);
            this.EditEntryTab.Controls.Add(this.OnFrameNameTable);
            this.EditEntryTab.Controls.Add(this.CurrentEntryType);
            this.EditEntryTab.Controls.Add(this.EntryApplyChanges);
            this.EditEntryTab.Controls.Add(this.CurrentEntry);
            this.EditEntryTab.Controls.Add(this.RotationZLabel);
            this.EditEntryTab.Controls.Add(this.RotationYLabel);
            this.EditEntryTab.Controls.Add(this.RotationXLabel);
            this.EditEntryTab.Controls.Add(this.PositionZLabel);
            this.EditEntryTab.Controls.Add(this.PositionYLabel);
            this.EditEntryTab.Controls.Add(this.PositionXLabel);
            this.EditEntryTab.Controls.Add(this.FrameNameTableFlags);
            this.EditEntryTab.Location = new System.Drawing.Point(4, 22);
            this.EditEntryTab.Name = "EditEntryTab";
            this.EditEntryTab.Padding = new System.Windows.Forms.Padding(3);
            this.EditEntryTab.Size = new System.Drawing.Size(258, 375);
            this.EditEntryTab.TabIndex = 1;
            this.EditEntryTab.Text = "Edit Entry";
            this.EditEntryTab.UseVisualStyleBackColor = true;
            // 
            // RotationZNumeric
            // 
            this.RotationZNumeric.DecimalPlaces = 5;
            this.RotationZNumeric.Location = new System.Drawing.Point(67, 162);
            this.RotationZNumeric.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.RotationZNumeric.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.RotationZNumeric.Name = "RotationZNumeric";
            this.RotationZNumeric.Size = new System.Drawing.Size(185, 20);
            this.RotationZNumeric.TabIndex = 23;
            this.RotationZNumeric.ValueChanged += new System.EventHandler(this.EntryField_ValueChanged);
            // 
            // RotationYNumeric
            // 
            this.RotationYNumeric.DecimalPlaces = 5;
            this.RotationYNumeric.Location = new System.Drawing.Point(67, 136);
            this.RotationYNumeric.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.RotationYNumeric.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.RotationYNumeric.Name = "RotationYNumeric";
            this.RotationYNumeric.Size = new System.Drawing.Size(185, 20);
            this.RotationYNumeric.TabIndex = 22;
            this.RotationYNumeric.ValueChanged += new System.EventHandler(this.EntryField_ValueChanged);
            // 
            // RotationXNumeric
            // 
            this.RotationXNumeric.DecimalPlaces = 5;
            this.RotationXNumeric.Location = new System.Drawing.Point(67, 110);
            this.RotationXNumeric.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.RotationXNumeric.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.RotationXNumeric.Name = "RotationXNumeric";
            this.RotationXNumeric.Size = new System.Drawing.Size(185, 20);
            this.RotationXNumeric.TabIndex = 21;
            this.RotationXNumeric.ValueChanged += new System.EventHandler(this.EntryField_ValueChanged);
            // 
            // PositionZNumeric
            // 
            this.PositionZNumeric.DecimalPlaces = 5;
            this.PositionZNumeric.Location = new System.Drawing.Point(67, 84);
            this.PositionZNumeric.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.PositionZNumeric.Minimum = new decimal(new int[] {
            999999999,
            0,
            0,
            -2147483648});
            this.PositionZNumeric.Name = "PositionZNumeric";
            this.PositionZNumeric.Size = new System.Drawing.Size(185, 20);
            this.PositionZNumeric.TabIndex = 20;
            this.PositionZNumeric.ValueChanged += new System.EventHandler(this.EntryField_ValueChanged);
            // 
            // PositionYNumeric
            // 
            this.PositionYNumeric.DecimalPlaces = 5;
            this.PositionYNumeric.Location = new System.Drawing.Point(67, 58);
            this.PositionYNumeric.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.PositionYNumeric.Minimum = new decimal(new int[] {
            999999999,
            0,
            0,
            -2147483648});
            this.PositionYNumeric.Name = "PositionYNumeric";
            this.PositionYNumeric.Size = new System.Drawing.Size(185, 20);
            this.PositionYNumeric.TabIndex = 19;
            this.PositionYNumeric.ValueChanged += new System.EventHandler(this.EntryField_ValueChanged);
            // 
            // PositionXNumeric
            // 
            this.PositionXNumeric.DecimalPlaces = 5;
            this.PositionXNumeric.Location = new System.Drawing.Point(67, 32);
            this.PositionXNumeric.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.PositionXNumeric.Minimum = new decimal(new int[] {
            999999999,
            0,
            0,
            -2147483648});
            this.PositionXNumeric.Name = "PositionXNumeric";
            this.PositionXNumeric.Size = new System.Drawing.Size(185, 20);
            this.PositionXNumeric.TabIndex = 18;
            this.PositionXNumeric.ValueChanged += new System.EventHandler(this.EntryField_ValueChanged);
            // 
            // OnFrameNameTable
            // 
            this.OnFrameNameTable.AutoSize = true;
            this.OnFrameNameTable.Location = new System.Drawing.Point(13, 199);
            this.OnFrameNameTable.Name = "OnFrameNameTable";
            this.OnFrameNameTable.Size = new System.Drawing.Size(133, 17);
            this.OnFrameNameTable.TabIndex = 17;
            this.OnFrameNameTable.Text = "On FrameNameTable?";
            this.OnFrameNameTable.UseVisualStyleBackColor = true;
            // 
            // CurrentEntryType
            // 
            this.CurrentEntryType.AutoSize = true;
            this.CurrentEntryType.Location = new System.Drawing.Point(10, 339);
            this.CurrentEntryType.Name = "CurrentEntryType";
            this.CurrentEntryType.Size = new System.Drawing.Size(108, 13);
            this.CurrentEntryType.TabIndex = 15;
            this.CurrentEntryType.Text = "FRAME TYPE HERE";
            // 
            // EntryApplyChanges
            // 
            this.EntryApplyChanges.Location = new System.Drawing.Point(10, 355);
            this.EntryApplyChanges.Name = "EntryApplyChanges";
            this.EntryApplyChanges.Size = new System.Drawing.Size(188, 23);
            this.EntryApplyChanges.TabIndex = 14;
            this.EntryApplyChanges.Text = "Apply Changes";
            this.EntryApplyChanges.UseVisualStyleBackColor = true;
            this.EntryApplyChanges.Click += new System.EventHandler(this.EntryApplyChanges_OnClick);
            // 
            // CurrentEntry
            // 
            this.CurrentEntry.AutoSize = true;
            this.CurrentEntry.Location = new System.Drawing.Point(10, 7);
            this.CurrentEntry.Name = "CurrentEntry";
            this.CurrentEntry.Size = new System.Drawing.Size(111, 13);
            this.CurrentEntry.TabIndex = 12;
            this.CurrentEntry.Text = "FRAME NAME HERE";
            // 
            // RotationZLabel
            // 
            this.RotationZLabel.AutoSize = true;
            this.RotationZLabel.Location = new System.Drawing.Point(7, 164);
            this.RotationZLabel.Name = "RotationZLabel";
            this.RotationZLabel.Size = new System.Drawing.Size(57, 13);
            this.RotationZLabel.TabIndex = 10;
            this.RotationZLabel.Text = "Rotation Z";
            // 
            // RotationYLabel
            // 
            this.RotationYLabel.AutoSize = true;
            this.RotationYLabel.Location = new System.Drawing.Point(7, 138);
            this.RotationYLabel.Name = "RotationYLabel";
            this.RotationYLabel.Size = new System.Drawing.Size(57, 13);
            this.RotationYLabel.TabIndex = 8;
            this.RotationYLabel.Text = "Rotation Y";
            // 
            // RotationXLabel
            // 
            this.RotationXLabel.AutoSize = true;
            this.RotationXLabel.Location = new System.Drawing.Point(7, 112);
            this.RotationXLabel.Name = "RotationXLabel";
            this.RotationXLabel.Size = new System.Drawing.Size(57, 13);
            this.RotationXLabel.TabIndex = 6;
            this.RotationXLabel.Text = "Rotation X";
            // 
            // PositionZLabel
            // 
            this.PositionZLabel.AutoSize = true;
            this.PositionZLabel.Location = new System.Drawing.Point(7, 86);
            this.PositionZLabel.Name = "PositionZLabel";
            this.PositionZLabel.Size = new System.Drawing.Size(54, 13);
            this.PositionZLabel.TabIndex = 4;
            this.PositionZLabel.Text = "Position Z";
            // 
            // PositionYLabel
            // 
            this.PositionYLabel.AutoSize = true;
            this.PositionYLabel.Location = new System.Drawing.Point(7, 60);
            this.PositionYLabel.Name = "PositionYLabel";
            this.PositionYLabel.Size = new System.Drawing.Size(54, 13);
            this.PositionYLabel.TabIndex = 2;
            this.PositionYLabel.Text = "Position Y";
            // 
            // PositionXLabel
            // 
            this.PositionXLabel.AutoSize = true;
            this.PositionXLabel.Location = new System.Drawing.Point(7, 34);
            this.PositionXLabel.Name = "PositionXLabel";
            this.PositionXLabel.Size = new System.Drawing.Size(54, 13);
            this.PositionXLabel.TabIndex = 0;
            this.PositionXLabel.Text = "Position X";
            // 
            // DebugPG
            // 
            this.DebugPG.Controls.Add(this.DebugPropertyGrid);
            this.DebugPG.Location = new System.Drawing.Point(4, 22);
            this.DebugPG.Name = "DebugPG";
            this.DebugPG.Size = new System.Drawing.Size(258, 375);
            this.DebugPG.TabIndex = 2;
            this.DebugPG.Text = "Debug";
            this.DebugPG.UseVisualStyleBackColor = true;
            // 
            // DebugPropertyGrid
            // 
            this.DebugPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DebugPropertyGrid.Location = new System.Drawing.Point(0, 0);
            this.DebugPropertyGrid.Name = "DebugPropertyGrid";
            this.DebugPropertyGrid.Size = new System.Drawing.Size(258, 375);
            this.DebugPropertyGrid.TabIndex = 2;
            this.DebugPropertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.OnPropertyChanged);
            // 
            // RenderPanel
            // 
            this.RenderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RenderPanel.Location = new System.Drawing.Point(0, 0);
            this.RenderPanel.Name = "RenderPanel";
            this.RenderPanel.Size = new System.Drawing.Size(530, 401);
            this.RenderPanel.TabIndex = 0;
            // 
            // MeshBrowser
            // 
            this.MeshBrowser.Filter = "Meshes|*.m2t|FBX|*.fbx";
            // 
            // FrameNameTableFlags
            // 
            this.FrameNameTableFlags.CheckOnClick = true;
            this.FrameNameTableFlags.FormattingEnabled = true;
            this.FrameNameTableFlags.Location = new System.Drawing.Point(10, 222);
            this.FrameNameTableFlags.Name = "FrameNameTableFlags";
            this.FrameNameTableFlags.Size = new System.Drawing.Size(188, 94);
            this.FrameNameTableFlags.TabIndex = 16;
            // 
            // D3DForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.StatusStrip);
            this.Controls.Add(this.ToolbarStrip);
            this.Name = "D3DForm";
            this.Text = "Map Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.ToolbarStrip.ResumeLayout(false);
            this.ToolbarStrip.PerformLayout();
            this.EntryMenuStrip.ResumeLayout(false);
            this.StatusStrip.ResumeLayout(false);
            this.StatusStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.MainTabControl.ResumeLayout(false);
            this.SceneTab.ResumeLayout(false);
            this.EditEntryTab.ResumeLayout(false);
            this.EditEntryTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RotationZNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotationYNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotationXNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionZNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionYNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionXNumeric)).EndInit();
            this.DebugPG.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip ToolbarStrip;
        private System.Windows.Forms.ToolStripDropDownButton FileButton;
        private System.Windows.Forms.ToolStripMenuItem SaveButton;
        private System.Windows.Forms.ToolStripMenuItem ExitButton;
        private System.Windows.Forms.ToolStripDropDownButton ViewButton;
        private System.Windows.Forms.ToolStripMenuItem ToggleWireFrameButton;
        private System.Windows.Forms.ToolStripMenuItem ToggleCullingBottle;
        private System.Windows.Forms.StatusStrip StatusStrip;
        private System.Windows.Forms.ToolStripDropDownButton OptionsButton;
        private System.Windows.Forms.ContextMenuStrip EntryMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem PreviewButton;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl MainTabControl;
        private System.Windows.Forms.TabPage SceneTab;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TabPage EditEntryTab;
        private System.Windows.Forms.CheckBox OnFrameNameTable;
        private System.Windows.Forms.Label CurrentEntryType;
        private System.Windows.Forms.Button EntryApplyChanges;
        private System.Windows.Forms.Label CurrentEntry;
        private System.Windows.Forms.Label RotationZLabel;
        private System.Windows.Forms.Label RotationYLabel;
        private System.Windows.Forms.Label RotationXLabel;
        private System.Windows.Forms.Label PositionZLabel;
        private System.Windows.Forms.Label PositionYLabel;
        private System.Windows.Forms.Label PositionXLabel;
        private FlagCheckedListBox FrameNameTableFlags;
        private System.Windows.Forms.TabPage DebugPG;
        private System.Windows.Forms.PropertyGrid DebugPropertyGrid;
        private System.Windows.Forms.Panel RenderPanel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripTextBox TEMPCameraSpeed;
        private System.Windows.Forms.ToolStripMenuItem DeleteButton;
        private System.Windows.Forms.ToolStripMenuItem DuplicateButton;
        private System.Windows.Forms.ToolStripMenuItem DisableLODButton;
        private System.Windows.Forms.ToolStripTextBox NameTableFlagLimit;
        private System.Windows.Forms.NumericUpDown RotationZNumeric;
        private System.Windows.Forms.NumericUpDown RotationYNumeric;
        private System.Windows.Forms.NumericUpDown RotationXNumeric;
        private System.Windows.Forms.NumericUpDown PositionZNumeric;
        private System.Windows.Forms.NumericUpDown PositionYNumeric;
        private System.Windows.Forms.NumericUpDown PositionXNumeric;
        private System.Windows.Forms.ToolStripMenuItem Export3DButton;
        private System.Windows.Forms.ToolStripMenuItem ToggleCollisionsButton;
        private System.Windows.Forms.ToolStripMenuItem ToggleModelsButton;
        private System.Windows.Forms.ToolStripDropDownButton EditButton;
        private System.Windows.Forms.ToolStripMenuItem AddButton;
        private System.Windows.Forms.OpenFileDialog MeshBrowser;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ImageList imageList1;
    }
}