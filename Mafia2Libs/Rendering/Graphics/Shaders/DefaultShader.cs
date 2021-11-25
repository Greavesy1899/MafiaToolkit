using System.Numerics;
using System.Runtime.InteropServices;
using Utils.Types;
using Vortice.Direct3D;
using Vortice.Direct3D11;

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
        protected ID3D11Buffer ConstantExtraParameterBuffer { get; set; }
        protected int previousRenderType;

        public DefaultShader(ID3D11Device Dx11Device, ShaderInitParams InitParams) : base(Dx11Device, InitParams) { }

        public override bool Init(ID3D11Device Dx11Device, ShaderInitParams InitParams)
        {
            if(!base.Init(Dx11Device, InitParams))
            {
                return false;
            }

            ConstantExtraParameterBuffer = ConstantBufferFactory.ConstructBuffer<ExtraParameterBuffer>(Dx11Device, "ExtraBuffer");

            return true;
        }
        public override void Render(ID3D11DeviceContext context, PrimitiveTopology type, int size, uint offset)
        {
            base.Render(context, type, size, offset);
        }
        public override void SetShaderParameters(ID3D11Device device, ID3D11DeviceContext context, MaterialParameters matParams)
        {           
            base.SetShaderParameters(device, context, matParams);

            int previousHasTangentSpace = extraParams.hasTangentSpace;

            var material = matParams.MaterialData;
            if (material == null)
            {
                ID3D11ShaderResourceView texture = RenderStorageSingleton.Instance.TextureCache[0];
                context.PSSetShaderResource(0, texture);
            }
            else
            {     
                HashName TextureFile = material.GetTextureByID("S000");
                ID3D11ShaderResourceView[] ShaderTextures = new ID3D11ShaderResourceView[2];
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

                context.PSSetShaderResources(0, ShaderTextures);
            }

            if(previousRenderType != extraParams.hasTangentSpace)
            {
                ConstantBufferFactory.UpdatePixelBuffer(context, ConstantExtraParameterBuffer, 2, extraParams);
            }         
        }
    }
}