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
        private VertexLayouts.BasicLayout.Vertex[] vertices;
        private ushort[] indices;
        private Color colour;

        public RenderBoundingBox()
        {
            DoRender = true;
            SetTransform(Matrix4x4.Identity);
            colour = Color.White;
        }

        public bool InitSwap(BoundingBox bbox)
        {
            Vector3 NewMax = bbox.Maximum;
            float y = NewMax.Y;
            NewMax.Y = -NewMax.Z;
            NewMax.Z = y;

            Vector3 NewMin = bbox.Minimum;
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
            for(int i = 0; i < corners.Length; i++)
            {
                vertices[i] = new VertexLayouts.BasicLayout.Vertex();
                vertices[i].Position = corners[i];
                vertices[i].Colour = colour.ToArgb();
            }

            indices = new ushort[] {
                0, 1, 1, 2, 2, 3, 3, 0, // Front edges
                4, 5, 5, 6, 6, 7, 7, 4, // Back edges
                0, 4, 1, 5, 2, 6, 3, 7 // Side edges connecting front and back
            };

            shader = RenderStorageSingleton.Instance.ShaderManager.shaders[1];
            return true;
        }

        public void Update(BoundingBox box)
        {
            isUpdatedNeeded = true;
            Init(box);
        }

        public override void InitBuffers(ID3D11Device d3d, ID3D11DeviceContext deviceContext)
        {
            vertexBuffer = d3d.CreateBuffer(BindFlags.VertexBuffer, vertices, 0, ResourceUsage.Dynamic, CpuAccessFlags.Write);
            indexBuffer = d3d.CreateBuffer(BindFlags.VertexBuffer, indices, 0, ResourceUsage.Dynamic, CpuAccessFlags.Write);
        }

        public void SetColour(Color newColour, bool update = false)
        {
            colour = newColour;
            isUpdatedNeeded = update;
        }

        public override void SetTransform(Matrix4x4 matrix)
        {
            this.Transform = matrix;
        }

        public override void Render(ID3D11Device device, ID3D11DeviceContext deviceContext, Camera camera)
        {
            if (!DoRender)
            {
                return;
            }

            VertexBufferView VertexBufferView = new VertexBufferView(vertexBuffer, Unsafe.SizeOf<VertexLayouts.BasicLayout.Vertex>(), 0);
            deviceContext.IASetVertexBuffers(0, VertexBufferView);
            deviceContext.IASetIndexBuffer(indexBuffer, Vortice.DXGI.Format.R16_UInt, 0);
            deviceContext.IASetPrimitiveTopology(PrimitiveTopology.LineList);

            shader.SetSceneVariables(deviceContext, Transform, camera);
            shader.Render(deviceContext, PrimitiveTopology.LineList, indices.Length, 0);
        }

        public override void Shutdown()
        {
            vertices = null;
            indices = null;
            indexBuffer?.Dispose();
            indexBuffer = null;
            vertexBuffer?.Dispose();
            vertexBuffer = null;
        }

        public override void UpdateBuffers(ID3D11Device device, ID3D11DeviceContext deviceContext)
        {
            if(isUpdatedNeeded)
            {
                MappedSubresource mappedResource = deviceContext.Map(vertexBuffer, MapMode.WriteDiscard, MapFlags.None);
                unsafe
                {
                    UnsafeUtilities.Write(mappedResource.DataPointer, vertices);
                }
                deviceContext.Unmap(vertexBuffer, 0);
                isUpdatedNeeded = false;
            }
        }

        public override void Select()
        {
            colour = Color.Red;

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Colour = colour.ToArgb();
            }

            isUpdatedNeeded = true;
        }

        public override void Unselect()
        {
            colour = Color.White;

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Colour = colour.ToArgb();
            }

            isUpdatedNeeded = true;
        }
    }
}
