using System.IO;
using Utils.StringHelpers;
using Utils.VorticeUtils;
using System.Numerics;
using SharpGLTF.Scenes;
using System.Collections.Generic;
using SharpGLTF.Animations;

namespace ResourceTypes.ModelHelpers.ModelExporter
{
    public class MT_Joint
    {
        public string Name { get; set; }
        public uint UsageFlags { get; set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Scale { get; set; }
        public int ParentJointIndex { get; set; }

    }
    public class MT_Skeleton
    {
        public MT_Joint[] Joints { get; set; }
        public NodeBuilder[] BuildGLTF(int LodIndex)
        {
            NodeBuilder[] JointNodes = new NodeBuilder[Joints.Length];
            for(uint Index = 0; Index < Joints.Length; Index++)
            {
                MT_Joint CurrentJoint = Joints[Index];

                // create node with transform
                NodeBuilder JointNode = new NodeBuilder(CurrentJoint.Name)
                    .WithLocalTranslation(CurrentJoint.Position)
                    .WithLocalRotation(CurrentJoint.Rotation)
                    .WithLocalScale(CurrentJoint.Scale);

                // Add to the parent joint node
                if (CurrentJoint.ParentJointIndex != 255)
                {
                    string ParentJointName = Joints[CurrentJoint.ParentJointIndex].Name;
                    JointNodes[CurrentJoint.ParentJointIndex].AddNode(JointNode);
                }

                // then to lookup
                JointNodes[Index] = JointNode;
            }

            return JointNodes;
        }
    }
}
