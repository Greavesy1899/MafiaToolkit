using System.IO;
using Utils.Logging;

namespace ResourceTypes.Cutscene.AnimEntities
{
    public static class CutsceneEntityFactory
    {
        // Finds the correct type of AnimEntity and returns the data.
        // TODO: Ideally move this to our friend MemoryStream.
        public static AnimEntityWrapper ReadAnimEntityWrapperFromFile(AnimEntityTypes EntityType, MemoryStream Reader)
        {
            // Construct our AnimEntity
            AnimEntityWrapper Entity = ConstructAnimEntityWrapper(EntityType);
            bool isBigEndian = false;

            // Make sure our entity is not valid, and read our data
            if(Entity != null)
            {
                Entity.ReadFromFile(Reader, isBigEndian);
            }
            
            ToolkitAssert.Ensure(Entity != null, "Did not find a AnimEntityType. Maybe the Toolkit does not support it?");
            ToolkitAssert.Ensure(Reader.Position == Reader.Length, "When reading the AnimEntity Definition, we did not reach the end of stream!");
            return Entity;
        }

        public static void WriteAnimEntityToFile(MemoryStream Writer, AnimEntityWrapper EntityWrapper)
        {
            ToolkitAssert.Ensure(EntityWrapper != null, "The passed Entity was not valid. Maybe the Toolkit does not support it?");

            bool isBigEndian = false;
            EntityWrapper.WriteToFile(Writer, isBigEndian);
        }

        public static AnimEntityWrapper ConstructAnimEntityWrapper(AnimEntityTypes EntityType)
        {
            AnimEntityWrapper Entity = new AnimEntityWrapper();
            
            switch (EntityType)
            {
                case AnimEntityTypes.AeOmniLight:
                    Entity = new AeOmniLightWrapper();
                    break;
                case AnimEntityTypes.AeSpotLight:
                    Entity = new AeSpotLightWrapper();
                    break;
                case AnimEntityTypes.AeCamera:
                    Entity = new AeCameraWrapper();
                    break;
                case AnimEntityTypes.AeTargetCamera:
                    Entity = new AeTargetCameraWrapper();
                    break;
                case AnimEntityTypes.AeModel:
                    Entity = new AeModelWrapper();
                    break;
                case AnimEntityTypes.AeUnk7:
                    Entity = new AeUnk7Wrapper();
                    break;
                case AnimEntityTypes.AeSoundPoint:
                    Entity = new AeSoundPointWrapper();
                    break;
                case AnimEntityTypes.AeScript:
                    Entity = new AeScriptWrapper();
                    break;
                case AnimEntityTypes.AeSubtitles:
                    Entity = new AeSubtitlesWrapper();
                    break;
                case AnimEntityTypes.AeParticles:
                    Entity = new AeParticlesWrapper();
                    break;
                case AnimEntityTypes.AeCutEdit:
                    Entity = new AeCutEditWrapper();
                    break;
                case AnimEntityTypes.AeVehicle:
                    Entity = new AeVehicleWrapper();
                    break;
                case AnimEntityTypes.AeFrame:
                    Entity = new AeFrameWrapper();
                    break;
                case AnimEntityTypes.AeHumanFx:
                    Entity = new AeHumanFxWrapper();
                    break;
                case AnimEntityTypes.AeEffects:
                    Entity = new AeEffectsWrapper();
                    break;
                case AnimEntityTypes.AeSoundSphereAmbient:
                    Entity = new AeSoundSphereAmbientWrapper();
                    break;
                case AnimEntityTypes.AeSunLight:
                    Entity = new AeSunLightWrapper();
                    break;
                case AnimEntityTypes.AeSoundListener:
                    Entity = new AeSoundListenerWrapper();
                    break;
                case AnimEntityTypes.AeSoundEntity:
                    Entity = new AeSoundEntityWrapper();
                    break;
                case AnimEntityTypes.AeUnk32:
                    Entity = new AeUnk32Wrapper();
                    break;
                case AnimEntityTypes.AeSound_Type33:
                    Entity = new AeSound_Type33Wrapper();
                    break;
            }

            return Entity;
        }
    }
}
