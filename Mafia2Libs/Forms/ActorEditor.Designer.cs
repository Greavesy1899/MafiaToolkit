namespace Mafia2Tool
{
    partial class ActorEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ActorEditor));
            ActorGrid = new System.Windows.Forms.PropertyGrid();
            ActorTreeView = new Controls.MTreeView();
            ActorContext = new System.Windows.Forms.ContextMenuStrip(components);
            ContextDelete = new System.Windows.Forms.ToolStripMenuItem();
            ContextCopy = new System.Windows.Forms.ToolStripMenuItem();
            ContextPaste = new System.Windows.Forms.ToolStripMenuItem();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            FileButton = new System.Windows.Forms.ToolStripDropDownButton();
            SaveButton = new System.Windows.Forms.ToolStripMenuItem();
            ReloadButton = new System.Windows.Forms.ToolStripMenuItem();
            ExitButton = new System.Windows.Forms.ToolStripMenuItem();
            EditButton = new System.Windows.Forms.ToolStripDropDownButton();
            AddItemButton = new System.Windows.Forms.ToolStripMenuItem();
            AddDefinitionButton = new System.Windows.Forms.ToolStripMenuItem();
            Button_MoveUp = new System.Windows.Forms.ToolStripMenuItem();
            Button_MoveDown = new System.Windows.Forms.ToolStripMenuItem();
            ActorContext.SuspendLayout();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // ActorGrid
            // 
            ActorGrid.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            ActorGrid.Location = new System.Drawing.Point(469, 32);
            ActorGrid.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ActorGrid.Name = "ActorGrid";
            ActorGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            ActorGrid.Size = new System.Drawing.Size(450, 473);
            ActorGrid.TabIndex = 10;
            ActorGrid.PropertyValueChanged += ActorGrid_OnPropertyValueChanged;
            // 
            // ActorTreeView
            // 
            ActorTreeView.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            ActorTreeView.ContextMenuStrip = ActorContext;
            ActorTreeView.Location = new System.Drawing.Point(14, 32);
            ActorTreeView.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ActorTreeView.Name = "ActorTreeView";
            ActorTreeView.Size = new System.Drawing.Size(429, 472);
            ActorTreeView.TabIndex = 11;
            ActorTreeView.AfterSelect += OnNodeSelectSelect;
            ActorTreeView.KeyUp += ActorTreeView_OnKeyUp;
            // 
            // ActorContext
            // 
            ActorContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ContextDelete, ContextCopy, ContextPaste, Button_MoveUp, Button_MoveDown });
            ActorContext.Name = "SDSContext";
            ActorContext.Size = new System.Drawing.Size(261, 136);
            ActorContext.Opening += ContextMenu_OnOpening;
            // 
            // ContextDelete
            // 
            ContextDelete.Name = "ContextDelete";
            ContextDelete.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete;
            ContextDelete.Size = new System.Drawing.Size(260, 22);
            ContextDelete.Text = "Delete";
            ContextDelete.Click += ContextDelete_Click;
            // 
            // ContextCopy
            // 
            ContextCopy.Name = "ContextCopy";
            ContextCopy.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C;
            ContextCopy.Size = new System.Drawing.Size(260, 22);
            ContextCopy.Text = "$COPY";
            ContextCopy.Click += ContextCopy_Click;
            // 
            // ContextPaste
            // 
            ContextPaste.Name = "ContextPaste";
            ContextPaste.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V;
            ContextPaste.Size = new System.Drawing.Size(260, 22);
            ContextPaste.Text = "$PASTE";
            ContextPaste.Click += ContextPaste_Click;
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { FileButton, EditButton });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(933, 25);
            toolStrip1.TabIndex = 15;
            toolStrip1.Text = "toolStrip1";
            // 
            // FileButton
            // 
            FileButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            FileButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { SaveButton, ReloadButton, ExitButton });
            FileButton.Image = (System.Drawing.Image)resources.GetObject("FileButton.Image");
            FileButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            FileButton.Name = "FileButton";
            FileButton.Size = new System.Drawing.Size(47, 22);
            FileButton.Text = "$FILE";
            // 
            // SaveButton
            // 
            SaveButton.Name = "SaveButton";
            SaveButton.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S;
            SaveButton.Size = new System.Drawing.Size(165, 22);
            SaveButton.Text = "$SAVE";
            SaveButton.Click += SaveButton_OnClick;
            // 
            // ReloadButton
            // 
            ReloadButton.Name = "ReloadButton";
            ReloadButton.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R;
            ReloadButton.Size = new System.Drawing.Size(165, 22);
            ReloadButton.Text = "$RELOAD";
            ReloadButton.Click += ReloadButton_OnClick;
            // 
            // ExitButton
            // 
            ExitButton.Name = "ExitButton";
            ExitButton.Size = new System.Drawing.Size(165, 22);
            ExitButton.Text = "$EXIT";
            ExitButton.Click += ExitButton_OnClick;
            // 
            // EditButton
            // 
            EditButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            EditButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { AddItemButton, AddDefinitionButton });
            EditButton.Image = (System.Drawing.Image)resources.GetObject("EditButton.Image");
            EditButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            EditButton.Name = "EditButton";
            EditButton.Size = new System.Drawing.Size(49, 22);
            EditButton.Text = "$EDIT";
            // 
            // AddItemButton
            // 
            AddItemButton.Name = "AddItemButton";
            AddItemButton.Size = new System.Drawing.Size(171, 22);
            AddItemButton.Text = "$ADD_ITEM";
            AddItemButton.Click += AddItemButton_Click;
            // 
            // AddDefinitionButton
            // 
            AddDefinitionButton.Name = "AddDefinitionButton";
            AddDefinitionButton.Size = new System.Drawing.Size(171, 22);
            AddDefinitionButton.Text = "$ADD_DEFINITION";
            AddDefinitionButton.Click += AddDefinitionButton_Click;
            // 
            // Button_MoveUp
            // 
            Button_MoveUp.Name = "Button_MoveUp";
            Button_MoveUp.ShortcutKeyDisplayString = "CTRL + PageUp";
            Button_MoveUp.Size = new System.Drawing.Size(260, 22);
            Button_MoveUp.Text = "$MOVE_UP";
            Button_MoveUp.Click += Button_MoveUp_Clicked;
            // 
            // Button_MoveDown
            // 
            Button_MoveDown.Name = "Button_MoveDown";
            Button_MoveDown.ShortcutKeyDisplayString = "CTRL + PageDown";
            Button_MoveDown.Size = new System.Drawing.Size(260, 22);
            Button_MoveDown.Text = "$MOVE_DOWN";
            Button_MoveDown.Click += Button_MoveDown_Clicked;
            // 
            // ActorEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(933, 519);
            Controls.Add(toolStrip1);
            Controls.Add(ActorGrid);
            Controls.Add(ActorTreeView);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "ActorEditor";
            Text = "$ACTOR_EDITOR_TITLE";
            FormClosing += ActorEditor_Closing;
            ActorContext.ResumeLayout(false);
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.PropertyGrid ActorGrid;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton FileButton;
        private System.Windows.Forms.ToolStripMenuItem SaveButton;
        private System.Windows.Forms.ToolStripMenuItem ReloadButton;
        private System.Windows.Forms.ToolStripMenuItem ExitButton;
        private System.Windows.Forms.ContextMenuStrip ActorContext;
        private System.Windows.Forms.ToolStripMenuItem ContextDelete;
        private System.Windows.Forms.ToolStripDropDownButton EditButton;
        private System.Windows.Forms.ToolStripMenuItem AddItemButton;
        private System.Windows.Forms.ToolStripMenuItem AddDefinitionButton;
        private System.Windows.Forms.ToolStripMenuItem ContextCopy;
        private System.Windows.Forms.ToolStripMenuItem ContextPaste;
        private Mafia2Tool.Controls.MTreeView ActorTreeView;
        private System.Windows.Forms.ToolStripMenuItem Button_MoveUp;
        private System.Windows.Forms.ToolStripMenuItem Button_MoveDown;
    }
}