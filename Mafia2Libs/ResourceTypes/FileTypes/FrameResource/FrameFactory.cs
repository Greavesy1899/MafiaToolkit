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
                case FrameResourceObjectType.Joint:
                    FrameObjectJoint joint = OwningResource.ConstructFrameAssetOfType<FrameObjectJoint>();
                    return joint;
                case FrameResourceObjectType.SingleMesh:
                    FrameObjectSingleMesh mesh = OwningResource.ConstructFrameAssetOfType<FrameObjectSingleMesh>();
                    return mesh;
                case FrameResourceObjectType.Frame:
                    FrameObjectFrame frame = OwningResource.ConstructFrameAssetOfType<FrameObjectFrame>();
                    return frame;
                case FrameResourceObjectType.Light:
                    FrameObjectLight light = OwningResource.ConstructFrameAssetOfType<FrameObjectLight>();
                    return light;
                case FrameResourceObjectType.Camera:
                    FrameObjectCamera camera = OwningResource.ConstructFrameAssetOfType<FrameObjectCamera>();
                    return camera;
                case FrameResourceObjectType.Component_U00000005:
                    FrameObjectComponent_U005 u005 = OwningResource.ConstructFrameAssetOfType<FrameObjectComponent_U005>();
                    return u005;
                case FrameResourceObjectType.Sector:
                    FrameObjectSector sector = OwningResource.ConstructFrameAssetOfType<FrameObjectSector>();
                    return sector;
                case FrameResourceObjectType.Dummy:
                    FrameObjectDummy dummy = OwningResource.ConstructFrameAssetOfType<FrameObjectDummy>();
                    return dummy;
                case FrameResourceObjectType.ParticleDeflector:
                    FrameObjectDeflector deflector = OwningResource.ConstructFrameAssetOfType<FrameObjectDeflector>();
                    return deflector;
                case FrameResourceObjectType.Area:
                    FrameObjectArea area = OwningResource.ConstructFrameAssetOfType<FrameObjectArea>();
                    return area;
                case FrameResourceObjectType.Target:
                    FrameObjectTarget target = OwningResource.ConstructFrameAssetOfType<FrameObjectTarget>();
                    return target;
                case FrameResourceObjectType.Model:
                    FrameObjectModel model = OwningResource.ConstructFrameAssetOfType<FrameObjectModel>();
                    return model;
                case FrameResourceObjectType.Collision:
                    FrameObjectCollision collision = OwningResource.ConstructFrameAssetOfType<FrameObjectCollision>();
                    return collision;
                default:
                    Debug.WriteLine("Missing frame type!");
                    return null;
            }
        }
    }
}
