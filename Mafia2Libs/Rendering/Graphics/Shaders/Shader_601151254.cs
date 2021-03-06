﻿using System.Numerics;
using System.Runtime.InteropServices;
using Vortice.Direct3D11;

namespace Rendering.Graphics
{
    class Shader_601151254 : BaseShader
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct Shader_601151254Params
        {
            public Vector4 C002_MaterialColor;
        }

        protected ID3D11Buffer ConstantShaderParamBuffer { get; set; }
        protected Shader_601151254Params ShaderParams { get; private set; }

        public Shader_601151254(ID3D11Device Dx11Device, ShaderInitParams InitParams) : base(Dx11Device, InitParams) { }

        public override bool Init(ID3D11Device Dx11Device, ShaderInitParams InitParams)
        {
            if (!base.Init(Dx11Device, InitParams))
            {
                return false;
            }

            ConstantShaderParamBuffer = ConstantBufferFactory.ConstructBuffer<Shader_601151254Params>(Dx11Device, "ShaderParamBuffer");

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

            Shader_601151254Params parameters = new Shader_601151254Params();
            var material = matParams.MaterialData;

            var param = material.GetParameterByKey("C002");
            if (param != null)
            {
                parameters.C002_MaterialColor = new Vector4(param.Paramaters[0], param.Paramaters[1], param.Paramaters[2], param.Paramaters[3]);
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
