using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using Utils.Settings;
using ResourceTypes.Materials;

namespace Rendering.Graphics
{
    public class DebugShader : BaseShader
    {
        public VertexShader VertexShader { get; set; }
        public PixelShader PixelShader { get; set; }
        public InputLayout Layout { get; set; }
        public Buffer ConstantMatrixBuffer { get; set; }

        public DebugShader(Device device, InputElement[] element, string psPath, string vsPath, string vsEntryPoint, string psEntryPoint)
        {
            if (!Init(device, element, vsPath, psPath, vsEntryPoint, psEntryPoint))
                throw new System.Exception("Failed to load Shader!");
        }

        public override bool Init(Device device, InputElement[] element, string vsFileName, string psFileName, string vsEntryPoint, string psEntryPoint)
        {
            ShaderBytecode pixelShaderByteCode;
            ShaderBytecode vertexShaderByteCode;

            vsFileName = ToolkitSettings.ShaderPath + vsFileName;
            psFileName = ToolkitSettings.ShaderPath + psFileName;

            pixelShaderByteCode = ShaderBytecode.CompileFromFile(psFileName, psEntryPoint, "ps_4_0", ShaderFlags.None, EffectFlags.None);
            vertexShaderByteCode = ShaderBytecode.CompileFromFile(vsFileName, vsEntryPoint, "vs_4_0", ShaderFlags.None, EffectFlags.None);
            PixelShader = new PixelShader(device, pixelShaderByteCode);
            VertexShader = new VertexShader(device, vertexShaderByteCode);
            Layout = new InputLayout(device, ShaderSignature.GetInputSignature(vertexShaderByteCode), element);

            ConstantMatrixBuffer = ConstantBufferFactory.ConstructBuffer<MatrixBuffer>(device, "MatrixBuffer");

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

        public override void InitCBuffersFrame(DeviceContext context, Camera camera, LightClass light)
        {
            //throw new System.NotImplementedException();
        }

        public override void SetSceneVariables(DeviceContext deviceContext, Matrix WorldMatrix, Camera camera)
        {
            Matrix tMatrix = WorldMatrix;
            Matrix vMatrix = camera.ViewMatrix;
            Matrix cMatrix = camera.ProjectionMatrix;
            vMatrix.Transpose();
            cMatrix.Transpose();
            tMatrix.Transpose();

            MatrixBuffer matrixBuffer = new MatrixBuffer()
            {
                world = tMatrix,
                view = vMatrix,
                projection = cMatrix
            };
            ConstantBufferFactory.UpdateVertexBuffer(deviceContext, ConstantMatrixBuffer, 0, matrixBuffer);
        }

        public override void Render(DeviceContext deviceContext, SharpDX.Direct3D.PrimitiveTopology type, int size, uint offset)
        {
            deviceContext.InputAssembler.InputLayout = Layout;
            deviceContext.VertexShader.Set(VertexShader);
            deviceContext.PixelShader.Set(PixelShader);

            switch (type)
            {
                case SharpDX.Direct3D.PrimitiveTopology.LineList:
                case SharpDX.Direct3D.PrimitiveTopology.TriangleList:
                    deviceContext.DrawIndexed(size, (int)offset, 0);
                    break;
                case SharpDX.Direct3D.PrimitiveTopology.LineStrip:
                    deviceContext.Draw(size, 0);
                    break;
                default:
                    break;
            }
        }

        public override void SetShaderParameters(Device device, DeviceContext context, Material material)
        {
            //empty
        }
    }
}