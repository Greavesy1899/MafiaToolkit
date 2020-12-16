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

    [Flags]
    public enum ECarMtrStuffFlags
    {
        E_CMSF_RIM_SPARKS = 1
    }
}
