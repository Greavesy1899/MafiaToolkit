using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Color = System.Drawing.Color;

namespace Rendering.Graphics
{
    public class RenderPlane3D : IRenderer
    {

        private VertexLayouts.BasicLayout.Vertex[] vertices;
        private ushort[] indices;
        private Color UnselectedColour;

        private static Color[] UnselectedColours =
        {
            Color.Red,
            Color.Blue,
            Color.Green,
            Color.White,
            Color.Gray,
            Color.Yellow,
            Color.Brown,
            Color.Turquoise,
            Color.Beige,
            Color.DarkRed,
            Color.DarkGreen,
            Color.DarkBlue
        };

        public RenderPlane3D()
        {
            DoRender = true;
            shader = RenderStorageSingleton.Instance.ShaderManager.shaders[1];
            Transform = Matrix4x4.Identity;
            UnselectedColour = Color.Red;
            vertices = new VertexLayouts.BasicLayout.Vertex[0];
            indices = new ushort[0];
        }

        public void InitPlanes(Vector4[] InPlanes, Matrix4x4 InMatrix)
        {
            vertices = new VertexLayouts.BasicLayout.Vertex[InPlanes.Length * 4];
            indices = new ushort[InPlanes.Length * 6];

            uint VertexOffset = 0;
            uint IndexOffset = 0;
            for(int i = 0; i < InPlanes.Length; i++)
            {
                Vector4 CurrentPlane = InPlanes[i];
                InitPlane(CurrentPlane, InMatrix, VertexOffset, IndexOffset, UnselectedColours[i]);

                VertexOffset += 4;
                IndexOffset += 6;
            }
        }

        private void InitPlane(Vector4 InPlane, Matrix4x4 InMatrix, uint VertexOffset, uint IndexOffset, Color Colour)
        {
            Vector3 Position = InMatrix.Translation;
            Plane OurPlane = new Plane(InPlane);

            Vector3 ClosestPtOnPlane = Position - PlaneDot(InPlane, Position) * OurPlane.Normal;

            FindBestAxisVectors(OurPlane.Normal, out Vector3 U, out Vector3 V);
            U *= InPlane.W;
            V *= InPlane.W;

            vertices[VertexOffset + 0] = new VertexLayouts.BasicLayout.Vertex();
            vertices[VertexOffset + 0].Position = ClosestPtOnPlane + U + V;
            vertices[VertexOffset + 0].Colour = Colour.ToArgb();
            vertices[VertexOffset + 1] = new VertexLayouts.BasicLayout.Vertex();
            vertices[VertexOffset + 1].Position = ClosestPtOnPlane - U + V;
            vertices[VertexOffset + 1].Colour = Colour.ToArgb();
            vertices[VertexOffset + 2] = new VertexLayouts.BasicLayout.Vertex();
            vertices[VertexOffset + 2].Position = ClosestPtOnPlane + U - V;
            vertices[VertexOffset + 2].Colour = Colour.ToArgb();
            vertices[VertexOffset + 3] = new VertexLayouts.BasicLayout.Vertex();
            vertices[VertexOffset + 3].Position = ClosestPtOnPlane - U - V;
            vertices[VertexOffset + 3].Colour = Colour.ToArgb();

            indices[IndexOffset + 0] = (ushort)(VertexOffset + 0); indices[IndexOffset + 1] = (ushort)(VertexOffset + 2); indices[IndexOffset + 2] = (ushort)(VertexOffset + 1);
            indices[IndexOffset + 3] = (ushort)(VertexOffset + 1); indices[IndexOffset + 4] = (ushort)(VertexOffset + 2); indices[IndexOffset + 5] = (ushort)(VertexOffset + 3);
        }

        private float PlaneDot(Vector4 InPlane, Vector3 Location)
        {
            return InPlane.X * Location.X + InPlane.Y * Location.Y + InPlane.Z * Location.Z - InPlane.W;
        }

        private void FindBestAxisVectors(Vector3 Vector, out Vector3 Axis1, out Vector3 Axis2)
        {
            float NX = Math.Abs(Vector.X);
            float NY = Math.Abs(Vector.Y);
            float NZ = Math.Abs(Vector.Z);

            if (NZ > NX && NZ > NY) Axis1 = Vector3.UnitX;
            else Axis1 = Vector3.UnitZ;

            Axis1 = Vector3.Normalize(Axis1 - Vector * Vector3.Dot(Axis1, Vector));
            Axis2 = Vector3.Cross(Axis1, Vector);
        }

        public override void InitBuffers(ID3D11Device d3d, ID3D11DeviceContext context)
        {
            vertexBuffer = d3d.CreateBuffer(BindFlags.VertexBuffer, vertices, 0, ResourceUsage.Dynamic, CpuAccessFlags.Write);
            indexBuffer = d3d.CreateBuffer(BindFlags.IndexBuffer, indices, 0, ResourceUsage.Dynamic, CpuAccessFlags.Write);
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
            shader.Render(deviceContext, PrimitiveTopology.TriangleList, indices.Length, 0);
        }

        public override void Select()
        {
        }

        public override void SetTransform(Matrix4x4 matrix)
        {
            this.Transform = matrix;
        }

        public override void Shutdown()
        {
            vertices = null;
            indexBuffer = null;
            vertexBuffer?.Dispose();
            vertexBuffer = null;
            indexBuffer?.Dispose();
            indexBuffer = null;
        }

        public override void Unselect()
        {
        }

        public override void UpdateBuffers(ID3D11Device device, ID3D11DeviceContext deviceContext)
        {
            if (vertexBuffer != null)
            {
                vertexBuffer.Dispose();
                vertexBuffer = null;
            }

            if (indexBuffer != null)
            {
                indexBuffer.Dispose();
                indexBuffer = null;
            }

            InitBuffers(device, deviceContext);
            bIsUpdatedNeeded = false;
        }
    }
}
