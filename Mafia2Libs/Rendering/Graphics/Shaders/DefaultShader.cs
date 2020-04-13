using SharpDX.Direct3D11;
using ResourceTypes.Materials;

namespace Rendering.Graphics
{
    public class DefaultShader : BaseShader
    {
        public DefaultShader(Device device, InputElement[] elements, string psPath, string vsPath, string vsEntryPoint, string psEntryPoint)
        {
            if (!Init(device, elements, vsPath, psPath, vsEntryPoint, psEntryPoint))
            {
                throw new System.Exception("Failed to load Shader!");
            }
        }

        public override bool Init(Device device, InputElement[] elements, string vsFileName, string psFileName, string vsEntryPoint, string psEntryPoint)
        {
            if(!base.Init(device, elements, vsFileName, psFileName, vsEntryPoint, psEntryPoint))
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
            var material = matParams.MaterialData;
            if (material == null)
            {
                ShaderResourceView texture = RenderStorageSingleton.Instance.TextureCache[0];
                context.PixelShader.SetShaderResource(0, texture);
            }
            else
            {
                
                ShaderParameterSampler sampler;
                ShaderResourceView texture = null;
                if (material.Samplers.TryGetValue("S000", out sampler))
                {
                    texture = RenderStorageSingleton.Instance.TextureCache[sampler.TextureHash];
                }
                else
                {
                    texture = RenderStorageSingleton.Instance.TextureCache[0];
                }

                context.PixelShader.SetShaderResource(0, texture);

                if (material.Samplers.TryGetValue("S001", out sampler))
                {
                    texture = RenderStorageSingleton.Instance.TextureCache[sampler.TextureHash];
                }
                else
                {
                    texture = RenderStorageSingleton.Instance.TextureCache[0];
                }
                context.PixelShader.SetShaderResource(1, texture);
            }
        }
    }
}