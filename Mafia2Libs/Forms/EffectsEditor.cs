using ResourceTypes.Effects;
using System;
using System.IO;
using System.Windows.Forms;

namespace Toolkit.Forms
{
    /// <summary>
    /// Lightweight viewer/editor for ".eff" (SDS "Effects") files. Presents the generic
    /// chunk tree (tag/size/payload) in a TreeView with a PropertyGrid for the selected chunk,
    /// and supports XML export/import plus saving back to the binary .eff.
    ///
    /// Built entirely in code (no designer/resx) to keep the new format self-contained.
    /// </summary>
    public class EffectsEditor : Form
    {
        private readonly FileInfo effFile;
        private EffectsFile effects;

        private TreeView treeChunks;
        private PropertyGrid propertyGrid;
        private DataGridView valueGrid;
        private SaveFileDialog saveDialog;
        private OpenFileDialog openDialog;

        private bool isEdited;

        public EffectsEditor(FileInfo file)
        {
            effFile = file;
            BuildUi();
            LoadData();
            Show();
        }

        private void BuildUi()
        {
            Text = "Effects Editor - " + effFile.Name;
            ClientSize = new System.Drawing.Size(900, 560);
            KeyPreview = true;

            MenuStrip menu = new MenuStrip();

            ToolStripMenuItem fileMenu = new ToolStripMenuItem("File");
            ToolStripMenuItem saveItem = new ToolStripMenuItem("Save", null, (s, e) => Save())
            {
                ShortcutKeys = Keys.Control | Keys.S
            };
            ToolStripMenuItem reloadItem = new ToolStripMenuItem("Reload", null, (s, e) => Reload())
            {
                ShortcutKeys = Keys.Control | Keys.R
            };
            ToolStripMenuItem exitItem = new ToolStripMenuItem("Exit", null, (s, e) => Close());
            fileMenu.DropDownItems.Add(saveItem);
            fileMenu.DropDownItems.Add(reloadItem);
            fileMenu.DropDownItems.Add(exitItem);

            ToolStripMenuItem toolsMenu = new ToolStripMenuItem("Tools");
            toolsMenu.DropDownItems.Add(new ToolStripMenuItem("Export XML", null, (s, e) => ExportXml()));
            toolsMenu.DropDownItems.Add(new ToolStripMenuItem("Import XML", null, (s, e) => ImportXml()));

            menu.Items.Add(fileMenu);
            menu.Items.Add(toolsMenu);

            SplitContainer split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                SplitterDistance = 430
            };

            treeChunks = new TreeView { Dock = DockStyle.Fill, HideSelection = false };
            treeChunks.AfterSelect += OnNodeSelected;

            propertyGrid = new PropertyGrid { Dock = DockStyle.Fill };
            propertyGrid.PropertyValueChanged += OnPropertyValueChanged;

            valueGrid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Visible = false
            };
            valueGrid.CellEndEdit += OnValueCellEdited;

            ContextMenuStrip gridMenu = new ContextMenuStrip();
            gridMenu.Items.Add("Add key (duplicate selected)", null, (s, e) => DoKeyEdit(true));
            gridMenu.Items.Add("Remove selected key", null, (s, e) => DoKeyEdit(false));
            gridMenu.Opening += (s, e) =>
            {
                bool can = valueGrid.Tag is EffectParamInfo p && p.CanAddRemoveKeys;
                foreach (ToolStripItem it in gridMenu.Items) it.Enabled = can;
            };
            valueGrid.ContextMenuStrip = gridMenu;

            // Right side: PropertyGrid on top, editable value grid below.
            SplitContainer rightSplit = new SplitContainer { Dock = DockStyle.Fill, Orientation = Orientation.Horizontal };
            rightSplit.Panel1.Controls.Add(propertyGrid);
            rightSplit.Panel2.Controls.Add(valueGrid);

            split.Panel1.Controls.Add(treeChunks);
            split.Panel2.Controls.Add(rightSplit);

            Controls.Add(split);
            Controls.Add(menu);
            MainMenuStrip = menu;

            saveDialog = new SaveFileDialog { DefaultExt = "xml", Filter = "XML|*.xml" };
            openDialog = new OpenFileDialog { DefaultExt = "xml", Filter = "XML|*.xml" };

            FormClosing += OnFormClosing;
        }

        private void LoadData()
        {
            effects = new EffectsFile();
            effects.ReadFromFile(effFile.FullName);
            BuildTree();
            SetNotEdited();
        }

        private void BuildTree()
        {
            treeChunks.BeginUpdate();
            treeChunks.Nodes.Clear();
            propertyGrid.SelectedObject = null;

            if (effects.IsRawFallback)
            {
                TreeNode rawNode = new TreeNode("Unparsed (raw passthrough) - " + effects.RawFallback.Length + " bytes");
                treeChunks.Nodes.Add(rawNode);
            }
            else
            {
                // Readable summary: one node per effect, named by its generation(s) and classified by operators.
                System.Collections.Generic.List<EffectPatternInfo> summary = effects.GetSummary();
                if (summary.Count > 0)
                {
                    TreeNode effectsRoot = new TreeNode("Effects (" + summary.Count + ")");
                    foreach (EffectPatternInfo effect in summary)
                    {
                        TreeNode effectNode = new TreeNode(effect.ToString()) { Tag = effect };
                        foreach (EffectFrameInfo frame in effect.Frames)
                        {
                            effectNode.Nodes.Add(new TreeNode(frame.ToString()) { Tag = frame });
                        }
                        foreach (EffectSoundInfo snd in effect.Sounds)
                        {
                            effectNode.Nodes.Add(new TreeNode(snd.ToString()) { Tag = snd });
                        }
                        foreach (EffectGenerationInfo gen in effect.Generations)
                        {
                            TreeNode genNode = new TreeNode(gen.ToString()) { Tag = gen };
                            foreach (EffectOperatorInfo op in gen.Operators)
                            {
                                TreeNode opNode = new TreeNode(op.ToString()) { Tag = op };
                                foreach (EffectParamInfo p in op.Parameters)
                                {
                                    opNode.Nodes.Add(new TreeNode(p.ToString()) { Tag = p });
                                }
                                genNode.Nodes.Add(opNode);
                            }
                            effectNode.Nodes.Add(genNode);
                        }
                        effectsRoot.Nodes.Add(effectNode);
                    }
                    treeChunks.Nodes.Add(effectsRoot);
                }

                // Raw chunk tree for power users / round-trip editing.
                TreeNode rawRoot = new TreeNode("Raw chunks");
                foreach (EffectChunk root in effects.Roots)
                {
                    rawRoot.Nodes.Add(BuildNode(root));
                }
                treeChunks.Nodes.Add(rawRoot);
            }

            treeChunks.EndUpdate();
            if (treeChunks.Nodes.Count > 0)
            {
                treeChunks.Nodes[0].Expand();
            }
        }

        private static TreeNode BuildNode(EffectChunk chunk)
        {
            TreeNode node = new TreeNode(chunk.ToString()) { Tag = chunk };
            if (chunk.Children != null)
            {
                foreach (EffectChunk child in chunk.Children)
                {
                    node.Nodes.Add(BuildNode(child));
                }
            }
            return node;
        }

        private void OnNodeSelected(object sender, TreeViewEventArgs e)
        {
            object tag = e.Node?.Tag;
            propertyGrid.SelectedObject = tag;
            PopulateValueGrid(tag as EffectParamInfo);
        }

        private void PopulateValueGrid(EffectParamInfo p)
        {
            valueGrid.Tag = null;
            valueGrid.Rows.Clear();
            valueGrid.Columns.Clear();

            bool hasScalars = p != null && p.Scalars != null && p.Scalars.Length > 0 && p.ScalarOffset >= 0;
            bool hasKeys = p != null && p.Keys != null && p.Keys.Count > 0 && p.KeyOffsets != null;
            if (!hasScalars && !hasKeys)
            {
                valueGrid.Visible = false;
                return;
            }

            if (hasScalars)
            {
                valueGrid.Columns.Add("v", "Value");
                foreach (float f in p.Scalars)
                {
                    valueGrid.Rows.Add(f.ToString("0.######", System.Globalization.CultureInfo.InvariantCulture));
                }
            }
            else
            {
                int cols = 0;
                foreach (float[] k in p.Keys) cols = System.Math.Max(cols, k.Length);
                valueGrid.Columns.Add("t", "Time");
                for (int c = 1; c < cols; c++) valueGrid.Columns.Add("v" + c, "Value " + c);
                foreach (float[] k in p.Keys)
                {
                    string[] cells = new string[cols];
                    for (int c = 0; c < cols; c++)
                        cells[c] = c < k.Length ? k[c].ToString("0.######", System.Globalization.CultureInfo.InvariantCulture) : "";
                    valueGrid.Rows.Add(cells);
                }
            }

            valueGrid.Tag = p;
            valueGrid.Visible = true;
        }

        private void OnValueCellEdited(object sender, DataGridViewCellEventArgs e)
        {
            if (!(valueGrid.Tag is EffectParamInfo p)) return;
            object cell = valueGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            if (!float.TryParse(cell?.ToString(), System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture, out float value))
            {
                // restore displayed value on bad input
                RestoreCell(p, e.RowIndex, e.ColumnIndex);
                return;
            }

            int offset;
            if (p.ScalarOffset >= 0 && p.Scalars != null)
            {
                if (e.RowIndex >= p.Scalars.Length) return;
                offset = p.ScalarOffset + e.RowIndex * 4;
                p.Scalars[e.RowIndex] = value;
            }
            else if (p.KeyOffsets != null && e.RowIndex < p.KeyOffsets.Count)
            {
                float[] key = p.Keys[e.RowIndex];
                if (e.ColumnIndex >= key.Length) { RestoreCell(p, e.RowIndex, e.ColumnIndex); return; }
                offset = p.KeyOffsets[e.RowIndex] + e.ColumnIndex * 4;
                key[e.ColumnIndex] = value;
            }
            else return;

            effects.PatchFloat(offset, value);
            SetEdited();
        }

        private void DoKeyEdit(bool add)
        {
            if (!(valueGrid.Tag is EffectParamInfo p) || !p.CanAddRemoveKeys) return;
            int row = valueGrid.CurrentCell != null ? valueGrid.CurrentCell.RowIndex : 0;
            if (row < 0) row = 0;

            System.Collections.Generic.List<int> path = GetNodePath(treeChunks.SelectedNode);
            bool ok = add ? effects.AddKeyframe(p, row) : effects.RemoveKeyframe(p, row);
            if (!ok) return;

            SetEdited();
            BuildTree();          // structural change shifts offsets -> reparse + rebuild
            SelectNodePath(path); // restore selection so the grid shows the updated keys
        }

        private static System.Collections.Generic.List<int> GetNodePath(TreeNode node)
        {
            System.Collections.Generic.List<int> path = new System.Collections.Generic.List<int>();
            while (node != null) { path.Insert(0, node.Index); node = node.Parent; }
            return path;
        }

        private void SelectNodePath(System.Collections.Generic.List<int> path)
        {
            if (path == null || path.Count == 0) return;
            TreeNodeCollection nodes = treeChunks.Nodes;
            TreeNode cur = null;
            foreach (int idx in path)
            {
                if (idx < 0 || idx >= nodes.Count) return;
                cur = nodes[idx];
                nodes = cur.Nodes;
            }
            if (cur != null) { cur.EnsureVisible(); treeChunks.SelectedNode = cur; }
        }

        private void RestoreCell(EffectParamInfo p, int row, int col)
        {
            float? v = null;
            if (p.Scalars != null && row < p.Scalars.Length) v = p.Scalars[row];
            else if (p.Keys != null && row < p.Keys.Count && col < p.Keys[row].Length) v = p.Keys[row][col];
            if (v.HasValue)
                valueGrid.Rows[row].Cells[col].Value = v.Value.ToString("0.######", System.Globalization.CultureInfo.InvariantCulture);
        }

        private void OnPropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
        {
            SetEdited();

            // Refresh the label of the edited node (tag/size may have changed).
            TreeNode selected = treeChunks.SelectedNode;
            if (selected?.Tag is EffectChunk chunk)
            {
                selected.Text = chunk.ToString();
            }
        }

        private void Save()
        {
            // Keep a backup, matching the convention used by the other editors.
            File.Copy(effFile.FullName, effFile.FullName + "_old", true);
            effects.WriteToFile(effFile.FullName);
            SetNotEdited();
        }

        private void Reload()
        {
            LoadData();
        }

        private void ExportXml()
        {
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                effects.ConvertToXML(saveDialog.FileName);
            }
        }

        private void ImportXml()
        {
            if (openDialog.ShowDialog() == DialogResult.OK && File.Exists(openDialog.FileName))
            {
                effects.ConvertFromXML(openDialog.FileName);
                BuildTree();
                SetEdited();
            }
        }

        private void SetEdited()
        {
            if (!isEdited)
            {
                isEdited = true;
                Text = "Effects Editor - " + effFile.Name + "*";
            }
        }

        private void SetNotEdited()
        {
            isEdited = false;
            Text = "Effects Editor - " + effFile.Name;
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isEdited)
            {
                return;
            }

            DialogResult result = MessageBox.Show(
                "Save changes to " + effFile.Name + "?", "Toolkit", MessageBoxButtons.YesNoCancel);

            if (result == DialogResult.Yes)
            {
                Save();
            }
            else if (result == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }
    }
}
