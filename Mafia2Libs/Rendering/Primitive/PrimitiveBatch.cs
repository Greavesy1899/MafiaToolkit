using Rendering.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using Utils.Extensions;
using Vortice;
using Vortice.Direct3D;
using Vortice.Direct3D11;

namespace Rendering.Core
{
    public class PrimitiveBatch
    {
        public PrimitiveType BatchType { get; private set; }
        public string BatchID { get; private set; }
        public Dictionary<int, IRenderer> Objects { get; private set; }

        private bool bIsDirty;
        private ID3D11Buffer VertexBuffer;
        private ID3D11Buffer IndexBuffer;
        private int SizeToRender;

        public PrimitiveBatch(PrimitiveType InType, string InBatchID)
        {
            BatchType = InType;
            BatchID = InBatchID;
            bIsDirty = true;

            Objects = new Dictionary<int, IRenderer>();
        }

        public void AddObject(int ObjectID, IRenderer Object)
        {
            if (IsOfType(Object))
            {
                Objects.Add(ObjectID, Object);
            }

            SetIsDirty();

            // TODO: log failure
        }

        public bool RemoveObject(int ObjectID)
        {
            if (!Objects.TryRemove(ObjectID))
            {
                // TODO: log failure
            }

            SetIsDirty();

            return true;
        }

        public IRenderer GetObject(int ObjectID)
        {
            if (Objects.ContainsKey(ObjectID))
            {
                SetIsDirty();

                return Objects[ObjectID];
            }

            return null;
        }

        public void ClearObjects()
        {
            foreach (IRenderer Object in Objects.Values)
            {
                Object.Shutdown();
            }

            Objects.Clear();
            SetIsDirty();
        }

        private bool IsOfType(IRenderer ObjectToCheck)
        {
            if (BatchType == PrimitiveType.Box)
            {
                return (ObjectToCheck is RenderBoundingBox);
            }
            else if (BatchType == PrimitiveType.Line)
            {
                return (ObjectToCheck is RenderLine);
            }

            return false;
        }

        public void RenderBatch(ID3D11Device InDevice, ID3D11DeviceContext InDeviceContext, Camera InCamera)
        {
            if (bIsDirty)
            {
                UpdateBuffer(InDevice, InDeviceContext);
            }

            if (BatchType == PrimitiveType.Box)
            {
                RenderBBox(InDeviceContext, InCamera);
            }
            else if (BatchType == PrimitiveType.Line)
            {
                RenderLines(InDeviceContext, InCamera);
            }
        }

        private void UpdateBuffer(ID3D11Device InDevice, ID3D11DeviceContext InDContext)
        {
            if (Objects.Count > 0)
            {
                if (BatchType == PrimitiveType.Box)
                {
                    UpdateBBox(InDevice, InDContext);
                }
                else if (BatchType == PrimitiveType.Line)
                {
                    UpdateLines(InDevice, InDContext);
                }
            }

            bIsDirty = false;
        }

        private void UpdateBBox(ID3D11Device InDevice, ID3D11DeviceContext InDContext)
        {
            // Update vertex buffer
            VertexLayouts.BasicLayout.Vertex[] BBoxVertices = new VertexLayouts.BasicLayout.Vertex[Objects.Count * 8];

            int CurrentBBoxIndex = 0;
            foreach (RenderBoundingBox BBox in Objects.Values)
            {
                VertexLayouts.BasicLayout.Vertex[] Cached = BBox.GetTransformVertices();
                System.Array.Copy(Cached, 0, BBoxVertices, CurrentBBoxIndex * Cached.Length, Cached.Length);
                CurrentBBoxIndex++;
            }

            // Update index buffer
            uint[] BBoxIndices = new uint[Objects.Count * 24];

            CurrentBBoxIndex = 0;
            foreach (RenderBoundingBox BBox in Objects.Values)
            {
                uint[] CopiedIndices = new uint[BBox.Indices.Length];
                for (int i = 0; i < CopiedIndices.Length; i++)
                {
                    int BBoxOffset = (CurrentBBoxIndex * 8);
                    CopiedIndices[i] = (ushort)(BBox.Indices[i] + BBoxOffset);
                }

                System.Array.Copy(CopiedIndices, 0, BBoxIndices, CurrentBBoxIndex * BBox.Indices.Length, BBox.Indices.Length);
                CurrentBBoxIndex++;
            }

            SizeToRender = Objects.Count * 24;
            if (VertexBuffer == null && IndexBuffer == null)
            {
                VertexBuffer = InDevice.CreateBuffer(BindFlags.VertexBuffer, BBoxVertices, 0, ResourceUsage.Dynamic, CpuAccessFlags.Write);
                IndexBuffer = InDevice.CreateBuffer(BindFlags.IndexBuffer, BBoxIndices, 0, ResourceUsage.Dynamic, CpuAccessFlags.Write);
            }
            else
            {
                // TODO: Templatize this
                MappedSubresource mappedResource = InDContext.Map(VertexBuffer, MapMode.WriteDiscard, MapFlags.None);
                unsafe
                {
                    UnsafeUtilities.Write(mappedResource.DataPointer, BBoxVertices);
                }
                InDContext.Unmap(VertexBuffer);

                mappedResource = InDContext.Map(IndexBuffer, MapMode.WriteDiscard, MapFlags.None);
                unsafe
                {
                    UnsafeUtilities.Write(mappedResource.DataPointer, BBoxIndices);
                }
                InDContext.Unmap(IndexBuffer);
            }
        }

        private void UpdateLines(ID3D11Device InDevice, ID3D11DeviceContext InDContext)
        {
            // Update buffers
            List<VertexLayouts.BasicLayout.Vertex> TempLineVertices = new List<VertexLayouts.BasicLayout.Vertex>();
            List<ushort> TempLineIndices = new List<ushort>();

            int CurrentVertexCount = 0;
            foreach (RenderLine Line in Objects.Values)
            {
                TempLineVertices.AddRange(Line.GetVertices);

                ushort[] CopiedIndices = Line.GetStrippedIndices;
                for (uint x = 0; x < CopiedIndices.Length; x++)
                {
                    CopiedIndices[x] += (ushort)(CurrentVertexCount);
                }

                TempLineIndices.AddRange(CopiedIndices.ToList());
                TempLineIndices.Add(ushort.MaxValue);

                CurrentVertexCount = TempLineVertices.Count;
            }

            SizeToRender = TempLineIndices.Count;
            if (VertexBuffer == null && IndexBuffer == null)
            {
                VertexBuffer = InDevice.CreateBuffer(BindFlags.VertexBuffer, TempLineVertices.ToArray(), 0, ResourceUsage.Dynamic, CpuAccessFlags.Write);
                IndexBuffer = InDevice.CreateBuffer(BindFlags.IndexBuffer, TempLineIndices.ToArray(), 0, ResourceUsage.Dynamic, CpuAccessFlags.Write);
            }
            else
            {
                // TODO: Templatize this
                MappedSubresource mappedResource = InDContext.Map(VertexBuffer, MapMode.WriteDiscard, MapFlags.None);
                unsafe
                {
                    UnsafeUtilities.Write(mappedResource.DataPointer, TempLineVertices.ToArray());
                }
                InDContext.Unmap(VertexBuffer);

                mappedResource = InDContext.Map(IndexBuffer, MapMode.WriteDiscard, MapFlags.None);
                unsafe
                {
                    UnsafeUtilities.Write(mappedResource.DataPointer, TempLineIndices.ToArray());
                }
                InDContext.Unmap(IndexBuffer);
            }
        }

        private void RenderBBox(ID3D11DeviceContext InDeviceContext, Camera InCamera)
        {
            VertexBufferView BufferView = new VertexBufferView(VertexBuffer, Unsafe.SizeOf<VertexLayouts.BasicLayout.Vertex>(), 0);
            InDeviceContext.IASetVertexBuffers(0, BufferView);
            InDeviceContext.IASetIndexBuffer(IndexBuffer, Vortice.DXGI.Format.R32_UInt, 0);
            InDeviceContext.IASetPrimitiveTopology(PrimitiveTopology.LineList);

            BaseShader Shader = RenderStorageSingleton.Instance.ShaderManager.shaders[1];
            Shader.SetSceneVariables(InDeviceContext, Matrix4x4.Identity, InCamera);
            Shader.Render(InDeviceContext, PrimitiveTopology.LineList, SizeToRender, 0);
        }

        private void RenderLines(ID3D11DeviceContext InDeviceContext, Camera InCamera)
        {
            VertexBufferView BufferView = new VertexBufferView(VertexBuffer, Unsafe.SizeOf<VertexLayouts.BasicLayout.Vertex>(), 0);
            InDeviceContext.IASetVertexBuffers(0, BufferView);
            InDeviceContext.IASetIndexBuffer(IndexBuffer, Vortice.DXGI.Format.R16_UInt, 0);
            InDeviceContext.IASetPrimitiveTopology(PrimitiveTopology.LineStrip);

            BaseShader Shader = RenderStorageSingleton.Instance.ShaderManager.shaders[1];
            Shader.SetSceneVariables(InDeviceContext, Matrix4x4.Identity, InCamera);
            Shader.Render(InDeviceContext, PrimitiveTopology.LineList, SizeToRender, 0);
        }

        public void SetIsDirty()
        {
            bIsDirty = true;
        }

        public void Shutdown()
        {
            foreach (IRenderer Object in Objects.Values)
            {
                Object.Shutdown();
            }

            Objects = null;

            VertexBuffer?.Dispose();
            IndexBuffer?.Dispose();
        }
    }
}