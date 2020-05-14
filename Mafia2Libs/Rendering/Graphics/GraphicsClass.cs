using Rendering.Input;
using System;
using System.Windows.Forms;
using SharpDX;
using System.Collections.Generic;
using Utils.Settings;
using System.IO;
using Rendering.Sys;
using Utils.Models;

namespace Rendering.Graphics
{
    public class GraphicsClass
    {
        public FPSClass FPS { get; set; }
        public InputClass Input { get; private set; }
        public Camera Camera { get; set; }

        public Dictionary<int, IRenderer> Assets { get; private set; }
        public Dictionary<int, IRenderer> InitObjectStack { get; set; }

        public TimerClass Timer;

        private int selectedID;
        private DirectX11Class D3D;
        public LightClass Light;

        public GraphicsClass()
        {
            InitObjectStack = new Dictionary<int, IRenderer>();
            Assets = new Dictionary<int, IRenderer>();
        }

        public bool PreInit(IntPtr WindowHandle)
        {
            D3D = new DirectX11Class();
            if (!D3D.Init(WindowHandle))
            {
                MessageBox.Show("Failed to initialize DirectX11!");
            }

            Timer = new TimerClass();
            FPS = new FPSClass();

            Timer.Init();
            FPS.Init();

            if(!RenderStorageSingleton.Instance.IsInitialised())
            {
                bool result = RenderStorageSingleton.Instance.Initialise(D3D);
                var structure = new M2TStructure();
                //import gizmo
                RenderModel model = new RenderModel();
                structure.ReadFromM2T("Resources/GizmoModel.m2t");
                model.ConvertMTKToRenderModel(structure);
                model.InitBuffers(D3D.Device, D3D.DeviceContext);
                model.DoRender = false;

                RenderModel sky = new RenderModel();
                structure = new M2TStructure();
                structure.ReadFromM2T("Resources/sky_backdrop.m2t");
                sky.ConvertMTKToRenderModel(structure);
                sky.InitBuffers(D3D.Device, D3D.DeviceContext);
                sky.DoRender = false;
                Assets.Add(1, sky);

                RenderModel clouds = new RenderModel();
                structure = new M2TStructure();
                structure.ReadFromM2T("Resources/weather_clouds.m2t");
                clouds.ConvertMTKToRenderModel(structure);
                clouds.InitBuffers(D3D.Device, D3D.DeviceContext);
                clouds.DoRender = false;
                Assets.Add(2, clouds);
            }

            return true;
        }

        public bool InitScene(int width, int height)
        {
            Camera = new Camera();
            Camera.Position = new Vector3(0, 0, 15);
            Camera.SetProjectionMatrix(width, height);
            ClearRenderStack();
            Light = new LightClass();
            Light.SetAmbientColor(0.5f, 0.5f, 0.5f, 1f);
            Light.SetDiffuseColour(0.5f, 0.5f, 0.5f, 1f);
            Light.Direction = new Vector3(-0.2f, -1f, -0.3f);
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
            return Render();
        }
        public bool Render()
        {
            D3D.BeginScene(0.0f, 0f, 0f, 1.0f);
            Camera.Render();

            foreach (KeyValuePair<ulong, BaseShader> shader in RenderStorageSingleton.Instance.ShaderManager.shaders)
            {
                shader.Value.InitCBuffersFrame(D3D.DeviceContext, Camera, Light);
            }

            foreach (KeyValuePair<int, IRenderer> entry in Assets)
            {
                entry.Value.UpdateBuffers(D3D.Device, D3D.DeviceContext);
                entry.Value.Render(D3D.Device, D3D.DeviceContext, Camera, Light);
            }

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
            IRenderer newObj, oldObj, gizmo;
            bool foundNew = Assets.TryGetValue(id, out newObj);
            bool foundOld = Assets.TryGetValue(selectedID, out oldObj);
            gizmo = Assets[1];

            if (selectedID == id)
                return;

            if (foundNew)
            {
                if (foundOld)
                {
                    oldObj.Unselect();
                }

                gizmo.SetTransform(newObj.Transform);
                gizmo.DoRender = false;
                newObj.Select();
                selectedID = id;
            }
        }

        public void OnResize(int width, int height)
        {
            Camera.SetProjectionMatrix(width, height);
            //D3D.Resize(width, height);
        }

        public void ToggleD3DFillMode() => D3D.ToggleFillMode();
        public void ToggleD3DCullMode() => D3D.ToggleCullMode();
    }
}