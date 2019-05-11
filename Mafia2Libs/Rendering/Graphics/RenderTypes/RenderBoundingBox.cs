using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using Utils.Types;

namespace Rendering.Graphics
{
    public class RenderBoundingBox : IRenderer
    {
        private VertexLayouts.BasicLayout.Vertex[] vertices;
        private ushort[] indices;
        private Vector4 colour;
        private BoundingBox bbox;
        public BoundingBox BBox { get { return bbox; } }

        public RenderBoundingBox()
        {
            DoRender = true;
            SetTransform(new Vector3(0.0f), new Matrix33());
            colour = new Vector4(1.0f);
        }

        public bool Init(BoundingBox bbox)
        {
            this.bbox = bbox;

            vertices = new VertexLayouts.BasicLayout.Vertex[8];
            //1
            vertices[0].Position = new Vector3(BBox.Minimum.X, BBox.Minimum.Y, BBox.Maximum.Z);
            vertices[0].Colour = colour;

            //2
            vertices[1].Position = new Vector3(BBox.Maximum.X, BBox.Minimum.Y, BBox.Maximum.Z);
            vertices[1].Colour = colour;

            //3
            vertices[2].Position = new Vector3(BBox.Minimum.X, BBox.Minimum.Y, BBox.Minimum.Z);
            vertices[2].Colour = colour;

            //4
            vertices[3].Position = new Vector3(BBox.Maximum.X, BBox.Minimum.Y, BBox.Minimum.Z);
            vertices[3].Colour = colour;

            //5
            vertices[4].Position = new Vector3(BBox.Minimum.X, BBox.Maximum.Y, BBox.Maximum.Z);
            vertices[4].Colour = colour;

            //6
            vertices[5].Position = new Vector3(BBox.Maximum.X, BBox.Maximum.Y, BBox.Maximum.Z);
            vertices[5].Colour = colour;


            //7
            vertices[6].Position = new Vector3(BBox.Minimum.X, BBox.Maximum.Y, BBox.Minimum.Z);
            vertices[6].Colour = colour;

            //8
            vertices[7].Position = new Vector3(BBox.Maximum.X, BBox.Maximum.Y, BBox.Minimum.Z);
            vertices[7].Colour = colour;

            indices = new ushort[] {
            0, 2, 3,
            3, 1, 0,
            4, 5, 7,
            7, 6, 4,
            0, 1, 5,
            5, 4, 0,
            1, 3, 7,
            7, 5, 1,
            3, 2, 6,
            6, 7, 3,
            2, 0, 4,
            4, 6, 2
            };

            shader = RenderStorageSingleton.Instance.ShaderManager.shaders[1];
            return true;
        }

        public void Update(BoundingBox box)
        {
            isUpdatedNeeded = true;
            Init(box);
        }

        public override void InitBuffers(Device d3d)
        {
            vertexBuffer = Buffer.Create(d3d, BindFlags.VertexBuffer, vertices, 0, ResourceUsage.Dynamic, CpuAccessFlags.Write);
            indexBuffer = Buffer.Create(d3d, BindFlags.IndexBuffer, indices, 0, ResourceUsage.Dynamic, CpuAccessFlags.Write);
        }

        public void SetColour(Vector4 vec)
        {
            colour = vec;
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

        public override void Render(Device device, DeviceContext deviceContext, Camera camera, LightClass light)
        {
            if (!DoRender)
                return;

            deviceContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertexBuffer, Utilities.SizeOf<VertexLayouts.BasicLayout.Vertex>(), 0));
            deviceContext.InputAssembler.SetIndexBuffer(indexBuffer, SharpDX.DXGI.Format.R16_UInt, 0);
            deviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.LineList;

            shader.SetSceneVariables(deviceContext, Transform, camera);
            shader.Render(deviceContext, PrimitiveTopology.LineList, 36, 0);
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

        public override void UpdateBuffers(DeviceContext device)
        {
            if(isUpdatedNeeded)
            {
                DataBox dataBox;
                dataBox = device.MapSubresource(vertexBuffer, 0, MapMode.WriteDiscard, MapFlags.None);
                Utilities.Write(dataBox.DataPointer, vertices, 0, vertices.Length);
                device.UnmapSubresource(vertexBuffer, 0);
                dataBox = device.MapSubresource(indexBuffer, 0, MapMode.WriteDiscard, MapFlags.None);
                Utilities.Write(dataBox.DataPointer, indices, 0, indices.Length);
                device.UnmapSubresource(indexBuffer, 0);
                isUpdatedNeeded = false;
            }
        }

        public override void Select()
        {
            for(int i = 0; i < vertices.Length; i++)
                vertices[i].Colour = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);

            isUpdatedNeeded = true;
        }

        public override void Unselect()
        {
            for (int i = 0; i < vertices.Length; i++)
                vertices[i].Colour = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);

            isUpdatedNeeded = true;
        }
    }
}
