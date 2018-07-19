using Mafia2;
using System.Drawing;
using System.Windows.Forms;
using SharpDX.Windows;
using ModelViewer.Input;
using ModelViewer.Graphics;

namespace ModelViewer.System
{
    public class SystemClass
    {
        private RenderForm RenderForm { get; set; }
        public SystemConfigClass Config { get; private set; }
        public InputClass Input { get; private set; }
        public GraphicsClass Graphics { get; set; }
        public TimerClass Timer { get; private set; }

        public SystemClass() { }

        public static void StartRenderForm(string title, int width, int height, bool Vsync, CustomEDM model, bool fullscreen = true, int testTimeSeconds = 0)
        {
            SystemClass System = new SystemClass();
            System.Init(title, width, height, Vsync, model, fullscreen, testTimeSeconds);
            System.RunRenderForm();
        }

        public virtual bool Init(string title, int width, int height, bool Vsync, CustomEDM model, bool fullscreen, int testTimeSeconds)
        {
            bool result = false;

            if (Config == null)
            {
                Config = new SystemConfigClass(title, width, height, fullscreen, Vsync);
            }

            InitWindows(title);

            if (Input == null)
            {
                Input = new InputClass();
                Input.Init();
            }

            if (Graphics == null)
            {
                Graphics = new GraphicsClass();
                result = Graphics.Init(Config, RenderForm.Handle, model);
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
                FormBorderStyle = SystemConfigClass.BorderStyle,
                BackColor = Color.AliceBlue
            };
            RenderForm.Show();
            RenderForm.Location = new Point((Width / 2) - (Config.Width / 2), (Height / 2) - (Config.Height / 2));
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
