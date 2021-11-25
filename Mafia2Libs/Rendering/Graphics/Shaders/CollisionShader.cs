using Rendering.Core;
using Vortice.Direct3D11;

namespace Rendering.Graphics
{
    public class CollisionShader : BaseShader
    {
        public CollisionShader(ID3D11Device Dx11Device, ShaderInitParams InitParams) : base(Dx11Device, InitParams) { }

        public override void InitCBuffersFrame(ID3D11DeviceContext context, Camera camera, WorldSettings settings)
        {
            //throw new System.NotImplementedException();
        }

        public override void SetShaderParameters(ID3D11Device device, ID3D11DeviceContext context, MaterialParameters material)
        {
            base.SetShaderParameters(device, context, material);
        }      
    }
}
