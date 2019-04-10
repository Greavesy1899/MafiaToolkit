using Rendering.Utils;
using Rendering.Input;
using System;
using System.Windows.Forms;
using SharpDX;
using System.Collections.Generic;
using ResourceTypes.FrameResource;

namespace Rendering.Graphics
{
    public class GraphicsClass
    {
        private DirectX11Class D3D { get; set; }
        private LightClass Light { get; set; }
        public TimerClass Timer { get; set; }
        public InputClass Input { get; private set; }
        public Camera Camera { get; set; }

        public Dictionary<int, RenderModel> Models { get; private set; }
        public Dictionary<int, RenderBoundingBox> Areas { get; private set; }
        public Dictionary<int, RenderBoundingBox> Dummies { get; private set; }
        public Dictionary<int, IRenderer> InitObjectStack { get; set; }
        public RenderBoundingBox SelectedEntryBBox { get; private set; }

        public GraphicsClass()
        {
            Dummies = new Dictionary<int, RenderBoundingBox>();
            Areas = new Dictionary<int, RenderBoundingBox>();
            Models = new Dictionary<int, RenderModel>();
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
            if (!Timer.Init())
            {
                return false;
            }
            if (!RenderStorageSingleton.Instance.ShaderManager.Init(D3D.Device))
            {
                MessageBox.Show("Failed to initialize Shader Manager!");
                return false;
            }
            return true;
        }

        public bool InitScene()
        {
            Camera = new Camera();
            Camera.Position = new Vector3(0, 0, 15);
            Camera.SetProjectionMatrix();
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

            foreach (KeyValuePair<int, RenderModel> model in Models)
                model.Value?.Shutdown();

            foreach (KeyValuePair<int, RenderBoundingBox> area in Areas)
                area.Value?.Shutdown();

            foreach (KeyValuePair<int, RenderBoundingBox> dummy in Dummies)
                dummy.Value?.Shutdown();

            if (SelectedEntryBBox != null)
                SelectedEntryBBox.Shutdown();

            Models = null;
            Areas = null;
            Dummies = null;
            D3D?.Shutdown();
            D3D = null;
        }
        public bool Frame()
        {
            foreach (KeyValuePair<int, IRenderer> asset in InitObjectStack)
            {
                asset.Value.InitBuffers(D3D.Device);

                if (asset.Value.GetType() == typeof(RenderModel))
                    Models.Add(asset.Key, (RenderModel)asset.Value);
                else
                    Dummies.Add(asset.Key, (RenderBoundingBox)asset.Value);
            }
            InitObjectStack.Clear();
            return Render();
        }
        public bool Render()
        {
            D3D.BeginScene(0.0f, 0f, 0f, 1.0f);
            Camera.Render();

            foreach(KeyValuePair<int, RenderModel> entry in Models)
                entry.Value.Render(D3D.Device, D3D.DeviceContext, Camera, Light);

            foreach (KeyValuePair<int, RenderBoundingBox> dummy in Dummies)
                dummy.Value.Render(D3D.Device, D3D.DeviceContext, Camera, Light);

            foreach (KeyValuePair<int, RenderBoundingBox> entry in Areas)
                entry.Value.Render(D3D.Device, D3D.DeviceContext, Camera, Light);

            if (SelectedEntryBBox != null)
                SelectedEntryBBox.Render(D3D.Device, D3D.DeviceContext, Camera, Light);

            D3D.EndScene();
            return true;
        }

        public void BuildSelectedEntry(FrameObjectBase obj)
        {
            FrameObjectSingleMesh mesh = (obj as FrameObjectSingleMesh);

            if (SelectedEntryBBox != null)
            {
                SelectedEntryBBox.Shutdown();
                SelectedEntryBBox = null;
            }

            if (mesh != null)
            {
                SelectedEntryBBox = new RenderBoundingBox();
                SelectedEntryBBox.SetTransform(Models[mesh.RefID].Transform);
                SelectedEntryBBox.Init(mesh.Boundings);
                SelectedEntryBBox.InitBuffers(D3D.Device);
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