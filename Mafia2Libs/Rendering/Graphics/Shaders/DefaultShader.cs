using SharpDX.Direct3D11;
using Rendering.Core;
using System.Runtime.InteropServices;
using SharpDX;
using Utils.Types;

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

        public DefaultShader(Device Dx11Device, ShaderInitParams InitParams) : base(Dx11Device, InitParams) { }

        public override bool Init(Device Dx11Device, ShaderInitParams InitParams)
        {
            if(!base.Init(Dx11Device, InitParams))
            {
                return false;
            }

            ConstantExtraParameterBuffer = ConstantBufferFactory.ConstructBuffer<ExtraParameterBuffer>(Dx11Device, "ExtraBuffer");

            return true;
        }
        public override void Render(DeviceContext context, SharpDX.Direct3D.PrimitiveTopology type, int size, uint offset)
        {
            base.Render(context, type, size, offset);
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
                HashName TextureFile = material.GetTextureByID("S000");
                ShaderResourceView[] ShaderTextures = new ShaderResourceView[2];
                if (TextureFile != null)
                {
                    ShaderTextures[0] = RenderStorageSingleton.Instance.TextureCache[TextureFile.Hash];
                }
                else
                {
                    ShaderTextures[0] = RenderStorageSingleton.Instance.TextureCache[0];
                }

                TextureFile = material.GetTextureByID("S001");
                if (TextureFile != null)
                {
                    ShaderTextures[1] = RenderStorageSingleton.Instance.TextureCache[TextureFile.Hash];
                    extraParams.hasTangentSpace = 1;
                }
                else
                {
                    ShaderTextures[1] = RenderStorageSingleton.Instance.TextureCache[1];
                    extraParams.hasTangentSpace = 0;
                }

                context.PixelShader.SetShaderResources(0, ShaderTextures.Length, ShaderTextures);
            }

            if(previousRenderType != extraParams.hasTangentSpace)
            {
                ConstantBufferFactory.UpdatePixelBuffer(context, ConstantExtraParameterBuffer, 2, extraParams);
            }         
        }
    }
}