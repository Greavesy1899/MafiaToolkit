using SharpDX;
using SharpDX.Direct3D11;
using Utils.Types;

namespace Rendering.Graphics
{
    public class RenderInstance : IRenderer
    {
        private RenderStaticCollision collision;
        private bool isSelected;
        

        public void Init(RenderStaticCollision col)
        {
            DoRender = true;
            isSelected = false;
            Transform = Matrix.Identity;
            collision = col;
        }

        public override void InitBuffers(Device d3d, DeviceContext context)
        {
            collision.InitBuffers(d3d, context);
        }

        public override void Render(Device device, DeviceContext deviceContext, Camera camera, LightClass light)
        {
            if (!DoRender)
                return;

            collision.SetTransform(Transform);
            collision.Render(device, deviceContext, camera, light);   


            //unique to instanced collision; we need to do this rather than the usual, because otherwise all instanced collisions will show as being selected.
            if(isSelected)
            {
                collision.BoundingBox.DoRender = true;
                collision.BoundingBox.Render(device, deviceContext, camera, light);
                collision.BoundingBox.DoRender = false;
            }
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
            collision.SetTransform(m_trans);
        }

        public override void SetTransform(Matrix matrix)
        {
            Transform = matrix;
            collision.SetTransform(matrix);
        }

        public override void Shutdown()
        {
            //not required
        }

        public override void UpdateBuffers(Device device, DeviceContext deviceContext)
        {
            collision.UpdateBuffers(device, deviceContext);
        }

        public RenderStaticCollision GetCollision()
        {
            return collision;
        }

        public override void Select()
        {
            isSelected = true;
            collision.BoundingBox.Select();
        }

        public override void Unselect()
        {
            isSelected = false;
            collision.BoundingBox.Unselect();
        }
    }
}
