namespace ResourceTypes.M3.XBin
{
    public enum ERespawnPlaceType
    {
        E_RESPAWN_PLACE_HOSPITAL,
        E_RESPAWN_PLACE_POLICE,
        E_RESPAWN_PLACE_HOME
    }

    public enum EGameObjectiveMethodOfSetting
    {
        E_GOMS_POSITION,
        E_GOMS_ENTITY,
        E_GOMS_SHOP,
        E_GOMS_PHONE
    }
	
    public enum EMissionType
    {
        E_MT_STORY,
        E_MT_BUSINESS,
        E_MT_CLUE
    }
	
    public enum EGameQuestLoadType
    {
        E_LOAD_POSITION = 0,
        E_LOAD_INSTANT = 2
    }
	
    public enum EQuestUIType
    {
        E_QUIT_STORY,
        E_QUIT_SIDE,
        E_QUIT_BUSINESS,
        E_QUIT_ACTIVITY,
        E_QUIT_CHALLENGE,
        E_QUIT_INVISIBLE
    }
	
	public enum EQuestMarkerType
    {
        E_QMT_NONE,
        E_QMT_ICON,
        E_QMT_CIRCLE,
        E_QMT_AREA
    }
}
