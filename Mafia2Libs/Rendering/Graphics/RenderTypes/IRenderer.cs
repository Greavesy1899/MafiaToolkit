using System.Numerics;
using Vortice.Direct3D11;
using Vortice.Mathematics;

namespace Rendering.Graphics
{
    public class IRenderer
    {
        protected BaseShader shader;
        protected bool bIsUpdatedNeeded;
        protected ID3D11Buffer indexBuffer;
        protected ID3D11Buffer vertexBuffer;

        private bool bIsSelected = false;

        // TODO: Make this less messy
        public GraphicsClass CachedGraphics = null;

        public int RefID { get; protected set; }     
        public bool DoRender { get; protected set; }
        public Matrix4x4 Transform { get; protected set; }
        public BoundingBox BoundingBox { get; protected set; }

        public virtual void Select() { bIsSelected = true; }
        public virtual void Unselect() { bIsSelected = false; }
        public bool IsSelected() { return bIsSelected; }
        public virtual bool IsRayIntersecting(Ray PickingRay, out PickOutParams OutParams) { OutParams = new PickOutParams(); return false; }
        public virtual void SetVisibility(bool bIsVisible) { DoRender = bIsVisible; }
        public virtual void InitBuffers(ID3D11Device d3d, ID3D11DeviceContext deviceContext) { }
        public virtual void SetTransform(Matrix4x4 matrix) { Transform = matrix; }
        public virtual void UpdateBuffers(ID3D11Device device, ID3D11DeviceContext deviceContext) { }
        public virtual void Render(DirectX11Class Dx11Object, Camera camera) { }
        public virtual void Shutdown()
        {
            indexBuffer?.Dispose();
            indexBuffer = null;
            vertexBuffer?.Dispose();
            vertexBuffer = null;
        }

    }
}
