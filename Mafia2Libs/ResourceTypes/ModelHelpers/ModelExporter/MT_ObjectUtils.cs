using ResourceTypes.FrameResource;
using System;
using System.Diagnostics;

namespace ResourceTypes.ModelHelpers.ModelExporter
{
    public static class MT_ObjectUtils
    {
        public static MT_ObjectType GetTypeFromFrame(FrameObjectBase FrameObject)
        {
            // TODO: I hate huge if statements, but it has to be done here
            Type FrameType = FrameObject.GetType();
            if(FrameType == typeof(FrameObjectSingleMesh))
            {
                return MT_ObjectType.StaticMesh;
            }
            else if(FrameType == typeof(FrameObjectModel))
            {
                return MT_ObjectType.RiggedMesh;
            }
            else if(FrameType == typeof(FrameObjectJoint))
            {
                return MT_ObjectType.Joint;
            }
            else if(FrameType == typeof(FrameObjectDummy))
            {
                return MT_ObjectType.Dummy;
            }
            else if(FrameType == typeof(FrameObjectFrame))
            {
                return MT_ObjectType.Actor;
            }
            else if(FrameType == typeof(FrameObjectCollision))
            {
                return MT_ObjectType.ItemDesc;
            }
            else
            {
                Debug.Assert(false, "No support for frame type " + FrameType.Name);
                return MT_ObjectType.Null;
            }
        }
    }
}
