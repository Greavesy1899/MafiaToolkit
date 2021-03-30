using ResourceTypes.Materials;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using System.Runtime.InteropServices;
using Rendering.Core;
using System.Windows;

namespace Rendering.Graphics
{
    public class ShaderInitParams
    {
        public struct ShaderFileEntryPoint
        {
            public string FilePath { get; set; }
            public string EntryPoint { get; set; }
            public string Target { get; set; }

            public ShaderFileEntryPoint(string InFilePath, string InEntryPoint, string InTarget)
            {
                FilePath = InFilePath;
                EntryPoint = InEntryPoint;
                Target = InTarget;
            }

            public bool IsValid()
            {
                return !string.IsNullOrEmpty(FilePath) && !string.IsNullOrEmpty(EntryPoint) && !string.IsNullOrEmpty(Target);
            }
        }

        public ShaderInitParams() { }
        public ShaderInitParams(InputElement[] InElements, ShaderFileEntryPoint InPSShader, ShaderFileEntryPoint InVSShader, ShaderFileEntryPoint InGSShader)
        {
            Elements = InElements;
            PixelShaderFile = InPSShader;
            VertexShaderFile = InVSShader;
            GeometryShaderFile = InGSShader;
        }

        public InputElement[] Elements { get; set; }
        public ShaderFileEntryPoint PixelShaderFile { get; set; }
        public ShaderFileEntryPoint VertexShaderFile { get; set; }
        public ShaderFileEntryPoint GeometryShaderFile { get; set; }
    }

    public class BaseShader
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
        protected struct EditorParameterBuffer
        {
            public Vector3 selectionColour;
            public int renderMode;
        }

        public struct MaterialParameters
        {
            public MaterialParameters(IMaterial material, Vector3 vector)
            {
                MaterialData = material;
                SelectionColour = vector;
            }

            public IMaterial MaterialData { get; set; }
            public Vector3 SelectionColour { get; set; }
        }
        protected VertexShader OurVertexShader { get; set; }
        protected PixelShader OurPixelShader { get; set; }
        protected GeometryShader OurGeometryShader { get; set; }
        protected InputLayout Layout { get; set; }
        protected Buffer ConstantMatrixBuffer { get; set; }
        protected Buffer ConstantLightBuffer { get; set; }
        protected Buffer ConstantCameraBuffer { get; set; }
        protected Buffer ConstantEditorParamsBuffer { get; set; }
        protected SamplerState SamplerState { get; set; }

        // These allow the editor to only make changes if the 
        // incoming changes are different.
        protected LightClass previousLighting = null;      
        protected Vector3 previousEditorParams;

        private const string ShaderPath = @"Shaders\";

        public BaseShader(Device Dx11Device, ShaderInitParams InitParams)
        {
            if (!Init(Dx11Device, InitParams))
            {
                MessageBox.Show("Failed to construct shader!", "Toolkit");
            }
        }

        public virtual bool Init(Device device, ShaderInitParams InitParams)
        {
            // Attempt to construct pixel shader
            if (InitParams.PixelShaderFile.IsValid())
            {
                ShaderBytecode PixelBytecode = ConstructBytecode(InitParams.PixelShaderFile);
                OurPixelShader = new PixelShader(device, PixelBytecode);
                PixelBytecode.Dispose();
            }

            // Attempt to construct vertex shader
            if (InitParams.VertexShaderFile.IsValid())
            {
                ShaderBytecode VertexBytecode = ConstructBytecode(InitParams.VertexShaderFile);
                OurVertexShader = new VertexShader(device, VertexBytecode);
                Layout = new InputLayout(device, ShaderSignature.GetInputSignature(VertexBytecode), InitParams.Elements);
                VertexBytecode.Dispose();
            }

            // Attempt to construct geometry shader
            if (InitParams.GeometryShaderFile.IsValid())
            {
                ShaderBytecode GeometryBytecode = ConstructBytecode(InitParams.GeometryShaderFile);
                OurGeometryShader = new GeometryShader(device, GeometryBytecode);
                GeometryBytecode.Dispose();
            }

            SamplerStateDescription samplerDesc = new SamplerStateDescription()
            {
                Filter = Filter.Anisotropic,
                AddressU = TextureAddressMode.Wrap,
                AddressV = TextureAddressMode.Wrap,
                AddressW = TextureAddressMode.Wrap,
                MipLodBias = 0,
                MaximumAnisotropy = 8,
                ComparisonFunction = Comparison.Always,
                BorderColor = new Color4(0, 0, 0, 0),
                MinimumLod = 0,
                MaximumLod = 0
            };

            SamplerState = new SamplerState(device, samplerDesc);

            ConstantCameraBuffer = ConstantBufferFactory.ConstructBuffer<DCameraBuffer>(device, "CameraBuffer");
            ConstantLightBuffer = ConstantBufferFactory.ConstructBuffer<LightBuffer>(device, "LightBuffer");
            ConstantMatrixBuffer = ConstantBufferFactory.ConstructBuffer<MatrixBuffer>(device, "MatrixBuffer");
            ConstantEditorParamsBuffer = ConstantBufferFactory.ConstructBuffer<EditorParameterBuffer>(device, "EditorBuffer");

            return true;
        }

        public virtual void InitCBuffersFrame(DeviceContext context, Camera camera, WorldSettings settings)
        {
            var cameraBuffer = new DCameraBuffer()
            {
                cameraPosition = camera.Position,
                padding = 0.0f
            };
            ConstantBufferFactory.UpdateVertexBuffer(context, ConstantCameraBuffer, 1, cameraBuffer);

            if (previousLighting == null || !previousLighting.Equals(settings.Lighting))
            {
                LightBuffer lightbuffer = new LightBuffer()
                {
                    ambientColor = settings.Lighting.AmbientColor,
                    diffuseColor = settings.Lighting.DiffuseColour,
                    LightDirection = settings.Lighting.Direction,
                    specularColor = settings.Lighting.SpecularColor,
                    specularPower = settings.Lighting.SpecularPower
                };
                previousLighting = settings.Lighting;
                ConstantBufferFactory.UpdatePixelBuffer(context, ConstantLightBuffer, 0, lightbuffer);
            }
        }

        public virtual void SetSceneVariables(DeviceContext context, Matrix WorldMatrix, Camera camera)
        {
            Matrix tMatrix = WorldMatrix;
            tMatrix.Transpose();

            MatrixBuffer matrixBuffer = new MatrixBuffer()
            {
                world = tMatrix,
                view = camera.ViewMatrixTransposed,
                projection = camera.ProjectionMatrixTransposed
            };
            ConstantBufferFactory.UpdateVertexBuffer(context, ConstantMatrixBuffer, 0, matrixBuffer);
        }

        public virtual void SetShaderParameters(Device device, DeviceContext deviceContext, MaterialParameters matParams)
        {
            if (!previousEditorParams.Equals(matParams.SelectionColour))
            {
                var editorParams = new EditorParameterBuffer()
                { 
                    selectionColour = matParams.SelectionColour
                };

                ConstantBufferFactory.UpdatePixelBuffer(deviceContext, ConstantEditorParamsBuffer, 1, editorParams);
                previousEditorParams = editorParams.selectionColour;
            }

            //experiments with samplers; currently the toolkit doesn't not support any types.
            /*SamplerStateDescription samplerDesc = new SamplerStateDescription()
            {
                Filter = Filter.Anisotropic,
                AddressU = (material != null) ? (TextureAddressMode)material.Samplers["S000"].SamplerStates[0] : TextureAddressMode.Wrap,
                AddressV = (material != null) ? (TextureAddressMode)material.Samplers["S000"].SamplerStates[1] : TextureAddressMode.Wrap,
                AddressW = (material != null) ? (TextureAddressMode)material.Samplers["S000"].SamplerStates[2] : TextureAddressMode.Wrap,
                MipLodBias = 0,
                MaximumAnisotropy = 16,
                ComparisonFunction = Comparison.Always,
                BorderColor = new Color4(0, 0, 0, 0),
                MinimumLod = 0,
                MaximumLod = float.MaxValue
            };

            SamplerState = new SamplerState(device, samplerDesc);*/
        }

        public virtual void Render(DeviceContext context, SharpDX.Direct3D.PrimitiveTopology type, int size, uint offset)
        {
            context.InputAssembler.InputLayout = Layout;

            // set shaders only if available
            if(OurVertexShader != null)
            {
                context.VertexShader.Set(OurVertexShader);
            }

            if (OurVertexShader != null)
            {
                context.PixelShader.Set(OurPixelShader);
                context.PixelShader.SetSampler(0, SamplerState);
            }

            if (OurVertexShader != null)
            {
                context.GeometryShader.Set(OurGeometryShader);
            }

            context.DrawIndexed(size, (int)offset, 0);

            Profiler.NumDrawCallsThisFrame++;
        }

        public virtual void Shutdown() 
        {
            ConstantLightBuffer?.Dispose();
            ConstantLightBuffer = null;
            ConstantCameraBuffer?.Dispose();
            ConstantCameraBuffer = null;
            ConstantMatrixBuffer?.Dispose();
            ConstantMatrixBuffer = null;
            ConstantEditorParamsBuffer?.Dispose();
            ConstantEditorParamsBuffer = null;
            SamplerState?.Dispose();
            SamplerState = null;
            Layout?.Dispose();
            Layout = null;
            OurPixelShader?.Dispose();
            OurPixelShader = null;
            OurVertexShader?.Dispose();
            OurVertexShader = null;
            OurGeometryShader?.Dispose();
            OurGeometryShader = null;
        }

        private ShaderBytecode ConstructBytecode(ShaderInitParams.ShaderFileEntryPoint ShaderFileData)
        {
            string ShaderFileName = ShaderPath + ShaderFileData.FilePath;

            return ShaderBytecode.CompileFromFile(ShaderFileName, ShaderFileData.EntryPoint, ShaderFileData.Target, ShaderFlags.None, EffectFlags.None);
        }
    }
}
