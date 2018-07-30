﻿using Mafia2;
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
        private List<TreeNode> unadded = new List<TreeNode>();
        private IniFile ini = new IniFile();
        public FrameResourceTool()
        {
            InitializeComponent();
            if (SceneData.ScenePath == "")
            {
                folderBrowser.Description = "Select a folder which includes extracted SDS data.";
                if (folderBrowser.ShowDialog() == DialogResult.OK)
                {
                    string path = ini.Read("SDSPath", "Directories");
                    path = folderBrowser.SelectedPath;
                    SceneData.ScenePath = path;
                    ini.Write("SDSPath", path, "Directories");
                }
            }
            if (!MaterialData.HasLoaded)
            {
                folderBrowser.Description = "Select the folder which contains 'default*.mtl' files. (Located in ..Mafia II/edit/materials/)";
                if (folderBrowser.ShowDialog() == DialogResult.OK)
                {
                    string path = ini.Read("MaterialPath", "Directories");
                    path = folderBrowser.SelectedPath;
                    ini.Write("MaterialPath", path, "Directories");
                    MaterialData.Load();
                }
            }
            SceneData.BuildData();
            ReadFrameResource();
        }

        public void ReadFrameResource()
        {
            int numBlocks = SceneData.FrameResource.EntireFrame.Count - SceneData.FrameResource.Header.NumObjects;
            foreach (FrameNameTable.Data data in SceneData.FrameNameTable.FrameData)
            {
                int index = treeView1.Nodes.IndexOfKey(data.ParentName);

                if (index == -1)
                    treeView1.Nodes.Add(data.ParentName, data.ParentName);

                index = treeView1.Nodes.IndexOfKey(data.ParentName);

                TreeNode root = treeView1.Nodes[index];

                if (data.FrameIndex != -1)
                {
                    object block = SceneData.FrameResource.EntireFrame[(data.FrameIndex + numBlocks)];
                    if (block.GetType().BaseType == typeof(FrameObjectBase) || block.GetType().BaseType == typeof(FrameObjectJoint))
                    {
                        (block as FrameObjectBase).FrameNameTableFlags = data.Flags;
                        (block as FrameObjectBase).IsOnFrameTable = true;
                    }
                    else
                        throw new Exception("Unknown type.");

                    TreeNode node = CreateTreeNode((SceneData.FrameResource.EntireFrame[(data.FrameIndex + numBlocks)] as FrameObjectBase));

                    if (node == null)
                        continue;

                    root.Nodes.Add(node);
                }
            }
            for (int i = 0; i != SceneData.FrameResource.FrameBlocks.Length; i++)
                FrameResourceListBox.Items.Add(SceneData.FrameResource.FrameBlocks[i]);

            for (int i = 0; i != SceneData.FrameResource.FrameObjects.Length; i++)
                FrameResourceListBox.Items.Add(SceneData.FrameResource.FrameObjects[i]);

            for (int i = 0; i != SceneData.FrameResource.FrameObjects.Length; i++)
            {
                FrameObjectBase fObject = (FrameObjectBase)SceneData.FrameResource.FrameObjects[i];
                TreeNode node = CreateTreeNode(fObject);

                if (node == null)
                    continue;

                //if ParentIndex1 && ParentIndex2 equals then its kind of like the root of the object.
                //All subobjects of this 'root' should have ParentIndex2 set to the 'roots' parent.
                //ParentIndex1 is the unique one. 
                //Looks to me that the objects which have ParentIndex1 && ParentIndex2 having the same value is usually on the 'FrameNameTable'.

                //all nodes found are dumped here.
                TreeNode[] nodes = null;

                //so first we check for 'root' objects.
                if (fObject.ParentIndex1.Index == fObject.ParentIndex2.Index)
                {
                    nodes = treeView1.Nodes.Find(fObject.ParentIndex2.Name, true);
                }
                else if (fObject.ParentIndex1.Index != fObject.ParentIndex2.Index)
                {
                    //test, still trying to nail this fucker down.
                    nodes = treeView1.Nodes.Find(fObject.ParentIndex1.Name, true);
                }


                if (nodes.Length > 0)
                    nodes[0].Nodes.Add(node);
                else if (fObject.ParentIndex1.Index == -1 && fObject.ParentIndex2.Index == -1)
                    treeView1.Nodes.Add(node);
                else
                    unadded.Add(node);

            }

            foreach (TreeNode obj in unadded)
            {
                TreeNode[] nodes = treeView1.Nodes.Find((obj.Tag as FrameObjectBase).ParentIndex2.Name, true);

                if (nodes.Length > 0)
                    nodes[0].Nodes.Add(obj);
                else if ((treeView1.Nodes.Find((obj.Tag as FrameObjectBase).Name.String, true).Length != 0))
                    Debug.WriteLine("Unadded node was found");
                else
                    Debug.WriteLine(string.Format("WARNING: node: {0} was not added properly", obj.Name));
            }
        }
        private TreeNode AddChildren(TreeNode node, FrameObjectBase fObject)
        {
            while (fObject.ParentIndex1.Index != -1)
            {
                fObject = (SceneData.FrameResource.EntireFrame[fObject.ParentIndex1.Index] as FrameObjectBase);

                if (fObject.ParentIndex1.Index == fObject.ParentIndex2.Index)
                    return node;

                TreeNode child = CreateTreeNode(fObject);

                if (child == null)
                    return node;

                node.Nodes.Add(child);
            }
            return node;
        }

        private TreeNode CreateTreeNode(string NameText, int index)
        {
            TreeNode node = new TreeNode
            {
                Name = NameText,
                Text = NameText,
                Tag = SceneData.FrameResource.FrameBlocks[index]
            };

            return node;
        }
        private TreeNode CreateTreeNode(FrameObjectBase fObject)
        {
            TreeNode[] nodes2 = treeView1.Nodes.Find(fObject.Name.String, true);

            if (nodes2.Length > 0)
                return null;

            TreeNode node = ConvertNode(fObject.NodeData);

            if (fObject.GetType() == typeof(FrameObjectSingleMesh))
            {
                node.Nodes.Add(CreateTreeNode("Material", (fObject as FrameObjectSingleMesh).MaterialIndex));
                node.Nodes.Add(CreateTreeNode("Geometry", (fObject as FrameObjectSingleMesh).MeshIndex));
                node.ContextMenuStrip = contextMenu;
            }
            else if (fObject.GetType() == typeof(FrameObjectModel))
            {
                node.Nodes.Add(CreateTreeNode("Material", (fObject as FrameObjectModel).MaterialIndex));
                node.Nodes.Add(CreateTreeNode("Geometry", (fObject as FrameObjectModel).MeshIndex));
                node.Nodes.Add(CreateTreeNode("Skeleton Info", (fObject as FrameObjectModel).SkeletonIndex));
                node.Nodes.Add(CreateTreeNode("Skeleton Hierachy Info", (fObject as FrameObjectModel).SkeletonHierachyIndex));
                node.ContextMenuStrip = contextMenu;
            }

            node = AddChildren(node, fObject);
            return node;
        }
        private TreeNode ConvertNode(Node node)
        {
            TreeNode treeNode = new TreeNode()
            {
                Name = node.NameText,
                Text = node.NameText,
                Tag = node.Tag,
            };

            return treeNode;
        }

        private Vector3 RetrieveParent1Position(FrameObjectSingleMesh mesh)
        {
            Vector3 curPos;
            curPos = mesh.Matrix.Position;
            FrameObjectBase parent = (SceneData.FrameResource.EntireFrame[mesh.ParentIndex1.Index] as FrameObjectBase);

            while (parent != null)
            {
                if (parent.GetType() == typeof(FrameObjectFrame))
                {
                    if ((parent as FrameObjectFrame).Item != null)
                        curPos += (parent as FrameObjectFrame).Item.Position;
                }
                else
                {
                    curPos += parent.Matrix.Position;
                }

                if (parent.ParentIndex1.Index != -1)
                    parent = (SceneData.FrameResource.EntireFrame[parent.ParentIndex1.Index] as FrameObjectBase);
                else
                    parent = null;
            }

            return curPos;
        }

        private void OnSelectedChanged(object sender, EventArgs e)
        {
            FrameResourceGrid.SelectedObject = FrameResourceListBox.SelectedItem;
        }
        private void OnClickLoadAll(object sender, EventArgs e)
        {
            List<object> meshes = new List<object>();

            for (int i = 0; i != SceneData.FrameResource.EntireFrame.Count; i++)
            {
                object fObject = SceneData.FrameResource.EntireFrame[i];

                if (fObject.GetType() == typeof(FrameObjectSingleMesh))
                {
                    meshes.Add(fObject);
                }
                if (fObject.GetType() == typeof(FrameObjectModel))
                {
                    meshes.Add(fObject);
                }
            }


            string[] fileNames = new string[meshes.Count];
            Vector3[] filePos = new Vector3[meshes.Count];
            Matrix33[] rotPos = new Matrix33[meshes.Count];

            CustomEDD frameEDD = new CustomEDD();
            frameEDD.EntryCount = meshes.Count;
            frameEDD.Entries = new CustomEDD.Entry[frameEDD.EntryCount];

            Parallel.For(0, meshes.Count, i =>
            {
                CustomEDD.Entry entry = new CustomEDD.Entry();
                FrameObjectSingleMesh mesh = (meshes[i] as FrameObjectSingleMesh);
                FrameGeometry geom = SceneData.FrameResource.EntireFrame[mesh.MeshIndex] as FrameGeometry;
                FrameMaterial mat = SceneData.FrameResource.EntireFrame[mesh.MaterialIndex] as FrameMaterial;
                IndexBuffer[] indexBuffers = new IndexBuffer[geom.LOD.Length];
                VertexBuffer[] vertexBuffers = new VertexBuffer[geom.LOD.Length];

                //we need to retrieve buffers first.
                for (int c = 0; c != geom.LOD.Length; c++)
                {
                    indexBuffers[c] = SceneData.IndexBufferPool.GetBuffer(geom.LOD[c].IndexBufferRef.uHash);
                    vertexBuffers[c] = SceneData.VertexBufferPool.GetBuffer(geom.LOD[c].VertexBufferRef.uHash);
                }

                Model newModel = new Model(mesh, indexBuffers, vertexBuffers, geom, mat);

                if (mesh.ParentIndex1.Index != -1)
                {
                    FrameObjectBase parent = (SceneData.FrameResource.EntireFrame[mesh.ParentIndex1.Index] as FrameObjectBase);
                    filePos[i] = RetrieveParent1Position(mesh);
                }

                if (((mesh.ParentIndex1.Index != -1)) && ((mesh.ParentIndex1.Index == mesh.ParentIndex2.Index)))
                {
                    FrameObjectFrame frame = SceneData.FrameResource.EntireFrame[mesh.ParentIndex1.Index] as FrameObjectFrame;
                    if (frame.Item != null)
                    {
                        filePos[i] = frame.Item.Position;
                    }
                }

                entry.LodCount = newModel.Lods.Length;
                entry.LODNames = new string[entry.LodCount];

                for (int c = 0; c != newModel.Lods.Length; c++)
                {
                    string edmName;
                    FrameGeometry meshGeom;

                    if (mesh.Name.String != "")
                    {
                        edmName = mesh.Name.String;
                    }
                    else
                    {
                        if (mesh.Mesh == null)
                        {
                            meshGeom = SceneData.FrameResource.EntireFrame[mesh.MeshIndex] as FrameGeometry;
                            edmName = meshGeom.LOD[c].VertexBufferRef.String;
                            edmName.Remove(edmName.Length - 5);
                        }
                        else
                        {
                            meshGeom = mesh.Mesh;
                            edmName = meshGeom.LOD[c].VertexBufferRef.String;
                        }
                    }
                    newModel.ExportToM2T();

                    entry.LODNames[c] = newModel.FrameMesh.Name.String;
                }
                entry.Position = mesh.Matrix.Position;
                entry.Rotation = mesh.Matrix.Rotation;

                frameEDD.Entries[i] = entry;

            });

            using (BinaryWriter writer = new BinaryWriter(File.Create("exported/frame.edd")))
            {
                frameEDD.WriteToFile(writer);
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

        private void LoadMaterialTool(object sender, EventArgs e)
        {
            if (!MaterialData.HasLoaded)
                return;

            MaterialTool tool = new MaterialTool();
            tool.ShowDialog();
        }

        private void CollisionEditor_Click(object sender, EventArgs e)
        {
            CollisionEditor editor = new CollisionEditor();
        }

        private void OverwriteBuffer_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null)
                return;

            if ((treeView1.SelectedNode.Tag.GetType() == typeof(FrameObjectSingleMesh) || (treeView1.SelectedNode.Tag.GetType() == typeof(FrameObjectModel))))
            {
                if ((treeView1.SelectedNode.Tag as FrameObjectSingleMesh) == null)
                    return;

                Model model = new Model();
                model.FrameMesh = treeView1.SelectedNode.Tag as FrameObjectSingleMesh;
                model.FrameGeometry = SceneData.FrameResource.EntireFrame[model.FrameMesh.MeshIndex] as FrameGeometry;
                model.FrameMaterial = SceneData.FrameResource.EntireFrame[model.FrameMesh.MaterialIndex] as FrameMaterial;

                BufferLocationStruct[] iIndexes = new BufferLocationStruct[model.FrameGeometry.NumLods];
                BufferLocationStruct[] iVertexes = new BufferLocationStruct[model.FrameGeometry.NumLods];
                ulong[] indexRefs = new ulong[model.FrameGeometry.NumLods];
                ulong[] vertexRefs = new ulong[model.FrameGeometry.NumLods];
                model.IndexBuffers = new IndexBuffer[model.FrameGeometry.NumLods];
                model.VertexBuffers = new VertexBuffer[model.FrameGeometry.NumLods];

                for (int i = 0; i != model.FrameGeometry.NumLods; i++)
                {
                    indexRefs[i] = model.FrameGeometry.LOD[i].IndexBufferRef.uHash;
                    iIndexes[i] = SceneData.IndexBufferPool.SearchBuffer(indexRefs[i]);
                    vertexRefs[i] = model.FrameGeometry.LOD[i].VertexBufferRef.uHash;
                    iVertexes[i] = SceneData.VertexBufferPool.SearchBuffer(vertexRefs[i]);
                    model.IndexBuffers[i] = SceneData.IndexBufferPool.GetBuffer(indexRefs[i]);
                    model.VertexBuffers[i] = SceneData.VertexBufferPool.GetBuffer(vertexRefs[i]);

                    if (iIndexes[i] == null || iVertexes[i] == null)
                        return;

                }

                m2tBrowser.ShowDialog();

                if (m2tBrowser.FileName == null)
                    return;

                using (BinaryReader reader = new BinaryReader(File.Open(m2tBrowser.FileName, FileMode.Open)))
                {
                    model.ReadFromM2T(reader);
                    model.CalculateBounds();
                    model.CalculateDecompression();
                    model.BuildIndexBuffer();
                    model.BuildVertexBuffer();
                }
                model.UpdateObjectsFromModel();

                treeView1.SelectedNode.Tag = model.FrameMesh;
                SceneData.FrameResource.EntireFrame[model.FrameMesh.MeshIndex] = model.FrameGeometry;
                SceneData.FrameResource.EntireFrame[model.FrameMesh.MaterialIndex] = model.FrameMaterial;
                SceneData.IndexBufferPool.BufferPools[iIndexes[0].PoolLocation].Buffers[iIndexes[0].BufferLocation] = model.IndexBuffers[0];
                SceneData.VertexBufferPool.BufferPools[iVertexes[0].PoolLocation].Buffers[iVertexes[0].BufferLocation] = model.VertexBuffers[0];
                SceneData.IndexBufferPool.WriteToFile();
                SceneData.VertexBufferPool.WriteToFile();
            }
            else
            {
                MessageBox.Show("Click on a \"Single Mesh\" type of \"Model\" type in the tree view.", "Error");
            }
        }

        private void OnExit(object sender, FormClosingEventArgs e)
        {
            SaveChanges();
        }

        private void SaveClick(object sender, EventArgs e)
        {
            SaveChanges();
        }

        private void ExitClick(object sender, EventArgs e)
        {
            SaveChanges();
            Application.Exit();
        }

        private void SaveChanges()
        {
            DialogResult result = MessageBox.Show("Do you want to save your changes?", "Save Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open("FrameResource_0.bin", FileMode.Create)))
                {
                    SceneData.FrameResource.WriteToFile(writer);
                }
                using (BinaryWriter writer = new BinaryWriter(File.Open("FrameNameTable_0.bin", FileMode.Create)))
                {
                    FrameNameTable nameTable = new FrameNameTable();
                    nameTable.BuildDataFromResource(SceneData.FrameResource);
                    nameTable.AddNames();
                    nameTable.WriteToFile(writer);
                }
                MessageBox.Show("Your saved file has been stored in the same folder as the executable.", "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void ReloadClick(object sender, EventArgs e)
        {
            SceneData.Reload();
            treeView1.Nodes.Clear();
            FrameResourceListBox.Items.Clear();
            ReadFrameResource();
        }

        private void OpenClick(object sender, EventArgs e)
        {
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                string path = ini.Read("SDSPath", "Directories");
                path = folderBrowser.SelectedPath;
                SceneData.ScenePath = path;
                ini.Write("SDSPath", path, "Directories");
            }
            SceneData.Reload();
            treeView1.Nodes.Clear();
            FrameResourceListBox.Items.Clear();
            ReadFrameResource();
        }

        private void ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name == "contextExtract3D")
            {
                FrameObjectSingleMesh mesh = treeView1.SelectedNode.Tag as FrameObjectSingleMesh;

                FrameGeometry geom = SceneData.FrameResource.EntireFrame[mesh.MeshIndex] as FrameGeometry;
                FrameMaterial mat = SceneData.FrameResource.EntireFrame[mesh.MaterialIndex] as FrameMaterial;
                IndexBuffer[] indexBuffers = new IndexBuffer[geom.LOD.Length];
                VertexBuffer[] vertexBuffers = new VertexBuffer[geom.LOD.Length];

                //we need to retrieve buffers first.
                for (int c = 0; c != geom.LOD.Length; c++)
                {
                    indexBuffers[c] = SceneData.IndexBufferPool.GetBuffer(geom.LOD[c].IndexBufferRef.uHash);
                    vertexBuffers[c] = SceneData.VertexBufferPool.GetBuffer(geom.LOD[c].VertexBufferRef.uHash);
                }

                Model newModel = new Model(mesh, indexBuffers, vertexBuffers, geom, mat);
                newModel.ExportToM2T();
            }
        }

        private void OnDelete(object sender, EventArgs e)
        {
            FrameObjectBase fObject = treeView1.SelectedNode.Tag as FrameObjectBase;
            for (int i = 0; i != SceneData.FrameResource.EntireFrame.Count; i++)
            {
                object block = SceneData.FrameResource.EntireFrame[i];
                if (block == fObject)
                {
                    SceneData.FrameResource.EntireFrame.RemoveAt(i);
                }
            }
            treeView1.SelectedNode.Remove();
        }

        private void OnSelect(object sender, TreeViewEventArgs e)
        {
            FrameResourceGrid.SelectedObject = treeView1.SelectedNode.Tag;
        }
    }
}