using System.Numerics;
using System.Runtime.InteropServices;
using Utils.Types;
using Vortice.Direct3D11;

namespace Rendering.Graphics
{
    class Shader_50760736 : BaseShader
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct Shader_50760736Params
        {
            public Vector4 C005_EmissiveFacadeColorAndIntensity;
        }
        protected ID3D11Buffer ConstantShaderParamBuffer { get; set; }
        protected Shader_50760736Params ShaderParams { get; private set; }

        public Shader_50760736(DirectX11Class Dx11Object, ShaderInitParams InitParams) : base(Dx11Object, InitParams) { }

        public override bool Init(DirectX11Class Dx11Object, ShaderInitParams InitParams)
        {
            if (!base.Init(Dx11Object, InitParams))
            {
                return false;
            }

            ConstantShaderParamBuffer = ConstantBufferFactory.ConstructBuffer<Shader_50760736Params>(Dx11Object.Device, "ShaderParamBuffer");

            return true;
        }
        public override void SetSceneVariables(ID3D11DeviceContext deviceContext, Matrix4x4 WorldMatrix, Camera camera)
        {
            base.SetSceneVariables(deviceContext, WorldMatrix, camera);
            ConstantBufferFactory.UpdatePixelBuffer(deviceContext, ConstantShaderParamBuffer, 2, ShaderParams);
        }

        public override void SetShaderParameters(ID3D11Device device, ID3D11DeviceContext deviceContext, MaterialParameters matParams)
        {
            base.SetShaderParameters(device, deviceContext, matParams);

            Shader_50760736Params parameters = new Shader_50760736Params();
            var material = matParams.RuntimeMaterialData;

            var param = material.GetParamtersFor(2562079585);
            if (param != null)
            {
                parameters.C005_EmissiveFacadeColorAndIntensity = new Vector4(param[0], param[1], param[2], param[3]);
            }

            if (material == null)
            {
                ID3D11ShaderResourceView texture = RenderStorageSingleton.Instance.TextureCache[0];
                deviceContext.PSSetShaderResource(0, texture);
                ShaderParams = parameters;
            }
            else
            {
                var RuntimeMatData = matParams.RuntimeMaterialData;
                ID3D11ShaderResourceView[] textures = new ID3D11ShaderResourceView[2];

                ulong TextureFile = RuntimeMatData.GetSamplerTexture(3388704532);             
                textures[0] = RenderStorageSingleton.Instance.TextureCache[TextureFile];
                TextureFile = RuntimeMatData.GetSamplerTexture(3405482118);
                textures[1] = RenderStorageSingleton.Instance.TextureCache[TextureFile];

                deviceContext.PSSetShaderResources(0, textures);
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
