using System;
using System.Collections.Generic;
using SharpDX;
using SharpDX.Direct3D11;
using ResourceTypes.Navigation;
using Rendering.Core;
using Utils.StringHelpers;

namespace Rendering.Graphics
{
    //[TypeConverter(typeof(ExpandableObjectConverter))]
    public class RenderNavCell : IRenderer
    {
        private KynogonRuntimeMesh.Cell cell;
        private GraphicsClass OwnGraphics;

        private PrimitiveBatch LineBatch = null;
        private PrimitiveBatch BBoxBatch = null;

        // TODO: Make OwnGraphics in the base class
        public RenderNavCell(GraphicsClass InGraphicsOwner)
        {
            OwnGraphics = InGraphicsOwner;
        }

        public void Init(KynogonRuntimeMesh.Cell cell)
        {
            this.cell = cell;

            DoRender = true;
            List<RenderBoundingBox> boundingBoxes = new List<RenderBoundingBox>();
            List<RenderLine> lines = new List<RenderLine>();

            foreach(var set in cell.Sets)
            {
                foreach (var unk10 in set.unk10Boxes)
                {
                    RenderLine line = new RenderLine();
                    line.SetUnselectedColour(System.Drawing.Color.Turquoise);
                    line.InitSwap(new Vector3[2] { unk10.B1.Maximum, unk10.B1.Minimum });
                    lines.Add(line);
                }

                foreach (var unk12 in set.unk12Boxes)
                {
                    RenderLine line = new RenderLine();
                    line.SetUnselectedColour(System.Drawing.Color.Green);
                    line.InitSwap(new Vector3[2] { unk12.B1.Maximum, unk12.B1.Minimum });
                    lines.Add(line);
                }

                foreach (var unk14 in set.unk14Boxes)
                {
                    RenderLine line = new RenderLine();
                    line.SetUnselectedColour(System.Drawing.Color.Yellow);
                    line.InitSwap(unk14.Points);
                    lines.Add(line);
                }

                foreach (var unk16 in set.EdgeBoxes)
                {
                    RenderLine line = new RenderLine();
                    line.SetUnselectedColour(System.Drawing.Color.Brown);
                    line.InitSwap(new Vector3[2] { unk16.Maximum, unk16.Minimum});
                    lines.Add(line);
                }

                foreach(var unk18 in set.unk18Set)
                {
                    RenderLine line = new RenderLine();
                    line.SetUnselectedColour(System.Drawing.Color.Red);
                    line.InitSwap(unk18.Points);
                    lines.Add(line);
                }
            }

            if (lines.Count > 0)
            {
                string LineBatchID = string.Format("NavCellLineBatch_{0}", StringHelpers.GetNewRefID());
               LineBatch = new PrimitiveBatch(PrimitiveType.Line, LineBatchID);
                foreach (RenderLine line in lines)
                {
                    int PathHandle = StringHelpers.GetNewRefID();
                    LineBatch.AddObject(PathHandle, line);
                }

                OwnGraphics.OurPrimitiveManager.AddPrimitiveBatch(LineBatch);
            }

            if (boundingBoxes.Count > 0)
            {
                string BBoxBatchID = string.Format("NavCellBBoxBatch_{0}", StringHelpers.GetNewRefID());
                BBoxBatch = new PrimitiveBatch(PrimitiveType.Box, BBoxBatchID);
                foreach (RenderBoundingBox bbox in boundingBoxes)
                {
                    int PathHandle = StringHelpers.GetNewRefID();
                    BBoxBatch.AddObject(PathHandle, bbox);
                }

                OwnGraphics.OurPrimitiveManager.AddPrimitiveBatch(BBoxBatch);
            }
        }

        public override void InitBuffers(Device d3d, DeviceContext deviceContext) {}

        public override void Render(Device device, DeviceContext deviceContext, Camera camera) {}

        public override void Select()
        {
            throw new NotImplementedException();
        }

        public override void SetTransform(Matrix matrix)
        {
            Transform = matrix;
        }

        public override void Shutdown()
        {
            if(BBoxBatch != null)
            {
                OwnGraphics.OurPrimitiveManager.RemovePrimitiveBatch(BBoxBatch);
                BBoxBatch = null;
            }

            if(LineBatch != null)
            {
                OwnGraphics.OurPrimitiveManager.RemovePrimitiveBatch(LineBatch);
                LineBatch = null;
            }
        }

        public override void Unselect()
        {
            throw new NotImplementedException();
        }

        public override void UpdateBuffers(Device device, DeviceContext deviceContext) {}
    }
}
