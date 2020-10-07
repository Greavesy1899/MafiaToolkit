using System;

namespace ResourceTypes.M3.XBin
{
    public enum EStreamMapLineType
    {
        E_LINE_GAME,
        E_LINE_MISSION,
        E_LINE_PART,
    }

    public enum ESaveType
    {
        E_INVALID = 0,
        E_CHECKPOINT = 1,
        E_TEST_SCENE = 3,
        E_DEBUG_CHECKPOINT = 4,
        E_FREEROAM = 998,
        E_CHAPTER = 999
    }

    public enum ESlotType
    {
        E_UNKNOWN = 0,
        E_CITY_PART,
        E_CITY_GROUND,
        E_CITY_UNIVERSAL,
        E_CITY_SHOP,
        E_CHAR_UNIVERS,
        E_CHAR_PLAYER,
        E_CHAR_HI,
        E_CHAR_LO,
        E_CHAR_POLICE,
        E_CAR_UNIVERS,
        E_CAR_SMALL,
        E_CAR_BIG,
        E_CAR_POLICE,
        E_CAR_SCRIPT,
        E_BASE_ANIM,
        E_WEAPONS,
        E_GUI,
        E_SKY,
        E_TABLES,
        E_SOUND_DEFAULT,
        E_PARTICLES,
        E_MUSIC,
        E_GAME_SCRIPT,
        E_MISSION_SCRIPT,
        E_SCRIPT,
        E_HI_CHAR_UNITED,
        E_LO_CHAR_UNITED,
        E_CAR_UNITED,
        E_POLICE_UNITED,
        E_TREES = 30,
        E_CITY_CRASH = 31,
        E_GENERATE = 32,
        E_SMALL = 33,
        E_SCRIPT_SOUND = 34,
        E_LUA = 35,
        E_MAPA = 36,
        E_SOUND_CITY = 37,
        E_APDATA = 38,
        E_KYN_CITY_PART = 39,
        E_KYN_CITY_SHOP = 40,
        E_GENSPEECH_NORMAL = 41,
        E_GENSPEECH_GANG = 42,
        E_GENSPEECH_VARIOUS = 43,
        E_GENSPEECH_STORY = 44,
        E_SCRIPT_UNITED = 45,
        E_MISSION_SCRIPT_UNITED = 46,
        E_GENSPEECH_POLICE = 47,
        E_TEXT_DATABASE = 48,
        E_WARDROBE = 49,
        E_INGAME = 50,
        E_INGAME_GUI = 51,
        E_DABING = 52,
        E_CAR_PAINTING = 53,
        E_APDATA_SMALL = 54,
        E_SANDBOX = 55,
        E_ROADMAP = 56,
        E_TEXT_DATABASE_CHAPTER = 57,
        E_GUI2 = 66,
        E_TEMP_SUBTITLES = 67,
        E_PHONE = 68,
        E_GUI_IMAGES = 69,
        E_PREGAME_GUI = 71,
        E_SHARED_GUI = 72,
        E_FONTS_EN_PC_GUI = 73,
        E_FONTS_EN_PS4_GUI = 74,
        E_FONTS_EN_XB1_GUI = 75,
        E_FONTS_CHS_GUI = 76,
        E_FONTS_CHT_GUI = 77,
        E_FONTS_JP_GUI = 78,
        E_FONTS_KR_GUI = 79,
        E_SLOT_TYPE_LAST = 80
    }

    public enum ECommandIfOperator
    {
        E_SMCIO_EQUAL,
        E_SMCIO_NOTEQUAL,
        E_SMCIO_LESSER,
        E_SMCIO_LESSEROREQUAL,
        E_SMCIO_GREATER,
        E_SMCIO_GREATEROREQUAL
    }

    [Flags]
    public enum EVehicleLoadFlags
    {
        E_SMVLF_ACTIVATE = 1,
        E_SMVLF_NOTUSED = 2,
    }

    [Flags]
    public enum ESDSLoadFlags
    {
        E_SDSLF_REPORT_OBJECTS = 1,
        E_SDSLF_RETRY_PERSISTENT = 2,
    }

}
