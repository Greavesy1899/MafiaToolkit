using System;

namespace ResourceTypes.M3.XBin
{

    public enum ERadioStation
    {
        E_RADIO_1,
        E_RADIO_2,
        E_RADIO_3,
        E_RADIO_POLICE,
        E_RADIO_RANDOM,
        E_RADIO_LAST,
    }

    public enum EVehicleRaceClass
    {
        E_VRC_NONE,
        E_VRC_EXOTIC,
        E_VRC_SPORT,
        E_VRC_STREET,
        E_VRC_STANDARD,
        E_VRC_UTILITY
    }

    public enum EBodyPartType
    {
        E_BPT_HEAD = 0,
        E_BPT_BODY = 1,
        E_BPT_LOWERBODY = 2,
        E_BPT_LEFTHAND = 3,
        E_BPT_RIGHTHAND = 4,
        E_BPT_LEFTLEG = 5,
        E_BPT_RIGHTLEG = 6
    }

    [Flags]
    public enum ECarMtrStuffFlags
    {
        E_CMSF_RIM_SPARKS = 1
    }

    [Flags]
    public enum EHumanMaterialsTableItemFlags
    {
        E_HMTIF_BUSH = 0x2,
        E_HMTIF_SIDEWALK = 0x8000,
        E_HMTIF_ROAD = 0x40000
    }
}
