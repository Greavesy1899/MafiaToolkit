using Mafia2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mafia2Tool
{
    public partial class FrameResourceTool : Form
    {
        private List<FrameObjectSingleMesh> mesh = new List<FrameObjectSingleMesh>();

        public FrameResourceTool()
        {
            InitializeComponent();
            if(SceneData.ScenePath == null)
                folderBrowser.ShowDialog();
            SceneData.BuildData();
            ReadFrameResource();
        }

        public void ReadFrameResource()
        {
            foreach (FrameNameTable.Data data in SceneData.FrameNameTable.FrameData)
            {
                int index = treeView1.Nodes.IndexOfKey(data.ParentName);

                if (index == -1)
                    treeView1.Nodes.Add(data.ParentName, data.ParentName);

                index = treeView1.Nodes.IndexOfKey(data.ParentName);

                TreeNode root = treeView1.Nodes[index];

                TreeNode node = createTreeNode((SceneData.FrameResource.FrameObjects[data.FrameIndex] as FrameObjectBase));

                if (node == null)
                    continue;

                root.Nodes.Add(node);
            }
            for (int i = 0; i != SceneData.FrameResource.FrameBlocks.Length; i++)
                FrameResourceListBox.Items.Add(SceneData.FrameResource.FrameBlocks[i]);

            for (int i = 0; i != SceneData.FrameResource.FrameObjects.Length; i++)
                FrameResourceListBox.Items.Add(SceneData.FrameResource.FrameObjects[i]);

            foreach (FrameObjectBase fObject in SceneData.FrameResource.FrameObjects)
            {
                TreeNode node = createTreeNode(fObject);

                if (node == null)
                    continue;

                int index = 0;

                for (int i = 0; i != treeView1.Nodes.Count; i++)
                {
                    if (index != -1)
                        continue;

                    index = treeView1.Nodes[i].Nodes.IndexOfKey(fObject.ParentIndex2.Name);
                }

                TreeNode[] nodes = treeView1.Nodes.Find(fObject.ParentIndex2.Name, true);

                if (fObject.ParentIndex2.Index == -1)
                    treeView1.Nodes[0].Nodes.Add(node);

                if (nodes.Length > 0)
                    nodes[0].Nodes.Add(node);
            }
        }

        private TreeNode createTreeNode(string NameText, int index)
        {
            TreeNode node = new TreeNode
            {
                Name = NameText,
                Text = NameText,
                Tag = SceneData.FrameResource.FrameBlocks[index]
            };

            return node;
        }
        private TreeNode createTreeNode(FrameObjectBase fObject)
        {
            TreeNode[] nodes2 = treeView1.Nodes.Find(fObject.Name.String, true);

            if (nodes2.Length > 0)
                return null;

            TreeNode node = convertNode(fObject.NodeData);

            if (fObject.GetType() == typeof(FrameObjectSingleMesh))
            {
                node.Nodes.Add(createTreeNode("Material", (fObject as FrameObjectSingleMesh).MaterialIndex));
                node.Nodes.Add(createTreeNode("Geometry", (fObject as FrameObjectSingleMesh).MeshIndex));
                mesh.Add((fObject as FrameObjectSingleMesh));
            }
            else if (fObject.GetType() == typeof(FrameObjectModel))
            {
                node.Nodes.Add(createTreeNode("Material", (fObject as FrameObjectModel).MaterialIndex));
                node.Nodes.Add(createTreeNode("Geometry", (fObject as FrameObjectModel).MeshIndex));
                node.Nodes.Add(createTreeNode("Skeleton Info", (fObject as FrameObjectModel).SkeletonIndex));
                node.Nodes.Add(createTreeNode("Skeleton Hierachy Info", (fObject as FrameObjectModel).SkeletonHierachyIndex));
                mesh.Add((fObject as FrameObjectModel));
            }

            return node;
        }
        private TreeNode convertNode(Node node)
        {
            TreeNode treeNode = new TreeNode()
            {
                Name = node.NameText,
                Text = node.NameText,
                Tag = node.Tag,
            };

            return treeNode;
        }

        private void OnSelectedChanged(object sender, EventArgs e)
        {
            FrameResourceGrid.SelectedObject = FrameResourceListBox.SelectedItem;
        }
        private void LoadMaterialTool(object sender, EventArgs e)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open("FrameResource_0.bin", FileMode.Create)))
            {
                SceneData.FrameResource.WriteToFile(writer);
            }

            MaterialTool tool = new MaterialTool();
            tool.ShowDialog();
        }
        private async void OnClickLoad3D(object sender, EventArgs e)
        {
            string[] fileNames = new string[mesh.Count];
            Vector3[] filePos = new Vector3[mesh.Count];
            Matrix33[] rotPos = new Matrix33[mesh.Count];

            Parallel.For(0, mesh.Count, i =>
            {
                Model newModel = new Model((mesh[i]), SceneData.VertexBufferPool, SceneData.IndexBufferPool, SceneData.FrameResource);
                fileNames[i] = mesh[i].Name.String + "_lod0";

                filePos[i] = mesh[i].Matrix.Position;
                rotPos[i] = mesh[i].Matrix.Rotation;

                if (((mesh[i].ParentIndex1.Index != -1)) && ((mesh[i].ParentIndex1.Index == mesh[i].ParentIndex2.Index)))
                {
                    FrameObjectFrame frame = SceneData.FrameResource.EntireFrame[mesh[i].ParentIndex1.Index] as FrameObjectFrame;
                    if (frame.Item != null)
                    {
                        filePos[i] = frame.Item.Position;
                    }
                }

                for (int c = 0; c != newModel.Lods.Length; c++)
                {
                    if (!File.Exists("exported/" + mesh[i].Name.String + "_lod" + c + ".edm"))
                    {
                        Stopwatch watch = new Stopwatch();
                        watch.Start();
                        newModel.ExportToEDM(newModel.Lods[c], mesh[i].Name.String + "_lod" + c);
                        Debug.WriteLine("Mesh: {0} and time taken was {1}", mesh[i].Name.String + "_lod" + c, watch.Elapsed);
                        watch.Stop();
                    }
                    //fileNames[i] = mesh[i].Name.Name + "_lod" + c;

                    Console.WriteLine("{0}/{1}", i, mesh.Count);
                }
            });

            using (BinaryWriter writer = new BinaryWriter(File.Create("exported/frame.edd")))
            {
                writer.Write(mesh.Count);

                for (int i = 0; i != mesh.Count; i++)
                {
                    writer.Write(fileNames[i]);
                    filePos[i].WriteToFile(writer);
                    rotPos[i].WriteToFile(writer);
                }
            }
        }
        private void OnNodeSelect(object sender, TreeViewEventArgs e)
        {
            FrameResourceGrid.SelectedObject = treeView1.SelectedNode.Tag;
        }
        private void SwitchView(object sender, EventArgs e)
        {
            treeView1.Visible = (!treeView1.Visible) ? true : false;
            FrameResourceListBox.Visible = (!FrameResourceListBox.Visible) ? true : false;
        }

        private void collisionEditor_Click(object sender, EventArgs e)
        {
            CollisionEditor editor = new CollisionEditor();
        }
    }
}
