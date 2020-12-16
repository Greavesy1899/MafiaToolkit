
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        RenderBoundingBox navigationBox;
        List<RenderLine> lines;
        OBJData.VertexStruct vertex;

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public OBJData.VertexStruct Vertex {
            get { return vertex; }
            set { vertex = value; }
        }

        public RenderBoundingBox NavigationBox {
            get { return navigationBox; }
        }
        public void Init(OBJData data, int i)
        {
            DoRender = true;
            this.data = data;
            lines = new List<RenderLine>();
            navigationBox = new RenderBoundingBox();
            navigationBox.Init(new BoundingBox(new Vector3(-0.1f), new Vector3(0.1f)));
            navigationBox.SetColour(System.Drawing.Color.Green);
            navigationBox.SetTransform(Matrix.Translation(data.vertices[i].Position));
            vertex = data.vertices[i];
        }

        public override void InitBuffers(Device d3d, DeviceContext deviceContext)
        {
            if (navigationBox != null) navigationBox.InitBuffers(d3d, deviceContext);

            if(lines != null)
            {
                for(int i = 0; i < lines.Count; i++)
                {
                    lines[i].InitBuffers(d3d, deviceContext);
                }
            }
        }

        public override void Render(Device device, DeviceContext deviceContext, Camera camera)
        {
            if (DoRender)
            {
                if (navigationBox != null) navigationBox.Render(device, deviceContext, camera);

                if (lines != null)
                {
                    for (int i = 0; i < lines.Count; i++)
                    {
                        lines[i].Render(device, deviceContext, camera);
                    }
                }
            }
        }

        public override void Select()
        {
            navigationBox.Select();
        }

        public override void SetTransform(Matrix matrix)
        {
            Transform = matrix;
        }

        public override void Shutdown()
        {
            if (navigationBox != null) navigationBox.Shutdown();

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
            navigationBox.Unselect();
        }

        public override void UpdateBuffers(Device device, DeviceContext deviceContext)
        {
            if (navigationBox != null) navigationBox.UpdateBuffers(device, deviceContext);

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
