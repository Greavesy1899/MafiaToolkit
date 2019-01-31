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
        private ShaderClass Shader { get; set; }
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
            Camera = new Camera();
            Camera.Position = new Vector3(0, 0, 15);

            foreach (KeyValuePair<int, RenderModel> model in Models)
                model.Value.Init(D3D.Device);

            Shader = new ShaderClass();
            if (!Shader.Init(D3D.Device, WindowHandle))
            {
                MessageBox.Show("Could not initialize the texture shader object. Error from GraphicsClass.");
                return false;
            }
            Light = new LightClass();
            Light.SetAmbientColor(0.75f, 0.75f, 0.75f, 1f);
            Light.SetDiffuseColour(1f, 1f, 1f, 1);
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
            Shader?.Shutdown();
            Shader = null;

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
            Matrix ViewMatrix = Camera.ViewMatrix;

            foreach (KeyValuePair<int, RenderModel> entry in Models)
            {
                RenderModel model = entry.Value;
                if (model.DoRender)
                {
                    //D3D.SwapFillMode(SharpDX.Direct3D11.FillMode.Solid);
                    Matrix ProjectionMatrix = D3D.ProjectionMatrix;
                    Matrix WorldMatrix = model.Transform;
                    model.Render(D3D.DeviceContext);
                    if (!Shader.PrepareRender(D3D.DeviceContext, WorldMatrix, ViewMatrix, ProjectionMatrix, Light.Direction, Light.AmbientColor, Light.DiffuseColour, Camera.Position, Light.SpecularColor, Light.SpecularPower))
                    {
                        return false;
                    }

                    SharpDX.Direct3D11.ShaderResourceView[] resources = new SharpDX.Direct3D11.ShaderResourceView[2];
                    resources[1] = model.AOTexture;

                    for (int i = 0; i != model.ModelParts.Length; i++)
                    {
                        resources[0] = model.ModelParts[i].Texture;
                        D3D.DeviceContext.PixelShader.SetShaderResources(0, 2, resources);
                        Shader.Render(D3D.DeviceContext, (int)model.ModelParts[i].NumFaces*3, (int)model.ModelParts[i].StartIndex);
                    }

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

        public Matrix GetProjectionMatrix()
        {
            return D3D.ProjectionMatrix;
        }

        public Matrix GetWorldMatrix()
        {
            return D3D.WorldMatrix;
        }
    }
}