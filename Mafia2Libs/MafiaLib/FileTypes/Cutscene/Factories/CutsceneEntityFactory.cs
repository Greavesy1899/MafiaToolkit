using System.Diagnostics;
using System.IO;

namespace ResourceTypes.Cutscene.AnimEntities
{
    public static class CutsceneEntityFactory
    {
        // Finds the correct type of AnimEntity and returns the data.
        // TODO: Ideally move this to our friend MemoryStream.
        public static AeBase ReadAnimEntityFromFile(AnimEntityTypes EntityType, MemoryStream Reader)
        {
            AeBase AnimEntity = null;
            bool isBigEndian = false;

            switch (EntityType)
            {
                case AnimEntityTypes.AeOmniLight:
                    AnimEntity = new AeOmniLight();
                    AnimEntity.ReadDefinitionFromFile(Reader, isBigEndian);
                    break;
                case AnimEntityTypes.AeSpotLight:
                    AnimEntity = new AeSpotLight();
                    AnimEntity.ReadDefinitionFromFile(Reader, isBigEndian);
                    break;
                case AnimEntityTypes.AeUnk4:
                    AnimEntity = new AeUnk4();
                    AnimEntity.ReadDefinitionFromFile(Reader, isBigEndian);
                    break;
                case AnimEntityTypes.AeTargetCamera:
                    AnimEntity = new AeTargetCamera();
                    AnimEntity.ReadDefinitionFromFile(Reader, isBigEndian);
                    break;
                case AnimEntityTypes.AeModel:
                    AnimEntity = new AeModel();
                    AnimEntity.ReadDefinitionFromFile(Reader, isBigEndian);
                    break;
                case AnimEntityTypes.AeUnk7:
                    AnimEntity = new AeUnk7();
                    AnimEntity.ReadDefinitionFromFile(Reader, isBigEndian);
                    break;
                case AnimEntityTypes.AeSound_Type8:
                    AnimEntity = new AeSound_Type8();
                    AnimEntity.ReadDefinitionFromFile(Reader, isBigEndian);
                    break;
                case AnimEntityTypes.AeUnk10:
                    AnimEntity = new AeUnk10();
                    AnimEntity.ReadDefinitionFromFile(Reader, isBigEndian);
                    break;
                case AnimEntityTypes.AeUnk12:
                    AnimEntity = new AeUnk12();
                    AnimEntity.ReadDefinitionFromFile(Reader, isBigEndian);
                    break;
                case AnimEntityTypes.AeUnk13:
                    AnimEntity = new AeUnk13();
                    AnimEntity.ReadDefinitionFromFile(Reader, isBigEndian);
                    break;
                case AnimEntityTypes.AeUnk18:
                    AnimEntity = new AeUnk18();
                    AnimEntity.ReadDefinitionFromFile(Reader, isBigEndian);
                    break;
                case AnimEntityTypes.AeVehicle:
                    AnimEntity = new AeVehicle();
                    AnimEntity.ReadDefinitionFromFile(Reader, isBigEndian);
                    break;
                case AnimEntityTypes.AeFrame:
                    AnimEntity = new AeFrame();
                    AnimEntity.ReadDefinitionFromFile(Reader, isBigEndian);
                    break;
                case AnimEntityTypes.AeUnk23:
                    AnimEntity = new AeUnk23();
                    AnimEntity.ReadDefinitionFromFile(Reader, isBigEndian);
                    break;
                case AnimEntityTypes.AeEffects:
                    AnimEntity = new AeEffects();
                    AnimEntity.ReadDefinitionFromFile(Reader, isBigEndian);
                    break;
                case AnimEntityTypes.AeSound_Type28:
                    AnimEntity = new AeSound_Type28();
                    AnimEntity.ReadDefinitionFromFile(Reader, isBigEndian);
                    break;
                case AnimEntityTypes.AeSunLight:
                    AnimEntity = new AeSunLight();
                    AnimEntity.ReadDefinitionFromFile(Reader, isBigEndian);
                    break;
                case AnimEntityTypes.AeUnk30:
                    AnimEntity = new AeUnk30();
                    AnimEntity.ReadDefinitionFromFile(Reader, isBigEndian);
                    break;
                case AnimEntityTypes.AeUnk32:
                    AnimEntity = new AeUnk32();
                    AnimEntity.ReadDefinitionFromFile(Reader, isBigEndian);
                    break;
            }

            Debug.Assert(AnimEntity != null, "Did not find a AnimEntityType. Maybe the Toolkit does not support it?");
            Debug.Assert(Reader.Position == Reader.Length, "When reading the AnimEntity Definition, we did not reach the end of stream!");
            return AnimEntity;
        }
    }
}
