using Rendering.Core;
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