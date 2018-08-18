using ModelViewer.Programming.GraphicClasses;
using ModelViewer.Programming.InputClasses;
using SharpDX.Windows;
using System.Drawing;
using System.Windows.Forms;
using Mafia2Tool;

namespace ModelViewer.Programming.SystemClasses
{
    public class SystemClass
    {
        private RenderForm RenderForm { get; set; }
        public InputClass Input { get; private set; }
        public GraphicsClass Graphics { get; set; }
        public TimerClass Timer { get; private set; }

        public SystemClass()
        { }

        public static void StartRenderForm(string title, int width, int height, bool Vsync, string meshName, bool fullscreen = true, int testTimeSeconds = 0)
        {
            SystemClass System = new SystemClass();
            System.Init(title, width, height, Vsync, meshName, fullscreen, testTimeSeconds);
            System.RunRenderForm();
        }

        public virtual bool Init(string title, int width, int height, bool Vsync, string meshName, bool fullscreen, int testTimeSeconds)
        {
            bool result = false;

            InitWindows(title);

            if(Input == null)
            {
                Input = new InputClass();
                Input.Init();
            }

            if (Graphics == null)
            {
                Graphics = new GraphicsClass();
                result = Graphics.Init(RenderForm.Handle, meshName);
            }
            return result;
        
        }

        private void InitWindows(string title)
        {
            int Width = Screen.PrimaryScreen.Bounds.Width;
            int Height = Screen.PrimaryScreen.Bounds.Height;

            RenderForm = new RenderForm(title)
            {
                ClientSize = new Size(ToolkitSettings.Width, ToolkitSettings.Height),
                FormBorderStyle = (FormBorderStyle)ToolkitSettings.BorderStyle
            };
            RenderForm.Show();
            RenderForm.Location = new Point((Width / 2) - (ToolkitSettings.Width / 2), (Height / 2) - (ToolkitSettings.Height / 2));
        }
        private void RunRenderForm()
        {
            RenderForm.KeyDown += (s, e) => Input.KeyDown(e.KeyCode);
            RenderForm.KeyUp += (s, e) => Input.KeyDown(e.KeyCode);

            RenderLoop.Run(RenderForm, () => { if (!Frame()) Shutdown(); });
        }
        public bool Frame()
        {
            if (Input.IsKeyDown(Keys.Escape))
            {
                return false;
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
        }
        private void ShutdownWindows()
        {
            RenderForm?.Dispose();
            RenderForm = null;
        }
    }
}
