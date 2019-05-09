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
        public Dictionary<int, IRenderer> UpdateObjectStack { get; set; }

        public RenderBoundingBox PickingRayBBox { get; private set; }

        public TimerClass Timer;

        private int selectedID;
        private DirectX11Class D3D;
        private LightClass Light;

        public GraphicsClass()
        {
            InitObjectStack = new Dictionary<int, IRenderer>();
            UpdateObjectStack = new Dictionary<int, IRenderer>();
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

            Assets = null;
            D3D?.Shutdown();
            D3D = null;
        }
        public bool Frame()
        {
            ClearRenderStack();
            ClearUpdateStack();
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

        private void ClearUpdateStack()
        {
            foreach (KeyValuePair<int, IRenderer> asset in UpdateObjectStack)
                asset.Value.UpdateBuffers(D3D.DeviceContext);

            UpdateObjectStack.Clear();
        }

        public void SelectEntry(int id)
        {
            IRenderer newObj, oldObj;
            Assets.TryGetValue(id, out newObj);
            Assets.TryGetValue(selectedID, out oldObj);

            if (newObj != null)
            {
                if(oldObj != null)
                    oldObj.Unselect();

                newObj.Select();
                selectedID = id;
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