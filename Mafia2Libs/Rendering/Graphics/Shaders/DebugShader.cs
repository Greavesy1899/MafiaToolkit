using Rendering.Core;
using SharpDX.Direct3D11;

namespace Rendering.Graphics
{
    public class DebugShader : BaseShader
    {
        public DebugShader(Device Dx11Device, ShaderInitParams InitParams) : base(Dx11Device, InitParams) { }

        public override void InitCBuffersFrame(DeviceContext context, Camera camera, WorldSettings settings)
        {
            //throw new System.NotImplementedException();
        }

        public override void Render(DeviceContext deviceContext, SharpDX.Direct3D.PrimitiveTopology type, int size, uint offset)
        {
            deviceContext.InputAssembler.InputLayout = Layout;
            deviceContext.VertexShader.Set(OurVertexShader);
            deviceContext.PixelShader.Set(OurPixelShader);

            switch (type)
            {
                case SharpDX.Direct3D.PrimitiveTopology.LineList:
                case SharpDX.Direct3D.PrimitiveTopology.TriangleList:
                    deviceContext.DrawIndexed(size, (int)offset, 0);
                    break;
                case SharpDX.Direct3D.PrimitiveTopology.LineStrip:
                    deviceContext.Draw(size, 0);
                    break;
                default:
                    break;
            }

            Profiler.NumDrawCallsThisFrame++;
        }

        public override void SetShaderParameters(Device device, DeviceContext context, MaterialParameters material)
        {
            //empty
        }
    }
}