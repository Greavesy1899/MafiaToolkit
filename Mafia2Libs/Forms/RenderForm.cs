using ModelViewer.Programming.GraphicClasses;
using ModelViewer.Programming.InputClasses;
using ModelViewer.Programming.SystemClasses;
using SharpDX.Windows;
using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using Mafia2;
using System.Diagnostics;
using SharpDX;

namespace Mafia2Tool
{
    public partial class D3DForm : Form
    {
        private SystemConfigClass Config { get; set; }
        private InputClass Input { get; set; }
        private GraphicsClass Graphics { get; set; }
        private TimerClass Timer { get; set; }

        private SharpDX.Point mousePos;
        private SharpDX.Point lastMousePos;
        private FileInfo fileLocation;
        private FrameObjectBase entryObj;

        public D3DForm(FileInfo info)
        {
            InitializeComponent();
            SceneData.ScenePath = info.DirectoryName;
            fileLocation = info;
            SceneData.BuildData();
            PopulateList();

            KeyPreview = true;
            RenderPanel.Focus();

            //do D3D stuff/
            StartD3DPanel();
        }

        public void PopulateList()
        {
            //foreach (KeyValuePair<int, object> entry in SceneData.FrameResource.FrameObjects)
            //    treeView1.Items.Add(entry.Value.ToString());
        }

        public void StartD3DPanel()
        {
            Init("Model Viewer", 1920, 1080, true, "F:/MafiaII Exported Models/11_G_07.m2t", this.RenderPanel.Handle, false, 0);
            Run();
        }

        public bool Init(string title, int width, int height, bool Vsync, string meshName, IntPtr handle, bool fullscreen, int testTimeSeconds)
        {
            bool result = false;

            if (Config == null)
            {
                Config = new SystemConfigClass(title, width, height, fullscreen, Vsync);
            }

            if (Input == null)
            {
                Input = new InputClass();
                Input.Init();
            }

            if (Graphics == null)
            {
                Graphics = new GraphicsClass();
                Stopwatch watch = new Stopwatch();
                watch.Start();
                BuildRenderObjects();
                watch.Stop();
                Console.WriteLine("It took {0} to build objects", watch.Elapsed);
                result = Graphics.Init(Config, handle, meshName);
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

        private void BuildRenderObjects()
        {
            Dictionary<int, RenderModel> meshes = new Dictionary<int, RenderModel>();

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

                    Model newModel = new Model(mesh, indexBuffers, vertexBuffers, geom, mat);
                    RenderModel model = new RenderModel();
                    model.BoundingBox.Init(mesh.Boundings);
                    model.ConvertM2ModelToRenderModel(newModel.ModelStructure);
                    model.DoRender = true;
                    model.SetTransform(mesh.Matrix.Position.X, mesh.Matrix.Position.Y, mesh.Matrix.Position.Z, mesh.Matrix.Rotation);
                    meshes.Add(fObject.RefID, model);
                }
            }
            Graphics.Models = meshes;
        }

        private void RenderForm_MouseMove(object sender, MouseEventArgs e)
        {
            mousePos = new Point(e.Location.X, e.Location.Y);
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
                else if(Input.IsButtonDown(MouseButtons.Left))
                {
                    //broken. Lots of refactoring of the old code to get this working.
                    //Pick(mousePos.X, mousePos.Y);
                }

                if (Input.IsKeyDown(Keys.A))
                    Graphics.Camera.Position.X += 5f;

                if (Input.IsKeyDown(Keys.D))
                    Graphics.Camera.Position.X -= 5f;

                if (Input.IsKeyDown(Keys.W))
                    Graphics.Camera.Position.Y += 5f;

                if (Input.IsKeyDown(Keys.S))
                    Graphics.Camera.Position.Y -= 5f;

                if (Input.IsKeyDown(Keys.Q))
                    Graphics.Camera.Position.Z += 5f;

                if (Input.IsKeyDown(Keys.E))
                    Graphics.Camera.Position.Z -= 5f;
            }
            lastMousePos = mousePos;
            Graphics.Timer.Frame2();
            return Graphics.Frame();
        }
        public void Shutdown()
        {
            Timer = null;
            Graphics?.Shutdown();
            Graphics = null;
            Input = null;
            Config = null;
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
            UpdateCurrentEntryData();
        }

        //Improvement Idea: Sync updates values IF selected indexes is valid.
        private void UpdateCurrentEntryData()
        {
            FrameObjectBase fObject = (SceneData.FrameResource.FrameObjects.ElementAt(listBox1.SelectedIndex).Value as FrameObjectBase);
            CurrentEntry.Text = fObject.Name.String;
            PositionXBox.Text = fObject.Matrix.Position.X.ToString();
            PositionYBox.Text = fObject.Matrix.Position.Y.ToString();
            PositionZBox.Text = fObject.Matrix.Position.Z.ToString();
            RotationXBox.Text = fObject.Matrix.Rotation.EulerRotation.X.ToString();
            RotationYBox.Text = fObject.Matrix.Rotation.EulerRotation.Y.ToString();
            RotationZBox.Text = fObject.Matrix.Rotation.EulerRotation.Z.ToString();
        }

        private void EntryApplyChanges_OnClick(object sender, EventArgs e)
        {
            FrameObjectBase fObject = (SceneData.FrameResource.FrameObjects.ElementAt(listBox1.SelectedIndex).Value as FrameObjectBase);
            fObject.Matrix.Position = new Vector3(float.Parse(PositionXBox.Text), float.Parse(PositionYBox.Text), float.Parse(PositionZBox.Text));
            fObject.Matrix.Rotation.EulerRotation = new Vector3(float.Parse(RotationXBox.Text), float.Parse(RotationYBox.Text), float.Parse(RotationZBox.Text));
            fObject.Matrix.Rotation.UpdateMatrixFromEuler();
            Graphics.Models[fObject.RefID].DoRender = HideInViewerCheckBox.Checked;
            Graphics.Models[fObject.RefID].SetTransform(fObject.Matrix.Position.X, fObject.Matrix.Position.Y, fObject.Matrix.Position.Z, fObject.Matrix.Rotation);
        }
        private void Pick(int sx, int sy)
        {

            var ray = Graphics.Camera.GetPickingRay(new SharpDX.Vector2(sx, sy), new SharpDX.Vector2(Config.Width, Config.Height), Graphics.GetProjectionMatrix());

            // transform the picking ray into the object space of the mesh
            var invWorld = Matrix.Invert(Graphics.GetWorldMatrix());
            ray.Direction = SharpDX.Vector3.TransformNormal(ray.Direction, invWorld);
            ray.Position = SharpDX.Vector3.TransformCoordinate(ray.Position, invWorld);
            ray.Direction.Normalize();

            float tmin;
            string pickedModel = "";
            foreach (KeyValuePair<int, RenderModel> model in Graphics.Models)
            {
                SharpDX.Vector3 minVector = new SharpDX.Vector3(
                    model.Value.Transform.Column1[3] + model.Value.BoundingBox.Boundings.Minimum.X,
                    model.Value.Transform.Column2[3] + model.Value.BoundingBox.Boundings.Minimum.Y,
                    model.Value.Transform.Column3[3] + model.Value.BoundingBox.Boundings.Minimum.Z
                    );
                SharpDX.Vector3 maxVector = new SharpDX.Vector3(
                    model.Value.Transform.Column1[3] + model.Value.BoundingBox.Boundings.Maximum.X,
                    model.Value.Transform.Column2[3] + model.Value.BoundingBox.Boundings.Maximum.Y,
                    model.Value.Transform.Column3[3] + model.Value.BoundingBox.Boundings.Maximum.Z
    );
                SharpDX.BoundingBox tempBox = new SharpDX.BoundingBox(minVector, maxVector);
                if(ray.Intersects(ref tempBox, out tmin))
                {
                    Console.WriteLine(string.Format("Name {0}, Distance {1}", (SceneData.FrameResource.FrameObjects[model.Key] as FrameObjectBase).Name.String, tmin));
                }
                float tmin2 = float.MaxValue;
                for (var i = 0; i < model.Value.Vertices.Length; i++)
                {

                    float t = 0;
                    SharpDX.Vector3 temp_vec3 = model.Value.Vertices[i].position;
                    if (!ray.Intersects(ref temp_vec3)) continue;
                    // find the closest intersection, exclude intersections behind camera
                    if (!(t < tmin2 || t < 0)) continue;
                    tmin2 = t;
                }
                if (tmin < tmin2)
                    pickedModel = (SceneData.FrameResource.FrameObjects[model.Key] as FrameObjectBase).Name.String;
            }

            Console.WriteLine(pickedModel);
        }
    }
}
