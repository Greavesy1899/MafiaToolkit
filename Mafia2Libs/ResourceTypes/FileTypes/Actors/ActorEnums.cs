using System;

namespace ResourceTypes.Actors
{
    public enum ActorTypes
    {
        None = 0,
        Human = 14,
        C_Player2 = 16,
        C_Car = 18,
        C_Train = 19,
        C_CrashObject = 20,
        C_TrafficCar = 21,
        C_TrafficHuman = 22,
        C_TrafficTrain = 23,
        ActionPoint = 25,
        ActionPointScript = 30,
        ActionPointSearch = 32,
        C_Item = 36,
        C_Door = 38,
        Tree = 39,
        Lift = 40,
        C_Sound = 41,
        SoundMixer = 43,
        Boat = 47,
        Radio = 48,
        JukeBox = 49,
        StaticEntity = 52,
        C_TranslocatedCar = 53,
        Garage = 54,
        FrameWrapper = 55,
        C_ActorDetector = 56,
        Blocker = 63,
        C_StaticWeapon = 64,
        C_StaticParticle = 66,
        FireTarget = 70,
        LightEntity = 71,
        C_Cutscene = 73,
        Telephone = 95,
        C_ScriptEntity = 98,
        DangerZone = 103,
        Airplane = 104,
        C_Pinup = 106,
        SpikeStrip = 107,
        C_DummyDoor = 109,
        FramesController = 110,
        Wardrobe = 112,
        PhysicsScene = 113,
        CleanEntity = 114
    }

    public enum ActorEDSTypes
    {
        Human = 14,
        C_Player2 = 16,
        C_Car = 18,
        C_Train = 19,
        C_ActionPointScript = 30,
    }

    [Flags]
    public enum ActorSoundEntityBehaviourFlags
    {
        PlayInWinter = 1,
        Loop = 2,
        UseAdvancedScene = 4,
        SectorRestricted = 8,
        PlayInDay = 16,
        PlayInNight = 32,
        PlayInRain = 64,
        PlayInSummer = 128,
        Unk0 = 256
    }

    [Flags]
    public enum ActorSoundEntityPlayType
    {
        RandomPosPerGroupOnly = 1,
        ImmediatePlay = 2,
    }

    [Flags]
    public enum ActorItemFlags
    {
        Physics = 0x40,
        AutoUse = 0x80
    }

    [Flags]
    public enum ActorCleanEntityFlags
    {
        ClearOnInit = 1,
        BlockPedestrians = 2,
        BlockTraffic = 4,
        PrimitiveType = 8,
    }

    [Flags]
    public enum ActorRadioFlags : int
    {
        Enabled = 1,
        Interactive = 2,
        Hidden = 4,
    }

    public enum HumanAIType : int
    {
        CIVILIAN,
        FRIEND,
        ENEMY,
        GUARD,
        POLICE,
        DETECTIVE,
        CELEMENTE,
        FALCONE,
        BOMBERS,
        BIKERS,
        IRISH,
        CHINESE,
        SCRIPTED,
        VINCI,
        FMV,
        SELLER,
        SELLER_FUNK
    }

    [Flags]
    public enum HumanAggressiveFlags : int
    {
        SEE_ENEMY = 0,
        SEE_ENEMY_WITH_WEAPON = 1,
        HEAR_GUNSHOT = 2,
        ON_ATTACK = 4
    }

    [Flags]
    public enum HumanCourageFlags : int
    {
        FUNK = 0,
        NORMAL = 1,
        COURAGOUS = 2,
        BOWLER = 4
    }

    public enum PathObjectTypes : int
    {
        INVALID = 0,
        CLIMB = 1,
        CLIMB_BEVEL = 2,
        CLIMB_BEVEL_WALL = 3,
        CLIMB_CO = 4,
        CLIMB_CAR = 5,
        DOOR = 11,
        E_LIFT = 12,
        LADDER = 13
    }
}
