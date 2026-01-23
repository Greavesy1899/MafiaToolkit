using Forms.Docking;
using Forms.EditorControls;
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
using ResourceTypes.ModelHelpers.ModelExporter;
using ResourceTypes.Navigation;
using ResourceTypes.Navigation.Traffic;
using SharpGLTF.Schema2;
using ResourceTypes.Translokator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Windows.Controls.Primitives;
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
using static ResourceTypes.Collisions.Collision;
using static UnluacNET.TableLiteral;
using Collision = ResourceTypes.Collisions.Collision;
using Object = ResourceTypes.Translokator.Object;

namespace Mafia2Tool
{
    public partial class MapEditor : Form
    {
        private SceneData SceneData = new SceneData();
        private SceneData ImportedScene;
        private InputClass Input { get; set; }
        private GraphicsClass Graphics { get; set; }

        private Point mousePos;
        private Point lastMousePos;
        private FileInfo fileLocation;

        //docking panels
        private DockPropertyGrid dPropertyGrid;
        private DockSceneTree dSceneTree;
        private DockImportSceneTree dImportSceneTree;
        private DockViewProperties dViewProperties;

        //parent nodes for data
        private TreeNode frameResourceRoot;
        private TreeNode importFRRoot;
        private TreeNode collisionRoot;
        private TreeNode roadRoot;
        private TreeNode junctionRoot;
        private TreeNode animalTrafficRoot;
        private TreeNode actorRoot;
        private TreeNode AIWorldRoot;
        private TreeNode OBJDataRoot;
        private TreeNode translokatorRoot;

        private MouseButtons dragButton;
        
        private bool bSelectMode = false;
        private float selectTimer = 0.0f;
        private bool bHideChildren = false;

        private Dictionary<string, int> NamesAndDuplicationStore;

        public MapEditor(FileInfo info,SceneData sceneData)
        {
            SceneData = sceneData;
            TextureLoader.ScenePath = SceneData.ScenePath;
            InitializeComponent();
            Localise();

            sceneData.FrameResource.OnFrameRemoved += OnFrameRemoved;

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
            ViewSideButton.Text = Language.GetString("$SIDE");
            ViewBottomButton.Text = Language.GetString("$BOTTOM");
            ViewSide2Button.Text = Language.GetString("$SIDE 2");
            OptionsButton.Text = Language.GetString("$OPTIONS");
            ToggleWireframeButton.Text = Language.GetString("$TOGGLE_WIREFRAME");
            ToggleCullingButton.Text = Language.GetString("$TOGGLE_CULLING");
            EditLighting.Text = Language.GetString("$EDIT_LIGHTING");
            ToggleTranslokatorTint.Text = Language.GetString("$TOGGLE_TRANSLOKATOR_TINT");
            Button_TestConvert32.Text = Language.GetString("$TEST_CONVERT_32BIT");
            Button_TestConvert16.Text = Language.GetString("$TEST_CONVERT_16BIT");
            Button_DumpTexture.Text = Language.GetString("$DUMP_TEXTURES");
            SceneTreeButton.Text = Language.GetString("$VIEW_SCENE_TREE");
            ObjectPropertiesButton.Text = Language.GetString("$VIEW_PROPERTY_GRID");
            WindowButton.Text = Language.GetString("$WINDOWS");
            ViewOptionProperties.Text = Language.GetString("$VIEW_UTILITIES");
            AddButton.Text = Language.GetString("$ADD");
            Button_ImportFrame.Text = Language.GetString("$IMPORT_FRAME");
            Button_ImportBundle.Text = Language.GetString("$IMPORT_BUNDLE");
            AddSceneFolderButton.Text = Language.GetString("$ADD_SCENE_FOLDER");
            SaveButton.Text = Language.GetString("$SAVE");
            ExitButton.Text = Language.GetString("$EXIT");
            CurrentModeButton.Text = Language.GetString("$CurrentModeLabel");
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
            dImportSceneTree = new DockImportSceneTree();
            dPropertyGrid.Show(dockPanel1, DockState.DockRight);
            dSceneTree.Show(dockPanel1, DockState.DockLeft);
            dSceneTree.Select();
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
            dSceneTree.TreeViewNodeDropped += OnTreeViewNodeDropped;
            dPropertyGrid.PropertyGrid.PropertyValueChanged += new PropertyValueChangedEventHandler(OnPropertyValueChanged);
            dPropertyGrid.OnObjectUpdated += ApplyEntryChanges;
            dSceneTree.TranslokatorNewInstanceButton.Click += new EventHandler(TranslokatorNewInstanceButton_Click);
            dSceneTree.ActorEntryNewTRObjectButton.Click += new EventHandler(ActorEntryNewTRObjectButton_Click);
            dSceneTree.TRRebuildObjectButton.Click += new EventHandler(TRRebuildObjectButton_Click);
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
        
        private void OnTreeViewNodeDropped(object sender, TreeViewDragEventArgs e)
        {
            if (e.DraggedNode.Tag is FrameHeaderScene || e.DraggedNode.Tag is FrameHeader)
            {
                return;
            }

            if (e.DraggedNode.Tag is not FrameObjectBase)
            {
                return;
            }
            if (e.DragButton == MouseButtons.Left)
            {
                FrameEntry NewParent = (e.TargetNode.Tag != null ? e.TargetNode.Tag as FrameEntry : null);
                int ParentRefID = (NewParent != null ? NewParent.RefID : -1);

                // Request parent1 update
                UpdateObjectParents(ParentInfo.ParentType.ParentIndex1, ParentRefID, NewParent);
            }
            else if (e.DragButton == MouseButtons.Right)
            {
                FrameEntry NewParent = (e.TargetNode.Tag != null ? e.TargetNode.Tag as FrameEntry : null);
                int ParentRefID = (NewParent != null ? NewParent.RefID : -1);

                // Request parent2 update
                UpdateObjectParents(ParentInfo.ParentType.ParentIndex2, ParentRefID, NewParent);
            }
            else if (e.DragButton == MouseButtons.Middle)
            {
                TreeNode node1 = (e.TargetNode != null ? e.TargetNode : null);
                TreeNode node2 = (e.DraggedNode != null ? e.DraggedNode: null);

                // frame switch
                SwitchFrames(node1, node2);
            }
        }
        
        // TODO: The fetching of the actor file should be inside SceneData,
        // or whatever I can the Multi-SDS class later on.
        private void LinkToActor_Click(object sender, EventArgs e)
        {
            var node = dSceneTree.SelectedNode;
            if(node == null)
            {
                // Not selecting a node
                return;
            }

            if(node.Tag == null)
            {
                // Doesn't have any valid data
                return;
            }

            if(SceneData.Actors != null)
            {
                // Actors array is invalid
                SceneData.Actors = new Actor[0];
                SceneData.CreateNewActor();

                LoadActorFiles();
            }

            // Should have atleast one file, try to link actors.
            if(SceneData.Actors.Length > 0 && SceneData.Actors[0] != null)
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
                    frame.ActorHash.Set(definition.Name);

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

        private void ExportFrame_Click(object sender, EventArgs e)
        {
            var node = dSceneTree.SelectedNode;
            if (node.Tag.GetType() == typeof(FrameHeaderScene) || node.Tag.GetType() == typeof(FrameHeader))//this should catch scenes and frameresource content
            {
                //todo manage exporting scenes, skip frameheader
                return;
            }

            FrameObjectBase frame = (node.Tag as FrameObjectBase);

            if (node != null)
            {
                if(node.Tag != null)
                {
                    if (SaveFileDialog != null) 
                    { 
                        SaveFileDialog.Reset(); 
                    }
                    string ExportName = null;
                    SaveFileDialog.FileName = frame.Name.String;
                    SaveFileDialog.RestoreDirectory = true;
                    SaveFileDialog.Filter = "FrameData File (*.framedata)|*.framedata*";
                    SaveFileDialog.FilterIndex = 1;
                    SaveFileDialog.DefaultExt = "framedata";

                    if (SaveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        ExportName = SaveFileDialog.FileName;
                    }
                    else
                    {
                        return;
                    }

                    SceneData.FrameResource.SaveFramesToFile(ExportName, frame);
                }
            }        
        }

        private void UpdateAssetVisualisation(TreeNode node, TreeNode parent)
        {
            if (node.Tag != null)
            {
                bool bIsFrame = FrameResource.IsFrameType(node.Tag);

                int result = -1;
                int.TryParse(node.Name, out result);

                if(bHideChildren && (node != parent))
                {
                    node.Checked = parent.Checked;
                }

                // Update rendered counterpart
                int refID = (bIsFrame) ? (node.Tag as FrameEntry).RefID : result;

                if (!bIsFrame)
                {
                    if (node.Tag is Instance && node.Parent.Tag is Object trObject)
                    {
                        UpdateInstanceVisualisation(node, trObject, node.Checked && node.CheckIfParentsAreValid());
                    }
                    else if (node.Tag is Grid trGrid)
                    {
                        bool enabled = node.Checked && node.CheckIfParentsAreValid();

                        if (enabled)
                        {
                            RebuildTranslokatorGrids();
                        }
                        else
                        {
                            int trGridIndex = Array.IndexOf(SceneData.Translokator.Grids, trGrid);
                            Graphics.SetTranslokatorGridEnabled(trGridIndex, enabled);
                        }
                    }
                    else
                    {
                        Graphics.SetAssetVisibility(refID, node.Checked && node.CheckIfParentsAreValid());
                    }
                }
                else
                {
                    Graphics.SetAssetVisibility(refID, node.Checked && node.CheckIfParentsAreValid());                
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
        
        public void PopulateImportedData(string ImportedFilename)
        {
            ImportedScene = new SceneData();
            ImportedScene.ScenePath = Path.GetDirectoryName(ImportedFilename);
            ImportedScene.BuildData(false);
            InitImportTree();
        }

        public void InitImportTree()
        {
            TreeNode Importedtree = ImportedScene.FrameResource.BuildTree(ImportedScene.FrameNameTable);
            Importedtree.Tag = ImportedScene.FrameResource.Header;
            importFRRoot = Importedtree;
            dImportSceneTree = new DockImportSceneTree(ImportedScene.ScenePath);
            dImportSceneTree.importButton.Click += new EventHandler(ImportButton_Click);
            dImportSceneTree.FormClosed += new FormClosedEventHandler(CancelButton_Click);
            dImportSceneTree.AddToTree(importFRRoot);
            dImportSceneTree.Owner = this;
            dImportSceneTree.Show();
            Button_ImportFrame.Enabled = false;//limiting users to one instance at a time
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
        private void CurrentModeButton_ButtonClick(object sender, EventArgs e) => SwitchMode(!bSelectMode);
        private void ViewOptionProperties_Click(object sender, EventArgs e) => dViewProperties.Show(dockPanel1, DockState.DockRight);

        private void UpdateParent_Click(object sender, EventArgs e)
        {
            string name = (sender as ToolStripMenuItem).Name;
            ParentInfo.ParentType ParentType = (name == "UpdateParent1Button" ? ParentInfo.ParentType.ParentIndex1 : ParentInfo.ParentType.ParentIndex2);
            ListWindow window = new ListWindow();
            window.PopulateForm(ParentType,SceneData.FrameResource);
            
            if (window.ShowDialog() == DialogResult.OK)
            {
                FrameEntry NewParent = (window.chosenObject != null ? window.chosenObject as FrameEntry : null);
                int ParentRefID = (NewParent != null ? NewParent.RefID : -1);

                // Request parent update
                UpdateObjectParents(ParentType, ParentRefID, NewParent);
            }
        }

        private void SwitchFrames(TreeNode node1, TreeNode node2)
        {
            if (node1 != null && node2 != null &&
                node1.Tag is FrameObjectBase frame1 &&
                node2.Tag is FrameObjectBase frame2)
            {
                if (frame1.Parent != null && frame2.Parent != null && frame1.Parent.RefID == frame2.Parent.RefID)
                {
                    return;//switching objects under same parent is redundant
                }

                var tempRefs = frame1.Refs;
                var tempParent1 = frame1.ParentIndex1;
                var tempParent2 = frame1.ParentIndex2;
                var tempParent = frame1.Parent;
                
                frame1.ParentIndex1 = frame2.ParentIndex1;
                frame1.ParentIndex2 = frame2.ParentIndex2;
                frame1.Parent = frame2.Parent;
                frame1.Refs = frame2.Refs;
                
                frame2.ParentIndex1 = tempParent1;
                frame2.ParentIndex2 = tempParent2;
                frame2.Parent = tempParent;
                frame2.Refs = tempRefs;

                int tempIcon = node1.ImageIndex;
                int tempIconSelect = node1.SelectedImageIndex;
                node1.Tag = frame2;
                node1.Name = frame2.RefID.ToString();
                node1.Text = frame2.ToString();
                node1.ImageIndex = node2.ImageIndex;
                node1.SelectedImageIndex = node2.SelectedImageIndex;

                node2.Tag = frame1;
                node2.Name = frame1.RefID.ToString();
                node2.Text = frame1.ToString();
                node2.ImageIndex = tempIcon;
                node2.SelectedImageIndex = tempIconSelect;
                
                TreeNode parent1 = node1.Parent;
                TreeNode parent2 = node2.Parent;
                
                if (parent1 != null && parent2 != null)
                {
                    int index1 = node1.Index;
                    int index2 = node2.Index;
                    
                    parent1.Nodes.RemoveAt(index1);
                    parent2.Nodes.RemoveAt(index2);
                    
                    parent1.Nodes.Insert(index2, node1);
                    parent2.Nodes.Insert(index1, node2);
                }

                ApplyChangesToRenderable(frame1);
                ApplyChangesToRenderable(frame2);
            }
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            SceneData.CleanData();
            RenderStorageSingleton.Instance.TextureCache.Clear();
            dSceneTree.Dispose();
            dImportSceneTree.Dispose();
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

                                // Update transform of instance
                                IRenderer Asset = Graphics.GetAsset(int.Parse(node.Name));
                                if (Asset != null)
                                {
                                    RenderInstance Instance = (Asset as RenderInstance);
                                    Instance.SetTransform(placement.Transform);
                                }
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
                UpdatePositionElement(Graphics.Camera.Position);
            }

            Process process = Process.GetCurrentProcess();
            Label_MemoryUsage.Text = string.Format("Usage: {0}", process.WorkingSet64.ConvertToMemorySize());
            Label_FPS.Text = Graphics.Profile.ToString();
            Label_StatusBar.Text = Graphics.GetStatusBarText();
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
                    // save code 
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

                if (SceneData.Translokator != null && ToolkitSettings.Experimental)
                    {
                        TranslokatorLoader translokator = SceneData.Translokator;
                           translokator.Grids = new Grid[translokatorRoot.Nodes[1].GetNodeCount(false)];
                           for (int i = 0; i < translokator.Grids.Length; i++)
                           {
                               Grid grid = (translokatorRoot.Nodes[1].Nodes[i].Tag as Grid);
                               translokator.Grids[i] = grid;
                           }

                           translokator.ObjectGroups = new ObjectGroup[translokatorRoot.Nodes[0].GetNodeCount(false)];
                           for (int i = 0; i < translokator.ObjectGroups.Length; i++)
                           {
                               ObjectGroup objectGroup = (translokatorRoot.Nodes[0].Nodes[i].Tag as ObjectGroup);
                               objectGroup.Objects = new ResourceTypes.Translokator.Object[translokatorRoot.Nodes[0].Nodes[i].GetNodeCount(false)];
                               for (int y = 0; y < objectGroup.Objects.Length; y++)
                               {
                                   ResourceTypes.Translokator.Object obj = (translokatorRoot.Nodes[0].Nodes[i].Nodes[y].Tag as ResourceTypes.Translokator.Object);
                                   obj.Instances = new Instance[translokatorRoot.Nodes[0].Nodes[i].Nodes[y].GetNodeCount(false)];
                                   for (int z = 0; z < obj.Instances.Length; z++)
                                   {
                                       Instance instance = (translokatorRoot.Nodes[0].Nodes[i].Nodes[y].Nodes[z].Tag as Instance);
                                       obj.Instances[z] = instance;
                                   }
                                   objectGroup.Objects[y] = obj;
                               }

                               translokator.ObjectGroups[i] = objectGroup;
                           }
                           translokator.WriteToFile(new FileInfo(SceneData.sdsContent.GetResourceFiles("Translokator", true)[0]));
                }
                SceneData.UpdateResourceType();
                Cursor.Current = Cursors.Default;

                Console.WriteLine("Saved Changes Succesfully");
            }
        }

        private IRenderer BuildRenderObjectFromFrame(FrameObjectBase fObject,Dictionary<int, IRenderer> assets)
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
                    IRenderer NewAsset = BuildRenderObjectFromFrame(FrameObject,assets);
                    if(NewAsset != null)
                    {
                        assets.Add(FrameObject.RefID, NewAsset);
                    }
                }
            }
            if (SceneData.roadMap != null && ToolkitSettings.Experimental)
            {
                TreeNode node = new TreeNode("Road Data");
                TreeNode node2 = new TreeNode("Junction Data");
                node.Tag = node2.Tag = "Folder";
                roadRoot = node;
                junctionRoot = node2;

                for (int i = 0; i < SceneData.roadMap.Roads.Count; i++)
                {
                    IRoadDefinition RoadDef = SceneData.roadMap.Roads[i];
                    if(RoadDef.Direction == RoadDirection.Backwards)
                    {
                        continue;
                    }

                    IRoadSpline RoadSpline = SceneData.roadMap.Splines[RoadDef.RoadSplineIndex];
                    RenderRoad road = new RenderRoad();
                    int generatedID = RefManager.GetNewRefID();
                    road.Init(RoadDef, RoadSpline);
                    assets.Add(generatedID, road);

                    TreeNode child = new TreeNode(i.ToString());
                    child.Text = "Road ID: " + i;
                    child.Name = generatedID.ToString();
                    child.Tag = road;
                    node.Nodes.Add(child);
                }

                for (int i = 0; i < SceneData.roadMap.Crossroads.Count; i++)
                {
                    int generatedID = RefManager.GetNewRefID();
                    RenderJunction junction = new RenderJunction();
                    junction.Init(SceneData.roadMap.Crossroads[i], Graphics);
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

                for (int i = 0; i < SceneData.HPDData.HPDEntries.Length; i++)
                {
                    generatedID = RefManager.GetNewRefID();
                    TreeNode hpdNode = new TreeNode();
                    hpdNode.Text = string.Format("NODE: {0}", i);
                    hpdNode.Name = generatedID.ToString();
                    var item = SceneData.HPDData.HPDEntries[i];
                    RenderBoundingBox bbox = new RenderBoundingBox();
                    BoundingBox box = new BoundingBox(item.BBoxMin, item.BBoxMax);
                    bbox.Init(box);
                    assets.Add(generatedID, bbox);
                    hpdNode.Tag = box;
                    navNode.Nodes.Add(hpdNode);
                }
                dSceneTree.AddToTree(navNode);
            }
            if (SceneData.OBJData != null && SceneData.OBJData.Length > 0)
            {
                OBJDataRoot = new TreeNode();
                OBJDataRoot.Tag = "Folder";
                OBJDataRoot.Name = OBJDataRoot.Text = "Navigation: OBJDATA";

                var data = new OBJData[SceneData.OBJData.Length];
                for (int i = 0; i < SceneData.OBJData.Length; i++)
                {
                    data[i] = (OBJData)SceneData.OBJData[i].Data;
                }

                TreeNode Grids = Graphics.SetNavigationGrid(data);
                OBJDataRoot.Nodes.Add(Grids);

                for (int i = 0; i < SceneData.OBJData.Length; i++)
                {
                    var obj = (SceneData.OBJData[i].Data as OBJData);
                    RenderNav navigationPoints = new RenderNav(Graphics);
                    navigationPoints.Init(obj);

                    TreeNode navNode = new TreeNode();
                    navNode.Text = string.Format("NAV: {0}", i);
                    navNode.Name = "NAV_OBJ_DATA";
                    navNode.Tag = navigationPoints;

                    for (int x = 0; x < obj.vertices.Length; x++)
                    {
                        TreeNode childNode = new TreeNode();
                        childNode.Text = string.Format("NAVNode: {0}", obj.vertices[x].Unk7);
                        childNode.Name = "NAV_INDEXED_NODE";
                        childNode.Tag = obj.vertices[x];
                        navNode.Nodes.Add(childNode);
                    }

                    OBJDataRoot.Nodes.Add(navNode);
                }

                dSceneTree.AddToTree(OBJDataRoot);
            }
            if (SceneData.AIWorlds != null && SceneData.AIWorlds.Length > 0)
            {
                AIWorldRoot = new TreeNode();
                AIWorldRoot.Tag = "Folder";
                AIWorldRoot.Name = AIWorldRoot.Text = "Navigation: AIWORLD";

                var data = new AIWorld[SceneData.AIWorlds.Length];
                for (int i = 0; i < SceneData.AIWorlds.Length; i++)
                {
                    data[i] = (AIWorld)SceneData.AIWorlds[i].Data;
                    data[i].ConstructRenderable(Graphics);

                    TreeNode AIWorldNode = data[i].PopulateTreeNode();
                    AIWorldRoot.Nodes.Add(AIWorldNode);
                }

                dSceneTree.AddToTree(AIWorldRoot);
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
                    collision.ConvertCollisionToRender(data.Hash, data.Mesh);
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
            if(SceneData.ATLoader != null)
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
                LoadActorFiles();
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

            if (SceneData.Translokator != null && ToolkitSettings.Experimental)
            {
                ToggleTranslokatorTint.Enabled = true;
                dSceneTree.hasTranslokatorData = true;
                translokatorRoot = new TreeNode("Translokator Items");
                translokatorRoot.Tag = "Folder";
                TreeNode ogNode = new TreeNode("Objects Groups");
                ogNode.Tag = "Folder";
                for (int z = 0; z < SceneData.Translokator.ObjectGroups.Length; z++)
                {
                    ObjectGroup objectGroup = SceneData.Translokator.ObjectGroups[z];
                    TreeNode objectGroupNode = new TreeNode(String.Format("Object Group: [{0}]", objectGroup.ActorType));
                    objectGroupNode.Tag = objectGroup;
                    for (int y = 0; y < objectGroup.Objects.Length; y++)
                    {
                        Object obj = objectGroup.Objects[y];
                        TreeNode objNode = new TreeNode(obj.Name.ToString());
                        objNode.Tag = obj;
                        objectGroupNode.Nodes.Add(objNode);
                        FrameObjectBase groupRef = SceneData.FrameResource.GetObjectByHash<FrameObjectBase>(obj.Name.Hash);

                        bool hasMesh = false;

                        if (groupRef != null)
                        {
                            hasMesh = groupRef.HasMeshObject();
                        }

                        for (int x = 0; x < obj.Instances.Length; x++)
                        {
                            Instance instance = obj.Instances[x];
                            instance.RefID = RefManager.GetNewRefID();

                            if (groupRef != null && hasMesh)
                            {
                                for (int i = 0; i < groupRef.Children.Count; i++)//i dont think this for cycle is needed really if done right
                                {
                                    InstanceTranslokatorPart(assets, groupRef.Children[i], Matrix4x4.Identity, instance);
                                }
                            }
                            else
                            {
                                Graphics.InstanceGizmo.InstanceTranslokator(instance);
                            }

                            TreeNode instanceNode = new TreeNode(obj.Name + " " + x);
                            instanceNode.Tag = instance;
                            instanceNode.Name = instance.RefID.ToString();
                            objNode.Nodes.Add( instanceNode);
                        }
                    }
                    ogNode.Nodes.Add(objectGroupNode);
                }

                translokatorRoot.Nodes.Add(ogNode);

                TreeNode gridNode = new TreeNode("Grids");
                gridNode.Tag = "Folder";
                for (int i = 0; i < SceneData.Translokator.Grids.Length; i++)
                {
                    Grid grid = SceneData.Translokator.Grids[i];
                    TreeNode child = new TreeNode("Grid " + i);
                    child.Tag = grid;
                    child.Checked = false;
                    gridNode.Nodes.Add(child);
                }

                translokatorRoot.Nodes.Add(gridNode);

                dSceneTree.AddToTree(translokatorRoot);
                Graphics.BuildTranslokatorGrid(SceneData.Translokator);
            }
        }

        public void InstanceTranslokatorPart(Dictionary<int, IRenderer> assets, FrameObjectBase refframe, Matrix4x4 ParentTransform, Instance instance,bool updateInstanceBuffers = false)
        {
            var refTransform = ComputeWorldTransform(refframe.LocalTransform, ParentTransform);
            refTransform.M44 = 1.0f;

            if (refframe is FrameObjectSingleMesh mesh)
            {
                if (!assets.ContainsKey(refframe.RefID))
                {
                    goto SkipToChildren;
                }

                if (assets[refframe.RefID] is RenderModel model)
                {
                    Matrix4x4 newtransform = new Matrix4x4();

                    newtransform = MatrixUtils.SetMatrix(instance.Quaternion, new Vector3(instance.Scale, instance.Scale, instance.Scale), instance.Position);
                    newtransform = refTransform * newtransform;

                    if (!model.InstanceTransforms.ContainsKey(instance.RefID))
                    {
                        model.InstanceTransforms.Add(instance.RefID, Matrix4x4.Transpose(newtransform));
                        if (updateInstanceBuffers)
                        {
                            model.ReloadInstanceBuffer(Graphics.GetId3D11Device());
                        }
                    }
                }
            }

        SkipToChildren:;
            if (refframe.Children.Count > 0)
            {
                for (int i = 0; i < refframe.Children.Count; i++)
                {
                    InstanceTranslokatorPart(assets, refframe.Children[i], refTransform, instance,updateInstanceBuffers);
                }
            }
        }

        public List<RenderModel> UpdateTranslocatorPart(FrameObjectBase refframe, Matrix4x4 ParentTransform, Instance instance)
        {
            List<RenderModel> modelsToUpdate = new();

            if (Graphics == null)
            {
                return modelsToUpdate;
            }

            var refTransform = ComputeWorldTransform(refframe.LocalTransform, ParentTransform); ;
            refTransform.M44 = 1.0f;

            if (refframe is FrameObjectSingleMesh mesh)
            {
                if (!Graphics.Assets.ContainsKey(refframe.RefID))
                {
                    goto SkipToChildren;
                }

                if (Graphics.Assets[refframe.RefID] is RenderModel model)
                {
                    Matrix4x4 newtransform = new Matrix4x4();

                    newtransform = MatrixUtils.SetMatrix(instance.Quaternion, new Vector3(instance.Scale, instance.Scale, instance.Scale), instance.Position);
                    newtransform = refTransform * newtransform;

                    if (!model.InstanceTransforms.ContainsKey(instance.RefID))
                    {
                        model.InstanceTransforms.Add(instance.RefID, Matrix4x4.Transpose(newtransform));
                    }
                    else
                    {
                        model.InstanceTransforms[instance.RefID] = Matrix4x4.Transpose(newtransform);
                    }

                    modelsToUpdate.Add(model);
                }
            }

        SkipToChildren:;
            if (refframe.Children.Count > 0)
            {
                for (int i = 0; i < refframe.Children.Count; i++)
                {
                    modelsToUpdate.AddRange(UpdateTranslocatorPart(refframe.Children[i], refTransform, instance));
                }
            }

            return modelsToUpdate;
        }

        public Matrix4x4 ComputeWorldTransform(Matrix4x4 LocalTransform, Matrix4x4 ParentTransform)
        {
            //The world transform is calculated and then decomposed because some reason,
            //the renderer does not update on the first startup of the editor.
            Vector3 position, scale, newPos;
            Quaternion rotation, newRot;
            Matrix4x4.Decompose(LocalTransform, out scale, out rotation, out position);

            Vector3 parentPosition = Vector3.Zero;
            Vector3 parentScale = Vector3.One;
            Quaternion parentRotation = Quaternion.Identity;
            Matrix4x4.Decompose(ParentTransform, out parentScale, out parentRotation, out parentPosition);

            newRot = parentRotation * rotation;
            newPos = Vector3Utils.TransformCoordinate(position, ParentTransform);

            return MatrixUtils.SetMatrix(newRot, scale, newPos);
            //ToolkitAssert.Ensure(!worldTransform.IsNaN(), string.Format("Frame: {0} caused NaN()!", name.ToString()));
        }

        private void LoadActorFiles()
        {
            actorRoot = new TreeNode("Actor Items");
            actorRoot.Tag = "Folder";
            for (int z = 0; z < SceneData.Actors.Length; z++)
            {
                Actor actor = SceneData.Actors[z];
                TreeNode actorFile = new TreeNode("Actor File " + z);
                actorFile.Tag = "Folder";
                actorRoot.Nodes.Add(actorFile);
                for (int c = 0; c < actor.Items.Count; c++)
                {
                    var item = actor.Items[c];
                    TreeNode itemNode = new TreeNode("actor_" + z + "-" + c);
                    itemNode.Text = item.EntityName;
                    itemNode.Tag = item;

                    var typeString = string.Format("actorType_" + item.ActorTypeName);
                    var foundnodes = actorFile.Nodes.Find(typeString, false);
                    if (foundnodes.Length > 0)
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
            else if(node.Parent != null && node.Parent.Tag is RenderNav)
            {
                RenderNav ObjNav = (node.Parent.Tag as RenderNav);
                ObjNav.SelectNode(node.Index);
            }
            else if (node.Tag is Instance instance)
            {
                Graphics.SelectInstance(instance.RefID);
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

                        //ToolkitAssert.Ensure(sorted, "Error: Did not detect the frame accompanying this actor " + item.EntityName + "; This means it will probably cause errors in game. Check your actors in the toolkit!");
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

                    // Update rendered counterpart
                    IRenderer Asset = Graphics.GetAsset(int.Parse(selected.Name));
                    if (Asset != null)
                    {
                        RenderInstance Instance = (Asset as RenderInstance);
                        Instance.SetTransform(placement.Transform);
                    }

                    // Send an event to update our selected item. (if this is indeed our selected)
                    UpdateSelectedEventArgs Arguments = new UpdateSelectedEventArgs();
                    Arguments.RefID = int.Parse(selected.Name);
                    Graphics.OnSelectedObjectUpdated(this, Arguments);
                }
                else if (selected.Tag is Instance)
                {
                    Instance instance = (selected.Tag as Instance);
                    dPropertyGrid.UpdateObject();
                    //get refframe and set instance index transform
                    if (dSceneTree.SelectedNode.Parent.Tag is Object objGroup)
                    {
                        FrameObjectBase groupRef = SceneData.FrameResource.GetObjectByHash<FrameObjectBase>(objGroup.Name.Hash);

                        if (groupRef != null)
                        {
                            for (int i = 0; i < groupRef.Children.Count; i++)
                            {
                                var modelsToUpdate = UpdateTranslocatorPart(groupRef.Children[i], Matrix4x4.Identity, instance);
                                Graphics.UpdateInstanceBuffers(modelsToUpdate);
                            }
                        }
                        else
                        {
                            Graphics.InstanceGizmo.UpdateInstanceBuffer(instance, Graphics.GetId3D11Device());
                        }
                    }
                }
            }
        }

        private void ApplyChangesToRenderable(FrameObjectBase obj)
        {
            if (obj is FrameObjectArea)
            {
                FrameObjectArea area = (obj as FrameObjectArea);
                area.FillPlanesArray();
                RenderBoundingBox bbox = (Graphics.GetAsset(obj.RefID) as RenderBoundingBox);
                bbox.SetTransform(area.WorldTransform);
                bbox.Update(area.Bounds);
            }
            else if(obj is FrameObjectDummy)
            {
                FrameObjectDummy dummy = (obj as FrameObjectDummy);
                RenderBoundingBox bbox = (Graphics.GetAsset(obj.RefID) as RenderBoundingBox);
                bbox.SetTransform(dummy.WorldTransform);
                bbox.Update(dummy.Bounds);
            }
            else if (obj is FrameObjectSector)
            {
                FrameObjectSector sector = (obj as FrameObjectSector);
                sector.FillPlanesArray();
                RenderBoundingBox bbox = (Graphics.GetAsset(obj.RefID) as RenderBoundingBox);
                bbox.Update(sector.Bounds);
            }
            else if (obj is FrameObjectSingleMesh)
            {
                FrameObjectSingleMesh mesh = (obj as FrameObjectSingleMesh);
                RenderModel model = (Graphics.GetAsset(obj.RefID) as RenderModel);
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

        private void CreateMeshBuffers(ModelWrapper model)
        {
            // TODO: I want to move this into FrameObjectSingleMesh.
            FrameGeometry MeshGeometry = model.FrameMesh.Geometry;

            for (int i = 0; i < MeshGeometry.NumLods; i++)
            {
                bool bAdded = SceneData.VertexBufferPool.TryAddBuffer(model.VertexBuffers[i]);
                bAdded = SceneData.IndexBufferPool.TryAddBuffer(model.IndexBuffers[i]);
            }
        }

        private void CreateNewEntry(FrameResourceObjectType SelectedType, string name, bool bAddToNameTable)
        {
            FrameObjectBase frame = FrameFactory.ConstructFrameByObjectID(SceneData.FrameResource, SelectedType);

            // Frame was not valid, there is no need to carry on.
            if (frame == null)
            {
                return;
            }

            ToolkitAssert.Ensure(frame != null, "Frame was null!");

            frame.Name.Set(name);
            frame.IsOnFrameTable = bAddToNameTable;
            TreeNode node = new TreeNode(frame.Name.String);
            node.Tag = frame;
            node.Name = frame.RefID.ToString();

            if (frame is FrameObjectSingleMesh)
            {
                // TODO: We need to find an alternative method to creating single meshes
                // The Bundle system consists of multiple objects, in which one may not even be a single mesh.
                // Therefore it doesn't make sense to use this method - all users should use bundles.
                // However there may be some benefit in keeping this, maybe as a way to re-use loaded Single Meshes?
            }

            // If everything was succesful, then we would have reached this point.
            dSceneTree.AddToTree(node, frameResourceRoot);

            IRenderer renderer = BuildRenderObjectFromFrame(frame,null);
            if (renderer != null)
            {
                Graphics.InitObjectStack.Add(frame.RefID, renderer);
            }
        }

        private void Pick(int sx, int sy)
        {
            PickOutParams OutParams = Graphics.Pick(sx, sy, RenderPanel.Size.Width, RenderPanel.Size.Height);
            dViewProperties.SetPickInfo(OutParams);
            if (OutParams.LowestInstanceID != -1)
            {
                TreeNode[] instancenodes = dSceneTree.Find(OutParams.LowestInstanceID.ToString(), true);
                if (instancenodes.Length > 0)
                {
                    dSceneTree.SelectedNode = instancenodes[0];
                    TreeViewUpdateSelected();
                }
            }
            else
            {
                TreeNode[] nodes = dSceneTree.Find(OutParams.LowestRefID.ToString(), true);
    
                if (nodes.Length > 0)
                {
                    dSceneTree.SelectedNode = nodes[0];
                    
                        if (dSceneTree.SelectedNode.Tag is FrameObjectBase obj)
                        {
                            int Parent1Index = obj.ParentIndex1.Index;
                            int Parent2Index = obj.ParentIndex2.Index;
    
                            while(Parent1Index != -1 && Parent2Index != -1)
                            {
                                if (dSceneTree.SelectedNode.Parent != null)
                                {
                                    dSceneTree.SelectedNode = dSceneTree.SelectedNode.Parent;
    
                                    if (dSceneTree.SelectedNode.Tag is FrameObjectBase obj2)
                                    {
                                        Parent1Index = obj2.ParentIndex1.Index;
                                        Parent2Index = obj2.ParentIndex2.Index;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
    
                    TreeViewUpdateSelected();
                }            
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
            UpdatePositionElement(Graphics.Camera.Position);
        }
        
        private void ImportButton_Click(object sender, EventArgs e)
        {
            var frnode = dImportSceneTree.SelectedNode;
            if (frnode.Tag.GetType() == typeof(FrameHeaderScene) || frnode.Tag.GetType() == typeof(FrameHeader))//this should catch scenes and frameresource content
            {
                //todo manage exporting scenes, skip frameheader
                return;
            }
            
            FrameObjectBase frame = frnode.Tag as FrameObjectBase;
            TreeNode parent;
            parent = SceneData.FrameResource.ReadFramesFromImport(frame.Name.String, ImportedScene.FrameResource.SaveFramesStream(frame));

            if (dImportSceneTree.importTextures.Checked && parent != null && ImportedScene.FrameResource.CheckForMeshObjects(frnode))
            {
                Dictionary<uint, string> allTexturesDict = new Dictionary<uint, string>();
                ImportedScene.FrameResource.CollectAllTextureNames(frnode,allTexturesDict);
                List<string> allTextures = allTexturesDict.Values.ToList();
                SceneData.ImportTextures(allTextures, ImportedScene.ScenePath);
                

            }
            dSceneTree.AddToTree(parent, frameResourceRoot);
            ConvertNodeToFrame(parent);
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Button_ImportFrame.Enabled = true;
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

        private void UpdateObjectParents(ParentInfo.ParentType ParentType, int refID, FrameEntry entry = null)
        {
            FrameObjectBase obj = (dSceneTree.SelectedNode.Tag as FrameObjectBase);
            
            
            TreeNode[] newChildChildren = dSceneTree.Find(obj.RefID.ToString(), true);
            //checking if we are not trying to make children our new parent
            foreach (var child in newChildChildren)
            {
                if (child.Tag is FrameObjectBase childFrame)
                {
                    if (childFrame.IsFrameOwnChildren(refID)) {
                        return;
                    }                
                }
            }
            
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
                
                SceneData.FrameResource.SetParentOfObject(ParentType, obj, entry);
            }
            else
            {
                SceneData.FrameResource.SetParentOfObject(ParentType, obj, null);
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
                    // Used just in case the user wants to set the parent to "root"
                    ParentInfo.ParentType ParentType = (e.ChangedItem.Parent.Label == "ParentIndex1" ? ParentInfo.ParentType.ParentIndex1 : ParentInfo.ParentType.ParentIndex2);
                    UpdateObjectParents(ParentType, (int)e.ChangedItem.Value);
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
            if (pGrid.SelectedObject is Instance instance && dSceneTree.SelectedNode.Parent.Tag is Object objGroup)
            {
                FrameObjectBase groupRef = SceneData.FrameResource.GetObjectByHash<FrameObjectBase>(objGroup.Name.Hash);

                if (groupRef != null)
                {
                    for (int i = 0; i < groupRef.Children.Count; i++)
                    {
                        var modelsToUpdate = UpdateTranslocatorPart(groupRef.Children[i], Matrix4x4.Identity, instance);
                        Graphics.UpdateInstanceBuffers(modelsToUpdate);
                    }
                }
                else
                {
                    Graphics.InstanceGizmo.UpdateInstanceBuffer(instance, Graphics.GetId3D11Device());
                }
                dPropertyGrid.SetObject(instance);//this is done so edit transforms tab updates as it didnt happen before
            }
            if (pGrid.SelectedObject is Grid trGrid)
            {
                RebuildTranslokatorGrids();
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

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            TreeNode node = dSceneTree.SelectedNode;

            if (FrameResource.IsFrameType(node.Tag))
            {
                FrameEntry entry = node.Tag as FrameEntry;
                bool bDidRemove = SceneData.FrameResource.DeleteFrame(entry);

                ToolkitAssert.Ensure(bDidRemove == true, "Failed to remove!");

                // we can just delete root node here, all children are vanquished
                dSceneTree.RemoveNode(node);
            }
            else if(node.Tag.GetType() == typeof(FrameHeaderScene))
            {
                FrameHeaderScene scene = (node.Tag as FrameHeaderScene);
                bool bDidRemove = SceneData.FrameResource.DeleteScene(scene);

                ToolkitAssert.Ensure(bDidRemove == true, "Failed to remove!");

                // we can just delete root node here, all children are vanquished
                dSceneTree.RemoveNode(node);
            }
            else if (node.Tag.GetType() == typeof(Collision.Placement))
            {
                dSceneTree.RemoveNode(node);

                int iName = Convert.ToInt32(node.Name);
                Graphics.DeleteAsset(iName);
            }
            else if (node.Tag.GetType() == typeof(RenderRoad))
            {
                dSceneTree.RemoveNode(node);
                Graphics.DeleteAsset(int.Parse(node.Name));
            }
            else if (node.Tag.GetType() == typeof(RenderJunction))
            {
                dSceneTree.RemoveNode(node);
                Graphics.DeleteAsset(int.Parse(node.Name));
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
                    Graphics.DeleteAsset(iName);
                }
            }
            else if (node.Tag is Instance instance)
            {
                DeleteTRInstance(node);
            }
            else if (node.Tag is Object obj)
            {
                DeleteTRObject(node);
            }
            else if (node.Tag is ObjectGroup og)
            {
                while (node.Nodes.Count>0)
                {
                    DeleteTRObject(node.FirstNode);
                }
                dSceneTree.RemoveNode(node);
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
                else if (node.Tag.GetType() == typeof(FrameObjectPoint))
                    newEntry = new FrameObjectPoint((FrameObjectPoint)node.Tag);
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
            else if (node.Tag is Instance instance)
            {
                TranslokatorNewInstance(node.Parent,instance);
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
            ModelWrapper WrapperObject = null;
            if (dSceneTree.SelectedNode.Tag.GetType() == typeof(Collision.CollisionModel))
            {
                WrapperObject = ExportCollision(dSceneTree.SelectedNode.Tag as Collision.CollisionModel);
            }
            else if (dSceneTree.SelectedNode.Tag.GetType() == typeof(FrameHeaderScene))
            {
                WrapperObject = ExportScene(dSceneTree.SelectedNode.Tag as FrameHeaderScene);
            }
            else if (dSceneTree.SelectedNode.Text == "Collision Data")
            {
                WrapperObject = ExportCollisions(dSceneTree.SelectedNode);
            }
            else
            {
                WrapperObject = Export3DFrame();
            }

            // Create a bundle to make it easier to validate
            MT_ObjectBundle CurrentBundle = new MT_ObjectBundle();
            CurrentBundle.Objects = new MT_Object[1];
            CurrentBundle.Objects[0] = WrapperObject.ModelObject;

            FrameResourceModelExporter ModelExporter = new FrameResourceModelExporter(CurrentBundle);
            if (ModelExporter.ShowDialog() != DialogResult.OK)
            {
                ModelExporter.Dispose();
                return;
            }

            // Now we should choose on a name
            if (SaveFileDialog != null)
            {
                SaveFileDialog.Reset();
            }
            SaveFileDialog.FileName = CurrentBundle.Objects[0].ObjectName;
            SaveFileDialog.RestoreDirectory = true;
            SaveFileDialog.Filter = "GLTF File (Binary) (*.glb)|*.glb|GLTF File (ASCII) (*.gltf)|*.gltf*";

            if (SaveFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            WrapperObject.ExportObject(SaveFileDialog.FileName, 0);
        }

        private ModelWrapper ExportCollision(Collision.CollisionModel data)
        {
            Collision.Placement[] TempPlacements = new Collision.Placement[1];
            TempPlacements[0] = new Collision.Placement();

            MT_Object CollisionObject = MT_Object.TryBuildObject(data, TempPlacements);

            ModelWrapper WrapperObject = new ModelWrapper();
            WrapperObject.ModelObject = CollisionObject;

            return WrapperObject;
        }

        private ModelWrapper ExportCollisions(TreeNode CollisionRoot)
        {
            MT_Object RootObject = new MT_Object();
            RootObject.ObjectName = "COLLISION_ROOT";
            RootObject.ObjectType = MT_ObjectType.Dummy;

            List<MT_Object> ChildObjects = new List<MT_Object>();

            foreach (TreeNode CollisionNode in CollisionRoot.Nodes)
            {
                // Skip non collision models
                Collision.CollisionModel CurrentModel = (CollisionNode.Tag as Collision.CollisionModel);
                if(CurrentModel == null)
                {
                    continue;
                }

                List<Collision.Placement> Placements = new List<Collision.Placement>();
                foreach(TreeNode PlacementNode in CollisionNode.Nodes)
                {
                    if (PlacementNode.Tag.GetType() == typeof(Collision.Placement))
                    {
                        Placements.Add(PlacementNode.Tag as Collision.Placement);
                    }
                }

                // construct collision using model and placements
                MT_Object NewCollisionObject = MT_Object.TryBuildObject(CurrentModel, Placements.ToArray());
                ChildObjects.Add(NewCollisionObject);
            }

            RootObject.Children = ChildObjects.ToArray();
            RootObject.ObjectFlags |= MT_ObjectFlags.HasChildren;

            ModelWrapper WrapperObject = new ModelWrapper();
            WrapperObject.ModelObject = RootObject;

            return WrapperObject;
        }

        private ModelWrapper Export3DFrame()
        {
            FrameObjectBase FrameObject = (dSceneTree.SelectedNode.Tag as FrameObjectBase);
            ModelWrapper ModelWrapperObject = null;

            if (FrameObject is FrameObjectSingleMesh)
            {
                FrameObjectSingleMesh SingleMesh = (FrameObject as FrameObjectSingleMesh);
                IndexBuffer[] indexBuffers = new IndexBuffer[SingleMesh.Geometry.LOD.Length];
                VertexBuffer[] vertexBuffers = new VertexBuffer[SingleMesh.Geometry.LOD.Length];

                //we need to retrieve buffers first.
                for (int c = 0; c != SingleMesh.Geometry.LOD.Length; c++)
                {
                    indexBuffers[c] = SceneData.IndexBufferPool.GetBuffer(SingleMesh.Geometry.LOD[c].IndexBufferRef.Hash);
                    vertexBuffers[c] = SceneData.VertexBufferPool.GetBuffer(SingleMesh.Geometry.LOD[c].VertexBufferRef.Hash);
                }

                // Construct wrapper (based on model)
                if (FrameObject is FrameObjectModel)
                {
                    ModelWrapperObject = new ModelWrapper(FrameObject as FrameObjectModel, indexBuffers, vertexBuffers);
                }
                else
                {
                    ModelWrapperObject = new ModelWrapper(FrameObject as FrameObjectSingleMesh, indexBuffers, vertexBuffers);
                }
            }
            else
            {
                ModelWrapperObject = new ModelWrapper(FrameObject);
            }

            return ModelWrapperObject;
        }

        private ModelWrapper ExportScene(FrameHeaderScene Scene)
        {
            ModelWrapper ModelWrapperObject = new ModelWrapper(Scene);
            return ModelWrapperObject;
        }

        private void AddButtonOnClick(object sender, EventArgs e)
        {
            NewObjectForm form = new NewObjectForm(true);
            form.SetLabel(Language.GetString("$QUESTION_FRADD"));
            form.LoadOption(new ControlOptionFrameAdd());

            if(form.ShowDialog() == DialogResult.OK)
            {
                ControlOptionFrameAdd window = (form.control as ControlOptionFrameAdd);
                FrameResourceObjectType selection = window.GetSelectedType();
                CreateNewEntry(selection, form.GetInputText(), window.GetAddToNameTable());
            }
        }

        private void AddSceneFolderButton_Click(object sender, EventArgs e)
        {
            var scene = SceneData.FrameResource.AddSceneFolder("NEW_SCENE");
            TreeNode node = new TreeNode(scene.ToString());
            node.Tag = scene;
            node.Name = scene.RefID.ToString();
            dSceneTree.AddToTree(node, frameResourceRoot);
        }

        // TODO: Need to cleanup this function, it's atrocious.
        private void ValidateCollisionFile()
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
        }

        private Collision.CollisionModel CreateCollision(Collision.CollisionModel collisionModel)
        {
            ulong CollisionHash = collisionModel.Hash;
            Collision.CollisionModel CollisionModel = null;
            if (!SceneData.Collisions.Models.ContainsKey(CollisionHash))
            {
                // Create a new renderable for collision object
                RenderStaticCollision collision = new RenderStaticCollision();
                collision.ConvertCollisionToRender(collisionModel.Hash, collisionModel.Mesh);
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

            return CollisionModel;
        }

        private Collision.Placement CreatePlacement(Collision.CollisionModel ColModel, Vector3 Position, Quaternion Rotation)
        {
            // Create a new placement for this mesh
            Collision.Placement placement = new Collision.Placement();
            placement.Hash = ColModel.Hash;
            placement.Position = Position;
            placement.RotationDegrees = Vector3.Zero;

            // Try and find the collision node
            TreeNode ExistingCollisionNode = dSceneTree.GetTreeNode(ColModel.Hash.ToString(), collisionRoot, true);
            if (ExistingCollisionNode == null)
            {
                // Create a new TreeNode for the CollisionModel
                TreeNode CollisionNode = new TreeNode(ColModel.Hash.ToString());
                CollisionNode.Text = ColModel.Hash.ToString();
                CollisionNode.Name = ColModel.Hash.ToString();
                CollisionNode.Tag = ColModel;
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
            dSceneTree.SelectedNode = child;

            // Complete it
            RenderInstance instance = new RenderInstance();
            instance.Init(RenderStorageSingleton.Instance.StaticCollisions[placement.Hash]);
            instance.SetTransform(placement.Transform);
            Graphics.InitObjectStack.Add(refID, instance);
            SceneData.Collisions.Placements.Add(placement);

            return placement;
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

        private void TranslokatorTint_Click(object sender, EventArgs e)
        {
            ToolkitSettings.bTranslokatorTint = !ToolkitSettings.bTranslokatorTint;
            ToolkitSettings.WriteKey("EnableTranslokator", "ModelViewer", ToolkitSettings.bTranslokatorTint.ToString());
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
            IRenderer asset = BuildRenderObjectFromFrame(parent,null);
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
                
                if (FrameBrowser.FilterIndex.Equals(1))
                {
                    PopulateImportedData(Filename);
                }
                else
                {
                    
                    TreeNode parent = SceneData.FrameResource.ReadFramesFromImport(Filename);
                    dSceneTree.AddToTree(parent, frameResourceRoot);
                    ConvertNodeToFrame(parent);
                    
                }
                
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

        private void UpdatePositionElement(Vector3 InPosition)
        {
            // Hack: We have to remove the delegate before we can change the values, 
            // or we'll fire some unnecessary code..

            PositionXTool.ValueChanged -= new EventHandler(CameraToolsOnValueChanged);
            PositionYTool.ValueChanged -= new EventHandler(CameraToolsOnValueChanged);
            PositionZTool.ValueChanged -= new EventHandler(CameraToolsOnValueChanged);
            PositionXTool.Value = (decimal)InPosition.X;
            PositionYTool.Value = (decimal)InPosition.Y;
            PositionZTool.Value = (decimal)InPosition.Z;
            PositionXTool.ValueChanged += new EventHandler(CameraToolsOnValueChanged);
            PositionYTool.ValueChanged += new EventHandler(CameraToolsOnValueChanged);
            PositionZTool.ValueChanged += new EventHandler(CameraToolsOnValueChanged);
        }

        private void Button_ImportBundle_OnClick(object sender, EventArgs e)
        {
            if (MeshBrowser.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            if(Path.Exists(MeshBrowser.FileName) == false)
            {
                return;
            }

            // pass to importer
            FrameResourceModelImporter modelForm = new FrameResourceModelImporter(MeshBrowser.FileName);
            DialogResult Result = modelForm.ShowDialog();
            if(Result != DialogResult.OK)
            {
                modelForm.Dispose();
                return;
            }

            // TODO: In an ideal world this would not live in MapEditor.cs
            // and probably live within FrameResourceModelImporter.

            // Only ask we they want to save the materials if we have some.
            if (modelForm.NewMaterials.Count > 0)
            {
                if (MessageBox.Show(Language.GetString("$Q_IMPORT_MATERIALS"), "Toolkit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Manager will handle adding for us.
                    MaterialsManager.AddMaterialsToLibrary(modelForm.NewMaterials);
                }
            }

            // Continue with the importing of the bundle
            foreach (MT_Object ModelObject in modelForm.CurrentBundle.Objects)
            {
                ConstructFrameFromImportedObject(ModelObject, frameResourceRoot);
            }

            modelForm.Dispose();
        }

        private void ConstructFrameFromImportedObject(MT_Object ObjectInfo, TreeNode Parent)
        {
            ModelWrapper Wrapper = new ModelWrapper();
            Wrapper.ModelObject = ObjectInfo;

            // Prep for frame node
            TreeNode FrameNode = null;

            // Convert object into SingleMesh
            FrameObjectBase NewFrame = FrameFactory.ConstructFrameByObjectType(ObjectInfo.ObjectType, SceneData.FrameResource);
            if (NewFrame != null)
            {
                // Set other MetaInfo
                Matrix4x4 LocalTransform = MatrixUtils.SetMatrix(ObjectInfo.Rotation, ObjectInfo.Scale, ObjectInfo.Position);
                NewFrame.LocalTransform = LocalTransform;
                NewFrame.Name.Set(ObjectInfo.ObjectName);
                NewFrame.IsOnFrameTable = true;

                // Construct mesh (if applicable)
                if (ObjectInfo.ObjectType == MT_ObjectType.StaticMesh)
                {
                    FrameObjectSingleMesh NewMesh = (NewFrame as FrameObjectSingleMesh);
                    NewMesh.CreateMeshFromRawModel(Wrapper);
                    CreateMeshBuffers(Wrapper);
                }
                else if (ObjectInfo.ObjectType == MT_ObjectType.RiggedMesh)
                {
                    FrameObjectModel NewMesh = (NewFrame as FrameObjectModel);
                    NewMesh.CreateMeshFromRawModel(Wrapper);
                    CreateMeshBuffers(Wrapper);
                }

                // Construct TreeNode
                FrameNode = new TreeNode(NewFrame.Name.ToString());
                FrameNode.Tag = NewFrame;
                FrameNode.Name = NewFrame.RefID.ToString();
                dSceneTree.AddToTree(FrameNode, Parent);

                if (Parent.Tag is FrameHeaderScene)
                {
                    FrameHeaderScene SceneEntry = (Parent.Tag as FrameHeaderScene);
                    SceneData.FrameResource.SetParentOfObject(ParentInfo.ParentType.ParentIndex2, NewFrame, SceneEntry);
                }
                else if (Parent.Tag is FrameEntry)
                {
                    FrameEntry ParentEntry = (Parent.Tag as FrameEntry);
                    SceneData.FrameResource.SetParentOfObject(ParentInfo.ParentType.ParentIndex2, NewFrame, ParentEntry);
                    SceneData.FrameResource.SetParentOfObject(ParentInfo.ParentType.ParentIndex1, NewFrame, ParentEntry);
                }
               
                // Construct renderer and add to stack
                IRenderer Renderer = BuildRenderObjectFromFrame(NewFrame,null);
                if (Renderer != null)
                {
                    Graphics.InitObjectStack.Add(NewFrame.RefID, Renderer);
                }
            }

            // object is a scene so it has an alternative import route
            if(ObjectInfo.ObjectType == MT_ObjectType.Scene)
            {
                var scene = SceneData.FrameResource.AddSceneFolder(ObjectInfo.ObjectName);
                FrameNode = new TreeNode(scene.ToString());
                FrameNode.Tag = scene;
                FrameNode.Name = scene.RefID.ToString();

                dSceneTree.AddToTree(FrameNode, frameResourceRoot);
            }

            if (ObjectInfo.ObjectFlags.HasFlag(MT_ObjectFlags.HasCollisions))
            {
                ValidateCollisionFile();

                Collision.CollisionModel collisionModel = new CollisionModelBuilder().BuildFromMTCollision(ObjectInfo.ObjectName, ObjectInfo.Collision);
                CreateCollision(collisionModel);

                // now add instances
                foreach(MT_CollisionInstance ColInstance in ObjectInfo.Collision.Instances)
                {
                    CreatePlacement(collisionModel, ColInstance.Position, ColInstance.Rotation);
                }
            }

            if (ObjectInfo.ObjectFlags.HasFlag(MT_ObjectFlags.HasChildren))
            {
                foreach (MT_Object Child in ObjectInfo.Children)
                {
                    ConstructFrameFromImportedObject(Child, FrameNode);
                }
            }
        }
        private void TranslokatorNewInstanceButton_Click(object sender, EventArgs e)
        {
            TranslokatorNewInstance(dSceneTree.SelectedNode,null);
        }

        private void TranslokatorNewInstance(TreeNode parentObj, Instance old)
        {
            Instance newInstance = (old == null) ? new Instance() : new Instance(old);

            newInstance.RefID = RefManager.GetNewRefID();
            TreeNode newInstanceNode = new TreeNode(parentObj.Text + " " + parentObj.Nodes.Count.ToString());
            newInstanceNode.Tag = newInstance;
            newInstanceNode.Name = newInstance.RefID.ToString();
                
            Object parent = parentObj.Tag as Object;
            FrameObjectBase frameref = SceneData.FrameResource.GetObjectByHash<FrameObjectBase>(parent.Name.Hash);
            if (frameref != null && frameref.HasMeshObject())
            {
                for (int i = 0; i < frameref.Children.Count; i++)
                {
                    InstanceTranslokatorPart(Graphics.Assets, frameref.Children[i], Matrix4x4.Identity, newInstance,true);
                }
            }
            else
            {
                Graphics.InstanceGizmo.InstanceTranslokator(newInstance,Graphics.GetId3D11Device());
            }
            
            dSceneTree.AddToTree(newInstanceNode,parentObj);
        }
        
        private void UpdateInstanceVisualisation(TreeNode instanceNode, Object trObject, bool visibility)
        {
            FrameObjectBase groupRef = SceneData.FrameResource.GetObjectByHash<FrameObjectBase>(trObject.Name.Hash);
            
            Instance instance = instanceNode.Tag as Instance;
            if (visibility)
            {
                if (groupRef != null && groupRef.HasMeshObject())
                {
                    for (int i = 0; i < groupRef.Children.Count; i++)
                    {
                        InstanceTranslokatorPart(Graphics.Assets, groupRef.Children[i], Matrix4x4.Identity, instance,true);
                    }
                }
                else
                {
                    Graphics.InstanceGizmo.InstanceTranslokator(instance,Graphics.GetId3D11Device());
                }
            }
            else
            {
                if (groupRef != null && groupRef.HasMeshObject())
                {
                    Graphics.DeleteInstance(groupRef,instance.RefID);
                }
                else
                {
                    Graphics.DeleteInstance(instance.RefID);
                }
            }
            
        }
        
        private void ActorEntryNewTRObjectButton_Click(object sender, EventArgs e)
        {
            TreeNode ActorNode = dSceneTree.SelectedNode;
            ActorEntry actor = ActorNode.Tag as ActorEntry;
            if (ActorNode == null || actor == null)
            {
                return;
            }
            FrameObjectBase groupRef = SceneData.FrameResource.GetObjectByHash<FrameObjectBase>(actor.FrameNameHash);
            if (groupRef == null)//todo: once multisds is added, tweak this
            {
                if (MessageBox.Show("There is no matching Frame: " + actor.FrameName + " in FrameResource contents. If you intend to reference Frame of this name, it is not present. Do you want to continue?", "Toolkit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
            }

            TreeNode ogNode = dSceneTree.GetObjectGroupByActorType(translokatorRoot, actor.ActorTypeID);
            if (ogNode==null)
            {
                //create objectgroup if not present
                ObjectGroup newOG = new ObjectGroup();
                newOG.ActorType = (ActorTypes)actor.ActorTypeID;
                TreeNode newOGNode = new TreeNode(String.Format("Object Group: [{0}]", newOG.ActorType));
                newOGNode.Tag = newOG;
                dSceneTree.AddToTree(newOGNode,translokatorRoot.Nodes[0]);
                ogNode = newOGNode;
                Log.WriteLine("New Translokator ObjectGroup:" + newOG.ActorType,LoggingTypes.MESSAGE,LogCategoryTypes.FUNCTION);
            }

            if (dSceneTree.ObjectGroupHasObject(ogNode, actor.FrameNameHash))
            {
                ToolkitAssert.Ensure(!dSceneTree.ObjectGroupHasObject(ogNode, actor.FrameNameHash),"Error: The Object: " + actor.FrameName + " is already present.");
                return;
            }
            else
            {
                Object newObj = new Object();
                newObj.Name.Set(actor.FrameName);
                TreeNode objNode = new TreeNode(newObj.Name.ToString());
                objNode.Tag = newObj;
                dSceneTree.AddToTree(objNode,ogNode);
                Log.WriteLine("New Translokator Object:" + newObj.Name.String,LoggingTypes.MESSAGE,LogCategoryTypes.FUNCTION);
            }
        }

        private void DeleteTRInstance(TreeNode instanceNode)
        {
            Instance instance = instanceNode.Tag as Instance;
            FrameObjectBase groupRef = SceneData.FrameResource.GetObjectByHash<FrameObjectBase>((instanceNode.Parent.Tag as Object).Name.Hash);
            dSceneTree.RemoveNode(instanceNode);
            if (groupRef != null)
            {
                Graphics.DeleteInstance(groupRef, instance.RefID);
            }
            else
            {
                Graphics.DeleteInstance(instance.RefID);
            }
        }

        private void DeleteTRObject(TreeNode objectNode)
        {
            while (objectNode.Nodes.Count>0)
            {
                DeleteTRInstance(objectNode.FirstNode);
            }
            dSceneTree.RemoveNode(objectNode);
        }

        private void RebuildTranslokatorGrids()
        {
            SceneData.Translokator.RebuildGridData();
            Graphics.BuildTranslokatorGrid(SceneData.Translokator);

            TreeNode gridsNode = null;

            foreach (TreeNode node in translokatorRoot.Nodes)
            {
                if (node.Text.Equals("Grids", StringComparison.InvariantCultureIgnoreCase))
                {
                    gridsNode = node;
                    break;
                }
            }

            for (int i = 0; i < gridsNode.Nodes.Count; i++)
            {
                TreeNode child = gridsNode.Nodes[i];

                if (child.Tag is Grid)
                {
                    Graphics.SetTranslokatorGridEnabled(i, child.Checked && child.CheckIfParentsAreValid());
                }
            }
        }
        
        private void TRRebuildObjectButton_Click(object sender, EventArgs e)
        {
            TreeNode ObjectNode = dSceneTree.SelectedNode;
            Object obj = ObjectNode.Tag as Object;
            if (ObjectNode == null || obj == null || ObjectNode.Nodes.Count == 0)
            {
                return;
            }
            FrameObjectBase groupRef = SceneData.FrameResource.GetObjectByHash<FrameObjectBase>(obj.Name.Hash);

            foreach (TreeNode instanceNode in ObjectNode.Nodes)//deleting all instances under selected object and rebuilding them
            {
                Instance instance = instanceNode.Tag as Instance;
                if (groupRef != null && groupRef.HasMeshObject())
                {
                    Graphics.DeleteInstance(instance.RefID);//in case the object didnt have mesh before, so there are no duplicates
                    Graphics.DeleteInstance(groupRef,instance.RefID);//maybe add optionable bool to delete in rendermodel so it doesnt reload every instance here
                    for (int i = 0; i < groupRef.Children.Count; i++)
                    {
                        InstanceTranslokatorPart(Graphics.Assets, groupRef.Children[i], Matrix4x4.Identity, instance,true);
                    }
                }
                else
                {
                    Graphics.DeleteInstance(instance.RefID);
                    Graphics.InstanceGizmo.InstanceTranslokator(instance,Graphics.GetId3D11Device());
                }
            }
		}

        private void OnFrameRemoved(object sender, OnFrameRemovedArgs e)
        {
            Graphics.DeleteAsset(e.FrameRefID);
        }
    }
}

