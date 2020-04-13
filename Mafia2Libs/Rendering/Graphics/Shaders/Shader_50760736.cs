using System.Runtime.InteropServices;
using ResourceTypes.Materials;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using Utils.Settings;

namespace Rendering.Graphics
{
    class Shader_50760736 : BaseShader
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct Shader_50760736Params
        {
            public Vector4 C005_EmissiveFacadeColorAndIntensity;
        }

        public VertexShader VertexShader { get; set; }
        public PixelShader PixelShader { get; set; }
        public InputLayout Layout { get; set; }
        public Buffer ConstantMatrixBuffer { get; set; }
        public Buffer ConstantLightBuffer { get; set; }
        public Buffer ConstantCameraBuffer { get; set; }
        public Buffer ConstantShaderParamBuffer { get; set; }
        public SamplerState SamplerState { get; set; }
        public Shader_50760736Params ShaderParams { get; private set; }
        private LightClass lighting = null;

        public Shader_50760736(Device device, InputElement[] elements, string psPath, string vsPath, string vsEntryPoint, string psEntryPoint)
        {
            if (!Init(device, elements, vsPath, psPath, vsEntryPoint, psEntryPoint))
                throw new System.Exception("Failed to load Shader!");
        }

        public override bool Init(Device device, InputElement[] elements, string vsFileName, string psFileName, string vsEntryPoint, string psEntryPoint)
        {
            ShaderBytecode pixelShaderByteCode;
            ShaderBytecode vertexShaderByteCode;

            vsFileName = ToolkitSettings.ShaderPath + vsFileName;
            psFileName = ToolkitSettings.ShaderPath + psFileName;

            pixelShaderByteCode = ShaderBytecode.CompileFromFile(psFileName, psEntryPoint, "ps_4_0", ShaderFlags.None, EffectFlags.None);
            vertexShaderByteCode = ShaderBytecode.CompileFromFile(vsFileName, vsEntryPoint, "vs_4_0", ShaderFlags.None, EffectFlags.None);
            PixelShader = new PixelShader(device, pixelShaderByteCode);
            VertexShader = new VertexShader(device, vertexShaderByteCode);
            Layout = new InputLayout(device, ShaderSignature.GetInputSignature(vertexShaderByteCode), elements);

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

            ConstantCameraBuffer = ConstantBufferFactory.ConstructBuffer<DCameraBuffer>(device, "CameraBuffer");
            ConstantLightBuffer = ConstantBufferFactory.ConstructBuffer<LightBuffer>(device, "LightBuffer");
            ConstantMatrixBuffer = ConstantBufferFactory.ConstructBuffer<MatrixBuffer>(device, "MatrixBuffer");
            ConstantShaderParamBuffer = ConstantBufferFactory.ConstructBuffer<Shader_50760736Params>(device, "ShaderParamsBuffer");

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
            var cameraBuffer = new DCameraBuffer()
            {
                cameraPosition = camera.Position,
                padding = 0.0f
            };
            ConstantBufferFactory.UpdateVertexBuffer(deviceContext, ConstantCameraBuffer, 1, cameraBuffer);

            if (lighting == null || !lighting.Equals(light))
            {
                LightBuffer lightbuffer = new LightBuffer()
                {
                    ambientColor = light.AmbientColor,
                    diffuseColor = light.DiffuseColour,
                    LightDirection = light.Direction,
                    specularColor = light.SpecularColor,
                    specularPower = light.SpecularPower
                };
                lighting = light;
                ConstantBufferFactory.UpdatePixelBuffer(deviceContext, ConstantLightBuffer, 0, lightbuffer);
            }
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
            ConstantBufferFactory.UpdatePixelBuffer(deviceContext, ConstantShaderParamBuffer, 1, ShaderParams);
        }

        public override void SetShaderParameters(Device device, DeviceContext deviceContext, Material material)
        {
            Shader_50760736Params parameters = new Shader_50760736Params();

            if (material.Parameters.ContainsKey("C005"))
            {
                ShaderParameter param = material.Parameters["C005"];
                parameters.C005_EmissiveFacadeColorAndIntensity = new Vector4(param.Paramaters[0], param.Paramaters[1], param.Paramaters[2], param.Paramaters[3]);
            }
            else
            {
                parameters.C005_EmissiveFacadeColorAndIntensity = new Vector4(0f);
            }

            if (material == null)
            {
                ShaderResourceView texture = RenderStorageSingleton.Instance.TextureCache[0];
                deviceContext.PixelShader.SetShaderResource(0, texture);
                ShaderParams = parameters;
            }
            else
            {

                ShaderParameterSampler sampler;
                ShaderResourceView[] textures = new ShaderResourceView[2];
                if (material.Samplers.TryGetValue("S000", out sampler))
                {
                    textures[0] = RenderStorageSingleton.Instance.TextureCache[sampler.TextureHash];
                }
                else
                {
                    textures[0] = RenderStorageSingleton.Instance.TextureCache[0];
                }

                if (material.Samplers.TryGetValue("S011", out sampler))
                {
                    textures[1] = RenderStorageSingleton.Instance.TextureCache[sampler.TextureHash];
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
