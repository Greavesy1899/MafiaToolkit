using System;
using System.Collections.Generic;
using System.IO;
namespace Mafia2
{
    public class FrameBlendInfo
    {
        BlendDataToBoneIndexInfo[] blendDataToBoneIndexInfos;
        Bounds bounds;
        BoundingBox[] boundingBoxes;
        BlendDataToBoneIndexMap[] blendDataToBoneIndexMaps;

        public BlendDataToBoneIndexMap[] BlendDataToBoneIndexMaps {
            get { return blendDataToBoneIndexMaps; }
            set { blendDataToBoneIndexMaps = value; }
        }

        public FrameBlendInfo(BinaryReader reader)
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
    }

    public class BlendDataToBoneIndexMap
    {
        private byte[] numData;
        byte[] blendIndices;
        byte[] blendIndexRanges;

        public byte[] BlendIndices {
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
            //DO HERE

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
            transform.WriteToFile(writer);
            bounds.WriteToFile(writer);
            writer.Write(isValid);
        }
    }
}
