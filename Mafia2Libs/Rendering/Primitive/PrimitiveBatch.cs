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

        public void RenderBatch(DirectX11Class Dx11Object, Camera InCamera)
        {
            if (bIsDirty)
            {
                UpdateBuffer(Dx11Object.Device, Dx11Object.DeviceContext);
            }

            // Only attempt to render if we've got visible data in this batch.
            if (SizeToRender > 0)
            {
                if (BatchType == PrimitiveType.Box)
                {
                    RenderBBox(Dx11Object, InCamera);
                }
                else if (BatchType == PrimitiveType.Line)
                {
                    RenderLines(Dx11Object, InCamera);
                }
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
            int NumSkippedVertices = 0;
            foreach (RenderBoundingBox BBox in Objects.Values)
            {
                if (!BBox.DoRender)
                {
                    NumSkippedVertices += 8;
                    continue;
                }

                VertexLayouts.BasicLayout.Vertex[] Cached = BBox.GetTransformVertices();
                System.Array.Copy(Cached, 0, BBoxVertices, CurrentBBoxIndex * Cached.Length, Cached.Length);
                CurrentBBoxIndex++;
            }

            // Update index buffer
            uint[] BBoxIndices = new uint[Objects.Count * 24];

            CurrentBBoxIndex = 0;
            int NumSkippedIndices = 0;
            foreach (RenderBoundingBox BBox in Objects.Values)
            {
                if (!BBox.DoRender)
                {
                    NumSkippedIndices += 24;
                    continue;
                }

                uint[] CopiedIndices = new uint[BBox.Indices.Length];
                for (int i = 0; i < CopiedIndices.Length; i++)
                {
                    int BBoxOffset = (CurrentBBoxIndex * 8);
                    CopiedIndices[i] = (ushort)(BBox.Indices[i] + BBoxOffset);
                }

                System.Array.Copy(CopiedIndices, 0, BBoxIndices, CurrentBBoxIndex * BBox.Indices.Length, BBox.Indices.Length);
                CurrentBBoxIndex++;
            }

            // Remove empty data
            if (NumSkippedVertices > 0)
            {
                System.Array.Resize(ref BBoxVertices, BBoxVertices.Length - NumSkippedVertices);
            }

            if (NumSkippedIndices > 0)
            {
                System.Array.Resize(ref BBoxIndices, BBoxIndices.Length - NumSkippedIndices);
            }

            // Remove the objects we skipped
            int NumObjectsSkipped = (NumSkippedIndices / 24);
            SizeToRender = (Objects.Count - NumObjectsSkipped) * 24;
            if (VertexBuffer == null && IndexBuffer == null)
            {
                VertexBuffer = InDevice.CreateBuffer<VertexLayouts.BasicLayout.Vertex>(BindFlags.VertexBuffer, BBoxVertices, 0, ResourceUsage.Dynamic, CpuAccessFlags.Write);
                IndexBuffer = InDevice.CreateBuffer<uint>(BindFlags.IndexBuffer, BBoxIndices, 0, ResourceUsage.Dynamic, CpuAccessFlags.Write);
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
                // Skip line if no desire to render
                if(!Line.DoRender)
                {
                    continue;
                }

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
                VertexBuffer = InDevice.CreateBuffer<VertexLayouts.BasicLayout.Vertex>(BindFlags.VertexBuffer, TempLineVertices.ToArray(), 0, ResourceUsage.Dynamic, CpuAccessFlags.Write);
                IndexBuffer = InDevice.CreateBuffer<ushort>(BindFlags.IndexBuffer, TempLineIndices.ToArray(), 0, ResourceUsage.Dynamic, CpuAccessFlags.Write);
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

        private void RenderBBox(DirectX11Class Dx11Object, Camera InCamera)
        {
            Dx11Object.DeviceContext.IASetVertexBuffer(0, VertexBuffer, Unsafe.SizeOf<VertexLayouts.BasicLayout.Vertex>());
            Dx11Object.DeviceContext.IASetIndexBuffer(IndexBuffer, Vortice.DXGI.Format.R32_UInt, 0);
            Dx11Object.SetPrimitiveTopology(PrimitiveTopology.LineList);

            BaseShader Shader = RenderStorageSingleton.Instance.ShaderManager.shaders[1];
            Shader.SetSceneVariables(Dx11Object.DeviceContext, Matrix4x4.Identity, InCamera);
            Shader.Render(Dx11Object, PrimitiveTopology.LineList, SizeToRender, 0);
        }

        private void RenderLines(DirectX11Class Dx11Object, Camera InCamera)
        {
            Dx11Object.DeviceContext.IASetVertexBuffer(0, VertexBuffer, Unsafe.SizeOf<VertexLayouts.BasicLayout.Vertex>());
            Dx11Object.DeviceContext.IASetIndexBuffer(IndexBuffer, Vortice.DXGI.Format.R16_UInt, 0);
            Dx11Object.SetPrimitiveTopology(PrimitiveTopology.LineStrip);

            BaseShader Shader = RenderStorageSingleton.Instance.ShaderManager.shaders[1];
            Shader.SetSceneVariables(Dx11Object.DeviceContext, Matrix4x4.Identity, InCamera);
            Shader.Render(Dx11Object, PrimitiveTopology.LineList, SizeToRender, 0);
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