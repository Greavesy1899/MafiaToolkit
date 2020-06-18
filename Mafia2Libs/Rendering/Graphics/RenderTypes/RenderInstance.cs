using SharpDX;
using SharpDX.Direct3D11;

namespace Rendering.Graphics
{
    public class RenderInstance : IRenderer
    {
        private IRenderer instance;

        public void Init(RenderStaticCollision col)
        {
            DoRender = true;
            Transform = Matrix.Identity;
            BoundingBox = col.BoundingBox;
            instance = col;
        }

        public override void InitBuffers(Device d3d, DeviceContext context)
        {
            instance.InitBuffers(d3d, context);
        }

        public override void Render(Device device, DeviceContext deviceContext, Camera camera)
        {
            if (!DoRender)
                return;

            instance.SetTransform(Transform);
            instance.Render(device, deviceContext, camera);   
        }

        public override void SetTransform(Matrix matrix)
        {
            Transform = matrix;
            instance.SetTransform(matrix);
        }

        public override void Shutdown()
        {
            //not required
        }

        public override void UpdateBuffers(Device device, DeviceContext deviceContext)
        {
            instance.UpdateBuffers(device, deviceContext);
        }

        public RenderStaticCollision GetCollision()
        {
            return (instance as RenderStaticCollision);
        }

        public override void Select()
        {     
            (instance as RenderStaticCollision).Select();
        }

        public override void Unselect()
        {
            (instance as RenderStaticCollision).Unselect();
        }
    }
}
