using System.IO;

namespace Mafia2
{
    public class FrameSkeleton : FrameEntry
    {
        int count1;
        int count2;
        int count3;
        int count4;
        int numBlendIndices;
        byte lodFlag;
        Hash[] boneIDs;
        TransformMatrix[] mats1;
        byte[] lodFlags;
        TransformMatrix[] worldTransforms;
        SkeletonLodInfo[] lodInfo;
        int[] numArray;

        public int Count1 {
            get { return count1; }
            set { count1 = value; }
        }
        public int Count4 {
            get { return count4; }
            set { count4 = value; }
        }
        public SkeletonLodInfo[] LodInfo {
            get { return lodInfo; }
            set { lodInfo = value; }
        }
        public Hash[] BoneIDs {
            get { return boneIDs; }
            set { boneIDs = value; }
        }

        public FrameSkeleton(BinaryReader reader) : base()
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            count1 = reader.ReadInt32();
            count2 = reader.ReadInt32();
            count3 = reader.ReadInt32();
            count4 = reader.ReadInt32();

            numBlendIndices = reader.ReadInt32();

            int length = reader.ReadInt32();
             numArray = new int[length];

            for (int i = 0; i != length; i++)
                numArray[i] = reader.ReadInt32();

            lodFlag = reader.ReadByte();

            boneIDs = new Hash[count1];

            for (int i = 0; i != count1; i++)
                boneIDs[i] = new Hash(reader);

            mats1 = new TransformMatrix[count2];

            for (int i = 0; i != count2; i++)
                mats1[i] = new TransformMatrix(reader);

            int count = reader.ReadInt32();
            lodFlags = reader.ReadBytes(count);

            worldTransforms = new TransformMatrix[count3];

            for (int i = 0; i != count3; i++)
                worldTransforms[i] = new TransformMatrix(reader);

            lodInfo = new SkeletonLodInfo[length];

            for (int i = 0; i != length; i++)
                lodInfo[i] = new SkeletonLodInfo(reader, this);
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(count1);
            writer.Write(count2);
            writer.Write(count3);
            writer.Write(count4);

            writer.Write(numBlendIndices);
            writer.Write(numArray.Length);

            for (int i = 0; i != numArray.Length; i++)
                writer.Write(numArray[i]);

            writer.Write(lodFlag);

            for (int i = 0; i != count1; i++)
                boneIDs[i].WriteToFile(writer);

            for (int i = 0; i != count2; i++)
                mats1[i].WriteToFrame(writer);

            writer.Write(lodFlags.Length);
            writer.Write(lodFlags);

            for (int i = 0; i != count3; i++)
                worldTransforms[i].WriteToFrame(writer);

            for (int i = 0; i != lodInfo.Length; i++)
                lodInfo[i].WriteToFile(writer);
        }

        public override string ToString()
        {
            return $"Skeleton Block";
        }
    }

    public class SkeletonLodInfo
    {
        Bounds[] bounds;
        byte[] indexMap;
        byte[] lodBlendIndexMap;

        public Bounds[] Bounds {
            get { return bounds; }
            set { bounds = value; }
        }
        public byte[] IndexMap {
            get { return indexMap; }
            set { indexMap = value; }
        }
        public byte[] LodBlendIndexMap {
            get { return lodBlendIndexMap; }
            set { lodBlendIndexMap = value; }
        }

        public SkeletonLodInfo(BinaryReader reader, FrameSkeleton curFrame)
        {
            ReadFromFile(reader, curFrame);
        }

        public void ReadFromFile(BinaryReader reader, FrameSkeleton curFrame)
        {
            int count4 = curFrame.Count4;
            bounds = new Bounds[count4];

            for (int i = 0; i != count4; i++)
                bounds[i] = new Bounds(reader);

            indexMap = reader.ReadBytes(count4);
            byte num = reader.ReadByte();
            lodBlendIndexMap = reader.ReadBytes(num);
        }

        public void WriteToFile(BinaryWriter writer)
        {
            for (int i = 0; i != bounds.Length; i++)
                bounds[i].WriteToFile(writer);

            writer.Write(indexMap);
            writer.Write((byte)lodBlendIndexMap.Length);
            writer.Write(lodBlendIndexMap);
        }
    }
}
