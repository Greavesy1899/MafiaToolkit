using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using Color = System.Drawing.Color;
using Buffer = SharpDX.Direct3D11.Buffer;
using Utils.Extensions;

namespace Rendering.Graphics
{
    public class RenderLine : IRenderer
    {

        private Vector3[] points;
        public Vector3[] Points {
            get { return points; }
            set {
                points = value;
                UpdateVertices();
            }
        }
        private VertexLayouts.BasicLayout.Vertex[] vertices;
        private Color SelectedColour;
        private Color UnselectedColour;

        public RenderLine()
        {
            DoRender = true;
            shader = RenderStorageSingleton.Instance.ShaderManager.shaders[1];
            Transform = Matrix.Identity;
            SelectedColour = Color.Blue;
            UnselectedColour = Color.Red;
            points = new Vector3[0];
            vertices = new VertexLayouts.BasicLayout.Vertex[0];
        }

        public void InitSwap(Vector3[] points)
        {
            for(int i = 0; i < points.Length; i++)
            {
                Vector3 pos = points[i];
                float y = pos.Y;
                pos.Y = -pos.Z;
                pos.Z = y;
                points[i] = pos;
            }
            Init(points);
        }
        public void Init(Vector3[] points)
        {
            this.points = points;
            UpdateVertices();
        }

        public void UpdateVertices(bool selected = false)
        {
            vertices = new VertexLayouts.BasicLayout.Vertex[points.Length];

            var color = (selected ? SelectedColour : UnselectedColour).ToArgb();

            for (int i = 0; i != vertices.Length; i++)
            {
                VertexLayouts.BasicLayout.Vertex vertex = new VertexLayouts.BasicLayout.Vertex();
                vertex.Position = points[i];
                vertex.Colour = color;
                vertices[i] = vertex;
            }
            isUpdatedNeeded = true;
        }

        public void SetSelectedColour(Color color)
        {
            SelectedColour = color;
        }
        public void SetUnselectedColour(Color color)
        {
            UnselectedColour = color;
        }

        public override void InitBuffers(Device d3d, DeviceContext context)
        {
            if (vertices.Length != 0)
            {
                vertexBuffer = Buffer.Create(d3d, BindFlags.VertexBuffer, vertices, 0, ResourceUsage.Dynamic, CpuAccessFlags.Write);
            }
        }

        public override void Render(Device device, DeviceContext deviceContext, Camera camera)
        {
            if (!DoRender)
                return;

            deviceContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertexBuffer, Utilities.SizeOf<VertexLayouts.BasicLayout.Vertex>(), 0));
            deviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.LineStrip;

            shader.SetSceneVariables(deviceContext, Transform, camera);
            shader.Render(deviceContext, PrimitiveTopology.LineStrip, vertices.Length, 0);
        }

        public override void SetTransform(Matrix matrix)
        {
            this.Transform = matrix;
        }

        public override void Shutdown()
        {
            vertices = null;
            vertexBuffer?.Dispose();
            vertexBuffer = null;
        }

        public override void UpdateBuffers(Device device, DeviceContext deviceContext)
        {
            if(vertexBuffer != null)
            {
                vertexBuffer.Dispose();
                vertexBuffer = null;
            }
            InitBuffers(device, deviceContext);
            isUpdatedNeeded = false;
        }

        public override void Select()
        {
            UpdateVertices(true);
            isUpdatedNeeded = true;
        }

        public override void Unselect()
        {
            UpdateVertices();
            isUpdatedNeeded = true;
        }
    }
}
