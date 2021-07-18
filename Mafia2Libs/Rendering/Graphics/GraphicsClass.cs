using Rendering.Input;
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using Rendering.Core;
using Utils.Models;
using ResourceTypes.Translokator;
using Utils.Settings;
using Utils.VorticeUtils;
using System.Numerics;
using Vortice.Mathematics;

namespace Rendering.Graphics
{
    public class UpdateSelectedEventArgs : EventArgs
    {
        public int RefID { get; set; }
    }

    public class GraphicsClass
    {
        public InputClass Input { get; private set; }
        public WorldSettings WorldSettings { get; set; }
        public Camera Camera { get; set; }
        public Dictionary<int, IRenderer> Assets { get; private set; }
        public Dictionary<int, IRenderer> InitObjectStack { get; set; }
        public Profiler Profile { get; set; }

        public EventHandler<UpdateSelectedEventArgs> OnSelectedObjectUpdated;

        private int selectedID;
        private RenderBoundingBox selectionBox;
        private RenderModel sky;
        private RenderModel clouds;
        private GizmoTool TranslationGizmo;

        private DirectX11Class D3D;

        private SpatialGrid translokatorGrid;
        private SpatialGrid[] navigationGrids;


        public GraphicsClass()
        {
            InitObjectStack = new Dictionary<int, IRenderer>();
            Profile = new Profiler();
            Assets = new Dictionary<int, IRenderer>();
            selectionBox = new RenderBoundingBox();
            translokatorGrid = new SpatialGrid();
            navigationGrids = new SpatialGrid[0];

            OnSelectedObjectUpdated += OnSelectedObjectHasUpdated;
        }

        public bool PreInit(IntPtr WindowHandle)
        {
            D3D = new DirectX11Class();
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
                RenderModel gizmo = new RenderModel();
                structure.ReadFromM2T("Resources/GizmoModel.m2t");
                gizmo.ConvertMTKToRenderModel(structure);
                gizmo.InitBuffers(D3D.Device, D3D.DeviceContext);
                gizmo.DoRender = true;
                TranslationGizmo = new GizmoTool(gizmo);

                sky = new RenderModel();
                structure = new M2TStructure();
                structure.ReadFromM2T("Resources/sky_backdrop.m2t");
                sky.ConvertMTKToRenderModel(structure);
                sky.InitBuffers(D3D.Device, D3D.DeviceContext);

                clouds = new RenderModel();
                structure = new M2TStructure();
                structure.ReadFromM2T("Resources/weather_clouds.m2t");
                clouds.ConvertMTKToRenderModel(structure);
                clouds.InitBuffers(D3D.Device, D3D.DeviceContext);
                clouds.DoRender = false;
            }

            selectionBox.SetColour(System.Drawing.Color.Red);
            selectionBox.Init(new BoundingBox(new Vector3(0.5f), new Vector3(-0.5f)));          
            selectionBox.DoRender = false;
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
            sky.InitBuffers(D3D.Device, D3D.DeviceContext);
            sky.DoRender = WorldSettings.RenderSky;
            clouds.InitBuffers(D3D.Device, D3D.DeviceContext);
            Input = new InputClass();
            Input.Init();
            return true;
        }

        public TreeNode SetTranslokatorGrid(TranslokatorLoader translokator)
        {
            translokatorGrid = new SpatialGrid(translokator);
            translokatorGrid.Initialise(D3D.Device, D3D.DeviceContext);
            return translokatorGrid.GetTreeNodes();
        }

        public TreeNode SetNavigationGrid(ResourceTypes.Navigation.OBJData[] data)
        {
            TreeNode[] Grids = new TreeNode[data.Length];
            navigationGrids = new SpatialGrid[data.Length];

            for(int i = 0; i < navigationGrids.Length; i++)
            {
                navigationGrids[i] = new SpatialGrid(data[i].runtimeMesh);
                navigationGrids[i].Initialise(D3D.Device, D3D.DeviceContext);
                Grids[i] = navigationGrids[i].GetTreeNodes();
                Grids[i].Text = string.Format("Grid: {0}", i);
            }

            TreeNode Parent = new TreeNode("Navigation Grids");
            Parent.Nodes.AddRange(Grids);
            return Parent;
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
                grid?.Shutdown();
            }

            navigationGrids = null;
            translokatorGrid?.Shutdown();
            translokatorGrid = null;
            selectionBox.Shutdown();
            selectionBox = null;
            TranslationGizmo.Shutdown();
            TranslationGizmo = null;
            clouds.Shutdown();
            clouds = null;
            sky.Shutdown();
            sky = null;
            Assets = null;
            D3D?.Shutdown();
            D3D = null;
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
                RenderEntry.Render(D3D.Device, D3D.DeviceContext, Camera);
            }

            //navigationGrids[0].Render(D3D.Device, D3D.DeviceContext, Camera);
            foreach (var grid in navigationGrids)
            {
                grid.Render(D3D.Device, D3D.DeviceContext, Camera);
            }

            translokatorGrid.Render(D3D.Device, D3D.DeviceContext, Camera);
            selectionBox.UpdateBuffers(D3D.Device, D3D.DeviceContext);
            selectionBox.Render(D3D.Device, D3D.DeviceContext, Camera);
            TranslationGizmo.UpdateBuffers(D3D.Device, D3D.DeviceContext);
            TranslationGizmo.Render(D3D.Device, D3D.DeviceContext, Camera);
            clouds.UpdateBuffers(D3D.Device, D3D.DeviceContext);
            clouds.Render(D3D.Device, D3D.DeviceContext, Camera);
            sky.DoRender = WorldSettings.RenderSky;
            sky.UpdateBuffers(D3D.Device, D3D.DeviceContext);
            sky.Render(D3D.Device, D3D.DeviceContext, Camera);

            D3D.EndScene();
            return true;
        }

        private void ClearRenderStack()
        {
            foreach (KeyValuePair<int, IRenderer> asset in InitObjectStack)
            {
                asset.Value.InitBuffers(D3D.Device, D3D.DeviceContext);
                Assets.Add(asset.Key, asset.Value);
            }
            InitObjectStack.Clear();
        }

        public void SelectEntry(int id)
        {
            IRenderer newObj, oldObj;
            bool foundNew = Assets.TryGetValue(id, out newObj);
            bool foundOld = Assets.TryGetValue(selectedID, out oldObj);

            if (selectedID == id)
            {
                return;
            }

            if (foundNew)
            {
                if (foundOld)
                {
                    oldObj.Unselect();
                }

                TranslationGizmo.OnSelectEntry(newObj.Transform, true);
                newObj.Select();
                selectionBox.DoRender = true;
                selectionBox.SetTransform(newObj.Transform);
                selectionBox.Update(newObj.BoundingBox);
                selectedID = id;
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
                IRenderer RenderAsset = Assets[Args.RefID];
                selectionBox.SetTransform(RenderAsset.Transform);
                selectionBox.Update(RenderAsset.BoundingBox);
            }
        }

        public void ToggleD3DFillMode() => D3D.ToggleFillMode();
        public void ToggleD3DCullMode() => D3D.ToggleCullMode();
    }
}