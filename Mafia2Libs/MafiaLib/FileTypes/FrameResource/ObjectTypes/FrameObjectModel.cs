﻿using System.ComponentModel;
using System.IO;
using Utils.Extensions;
using Utils.Models;
using Utils.Types;

namespace ResourceTypes.FrameResource
{
    public class FrameObjectModel : FrameObjectSingleMesh
    {
        private FrameSkeleton skeleton;
        private FrameBlendInfo blendInfo;
        private FrameSkeletonHierachy hierachy;

        int blendInfoIndex;
        int skeletonIndex;
        int skeletonHierachyIndex;
        TransformMatrix[] restPose;
        TransformMatrix unkTrasform;
        AttachmentReference[] attachmentReferences;
        uint unkFlags;
        int physSplitSize;
        int hitBoxSize;
        short nPhysSplits;
        WeightedByMeshSplit[] blendMeshSplits;
        HitBoxInfo[] hitBoxInfo;

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
        public WeightedByMeshSplit[] BlendMeshSplits {
            get { return blendMeshSplits; }
            set { blendMeshSplits = value; }
        }
        public TransformMatrix[] RestPose {
            get { return restPose; }
            set { restPose = value; }
        }
        public TransformMatrix UnkTrasform {
            get { return unkTrasform; }
            set { unkTrasform = value; }
        }
        public HitBoxInfo[] HitBoxes {
            get { return hitBoxInfo; }
            set { hitBoxInfo = value; }
        }
        public AttachmentReference[] AttachmentReferences {
            get { return attachmentReferences; }
            set { attachmentReferences = value; }
        }
        public uint UnkFlags {
            get { return unkFlags; }
            set { unkFlags = value; }
        }
        public FrameSkeleton Skeleton {
            get { return skeleton; }
            set { skeleton = value; }
        }
        public FrameBlendInfo BlendInfo {
            get { return blendInfo; }
            set { blendInfo = value; }
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public FrameSkeletonHierachy SkeletonHierarchy {
            get { return hierachy; }
            set { hierachy = value; }
        }

        public FrameObjectModel (MemoryStream reader, bool isBigEndian)
        {
        }

        public FrameObjectModel(FrameObjectModel other) : base(other)
        {
            blendInfoIndex = other.blendInfoIndex;
            skeletonIndex = other.skeletonIndex;
            skeletonHierachyIndex = other.skeletonHierachyIndex;
            skeleton = other.skeleton;
            blendInfo = other.blendInfo;
            restPose = new TransformMatrix[skeleton.NumBones[0]];
            for (int i = 0; i != restPose.Length; i++)
                restPose[i] = new TransformMatrix(other.restPose[i]);
            unkTrasform = other.unkTrasform;
            attachmentReferences = new AttachmentReference[other.attachmentReferences.Length];
            for (int i = 0; i != attachmentReferences.Length; i++)
                attachmentReferences[i] = new AttachmentReference(other.attachmentReferences[i]);
            unkFlags = other.unkFlags;
            physSplitSize = other.physSplitSize;
            hitBoxSize = other.hitBoxSize;
            nPhysSplits = other.nPhysSplits;
            blendMeshSplits = new WeightedByMeshSplit[nPhysSplits];
            for (int i = 0; i != blendMeshSplits.Length; i++)
                blendMeshSplits[i] = new WeightedByMeshSplit(other.blendMeshSplits[i]);
            hitBoxInfo = new HitBoxInfo[other.hitBoxInfo.Length];
            for (int i = 0; i != hitBoxInfo.Length; i++)
                hitBoxInfo[i] = new HitBoxInfo(hitBoxInfo[i]);
        }

        public override void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            base.ReadFromFile(reader, isBigEndian);

            blendInfoIndex = reader.ReadInt32(isBigEndian);
            skeletonIndex = reader.ReadInt32(isBigEndian);
            skeletonHierachyIndex = reader.ReadInt32(isBigEndian);
        }

        public void ReadFromFilePart2(MemoryStream reader, bool isBigEndian, FrameSkeleton skeleton, FrameBlendInfo blendInfo)
        {
            this.skeleton = skeleton;
            this.blendInfo = blendInfo;

            //do rest matrices.
            restPose = new TransformMatrix[skeleton.NumBones[0]];
            for (int i = 0; i != restPose.Length; i++)
                restPose[i] = new TransformMatrix(reader, isBigEndian);

            //unknown transform.
            unkTrasform = new TransformMatrix(reader, isBigEndian);

            //attachments.
            int length1 = reader.ReadInt32(isBigEndian);
            attachmentReferences = new AttachmentReference[length1];

            for (int i = 0; i != length1; i++)
                attachmentReferences[i] = new AttachmentReference(reader, isBigEndian);

            //unknwon.
            unkFlags = reader.ReadUInt32(isBigEndian);
            physSplitSize = reader.ReadInt32(isBigEndian);
            hitBoxSize = reader.ReadInt32(isBigEndian);

            if (physSplitSize > 0)
                nPhysSplits = reader.ReadInt16(isBigEndian);
            else
                nPhysSplits = 0;

            int totalSplits = 0;
            blendMeshSplits = new WeightedByMeshSplit[nPhysSplits];
            for (int i = 0; i != nPhysSplits; i++)
            {
                blendMeshSplits[i] = new WeightedByMeshSplit(reader, isBigEndian);
                totalSplits += blendMeshSplits[i].Data.Length;
            }

            hitBoxInfo = new HitBoxInfo[totalSplits];
            for (int i = 0; i != hitBoxInfo.Length; i++)
                hitBoxInfo[i] = new HitBoxInfo(reader, isBigEndian);
        }

        public override void WriteToFile(BinaryWriter writer)
        {
            base.WriteToFile(writer);
            writer.Write(blendInfoIndex);
            writer.Write(skeletonIndex);
            writer.Write(skeletonHierachyIndex);

            //do rest matrices.
            for (int i = 0; i != restPose.Length; i++)
                restPose[i].WriteToFile(writer);

            //unknown transform.
            unkTrasform.WriteToFile(writer);

            //attachments.
            writer.Write(attachmentReferences.Length);

            for (int i = 0; i != attachmentReferences.Length; i++)
                attachmentReferences[i].WriteToFile(writer);

            //unknwon.
            writer.Write(unkFlags);
            writer.Write(physSplitSize);
            writer.Write(hitBoxSize);

            if (physSplitSize > 0)
                writer.Write(nPhysSplits);

            for (int i = 0; i != nPhysSplits; i++)
                blendMeshSplits[i].WriteToFile(writer);

            for (int i = 0; i != hitBoxInfo.Length; i++)
                hitBoxInfo[i].WriteToFile(writer);
        }

        public override string ToString()
        {
            return string.Format("{0}", Name.ToString());
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

            public AttachmentReference(MemoryStream reader, bool isBigEndian)
            {
                ReadFromFile(reader, isBigEndian);
            }

            public AttachmentReference(AttachmentReference other)
            {
                attachmentIndex = other.attachmentIndex;
                jointIndex = other.jointIndex;
            }

            public void ReadFromFile(MemoryStream reader, bool isBigEndian)
            {
                attachmentIndex = reader.ReadInt32(isBigEndian);
                jointIndex = reader.ReadByte8();
            }

            public void WriteToFile(BinaryWriter writer)
            {
                writer.Write(attachmentIndex);
                writer.Write(jointIndex);
            }
        }


        public class HitBoxInfo
        {
            uint unk;
            Short3 pos;
            Short3 size;

            public uint Unk {
                get { return unk; }
                set { unk = value; }
            }
            public Short3 Position {
                get { return pos; }
                set { pos = value; }
            }
            public Short3 Size {
                get { return size; }
                set { size = value; }
            }

            public HitBoxInfo(MemoryStream reader, bool isBigEndian)
            {
                ReadFromFile(reader, isBigEndian);
            }

            public HitBoxInfo(HitBoxInfo other)
            {
                unk = other.unk;
                pos = new Short3(other.pos);
                size = new Short3(other.size);
            }

            public void ReadFromFile(MemoryStream reader, bool isBigEndian)
            {
                unk = reader.ReadUInt32(isBigEndian);
                pos = new Short3(reader, isBigEndian);
                size = new Short3(reader, isBigEndian);
            }

            public void WriteToFile(BinaryWriter writer)
            {
                writer.Write(unk);
                pos.WriteToFile(writer);
                size.WriteToFile(writer);
            }
        }
        public class WeightedByMeshSplit
        {
            ushort blendIndex;
            BlendMeshSplitInfo[] data;
            SkeletonBoneIDs jointName;

            public ushort BlendIndex {
                get { return blendIndex; }
                set { blendIndex = value; }
            }
            public BlendMeshSplitInfo[] Data {
                get { return data; }
                set { data = value; }
            }
            public SkeletonBoneIDs JointName {
                get { return jointName; }
                set { jointName = value; }
            }

            public WeightedByMeshSplit(MemoryStream reader, bool isBigEndian)
            {
                ReadFromFile(reader, isBigEndian);
            }

            public WeightedByMeshSplit(WeightedByMeshSplit other)
            {
                blendIndex = other.blendIndex;
                data = new BlendMeshSplitInfo[other.data.Length];
                for (int i = 0; i != other.data.Length; i++)
                    data[i] = other.data[i];
                jointName = other.jointName;
            }

            public void ReadFromFile(MemoryStream reader, bool isBigEndian)
            {
                blendIndex = reader.ReadUInt16(isBigEndian);

                ushort num = reader.ReadUInt16(isBigEndian);
                data = new BlendMeshSplitInfo[num];

                for (int i = 0; i != num; i++)
                    data[i] = new BlendMeshSplitInfo(reader, isBigEndian);
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

        public class BlendMeshSplitInfo
        {
            MiniMaterialBurst[] data;

            public MiniMaterialBurst[] Data {
                get { return data; }
                set { data = value; }
            }

            public BlendMeshSplitInfo(MemoryStream reader, bool isBigEndian)
            {
                ReadFromFile(reader, isBigEndian);
            }

            public BlendMeshSplitInfo(BlendMeshSplitInfo other)
            {
                data = new MiniMaterialBurst[other.data.Length];
                for (int i = 0; i != other.data.Length; i++)
                    data[i] = other.data[i];
            }

            public void ReadFromFile(MemoryStream reader, bool isBigEndian)
            {
                short num = reader.ReadInt16(isBigEndian);
                data = new MiniMaterialBurst[num];

                for (int i = 0; i != num; i++)
                    data[i] = new MiniMaterialBurst(reader, isBigEndian);
            }

            public void WriteToFile(BinaryWriter writer)
            {
                writer.Write((short)data.Length);
                for (int i = 0; i != data.Length; i++)
                    data[i].WriteToFile(writer);
            }
        }

        public class MiniMaterialBurst
        {
            ushort materialIndex;
            FacesBurst[] data;

            public ushort MaterialIndex {
                get { return materialIndex; }
                set { materialIndex = value; }
            }
            public FacesBurst[] Data {
                get { return data; }
                set { data = value; }
            }

            public MiniMaterialBurst(MemoryStream reader, bool isBigEndian)
            {
                ReadFromFile(reader, isBigEndian);
            }

            public MiniMaterialBurst(MiniMaterialBurst other)
            {
                materialIndex = other.materialIndex;
                data = new FacesBurst[other.data.Length];
                for (int i = 0; i != other.data.Length; i++)
                    data[i] = other.data[i];
            }

            public void ReadFromFile(MemoryStream reader, bool isBigEndian)
            {
                materialIndex = reader.ReadUInt16(isBigEndian);

                ushort num = reader.ReadUInt16(isBigEndian);
                data = new FacesBurst[num];

                for (int i = 0; i != num; i++)
                    data[i] = new FacesBurst(reader, isBigEndian);
            }

            public void WriteToFile(BinaryWriter writer)
            {
                writer.Write(materialIndex);
                writer.Write((ushort)data.Length);
                for (int i = 0; i != data.Length; i++)
                    data[i].WriteToFile(writer);
            }
        }

        public class FacesBurst
        {
            ushort startIndex;
            ushort numFaces;

            public ushort StartIndex {
                get { return startIndex; }
                set { startIndex = value; }
            }
            public ushort NumFaces {
                get { return numFaces; }
                set { numFaces = value; }
            }

            public FacesBurst(MemoryStream reader, bool isBigEndian)
            {
                ReadFromFile(reader, isBigEndian);
            }

            public FacesBurst(FacesBurst other)
            {
                startIndex = other.startIndex;
                numFaces = other.numFaces;
            }

            public void ReadFromFile(MemoryStream reader, bool isBigEndian)
            {
                startIndex = reader.ReadUInt16(isBigEndian);
                numFaces = reader.ReadUInt16(isBigEndian);
            }
            public void WriteToFile(BinaryWriter writer)
            {
                writer.Write(startIndex);
                writer.Write(numFaces);
            }
        }
    }
}
