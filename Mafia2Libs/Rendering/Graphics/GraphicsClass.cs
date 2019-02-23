using Rendering.Utils;
using Rendering.Input;
using System;
using System.Windows.Forms;
using SharpDX;
using System.Collections.Generic;
using Mafia2Tool;
using ResourceTypes.FrameResource;
using Mafia2;

namespace Rendering.Graphics
{
    public class GraphicsClass
    {
        private DirectX11Class D3D { get; set; }
        private LightClass Light { get; set; }
        private ShaderManager ShaderManager { get; set; }
        public TimerClass Timer { get; set; }
        public InputClass Input { get; private set; }
        public Camera Camera { get; set; }
        public Dictionary<int, RenderModel> Models { get; set; }
        public static float Rotation { get; set; }
        public GraphicsClass()
        {
            Models = new Dictionary<int, RenderModel>();
        }

        public bool Init(IntPtr WindowHandle)
        {
            D3D = new DirectX11Class();
            if (!D3D.Init(WindowHandle))
            {
                MessageBox.Show("Failed to initialize DirectX11!");
                return false;
            }
            Timer = new TimerClass();
            if (!Timer.Init())
            {
                return false;
            }
            ShaderManager = new ShaderManager();
            if (!ShaderManager.Init(D3D.Device))
            {
                MessageBox.Show("Failed to initialize Shader Manager!");
                return false;
            }
            Camera = new Camera();
            Camera.Position = new Vector3(0, 0, 15);
            Camera.SetProjectionMatrix();

            foreach (KeyValuePair<int, RenderModel> model in Models)
            {
                model.Value.Init(D3D.Device, ShaderManager);
            }

            Light = new LightClass();
            Light.SetAmbientColor(0.5f, 0.5f, 0.5f, 1f);
            Light.SetDiffuseColour(0f, 0f, 0f, 0);
            Light.Direction = Camera.Position;
            Light.SetSpecularColor(1.0f, 1.0f, 1.0f, 1.0f);
            Light.SetSpecularPower(32.0f);
            Input = new InputClass();
            Input.Init();
            return true;
        }
        public void Shutdown()
        {
            Camera = null;
            Timer = null;
            Light = null;
            ShaderManager.Shutdown();
            ShaderManager = null;

            foreach (KeyValuePair<int, RenderModel> model in Models)
                model.Value?.Shutdown();

            Models = null;
            D3D?.Shutdown();
            D3D = null;
        }
        public bool Frame()
        {
            return Render();
        }
        public bool Render()
        {
            D3D.BeginScene(0.0f, 0f, 0f, 1.0f);
            Camera.Render();

            foreach(KeyValuePair<int, RenderModel> entry in Models)
            {
                D3D.SwapFillMode(SharpDX.Direct3D11.FillMode.Solid);
                entry.Value.Render(D3D.Device, D3D.DeviceContext, Camera, Light);
            }
            foreach(KeyValuePair<int, RenderModel> entry in Models)
            {
                D3D.SwapFillMode(SharpDX.Direct3D11.FillMode.Wireframe);
                entry.Value.BoundingBox.Render(D3D.Device, D3D.DeviceContext, entry.Value.Transform, Camera, Light);
            }
            D3D.EndScene();
            return true;
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