using System.IO;
using Utils.StringHelpers;
using Utils.VorticeUtils;
using System.Numerics;
using SharpGLTF.Scenes;
using System.Collections.Generic;

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

        public void ReadFromFile(BinaryReader reader)
        {
            Name = StringHelpers.ReadString8(reader);
            UsageFlags = reader.ReadUInt32();
            Position = Vector3Utils.ReadFromFile(reader);
            Rotation = QuaternionExtensions.ReadFromFile(reader);
            Scale = Vector3Utils.ReadFromFile(reader);
            ParentJointIndex = reader.ReadInt32();
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.WriteString8(Name);
            writer.Write(UsageFlags);
            Position.WriteToFile(writer);
            Rotation.WriteToFile(writer);
            Scale.WriteToFile(writer);
            writer.Write(ParentJointIndex);
        }

    }
    public class MT_Skeleton
    {
        public MT_Joint[] Joints { get; set; }

        public bool ReadFromFile(BinaryReader reader)
        {
            uint NumJoints = reader.ReadUInt32();
            Joints = new MT_Joint[NumJoints];

            for(int i = 0; i < NumJoints; i++)
            {
                MT_Joint JointObject = new MT_Joint();
                JointObject.ReadFromFile(reader);
                Joints[i] = JointObject;
            }

            return true;
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(Joints.Length);
            
            foreach(var JointObject in Joints)
            {
                JointObject.WriteToFile(writer);
            }
        }

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
