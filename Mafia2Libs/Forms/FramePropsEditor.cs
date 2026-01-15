using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ResourceTypes.FrameProps;
using Utils.Language;
using Utils.Settings;

namespace Mafia2Tool
{
    public partial class FramePropsEditor : Form
    {
        private FileInfo propsFile;
        private FramePropsFile propsData;

        private TreeNode RootNode;

        private bool bIsFileEdited;

        public FramePropsEditor(FileInfo file)
        {
            InitializeComponent();
            Localise();
            propsFile = file;
            BuildData(true);
            Show();
            ToolkitSettings.UpdateRichPresence("Editing FrameProps File.");
        }

        private void Localise()
        {
            Text = Language.GetString("$FRAMEPROPS_EDITOR_TITLE");
            Button_File.Text = Language.GetString("$FILE");
            Button_Save.Text = Language.GetString("$SAVE");
            Button_Reload.Text = Language.GetString("$RELOAD");
            Button_Exit.Text = Language.GetString("$EXIT");
            Button_Tools.Text = Language.GetString("$TOOLS");
            Button_ExportXml.Text = Language.GetString("$EXPORT_XML");
            Button_ImportXml.Text = Language.GetString("$IMPORT_XML");
            Button_ExpandAll.Text = Language.GetString("$EXPAND_ALL");
            Button_CollapseAll.Text = Language.GetString("$COLLAPSE_ALL");
            Button_AddEntry.Text = Language.GetString("$FRAMEPROPS_ADD_ENTRY");
            Button_DeleteEntry.Text = Language.GetString("$FRAMEPROPS_DELETE_ENTRY");
            Button_AddProperty.Text = Language.GetString("$FRAMEPROPS_ADD_PROPERTY");
            Button_DeleteProperty.Text = Language.GetString("$FRAMEPROPS_DELETE_PROPERTY");
        }

        private void BuildData(bool fromFile)
        {
            TreeView_Main.Nodes.Clear();

            if (fromFile)
            {
                propsData = new FramePropsFile(propsFile);
            }

            // Root node shows file name
            string fileName = Path.GetFileName(propsFile.FullName);
            RootNode = new TreeNode($"FrameProps: {fileName}");
            RootNode.Tag = propsData;
            RootNode.NodeFont = new Font(TreeView_Main.Font, FontStyle.Bold);

            // Add each frame entry
            for (int i = 0; i < propsData.Entries.Length; i++)
            {
                FramePropsEntry entry = propsData.Entries[i];
                TreeNode entryNode = CreateEntryNode(entry, i);
                RootNode.Nodes.Add(entryNode);
            }

            TreeView_Main.Nodes.Add(RootNode);
            RootNode.Expand();

            UpdateStatusBar();
        }

        private TreeNode CreateEntryNode(FramePropsEntry entry, int index)
        {
            string displayName = entry.FrameName;
            TreeNode entryNode = new TreeNode($"[{index}] {displayName}");
            entryNode.Tag = entry;

            // Add property children
            for (int i = 0; i < entry.Properties.Length; i++)
            {
                FrameProperty prop = entry.Properties[i];

                // Split value by semicolons to get individual parts
                string[] valueParts = prop.Value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                // Create property node with count of values
                string propDisplayText = valueParts.Length > 1
                    ? $"[{i}] {prop.PropertyName} ({valueParts.Length} values)"
                    : $"[{i}] {prop.PropertyName} = {prop.Value}";

                TreeNode propNode = new TreeNode(propDisplayText);
                propNode.Tag = prop;

                // If multiple values, add each as a child node
                if (valueParts.Length > 1)
                {
                    for (int j = 0; j < valueParts.Length; j++)
                    {
                        string trimmedValue = valueParts[j].Trim();
                        var valuePart = new PropertyValuePart(prop, j, trimmedValue);

                        // Create display text based on whether it's a key-value pair
                        string nodeText = valuePart.IsKeyValue
                            ? $"[{j}] {valuePart.Key}: {valuePart.ParsedValue}"
                            : $"[{j}] {trimmedValue}";

                        TreeNode valueNode = new TreeNode(nodeText);
                        valueNode.Tag = valuePart;
                        propNode.Nodes.Add(valueNode);
                    }
                }

                entryNode.Nodes.Add(propNode);
            }

            return entryNode;
        }

        /// <summary>
        /// Helper class to represent a single part of a multi-value property.
        /// Parses key-value pairs like "MASS: 20" into separate Key and ParsedValue.
        /// </summary>
        [TypeConverter(typeof(ExpandableObjectConverter))]
        private class PropertyValuePart
        {
            private string rawValue;

            [Browsable(false)]
            public FrameProperty ParentProperty { get; }

            [Description("Index of this value in the property's value list")]
            [ReadOnly(true)]
            [Category("Info")]
            public int Index { get; }

            [Description("Whether this value is a key-value pair (contains ':')")]
            [ReadOnly(true)]
            [Category("Info")]
            public bool IsKeyValue { get; private set; }

            [Description("The key part (before ':') - only for key-value pairs")]
            [Category("Key-Value")]
            public string Key { get; set; }

            [Description("The value part (after ':') - only for key-value pairs")]
            [Category("Key-Value")]
            public string ParsedValue { get; set; }

            [Description("The raw value - editing this will update the parent property")]
            [Category("Raw")]
            public string Value
            {
                get => rawValue;
                set
                {
                    rawValue = value;
                    ParseKeyValue();
                    UpdateParentProperty();
                }
            }

            public PropertyValuePart(FrameProperty parent, int index, string value)
            {
                ParentProperty = parent;
                Index = index;
                rawValue = value;
                ParseKeyValue();
            }

            private void ParseKeyValue()
            {
                // Check if this is a key-value pair (contains ":" but not at the start)
                int colonIndex = rawValue.IndexOf(':');
                if (colonIndex > 0 && colonIndex < rawValue.Length - 1)
                {
                    IsKeyValue = true;
                    Key = rawValue.Substring(0, colonIndex).Trim();
                    ParsedValue = rawValue.Substring(colonIndex + 1).Trim();
                }
                else
                {
                    IsKeyValue = false;
                    Key = "";
                    ParsedValue = rawValue;
                }
            }

            private void UpdateParentProperty()
            {
                // Get all current values from parent
                string[] parts = ParentProperty.Value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                // Rebuild raw value from Key and ParsedValue if it's a key-value pair
                if (IsKeyValue && !string.IsNullOrEmpty(Key))
                {
                    rawValue = $"{Key}: {ParsedValue}";
                }

                // Update the value at our index
                if (Index < parts.Length)
                {
                    parts[Index] = rawValue;
                }

                // Rebuild the parent value string
                ParentProperty.Value = string.Join(";", parts);
            }

            public override string ToString() => IsKeyValue ? $"{Key}: {ParsedValue}" : rawValue;
        }

        private void RefreshTree()
        {
            // Remember selected node path
            string selectedPath = GetNodePath(TreeView_Main.SelectedNode);

            BuildData(false);

            // Try to restore selection
            if (!string.IsNullOrEmpty(selectedPath))
            {
                TreeNode node = FindNodeByPath(selectedPath);
                if (node != null)
                {
                    TreeView_Main.SelectedNode = node;
                    node.EnsureVisible();
                }
            }
        }

        private string GetNodePath(TreeNode node)
        {
            if (node == null) return null;

            List<string> parts = new List<string>();
            while (node != null)
            {
                parts.Insert(0, node.Index.ToString());
                node = node.Parent;
            }
            return string.Join("/", parts);
        }

        private TreeNode FindNodeByPath(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;

            string[] parts = path.Split('/');
            TreeNode current = null;

            foreach (string part in parts)
            {
                if (!int.TryParse(part, out int index)) return null;

                TreeNodeCollection nodes = current == null ? TreeView_Main.Nodes : current.Nodes;
                if (index >= 0 && index < nodes.Count)
                {
                    current = nodes[index];
                }
                else
                {
                    return null;
                }
            }

            return current;
        }

        private void UpdateStatusBar()
        {
            int entryCount = propsData?.Entries?.Length ?? 0;
            int propertyCount = propsData?.Entries?.Sum(e => e.Properties?.Length ?? 0) ?? 0;

            StatusLabel_EntryCount.Text = $"Entries: {entryCount}";
            StatusLabel_PropertyCount.Text = $"Properties: {propertyCount}";
            StatusLabel_Selection.Text = "";
        }

        private void Save()
        {
            // Create backup
            File.Copy(propsFile.FullName, propsFile.FullName + "_old", true);

            // Write the file
            propsData.WriteToFile(propsFile.FullName);

            // Mark as not edited
            Text = Language.GetString("$FRAMEPROPS_EDITOR_TITLE");
            bIsFileEdited = false;
        }

        private void Reload()
        {
            PropertyGrid_Main.SelectedObject = null;
            TreeView_Main.SelectedNode = null;
            BuildData(true);

            Text = Language.GetString("$FRAMEPROPS_EDITOR_TITLE");
            bIsFileEdited = false;
        }

        private void ExportXml()
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "XML|*.xml";
            saveFile.FileName = Path.GetFileNameWithoutExtension(propsFile.Name);
            saveFile.InitialDirectory = propsFile.DirectoryName;

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                propsData.ConvertToXML(saveFile.FileName);
                MessageBox.Show("Export successful!", "FrameProps Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ImportXml()
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "XML|*.xml";
            openFile.CheckFileExists = true;
            openFile.InitialDirectory = propsFile.DirectoryName;

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(openFile.FileName))
                {
                    propsData.ConvertFromXML(openFile.FileName);
                    BuildData(false);
                    MarkAsEdited();
                }
            }
        }

        private void AddEntry()
        {
            // Create new frame entry
            FramePropsEntry newEntry = new FramePropsEntry
            {
                FrameNameHash = 0,
                Properties = Array.Empty<FrameProperty>()
            };

            // Add to array
            FramePropsEntry[] newEntries = new FramePropsEntry[propsData.Entries.Length + 1];
            Array.Copy(propsData.Entries, newEntries, propsData.Entries.Length);
            newEntries[^1] = newEntry;
            propsData.Entries = newEntries;

            RefreshTree();
            MarkAsEdited();
        }

        private void DeleteEntry()
        {
            if (TreeView_Main.SelectedNode?.Tag is FramePropsEntry selectedEntry)
            {
                int entryIndex = Array.IndexOf(propsData.Entries, selectedEntry);
                if (entryIndex >= 0)
                {
                    FramePropsEntry[] newEntries = new FramePropsEntry[propsData.Entries.Length - 1];
                    int j = 0;
                    for (int i = 0; i < propsData.Entries.Length; i++)
                    {
                        if (i != entryIndex)
                        {
                            newEntries[j++] = propsData.Entries[i];
                        }
                    }
                    propsData.Entries = newEntries;

                    RefreshTree();
                    PropertyGrid_Main.SelectedObject = null;
                    MarkAsEdited();
                }
            }
        }

        private void AddProperty()
        {
            FramePropsEntry targetEntry = null;

            // Determine target entry from selection
            if (TreeView_Main.SelectedNode?.Tag is FramePropsEntry entry)
            {
                targetEntry = entry;
            }
            else if (TreeView_Main.SelectedNode?.Tag is FrameProperty prop)
            {
                // Find parent entry
                if (TreeView_Main.SelectedNode.Parent?.Tag is FramePropsEntry parentEntry)
                {
                    targetEntry = parentEntry;
                }
            }

            if (targetEntry != null)
            {
                // Create new property
                FrameProperty newProp = new FrameProperty
                {
                    PropertyNameHash = 0,
                    Value = ""
                };

                // Add to array
                FrameProperty[] newProps = new FrameProperty[targetEntry.Properties.Length + 1];
                Array.Copy(targetEntry.Properties, newProps, targetEntry.Properties.Length);
                newProps[^1] = newProp;
                targetEntry.Properties = newProps;

                RefreshTree();
                MarkAsEdited();
            }
            else
            {
                MessageBox.Show("Please select a frame entry first!", "FrameProps Editor", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DeleteProperty()
        {
            if (TreeView_Main.SelectedNode?.Tag is FrameProperty selectedProp)
            {
                // Find parent entry
                if (TreeView_Main.SelectedNode.Parent?.Tag is FramePropsEntry parentEntry)
                {
                    int propIndex = Array.IndexOf(parentEntry.Properties, selectedProp);
                    if (propIndex >= 0)
                    {
                        FrameProperty[] newProps = new FrameProperty[parentEntry.Properties.Length - 1];
                        int j = 0;
                        for (int i = 0; i < parentEntry.Properties.Length; i++)
                        {
                            if (i != propIndex)
                            {
                                newProps[j++] = parentEntry.Properties[i];
                            }
                        }
                        parentEntry.Properties = newProps;

                        RefreshTree();
                        PropertyGrid_Main.SelectedObject = null;
                        MarkAsEdited();
                    }
                }
            }
        }

        private void AddValueToProperty()
        {
            FrameProperty targetProp = null;

            if (TreeView_Main.SelectedNode?.Tag is FrameProperty prop)
            {
                targetProp = prop;
            }
            else if (TreeView_Main.SelectedNode?.Tag is PropertyValuePart valuePart)
            {
                targetProp = valuePart.ParentProperty;
            }

            if (targetProp != null)
            {
                // Add a new value to the property
                if (string.IsNullOrEmpty(targetProp.Value))
                {
                    targetProp.Value = "NewValue";
                }
                else
                {
                    targetProp.Value += ";NewValue";
                }

                RefreshTree();
                MarkAsEdited();
            }
        }

        private void DeleteValueFromProperty()
        {
            if (TreeView_Main.SelectedNode?.Tag is PropertyValuePart valuePart)
            {
                var parentProp = valuePart.ParentProperty;
                string[] parts = parentProp.Value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length > 1 && valuePart.Index < parts.Length)
                {
                    // Remove the value at the index
                    var newParts = parts.Where((_, i) => i != valuePart.Index).ToArray();
                    parentProp.Value = string.Join(";", newParts);

                    RefreshTree();
                    MarkAsEdited();
                }
                else if (parts.Length == 1)
                {
                    // Last value - clear the property value
                    parentProp.Value = "";
                    RefreshTree();
                    MarkAsEdited();
                }
            }
        }

        private void OnNodeSelectSelect(object sender, TreeViewEventArgs e)
        {
            // Show the selected object directly in the property grid
            PropertyGrid_Main.SelectedObject = e.Node.Tag;

            // Enable/disable buttons based on selection
            Button_DeleteEntry.Enabled = e.Node.Tag is FramePropsEntry;
            Button_AddProperty.Enabled = e.Node.Tag is FramePropsEntry || e.Node.Tag is FrameProperty;
            Button_DeleteProperty.Enabled = e.Node.Tag is FrameProperty;

            // Update status bar with selection info
            UpdateSelectionStatus(e.Node);
        }

        private void UpdateSelectionStatus(TreeNode node)
        {
            if (node?.Tag is FramePropsEntry entry)
            {
                StatusLabel_Selection.Text = $"Entry: {entry.FrameName} | Hash: 0x{entry.FrameNameHash:X16}";
            }
            else if (node?.Tag is FrameProperty prop)
            {
                StatusLabel_Selection.Text = $"Property: {prop.PropertyName} | Hash: 0x{prop.PropertyNameHash:X16}";
            }
            else if (node?.Tag is PropertyValuePart valuePart)
            {
                if (valuePart.IsKeyValue)
                {
                    StatusLabel_Selection.Text = $"Key-Value: {valuePart.Key} = {valuePart.ParsedValue}";
                }
                else
                {
                    StatusLabel_Selection.Text = $"Value: {valuePart.Value}";
                }
            }
            else
            {
                StatusLabel_Selection.Text = "";
            }
        }

        private void OnNodeDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // Expand/collapse on double-click
            if (e.Node.Nodes.Count > 0)
            {
                if (e.Node.IsExpanded)
                    e.Node.Collapse();
                else
                    e.Node.Expand();
            }
        }

        private void PropertyGrid_PropertyChanged(object sender, PropertyValueChangedEventArgs e)
        {
            // Update TreeView node display
            if (TreeView_Main.SelectedNode != null)
            {
                var tag = TreeView_Main.SelectedNode.Tag;

                if (tag is FramePropsEntry entry)
                {
                    int index = TreeView_Main.SelectedNode.Index;
                    TreeView_Main.SelectedNode.Text = $"[{index}] {entry.FrameName}";
                }
                else if (tag is FrameProperty prop)
                {
                    UpdatePropertyNode(TreeView_Main.SelectedNode, prop);
                }
                else if (tag is PropertyValuePart valuePart)
                {
                    // Update current node text
                    int valueIndex = TreeView_Main.SelectedNode.Index;

                    // Rebuild display based on whether it's a key-value pair
                    string nodeText = valuePart.IsKeyValue
                        ? $"[{valueIndex}] {valuePart.Key}: {valuePart.ParsedValue}"
                        : $"[{valueIndex}] {valuePart.Value}";

                    TreeView_Main.SelectedNode.Text = nodeText;

                    // Also update the parent property node display
                    var parentNode = TreeView_Main.SelectedNode.Parent;
                    if (parentNode?.Tag is FrameProperty parentProp)
                    {
                        int propIndex = parentNode.Index;
                        string[] parts = parentProp.Value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        parentNode.Text = parts.Length > 1
                            ? $"[{propIndex}] {parentProp.PropertyName} ({parts.Length} values)"
                            : $"[{propIndex}] {parentProp.PropertyName} = {parentProp.Value}";
                    }
                }

                UpdateSelectionStatus(TreeView_Main.SelectedNode);
            }

            MarkAsEdited();
        }

        private void UpdatePropertyNode(TreeNode propNode, FrameProperty prop)
        {
            int index = propNode.Index;

            // Split value by semicolons to get individual parts
            string[] valueParts = prop.Value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            // Update property node text
            propNode.Text = valueParts.Length > 1
                ? $"[{index}] {prop.PropertyName} ({valueParts.Length} values)"
                : $"[{index}] {prop.PropertyName} = {prop.Value}";

            // Clear and rebuild child nodes if needed
            propNode.Nodes.Clear();
            if (valueParts.Length > 1)
            {
                for (int j = 0; j < valueParts.Length; j++)
                {
                    string trimmedValue = valueParts[j].Trim();
                    var valuePart = new PropertyValuePart(prop, j, trimmedValue);

                    string nodeText = valuePart.IsKeyValue
                        ? $"[{j}] {valuePart.Key}: {valuePart.ParsedValue}"
                        : $"[{j}] {trimmedValue}";

                    TreeNode valueNode = new TreeNode(nodeText);
                    valueNode.Tag = valuePart;
                    propNode.Nodes.Add(valueNode);
                }
            }
        }

        private void ContextMenu_Opening(object sender, CancelEventArgs e)
        {
            var node = TreeView_Main.SelectedNode;

            // Enable/disable context menu items based on selection
            Context_DeleteEntry.Enabled = node?.Tag is FramePropsEntry;
            Context_AddProperty.Enabled = node?.Tag is FramePropsEntry || node?.Tag is FrameProperty;
            Context_DeleteProperty.Enabled = node?.Tag is FrameProperty;
            Context_AddValue.Enabled = node?.Tag is FrameProperty || node?.Tag is PropertyValuePart;
            Context_DeleteValue.Enabled = node?.Tag is PropertyValuePart;
            Context_CopyHash.Enabled = node?.Tag is FramePropsEntry || node?.Tag is FrameProperty;
        }

        private void Context_AddValue_OnClick(object sender, EventArgs e) => AddValueToProperty();
        private void Context_DeleteValue_OnClick(object sender, EventArgs e) => DeleteValueFromProperty();

        private void Context_CopyHash_OnClick(object sender, EventArgs e)
        {
            var node = TreeView_Main.SelectedNode;
            string hash = null;

            if (node?.Tag is FramePropsEntry entry)
            {
                hash = $"0x{entry.FrameNameHash:X16}";
            }
            else if (node?.Tag is FrameProperty prop)
            {
                hash = $"0x{prop.PropertyNameHash:X16}";
            }

            if (!string.IsNullOrEmpty(hash))
            {
                Clipboard.SetText(hash);
                StatusLabel_Selection.Text = $"Copied: {hash}";
            }
        }

        private void FramePropsEditor_Closing(object sender, FormClosingEventArgs e)
        {
            if (bIsFileEdited)
            {
                System.Windows.MessageBoxResult SaveChanges = System.Windows.MessageBox.Show(
                    Language.GetString("$SAVE_PROMPT"),
                    "Toolkit",
                    System.Windows.MessageBoxButton.YesNoCancel);

                if (SaveChanges == System.Windows.MessageBoxResult.Yes)
                {
                    Save();
                }
                else if (SaveChanges == System.Windows.MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void MarkAsEdited()
        {
            if (!bIsFileEdited)
            {
                Text = Language.GetString("$FRAMEPROPS_EDITOR_TITLE") + "*";
                bIsFileEdited = true;
            }
        }

        private void Button_Save_OnClick(object sender, EventArgs e) => Save();
        private void Button_Reload_OnClick(object sender, EventArgs e) => Reload();
        private void Button_Exit_OnClick(object sender, EventArgs e) => Close();
        private void Button_ExportXml_OnClick(object sender, EventArgs e) => ExportXml();
        private void Button_ImportXml_OnClick(object sender, EventArgs e) => ImportXml();
        private void Button_ExpandAll_OnClick(object sender, EventArgs e) => TreeView_Main.ExpandAll();
        private void Button_CollapseAll_OnClick(object sender, EventArgs e) => TreeView_Main.CollapseAll();
        private void Button_AddEntry_OnClick(object sender, EventArgs e) => AddEntry();
        private void Button_DeleteEntry_OnClick(object sender, EventArgs e) => DeleteEntry();
        private void Button_AddProperty_OnClick(object sender, EventArgs e) => AddProperty();
        private void Button_DeleteProperty_OnClick(object sender, EventArgs e) => DeleteProperty();
    }
}
