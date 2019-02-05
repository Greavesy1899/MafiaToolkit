using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using System.Runtime.InteropServices;
using Mafia2Tool;

namespace Rendering.Graphics
{
    public class ShaderClass
    {
        [StructLayout(LayoutKind.Sequential)]
        internal struct MatrixBuffer
        {
            public Matrix world;
            public Matrix view;
            public Matrix projection;
        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct DCameraBuffer
        {
            public Vector3 cameraPosition;
            public float padding;
        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct LightBuffer
        {
            public Vector4 ambientColor;
            public Vector4 diffuseColor;
            public Vector3 LightDirection;
            public float specularPower;
            public Vector4 specularColor;
        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct ShaderParams
        {
            public int EnableTexture;
            public Vector4 C007MaterialColor;
        }

        public VertexShader VertexShader { get; set; }
        public PixelShader PixelShader { get; set; }
        public InputLayout Layout { get; set; }
        public Buffer ConstantMatrixBuffer { get; set; }
        public Buffer ConstantLightBuffer { get; set; }
        public Buffer ConstantCameraBuffer { get; set; }
        public Buffer ConstantShaderParamBuffer { get; set; }
        public SamplerState SamplerState { get; set; }

        public ShaderClass(Device device, string psPath, string vsPath, string vsEntryPoint, string psEntryPoint)
        {
            InitShader(device, vsPath, psPath, vsEntryPoint, psEntryPoint);
        }

        private bool InitShader(Device device, string vsFileName, string psFileName, string vsEntryPoint, string psEntryPoint)
        {
            ShaderBytecode vertexShaderByteCode;
            ShaderBytecode pixelShaderByteCode;

            vsFileName = ToolkitSettings.ShaderPath + vsFileName;
            psFileName = ToolkitSettings.ShaderPath + psFileName;

            pixelShaderByteCode = ShaderBytecode.CompileFromFile(psFileName, psEntryPoint, "ps_5_0", ShaderFlags.None, EffectFlags.None);
            vertexShaderByteCode = ShaderBytecode.CompileFromFile(vsFileName, vsEntryPoint, "vs_5_0", ShaderFlags.None, EffectFlags.None);
            PixelShader = new PixelShader(device, pixelShaderByteCode);
            VertexShader = new VertexShader(device, vertexShaderByteCode);
            Layout = new InputLayout(device, ShaderSignature.GetInputSignature(vertexShaderByteCode), VertexLayouts.NormalLayout.GetLayout());

            vertexShaderByteCode.Dispose();
            pixelShaderByteCode.Dispose();

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
                SizeInBytes = Utilities.SizeOf<LightBuffer>(),
                BindFlags = BindFlags.ConstantBuffer,
                CpuAccessFlags = CpuAccessFlags.Write,
                OptionFlags = ResourceOptionFlags.None,
                StructureByteStride = 0
            };

            ConstantShaderParamBuffer = new Buffer(device, shaderParamDesc);
            return true;
        }
        public void Shutdown()
        {
            ShutdownShader();
        }
        private void ShutdownShader()
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
        public bool PrepareRender(DeviceContext deviceContext, Matrix WorldMatrix, Camera camera, LightClass light)
        {
            if (!SetShaderParameters(deviceContext, WorldMatrix, camera, light))
            {
                return false;
            }
            return true;
        }
        private bool SetShaderParameters(DeviceContext deviceContext, Matrix WorldMatrix, Camera camera, LightClass light)
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
            #region Constant Camera Buffer
            deviceContext.MapSubresource(ConstantCameraBuffer, MapMode.WriteDiscard, MapFlags.None, out mappedResource);
            var cameraBuffer = new DCameraBuffer()
            {
                cameraPosition = camera.Position,
                padding = 0.0f
            };
            mappedResource.Write(cameraBuffer);
            deviceContext.UnmapSubresource(ConstantCameraBuffer, 0);
            bufferSlotNumber = 1;
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
            deviceContext.MapSubresource(ConstantShaderParamBuffer, MapMode.WriteDiscard, MapFlags.None, out mappedResource);
            ShaderParams shaderParams = new ShaderParams()
            {
                EnableTexture = 1,
                C007MaterialColor = new Vector4(1.0f, 0.5f, 0.5f, 1.0f)
            };
            mappedResource.Write(shaderParams);
            deviceContext.UnmapSubresource(ConstantShaderParamBuffer, 0);
            bufferSlotNumber = 1;
            deviceContext.PixelShader.SetConstantBuffer(bufferSlotNumber, ConstantShaderParamBuffer);

            return true;
        }
        public void Render(DeviceContext deviceContext, int indexCount, int startIndex)
        {
            deviceContext.InputAssembler.InputLayout = Layout;
            deviceContext.VertexShader.Set(VertexShader);
            deviceContext.PixelShader.Set(PixelShader);
            deviceContext.PixelShader.SetSampler(0, SamplerState);
            deviceContext.DrawIndexed(indexCount, startIndex, 0);
        }
    }
}