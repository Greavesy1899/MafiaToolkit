using SharpDX;
using SharpDX.Direct3D11;

namespace Rendering.Graphics
{
    public class RenderInstance : IRenderer
    {
        private IRenderer instance;
        private bool isSelected;

        public void Init(RenderStaticCollision col)
        {
            DoRender = true;
            isSelected = false;
            Transform = Matrix.Identity;
            instance = col;
        }

        public void Init(RenderModel model)
        {
            DoRender = true;
            isSelected = false;
            Transform = Matrix.Identity;
            instance = model;
        }

        public override void InitBuffers(Device d3d, DeviceContext context)
        {
            instance.InitBuffers(d3d, context);
        }

        public override void Render(Device device, DeviceContext deviceContext, Camera camera, LightClass light)
        {
            if (!DoRender)
                return;

            instance.SetTransform(Transform);
            instance.Render(device, deviceContext, camera, light);   


            //unique to instanced collision; we need to do this rather than the usual, because otherwise all instanced collisions will show as being selected.
            if(isSelected)
            {
                if (instance.GetType() == typeof(RenderStaticCollision) || instance.GetType() == typeof(RenderModel))
                {
                    (instance as RenderStaticCollision).BoundingBox.DoRender = true;
                    (instance as RenderStaticCollision).BoundingBox.Render(device, deviceContext, camera, light);
                    (instance as RenderStaticCollision).BoundingBox.DoRender = false;
                }
            }
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
            isSelected = true;            
            (instance as RenderStaticCollision).Select();
        }

        public override void Unselect()
        {
            isSelected = false;
            (instance as RenderStaticCollision).Unselect();
        }
    }
}
