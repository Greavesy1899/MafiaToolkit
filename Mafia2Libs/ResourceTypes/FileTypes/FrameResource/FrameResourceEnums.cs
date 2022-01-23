using System;

namespace ResourceTypes.FrameResource
{
    public enum FrameResourceObjectType
    {
        NULL = -1,
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
        flag_1 = 0x1,
        flag_2 = 0x2,
        flag_4 = 0x4,
        flag_8 = 0x8,
        Unk14_Flag = 0x10,
        flag_32 = 0x20,
        Unk13_Flag = 0x40,
        flag_128 = 0x80,
        flag_256 = 0x100,
        flag_512 = 0x200,
        flag_1024 = 0x400,
        flag_2048 = 0x800,
        flag_4096 = 0x1000,
        flag_8192 = 0x2000,
        flag_16384 = 0x4000,
        flag_32768 = 0x8000,
        flag_65536 = 0x10000,
        flag_131072 = 0x20000,
        flag_262144 = 0x40000,
        flag_524288 = 0x80000,
        flag_1048576 = 0x100000,
        flag_2097152 = 0x200000,
        flag_4194304 = 0x400000,
        flag_8388608 = 0x800000,
        flag_16777216 = 0x1000000,
        flag_33554432 = 0x2000000,
        flag_67108864 = 0x4000000,
        flag_134217728 = 0x8000000,
        flag_268435456 = 0x10000000,
        ParentIndex2_Flag = 0x20000000,
        OM_Flag = 0x40000000,
    }
}
