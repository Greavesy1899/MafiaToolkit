using System;
using System.Collections.Generic;
using System.IO;
namespace Mafia2
{
    public class FrameBlendInfo : FrameEntry
    {
        BlendDataToBoneIndexInfo[] blendDataToBoneIndexInfos;
        BlendDataToBoneIndexMap[] blendDataToBoneIndexMaps;
        BoundingBox[] boundingBoxes;
        Bounds bounds;

        public BlendDataToBoneIndexMap[] BlendDataToBoneIndexMaps {
            get { return blendDataToBoneIndexMaps; }
            set { blendDataToBoneIndexMaps = value; }
        }
        public BlendDataToBoneIndexInfo[] BlendDataToBoneIndexInfos {
            get { return blendDataToBoneIndexInfos; }
            set { blendDataToBoneIndexInfos = value; }
        }
        public BoundingBox[] BoundingBoxes {
            get { return boundingBoxes; }
            set { boundingBoxes = value; }
        }
        public Bounds Bound {
            get { return bounds; }
            set { bounds = value; }
        }

        public FrameBlendInfo(BinaryReader reader) : base()
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            int length = reader.ReadInt32();
            byte num = reader.ReadByte();
            blendDataToBoneIndexInfos = new BlendDataToBoneIndexInfo[num];

            for (int i = 0; i != num; i++)
                blendDataToBoneIndexInfos[i] = new BlendDataToBoneIndexInfo(reader);

            bounds = new Bounds(reader);
            boundingBoxes = new BoundingBox[length];

            for (int i = 0; i != length; i++)
                boundingBoxes[i] = new BoundingBox(reader);

            blendDataToBoneIndexMaps = new BlendDataToBoneIndexMap[num];

            for (int i = 0; i != num; i++)
                blendDataToBoneIndexMaps[i] = new BlendDataToBoneIndexMap(reader, blendDataToBoneIndexInfos[i]);
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(boundingBoxes.Length);
            writer.Write((byte)blendDataToBoneIndexInfos.Length);

            for (int i = 0; i != blendDataToBoneIndexInfos.Length; i++)
                blendDataToBoneIndexInfos[i].WriteToFile(writer);

            bounds.WriteToFile(writer);

            for (int i = 0; i != boundingBoxes.Length; i++)
                boundingBoxes[i].WriteToFile(writer);

            for (int i = 0; i != blendDataToBoneIndexMaps.Length; i++)
                blendDataToBoneIndexMaps[i].WriteToFile(writer);
        }

        public override string ToString()
        {
            return $"BlendInfo Block";
        }
    }

    public class BlendDataToBoneIndexInfo
    {
        int numBoneIndices;
        int numBlendIndexRanges;

        public int NumBoneIndices 
        {
            get { return numBoneIndices; }
            set { numBoneIndices = value; }
        }
        public int NumBlendIndexRanges {
            get { return numBlendIndexRanges; }
            set { numBlendIndexRanges = value; }
        }

        public BlendDataToBoneIndexInfo(BinaryReader reader)
        {
            numBoneIndices = reader.ReadInt32();
            NumBlendIndexRanges = reader.ReadInt32();
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(numBoneIndices);
            writer.Write(numBlendIndexRanges);
        }
    }

    public class BlendDataToBoneIndexMap
    {
        private byte[] numData;
        List<byte[]> blendIndices;
        byte[] blendIndexRanges;

        public List<byte[]> BlendIndices {
            get { return blendIndices; }
            set { blendIndices = value; }
        }
        public byte[] BlendIndexRanges {
            get { return blendIndexRanges; }
            set { blendIndexRanges = value; }
        }

        public BlendDataToBoneIndexMap(BinaryReader reader, BlendDataToBoneIndexInfo info)
        {
            ReadFromFile(reader, info);
        }

        public void ReadFromFile(BinaryReader reader, BlendDataToBoneIndexInfo info)
        {
            numData = reader.ReadBytes(8);

            blendIndices = new List<byte[]>();

            for (int i = 0; i != 8; i++)
            {
                if (numData[i] != 0)
                    blendIndices.Add(reader.ReadBytes(numData[i]));
            }

            blendIndexRanges = reader.ReadBytes(info.NumBlendIndexRanges * 2);
        }

        public void WriteToFile(BinaryWriter writer)
        {
            for (int i = 0; i != numData.Length; i++)
                writer.Write(numData[i]);

            for (int i = 0; i != blendIndices.Count; i++)
                writer.Write(blendIndices[i]);

            writer.Write(blendIndexRanges);


        }
    }

    public class BoundingBox
    {
        TransformMatrix transform;
        Bounds bounds;
        bool isValid;

        public TransformMatrix Transform {
            get { return transform; }
            set { transform = value; }
        }
        public Bounds Bounds {
            get { return bounds; }
            set { bounds = value; }
        }
        public bool IsValid {
            get { return isValid; }
            set { isValid = value; }
        }

        public BoundingBox(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            transform = new TransformMatrix(reader);
            bounds = new Bounds(reader);
            isValid = reader.ReadBoolean();
        }

        public void WriteToFile(BinaryWriter writer)
        {
            transform.WriteToFrame(writer);
            bounds.WriteToFile(writer);

            if (isValid)
                writer.Write((byte)0xFF);
            else
                writer.Write((byte)0x00);
        }
    }
}
