using Rendering.Core;
using Vortice.Direct3D;
using Vortice.Direct3D11;

namespace Rendering.Graphics
{
    public class DebugShader : BaseShader
    {
        public DebugShader(DirectX11Class DxObject, ShaderInitParams InitParams) : base(DxObject, InitParams) { }

        public override void InitCBuffersFrame(ID3D11DeviceContext context, Camera camera, WorldSettings settings)
        {
            //throw new System.NotImplementedException();
        }

        public override void Render(DirectX11Class Dx11Object, PrimitiveTopology type, int size, uint offset)
        {
            Dx11Object.SetInputLayout(Layout);
            Dx11Object.SetPixelShader(OurPixelShader);
            Dx11Object.SetVertexShader(OurVertexShader);

            switch (type)
            {
                case PrimitiveTopology.LineList:
                case PrimitiveTopology.TriangleList:
                    Dx11Object.DeviceContext.DrawIndexed(size, (int)offset, 0);
                    break;
                case PrimitiveTopology.LineStrip:
                    Dx11Object.DeviceContext.Draw(size, 0);
                    break;
                default:
                    break;
            }

            Profiler.NumDrawCallsThisFrame++;
        }

        public override void SetShaderParameters(ID3D11Device device, ID3D11DeviceContext context, MaterialParameters material)
        {
            //empty
        }
    }
}