using Core.IO;
using ResourceTypes.Cutscene;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mafia2Tool.Forms
{
    public partial class CutsceneEditor : Form
    {
        // File access. We should not directly edit cutscene from here.
        private FileCutscene OriginalFile;

        private CutsceneLoader.Cutscene[] Cutscenes;

        public CutsceneEditor(FileCutscene CutsceneFile)
        {
            InitializeComponent();
            OriginalFile = CutsceneFile;

            Localise();
            InitialiseData();
        }

        public void Localise()
        {

        }

        private void AddCutsceneToTreeView(CutsceneLoader.Cutscene Cutscene)
        {
            TreeNode CutsceneParent = new TreeNode(Cutscene.CutsceneName);

            if(Cutscene.AssetContent != null)
            {
                var Assets = Cutscene.AssetContent;
                TreeNode AssetsParent = new TreeNode("Game Cutscene Content: (GCS Data)");
                
                for(int i = 0; i < Assets.entities.Length; i++)
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

        public void InitialiseData()
        {
            Cutscenes = OriginalFile.GetCutsceneLoader().Cutscenes;

            for (int i = 0; i < Cutscenes.Length; i++)
            {
                AddCutsceneToTreeView(Cutscenes[i]);
            }
        }

        private void TreeView_Cutscene_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if(e.Action == TreeViewAction.ByKeyboard || e.Action == TreeViewAction.ByMouse)
            {
                PropertyGrid_Cutscene.SelectedObject = e.Node.Tag;
            }
        }
    }
}
