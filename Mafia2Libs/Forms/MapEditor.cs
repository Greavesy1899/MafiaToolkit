using Forms.Docking;
using Forms.EditorControls;
using Gibbed.Illusion.FileFormats.Hashing;
using Rendering.Core;
using Rendering.Factories;
using Rendering.Graphics;
using Rendering.Input;
using ResourceTypes.Actors;
using ResourceTypes.BufferPools;
using ResourceTypes.Collisions;
using ResourceTypes.FrameNameTable;
using ResourceTypes.FrameResource;
using ResourceTypes.Materials;
using ResourceTypes.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;
using Toolkit.Core;
using Utils.Extensions;
using Utils.Language;
using Utils.Logging;
using Utils.Models;
using Utils.Settings;
using Utils.VorticeUtils;
using Vortice.Mathematics;
using WeifenLuo.WinFormsUI.Docking;
using Collision = ResourceTypes.Collisions.Collision;

namespace Toolkit
{
    public partial class MapEditor : Form
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
        private TreeNode actorRoot;

        private bool bSelectMode = false;
        private float selectTimer = 0.0f;
        private bool bHideChildren = false;

        private Dictionary<string, int> NamesAndDuplicationStore;

        public MapEditor(FileInfo info)
        {
            InitializeComponent();
            Localise();

            if (MaterialsManager.MaterialLibraries.Count == 0)
            {
                MessageBox.Show("No material libraries have loaded, make sure they are set up correctly in the options window!", "Warning!", MessageBoxButtons.OK);
            }

            ToolkitSettings.UpdateRichPresence(string.Format("Editing '{0}'", info.Directory.Name));
            fileLocation = info;
            InitDockingControls();
            PopulateList();
            NamesAndDuplicationStore = new Dictionary<string, int>();
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
            ViewButton.Text = Language.GetString("$VIEW");
            ViewTopButton.Text = Language.GetString("$TOP");
            ViewFrontButton.Text = Language.GetString("$FRONT");
            OptionsButton.Text = Language.GetString("$OPTIONS");
            ToggleWireframeButton.Text = Language.GetString("$TOGGLE_WIREFRAME");
            ToggleCullingButton.Text = Language.GetString("$TOGGLE_CULLING");
            EditLighting.Text = Language.GetString("$EDIT_LIGHTING");
            SceneTreeButton.Text = Language.GetString("$VIEW_SCENE_TREE");
            ObjectPropertiesButton.Text = Language.GetString("$VIEW_PROPERTY_GRID");
            WindowButton.Text = Language.GetString("$VIEW_OPTIONS");
            ViewOptionProperties.Text = Language.GetString("$VIEW_VIS_OPTIONS");
            AddButton.Text = Language.GetString("$ADD");
            Button_ImportFrame.Text = Language.GetString("$IMPORT_FRAME");
            AddSceneFolderButton.Text = Language.GetString("$ADD_SCENE_FOLDER");
            AddCollisionButton.Text = Language.GetString("$ADD_COLLISION");
            AddRoadSplineButton.Text = Language.GetString("$ADD_ROAD_SPLINE");
            SaveButton.Text = Language.GetString("$SAVE");
            ExitButton.Text = Language.GetString("$EXIT");
        }

        private void InitDockingControls()
        {
            VS2015LightTheme BlueTheme = new VS2015LightTheme();
            dockPanel1.Theme = BlueTheme;
            dockPanel1.Controls.Add(RenderPanel);
            RenderPanel.Resize += RenderPanel_Resize;
            RenderPanel.MouseWheel += RenderPanel_MouseWheel;
            dPropertyGrid = new DockPropertyGrid();
            dSceneTree = new DockSceneTree();
            dViewProperties = new DockViewProperties();
            dPropertyGrid.Show(dockPanel1, DockState.DockRight);
            dSceneTree.Show(dockPanel1, DockState.DockLeft);
            dSceneTree.SetEventHandler("AfterSelect", new TreeViewEventHandler(OnAfterSelect));
            dSceneTree.ExportFrameButton.Click += new EventHandler(ExportFrame_Click);
            dSceneTree.Export3DButton.Click += new EventHandler(Export3DButton_Click);
            dSceneTree.JumpToButton.Click += new EventHandler(JumpButton_Click);
            dSceneTree.DeleteButton.Click += new EventHandler(DeleteButton_Click);
            dSceneTree.DuplicateButton.Click += new EventHandler(DuplicateButton_Click);
            dSceneTree.SetEventHandler("AfterCheck", new TreeViewEventHandler(OutlinerAfterCheck));
            dSceneTree.SetKeyHandler("KeyUp", new KeyEventHandler(OnKeyUpDockedPanel));
            dSceneTree.SetKeyHandler("KeyDown", new KeyEventHandler(OnKeyDownDockedPanel));
            dSceneTree.LinkToActorButton.Click += new EventHandler(LinkToActor_Click);
            dPropertyGrid.KeyUp += new KeyEventHandler(OnKeyUpDockedPanel);
            dSceneTree.UpdateParent1Button.Click += new EventHandler(UpdateParent_Click);
            dSceneTree.UpdateParent2Button.Click += new EventHandler(UpdateParent_Click);
            dPropertyGrid.PropertyGrid.PropertyValueChanged += new PropertyValueChangedEventHandler(OnPropertyValueChanged);
            dPropertyGrid.OnObjectUpdated += ApplyEntryChanges;
        }

        private void RenderPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            decimal value = (e.Delta > 0 ? CameraSpeedTool.Increment : -CameraSpeedTool.Increment);
            CameraSpeedTool.Value += value;
        }

        private void RenderPanel_Resize(object sender, EventArgs e)
        {
            // TODO: Do we need to restructure the initialisation order for this form?

            // On some PCs this resized before graphics has initialised.
            if (Graphics != null)
            {
                Graphics.OnResize(RenderPanel.Width, RenderPanel.Height);
            }
        }
        
        private void LinkToActor_Click(object sender, EventArgs e)
        {
            var node = dSceneTree.SelectedNode;
            if (SceneData.Actors != null)
            {
                if (node != null)
                {
                    if (node.Tag != null)
                    {
                        FrameObjectFrame frame = (node.Tag as FrameObjectFrame);

                        NewObjectForm objectForm = new NewObjectForm(true);
                        objectForm.SetLabel("$SELECT_TYPE_AND_NAME");
                        ActorItemAddOption optionControl = new ActorItemAddOption();
                        objectForm.LoadOption(optionControl);

                        if (objectForm.ShowDialog() == DialogResult.OK)
                        {
                            //create the new entry
                            ActorTypes type = optionControl.GetSelectedType();
                            string def = optionControl.GetDefinitionName();
                            ActorEntry entry = SceneData.Actors[0].CreateActorEntry(type, objectForm.GetInputText());
                            entry.DefinitionName = def;
                            entry.FrameName = frame.Name.String;
                            entry.FrameNameHash = frame.Name.Hash;
                            frame.Item = entry;

                            //create the definition
                            ActorDefinition definition = SceneData.Actors[0].CreateActorDefinition(entry);
                            definition.FrameIndex = (uint)SceneData.FrameResource.FrameObjects.IndexOfValue(frame.RefID);

                            //create the node
                            TreeNode entityNode = new TreeNode("actor_" + entry.EntityName);
                            entityNode.Text = entry.EntityName;
                            entityNode.Tag = entry;

                            //now add the node to the scene tree
                            var typeString = string.Format("actorType_" + entry.ActorTypeName);
                            var foundnodes = actorRoot.Nodes[0].Nodes.Find(typeString, false);
                            if (foundnodes.Length > 0)
                            {
                                dSceneTree.AddToTree(entityNode, foundnodes[0]);
                            }
                            else
                            {
                                TreeNode typeNode = new TreeNode(typeString);
                                typeNode.Name = typeString;
                                typeNode.Text = entry.ActorTypeName;
                                typeNode.Nodes.Add(entityNode);
                                dSceneTree.AddToTree(typeNode, actorRoot.Nodes[0]);
                            }
                        }

                        objectForm.Dispose();
                    }
                }
            }
        }

        private void ExportFrame_Click(object sender, EventArgs e)
        {
            var node = dSceneTree.SelectedNode;

            FrameObjectBase frame = (node.Tag as FrameObjectBase);

            if (node != null)
            {
                if(node.Tag != null)
                {
                    SceneData.FrameResource.SaveFramesToFile(frame);
                }
            }        
        }

        private void UpdateAssetVisualisation(TreeNode node, TreeNode parent)
        {
            if (node.Tag != null)
            {
                bool isFrame = FrameResource.IsFrameType(node.Tag);

                int result = -1;
                int.TryParse(node.Name, out result);

                if(bHideChildren && (node != parent))
                {
                    node.Checked = parent.Checked;
                }

                int refID = (isFrame) ? (node.Tag as FrameEntry).RefID : result;
                if (Graphics.Assets.ContainsKey(refID))
                {
                    Graphics.Assets[refID].DoRender = node.Checked && node.CheckIfParentsAreValid();
                }
            }

            foreach (TreeNode child in node.Nodes)
            {
                UpdateAssetVisualisation(child, node);
            }
        }

        private void OutlinerAfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null)
            {
                dSceneTree.RemoveEventHandler("AfterCheck", new TreeViewEventHandler(OutlinerAfterCheck));
                UpdateAssetVisualisation(e.Node, e.Node);
                dSceneTree.SetEventHandler("AfterCheck", new TreeViewEventHandler(OutlinerAfterCheck));
            }
        }

        public void PopulateList()
        {
            TreeNode tree = SceneData.FrameResource.BuildTree(SceneData.FrameNameTable);
            tree.Tag = SceneData.FrameResource.Header;
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

            if (Graphics == null)
            {
                Graphics = new GraphicsClass();
                Graphics.PreInit(handle);
                BuildRenderObjects();
                result = Graphics.InitScene(RenderPanel.Width, RenderPanel.Height);
            }

            if (Input == null)
            {
                Input = Graphics.Input;
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
        private void UpdateParent_Click(object sender, EventArgs e)
        {
            string name = (sender as ToolStripMenuItem).Name;
            int parent = (name == "UpdateParent1Button" ? 0 : 1);
            ListWindow window = new ListWindow();
            window.PopulateForm(parent);
            
            if (window.ShowDialog() == DialogResult.OK)
            {
                FrameEntry obj = (window.chosenObject as FrameEntry);
                UpdateObjectParents(parent, obj.RefID, obj);
            }
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            SceneData.CleanData();
            RenderStorageSingleton.Instance.TextureCache.Clear();
            dSceneTree.Dispose();
            dPropertyGrid.Dispose();
            dViewProperties.Dispose();
            Shutdown();
        }

        private Vector3 MoveObjectWithMouse(float z, int sx, int sy)
        {
            Ray ray = Graphics.Camera.GetPickingRay(new Vector2(sx, sy), new Vector2(RenderPanel.Size.Width, RenderPanel.Size.Height));
            Vector3 worldPosition = ray.Position + (ray.Direction * 1);
            //Vector3 worldPosition = ray.Position + (ray.Direction*(ray.Position.Z + ray.Direction.Z / fObject.LocalTransform.TranslationVector.Z)); nefunguje, asi chyba v odcitani?

            for (int i = 0; i < 99999; i++)
            {
                worldPosition = ray.Position + (ray.Direction * i);
                if (worldPosition.Z - z < 10)
                {
                    break;
                }
            }

            return worldPosition;
        }

        public bool Frame()
        {
            bool bCameraUpdated = false;

            if (Input.IsKeyDown(Keys.Delete))
            {
                dSceneTree.DeleteButton.PerformClick();
            }

            if (RenderPanel.Focused)
            {
                if (Input.IsButtonDown(MouseButtons.Right))
                {
                    var dx = -0.25f * (mousePos.X - lastMousePos.X);
                    var dy = -0.25f * (mousePos.Y - lastMousePos.Y);
                    Graphics.RotateCamera(dx, dy);
                    bCameraUpdated = true;
                    
                }
                else if (Input.IsButtonDown(MouseButtons.Left) && selectTimer <= 0.0f)
                {
                    if (bSelectMode)
                    {
                        Pick(mousePos.X, mousePos.Y);
                        selectTimer = 1.0f;
                    }
                    else
                    {
                        if(dSceneTree.SelectedNode != null)
                        {
                            var node = dSceneTree.SelectedNode;
                            var tag = dSceneTree.SelectedNode.Tag;
                            
                            if(FrameResource.IsFrameType(tag))
                            {
                                FrameObjectBase fObject = (tag as FrameObjectBase);
                                var translation = MoveObjectWithMouse(fObject.LocalTransform.Translation.Z, mousePos.X, mousePos.Y);
                                var local = fObject.LocalTransform;
                                translation.Z = local.Translation.Z;
                                fObject.LocalTransform = MatrixUtils.SetTranslationVector(local, translation);
                                TreeViewUpdateSelected();
                                ApplyChangesToRenderable(fObject);
                            }
                            else if(tag is Collision.Placement)
                            {
                                Collision.Placement placement = (tag as Collision.Placement);
                                var translation = MoveObjectWithMouse(placement.Position.Z, mousePos.X, mousePos.Y);
                                var local = placement.Position;
                                translation.Z = local.Z;
                                placement.Position = translation;
                                TreeViewUpdateSelected();
                                IRenderer asset;
                                Graphics.Assets.TryGetValue(int.Parse(node.Name), out asset);
                                RenderInstance instance = (asset as RenderInstance);
                                instance.SetTransform(placement.Transform);
                            }
                        }
                        
                    }
                }

                bCameraUpdated = Graphics.UpdateInput();

                if (selectTimer > 0.0f)
                {
                    selectTimer -= 0.1f;
                }
            }

            lastMousePos = mousePos;
            Graphics.Frame();

            if (bCameraUpdated)
            {
                // Hack: We have to remove the delegate before we can change the values, 
                // or we'll fire some unnecessary code.
                PositionXTool.ValueChanged -= new EventHandler(CameraToolsOnValueChanged);
                PositionYTool.ValueChanged -= new EventHandler(CameraToolsOnValueChanged);
                PositionZTool.ValueChanged -= new EventHandler(CameraToolsOnValueChanged);
                PositionXTool.Value = (decimal)Graphics.Camera.Position.X;
                PositionYTool.Value = (decimal)Graphics.Camera.Position.Y;
                PositionZTool.Value = (decimal)Graphics.Camera.Position.Z;
                PositionXTool.ValueChanged += new EventHandler(CameraToolsOnValueChanged);
                PositionYTool.ValueChanged += new EventHandler(CameraToolsOnValueChanged);
                PositionZTool.ValueChanged += new EventHandler(CameraToolsOnValueChanged);
            }
            Process process = Process.GetCurrentProcess();
            Label_MemoryUsage.Text = string.Format("Usage: {0}", process.WorkingSet64.ConvertToMemorySize());
            Label_FPS.Text = Graphics.Profile.ToString();
            return true;
        }

        private void SanitizeBuffers()
        {
            #region vertex sanitize;
            //vertex pool check
            var bufferPools = new Dictionary<ulong, bool>();
            
            foreach(var pool in SceneData.VertexBufferPool.Buffers)
            {
                bufferPools.Add(pool.Key, false);
            }

            foreach(KeyValuePair<int, FrameGeometry> pair in SceneData.FrameResource.FrameGeometries)
            {
                foreach(var lod in pair.Value.LOD)
                {
                    if (bufferPools.ContainsKey(lod.VertexBufferRef.Hash))
                    {
                        bufferPools[lod.VertexBufferRef.Hash] = true;
                    }
                }
            }

            for(int i = 0; i < bufferPools.Count; i++)
            {
                KeyValuePair<ulong, bool> pair = bufferPools.ElementAt(i);
                if (!pair.Value)
                {
                    SceneData.VertexBufferPool.RemoveBuffer(pair.Key);
                    Console.WriteLine("Removed Vertex Buffer {0}", pair.Key);
                }
            }

            #endregion vertex sanitize;
            #region index sanitize;
            //index pool check
            bufferPools = new Dictionary<ulong, bool>();

            foreach (var pool in SceneData.IndexBufferPool.Buffers)
            {
                bufferPools.Add(pool.Key, false);
            }

            foreach (KeyValuePair<int, FrameGeometry> pair in SceneData.FrameResource.FrameGeometries)
            {
                foreach (var lod in pair.Value.LOD)
                {
                    if(bufferPools.ContainsKey(lod.IndexBufferRef.Hash))
                    {
                        bufferPools[lod.IndexBufferRef.Hash] = true;
                    }
                }
            }

            for (int i = 0; i < bufferPools.Count; i++)
            {
                KeyValuePair<ulong, bool> pair = bufferPools.ElementAt(i);
                if (!pair.Value)
                {
                    SceneData.IndexBufferPool.RemoveBuffer(pair.Key);
                    Console.WriteLine("Removed Index Buffer {0}", pair.Key);
                }
            }

            #endregion index sanitize;
        }

        private void Save()
        {
            DialogResult result = MessageBox.Show("Do you want to save your changes?", "Toolkit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
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
                SanitizeBuffers();
                SceneData.IndexBufferPool.WriteToFile();
                SceneData.VertexBufferPool.WriteToFile();

                if(SceneData.Actors != null && ToolkitSettings.Experimental)
                {
                    for(int i = 0; i < SceneData.Actors.Length; i++)
                    {
                        FixActorDefintions(SceneData.Actors[i]);
                        SceneData.Actors[i].WriteToFile();
                    }
                }

                if (SceneData.roadMap != null && ToolkitSettings.Experimental)
                {
                    List<SplineDefinition> splines = new List<SplineDefinition>();
                    List<JunctionDefinition> junctions = new List<JunctionDefinition>();

                    for (int i = 0; i != roadRoot.Nodes.Count; i++)
                    {
                        RenderRoad road = (RenderRoad)roadRoot.Nodes[i].Tag;
                        SplineDefinition spline = new SplineDefinition();
                        spline.NumSplines1 = spline.NumSplines2 = (ushort)road.Spline.Points.Length;
                        spline.Points = road.Spline.Points;
                        spline.HasToward = road.HasToward;
                        spline.HasBackward = road.HasBackward;
                        spline.Backward = road.Backward;
                        spline.Toward = road.Toward;
                        spline.IndexOffset = road.IndexOffset;
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
                        Collision.CollisionModel collisionModel = (node.Tag as Collision.CollisionModel);
                        collision.Models.Add(collisionModel.Hash, collisionModel);

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
                SceneData.UpdateResourceType();
                Cursor.Current = Cursors.Default;

                Console.WriteLine("Saved Changes Succesfully");
            }
        }

        private IRenderer BuildRenderObjectFromFrame(FrameObjectBase fObject)
        {
            fObject.ConstructRenderable();
            IRenderer Renderable = fObject.GetRenderItem();
            if(Renderable != null)
            {
                return Renderable;
            }

            return null;
        }

        private void BuildRenderObjects()
        {
            Dictionary<int, IRenderer> assets = new Dictionary<int, IRenderer>();

            if (SceneData.FrameResource != null && SceneData.FrameNameTable != null)
            {
                foreach(FrameObjectBase FrameObject in SceneData.FrameResource.FrameObjects.Values)
                {
                    IRenderer NewAsset = BuildRenderObjectFromFrame(FrameObject);
                    if(NewAsset != null)
                    {
                        assets.Add(FrameObject.RefID, NewAsset);
                    }
                }
            }
            //if (SceneData.Translokator != null && ToolkitSettings.Experimental)
            //{
            //    Graphics.SetTranslokatorGrid(SceneData.Translokator);
            //}

            if (SceneData.roadMap != null && ToolkitSettings.Experimental)
            {
                TreeNode node = new TreeNode("Road Data");
                TreeNode node2 = new TreeNode("Junction Data");
                node.Tag = node2.Tag = "Folder";
                roadRoot = node;             
                junctionRoot = node2;

                for (int i = 0; i != SceneData.roadMap.splines.Length; i++)
                {
                    RenderRoad road = new RenderRoad();
                    int generatedID = RefManager.GetNewRefID();
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
                    int generatedID = RefManager.GetNewRefID();
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
            if (SceneData.HPDData != null)
            {
                int generatedID = RefManager.GetNewRefID();
                TreeNode navNode = new TreeNode();
                navNode.Text = string.Format("HPD");
                navNode.Name = generatedID.ToString();

                for (int i = 0; i < SceneData.HPDData.unkData.Count; i++)
                {
                    generatedID = RefManager.GetNewRefID();
                    TreeNode hpdNode = new TreeNode();
                    hpdNode.Text = string.Format("NODE: {0}", i);
                    hpdNode.Name = generatedID.ToString();
                    var item = SceneData.HPDData.unkData[i];
                    RenderBoundingBox bbox = new RenderBoundingBox();
                    BoundingBox box = new BoundingBox(item.unk0, item.unk1);
                    bbox.Init(box);
                    assets.Add(generatedID, bbox);
                    hpdNode.Tag = box;
                    navNode.Nodes.Add(hpdNode);
                }
                dSceneTree.AddToTree(navNode);
            }
            if (SceneData.OBJData != null)
            {
                var data = new OBJData[SceneData.OBJData.Length];
                for (int i = 0; i < SceneData.OBJData.Length; i++)
                {
                    data[i] = (OBJData)SceneData.OBJData[i].data;
                }
                TreeNode Grids = Graphics.SetNavigationGrid(data);
                dSceneTree.AddToTree(Grids);
                //for (int i = 0; i < SceneData.OBJData.Length; i++)
                //{
                //    int generatedID = StringHelpers.GetNewRefID();
                //    TreeNode navNode = new TreeNode();
                //    navNode.Text = string.Format("NAV: {0}", i);
                //    navNode.Name = generatedID.ToString();
                //    var obj = (SceneData.OBJData[i].data as OBJData);

                //    for (int x = 0; x < obj.vertices.Length; x++)
                //    {
                //        var childID = StringHelpers.GetNewRefID();
                //        RenderNav navigationPoints = new RenderNav();
                //        navigationPoints.Init(obj, x);
                //        assets.Add(childID, navigationPoints);

                //        TreeNode childNode = new TreeNode();
                //        childNode.Text = string.Format("NAVNode: {0}", x);
                //        childNode.Name = childID.ToString();
                //        childNode.Tag = navigationPoints;
                //        navNode.Nodes.Add(childNode);
                //    }

                //    for (int y = 0; y < obj.connections.Length; y++)
                //    {
                //        RenderLine navigationLine = new RenderLine();
                //        navigationLine.SetUnselectedColour(System.Drawing.Color.AliceBlue);
                //        navigationLine.Init(new Vector3[2] { obj.vertices[obj.connections[y].NodeID].Position, obj.vertices[obj.connections[y].ConnectedNodeID].Position });
                //        navigationLine.SetSelectedColour(System.Drawing.Color.AliceBlue);
                //        navigationLine.SetUnselectedColour(System.Drawing.Color.AliceBlue);
                //        assets.Add(StringHelpers.GetNewRefID(), navigationLine);
                //    }

                //    dSceneTree.AddToTree(navNode);
                //}
            }
            if (SceneData.Collisions != null)
            {
                TreeNode node = new TreeNode("Collision Data");
                node.Tag = "Folder";
                collisionRoot = node;

                for (int i = 0; i != SceneData.Collisions.Models.Count; i++)
                {
                    Collision.CollisionModel data = SceneData.Collisions.Models.ElementAt(i).Value;
                    RenderStaticCollision collision = new RenderStaticCollision();
                    collision.ConvertCollisionToRender(data.Mesh);
                    RenderStorageSingleton.Instance.StaticCollisions.Add(SceneData.Collisions.Models.ElementAt(i).Key, collision);
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
                        int refID = RefManager.GetNewRefID();
                        RenderInstance instance = new RenderInstance();
                        instance.Init(RenderStorageSingleton.Instance.StaticCollisions[placement.Hash]);
                        instance.SetTransform(placement.Transform);
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
            if(SceneData.ATLoader != null && ToolkitSettings.Experimental)
            {
                animalTrafficRoot = new TreeNode("Animal Traffic Paths");
                animalTrafficRoot.Tag = "Folder";
                for (int i = 0; i < SceneData.ATLoader.Paths.Length; i++)
                {
                    int refID = RefManager.GetNewRefID();
                    RenderATP atp = new RenderATP();
                    atp.Init(SceneData.ATLoader.Paths[i]);
                    TreeNode child = new TreeNode();
                    child.Text = animalTrafficRoot.Nodes.Count.ToString();
                    child.Name = refID.ToString();
                    child.Tag = atp;
                    assets.Add(refID, atp);
                    animalTrafficRoot.Nodes.Add(child);
                }
                dSceneTree.AddToTree(animalTrafficRoot);
            }
            if (SceneData.Actors.Length > 0 && ToolkitSettings.Experimental)
            {
                actorRoot = new TreeNode("Actor Items");
                actorRoot.Tag = "Folder";
                for (int z = 0; z < SceneData.Actors.Length; z++)
                {
                    Actor actor = SceneData.Actors[z];
                    TreeNode actorFile = new TreeNode("Actor File " + z);
                    actorFile.Tag = "Folder";
                    actorRoot.Nodes.Add(actorFile);
                    for (int c = 0; c != actor.Items.Count; c++)
                    {
                        var item = actor.Items[c];
                        TreeNode itemNode = new TreeNode("actor_" + z + "-" + c);
                        itemNode.Text = item.EntityName;
                        itemNode.Tag = item;

                        var typeString = string.Format("actorType_" + item.ActorTypeName);
                        var foundnodes = actorFile.Nodes.Find(typeString, false);
                        if(foundnodes.Length > 0)
                        {
                            foundnodes[0].Nodes.Add(itemNode);
                        }
                        else
                        {
                            TreeNode typeNode = new TreeNode(typeString);
                            typeNode.Name = typeString;
                            typeNode.Text = item.ActorTypeName;
                            typeNode.Nodes.Add(itemNode);
                            actorFile.Nodes.Add(typeNode);
                        }
                    }
                    FixActorDefintions(actor);
                }
                dSceneTree.AddToTree(actorRoot);
            }
            for(int i = 0; i < SceneData.FrameNameTable.FrameData.Length; i++)
            {
                FrameNameTable.Data data = SceneData.FrameNameTable.FrameData[i];
                if (data.FrameIndex != -1)
                {
                    FrameObjectBase frame = (SceneData.FrameResource.FrameObjects.ElementAt(data.FrameIndex).Value as FrameObjectBase);
                    if (frame != null)
                    {
                        frame.FrameNameTableFlags = data.Flags;
                        frame.IsOnFrameTable = true;
                    }
                }
            }

            foreach (var pair in SceneData.FrameResource.FrameObjects)
            {
                FrameObjectBase frame = (pair.Value as FrameObjectBase);
                if (assets.ContainsKey(frame.RefID))
                {
                    assets[frame.RefID].SetTransform(frame.WorldTransform);
                }
            }

            Graphics.InitObjectStack = assets;
        }

        private void TreeViewUpdateSelected()
        {
            var node = dSceneTree.SelectedNode;
            if (node.Tag == null)
            {
                return;
            }

            if (FrameResource.IsFrameType(node.Tag))
            {
                Graphics.SelectEntry((node.Tag as FrameEntry).RefID);
            }
            else if(node.Tag is SpatialCell)
            {
                SpatialGrid grid = (node.Parent.Tag as SpatialGrid);
                grid.SetSelectedCell(node.Index);
            }
            else
            {
                int result = 0;
                if(int.TryParse(node.Name, out result))
                {
                    Graphics.SelectEntry(result);
                }
                
            }

            dPropertyGrid.SetObject(node.Tag);
        }

        private void FixActorDefintions(Actor actor)
        {
            List<int> frameIndexes = new List<int>();
            List<FrameObjectFrame> frames = new List<FrameObjectFrame>();
            for (int x = 0; x < SceneData.FrameResource.FrameObjects.Count; x++)
            {
                FrameObjectFrame frame = (SceneData.FrameResource.FrameObjects.ElementAt(x).Value as FrameObjectFrame);

                if (frame != null)
                {
                    frames.Add(frame);
                    frameIndexes.Add(x);
                }
            }

            for (int i = 0; i != actor.Definitions.Count; i++)
            {
                FrameObjectFrame frame = null;
                ActorDefinition definition = actor.Definitions[i];
                
                for (int c = 0; c != actor.Items.Count; c++)
                {
                    ActorEntry item = actor.Items[c];
                    if (definition.FrameNameHash == item.FrameNameHash)
                    {
                        bool sorted = false;
                        for (int x = 0; x < frames.Count; x++)
                        {
                            FrameObjectFrame nFrame = frames[x];
                            if (nFrame.Name.Hash == item.FrameNameHash)
                            {
                                if (!nFrame.ActorHash.String.Equals(item.DefinitionName))
                                {
                                    Console.WriteLine("ActorHash and Definition Do NotMatch");
                                }
                                definition.FrameIndex = (uint)frameIndexes[x];
                                frame = nFrame;
                                frame.Item = actor.Items[c];
                                frame.LocalTransform = MatrixUtils.SetMatrix(actor.Items[c].Rotation, actor.Items[c].Scale, actor.Items[c].Position);
                                sorted = true;
                            }
                        }

                        Debug.Assert(sorted, "Error: Did not detect the frame accompanying this actor " + item.EntityName + "; This means it will probably cause errors in game. Check your actors in the toolkit!");
                    }
                }
            }

            frames.Clear();
            frameIndexes.Clear();
        }

        private void ApplyEntryChanges(object sender, EventArgs e)
        {
            if (dPropertyGrid.IsEntryReady)
            {
                TreeNode selected = dSceneTree.SelectedNode;
                if (selected.Tag is FrameObjectBase)
                {
                    FrameObjectBase fObject = (selected.Tag as FrameObjectBase);
                    selected.Text = fObject.ToString();
                    dPropertyGrid.UpdateObject();
                    ApplyChangesToRenderable(fObject);
                }
                else if (selected.Tag is FrameHeaderScene)
                {
                    FrameHeaderScene scene = (selected.Tag as FrameHeaderScene);
                    selected.Text = scene.ToString();
                }
                else if (selected.Tag is Collision.Placement)
                {
                    dPropertyGrid.UpdateObject();
                    Collision.Placement placement = (selected.Tag as Collision.Placement);
                    IRenderer asset;
                    Graphics.Assets.TryGetValue(int.Parse(selected.Name), out asset);
                    RenderInstance instance = (asset as RenderInstance);
                    instance.SetTransform(placement.Transform);

                    // Send an event to update our selected item. (if this is indeed our selected)
                    UpdateSelectedEventArgs Arguments = new UpdateSelectedEventArgs();
                    Arguments.RefID = int.Parse(selected.Name);
                    Graphics.OnSelectedObjectUpdated(this, Arguments);
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
                bbox.SetTransform(area.WorldTransform);
                bbox.Update(area.Bounds);
            }
            else if(obj is FrameObjectDummy)
            {
                FrameObjectDummy dummy = (obj as FrameObjectDummy);
                RenderBoundingBox bbox = (Graphics.Assets[obj.RefID] as RenderBoundingBox);
                bbox.SetTransform(dummy.WorldTransform);
                bbox.Update(dummy.Bounds);
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
                model.SetTransform(mesh.WorldTransform);
                model.UpdateMaterials(mesh.Material);
            }

            foreach(var child in obj.Children)
            {
                ApplyChangesToRenderable(child);
            }

            // Send an event to update our selected item. (if this is indeed our selected)
            UpdateSelectedEventArgs Arguments = new UpdateSelectedEventArgs();
            Arguments.RefID = obj.RefID;
            Graphics.OnSelectedObjectUpdated(this, Arguments);
        }

        private Model LoadModelFromFile()
        {
            Model model = new Model();

            if (MeshBrowser.ShowDialog() == DialogResult.Cancel)
            {
                return null;
            }

            if (MeshBrowser.FileName.ToLower().EndsWith(".fbx"))
            {
                if (!model.ModelStructure.ReadFromFbx(MeshBrowser.FileName))
                {
                    return null;
                }
            }
            else if (MeshBrowser.FileName.ToLower().EndsWith(".m2t"))
            {
                using (BinaryReader reader = new BinaryReader(File.Open(MeshBrowser.FileName, FileMode.Open)))
                {
                    model.ModelStructure.ReadFromM2T(reader);
                }
            }

            for (int i = 0; i < model.ModelStructure.Lods.Length; i++)
            {
                var lod = model.ModelStructure.Lods[i];
                var is32bit = model.ModelStructure.Lods[i].Over16BitLimit();
                FrameResourceModelOptions modelForm = new FrameResourceModelOptions(lod.VertexDeclaration, i, is32bit);
                if (modelForm.ShowDialog() != DialogResult.OK)
                {
                    return null;
                }
                var options = modelForm.Options;
                modelForm.Dispose();
                lod.VertexDeclaration = VertexFlags.Position;
                lod.VertexDeclaration |= (options["NORMALS"] == true ? VertexFlags.Normals : 0);
                lod.VertexDeclaration |= (options["TANGENTS"] == true ? VertexFlags.Tangent : 0);
                lod.VertexDeclaration |= (options["DIFFUSE"] == true ? VertexFlags.TexCoords0 : 0);
                lod.VertexDeclaration |= (options["UV1"] == true ? VertexFlags.TexCoords1 : 0);
                lod.VertexDeclaration |= (options["UV2"] == true ? VertexFlags.TexCoords2 : 0);
                lod.VertexDeclaration |= (options["AO"] == true ? VertexFlags.ShadowTexture : 0);
                lod.VertexDeclaration |= (options["COLOR0"] == true ? VertexFlags.Color : 0);
                lod.VertexDeclaration |= (options["COLOR1"] == true ? VertexFlags.Color1 : 0);
            }

            return model;
        }

        private void CreateMeshBuffers(Model model)
        {
            for (int i = 0; i < model.FrameGeometry.NumLods; i++)
            {
               bool bAdded = SceneData.VertexBufferPool.TryAddBuffer(model.VertexBuffers[i]);
               bAdded = SceneData.IndexBufferPool.TryAddBuffer(model.IndexBuffers[i]);
            }
        }

        private FrameObjectBase CreateSingleMesh(Model model)
        {
            // The model is invalid; we should not continue;
            // TODO:: Ideally we should move this check somewhere else..
            if(model == null)
            {
                return null;

            }

            // Create a new SM and assign the frame mesh onto the model for future frame construction.
            FrameObjectSingleMesh sm = new FrameObjectSingleMesh();
            model.FrameMesh = sm;

            Debug.Assert(sm != null && model != null, "Failed to load model from file!");

            sm.Name.Set(model.ModelStructure.Name);
            model.CreateObjectsFromModel();
            sm.AddRef(FrameEntryRefTypes.Geometry, model.FrameGeometry.RefID);
            sm.Geometry = model.FrameGeometry;
            sm.AddRef(FrameEntryRefTypes.Material, model.FrameMaterial.RefID);
            sm.Material = model.FrameMaterial;
            SceneData.FrameResource.FrameMaterials.Add(model.FrameMaterial.RefID, model.FrameMaterial);
            SceneData.FrameResource.FrameGeometries.Add(model.FrameGeometry.RefID, model.FrameGeometry);

            sm.LocalTransform = Matrix4x4.Identity;
            sm.WorldTransform = Matrix4x4.Identity;

            CreateMeshBuffers(model);
            return sm;
        }

        private FrameObjectBase CreateSkinnedMesh(Model model)
        {
            // We use the single mesh and convert to skinned and replace the old data on the model
            FrameObjectSingleMesh sm = (CreateSingleMesh(model) as FrameObjectSingleMesh);
            FrameObjectModel rigged = new FrameObjectModel(sm);
            model.FrameMesh = sm;
            model.FrameModel = rigged;

            rigged.AddRef(FrameEntryRefTypes.BlendInfo, model.BlendInfoBlock.RefID);
            rigged.BlendInfo = model.BlendInfoBlock;
            rigged.AddRef(FrameEntryRefTypes.Skeleton, model.SkeletonBlock.RefID);
            rigged.Skeleton = model.SkeletonBlock;
            rigged.AddRef(FrameEntryRefTypes.SkeletonHierachy, model.SkeletonHierachyBlock.RefID);
            rigged.SkeletonHierarchy = model.SkeletonHierachyBlock;

            SceneData.FrameResource.FrameBlendInfos.Add(rigged.BlendInfo.RefID, rigged.BlendInfo);
            SceneData.FrameResource.FrameSkeletons.Add(rigged.Skeleton.RefID, rigged.Skeleton);
            SceneData.FrameResource.FrameSkeletonHierachies.Add(rigged.SkeletonHierarchy.RefID, rigged.SkeletonHierarchy);

            return rigged;
        }

        private void CreateNewEntry(int selected, string name, bool addToNameTable)
        {
            FrameObjectBase frame;

            switch (selected)
            {
                case 0:
                    frame = CreateSingleMesh(LoadModelFromFile());
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
                    frame = CreateSkinnedMesh(LoadModelFromFile());
                    break;
                case 11:
                    frame = new FrameObjectCollision();
                    break;
                case 12:
                    frame = new FrameObjectJoint();
                    break;
                default:
                    frame = new FrameObjectBase();
                    Console.WriteLine("Unknown type selected");
                    break;
            }

            // Frame was not valid, there is no need to carry on.
            if (frame == null)
            {
                return;
            }

            Debug.Assert(frame != null, "Frame was null!");

            frame.Name.Set(name);
            frame.IsOnFrameTable = addToNameTable;
            SceneData.FrameResource.FrameObjects.Add(frame.RefID, frame);
            TreeNode node = new TreeNode(frame.Name.String);
            node.Tag = frame;
            node.Name = frame.RefID.ToString();
            dSceneTree.AddToTree(node, frameResourceRoot);

            IRenderer renderer = BuildRenderObjectFromFrame(frame);
            if(renderer != null)
            {
                Graphics.InitObjectStack.Add(frame.RefID, renderer);
            }
        }

        private void Pick(int sx, int sy)
        {
            float lowest = float.MaxValue;
            int lowestRefID = -1;

            Ray ray = Graphics.Camera.GetPickingRay(new Vector2(sx, sy), new Vector2(RenderPanel.Size.Width, RenderPanel.Size.Height));
            int index = 0;
            foreach (KeyValuePair<int, IRenderer> model in Graphics.Assets)
            {
                if (!model.Value.DoRender)
                {
                    continue;
                }

                Matrix4x4 vWM = Matrix4x4.Identity;
                Matrix4x4.Invert(model.Value.Transform, out vWM);
                var localRay = new Ray(
                    Vector3Utils.TransformCoordinate(ray.Position, vWM),
                    Vector3.TransformNormal(ray.Direction, vWM)
                );
                if (model.Value is RenderModel)
                {
                    RenderModel mesh = (model.Value as RenderModel);
                    var bbox = mesh.BoundingBox;

                    if (localRay.Intersects(bbox) == 0.0f) continue;

                    for (var i = 0; i < mesh.LODs[0].Indices.Length / 3; i++)
                    {
                        var v0 = mesh.LODs[0].Vertices[mesh.LODs[0].Indices[i * 3]].Position;
                        var v1 = mesh.LODs[0].Vertices[mesh.LODs[0].Indices[i * 3 + 1]].Position;
                        var v2 = mesh.LODs[0].Vertices[mesh.LODs[0].Indices[i * 3 + 2]].Position;

                        float t = 0.0f;
                        if (!Toolkit.Mathematics.Collision.RayIntersectsTriangle(ref localRay, ref v0, ref v1, ref v2, out t)) continue;

                        if (t < 0.0f || float.IsNaN(t))
                        {
                            if (SceneData.FrameResource.FrameObjects.ContainsKey(model.Key))
                            {
                                var frame = (SceneData.FrameResource.FrameObjects[model.Key] as FrameObjectBase);
                                Log.WriteLine(string.Format("The toolkit has failed to analyse a model: {0} {1}", frame.Name, t));
                            }
                        }

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
                    var bbox = collision.BoundingBox;

                    if (localRay.Intersects(bbox) == 0.0f) continue;

                    for (var i = 0; i < collision.Indices.Length / 3; i++)
                    {
                        var v0 = collision.Vertices[collision.Indices[i * 3]].Position;
                        var v1 = collision.Vertices[collision.Indices[i * 3 + 1]].Position;
                        var v2 = collision.Vertices[collision.Indices[i * 3 + 2]].Position;

                        float t = 0.0f;                
                        if (!Toolkit.Mathematics.Collision.RayIntersectsTriangle(ref localRay, ref v0, ref v1, ref v2, out t)) continue;

                        var worldPosition = ray.Position + t * ray.Direction;
                        var distance = (worldPosition - ray.Position).LengthSquared();

                        if (distance < lowest)
                        {
                            lowest = distance;
                            lowestRefID = model.Key;
                        }
                    }
                }
                index++;
            }
            TreeNode[] nodes = dSceneTree.Find(lowestRefID.ToString(), true);

            if (nodes.Length > 0)
            {
                dSceneTree.SelectedNode = nodes[0];
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

        private void UpdateObjectParentsRecurse(TreeNode parent, FrameObjectBase entry)
        {
            foreach (var child in entry.Children)
            {
                TreeNode childNode = new TreeNode();
                childNode.Tag = child;
                childNode.Name = child.RefID.ToString();
                childNode.Text = child.ToString();
                parent.Nodes.Add(childNode);
                UpdateObjectParentsRecurse(childNode, child);
            }
        }

        private void UpdateObjectParents(int parent, int refID, FrameEntry entry = null)
        {
            FrameObjectBase obj = (dSceneTree.SelectedNode.Tag as FrameObjectBase);
            //make sure refID is not root.
            if (refID != 0)
            {
                //make sure entry is not null.
                if (entry == null)
                {
                    TreeNode[] objs = dSceneTree.Find(refID.ToString(), true);

                    if (objs.Length > 0)
                    {
                        entry = (objs[0].Tag as FrameEntry);
                    }
                }

                SceneData.FrameResource.SetParentOfObject(parent, obj, entry);
            }
            else
            {
                SceneData.FrameResource.SetParentOfObject(parent, obj, null);
            }

            dSceneTree.RemoveNode(dSceneTree.SelectedNode);
            TreeNode newNode = new TreeNode();
            newNode.Tag = obj;
            newNode.Name = obj.RefID.ToString();
            newNode.Text = obj.ToString();
            UpdateObjectParentsRecurse(newNode, obj);

            TreeNode[] nodes = null;
            if (obj.ParentIndex1.Index != -1)
            {
                nodes = dSceneTree.Find(obj.ParentIndex1.RefID.ToString(), true);

                if (nodes.Length > 0)
                {
                    dSceneTree.AddToTree(newNode, nodes[0]);
                }
            }
            else if (obj.ParentIndex2.Index != -1)
            {
                nodes = dSceneTree.Find(obj.ParentIndex2.RefID.ToString(), true);

                if (nodes.Length > 0)
                {
                    dSceneTree.AddToTree(newNode, nodes[0]);
                }
            }
            else
            {
                dSceneTree.AddToTree(newNode, frameResourceRoot);
            }

            dSceneTree.SelectedNode = newNode;
            ApplyChangesToRenderable(obj);
        }

        private void OnPropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            PropertyGrid pGrid = (s as PropertyGrid);
            if (pGrid.SelectedObject is FrameObjectBase)
            {
                FrameObjectBase obj = (dSceneTree.SelectedNode.Tag as FrameObjectBase);

                if (dSceneTree.SelectedNode.Tag == pGrid.SelectedObject)
                {
                    TreeNode selected = dSceneTree.SelectedNode;
                    selected.Text = (pGrid.SelectedObject as FrameObjectBase).Name.ToString();
                }
                if(e.ChangedItem.Label == "Index")
                {
                    //used just incase the user wants to set the parent to "root"
                    if ((int)e.ChangedItem.Value == -1)
                    {
                        if (e.ChangedItem.Parent.Label == "ParentIndex1")
                        {
                            obj.ParentIndex1.Index = -1;
                            obj.ParentIndex1.Name = "";
                            obj.ParentIndex1.RefID = 0;
                            obj.SubRef(FrameEntryRefTypes.Parent1);
                        }
                        else if (e.ChangedItem.Parent.Label == "ParentIndex2")
                        {
                            obj.ParentIndex2.Index = -1;
                            obj.ParentIndex2.Name = "";
                            obj.ParentIndex2.RefID = 0;
                            obj.SubRef(FrameEntryRefTypes.Parent2);
                        }
                    }
                }
                else if (e.ChangedItem.Label == "RefID")
                {
                    //used just incase the user wants to set the parent to "root"
                    int parent = (e.ChangedItem.Parent.Label == "ParentIndex1" ? 0 : 1);
                    UpdateObjectParents(parent, (int)e.ChangedItem.Value);
                }
                
                ApplyChangesToRenderable((FrameObjectBase)pGrid.SelectedObject);
            }
            if (pGrid.SelectedObject is RenderRoad)
            {
                RenderRoad road = (pGrid.SelectedObject as RenderRoad);
                road.Spline.UpdateVertices();
            }
            if (pGrid.SelectedObject is RenderJunction)
            {
                RenderJunction junction = (pGrid.SelectedObject as RenderJunction);
                junction.UpdateVertices();
            }
            if (pGrid.SelectedObject is ActorEntry)
            {
                if (dSceneTree.SelectedNode.Tag == pGrid.SelectedObject)
                {
                    TreeNode selected = dSceneTree.SelectedNode;
                    selected.Text = (pGrid.SelectedObject as ActorEntry).EntityName.ToString();
                }
            }
            if (pGrid.SelectedObject is FrameHeaderScene)
            {
                if (dSceneTree.SelectedNode.Tag == pGrid.SelectedObject)
                {
                    TreeNode selected = dSceneTree.SelectedNode;
                    selected.Text = (pGrid.SelectedObject as FrameHeaderScene).Name.ToString();
                }
            }
            pGrid.Refresh();
        }

        private void CameraSpeedUpdate(object sender, EventArgs e)
        {
            UpdateCameraSpeed();
        }

        private void UpdateCameraSpeed()
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

        private void DeleteFrames(TreeNode node)
        {
            for (int i = 0; i < node.Nodes.Count; i++)
            {
                if (FrameResource.IsFrameType(node.Nodes[i].Tag))
                {
                    FrameEntry entry = node.Nodes[i].Tag as FrameEntry;
                    bool bDidRemove = SceneData.FrameResource.DeleteFrame(entry);
                    Graphics.Assets.TryRemove(entry.RefID);
                    DeleteFrames(node.Nodes[i]);

                    Debug.Assert(bDidRemove == true, "Failed to remove!");
                }
            }
        }
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            TreeNode node = dSceneTree.SelectedNode;

            if (FrameResource.IsFrameType(node.Tag))
            {
                FrameEntry obj = node.Tag as FrameEntry;

                if (obj != null)
                {
                    dSceneTree.RemoveNode(node);
                    Graphics.Assets.Remove(obj.RefID);
                    bool bDidRemove = SceneData.FrameResource.DeleteFrame(obj);
                    Graphics.Assets.TryRemove(obj.RefID);

                    Debug.Assert(bDidRemove == true, "Failed to remove!");
                }

                DeleteFrames(node);
            }
            else if(node.Tag.GetType() == typeof(FrameHeaderScene))
            {
                var scene = (node.Tag as FrameHeaderScene);
                dSceneTree.RemoveNode(node);
                SceneData.FrameResource.FrameScenes.Remove(scene.RefID);
                DeleteFrames(node);
            }
            else if (node.Tag.GetType() == typeof(Collision.Placement))
            {
                dSceneTree.RemoveNode(node);

                int iName = Convert.ToInt32(node.Name);
                Graphics.Assets.TryRemove(iName);
            }
            else if (node.Tag.GetType() == typeof(RenderRoad))
            {
                dSceneTree.RemoveNode(node);
                Graphics.Assets.TryRemove(int.Parse(node.Name));
            }
            else if (node.Tag.GetType() == typeof(RenderJunction))
            {
                dSceneTree.RemoveNode(node);
                Graphics.Assets.TryRemove(int.Parse(node.Name));
            }
            else if (node.Tag.GetType() == typeof(Collision.CollisionModel))
            {
                dSceneTree.RemoveNode(node);

                Collision.CollisionModel data = (node.Tag as Collision.CollisionModel);
                SceneData.Collisions.RemoveModel(data);
                RenderStorageSingleton.Instance.StaticCollisions.TryRemove(data.Hash);

                for (int i = 0; i != node.Nodes.Count; i++)
                {
                    int iName = Convert.ToInt32(node.Nodes[i].Name);
                    Graphics.Assets.TryRemove(iName);
                }
            }
        }

        private void DuplicateButton_Click(object sender, EventArgs e)
        {
            TreeNode node = dSceneTree.SelectedNode;
            FrameObjectBase newEntry = null;

            //new safety net
            if (FrameResource.IsFrameType(node.Tag))
            {
                if (node.Tag is FrameObjectSingleMesh || node.Tag is FrameObjectModel)
                {
                    DialogResult result = MessageBox.Show("$DUPLICATE_MATERIAL_BLOCK", "Toolkit", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                    //we don't want to duplicate anymore
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }
                }

                //is this even needed? hmm.
                if (node.Tag.GetType() == typeof(FrameObjectArea))
                {
                    newEntry = new FrameObjectArea((FrameObjectArea)node.Tag);
                    FrameObjectArea area = (newEntry as FrameObjectArea);
                    RenderBoundingBox RenderBox = RenderableFactory.BuildBoundingBox(area.Bounds, area.WorldTransform);
                    Graphics.InitObjectStack.Add(area.RefID, RenderBox);
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
                    RenderBoundingBox RenderBox = RenderableFactory.BuildBoundingBox(dummy.Bounds, dummy.WorldTransform);
                    Graphics.InitObjectStack.Add(dummy.RefID, RenderBox);
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
                    FrameObjectModel mesh = (newEntry as FrameObjectModel);
                    SceneData.FrameResource.DuplicateBlocks(mesh);
                    RenderModel model = RenderableFactory.BuildRenderModelFromFrame(mesh);
                    Graphics.InitObjectStack.Add(mesh.RefID, model);
                }
                else if (node.Tag.GetType() == typeof(FrameObjectSector))
                {
                    newEntry = new FrameObjectSector((FrameObjectSector)node.Tag);
                    FrameObjectSector sector = (newEntry as FrameObjectSector);
                    RenderBoundingBox RenderBox = RenderableFactory.BuildBoundingBox(sector.Bounds, sector.WorldTransform);
                    Graphics.InitObjectStack.Add(sector.RefID, RenderBox);
                }
                else if (node.Tag.GetType() == typeof(FrameObjectSingleMesh))
                {
                    newEntry = new FrameObjectSingleMesh((FrameObjectSingleMesh)node.Tag);
                    FrameObjectSingleMesh mesh = (newEntry as FrameObjectSingleMesh);
                    SceneData.FrameResource.DuplicateBlocks(mesh);
                    RenderModel model = RenderableFactory.BuildRenderModelFromFrame(mesh);
                    Graphics.InitObjectStack.Add(mesh.RefID, model);
                }
                else if (node.Tag.GetType() == typeof(FrameObjectTarget))
                    newEntry = new FrameObjectTarget((FrameObjectTarget)node.Tag);
                else
                    newEntry = new FrameObjectBase((FrameObjectBase)node.Tag);

                // Try and add the numeric value to the end of the name.
                // Either increment on the numeric value or add it.
                string FrameName = newEntry.Name.String;
                int LastIndex = FrameName.LastIndexOf('_');
                bool bIsValid = false;
                if (LastIndex != -1)
                {
                    int NumericValue = 0;
                    string NameSplit = FrameName.Substring(LastIndex).Remove(0, 1);
                    string LeftSplit = FrameName.Substring(0, LastIndex);
                    bool bHasNumericValue = int.TryParse(NameSplit, out NumericValue);

                    if (bHasNumericValue)
                    {
                        NumericValue = CheckIfDuplicationContainsString(LeftSplit);
                        string NumericValueStringed = string.Format("_{0}", NumericValue);
                        newEntry.Name.Set(LeftSplit + NumericValueStringed);
                        bIsValid = true;
                    }
                }

                if(!bIsValid)
                {
                    int NewNumericValue = CheckIfDuplicationContainsString(FrameName);
                    string NumericString = string.Format("_{0}", NewNumericValue);
                    newEntry.Name.Set(newEntry.Name.String + NumericString);
                }

                TreeNode tNode = new TreeNode(newEntry.ToString());
                tNode.Tag = newEntry;
                tNode.Name = newEntry.RefID.ToString();
                //fix for objects with -1 on root.
                if (newEntry.ParentIndex2.Index == -1)
                    dSceneTree.AddToTree(tNode, frameResourceRoot);
                else
                    dSceneTree.AddToTree(tNode, dSceneTree.Find(newEntry.ParentIndex2.RefID.ToString(), true)[0]);
                SceneData.FrameResource.FrameObjects.Add(newEntry.RefID, newEntry);
                dSceneTree.SelectedNode = tNode;
            }
            else if (node.Tag.GetType() == typeof(Collision.Placement))
            {
                Collision.Placement placement = new Collision.Placement((Collision.Placement)node.Tag);

                int pIdxName = 0;
                int.TryParse(node.Text, out pIdxName);
                pIdxName++;

                int refID = RefManager.GetNewRefID();
                TreeNode child = new TreeNode();
                child.Text = pIdxName.ToString();
                child.Name = refID.ToString();
                child.Tag = placement;
                dSceneTree.AddToTree(child, node.Parent);

                RenderInstance instance = new RenderInstance();
                instance.Init(RenderStorageSingleton.Instance.StaticCollisions[placement.Hash]);
                instance.SetTransform(placement.Transform);
                Graphics.InitObjectStack.Add(refID, instance);
            }
        }

        private int CheckIfDuplicationContainsString(string Key)
        {
            int NewNumericValue = 0;
            if (NamesAndDuplicationStore.ContainsKey(Key))
            {
                NewNumericValue = ++NamesAndDuplicationStore[Key];
            }
            else
            {
                NamesAndDuplicationStore.Add(Key, NewNumericValue);
            }

            return NewNumericValue;
        }

        private void Export3DButton_Click(object sender, EventArgs e)
        {
            if (dSceneTree.SelectedNode.Tag.GetType() == typeof(Collision.CollisionModel))
                ExportCollision(dSceneTree.SelectedNode.Tag as Collision.CollisionModel);
            else
                Export3DFrame();
        }

        private void ExportCollision(Collision.CollisionModel data)
        {
            M2TStructure structure = new M2TStructure();
            structure.BuildCollision(data, dSceneTree.SelectedNode.Name);
            structure.ExportCollisionToM2T(ToolkitSettings.ExportPath, data.Hash.ToString());

            if (ToolkitSettings.Format != 2)
            {
                structure.ExportToFbx(ToolkitSettings.ExportPath, false);
            }
        }
        private void Export3DFrame()
        {
            var tag = dSceneTree.SelectedNode.Tag;
            FrameObjectSingleMesh model;
            if(tag is FrameObjectSingleMesh)
            {
                model = (tag as FrameObjectSingleMesh);
            }
            else
            {
                //enter message here
                return;
            }

            IndexBuffer[] indexBuffers = new IndexBuffer[model.Geometry.LOD.Length];
            VertexBuffer[] vertexBuffers = new VertexBuffer[model.Geometry.LOD.Length];

            //we need to retrieve buffers first.
            for (int c = 0; c != model.Geometry.LOD.Length; c++)
            {
                indexBuffers[c] = SceneData.IndexBufferPool.GetBuffer(model.Geometry.LOD[c].IndexBufferRef.Hash);
                vertexBuffers[c] = SceneData.VertexBufferPool.GetBuffer(model.Geometry.LOD[c].VertexBufferRef.Hash);
            }

            Model newModel = null;
            if (tag is FrameObjectModel)
            {
                newModel = new Model(tag as FrameObjectModel, indexBuffers, vertexBuffers);
            }
            else
            {
                newModel = new Model(tag as FrameObjectSingleMesh, indexBuffers, vertexBuffers);
            }

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
            form.LoadOption(new ControlOptionFrameAdd());

            if(form.ShowDialog() == DialogResult.OK)
            {
                ControlOptionFrameAdd window = (form.control as ControlOptionFrameAdd);
                int selection = window.GetSelectedType();
                CreateNewEntry(selection, form.GetInputText(), window.GetAddToNameTable());
            }
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
            spline.Points = new Vector3[2] { new Vector3(0, 0, 0), new Vector3(10, 10, 10) };
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


            int generatedID = RefManager.GetNewRefID();
            RenderStorageSingleton.Instance.SplineStorage.Add(spline);
            Graphics.InitObjectStack.Add(generatedID, road);
            int nodeID = (roadRoot.Nodes.Count);
            TreeNode child = new TreeNode(nodeID.ToString());
            child.Text = "Road ID: " + nodeID;
            child.Name = generatedID.ToString();
            child.Tag = road;
            dSceneTree.AddToTree(child, roadRoot);
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


            int generatedID = RefManager.GetNewRefID();
            RenderStorageSingleton.Instance.SplineStorage.Add(spline);
            Graphics.InitObjectStack.Add(generatedID, road);
            int nodeID = (roadRoot.Nodes.Count);
            TreeNode child = new TreeNode(nodeID.ToString());
            child.Text = "Road ID: " + nodeID;
            child.Name = generatedID.ToString();
            child.Tag = road;
            dSceneTree.AddToTree(child, roadRoot);
        }

        private void AddJunctionOnClick(object sender, EventArgs e)
        {
            if (SceneData.roadMap == null)
                return;

            JunctionDefinition definition = new JunctionDefinition();
            RenderJunction junction = new RenderJunction();
            definition.JunctionIDX = junctionRoot.Nodes.Count;
            junction.Init(definition);

            int generatedID = RefManager.GetNewRefID();
            Graphics.InitObjectStack.Add(generatedID, junction);
            int nodeID = (junctionRoot.Nodes.Count);
            TreeNode child = new TreeNode(nodeID.ToString());
            child.Text = "Junction ID: " + nodeID;
            child.Name = generatedID.ToString();
            child.Tag = junction;
            dSceneTree.AddToTree(child, junctionRoot);
        }

        private void EditUnkSet3Click(object sender, EventArgs e)
        {
            if (SceneData.roadMap != null)
                dPropertyGrid.SetObject(SceneData.roadMap);
        }

        private void AddTowardClick(object sender, EventArgs e)
        {
            if (SceneData.roadMap != null && dSceneTree.SelectedNode != null)
            {
                if (dSceneTree.SelectedNode.Tag.GetType() == typeof(RenderRoad))
                {
                    RenderRoad road = (dSceneTree.SelectedNode.Tag as RenderRoad);
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
            if (SceneData.roadMap != null && dSceneTree.SelectedNode != null)
            {
                if (dSceneTree.SelectedNode.Tag.GetType() == typeof(RenderRoad))
                {
                    RenderRoad road = (dSceneTree.SelectedNode.Tag as RenderRoad);
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
            // Check if we need to create a collisions folder
            if (SceneData.Collisions == null)
            {
                DialogResult result = MessageBox.Show(Language.GetString("$NO_COL_FILE_CREATE_NEW"), "Toolkit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    SceneData.Collisions = new Collision();
                    SceneData.Collisions.Name = Path.Combine(SceneData.ScenePath, "Collisions_0.col");
                    TreeNode node = new TreeNode("Collision Data");
                    node.Tag = "Folder";
                    collisionRoot = node;
                    dSceneTree.AddToTree(node);
                    collisionRoot.Collapse(false);
                }
                else
                {
                    MessageBox.Show(Language.GetString("$CANNOT_CREATE_COL"), "Toolkit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    return;
                }
            }

            // Try and select a model
            if (MeshBrowser.ShowDialog() != DialogResult.OK)
            {
                MessageBox.Show("Failed to select model.", "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Read M2T file
            M2TStructure m2tColModel = new M2TStructure();
            if (MeshBrowser.FileName.ToLower().EndsWith(".m2t"))
            {
                using (BinaryReader reader = new BinaryReader(File.Open(MeshBrowser.FileName, FileMode.Open)))
                {
                    m2tColModel.ReadFromM2T(reader);
                }
            }
            else if (MeshBrowser.FileName.ToLower().EndsWith(".fbx"))
            {
                m2tColModel.ReadFromFbx(MeshBrowser.FileName);
            }

            // If we have no LODs, crash happened.
            if (m2tColModel.Lods[0] == null)
            {
                MessageBox.Show("Failed to load model! No LOD[0] is present.", "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ulong CollisionHash = FNV64.Hash(m2tColModel.Name);
            Collision.CollisionModel CollisionModel = null;
            if(!SceneData.Collisions.Models.ContainsKey(CollisionHash))
            {
                // Create a new renderable for collision object
                Collision.CollisionModel collisionModel = new CollisionModelBuilder().BuildFromM2TStructure(m2tColModel);
                RenderStaticCollision collision = new RenderStaticCollision();
                collision.ConvertCollisionToRender(collisionModel.Mesh);
                RenderStorageSingleton.Instance.StaticCollisions.TryAdd(collisionModel.Hash, collision);

                // Push it onto the collisions dictionary
                SceneData.Collisions.Models.Add(collisionModel.Hash, collisionModel);
                CollisionModel = collisionModel;

                // Create a new TreeNode for the CollisionModel
                TreeNode CollisionNode = new TreeNode(CollisionHash.ToString());
                CollisionNode.Text = CollisionHash.ToString();
                CollisionNode.Name = CollisionHash.ToString();
                CollisionNode.Tag = collisionModel;
                dSceneTree.AddToTree(CollisionNode, collisionRoot);
            }
            else
            {
                // Get the model if it exists
                CollisionModel = SceneData.Collisions.Models[CollisionHash];
            }

            // Create a new placement for this mesh
            Collision.Placement placement = new Collision.Placement();
            placement.Hash = CollisionHash;

            // Try and find the collision node
            TreeNode ExistingCollisionNode = dSceneTree.GetTreeNode(CollisionHash.ToString(), collisionRoot, true);
            if (ExistingCollisionNode == null)
            {
                // Create a new TreeNode for the CollisionModel
                TreeNode CollisionNode = new TreeNode(CollisionHash.ToString());
                CollisionNode.Text = CollisionHash.ToString();
                CollisionNode.Name = CollisionHash.ToString();
                CollisionNode.Tag = CollisionModel;
                ExistingCollisionNode = CollisionNode;
                dSceneTree.AddToTree(CollisionNode, collisionRoot);
            }

            // Add new Placement object
            int refID = RefManager.GetNewRefID();
            TreeNode child = new TreeNode();
            child.Text = ExistingCollisionNode.Nodes.Count.ToString();
            child.Name = refID.ToString();
            child.Tag = placement;
            dSceneTree.AddToTree(child, ExistingCollisionNode);

            // Complete it
            RenderInstance instance = new RenderInstance();
            instance.Init(RenderStorageSingleton.Instance.StaticCollisions[placement.Hash]);
            instance.SetTransform(placement.Transform);
            Graphics.InitObjectStack.Add(refID, instance);
            SceneData.Collisions.Placements.Add(placement);
        }

        private void CameraToolsOnValueChanged(object sender, EventArgs e)
        {
            Graphics.Camera.Position = new Vector3(Convert.ToSingle(PositionXTool.Value), Convert.ToSingle(PositionYTool.Value), Convert.ToSingle(PositionZTool.Value));
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

        private void OnKeyUpDockedPanel(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                bHideChildren = false;
            }
            else if (e.KeyCode == Keys.Delete)
            {
                dSceneTree.DeleteButton.PerformClick();
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (dSceneTree.SelectedNode.PrevVisibleNode != null)
                {
                    dSceneTree.SelectedNode = dSceneTree.SelectedNode.PrevVisibleNode;
                }
            }
            else if (e.KeyCode == Keys.Down)
            {
                if (dSceneTree.SelectedNode.NextVisibleNode != null)
                {
                    dSceneTree.SelectedNode = dSceneTree.SelectedNode.NextVisibleNode;
                }
            }

            e.Handled = true;
        }

        private void OnKeyDownDockedPanel(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.ControlKey)
            {
                bHideChildren = true;
            }

            e.Handled = true;
        }

        private void OnViewSide2ButtonClicked(object sender, EventArgs e)
        {

        }

        private void SwitchMode(bool isSelectMode)
        {
            bSelectMode = isSelectMode;
            CurrentModeButton.Text = (bSelectMode) ? "Select Mode" : "Edit Mode";
        }

        private void EditLighting_Click(object sender, EventArgs e)
        {
            dPropertyGrid.SetObject(Graphics.WorldSettings);
        }

        private void Button_TestConvert_Click(object sender, EventArgs e)
        {
            ConvertBuffer(1);
        }

        private void Button_TestConvert32_Click(object sender, EventArgs e)
        {
            ConvertBuffer(2);
        }

        private void ConvertBuffer(int format)
        {
            var frames = SceneData.FrameResource.FrameObjects;
            var geoms = SceneData.FrameResource.FrameGeometries;
            var mats = SceneData.FrameResource.FrameMaterials;
            var indexbuffer = SceneData.IndexBufferPool.Buffers;

            foreach (var geom in geoms)
            {
                foreach (var lod in geom.Value.LOD)
                {
                    lod.SplitInfo.IndexStride = (format == 1 ? 2 : 4);
                }
            }

            foreach (var buffer in indexbuffer)
            {
                buffer.Value.SetFormat(format);
            }

            Save();
        }

        private void ConvertFrameToRender(FrameObjectBase parent)
        {
            IRenderer asset = BuildRenderObjectFromFrame(parent);
            if(asset != null)
            {
                Graphics.InitObjectStack.TryAdd(parent.RefID, asset);
            }
        }

        private void ConvertNodeToFrame(TreeNode node)
        {
            ConvertFrameToRender((node.Tag as FrameObjectBase));

            foreach (TreeNode child in node.Nodes)
            {
                ConvertFrameToRender((child.Tag as FrameObjectBase));
                ConvertNodeToFrame(child);
            }
        }

        private void Button_ImportFrame_OnClicked(object sender, EventArgs e)
        {
            if (FrameBrowser.ShowDialog() == DialogResult.OK)
            {
                string Filename = FrameBrowser.FileName;
                TreeNode parent = SceneData.FrameResource.ReadFramesFromFile(Filename);
                dSceneTree.AddToTree(parent, frameResourceRoot);
                ConvertNodeToFrame(parent);
            }
        }

        private void Button_DumpTexture_Click(object sender, EventArgs e)
        {
            List<string> AllTextures = new List<string>();

            // Get header scene name
            string HeaderSceneName = SceneData.FrameResource.Header.SceneName.String;
            if (!string.IsNullOrEmpty(HeaderSceneName))
            {
                if(!AllTextures.Contains(HeaderSceneName))
                {
                    AllTextures.Add(HeaderSceneName);
                }
            }

            // Iterate through FrameObjects
            foreach(var Frame in SceneData.FrameResource.FrameObjects)
            {
                // We can only take textures from SingleMesh
                var SingleMesh = (Frame.Value as FrameObjectSingleMesh);
                if (SingleMesh != null)
                {
                    // Store OM texture
                    if(!AllTextures.Contains(SingleMesh.OMTextureHash.String))
                    {
                        AllTextures.Add(SingleMesh.OMTextureHash.String);
                    }              

                    // Collect textures from FrameMaterial object.
                    List<string> CollectedTextures = SingleMesh.Material.CollectAllTextureNames();
                    if (CollectedTextures != null)
                    {
                        foreach(var Texture in CollectedTextures)
                        {
                            if(!AllTextures.Contains(Texture))
                            {
                                AllTextures.Add(Texture);
                            }
                        }
                    }
                }
            }

            File.WriteAllLines("AllTextures.txt", AllTextures.ToArray());
        }
    }
}

