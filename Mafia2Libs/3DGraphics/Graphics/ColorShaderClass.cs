using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ModelViewer.Programming.SystemClasses;
using Mafia2Tool;
using System.Diagnostics;

namespace ModelViewer.Programming.GraphicClasses
{
    public class ColorShaderClass
    {
        [StructLayout(LayoutKind.Sequential)]
        internal struct Vertex
        {
            public static int AppendAlignedElement = 12;
            public Vector3 position;
            public Vector4 color;
        }
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

        public ColorShaderClass() { }

        public bool Init(Device device, IntPtr WindowsHandle)
        {
            return InitShader(device, WindowsHandle, "VertexColor.hlsl", "PixelColor.hlsl");
        }
        private bool InitShader(Device device, IntPtr WindowsHandle, string vsFileName, string psFileName)
        {
            try
            {
                ShaderBytecode vertexShaderByteCode;
                ShaderBytecode pixelShaderByteCode;
                vsFileName = ToolkitSettings.ShaderPath + vsFileName;
                psFileName = ToolkitSettings.ShaderPath + psFileName;
                vertexShaderByteCode = ShaderBytecode.CompileFromFile(vsFileName, "ColorVertexShader", "vs_5_0", ShaderFlags.None, EffectFlags.None);
                pixelShaderByteCode = ShaderBytecode.CompileFromFile(psFileName, "ColorPixelShader", "ps_5_0", ShaderFlags.None, EffectFlags.None);
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
                        SemanticName = "COLOR",
                        SemanticIndex = 0,
                        Format = SharpDX.DXGI.Format.R32G32B32A32_Float,
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
            ConstantMatrixBuffer?.Dispose();
            ConstantMatrixBuffer = null;
            Layout?.Dispose();
            Layout = null;
            PixelShader?.Dispose();
            PixelShader = null;
            VertexShader?.Dispose();
            VertexShader = null;
        }
        public bool Render(DeviceContext deviceContext, int indexCount, Matrix WorldMatrix, Matrix ViewMatrix, Matrix ProjectionMatrix)
        {
            if (!SetShaderParameters(deviceContext, WorldMatrix, ViewMatrix, ProjectionMatrix))
            {
                return false;
            }
            RenderShader(deviceContext, indexCount);
            return true;
        }
        private bool SetShaderParameters(DeviceContext deviceContext, Matrix WorldMatrix, Matrix ViewMatrix, Matrix ProjectionMatrix)
        {
            try
            {
                WorldMatrix.Transpose();
                ViewMatrix.Transpose();
                ProjectionMatrix.Transpose();
                DataStream mappedResource;
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
            deviceContext.DrawIndexed(indexCount, 0, 0);
        }
    }
}