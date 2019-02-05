using SharpDX.Direct3D11;
using System.Collections.Generic;

namespace Rendering.Graphics
{
    public class ShaderManager
    {
        public Dictionary<ulong, BaseShader> shaders;

        public ShaderManager() { }
        public bool Init(Device device)
        {
            shaders = new Dictionary<ulong, BaseShader>();
            shaders.Add(0, new DefaultShader(device, "LightPS.hlsl", "LightVS.hlsl", "LightVertexShader", "LightPixelShader"));
            shaders.Add(601151254, new Shader_601151254(device, "LightPS.hlsl", "LightVS.hlsl", "LightVertexShader", "PS_601151254"));
            return true;
        }

        public void Shutdown()
        {
            foreach(KeyValuePair<ulong, BaseShader> shader in shaders)
                shader.Value.Shutdown();

            shaders.Clear();
        }
    }
}
