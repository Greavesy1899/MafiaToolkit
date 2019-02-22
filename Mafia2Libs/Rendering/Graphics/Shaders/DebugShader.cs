using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using System.Runtime.InteropServices;
using Mafia2Tool;
using Mafia2;

namespace Rendering.Graphics
{
    public class DebugShader : BaseShader
    {
        public VertexShader VertexShader { get; set; }
        public PixelShader PixelShader { get; set; }
        public InputLayout Layout { get; set; }
        public Buffer ConstantMatrixBuffer { get; set; }

        public DebugShader(Device device, string psPath, string vsPath, string vsEntryPoint, string psEntryPoint)
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
            Layout = new InputLayout(device, ShaderSignature.GetInputSignature(vertexShaderByteCode), VertexLayouts.BBoxLayout.GetLayout());

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

            pixelShaderByteCode.Dispose();
            vertexShaderByteCode.Dispose();

            return true;
        }

        public override void Shutdown()
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

        public override void SetSceneVariables(DeviceContext deviceContext, Matrix WorldMatrix, Camera camera, LightClass light)
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
        }

        public override void Render(DeviceContext deviceContext, uint numTriangles, uint offset)
        {
            deviceContext.InputAssembler.InputLayout = Layout;
            deviceContext.VertexShader.Set(VertexShader);
            deviceContext.PixelShader.Set(PixelShader);
            deviceContext.DrawIndexed((int)numTriangles, (int)offset, 0);
        }

        public override void SetShaderParamters(Device device, DeviceContext context, Material material)
        {
            //empty
        }
    }
}