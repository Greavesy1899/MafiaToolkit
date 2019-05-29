using System.ComponentModel;
using System.IO;
using SharpDX;
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
        TransformMatrix[] matrices1;
        int numUnkCount2;
        byte[] boneLODUsage;
        TransformMatrix[] matrices2;
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
        public TransformMatrix[] Matrices1 {
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
        public TransformMatrix[] Matrices2 {
            get { return matrices2; }
            set { matrices2 = value; }
        }
        public MappingForBlendingInfo[] MappingForBlendingInfos {
            get { return mappingForBlendingInfos; }
            set { mappingForBlendingInfos = value; }
        }

        public FrameSkeleton(BinaryReader reader) : base()
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            //all the same values?
            for (int i = 0; i != numBones.Length; i++)
                numBones[i] = reader.ReadInt32();

            numBlendIDs = reader.ReadInt32();
            numLods = reader.ReadInt32();

            //unknown lod data; 
            unkLodData = new int[numLods];
            for (int i = 0; i != unkLodData.Length; i++)
                unkLodData[i] = reader.ReadInt32();

            idType = reader.ReadByte();

            //Bone Names and LOD data.
            boneNames = new Hash[numBones[0]];
            for (int i = 0; i != boneNames.Length; i++)
                boneNames[i] = new Hash(reader);

            //Matrices;
            matrices1 = new TransformMatrix[numBones[1]];
            matrices2 = new TransformMatrix[numBones[3]];

            for (int i = 0; i != matrices1.Length; i++)
                matrices1[i] = new TransformMatrix(reader);

            numUnkCount2 = reader.ReadInt32();
            boneLODUsage = reader.ReadBytes(numUnkCount2);

            for (int i = 0; i != matrices2.Length; i++)
                matrices2[i] = new TransformMatrix(reader);

            //BoneMappings.
            mappingForBlendingInfos = new MappingForBlendingInfo[numLods];
            for (int i = 0; i != mappingForBlendingInfos.Length; i++)
            {
                mappingForBlendingInfos[i].Bounds = new BoundingBox[numBones[2]];

                for (int x = 0; x != mappingForBlendingInfos[i].Bounds.Length; x++)
                {
                    mappingForBlendingInfos[i].Bounds[x] = BoundingBoxExtenders.ReadFromFile(reader);
                }
                if (reader.ReadByte() != 0)
                    throw new System.Exception("oops");

                mappingForBlendingInfos[i].RefToUsageArray = reader.ReadBytes(numBones[2]);
                mappingForBlendingInfos[i].UsageArray = reader.ReadBytes(unkLodData[i]);
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
                matrices1[i].WriteToFrame(writer);

            writer.Write(numUnkCount2);
            writer.Write(boneLODUsage);

            for (int i = 0; i != matrices2.Length; i++)
                matrices2[i].WriteToFrame(writer);

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