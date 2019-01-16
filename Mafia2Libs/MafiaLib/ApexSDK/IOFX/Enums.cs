namespace ApexSDK
{
    public enum ModifierType
    {
        ModifierType_Invalid = 0,
        ModifierType_Rotation = 1,
        ModifierType_SimpleScale = 2,
        ModifierType_RandomScale = 3,
        ModifierType_ColorVsLife = 4,
        ModifierType_ColorVsDensity = 5,
        ModifierType_SubtextureVsLife = 6,
        ModifierType_OrientAlongVelocity = 7,
        ModifierType_ScaleAlongVelocity = 8,
        ModifierType_RandomSubtexture = 9,
        ModifierType_RandomRotation = 10,
        ModifierType_ScaleVsLife = 11,
        ModifierType_ScaleVsDensity = 12,
        ModifierType_ScaleVsCameraDistance = 13,
        ModifierType_ViewDirectionSorting = 14,
        ModifierType_RotationRate = 15,
        ModifierType_RotationRateVsLife = 16,
        ModifierType_OrientScaleAlongScreenVelocity = 17,
        ModifierType_ScaleByMass = 18,
        ModifierType_ColorVsVelocity = 19,
        ModifierType_Count = 20
    }

    public enum ApexMeshParticleRollType
    {
        SPHERICAL = 0,
        CUBIC,
        FLAT_X,
        FLAT_Y,
        FLAT_Z,
        LONG_X,
        LONG_Y,
        LONG_Z,
        SPRITE,
        COUNT
    }

    public enum ColorChannel
    {
        red,
        green,
        blue,
        alpha
    }

    public enum ScaleAxis
    {
        xAxis,
        yAxis,
        zAxis
    }
}
