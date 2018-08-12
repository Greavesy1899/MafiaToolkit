using System;
using System.ComponentModel;

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
        Human = 14,
        CrashObject = 20,
        TrafficCar = 21,
        TrafficHuman = 22,
        TrafficTrain = 23,
        ActionPoint = 25,
        ActionPointScript = 30,
        ActionPointSearch = 32,
        Item = 36,
        Door = 38,
        Tree = 39,
        Sound = 41,
        SoundMixer = 43,
        Radio = 48,
        JukeBox = 49,
        StaticEntity = 52,
        Garage = 54,
        FrameWrapper = 55,
        ActorDetector = 56,
        Blocker = 63,
        StaticParticle = 66,
        FireTarget = 70,
        LightEntity = 71,
        Cutscene = 73,
        Telephone = 95,
        ScriptEntity = 98,
        DangerZone = 103,
        Pinup = 106,
        FramesController = 110,
        Wardrobe = 112,
        PhysicsScene = 113,
        CleanEntity = 114
    }

    public enum SkeletonBoneIDs
    {
        //BASE
        BaseRef = 1,
        Scale_Dummy = 1007,
        Trace = 1000,
        Opposite = 1004,
        CoatSim_Root = 1510,

        //BODY
        Blnd_Dummy = 1001,
        Aim_Dummy = 1002,
        Camera_Dummy = 1003,
        FootStep_L = 1005,
        FootStep_R = 1006,
        Hips = 101,
        Spine = 102,
        Spine1 = 103,
        Spine2 = 104,
        Spine3 = 105,
        Spine4 = 106,
        Neck = 107,
        Head = 108,
        LeftBreast = 109,
        RightBrest = 110,

        //LEFT LEG
        LeftUpLeg = 201,
        LeftUpLegRoll = 202,
        LeftLeg = 203,
        LeftFoot = 204,
        LeftToeBase = 205,
        L_LegSleeve = 206,
        LeftNates = 207,

        //Right Leg
        RightUpLeg = 301,
        RightUpLegRoll = 302,
        RightLeg = 303,
        RightFoot = 304,
        RightToeBase = 305,
        R_LegSleeve = 306,
        RightNates = 307,

        //Left Arm
        LeftShoulder = 401,
        LeftArm = 402,
        LeftArmRoll = 403,
        LeftForeArm = 404,
        LeftForeArmRoll = 405,

        //Left Hand
        LeftHand = 406,
        L_Sleeve = 407,
        LeftHandThumb1 = 410,
        LeftHandThumb2 = 411,
        LeftHandThumb3 = 412,
        LeftInHandIndex = 420,
        LeftHandIndex1 = 421,
        LeftHandIndex2 = 422,
        LeftHandIndex3 = 423,
        LeftHandMiddle1 = 431,
        LeftHandMiddle2 = 432,
        LeftHandMiddle3 = 433,
        LeftHandRing1 = 441,
        LeftHandRing2 = 442,
        LeftHandRing3 = 443,
        LeftInHandPinky = 450,
        LeftHandPinky1 = 451,
        LeftHandPinky2 = 452,
        LeftHandPinky3 = 453,

        //Left Arm
        RightShoulder = 501,
        RightArm = 502,
        RightArmRoll = 503,
        RightForeArm = 504,
        RightForeArmRoll = 505,

        //Left Hand
        RightHand = 506,
        R_Sleeve = 507,
        RightHandThumb1 = 510,
        RightHandThumb2 = 511,
        RightHandThumb3 = 512,
        RightInHandIndex = 520,
        RightHandIndex1 = 521,
        RightHandIndex2 = 522,
        RightHandIndex3 = 523,
        RightHandMiddle1 = 531,
        RightHandMiddle2 = 532,
        RightHandMiddle3 = 533,
        RightHandRing1 = 541,
        RightHandRing2 = 542,
        RightHandRing3 = 543,
        RightInHandPinky = 550,
        RightHandPinky1 = 551,
        RightHandPinky2 = 552,
        RightHandPinky3 = 553,

        //Head
        Jaw = 601,
        HairScale = 602,
        Hat = 603,
        Lo_MLip = 604,
        Up_Mlip = 605,
        Nose = 606,
        Gathers = 607,
        Hat_mesh = 608,
        Tongue = 609,
        Cigareta = 842,
        Bryle = 843,
        Scruff = 844,
        Fmv_cigareta = 845,

        //FaceLeft
        Brow2L = 726,
        Brow1L = 727,
        Lo_LidL = 728,
        NostrilL = 729,
        Up_CheekL = 730,
        CheekL = 731,
        Up_MLipL = 732,
        Lo_MLipL = 733,
        MCornerL = 734,
        Up_LidL = 735,
        EYE_L = 736,

        //FaceRight
        Brow2R = 826,
        Brow1R827,
        Lo_LidR = 828,
        NostrilR = 829,
        Up_CheekR = 830,
        CheekR = 831,
        Up_MLipR = 832,
        Lo_MLipR = 833,
        MCornerR = 834,
        Up_LidR = 835,
        EYE_R = 836,

        //WEAPONS LEFT
        GunHand_L = 2014,
        WeaponBodyLeft = 2001,
        ChangeMagazineLeft = 2002,
        TriggerLeft = 2003,
        FireLeft = 2004,
        CapLeft = 2005,
        UpshotLeft = 2006,
        CylinderFireLeft = 2007,
        CylinderOutLeft = 2008,
        ReloadLeft = 2009,
        UnloadLeft = 2010,
        OpenLeft = 2011,
        MuzzleLeft = 2012,
        ShellLeft = 2013,
        andle_RLeft = 2014,
        Handle_LLeft = 2015,

        //WEAPONS RIGHT
        WeaponBody = 2101,
        ChangeMagazine = 2102,
        Trigger = 2103,
        Handle_L = 2104,
        Shell = 2105,
        Upshot = 2106,
        CylinderFire = 2107,
        CylinderOut = 2108,
        Cap = 2109,
        Unload = 2110,
        Open = 2111,
        Muzzle = 2112,
        GunHand_R = 2114,
        bipod = 2116,
        Tripod = 2117,
        TripodAxe = 2118,
        WeaponAxe = 2119,
        Handle_R = 2120,
        WeaponOffset = 2121,

        //COAT LEFT
        L_FrontCoat01 = 1600,
        L_FrontCoat02 = 1601,
        L_FrontCoat03 = 1602,
        L_FrontCoat04 = 1603,
        L_FrontCoat05 = 1604,
        L_FrontCoat06 = 1605,
        L_FrontCoat07 = 1606,
        L_sideCoat01 = 1610,
        L_sideCoat02 = 1611,
        L_sideCoat03 = 1612,
        L_sideCoat04 = 1613,
        L_sideCoat05 = 1614,
        L_BackCoat01 = 1620,
        L_BackCoat02 = 1621,
        L_BackCoat03 = 1622,
        L_BackCoat04 = 1623,
        L_BackCoat05 = 1624,
        L_BackCoat06 = 625,

        //COAT RIGHT
        R_FrontCoat01 = 650,
        R_FrontCoat02 = 1651,
        R_FrontCoat03 = 1652,
        R_FrontCoat04 = 1653,
        R_FrontCoat05 = 1654,
        R_FrontCoat06 = 1655,
        R_FrontCoat07 = 1656,
        R_sideCoat01 = 1640,
        R_sideCoat02 = 1641,
        R_sideCoat03 = 1642,
        R_sideCoat04 = 1643,
        R_sideCoat05 = 1644,
        R_BackCoat01 = 1630,
        R_BackCoat02 = 1631,
        R_BackCoat03 = 1632,
        R_BackCoat04 = 1633,
        R_BackCoat05 = 1634,
        R_BackCoat06 = 1635,

        //PROP SAKO
        Bone01 = 3000,
        Bone02 = 3001,
        Bone03 = 3002,
        Bone04 = 3003,
        Bone05 = 3004,
        Bone06 = 3005,
        Bone07 = 3006,
        Bone08 = 3007,
        Bone09 = 3008,
        Bone10 = 3009,
        Bone11 = 3010,
        Bone12 = 3011,
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
    public enum NameTableFlags : int //There is a possibility that this is correct.
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
