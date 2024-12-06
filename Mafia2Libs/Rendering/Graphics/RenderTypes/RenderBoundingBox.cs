using Rendering.Core;
using System;
using System.Collections.Generic;
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
        private List<Matrix4x4> InstanceTransforms = new();

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
            vertexBuffer = d3d.CreateBuffer(BindFlags.VertexBuffer, vertices, 0, ResourceUsage.Dynamic, CpuAccessFlags.Write);
            indexBuffer = d3d.CreateBuffer(BindFlags.IndexBuffer, Indices, 0, ResourceUsage.Dynamic, CpuAccessFlags.Write);

            InitInstanceBuffer(d3d);
        }

        public void InitInstanceBuffer(ID3D11Device d3d)
        {
            int newSize = InstanceTransforms.Count * Marshal.SizeOf<Matrix4x4>();

            if (InstanceTransforms.Count == 0)
            {
                return;
            }

            // Create or update buffer only if necessary
            if (instanceBuffer == null || instanceBuffer.Description.SizeInBytes < newSize)
            {
                // Buffer description for instance buffer
                var bufferDescription = new BufferDescription
                {
                    SizeInBytes = newSize,
                    Usage = ResourceUsage.Dynamic,
                    BindFlags = BindFlags.ShaderResource,
                    OptionFlags = ResourceOptionFlags.BufferStructured,
                    CpuAccessFlags = CpuAccessFlags.Write,
                    StructureByteStride = Marshal.SizeOf<Matrix4x4>(),
                };

                var viewDescription = new ShaderResourceViewDescription()
                {
                    Format = Vortice.DXGI.Format.Unknown,
                    ViewDimension = ShaderResourceViewDimension.Buffer,
                };

                viewDescription.Buffer.FirstElement = 0;
                viewDescription.Buffer.NumElements = InstanceTransforms.Count;

                // Dispose old buffer if necessary
                instanceBuffer?.Dispose();

                // Convert list to array
                Matrix4x4[] transformsArray = InstanceTransforms.ToArray();

                // Pin the array in memory
                GCHandle handle = GCHandle.Alloc(transformsArray, GCHandleType.Pinned);
                try
                {
                    IntPtr pointer = handle.AddrOfPinnedObject();
                    // Update the instance buffer
                    instanceBuffer = d3d.CreateBuffer(bufferDescription, pointer);

                    instanceBufferView = d3d.CreateShaderResourceView(instanceBuffer, viewDescription);
                }
                finally
                {
                    handle.Free();
                }
            }
        }

        public void ReloadInstanceBuffer(ID3D11Device d3d)
        {
            instanceBuffer?.Dispose();
            instanceBuffer = null;
            instanceBufferView?.Dispose();
            instanceBufferView = null;

            InitInstanceBuffer(d3d);
        }

        public void SetColour(Color newColour, bool update = false)
        {
            UnselectedColour = newColour;
            CurrentColour = UnselectedColour;

            bIsUpdatedNeeded = update;
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

            bool BuffersSet = false;

            if (InstanceTransforms.Count > 0)
            {
                VertexBufferView VertexBufferView = new VertexBufferView(vertexBuffer, Unsafe.SizeOf<VertexLayouts.BasicLayout.Vertex>(), 0);
                deviceContext.IASetVertexBuffers(0, VertexBufferView);
                deviceContext.IASetIndexBuffer(indexBuffer, Vortice.DXGI.Format.R32_UInt, 0);
                deviceContext.IASetPrimitiveTopology(PrimitiveTopology.LineList);

                BuffersSet = true;

                RenderInstances(deviceContext, camera, device);
            }

            if (DoRenderInstancesOnly)
            {
                return;
            }

            if (!camera.CheckBBoxFrustum(Transform, BoundingBox))
                return;

            if (!BuffersSet)
            {
                VertexBufferView VertexBufferView = new VertexBufferView(vertexBuffer, Unsafe.SizeOf<VertexLayouts.BasicLayout.Vertex>(), 0);
                deviceContext.IASetVertexBuffers(0, VertexBufferView);
                deviceContext.IASetIndexBuffer(indexBuffer, Vortice.DXGI.Format.R32_UInt, 0);
                deviceContext.IASetPrimitiveTopology(PrimitiveTopology.LineList);
                BuffersSet = true;
            }

            shader.SetSceneVariables(deviceContext, Transform, camera);
            shader.Render(deviceContext, PrimitiveTopology.LineList, ReadOnlyIndices.Length, 0);
        }

        public void RenderInstances(ID3D11DeviceContext deviceContext, Camera camera, ID3D11Device device)
        {
            deviceContext.VSSetShaderResource(0, instanceBufferView);

            shader.SetSceneVariables(deviceContext, Transform, camera);

            shader.RenderInstanced(deviceContext, PrimitiveTopology.LineList, Indices.Length, 0, InstanceTransforms.Count);
            Profiler.NumDrawCallsThisFrame++;
        }

        public override void Shutdown()
        {
            vertices = null;
            indexBuffer?.Dispose();
            indexBuffer = null;
            vertexBuffer?.Dispose();
            vertexBuffer = null;
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

        public void SetInstanceTransforms(List<Matrix4x4> Transforms)
        {
            InstanceTransforms = Transforms;
        }
    }
}