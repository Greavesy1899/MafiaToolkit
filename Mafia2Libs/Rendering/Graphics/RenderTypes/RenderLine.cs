using System.Numerics;
using System.Runtime.CompilerServices;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Color = System.Drawing.Color;

namespace Rendering.Graphics
{
    public class RenderLine : IRenderer
    {
        public Vector3[] Points
        {
            get { return points; }
            set
            {
                points = value;
                UpdateVertices();
            }
        }

        public ushort[] GetStrippedIndices { get { return InternalGetStrippedIndices(); } }

        public VertexLayouts.BasicLayout.Vertex[] GetVertices { get { return vertices; } }

        private Vector3[] points;
        private VertexLayouts.BasicLayout.Vertex[] vertices;
        private Color SelectedColour;
        private Color UnselectedColour;

        public RenderLine()
        {
            DoRender = true;
            shader = RenderStorageSingleton.Instance.ShaderManager.shaders[1];
            Transform = Matrix4x4.Identity;
            SelectedColour = Color.Blue;
            UnselectedColour = Color.Red;
            points = new Vector3[0];
            vertices = new VertexLayouts.BasicLayout.Vertex[0];
        }

        public void InitSwap(Vector3[] points)
        {
            for (int i = 0; i < points.Length; i++)
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

        public void UpdateVertices(bool bSelected = false)
        {
            vertices = new VertexLayouts.BasicLayout.Vertex[points.Length];

            var color = (bSelected ? SelectedColour : UnselectedColour).ToArgb();

            for (int i = 0; i != vertices.Length; i++)
            {
                VertexLayouts.BasicLayout.Vertex vertex = new VertexLayouts.BasicLayout.Vertex();
                vertex.Position = points[i];
                vertex.Colour = color;
                vertices[i] = vertex;
            }
            bIsUpdatedNeeded = true;
        }

        public void SetSelectedColour(Color color)
        {
            SelectedColour = color;
        }
        public void SetUnselectedColour(Color color)
        {
            UnselectedColour = color;
        }

        public override void InitBuffers(ID3D11Device d3d, ID3D11DeviceContext context)
        {
            if (vertices.Length != 0)
            {
                vertexBuffer = d3d.CreateBuffer(BindFlags.VertexBuffer, vertices, 0, ResourceUsage.Dynamic, CpuAccessFlags.Write);
            }
        }

        public override void Render(ID3D11Device device, ID3D11DeviceContext deviceContext, Camera camera)
        {
            if (!DoRender)
            {
                return;
            }
            if (!camera.CheckBBoxFrustum(Transform, BoundingBox))
                return;

            VertexBufferView VertexBufferView = new VertexBufferView(vertexBuffer, Unsafe.SizeOf<VertexLayouts.BasicLayout.Vertex>(), 0);
            deviceContext.IASetVertexBuffers(0, VertexBufferView);
            deviceContext.IASetPrimitiveTopology(PrimitiveTopology.LineStrip);

            shader.SetSceneVariables(deviceContext, Transform, camera);
            shader.Render(deviceContext, PrimitiveTopology.LineStrip, vertices.Length, 0);
        }

        public ushort[] InternalGetStrippedIndices()
        {
            ushort[] Indices = new ushort[points.Length];
            for (ushort i = 0; i < points.Length; i++)
            {
                Indices[i] = i;
            }

            return Indices;
        }

        public override void SetTransform(Matrix4x4 matrix)
        {
            this.Transform = matrix;
        }

        public override void Shutdown()
        {
            vertices = null;
            vertexBuffer?.Dispose();
            vertexBuffer = null;
        }

        public override void UpdateBuffers(ID3D11Device device, ID3D11DeviceContext deviceContext)
        {
            if (vertexBuffer != null)
            {
                vertexBuffer.Dispose();
                vertexBuffer = null;
            }
            InitBuffers(device, deviceContext);
            bIsUpdatedNeeded = false;
        }

        public override void Select()
        {
            UpdateVertices(true);
            bIsUpdatedNeeded = true;
        }

        public override void Unselect()
        {
            UpdateVertices();
            bIsUpdatedNeeded = true;
        }
    }
}