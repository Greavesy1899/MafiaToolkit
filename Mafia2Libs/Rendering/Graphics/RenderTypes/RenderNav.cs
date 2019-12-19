
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResourceTypes.Navigation;
using SharpDX;
using SharpDX.Direct3D11;
using Utils.Types;

namespace Rendering.Graphics
{
    public class RenderNav : IRenderer
    {
        OBJData data;
        RenderBoundingBox boundingBox;
        List<RenderLine> lines;
        public void Init(OBJData data, int i)
        {
            this.data = data;
            lines = new List<RenderLine>();
            boundingBox = new RenderBoundingBox();
            boundingBox.Init(new BoundingBox(new Vector3(-0.5f), new Vector3(0.5f)));
            boundingBox.SetTransform(Matrix.Translation(data.vertices[i].position));

            if (data.vertices[i].unk3 < data.vertices.Length)
            {
                RenderLine line = new RenderLine();
                Vector3 pos1 = data.vertices[i].position;
                Vector3 pos2 = data.vertices[data.vertices[i].unk3].position;
                line.Init(new Vector3[] { pos1, pos2 });
                lines.Add(line);
            }

            if (data.vertices[i].unk4 < data.vertices.Length)
            {
                RenderLine line = new RenderLine();
                line.Init(new Vector3[] { data.vertices[i].position, data.vertices[data.vertices[i].unk4].position });
                lines.Add(line);
            }

            if (data.vertices[i].unk5 < data.vertices.Length)
            {
                RenderLine line = new RenderLine();
                line.Init(new Vector3[] { data.vertices[i].position, data.vertices[data.vertices[i].unk5].position });
                lines.Add(line);
            }
        }

        public override void InitBuffers(Device d3d, DeviceContext deviceContext)
        {
            if (boundingBox != null) boundingBox.InitBuffers(d3d, deviceContext);

            if(lines != null)
            {
                for(int i = 0; i < lines.Count; i++)
                {
                    lines[i].InitBuffers(d3d, deviceContext);
                }
            }
        }

        public override void Render(Device device, DeviceContext deviceContext, Camera camera, LightClass light)
        {
            if (boundingBox != null) boundingBox.Render(device, deviceContext, camera, light);

            if (lines != null)
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    lines[i].Render(device, deviceContext, camera, light);
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
            if (boundingBox != null) boundingBox.Shutdown();

            if (lines != null)
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    lines[i].Shutdown();
                }
            }
        }

        public override void Unselect()
        {
            throw new NotImplementedException();
        }

        public override void UpdateBuffers(Device device, DeviceContext deviceContext)
        {
            if (boundingBox != null) boundingBox.UpdateBuffers(device, deviceContext);

            if (lines != null)
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    lines[i].UpdateBuffers(device, deviceContext);
                }
            }
        }
    }
}
