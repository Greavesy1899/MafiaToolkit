using System.Windows.Forms;

namespace Mafia2Tool
{
    partial class GameExplorer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameExplorer));
            this.mainContainer = new System.Windows.Forms.SplitContainer();
            this.folderView = new System.Windows.Forms.TreeView();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.buttonStripUp = new System.Windows.Forms.ToolStripButton();
            this.textStripFolderPath = new System.Windows.Forms.ToolStripTextBox();
            this.buttonStripRefresh = new System.Windows.Forms.ToolStripButton();
            this.textStripSearch = new System.Windows.Forms.ToolStripTextBox();
            this.fileListView = new System.Windows.Forms.ListView();
            this.columnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnLastModified = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SDSContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ContextSDSUnpack = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextSDSPack = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextOpenFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextSDSUnpackAll = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextView = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextViewIcon = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextViewDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextViewSmallIcon = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextViewList = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextViewTile = new System.Windows.Forms.ToolStripMenuItem();
            this.imageBank = new System.Windows.Forms.ImageList(this.components);
            this.topContainer = new System.Windows.Forms.ToolStripContainer();
            this.tools = new System.Windows.Forms.ToolStrip();
            this.dropdownFile = new System.Windows.Forms.ToolStripDropDownButton();
            this.openMafiaIIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runMafiaIIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dropdownView = new System.Windows.Forms.ToolStripDropDownButton();
            this.dropdownTools = new System.Windows.Forms.ToolStripDropDownButton();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bottomContainer = new System.Windows.Forms.ToolStripContainer();
            this.status = new System.Windows.Forms.StatusStrip();
            this.infoText = new System.Windows.Forms.ToolStripStatusLabel();
            this.MafiaIIBrowser = new System.Windows.Forms.FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)(this.mainContainer)).BeginInit();
            this.mainContainer.Panel1.SuspendLayout();
            this.mainContainer.Panel2.SuspendLayout();
            this.mainContainer.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.SDSContext.SuspendLayout();
            this.topContainer.ContentPanel.SuspendLayout();
            this.topContainer.SuspendLayout();
            this.tools.SuspendLayout();
            this.bottomContainer.ContentPanel.SuspendLayout();
            this.bottomContainer.SuspendLayout();
            this.status.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainContainer
            // 
            this.mainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainContainer.Location = new System.Drawing.Point(0, 23);
            this.mainContainer.Name = "mainContainer";
            // 
            // mainContainer.Panel1
            // 
            this.mainContainer.Panel1.Controls.Add(this.folderView);
            // 
            // mainContainer.Panel2
            // 
            this.mainContainer.Panel2.Controls.Add(this.toolStripContainer1);
            this.mainContainer.Panel2.Controls.Add(this.fileListView);
            this.mainContainer.Size = new System.Drawing.Size(800, 401);
            this.mainContainer.SplitterDistance = 266;
            this.mainContainer.TabIndex = 0;
            // 
            // folderView
            // 
            this.folderView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.folderView.Location = new System.Drawing.Point(0, 0);
            this.folderView.Name = "folderView";
            this.folderView.Size = new System.Drawing.Size(266, 401);
            this.folderView.TabIndex = 0;
            this.folderView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            // 
            // toolStripContainer1
            // 
            this.toolStripContainer1.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.toolStrip2);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(530, 29);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            this.toolStripContainer1.Size = new System.Drawing.Size(530, 29);
            this.toolStripContainer1.TabIndex = 1;
            this.toolStripContainer1.Text = "toolStripContainer1";
            this.toolStripContainer1.TopToolStripPanelVisible = false;
            // 
            // toolStrip2
            // 
            this.toolStrip2.CanOverflow = false;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttonStripUp,
            this.textStripFolderPath,
            this.buttonStripRefresh,
            this.textStripSearch});
            this.toolStrip2.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(530, 23);
            this.toolStrip2.TabIndex = 2;
            this.toolStrip2.Text = "toolStrip1";
            this.toolStrip2.Resize += new System.EventHandler(this.toolStrip1_Resize);
            // 
            // buttonStripUp
            // 
            this.buttonStripUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonStripUp.Image = ((System.Drawing.Image)(resources.GetObject("buttonStripUp.Image")));
            this.buttonStripUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonStripUp.Name = "buttonStripUp";
            this.buttonStripUp.Size = new System.Drawing.Size(23, 20);
            this.buttonStripUp.Text = "buttonStripUp";
            this.buttonStripUp.ToolTipText = "$UP_TOOLTIP";
            this.buttonStripUp.Click += new System.EventHandler(this.buttonStripUp_Click);
            // 
            // textStripFolderPath
            // 
            this.textStripFolderPath.AutoSize = false;
            this.textStripFolderPath.Name = "textStripFolderPath";
            this.textStripFolderPath.Size = new System.Drawing.Size(200, 23);
            this.textStripFolderPath.ToolTipText = "$FOLDER_PATH_TOOLTIP";
            this.textStripFolderPath.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.onPathChange);
            // 
            // buttonStripRefresh
            // 
            this.buttonStripRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonStripRefresh.Image = ((System.Drawing.Image)(resources.GetObject("buttonStripRefresh.Image")));
            this.buttonStripRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonStripRefresh.Name = "buttonStripRefresh";
            this.buttonStripRefresh.Size = new System.Drawing.Size(23, 20);
            this.buttonStripRefresh.Text = "$REFRESH";
            this.buttonStripRefresh.Click += new System.EventHandler(this.buttonStripRefresh_Click);
            // 
            // textStripSearch
            // 
            this.textStripSearch.AutoSize = false;
            this.textStripSearch.Name = "textStripSearch";
            this.textStripSearch.Size = new System.Drawing.Size(200, 23);
            this.textStripSearch.ToolTipText = "$SEARCH_TOOLTIP";
            this.textStripSearch.TextChanged += new System.EventHandler(this.SearchBarOnTextChanged);
            // 
            // fileListView
            // 
            this.fileListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnName,
            this.columnType,
            this.columnSize,
            this.columnLastModified});
            this.fileListView.ContextMenuStrip = this.SDSContext;
            this.fileListView.LargeImageList = this.imageBank;
            this.fileListView.Location = new System.Drawing.Point(3, 30);
            this.fileListView.Name = "fileListView";
            this.fileListView.Size = new System.Drawing.Size(530, 371);
            this.fileListView.SmallImageList = this.imageBank;
            this.fileListView.TabIndex = 0;
            this.fileListView.UseCompatibleStateImageBehavior = false;
            this.fileListView.View = System.Windows.Forms.View.Details;
            this.fileListView.ItemActivate += new System.EventHandler(this.listView1_ItemActivate);
            // 
            // columnName
            // 
            this.columnName.Text = "$NAME";
            this.columnName.Width = 157;
            // 
            // columnType
            // 
            this.columnType.Text = "$TYPE";
            this.columnType.Width = 143;
            // 
            // columnSize
            // 
            this.columnSize.Text = "$SIZE";
            // 
            // columnLastModified
            // 
            this.columnLastModified.Text = "$LAST_MODIFIED";
            this.columnLastModified.Width = 281;
            // 
            // SDSContext
            // 
            this.SDSContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ContextSDSUnpack,
            this.ContextSDSPack,
            this.ContextOpenFolder,
            this.ContextSDSUnpackAll,
            this.ContextView});
            this.SDSContext.Name = "SDSContext";
            this.SDSContext.Size = new System.Drawing.Size(219, 114);
            this.SDSContext.Text = "$VIEW";
            this.SDSContext.Opening += new System.ComponentModel.CancelEventHandler(this.OnOpening);
            // 
            // ContextSDSUnpack
            // 
            this.ContextSDSUnpack.Name = "ContextSDSUnpack";
            this.ContextSDSUnpack.Size = new System.Drawing.Size(218, 22);
            this.ContextSDSUnpack.Text = "$UNPACK";
            this.ContextSDSUnpack.Visible = false;
            this.ContextSDSUnpack.Click += new System.EventHandler(this.ContextSDSUnpack_Click);
            // 
            // ContextSDSPack
            // 
            this.ContextSDSPack.Name = "ContextSDSPack";
            this.ContextSDSPack.Size = new System.Drawing.Size(218, 22);
            this.ContextSDSPack.Text = "$PACK";
            this.ContextSDSPack.Visible = false;
            this.ContextSDSPack.Click += new System.EventHandler(this.ContextSDSPack_Click);
            // 
            // ContextOpenFolder
            // 
            this.ContextOpenFolder.Name = "ContextOpenFolder";
            this.ContextOpenFolder.Size = new System.Drawing.Size(218, 22);
            this.ContextOpenFolder.Text = "$OPEN_FOLDER_EXPLORER";
            this.ContextOpenFolder.Click += new System.EventHandler(this.ContextOpenFolder_Click);
            // 
            // ContextSDSUnpackAll
            // 
            this.ContextSDSUnpackAll.Name = "ContextSDSUnpackAll";
            this.ContextSDSUnpackAll.Size = new System.Drawing.Size(218, 22);
            this.ContextSDSUnpackAll.Text = "$UNPACK_ALL_SDS";
            this.ContextSDSUnpackAll.Click += new System.EventHandler(this.ContextSDSUnpackAll_Click);
            // 
            // ContextView
            // 
            this.ContextView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ContextViewIcon,
            this.ContextViewDetails,
            this.ContextViewSmallIcon,
            this.ContextViewList,
            this.ContextViewTile});
            this.ContextView.Name = "ContextView";
            this.ContextView.Size = new System.Drawing.Size(218, 22);
            this.ContextView.Text = "$VIEW";
            // 
            // ContextViewIcon
            // 
            this.ContextViewIcon.CheckOnClick = true;
            this.ContextViewIcon.Name = "ContextViewIcon";
            this.ContextViewIcon.Size = new System.Drawing.Size(151, 22);
            this.ContextViewIcon.Text = "$ICON";
            this.ContextViewIcon.Click += new System.EventHandler(this.ContextViewBtn_Click);
            // 
            // ContextViewDetails
            // 
            this.ContextViewDetails.CheckOnClick = true;
            this.ContextViewDetails.Name = "ContextViewDetails";
            this.ContextViewDetails.Size = new System.Drawing.Size(151, 22);
            this.ContextViewDetails.Text = "$DETAILS";
            this.ContextViewDetails.Click += new System.EventHandler(this.ContextViewBtn_Click);
            // 
            // ContextViewSmallIcon
            // 
            this.ContextViewSmallIcon.CheckOnClick = true;
            this.ContextViewSmallIcon.Name = "ContextViewSmallIcon";
            this.ContextViewSmallIcon.Size = new System.Drawing.Size(151, 22);
            this.ContextViewSmallIcon.Text = "$SMALL_ICON";
            this.ContextViewSmallIcon.Click += new System.EventHandler(this.ContextViewBtn_Click);
            // 
            // ContextViewList
            // 
            this.ContextViewList.CheckOnClick = true;
            this.ContextViewList.Name = "ContextViewList";
            this.ContextViewList.Size = new System.Drawing.Size(151, 22);
            this.ContextViewList.Text = "$LIST";
            this.ContextViewList.Click += new System.EventHandler(this.ContextViewBtn_Click);
            // 
            // ContextViewTile
            // 
            this.ContextViewTile.CheckOnClick = true;
            this.ContextViewTile.Name = "ContextViewTile";
            this.ContextViewTile.Size = new System.Drawing.Size(151, 22);
            this.ContextViewTile.Text = "$TILE";
            this.ContextViewTile.Click += new System.EventHandler(this.ContextViewBtn_Click);
            // 
            // imageBank
            // 
            this.imageBank.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageBank.ImageStream")));
            this.imageBank.TransparentColor = System.Drawing.Color.Transparent;
            this.imageBank.Images.SetKeyName(0, "folderIcon");
            // 
            // topContainer
            // 
            this.topContainer.BottomToolStripPanelVisible = false;
            // 
            // topContainer.ContentPanel
            // 
            this.topContainer.ContentPanel.Controls.Add(this.tools);
            this.topContainer.ContentPanel.Size = new System.Drawing.Size(800, 23);
            this.topContainer.Dock = System.Windows.Forms.DockStyle.Top;
            this.topContainer.LeftToolStripPanelVisible = false;
            this.topContainer.Location = new System.Drawing.Point(0, 0);
            this.topContainer.Name = "topContainer";
            this.topContainer.RightToolStripPanelVisible = false;
            this.topContainer.Size = new System.Drawing.Size(800, 23);
            this.topContainer.TabIndex = 1;
            this.topContainer.Text = "topContainer";
            this.topContainer.TopToolStripPanelVisible = false;
            // 
            // tools
            // 
            this.tools.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dropdownFile,
            this.dropdownView,
            this.dropdownTools});
            this.tools.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.tools.Location = new System.Drawing.Point(0, 0);
            this.tools.Name = "tools";
            this.tools.Size = new System.Drawing.Size(800, 22);
            this.tools.TabIndex = 1;
            this.tools.Text = "toolStrip1";
            // 
            // dropdownFile
            // 
            this.dropdownFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.dropdownFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openMafiaIIToolStripMenuItem,
            this.runMafiaIIToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.dropdownFile.Image = ((System.Drawing.Image)(resources.GetObject("dropdownFile.Image")));
            this.dropdownFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.dropdownFile.Name = "dropdownFile";
            this.dropdownFile.Size = new System.Drawing.Size(47, 19);
            this.dropdownFile.Text = "$FILE";
            // 
            // openMafiaIIToolStripMenuItem
            // 
            this.openMafiaIIToolStripMenuItem.Name = "openMafiaIIToolStripMenuItem";
            this.openMafiaIIToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.openMafiaIIToolStripMenuItem.Text = "$BTN_OPEN_MII";
            this.openMafiaIIToolStripMenuItem.Click += new System.EventHandler(this.openMafiaIIToolStripMenuItem_Click);
            // 
            // runMafiaIIToolStripMenuItem
            // 
            this.runMafiaIIToolStripMenuItem.Name = "runMafiaIIToolStripMenuItem";
            this.runMafiaIIToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.runMafiaIIToolStripMenuItem.Text = "$BTN_RUN_MII";
            this.runMafiaIIToolStripMenuItem.Click += new System.EventHandler(this.runMafiaIIToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.exitToolStripMenuItem.Text = "$EXIT";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // dropdownView
            // 
            this.dropdownView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.dropdownView.Image = ((System.Drawing.Image)(resources.GetObject("dropdownView.Image")));
            this.dropdownView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.dropdownView.Name = "dropdownView";
            this.dropdownView.Size = new System.Drawing.Size(53, 19);
            this.dropdownView.Text = "$VIEW";
            // 
            // dropdownTools
            // 
            this.dropdownTools.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.dropdownTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem});
            this.dropdownTools.Image = ((System.Drawing.Image)(resources.GetObject("dropdownTools.Image")));
            this.dropdownTools.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.dropdownTools.Name = "dropdownTools";
            this.dropdownTools.Size = new System.Drawing.Size(62, 19);
            this.dropdownTools.Text = "$TOOLS";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.optionsToolStripMenuItem.Text = "$OPTIONS";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // bottomContainer
            // 
            this.bottomContainer.BottomToolStripPanelVisible = false;
            // 
            // bottomContainer.ContentPanel
            // 
            this.bottomContainer.ContentPanel.Controls.Add(this.status);
            this.bottomContainer.ContentPanel.Size = new System.Drawing.Size(800, 26);
            this.bottomContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomContainer.LeftToolStripPanelVisible = false;
            this.bottomContainer.Location = new System.Drawing.Point(0, 424);
            this.bottomContainer.Name = "bottomContainer";
            this.bottomContainer.RightToolStripPanelVisible = false;
            this.bottomContainer.Size = new System.Drawing.Size(800, 26);
            this.bottomContainer.TabIndex = 1;
            this.bottomContainer.Text = "bottomContainer";
            this.bottomContainer.TopToolStripPanelVisible = false;
            // 
            // status
            // 
            this.status.Dock = System.Windows.Forms.DockStyle.Fill;
            this.status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.infoText});
            this.status.Location = new System.Drawing.Point(0, 0);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(800, 26);
            this.status.TabIndex = 0;
            this.status.Text = "statusStrip1";
            // 
            // infoText
            // 
            this.infoText.Name = "infoText";
            this.infoText.Size = new System.Drawing.Size(0, 21);
            // 
            // MafiaIIBrowser
            // 
            this.MafiaIIBrowser.Description = "$SELECT_MII_FOLDER";
            // 
            // GameExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.mainContainer);
            this.Controls.Add(this.bottomContainer);
            this.Controls.Add(this.topContainer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GameExplorer";
            this.Text = "$MII_TK_GAME_EXPLORER";
            this.Load += new System.EventHandler(this.toolStrip1_Resize);
            this.mainContainer.Panel1.ResumeLayout(false);
            this.mainContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainContainer)).EndInit();
            this.mainContainer.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.SDSContext.ResumeLayout(false);
            this.topContainer.ContentPanel.ResumeLayout(false);
            this.topContainer.ContentPanel.PerformLayout();
            this.topContainer.ResumeLayout(false);
            this.topContainer.PerformLayout();
            this.tools.ResumeLayout(false);
            this.tools.PerformLayout();
            this.bottomContainer.ContentPanel.ResumeLayout(false);
            this.bottomContainer.ContentPanel.PerformLayout();
            this.bottomContainer.ResumeLayout(false);
            this.bottomContainer.PerformLayout();
            this.status.ResumeLayout(false);
            this.status.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer mainContainer;
        private System.Windows.Forms.TreeView folderView;
        private System.Windows.Forms.ListView fileListView;
        private System.Windows.Forms.ColumnHeader columnName;
        private System.Windows.Forms.ColumnHeader columnType;
        private System.Windows.Forms.ColumnHeader columnLastModified;
        private System.Windows.Forms.ColumnHeader columnSize;
        private ToolStripContainer topContainer;
        private ToolStrip tools;
        private ToolStripContainer bottomContainer;
        private StatusStrip status;
        private ToolStripStatusLabel infoText;
        private ContextMenuStrip SDSContext;
        private ToolStripMenuItem ContextSDSUnpack;
        private ToolStripMenuItem ContextSDSPack;
        private FolderBrowserDialog MafiaIIBrowser;
        private ToolStripDropDownButton dropdownFile;
        private ToolStripDropDownButton dropdownTools;
        private ToolStripMenuItem openMafiaIIToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem runMafiaIIToolStripMenuItem;
        private ToolStripContainer toolStripContainer1;
        private ToolStrip toolStrip2;
        private ToolStripButton buttonStripUp;
        private ToolStripTextBox textStripFolderPath;
        private ToolStripButton buttonStripRefresh;
        private ToolStripTextBox textStripSearch;
        private ToolStripMenuItem ContextOpenFolder;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripMenuItem ContextSDSUnpackAll;
        private ImageList imageBank;
        private ToolStripMenuItem ContextView;
        private ToolStripMenuItem ContextViewIcon;
        private ToolStripMenuItem ContextViewDetails;
        private ToolStripMenuItem ContextViewSmallIcon;
        private ToolStripMenuItem ContextViewList;
        private ToolStripMenuItem ContextViewTile;
        private ToolStripDropDownButton dropdownView;
    }
}