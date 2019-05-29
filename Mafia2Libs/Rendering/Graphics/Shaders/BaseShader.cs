using ResourceTypes.Materials;
using SharpDX;
using SharpDX.Direct3D11;
using System.Runtime.InteropServices;

namespace Rendering.Graphics
{
    public abstract class BaseShader
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

        public BaseShader() { }
        public abstract bool Init(Device device, string vsFileName, string psFileName, string vsEntryPoint, string psEntryPoint);
        public abstract void InitCBuffersFrame(DeviceContext context, Camera camera, LightClass light);
        public abstract void SetSceneVariables(DeviceContext context, Matrix WorldMatrix, Camera camera);
        public abstract void SetShaderParamters(Device device, DeviceContext deviceContext, Material material);
        public abstract void Render(DeviceContext context, SharpDX.Direct3D.PrimitiveTopology type, int size, uint offset);
        public abstract void Shutdown();
    }
}
