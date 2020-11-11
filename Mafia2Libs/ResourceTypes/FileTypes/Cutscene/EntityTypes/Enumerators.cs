namespace ResourceTypes.Cutscene.AnimEntities
{
    public enum AnimEntityTypes
    {
        AnimEntity = -1,
        AeOmniLight = 2, // AeOmniLight
        AeSpotLight = 3, // AeSpotLight
        AeCamera = 4, // AeCamera
        AeTargetCamera = 5, // AeTargetCamera
        AeModel = 6, // AeModel
        AeUnk7 = 7,
        AeSoundPoint = 8, // AeSoundPoint
        AeScript = 10, // AeScript
        AeSubtitles = 12, // AeSubtitles
        AeParticles = 13, // AeParticles
        AeVehicle = 14, // AeVehicle
        AeCutEdit = 18, // AeCutEdit
        AeFrame = 22, // AeFrame
        AeHumanFx = 23, // AeHumanFx
        AeEffects = 25, // AeEffects
        AeSoundSphereAmbient = 28, // AeSoundSphereAmbient
        AeSunLight = 29, // AeSunLight - Note also could be 39.
        AeSoundListener = 30, // AeSoundListener.
        AeUnk32 = 32,
        AeSoundEntity = 31, // AeSoundEntity.
        AeSound_Type33 = 33
    }

    // Mafia III and Mafia I: DE. Could be in Mafia II also.
    // AeDummy = 1.
    // AeSoundCode = 9.
    // AeMultimedia = 11.
    // AeSoundGroup = 19.
    // AeDirectLight = 34.
    // AeWwise = 35.
    // AeMovie = 36.
    // AeDebugOverlay = 37.
    // AeCutscene = 38.
    // AeLightGroup = 40.
    // AeFog = 41.
    // AeMaterial = 42.

    // Looks different to me; could be SPD?
    // AeSoundAmbient::C_SoundObjectAmbient = 0.
    // AeSoundPoint::C_SoundObjectPoint = 1.
    // AeSoundCone::C_SoundObjectCone = 2.
    // AeSoundSphereAmbient::C_SoundObjectSphereAmbient = 3.
}
