using System;

namespace ResourceTypes.M3.XBin
{
    [Flags]
    public enum ETrafficCommonFlags_M3 : uint
    {
        // Time Periods
        E_TCF_TIME_PREREVOLUTION = 0x1,
        E_TCF_TIME_REVOLUTION = 0x2,
        E_TCF_TIME_FIFTIES = 0x4,
        E_TCF_TIME_SIXTIES = 0x8,

        // Locations
        E_TCF_LOC_BOURBONCITY = 0x10,
        E_TCF_LOC_PUERTOLIBRE = 0x20,
        E_TCF_LOC_EMPIREBAY = 0x40,
        E_TCF_LOC_SAIGON = 0x80,
    }

    [Flags]
    public enum ETrafficVehicleFlags_M3 : uint
    {
        E_TVF_TRUCK = 0x1,
        E_TVF_DELIVERY = 0x2,
        E_TVF_LUXURY = 0x4,
        E_TVF_REGULAR = 0x8,
        E_TVF_SPORTS = 0x10,
        E_TVF_BUS = 0x20,
        E_TVF_OFFROAD = 0x40,
        E_TVF_SECURITY = 0x80,
        E_TVF_TAXI = 0x100,
        E_TVF_POLICE = 0x200,
        E_TVF_MILITARY = 0x400,
        E_TVF_TRAILER = 0x800,
        E_TVF_SEMITRACTOR = 0x1000,

        //race
        E_TVF_SPEED_SLOW = 0x2000,
        E_TVF_SPEED_MIDDLE = 0x4000,
        E_TVF_SPEED_FAST = 0x8000,
        E_TVF_SPEED_SUPERFAST = 0x10000,

        //size
        E_TVF_SIZE_SMALL = 0x20000,
        E_TVF_SIZE_MIDDLE = 0x40000,
        E_TVF_SIZE_BIG = 0x80000,

        //special
        E_TVF_BOAT = 0x100000,
        E_TVF_CIVILIAN = 0x200000,
        E_TVF_CAR = 0x400000,
        E_TVF_POLICE_STATE = 0x800000,
    }

    [Flags]
    public enum ETrafficVehicleLookFlags_M3 : uint
    {
        E_TVFL_CONVERTIBLE_CLOSED = 0x1,
        E_TVFL_CONVERTIBLE_OPENED = 0x2,
        E_TVFL_HARDTOP = 0x4,
        E_TVFL_ROOFRACK = 0x8,
        E_TVFL_PICKUP = 0x10,
        E_TVFL_STATION = 0x20,
        E_TVFL_VAN = 0x40,
        E_TVFL_BOX = 0x80,
        E_TVFL_FLATBED = 0x100,
        E_TVFL_COVERED = 0x200,
        E_TVFL_SPECIAL = 0x400,
        E_TVFL_MINIVAN = 0x800,
        E_TVFL_STRETCHLIMO = 0x1000,
        E_TVFL_CONCEPT = 0x2000,
        E_TVFL_TUDOR = 0x4000,
        E_TVFL_FORDOR = 0x8000,
        E_TVFL_MUSCLE = 0x10000,
        E_TVFL_CARGO = 0x20000,
        E_TVFL_AIRBOAT = 0x40000,
        E_TVFL_OLD = 0x80000,
        E_TVFL_NEW = 0x100000,
        E_TVFL_SPORTS = 0x200000,
    }
}
