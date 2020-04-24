using System.Runtime.InteropServices;
using ResourceTypes.Materials;
using SharpDX;
using SharpDX.Direct3D11;

namespace Rendering.Graphics
{
    class Shader_601151254 : BaseShader
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct Shader_601151254Params
        {
            public Vector4 C002_MaterialColor;
        }

        protected Buffer ConstantShaderParamBuffer { get; set; }
        protected Shader_601151254Params ShaderParams { get; private set; }

        public Shader_601151254(Device device, InputElement[] elements, string psPath, string vsPath, string vsEntryPoint, string psEntryPoint)
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
            ConstantShaderParamBuffer = ConstantBufferFactory.ConstructBuffer<Shader_601151254Params>(device, "ShaderParamBuffer");
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
        public override void SetSceneVariables(DeviceContext deviceContext, Matrix WorldMatrix, Camera camera)
        {
            base.SetSceneVariables(deviceContext, WorldMatrix, camera);
            ConstantBufferFactory.UpdatePixelBuffer(deviceContext, ConstantShaderParamBuffer, 2, ShaderParams);
        }
        public override void InitCBuffersFrame(DeviceContext context, Camera camera, LightClass light)
        {
            base.InitCBuffersFrame(context, camera, light);
        }
        public override void SetShaderParameters(Device device, DeviceContext deviceContext, MaterialParameters matParams)
        {
            base.SetShaderParameters(device, deviceContext, matParams);

            Shader_601151254Params parameters = new Shader_601151254Params();
            var material = matParams.MaterialData;

            if (material.Parameters.ContainsKey("C002"))
            {
                ShaderParameter param = material.Parameters["C002"];
                parameters.C002_MaterialColor = new Vector4(param.Paramaters[0], param.Paramaters[1], param.Paramaters[2], param.Paramaters[3]);
            }
            else
            {
                parameters.C002_MaterialColor = new Vector4(0f);
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
