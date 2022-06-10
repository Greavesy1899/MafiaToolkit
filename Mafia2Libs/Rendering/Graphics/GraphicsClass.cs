using Mafia2Tool;
using Rendering.Core;
using Rendering.Input;
using ResourceTypes.FrameResource;
using ResourceTypes.Translokator;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Windows.Forms;
using Toolkit.Core;
using Utils.Models;
using Utils.Settings;
using Utils.VorticeUtils;
using Vortice.Mathematics;

namespace Rendering.Graphics
{
    public class UpdateSelectedEventArgs : EventArgs
    {
        public int RefID { get; set; }
    }

    public struct PickOutParams
    {
        public float LowestDistance { get; set; }
        public int LowestRefID { get; set; }
        public Vector3 WorldPosition { get; set; }
    }

    public class GraphicsClass
    {
        public InputClass Input { get; private set; }
        public WorldSettings WorldSettings { get; set; }
        public Camera Camera { get; set; }
        public Dictionary<int, IRenderer> InitObjectStack { get; set; }
        public Profiler Profile { get; set; }

        public EventHandler<UpdateSelectedEventArgs> OnSelectedObjectUpdated;

        private Dictionary<int, IRenderer> Assets;
        private int selectedID;
        private RenderBoundingBox selectionBox;
        private RenderModel sky;
        private RenderModel clouds;
        private GizmoTool TranslationGizmo;

        private DirectX11Class D3D;

        private SpatialGrid translokatorGrid;
        private SpatialGrid[] navigationGrids;

        // Local batches for objects passed through
        private PrimitiveBatch LineBatch = null;
        private PrimitiveBatch BBoxBatch = null;
        public PrimitiveManager OurPrimitiveManager { get; private set; }
        public ProxyRenderManager OurProxyRenderManager { get; private set; }
        public BufferManager OurBufferManager { get; private set; }
        public RuntimeCollisionManager OurCollisionManager { get; private set; }

        public GraphicsClass()
        {
            D3D = new DirectX11Class();

            InitObjectStack = new Dictionary<int, IRenderer>();
            Profile = new Profiler();
            Assets = new Dictionary<int, IRenderer>();
            selectionBox = new RenderBoundingBox();
            translokatorGrid = new SpatialGrid();
            navigationGrids = new SpatialGrid[0];
            OurPrimitiveManager = new PrimitiveManager();
            OurProxyRenderManager = new ProxyRenderManager(this);
            OurBufferManager = new BufferManager(this, D3D);
            OurCollisionManager = new RuntimeCollisionManager(this, D3D);

            OnSelectedObjectUpdated += OnSelectedObjectHasUpdated;

            // Create bespoke batches for any lines or boxes passed in via the construct stack
            string LineBatchID = string.Format("Graphics_LineBatcher_{0}", RefManager.GetNewRefID());
            LineBatch = new PrimitiveBatch(PrimitiveType.Line, LineBatchID);

            string BBoxBatchID = string.Format("Graphics_BBoxBatcher_{0}", RefManager.GetNewRefID());
            BBoxBatch = new PrimitiveBatch(PrimitiveType.Box, BBoxBatchID);

            OurPrimitiveManager.AddPrimitiveBatch(LineBatch);
            OurPrimitiveManager.AddPrimitiveBatch(BBoxBatch);
        }

        public bool PreInit(IntPtr WindowHandle)
        {
            if (!D3D.Init(WindowHandle))
            {
                MessageBox.Show("Failed to initialize DirectX11!");
            }
            Profile.Init();
            if(!RenderStorageSingleton.Instance.IsInitialised())
            {
                bool result = RenderStorageSingleton.Instance.Initialise(D3D);
                var structure = new M2TStructure();
                //import gizmo
                RenderModel gizmo = new RenderModel(-1);
                gizmo.CachedGraphics = this;
                structure.ReadFromM2T("Resources/GizmoModel.m2t");
                gizmo.ConvertMTKToRenderModel(structure);
                gizmo.InitBuffers(D3D.Device, D3D.DeviceContext);
                gizmo.SetVisibility(true);
                TranslationGizmo = new GizmoTool(gizmo);

                //sky = new RenderModel(-1);
                //sky.CachedGraphics = this;
                //structure = new M2TStructure();
                //structure.ReadFromM2T("Resources/sky_backdrop.m2t");
                //sky.ConvertMTKToRenderModel(structure);
                //sky.InitBuffers(D3D.Device, D3D.DeviceContext);

                //clouds = new RenderModel(-1);
                //clouds.CachedGraphics = this;
                //structure = new M2TStructure();
                //structure.ReadFromM2T("Resources/weather_clouds.m2t");
                //clouds.ConvertMTKToRenderModel(structure);
                //clouds.InitBuffers(D3D.Device, D3D.DeviceContext);
               // clouds.SetVisibility(false);
            }

            selectionBox.SetColour(System.Drawing.Color.Red);
            selectionBox.Init(new BoundingBox(new Vector3(0.5f), new Vector3(-0.5f)));          
            selectionBox.SetVisibility(false);
            return true;
        }

        public bool InitScene(int width, int height)
        {
            WorldSettings = new WorldSettings();
            WorldSettings.SetupLighting();
            Camera = new Camera();
            Camera.Position = new Vector3(0.0f, 0.0f, 15.0f);
            Camera.SetProjectionMatrix(width, height);
            ClearRenderStack();
            selectionBox.InitBuffers(D3D.Device, D3D.DeviceContext);
            TranslationGizmo.InitBuffers(D3D.Device, D3D.DeviceContext);
            //sky.InitBuffers(D3D.Device, D3D.DeviceContext);
            //sky.SetVisibility(false);
            //clouds.InitBuffers(D3D.Device, D3D.DeviceContext);
            Input = new InputClass();
            Input.Init();
            return true;
        }

        public TreeNode SetTranslokatorGrid(TranslokatorLoader translokator)
        {
            translokatorGrid = new SpatialGrid(this, translokator);
            translokatorGrid.Initialise(D3D.Device, D3D.DeviceContext);
            return translokatorGrid.GetTreeNodes();
        }

        public TreeNode SetNavigationGrid(ResourceTypes.Navigation.OBJData[] data)
        {
            TreeNode[] Grids = new TreeNode[data.Length];
            navigationGrids = new SpatialGrid[data.Length];

            for(int i = 0; i < navigationGrids.Length; i++)
            {
                navigationGrids[i] = new SpatialGrid(this, data[i].runtimeMesh);
                navigationGrids[i].Initialise(D3D.Device, D3D.DeviceContext);
                Grids[i] = navigationGrids[i].GetTreeNodes();
                Grids[i].Text = string.Format("Grid: {0}", i);
            }

            TreeNode Parent = new TreeNode("Navigation Grids");
            Parent.Nodes.AddRange(Grids);
            return Parent;
        }

        public PickOutParams Pick(int sx, int sy, int Width, int Height)
        {
            float lowest = float.MaxValue;
            int lowestRefID = -1;
            Vector3 WorldPosIntersect = Vector3.Zero;

            Ray ray = Camera.GetPickingRay(new Vector2(sx, sy), new Vector2(Width, Height));
            PickOutParams ProxyOutParams = OurProxyRenderManager.Pick(ray);
            lowest = ProxyOutParams.LowestDistance;
            lowestRefID = ProxyOutParams.LowestRefID;
            WorldPosIntersect = ProxyOutParams.WorldPosition;

            foreach (KeyValuePair<int, IRenderer> model in Assets)
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
                        float t;

                        if (!Toolkit.Mathematics.Collision.RayIntersectsTriangle(ref localRay, ref v0, ref v1, ref v2, out t)) continue;

                        if (t < 0.0f || float.IsNaN(t))
                        {
                            //if (SceneData.FrameResource.FrameObjects.ContainsKey(model.Key))
                            //{
                            //    var frame = (SceneData.FrameResource.FrameObjects[model.Key] as FrameObjectBase);
                            //    Utils.Logging.Log.WriteLine(string.Format("The toolkit has failed to analyse a model: {0} {1}", frame.Name, t));
                            //}
                        }

                        var worldPosition = ray.Position + t * ray.Direction;
                        var distance = (worldPosition - ray.Position).LengthSquared();

                        if (distance < lowest)
                        {
                            lowest = distance;
                            lowestRefID = model.Key;
                            WorldPosIntersect = worldPosition;
                        }
                    }
                }
            }

            PickOutParams OutputParams = new PickOutParams();
            OutputParams.LowestRefID = lowestRefID;
            OutputParams.WorldPosition = WorldPosIntersect;

            return OutputParams;
        }

        public void Frame()
        {
            ClearRenderStack();
            Render();
            Profile.Update();
        }

        public bool UpdateInput()
        {
            bool bCameraUpdated = false;
            float Multiplier = ToolkitSettings.CameraSpeed;

            if (Input.IsKeyDown(Keys.ShiftKey))
            {
                Multiplier *= 2.0f;
            }

            float speed = Profile.DeltaTime * Multiplier;

            if (Input.IsKeyDown(Keys.A))
            {
                Camera.Position -= Vector3Utils.FromVector4(Vector4.Multiply(Camera.ViewMatrix.GetColumn(0), speed));
                bCameraUpdated = true;
            }

            if (Input.IsKeyDown(Keys.D))
            {
                Camera.Position += Vector3Utils.FromVector4(Vector4.Multiply(Camera.ViewMatrix.GetColumn(0), speed));
                bCameraUpdated = true;
            }

            if (Input.IsKeyDown(Keys.W))
            {
                Camera.Position -= Vector3Utils.FromVector4(Vector4.Multiply(Camera.ViewMatrix.GetColumn(2), speed));
                bCameraUpdated = true;
            }

            if (Input.IsKeyDown(Keys.S))
            {
                Camera.Position += Vector3Utils.FromVector4(Vector4.Multiply(Camera.ViewMatrix.GetColumn(2), speed));
                bCameraUpdated = true;
            }

            if (Input.IsKeyDown(Keys.Q))
            {
                Camera.Position.Z += speed;
                bCameraUpdated = true;
            }

            if (Input.IsKeyDown(Keys.E))
            {
                Camera.Position.Z -= speed;
                bCameraUpdated = true;
            }

            return bCameraUpdated;
        }

        public bool Render()
        {
            D3D.BeginScene(0.0f, 0f, 0f, 1.0f);
            Camera.Render();

            foreach (BaseShader Shader in RenderStorageSingleton.Instance.ShaderManager.shaders.Values)
            {
                Shader.InitCBuffersFrame(D3D.DeviceContext, Camera, WorldSettings);
            }

            foreach (IRenderer RenderEntry in Assets.Values)
            {
                RenderEntry.UpdateBuffers(D3D.Device, D3D.DeviceContext);
                RenderEntry.Render(D3D, Camera);
            }

            //navigationGrids[0].Render(D3D.Device, D3D.DeviceContext, Camera);
            foreach (var grid in navigationGrids)
            {
                grid.Render(D3D.Device, D3D.DeviceContext, Camera);
            }

            OurPrimitiveManager.RenderPrimitives(D3D, Camera);
            OurProxyRenderManager.RenderProxies(D3D, Camera);

            translokatorGrid.Render(D3D.Device, D3D.DeviceContext, Camera);
            selectionBox.UpdateBuffers(D3D.Device, D3D.DeviceContext);
            selectionBox.Render(D3D, Camera);
            //TranslationGizmo.UpdateBuffers(D3D.Device, D3D.DeviceContext);
            TranslationGizmo.Render(D3D, Camera);
            //clouds.UpdateBuffers(D3D.Device, D3D.DeviceContext);
            //clouds.Render(D3D.Device, D3D.DeviceContext, Camera);
            //sky.SetVisibility(false);
            //sky.UpdateBuffers(D3D.Device, D3D.DeviceContext);
            //sky.Render(D3D.Device, D3D.DeviceContext, Camera);

            D3D.EndScene();
            return true;
        }

        private void ClearRenderStack()
        {
            foreach (KeyValuePair<int, IRenderer> asset in InitObjectStack)
            {
                asset.Value.CachedGraphics = this;
                asset.Value.InitBuffers(D3D.Device, D3D.DeviceContext);

                if (asset.Value is RenderBoundingBox)
                {
                    BBoxBatch.AddObject(asset.Key, asset.Value);
                }
                else if (asset.Value is RenderLine)
                {
                    LineBatch.AddObject(asset.Key, asset.Value);
                }
                else
                {
                    Assets.Add(asset.Key, asset.Value);
                }
            }

            InitObjectStack.Clear();
        }

        public void SelectEntry(int id)
        {
            IRenderer NewObject = GetAsset(id);
            IRenderer OldObject = GetAsset(selectedID);

            if (selectedID == id)
            {
                return;
            }

            if (NewObject != null)
            {
                if (OldObject != null)
                {
                    OldObject.Unselect();
                }

                TranslationGizmo.OnSelectEntry(NewObject.Transform, true);
                NewObject.Select();
                selectionBox.SetVisibility(true);
                selectionBox.SetTransform(NewObject.Transform);
                selectionBox.Update(NewObject.BoundingBox);
                selectedID = id;
            }
        }

        public IRenderer GetAsset(int RefID)
        {
            if (Assets.ContainsKey(RefID))
            {
                return Assets[RefID];
            }

            IRenderer ProxyObject = OurProxyRenderManager.GetObject(RefID);
            if(ProxyObject != null)
            {
                return ProxyObject;
            }

            IRenderer ObjectInPrimitive = OurPrimitiveManager.GetObject(RefID);
            if(ObjectInPrimitive != null)
            {
                return ObjectInPrimitive;
            }

            return OurPrimitiveManager.GetObject(RefID);
        }

        public bool DeleteAsset(int RefID)
        {
            if (Assets.ContainsKey(RefID))
            {
                return Assets.Remove(RefID);
            }

            // TODO: The owner if a 'PrimitiveBatch' is pretty ambiguous right now.
            return OurPrimitiveManager.RemoveObject(RefID);
        }

        public void SetAssetVisibility(int RefID, bool bVisibility)
        {
            IRenderer ObjectAsset = GetAsset(RefID);
            if (ObjectAsset != null)
            {
                ObjectAsset.SetVisibility(bVisibility);
            }
        }

        public void MoveGizmo(int sx, int sy, int Width, int Height)
        {
            TranslationGizmo.ManipulateGizmo(Camera, sx, sy, Width, Height);
        }

        public void OnResize(int width, int height)
        {
            Camera.SetProjectionMatrix(width, height);
        }

        public void RotateCamera(float deltaX, float deltaY)
        {
            Camera.Pitch(deltaY);
            Camera.Yaw(deltaX);
        }

        private void OnSelectedObjectHasUpdated(object Sender, UpdateSelectedEventArgs Args)
        {
            if(selectedID == Args.RefID)
            {
                IRenderer RenderAsset = GetAsset(Args.RefID);
                selectionBox.SetTransform(RenderAsset.Transform);
                selectionBox.Update(RenderAsset.BoundingBox);

                // TODO: Improve this. We're not actually selecting an entry.
                // Gizmo should be scrapped and re-attempted.
                Matrix4x4 TempTransform = Matrix4x4.Identity;
                TempTransform.Translation = selectionBox.Transform.Translation;
                TranslationGizmo.OnSelectEntry(TempTransform, true);
            }
        }

        public void Shutdown()
        {
            WorldSettings.Shutdown();
            WorldSettings = null;
            Camera = null;

            foreach (IRenderer RenderAsset in Assets.Values)
            {
                RenderAsset.Shutdown();
            }

            foreach (SpatialGrid grid in navigationGrids)
            {
                grid.Shutdown();
            }

            navigationGrids = null;
            translokatorGrid?.Shutdown();
            translokatorGrid = null;
            selectionBox.Shutdown();
            selectionBox = null;
            TranslationGizmo.Shutdown();
            TranslationGizmo = null;
            clouds?.Shutdown();
            clouds = null;
            sky?.Shutdown();
            sky = null;

            Assets = null;
            OurPrimitiveManager?.Shutdown();
            OurPrimitiveManager = null;
            OurProxyRenderManager?.Shutdown();
            OurProxyRenderManager = null;
            OurBufferManager?.Shutdown();
            OurBufferManager = null;
            OurCollisionManager?.Shutdown();
            OurCollisionManager = null;

            D3D?.Shutdown();
            D3D = null;
        }

        public void ToggleD3DFillMode() => D3D.ToggleFillMode();
        public void ToggleD3DCullMode() => D3D.ToggleCullMode();
    }
}