using SharpDX;
using SharpDX.Direct3D11;
using System.Runtime.InteropServices;

namespace Rendering.Graphics
{
    public class VertexLayouts
    {
        public class NormalLayout
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct Vertex
            {
                public Vector3 position;
                public Vector2 tex0;
                public Vector2 tex7;
                public Vector3 normal;
            }

            public static InputElement[] GetLayout()
            {
                return new InputElement[]
                {
                    new InputElement("POSITION", 0, SharpDX.DXGI.Format.R32G32B32_Float, 0, 0, InputClassification.PerVertexData, 0),
                    new InputElement("TEXCOORD", 0, SharpDX.DXGI.Format.R32G32_Float, 0, 12, InputClassification.PerVertexData, 0),
                    new InputElement("TEXCOORD", 1, SharpDX.DXGI.Format.R32G32_Float, 0, 20, InputClassification.PerVertexData, 0),
                    new InputElement("NORMAL", 0, SharpDX.DXGI.Format.R32G32B32_Float, 0, 28, InputClassification.PerVertexData, 0),
                };

            }
        }
    }
}