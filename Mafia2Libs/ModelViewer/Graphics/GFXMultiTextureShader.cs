using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ModelViewer.System;
using System.Diagnostics;

namespace ModelViewer.Graphics
{
    public class MultiTextureShaderClass
    {
        [StructLayout(LayoutKind.Sequential)]
        internal struct MatrixBuffer
        {
            public Matrix world;
            public Matrix view;
            public Matrix projection;
        }

        public VertexShader VertexShader { get; set; }
        public PixelShader PixelShader { get; set; }
        public InputLayout Layout { get; set; }
        public SharpDX.Direct3D11.Buffer ConstantMatrixBuffer { get; set; }
        public SamplerState SamplerState { get; set; }

        public MultiTextureShaderClass() { }

        public bool Init(Device device, IntPtr WindowsHandle)
        {
            return InitShader(device, WindowsHandle, "MultiTextureVS.hlsl", "MultiTexturePS.hlsl");
        }
        private bool InitShader(Device device, IntPtr WindowsHandle, string vsFileName, string psFileName)
        {
            try
            {
                ShaderBytecode vertexShaderByteCode;
                ShaderBytecode pixelShaderByteCode;
                vsFileName = SystemConfigClass.ShaderFilePath + vsFileName;
                psFileName = SystemConfigClass.ShaderFilePath + psFileName;
                vertexShaderByteCode = ShaderBytecode.CompileFromFile(vsFileName, "MultiTextureVertexShader", "vs_5_0", ShaderFlags.None, EffectFlags.None);
                pixelShaderByteCode = ShaderBytecode.CompileFromFile(psFileName, "MultiTexturePixelShader", "ps_5_0", ShaderFlags.None, EffectFlags.None);
                VertexShader = new VertexShader(device, vertexShaderByteCode);
                PixelShader = new PixelShader(device, pixelShaderByteCode);

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
                    }
                };

                Layout = new InputLayout(device, ShaderSignature.GetInputSignature(vertexShaderByteCode), inputElements);

                vertexShaderByteCode.Dispose();
                pixelShaderByteCode.Dispose();

                BufferDescription MatrixBufDesc = new BufferDescription()
                {
                    Usage = ResourceUsage.Dynamic,
                    SizeInBytes = Utilities.SizeOf<MatrixBuffer>(),
                    BindFlags = BindFlags.ConstantBuffer,
                    CpuAccessFlags = CpuAccessFlags.Write,
                    OptionFlags = ResourceOptionFlags.None,
                    StructureByteStride = 0
                };

                ConstantMatrixBuffer = new SharpDX.Direct3D11.Buffer(device, MatrixBufDesc);
                SamplerStateDescription samplerDesc = new SamplerStateDescription()
                {
                    Filter = Filter.MinMagMipLinear,
                    AddressU = TextureAddressMode.Wrap,
                    AddressV = TextureAddressMode.Wrap,
                    AddressW = TextureAddressMode.Wrap,
                    MipLodBias = 0,
                    MaximumAnisotropy = 1,
                    ComparisonFunction = Comparison.Always,
                    BorderColor = new Color4(0, 0, 0, 0),
                    MinimumLod = 0,
                    MaximumLod = float.MaxValue
                };
                SamplerState = new SamplerState(device, samplerDesc);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ini shader: " + ex.Message);
                Debug.WriteLine(VertexShader);
                return false;
            }
        }
        public void Shutdown()
        {
            ShutdownShader();
        }
        private void ShutdownShader()
        {
            SamplerState?.Dispose();
            SamplerState = null;
            ConstantMatrixBuffer?.Dispose();
            ConstantMatrixBuffer = null;
            Layout?.Dispose();
            Layout = null;
            PixelShader?.Dispose();
            PixelShader = null;
            VertexShader?.Dispose();
            VertexShader = null;
        }
        public bool Render(DeviceContext deviceContext, int indexCount, Matrix WorldMatrix, Matrix ViewMatrix, Matrix ProjectionMatrix, ShaderResourceView[] texture)
        {
            if (!SetShaderParameters(deviceContext, WorldMatrix, ViewMatrix, ProjectionMatrix, texture))
            {
                return false;
            }
            RenderShader(deviceContext, indexCount);
            return true;
        }
        private bool SetShaderParameters(DeviceContext deviceContext, Matrix WorldMatrix, Matrix ViewMatrix, Matrix ProjectionMatrix, ShaderResourceView[] texture)
        {
            try
            {
                DataStream mappedResource;

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
                deviceContext.PixelShader.SetShaderResources(0, 2, texture);
                return true;
            }
            catch
            {
                return false;
            }
        }
        private void RenderShader(DeviceContext deviceContext, int indexCount)
        {
            deviceContext.InputAssembler.InputLayout = Layout;
            deviceContext.VertexShader.Set(VertexShader);
            deviceContext.PixelShader.Set(PixelShader);
            deviceContext.PixelShader.SetSampler(0, SamplerState);
            deviceContext.DrawIndexed(indexCount, 0, 0);
        }
    }
}