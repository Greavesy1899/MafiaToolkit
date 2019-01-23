//DEPRECATED CLASS.
//This is scrapped. When taken from the old project, it was merged into a form.
using ModelViewer.Programming.GraphicClasses;
using ModelViewer.Programming.InputClasses;
using SharpDX.Windows;
using System.Drawing;
using System.Windows.Forms;
using System;
using System.Diagnostics;

namespace ModelViewer.Programming.SystemClasses
{
    public class SystemClass
    {
        private RenderForm RenderForm { get; set; }
        public SystemConfigClass Config { get; private set; }
        public InputClass Input { get; private set; }
        public GraphicsClass Graphics { get; set; }
        public TimerClass Timer { get; private set; }

        private Point mousePos;

        public SystemClass()
        { }

        public static void StartRenderForm(string title, int width, int height, bool Vsync, string meshName, IntPtr handle, bool fullscreen = true, int testTimeSeconds = 0)
        {
            SystemClass System = new SystemClass();
            System.Init(title, width, height, Vsync, meshName, handle, fullscreen,  testTimeSeconds);
            System.RunRenderForm();
        }

        public virtual bool Init(string title, int width, int height, bool Vsync, string meshName, IntPtr handle, bool fullscreen, int testTimeSeconds)
        {
            bool result = false;

            if (Config == null)
            {
                Config = new SystemConfigClass(title, width, height, fullscreen, Vsync);
            }

            //InitWindows(title);

            if(Input == null)
            {
                Input = new InputClass();
                Input.Init();
            }

            if (Graphics == null)
            {
                Graphics = new GraphicsClass();
                result = Graphics.Init(Config, handle);
            }
            return result;
        
        }

        private void InitWindows(string title)
        {
            int Width = Screen.PrimaryScreen.Bounds.Width;
            int Height = Screen.PrimaryScreen.Bounds.Height;

            RenderForm = new RenderForm(title)
            {
                ClientSize = new Size(Config.Width, Config.Height),
                FormBorderStyle = SystemConfigClass.BorderStyle
            };
            RenderForm.Show();
            RenderForm.Location = new Point((Width / 2) - (Config.Width / 2), (Height / 2) - (Config.Height / 2));
        }
        private void RunRenderForm()
        {
            //RenderForm.KeyDown += (s, e) => Input.KeyDown(e.KeyCode);
            //RenderForm.KeyUp += (s, e) => Input.KeyUp(e.KeyCode);
            //RenderForm.MouseDown += (s, e) => Input.ButtonDown(e.Button);
            //RenderForm.MouseUp += (s, e) => Input.ButtonUp(e.Button);
            //RenderForm.MouseMove += RenderForm_MouseMove;
            //RenderForm.Resize += RenderForm_Resize;
            RenderLoop.Run(RenderForm, () => { if (!Frame()) Shutdown(); });
        }

        private void RenderForm_Resize(object sender, EventArgs e)
        {
            //bugged
            //Graphics.ResizeD3D(RenderForm.Size.Width, RenderForm.Size.Height);
        }

        private void RenderForm_MouseMove(object sender, MouseEventArgs e)
        {
            mousePos = e.Location;
        }

        public bool Frame()
        {
            //if (Input.IsButtonDown(MouseButtons.Left))
            //{
            //    var dx = 0.25f * (mousePos.X - _lastMousePos.X));
            //    var dy = 0.25f * (mousePos.Y - _lastMousePos.Y));

            //    Graphics.Camera.Pitch(dy);
            //    Graphics.Camera.Yaw(dx);
            //}

            if (Input.IsKeyDown(Keys.A))
            {
                Graphics.Camera.Position.X += 1f;
            }
            else if (Input.IsKeyDown(Keys.D))
            {
                Graphics.Camera.Position.X -= 1f;
            }
            else if (Input.IsKeyDown(Keys.W))
            {
                Graphics.Camera.Position.Y += 1f;
            }
            else if (Input.IsKeyDown(Keys.S))
            {
                Graphics.Camera.Position.Y -= 1f;
            }
            else if (Input.IsKeyDown(Keys.Q))
            {
                Graphics.Camera.Position.Z += 1f;
            }
            else if (Input.IsKeyDown(Keys.E))
            {
                Graphics.Camera.Position.Z -= 1f;
            }
            else if(Input.IsKeyDown(Keys.D1))
            {
                Graphics.ToggleD3DCullMode();
            }
            else if (Input.IsKeyDown(Keys.D2))
            {
                Graphics.ToggleD3DFillMode();
            }
            Graphics.Timer.Frame2();
            return Graphics.Frame();
        }
        public void Shutdown()
        {
            ShutdownWindows();
            Timer = null;
            Graphics?.Shutdown();
            Graphics = null;
            Input = null;
            Config = null;
        }
        private void ShutdownWindows()
        {
            RenderForm?.Dispose();
            RenderForm = null;
        }
    }
}
