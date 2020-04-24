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
            shaders.Add(0, new DefaultShader(device, VertexLayouts.NormalLayout.GetLayout(), "LightPS.hlsl", "LightVS.hlsl", "LightVertexShader", "LightPixelShader"));
            shaders.Add(1, new DebugShader(device, VertexLayouts.BasicLayout.GetLayout(), "DebugPS.hlsl", "DebugVS.hlsl", "DebugVertexShader", "DebugPixelShader"));
            shaders.Add(2, new CollisionShader(device, VertexLayouts.CollisionLayout.GetLayout(), "CollisionPS.hlsl", "CollisionVS.hlsl", "CollisionShader", "CollisionShader"));
            shaders.Add(601151254, new Shader_601151254(device, VertexLayouts.NormalLayout.GetLayout(), "LightPS.hlsl", "LightVS.hlsl", "LightVertexShader", "PS_601151254"));
            shaders.Add(50760736, new Shader_50760736(device, VertexLayouts.NormalLayout.GetLayout(), "LightPS.hlsl", "LightVS.hlsl", "LightVertexShader", "PS_50760736"));
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
