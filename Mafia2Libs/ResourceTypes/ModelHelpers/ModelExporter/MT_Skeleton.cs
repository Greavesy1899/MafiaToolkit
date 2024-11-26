using SharpGLTF.Animations;
using SharpGLTF.Scenes;
using SharpGLTF.Schema2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.Json.Nodes;

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

    public class MT_Attachment
    {
        public string Name { get; set; }
        public int JointIndex { get; set; }
    }

    public class MT_Skeleton : IValidator
    {
        public MT_Joint[] Joints { get; set; }
        public MT_Animation[] Animations { get; set; }
        public MT_Attachment[] Attachments { get; set; }

        private const string PROP_OBJECT_ATTACHMENT_NAME = "MT_ATTACHMENT_NAME";
        private const string PROP_OBJECT_JOINT_NAME = "MT_JOINT_NAME";

        public MT_Skeleton()
        {
            Joints = new MT_Joint[0];
            Animations = new MT_Animation[0];
            Attachments = new MT_Attachment[0];
        }

        public NodeBuilder[] BuildGLTF(int LodIndex)
        {
            NodeBuilder[] JointNodes = new NodeBuilder[Joints.Length];
            for (uint Index = 0; Index < Joints.Length; Index++)
            {
                MT_Joint CurrentJoint = Joints[Index];

                // create node with transform
                NodeBuilder JointNode = new NodeBuilder(CurrentJoint.Name)
                    .WithLocalTranslation(CurrentJoint.Position)
                    .WithLocalRotation(CurrentJoint.Rotation)
                    .WithLocalScale(CurrentJoint.Scale);

                // We must include Joint name because modeling suites like blender could
                // badly adjust the name by doing something stupid like adding ".001"
                JointNode.Extras = new JsonObject();
                JointNode.Extras[PROP_OBJECT_JOINT_NAME] = CurrentJoint.Name;

                // Add to the parent joint node
                if (CurrentJoint.ParentJointIndex != 255)
                {
                    string ParentJointName = Joints[CurrentJoint.ParentJointIndex].Name;
                    JointNodes[CurrentJoint.ParentJointIndex].AddNode(JointNode);
                }

                // then to lookup
                JointNodes[Index] = JointNode;
            }

            // add attachments
            foreach(MT_Attachment Attachment in Attachments)
            {
                NodeBuilder AttachmentNode = new NodeBuilder(Attachment.Name);
                AttachmentNode.Extras = new JsonObject();
                AttachmentNode.Extras[PROP_OBJECT_ATTACHMENT_NAME] = Attachment.Name;

                JointNodes[Attachment.JointIndex].AddNode(AttachmentNode);
            }

            // let the skeleton add any animations
            AddAnimations(JointNodes);

            return JointNodes;
        }

        public void BuildSkeletonFromGLTF(Skin InSkin)
        {
            Node SkeletonNode = InSkin.Skeleton;
            List<Node> SkinJoints = InSkin.Joints.ToList();
            IReadOnlyList<Matrix4x4> BindMatrix = InSkin.InverseBindMatrices;

            // we have no idea what attachments we mind find; therefore use a list
            List<MT_Attachment> FoundAttachments = new List<MT_Attachment>();

            Joints = new MT_Joint[SkinJoints.Count];
            for(int i = 0; i < SkinJoints.Count; i++)
            {
                Node CurrentJoint = SkinJoints[i];
                Matrix4x4 CurrentMatrix = BindMatrix[i];

                Vector3 Scale = Vector3.One;
                Quaternion Rotation = Quaternion.Identity;
                Vector3 Position = Vector3.Zero;
                Matrix4x4.Decompose(BindMatrix[i], out Scale, out Rotation, out Position);

                // TODO: pull name from extras to avoid blender anarchy
                MT_Joint NewJoint = new MT_Joint();
                NewJoint.Name = CurrentJoint.Name;
                NewJoint.UsageFlags = 1;
                NewJoint.ParentJointIndex = SkinJoints.IndexOf(CurrentJoint.VisualParent);
                NewJoint.Position = Position;
                NewJoint.Rotation = Rotation;
                NewJoint.Scale = Scale;

                // insert into array
                Joints[i] = NewJoint;

                // check for attachments in the visual children
                foreach(Node PotentialAttachment in CurrentJoint.VisualChildren)
                {
                    if(PotentialAttachment.Extras == null)
                    {
                        continue;
                    }

                    if (PotentialAttachment.Extras[PROP_OBJECT_ATTACHMENT_NAME] == null)
                    {
                        continue;
                    }

                    // We've got an attachment, create object and push into list
                    string AttachmentName = PotentialAttachment.Extras[PROP_OBJECT_ATTACHMENT_NAME].GetValue<string>();

                    MT_Attachment NewAttachment = new MT_Attachment();
                    NewAttachment.Name = AttachmentName;
                    NewAttachment.JointIndex = i;

                    FoundAttachments.Add(NewAttachment);
                }
            }

            Attachments = FoundAttachments.ToArray();
        }

        private void AddAnimations(NodeBuilder[] JointNodes)
        {
            // Dictionary is faster lookups, better then array
            Dictionary<string, NodeBuilder> JointLookup = new Dictionary<string, NodeBuilder>();
            foreach (NodeBuilder JointNode in JointNodes)
            {
                JointLookup.Add(JointNode.Name, JointNode);
            }

            // we can build up the animations here, using the joint nodes provided
            foreach (MT_Animation Animation in Animations)
            {
                foreach (MT_AnimTrack Track in Animation.Tracks)
                {
                    List<(float, Quaternion)> RotationKeyFrames = new List<(float, Quaternion)>();
                    Array.ForEach<MT_RotKey>(Track.RotKeyFrames, (delegate (MT_RotKey Item) { RotationKeyFrames.Add(Item.AsPair()); }));

                    List<(float, Vector3)> PositionKeyFrames = new List<(float, Vector3)>();
                    Array.ForEach<MT_PosKey>(Track.PosKeyFrames, (delegate (MT_PosKey Item) { PositionKeyFrames.Add(Item.AsPair()); }));

                    if (JointLookup.ContainsKey(Track.BoneName))
                    {
                        NodeBuilder Joint = JointLookup[Track.BoneName];
                        Joint.SetRotationTrack(Animation.AnimName, CurveSampler.CreateSampler(RotationKeyFrames.ToArray()));
                        Joint.SetTranslationTrack(Animation.AnimName, CurveSampler.CreateSampler(PositionKeyFrames.ToArray()));
                    }
                }
            }
        }

        protected override bool InternalValidate(MT_ValidationTracker TrackerObject)
        {
            // TODO
            return true;
        }
    }
}
