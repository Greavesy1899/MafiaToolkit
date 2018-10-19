using Mafia2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mafia2Tool
{
    public partial class FrameResourceTool : Form
    {
        private List<TreeNode> unadded = new List<TreeNode>();
        private FileInfo fileLocation;

        public FrameResourceTool(FileInfo info)
        {
            InitializeComponent();
            Localise();
            SceneData.ScenePath = info.DirectoryName;
            fileLocation = info;
            SceneData.BuildData();
            PopulateForm();
            ShowDialog();
        }

        private void Localise()
        {
            fileToolButton.Text = Language.GetString("$FILE");
            saveToolStripMenuItem.Text = Language.GetString("$SAVE");
            reloadToolStripMenuItem.Text = Language.GetString("$RELOAD");
            exitToolStripMenuItem.Text = Language.GetString("$EXIT");
            toolsButton.Text = Language.GetString("$TOOLS");
            exportAllSubButton.Text = Language.GetString("$FRAME_EDITOR_EXPORT_BTN");
            modelsToolStripMenuItem.Text = Language.GetString("$FRAME_EDITOR_MODELS");
            allToolStripMenuItem.Text = Language.GetString("$FRAME_EDITOR_ALL");
            overwriteBufferSubButton.Text = Language.GetString("$FRAME_EDITOR_OVERWRITE_BUFFER");
            addButton.Text = Language.GetString("$FRAME_EDITOR_ADD_SINGLEMODEL");
            viewToolButton.Text = Language.GetString("$VIEW");
            switchViewSubButton.Text = Language.GetString("$FRAME_EDITOR_SWITCH_VIEW");
            contextExtract3D.Text = Language.GetString("$FRAME_EDITOR_EXTRACT3D");
            contextDelete.Text = Language.GetString("$DELETE");
            contextUpdateParents.Text = Language.GetString("$FRAME_EDITOR_UPDATE_PARENTS");
            Text = Language.GetString("$FRAME_EDITOR_TITLE");
        }

        public void PopulateForm()
        {
            foreach (KeyValuePair<int, FrameHeaderScene> entry in SceneData.FrameResource.FrameScenes)
            {
                FrameResourceListBox.Items.Add(entry.Value);
                TreeNode node = new TreeNode
                {
                    Text = entry.Value.Name.String,
                    Name = entry.Value.Name.String,
                    Tag = entry.Value
                };
                treeView1.Nodes.Add(node);
            }
            foreach (KeyValuePair<int, FrameGeometry> entry in SceneData.FrameResource.FrameGeometries)
            {
                FrameResourceListBox.Items.Add(entry.Value);
            }
            foreach (KeyValuePair<int, FrameMaterial> entry in SceneData.FrameResource.FrameMaterials)
            {
                FrameResourceListBox.Items.Add(entry.Value);
            }
            foreach (KeyValuePair<int, FrameBlendInfo> entry in SceneData.FrameResource.FrameBlendInfos)
            {
                FrameResourceListBox.Items.Add(entry.Value);
            }
            foreach (KeyValuePair<int, FrameSkeleton> entry in SceneData.FrameResource.FrameSkeletons)
            {
                FrameResourceListBox.Items.Add(entry.Value);
            }
            foreach (KeyValuePair<int, FrameSkeletonHierachy> entry in SceneData.FrameResource.FrameSkeletonHierachies)
            {
                FrameResourceListBox.Items.Add(entry.Value);
            }
            foreach (KeyValuePair<int, object> entry in SceneData.FrameResource.FrameObjects)
            {
                FrameResourceListBox.Items.Add(entry.Value);
            }

            //TODO: Modify this when I implement scenes. I want to add REFIDs to them too.

            if (SceneData.FrameNameTable != null)
            {
                foreach (FrameNameTable.Data data in SceneData.FrameNameTable.FrameData)
                {
                    if (data.ParentName == null)
                        data.ParentName = "<scene>";

                    int index = treeView1.Nodes.IndexOfKey(data.ParentName);

                    if (index == -1)
                        treeView1.Nodes.Add(data.ParentName, data.ParentName);

                    index = treeView1.Nodes.IndexOfKey(data.ParentName);
                    TreeNode root = treeView1.Nodes[index];

                    if (data.FrameIndex != -1)
                    {
                        int total = SceneData.FrameResource.Header.SceneFolders.Length +
                            SceneData.FrameResource.Header.NumGeometries +
                            SceneData.FrameResource.Header.NumMaterialResources +
                            SceneData.FrameResource.Header.NumBlendInfos +
                            SceneData.FrameResource.Header.NumSkeletons +
                            SceneData.FrameResource.Header.NumSkelHierachies;

                        FrameObjectBase block = (SceneData.FrameResource.FrameObjects.ElementAt(data.FrameIndex).Value as FrameObjectBase);

                        block.FrameNameTableFlags = data.Flags;
                        block.IsOnFrameTable = true;

                        TreeNode node = CreateTreeNode(block);

                        if (node == null)
                            continue;

                        root.Nodes.Add(node);
                    }
                }
            }

            foreach (KeyValuePair<int, object> entry in SceneData.FrameResource.FrameObjects)
            {
                FrameObjectBase fObject = (FrameObjectBase)entry.Value;
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
                    nodes = treeView1.Nodes.Find(fObject.ParentIndex1.RefID.ToString(), true);
                }
                else if (fObject.ParentIndex1.Index != fObject.ParentIndex2.Index)
                {
                    //test, still trying to nail this fucker down.
                    nodes = treeView1.Nodes.Find(fObject.ParentIndex2.RefID.ToString(), true);
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
                TreeNode[] nodes = treeView1.Nodes.Find((obj.Tag as FrameObjectBase).ParentIndex1.RefID.ToString(), true);

                if (nodes.Length > 0)
                {
                    nodes[0].Nodes.Add(obj);
                    Debug.WriteLine("Added: " + obj.Text);
                }
                else if ((treeView1.Nodes.Find((obj.Tag as FrameObjectBase).RefID.ToString(), true).Length != 0))
                    Debug.WriteLine("Unadded node was found");
                else
                {
                    Debug.WriteLine(string.Format("WARNING: node: {0} was not added properly", obj.Text));
                    treeView1.Nodes.Add(obj);
                }
            }
            ToolkitSettings.UpdateRichPresence("Using the Frame Resource editor");
        }

        private TreeNode AddChildren(TreeNode node, FrameObjectBase fObject)
        {
            while (fObject.ParentIndex1.Index != -1)
            {
                fObject = (SceneData.FrameResource.FrameObjects[fObject.ParentIndex1.RefID] as FrameObjectBase);

                if (fObject.ParentIndex1.Index == fObject.ParentIndex2.Index)
                    return node;

                TreeNode child = CreateTreeNode(fObject);

                if (child == null)
                    return node;

                node.Nodes.Add(child);
            }
            return node;
        }

        private TreeNode CreateTreeNode(string NameText, object data)
        {
            TreeNode node = new TreeNode
            {
                Name = NameText,
                Text = NameText,
                Tag = data
            };

            return node;
        }
        private TreeNode CreateTreeNode(FrameObjectBase fObject)
        {
            TreeNode[] nodes2 = treeView1.Nodes.Find(fObject.RefID.ToString(), true);

            if (nodes2.Length > 0)
                return null;

            TreeNode node = ConvertNode(fObject.NodeData);

            if (fObject.GetType() == typeof(FrameObjectSingleMesh))
            {
                if (fObject.Refs.Count != 0)
                {
                    node.Nodes.Add(CreateTreeNode("Material", SceneData.FrameResource.FrameMaterials[fObject.Refs["Material"]]));
                    node.Nodes.Add(CreateTreeNode("Geometry", SceneData.FrameResource.FrameGeometries[fObject.Refs["Mesh"]]));
                }
                node.ContextMenuStrip = contextMenu;
            }
            else if (fObject.GetType() == typeof(FrameObjectModel))
            {
                node.Nodes.Add(CreateTreeNode("Material", SceneData.FrameResource.FrameMaterials[fObject.Refs["Material"]]));
                node.Nodes.Add(CreateTreeNode("Geometry", SceneData.FrameResource.FrameGeometries[fObject.Refs["Mesh"]]));
                node.Nodes.Add(CreateTreeNode("BlendInfo", SceneData.FrameResource.FrameBlendInfos[fObject.Refs["BlendInfo"]]));
                node.Nodes.Add(CreateTreeNode("Skeleton", SceneData.FrameResource.FrameSkeletons[fObject.Refs["Skeleton"]]));
                node.Nodes.Add(CreateTreeNode("SkeletonHierachy", SceneData.FrameResource.FrameSkeletonHierachies[fObject.Refs["SkeletonHierachy"]]));
                node.ContextMenuStrip = contextMenu;
            }

            node = AddChildren(node, fObject);
            return node;
        }
        private TreeNode ConvertNode(Node node)
        {
            TreeNode treeNode = new TreeNode()
            {
                Name = node.Name,
                Text = node.Text,
                Tag = node.Tag,
            };

            return treeNode;
        }

        //private Vector3 RetrieveParent1Position(FrameObjectSingleMesh mesh)
        //{
        //    Vector3 curPos;
        //    curPos = mesh.Matrix.Position;
        //    FrameObjectBase parent = (SceneData.FrameResource.EntireFrame[mesh.ParentIndex1.Index] as FrameObjectBase);

        //    while (parent != null)
        //    {
        //        if (parent.GetType() == typeof(FrameObjectFrame))
        //        {
        //            if ((parent as FrameObjectFrame).Item != null)
        //                curPos += (parent as FrameObjectFrame).Item.Position;
        //        }
        //        else
        //        {
        //            curPos += parent.Matrix.Position;
        //        }

        //        if (parent.ParentIndex1.Index != -1)
        //            parent = (SceneData.FrameResource.EntireFrame[parent.ParentIndex1.Index] as FrameObjectBase);
        //        else
        //            parent = null;
        //    }

        //    return curPos;
        //}

        private void OnSelectedChanged(object sender, EventArgs e)
        {
            FrameResourceGrid.SelectedObject = FrameResourceListBox.SelectedItem;
        }

        private void ExportModels(List<object> meshes)
        {
            string[] fileNames = new string[meshes.Count];
            Vector3[] filePos = new Vector3[meshes.Count];
            Matrix33[] rotPos = new Matrix33[meshes.Count];

            CustomEDD frameEDD = new CustomEDD();
            frameEDD.Entries = new List<CustomEDD.Entry>();

            Parallel.For(0, meshes.Count, i =>
            {
                CustomEDD.Entry entry = new CustomEDD.Entry();
                FrameObjectSingleMesh mesh = (meshes[i] as FrameObjectSingleMesh);
                FrameGeometry geom = SceneData.FrameResource.FrameGeometries[mesh.Refs["Mesh"]];
                FrameMaterial mat = SceneData.FrameResource.FrameMaterials[mesh.Refs["Material"]];
                IndexBuffer[] indexBuffers = new IndexBuffer[geom.LOD.Length];
                VertexBuffer[] vertexBuffers = new VertexBuffer[geom.LOD.Length];

                //we need to retrieve buffers first.
                for (int c = 0; c != geom.LOD.Length; c++)
                {
                    indexBuffers[c] = SceneData.IndexBufferPool.GetBuffer(geom.LOD[c].IndexBufferRef.uHash);
                    vertexBuffers[c] = SceneData.VertexBufferPool.GetBuffer(geom.LOD[c].VertexBufferRef.uHash);
                }

                Model newModel = new Model(mesh, indexBuffers, vertexBuffers, geom, mat);

                //if (mesh.ParentIndex1.Index != -1)
                //{
                //    FrameObjectBase parent = (SceneData.FrameResource.EntireFrame[mesh.ParentIndex1.Index] as FrameObjectBase);
                //    filePos[i] = RetrieveParent1Position(mesh);
                //}

                //if (((mesh.ParentIndex1.Index != -1)) && ((mesh.ParentIndex1.Index == mesh.ParentIndex2.Index)))
                //{
                //    FrameObjectFrame frame = SceneData.FrameResource.EntireFrame[mesh.ParentIndex1.Index] as FrameObjectFrame;
                //    if (frame.Item != null)
                //    {
                //        filePos[i] = frame.Item.Position;
                //    }
                //}

                entry.LodCount = newModel.Lods.Length;
                entry.LODNames = new string[entry.LodCount];

                for (int c = 0; c != newModel.Lods.Length; c++)
                {
                    newModel.ExportToM2T(ToolkitSettings.ExportPath + "\\");
                    switch (ToolkitSettings.Format)
                    {
                        case 0:
                            newModel.ExportToFbx(ToolkitSettings.ExportPath + "\\", false);
                            break;
                        case 1:
                            newModel.ExportToFbx(ToolkitSettings.ExportPath + "\\", true);
                            break;
                        case 2:
                            newModel.ExportToM2T(ToolkitSettings.ExportPath + "\\");
                            break;
                        default:
                            Log.WriteLine("Error! Unknown value set for ToolkitSettings.Format!", LoggingTypes.ERROR);
                            break;
                    }
                    Console.WriteLine(newModel.FrameMesh.Name.String);
                    if (newModel.FrameMesh.Name.String == "")
                        entry.LODNames[c] = newModel.FrameGeometry.LOD[c].VertexBufferRef.String;
                    else
                        entry.LODNames[c] = newModel.FrameMesh.Name.String;
                }
                entry.Position = mesh.Matrix.Position;
                entry.Rotation = mesh.Matrix.Rotation;

                frameEDD.Entries.Add(entry);

            });
            frameEDD.EntryCount = frameEDD.Entries.Count;
            using (BinaryWriter writer = new BinaryWriter(File.Create(ToolkitSettings.ExportPath + "\\" + "frame.edd")))
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
            treeView1.Visible = !treeView1.Visible;
            FrameResourceListBox.Visible = !FrameResourceListBox.Visible;
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
                model.FrameGeometry = SceneData.FrameResource.FrameGeometries[model.FrameMesh.Refs["Mesh"]];
                model.FrameMaterial = SceneData.FrameResource.FrameMaterials[model.FrameMesh.Refs["Material"]];

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

                if (m2tBrowser.ShowDialog() == DialogResult.Cancel)
                    return;

                if (m2tBrowser.FileName.ToLower().EndsWith(".m2t"))
                    model.ReadFromM2T(new BinaryReader(File.Open(m2tBrowser.FileName, FileMode.Open)));
                else if (m2tBrowser.FileName.ToLower().EndsWith(".fbx"))
                    model.ReadFromFbx(m2tBrowser.FileName);

                List<Vertex[]> vertData = new List<Vertex[]>();
                for (int i = 0; i != model.Lods.Length; i++)
                    vertData.Add(model.Lods[i].Vertices);

                model.FrameMesh.Boundings = new BoundingBox();
                model.FrameMesh.Boundings.CalculateBounds(vertData);
                model.FrameMaterial.Bounds = model.FrameMesh.Boundings;
                model.CalculateDecompression();
                model.BuildIndexBuffer();
                model.BuildVertexBuffer();
                model.UpdateObjectsFromModel();

                treeView1.SelectedNode.Tag = model.FrameMesh;
                SceneData.FrameResource.FrameGeometries[model.FrameMesh.Refs["Mesh"]] = model.FrameGeometry;
                SceneData.FrameResource.FrameMaterials[model.FrameMesh.Refs["Material"]] = model.FrameMaterial;
                SceneData.IndexBufferPool.BufferPools[iIndexes[0].PoolLocation].Buffers[indexRefs[0]] = model.IndexBuffers[0];
                SceneData.VertexBufferPool.BufferPools[iVertexes[0].PoolLocation].Buffers[vertexRefs[0]] = model.VertexBuffers[0];
            }
            else
            {
                MessageBox.Show("Click on a \"Single Mesh\" type of \"Model\" type in the tree view.", "Error");
            }
        }

        private void OnExit(object sender, FormClosingEventArgs e)
        {
            SaveChanges();
            SceneData.CleanData();
            Dispose();
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
                using (BinaryWriter writer = new BinaryWriter(File.Open(fileLocation.FullName, FileMode.Create)))
                {
                    SceneData.FrameResource.WriteToFile(writer);
                }
                using (BinaryWriter writer = new BinaryWriter(File.Open(SceneData.FrameNameTable.FileName, FileMode.Create)))
                {
                    FrameNameTable nameTable = new FrameNameTable();
                    nameTable.BuildDataFromResource(SceneData.FrameResource);
                    nameTable.AddNames();
                    nameTable.WriteToFile(writer);
                    SceneData.FrameNameTable = nameTable;
                }
                SceneData.IndexBufferPool.WriteToFile();
                SceneData.VertexBufferPool.WriteToFile();
                MessageBox.Show("Your saved file has been stored in the same folder as the executable.", "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ReloadClick(null, null);
            }
        }
        private void ReloadClick(object sender, EventArgs e)
        {
            SceneData.Reload();
            treeView1.Nodes.Clear();
            FrameResourceListBox.Items.Clear();
            PopulateForm();
        }

        private void ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (treeView1.SelectedNode == null)
                return;

            if (e.ClickedItem.Name == "contextExtract3D")
            {
                FrameObjectSingleMesh mesh = treeView1.SelectedNode.Tag as FrameObjectSingleMesh;

                FrameGeometry geom = SceneData.FrameResource.FrameGeometries[mesh.Refs["Mesh"]];
                FrameMaterial mat = SceneData.FrameResource.FrameMaterials[mesh.Refs["Material"]];
                IndexBuffer[] indexBuffers = new IndexBuffer[geom.LOD.Length];
                VertexBuffer[] vertexBuffers = new VertexBuffer[geom.LOD.Length];

                //we need to retrieve buffers first.
                for (int c = 0; c != geom.LOD.Length; c++)
                {
                    indexBuffers[c] = SceneData.IndexBufferPool.GetBuffer(geom.LOD[c].IndexBufferRef.uHash);
                    vertexBuffers[c] = SceneData.VertexBufferPool.GetBuffer(geom.LOD[c].VertexBufferRef.uHash);
                }

                Model newModel = new Model(mesh, indexBuffers, vertexBuffers, geom, mat);
                newModel.ExportToM2T(ToolkitSettings.ExportPath + "\\");
                switch (ToolkitSettings.Format)
                {
                    case 0:
                        newModel.ExportToFbx(ToolkitSettings.ExportPath + "\\", false);
                        break;
                    case 1:
                        newModel.ExportToFbx(ToolkitSettings.ExportPath + "\\", true);
                        break;
                    case 2:
                        newModel.ExportToM2T(ToolkitSettings.ExportPath + "\\");
                        break;
                    default:
                        Log.WriteLine("Error! Unknown value set for ToolkitSettings.Format!", LoggingTypes.ERROR);
                        break;
                }
            }
            else if (e.ClickedItem.Name == "contextUpdateParents")
            {
                FrameObjectBase obj = treeView1.SelectedNode.Tag as FrameObjectBase;
                ListWindow window = new ListWindow();
                window.PopulateForm(true);
                window.ShowDialog();
                if (window.type == -1)
                    return;

                int refID = (window.chosenObject as FrameEntry).RefID;
                obj.SubRef(FrameEntryRefTypes.Parent2);
                obj.AddRef(FrameEntryRefTypes.Parent2, refID);
            }
        }

        private void OnDelete(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null)
                return;

            SceneData.FrameResource.FrameObjects.Remove((treeView1.SelectedNode.Tag as FrameObjectBase).RefID);
            treeView1.SelectedNode.Remove();
        }

        private void OnSelect(object sender, TreeViewEventArgs e)
        {
            FrameResourceGrid.SelectedObject = treeView1.SelectedNode.Tag;
        }

        private void AddFrameSingleMesh_Click(object sender, EventArgs e)
        {
            bool createNewResource = false;
            DialogResult result = MessageBox.Show("Do you want to import a new model?", "Toolkit", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
                createNewResource = true;
            else if (result == DialogResult.Cancel)
                return;

            FrameObjectSingleMesh mesh = new FrameObjectSingleMesh();

            if (createNewResource)
            {
                Model model = new Model();
                model.FrameMesh = mesh;

                if (m2tBrowser.ShowDialog() == DialogResult.Cancel)
                    return;

                if (m2tBrowser.FileName.ToLower().EndsWith(".m2t"))
                    model.ReadFromM2T(new BinaryReader(File.Open(m2tBrowser.FileName, FileMode.Open)));
                else if (m2tBrowser.FileName.ToLower().EndsWith(".fbx"))
                    model.ReadFromFbx(m2tBrowser.FileName);

                mesh.Name.Set(model.Name);
                model.CreateObjectsFromModel();
                mesh.AddRef(FrameEntryRefTypes.Mesh, model.FrameGeometry.RefID);
                mesh.AddRef(FrameEntryRefTypes.Material, model.FrameMaterial.RefID);
                SceneData.FrameResource.FrameMaterials.Add(model.FrameMaterial.RefID, model.FrameMaterial);
                SceneData.FrameResource.FrameGeometries.Add(model.FrameGeometry.RefID, model.FrameGeometry);
                FrameResourceListBox.Items.Add(model.FrameMaterial);
                FrameResourceListBox.Items.Add(model.FrameGeometry);

                //Check for existing buffer; if it exists, remove so we can add one later.
                if (SceneData.IndexBufferPool.SearchBuffer(model.IndexBuffers[0].Hash) != null)
                    SceneData.IndexBufferPool.RemoveBuffer(model.IndexBuffers[0]);

                //do the same for vertexbuffer pools.
                if (SceneData.VertexBufferPool.SearchBuffer(model.VertexBuffers[0].Hash) != null)
                    SceneData.VertexBufferPool.RemoveBuffer(model.VertexBuffers[0]);

                SceneData.IndexBufferPool.AddBuffer(model.IndexBuffers[0]);
                SceneData.VertexBufferPool.AddBuffer(model.VertexBuffers[0]);

                mesh.UpdateNode();
                treeView1.Nodes.Add(CreateTreeNode(mesh));
            }
            else
            {
                ListWindow window = new ListWindow();
                window.PopulateForm();
                window.ShowDialog();
                if (window.type == -1)
                    return;

                FrameObjectSingleMesh copy = window.chosenObject as FrameObjectSingleMesh;
                mesh = new FrameObjectSingleMesh();
                mesh.Name.Set("domek2");
                mesh.Boundings = copy.Boundings;
                mesh.ParentIndex1 = copy.ParentIndex1;
                mesh.ParentIndex2 = copy.ParentIndex2;
                mesh.Matrix = copy.Matrix;
                mesh.Flags = copy.Flags;
                mesh.FrameNameTableFlags = copy.FrameNameTableFlags;
                mesh.IsOnFrameTable = copy.IsOnFrameTable;
            }

            SceneData.FrameResource.FrameObjects.Add(mesh.RefID, mesh);
            FrameResourceListBox.Items.Add(mesh);
            //SaveChanges();
        }

        private void OnExportFarLods(object sender, EventArgs e)
        {
            List<object> meshes = new List<object>();

            for (int i = 0; i != SceneData.FrameResource.FrameObjects.Count; i++)
            {
                object fObject = SceneData.FrameResource.FrameObjects.ElementAt(i).Value;

                if ((fObject as FrameObjectBase).IsOnFrameTable && (fObject as FrameObjectBase).FrameNameTableFlags != 0)
                {
                    if (fObject.GetType() == typeof(FrameObjectSingleMesh) || fObject.GetType() == typeof(FrameObjectModel))
                    {
                        meshes.Add(fObject);
                    }
                }
            }
            ExportModels(meshes);
        }

        private void OnExportModels(object sender, EventArgs e)
        {
            List<object> meshes = new List<object>();

            for (int i = 0; i != SceneData.FrameResource.FrameObjects.Count; i++)
            {
                object fObject = SceneData.FrameResource.FrameObjects.ElementAt(i).Value;

                if ((fObject as FrameObjectBase).IsOnFrameTable && (fObject as FrameObjectBase).FrameNameTableFlags == 0)
                {
                    if (fObject.GetType() == typeof(FrameObjectSingleMesh) || fObject.GetType() == typeof(FrameObjectModel))
                    {
                        meshes.Add(fObject);
                    }
                }
            }
            ExportModels(meshes);
        }

        private void OnExportAll(object sender, EventArgs e)
        {
            List<object> meshes = new List<object>();

            for (int i = 0; i != SceneData.FrameResource.FrameObjects.Count; i++)
            {
                object fObject = SceneData.FrameResource.FrameObjects.ElementAt(i).Value;

                if (fObject.GetType() == typeof(FrameObjectSingleMesh) || fObject.GetType() == typeof(FrameObjectModel))
                {
                    meshes.Add(fObject);
                }
            }
            ExportModels(meshes);
        }
    }
}