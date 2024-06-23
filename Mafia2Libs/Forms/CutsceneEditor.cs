using Core.IO;
using ResourceTypes.Cutscene;
using System;
using System.Windows.Forms;
using Utils.Helpers;
using Utils.Language;
using Utils.Settings;
using static ResourceTypes.Cutscene.CutsceneLoader;

namespace Mafia2Tool.Forms
{
    public partial class CutsceneEditor : Form
    {
        // File access. We should not directly edit cutscene from here.
        private FileCutscene OriginalFile;

        private CutsceneLoader.Cutscene[] Cutscenes;
        private CutsceneLoader.GCRData[] VehicleData;

        private bool bIsFileEdited = false;

        public CutsceneEditor(FileCutscene CutsceneFile)
        {
            InitializeComponent();
            OriginalFile = CutsceneFile;

            Localise();
            BuildData();
        }

        public void Localise()
        {
            Text = Language.GetString("$CUTSCENE_EDITOR");
            Button_File.Text = Language.GetString("$FILE");
            Button_Save.Text = Language.GetString("$SAVE");
            Button_Reload.Text = Language.GetString("$RELOAD");
            Button_Exit.Text = Language.GetString("$EXIT");
        }

        private void AddCutsceneToTreeView(CutsceneLoader.Cutscene Cutscene)
        {
            TreeNode CutsceneParent = new TreeNode(Cutscene.CutsceneName);

            if (Cutscene.AssetContent != null)
            {
                var Assets = Cutscene.AssetContent;
                TreeNode AssetsParent = new TreeNode("Game Cutscene Content: (GCS Data)");

                if (Assets.FaceFX != null)
                {
                    TreeNode AssetNode = new TreeNode("FaceFX");
                    AssetNode.Tag = Assets.FaceFX;

                    AssetsParent.Nodes.Add(AssetNode);
                }

                for (int i = 0; i < Assets.entities.Length; i++)
                {
                    var Asset = Assets.entities[i];
                    TreeNode AssetNode = new TreeNode(string.Format("Asset: {0}", i));
                    AssetNode.Tag = Asset;

                    AssetsParent.Nodes.Add(AssetNode);
                }

                CutsceneParent.Nodes.Add(AssetsParent);
            }

            if (Cutscene.SoundContent != null)
            {
                var Assets = Cutscene.SoundContent;
                TreeNode AssetsParent = new TreeNode("Sound Content: (SPD Data)");

                for (int i = 0; i < Assets.EntityDefinitions.Length; i++)
                {
                    var Asset = Assets.EntityDefinitions[i];
                    TreeNode AssetNode = new TreeNode(string.Format("Asset: {0}", i));
                    AssetNode.Tag = Asset;

                    AssetsParent.Nodes.Add(AssetNode);
                }

                CutsceneParent.Nodes.Add(AssetsParent);
            }

            TreeView_Cutscene.Nodes.Add(CutsceneParent);
        }

        public void BuildData()
        {
            Cutscenes = OriginalFile.GetCutsceneLoader().Cutscenes;

            for (int i = 0; i < Cutscenes.Length; i++)
            {
                AddCutsceneToTreeView(Cutscenes[i]);
            }

            VehicleData = OriginalFile.GetCutsceneLoader().VehicleContent;

            TreeNode GCRParent = new TreeNode("Vehicle Content: (GCR Data)");

            for (int i = 0; i < VehicleData.Length; i++)
            {
                TreeNode GCR = new TreeNode(VehicleData[i].Name);
                GCR.Tag = VehicleData[i];
                GCRParent.Nodes.Add(GCR);
            }

            TreeView_Cutscene.Nodes.Add(GCRParent);
        }

        private void Save()
        {
            CutsceneLoader Loader = OriginalFile.GetCutsceneLoader();
            Loader.WriteToFile(OriginalFile.GetUnderlyingFileInfo().FullName);

            Text = Language.GetString("$CUTSCENE_EDITOR");
            bIsFileEdited = false;
        }

        private void Reload()
        {
            PropertyGrid_Cutscene.SelectedObject = null;
            TreeView_Cutscene.SelectedNode = null;
            TreeView_Cutscene.Nodes.Clear();
            BuildData();
            Text = Language.GetString("$CUTSCENE_EDITOR");
            bIsFileEdited = false;
        }

        private void TreeView_Cutscene_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByKeyboard || e.Action == TreeViewAction.ByMouse)
            {
                PropertyGrid_Cutscene.SelectedObject = e.Node.Tag;
            }
        }

        private void PropertyGrid_Cutscene_PropertyChanged(object sender, PropertyValueChangedEventArgs e)
        {
            if (e.ChangedItem.Label == "Name")
                TreeView_Cutscene.SelectedNode.Text = e.ChangedItem.Value.ToString();

            Text = Language.GetString("$CUTSCENE_EDITOR") + "*";
            bIsFileEdited = true;
        }

        private void CutsceneEditor_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.D)
            {
                PropertyGrid_Cutscene.SelectedObject = null;
                TreeView_Cutscene.SelectedNode = null;
            }
        }

        private void Button_Save_OnClick(object sender, EventArgs e) => Save();

        private void Button_Exit_OnClick(object sender, EventArgs e) => Close();

        private void Button_Reload_OnClick(object sender, EventArgs e) => Reload();

        private void CutsceneEditor_Closing(object sender, FormClosingEventArgs e)
        {
            if (bIsFileEdited)
            {
                System.Windows.MessageBoxResult SaveChanges = System.Windows.MessageBox.Show(Language.GetString("$SAVE_PROMPT"), "Toolkit", System.Windows.MessageBoxButton.YesNoCancel);

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
    }
}
