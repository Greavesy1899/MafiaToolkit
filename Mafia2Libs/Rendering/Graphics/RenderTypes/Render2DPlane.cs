using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using Color = System.Drawing.Color;
using Buffer = SharpDX.Direct3D11.Buffer;
using Utils.Extensions;

namespace Rendering.Graphics
{
    public class Render2DPlane : IRenderer
    {
        private VertexLayouts.BasicLayout.Vertex[] vertices;
        private ushort[] indices;
        private Color colour;

        public Render2DPlane()
        {
            DoRender = true;
            shader = RenderStorageSingleton.Instance.ShaderManager.shaders[1];
            Transform = Matrix.Identity;
            colour = Color.White;
        }

        public void Init(ref Vector3[] points, ResourceTypes.Navigation.LaneProperties lane, ResourceTypes.Navigation.RoadFlags roadFlags)
        {
            vertices = new VertexLayouts.BasicLayout.Vertex[points.Length * 2];
            indices = new ushort[(vertices.Length - 2) * 3];
            int idx = 0;
            for (int i = 0; i < points.Length; i++)
            {
                if (lane.Flags.HasFlag(ResourceTypes.Navigation.LaneTypes.MainRoad) || (lane.Flags.HasFlag(ResourceTypes.Navigation.LaneTypes.IsHighway)))
                    colour = Color.Blue;
                else if (lane.Flags.HasFlag(ResourceTypes.Navigation.LaneTypes.Parking))
                    colour = Color.Green;
                else if (lane.Flags.HasFlag(ResourceTypes.Navigation.LaneTypes.ExcludeImpassible))
                    colour = Color.FromArgb(255, 128, 26, 0);
                else if (lane.Flags.HasFlag(ResourceTypes.Navigation.LaneTypes.ExcludeImpassible) && lane.Flags.HasFlag(ResourceTypes.Navigation.LaneTypes.BackRoad))
                    colour = Color.FromArgb(255, 128, 51, 230);
                else if (lane.Flags.HasFlag(ResourceTypes.Navigation.LaneTypes.BackRoad))
                    colour = Color.FromArgb(255, 128, 51, 230);

                vertices[idx] = new VertexLayouts.BasicLayout.Vertex();
                vertices[idx].Position = points[i];
                vertices[idx].Colour = colour.ToArgb();
                Vector2 forward = Vector2.Zero;

                if (i < points.Length - 1)
                {
                    forward += (new Vector2(points[(i + 1)%points.Length].X, points[(i + 1) % points.Length].Y) - new Vector2(points[i].X, points[i].Y));
                }
                if (i > 0)
                {
                    forward += new Vector2(points[i].X, points[i].Y) - new Vector2(points[(i - 1)%points.Length].X, points[(i - 1) % points.Length].Y);
                }

                forward.Normalize();
                Vector3 left = new Vector3(-forward.Y, forward.X, points[i].Z);
                idx++;
                vertices[idx] = new VertexLayouts.BasicLayout.Vertex();

                float x = 0.0f;
                float y = 0.0f;

                if (roadFlags.HasFlag(ResourceTypes.Navigation.RoadFlags.BackwardDirection))
                {
                    x = (points[i].X - left.X * lane.Width);
                    y = (points[i].Y - left.Y * lane.Width);
                }
                else
                {
                    x = (points[i].X + left.X * lane.Width);
                    y = (points[i].Y + left.Y * lane.Width);
                }

                vertices[idx].Position = new Vector3(x, y, points[i].Z);
                vertices[idx].Colour = colour.ToArgb();
                points[i] = vertices[idx].Position;

                RenderLine line = new RenderLine();
                line.SetUnselectedColour(Color.Blue);
                line.Init(new Vector3[2] { vertices[idx - 1].Position, vertices[idx].Position });
                idx++;

                ushort sIdx = 0;
                int indIdx = 0;
                bool switcheroo = false;
                while (indIdx < indices.Length)
                {
                    if (switcheroo == false)
                    {
                        indices[indIdx] = sIdx++;
                        indices[indIdx + 1] = sIdx++;
                        indices[indIdx + 2] = sIdx++;
                        switcheroo = true;
                    }
                    else
                    {
                        indices[indIdx] = sIdx--;
                        indices[indIdx + 1] = sIdx--;
                        indices[indIdx + 2] = sIdx--;

                        switcheroo = false;
                    }
                    sIdx = (ushort)(indices[indIdx + 2] + 1);
                    indIdx += 3;
                }
            }
        }

        public void SetColour(Color newColor)
        {
            colour = newColor;
        }

        public override void InitBuffers(Device d3d, DeviceContext context)
        {
            vertexBuffer = Buffer.Create(d3d, BindFlags.VertexBuffer, vertices);
            indexBuffer = Buffer.Create(d3d, BindFlags.IndexBuffer, indices);
        }

        public override void Render(Device device, DeviceContext deviceContext, Camera camera)
        {
            if (!DoRender)
                return;

            deviceContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertexBuffer, Utilities.SizeOf<VertexLayouts.BasicLayout.Vertex>(), 0));
            deviceContext.InputAssembler.SetIndexBuffer(indexBuffer, SharpDX.DXGI.Format.R16_UInt, 0);
            deviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;

            shader.SetSceneVariables(deviceContext, Transform, camera);
            shader.Render(deviceContext, PrimitiveTopology.TriangleList, indices.Length, 0);
        }

        public override void SetTransform(Matrix matrix)
        {
            this.Transform = matrix;
        }

        public override void Shutdown()
        {
            vertices = null;
            indices = null;
            indexBuffer?.Dispose();
            indexBuffer = null;
            vertexBuffer?.Dispose();
            vertexBuffer = null;
        }

        public override void UpdateBuffers(Device device, DeviceContext deviceContext)
        {
            if(isUpdatedNeeded)
            {
                for (int i = 0; i < vertices.Length; i++)
                {
                    vertices[i].Colour = colour.ToArgb();
                }

                if(vertexBuffer != null && indexBuffer != null)
                {
                    indexBuffer?.Dispose();
                    indexBuffer = null;
                    vertexBuffer?.Dispose();
                    vertexBuffer = null;
                }
                InitBuffers(device, deviceContext);
            }
        }

        public override void Select()
        {
            //TODO
        }

        public override void Unselect()
        {
            //TODO
        }
    }
}
