using Rendering.Core;
using ResourceTypes.Navigation;
using System;
using System.Collections.Generic;
using System.Numerics;
using Rendering.Graphics.Instances;
using Toolkit.Core;
using Vortice.Direct3D11;

namespace Rendering.Graphics
{
    //[TypeConverter(typeof(ExpandableObjectConverter))]
    public class RenderNavCell : IRenderer
    {
        private KynogonRuntimeMesh.Cell cell;
        private GraphicsClass OwnGraphics;

        private PrimitiveBatch LineBatch = null;
        private List<RenderLine> Lines;

        // TODO: Make OwnGraphics in the base class
        public RenderNavCell(GraphicsClass InGraphicsOwner)
        {
            OwnGraphics = InGraphicsOwner;
            Lines = new List<RenderLine>();

            string LineBatchID = string.Format("NavCellLineBatch_{0}", RefManager.GetNewRefID());
            LineBatch = new PrimitiveBatch(PrimitiveType.Line, LineBatchID);
        }

        public void Init(KynogonRuntimeMesh.Cell cell)
        {
            this.cell = cell;

            DoRender = true;

            foreach (var set in cell.Sets)
            {
                foreach (var unk10 in set.unk10Boxes)
                {
                    RenderLine line = new RenderLine();
                    line.SetUnselectedColour(System.Drawing.Color.Turquoise);
                    line.InitSwap(new Vector3[2] { unk10.B1.Max, unk10.B1.Min });
                    Lines.Add(line);
                }

                foreach (var unk12 in set.unk12Boxes)
                {
                    RenderLine line = new RenderLine();
                    line.SetUnselectedColour(System.Drawing.Color.Green);
                    line.InitSwap(new Vector3[2] { unk12.B1.Max, unk12.B1.Min });
                    Lines.Add(line);
                }

                foreach (var unk14 in set.unk14Boxes)
                {
                    RenderLine line = new RenderLine();
                    line.SetUnselectedColour(System.Drawing.Color.Yellow);
                    line.InitSwap(unk14.Points);
                    Lines.Add(line);
                }

                foreach (var unk16 in set.EdgeBoxes)
                {
                    RenderLine line = new RenderLine();
                    line.SetUnselectedColour(System.Drawing.Color.Brown);
                    line.InitSwap(new Vector3[2] { unk16.Max, unk16.Min });
                    Lines.Add(line);
                }

                foreach (var unk18 in set.unk18Set)
                {
                    RenderLine line = new RenderLine();
                    line.SetUnselectedColour(System.Drawing.Color.Red);
                    line.InitSwap(unk18.Points);
                    Lines.Add(line);
                }
            }

            Update();
        }

        public override void InitBuffers(ID3D11Device d3d, ID3D11DeviceContext deviceContext,ModelInstanceManager modelManager) { }

        public override void Render(ID3D11Device device, ID3D11DeviceContext deviceContext, Camera camera) { }

        public override void Select()
        {
            throw new NotImplementedException();
        }

        public override void SetTransform(Matrix4x4 matrix)
        {
            Transform = matrix;
        }

        public override void Shutdown()
        {
            if (LineBatch != null)
            {
                OwnGraphics.OurPrimitiveManager.RemovePrimitiveBatch(LineBatch);
                LineBatch = null;
            }
        }

        public override void Unselect()
        {
            throw new NotImplementedException();
        }

        public override void UpdateBuffers(ID3D11Device device, ID3D11DeviceContext deviceContext) { }

        public void SetVisibility(bool bNewVisibility)
        {
            DoRender = bNewVisibility;
            Update();
        }

        private void Update()
        {
            // TODO: Ideally, we should be using the same primitive batcher.
            // Problem is, calling ClearObjects on the batcher shuts down the lines too.
            // Once the RenderLine and RenderBBox has been decoupled, then this should be easier.
            OwnGraphics.OurPrimitiveManager.RemovePrimitiveBatch(LineBatch);

            if (DoRender)
            {
                if (Lines.Count > 0)
                {
                    string LineBatchID = string.Format("NavCellLineBatch_{0}", RefManager.GetNewRefID());
                    LineBatch = new PrimitiveBatch(PrimitiveType.Line, LineBatchID);

                    foreach (RenderLine line in Lines)
                    {
                        int PathHandle = RefManager.GetNewRefID();
                        LineBatch.AddObject(PathHandle, line);
                    }

                    OwnGraphics.OurPrimitiveManager.AddPrimitiveBatch(LineBatch);
                }
            }
        }
    }
}
