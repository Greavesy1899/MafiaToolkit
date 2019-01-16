using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using System;
using System.Runtime.InteropServices;
using ModelViewer.Programming.SystemClasses;

namespace ModelViewer.Programming.GraphicClasses
{
    public class ShaderClass
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct Vertex
        {
            public Vector3 position;
            public Vector2 texture;
            public Vector3 normal;
        }
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

        public VertexShader VertexShader { get; set; }
        public PixelShader PixelShader { get; set; }
        public InputLayout Layout { get; set; }
        public SharpDX.Direct3D11.Buffer ConstantMatrixBuffer { get; set; }
        public SharpDX.Direct3D11.Buffer ConstantLightBuffer { get; set; }
        public SharpDX.Direct3D11.Buffer ConstantCameraBuffer { get; set; }
        public SamplerState SamplerState { get; set; }

        public ShaderClass() { }

        public bool Init(Device device, IntPtr WindowsHandle)
        {
            return InitShader(device, WindowsHandle, "LightVS.hlsl", "LightPS.hlsl");
        }
        private bool InitShader(Device device, IntPtr WindowsHandle, string vsFileName, string psFileName)
        {
            ShaderBytecode vertexShaderByteCode;
            ShaderBytecode pixelShaderByteCode;

            vsFileName = SystemConfigClass.ShaderFilePath + vsFileName;
            psFileName = SystemConfigClass.ShaderFilePath + psFileName;
            pixelShaderByteCode = ShaderBytecode.CompileFromFile(psFileName, "LightPixelShader", "ps_5_0", ShaderFlags.None, EffectFlags.None);
            vertexShaderByteCode = ShaderBytecode.CompileFromFile(vsFileName, "LightVertexShader", "vs_5_0", ShaderFlags.None, EffectFlags.None);
            PixelShader = new PixelShader(device, pixelShaderByteCode);
            VertexShader = new VertexShader(device, vertexShaderByteCode);
            InputElement[] inputElements = new InputElement[]
            {
                    new InputElement()
                    {
                        SemanticName = "POSITION",
                        SemanticIndex = 0,
                        Format = SharpDX.DXGI.Format.R32G32B32_Float,
                        Slot = 0,
                        AlignedByteOffset = 0,
                        Classification = InputClassification.PerVertexData,
                        InstanceDataStepRate = 0
                    },
                    new InputElement()
                    {
                        SemanticName = "TEXCOORD",
                        SemanticIndex = 0,
                        Format = SharpDX.DXGI.Format.R32G32_Float,
                        Slot = 0,
                        AlignedByteOffset = InputElement.AppendAligned,
                        Classification = InputClassification.PerVertexData,
                        InstanceDataStepRate = 0
                    },
                    new InputElement()
                    {
                        SemanticName = "NORMAL",
                        SemanticIndex = 0,
                        Format = SharpDX.DXGI.Format.R32G32B32_Float,
                        Slot = 0,
                        AlignedByteOffset = InputElement.AppendAligned,
                        Classification = InputClassification.PerVertexData,
                        InstanceDataStepRate = 0
                    }
            };

            Layout = new InputLayout(device, ShaderSignature.GetInputSignature(vertexShaderByteCode), inputElements);

            vertexShaderByteCode.Dispose();
            pixelShaderByteCode.Dispose();

            SamplerStateDescription samplerDesc = new SamplerStateDescription()
            {
                Filter = Filter.MinMagMipLinear,
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

            ConstantMatrixBuffer = new SharpDX.Direct3D11.Buffer(device, MatrixBuffDesc);
            var camaraBufferDesc = new BufferDescription()
            {
                Usage = ResourceUsage.Dynamic,
                SizeInBytes = Utilities.SizeOf<DCameraBuffer>(),
                BindFlags = BindFlags.ConstantBuffer,
                CpuAccessFlags = CpuAccessFlags.Write,
                OptionFlags = ResourceOptionFlags.None,
                StructureByteStride = 0
            };
            ConstantCameraBuffer = new SharpDX.Direct3D11.Buffer(device, camaraBufferDesc);

            BufferDescription LightBuffDesc = new BufferDescription()
            {
                Usage = ResourceUsage.Dynamic,
                SizeInBytes = Utilities.SizeOf<LightBuffer>(),
                BindFlags = BindFlags.ConstantBuffer,
                CpuAccessFlags = CpuAccessFlags.Write,
                OptionFlags = ResourceOptionFlags.None,
                StructureByteStride = 0
            };

            ConstantLightBuffer = new SharpDX.Direct3D11.Buffer(device, LightBuffDesc);
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
            SamplerState?.Dispose();
            SamplerState = null;
            Layout?.Dispose();
            Layout = null;
            PixelShader?.Dispose();
            PixelShader = null;
            VertexShader?.Dispose();
            VertexShader = null;
        }
        public bool PrepareRender(DeviceContext deviceContext, Matrix WorldMatrix, Matrix ViewMatrix, Matrix ProjectionMatrix, Vector3 lightDirection, Vector4 ambientColor, Vector4 diffuseColor, Vector3 cameraPosition, Vector4 specularColor, float specularPower)
        {
            if (!SetShaderParameters(deviceContext, WorldMatrix, ViewMatrix, ProjectionMatrix, lightDirection, ambientColor, diffuseColor, cameraPosition, specularColor, specularPower))
            {
                return false;
            }
            return true;
        }
        private bool SetShaderParameters(DeviceContext deviceContext, Matrix WorldMatrix, Matrix ViewMatrix, Matrix ProjectionMatrix, Vector3 lightDirection, Vector4 ambientColor, Vector4 diffuseColour, Vector3 cameraPosition, Vector4 specularColor, float specularPower)
        {
            DataStream mappedResource;

            #region Constant Matrix Buffer
            WorldMatrix.Transpose();
            ViewMatrix.Transpose();
            ProjectionMatrix.Transpose();

            deviceContext.MapSubresource(ConstantMatrixBuffer, MapMode.WriteDiscard, MapFlags.None, out mappedResource);
            MatrixBuffer matrixBuffer = new MatrixBuffer()
            {
                world = WorldMatrix,
                view = ViewMatrix,
                projection = ProjectionMatrix
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
                cameraPosition = cameraPosition,
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
                ambientColor = ambientColor,
                diffuseColor = diffuseColour,
                LightDirection = lightDirection,
                specularColor = specularColor,
                specularPower = specularPower
            };
            mappedResource.Write(lightbuffer);
            deviceContext.UnmapSubresource(ConstantLightBuffer, 0);
            bufferSlotNumber = 0;
            deviceContext.PixelShader.SetConstantBuffer(bufferSlotNumber, ConstantLightBuffer);
            #endregion

            return true;
        }
        public void Render(DeviceContext deviceContext, int indexCount)
        {
            deviceContext.InputAssembler.InputLayout = Layout;
            deviceContext.VertexShader.Set(VertexShader);
            deviceContext.PixelShader.Set(PixelShader);
            deviceContext.PixelShader.SetSampler(0, SamplerState);
            deviceContext.DrawIndexed(indexCount, 0, 0);
        }
    }
}