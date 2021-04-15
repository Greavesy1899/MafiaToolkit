using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Gibbed.Illusion.FileFormats.Hashing;
using ResourceTypes.Prefab;
using Utils.Helpers;
using Utils.Language;
using Utils.StringHelpers;

namespace Mafia2Tool
{
    public partial class PrefabEditor : Form
    {
        private FileInfo prefabFile;
        private PrefabLoader prefabs;

        public PrefabEditor(FileInfo file)
        {
            InitializeComponent();
            Localise();
            prefabFile = file;
        }

        private void Localise()
        {
            Text = Language.GetString("PREFAB_EDITOR_TITLE");
            Button_File.Text = Language.GetString("$FILE");
            Button_Save.Text = Language.GetString("$SAVE");
            Button_Reload.Text = Language.GetString("$RELOAD");
            Button_Exit.Text = Language.GetString("$EXIT");
        }

        public void InitEditor(List<string> definitionNames)
        {
            prefabs = new PrefabLoader(prefabFile);

            Show();

            for(int i = 0; i < definitionNames.Count; i++)
            {
                var name = definitionNames[i];
                var hash = FNV64.Hash(name);

                foreach(var prefab in prefabs.Prefabs)
                {
                    if (hash == prefab.Hash)
                    {
                        prefab.AssignedName = name;
                    }
                    else
                    {
                        Console.WriteLine(prefab.Hash);
                    }
                }
            }

            foreach(var prefab in prefabs.Prefabs)
            {
                var name = string.IsNullOrEmpty(prefab.AssignedName) ? "Not Found!" : prefab.AssignedName;
                TreeNode node = new TreeNode();
                node.Tag = prefab;
                node.Text = name;
                node.Name = name;
                TreeView_Prefabs.Nodes.Add(node);
            }

        }

        private void OnNodeSelectSelect(object sender, TreeViewEventArgs e)
        {
            Grid_Prefabs.SelectedObject = e.Node.Tag;
        }

        private void Button_Export_Click(object sender, EventArgs e)
        {
            TreeNode SelectedNode = TreeView_Prefabs.SelectedNode;

            if (SelectedNode != null)
            {
                if (SelectedNode.Tag is PrefabLoader.PrefabStruct)
                {
                    PrefabLoader.PrefabStruct Prefab = (SelectedNode.Tag as PrefabLoader.PrefabStruct);
                    
                    if(Browser_ExportPRB.ShowDialog() == DialogResult.OK)
                    {
                        string FileName = Browser_ExportPRB.FileName;

                        using(BinaryWriter writer = new BinaryWriter(File.Open(FileName, FileMode.Create)))
                        {
                            writer.WriteString16(Prefab.AssignedName);
                            Prefab.WriteToFile(writer);
                        }
                    }
                }
            }
        }

        private void Button_Import_Click(object sender, EventArgs e)
        {
            if (Browser_ImportPRB.ShowDialog() == DialogResult.OK)
            {
                string FileName = Browser_ImportPRB.FileName;
                PrefabLoader.PrefabStruct NewPrefab = new PrefabLoader.PrefabStruct();

                using (BinaryReader reader = new BinaryReader(File.Open(FileName, FileMode.Open)))
                {
                    string AssignedName = StringHelpers.ReadString16(reader);
                    NewPrefab.ReadFromFile(reader);
                    NewPrefab.AssignedName = AssignedName;
                }

                TreeNode node = new TreeNode();
                node.Tag = NewPrefab;
                node.Text = NewPrefab.AssignedName;
                node.Name = NewPrefab.AssignedName;
                TreeView_Prefabs.Nodes.Add(node);
            }
        }

        private void Button_Delete_Click(object sender, EventArgs e)
        {
            TreeNode SelectedNode = TreeView_Prefabs.SelectedNode;

            if(SelectedNode != null)
            {
                SelectedNode.Remove();
            }
        }

        private void Button_Save_Click(object sender, EventArgs e)
        {
            List<PrefabLoader.PrefabStruct> NewPrefabs = new List<PrefabLoader.PrefabStruct>();

            foreach(TreeNode Node in TreeView_Prefabs.Nodes)
            {
                if(Node.Tag is PrefabLoader.PrefabStruct)
                {
                    PrefabLoader.PrefabStruct Prefab = (Node.Tag as PrefabLoader.PrefabStruct);
                    NewPrefabs.Add(Prefab);
                }
            }

            // Create backup, set our new prefabs, and then save.
            File.Copy(prefabFile.FullName, prefabFile.FullName + "_old", true);
            prefabs.Prefabs = NewPrefabs.ToArray();
            prefabs.WriteToFile(prefabFile);
        }
    }
}