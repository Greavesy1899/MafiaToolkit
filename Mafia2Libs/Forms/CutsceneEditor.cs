using Core.IO;
using ResourceTypes.Cutscene;
using ResourceTypes.Cutscene.AnimEntities;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Utils.Language;
using static ResourceTypes.Cutscene.CutsceneLoader;
using Utils.Extensions;

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
            CutsceneParent.Tag = Cutscene;

            if (Cutscene.AssetContent != null)
            {
                var Assets = Cutscene.AssetContent;
                TreeNode AssetsParent = new TreeNode("Game Cutscene Content: (GCS Data)");

                AssetsParent.Tag = Assets;

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

                AssetsParent.Tag = Assets;

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
            PropertyGrid_Cutscene.SelectedObject = e.Node.Tag;
        }

        private void PropertyGrid_Cutscene_PropertyChanged(object sender, PropertyValueChangedEventArgs e)
        {
            if (e.ChangedItem.Label == "Name" || e.ChangedItem.Label == "CutsceneName")
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

        private void TreeViewContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ContextMenu_Duplicate.Enabled = false;

            if (TreeView_Cutscene.SelectedNode.Tag is AnimEntityWrapper)
            {
                ContextMenu_Duplicate.Enabled = true;
            }
        }

        private void ContextMenu_Duplicate_Click(object sender, EventArgs e)
        {
            //Probably not the most optimal code, but cba to make better code

            AnimEntityWrapper entity = (AnimEntityWrapper)TreeView_Cutscene.SelectedNode.Tag;
            AnimEntityWrapper newEntity;
            byte[] entityData = new byte[0];
            byte[] animEntityData = new byte[0];

            using (MemoryStream stream = new MemoryStream())
            {
                // Write Entity to the Stream
                CutsceneEntityFactory.WriteAnimEntityToFile(stream, entity);
                entityData = stream.ToArray();
            }

            using (MemoryStream EntityStream = new MemoryStream())
            {
                bool isBigEndian = false;
                EntityStream.Write(entity.AnimEntityData.DataType, isBigEndian);
                EntityStream.Write(0, isBigEndian);
                entity.AnimEntityData.WriteToFile(EntityStream, isBigEndian);

                animEntityData = EntityStream.ToArray();
            }

            using (MemoryStream Reader = new MemoryStream(entityData))
            {
                newEntity = CutsceneEntityFactory.ReadAnimEntityWrapperFromFile(entity.GetEntityType(), Reader);
            }

            using (MemoryStream stream = new MemoryStream(animEntityData))
            {
                newEntity.AnimEntityData.ReadFromFile(stream, false);
            }

            var cutscenes = OriginalFile.GetCutsceneLoader().Cutscenes;

            for (int i = 0; i < cutscenes.Length; i++)
            {
                var cutscene = cutscenes[i];

                if (cutscene.AssetContent.entities.Contains(entity))
                {
                    var list = cutscene.AssetContent.entities.ToList();
                    var index = list.IndexOf(entity);
                    list.Insert(index + 1, newEntity);
                    cutscene.AssetContent.entities = list.ToArray();
                    Reload();
                    TreeView_Cutscene.Nodes[i].Nodes[0].Expand();
                    TreeView_Cutscene.SelectedNode = TreeView_Cutscene.Nodes[i].Nodes[0].Nodes[index];
                    Text = Language.GetString("$CUTSCENE_EDITOR") + "*";
                    bIsFileEdited = true;
                    return;
                }
                else if (cutscene.SoundContent.EntityDefinitions.Contains(entity))
                {
                    var list = cutscene.SoundContent.EntityDefinitions.ToList();
                    var index = list.IndexOf(entity);
                    list.Insert(index + 1, newEntity);
                    cutscene.SoundContent.EntityDefinitions = list.ToArray();
                    Reload();
                    TreeView_Cutscene.Nodes[i].Nodes[1].Expand();
                    TreeView_Cutscene.SelectedNode = TreeView_Cutscene.Nodes[i].Nodes[1].Nodes[index];
                    Text = Language.GetString("$CUTSCENE_EDITOR") + "*";
                    bIsFileEdited = true;
                    return;
                }
            }
        }
    }
}
