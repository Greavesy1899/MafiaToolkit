using System;
using System.IO;
using System.Windows.Forms;
using ResourceTypes.CGame;
using Utils.Language;
using Utils.Settings;

namespace Mafia2Tool
{
    public partial class CGameEditor : Form
    {
        private FileInfo gameFile;
        private CGame gameData;

        private TreeNode RootNode;

        private bool bIsFileEdited;

        public CGameEditor(FileInfo file)
        {
            InitializeComponent();
            Localise();
            gameFile = file;
            BuildData(true);
            Show();
            ToolkitSettings.UpdateRichPresence("Editing Game File.");
        }

        private void Localise()
        {
            Text = Language.GetString("$CGAME_EDITOR_TITLE");
            Button_File.Text = Language.GetString("$FILE");
            Button_Save.Text = Language.GetString("$SAVE");
            Button_Reload.Text = Language.GetString("$RELOAD");
            Button_Exit.Text = Language.GetString("$EXIT");
            Button_Tools.Text = Language.GetString("$TOOLS");
            Button_ExportXml.Text = Language.GetString("$EXPORT_XML");
            Button_ImportXml.Text = Language.GetString("$IMPORT_XML");
            Button_ExpandAll.Text = Language.GetString("$EXPAND_ALL");
            Button_CollapseAll.Text = Language.GetString("$COLLAPSE_ALL");
            Button_AddSlot.Text = Language.GetString("$CGAME_ADD_SLOT");
            Button_DeleteSlot.Text = Language.GetString("$CGAME_DELETE_SLOT");
        }

        private void BuildData(bool fromFile)
        {
            TreeView_Main.Nodes.Clear();

            if (fromFile)
            {
                gameData = new CGame(gameFile);
            }

            // Root node shows file name
            string fileName = Path.GetFileName(gameFile.FullName);
            RootNode = new TreeNode($"Game: {fileName}");
            RootNode.Tag = gameData;

            // Add each chunk
            for (int i = 0; i < gameData.Chunks.Length; i++)
            {
                IGameChunk chunk = gameData.Chunks[i];
                TreeNode chunkNode = CreateChunkNode(chunk, i);
                RootNode.Nodes.Add(chunkNode);
            }

            TreeView_Main.Nodes.Add(RootNode);
            RootNode.Expand();

            // Expand first chunk if it's PreloadManager
            if (RootNode.Nodes.Count > 0)
            {
                RootNode.Nodes[0].Expand();
            }
        }

        private TreeNode CreateChunkNode(IGameChunk chunk, int index)
        {
            string chunkName = GetDisplayName(chunk);
            TreeNode chunkNode = new TreeNode($"[{index}] {chunkName}");
            chunkNode.Tag = chunk;

            // Special handling for PreloadManager to show slots
            if (chunk is PreloadManager preload)
            {
                for (int i = 0; i < preload.Slots.Length; i++)
                {
                    PreloadSlot slot = preload.Slots[i];
                    TreeNode slotNode = new TreeNode($"[{i}] {slot.Path}");
                    slotNode.Tag = slot;
                    chunkNode.Nodes.Add(slotNode);
                }
            }
            // Special handling for SDSEntries to show SDS entries
            else if (chunk is SDSEntries sdsEntries)
            {
                for (int i = 0; i < sdsEntries.Entries.Length; i++)
                {
                    SDSPreloadEntry entry = sdsEntries.Entries[i];
                    TreeNode entryNode = new TreeNode($"[{i}] Type {entry.SlotType}: {entry.Path}");
                    entryNode.Tag = entry;
                    chunkNode.Nodes.Add(entryNode);
                }
            }
            // Special handling for MissionSettings to show mission info
            else if (chunk is MissionSettings mission)
            {
                // Show MissionTrick as first child
                if (!string.IsNullOrEmpty(mission.MissionTrick))
                {
                    TreeNode trickNode = new TreeNode($"Mission Trick: {mission.MissionTrick}");
                    trickNode.Tag = mission;
                    chunkNode.Nodes.Add(trickNode);
                }
                // Show ActorsFilePath as second child
                if (!string.IsNullOrEmpty(mission.ActorsFilePath))
                {
                    TreeNode actorsNode = new TreeNode($"Actors File: {mission.ActorsFilePath}");
                    actorsNode.Tag = mission;
                    chunkNode.Nodes.Add(actorsNode);
                }
            }

            return chunkNode;
        }

        private string GetDisplayName(IGameChunk chunk)
        {
            return chunk switch
            {
                PreloadManager => "Preload Manager",
                SDSEntries => "SDS Entries",
                WeatherSettings => "Weather Settings",
                GameFlags => "Game Flags",
                MissionSettings => "Mission Settings",
                UnknownChunk unk => $"Unknown (0x{unk.Type:X})",
                _ => chunk.GetType().Name
            };
        }

        private void RefreshTree()
        {
            BuildData(false);
        }

        private void Save()
        {
            // Create backup
            File.Copy(gameFile.FullName, gameFile.FullName + "_old", true);

            // Write the file
            gameData.WriteToFile(gameFile.FullName);

            // Mark as not edited
            Text = Language.GetString("$CGAME_EDITOR_TITLE");
            bIsFileEdited = false;
        }

        private void Reload()
        {
            PropertyGrid_Main.SelectedObject = null;
            TreeView_Main.SelectedNode = null;
            BuildData(true);

            Text = Language.GetString("$CGAME_EDITOR_TITLE");
            bIsFileEdited = false;
        }

        private void ExportXml()
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "XML|*.xml";
            saveFile.FileName = Path.GetFileNameWithoutExtension(gameFile.Name);
            saveFile.InitialDirectory = gameFile.DirectoryName;

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                gameData.ConvertToXML(saveFile.FileName);
                MessageBox.Show("Export successful!", "Game Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ImportXml()
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "XML|*.xml";
            openFile.CheckFileExists = true;
            openFile.InitialDirectory = gameFile.DirectoryName;

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(openFile.FileName))
                {
                    gameData.ConvertFromXML(openFile.FileName);
                    BuildData(false);
                    MarkAsEdited();
                }
            }
        }

        private void AddSlot()
        {
            // Find PreloadManager chunk
            for (int i = 0; i < gameData.Chunks.Length; i++)
            {
                if (gameData.Chunks[i] is PreloadManager preload)
                {
                    // Add new slot
                    PreloadSlot newSlot = new PreloadSlot { Path = "/sds/NewSlot/" };
                    PreloadSlot[] newSlots = new PreloadSlot[preload.Slots.Length + 1];
                    Array.Copy(preload.Slots, newSlots, preload.Slots.Length);
                    newSlots[newSlots.Length - 1] = newSlot;
                    preload.Slots = newSlots;

                    RefreshTree();
                    MarkAsEdited();
                    return;
                }
            }

            MessageBox.Show("No PreloadManager chunk found!", "Game Editor", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void DeleteSlot()
        {
            if (TreeView_Main.SelectedNode?.Tag is PreloadSlot selectedSlot)
            {
                // Find PreloadManager chunk
                for (int i = 0; i < gameData.Chunks.Length; i++)
                {
                    if (gameData.Chunks[i] is PreloadManager preload)
                    {
                        // Find and remove the slot
                        int slotIndex = Array.IndexOf(preload.Slots, selectedSlot);
                        if (slotIndex >= 0)
                        {
                            PreloadSlot[] newSlots = new PreloadSlot[preload.Slots.Length - 1];
                            int j = 0;
                            for (int k = 0; k < preload.Slots.Length; k++)
                            {
                                if (k != slotIndex)
                                {
                                    newSlots[j++] = preload.Slots[k];
                                }
                            }
                            preload.Slots = newSlots;

                            RefreshTree();
                            PropertyGrid_Main.SelectedObject = null;
                            MarkAsEdited();
                            return;
                        }
                    }
                }
            }
        }

        private void OnNodeSelectSelect(object sender, TreeViewEventArgs e)
        {
            PropertyGrid_Main.SelectedObject = e.Node.Tag;

            // Enable/disable Delete Slot button based on selection
            Button_DeleteSlot.Enabled = e.Node.Tag is PreloadSlot;
        }

        private void PropertyGrid_PropertyChanged(object sender, PropertyValueChangedEventArgs e)
        {
            // Update TreeView node display if path/value changed
            if (TreeView_Main.SelectedNode != null)
            {
                if (e.ChangedItem.Label == "Path" && TreeView_Main.SelectedNode.Tag is PreloadSlot slot)
                {
                    int index = TreeView_Main.SelectedNode.Index;
                    TreeView_Main.SelectedNode.Text = $"[{index}] {slot.Path}";
                }
                else if ((e.ChangedItem.Label == "Path" || e.ChangedItem.Label == "SlotType")
                         && TreeView_Main.SelectedNode.Tag is SDSPreloadEntry entry)
                {
                    int index = TreeView_Main.SelectedNode.Index;
                    TreeView_Main.SelectedNode.Text = $"[{index}] Type {entry.SlotType}: {entry.Path}";
                }
                else if (TreeView_Main.SelectedNode.Tag is MissionSettings)
                {
                    // Refresh the parent chunk node for MissionSettings
                    RefreshTree();
                }
            }

            MarkAsEdited();
        }

        private void CGameEditor_Closing(object sender, FormClosingEventArgs e)
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
                Text = Language.GetString("$CGAME_EDITOR_TITLE") + "*";
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
        private void Button_AddSlot_OnClick(object sender, EventArgs e) => AddSlot();
        private void Button_DeleteSlot_OnClick(object sender, EventArgs e) => DeleteSlot();
    }
}
