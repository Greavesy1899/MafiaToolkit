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
using ResourceTypes.Materials;
using Utils.SharpDXExtensions;
using Gibbed.Illusion.FileFormats.Hashing;
using ResourceTypes.Collisions;

namespace Mafia2Tool
{
    public partial class D3DForm : Form
    {
        private InputClass Input { get; set; }
        private GraphicsClass Graphics { get; set; }

        private Point mousePos;
        private Point lastMousePos;
        private FileInfo fileLocation;

        //docking panels
        private DockPropertyGrid dPropertyGrid;
        private DockSceneTree dSceneTree;
        private DockViewProperties dViewProperties;

        //parent nodes for data
        private TreeNode frameResourceRoot;
        private TreeNode collisionRoot;
        private TreeNode roadRoot;
        private TreeNode junctionRoot;
        private TreeNode animalTrafficRoot;
        private TreeNode translokatorRoot;
        private TreeNode actorRoot;

        private bool bSelectMode = false;
        private float selectTimer = 0.0f;

        public D3DForm(FileInfo info)
        {
            InitializeComponent();
            Localise();
            MaterialData.Load();

            if (MaterialsManager.MTLs.Count == 0)
                MessageBox.Show("No material libraries have loaded, make sure they are set up correctly in the options window!", "Warning!", MessageBoxButtons.OK);

            TypeDescriptor.AddAttributes(typeof(Vector3), new TypeConverterAttribute(typeof(Vector3Converter)));
            ToolkitSettings.UpdateRichPresence(string.Format("Editing '{0}'", info.Directory.Name));
            SceneData.ScenePath = info.DirectoryName;
            fileLocation = info;
            SceneData.BuildData();
            InitDockingControls();
            PopulateList(info);
            CameraSpeedTool.Value = (decimal)ToolkitSettings.CameraSpeed;
            KeyPreview = true;
            Text += " -" + info.Directory.Name;
            SwitchMode(true);
            StartD3DPanel();
        }

        private void Localise()
        {
            FileButton.Text = Language.GetString("$FILE");
            EditButton.Text = Language.GetString("$CREATE");
            WindowButton.Text = Language.GetString("$VIEW");
            OptionsButton.Text = Language.GetString("$OPTIONS");
            ToggleWireframeButton.Text = Language.GetString("$TOGGLE_WIREFRAME");
            ToggleCullingButton.Text = Language.GetString("$TOGGLE_CULLING");
            SceneTreeButton.Text = Language.GetString("$VIEW_SCENE_TREE");
            ObjectPropertiesButton.Text = Language.GetString("$VIEW_PROPERTY_GRID");
            WindowButton.Text = Language.GetString("$VIEW_OPTIONS");
            ViewOptionProperties.Text = Language.GetString("$VIEW_VIS_OPTIONS");
            AddButton.Text = Language.GetString("$ADD");
            AddSceneFolderButton.Text = Language.GetString("$ADD_SCENE_FOLDER");
            AddRoadSplineButton.Text = Language.GetString("$ADD_ROAD_SPLINE");
            SaveButton.Text = Language.GetString("$SAVE");
            ExitButton.Text = Language.GetString("$EXIT");
        }

        private void InitDockingControls()
        {
            dockPanel1.Controls.Add(RenderPanel);
            dPropertyGrid = new DockPropertyGrid();
            dSceneTree = new DockSceneTree();
            dViewProperties = new DockViewProperties();
            dPropertyGrid.Show(dockPanel1, DockState.DockRight);
            dSceneTree.Show(dockPanel1, DockState.DockLeft);
            //dViewProperties.Show(dockPanel1, DockState.DockRight);
            dSceneTree.treeView1.AfterSelect += new TreeViewEventHandler(OnAfterSelect);
            dSceneTree.ExportFrameButton.Click += new EventHandler(ExportFrame_Click);
            dSceneTree.Export3DButton.Click += new EventHandler(Export3DButton_Click);
            dSceneTree.JumpToButton.Click += new EventHandler(JumpButton_Click);
            dSceneTree.DeleteButton.Click += new EventHandler(DeleteButton_Click);
            dSceneTree.DuplicateButton.Click += new EventHandler(DuplicateButton_Click);
            dSceneTree.treeView1.AfterCheck += new TreeViewEventHandler(OutlinerAfterCheck);
            dSceneTree.treeView1.KeyUp += new KeyEventHandler(OnKeyPressedDockedPanel);
            dPropertyGrid.KeyUp += new KeyEventHandler(OnKeyPressedDockedPanel);
            dPropertyGrid.PropertyGrid.SelectedGridItemChanged += new SelectedGridItemChangedEventHandler(OnPropertyGridSelectChanged);
            dPropertyGrid.PropertyGrid.PropertyValueChanged += new PropertyValueChangedEventHandler(OnPropertyValueChanged);
            dPropertyGrid.PositionXNumeric.ValueChanged += new EventHandler(ApplyEntryChanges);
            dPropertyGrid.PositionYNumeric.ValueChanged += new EventHandler(ApplyEntryChanges);
            dPropertyGrid.PositionZNumeric.ValueChanged += new EventHandler(ApplyEntryChanges);
            dPropertyGrid.RotationXNumeric.ValueChanged += new EventHandler(ApplyEntryChanges);
            dPropertyGrid.RotationYNumeric.ValueChanged += new EventHandler(ApplyEntryChanges);
            dPropertyGrid.RotationZNumeric.ValueChanged += new EventHandler(ApplyEntryChanges);
            dPropertyGrid.ScaleXNumeric.ValueChanged += new EventHandler(ApplyEntryChanges);
            dPropertyGrid.ScaleYNumeric.ValueChanged += new EventHandler(ApplyEntryChanges);
            dPropertyGrid.ScaleZNumeric.ValueChanged += new EventHandler(ApplyEntryChanges);
        }

        private void ExportFrame_Click(object sender, EventArgs e)
        {
            if(dSceneTree.treeView1.SelectedNode != null)
            {
                if(dSceneTree.treeView1.SelectedNode.Tag != null)
                {
                    SaveFrame(dSceneTree.treeView1.SelectedNode.Tag);
                }
            }        
        }

        private void OutlinerAfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null)
            {
                if (e.Node.Tag != null)
                {
                    bool isFrame = FrameResource.IsFrameType(e.Node.Tag);

                    if (isFrame)
                    {
                        int refID = (e.Node.Tag as FrameEntry).RefID;
                        if (Graphics.Assets.ContainsKey(refID))
                            Graphics.Assets[refID].DoRender = e.Node.Checked;
                    }
                    else if (e.Node.Tag.GetType() == typeof(RenderRoad) ||
                         e.Node.Tag.GetType() == typeof(RenderJunction) ||
                         e.Node.Tag.GetType() == typeof(RenderBoundingBox) ||
                         e.Node.Tag.GetType() == typeof(Collision.Placement))
                        Graphics.Assets[Convert.ToInt32(e.Node.Name)].DoRender = e.Node.Checked;
                }
            }
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
                result = Graphics.InitScene(RenderPanel.Width, RenderPanel.Height);
                UpdateMatricesRecursive();
            }
            return result;
        }

        public void Run()
        {
            RenderPanel.KeyDown += (s, e) => Input.KeyDown(e.KeyCode);
            RenderPanel.KeyUp += (s, e) => Input.KeyUp(e.KeyCode);
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
        private void ViewOptionProperties_Click(object sender, EventArgs e) => dSceneTree.Show(dockPanel1, DockState.DockLeft);
        private void CurrentModeButton_ButtonClick(object sender, EventArgs e) => SwitchMode(!bSelectMode);

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
            bool camUpdated = false;

            if(Input.IsKeyDown(Keys.Delete))
                dSceneTree.DeleteButton.PerformClick();

            if (RenderPanel.Focused)
            {
                if (Input.IsButtonDown(MouseButtons.Right))
                {
                    var dx = -0.25f * (mousePos.X - lastMousePos.X);
                    var dy = -0.25f * (mousePos.Y - lastMousePos.Y);
                    Graphics.Camera.Pitch(dy);
                    Graphics.Camera.Yaw(dx);
                    camUpdated = true;
                    
                }
                else if (Input.IsButtonDown(MouseButtons.Left) && bSelectMode && selectTimer <= 0.0f)
                {
                    Pick(mousePos.X, mousePos.Y);
                    selectTimer = 1.0f;
                }

                Ray ray = Graphics.Camera.GetPickingRay(new Vector2(mousePos.X, mousePos.Y), new Vector2(RenderPanel.Size.Width, RenderPanel.Size.Height));
                Graphics.Light.Direction = new Vector3(-0.2f, -1f, -0.3f);

                float multiplier = ToolkitSettings.CameraSpeed;

                if (Input.IsKeyDown(Keys.ShiftKey))
                    multiplier *= 2.0f;

                float speed = Graphics.Timer.FrameTime * multiplier;

                if (Input.IsKeyDown(Keys.A))
                {
                    Graphics.Camera.Position -= Vector3Extenders.FromVector4(Vector4.Multiply(Graphics.Camera.ViewMatrix.Column1, speed));
                    camUpdated = true;
                }

                if (Input.IsKeyDown(Keys.D))
                {
                    Graphics.Camera.Position += Vector3Extenders.FromVector4(Vector4.Multiply(Graphics.Camera.ViewMatrix.Column1, speed));
                    camUpdated = true;
                }

                if (Input.IsKeyDown(Keys.W))
                {
                    Graphics.Camera.Position -= Vector3Extenders.FromVector4(Vector4.Multiply(Graphics.Camera.ViewMatrix.Column3, speed));
                    camUpdated = true;
                }

                if (Input.IsKeyDown(Keys.S))
                {
                    Graphics.Camera.Position += Vector3Extenders.FromVector4(Vector4.Multiply(Graphics.Camera.ViewMatrix.Column3, speed));
                    camUpdated = true;
                }

                if (Input.IsKeyDown(Keys.Q))
                {
                    Graphics.Camera.Position.Z += speed;
                    camUpdated = true;
                }

                if (Input.IsKeyDown(Keys.E))
                {
                    Graphics.Camera.Position.Z -= speed;
                    camUpdated = true;
                }

                if (selectTimer > 0.0f)
                    selectTimer -= 0.1f;

                Graphics.Camera.SetProjectionMatrix(RenderPanel.Width, RenderPanel.Height);
            }
            lastMousePos = mousePos;
            Graphics.Timer.Frame2();
            Graphics.FPS.Frame();
            Graphics.Frame();

            if (camUpdated)
            {
                PositionXTool.ValueChanged -= new EventHandler(CameraToolsOnValueChanged);
                PositionYTool.ValueChanged -= new EventHandler(CameraToolsOnValueChanged);
                PositionZTool.ValueChanged -= new EventHandler(CameraToolsOnValueChanged);
                PositionXTool.Value = (decimal)Graphics.Camera.Position.X;
                PositionYTool.Value = (decimal)Graphics.Camera.Position.Y;
                PositionZTool.Value = (decimal)Graphics.Camera.Position.Z;
                PositionXTool.ValueChanged += new EventHandler(CameraToolsOnValueChanged);
                PositionYTool.ValueChanged += new EventHandler(CameraToolsOnValueChanged);
                PositionZTool.ValueChanged += new EventHandler(CameraToolsOnValueChanged);
                //RotationXTool.Value = (decimal)Graphics.Camera.Rotation.X;
                //RotationYTool.Value = (decimal)Graphics.Camera.Rotation.Y;
                //RotationZTool.Value = (decimal)Graphics.Camera.Rotation.Z;
            }
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

            TransformMatrix mat = matrix;
            foreach (TreeNode cNode in node.Nodes)
            {
                mat = matrix + ((obj2 != null) ? obj2.Matrix : new TransformMatrix());
                CallMatricesRecursive(cNode, mat);
            }
        }

        private void UpdateRenderedObjects(TransformMatrix obj1Matrix, FrameObjectBase obj)
        {
            if (Graphics.Assets.ContainsKey(obj.RefID))
                Graphics.Assets[obj.RefID].SetTransform(obj1Matrix.Position + obj.Matrix.Position, obj.Matrix.Matrix);
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

                if (SceneData.roadMap != null && ToolkitSettings.Experimental)
                {
                    List<SplineDefinition> splines = new List<SplineDefinition>();
                    List<JunctionDefinition> junctions = new List<JunctionDefinition>();

                    for (int i = 0; i != roadRoot.Nodes.Count; i++)
                    {
                        RenderRoad road = (RenderRoad)roadRoot.Nodes[i].Tag;
                        SplineDefinition spline = new SplineDefinition();
                        spline.NumSplines1 = spline.NumSplines2 = (short)road.Spline.Points.Length;
                        spline.unk0 = 128;
                        spline.points = road.Spline.Points;
                        spline.hasToward = road.HasToward;
                        spline.hasBackward = road.HasBackward;
                        spline.backward = road.Backward;
                        spline.toward = road.Toward;
                        splines.Add(spline);
                    }

                    for (int i = 0; i < junctionRoot.Nodes.Count; i++)
                    {
                        RenderJunction junction = (RenderJunction)junctionRoot.Nodes[i].Tag;
                        JunctionDefinition definition = junction.Data;
                        junctions.Add(definition);
                    }

                    SceneData.roadMap.splines = splines.ToArray();
                    SceneData.roadMap.junctionData = junctions.ToArray();
                    SceneData.roadMap.WriteToFile();
                }

                if (SceneData.Collisions != null)
                {
                    Collision collision = new Collision();
                    collision.Name = SceneData.Collisions.Name;
                    for (int i = 0; i != collisionRoot.Nodes.Count; i++)
                    {
                        TreeNode node = collisionRoot.Nodes[i];
                        Collision.NXSStruct nxsData = (node.Tag as Collision.NXSStruct);
                        collision.NXSData.Add(nxsData.Hash, nxsData);

                        for (int x = 0; x != node.Nodes.Count; x++)
                        {
                            TreeNode child = node.Nodes[x];
                            Collision.Placement placement = (child.Tag as Collision.Placement);
                            collision.Placements.Add(placement);
                        }
                    }

                    SceneData.Collisions = collision;
                    SceneData.Collisions.WriteToFile();
                }

              

                Console.WriteLine("Saved Changes Succesfully");
            }
        }

        private RenderBoundingBox BuildRenderBounds(FrameObjectDummy dummy)
        {
            RenderBoundingBox dummyBBox = new RenderBoundingBox();
            dummyBBox.SetTransform(dummy.Matrix.Position, dummy.Matrix.Matrix);
            dummyBBox.Init(dummy.Bounds);
            return dummyBBox;
        }

        private RenderBoundingBox BuildRenderBounds(FrameObjectArea area)
        {
            RenderBoundingBox areaBBox = new RenderBoundingBox();
            areaBBox.SetTransform(area.Matrix.Position, area.Matrix.Matrix);
            areaBBox.Init(area.Bounds);
            return areaBBox;
        }

        private RenderBoundingBox BuildRenderBounds(FrameObjectSector sector)
        {
            RenderBoundingBox areaBBox = new RenderBoundingBox();
            areaBBox.SetTransform(sector.Matrix.Position, sector.Matrix.Matrix);
            areaBBox.Init(sector.Bounds);
            return areaBBox;
        }

        private RenderBoundingBox BuildRenderBounds(FrameObjectFrame frame)
        {
            RenderBoundingBox frameBBox = new RenderBoundingBox();
            frameBBox.SetTransform(frame.Matrix.Position, frame.Matrix.Matrix);
            frameBBox.Init(new BoundingBox(new Vector3(0.5f), new Vector3(0.5f)));
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

            if (SceneData.FrameResource != null && SceneData.FrameNameTable != null)
            {
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

                    if (fObject.GetType() == typeof(FrameObjectSector))
                    {
                        FrameObjectSector sector = (fObject as FrameObjectSector);
                        assets.Add(fObject.RefID, BuildRenderBounds(sector));
                    }

                    if (fObject.GetType() == typeof(FrameObjectDummy))
                    {
                        FrameObjectDummy dummy = (fObject as FrameObjectDummy);
                        assets.Add(fObject.RefID, BuildRenderBounds(dummy));
                    }

                    if (fObject.GetType() == typeof(FrameObjectFrame))
                    {
                        FrameObjectFrame frame = (fObject as FrameObjectFrame);
                        assets.Add(fObject.RefID, BuildRenderBounds(frame));
                    }
                }
            }

            //if (SceneData.Translokator != null && ToolkitSettings.Experimental)
            //{
            //    TreeNode node = new TreeNode("Translokator Data");
            //    translokatorRoot = node;

            //    BoundingBox bounds = SceneData.Translokator.Bounds;
            //    for (int i = 0; i != SceneData.Translokator.Grids.Length; i++)
            //    {
            //        var grid = SceneData.Translokator.Grids[i];
            //        TreeNode gridNode = new TreeNode("Grid " + (i+1));
            //        gridNode.Tag = grid;
            //        gridNode.Name = "Grid"+(i+1);

            //        var curCellX = grid.CellSize.X;
            //        var curCellY = grid.CellSize.Y;
            //        for (int y = 0; y != grid.Height; y++)
            //        {
            //            for (int x = 0; x < grid.Width; x++)
            //            {
            //                RenderBoundingBox bboxX = new RenderBoundingBox();
            //                var curBounds = new Vector3(bounds.Minimum.X + curCellX, bounds.Minimum.Y + curCellY, 0.0f);
            //                bboxX.Init(new BoundingBox(new Vector3(bounds.Minimum.X, bounds.Minimum.Y, 0.0f), curBounds));
            //                bboxX.SetTransform(new Vector3(0, 0, 0), new Matrix33());

            //                var refID = 0;
            //                var bAllow = false;
            //                while (!bAllow)
            //                {
            //                    if (!assets.ContainsKey(refID))
            //                        bAllow = true;
            //                    else
            //                        refID = StringHelpers.RandomGenerator.Next() + 65535 + y;
            //                }
            //                assets.Add(refID, bboxX);
            //                TreeNode gridXNode = new TreeNode(string.Format("[{0}, {1}]", x, y));
            //                gridXNode.Name = refID.ToString();
            //                gridXNode.Tag = bboxX;
            //                gridNode.Nodes.Add(gridXNode);
            //                curCellX += grid.CellSize.X;
            //            }
            //            curCellY += grid.CellSize.Y;
            //            curCellX = grid.CellSize.X;
            //        }
            //        node.Nodes.Add(gridNode);
            //    }
            //    for (int i = 0; i != SceneData.Translokator.ObjectGroups.Length; i++)
            //    {
            //        var objGroup = SceneData.Translokator.ObjectGroups[i];

            //        for (int x = 0; x != objGroup.Objects.Length; x++)
            //        {
            //            var objects = objGroup.Objects[x];
            //            TreeNode objectNode = new TreeNode(objects.Name);
            //            node.Nodes.Add(objectNode);

            //            //RenderModel model = null;

            //            //foreach (KeyValuePair<int, object> pair in SceneData.FrameResource.FrameObjects)
            //            //{
            //            //    FrameObjectBase baseObject = (FrameObjectBase)pair.Value;
            //            //    if (baseObject.GetType() == typeof(FrameObjectFrame))
            //            //    {
            //            //        if ((baseObject as FrameObjectFrame).ActorHash.uHash == objects.Hash)
            //            //        {
            //            //            model = (RenderModel)assets[baseObject.RefID];
            //            //        }
            //            //    }
            //            //}

            //            for (int z = 0; z != objects.Instances.Length; z++)
            //            {
            //                var instance = objects.Instances[z];
            //                RenderBoundingBox bbox = new RenderBoundingBox();
            //                bbox.Init(new BoundingBox(new Vector3(-0.5f), new Vector3(0.5f)));
            //                bbox.SetTransform(instance.Position, new Matrix33());
            //                int refID = 65535 + z;

            //                var bAllow = false;
            //                while (!bAllow)
            //                {
            //                    if (!assets.ContainsKey(refID))
            //                        bAllow = true;
            //                    else
            //                        refID = StringHelpers.RandomGenerator.Next() + 65535 + z;
            //                }
            //                assets.Add(refID, bbox);

            //                TreeNode child = new TreeNode(objects.Name + " " + (z + 1));
            //                child.Tag = bbox;
            //                child.Name = refID.ToString();
            //                objectNode.Nodes.Add(child);
            //            }
            //        }
            //    }
            //    dSceneTree.AddToTree(node);
            //}

            if (SceneData.roadMap != null && ToolkitSettings.Experimental)
            {
                TreeNode node = new TreeNode("Road Data");
                roadRoot = node;
                TreeNode node2 = new TreeNode("Junction Data");
                junctionRoot = node2;

                for (int i = 0; i != SceneData.roadMap.splines.Length; i++)
                {
                    RenderRoad road = new RenderRoad();
                    int generatedID = StringHelpers.RandomGenerator.Next();
                    road.Init(SceneData.roadMap.splines[i]);
                    assets.Add(generatedID, road);
                    TreeNode child = new TreeNode(i.ToString());
                    child.Text = "Road ID: " + i;
                    child.Name = generatedID.ToString();
                    child.Tag = road;
                    node.Nodes.Add(child);
                }

                for (int i = 0; i < SceneData.roadMap.junctionData.Length; i++)
                {
                    int generatedID = StringHelpers.RandomGenerator.Next();
                    RenderJunction junction = new RenderJunction();
                    junction.Init(SceneData.roadMap.junctionData[i]);
                    assets.Add(generatedID, junction);
                    TreeNode child = new TreeNode(i.ToString());
                    child.Text = "Junction ID: " + i;
                    child.Name = generatedID.ToString();
                    child.Tag = junction;
                    junctionRoot.Nodes.Add(child);
                }
                dSceneTree.AddToTree(node);
                dSceneTree.AddToTree(node2);
            }
            if (SceneData.Collisions != null)
            {
                TreeNode node = new TreeNode("Collision Data");
                collisionRoot = node;

                for (int i = 0; i != SceneData.Collisions.NXSData.Count; i++)
                {
                    Collision.NXSStruct data = SceneData.Collisions.NXSData.ElementAt(i).Value;
                    RenderStaticCollision collision = new RenderStaticCollision();
                    collision.ConvertCollisionToRender(data.Data);
                    RenderStorageSingleton.Instance.StaticCollisions.Add(SceneData.Collisions.NXSData.ElementAt(i).Key, collision);
                    TreeNode treeNode = new TreeNode(data.Hash.ToString());
                    treeNode.Text = data.Hash.ToString();
                    treeNode.Name = data.Hash.ToString();
                    treeNode.Tag = data;
                    dSceneTree.AddToTree(treeNode, collisionRoot);
                }

                for (int i = 0; i != SceneData.Collisions.Placements.Count; i++)
                {
                    Collision.Placement placement = SceneData.Collisions.Placements[i];
                    TreeNode[] nodes = collisionRoot.Nodes.Find(placement.Hash.ToString(), false);

                    if (nodes.Length > 0)
                    {
                        int refID = StringHelpers.RandomGenerator.Next();
                        RenderInstance instance = new RenderInstance();
                        instance.Init(RenderStorageSingleton.Instance.StaticCollisions[placement.Hash]);
                        Matrix33 rot = new Matrix33();
                        rot.SetEuler(placement.Rotation);
                        instance.SetTransform(placement.Position, rot);
                        TreeNode child = new TreeNode();
                        child.Text = nodes[0].Nodes.Count.ToString();
                        child.Name = refID.ToString();
                        child.Tag = placement;
                        assets.Add(refID, instance);
                        nodes[0].Nodes.Add(child);
                    }

                }
                dSceneTree.AddToTree(node);
                collisionRoot.Collapse(false);
            }
            //if (SceneData.ATLoader != null && SceneData.ATLoader.paths != null && ToolkitSettings.Experimental)
            //{
            //    TreeNode node = new TreeNode("Animal Traffic");
            //    animalTrafficRoot = node;
            //    for (int i = 0; i != SceneData.ATLoader.paths.Length; i++)
            //    {
            //        AnimalTrafficLoader.AnimalTrafficPath path = SceneData.ATLoader.paths[i];
            //        RenderATP atp = new RenderATP();
            //        atp.Init(path);

            //        int refID = StringHelpers.RandomGenerator.Next();
            //        TreeNode child = new TreeNode("Path: " + i);
            //        child.Name = refID.ToString();
            //        child.Text = "Path: " + i;
            //        child.Tag = atp;
            //        animalTrafficRoot.Nodes.Add(child);
            //        assets.Add(refID, atp);
            //    }
            //    dSceneTree.AddToTree(animalTrafficRoot);
            //}

            //if (SceneData.OBJData.Length > 0 && ToolkitSettings.Experimental)
            //{
            //    foreach (var data in SceneData.OBJData)
            //    {
            //        var objData = (data.data as OBJData);

            //        for (int i = 0; i != objData.vertices.Length; i++)
            //        {
            //            var nodeData = objData.vertices[i];
            //            RenderBoundingBox box = new RenderBoundingBox();
            //            box.Init(new BoundingBox(new Vector3(-1.0f), new Vector3(1.0f)));
            //            box.SetTransform(nodeData.position, new Matrix33());
            //            assets.Add(StringHelpers.RandomGenerator.Next(), box);

            //            if (nodeData.unk5 > 0)
            //            {
            //                RenderLine line = new RenderLine();
            //                line.Init(new Vector3[2] { nodeData.position, objData.vertices[nodeData.unk5].position });
            //                assets.Add(StringHelpers.RandomGenerator.Next(), line);
            //            }
            //        }
            //    }
            //}

            if (SceneData.Actors.Length > 0 && ToolkitSettings.Experimental)
            {
                actorRoot = new TreeNode("Actor Items");
                for (int z = 0; z < SceneData.Actors.Length; z++)
                {
                    ResourceTypes.Actors.Actor actor = SceneData.Actors[z];
                    for (int c = 0; c != actor.Items.Length; c++)
                    {
                        var item = actor.Items[c];
                        TreeNode itemNode = new TreeNode("actor_" + z + "-" + c);
                        itemNode.Text = item.EntityType;
                        itemNode.Tag = item;

                        var typeString = string.Format("actorType_" + item.ItemType);
                        var foundnodes = actorRoot.Nodes.Find(typeString, false);
                        if(foundnodes.Length > 0)
                        {
                            foundnodes[0].Nodes.Add(itemNode);
                        }
                        else
                        {
                            TreeNode typeNode = new TreeNode(typeString);
                            typeNode.Name = typeString;
                            typeNode.Text = item.ItemType;
                            typeNode.Nodes.Add(itemNode);
                            actorRoot.Nodes.Add(typeNode);
                        }
                    }


                    for (int i = 0; i != actor.Definitions.Length; i++)
                    {
                        bool sorted = false;

                        for (int c = 0; c != actor.Items.Length; c++)
                        {
                            if (actor.Definitions[i].Hash == actor.Items[c].Hash2)
                            {
                                FrameObjectFrame frame = (SceneData.FrameResource.FrameObjects.ElementAt(actor.Definitions[i].FrameIndex).Value as FrameObjectFrame);
                                if (frame != null)
                                {
                                    frame.Item = actor.Items[c];
                                    frame.Matrix.Position = actor.Items[c].Position;
                                    sorted = true;
                                }
                            }
                        }
                        //Console.WriteLine("Sorted: {0}", sorted);
                    }
                }
                dSceneTree.AddToTree(actorRoot);
            }

            Graphics.InitObjectStack = assets;
        }

        private void TreeViewUpdateSelected()
        {
            if (dSceneTree.treeView1.SelectedNode.Tag == null)
                return;

            if (dSceneTree.treeView1.SelectedNode.Tag is RenderRoad)
            {
                Graphics.SelectEntry(Convert.ToInt32(dSceneTree.treeView1.SelectedNode.Name));
            }
            else if (dSceneTree.treeView1.SelectedNode.Tag is RenderJunction)
            {
                Graphics.SelectEntry(Convert.ToInt32(dSceneTree.treeView1.SelectedNode.Name));
            }
            else if (dSceneTree.treeView1.SelectedNode.Tag is Collision.Placement)
            {
                Graphics.SelectEntry(Convert.ToInt32(dSceneTree.treeView1.SelectedNode.Name));
            }
            else if (dSceneTree.treeView1.SelectedNode.Tag is FrameEntry)
            {
                Graphics.SelectEntry((dSceneTree.treeView1.SelectedNode.Tag as FrameEntry).RefID);
            }

            dPropertyGrid.SetObject(dSceneTree.treeView1.SelectedNode.Tag);
            //dPropertyGrid.Show(dockPanel1, DockState.DockRight);
            //RenderPanel.Focus();
        }

        private void ApplyEntryChanges(object sender, EventArgs e)
        {
            if (dPropertyGrid.IsEntryReady)
            {
                UpdateMatricesRecursive();
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
                else if (selected.Tag is Collision.Placement)
                {
                    RenderInstance instance = null;
                    dPropertyGrid.UpdateObject();
                    Collision.Placement placement = (selected.Tag as Collision.Placement);
                    selected.Text = placement.Hash.ToString();
                    IRenderer asset;
                    Graphics.Assets.TryGetValue(int.Parse(selected.Name), out asset);
                    instance = (asset as RenderInstance);
                    Matrix33 matrix = new Matrix33();
                    matrix.SetEuler(placement.Rotation);
                    instance.SetTransform(placement.Position, matrix);
                }
            }
        }

        private void ApplyChangesToRenderable(FrameObjectBase obj)
        {
            if (obj is FrameObjectArea)
            {
                FrameObjectArea area = (obj as FrameObjectArea);
                area.FillPlanesArray();
                RenderBoundingBox bbox = (Graphics.Assets[obj.RefID] as RenderBoundingBox);
                bbox.Update(area.Bounds);
            }
            else if (obj is FrameObjectSector)
            {
                FrameObjectSector sector = (obj as FrameObjectSector);
                sector.FillPlanesArray();
                RenderBoundingBox bbox = (Graphics.Assets[obj.RefID] as RenderBoundingBox);
                bbox.Update(sector.Bounds);
            }
            else if (obj is FrameObjectSingleMesh)
            {
                FrameObjectSingleMesh mesh = (obj as FrameObjectSingleMesh);
                RenderModel model = (Graphics.Assets[obj.RefID] as RenderModel);
                model.UpdateMaterials(mesh.Material);
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
                if (!model.ModelStructure.ReadFromFbx(MeshBrowser.FileName))
                    return null;
            }

            FrameResourceModelOptions options = new FrameResourceModelOptions(model.ModelStructure.Lods[0].VertexDeclaration);
            if(options.ShowDialog() != DialogResult.OK)
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

            for (int i = 0; i < model.FrameGeometry.NumLods; i++)
            {
                //Check for existing buffer; if it exists, remove so we can add one later.
                if (SceneData.IndexBufferPool.SearchBuffer(model.IndexBuffers[i].Hash) != null)
                    SceneData.IndexBufferPool.RemoveBuffer(model.IndexBuffers[i]);

                //do the same for vertexbuffer pools.
                if (SceneData.VertexBufferPool.SearchBuffer(model.VertexBuffers[i].Hash) != null)
                    SceneData.VertexBufferPool.RemoveBuffer(model.VertexBuffers[i]);

                SceneData.IndexBufferPool.AddBuffer(model.IndexBuffers[i]);
                SceneData.VertexBufferPool.AddBuffer(model.VertexBuffers[i]);
            }

            return mesh;
        }

        private void CreateNewEntry(int selected, string name)
        {
            FrameObjectBase frame;

            switch (selected)
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

            Ray ray = Graphics.Camera.GetPickingRay(new Vector2(sx, sy), new Vector2(RenderPanel.Size.Width, RenderPanel.Size.Height));
            foreach (KeyValuePair<int, IRenderer> model in Graphics.Assets)
            {
                if (!model.Value.DoRender)
                    continue;

                var vWM = Matrix.Invert(model.Value.Transform);
                var localRay = new Ray(
                    Vector3.TransformCoordinate(ray.Position, vWM),
                    Vector3.TransformNormal(ray.Direction, vWM)
                );
                if (model.Value is RenderModel)
                {
                    RenderModel mesh = (model.Value as RenderModel);
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
                if (model.Value is RenderInstance)
                {
                    RenderInstance instance = (model.Value as RenderInstance);
                    RenderStaticCollision collision = instance.GetCollision();
                    var bbox = collision.BoundingBox.BBox;

                    if (!localRay.Intersects(ref bbox)) continue;

                    for (var i = 0; i < collision.Indices.Length / 3; i++)
                    {
                        var v0 = collision.Vertices[collision.Indices[i * 3]].Position;
                        var v1 = collision.Vertices[collision.Indices[i * 3 + 1]].Position;
                        var v2 = collision.Vertices[collision.Indices[i * 3 + 2]].Position;
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

        private void JumpButton_Click(object sender, EventArgs e)
        {
            Graphics.Camera.Position = dSceneTree.JumpToHelper();
        }

        private void OnPropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            PropertyGrid pGrid = (s as PropertyGrid);
            if (pGrid.SelectedObject is FrameObjectBase)
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

                        if (obj.ParentIndex1.Index != -1)
                        {
                            nodes = dSceneTree.treeView1.Nodes.Find(obj.ParentIndex1.RefID.ToString(), true);

                            if (nodes.Length > 0)
                                dSceneTree.AddToTree(newNode, nodes[0]);
                        }
                        else if (obj.ParentIndex2.Index != -1)
                        {
                            nodes = dSceneTree.treeView1.Nodes.Find(obj.ParentIndex2.RefID.ToString(), true);

                            if (nodes.Length > 0)
                                dSceneTree.AddToTree(newNode, nodes[0]);
                        }
                    }
                }
            }
            if (pGrid.SelectedObject is FrameObjectSingleMesh)
            {
                FrameObjectSingleMesh obj = (dSceneTree.treeView1.SelectedNode.Tag as FrameObjectSingleMesh);

                if (e.ChangedItem.Label == "MeshIndex")
                {
                    int value = (int)e.ChangedItem.Value;
                    obj.Refs["Mesh"] = SceneData.FrameResource.NewFrames[value].Data.RefID;
                    obj.Geometry = SceneData.FrameResource.FrameGeometries[obj.Refs["Mesh"]];

                }
                if (e.ChangedItem.Label == "MaterialIndex")
                {
                    int value = (int)e.ChangedItem.Value;
                    obj.Refs["Material"] = SceneData.FrameResource.NewFrames[value].Data.RefID;
                    obj.Material = SceneData.FrameResource.FrameMaterials[obj.Refs["Material"]];
                }

                Graphics.Assets[obj.RefID].SetTransform(obj.Matrix.Position, obj.Matrix.Matrix);
            }
        }

        private void OnPropertyGridSelectChanged(object sender, SelectedGridItemChangedEventArgs e)
        {
            if (e.OldSelection == null || e.NewSelection == null)
                return;

            PropertyGrid pGrid = (sender as PropertyGrid);
            if (pGrid.SelectedObject is FrameObjectSingleMesh)
            {
                ApplyChangesToRenderable((FrameObjectBase)pGrid.SelectedObject);
            }
            if (pGrid.SelectedObject is RenderRoad)
            {
                RenderRoad road = (pGrid.SelectedObject as RenderRoad);
                road.Spline.UpdateVertices();
            }
            if (pGrid.SelectedObject is FrameObjectArea || pGrid.SelectedObject is FrameObjectSector)
            {
                switch (e.OldSelection.Label)
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

        }

        private void CameraSpeedUpdate(object sender, EventArgs e)
        {
            if (CameraSpeedTool.Value == CameraSpeedTool.Increment)
            {
                CameraSpeedTool.Increment = CameraSpeedTool.Increment * Convert.ToDecimal(0.1);
            }
            else if (CameraSpeedTool.Value == (CameraSpeedTool.Increment * 10) + CameraSpeedTool.Increment)
            {
                CameraSpeedTool.Value = CameraSpeedTool.Increment * 20;
                CameraSpeedTool.Increment = CameraSpeedTool.Increment * 10;
            }

            ToolkitSettings.CameraSpeed = Convert.ToSingle(CameraSpeedTool.Value);
            ToolkitSettings.WriteKey("CameraSpeed", "ModelViewer", ToolkitSettings.CameraSpeed.ToString());
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            TreeNode node = dSceneTree.treeView1.SelectedNode;

            if (FrameResource.IsFrameType(node.Tag))
            {
                if (node.Nodes.Count > 0)
                {
                    MessageBox.Show("Cannot delete a node with children!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }

                FrameEntry obj = node.Tag as FrameEntry;

                if (obj != null)
                {
                    dSceneTree.treeView1.Nodes.Remove(node);
                    Graphics.Assets.Remove(obj.RefID);
                    SceneData.FrameResource.FrameObjects.Remove(obj.RefID);

                    if (Graphics.Assets.ContainsKey(obj.RefID))
                        Graphics.Assets.Remove(obj.RefID);
                }
            }
            else if (node.Tag.GetType() == typeof(Collision.Placement))
            {
                dSceneTree.treeView1.Nodes.Remove(node);

                int iName = Convert.ToInt32(node.Name);
                if (Graphics.Assets.ContainsKey(iName))
                    Graphics.Assets.Remove(iName);
            }
            else if (node.Tag.GetType() == typeof(RenderRoad))
            {
                dSceneTree.treeView1.Nodes.Remove(node);
                if (Graphics.Assets.ContainsKey(int.Parse(node.Name)))
                    Graphics.Assets.Remove(int.Parse(node.Name));
            }
            else if (node.Tag.GetType() == typeof(Collision.NXSStruct))
            {
                dSceneTree.treeView1.Nodes.Remove(node);

                Collision.NXSStruct data = (node.Tag as Collision.NXSStruct);
                if (RenderStorageSingleton.Instance.StaticCollisions.ContainsKey(data.Hash))
                    RenderStorageSingleton.Instance.StaticCollisions.Remove(data.Hash);

                for (int i = 0; i != node.Nodes.Count; i++)
                {
                    int iName = Convert.ToInt32(node.Nodes[i].Name);
                    if (Graphics.Assets.ContainsKey(iName))
                        Graphics.Assets.Remove(iName);
                }
            }
        }

        private void DuplicateButton_Click(object sender, EventArgs e)
        {
            TreeNode node = dSceneTree.treeView1.SelectedNode;
            FrameObjectBase newEntry = null;

            //new safety net
            if (FrameResource.IsFrameType(node.Tag))
            {
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
                dSceneTree.treeView1.SelectedNode = tNode;
                UpdateMatricesRecursive();
            }
            else if (node.Tag.GetType() == typeof(Collision.Placement))
            {
                Collision.Placement placement = new Collision.Placement((Collision.Placement)node.Tag);

                int pIdxName = 0;
                int.TryParse(node.Text, out pIdxName);
                pIdxName++;

                int refID = StringHelpers.RandomGenerator.Next();
                TreeNode child = new TreeNode();
                child.Text = pIdxName.ToString();
                child.Name = refID.ToString();
                child.Tag = placement;
                dSceneTree.AddToTree(child, node.Parent);

                RenderInstance instance = new RenderInstance();
                instance.Init(RenderStorageSingleton.Instance.StaticCollisions[placement.Hash]);
                Matrix33 rot = new Matrix33();
                rot.SetEuler(placement.Rotation);
                instance.SetTransform(placement.Position, rot);
                Graphics.InitObjectStack.Add(refID, instance);
            }
        }

        private void Export3DButton_Click(object sender, EventArgs e)
        {
            if (dSceneTree.treeView1.SelectedNode.Tag.GetType() == typeof(Collision.NXSStruct))
                ExportCollision(dSceneTree.treeView1.SelectedNode.Tag as Collision.NXSStruct);
            else
                Export3DFrame(dSceneTree.treeView1.SelectedNode.Tag);
        }

        private void ExportCollision(Collision.NXSStruct data)
        {
            M2TStructure structure = new M2TStructure();
            structure.BuildCollision(data, dSceneTree.treeView1.SelectedNode.Name);
            structure.ExportCollisionToM2T(ToolkitSettings.ExportPath, data.Hash.ToString());
            structure.ExportToFbx(ToolkitSettings.ExportPath, false);
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

        private void SaveFrame(object tag)
        {
            string filename = (tag as FrameObjectBase).Name.String;
            using (BinaryWriter writer = new BinaryWriter(File.Open(Path.Combine(ToolkitSettings.ExportPath, filename) + ".framedata", FileMode.Create)))
            {
                if (FrameResource.IsFrameType(tag))
                {
                    //is this even needed? hmm.
                    if (tag.GetType() == typeof(FrameObjectArea))
                    {
                        writer.Write((ushort)ObjectType.Area);
                        (tag as FrameObjectArea).WriteToFile(writer);
                    }
                    else if (tag.GetType() == typeof(FrameObjectCamera))
                    {
                        writer.Write((ushort)ObjectType.Camera);
                        (tag as FrameObjectCamera).WriteToFile(writer);
                    }
                    else if (tag.GetType() == typeof(FrameObjectCollision))
                    {
                        writer.Write((ushort)ObjectType.Collision);
                        (tag as FrameObjectCollision).WriteToFile(writer);
                    }
                    else if (tag.GetType() == typeof(FrameObjectComponent_U005))
                    {
                        writer.Write((ushort)ObjectType.Collision);
                        (tag as FrameObjectComponent_U005).WriteToFile(writer);
                    }
                    else if (tag.GetType() == typeof(FrameObjectDummy))
                    {
                        writer.Write((ushort)ObjectType.Dummy);
                        (tag as FrameObjectDummy).WriteToFile(writer);
                    }
                    else if (tag.GetType() == typeof(FrameObjectDeflector))
                    {
                        writer.Write((ushort)ObjectType.ParticleDeflector);
                        (tag as FrameObjectDeflector).WriteToFile(writer);
                    }
                    else if (tag.GetType() == typeof(FrameObjectFrame))
                    {
                        writer.Write((ushort)ObjectType.Frame);
                        (tag as FrameObjectFrame).WriteToFile(writer);
                    }
                    else if (tag.GetType() == typeof(FrameObjectJoint))
                    {
                        writer.Write((ushort)ObjectType.Joint);
                        (tag as FrameObjectJoint).WriteToFile(writer);
                    }
                    else if (tag.GetType() == typeof(FrameObjectLight))
                    {
                        writer.Write((ushort)ObjectType.Light);
                        (tag as FrameObjectLight).WriteToFile(writer);
                    }
                    else if (tag.GetType() == typeof(FrameObjectModel))
                    {
                        var mesh = (tag as FrameObjectModel);
                        writer.Write((ushort)ObjectType.Model);
                        mesh.WriteToFile(writer);
                        mesh.Geometry.WriteToFile(writer);
                        mesh.Material.WriteToFile(writer);
                        mesh.BlendInfo.WriteToFile(writer);
                        mesh.Skeleton.WriteToFile(writer);
                        mesh.SkeletonHierarchy.WriteToFile(writer);

                        foreach(var lod in mesh.Geometry.LOD)
                        {
                            SceneData.IndexBufferPool.GetBuffer(lod.IndexBufferRef.uHash).WriteToFile(writer);
                            SceneData.VertexBufferPool.GetBuffer(lod.VertexBufferRef.uHash).WriteToFile(writer);
                        }
                    }
                    else if (tag.GetType() == typeof(FrameObjectSector))
                    {
                        writer.Write((ushort)ObjectType.Sector);
                        (tag as FrameObjectSector).WriteToFile(writer);
                    }
                    else if (tag.GetType() == typeof(FrameObjectSingleMesh))
                    {
                        var mesh = (tag as FrameObjectSingleMesh);
                        writer.Write((ushort)ObjectType.SingleMesh);
                        mesh.WriteToFile(writer);
                        mesh.Geometry.WriteToFile(writer);
                        mesh.Material.WriteToFile(writer);

                        foreach (var lod in mesh.Geometry.LOD)
                        {
                            SceneData.IndexBufferPool.GetBuffer(lod.IndexBufferRef.uHash).WriteToFile(writer);
                            SceneData.VertexBufferPool.GetBuffer(lod.VertexBufferRef.uHash).WriteToFile(writer);
                        }
                    }
                    else if (tag.GetType() == typeof(FrameObjectTarget))
                    {
                        writer.Write((ushort)ObjectType.Target);
                        (tag as FrameObjectTarget).WriteToFile(writer);
                    }
                    else
                    {
                        writer.Write((ushort)0);
                        (tag as FrameObjectBase).WriteToFile(writer);
                    }
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
            road.HasToward = true;
            road.Toward = new SplineProperties();
            road.Toward.Flags = 0;
            road.Toward.LaneSize0 = road.Toward.LaneSize1 = 2;
            road.Toward.Lanes = new LaneProperties[2];
            road.Toward.Lanes[0] = new LaneProperties();
            road.Toward.Lanes[0].Width = 3.5f;
            road.Toward.Lanes[0].Unk03 = 440;
            road.Toward.Lanes[0].Flags = LaneTypes.MainRoad;
            road.Toward.Lanes[1] = new LaneProperties();
            road.Toward.Lanes[1].Width = 3.5f;
            road.Toward.Lanes[1].Unk03 = 440;
            road.Toward.Lanes[1].Flags = LaneTypes.None;
            road.HasBackward = true;
            road.Backward = new SplineProperties();
            road.Backward.Flags = RoadFlags.BackwardDirection;
            road.Backward.LaneSize0 = road.Backward.LaneSize1 = 2;
            road.Backward.Lanes = new LaneProperties[2];
            road.Backward.Lanes[1] = new LaneProperties();
            road.Backward.Lanes[1].Width = 3.5f;
            road.Backward.Lanes[1].Unk03 = 440;
            road.Backward.Lanes[1].Flags = LaneTypes.MainRoad;
            road.Backward.Lanes[0] = new LaneProperties();
            road.Backward.Lanes[0].Width = 3.5f;
            road.Backward.Lanes[0].Unk03 = 440;
            road.Backward.Lanes[0].Flags = LaneTypes.None;


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

        private void AddSplineTxT_Click(object sender, EventArgs e)
        {
            if (SceneData.roadMap == null)
                return;

            if (TxtBrowser.ShowDialog() == DialogResult.Cancel)
                return;

            RenderRoad road = new RenderRoad();
            RenderLine spline = new RenderLine();

            string[] content = File.ReadAllLines(TxtBrowser.FileName);
            int numVertexes = int.Parse(content[0]);
            spline.Points = new Vector3[numVertexes];

            for (int i = 1; i != numVertexes; i++)
            {
                string[] splits = content[i].Split(' ');
                spline.Points[i - 1] = new Vector3(float.Parse(splits[0]), float.Parse(splits[1]), float.Parse(splits[2]));
            }

            road.Spline = spline;
            road.HasToward = true;
            road.Toward = new SplineProperties();
            road.Toward.Flags = 0;
            road.Toward.LaneSize0 = road.Toward.LaneSize1 = 2;
            road.Toward.Lanes = new LaneProperties[2];
            road.Toward.Lanes[0] = new LaneProperties();
            road.Toward.Lanes[0].Width = 3.5f;
            road.Toward.Lanes[0].Unk03 = 440;
            road.Toward.Lanes[0].Flags = LaneTypes.MainRoad;
            road.Toward.Lanes[1] = new LaneProperties();
            road.Toward.Lanes[1].Width = 3.5f;
            road.Toward.Lanes[1].Unk03 = 440;
            road.Toward.Lanes[1].Flags = LaneTypes.None;
            road.HasBackward = true;
            road.Backward = new SplineProperties();
            road.Backward.Flags = RoadFlags.BackwardDirection;
            road.Backward.LaneSize0 = road.Backward.LaneSize1 = 2;
            road.Backward.Lanes = new LaneProperties[2];
            road.Backward.Lanes[1] = new LaneProperties();
            road.Backward.Lanes[1].Width = 3.5f;
            road.Backward.Lanes[1].Unk03 = 440;
            road.Backward.Lanes[1].Flags = LaneTypes.MainRoad;
            road.Backward.Lanes[0] = new LaneProperties();
            road.Backward.Lanes[0].Width = 3.5f;
            road.Backward.Lanes[0].Unk03 = 440;
            road.Backward.Lanes[0].Flags = LaneTypes.None;


            int generatedID = StringHelpers.RandomGenerator.Next();
            RenderStorageSingleton.Instance.SplineStorage.Add(spline);
            Graphics.InitObjectStack.Add(generatedID, road);
            int nodeID = (roadRoot.Nodes.Count);
            TreeNode child = new TreeNode(nodeID.ToString());
            child.Text = "Road ID: " + nodeID;
            child.Name = generatedID.ToString();
            child.Tag = road;
            roadRoot.Nodes.Add(child);
        }

        private void AddJunctionOnClick(object sender, EventArgs e)
        {
            if (SceneData.roadMap == null)
                return;

            JunctionDefinition definition = new JunctionDefinition();
            RenderJunction junction = new RenderJunction();
            definition.JunctionIDX = junctionRoot.Nodes.Count;
            junction.Init(definition);

            int generatedID = StringHelpers.RandomGenerator.Next();
            Graphics.InitObjectStack.Add(generatedID, junction);
            int nodeID = (junctionRoot.Nodes.Count);
            TreeNode child = new TreeNode(nodeID.ToString());
            child.Text = "Junction ID: " + nodeID;
            child.Name = generatedID.ToString();
            child.Tag = junction;
            junctionRoot.Nodes.Add(child);
        }

        private void EditUnkSet3Click(object sender, EventArgs e)
        {
            if (SceneData.roadMap != null)
                dPropertyGrid.SetObject(SceneData.roadMap);
        }

        private void AddTowardClick(object sender, EventArgs e)
        {
            if (SceneData.roadMap != null && dSceneTree.treeView1.SelectedNode != null)
            {
                if (dSceneTree.treeView1.SelectedNode.Tag.GetType() == typeof(RenderRoad))
                {
                    RenderRoad road = (dSceneTree.treeView1.SelectedNode.Tag as RenderRoad);
                    road.HasToward = true;
                    road.Toward = new SplineProperties();
                    road.Toward.Flags = 0;
                    road.Toward.LaneSize0 = road.Toward.LaneSize1 = 2;
                    road.Toward.Lanes = new LaneProperties[2];
                    road.Toward.Lanes[0] = new LaneProperties();
                    road.Toward.Lanes[0].Width = 3.5f;
                    road.Toward.Lanes[0].Unk03 = 440;
                    road.Toward.Lanes[0].Flags = LaneTypes.MainRoad;
                    road.Toward.Lanes[1] = new LaneProperties();
                    road.Toward.Lanes[1].Width = 3.5f;
                    road.Toward.Lanes[1].Unk03 = 440;
                    road.Toward.Lanes[1].Flags = LaneTypes.None;
                }
            }
        }

        private void AddBackwardClick(object sender, EventArgs e)
        {
            if (SceneData.roadMap != null && dSceneTree.treeView1.SelectedNode != null)
            {
                if (dSceneTree.treeView1.SelectedNode.Tag.GetType() == typeof(RenderRoad))
                {
                    RenderRoad road = (dSceneTree.treeView1.SelectedNode.Tag as RenderRoad);
                    road.HasBackward = true;
                    road.Backward = new SplineProperties();
                    road.Backward.Flags = RoadFlags.BackwardDirection;
                    road.Backward.LaneSize0 = road.Backward.LaneSize1 = 2;
                    road.Backward.Lanes = new LaneProperties[2];
                    road.Backward.Lanes[1] = new LaneProperties();
                    road.Backward.Lanes[1].Width = 3.5f;
                    road.Backward.Lanes[1].Unk03 = 440;
                    road.Backward.Lanes[1].Flags = LaneTypes.MainRoad;
                    road.Backward.Lanes[0] = new LaneProperties();
                    road.Backward.Lanes[0].Width = 3.5f;
                    road.Backward.Lanes[0].Unk03 = 440;
                    road.Backward.Lanes[0].Flags = LaneTypes.None;
                }
            }
        }

        private void AddCollisionButton_Click(object sender, EventArgs e)
        {
            if (SceneData.Collisions != null && ToolkitSettings.Experimental)
            {
                if (MeshBrowser.ShowDialog() != DialogResult.OK)
                {
                    MessageBox.Show("Failed to select model.", "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                M2TStructure colModel = new M2TStructure();

                if (MeshBrowser.FileName.ToLower().EndsWith(".m2t"))
                    colModel.ReadFromM2T(new BinaryReader(File.Open(MeshBrowser.FileName, FileMode.Open)));
                else if (MeshBrowser.FileName.ToLower().EndsWith(".fbx"))
                    colModel.ReadFromFbx(MeshBrowser.FileName);

                //crash happened/
                if (colModel.Lods[0] == null)
                    return;

                Collision.NXSStruct nxsData = new Collision.NXSStruct();
                nxsData.Hash = FNV64.Hash(colModel.Name);
                nxsData.Data.BuildBasicCollision(colModel.Lods[0]);
                nxsData.Sections = new Collision.Section[colModel.Lods[0].Parts.Length];

                int curEdges = 0;
                for (int i = 0; i != nxsData.Sections.Length; i++)
                {
                    nxsData.Sections[i] = new Collision.Section();

                    //handle collision type.
                    var result = CollisionMaterials.Concrete;
                    if (!Enum.TryParse(colModel.Lods[0].Parts[i].Material, out result))
                        result = CollisionMaterials.Concrete;

                    nxsData.Sections[i].Unk1 = (int)result - 2;
                    nxsData.Sections[i].Start = curEdges;
                    nxsData.Sections[i].NumEdges = (int)colModel.Lods[0].Parts[i].NumFaces * 3;
                }

                RenderStaticCollision collision = new RenderStaticCollision();
                collision.ConvertCollisionToRender(nxsData.Data);
                RenderStorageSingleton.Instance.StaticCollisions.Add(nxsData.Hash, collision);

                Collision.Placement placement = new Collision.Placement();
                placement.Hash = nxsData.Hash;
                placement.Unk5 = 128;
                placement.Unk4 = -1;
                placement.Position = new Vector3(0, 0, 0);
                placement.Rotation = new Vector3(0);

                //add to render storage
                TreeNode treeNode = new TreeNode(nxsData.Hash.ToString());
                treeNode.Text = nxsData.Hash.ToString();
                treeNode.Name = nxsData.Hash.ToString();
                treeNode.Tag = nxsData;

                //add instance of object.
                int refID = StringHelpers.RandomGenerator.Next();
                TreeNode child = new TreeNode();
                child.Text = treeNode.Nodes.Count.ToString();
                child.Name = refID.ToString();
                child.Tag = placement;
                treeNode.Nodes.Add(child);

                //complete
                RenderInstance instance = new RenderInstance();
                instance.Init(RenderStorageSingleton.Instance.StaticCollisions[placement.Hash]);
                Matrix33 rot = new Matrix33();
                rot.SetEuler(placement.Rotation);
                instance.SetTransform(placement.Position, rot);
                Graphics.InitObjectStack.Add(refID, instance);
                dSceneTree.AddToTree(treeNode, collisionRoot);
                SceneData.Collisions.NXSData.Add(nxsData.Hash, nxsData);
                SceneData.Collisions.Placements.Add(placement);
            }
        }

        private void CameraToolsOnValueChanged(object sender, EventArgs e)
        {
            Graphics.Camera.Position = new Vector3(Convert.ToSingle(PositionXTool.Value), Convert.ToSingle(PositionYTool.Value), Convert.ToSingle(PositionZTool.Value));
            //Graphics.Camera.SetRotation(Convert.ToSingle(RotationXTool.Value), Convert.ToSingle(RotationYTool.Value));
            //lastMousePos = new Point(RenderPanel.Height / 2, RenderPanel.Width / 2);
        }

        private void OnViewTopButtonClicked(object sender, EventArgs e)
        {
            Graphics.Camera.SetRotation(0.0f, 180.0f);
            lastMousePos = new Point(RenderPanel.Height / 2, RenderPanel.Width / 2);
        }

        private void OnViewFrontButtonClicked(object sender, EventArgs e)
        {
            Graphics.Camera.SetRotation(90.0f, 90.0f);
            lastMousePos = new Point(RenderPanel.Height / 2, RenderPanel.Width / 2);
        }

        private void OnViewSideButtonClicked(object sender, EventArgs e)
        {
            Graphics.Camera.SetRotation(90.0f, 270.0f);
            lastMousePos = new Point(RenderPanel.Height / 2, RenderPanel.Width / 2);
        }

        private void OnViewBottomButtonClicked(object sender, EventArgs e)
        {
            Graphics.Camera.SetRotation(90.0f, 90.0f);
            lastMousePos = new Point(RenderPanel.Height / 2, RenderPanel.Width / 2);
        }

        private void OnKeyPressedDockedPanel(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Delete)
                dSceneTree.DeleteButton.PerformClick();
        }

        private void OnViewSide2ButtonClicked(object sender, EventArgs e)
        {

        }

        private void SwitchMode(bool isSelectMode)
        {
            bSelectMode = isSelectMode;
            CurrentModeButton.Text = (bSelectMode) ? "Select Mode" : "Edit Mode";
        }
    }
}

