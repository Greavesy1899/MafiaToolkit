using System.Runtime.InteropServices;
using ResourceTypes.Materials;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using Utils.Settings;

namespace Rendering.Graphics
{
    class Shader_601151254 : BaseShader
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct Shader_601151254Params
        {
            public Vector4 C002_MaterialColor;
        }

        public VertexShader VertexShader { get; set; }
        public PixelShader PixelShader { get; set; }
        public InputLayout Layout { get; set; }
        public Buffer ConstantMatrixBuffer { get; set; }
        public Buffer ConstantLightBuffer { get; set; }
        public Buffer ConstantCameraBuffer { get; set; }
        public Buffer ConstantShaderParamBuffer { get; set; }
        public SamplerState SamplerState { get; set; }
        public Shader_601151254Params ShaderParams { get; private set; }

        public Shader_601151254(Device device, string psPath, string vsPath, string vsEntryPoint, string psEntryPoint)
        {
            if (!Init(device, vsPath, psPath, vsEntryPoint, psEntryPoint))
                throw new System.Exception("Failed to load Shader!");
        }

        public override bool Init(Device device, string vsFileName, string psFileName, string vsEntryPoint, string psEntryPoint)
        {
            ShaderBytecode pixelShaderByteCode;
            ShaderBytecode vertexShaderByteCode;

            vsFileName = ToolkitSettings.ShaderPath + vsFileName;
            psFileName = ToolkitSettings.ShaderPath + psFileName;

            pixelShaderByteCode = ShaderBytecode.CompileFromFile(psFileName, psEntryPoint, "ps_5_0", ShaderFlags.None, EffectFlags.None);
            vertexShaderByteCode = ShaderBytecode.CompileFromFile(vsFileName, vsEntryPoint, "vs_5_0", ShaderFlags.None, EffectFlags.None);
            PixelShader = new PixelShader(device, pixelShaderByteCode);
            VertexShader = new VertexShader(device, vertexShaderByteCode);
            Layout = new InputLayout(device, ShaderSignature.GetInputSignature(vertexShaderByteCode), VertexLayouts.NormalLayout.GetLayout());

            SamplerStateDescription samplerDesc = new SamplerStateDescription()
            {
                Filter = Filter.Anisotropic,
                AddressU = TextureAddressMode.Wrap,
                AddressV = TextureAddressMode.Wrap,
                AddressW = TextureAddressMode.Wrap,
                MipLodBias = 0,
                MaximumAnisotropy = 16,
                ComparisonFunction = Comparison.Always,
                BorderColor = new Color4(0, 0, 0, 0),
                MinimumLod = 0,
                MaximumLod = float.MaxValue
            };

            SamplerState = new SamplerState(device, samplerDesc);

            BufferDescription MatrixBuffDesc = new BufferDescription()
            {
                Usage = ResourceUsage.Dynamic,
                SizeInBytes = Utilities.SizeOf<MatrixBuffer>(),
                BindFlags = BindFlags.ConstantBuffer,
                CpuAccessFlags = CpuAccessFlags.Write,
                OptionFlags = ResourceOptionFlags.None,
                StructureByteStride = 0
            };

            ConstantMatrixBuffer = new Buffer(device, MatrixBuffDesc);
            var camaraBufferDesc = new BufferDescription()
            {
                Usage = ResourceUsage.Dynamic,
                SizeInBytes = Utilities.SizeOf<DCameraBuffer>(),
                BindFlags = BindFlags.ConstantBuffer,
                CpuAccessFlags = CpuAccessFlags.Write,
                OptionFlags = ResourceOptionFlags.None,
                StructureByteStride = 0
            };
            ConstantCameraBuffer = new Buffer(device, camaraBufferDesc);

            var LightBuffDesc = new BufferDescription()
            {
                Usage = ResourceUsage.Dynamic,
                SizeInBytes = Utilities.SizeOf<LightBuffer>(),
                BindFlags = BindFlags.ConstantBuffer,
                CpuAccessFlags = CpuAccessFlags.Write,
                OptionFlags = ResourceOptionFlags.None,
                StructureByteStride = 0
            };

            ConstantLightBuffer = new Buffer(device, LightBuffDesc);

            var shaderParamDesc = new BufferDescription()
            {
                Usage = ResourceUsage.Dynamic,
                SizeInBytes = Utilities.SizeOf<Shader_601151254Params>(),
                BindFlags = BindFlags.ConstantBuffer,
                CpuAccessFlags = CpuAccessFlags.Write,
                OptionFlags = ResourceOptionFlags.None,
                StructureByteStride = 0
            };

            ConstantShaderParamBuffer = new Buffer(device, shaderParamDesc);

            pixelShaderByteCode.Dispose();
            vertexShaderByteCode.Dispose();

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

        public override void InitCBuffersFrame(DeviceContext deviceContext, Camera camera, LightClass light)
        {
            DataStream mappedResource;
            #region Constant Camera Buffer
            deviceContext.MapSubresource(ConstantCameraBuffer, MapMode.WriteDiscard, MapFlags.None, out mappedResource);
            var cameraBuffer = new DCameraBuffer()
            {
                cameraPosition = camera.Position,
                padding = 0.0f
            };
            mappedResource.Write(cameraBuffer);
            deviceContext.UnmapSubresource(ConstantCameraBuffer, 0);
            int bufferSlotNumber = 1;
            deviceContext.VertexShader.SetConstantBuffer(bufferSlotNumber, ConstantCameraBuffer);
            #endregion
            #region Constant Light Buffer
            deviceContext.MapSubresource(ConstantLightBuffer, MapMode.WriteDiscard, MapFlags.None, out mappedResource);
            LightBuffer lightbuffer = new LightBuffer()
            {
                ambientColor = light.AmbientColor,
                diffuseColor = light.DiffuseColour,
                LightDirection = light.Direction,
                specularColor = light.SpecularColor,
                specularPower = light.SpecularPower
            };
            mappedResource.Write(lightbuffer);
            deviceContext.UnmapSubresource(ConstantLightBuffer, 0);
            bufferSlotNumber = 0;
            deviceContext.PixelShader.SetConstantBuffer(bufferSlotNumber, ConstantLightBuffer);
            #endregion
        }

        public override void SetSceneVariables(DeviceContext deviceContext, Matrix WorldMatrix, Camera camera)
        {
            DataStream mappedResource;

            #region Constant Matrix Buffer
            Matrix tMatrix = WorldMatrix;
            Matrix vMatrix = camera.ViewMatrix;
            Matrix cMatrix = camera.ProjectionMatrix;
            vMatrix.Transpose();
            cMatrix.Transpose();
            tMatrix.Transpose();

            deviceContext.MapSubresource(ConstantMatrixBuffer, MapMode.WriteDiscard, MapFlags.None, out mappedResource);
            MatrixBuffer matrixBuffer = new MatrixBuffer()
            {
                world = tMatrix,
                view = vMatrix,
                projection = cMatrix
            };
            mappedResource.Write(matrixBuffer);
            deviceContext.UnmapSubresource(ConstantMatrixBuffer, 0);
            int bufferSlotNumber = 0;
            deviceContext.VertexShader.SetConstantBuffer(bufferSlotNumber, ConstantMatrixBuffer);
            #endregion
            deviceContext.MapSubresource(ConstantShaderParamBuffer, MapMode.WriteDiscard, MapFlags.None, out mappedResource);
            mappedResource.Write(ShaderParams);
            deviceContext.UnmapSubresource(ConstantShaderParamBuffer, 0);
            bufferSlotNumber = 1;
            deviceContext.PixelShader.SetConstantBuffer(bufferSlotNumber, ConstantShaderParamBuffer);
        }

        public override void SetShaderParamters(Device device, DeviceContext deviceContext, Material material)
        {
            Shader_601151254Params parameters = new Shader_601151254Params();

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
            ConstantLightBuffer?.Dispose();
            ConstantLightBuffer = null;
            ConstantCameraBuffer?.Dispose();
            ConstantCameraBuffer = null;
            ConstantMatrixBuffer?.Dispose();
            ConstantMatrixBuffer = null;
            ConstantShaderParamBuffer?.Dispose();
            ConstantShaderParamBuffer = null;
            SamplerState?.Dispose();
            SamplerState = null;
            Layout?.Dispose();
            Layout = null;
            PixelShader?.Dispose();
            PixelShader = null;
            VertexShader?.Dispose();
            VertexShader = null;
        }
    }
}
