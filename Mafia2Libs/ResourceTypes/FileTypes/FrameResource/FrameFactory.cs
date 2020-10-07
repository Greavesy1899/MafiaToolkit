using System.Diagnostics;
using System.IO;

namespace ResourceTypes.FrameResource
{
    public class FrameFactory
    {
        public static FrameObjectBase ReadFrameByObjectID(MemoryStream stream, ObjectType FrameType, bool isBigEndian)
        {
            switch(FrameType)
            {
                case ObjectType.Joint:
                    FrameObjectJoint joint = new FrameObjectJoint();
                    joint.ReadFromFile(stream, isBigEndian);
                    return joint;
                case ObjectType.SingleMesh:
                    FrameObjectSingleMesh mesh = new FrameObjectSingleMesh();
                    mesh.ReadFromFile(stream, isBigEndian);
                    return mesh;
                case ObjectType.Frame:
                    FrameObjectFrame frame = new FrameObjectFrame();
                    frame.ReadFromFile(stream, isBigEndian);
                    return frame;
                case ObjectType.Light:
                    FrameObjectLight light = new FrameObjectLight();
                    light.ReadFromFile(stream, isBigEndian);
                    return light;
                case ObjectType.Camera:
                    FrameObjectCamera camera = new FrameObjectCamera();
                    camera.ReadFromFile(stream, isBigEndian);
                    return camera;
                case ObjectType.Component_U00000005:
                    FrameObjectComponent_U005 u005 = new FrameObjectComponent_U005();
                    u005.ReadFromFile(stream, isBigEndian);
                    return u005;
                case ObjectType.Sector:
                    FrameObjectSector sector = new FrameObjectSector();
                    sector.ReadFromFile(stream, isBigEndian);
                    return sector;
                case ObjectType.Dummy:
                    FrameObjectDummy dummy = new FrameObjectDummy();
                    dummy.ReadFromFile(stream, isBigEndian);
                    return dummy;
                case ObjectType.ParticleDeflector:
                    FrameObjectDeflector deflector = new FrameObjectDeflector();
                    deflector.ReadFromFile(stream, isBigEndian);
                    return deflector;
                case ObjectType.Area:
                    FrameObjectArea area = new FrameObjectArea();
                    area.ReadFromFile(stream, isBigEndian);
                    return area;
                case ObjectType.Target:
                    FrameObjectTarget target = new FrameObjectTarget();
                    target.ReadFromFile(stream, isBigEndian);
                    return target;
                case ObjectType.Model:
                    FrameObjectModel model = new FrameObjectModel();
                    model.ReadFromFile(stream, isBigEndian);
                    return model;
                case ObjectType.Collision:
                    FrameObjectCollision collision = new FrameObjectCollision();
                    collision.ReadFromFile(stream, isBigEndian);
                    return collision;
                default:
                    Debug.WriteLine("Missing frame type!");
                    return null;
            }
        }
    }
}
