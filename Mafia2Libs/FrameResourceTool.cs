using Mafia2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Mafia2Tool
{
    public partial class FrameResourceTool : Form
    {

        FrameResource frameResource;
        IndexBufferPool indexBufferPool;
        VertexBufferPool vertexBufferPool;

        private List<FrameObjectSingleMesh> mesh = new List<FrameObjectSingleMesh>();

        public FrameResourceTool()
        {
            InitializeComponent();
            MaterialsParse.ReadMatFile("default.mtl");

            ReadFrameResource();
        }

        public void ReadFrameResource()
        {

            using (BinaryReader reader = new BinaryReader(File.Open("FrameResource_0.bin", FileMode.Open)))
            {
                frameResource = new FrameResource();
                frameResource.ReadFromFile(reader);
                frameResource.DefineFrameBlockParents();

                for (int i = 0; i != frameResource.FrameBlocks.Count; i++)
                {
                    FrameResourceListBox.Items.Add(frameResource.FrameBlocks[i]);

                    FrameObjectBase frame = frameResource.FrameBlocks[i] as FrameObjectBase;

                    if (frame == null)
                        continue;

                    string nodeText = "";

                    if (frame.Name.Name == "")
                        nodeText = "Joint block";
                    else
                        nodeText = frame.Name.Name;
                    
                    TreeNode node = new TreeNode()
                    {
                        Name = nodeText,
                        Text = nodeText,
                        Tag = frameResource.FrameBlocks[i],
                    };

                    if (frameResource.FrameBlocks[i].GetType() == typeof(FrameObjectSingleMesh))
                    {
                        FrameObjectSingleMesh singleMesh = frameResource.FrameBlocks[i] as FrameObjectSingleMesh;
                        node.Nodes.Add(createTreeNode("Material", singleMesh.MaterialIndex));
                        node.Nodes.Add(createTreeNode("Geometry", singleMesh.MeshIndex));
                        mesh.Add(singleMesh);

                        if (frameResource.FrameBlocks[i].GetType() == typeof(FrameObjectModel))
                        {
                            FrameObjectModel modelMesh = frameResource.FrameBlocks[i] as FrameObjectModel;
                            node.Nodes.Add(createTreeNode("Blend Info", modelMesh.BlendInfoIndex));
                            node.Nodes.Add(createTreeNode("Skeleton Info", modelMesh.SkeletonIndex));
                            node.Nodes.Add(createTreeNode("Skeleton Hierachy Info", modelMesh.SkeletonHierachyIndex));
                        }
                    }

                    if (treeView1.Nodes.ContainsKey(frame.ParentIndex2.Name))
                    {
                        string name = frame.ParentIndex2.Name;
                        int index = treeView1.Nodes.IndexOfKey(frame.ParentIndex2.Name);
                        treeView1.Nodes[index].Nodes.Add(node);
                    }
                    else
                    {
                        string name = "";
                        int pIndex = frame.ParentIndex2.Index;

                        if (pIndex == -1)
                        {
                            treeView1.Nodes.Add(node);
                            continue;
                        }
                        else if (frameResource.FrameBlocks[pIndex].GetType() == typeof(FrameHeaderScene))
                        {
                            name = (frameResource.FrameBlocks[pIndex] as FrameHeaderScene).Name.Name;
                        }
                        else if (frameResource.FrameBlocks[pIndex].GetType() == typeof(FrameObjectFrame))
                        {
                            name = (frameResource.FrameBlocks[pIndex] as FrameObjectFrame).Name.Name;
                        }

                        treeView1.Nodes.Add(createTreeNode(name, frameResource.FrameBlocks[pIndex]));
                        int index = treeView1.Nodes.IndexOfKey(frame.ParentIndex2.Name);
                        treeView1.Nodes[index].Nodes.Add(node);
                    }
                }
            }
        }

        private TreeNode createTreeNode(string NameText, int index)
        {
            TreeNode node = new TreeNode
            {
                Name = NameText,
                Text = NameText,
                Tag = frameResource.FrameBlocks[index]
            };

            return node;
        }
        private TreeNode createTreeNode(string NameText, object frame)
        {
            TreeNode node = new TreeNode
            {
                Name = NameText,
                Text = NameText,
                Tag = frame
            };

            return node;
        }

        private void OnSelectedChanged(object sender, System.EventArgs e)
        {
            FrameResourceGrid.SelectedObject = FrameResourceListBox.SelectedItem;
        }
        private void LoadMaterialTool(object sender, System.EventArgs e)
        {
            MaterialTool tool = new MaterialTool();
            tool.ShowDialog();
        }
        private void OnClickLoad3D(object sender, System.EventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo(Application.StartupPath);
            FileInfo[] files = dir.GetFiles();

            List<FileInfo> indexFiles = new List<FileInfo>();
            List<FileInfo> vertexFiles = new List<FileInfo>();

            foreach (FileInfo file in files)
            {
                if (file.FullName.Contains("IndexBufferPool"))
                    indexFiles.Add(file);
                if (file.FullName.Contains("VertexBufferPool"))
                    vertexFiles.Add(file);
            }

            indexBufferPool = new IndexBufferPool(indexFiles);
            vertexBufferPool = new VertexBufferPool(vertexFiles);

            string[] fileNames = new string[mesh.Count];
            Vector3[] filePos = new Vector3[mesh.Count];
            float[] rotPos = new float[mesh.Count];

            for (int i = 0; i != mesh.Count; i++)
            {
                Model newModel = new Model((mesh[i]), vertexBufferPool, indexBufferPool, frameResource);

                for (int c = 0; c != newModel.Lods.Length; c++)
                {
                    if (!File.Exists("exported/" + mesh[i].Name.Name + "_lod" + c + ".edm"))
                    {
                        Stopwatch watch = new Stopwatch();
                        watch.Start();
                        //newModel.ExportToOBJ(newModel.Lods[c], mesh[i].Name.Name + "_lod" + c);
                        newModel.ExportToEDM(newModel.Lods[c], mesh[i].Name.Name + "_lod" + c);
                        Debug.WriteLine("Mesh: {0} and time taken was {1}", mesh[i].Name.Name + "_lod" + c, watch.Elapsed);
                        watch.Stop();
                    }
                    fileNames[i] = mesh[i].Name.Name + "_lod" + c;
                    filePos[i] = mesh[i].Matrix.Position;
                    rotPos[i] = mesh[i].Matrix.Rotation.Vector.X;

                    Console.WriteLine("{0}/{1}", i, mesh.Count);
                }
            }

            using (BinaryWriter writer = new BinaryWriter(File.Create("exported/frame.edd")))
            {
                writer.Write(mesh.Count);

                for (int i = 0; i != mesh.Count; i++)
                {
                    writer.Write(fileNames[i]);
                    writer.Write(filePos[i].X);
                    writer.Write(filePos[i].Y);
                    writer.Write(filePos[i].Z);
                    writer.Write(rotPos[i]);
                }
            }
        }
        private void OnNodeSelect(object sender, TreeViewEventArgs e)
        {
            FrameResourceGrid.SelectedObject = treeView1.SelectedNode.Tag;
        }
    }
}
