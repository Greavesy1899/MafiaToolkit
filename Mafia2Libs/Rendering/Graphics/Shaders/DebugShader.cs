using SharpDX.Direct3D11;

namespace Rendering.Graphics
{
    public class DebugShader : BaseShader
    {
        public DebugShader(Device device, InputElement[] elements, string psPath, string vsPath, string vsEntryPoint, string psEntryPoint)
        {
            if (!Init(device, elements, vsPath, psPath, vsEntryPoint, psEntryPoint))
            {
                throw new System.Exception("Failed to load Shader!");
            }
        }

        public override bool Init(Device device, InputElement[] elements, string vsFileName, string psFileName, string vsEntryPoint, string psEntryPoint)
        {
            if (!base.Init(device, elements, vsFileName, psFileName, vsEntryPoint, psEntryPoint))
            {
                return false;
            }

            return true;
        }

        public override void InitCBuffersFrame(DeviceContext context, Camera camera, LightClass light)
        {
            //throw new System.NotImplementedException();
        }

        public override void Render(DeviceContext deviceContext, SharpDX.Direct3D.PrimitiveTopology type, int size, uint offset)
        {
            deviceContext.InputAssembler.InputLayout = Layout;
            deviceContext.VertexShader.Set(VertexShader);
            deviceContext.PixelShader.Set(PixelShader);

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
        }

        public override void SetShaderParameters(Device device, DeviceContext context, MaterialParameters material)
        {
            //empty
        }
    }
}