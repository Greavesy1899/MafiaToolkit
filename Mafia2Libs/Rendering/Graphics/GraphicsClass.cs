using Rendering.Input;
using System;
using System.Windows.Forms;
using SharpDX;
using System.Collections.Generic;
using Utils.Settings;
using System.IO;
using Rendering.Sys;

namespace Rendering.Graphics
{
    public class GraphicsClass
    {
        public FPSClass FPS { get; set; }
        public InputClass Input { get; private set; }
        public Camera Camera { get; set; }

        public Dictionary<int, IRenderer> Assets { get; private set; }
        public Dictionary<int, IRenderer> InitObjectStack { get; set; }
        public RenderBoundingBox SelectedEntryBBox { get; private set; }
        public RenderLine SelectedEntryLine { get; private set; }
        private int selectedEntryLineID;
        public RenderBoundingBox PickingRayBBox { get; private set; }

        private RenderLine[] SplineStorage;
        private DirectX11Class D3D;
        private LightClass Light;
        public TimerClass Timer;

        public GraphicsClass()
        {
            Assets = new Dictionary<int, IRenderer>();
            PickingRayBBox = new RenderBoundingBox();
        }

        public bool PreInit(IntPtr WindowHandle)
        {
            D3D = new DirectX11Class();
            if (!D3D.Init(WindowHandle))
            {
                MessageBox.Show("Failed to initialize DirectX11!");
                return false;
            }

            Timer = new TimerClass();
            FPS = new FPSClass();

            Timer.Init();
            FPS.Init();

            if (!RenderStorageSingleton.Instance.ShaderManager.Init(D3D.Device))
            {
                MessageBox.Show("Failed to initialize Shader Manager!");
                return false;
            }
            PickingRayBBox.Init(new BoundingBox(new Vector3(-5, -5, -5), new Vector3(5, 5, 5)));
            PickingRayBBox.InitBuffers(D3D.Device);
            //this is backup!
            RenderStorageSingleton.Instance.TextureCache.Add(0, TextureLoader.LoadTexture(D3D.Device, Path.Combine(ToolkitSettings.TexturePath, "texture.dds")));
            RenderStorageSingleton.Instance.TextureCache.Add(1, TextureLoader.LoadTexture(D3D.Device, Path.Combine(ToolkitSettings.TexturePath, "OM_3Bpatro_WALLSA.dds")));
            return true;
        }

        public bool InitScene()
        {
            Camera = new Camera();
            Camera.Position = new Vector3(0, 0, 15);
            Camera.SetProjectionMatrix();
            ClearRenderStack();
            Light = new LightClass();
            Light.SetAmbientColor(0.5f, 0.5f, 0.5f, 1f);
            Light.SetDiffuseColour(0f, 0f, 0f, 0);
            Light.Direction = new Vector3(0, 0, 1.0f);
            Light.SetSpecularColor(1.0f, 1.0f, 1.0f, 1.0f);
            Light.SetSpecularPower(255.0f);
            Input = new InputClass();
            Input.Init();
            return true;
        }
        public void Shutdown()
        {
            Camera = null;
            Timer = null;
            Light = null;

            foreach (KeyValuePair<int, IRenderer> model in Assets)
                model.Value?.Shutdown();

            if (SelectedEntryBBox != null)
                SelectedEntryBBox.Shutdown();

            Assets = null;
            D3D?.Shutdown();
            D3D = null;
        }
        public bool Frame()
        {
            ClearRenderStack();
            return Render();
        }
        public bool Render()
        {
            D3D.BeginScene(0.0f, 0f, 0f, 1.0f);
            Camera.Render();

            foreach(KeyValuePair<ulong, BaseShader> shader in RenderStorageSingleton.Instance.ShaderManager.shaders)
                shader.Value.InitCBuffersFrame(D3D.DeviceContext, Camera, Light);

            foreach(KeyValuePair<int, IRenderer> entry in Assets)
                entry.Value.Render(D3D.Device, D3D.DeviceContext, Camera, Light);

            if (SelectedEntryBBox != null)
                SelectedEntryBBox.Render(D3D.Device, D3D.DeviceContext, Camera, Light);

            if (SelectedEntryLine != null)
                SelectedEntryLine.Render(D3D.Device, D3D.DeviceContext, Camera, Light);

            PickingRayBBox.Render(D3D.Device, D3D.DeviceContext, Camera, Light);
            D3D.EndScene();
            return true;
        }

        private void ClearRenderStack()
        {
            foreach (KeyValuePair<int, IRenderer> asset in InitObjectStack)
            {
                asset.Value.InitBuffers(D3D.Device);
                Assets.Add(asset.Key, asset.Value);
            }
            InitObjectStack.Clear();
        }

        public void UpdateSplineStorage(ResourceTypes.Navigation.SplineDefinition[] splines)
        {
            SplineStorage = new RenderLine[splines.Length];

            for(int i = 0; i != SplineStorage.Length; i++)
            {
                RenderLine line = new RenderLine();
                SelectedEntryLine.SetTransform(new Vector3(), new Utils.Types.Matrix33());
                SelectedEntryLine.SetColour(new Vector4(0.0f, 1.0f, 0.0f, 1.0f));
                SelectedEntryLine.Init(splines[i].points);
                SelectedEntryLine.InitBuffers(D3D.Device);
                SplineStorage[i] = line;
            }
        }

        public void BuildSelectedEntry(int id)
        {
            IRenderer obj;
            Assets.TryGetValue(id, out obj);

            if (obj == null)
                return;

            if (obj.GetType() == typeof(RenderLine))
            {
                RenderLine line = (obj as RenderLine);
                if (SelectedEntryLine != null)
                {
                    SelectedEntryLine.Shutdown();
                    SelectedEntryLine = null;
                    Assets[selectedEntryLineID].DoRender = true;
                }

                if (line != null)
                {
                    SelectedEntryLine = new RenderLine();
                    SelectedEntryLine.SetTransform(new Vector3(), new Utils.Types.Matrix33());
                    selectedEntryLineID = id;
                    Assets[selectedEntryLineID].DoRender = false;
                    SelectedEntryLine.SetColour(new Vector4(0.0f, 1.0f, 0.0f, 1.0f));
                    SelectedEntryLine.Init(line.RawPoints);
                    SelectedEntryLine.InitBuffers(D3D.Device);
                }
            }
            else if(obj.GetType() == typeof(RenderStaticCollision))
            {
                RenderStaticCollision collision = (obj as RenderStaticCollision);
                if (SelectedEntryBBox != null)
                {
                    SelectedEntryBBox.Shutdown();
                    SelectedEntryBBox = null;
                }

                if (collision != null)
                {
                    SelectedEntryBBox = new RenderBoundingBox();
                    SelectedEntryBBox.SetTransform(collision.Transform);
                    SelectedEntryBBox.SetColour(new Vector4(1.0f, 0.0f, 0.0f, 1.0f));
                    SelectedEntryBBox.Init(collision.BoundingBox.BBox);
                    SelectedEntryBBox.InitBuffers(D3D.Device);
                }
            }
            else if (obj.GetType() == typeof(RenderModel))
            {
                RenderModel mesh = (obj as RenderModel);
                if (SelectedEntryBBox != null)
                {
                    SelectedEntryBBox.Shutdown();
                    SelectedEntryBBox = null;
                }

                if (mesh != null)
                {
                    SelectedEntryBBox = new RenderBoundingBox();
                    SelectedEntryBBox.SetTransform(mesh.Transform);
                    SelectedEntryBBox.Init(mesh.BoundingBox.BBox);
                    SelectedEntryBBox.InitBuffers(D3D.Device);
                }
            }
            else if(obj.GetType() == typeof(RenderRoad))
            {
                RenderRoad road = (obj as RenderRoad);
                if (SelectedEntryLine != null)
                {
                    SelectedEntryLine.Shutdown();
                    SelectedEntryLine = null;
                    (Assets[selectedEntryLineID] as RenderRoad).Spline.DoRender = true;
                }

                if (road.Spline != null)
                {
                    SelectedEntryLine = new RenderLine();
                    SelectedEntryLine.SetTransform(new Vector3(), new Utils.Types.Matrix33());
                    selectedEntryLineID = id;
                    road.Spline.DoRender = false;
                    SelectedEntryLine.SetColour(new Vector4(0.0f, 1.0f, 0.0f, 1.0f));
                    SelectedEntryLine.Init(road.Spline.RawPoints);
                    SelectedEntryLine.InitBuffers(D3D.Device);
                }
            }
        }

        public void ToggleD3DFillMode()
        {
            D3D.ToggleFillMode();
        }

        public void ToggleD3DCullMode()
        {
            D3D.ToggleCullMode();
        }
    }
}