using Rendering.Core;
using Vortice.Direct3D;
using Vortice.Direct3D11;

namespace Rendering.Graphics
{
    public class DebugShader : BaseShader
    {
        public DebugShader(ID3D11Device Dx11Device, ShaderInitParams InitParams) : base(Dx11Device, InitParams) { }

        public override void InitCBuffersFrame(ID3D11DeviceContext context, Camera camera, WorldSettings settings)
        {
            //throw new System.NotImplementedException();
        }

        public override void Render(ID3D11DeviceContext deviceContext, PrimitiveTopology type, int size, uint offset)
        {
            deviceContext.IASetInputLayout(Layout);
            deviceContext.VSSetShader(OurVertexShader);
            deviceContext.PSSetShader(OurPixelShader);

            switch (type)
            {
                case PrimitiveTopology.LineList:
                case PrimitiveTopology.TriangleList:
                    deviceContext.DrawIndexed(size, (int)offset, 0);
                    break;
                case PrimitiveTopology.LineStrip:
                    deviceContext.Draw(size, 0);
                    break;
                default:
                    break;
            }

            Profiler.NumDrawCallsThisFrame++;
        }

        public override void RenderInstanced(ID3D11DeviceContext context, PrimitiveTopology type, int size, int offset, int count)
        {
            context.IASetInputLayout(Layout);

            // set shaders only if available
            if (OurInstanceVertexShader != null)
            {
                context.VSSetShader(OurInstanceVertexShader);
            }

            if (OurInstanceVertexShader != null)
            {
                context.PSSetShader(OurPixelShader);
            }

            switch (type)
            {
                case PrimitiveTopology.LineList:
                case PrimitiveTopology.TriangleList:
                    context.DrawIndexedInstanced(size, count, offset, 0, 0);
                    break;
                case PrimitiveTopology.LineStrip:
                    //context.Draw(size, 0);
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