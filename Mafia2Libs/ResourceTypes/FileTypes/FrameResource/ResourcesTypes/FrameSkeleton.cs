﻿using System.ComponentModel;
using System.IO;
using System.Numerics;
using Utils.Extensions;
using Utils.Types;
using Utils.VorticeUtils;
using Vortice.Mathematics;

namespace ResourceTypes.FrameResource
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class FrameSkeleton : FrameEntry
    {
        int[] numBones = new int[4];
        int numBlendIDs;
        byte idType;

        // Name of bones
        HashName[] boneNames;

        // maybe joint space
        Matrix4x4[] jointTransforms; 
        int numUnkCount2;

        // This stores if the LOD vertices use the bone - does not mean exclude bone from skeleton.
        // We'd only add whether or not the bone is used if the models vertices has any weight to the bone
        byte[] boneLODUsage;

        //world space = extract position matrix, extract rotation matrix, multiply -position * rotation
        Matrix4x4[] worldTransforms; 

        MappingForBlendingInfo[] mappingForBlendingInfos;

        // TODO: boneNames, boneLODUsage and jointTransforms all could be stored as same class

        public int[] NumBones {
            get { return numBones; }
            set { numBones = value; }
        }
        public int NumBlendIDs {
            get { return numBlendIDs; }
            set { numBlendIDs = value; }
        }

        // How many Remap IDs are present for the LOD. This must match LOD count.
        public int[] LodRemapIDCount { get; set; }

        public byte IDType {
            get { return idType; }
            set { idType = value; }
        }
        public HashName[] BoneNames {
            get { return boneNames; }
            set { boneNames = value; }
        }
        public Matrix4x4[] JointTransforms {
            get { return jointTransforms; }
            set { jointTransforms = value; }
        }
        public int NumUnkCount2 {
            get { return numUnkCount2; }
            set { numUnkCount2 = value; }
        }
        public byte[] BoneLODUsage {
            get { return boneLODUsage; }
            set { boneLODUsage = value; }
        }
        public Matrix4x4[] WorldTransforms {
            get { return worldTransforms; }
            set { worldTransforms = value; }
        }
        public MappingForBlendingInfo[] MappingForBlendingInfos {
            get { return mappingForBlendingInfos; }
            set { mappingForBlendingInfos = value; }
        }

        public FrameSkeleton(FrameResource OwningResource) : base(OwningResource)
        {
            numBones = new int[4];
            LodRemapIDCount = new int[0];
            boneNames = new HashName[0];
            jointTransforms = new Matrix4x4[0];
            worldTransforms = new Matrix4x4[0];
            mappingForBlendingInfos = new MappingForBlendingInfo[0];
        }

        public void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            //all the same values?
            for (int i = 0; i != numBones.Length; i++)
            {
                numBones[i] = stream.ReadInt32(isBigEndian);
            }

            numBlendIDs = stream.ReadInt32(isBigEndian);

            // Need to load how many Remap IDs are present for each LOD
            int NumLODs = stream.ReadInt32(isBigEndian);
            LodRemapIDCount = new int[NumLODs];
            for (int i = 0; i < LodRemapIDCount.Length; i++)
            {
                LodRemapIDCount[i] = stream.ReadInt32(isBigEndian);
            }

            idType = stream.ReadByte8();

            //Bone Names and LOD data.
            boneNames = new HashName[numBones[0]];
            for (int i = 0; i != boneNames.Length; i++)
            {
                boneNames[i] = new HashName(stream, isBigEndian);
            }

            //Matrices;
            jointTransforms = new Matrix4x4[numBones[1]];
            worldTransforms = new Matrix4x4[numBones[3]];

            for (int i = 0; i != jointTransforms.Length; i++)
            {
                jointTransforms[i] = MatrixUtils.ReadFromFile(stream, isBigEndian);
            }

            numUnkCount2 = stream.ReadInt32(isBigEndian);
            boneLODUsage = stream.ReadBytes(numUnkCount2);

            for (int i = 0; i != worldTransforms.Length; i++)
            {
                worldTransforms[i] = MatrixUtils.ReadFromFile(stream, isBigEndian);
            }

            //BoneMappings.
            mappingForBlendingInfos = new MappingForBlendingInfo[NumLODs];
            for (int i = 0; i != mappingForBlendingInfos.Length; i++)
            {
                mappingForBlendingInfos[i].Bounds = new BoundingBox[numBones[2]];

                for (int x = 0; x != mappingForBlendingInfos[i].Bounds.Length; x++)
                {
                    mappingForBlendingInfos[i].Bounds[x] = BoundingBoxExtenders.ReadFromFile(stream, isBigEndian);
                }
                if (stream.ReadByte() != 0)
                    throw new System.Exception("oops");

                mappingForBlendingInfos[i].RefToUsageArray = stream.ReadBytes(numBones[2]);
                mappingForBlendingInfos[i].UsageArray = stream.ReadBytes(LodRemapIDCount[i]);
            }
        }

        public void WriteToFile(BinaryWriter writer)
        {
            //all the same values?
            for (int i = 0; i != numBones.Length; i++)
                writer.Write(numBones[i]);

            writer.Write(numBlendIDs);

            // Write Remap ID counts for each LOD
            writer.Write(LodRemapIDCount.Length);
            foreach(int RemapCount in LodRemapIDCount)
            {
                writer.Write(RemapCount);
            }

            writer.Write(idType);

            //Bone Names and LOD data.
            for (int i = 0; i != boneNames.Length; i++)
                boneNames[i].WriteToFile(writer);

            for (int i = 0; i != jointTransforms.Length; i++)
                jointTransforms[i].WriteToFile(writer);

            writer.Write(numUnkCount2);
            writer.Write(boneLODUsage);

            for (int i = 0; i != worldTransforms.Length; i++)
            {
                worldTransforms[i].WriteToFile(writer);
            }

            //BoneMappings.
            for (int i = 0; i != mappingForBlendingInfos.Length; i++)
            {
                for (int x = 0; x != mappingForBlendingInfos[i].Bounds.Length; x++)
                {
                    mappingForBlendingInfos[i].Bounds[x].WriteToFile(writer);
                }
                writer.Write((byte)0);

                writer.Write(mappingForBlendingInfos[i].RefToUsageArray);
                writer.Write(mappingForBlendingInfos[i].UsageArray);
            }
        }

        public override string ToString()
        {
            return "Skeleton Block";
        }

        public struct MappingForBlendingInfo
        {
            BoundingBox[] bounds;
            byte[] refToUsageArray;
            byte[] usageArray;

            public BoundingBox[] Bounds {
                get { return bounds; }
                set { bounds = value; }
            }

            // TODO: This is loaded using Bone Count in Skeleton
            // Can we determine what this is? My suspicion is an easy lookup between Bone -> Remapped ID.
            // This may be a code side of finding the remapped vertices using the bone ID.
            public byte[] RefToUsageArray {
                get { return refToUsageArray; }
                set { refToUsageArray = value; }
            }

            // TODO: This is loaded using Remap Count for each LOD.
            // Can we determine what this is? My suspicion is how many times the Remap ID is used.
            // But does that mean across Materials, or per vertex? And does it cross Material boundaries?
            public byte[] UsageArray {
                get { return usageArray; }
                set { usageArray = value; }
            }
        }
    }
}