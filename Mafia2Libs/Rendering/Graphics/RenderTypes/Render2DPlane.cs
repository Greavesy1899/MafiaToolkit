using ResourceTypes.Navigation.Traffic;
using System.Numerics;
using System.Runtime.CompilerServices;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Color = System.Drawing.Color;

namespace Rendering.Graphics
{
    public class Render2DPlane : IRenderer
    {
        private VertexLayouts.BasicLayout.Vertex[] Vertices;
        private ushort[] Indices;
        private Color colour;

        public Render2DPlane()
        {
            DoRender = true;
            shader = RenderStorageSingleton.Instance.ShaderManager.shaders[1];
            Transform = Matrix4x4.Identity;
            colour = Color.White;
        }

        public void Init(ILaneDefinition LaneDefinition, Vector3[] Points, float Width, float Offset, float zOffset, System.Drawing.Color InColour)
        {
            Vertices = new VertexLayouts.BasicLayout.Vertex[Points.Length * 2];
            int NumTris = 2 * (Points.Length - 1);
            Indices = new ushort[NumTris * 3];

            ushort VertIndex = 0;
            ushort TriIndex = 0;
            for (int i = 0; i < Points.Length; i++)
            {
                Vector3 TempForward = Vector3.Zero; // z is empty
                if (i < Points.Length - 1)
                {
                    TempForward += Points[i + 1 % Points.Length] - Points[i];
                }

                if (i > 0)
                {
                    TempForward += Points[i] - Points[i - 1 % Points.Length];
                }

                Vector2 FinalForward = Vector2.Normalize(new Vector2(TempForward.X, TempForward.Y));
                Vector3 Left = new Vector3(-FinalForward.Y, FinalForward.X, 0.0f); // Z is empty

                Vector3 v1 = Points[i] + Left * Width + Left * Offset;
                Vector3 v2 = Points[i] + Left * Offset;


                Vertices[VertIndex] = new VertexLayouts.BasicLayout.Vertex();
                Vertices[VertIndex].Colour = InColour.ToArgb();
                Vertices[VertIndex].Position = new Vector3(v1.X, v1.Y, Points[i].Z);

                Vertices[VertIndex + 1] = new VertexLayouts.BasicLayout.Vertex();
                Vertices[VertIndex + 1].Colour = InColour.ToArgb();
                Vertices[VertIndex + 1].Position = new Vector3(v2.X, v2.Y, Points[i].Z);

                if (i < Points.Length - 1)
                {
                    Indices[TriIndex + 2] = VertIndex;
                    Indices[TriIndex + 1] = (ushort)((VertIndex + 2) % Vertices.Length);
                    Indices[TriIndex + 0] = (ushort)(VertIndex + 1);

                    Indices[TriIndex + 5] = (ushort)(VertIndex + 1);
                    Indices[TriIndex + 4] = (ushort)((VertIndex + 2) % Vertices.Length);
                    Indices[TriIndex + 3] = (ushort)((VertIndex + 3) % Vertices.Length);
                }

                VertIndex += 2;
                TriIndex += 6;
            }
        }

        public void SetColour(Color newColor)
        {
            colour = newColor;
        }

        public override void InitBuffers(ID3D11Device d3d, ID3D11DeviceContext deviceContext)
        {
            vertexBuffer = d3d.CreateBuffer(BindFlags.VertexBuffer, Vertices);
            indexBuffer = d3d.CreateBuffer(BindFlags.IndexBuffer, Indices);
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
            deviceContext.IASetIndexBuffer(indexBuffer, Vortice.DXGI.Format.R16_UInt, 0);
            deviceContext.IASetPrimitiveTopology(PrimitiveTopology.TriangleList);

            shader.SetSceneVariables(deviceContext, Transform, camera);
            shader.Render(deviceContext, PrimitiveTopology.TriangleList, Indices.Length, 0);
        }

        public override void SetTransform(Matrix4x4 matrix)
        {
            this.Transform = matrix;
        }

        public override void Shutdown()
        {
            Vertices = null;
            Indices = null;
            indexBuffer?.Dispose();
            indexBuffer = null;
            vertexBuffer?.Dispose();
            vertexBuffer = null;
        }

        public override void UpdateBuffers(ID3D11Device device, ID3D11DeviceContext deviceContext)
        {
            if(bIsUpdatedNeeded)
            {
                for (int i = 0; i < Vertices.Length; i++)
                {
                    Vertices[i].Colour = colour.ToArgb();
                }

                if(vertexBuffer != null && indexBuffer != null)
                {
                    indexBuffer?.Dispose();
                    indexBuffer = null;
                    vertexBuffer?.Dispose();
                    vertexBuffer = null;
                }

                InitBuffers(device, deviceContext);
            }
        }

        public override void Select()
        {
            //TODO
        }

        public override void Unselect()
        {
            //TODO
        }
    }
}
