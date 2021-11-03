using System;

namespace ResourceTypes.M3.XBin
{
    public enum EOffenceType
    {
        E_OT_SIDEWALK_DRIVING = 0,
        E_OT_KNOCKING_CRASH_OBJECT = 1,
        E_OT_CARRYING_WEAPON = 2,
        E_OT_HITTING_VEHICLE = 3,
        E_OT_FISTFIGHT = 4,
        E_OT_BREAK_SHOP_WINDOW = 5,
        E_OT_BREAK_CAR_WINDOW = 6,
        E_OT_STREET_SHOOTING = 7,
        E_OT_NONFATAL_CIVIL_HIT_BY_VEHICLE = 8,
        E_OT_SHOOTING_AT_VEHICLE = 9,
        E_OT_HIT_DISCIPLINARY_CAR_BY_CAR = 10,
        E_OT_THROWING_DRIVER = 11,
        E_OT_STAY_IN_ROBBED_SHOP = 12,
        E_OT_STAY_IN_SHOP_BACKROOM = 13,
        E_OT_SHOP_ALARM = 14,
        E_OT_AIMING_AT_CIVIL = 15,
        E_OT_AIMING_AT_DISCIPLINARY = 16,
        E_OT_NONFATAL_DISCIPLINARY_HIT_BY_VEHICLE = 17,
        E_OT_CIVIL_KILL_ATTACK = 18,
        E_OT_DISCIPLINARY_KILL = 19,
        E_OT_SHOOT_AT_DISCIPLINARY_VEHICLE = 20,
        E_OT_VEHICLE_EXPLODE = 21,
        E_OT_DISCIPLINARY_CAR_THEFT = 22,
        E_OT_SHOOT_AT_DISCIPLINARY = 23,
        E_OT_SHOOT_AT_CIVIL = 24,
        E_OT_SHOOT_AT_ENEMY = 25,
        E_OT_ENEMY_KILL = 26,
        E_OT_THROWING_POLICEMAN = 27,
        E_OT_OFFENCE_TEST = 28,
        E_OT_CIVIL_KILL_BYVEHICLE = 29,
        E_OT_WEAPON_EXPLODE = 30,
        E_OT_CAR_CRASH_NOT_OCCUPANT = 31,
        E_OT_TAKEDOWN = 32,
        E_OT_INTERROGATION = 33,
        E_OT_VEHICLE_FLIPPED = 34,
        E_OT_RUNNING = 35,
        E_OT_BUMPING_TO_PEOPLE = 36,
        E_OT_VEHICLE_CRASH_OTHER = 37,
        E_OT_RED_LIGHT = 38,
        E_OT_RECKLESS_DRIVING = 39,
        E_OT_SPEEDING = 40,
        E_OT_PLACING_EXPLOSIVE = 41,
        E_OT_BUMPING_TO_POLICE = 42,
        E_OT_NON_LETHAL_TAKEDOWN = 43,
        E_OT_CARRYING_BODY = 44,
        E_OT_LAST = 45,
    }

    [Flags]
    public enum EPoliceCarShootFlags
    {
        E_PCSF_WHEEL = 1,
        E_PCSF_MOTOR = 2,
        E_PCSF_OFFENDER = 4,
        E_PCSF_NOTHING = 8
    }

    [Flags]
    public enum ECarHuntRoleFlags
    {
        E_CHRF_PIT_LEFT = 1,
        E_CHRF_PIT_RIGHT = 2,
        E_CHRF_POKE = 4,
        E_CHRF_STOP_LEFT = 8,
        E_CHRF_STOP_RIGHT = 0x10,
        E_CHRF_BLOCKAHEAD = 0x20,
        E_CHRF_FOLLOW = 0x40,
        E_CHRF_FRONT = 0x80,
        E_CHRF_FRONTBLOCK = 0x100
    }

    public enum ERoadblockSpawnType
    {
        E_RST_ONE_RIGHT_LANE,
        E_RST_ALL_RIGHT_LANES,
        E_RST_ALL_LANES
    }

    public enum EPoliceGroupType
    {
        E_PGT_PURSUIT,
        E_PGT_FLANK,
        E_PGT_BARRICADE,
        E_PGT_NONE
    }
}
