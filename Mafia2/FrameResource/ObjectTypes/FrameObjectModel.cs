using System;
using System.Collections;
using System.IO;

namespace Mafia2
{
    public class FrameObjectModel : FrameObjectSingleMesh
    {
        private object[] frameBlocks;
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
        byte[] unkData;

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
        public unk_struct2[] Unk_30 {
            get { return unk_30_list; }
            set { unk_30_list = value; }
        }

        public FrameObjectModel (BinaryReader reader, object[] fb)
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
                unk_30_list[i].JointName = (SkeletonBoneIDs)skeleton.BoneIDs[index2].uHash;
            }
            unkData = reader.ReadBytes(count);           
        }

        public override void WriteToFile(BinaryWriter writer)
        {
            base.WriteToFile(writer);
            writer.Write(blendInfoIndex);
            writer.Write(skeletonIndex);
            writer.Write(skeletonHierachyIndex);

            for (int i = 0; i != restPose.Length; i++)
                restPose[i].WriteToFile(writer);

            unkTrasform.WriteToFile(writer);
            writer.Write(attachmentReferences.Length);

            for (int i = 0; i != attachmentReferences.Length; i++)
                attachmentReferences[i].WriteToFile(writer);

            writer.Write(unk_28_int);
            writer.Write(unk_29_int);

            writer.Write(unkData.Length);
            writer.Write(unk_30_list.Length);

            for (int i = 0; i != unk_30_list.Length; i++)
                unk_30_list[i].WriteToFile(writer);
        }

        public override string ToString()
        {
            return string.Format("{0}", Name.String);
        }
    }
    public class AttachmentReference
    {
        int attachmentIndex;
        byte jointIndex;

        public int AttachmentIndex {
            get { return attachmentIndex; }
            set { attachmentIndex = value; }
        }
        public byte JointIndex {
            get { return jointIndex; }
            set { jointIndex = value; }
        }

        public AttachmentReference(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            attachmentIndex = reader.ReadInt32();
            jointIndex = reader.ReadByte();
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(attachmentIndex);
            writer.Write(jointIndex);
        }
    }

    public class unk_struct2
    {
        short blendIndex;
        unk_struct2_1[] data;
        SkeletonBoneIDs jointName;

        public short BlendIndex {
            get { return blendIndex; }
            set { blendIndex = value; }
        }
        public unk_struct2_1[] Data {
            get { return data; }
            set { data = value; }
        }
        public SkeletonBoneIDs JointName {
            get { return jointName; }
            set { jointName = value; }
        }

        public unk_struct2(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            blendIndex = reader.ReadInt16();

            short num = reader.ReadInt16();
            data = new unk_struct2_1[num];

            for (int i = 0; i != num; i++)
                data[i] = new unk_struct2_1(reader);
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(blendIndex);
            writer.Write((short)data.Length);

            for (int i = 0; i != data.Length; i++)
                data[i].WriteToFile(writer);
        }

        public override string ToString()
        {
            return jointName.ToString();
        }
    }

    public class unk_struct2_1
    {
        unk_struct2_1_1[] data;

        public unk_struct2_1_1[] Data {
            get { return data; }
            set { data = value; }
        }

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

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write((short)data.Length);
            for (int i = 0; i != data.Length; i++)
                data[i].WriteToFile(writer);
        }
    }

    public class unk_struct2_1_1
    {
        short materialIndex;
        unk_struct2_1_1_1[] data;
        
        public short MaterialIndex {
            get { return materialIndex; }
            set { materialIndex = value; }
        }
        public unk_struct2_1_1_1[] Data {
            get { return data; }
            set { data = value; }
        }

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

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(materialIndex);
            writer.Write((short)data.Length);
            for (int i = 0; i != data.Length; i++)
                data[i].WriteToFile(writer);
        }
    }

    public class unk_struct2_1_1_1
    {
        short startIndex;
        short numFaces;

        public short StartIndex {
            get { return startIndex; }
            set { startIndex = value; }
        }
        public short NumFaces {
            get { return numFaces; }
            set { numFaces = value; }
        }

        public unk_struct2_1_1_1(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            startIndex = reader.ReadInt16();
            numFaces = reader.ReadInt16();
        }
        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(startIndex);
            writer.Write(numFaces);
        }
    }

}
