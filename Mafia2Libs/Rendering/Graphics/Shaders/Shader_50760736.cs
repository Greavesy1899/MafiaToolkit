using System.Runtime.InteropServices;
using ResourceTypes.Materials;
using SharpDX;
using SharpDX.Direct3D11;

namespace Rendering.Graphics
{
    class Shader_50760736 : BaseShader
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct Shader_50760736Params
        {
            public Vector4 C005_EmissiveFacadeColorAndIntensity;
        }
        protected Buffer ConstantShaderParamBuffer { get; set; }
        protected Shader_50760736Params ShaderParams { get; private set; }

        public Shader_50760736(Device device, InputElement[] elements, string psPath, string vsPath, string vsEntryPoint, string psEntryPoint)
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
            ConstantShaderParamBuffer = ConstantBufferFactory.ConstructBuffer<Shader_50760736Params>(device, "ShaderParamBuffer");
            return true;
        }
        public override void Render(DeviceContext deviceContext, SharpDX.Direct3D.PrimitiveTopology type, int numTriangles, uint offset)
        {
            deviceContext.InputAssembler.InputLayout = Layout;
            deviceContext.VertexShader.Set(VertexShader);
            deviceContext.PixelShader.Set(PixelShader);
            deviceContext.PixelShader.SetSampler(0, SamplerState);
            deviceContext.DrawIndexed(numTriangles, (int)offset, 0);
        }
        public override void InitCBuffersFrame(DeviceContext context, Camera camera, LightClass light)
        {
            base.InitCBuffersFrame(context, camera, light);
        }
        public override void SetSceneVariables(DeviceContext deviceContext, Matrix WorldMatrix, Camera camera)
        {
            base.SetSceneVariables(deviceContext, WorldMatrix, camera);
            ConstantBufferFactory.UpdatePixelBuffer(deviceContext, ConstantShaderParamBuffer, 2, ShaderParams);
        }

        public override void SetShaderParameters(Device device, DeviceContext deviceContext, MaterialParameters matParams)
        {
            base.SetShaderParameters(device, deviceContext, matParams);

            Shader_50760736Params parameters = new Shader_50760736Params();
            var material = matParams.MaterialData;

            if (material.Parameters.ContainsKey("C005"))
            {
                ShaderParameter param = material.Parameters["C005"];
                parameters.C005_EmissiveFacadeColorAndIntensity = new Vector4(param.Paramaters[0], param.Paramaters[1], param.Paramaters[2], param.Paramaters[3]);
            }
            else
            {
                parameters.C005_EmissiveFacadeColorAndIntensity = new Vector4(0f);
            }

            if (material == null)
            {
                ShaderResourceView texture = RenderStorageSingleton.Instance.TextureCache[0];
                deviceContext.PixelShader.SetShaderResource(0, texture);
                ShaderParams = parameters;
            }
            else
            {

                ShaderParameterSampler sampler;
                ShaderResourceView[] textures = new ShaderResourceView[2];
                if (material.Samplers.TryGetValue("S000", out sampler))
                {
                    textures[0] = RenderStorageSingleton.Instance.TextureCache[sampler.TextureHash];
                }
                else
                {
                    textures[0] = RenderStorageSingleton.Instance.TextureCache[0];
                }

                if (material.Samplers.TryGetValue("S011", out sampler))
                {
                    textures[1] = RenderStorageSingleton.Instance.TextureCache[sampler.TextureHash];
                }
                else
                {
                    textures[1] = RenderStorageSingleton.Instance.TextureCache[0];
                }

                deviceContext.PixelShader.SetShaderResources(0, textures.Length, textures);
            }

            ShaderParams = parameters;
        }

        public override void Shutdown()
        {
            base.Shutdown();
            ConstantShaderParamBuffer?.Dispose();
            ConstantShaderParamBuffer = null;
        }
    }
}
