using Rendering.Graphics;
using Rendering.Input;
using SharpDX.Windows;
using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using Mafia2;
using SharpDX;
using System.Threading;
using ResourceTypes.FrameNameTable;
using ResourceTypes.FrameResource;
using Utils.Settings;
using ResourceTypes.BufferPools;
using Utils.Types;

namespace Mafia2Tool
{
    public partial class D3DForm : Form
    {
        private InputClass Input { get; set; }
        private GraphicsClass Graphics { get; set; }

        private Point mousePos;
        private Point lastMousePos;
        private FileInfo fileLocation;

        public D3DForm(FileInfo info)
        {
            InitializeComponent();
            SceneData.ScenePath = info.DirectoryName;
            fileLocation = info;
            SceneData.BuildData();
            PopulateList(info);
            TEMPCameraSpeed.Text = ToolkitSettings.CameraSpeed.ToString();
            KeyPreview = true;
            RenderPanel.Focus();
            StartD3DPanel();
        }

        public void PopulateList(FileInfo info)
        {
            TreeNode tree = SceneData.FrameResource.BuildTree(SceneData.FrameNameTable);
            treeView1.Nodes.Add(tree);
        }

        public void StartD3DPanel()
        {
            Init(RenderPanel.Handle);
            Run();
        }

        public bool Init(IntPtr handle)
        {
            bool result = false;

            if (Input == null)
            {
                Input = new InputClass();
                Input.Init();
            }

            if (Graphics == null)
            {
                Graphics = new GraphicsClass();
                Graphics.PreInit(handle);
                BuildRenderObjects();
                result = Graphics.InitScene();
            }
            return result;
        }

        public void Run()
        {
            KeyDown += (s, e) => Input.KeyDown(e.KeyCode);
            KeyUp += (s, e) => Input.KeyUp(e.KeyCode);
            RenderPanel.MouseDown += (s, e) => Input.ButtonDown(e.Button);
            RenderPanel.MouseUp += (s, e) => Input.ButtonUp(e.Button);
            RenderPanel.MouseMove += RenderForm_MouseMove;
            RenderPanel.MouseEnter += RenderPanel_MouseEnter;
            RenderLoop.Run(this, () => { if (!Frame()) Shutdown(); });
        }

        private void RenderPanel_MouseEnter(object sender, EventArgs e)
        {
            RenderPanel.Focus();
        }

        private void RenderForm_MouseMove(object sender, MouseEventArgs e)
        {
            mousePos = new Point(e.Location.X, e.Location.Y);
        }

        private void CullModeButton_Click(object sender, EventArgs e)
        {
            Graphics.ToggleD3DCullMode();
        }

        private void FillModeButton_Click(object sender, EventArgs e)
        {
            Graphics.ToggleD3DFillMode();
        }

        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            TreeViewUpdateSelected();
        }

        private void OnAfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeViewUpdateSelected();
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            RenderStorageSingleton.Instance.TextureCache.Clear();
            Shutdown();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveChanges();
        }

        public bool Frame()
        {
            if (RenderPanel.Focused)
            {
                if (Input.IsButtonDown(MouseButtons.Right))
                {
                    var dx = 0.25f * (mousePos.X - lastMousePos.X);
                    var dy = 0.25f * (mousePos.Y - lastMousePos.Y);

                    Graphics.Camera.Pitch(dy);
                    Graphics.Camera.Yaw(dx);
                }
                else if (Input.IsButtonDown(MouseButtons.Left))
                {
                    //broken. Lots of refactoring of the old code to get this working.
                    Pick(mousePos.X, mousePos.Y);
                }

                float speed = /*Graphics.Timer.FrameTime * */ToolkitSettings.CameraSpeed;

                if (Input.IsKeyDown(Keys.A))
                    Graphics.Camera.Position.X += speed;

                if (Input.IsKeyDown(Keys.D))
                    Graphics.Camera.Position.X -= speed;

                if (Input.IsKeyDown(Keys.W))
                    Graphics.Camera.Position.Y += speed;

                if (Input.IsKeyDown(Keys.S))
                    Graphics.Camera.Position.Y -= speed;

                if (Input.IsKeyDown(Keys.Q))
                    Graphics.Camera.Position.Z += speed;

                if (Input.IsKeyDown(Keys.E))
                    Graphics.Camera.Position.Z -= speed;
            }
            lastMousePos = mousePos;
            Graphics.Timer.Frame2();
            Graphics.Frame();

            //awful i know
            if (Graphics.Timer.FrameTime < 1000 / 60)
            {
                Thread.Sleep((int)Math.Abs(Graphics.Timer.FrameTime - 1000 / 60));
            }
            return true;
        }

        private void UpdateChildRenderNodes(TreeNode node, TransformMatrix matrix)
        {
            FrameObjectBase obj2 = (node.Tag as FrameObjectBase);

            if (obj2 != null)
                UpdateRenderedObjects(matrix, obj2);

            foreach (TreeNode cNode in node.Nodes)
            {
                matrix = ((obj2 != null) ? obj2.Matrix : new TransformMatrix());
                UpdateChildRenderNodes(cNode, matrix);
            }
        }

        private void UpdateRenderedObjects(TransformMatrix obj1Matrix, FrameObjectBase obj)
        {
            if(Graphics.Dummies.ContainsKey(obj.RefID) && obj.GetType() == typeof(FrameObjectDummy))
            {
                Graphics.Dummies[obj.RefID].SetTransform(obj1Matrix.Position + obj.Matrix.Position, obj.Matrix.Rotation);
            }
            else if (Graphics.Models.ContainsKey(obj.RefID) && obj.GetType() == typeof(FrameObjectSingleMesh) || obj.GetType() == typeof(FrameObjectModel))
            {
                Graphics.Models[obj.RefID].SetTransform(obj1Matrix.Position + obj.Matrix.Position, obj.Matrix.Rotation);
            }
            else if (Graphics.Areas.ContainsKey(obj.RefID) && obj.GetType() == typeof(FrameObjectArea))
            {
                Graphics.Areas[obj.RefID].SetTransform(obj1Matrix.Position + obj.Matrix.Position, obj.Matrix.Rotation);
            }
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
                    nameTable.WriteToFile(writer);
                    SceneData.FrameNameTable = nameTable;
                }
                SceneData.IndexBufferPool.WriteToFile();
                SceneData.VertexBufferPool.WriteToFile();
                Console.WriteLine("Saved Changes Succesfully");
            }
        }

        private void BuildRenderObjects()
        {
            Dictionary<int, RenderModel> meshes = new Dictionary<int, RenderModel>();
            Dictionary<int, RenderBoundingBox> areas = new Dictionary<int, RenderBoundingBox>();
            Dictionary<int, RenderBoundingBox> dummies = new Dictionary<int, RenderBoundingBox>();

            for (int i = 0; i != SceneData.FrameResource.FrameObjects.Count; i++)
            {
                FrameEntry fObject = (SceneData.FrameResource.FrameObjects.ElementAt(i).Value as FrameEntry);

                if (fObject.GetType() == typeof(FrameObjectSingleMesh) || fObject.GetType() == typeof(FrameObjectModel))
                {
                    FrameObjectSingleMesh mesh = (fObject as FrameObjectSingleMesh);

                    if (mesh.MaterialIndex == -1 && mesh.MeshIndex == -1)
                        continue;

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

                    RenderModel model = new RenderModel();
                    model.ConvertFrameToRenderModel(mesh, geom, mat, indexBuffers, vertexBuffers);
                    meshes.Add(fObject.RefID, model);
                }

                if (fObject.GetType() == typeof(FrameObjectArea))
                {
                    FrameObjectArea area = (fObject as FrameObjectArea);
                    RenderBoundingBox areaBBox = new RenderBoundingBox();
                    areaBBox.SetTransform(area.Matrix.Position, area.Matrix.Rotation);
                    areaBBox.Init(area.Bounds);
                    areas.Add(fObject.RefID, areaBBox);
                }

                if (fObject.GetType() == typeof(FrameObjectDummy))
                {
                    FrameObjectDummy dummy = (fObject as FrameObjectDummy);
                    RenderBoundingBox dummyBBox = new RenderBoundingBox();
                    dummyBBox.SetTransform(dummy.Matrix.Position, dummy.Matrix.Rotation);
                    dummyBBox.Init(dummy.Bounds);
                    dummies.Add(fObject.RefID, dummyBBox);

                }
            }
            Graphics.Models = meshes;
            Graphics.Areas = areas;
            Graphics.Dummies = dummies;
        }

        private void TreeViewUpdateSelected()
        {
            if (treeView1.SelectedNode.Tag == null || treeView1.SelectedNode.Tag.GetType() == typeof(FrameHeaderScene))
                return;

            UpdateCurrentEntryData(treeView1.SelectedNode.Tag as FrameObjectBase);
        }

        //Improvement Idea: Sync updates values IF selected indexes is valid.
        private void UpdateCurrentEntryData(FrameObjectBase fObject)
        {
            CurrentEntry.Text = fObject.Name.String;
            PositionXBox.Text = fObject.Matrix.Position.X.ToString();
            PositionYBox.Text = fObject.Matrix.Position.Y.ToString();
            PositionZBox.Text = fObject.Matrix.Position.Z.ToString();
            RotationXBox.Text = fObject.Matrix.Rotation.EulerRotation.X.ToString();
            RotationYBox.Text = fObject.Matrix.Rotation.EulerRotation.Y.ToString();
            RotationZBox.Text = fObject.Matrix.Rotation.EulerRotation.Z.ToString();
            CurrentEntryType.Text = fObject.GetType().Name;
            Graphics.BuildSelectedEntry(fObject);
            DebugPropertyGrid.SelectedObject = fObject;
            OnFrameNameTable.Checked = fObject.IsOnFrameTable;
            FrameNameTableFlags.EnumValue = (Enum)Convert.ChangeType(fObject.FrameNameTableFlags, typeof(NameTableFlags));
        }

        private void EntryApplyChanges_OnClick(object sender, EventArgs e)
        {
            FrameObjectBase fObject = (treeView1.SelectedNode.Tag as FrameObjectBase);
            fObject.Matrix.Position = new Vector3(float.Parse(PositionXBox.Text), float.Parse(PositionYBox.Text), float.Parse(PositionZBox.Text));
            fObject.Matrix.Rotation.EulerRotation = new Vector3(float.Parse(RotationXBox.Text), float.Parse(RotationYBox.Text), float.Parse(RotationZBox.Text));
            fObject.Matrix.Rotation.UpdateMatrixFromEuler();
            fObject.IsOnFrameTable = OnFrameNameTable.Checked;
            fObject.FrameNameTableFlags = (NameTableFlags)FrameNameTableFlags.GetCurrentValue();
            Graphics.BuildSelectedEntry(fObject);

            FrameObjectBase obj1;

            foreach (TreeNode node in treeView1.Nodes)
            {
                obj1 = (node.Tag as FrameObjectBase);
                TransformMatrix matrix = ((obj1 != null) ? obj1.Matrix : new TransformMatrix());

                if (obj1 != null)
                    UpdateRenderedObjects(matrix, obj1);

                foreach (TreeNode cNode in node.Nodes)
                {
                    UpdateChildRenderNodes(cNode, matrix);
                }
            }
        }

        private void Pick(int sx, int sy)
        {
            var ray = Graphics.Camera.GetPickingRay(new Vector2(sx, sy), new Vector2(ToolkitSettings.Width, ToolkitSettings.Height));
            FrameObjectSingleMesh selected = null;
            float seltMin = float.MaxValue;

            foreach (KeyValuePair<int, RenderModel> model in Graphics.Models)
            {
                Matrix worldMat = model.Value.Transform;
                var invWorld = Matrix.Invert(worldMat);
                //ray.Direction = Vector3.TransformNormal(ray.Direction, invWorld);
                //ray.Position = Vector3.TransformCoordinate(ray.Position, invWorld);
                //ray.Direction.Normalize();

                FrameObjectSingleMesh objBase = null;
                foreach (KeyValuePair<int, object> obj in SceneData.FrameResource.FrameObjects)
                {
                    if ((obj.Value as FrameObjectBase).RefID == model.Key)
                        objBase = (obj.Value as FrameObjectSingleMesh);
                }

                if (objBase == null)
                    continue;

                Vector3 minVector = new Vector3(
                model.Value.Transform.M41 + model.Value.BoundingBox.Boundings.Minimum.X,
                model.Value.Transform.M42 + model.Value.BoundingBox.Boundings.Minimum.Y,
                model.Value.Transform.M43 + model.Value.BoundingBox.Boundings.Minimum.Z
                );
                Vector3 maxVector = new Vector3(
                   model.Value.Transform.M41 + model.Value.BoundingBox.Boundings.Maximum.X,
                   model.Value.Transform.M42 + model.Value.BoundingBox.Boundings.Maximum.Y,
                   model.Value.Transform.M43 + model.Value.BoundingBox.Boundings.Maximum.Z
                   );
               BoundingBox tempBox0 = new BoundingBox(minVector, maxVector);
                BoundingBox tempBox1 = objBase.Boundings;
                float tmin;

                if (!ray.Intersects(ref tempBox0, out tmin))
                    continue;

                Console.WriteLine("intersect with {0} {1}", objBase.Name.String, tmin);

                if (tmin < seltMin)
                {
                    selected = objBase;
                    seltMin = tmin;
                }

                //float maxT = float.MaxValue;
                //for (var i = 0; i < model.Value.LODs[0].Indices.Length / 3; i++)
                //{
                //    var v0 = model.Value.Transform.M41 + model.Value.LODs[0].Vertices[model.Value.LODs[0].Indices[i * 3]].Position;
                //    var v1 = model.Value.Transform.M42 + model.Value.LODs[0].Vertices[model.Value.LODs[0].Indices[i * 3 + 1]].Position;
                //    var v2 = model.Value.Transform.M43 + model.Value.LODs[0].Vertices[model.Value.LODs[0].Indices[i * 3 + 2]].Position;
                //    float t = 0;
                //    if (!ray.Intersects(ref v0, ref v1, ref v2, out t)) continue;
                //    if (!(t < tmin || t < 0)) continue;
                //    maxT = t;
                //}

                //float curTmin = float.MaxValue;
                //if (ray.Position.X > tempBox0.Minimum.X && ray.Position.X < tempBox0.Maximum.X)
                //{
                //    if (ray.Position.Y > tempBox0.Minimum.Y && ray.Position.Y < tempBox0.Maximum.Y)
                //    {
                //        if (ray.Position.Z > tempBox0.Minimum.Z && ray.Position.Z < tempBox0.Maximum.Z)
                //        {
                            
                //            for (var i = 0; i < model.Value.LODs[0].Indices.Length / 3; i++)
                //            {
                //                float tmin2 = float.MaxValue/2;
                //                var v0 = model.Value.Transform.M41 + model.Value.LODs[0].Vertices[model.Value.LODs[0].Indices[i * 3]].Position;
                //                var v1 = model.Value.Transform.M42 + model.Value.LODs[0].Vertices[model.Value.LODs[0].Indices[i * 3 + 1]].Position;
                //                var v2 = model.Value.Transform.M43 + model.Value.LODs[0].Vertices[model.Value.LODs[0].Indices[i * 3 + 2]].Position;
                //                float t = 0;
                //                if (!ray.Intersects(ref v0, ref v1, ref v2, out t)) continue;
                //                // find the closest intersection, exclude intersections behind camera
                //                if (!(t < tmin2 || t < 0)) continue;
                //                tmin2 = t;
                //                if (curTmin < tmin2)
                //                {
                //                    selected = objBase;
                //                }
                //            }
                //        }
                //    }
                //}
                //ray.Intersects(ref tempBox0, out tmin0);
                //ray.Intersects(ref tempBox1, out tmin1);
                //Console.WriteLine(tmin0 + " " + tmin1);
                //continue;

                //Console.WriteLine("Intersection!, " + objBase.Name.String);
                //float tmin2 = float.MaxValue;
                //for (var i = 0; i < model.Value.LODs[0].Indices.Length / 3; i++)
                //{
                //    var v0 = model.Value.Transform.M41 + model.Value.LODs[0].Vertices[model.Value.LODs[0].Indices[i * 3]].Position;
                //    var v1 = model.Value.Transform.M42 + model.Value.LODs[0].Vertices[model.Value.LODs[0].Indices[i * 3 + 1]].Position;
                //    var v2 = model.Value.Transform.M43 + model.Value.LODs[0].Vertices[model.Value.LODs[0].Indices[i * 3 + 2]].Position;
                //    float t = 0;
                //    if (!ray.Intersects(ref v0, ref v1, ref v2, out t)) continue;
                //    // find the closest intersection, exclude intersections behind camera
                //    if (!(t < tmin2 || t < 0)) continue;
                //    tmin2 = t;
                //}
                //if (tmin < tmin2)
                //{
                //    selected = objBase;
                //}
            }

            if (selected != null)
            {
                Graphics.BuildSelectedEntry(selected);
                Console.WriteLine(selected.Name.String);
                UpdateCurrentEntryData(selected);
            }
        }

        public void Shutdown()
        {
            Graphics?.Shutdown();
            Graphics = null;
            Input = null;
            RenderStorageSingleton.Instance.Shutdown();
        }

        private void PreviewButton_Click(object sender, EventArgs e)
        {
            //FrameObjectBase obj = (treeView1.SelectedNode.Tag as FrameObjectBase);
            //RenderModelView viewer = new RenderModelView(obj.RefID, Graphics.Models[obj.RefID]);
        }

        private void OnPropertyChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (e.ChangedItem.Label == "RefID")
            {
                TreeNode[] nodes = treeView1.Nodes.Find(e.ChangedItem.Value.ToString(), true);

                if (nodes.Length > 0)
                {
                    int newValue = (int)e.ChangedItem.Value;
                    FrameObjectBase obj = (treeView1.SelectedNode.Tag as FrameObjectBase);
                    int newIndex = 0;

                    for(int i = 0; i != SceneData.FrameResource.FrameObjects.Count; i++)
                    {
                        FrameObjectBase frameObj = (SceneData.FrameResource.FrameObjects[i] as FrameObjectBase);

                        if (frameObj.RefID == newValue)
                            newIndex = i;
                    }

                    if (newIndex == -1)
                    {
                        for (int i = 0; i != SceneData.FrameResource.FrameScenes.Count; i++)
                        {
                            FrameEntry frameObj = (SceneData.FrameResource.FrameObjects[i] as FrameEntry);

                            if (frameObj.RefID == newValue)
                                newIndex = i;
                        }
                    }

                    string name = (SceneData.FrameResource.FrameObjects.ElementAt(newIndex).Value as FrameObjectBase).Name.String;

                    //because C# doesn't allow me to get this data for some odd reason, im going to check for it in obj. Why does C# not allow me to see FullLabel in the e var?      
                    if (obj.ParentIndex1.RefID == newValue)
                    {
                        obj.ParentIndex1.Index = newIndex;
                        obj.ParentIndex1.Name = name;
                        obj.SubRef(FrameEntryRefTypes.Parent1);
                        obj.AddRef(FrameEntryRefTypes.Parent1, newValue);
                    }
                    else if (obj.ParentIndex2.RefID == newValue)
                    {
                        obj.ParentIndex2.Index = newIndex;
                        obj.ParentIndex2.Name = name;
                        obj.SubRef(FrameEntryRefTypes.Parent2);
                        obj.AddRef(FrameEntryRefTypes.Parent2, newValue);
                    }
                    obj.UpdateNode();
                    treeView1.Nodes.Remove(treeView1.SelectedNode);
                    TreeNode newNode = new TreeNode(obj.ToString());
                    newNode.Tag = obj;
                    newNode.Name = obj.RefID.ToString();
                    nodes[0].Nodes.Add(newNode);
                    treeView1.SelectedNode = newNode;
                }
            }
        }

        private void CameraSpeedUpdate(object sender, EventArgs e)
        {
            float.TryParse(TEMPCameraSpeed.Text, out ToolkitSettings.CameraSpeed);

            if (ToolkitSettings.CameraSpeed == 0.0f)
            {
                ToolkitSettings.CameraSpeed = 0.1f;
                TEMPCameraSpeed.Text = ToolkitSettings.CameraSpeed.ToString();
            }

            ToolkitSettings.WriteKey("CameraSpeed", "ModelViewer", ToolkitSettings.CameraSpeed.ToString());
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            TreeNode node = treeView1.SelectedNode;

            if (node.Nodes.Count > 0)
            {
                MessageBox.Show("Cannot delete a node with children!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                FrameEntry obj = node.Tag as FrameEntry;

                if (obj != null)
                {
                    treeView1.Nodes.Remove(node);
                    Graphics.Models.Remove(obj.RefID);
                    SceneData.FrameResource.FrameObjects.Remove(obj.RefID);

                    if (obj.GetType() == typeof(FrameObjectSingleMesh) || obj.GetType() == typeof(FrameObjectModel))
                        Graphics.Models.Remove(obj.RefID);
                    else if (obj.GetType() == typeof(FrameObjectArea))
                        Graphics.Areas.Remove(obj.RefID);
                    else if (obj.GetType() == typeof(FrameObjectDummy))
                        Graphics.Dummies.Remove(obj.RefID);
                }
            }

        }

        private void OpenEntryContext(object sender, System.ComponentModel.CancelEventArgs e)
        {
            EntryMenuStrip.Items[0].Visible = false;
            EntryMenuStrip.Items[1].Visible = false;

            if (treeView1.SelectedNode == null)
                e.Cancel = true;

            if(!e.Cancel)
            {
                EntryMenuStrip.Items[0].Visible = true;
                EntryMenuStrip.Items[1].Visible = true;
            }
        }
    }
}
