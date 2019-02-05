using SharpDX;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rendering.Graphics
{
    public abstract class BaseShader
    {
        public abstract void Init(Device device, string vsFileName, string psFileName, string vsEntryPoint, string psEntryPoint);
        public abstract void SetInputLayout(DeviceContext context, InputElement[] elements);
        public abstract void SetSceneVariables(DeviceContext context, Matrix WorldMatrix, Matrix ViewMatrix, Matrix ProjectionMatrix, Vector3 lightDirection, Vector4 ambientColor, Vector4 diffuseColor, Vector3 cameraPosition, Vector4 specularColor, float specularPower);
    }
}
