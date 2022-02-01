using ResourceTypes.FrameResource;
using System;
using System.Diagnostics;

namespace ResourceTypes.ModelHelpers.ModelExporter
{
    public static class MT_ObjectUtils
    {
        /* TODO: It would be nice to export all Mafia II frame types but seems impossible right now...
         *       So types such as Mesh, joints and dummies are all exported as their representitive types.
         *       The others are just stored as dummies for now.. */
        public static MT_ObjectType GetTypeFromFrame(FrameObjectBase FrameObject)
        {
            // TODO: I hate huge if statements, but it has to be done here
            Type FrameType = FrameObject.GetType();
            if (FrameType == typeof(FrameObjectSingleMesh))
            {
                return MT_ObjectType.StaticMesh;
            }
            else if (FrameType == typeof(FrameObjectModel))
            {
                return MT_ObjectType.RiggedMesh;
            }
            else if (FrameType == typeof(FrameObjectJoint))
            {
                return MT_ObjectType.Joint;
            }
            else if (FrameType == typeof(FrameObjectDummy))
            {
                return MT_ObjectType.Dummy;
            }
            else
            {
                Console.WriteLine("No support for frame type: " + FrameType.Name);
                return MT_ObjectType.Dummy;
            }
        }
    }
}
