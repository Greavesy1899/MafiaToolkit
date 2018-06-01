using System;

namespace Mafia2
{
    public enum ObjectType
    {
        Joint = 0,
        SingleMesh,
        Frame,
        Light,
        Camera,
        Component_U00000005,
        Sector,
        Dummy,
        ParticleDeflector = 10,
        Area = 12,
        Target = 14,
        Model = 17,
        Collision = 472
    }

    [Flags]
    public enum VertexFlags : uint
    {
        Position = 1,
        Normals = 4,
        Tangent = 16, // 0x00000010
        BlendData = 64, // 0x00000040
        flag_0x80 = 128, // 0x00000080
        TexCoords0 = 256, // 0x00000100
        TexCoords1 = 512, // 0x00000200
        TexCoords2 = 1024, // 0x00000400
        TexCoords7 = 32768, // 0x00008000
        flag_0x20000 = 131072, // 0x00020000
        flag_0x40000 = 262144, // 0x00040000
        flag_0x100000 = 1048576, // 0x00100000
    }
}
