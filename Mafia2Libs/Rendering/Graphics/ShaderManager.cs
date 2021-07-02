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
            // construct default shader
            ShaderInitParams DefaultInitParams = new ShaderInitParams();
            DefaultInitParams.Elements = VertexLayouts.NormalLayout.GetLayout();
            DefaultInitParams.PixelShaderFile = new ShaderInitParams.ShaderFileEntryPoint("LightPS.hlsl", "LightPixelShader", "ps_4_0");
            DefaultInitParams.VertexShaderFile = new ShaderInitParams.ShaderFileEntryPoint("LightVS.hlsl", "LightVertexShader", "vs_4_0");

            DefaultShader OurDefaultShader = new DefaultShader(device, DefaultInitParams);

            // construct debug shader
            ShaderInitParams DebugInitParams = new ShaderInitParams();
            DebugInitParams.Elements = VertexLayouts.BasicLayout.GetLayout();
            DebugInitParams.PixelShaderFile = new ShaderInitParams.ShaderFileEntryPoint("DebugPS.hlsl", "DebugPixelShader", "ps_4_0");
            DebugInitParams.VertexShaderFile = new ShaderInitParams.ShaderFileEntryPoint("DebugVS.hlsl", "DebugVertexShader", "vs_4_0");

            DefaultShader OurDebugShader = new DefaultShader(device, DebugInitParams);

            // construct collision shader
            ShaderInitParams CollisionInitParams = new ShaderInitParams();
            CollisionInitParams.Elements = VertexLayouts.CollisionLayout.GetLayout();
            CollisionInitParams.PixelShaderFile = new ShaderInitParams.ShaderFileEntryPoint("CollisionPS.hlsl", "CollisionShader", "ps_4_0");
            CollisionInitParams.VertexShaderFile = new ShaderInitParams.ShaderFileEntryPoint("CollisionVS.hlsl", "CollisionShader", "vs_4_0");

            DefaultShader OurCollisionShader = new DefaultShader(device, CollisionInitParams);

            // construct shader_601151254 shader
            ShaderInitParams Shader601151254_InitParams = new ShaderInitParams();
            Shader601151254_InitParams.Elements = VertexLayouts.NormalLayout.GetLayout();
            Shader601151254_InitParams.PixelShaderFile = new ShaderInitParams.ShaderFileEntryPoint("LightPS.hlsl", "PS_601151254", "ps_4_0");
            Shader601151254_InitParams.VertexShaderFile = new ShaderInitParams.ShaderFileEntryPoint("LightVS.hlsl", "LightVertexShader", "vs_4_0");

            DefaultShader OurShader601151254 = new DefaultShader(device, Shader601151254_InitParams);

            // construct shader_50760736 shader
            ShaderInitParams Shader_50760736_InitParams = new ShaderInitParams();
            Shader_50760736_InitParams.Elements = VertexLayouts.NormalLayout.GetLayout();
            Shader_50760736_InitParams.PixelShaderFile = new ShaderInitParams.ShaderFileEntryPoint("LightPS.hlsl", "PS_50760736", "ps_4_0");
            Shader_50760736_InitParams.VertexShaderFile = new ShaderInitParams.ShaderFileEntryPoint("LightVS.hlsl", "LightVertexShader", "vs_4_0");

            DefaultShader OurShader50760736 = new DefaultShader(device, Shader_50760736_InitParams);

            shaders = new Dictionary<ulong, BaseShader>();
            shaders.Add(0, OurDefaultShader);
            shaders.Add(1, OurDebugShader);
            shaders.Add(2, OurCollisionShader);
            shaders.Add(601151254, OurShader601151254);
            shaders.Add(50760736, OurShader50760736);

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
