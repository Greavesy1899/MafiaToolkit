using Rendering.Core;
using SharpDX.Direct3D11;

namespace Rendering.Graphics
{
    public class CollisionShader : BaseShader
    {
        public CollisionShader(Device Dx11Device, ShaderInitParams InitParams) : base(Dx11Device, InitParams) { }

        public override void InitCBuffersFrame(DeviceContext context, Camera camera, WorldSettings settings)
        {
            //throw new System.NotImplementedException();
        }

        public override void SetShaderParameters(Device device, DeviceContext context, MaterialParameters material)
        {
            //empty
        }      
    }
}
