using System;
using System.IO;
using System.Windows.Forms;
using ResourceTypes.SDSConfig;
using Utils.Language;
using Utils.Settings;

namespace Mafia2Tool
{
    public partial class SdsConfigEditor : Form
    {
        private FileInfo configFile;
        private SdsConfigFile configData;

        private TreeNode TemplatesFolder;

        private bool bIsFileEdited;

        public SdsConfigEditor(FileInfo file)
        {
            InitializeComponent();
            Localise();
            configFile = file;
            BuildData(true);
            Show();
            ToolkitSettings.UpdateRichPresence("Editing SDS Config.");
        }

        private void Localise()
        {
            Text = Language.GetString("$SDSCONFIG_EDITOR_TITLE");
            Button_File.Text = Language.GetString("$FILE");
            Button_Save.Text = Language.GetString("$SAVE");
            Button_Reload.Text = Language.GetString("$RELOAD");
            Button_Exit.Text = Language.GetString("$EXIT");
            Button_Tools.Text = Language.GetString("$TOOLS");
            Button_ExportXml.Text = Language.GetString("$EXPORT_XML");
            Button_ImportXml.Text = Language.GetString("$IMPORT_XML");
            Button_ExpandAll.Text = Language.GetString("$EXPAND_ALL");
            Button_CollapseAll.Text = Language.GetString("$COLLAPSE_ALL");
        }

        private void BuildData(bool fromFile)
        {
            TreeView_Main.Nodes.Clear();

            if (fromFile)
            {
                configData = new SdsConfigFile();
                configData.ReadFromFile(configFile.FullName);
            }

            TemplatesFolder = new TreeNode("SDS Configuration Sets");
            TemplatesFolder.Tag = configData;

            foreach (Template template in configData.Template)
            {
                TreeNode templateNode = new TreeNode(template.Name);
                templateNode.Tag = template;

                // Add Base SDS References
                if (template.BaseSDSReferences.Length > 0)
                {
                    TreeNode baseRefsNode = new TreeNode($"Base References ({template.BaseSDSReferences.Length})");
                    foreach (Group baseRef in template.BaseSDSReferences)
                    {
                        TreeNode refNode = new TreeNode(baseRef.Name);
                        refNode.Tag = baseRef;
                        baseRefsNode.Nodes.Add(refNode);
                    }
                    templateNode.Nodes.Add(baseRefsNode);
                }

                // Add Virtual Slots
                if (template.VirtualSlots.Length > 0)
                {
                    TreeNode slotsNode = new TreeNode($"Virtual Slots ({template.VirtualSlots.Length})");
                    foreach (Unk03Data slot in template.VirtualSlots)
                    {
                        TreeNode slotNode = new TreeNode(slot.Name);
                        slotNode.Tag = slot;

                        // Add SDS Items within each slot
                        foreach (Unk04Data item in slot.SDSItems)
                        {
                            string itemLabel = item.IsDefault ? $"{item.Name} [DEFAULT]" : item.Name;
                            TreeNode itemNode = new TreeNode(itemLabel);
                            itemNode.Tag = item;

                            // Add SDS References within each item
                            if (item.SDSReferences.Length > 0)
                            {
                                foreach (Unk05Data sdsRef in item.SDSReferences)
                                {
                                    TreeNode sdsRefNode = new TreeNode(sdsRef.Name);
                                    sdsRefNode.Tag = sdsRef;
                                    itemNode.Nodes.Add(sdsRefNode);
                                }
                            }

                            slotNode.Nodes.Add(itemNode);
                        }

                        slotsNode.Nodes.Add(slotNode);
                    }
                    templateNode.Nodes.Add(slotsNode);
                }

                TemplatesFolder.Nodes.Add(templateNode);
            }

            TreeView_Main.Nodes.Add(TemplatesFolder);
            TemplatesFolder.Expand();
        }

        private void Save()
        {
            // Create backup
            File.Copy(configFile.FullName, configFile.FullName + "_old", true);

            // Write the file
            configData.WriteToFile(configFile.FullName);

            // Mark as not edited
            Text = Language.GetString("$SDSCONFIG_EDITOR_TITLE");
            bIsFileEdited = false;
        }

        private void Reload()
        {
            PropertyGrid_Main.SelectedObject = null;
            TreeView_Main.SelectedNode = null;
            BuildData(true);

            Text = Language.GetString("$SDSCONFIG_EDITOR_TITLE");
            bIsFileEdited = false;
        }

        private void ExportXml()
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "XML|*.xml";
            saveFile.FileName = Path.GetFileNameWithoutExtension(configFile.Name);
            saveFile.InitialDirectory = configFile.DirectoryName;

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                configData.ConvertToXML(saveFile.FileName);
                MessageBox.Show("Export successful!", "SDS Config Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ImportXml()
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "XML|*.xml";
            openFile.CheckFileExists = true;
            openFile.InitialDirectory = configFile.DirectoryName;

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(openFile.FileName))
                {
                    configData.ConvertFromXML(openFile.FileName);
                    BuildData(false);
                    MarkAsEdited();
                }
            }
        }

        private void OnNodeSelectSelect(object sender, TreeViewEventArgs e)
        {
            PropertyGrid_Main.SelectedObject = e.Node.Tag;
        }

        private void PropertyGrid_PropertyChanged(object sender, PropertyValueChangedEventArgs e)
        {
            // Update TreeView node display if Name changed
            if (e.ChangedItem.Label == "Name")
            {
                TreeView_Main.SelectedNode.Text = e.ChangedItem.Value.ToString();
            }

            MarkAsEdited();
        }

        private void SdsConfigEditor_Closing(object sender, FormClosingEventArgs e)
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
                Text = Language.GetString("$SDSCONFIG_EDITOR_TITLE") + "*";
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
    }
}
