using System.Numerics;
using Rendering.Graphics.Instances;
using Vortice.Direct3D11;
using Vortice.Mathematics;

namespace Rendering.Graphics
{
    public abstract class IRenderer
    {
        protected BaseShader shader;
        protected bool bIsUpdatedNeeded;
        public bool isInstance = false;
        protected ID3D11Buffer indexBuffer;
        protected ID3D11Buffer vertexBuffer;

        public bool DoRender { get; set; }
        public Matrix4x4 Transform { get; protected set; }
        public BoundingBox BoundingBox { get; protected set; }

        public abstract void Select();
        public abstract void Unselect();
        public abstract void InitBuffers(ID3D11Device d3d, ID3D11DeviceContext deviceContext,ModelInstanceManager modelManager);
        public abstract void SetTransform(Matrix4x4 matrix);
        public abstract void UpdateBuffers(ID3D11Device device, ID3D11DeviceContext deviceContext);
        public abstract void Render(ID3D11Device device, ID3D11DeviceContext deviceContext, Camera camera);
        public abstract void Shutdown();

    }
}
