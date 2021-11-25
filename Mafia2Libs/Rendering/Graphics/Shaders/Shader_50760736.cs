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

        public Shader_50760736(ID3D11Device Dx11Device, ShaderInitParams InitParams) : base(Dx11Device, InitParams) { }

        public override bool Init(ID3D11Device Dx11Device, ShaderInitParams InitParams)
        {
            if (!base.Init(Dx11Device, InitParams))
            {
                return false;
            }

            ConstantShaderParamBuffer = ConstantBufferFactory.ConstructBuffer<Shader_50760736Params>(Dx11Device, "ShaderParamBuffer");

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
            var material = matParams.MaterialData;

            var param = material.GetParameterByKey("C005");
            if (param != null)
            {
                parameters.C005_EmissiveFacadeColorAndIntensity = new Vector4(param.Paramaters[0], param.Paramaters[1], param.Paramaters[2], param.Paramaters[3]);
            }

            if (material == null)
            {
                ID3D11ShaderResourceView texture = RenderStorageSingleton.Instance.TextureCache[0];
                deviceContext.PSSetShaderResource(0, texture);
                ShaderParams = parameters;
            }
            else
            {

                ID3D11ShaderResourceView[] textures = new ID3D11ShaderResourceView[2];

                HashName TextureFile = material.GetTextureByID("S000");
                if (TextureFile != null)
                {
                    textures[0] = RenderStorageSingleton.Instance.TextureCache[TextureFile.Hash];
                }
                else
                {
                    textures[0] = RenderStorageSingleton.Instance.TextureCache[0];
                }

                TextureFile = material.GetTextureByID("S011");
                if (TextureFile != null)
                {
                    textures[1] = RenderStorageSingleton.Instance.TextureCache[TextureFile.Hash];
                }
                else
                {
                    textures[1] = RenderStorageSingleton.Instance.TextureCache[0];
                }

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
