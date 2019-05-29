using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using Utils.Types;
using Buffer = SharpDX.Direct3D11.Buffer;

namespace Rendering.Graphics
{
    public class Render2DPlane : IRenderer
    {
        private VertexLayouts.BasicLayout.Vertex[] vertices;
        private ushort[] indices;
        private Vector4 colour;

        public Render2DPlane()
        {
            DoRender = true;
            shader = RenderStorageSingleton.Instance.ShaderManager.shaders[1];
            Transform = Matrix.Identity;
            colour = new Vector4(1.0f);
        }

        public void Init(ref Vector3[] points, ResourceTypes.Navigation.unkStruct1Sect1 lane, int roadFlags)
        {
            vertices = new VertexLayouts.BasicLayout.Vertex[points.Length * 2];
            indices = new ushort[(vertices.Length - 2) * 3];
            int idx = 0;
            for (int i = 0; i < points.Length; i++)
            {
                vertices[idx] = new VertexLayouts.BasicLayout.Vertex();
                vertices[idx].Position = points[i];
                vertices[idx].Colour = colour;
                Vector2 forward = Vector2.Zero;

                if (i < points.Length - 1)
                {
                    forward += new Vector2(points[(i + 1)%points.Length].X, points[(i + 1) % points.Length].Y) - new Vector2(points[i].X, points[i].Y);
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

                if (roadFlags >= 4096)
                {
                    x = (points[i].X + left.X * lane.unk01);
                    y = (points[i].Y + left.Y * lane.unk01);
                }
                else
                {
                    x = (points[i].X - left.X * lane.unk01);
                    y = (points[i].Y - left.Y * lane.unk01);
                }

                vertices[idx].Position = new Vector3(x, y, points[i].Z);
                vertices[idx].Colour = colour;
                points[i] = vertices[idx].Position;

                RenderLine line = new RenderLine();
                line.SetColour(new Vector4(0.0f, 0.0f, 1.0f, 1.0f));
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

        public void SetColour(Vector4 vec)
        {
            colour = vec;
        }

        public override void InitBuffers(Device d3d)
        {
            vertexBuffer = Buffer.Create(d3d, BindFlags.VertexBuffer, vertices);
            indexBuffer = Buffer.Create(d3d, BindFlags.IndexBuffer, indices);
        }

        public override void Render(Device device, DeviceContext deviceContext, Camera camera, LightClass light)
        {
            if (!DoRender)
                return;

            deviceContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertexBuffer, Utilities.SizeOf<VertexLayouts.BasicLayout.Vertex>(), 0));
            deviceContext.InputAssembler.SetIndexBuffer(indexBuffer, SharpDX.DXGI.Format.R16_UInt, 0);
            deviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;

            shader.SetSceneVariables(deviceContext, Transform, camera);
            shader.Render(deviceContext, PrimitiveTopology.TriangleList, indices.Length, 0);
        }

        public override void SetTransform(Vector3 position, Matrix33 rotation)
        {
            Matrix m_trans = Matrix.Identity;
            m_trans[0, 0] = rotation.M00;
            m_trans[0, 1] = rotation.M01;
            m_trans[0, 2] = rotation.M02;
            m_trans[1, 0] = rotation.M10;
            m_trans[1, 1] = rotation.M11;
            m_trans[1, 2] = rotation.M12;
            m_trans[2, 0] = rotation.M20;
            m_trans[2, 1] = rotation.M21;
            m_trans[2, 2] = rotation.M22;
            m_trans[3, 0] = position.X;
            m_trans[3, 1] = position.Y;
            m_trans[3, 2] = position.Z;
            Transform = m_trans;
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
                for(int i = 0; i < vertices.Length; i++)
                    vertices[i].Colour = colour;

                DataBox dataBox;
                dataBox = deviceContext.MapSubresource(vertexBuffer, 0, MapMode.WriteDiscard, MapFlags.None);
                Utilities.Write(dataBox.DataPointer, vertices, 0, vertices.Length);
                deviceContext.UnmapSubresource(vertexBuffer, 0);
                dataBox = deviceContext.MapSubresource(indexBuffer, 0, MapMode.WriteDiscard, MapFlags.None);
                Utilities.Write(dataBox.DataPointer, indices, 0, indices.Length);
                deviceContext.UnmapSubresource(indexBuffer, 0);
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
