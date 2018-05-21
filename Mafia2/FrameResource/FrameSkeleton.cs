using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Mafia2
{
    public class FrameSkeleton
    {
        int count1;
        int count2;
        int count3;
        int count4;
        int numBlendIndices;
        byte lodFlag;
        Hash[] names;
        TransformMatrix[] mats1;
        byte[] lodFlags;
        TransformMatrix[] worldTransforms;
        SkeletonLodInfo[] lodInfo;

        public int Count4 {
            get { return count4; }
            set { count4 = value; }
        }

        public FrameSkeleton(BinaryReader reader)
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
            int[] numArray = new int[length];

            for (int i = 0; i != length; i++)
                numArray[i] = reader.ReadInt32();

            lodFlag = reader.ReadByte();

            names = new Hash[count1];

            for (int i = 0; i != count1; i++)
                names[i] = new Hash(reader);

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
    }

    public class SkeletonLodInfo
    {
        Bounds[] bounds;
        byte[] indexMap;
        byte[] lodBlendIndexMap;

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
    }
}
