using System;
using System.Windows.Forms;
using System.IO;
using SharpDX;
using System.Linq;
using ModelViewer.Input;
using ModelViewer.System;

namespace ModelViewer.Graphics
{
    public class GraphicsClass
    {
        private DirectX11Class D3D { get; set; }
        private LightClass Light { get; set; }
        private MultiTextureShaderClass LightShader { get; set; }
        public TimerClass Timer { get; set; }
        public InputClass Input { get; private set; }
        private Camera Camera { get; set; }
        private ModelClass Model { get; set; }
        public static float Rotation { get; set; }
        public GraphicsClass() { }

        public bool Init(SystemConfigClass Config, IntPtr WindowHandle, string meshName)
        {
            try
            {
                D3D = new DirectX11Class();
                if (!D3D.Init(Config, WindowHandle))
                {
                    return false;
                }
                Timer = new TimerClass();
                if (!Timer.Init())
                {
                    return false;
                }
                Camera = new Camera();
                string Bounding = File.ReadLines(SystemConfigClass.DataFilePath + meshName + ".txt").Skip(1).First();
                string[] Textures = new string[2];
                Textures[0] = File.ReadLines(SystemConfigClass.DataFilePath + meshName + ".txt").Skip(3).First();
                Textures[1] = "EffectPsiColonypod-nm.dds";
                Camera.SetPosition(0, 0, float.Parse(Bounding) - float.Parse(Bounding) * 2 - 100);
                Model = new ModelClass();
                if (!Model.Init(D3D.Device, SystemConfigClass.DataFilePath + meshName + ".txt", Textures))
                {
                    MessageBox.Show("Unable to init model. Error from GraphicsClass.");
                    return false;
                }
                LightShader = new MultiTextureShaderClass();
                if (!LightShader.Init(D3D.Device, WindowHandle))
                {
                    MessageBox.Show("Could not initialize the texture shader object. Error from GraphicsClass.");
                    return false;
                }
                Light = new LightClass();
                Light.SetAmbientColor(0.15f, 0.15f, 0.15f, 1f);
                Light.SetDiffuseColour(1, 1, 1, 1);
                Light.SetDirection(0, 0, 1);
                Light.SetSpecularColor(1.0f, 1.0f, 1.0f, 1.0f);
                Light.SetSpecularPower(32.0f);
                Input = new InputClass();
                Input.Init();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to init Direct3d. The Error is \n" + ex.Message);
                return false;
            }
        }
        public void Shutdown()
        {
            Camera = null;
            Timer = null;
            Light = null;
            LightShader?.Shutdown();
            LightShader = null;
            Model?.Shutdown();
            Model = null;
            D3D?.Shutdown();
            D3D = null;
        }
        public bool Frame()
        {
            Rotate();
            return Render(Rotation);
        }
        public bool Render(float rotation)
        {
            D3D.BeginScene(0f, 0f, 0f, 1.0f);
            Camera.Render();
            Matrix ViewMatrix = Camera.ViewMatrix;
            Matrix WorldMatrix = D3D.WorldMatrix;
            Matrix ProjectionMatrix = D3D.ProjectionMatrix;

            Matrix.RotationY(rotation / 10, out WorldMatrix);

            Model.Render(D3D.DeviceContext);
            if (!LightShader.Render(D3D.DeviceContext, Model.IndexCount, WorldMatrix, ViewMatrix, ProjectionMatrix, Model.Textures.Select(item => item.TextureResource).ToArray()))
            {
                return false;
            }
            D3D.EndScene();
            return true;
        }
        public static void Rotate()
        {
            Rotation += (float)Math.PI * 0.001f;

            if (Rotation > 360)
                Rotation -= 100;
        }
    }
}