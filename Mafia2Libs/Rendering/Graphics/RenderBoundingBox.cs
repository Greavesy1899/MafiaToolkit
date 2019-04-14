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

        public RenderBoundingBox()
        {
            DoRender = true;
        }

        public bool Init(BoundingBox bbox)
        {
            boundingBox = bbox;

            vertices = new VertexLayouts.BasicLayout.Vertex[8];
            //1
            vertices[0].Position = new Vector3(boundingBox.Minimum.X, boundingBox.Minimum.Y, boundingBox.Maximum.Z);
            vertices[0].Colour = new Vector3(1.0f);

            //2
            vertices[1].Position = new Vector3(boundingBox.Maximum.X, boundingBox.Minimum.Y, boundingBox.Maximum.Z);
            vertices[1].Colour = new Vector3(1.0f);

            //3
            vertices[2].Position = new Vector3(boundingBox.Minimum.X, boundingBox.Minimum.Y, boundingBox.Minimum.Z);
            vertices[2].Colour = new Vector3(1.0f);

            //4
            vertices[3].Position = new Vector3(boundingBox.Maximum.X, boundingBox.Minimum.Y, boundingBox.Minimum.Z);
            vertices[3].Colour = new Vector3(1.0f);

            //5
            vertices[4].Position = new Vector3(boundingBox.Minimum.X, boundingBox.Maximum.Y, boundingBox.Maximum.Z);
            vertices[4].Colour = new Vector3(1.0f);

            //6
            vertices[5].Position = new Vector3(boundingBox.Maximum.X, boundingBox.Maximum.Y, boundingBox.Maximum.Z);
            vertices[5].Colour = new Vector3(1.0f);


            //7
            vertices[6].Position = new Vector3(boundingBox.Minimum.X, boundingBox.Maximum.Y, boundingBox.Minimum.Z);
            vertices[6].Colour = new Vector3(1.0f);

            //8
            vertices[7].Position = new Vector3(boundingBox.Maximum.X, boundingBox.Maximum.Y, boundingBox.Minimum.Z);
            vertices[7].Colour = new Vector3(1.0f);

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

        public override void InitBuffers(Device d3d)
        {
            vertexBuffer = Buffer.Create(d3d, BindFlags.VertexBuffer, vertices);
            indexBuffer = Buffer.Create(d3d, BindFlags.IndexBuffer, indices);
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

            shader.SetSceneVariables(deviceContext, Transform, camera, light);
            shader.Render(deviceContext, 12 * 3, 0);
        }

        public override void Shutdown()
        {
            indexBuffer?.Dispose();
            indexBuffer = null;
            vertexBuffer?.Dispose();
            vertexBuffer = null;
        }
    }
}
