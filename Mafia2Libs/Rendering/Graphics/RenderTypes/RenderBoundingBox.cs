using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Vortice;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.Mathematics;
using Color = System.Drawing.Color;

namespace Rendering.Graphics
{
    public class RenderBoundingBox : IRenderer
    {
        private static readonly uint[] ReadOnlyIndices = {
                0, 1, 1, 2, 2, 3, 3, 0, // Front edges
                4, 5, 5, 6, 6, 7, 7, 4, // Back edges
                0, 4, 1, 5, 2, 6, 3, 7 // Side edges connecting front and back
        };

        public VertexLayouts.BasicLayout.Vertex[] Vertices { get { return vertices; } }
        public uint[] Indices { get { return ReadOnlyIndices; } }

        private VertexLayouts.BasicLayout.Vertex[] vertices;
        private Color CurrentColour;
        private Color UnselectedColour;

        public RenderBoundingBox()
        {
            DoRender = true;
            SetTransform(Matrix4x4.Identity);
            CurrentColour = Color.White;
            UnselectedColour = Color.White;
        }

        public bool InitSwap(BoundingBox bbox)
        {
            Vector3 NewMax = bbox.Max;
            float y = NewMax.Y;
            NewMax.Y = -NewMax.Z;
            NewMax.Z = y;

            Vector3 NewMin = bbox.Min;
            y = NewMin.Y;
            NewMin.Y = -NewMin.Z;
            NewMin.Z = y;

            return Init(new BoundingBox(NewMin, NewMax));
        }
        public bool Init(BoundingBox bbox)
        {
            BoundingBox = bbox;

            vertices = new VertexLayouts.BasicLayout.Vertex[8];

            Vector3[] corners = bbox.GetCorners();
            for (int i = 0; i < corners.Length; i++)
            {
                vertices[i] = new VertexLayouts.BasicLayout.Vertex();
                vertices[i].Position = corners[i];
                vertices[i].Colour = CurrentColour.ToArgb();
            }

            shader = RenderStorageSingleton.Instance.ShaderManager.shaders[1];
            return true;
        }

        public void Update(BoundingBox box)
        {
            bIsUpdatedNeeded = true;
            Init(box);
        }

        public override void InitBuffers(ID3D11Device d3d, ID3D11DeviceContext deviceContext)
        {
            vertexBuffer = d3d.CreateBuffer<VertexLayouts.BasicLayout.Vertex>(BindFlags.VertexBuffer, vertices, 0, ResourceUsage.Dynamic, CpuAccessFlags.Write);
            indexBuffer = d3d.CreateBuffer<uint>(BindFlags.IndexBuffer, Indices, 0, ResourceUsage.Dynamic, CpuAccessFlags.Write);
        }

        public void SetColour(Color newColour, bool update = false)
        {
            UnselectedColour = newColour;
            CurrentColour = UnselectedColour;

            bIsUpdatedNeeded = update;
        }

        public override void Render(DirectX11Class Dx11Object, Camera camera)
        {
            if (!DoRender)
            {
                return;
            }

            Dx11Object.DeviceContext.IASetVertexBuffer(0, vertexBuffer, Unsafe.SizeOf<VertexLayouts.BasicLayout.Vertex>());
            Dx11Object.DeviceContext.IASetIndexBuffer(indexBuffer, Vortice.DXGI.Format.R32_UInt, 0);
            Dx11Object.SetPrimitiveTopology(PrimitiveTopology.LineList);

            shader.SetSceneVariables(Dx11Object.DeviceContext, Transform, camera);
            shader.Render(Dx11Object, PrimitiveTopology.LineList, ReadOnlyIndices.Length, 0);
        }

        public override void Shutdown()
        {
            base.Shutdown();

            vertices = null;
        }

        public override void UpdateBuffers(ID3D11Device device, ID3D11DeviceContext deviceContext)
        {
            if (bIsUpdatedNeeded)
            {
                MappedSubresource mappedResource = deviceContext.Map(vertexBuffer, MapMode.WriteDiscard, MapFlags.None);
                unsafe
                {
                    UnsafeUtilities.Write(mappedResource.DataPointer, vertices);
                }
                deviceContext.Unmap(vertexBuffer, 0);
                bIsUpdatedNeeded = false;
            }
        }

        public override void Select()
        {
            CurrentColour = Color.Red;

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Colour = CurrentColour.ToArgb();
            }

            bIsUpdatedNeeded = true;
        }

        public override void Unselect()
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Colour = UnselectedColour.ToArgb();
            }

            bIsUpdatedNeeded = true;
        }

        public VertexLayouts.BasicLayout.Vertex[] GetTransformVertices()
        {
            VertexLayouts.BasicLayout.Vertex[] NewVertices = new VertexLayouts.BasicLayout.Vertex[vertices.Length];
            System.Array.Copy(vertices, NewVertices, vertices.Length);

            for (int i = 0; i < NewVertices.Length; i++)
            {
                Vector3 Result = Vector3.Transform(vertices[i].Position, Transform);
                NewVertices[i].Position = new Vector3(Result.X, Result.Y, Result.Z);
            }

            return NewVertices;
        }
    }
}