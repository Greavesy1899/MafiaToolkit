using System;
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
        public Vector3[] RawPoints;
        private Vector4 colour;

        public RenderLine()
        {
            DoRender = true;
            shader = RenderStorageSingleton.Instance.ShaderManager.shaders[1];
            Transform = Matrix.Identity;
            colour = new Vector4(1.0f);
        }

        public void Init(Vector3[] points)
        {
            RawPoints = points;
            vertices = new VertexLayouts.BasicLayout.Vertex[points.Length];

            for(int i = 0; i != vertices.Length; i++)
            {
                VertexLayouts.BasicLayout.Vertex vertex = new VertexLayouts.BasicLayout.Vertex();
                vertex.Position = points[i];
                vertex.Colour = colour;
                vertices[i] = vertex;
            }
        }

        public void SetColour(Vector4 vec)
        {
            colour = vec;
        }

        public override void InitBuffers(Device d3d)
        {
            vertexBuffer = Buffer.Create(d3d, BindFlags.VertexBuffer, vertices, 0, ResourceUsage.Dynamic, CpuAccessFlags.Write);
        }

        public override void Render(Device device, DeviceContext deviceContext, Camera camera, LightClass light)
        {
            if (!DoRender)
                return;

            deviceContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertexBuffer, Utilities.SizeOf<VertexLayouts.BasicLayout.Vertex>(), 0));
            deviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.LineStrip;

            shader.SetSceneVariables(deviceContext, Transform, camera);
            shader.Render(deviceContext, PrimitiveTopology.LineStrip, vertices.Length, 0);
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
            vertexBuffer?.Dispose();
            vertexBuffer = null;
        }

        public override void UpdateBuffers(DeviceContext device)
        {
            if(isUpdatedNeeded)
            {
                DataBox dataBox;
                dataBox = device.MapSubresource(vertexBuffer, 0, MapMode.WriteDiscard, MapFlags.None);
                Utilities.Write(dataBox.DataPointer, RawPoints, 0, RawPoints.Length);
                device.UnmapSubresource(vertexBuffer, 0);
            }
        }
    }
}
