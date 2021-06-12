using Rendering.Graphics;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using System.Collections.Generic;
using System.Linq;
using Utils.Extensions;

namespace Rendering.Core
{
    public class PrimitiveBatch
    {
        public PrimitiveType BatchType { get; private set; }
        public string BatchID { get; private set; }
        public Dictionary<int, IRenderer> Objects { get; private set; }

        private bool bIsDirty;
        private Buffer VertexBuffer;
        private Buffer IndexBuffer;
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
            if(IsOfType(Object))
            {
                Objects.Add(ObjectID, Object);
            }

            SetIsDirty();

            // TODO: log failure
        }

        public bool RemoveObject(int ObjectID)
        {
            if(!Objects.TryRemove(ObjectID))
            {
                // TODO: log failure
            }

            SetIsDirty();

            return true;
        }

        public IRenderer GetObject(int ObjectID)
        {
            if(Objects.ContainsKey(ObjectID))
            {
                SetIsDirty();

                return Objects[ObjectID];
            }

            return null;
        }

        public void ClearObjects()
        {
            foreach(IRenderer Object in Objects.Values)
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

        public void RenderBatch(Device InDevice, DeviceContext InDeviceContext, Camera InCamera)
        {
            if(bIsDirty)
            {
                UpdateBuffer(InDevice, InDeviceContext);
            }

            if (BatchType == PrimitiveType.Box)
            {
                RenderBBox(InDevice, InDeviceContext, InCamera);
            }
            else if (BatchType == PrimitiveType.Line)
            {
                RenderLines(InDevice, InDeviceContext, InCamera);
            }
        }

        private void UpdateBuffer(Device InDevice, DeviceContext InDContext)
        {
            if(Objects.Count > 0)
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

        private void UpdateBBox(Device InDevice, DeviceContext InDContext)
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
                VertexBuffer = Buffer.Create(InDevice, BindFlags.VertexBuffer, BBoxVertices, 0, ResourceUsage.Dynamic, CpuAccessFlags.Write);
                IndexBuffer = Buffer.Create(InDevice, BindFlags.IndexBuffer, BBoxIndices, 0, ResourceUsage.Dynamic, CpuAccessFlags.Write);
            }
            else
            {
                DataStream Stream;
                InDContext.MapSubresource(VertexBuffer, MapMode.WriteDiscard, MapFlags.None, out Stream);
                Stream.WriteRange(BBoxVertices);
                InDContext.UnmapSubresource(VertexBuffer, 0);
                Stream.Dispose();

                DataStream Stream2;
                InDContext.MapSubresource(IndexBuffer, MapMode.WriteDiscard, MapFlags.None, out Stream2);
                Stream2.WriteRange(BBoxIndices);
                InDContext.UnmapSubresource(IndexBuffer, 0);
                Stream2.Dispose();
            }


        }

        private void UpdateLines(Device InDevice, DeviceContext InDContext)
        {
            // Update buffers
            List<VertexLayouts.BasicLayout.Vertex> TempLineVertices = new List<VertexLayouts.BasicLayout.Vertex>();
            List<ushort> TempLineIndices = new List<ushort>();

            int CurrentVertexCount = 0;
            foreach(RenderLine Line in Objects.Values)
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
                VertexBuffer = Buffer.Create(InDevice, BindFlags.VertexBuffer, TempLineVertices.ToArray(), 0, ResourceUsage.Dynamic, CpuAccessFlags.Write);
                IndexBuffer = Buffer.Create(InDevice, BindFlags.IndexBuffer, TempLineIndices.ToArray(), 0, ResourceUsage.Dynamic, CpuAccessFlags.Write);
            }
            else
            {
                DataStream Stream;
                InDContext.MapSubresource(VertexBuffer, MapMode.WriteDiscard, MapFlags.None, out Stream);
                Stream.WriteRange(TempLineVertices.ToArray());
                InDContext.UnmapSubresource(VertexBuffer, 0);
                Stream.Dispose();

                DataStream Stream2;
                InDContext.MapSubresource(IndexBuffer, MapMode.WriteDiscard, MapFlags.None, out Stream2);
                Stream2.WriteRange(TempLineIndices.ToArray());
                InDContext.UnmapSubresource(IndexBuffer, 0);
                Stream2.Dispose();
            }
        }

        private void RenderBBox(Device InDevice, DeviceContext InDeviceContext, Camera InCamera)
        {
            InDeviceContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(VertexBuffer, Utilities.SizeOf<VertexLayouts.BasicLayout.Vertex>(), 0));
            InDeviceContext.InputAssembler.SetIndexBuffer(IndexBuffer, SharpDX.DXGI.Format.R32_UInt, 0);
            InDeviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.LineList;

            BaseShader Shader = RenderStorageSingleton.Instance.ShaderManager.shaders[1];
            Shader.SetSceneVariables(InDeviceContext, Matrix.Identity, InCamera);
            Shader.Render(InDeviceContext, PrimitiveTopology.LineList, SizeToRender, 0);
        }

        private void RenderLines(Device InDevice, DeviceContext InDeviceContext, Camera InCamera)
        {
            InDeviceContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(VertexBuffer, Utilities.SizeOf<VertexLayouts.BasicLayout.Vertex>(), 0));
            InDeviceContext.InputAssembler.SetIndexBuffer(IndexBuffer, SharpDX.DXGI.Format.R16_UInt, 0);
            InDeviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.LineStrip;

            BaseShader Shader = RenderStorageSingleton.Instance.ShaderManager.shaders[1];
            Shader.SetSceneVariables(InDeviceContext, Matrix.Identity, InCamera);
            Shader.Render(InDeviceContext, PrimitiveTopology.LineStrip, SizeToRender, 0);
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
