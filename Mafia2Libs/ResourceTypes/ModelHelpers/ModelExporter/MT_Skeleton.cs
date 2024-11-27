using SharpGLTF.Animations;
using SharpGLTF.Scenes;
using SharpGLTF.Schema2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.Json.Nodes;
using Utils.Models;

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

                // decompose to joint
                // TODO: This is inverse - Need to resolve World space / local space / model space?
                Matrix4x4 CurrentMatrix = BindMatrix[i];
                Matrix4x4.Decompose(BindMatrix[i], out Vector3 Scale, out Quaternion Rotation, out Vector3 Position);

                MT_Joint NewJoint = new MT_Joint();
                NewJoint.Name = CurrentJoint.Name;
                NewJoint.UsageFlags = 0; // NB: Usage flags are generated later, when we iterate through the LODs
                NewJoint.ParentJointIndex = SkinJoints.IndexOf(CurrentJoint.VisualParent);
                NewJoint.Position = Position;
                NewJoint.Rotation = Rotation;
                NewJoint.Scale = Scale;

                // If they have opted for explicit name rather than name on node, we'll use that
                if (GLTFDefines.GetValueFromNode(CurrentJoint, PROP_OBJECT_JOINT_NAME, out string ExplicitName))
                {
                    NewJoint.Name = ExplicitName;
                }

                // insert into array
                Joints[i] = NewJoint;

                // check for attachments in the visual children
                foreach(Node PotentialAttachment in CurrentJoint.VisualChildren)
                {
                    // we'll just assume any nodes with our bespoke attachment metadata is for us to use
                    // Users can sanity check this in the import window
                    if (GLTFDefines.GetValueFromNode(PotentialAttachment, PROP_OBJECT_ATTACHMENT_NAME, out string AttachmentName))
                    {
                        MT_Attachment NewAttachment = new MT_Attachment();
                        NewAttachment.Name = AttachmentName;
                        NewAttachment.JointIndex = i;

                        FoundAttachments.Add(NewAttachment);
                    }
                }
            }

            Attachments = FoundAttachments.ToArray();
        }

        public void GenerateRuntimeDataFromLod(int LodIndex, MT_Lod InLod)
        {
            // NB: This is a fairly unoptimised function;
            // The idea is that we just get it working to begin with, then optimize later.
            if (InLod.VertexDeclaration.HasFlag(VertexFlags.Skin) == false)
            {
                return;
            }

            uint[] UseCountPerBone = new uint[Joints.Length];

            // we will iterate through each material of the LOD.
            // That way we can also identify number of influences per vertex
            foreach(MT_FaceGroup FaceGroup in InLod.FaceGroups)
            {
                uint[] SlotUsage = new uint[4];

                uint StartIndex = FaceGroup.StartIndex;
                uint EndIndex = StartIndex + (FaceGroup.NumFaces * 3);
                for (uint Idx = StartIndex; Idx < EndIndex; Idx++)
                {
                    Vertex V0 = InLod.Vertices[InLod.Indices[Idx]];

                    // iterate to determine usage
                    for (int u = 0; u < 4; u++)
                    {
                        if (V0.BoneWeights[u] > 0.0f)
                        {
                            // used by lod
                            byte BoneID = V0.BoneIDs[u];
                            UseCountPerBone[BoneID]++;

                            SlotUsage[u]++;
                        }
                    }
                }

                // iterate through each slot and increment on facegroup
                for (int u = 0; u < 4; u++)
                {
                    if (SlotUsage[u] > 0)
                    {
                        FaceGroup.WeightsPerVertex++;
                    }
                }
            }

            // now that we know usage count for this lod, we can apply to the Joint
            for(int i = 0; i <  UseCountPerBone.Length; i++)
            {
                if(UseCountPerBone[i] > 0)
                {
                    Joints[i].UsageFlags |= (byte)(0x1 << LodIndex);
                }
            }

            UseCountPerBone = null;
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
