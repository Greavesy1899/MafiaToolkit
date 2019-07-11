
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct3D11;
using Utils.Types;

namespace Rendering.Graphics
{
    public class RenderFrame : IRenderer
    {
        public override void InitBuffers(Device d3d, DeviceContext deviceContext)
        {
            throw new NotImplementedException();
        }

        public override void Render(Device device, DeviceContext deviceContext, Camera camera, LightClass light)
        {
            throw new NotImplementedException();
        }

        public override void Select()
        {
            throw new NotImplementedException();
        }

        public override void SetTransform(Vector3 position, Matrix33 rotation)
        {
            throw new NotImplementedException();
        }

        public override void SetTransform(Matrix matrix)
        {
            throw new NotImplementedException();
        }

        public override void Shutdown()
        {
            throw new NotImplementedException();
        }

        public override void Unselect()
        {
            throw new NotImplementedException();
        }

        public override void UpdateBuffers(Device device, DeviceContext deviceContext)
        {
            throw new NotImplementedException();
        }
    }
}
