﻿using Rendering.Graphics;
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

            KeyPreview = true;
            RenderPanel.Focus();
            //do D3D stuff/
            StartD3DPanel();
        }

        public void PopulateList(FileInfo info)
        {
            SceneData.FrameResource.BuildFrameTree(SceneData.FrameNameTable);
            TreeNode tree = new TreeNode(info.Name);
            SceneData.FrameResource.Frame.ConvertToTreeNode(ref tree);
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
            AddToLog("Toggled Cull Mode");
        }

        private void FillModeButton_Click(object sender, EventArgs e)
        {
            Graphics.ToggleD3DFillMode();
            AddToLog("Toggled Fill Mode");
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

                if (Input.IsKeyDown(Keys.A))
                    Graphics.Camera.Position.X += 1f;

                if (Input.IsKeyDown(Keys.D))
                    Graphics.Camera.Position.X -= 1f;

                if (Input.IsKeyDown(Keys.W))
                    Graphics.Camera.Position.Y += 1f;

                if (Input.IsKeyDown(Keys.S))
                    Graphics.Camera.Position.Y -= 1f;

                if (Input.IsKeyDown(Keys.Q))
                    Graphics.Camera.Position.Z += 1f;

                if (Input.IsKeyDown(Keys.E))
                    Graphics.Camera.Position.Z -= 1f;
            }
            lastMousePos = mousePos;
            Graphics.Timer.Frame2();

            FrameObjectBase obj1;
            FrameObjectBase obj2;

            foreach (KeyValuePair<int, FrameNode> child in SceneData.FrameResource.Frame.Children)
            {
                obj1 = (child.Value.Object as FrameObjectBase);

                if(obj1 != null && Graphics.Models.ContainsKey(obj1.RefID))
                    Graphics.Models[obj1.RefID].SetTransform(obj1.Matrix.Position, obj1.Matrix.Rotation);

                foreach (KeyValuePair<int, FrameNode> child2 in child.Value.Children)
                {
                    obj2 = (child2.Value.Object as FrameObjectBase);

                    if (Graphics.Models.ContainsKey(obj2.RefID))
                    {
                        TransformMatrix matrix = ((obj1 != null) ? obj1.Matrix : new TransformMatrix());
                        Graphics.Models[obj2.RefID].SetTransform(matrix.Position + obj2.Matrix.Position, obj2.Matrix.Rotation);
                    }
                }
            }

            Graphics.Frame();

            //awful i know
            if (Graphics.Timer.FrameTime < 1000 / 60)
            {
                Thread.Sleep((int)Math.Abs(Graphics.Timer.FrameTime - 1000 / 60));
            }

            return true;
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
                AddToLog("Saved Changes Succesfully!");
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
            AddToLog(string.Format("New Current Entry", fObject.Name.String));
        }

        private void EntryApplyChanges_OnClick(object sender, EventArgs e)
        {
            FrameObjectBase fObject = (treeView1.SelectedNode.Tag as FrameObjectBase);
            fObject.Matrix.Position = new Vector3(float.Parse(PositionXBox.Text), float.Parse(PositionYBox.Text), float.Parse(PositionZBox.Text));
            fObject.Matrix.Rotation.EulerRotation = new Vector3(float.Parse(RotationXBox.Text), float.Parse(RotationYBox.Text), float.Parse(RotationZBox.Text));
            fObject.Matrix.Rotation.UpdateMatrixFromEuler();
            fObject.IsOnFrameTable = OnFrameNameTable.Checked;
            fObject.FrameNameTableFlags = (NameTableFlags)FrameNameTableFlags.GetCurrentValue();
            Graphics.Models[fObject.RefID].SetTransform(fObject.Matrix.Position, fObject.Matrix.Rotation);
            Graphics.BuildSelectedEntry(fObject);
            AddToLog(string.Format("Modified Currently Entry", fObject.Name.String));
        }

        private void Pick(int sx, int sy)
        {
            var ray = Graphics.Camera.GetPickingRay(new Vector2(sx, sy), new Vector2(ToolkitSettings.Width, ToolkitSettings.Height));
            FrameObjectSingleMesh selected = null;

            foreach (KeyValuePair<int, RenderModel> model in Graphics.Models)
            {
                // transform the picking ray into the object space of the mesh

                Matrix worldMat = model.Value.Transform;
                var invWorld = Matrix.Invert(worldMat);
                ray.Direction = Vector3.TransformNormal(ray.Direction, invWorld);
                //ray.Position = Vector3.TransformCoordinate(ray.Position, invWorld);
                ray.Direction.Normalize();

                float tmin0;
                float tmin1;


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

                if (!ray.Intersects(ref tempBox0, out tmin)) continue;
                Console.WriteLine("intersect with " + objBase.Name.String);
                float maxT = float.MaxValue;
                for (var i = 0; i < model.Value.LODs[0].Indices.Length / 3; i++)
                {
                    var v0 = model.Value.Transform.M41 + model.Value.LODs[0].Vertices[model.Value.LODs[0].Indices[i * 3]].Position;
                    var v1 = model.Value.Transform.M42 + model.Value.LODs[0].Vertices[model.Value.LODs[0].Indices[i * 3 + 1]].Position;
                    var v2 = model.Value.Transform.M43 + model.Value.LODs[0].Vertices[model.Value.LODs[0].Indices[i * 3 + 2]].Position;
                    float t = 0;
                    if (!ray.Intersects(ref v0, ref v1, ref v2, out t)) continue;
                    if (!(t < tmin || t < 0)) continue;
                    maxT = t;
                }

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

        public void AddToLog(string message)
        {
            //richTextBox1.AppendText(message + "\n");
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
    }
}