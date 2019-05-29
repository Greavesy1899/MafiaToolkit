namespace ApexSDK
{
    public enum ModifierType
    {
        Invalid = 0,
        Rotation = 1,
        SimpleScale = 2,
        RandomScale = 3,
        ColorVsLife = 4,
        ColorVsDensity = 5,
        SubtextureVsLife = 6,
        OrientAlongVelocity = 7,
        ScaleAlongVelocity = 8,
        RandomSubtexture = 9,
        RandomRotation = 10,
        ScaleVsLife = 11,
        ScaleVsDensity = 12,
        ScaleVsCameraDistance = 13,
        ViewDirectionSorting = 14,
        RotationRate = 15,
        RotationRateVsLife = 16,
        OrientScaleAlongScreenVelocity = 17,
        ScaleByMass = 18,
        ColorVsVelocity = 19,
        Count = 20
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
