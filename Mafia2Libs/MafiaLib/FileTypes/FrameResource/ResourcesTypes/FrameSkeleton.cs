using System.ComponentModel;
using System.IO;
using SharpDX;
using Utils.Extensions;
using Utils.SharpDXExtensions;
using Utils.Types;

namespace ResourceTypes.FrameResource
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class FrameSkeleton : FrameEntry
    {
        int[] numBones = new int[4];
        int numBlendIDs;
        int numLods;
        int[] unkLodData;
        byte idType;
        Hash[] boneNames;
        Matrix[] matrices1;
        int numUnkCount2;
        byte[] boneLODUsage;
        Matrix[] matrices2;
        MappingForBlendingInfo[] mappingForBlendingInfos;

        public int[] NumBones {
            get { return numBones; }
            set { numBones = value; }
        }
        public int NumBlendIDs {
            get { return numBlendIDs; }
            set { numBlendIDs = value; }
        }
        public int NumLods {
            get { return numLods; }
            set { numLods = value; }
        }
        public int[] UnkLodData {
            get { return unkLodData; }
            set { unkLodData = value; }
        }
        public byte IDType {
            get { return idType; }
            set { idType = value; }
        }
        public Hash[] BoneNames {
            get { return boneNames; }
            set { boneNames = value; }
        }
        public Matrix[] Matrices1 {
            get { return matrices1; }
            set { matrices1 = value; }
        }
        public int NumUnkCount2 {
            get { return numUnkCount2; }
            set { numUnkCount2 = value; }
        }
        public byte[] BoneLODUsage {
            get { return boneLODUsage; }
            set { boneLODUsage = value; }
        }
        public Matrix[] Matrices2 {
            get { return matrices2; }
            set { matrices2 = value; }
        }
        public MappingForBlendingInfo[] MappingForBlendingInfos {
            get { return mappingForBlendingInfos; }
            set { mappingForBlendingInfos = value; }
        }

        public FrameSkeleton(MemoryStream reader, bool isBigEndian) : base()
        {
            ReadFromFile(reader, isBigEndian);
        }

        public void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            //all the same values?
            for (int i = 0; i != numBones.Length; i++)
                numBones[i] = stream.ReadInt32(isBigEndian);

            numBlendIDs = stream.ReadInt32(isBigEndian);
            numLods = stream.ReadInt32(isBigEndian);

            //unknown lod data; 
            unkLodData = new int[numLods];
            for (int i = 0; i != unkLodData.Length; i++)
                unkLodData[i] = stream.ReadInt32(isBigEndian);

            idType = stream.ReadByte8();

            //Bone Names and LOD data.
            boneNames = new Hash[numBones[0]];
            for (int i = 0; i != boneNames.Length; i++)
                boneNames[i] = new Hash(stream, isBigEndian);

            //Matrices;
            matrices1 = new Matrix[numBones[1]];
            matrices2 = new Matrix[numBones[3]];

            for (int i = 0; i != matrices1.Length; i++)
                matrices1[i] = MatrixExtensions.ReadFromFile(stream, isBigEndian);

            numUnkCount2 = stream.ReadInt32(isBigEndian);
            boneLODUsage = stream.ReadBytes(numUnkCount2);

            for (int i = 0; i != matrices2.Length; i++)
                matrices2[i] = MatrixExtensions.ReadFromFile(stream, isBigEndian);

            //BoneMappings.
            mappingForBlendingInfos = new MappingForBlendingInfo[numLods];
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
                mappingForBlendingInfos[i].UsageArray = stream.ReadBytes(unkLodData[i]);
            }
        }

        public void WriteToFile(BinaryWriter writer)
        {
            //all the same values?
            for (int i = 0; i != numBones.Length; i++)
                writer.Write(numBones[i]);

            writer.Write(numBlendIDs);
            writer.Write(numLods);

            //unknown lod data; 
            for (int i = 0; i != unkLodData.Length; i++)
                writer.Write(unkLodData[i]);

            writer.Write(idType);

            //Bone Names and LOD data.
            for (int i = 0; i != boneNames.Length; i++)
                boneNames[i].WriteToFile(writer);

            for (int i = 0; i != matrices1.Length; i++)
                matrices1[i].WriteToFile(writer);

            writer.Write(numUnkCount2);
            writer.Write(boneLODUsage);

            for (int i = 0; i != matrices2.Length; i++)
                matrices2[i].WriteToFile(writer);

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
            return string.Format("Skeleton");
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
            public byte[] RefToUsageArray {
                get { return refToUsageArray; }
                set { refToUsageArray = value; }
            }
            public byte[] UsageArray {
                get { return usageArray; }
                set { usageArray = value; }
            }
        }
    }
}