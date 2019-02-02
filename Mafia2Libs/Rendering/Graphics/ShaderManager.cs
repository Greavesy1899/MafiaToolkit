using SharpDX.Direct3D11;
using System.Collections.Generic;

namespace Rendering.Graphics
{
    public class ShaderManager
    {
        Dictionary<ulong, ShaderClass> shaders;

        public ShaderManager() { }
        public bool Init(Device device)
        {
            shaders = new Dictionary<ulong, ShaderClass>();
            shaders.Add(0, new ShaderClass(device, "LightPS.hlsl", "LightVS.hlsl", "LightPixelShader"));
            return true;
        }

        public void Shutdown()
        {
            foreach(KeyValuePair<ulong, ShaderClass> shader in shaders)
                shader.Value.Shutdown();

            shaders.Clear();
        }
    }
}
