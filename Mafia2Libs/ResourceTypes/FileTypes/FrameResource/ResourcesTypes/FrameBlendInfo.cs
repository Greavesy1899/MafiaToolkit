using System.ComponentModel;
using System.IO;
using System.Numerics;
using Utils.Extensions;
using Utils.VorticeUtils;
using Vortice.Mathematics;

namespace ResourceTypes.FrameResource
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class FrameBlendInfo : FrameEntry
    {
        BoneIndexInfo[] boneIndexInfos;
        BoundingBox bounds;
        BoneTransform[] boneTransforms;

        public BoneIndexInfo[] BoneIndexInfos {
            get { return boneIndexInfos; }
            set { boneIndexInfos = value; }
        }
        public BoneTransform[] BoneTransforms {
            get { return boneTransforms; }
            set { boneTransforms = value; }
        }
        public BoundingBox Bound {
            get { return bounds; }
            set { bounds = value; }
        }

        public FrameBlendInfo(FrameResource OwningResource) : base(OwningResource)
        {
            boneIndexInfos = new BoneIndexInfo[0];
            boneTransforms = new BoneTransform[0];
        }

        public void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            int numBones = reader.ReadInt32(isBigEndian);
            byte numLods = reader.ReadByte8();

            //index infos
            boneIndexInfos = new BoneIndexInfo[numLods];
            for (int i = 0; i < boneIndexInfos.Length; i++)
            {
                uint NumberRemapIDs = reader.ReadUInt32(isBigEndian);
                boneIndexInfos[i].BoneRemapIDs = new byte[NumberRemapIDs];

                uint NumMaterials = reader.ReadUInt32(isBigEndian);
                boneIndexInfos[i].SkinnedMaterialInfo = new SkinnedMaterialInfo[NumMaterials];
            }

            //bounds for all bones together?
            bounds = BoundingBoxExtenders.ReadFromFile(reader, isBigEndian);

            //Bone Transforms
            boneTransforms = new BoneTransform[numBones];
            for (int i = 0; i < boneTransforms.Length; i++)
            {
                boneTransforms[i] = new BoneTransform();
                boneTransforms[i].ReadFromFile(reader, isBigEndian);
            }

            for (int i = 0; i < boneIndexInfos.Length; i++)
            {
                boneIndexInfos[i].BonesPerRemapPool = reader.ReadBytes(8);

                // This is actually creating the array again and could be fairly costly
                // I won't tell if you don't
                int NumberOfRemapIDs = boneIndexInfos[i].BoneRemapIDs.Length;
                boneIndexInfos[i].BoneRemapIDs = reader.ReadBytes(NumberOfRemapIDs);

                // Read additional weighted info
                for (int x = 0; x < boneIndexInfos[i].SkinnedMaterialInfo.Length; x++)
                {
                    SkinnedMaterialInfo NewWeightedInfo = new SkinnedMaterialInfo();
                    NewWeightedInfo.AssignedPoolIndex = reader.ReadByte8();
                    NewWeightedInfo.NumWeightsPerVertex = reader.ReadByte8();
                    boneIndexInfos[i].SkinnedMaterialInfo[x] = NewWeightedInfo;
                }
            }
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(boneTransforms.Length);
            writer.Write((byte)boneIndexInfos.Length);

            //index infos
            for (int i = 0; i < boneIndexInfos.Length; i++)
            {
                writer.Write(boneIndexInfos[i].BoneRemapIDs.Length);
                writer.Write(boneIndexInfos[i].SkinnedMaterialInfo.Length);
            }

            //bounds for all bones together?
            bounds.WriteToFile(writer);

            //Bone Transforms
            for (int i = 0; i < boneTransforms.Length; i++)
            {
                boneTransforms[i].WriteToFile(writer);
            }

            for (int i = 0; i < boneIndexInfos.Length; i++)
            {
                writer.Write(boneIndexInfos[i].BonesPerRemapPool);
                writer.Write(boneIndexInfos[i].BoneRemapIDs);

                // Write additional data for Materials
                foreach (SkinnedMaterialInfo MaterialInfo in boneIndexInfos[i].SkinnedMaterialInfo)
                {
                    writer.Write(MaterialInfo.AssignedPoolIndex);
                    writer.Write(MaterialInfo.NumWeightsPerVertex);
                }
            }
        }

        public override string ToString()
        {
            return "Blend Info Block";
        }

        public struct SkinnedMaterialInfo
        {
            // Stores the number of weights influencing the vertex within a facegroup.
            // Max number of weights per vertex is 4.
            public byte AssignedPoolIndex { get; set; }

            // Stores which pool of bones the material has been assigned to.
            // Each slot in the array is for a facegroup within the LOD
            public byte NumWeightsPerVertex { get; set; }
        }

        public struct BoneIndexInfo
        {
            // Number of bones within each Remap Pool, SkinnedMaterialInfo will refer to this.
            public byte[] BonesPerRemapPool { get; set; }

            // Remapping IDs for bones within the Skeletal Mesh for this LOD
            // Refer to @BonesPerPool to determine which range of bones is within each pool
            public byte[] BoneRemapIDs { get; set; }

            // Skinned Material data for this LOD
            public SkinnedMaterialInfo[] SkinnedMaterialInfo { get; set; }
        }

        public struct BoneTransform
        {
            Matrix4x4 transform;
            BoundingBox bounds;
            byte isValid;

            public Matrix4x4 Transform {
                get { return transform; }
                set { transform = value; }
            }
            public BoundingBox Bounds {
                get { return bounds; }
                set { bounds = value; }
            }
            public byte IsValid {
                get { return isValid; }
                set { isValid = value; }
            }

            public void ReadFromFile(MemoryStream stream, bool isBigEndian)
            {
                transform = MatrixUtils.ReadFromFile(stream, isBigEndian);
                bounds = BoundingBoxExtenders.ReadFromFile(stream, isBigEndian);
                isValid = stream.ReadByte8();
            }

            public void WriteToFile(BinaryWriter writer)
            {
                transform.WriteToFile(writer);
                bounds.WriteToFile(writer);
                writer.Write(IsValid);
            }
        }
    }
}
