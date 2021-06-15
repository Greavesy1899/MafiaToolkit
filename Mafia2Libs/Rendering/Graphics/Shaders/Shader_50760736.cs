using System.Runtime.InteropServices;
using SharpDX;
using SharpDX.Direct3D11;
using Utils.Types;

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

        public Shader_50760736(Device Dx11Device, ShaderInitParams InitParams) : base(Dx11Device, InitParams) { }

        public override bool Init(Device Dx11Device, ShaderInitParams InitParams)
        {
            if (!base.Init(Dx11Device, InitParams))
            {
                return false;
            }

            ConstantShaderParamBuffer = ConstantBufferFactory.ConstructBuffer<Shader_50760736Params>(Dx11Device, "ShaderParamBuffer");

            return true;
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

            var param = material.GetParameterByKey("C005");
            if (param != null)
            {
                parameters.C005_EmissiveFacadeColorAndIntensity = new Vector4(param.Paramaters[0], param.Paramaters[1], param.Paramaters[2], param.Paramaters[3]);
            }

            if (material == null)
            {
                ShaderResourceView texture = RenderStorageSingleton.Instance.TextureCache[0];
                deviceContext.PixelShader.SetShaderResource(0, texture);
                ShaderParams = parameters;
            }
            else
            {

                ShaderResourceView[] textures = new ShaderResourceView[2];

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
