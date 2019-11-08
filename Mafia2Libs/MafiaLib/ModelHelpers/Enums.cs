using System;
using System.ComponentModel;

namespace Utils.Models
{
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
        Handle_RLeft = 2014,
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
        //potential sizes

        Position = (1 << 0),                                               
        Position2D = (1 << 1),
        Normals = (1 << 2),
        Tangent = (1 << 4), // 0x00000010
        Skin = (1 << 6), // 0x00000040
        Color = (1 << 7), // 0x00000080
        Texture = (1 << 8),
        TexCoords0 = 256, // 0x00000100
        TexCoords1 = 512, // 0x00000200
        TexCoords2 = 1024, // 0x00000400
        ShadowTexture = (1 << 15), // 0x00008000 
        Color1 = (1 << 17), // 0x00020000
        BBCoeffs = (1 << 18), // 0x00040000
        DamageGroup = (1 << 20), // 0x00100000
    }

    //Fireboyd's work from nomad
//    enum MafiaVertexFormat : int
//    {
//        Position = (1 << 0),
//        Position2D = (1 << 1),
//        Normal = (1 << 2),

//        Tangent = (1 << 4),

//        Skin = (1 << 6),
//        Color = (1 << 7),
//        Texture = (1 << 8),

//        Texture1 = (1 << 9),
//        Texture2 = (1 << 10),
//        Texture3 = (1 << 11),
//        Texture4 = (1 << 12),

//        ShadowTexture = (1 << 15),

//        Color1 = (1 << 17),
//        BBCoeffs = (1 << 18),
//        Velocity = (1 << 19),

//        HelpingIndices = (1 << 20),
//        MorphIndices = (1 << 21),

//        FloatAlpha = (1 << 25),
//        Data_4_U8 = (1 << 26),

//        InstanceMatrix = (1 << 28),
//        InstanceGrass = (1 << 29),

//#if VFF_USE_DYNAMIC_PARAMETER
//    DynamicParameter    = (1 << 30), // Mafia III
//#else
//        InstanceGrassOrigin = (1 << 30), // Mafia II
//#endif
//    };




}
