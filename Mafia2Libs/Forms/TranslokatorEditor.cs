using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResourceTypes.Translokator;
using System.Windows.Forms;

namespace Mafia2Tool.Forms
{
    public partial class TranslokatorEditor : Form
    {
        private FileInfo file;
        private TranslokatorLoader translokator;
        public TranslokatorEditor(FileInfo info)
        {
            InitializeComponent();
            file = info;
            Localise();
            LoadFile();
            Show();
        }

        private void Localise()
        {

        }

        private void LoadFile()
        {
            translokator = new TranslokatorLoader(file);
            TranslokatorTree.Nodes.Clear();

            TreeNode headerData = new TreeNode("Header Data");
            headerData.Tag = translokator;

            TreeNode gridNode = new TreeNode("Grids");
            for (int i = 0; i < translokator.Grids.Length; i++)
            {
                Grid grid = translokator.Grids[i];
                TreeNode child = new TreeNode("Grid " + i);
                child.Tag = grid;
                gridNode.Nodes.Add(child);
            }
            TreeNode ogNode = new TreeNode("Objects Groups");
            for (int i = 0; i < translokator.ObjectGroups.Length; i++)
            {
                ObjectGroup objectGroup = translokator.ObjectGroups[i];
                TreeNode objectGroupNode = new TreeNode("Object Group " + i);
                objectGroupNode.Tag = objectGroup;

                for(int y = 0; y < objectGroup.Objects.Length; y++)
                {
                    ResourceTypes.Translokator.Object obj = objectGroup.Objects[y];
                    TreeNode objNode = new TreeNode(obj.Name);
                    objNode.Tag = obj;
                    objectGroupNode.Nodes.Add(objNode);

                    for(int x = 0; x < obj.Instances.Length; x++)
                    {
                        Instance instance = obj.Instances[x];
                        TreeNode instanceNode = new TreeNode(obj.Name + " " + x);
                        instanceNode.Tag = instance;
                        objNode.Nodes.Add(instanceNode);
                    }
                }

                ogNode.Nodes.Add(objectGroupNode);
            }
            TranslokatorTree.Nodes.Add(headerData);
            TranslokatorTree.Nodes.Add(gridNode);
            TranslokatorTree.Nodes.Add(ogNode);
        }

        private void SaveFile()
        {
            translokator.Grids = new Grid[TranslokatorTree.Nodes[1].GetNodeCount(false)];
            for (int i = 0; i < translokator.Grids.Length; i++)
            {
                Grid grid = (TranslokatorTree.Nodes[1].Nodes[i].Tag as Grid);
                translokator.Grids[i] = grid;
            }

            translokator.ObjectGroups = new ObjectGroup[TranslokatorTree.Nodes[2].GetNodeCount(false)];
            for (int i = 0; i < translokator.ObjectGroups.Length; i++)
            {
                ObjectGroup objectGroup = (TranslokatorTree.Nodes[2].Nodes[i].Tag as ObjectGroup);
                objectGroup.Objects = new ResourceTypes.Translokator.Object[TranslokatorTree.Nodes[2].Nodes[i].GetNodeCount(false)];
                objectGroup.NumObjects = objectGroup.Objects.Length;
                for (int y = 0; y < objectGroup.Objects.Length; y++)
                {
                    ResourceTypes.Translokator.Object obj = (TranslokatorTree.Nodes[2].Nodes[i].Nodes[y].Tag as ResourceTypes.Translokator.Object);
                    obj.Instances = new Instance[TranslokatorTree.Nodes[2].Nodes[i].Nodes[y].GetNodeCount(false)];
                    obj.NumInstances = obj.Instances.Length;
                    for (int z = 0; z < obj.Instances.Length; z++)
                    {
                        Instance instance = (TranslokatorTree.Nodes[2].Nodes[i].Nodes[y].Nodes[z].Tag as Instance);
                        obj.Instances[z] = instance;
                    }
                    objectGroup.Objects[y] = obj;
                }

                translokator.ObjectGroups[i] = objectGroup;
            }
            translokator.WriteToFile(file);
        }

        private void TranslokatorTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (TranslokatorTree?.SelectedNode.Tag != null)
            {
                PropertyGrid.SelectedObject = TranslokatorTree?.SelectedNode.Tag;
            }
        }

        private void ReloadButton_Click(object sender, EventArgs e)
        {
            LoadFile();
        }

        private void SaveToolButton_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
