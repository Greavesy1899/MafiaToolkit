using SharpDX;
using SharpDX.Direct3D11;
using Utils.Types;

namespace Rendering.Graphics
{
    public class RenderInstance : IRenderer
    {
        private RenderStaticCollision collision;

        public void Init(RenderStaticCollision col)
        {
            DoRender = true;
            Transform = Matrix.Identity;
            collision = col;
        }

        public override void InitBuffers(Device d3d)
        {
            collision.InitBuffers(d3d);
        }

        public override void Render(Device device, DeviceContext deviceContext, Camera camera, LightClass light)
        {
            collision.Render(device, deviceContext, camera, light);   
        }

        public override void SetTransform(Vector3 position, Matrix33 rotation)
        {
            collision.SetTransform(position, rotation);
        }

        public override void SetTransform(Matrix matrix)
        {
            collision.SetTransform(matrix);
        }

        public override void Shutdown()
        {
            //not required
        }

        public override void UpdateBuffers(DeviceContext device)
        {
            collision.UpdateBuffers(device);
        }

        public RenderStaticCollision GetCollision()
        {
            return collision;
        }

        public override void Select()
        {
            collision.BoundingBox.DoRender = true;
        }

        public override void Unselect()
        {
            collision.BoundingBox.DoRender = false;
        }
    }
}
