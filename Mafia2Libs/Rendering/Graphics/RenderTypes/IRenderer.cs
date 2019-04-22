using SharpDX;
using SharpDX.Direct3D11;
using Utils.Types;

namespace Rendering.Graphics
{
    public abstract class IRenderer
    {
        public bool DoRender;
        protected BaseShader shader;
        public Matrix Transform { get; protected set; }
        public BoundingBox BBox { get; protected set; }
        protected Buffer indexBuffer;
        protected Buffer vertexBuffer;

        public abstract void InitBuffers(Device d3d);
        public abstract void SetTransform(Vector3 position, Matrix33 rotation);
        public abstract void SetTransform(Matrix matrix);

        public abstract void Render(Device device, DeviceContext deviceContext, Camera camera, LightClass light);
        public abstract void Shutdown();

    }
}
