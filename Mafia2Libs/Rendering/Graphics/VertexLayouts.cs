using System.Numerics;
using System.Runtime.InteropServices;
using Vortice.Direct3D11;

namespace Rendering.Graphics
{
    public static class VertexLayouts
    {
        public static class BasicLayout
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct Vertex
            {
                public Vector3 Position;
                public int Colour;
            }

            public static InputElementDescription[] GetLayout()
            {
                return new InputElementDescription[]
                {
                    new InputElementDescription()
                    {
                        SemanticName = "POSITION",
                        SemanticIndex = 0,
                        Format = Vortice.DXGI.Format.R32G32B32_Float,
                        Slot = 0,
                        AlignedByteOffset = 0,
                        Classification = InputClassification.PerVertexData,
                        InstanceDataStepRate = 0
                    },
                    new InputElementDescription()
                    {
                        SemanticName = "COLOR",
                        SemanticIndex = 0,
                        Format = Vortice.DXGI.Format.R8G8B8A8_UNorm,
                        Slot = 0,
                        AlignedByteOffset = InputElementDescription.AppendAligned,
                        Classification = InputClassification.PerVertexData,
                        InstanceDataStepRate = 0
                    },
                };
            }
        }

        public class CollisionLayout
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct Vertex
            {
                public Vector3 Position;
                public Vector3 Normal;
                public int Colour;
            }

            public static InputElementDescription[] GetLayout()
            {
                return new InputElementDescription[]
                {
                    new InputElementDescription()
                    {
                        SemanticName = "POSITION",
                        SemanticIndex = 0,
                        Format = Vortice.DXGI.Format.R32G32B32_Float,
                        Slot = 0,
                        AlignedByteOffset = 0,
                        Classification = InputClassification.PerVertexData,
                        InstanceDataStepRate = 0
                    },
                    new InputElementDescription()
                    {
                        SemanticName = "NORMAL",
                        SemanticIndex = 0,
                        Format = Vortice.DXGI.Format.R32G32B32_Float,
                        Slot = 0,
                        AlignedByteOffset = InputElementDescription.AppendAligned,
                        Classification = InputClassification.PerVertexData,
                        InstanceDataStepRate = 0
                    },
                    new InputElementDescription()
                    {
                        SemanticName = "COLOR",
                        SemanticIndex = 0,
                        Format = Vortice.DXGI.Format.R8G8B8A8_UNorm,
                        Slot = 0,
                        AlignedByteOffset = InputElementDescription.AppendAligned,
                        Classification = InputClassification.PerVertexData,
                        InstanceDataStepRate = 0
                    },
                };
            }
        }

        public class NormalLayout
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct Vertex
            {
                public Vector3 Position;
                public Vector3 Normal;
                public Vector3 Tangent;
                public Vector3 Binormal;
                public Vector2 TexCoord0;
                public Vector2 TexCoord7;
                public uint InstanceID;
            }

public static InputElementDescription[] GetLayout()
{
    return new InputElementDescription[]
    {
        new InputElementDescription()
        {
            SemanticName = "POSITION",
            SemanticIndex = 0,
            Format = Vortice.DXGI.Format.R32G32B32_Float,
            Slot = 0,
            AlignedByteOffset = 0,
            Classification = InputClassification.PerVertexData,
            InstanceDataStepRate = 0
        },
        new InputElementDescription()
        {
            SemanticName = "NORMAL",
            SemanticIndex = 0,
            Format = Vortice.DXGI.Format.R32G32B32_Float,
            Slot = 0,
            AlignedByteOffset = InputElementDescription.AppendAligned, // Should compute correctly
            Classification = InputClassification.PerVertexData,
            InstanceDataStepRate = 0
        },
        new InputElementDescription()
        {
            SemanticName = "TANGENT",
            SemanticIndex = 0,
            Format = Vortice.DXGI.Format.R32G32B32_Float,
            Slot = 0,
            AlignedByteOffset = InputElementDescription.AppendAligned,
            Classification = InputClassification.PerVertexData,
            InstanceDataStepRate = 0
        },
        new InputElementDescription()
        {
            SemanticName = "BINORMAL",
            SemanticIndex = 0,
            Format = Vortice.DXGI.Format.R32G32B32_Float,
            Slot = 0,
            AlignedByteOffset = InputElementDescription.AppendAligned,
            Classification = InputClassification.PerVertexData,
            InstanceDataStepRate = 0
        },
        new InputElementDescription()
        {
            SemanticName = "TEXCOORD",
            SemanticIndex = 0,
            Format = Vortice.DXGI.Format.R32G32_Float,
            Slot = 0,
            AlignedByteOffset = InputElementDescription.AppendAligned,
            Classification = InputClassification.PerVertexData,
            InstanceDataStepRate = 0
        },
        new InputElementDescription()
        {
            SemanticName = "TEXCOORD",
            SemanticIndex = 1,
            Format = Vortice.DXGI.Format.R32G32_Float,
            Slot = 0,
            AlignedByteOffset = InputElementDescription.AppendAligned,
            Classification = InputClassification.PerVertexData,
            InstanceDataStepRate = 0
        },
        new InputElementDescription() // Adding the INSTANCEID element
        {
            SemanticName = "INSTANCEID", // The semantic name should match what is expected in the shader
            SemanticIndex = 0,
            Format = Vortice.DXGI.Format.R32_UInt,
            Slot = 0,
            AlignedByteOffset = InputElementDescription.AppendAligned, // Ensure this calculates correctly
            Classification = InputClassification.PerInstanceData,
            InstanceDataStepRate = 1 // Indicates it changes per instance
        }
    };
}

            
        }
    }
}