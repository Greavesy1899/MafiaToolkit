using System.Diagnostics;
using System.IO;

namespace ResourceTypes.Cutscene.AnimEntities
{
    public static class CutsceneEntityFactory
    {
        // Finds the correct type of AnimEntity and returns the data.
        // TODO: Ideally move this to our friend MemoryStream.
        public static AnimEntity ReadAnimEntityFromFile(AnimEntityTypes EntityType, MemoryStream Reader)
        {
            // Construct our AnimEntity
            AnimEntity Entity = ConstructAnimEntity(EntityType);
            bool isBigEndian = false;

            // Make sure our entity is not valid, and read our data
            if(Entity != null)
            {
                Entity.Definition.ReadFromFile(Reader, isBigEndian);
            }
            
            Debug.Assert(Entity != null, "Did not find a AnimEntityType. Maybe the Toolkit does not support it?");
            Debug.Assert(Reader.Position == Reader.Length, "When reading the AnimEntity Definition, we did not reach the end of stream!");
            return Entity;
        }

        public static AnimEntity ConstructAnimEntity(AnimEntityTypes EntityType)
        {
            AnimEntity Entity = new AnimEntity();

            switch (EntityType)
            {
                case AnimEntityTypes.AeOmniLight:
                    Entity.Definition = new AeOmniLight();
                    Entity.Data = new AeOmniLightData();
                    break;
                case AnimEntityTypes.AeSpotLight:
                    Entity.Definition = new AeSpotLight();
                    Entity.Data = new AeBaseData();
                    break;
                case AnimEntityTypes.AeUnk4:
                    Entity.Definition = new AeUnk4();
                    Entity.Data = new AeBaseData();
                    break;
                case AnimEntityTypes.AeTargetCamera:
                    Entity.Definition = new AeTargetCamera();
                    Entity.Data = new AeTargetCameraData();
                    break;
                case AnimEntityTypes.AeModel:
                    Entity.Definition = new AeModel();
                    Entity.Data = new AeModelData();
                    break;
                case AnimEntityTypes.AeUnk7:
                    Entity.Definition = new AeUnk7();
                    Entity.Data = new AeBaseData();
                    break;
                case AnimEntityTypes.AeSound_Type8:
                    Entity.Definition = new AeSound_Type8();
                    Entity.Data = new AeBaseData();
                    break;
                case AnimEntityTypes.AeUnk10:
                    Entity.Definition = new AeUnk10();
                    Entity.Data = new AeUnk10Data();
                    break;
                case AnimEntityTypes.AeUnk12:
                    Entity.Definition = new AeUnk12();
                    Entity.Data = new AeBaseData();
                    break;
                case AnimEntityTypes.AeUnk13:
                    Entity.Definition = new AeUnk13();
                    Entity.Data = new AeUnk13Data();
                    break;
                case AnimEntityTypes.AeUnk18:
                    Entity.Definition = new AeUnk18();
                    Entity.Data = new AeUnk18Data();
                    break;
                case AnimEntityTypes.AeVehicle:
                    Entity.Definition = new AeVehicle();
                    Entity.Data = new AeBaseData();
                    break;
                case AnimEntityTypes.AeFrame:
                    Entity.Definition = new AeFrame();
                    Entity.Data = new AeFrameData();
                    break;
                case AnimEntityTypes.AeUnk23:
                    Entity.Definition = new AeUnk23();
                    Entity.Data = new AeUnk23Data();
                    break;
                case AnimEntityTypes.AeEffects:
                    Entity.Definition = new AeEffects();
                    Entity.Data = new AeEffectsBase();
                    break;
                case AnimEntityTypes.AeSound_Type28:
                    Entity.Definition = new AeSound_Type28();
                    Entity.Data = new AeBaseData();
                    break;
                case AnimEntityTypes.AeSunLight:
                    Entity.Definition = new AeSunLight();
                    Entity.Data = new AeBaseData();
                    break;
                case AnimEntityTypes.AeUnk30:
                    Entity.Definition = new AeUnk30();
                    Entity.Data = new AeBaseData();
                    break;
                case AnimEntityTypes.AeUnk32:
                    Entity.Definition = new AeUnk32();
                    Entity.Data = new AeBaseData();
                    break;
                case AnimEntityTypes.AeSound_Type33:
                    Entity.Definition = new AeSound_Type33();
                    Entity.Data = new AeBaseData();
                    break;
            }

            return Entity;
        }
    }
}
