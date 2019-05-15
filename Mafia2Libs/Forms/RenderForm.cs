using Rendering.Graphics;
using Rendering.Input;
using SharpDX.Windows;
using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using SharpDX;
using ResourceTypes.FrameNameTable;
using ResourceTypes.FrameResource;
using ResourceTypes.BufferPools;
using Collision = ResourceTypes.Collisions.Collision;
using Utils.Settings;
using Utils.Types;
using Utils.Lang;
using Forms.EditorControls;
using Utils.StringHelpers;
using Forms.Docking;
using WeifenLuo.WinFormsUI.Docking;
using Utils.Models;
using System.ComponentModel;
using Utils.Extensions;
using ResourceTypes.Navigation;

namespace Mafia2Tool
{
    public partial class D3DForm : Form
    {
        private InputClass Input { get; set; }
        private GraphicsClass Graphics { get; set; }

        private Point mousePos;
        private Point lastMousePos;
        private FileInfo fileLocation;
        private Ray ray;

        //docking panels
        private DockPropertyGrid dPropertyGrid;
        private DockSceneTree dSceneTree;
        private DockViewProperties dViewProperties;

        //parent nodes for data
        private TreeNode frameResourceRoot;
        private TreeNode collisionRoot;
        private TreeNode roadRoot;
        private TreeNode animalTrafficRoot;

        public D3DForm(FileInfo info)
        {
            InitializeComponent();
            TypeDescriptor.AddAttributes(typeof(Vector3), new TypeConverterAttribute(typeof(Vector3Converter)));
            ToolkitSettings.UpdateRichPresence(string.Format("Editing '{0}'", info.Directory.Name));
            SceneData.ScenePath = info.DirectoryName;
            fileLocation = info;
            SceneData.BuildData();
            InitDockingControls();
            PopulateList(info);
            TEMPCameraSpeed.Text = ToolkitSettings.CameraSpeed.ToString();
            KeyPreview = true;
            RenderPanel.Focus();
            StartD3DPanel();
        }

        private void InitDockingControls()
        {
            dockPanel1.Controls.Add(RenderPanel);
            dPropertyGrid = new DockPropertyGrid();
            dSceneTree = new DockSceneTree();
            dViewProperties = new DockViewProperties();
            dPropertyGrid.Show(dockPanel1, DockState.DockRight);
            dSceneTree.Show(dockPanel1, DockState.DockLeft);
            dViewProperties.Show(dockPanel1, DockState.DockRight);
            dSceneTree.treeView1.AfterSelect += new TreeViewEventHandler(OnAfterSelect);
            dSceneTree.Export3DButton.Click += new EventHandler(Export3DButton_Click);
            dSceneTree.PreviewButton.Click += new EventHandler(PreviewButton_Click);
            dSceneTree.DeleteButton.Click += new EventHandler(DeleteButton_Click);
            dSceneTree.DuplicateButton.Click += new EventHandler(DuplicateButton_Click);
            dPropertyGrid.PropertyGrid.PropertyValueChanged += new PropertyValueChangedEventHandler(OnPropertyChanged);
            dPropertyGrid.PositionXNumeric.ValueChanged += new EventHandler(ApplyEntryChanges);
            dPropertyGrid.PositionYNumeric.ValueChanged += new EventHandler(ApplyEntryChanges);
            dPropertyGrid.PositionZNumeric.ValueChanged += new EventHandler(ApplyEntryChanges);
            dPropertyGrid.RotationXNumeric.ValueChanged += new EventHandler(ApplyEntryChanges);
            dPropertyGrid.RotationYNumeric.ValueChanged += new EventHandler(ApplyEntryChanges);
            dPropertyGrid.RotationZNumeric.ValueChanged += new EventHandler(ApplyEntryChanges);
        }

        public void PopulateList(FileInfo info)
        {
            TreeNode tree = SceneData.FrameResource.BuildTree(SceneData.FrameNameTable);
            frameResourceRoot = tree;
            dSceneTree.AddToTree(tree);
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
                UpdateMatricesRecursive();
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

        private void RenderPanel_MouseEnter(object sender, EventArgs e) => RenderPanel.Focus();
        private void RenderForm_MouseMove(object sender, MouseEventArgs e) => mousePos = new Point(e.Location.X, e.Location.Y);
        private void CullModeButton_Click(object sender, EventArgs e) => Graphics.ToggleD3DCullMode();
        private void FillModeButton_Click(object sender, EventArgs e) => Graphics.ToggleD3DFillMode();
        private void OnSelectedIndexChanged(object sender, EventArgs e) => TreeViewUpdateSelected();
        private void OnAfterSelect(object sender, TreeViewEventArgs e) => TreeViewUpdateSelected();
        private void ExitButton_Click(object sender, EventArgs e) => Close();
        private void SaveButton_Click(object sender, EventArgs e) => Save();
        private void PropertyGridOnClicked(object sender, EventArgs e) => dPropertyGrid.Show(dockPanel1, DockState.DockRight);
        private void SceneTreeOnClicked(object sender, EventArgs e) => dSceneTree.Show(dockPanel1, DockState.DockLeft);

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            SceneData.CleanData();
            RenderStorageSingleton.Instance.TextureCache.Clear();
            dSceneTree.Dispose();
            dPropertyGrid.Dispose();
            dViewProperties.Dispose();
            Shutdown();
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
                    Pick(mousePos.X, mousePos.Y);
                }

                float speed = Graphics.Timer.FrameTime * ToolkitSettings.CameraSpeed;

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
            Graphics.FPS.Frame();

            foreach (KeyValuePair<int, IRenderer> render in Graphics.Assets)
            {
                if (render.Value.GetType() == typeof(RenderModel))
                    render.Value.DoRender = dViewProperties.VisibleProperties[0];
                else if (render.Value.GetType() == typeof(RenderBoundingBox))
                    render.Value.DoRender = dViewProperties.VisibleProperties[2];
                else if (render.Value.GetType() == typeof(RenderInstance))
                    render.Value.DoRender = dViewProperties.VisibleProperties[1];
            }

            Graphics.Frame();
            toolStripStatusLabel1.Text = Graphics.Camera.Position.ToString();
            toolStripStatusLabel2.Text = string.Format("{0} {1}", mousePos.X, mousePos.Y);
            toolStripStatusLabel3.Text = string.Format("{0} FPS", Graphics.FPS.FPS);
            return true;
        }

        private void UpdateMatricesRecursive()
        {
            FrameObjectBase obj1;

            foreach (TreeNode node in dSceneTree.treeView1.Nodes)
            {
                obj1 = (node.Tag as FrameObjectBase);
                TransformMatrix matrix = ((obj1 != null) ? obj1.Matrix : new TransformMatrix());

                if (obj1 != null)
                    UpdateRenderedObjects(matrix, obj1);

                foreach (TreeNode cNode in node.Nodes)
                {
                    CallMatricesRecursive(cNode, matrix);
                }
            }
        }
        private void CallMatricesRecursive(TreeNode node, TransformMatrix matrix)
        {
            FrameObjectBase obj2 = (node.Tag as FrameObjectBase);

            if (obj2 != null)
                UpdateRenderedObjects(matrix, obj2);

            foreach (TreeNode cNode in node.Nodes)
            {
                matrix = ((obj2 != null) ? obj2.Matrix : new TransformMatrix());
                CallMatricesRecursive(cNode, matrix);
            }
        }

        private void UpdateRenderedObjects(TransformMatrix obj1Matrix, FrameObjectBase obj)
        {
            if (Graphics.Assets.ContainsKey(obj.RefID))
                Graphics.Assets[obj.RefID].SetTransform(obj1Matrix.Position + obj.Matrix.Position, obj.Matrix.Rotation);
        }

        private void Save()
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
                    nameTable.FileName = SceneData.FrameNameTable.FileName;
                    nameTable.BuildDataFromResource(SceneData.FrameResource);
                    nameTable.WriteToFile(writer);
                    SceneData.FrameNameTable = nameTable;
                }
                SceneData.IndexBufferPool.WriteToFile();
                SceneData.VertexBufferPool.WriteToFile();

                if(SceneData.roadMap != null)
                {
                    List<SplineDefinition> splines = new List<SplineDefinition>();
                    
                    for(int i = 0; i != RenderStorageSingleton.Instance.SplineStorage.Count; i++)
                    {
                        RenderLine line = RenderStorageSingleton.Instance.SplineStorage[i];
                        SplineDefinition spline = new SplineDefinition();
                        spline.NumSplines1 = spline.NumSplines2 = (short)line.Points.Length;
                        spline.unk0 = 128;
                        spline.points = line.Points;
                        splines.Add(spline);
                    }
                    SceneData.roadMap.data1 = splines.ToArray();
                    SceneData.roadMap.WriteToFile();
                }
                Console.WriteLine("Saved Changes Succesfully");
            }
        }

        private RenderBoundingBox BuildRenderBounds(FrameObjectDummy dummy)
        {
            RenderBoundingBox dummyBBox = new RenderBoundingBox();
            dummyBBox.SetTransform(dummy.Matrix.Position, dummy.Matrix.Rotation);
            dummyBBox.Init(dummy.Bounds);
            return dummyBBox;
        }

        private RenderBoundingBox BuildRenderBounds(FrameObjectArea area)
        {
            RenderBoundingBox areaBBox = new RenderBoundingBox();
            areaBBox.SetTransform(area.Matrix.Position, area.Matrix.Rotation);
            areaBBox.Init(area.Bounds);
            return areaBBox;
        }

        private RenderBoundingBox BuildRenderBounds(FrameObjectFrame frame)
        {
            RenderBoundingBox frameBBox = new RenderBoundingBox();
            frameBBox.SetTransform(frame.Matrix.Position, frame.Matrix.Rotation);
            frameBBox.Init(new BoundingBox(new Vector3(0.5f),  new Vector3(0.5f)));
            return frameBBox;
        }

        private RenderModel BuildRenderModel(FrameObjectSingleMesh mesh)
        {
            if (mesh.MaterialIndex == -1 && mesh.MeshIndex == -1)
                return null;

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
            return model;
        }

        private void BuildRenderObjects()
        {
            Dictionary<int, IRenderer> assets = new Dictionary<int, IRenderer>();

            for (int i = 0; i != SceneData.FrameResource.FrameObjects.Count; i++)
            {
                FrameEntry fObject = (SceneData.FrameResource.FrameObjects.ElementAt(i).Value as FrameEntry);

                if (fObject.GetType() == typeof(FrameObjectSingleMesh) || fObject.GetType() == typeof(FrameObjectModel))
                {
                    FrameObjectSingleMesh mesh = (fObject as FrameObjectSingleMesh);
                    RenderModel model = BuildRenderModel(mesh);

                    if (model == null)
                        continue;

                    assets.Add(fObject.RefID, model);
                }

                if (fObject.GetType() == typeof(FrameObjectArea))
                {
                    FrameObjectArea area = (fObject as FrameObjectArea);
                    assets.Add(fObject.RefID, BuildRenderBounds(area));
                }

                if (fObject.GetType() == typeof(FrameObjectDummy))
                {
                    FrameObjectDummy dummy = (fObject as FrameObjectDummy);
                    assets.Add(fObject.RefID, BuildRenderBounds(dummy));

                }

                if(fObject.GetType() == typeof(FrameObjectFrame))
                {
                    FrameObjectFrame frame = (fObject as FrameObjectFrame);
                    assets.Add(fObject.RefID, BuildRenderBounds(frame));
                }
            }

            if (SceneData.roadMap != null)
            {
                TreeNode node = new TreeNode("RoadMap Data");
                roadRoot = node;

                for(int i = 0; i != SceneData.roadMap.data1.Length; i++)
                {
                    ResourceTypes.Navigation.SplineDefinition definition = SceneData.roadMap.data1[i];
                    RenderLine line = new RenderLine();
                    line.SetColour(new Vector4(1.0f, 0.0f, 0.0f, 1.0f));
                    line.Init(definition.points);
                    RenderStorageSingleton.Instance.SplineStorage.Add(line);
                }
                for (int i = 0; i != SceneData.roadMap.data3.Length; i++)
                {
                    SplineProperties properties = SceneData.roadMap.data3[i];
                    RenderRoad road = new RenderRoad();
                    int generatedID = StringHelpers.RandomGenerator.Next();
                    road.Init(properties);
                    assets.Add(generatedID, road);
                    TreeNode child = new TreeNode(i.ToString());
                    child.Text = "ID: " + i;
                    child.Name = generatedID.ToString();
                    child.Tag = road;
                    node.Nodes.Add(child);
                }

                for (int i = 0; i < SceneData.roadMap.data4.Length; i++)
                {
                    if (SceneData.roadMap.data4[i].boundaries.Length > 0)
                    {
                        Vector3[] extraPoints = new Vector3[SceneData.roadMap.data4[i].boundaries.Length+1];
                        Array.Copy(SceneData.roadMap.data4[i].boundaries, extraPoints, SceneData.roadMap.data4[i].boundaries.Length);
                        extraPoints[extraPoints.Length - 1] = extraPoints[0];
                        RenderLine lineBoundary = new RenderLine();
                        lineBoundary.SetColour(new Vector4(1.0f, 1.0f, 1.0f, 1.0f));
                        lineBoundary.Init(extraPoints);
                        assets.Add(StringHelpers.RandomGenerator.Next(), lineBoundary);
                    }

                    for (int x = 0; x < SceneData.roadMap.data4[i].splines.Length; x++)
                    {
                        RenderLine line = new RenderLine();
                        line.SetColour(new Vector4(0.0f, 1.0f, 0.0f, 1.0f));
                        line.Init(SceneData.roadMap.data4[i].splines[x].path);
                        assets.Add(StringHelpers.RandomGenerator.Next(), line);
                    }
                }
                dSceneTree.AddToTree(node);
            }
            if (SceneData.Collisions != null)
            {
                TreeNode node = new TreeNode("Collision Data");
                collisionRoot = node;

                for(int i = 0; i != SceneData.Collisions.NXSData.Count; i++)
                {
                    Collision.NXSStruct data = SceneData.Collisions.NXSData.ElementAt(i).Value;
                    RenderStaticCollision collision = new RenderStaticCollision();
                    collision.ConvertCollisionToRender(data.Data);
                    RenderStorageSingleton.Instance.StaticCollisions.Add(SceneData.Collisions.NXSData.ElementAt(i).Key, collision);
                }

                for (int i = 0; i != SceneData.Collisions.Placements.Count; i++)
                {
                    Collision.Placement placement = SceneData.Collisions.Placements[i];
                    RenderInstance instance = new RenderInstance();
                    instance.Init(RenderStorageSingleton.Instance.StaticCollisions[placement.Hash]);

                    Matrix33 rot = new Matrix33();
                    rot.EulerRotation = placement.Rotation;
                    rot.UpdateMatrixFromEuler();
                    instance.SetTransform(placement.Position, rot);

                    bool isAdded = false;
                    int inc = 0;
                    int hash = 0;
                    while(!isAdded)
                    {
                        hash = (int)(placement.Hash);
                        hash += inc;
                        isAdded = !assets.ContainsKey(hash);
                        inc++;
                    }
                    assets.Add(hash, instance);

                    TreeNode child = new TreeNode(hash.ToString());
                    child.Text = "Hash: " + placement.Hash;
                    child.Name = hash.ToString();
                    child.Tag = placement;
                    node.Nodes.Add(child);
                }

                dSceneTree.AddToTree(node);
            }
            if (SceneData.ATLoader != null)
            {
                for(int i = 0; i != SceneData.ATLoader.paths.Length; i++)
                {
                    AnimalTrafficLoader.AnimalTrafficPath path = SceneData.ATLoader.paths[i];
                    RenderBoundingBox bbox = new RenderBoundingBox();
                    bbox.SetTransform(new Vector3(0.0f), new Matrix33());
                    bbox.Init(path.bbox);

                    RenderLine line = new RenderLine();
                    line.SetTransform(new Vector3(0.0f), new Matrix33());
                    List<Vector3> points = new List<Vector3>();
                    points.Add(path.bbox.Center);

                    for(int x = 0; x < path.vectors.Length; x++)
                        points.Add(path.vectors[x].vectors[0]);

                    line.SetColour(new Vector4(0.0f, 0.0f, 1.0f, 1.0f));
                    line.Init(points.ToArray());
                    assets.Add(StringHelpers.RandomGenerator.Next(), bbox);
                    assets.Add(StringHelpers.RandomGenerator.Next(), line);
                }
            }
            Graphics.InitObjectStack = assets;
        }

        private void TreeViewUpdateSelected()
        {
            if (dSceneTree.treeView1.SelectedNode.Tag == null)
                return;

            if(dSceneTree.treeView1.SelectedNode.Tag is RenderRoad)
            {
                Graphics.SelectEntry(Convert.ToInt32(dSceneTree.treeView1.SelectedNode.Name));
            }
            else if (dSceneTree.treeView1.SelectedNode.Tag is Collision.Placement)
            {
                Graphics.SelectEntry(Convert.ToInt32(dSceneTree.treeView1.SelectedNode.Name));
            }
            else if(dSceneTree.treeView1.SelectedNode.Tag is FrameEntry)
            {
                Graphics.SelectEntry((dSceneTree.treeView1.SelectedNode.Tag as FrameEntry).RefID);
            }   

            dPropertyGrid.SetObject(dSceneTree.treeView1.SelectedNode.Tag);
            dPropertyGrid.Show(dockPanel1, DockState.DockRight);
            RenderPanel.Focus();
        }

        private void ApplyEntryChanges(object sender, EventArgs e)
        {
            if (dPropertyGrid.IsEntryReady)
            {
                TreeNode selected = dSceneTree.treeView1.SelectedNode;
                if (selected.Tag is FrameObjectBase)
                {
                    FrameObjectBase fObject = (selected.Tag as FrameObjectBase);
                    selected.Text = fObject.ToString();
                    dPropertyGrid.UpdateObject();
                    //Graphics.SelectEntry(fObject.RefID);
                    UpdateMatricesRecursive();
                    ApplyChangesToRenderable(fObject);
                }
                else if (selected.Tag is FrameHeaderScene)
                {
                    FrameHeaderScene scene = (selected.Tag as FrameHeaderScene);
                    selected.Text = scene.ToString();
                }
                else if(selected.Tag is Collision.Placement)
                {
                    RenderInstance instance = null;
                    dPropertyGrid.UpdateObject();
                    Collision.Placement placement = (selected.Tag as Collision.Placement);
                    selected.Text = placement.Hash.ToString();
                    IRenderer asset;
                    Graphics.Assets.TryGetValue(int.Parse(selected.Name), out asset);
                    instance = (asset as RenderInstance);
                    Matrix33 matrix = new Matrix33();
                    matrix.EulerRotation = placement.Rotation;
                    matrix.UpdateMatrixFromEuler();
                    instance.SetTransform(placement.Position, matrix);
                }
            }
        }

        private void ApplyChangesToRenderable(FrameObjectBase obj)
        {
            if (obj is FrameObjectArea)
            {
                FrameObjectArea area = (obj as FrameObjectArea);
                RenderBoundingBox bbox = (Graphics.Assets[obj.RefID] as RenderBoundingBox);
                bbox.Update(area.Bounds);
            }
        }

        private FrameObjectBase CreateSingleMesh()
        {
            FrameObjectBase mesh = new FrameObjectSingleMesh();

            Model model = new Model();
            model.FrameMesh = (mesh as FrameObjectSingleMesh);

            if (MeshBrowser.ShowDialog() == DialogResult.Cancel)
                return null;

            if (MeshBrowser.FileName.ToLower().EndsWith(".m2t"))
                model.ModelStructure.ReadFromM2T(new BinaryReader(File.Open(MeshBrowser.FileName, FileMode.Open)));
            else if (MeshBrowser.FileName.ToLower().EndsWith(".fbx"))
            {
                if (model.ModelStructure.ReadFromFbx(MeshBrowser.FileName) == -1)
                    return null;
            }

            FrameResourceModelOptions options = new FrameResourceModelOptions();
            options.ShowDialog();

            if (options.type == -1)
                return null;

            bool[] data = options.data;
            options.Dispose();

            //for (int i = 0; i != model.ModelStructure.Lods.Length; i++)
            //{
            //    if (data[0])
            //    {
            //        model.ModelStructure.Lods[i].VertexDeclaration -= VertexFlags.Normals;
            //        model.ModelStructure.Lods[i].VertexDeclaration -= VertexFlags.Tangent;
            //    }

            //    if (data[5])
            //        model.ModelStructure.FlipUVs();
            //}

            FrameObjectSingleMesh sm = (mesh as FrameObjectSingleMesh);
            sm.Name.Set(model.ModelStructure.Name);
            model.CreateObjectsFromModel();
            sm.AddRef(FrameEntryRefTypes.Mesh, model.FrameGeometry.RefID);
            sm.Geometry = model.FrameGeometry;
            sm.AddRef(FrameEntryRefTypes.Material, model.FrameMaterial.RefID);
            sm.Material = model.FrameMaterial;
            SceneData.FrameResource.FrameMaterials.Add(model.FrameMaterial.RefID, model.FrameMaterial);
            SceneData.FrameResource.FrameGeometries.Add(model.FrameGeometry.RefID, model.FrameGeometry);

            //Check for existing buffer; if it exists, remove so we can add one later.
            if (SceneData.IndexBufferPool.SearchBuffer(model.IndexBuffers[0].Hash) != null)
                SceneData.IndexBufferPool.RemoveBuffer(model.IndexBuffers[0]);

            //do the same for vertexbuffer pools.
            if (SceneData.VertexBufferPool.SearchBuffer(model.VertexBuffers[0].Hash) != null)
                SceneData.VertexBufferPool.RemoveBuffer(model.VertexBuffers[0]);

            SceneData.IndexBufferPool.AddBuffer(model.IndexBuffers[0]);
            SceneData.VertexBufferPool.AddBuffer(model.VertexBuffers[0]);

            return mesh;
        }

        private void CreateNewEntry(int selected, string name)
        {
            FrameObjectBase frame;

            switch(selected)
            {
                case 0:
                    frame = CreateSingleMesh();

                    if (frame == null)
                        return;
                    break;
                case 1:
                    frame = new FrameObjectFrame();
                    break;
                case 2:
                    frame = new FrameObjectLight();
                    break;
                case 3:
                    frame = new FrameObjectCamera();
                    break;
                case 4:
                    frame = new FrameObjectComponent_U005();
                    break;
                case 5:
                    frame = new FrameObjectSector();
                    break;
                case 6:
                    frame = new FrameObjectDummy();
                    break;
                case 7:
                    frame = new FrameObjectDeflector();
                    break;
                case 8:
                    frame = new FrameObjectArea();
                    break;
                case 9:
                    frame = new FrameObjectTarget();
                    break;
                case 10:
                    throw new NotImplementedException();
                    break;
                case 11:
                    frame = new FrameObjectCollision();
                    break;
                default:
                    frame = new FrameObjectBase();
                    Console.WriteLine("Unknown type selected");
                    break;
            }

            frame.Name.Set(name);
            SceneData.FrameResource.FrameObjects.Add(frame.RefID, frame);
            TreeNode node = new TreeNode(frame.Name.String);
            node.Tag = frame;
            node.Name = frame.RefID.ToString();
            dSceneTree.AddToTree(node);

            if (frame.GetType() == typeof(FrameObjectSingleMesh) || frame.GetType() == typeof(FrameObjectModel))
            {
                FrameObjectSingleMesh mesh = (frame as FrameObjectSingleMesh);
                RenderModel model = BuildRenderModel(mesh);

                Graphics.InitObjectStack.Add(frame.RefID, model);
            }

            if (frame.GetType() == typeof(FrameObjectArea))
            {
                FrameObjectArea area = (frame as FrameObjectArea);
                Graphics.InitObjectStack.Add(frame.RefID, BuildRenderBounds(area));
            }

            if (frame.GetType() == typeof(FrameObjectDummy))
            {
                FrameObjectDummy dummy = (frame as FrameObjectDummy);
                Graphics.InitObjectStack.Add(frame.RefID, BuildRenderBounds(dummy));

            }
        }

        private void Pick(int sx, int sy)
        {
            float lowest = float.MaxValue;
            int lowestRefID = -1;

            ray = Graphics.Camera.GetPickingRay(new Vector2(sx, sy), new Vector2(RenderPanel.Size.Width, RenderPanel.Size.Height));

            foreach (KeyValuePair<int, IRenderer> model in Graphics.Assets)
            {
                if (model.Value is RenderModel)
                {
                    RenderModel mesh = (model.Value as RenderModel);
                    if (!mesh.DoRender)
                        continue;

                    var vWM = Matrix.Invert(model.Value.Transform);
                    var localRay = new Ray(
                        Vector3.TransformCoordinate(ray.Position, vWM),
                        Vector3.TransformNormal(ray.Direction, vWM)
                    );

                    var bbox = mesh.BoundingBox.BBox;

                    if (!localRay.Intersects(ref bbox)) continue;

                    for (var i = 0; i < mesh.LODs[0].Indices.Length / 3; i++)
                    {
                        var v0 = mesh.LODs[0].Vertices[mesh.LODs[0].Indices[i * 3]].Position;
                        var v1 = mesh.LODs[0].Vertices[mesh.LODs[0].Indices[i * 3 + 1]].Position;
                        var v2 = mesh.LODs[0].Vertices[mesh.LODs[0].Indices[i * 3 + 2]].Position;
                        float t;

                        if (!localRay.Intersects(ref v0, ref v1, ref v2, out t)) continue;
                        System.Diagnostics.Debug.Assert(t > 0f);

                        var worldPosition = ray.Position + t * ray.Direction;
                        var distance = (worldPosition - ray.Position).LengthSquared();

                        if (distance < lowest)
                        {
                            lowest = distance;
                            lowestRefID = model.Key;
                        }
                    }
                }
            }
            toolStripStatusLabel4.Text = ray.Position.ToString();
            TreeNode[] nodes = dSceneTree.treeView1.Nodes.Find(lowestRefID.ToString(), true);

            if (nodes.Length > 0)
            {
                dSceneTree.treeView1.SelectedNode = nodes[0];
                TreeViewUpdateSelected();
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
            PropertyGrid pGrid = (s as PropertyGrid);
            if(pGrid.SelectedObject is RenderRoad)
            {
                RenderRoad road = (pGrid.SelectedObject as RenderRoad);
                road.Spline.UpdateVertices();
            }

            if(pGrid.SelectedObject is FrameObjectArea)
            {
                switch(e.ChangedItem.Label)
                {
                    case "BoundsMinimumX":
                    case "BoundsMinimumY":
                    case "BoundsMinimumZ":
                    case "BoundsMaximumX":
                    case "BoundsMaximumY":
                    case "BoundsMaximumZ":
                        ApplyChangesToRenderable((FrameObjectBase)pGrid.SelectedObject);
                        break;
                }
            }
            if(pGrid.SelectedObject is FrameObjectBase)
            {
                if (e.ChangedItem.Label == "RefID")
                {
                    TreeNode[] nodes = dSceneTree.treeView1.Nodes.Find(e.ChangedItem.Value.ToString(), true);

                    if (nodes.Length > 0)
                    {
                        int newValue = (int)e.ChangedItem.Value;
                        FrameObjectBase obj = (dSceneTree.treeView1.SelectedNode.Tag as FrameObjectBase);
                        int newIndex = 0;
                        string name = "";

                        for (int i = 0; i != SceneData.FrameResource.FrameObjects.Count; i++)
                        {
                            FrameObjectBase frameObj = (SceneData.FrameResource.FrameObjects.ElementAt(i).Value as FrameObjectBase);

                            if (frameObj.RefID == newValue)
                            {
                                newIndex = i;
                                name = frameObj.Name.String;
                            }
                        }

                        if (newIndex == -1)
                        {
                            for (int i = 0; i != SceneData.FrameResource.FrameScenes.Count; i++)
                            {
                                FrameHeaderScene frameObj = SceneData.FrameResource.FrameScenes.ElementAt(i).Value;

                                if (frameObj.RefID == newValue)
                                {
                                    newIndex = i;
                                    name = frameObj.Name.String;
                                }
                            }
                        }

                        if (string.IsNullOrEmpty(name))
                            name = "unknown";

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
                        dSceneTree.treeView1.Nodes.Remove(dSceneTree.treeView1.SelectedNode);
                        TreeNode newNode = new TreeNode(obj.ToString());
                        newNode.Tag = obj;
                        newNode.Name = obj.RefID.ToString();
                        dSceneTree.AddToTree(newNode, nodes[0]);
                    }
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
            TreeNode node = dSceneTree.treeView1.SelectedNode;

            if (node.Nodes.Count > 0)
            {
                MessageBox.Show("Cannot delete a node with children!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                FrameEntry obj = node.Tag as FrameEntry;

                if (obj != null)
                {
                    dSceneTree.treeView1.Nodes.Remove(node);
                    Graphics.Assets.Remove(obj.RefID);
                    SceneData.FrameResource.FrameObjects.Remove(obj.RefID);

                    if(Graphics.Assets.ContainsKey(obj.RefID))
                        Graphics.Assets.Remove(obj.RefID);
                }
            }

        }

        private void DuplicateButton_Click(object sender, EventArgs e)
        {
            TreeNode node = dSceneTree.treeView1.SelectedNode;
            FrameObjectBase newEntry = null;

            //is this even needed? hmm.
            if (node.Tag.GetType() == typeof(FrameObjectArea))
            {
                newEntry = new FrameObjectArea((FrameObjectArea)node.Tag);
                FrameObjectArea area = (newEntry as FrameObjectArea);
                Graphics.InitObjectStack.Add(area.RefID, BuildRenderBounds(area));
            }
            else if (node.Tag.GetType() == typeof(FrameObjectCamera))
                newEntry = new FrameObjectCamera((FrameObjectCamera)node.Tag);
            else if (node.Tag.GetType() == typeof(FrameObjectCollision))
                newEntry = new FrameObjectCollision((FrameObjectCollision)node.Tag);
            else if (node.Tag.GetType() == typeof(FrameObjectComponent_U005))
                newEntry = new FrameObjectComponent_U005((FrameObjectComponent_U005)node.Tag);
            else if (node.Tag.GetType() == typeof(FrameObjectDummy))
            {
                newEntry = new FrameObjectDummy((FrameObjectDummy)node.Tag);
                FrameObjectDummy dummy = (newEntry as FrameObjectDummy);
                Graphics.InitObjectStack.Add(dummy.RefID, BuildRenderBounds(dummy));
            }
            else if (node.Tag.GetType() == typeof(FrameObjectDeflector))
                newEntry = new FrameObjectDeflector((FrameObjectDeflector)node.Tag);
            else if (node.Tag.GetType() == typeof(FrameObjectDummy))
                newEntry = new FrameObjectDummy((FrameObjectDummy)node.Tag);
            else if (node.Tag.GetType() == typeof(FrameObjectFrame))
                newEntry = new FrameObjectFrame((FrameObjectFrame)node.Tag);
            else if (node.Tag.GetType() == typeof(FrameObjectJoint))
                newEntry = new FrameObjectJoint((FrameObjectJoint)node.Tag);
            else if (node.Tag.GetType() == typeof(FrameObjectLight))
                newEntry = new FrameObjectLight((FrameObjectLight)node.Tag);
            else if (node.Tag.GetType() == typeof(FrameObjectModel))
            {
                newEntry = new FrameObjectModel((FrameObjectModel)node.Tag);
                FrameObjectSingleMesh mesh = (newEntry as FrameObjectSingleMesh);
                RenderModel model = BuildRenderModel(mesh);
                Graphics.InitObjectStack.Add(mesh.RefID, model);
            }
            else if (node.Tag.GetType() == typeof(FrameObjectSector))
                newEntry = new FrameObjectSector((FrameObjectSector)node.Tag);
            else if (node.Tag.GetType() == typeof(FrameObjectSingleMesh))
            {
                newEntry = new FrameObjectSingleMesh((FrameObjectSingleMesh)node.Tag);
                FrameObjectSingleMesh mesh = (newEntry as FrameObjectSingleMesh);
                RenderModel model = BuildRenderModel(mesh);
                Graphics.InitObjectStack.Add(mesh.RefID, model);
            }
            else if (node.Tag.GetType() == typeof(FrameObjectTarget))
                newEntry = new FrameObjectTarget((FrameObjectTarget)node.Tag);
            else
                newEntry = new FrameObjectBase((FrameObjectBase)node.Tag);

            newEntry.Name.Set(newEntry.Name.String + "_dupe");
            TreeNode tNode = new TreeNode(newEntry.ToString());
            tNode.Tag = newEntry;
            tNode.Name = newEntry.RefID.ToString();
            dSceneTree.AddToTree(tNode, dSceneTree.treeView1.Nodes.Find(newEntry.ParentIndex2.RefID.ToString(), true)[0]);
            SceneData.FrameResource.FrameObjects.Add(newEntry.RefID, newEntry);
            UpdateMatricesRecursive();
        }

        private void Export3DButton_Click(object sender, EventArgs e)
        {
            if(dSceneTree.treeView1.SelectedNode.Tag.GetType() == typeof(Collision.Placement))
                ExportCollision(dSceneTree.treeView1.SelectedNode.Tag as Collision.Placement);
            else
                Export3DFrame(dSceneTree.treeView1.SelectedNode.Tag);
        }

        private void ExportCollision(Collision.Placement data)
        {
            M2TStructure structure = new M2TStructure();
            structure.BuildCollision(SceneData.Collisions.NXSData[data.Hash], dSceneTree.treeView1.SelectedNode.Name);
            structure.ExportCollisionToM2T(dSceneTree.treeView1.SelectedNode.Name);
            structure.ExportToFbx("Collisions/", false);
        }
        private void Export3DFrame(object tag)
        {
            FrameObjectSingleMesh mesh = (dSceneTree.treeView1.SelectedNode.Tag as FrameObjectSingleMesh);
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

            for (int c = 0; c != newModel.ModelStructure.Lods.Length; c++)
            {
                newModel.ModelStructure.ExportToM2T(ToolkitSettings.ExportPath + "\\");
                switch (ToolkitSettings.Format)
                {
                    case 0:
                        newModel.ModelStructure.ExportToFbx(ToolkitSettings.ExportPath + "\\", false);
                        break;
                    case 1:
                        newModel.ModelStructure.ExportToFbx(ToolkitSettings.ExportPath + "\\", true);
                        break;
                    case 2:
                        newModel.ModelStructure.ExportToM2T(ToolkitSettings.ExportPath + "\\");
                        break;
                    default:
                        break;
                }
            }
        }

        private void AddButtonOnClick(object sender, EventArgs e)
        {
            NewObjectForm form = new NewObjectForm(true);
            form.SetLabel(Language.GetString("$QUESTION_FRADD"));
            form.LoadOption(new FrameResourceAddOption());
            form.ShowDialog();

            int selection;

            if (form.type != -1)
                selection = (form.control as FrameResourceAddOption).GetSelectedType();
            else return;

            CreateNewEntry(selection, form.GetInputText());
        }

        private void AddSceneFolderButton_Click(object sender, EventArgs e)
        {
            var scene = SceneData.FrameResource.AddSceneFolder("sceneNew");
            TreeNode node = new TreeNode(scene.ToString());
            node.Tag = scene;
            node.Name = scene.RefID.ToString();
            dSceneTree.AddToTree(node, frameResourceRoot);
        }

        private void AddRoadSplineButton_Click(object sender, EventArgs e)
        {
            if (SceneData.roadMap == null)
                return;

            RenderRoad road = new RenderRoad();
            RenderLine spline = new RenderLine();
            spline.Points = new Vector3[1] { new Vector3(0, 0, 0) };
            road.Spline = spline;

            int generatedID = StringHelpers.RandomGenerator.Next();
            RenderStorageSingleton.Instance.SplineStorage.Add(spline);
            Graphics.InitObjectStack.Add(generatedID, road);
            int nodeID = (roadRoot.Nodes.Count + 1);
            TreeNode child = new TreeNode(nodeID.ToString());
            child.Text = "ID: " + nodeID;
            child.Name = generatedID.ToString();
            child.Tag = road;
            roadRoot.Nodes.Add(child);
        }
    }
}
