using System;

namespace ResourceTypes.Actors
{
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
        Boat = 47,
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
        Airplane = 104,
        Pinup = 106,
        SpikeStrip = 107,
        FramesController = 110,
        Wardrobe = 112,
        PhysicsScene = 113,
        CleanEntity = 114
    }

    [Flags]
    public enum ActorSoundEntityFlags
    {
        PlayInWinter = 1,
        Loop = 2,
        UseAdvancedScene = 4,
        SectorRestricted = 8,
        PlayInDay = 16,
        PlayInNight = 32,
        PlayInRain = 64,
        PlayInSummer = 128
    }
}
