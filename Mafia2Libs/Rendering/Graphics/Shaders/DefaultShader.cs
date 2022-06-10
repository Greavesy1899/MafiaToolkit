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

        public DefaultShader(DirectX11Class Dx11Object, ShaderInitParams InitParams) : base(Dx11Object, InitParams) { }

        public override bool Init(DirectX11Class Dx11Object, ShaderInitParams InitParams)
        {
            if(!base.Init(Dx11Object, InitParams))
            {
                return false;
            }

            ConstantExtraParameterBuffer = ConstantBufferFactory.ConstructBuffer<ExtraParameterBuffer>(Dx11Object.Device, "ExtraBuffer");

            return true;
        }
        public override void Render(DirectX11Class Dx11Object, PrimitiveTopology type, int size, uint offset)
        {
            base.Render(Dx11Object, type, size, offset);
        }

        public override void SetShaderParameters(ID3D11Device device, ID3D11DeviceContext context, MaterialParameters matParams)
        {           
            base.SetShaderParameters(device, context, matParams);

            int previousHasTangentSpace = extraParams.hasTangentSpace;

            var RuntimeMatData = matParams.RuntimeMaterialData;
            ulong TextureFile = RuntimeMatData.GetSamplerTexture(3388704532);
            ID3D11ShaderResourceView[] ShaderTextures = new ID3D11ShaderResourceView[2];
            ShaderTextures[0] = RenderStorageSingleton.Instance.TextureCache[TextureFile];

            TextureFile = RuntimeMatData.GetSamplerTexture(3388704533);
            TextureFile = (TextureFile != 0 ? TextureFile : 1);
            ShaderTextures[1] = RenderStorageSingleton.Instance.TextureCache[TextureFile];
            extraParams.hasTangentSpace = 1;

            context.PSSetShaderResources(0, ShaderTextures);

            if (previousHasTangentSpace != extraParams.hasTangentSpace)
            {
                ConstantBufferFactory.UpdatePixelBuffer(context, ConstantExtraParameterBuffer, 2, extraParams);
            }         
        }
    }
}