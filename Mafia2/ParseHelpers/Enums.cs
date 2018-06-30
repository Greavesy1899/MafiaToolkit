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

    public enum CollisionTypes
    {
        Box = 1,
        Sphere,
        Capsule,
        Unk7 = 7
    }

    public enum ActorTypes
    {
        ScriptEntity = 98,
        Pinup = 106
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
        DamageGroup = 1048576, // 0x00100000
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

    [Flags]
    public enum MaterialFlags : uint //No idea which ones are used.
    {
        flag_1 = 1,
        flag_2 = 2,
        flag_4 = 4,
        flag_8 = 8,
        flag_16 = 16,
        flag_32 = 32,
        flag_64 = 64,
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
        flag_536870912 = 536870912,
        flag_1073741824 = 1073741824,
        flag_2147483648 = 2147483648
    }

    public enum BufferType
    {
        Vertex = 2,
        Index = 3,
    }

    public enum XMLDataType
    {
        Special = 1,
        Boolean,
        Float,
        String,
        Integer
    }

    public enum CollisionMaterials
    {
        Road,
        Road_Dusty,
        Pedestrian_Crossing,
        Heads_Of_Cats_UNK,
        Sidewalk,
        Tiles1,
        Grass,
        Ground_Loam,
        Gravel,
        Sand,
        Mud,
        Puddle,
        Water,
        Snow,
        Metal,
        Sheet_Metal,
        Mesh,
        Railing,
        Wood,
        Carpet,
        Wooden_Boards,
        Parquets,
        Gritty_Concrete,
        Tiles2,
        Wall,
        Plaster,
        Brick,
        Glass_Break1,
        Glass_Break2,
        Glass_BulletProof,
        Bushes_Trees,
        Universal_Hard,
        Universal_Squashy,
        Person,
        No_Shot_Coll,
        Paper,
        Upholstery,
        Cloth,
        Camera_Coll,
        Player_Coll,
        Sicily_Wall,
        Grass_Trashy,
        Grass_Negen,
        Grass_Trashy_Negen,
        SideWalk_Human,
        Car,
        Person_Headshot,
        Person_Leg,
        Person_Hand,
        Grass_Sicily,
        Hedgerow,
        Seabed,
        Channel,
        Road_KY,
        Road_Dusty_KY,
        SideWalk_KY,
        Tiles_KY,
        Wooden_Board_KY,
        Road_Tunnel,
        Railing_Concrete,
        Railing_Wood,
        Cartoon

    }
}
