using System;
using System.Drawing;
using Rendering.Graphics;
using Rendering.Input;
using System.Windows.Forms;
using SharpDX.Windows;
using System.Threading;
using System.Collections.Generic;
using ResourceTypes.FrameResource;
using Utils.Types;

namespace Mafia2Tool
{
    public partial class RenderModelView : Form
    {
        private InputClass Input { get; set; }
        private GraphicsClass Graphics { get; set; }
        private Point mousePos;
        private Point lastMousePos;

        public RenderModelView(int refID, RenderModel model)
        {
            InitializeComponent();
            RenderPanel.Focus();
            StartD3DPanel();
            PopulateView(refID, model);
            Run();
        }

        public void PopulateView(int refID, RenderModel model)
        {
            model.SetTransform(new SharpDX.Vector3(0), new Matrix33());
            Graphics.Assets.Add(refID, model);
        }

        public void StartD3DPanel()
        {
            Init(RenderPanel.Handle);
        }

        public bool Init(IntPtr handle)
        {
            bool result = false;

            if (Input == null)
            {
                Input = new InputClass();
                Input.Init();
            }

            if (Graphics == null)
            {
                Graphics = new GraphicsClass();
                result = Graphics.PreInit(handle);
            }
            return result;
        }

        public void Run()
        {
            KeyDown += (s, e) => Input.KeyDown(e.KeyCode);
            KeyUp += (s, e) => Input.KeyUp(e.KeyCode);
            RenderPanel.MouseDown += (s, e) => Input.ButtonDown(e.Button);
            RenderPanel.MouseUp += (s, e) => Input.ButtonUp(e.Button);
            RenderPanel.MouseMove += RenderForm_MouseMove;
            RenderPanel.MouseEnter += RenderPanel_MouseEnter;
            RenderLoop.Run(this, () => { if (!Frame()) Shutdown(); });
        }

        public void Shutdown()
        {
            Graphics?.Shutdown();
            Graphics = null;
            Input = null;
        }

        public bool Frame()
        {
            if (RenderPanel.Focused)
            {
                if (Input.IsButtonDown(MouseButtons.Right))
                {
                    var dx = 0.25f * (mousePos.X - lastMousePos.X);
                    var dy = 0.25f * (mousePos.Y - lastMousePos.Y);

                    Graphics.Camera.Pitch(dy);
                    Graphics.Camera.Yaw(dx);
                }
            }
            lastMousePos = mousePos;
            Graphics.Timer.Frame2();
            Graphics.Frame();

            //awful i know
            if (Graphics.Timer.FrameTime < 1000 / 60)
            {
                Thread.Sleep((int)Math.Abs(Graphics.Timer.FrameTime - 1000 / 60));
            }

            return true;
        }

        private void RenderPanel_MouseEnter(object sender, EventArgs e)
        {
            RenderPanel.Focus();
        }

        private void RenderForm_MouseMove(object sender, MouseEventArgs e)
        {
            mousePos = new Point(e.Location.X, e.Location.Y);
        }
    }
}
