using System;

namespace ResourceTypes.FrameResource
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
    public enum SingleMeshFlags : int //Need to sort these out.
    {
        flag_1 = 1,
        flag_2 = 2,
        flag_4 = 4,
        flag_8 = 8,
        Unk14_Flag = 16,
        flag_32 = 32,
        Unk13_Flag = 64,
        flag_128 = 128,
        flag_256 = 256,
        flag_512 = 512,
        flag_1024 = 1024,
        flag_2048 = 2048,
        flag_4096 = 4096,
        flag_8192 = 8192,
        flag_16384 = 16384,
        flag_32768 = 32768,
        flag_65536 = 65536,
        flag_131072 = 131072,
        flag_262144 = 262144,
        flag_524288 = 524288,
        flag_1048576 = 1048576,
        flag_2097152 = 2097152,
        flag_4194304 = 4194304,
        flag_8388608 = 8388608,
        flag_16777216 = 16777216,
        flag_33554432 = 33554432,
        flag_67108864 = 67108864,
        flag_134217728 = 134217728,
        flag_268435456 = 268435456,
        ParentIndex2_Flag = 536870912,
        OM_Flag = 1073741824,
    }
}
