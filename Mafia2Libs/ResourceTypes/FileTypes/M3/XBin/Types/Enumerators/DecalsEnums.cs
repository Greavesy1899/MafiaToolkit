namespace ResourceTypes.M3.XBin
{
    public enum EDecalFlags
    {
        DCLF_1Variants = 0x00000001,
        DCLF_2Variants = 0x00000002,
        DCLF_4Variants = 0x00000004,
        DCLF_8Variants = 0x00000008,
        DCLF_RandomRotate = 0x00000010,
        DCLF_ShotDir = 0x00000020,
        DCLF_Penetration = 0x00000040,
        DCLF_AutoUvDir = 0x00000080,
        DCLF_FrontPos = 0x00000100,
        DCLF_Killed = 0x00000200,
        DCLF_ParentSector = 0x00000800,
        DCLF_GenerateNext = 0x00001000,
        DCLF_RandomMirrorX = 0x00002000,
        DCLF_RandomMirrorY = 0x00004000,
        DCLF_Impact = 0x00008000,
        DCLF_OutPenetration = 0x00010000,
        DCLF_NoChildren = 0x00020000,
        DCLF_MirrorX = 0x00040000,
        DCLF_MirrorY = 0x00080000,
        DCLF_UseMatFlags = 0x00100000,
        DCLF_OnlyStaticGeom = 0x00400000,
        DCLF_ExactPos = 0x00800000,
        DCLF_CloserPos = 0x01000000,
        DCLF_KeepMaterial = 0x02000000,
        DCLF_ExcludeCarBones = 0x04000000
    }
	
    public enum EMultiDecalFlags
    {
        E_MD_RANDOM_DIR_RIGHT = 1,
        E_MD_RANDOM_DIR_UP = 2,
        E_MD_ADDITIVE_POSITION = 4
    }
}
