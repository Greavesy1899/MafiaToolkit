using Rendering.Graphics;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using System.Collections.Generic;

namespace Rendering.Core
{
    public enum PrimitiveType
    {
        Box,
        Line,
    }

    public class PrimitiveManager
    {
        private Dictionary<int, RenderBoundingBox> BoundingBoxes = null;
        private Dictionary<int, RenderLine> Lines = null;

        private bool bIsDirty = false;
        private Buffer BBoxVertexBuffer = null;
        private Buffer BBoxIndexBuffer = null;
        private Buffer LineVertexBuffer = null;
        private Buffer LineIndexBuffer = null;
        private int LineIndexSize = 0;

        public PrimitiveManager()
        {
            BoundingBoxes = new Dictionary<int, RenderBoundingBox>();
            Lines = new Dictionary<int, RenderLine>();
        }

        public IRenderer GetObject(int RefID)
        {
            if(BoundingBoxes.ContainsKey(RefID))
            {
                return BoundingBoxes[RefID];
            }
            else if (Lines.ContainsKey(RefID))
            {
                return Lines[RefID];
            }

            return null;
        }

        public void PushPrimitiveObject(PrimitiveType InType, int ObjectRefID, IRenderer ObjectToRender)
        {
            if(InType == PrimitiveType.Box)
            {
                BoundingBoxes.Add(ObjectRefID, ObjectToRender as RenderBoundingBox);
            }
            else if(InType == PrimitiveType.Line)
            {
                Lines.Add(ObjectRefID, (ObjectToRender as RenderLine));
            }

            bIsDirty = true;
        }

        public void RemovePrimitiveObject(PrimitiveType InType, int ObjectRefID)
        {
            if (InType == PrimitiveType.Box)
            {
                BoundingBoxes.Remove(ObjectRefID);
            }
            else if (InType == PrimitiveType.Line)
            {
                Lines.Remove(ObjectRefID);
            }

            bIsDirty = true;
        }

        public void RenderPrimitives(Device InDevice, DeviceContext InDeviceContext, Camera InCamera)
        {
            if(bIsDirty)
            {
                UpdateBuffer(InDevice);
            }

            if (BoundingBoxes.Count > 0)
            {
                InDeviceContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(BBoxVertexBuffer, Utilities.SizeOf<VertexLayouts.BasicLayout.Vertex>(), 0));
                InDeviceContext.InputAssembler.SetIndexBuffer(BBoxIndexBuffer, SharpDX.DXGI.Format.R16_UInt, 0);
                InDeviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.LineList;

                BaseShader Shader = RenderStorageSingleton.Instance.ShaderManager.shaders[1];
                Shader.SetSceneVariables(InDeviceContext, Matrix.Identity, InCamera);
                Shader.Render(InDeviceContext, PrimitiveTopology.LineList, BoundingBoxes.Count * 24, 0);
            }

            if (Lines.Count > 0)
            {
                InDeviceContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(LineVertexBuffer, Utilities.SizeOf<VertexLayouts.BasicLayout.Vertex>(), 0));
                InDeviceContext.InputAssembler.SetIndexBuffer(LineIndexBuffer, SharpDX.DXGI.Format.R16_UInt, 0);
                InDeviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.LineList;

                BaseShader Shader = RenderStorageSingleton.Instance.ShaderManager.shaders[1];
                Shader.SetSceneVariables(InDeviceContext, Matrix.Identity, InCamera);
                Shader.Render(InDeviceContext, PrimitiveTopology.LineStrip, LineIndexSize, 0);
            }
        }

        public void Shutdown()
        {
            foreach (RenderBoundingBox BBox in BoundingBoxes.Values)
            {
                BBox.Shutdown();
            }

            foreach (RenderLine Line in Lines.Values)
            {
                Line.Shutdown();
            }

            BoundingBoxes = null;
            Lines = null;
        }

        private void UpdateBuffer(Device InDevice)
        {
            if (BoundingBoxes.Count > 0)
            {
                UpdateBBox(InDevice);
            }

            if (Lines.Count > 0)
            {
                UpdateLines(InDevice);
            }

            bIsDirty = false;
        }

        private void UpdateBBox(Device InDevice)
        {
            // Update vertex buffer
            VertexLayouts.BasicLayout.Vertex[] BBoxVertices = new VertexLayouts.BasicLayout.Vertex[BoundingBoxes.Count * 8];

            int CurrentBBoxIndex = 0;
            foreach (RenderBoundingBox BBox in BoundingBoxes.Values)
            {
                VertexLayouts.BasicLayout.Vertex[] Cached = BBox.GetTransformVertices();
                System.Array.Copy(Cached, 0, BBoxVertices, CurrentBBoxIndex * Cached.Length, Cached.Length);
                CurrentBBoxIndex++;
            }

            // Update index buffer
            ushort[] BBoxIndices = new ushort[BoundingBoxes.Count * 24];

            CurrentBBoxIndex = 0;
            foreach (RenderBoundingBox BBox in BoundingBoxes.Values)
            {
                ushort[] CopiedIndices = BBox.Indices;
                for (int i = 0; i < CopiedIndices.Length; i++)
                {
                    int BBoxOffset = (CurrentBBoxIndex * 8);
                    CopiedIndices[i] += (ushort)(BBoxOffset);
                }

                System.Array.Copy(CopiedIndices, 0, BBoxIndices, CurrentBBoxIndex * BBox.Indices.Length, BBox.Indices.Length);
                CurrentBBoxIndex++;
            }

            BBoxVertexBuffer = Buffer.Create(InDevice, BindFlags.VertexBuffer, BBoxVertices, 0, ResourceUsage.Dynamic, CpuAccessFlags.Write);
            BBoxIndexBuffer = Buffer.Create(InDevice, BindFlags.IndexBuffer, BBoxIndices, 0, ResourceUsage.Dynamic, CpuAccessFlags.Write);
        }

        private void UpdateLines(Device InDevice)
        {
            // Update buffers
            List<VertexLayouts.BasicLayout.Vertex> LineVertices = new List<VertexLayouts.BasicLayout.Vertex>();
            List<ushort> LineIndices = new List<ushort>();

            int CurrentBBoxIndex = 0;
            foreach (RenderLine Line in Lines.Values)
            {
                if(Line.Points.Length > 1)
                {
                    int z = 0;
                }

                LineVertices.AddRange(Line.GetVertices);
                LineIndices.AddRange(Line.GetStrippedIndices);
                LineIndices.Add(ushort.MaxValue);

                CurrentBBoxIndex++;
            }

            LineIndexSize = LineIndices.Count;

            LineVertexBuffer = Buffer.Create(InDevice, BindFlags.VertexBuffer, LineVertices.ToArray(), 0, ResourceUsage.Dynamic, CpuAccessFlags.Write);
            LineIndexBuffer = Buffer.Create(InDevice, BindFlags.IndexBuffer, LineIndices.ToArray(), 0, ResourceUsage.Dynamic, CpuAccessFlags.Write);
        }
    }
}
