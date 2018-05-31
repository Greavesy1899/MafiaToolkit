using System;
using System.Collections;
using System.IO;

namespace Mafia2
{
    public class FrameObjectModel : FrameObjectSingleMesh
    {
        private ArrayList frameBlocks;
        private FrameSkeleton skeleton;
        private FrameBlendInfo blendInfo;

        int blendInfoIndex;
        int skeletonIndex;
        int skeletonHierachyIndex;
        TransformMatrix[] restPose;
        TransformMatrix unkTrasform;
        AttachmentReference[] attachmentReferences;
        int unk_28_int;
        int unk_29_int;
        unk_struct2[] unk_30_list;

        public int BlendInfoIndex {
            get { return blendInfoIndex; }
            set { blendInfoIndex = value; }
        }
        public int SkeletonIndex {
            get { return skeletonIndex; }
            set { skeletonIndex = value; }
        }
        public int SkeletonHierachyIndex {
            get { return skeletonHierachyIndex; }
            set { skeletonHierachyIndex = value; }
        }

        public FrameObjectModel (BinaryReader reader, ArrayList fb)
        {
            frameBlocks = fb;
            ReadFromFile(reader);
        }

        public override void ReadFromFile(BinaryReader reader)
        {
            base.ReadFromFile(reader);

            blendInfoIndex = reader.ReadInt32();
            skeletonIndex = reader.ReadInt32();
            skeletonHierachyIndex = reader.ReadInt32();

            skeleton = frameBlocks[skeletonIndex] as FrameSkeleton;
            blendInfo = frameBlocks[blendInfoIndex] as FrameBlendInfo;

            restPose = new TransformMatrix[skeleton.Count1];
            for (int i = 0; i != restPose.Length; i++)
                restPose[i] = new TransformMatrix(reader);

            unkTrasform = new TransformMatrix(reader);

            int length1 = reader.ReadInt32();
            attachmentReferences = new AttachmentReference[length1];

            for (int i = 0; i != length1; i++)
                attachmentReferences[i] = new AttachmentReference(reader);

            unk_28_int = reader.ReadInt32();
            unk_29_int = reader.ReadInt32();

            int count = reader.ReadInt32();
            short length2 = reader.ReadInt16();

            unk_30_list = new unk_struct2[length2];
            for (int i = 0; i != length2; i++)
                unk_30_list[i] = new unk_struct2(reader);

            byte[] numArray = new byte[skeleton.LodInfo[0].LodBlendIndexMap.Length];

            int destIndex = 0;
            foreach(byte[] blendIndex in blendInfo.BlendDataToBoneIndexMaps[0].BlendIndices)
            {
                Array.Copy(blendIndex, 0, numArray, destIndex, blendIndex.Length);
                destIndex += blendIndex.Length;
            }
            for(int i = 0; i != unk_30_list.Length; i++)
            {
                int index2 = numArray[unk_30_list[i].BlendIndex];
                unk_30_list[1].JointName = skeleton.Names[index2].Name;
            }
            reader.ReadBytes(count);           
        }

        public override string ToString()
        {
            return string.Format("Model Block:: {0}", Name.Name);
        }
    }
    public class AttachmentReference
    {
        int attachmentIndex;
        byte jointIndex;

        public AttachmentReference(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            attachmentIndex = reader.ReadInt32();
            jointIndex = reader.ReadByte();
        }
    }

    public class unk_struct2
    {
        short blendIndex;
        unk_struct2_1[] data;
        string jointName;

        public short BlendIndex {
            get { return blendIndex; }
            set { blendIndex = value; }
        }
        public string JointName {
            get { return jointName; }
            set { jointName = value; }
        }

        public unk_struct2(BinaryReader reader)
        {
            blendIndex = reader.ReadInt16();

            short num = reader.ReadInt16();
            data = new unk_struct2_1[num];

            for (int i = 0; i != num; i++)
                data[i] = new unk_struct2_1(reader);
        }
    }

    public class unk_struct2_1
    {
        unk_struct2_1_1[] data;

        public unk_struct2_1(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            short num = reader.ReadInt16();
            data = new unk_struct2_1_1[num];

            for (int i = 0; i != num; i++)
                data[i] = new unk_struct2_1_1(reader);
        }
    }

    public class unk_struct2_1_1
    {
        short materialIndex;
        unk_struct2_1_1_1[] data;

        public unk_struct2_1_1(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            materialIndex = reader.ReadInt16();

            short num = reader.ReadInt16();
            data = new unk_struct2_1_1_1[num];

            for (int i = 0; i != num; i++)
                data[i] = new unk_struct2_1_1_1(reader);
        }
    }

    public class unk_struct2_1_1_1
    {
        short startIndex;
        short numFaces;

        public unk_struct2_1_1_1(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            startIndex = reader.ReadInt16();
            numFaces = reader.ReadInt16();
        }
    }

}
