using System;

namespace ResourceTypes.Navigation
{
    [Flags]
    public enum RoadFlags
    {
        None = 0,
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
        BackwardDirection = 4096,
    }

    [Flags]
    public enum LaneTypes
    {
        None = 0,
        BackRoad = 1,
        ExcludeImpassible = 2,
        Parking = 4,
        flag_8 = 8,
        BusAndTrucks = 16,
        MainRoad = 32,
        IsHighway = 64,
        flag_128 = 128
    }
}
