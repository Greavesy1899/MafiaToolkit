using Rendering.Utils;
using Rendering.Input;
using System;
using System.Windows.Forms;
using SharpDX;
using System.Collections.Generic;

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
        public GraphicsClass() { }

        public bool Init(IntPtr WindowHandle)
        {
            D3D = new DirectX11Class();
            if (!D3D.Init(WindowHandle))
            {
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
                MessageBox.Show("Could not init ShaderManager!");
                return false;
            }
            Camera = new Camera();
            Camera.Position = new Vector3(0, 0, 15);
            Camera.SetProjectionMatrix();

            foreach (KeyValuePair<int, RenderModel> model in Models)
            {
                model.Value.Init(D3D.Device);
                model.Value.Shader = ShaderManager.shaders[0];
            }

            Light = new LightClass();
            Light.SetAmbientColor(0.75f, 0.75f, 0.75f, 1f);
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
            D3D.BeginScene(0f, 0f, 0f, 1.0f);
            Camera.Render();

            foreach (KeyValuePair<int, RenderModel> entry in Models)
            {
                RenderModel model = entry.Value;
                if (model.DoRender)
                {
                    //D3D.SwapFillMode(SharpDX.Direct3D11.FillMode.Solid);
                    model.Render(D3D.DeviceContext, Camera, Light);
                    //D3D.SwapFillMode(SharpDX.Direct3D11.FillMode.Wireframe);
                    //model.BoundingBox.Render(D3D.DeviceContext);
                    //D3D.DeviceContext.PixelShader.SetShaderResource(0, model.BoundingBox.Texture);
                    //D3D.DeviceContext.InputAssembler.SetIndexBuffer(model.BoundingBox.IndexBuffer, SharpDX.DXGI.Format.R16_UInt, 0);
                    //Shader.Render(D3D.DeviceContext, model.BoundingBox.Indices.Length);


                }
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