using ResourceTypes.ModelHelpers.ModelExporter;
using System.Diagnostics;
using System.IO;

namespace ResourceTypes.FrameResource
{
    public class FrameFactory
    {
        public static FrameObjectBase ReadFrameByObjectID(MemoryStream stream, bool isBigEndian, FrameResource OwningResource, FrameResourceObjectType FrameType)
        {
            FrameObjectBase NewFrameObject = ConstructFrameByObjectID(OwningResource, FrameType);
            NewFrameObject.ReadFromFile(stream, isBigEndian);
            return NewFrameObject;
        }

        public static FrameObjectBase ConstructFrameByObjectID(FrameResource OwningResource, FrameResourceObjectType FrameType)
        {
            switch (FrameType)
            {
                case FrameResourceObjectType.Point:
                    return OwningResource.ConstructFrameAssetOfType<FrameObjectPoint>();
                case FrameResourceObjectType.SingleMesh:
                    return OwningResource.ConstructFrameAssetOfType<FrameObjectSingleMesh>();
                case FrameResourceObjectType.Frame:
                    return OwningResource.ConstructFrameAssetOfType<FrameObjectFrame>();
                case FrameResourceObjectType.Light:
                    return OwningResource.ConstructFrameAssetOfType<FrameObjectLight>();
                case FrameResourceObjectType.Camera:
                    return OwningResource.ConstructFrameAssetOfType<FrameObjectCamera>();
                case FrameResourceObjectType.Component_U00000005:
                    return OwningResource.ConstructFrameAssetOfType<FrameObjectComponent_U005>();
                case FrameResourceObjectType.Sector:
                    return OwningResource.ConstructFrameAssetOfType<FrameObjectSector>();
                case FrameResourceObjectType.Dummy:
                    return OwningResource.ConstructFrameAssetOfType<FrameObjectDummy>();
                case FrameResourceObjectType.ParticleDeflector:
                    return OwningResource.ConstructFrameAssetOfType<FrameObjectDeflector>();
                case FrameResourceObjectType.Area:
                    return OwningResource.ConstructFrameAssetOfType<FrameObjectArea>();
                case FrameResourceObjectType.Target:
                    return OwningResource.ConstructFrameAssetOfType<FrameObjectTarget>();
                case FrameResourceObjectType.Model:
                    return OwningResource.ConstructFrameAssetOfType<FrameObjectModel>();
                case FrameResourceObjectType.Collision:
                    return OwningResource.ConstructFrameAssetOfType<FrameObjectCollision>();
                default:
                    Debug.WriteLine("Missing frame type!");
                    return null;
            }
        }

        public static FrameObjectBase ConstructFrameByObjectType(MT_ObjectType ObjectType, FrameResource OwningResource)
        {
            switch (ObjectType)
            {
                case MT_ObjectType.Point:
                    return OwningResource.ConstructFrameAssetOfType<FrameObjectPoint>();
                case MT_ObjectType.StaticMesh:
                    return OwningResource.ConstructFrameAssetOfType<FrameObjectSingleMesh>();
                case MT_ObjectType.Actor:
                    return OwningResource.ConstructFrameAssetOfType<FrameObjectFrame>();
                //case MT_ObjectType.Light:
                //    return OwningResource.ConstructFrameAssetOfType<FrameObjectLight>();
                //case MT_ObjectType.Camera:
                //    return OwningResource.ConstructFrameAssetOfType<FrameObjectCamera>();
                //case MT_ObjectType.Component_U00000005:
                //    return OwningResource.ConstructFrameAssetOfType<FrameObjectComponent_U005>();
                //case MT_ObjectType.Sector:
                //    return OwningResource.ConstructFrameAssetOfType<FrameObjectSector>();
                case MT_ObjectType.Dummy:
                    return OwningResource.ConstructFrameAssetOfType<FrameObjectDummy>();
                //case MT_ObjectType.ParticleDeflector:
                //    return OwningResource.ConstructFrameAssetOfType<FrameObjectDeflector>();
                //case MT_ObjectType.Area:
                //    return OwningResource.ConstructFrameAssetOfType<FrameObjectArea>();
                //case MT_ObjectType.Target:
                //    return OwningResource.ConstructFrameAssetOfType<FrameObjectTarget>();
                case MT_ObjectType.RiggedMesh:
                    return OwningResource.ConstructFrameAssetOfType<FrameObjectModel>();
                case MT_ObjectType.ItemDesc:
                    return OwningResource.ConstructFrameAssetOfType<FrameObjectCollision>();
                default:
                    return null;
            }

            // TODO: Log when failed
        }
    }
}
