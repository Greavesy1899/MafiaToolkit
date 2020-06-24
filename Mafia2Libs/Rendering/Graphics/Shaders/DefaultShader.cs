using SharpDX.Direct3D11;
using ResourceTypes.Materials;
using Rendering.Core;
using System.Runtime.InteropServices;
using SharpDX;

namespace Rendering.Graphics
{
    public class DefaultShader : BaseShader
    {
        [StructLayout(LayoutKind.Sequential)]
        protected struct ExtraParameterBuffer
        {
            public int hasTangentSpace;
            public Vector3 padding;
        }

        private ExtraParameterBuffer extraParams;
        protected Buffer ConstantExtraParameterBuffer { get; set; }
        protected int previousRenderType;

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

            ConstantExtraParameterBuffer = ConstantBufferFactory.ConstructBuffer<ExtraParameterBuffer>(device, "ExtraBuffer");

            return true;
        }
        public override void InitCBuffersFrame(DeviceContext context, Camera camera, WorldSettings settings)
        {
            base.InitCBuffersFrame(context, camera, settings);
        }
        public override void Render(DeviceContext context, SharpDX.Direct3D.PrimitiveTopology type, int size, uint offset)
        {
            context.InputAssembler.InputLayout = Layout;
            context.VertexShader.Set(VertexShader);
            context.PixelShader.Set(PixelShader);
            context.PixelShader.SetSampler(0, SamplerState);
            context.DrawIndexed(size, (int)offset, 0);
        }
        public override void SetShaderParameters(Device device, DeviceContext context, MaterialParameters matParams)
        {           
            base.SetShaderParameters(device, context, matParams);

            int previousHasTangentSpace = extraParams.hasTangentSpace;

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
                    extraParams.hasTangentSpace = 1;
                }
                else
                {
                    texture = RenderStorageSingleton.Instance.TextureCache[1];
                    extraParams.hasTangentSpace = 0;
                }
                context.PixelShader.SetShaderResource(1, texture);
            }

            if(previousRenderType != extraParams.hasTangentSpace)
            {
                ConstantBufferFactory.UpdatePixelBuffer(context, ConstantExtraParameterBuffer, 2, extraParams);
            }         
        }
    }
}