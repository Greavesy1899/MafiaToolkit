using System.Numerics;
using Vortice.Direct3D11;

namespace Rendering.Graphics
{
    public class RenderInstance : IRenderer
    {
        private IRenderer instance;

        public void Init(RenderStaticCollision col)
        {
            DoRender = true;
            Transform = Matrix4x4.Identity;
            BoundingBox = col.BoundingBox;
            instance = col;
        }

        public override void InitBuffers(ID3D11Device d3d, ID3D11DeviceContext context)
        {
            instance.InitBuffers(d3d, context);
        }

        public override void Render(DirectX11Class Dx11Object, Camera camera)
        {
            if (!DoRender)
            {
                return;
            }

            instance.SetTransform(Transform);
            instance.Render(Dx11Object, camera);   
        }

        public override void SetTransform(Matrix4x4 matrix)
        {
            base.SetTransform(matrix);

            instance.SetTransform(matrix);
        }

        public override void UpdateBuffers(ID3D11Device device, ID3D11DeviceContext deviceContext)
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
