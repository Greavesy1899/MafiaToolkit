using SharpDX.Direct3D11;

namespace Rendering.Graphics
{
    public class CollisionShader : BaseShader
    {
        public CollisionShader(Device device, InputElement[] elements, string psPath, string vsPath, string vsEntryPoint, string psEntryPoint)
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
            base.InitCBuffersFrame(context, camera, light);
        }
        public override void Render(DeviceContext context, SharpDX.Direct3D.PrimitiveTopology type, int size, uint offset)
        {
            context.InputAssembler.InputLayout = Layout;
            context.VertexShader.Set(VertexShader);
            context.PixelShader.Set(PixelShader);
            context.DrawIndexed(size, (int)offset, 0);
        }

        public override void SetShaderParameters(Device device, DeviceContext context, MaterialParameters matParams)
        {
            base.SetShaderParameters(device, context, matParams);
        }
    }
}