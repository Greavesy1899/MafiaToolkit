using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using Utils.Types;
using Buffer = SharpDX.Direct3D11.Buffer;

namespace Rendering.Graphics
{
    public class RenderLine : IRenderer
    {
        private VertexLayouts.BasicLayout.Vertex[] vertices;

        public RenderLine()
        {
            DoRender = true;
            shader = RenderStorageSingleton.Instance.ShaderManager.shaders[1];
            Transform = Matrix.Identity;
        }

        public void Init(Vector3[] points)
        {
            vertices = new VertexLayouts.BasicLayout.Vertex[points.Length];

            for(int i = 0; i != vertices.Length; i++)
            {
                VertexLayouts.BasicLayout.Vertex vertex = new VertexLayouts.BasicLayout.Vertex();
                vertex.Position = points[i];
                vertex.Colour = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);
                vertices[i] = vertex;
            }
        }

        public override void InitBuffers(Device d3d)
        {
            vertexBuffer = Buffer.Create(d3d, BindFlags.VertexBuffer, vertices);
        }

        public override void Render(Device device, DeviceContext deviceContext, Camera camera, LightClass light)
        {
            if (!DoRender)
                return;

            deviceContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertexBuffer, Utilities.SizeOf<VertexLayouts.BasicLayout.Vertex>(), 0));
            deviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.LineStrip;

            shader.SetSceneVariables(deviceContext, Transform, camera, light);
            shader.Render(deviceContext, vertices.Length, 0);
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
            vertexBuffer?.Dispose();
            vertexBuffer = null;
        }
    }
}
