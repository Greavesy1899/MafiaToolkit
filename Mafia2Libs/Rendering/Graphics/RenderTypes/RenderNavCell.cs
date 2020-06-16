using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct3D11;
using ResourceTypes.Navigation;

namespace Rendering.Graphics
{
    public class RenderNavCell : IRenderer
    {
        KynogonRuntimeMesh.Cell cell;
        List<RenderBoundingBox> boundingBoxes;
        List<RenderLine> lines;

        public void Init(KynogonRuntimeMesh.Cell cell)
        {
            this.cell = cell;
            DoRender = true;
            boundingBoxes = new List<RenderBoundingBox>();
            lines = new List<RenderLine>();

            foreach(var set in cell.Sets)
            {
                foreach (var unk10 in set.unk10Boxes)
                {
                    RenderBoundingBox bbox = new RenderBoundingBox();
                    bbox.SetColour(new Vector4(1.0f, 0.0f, 0.0f, 1.0f), true);
                    bbox.InitSwap(unk10.B1);
                    
                    boundingBoxes.Add(bbox);
                }

                foreach (var unk12 in set.unk12Boxes)
                {
                    RenderBoundingBox bbox = new RenderBoundingBox();
                    bbox.SetColour(new Vector4(0.0f, 1.0f, 0.0f, 1.0f), true);
                    bbox.InitSwap(unk12.B1);
                    
                    boundingBoxes.Add(bbox);
                }

                foreach (var unk14 in set.unk14Boxes)
                {
                    RenderLine line = new RenderLine();
                    line.SetUnselectedColour(new Vector4(0.992f, 0.992f, 0.168f, 1.0f));
                    line.InitSwap(unk14.Points);
                    lines.Add(line);
                }

                foreach (var unk16 in set.EdgeBoxes)
                {
                    //RenderBoundingBox bbox = new RenderBoundingBox();
                    //bbox.SetColour(new Vector4(0.0f, 0.0f, 1.0f, 1.0f), true);
                    //bbox.InitSwap(unk16);

                    RenderLine line = new RenderLine();
                    line.SetUnselectedColour(new Vector4(0.992f, 0.992f, 0.168f, 1.0f));
                    line.InitSwap(new Vector3[2] { unk16.Maximum, unk16.Minimum});
                    lines.Add(line);

                    //boundingBoxes.Add(bbox);
                }

                foreach(var unk18 in set.unk18Set)
                {
                    RenderLine line = new RenderLine();
                    line.SetUnselectedColour(new Vector4(1.0f));
                    line.InitSwap(unk18.Points);
                    lines.Add(line);
                }
            }
        }
        public override void InitBuffers(Device d3d, DeviceContext deviceContext)
        {
            if(boundingBoxes != null)
            {
                foreach(var bbox in boundingBoxes)
                {
                    bbox.InitBuffers(d3d, deviceContext);
                }
            }

            if(lines != null)
            {
                foreach(var line in lines)
                {
                    line.InitBuffers(d3d, deviceContext);
                }
            }
        }

        public override void Render(Device device, DeviceContext deviceContext, Camera camera, LightClass light)
        {
            if(DoRender)
            {
                if(boundingBoxes != null)
                {
                    foreach(var bbox in boundingBoxes)
                    {
                        bbox.Render(device, deviceContext, camera, light);
                    }
                }

                if (lines != null)
                {
                    foreach (var line in lines)
                    {
                        line.Render(device, deviceContext, camera, light);
                    }
                }
            }
        }

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
            if(boundingBoxes != null)
            {
                foreach(var bbox in boundingBoxes)
                {
                    bbox.Shutdown();
                }
            }

            if (lines != null)
            {
                foreach (var line in lines)
                {
                    line.Shutdown();
                }
            }

            boundingBoxes.Clear();
            boundingBoxes = null;
            lines.Clear();
            lines = null;
        }

        public override void Unselect()
        {
            throw new NotImplementedException();
        }

        public override void UpdateBuffers(Device device, DeviceContext deviceContext)
        {
            if (boundingBoxes != null)
            {
                foreach (var bbox in boundingBoxes)
                {
                    bbox.UpdateBuffers(device, deviceContext);
                }
            }

            if (lines != null)
            {
                foreach (var line in lines)
                {
                    line.UpdateBuffers(device, deviceContext);
                }
            }
        }
    }
}
