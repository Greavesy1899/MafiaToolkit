using System.IO;
using Utils.Models;
using Utils.Types;

namespace ResourceTypes.FrameResource
{
    public class FrameObjectModel : FrameObjectSingleMesh
    {
        private FrameSkeleton skeleton;
        private FrameBlendInfo blendInfo;

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

        public FrameObjectModel (BinaryReader reader)
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

        public override void ReadFromFile(BinaryReader reader)
        {
            base.ReadFromFile(reader);

            blendInfoIndex = reader.ReadInt32();
            skeletonIndex = reader.ReadInt32();
            skeletonHierachyIndex = reader.ReadInt32();
        }

        public void ReadFromFilePart2(BinaryReader reader, FrameSkeleton skeleton, FrameBlendInfo blendInfo)
        {
            this.skeleton = skeleton;
            this.blendInfo = blendInfo;

            //do rest matrices.
            restPose = new TransformMatrix[skeleton.NumBones[0]];
            for (int i = 0; i != restPose.Length; i++)
                restPose[i] = new TransformMatrix(reader);

            //unknown transform.
            unkTrasform = new TransformMatrix(reader);

            //attachments.
            int length1 = reader.ReadInt32();
            attachmentReferences = new AttachmentReference[length1];

            for (int i = 0; i != length1; i++)
                attachmentReferences[i] = new AttachmentReference(reader);

            //unknwon.
            unkFlags = reader.ReadUInt32();
            physSplitSize = reader.ReadInt32();
            hitBoxSize = reader.ReadInt32();

            if (physSplitSize > 0)
                nPhysSplits = reader.ReadInt16();
            else
                nPhysSplits = 0;

            int totalSplits = 0;
            blendMeshSplits = new WeightedByMeshSplit[nPhysSplits];
            for (int i = 0; i != nPhysSplits; i++)
            {
                blendMeshSplits[i] = new WeightedByMeshSplit(reader);
                totalSplits += blendMeshSplits[i].Data.Length;
            }

            hitBoxInfo = new HitBoxInfo[totalSplits];
            for (int i = 0; i != hitBoxInfo.Length; i++)
                hitBoxInfo[i] = new HitBoxInfo(reader);
        }

        public override void WriteToFile(BinaryWriter writer)
        {
            base.WriteToFile(writer);
            writer.Write(blendInfoIndex);
            writer.Write(skeletonIndex);
            writer.Write(skeletonHierachyIndex);

            //do rest matrices.
            for (int i = 0; i != restPose.Length; i++)
                restPose[i].WriteToFrame(writer);

            //unknown transform.
            unkTrasform.WriteToFrame(writer);

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

            public AttachmentReference(BinaryReader reader)
            {
                ReadFromFile(reader);
            }

            public AttachmentReference(AttachmentReference other)
            {
                attachmentIndex = other.attachmentIndex;
                jointIndex = other.jointIndex;
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

            public HitBoxInfo(BinaryReader reader)
            {
                ReadFromFile(reader);
            }

            public HitBoxInfo(HitBoxInfo other)
            {
                unk = other.unk;
                pos = new Short3(other.pos);
                size = new Short3(other.size);
            }

            public void ReadFromFile(BinaryReader reader)
            {
                unk = reader.ReadUInt32();
                pos = new Short3(reader);
                size = new Short3(reader);
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

            public WeightedByMeshSplit(BinaryReader reader)
            {
                ReadFromFile(reader);
            }

            public WeightedByMeshSplit(WeightedByMeshSplit other)
            {
                blendIndex = other.blendIndex;
                data = new BlendMeshSplitInfo[other.data.Length];
                for (int i = 0; i != other.data.Length; i++)
                    data[i] = other.data[i];
                jointName = other.jointName;
            }

            public void ReadFromFile(BinaryReader reader)
            {
                blendIndex = reader.ReadUInt16();

                ushort num = reader.ReadUInt16();
                data = new BlendMeshSplitInfo[num];

                for (int i = 0; i != num; i++)
                    data[i] = new BlendMeshSplitInfo(reader);
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

            public BlendMeshSplitInfo(BinaryReader reader)
            {
                ReadFromFile(reader);
            }

            public BlendMeshSplitInfo(BlendMeshSplitInfo other)
            {
                data = new MiniMaterialBurst[other.data.Length];
                for (int i = 0; i != other.data.Length; i++)
                    data[i] = other.data[i];
            }

            public void ReadFromFile(BinaryReader reader)
            {
                short num = reader.ReadInt16();
                data = new MiniMaterialBurst[num];

                for (int i = 0; i != num; i++)
                    data[i] = new MiniMaterialBurst(reader);
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

            public MiniMaterialBurst(BinaryReader reader)
            {
                ReadFromFile(reader);
            }

            public MiniMaterialBurst(MiniMaterialBurst other)
            {
                materialIndex = other.materialIndex;
                data = new FacesBurst[other.data.Length];
                for (int i = 0; i != other.data.Length; i++)
                    data[i] = other.data[i];
            }

            public void ReadFromFile(BinaryReader reader)
            {
                materialIndex = reader.ReadUInt16();

                ushort num = reader.ReadUInt16();
                data = new FacesBurst[num];

                for (int i = 0; i != num; i++)
                    data[i] = new FacesBurst(reader);
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

            public FacesBurst(BinaryReader reader)
            {
                ReadFromFile(reader);
            }

            public FacesBurst(FacesBurst other)
            {
                startIndex = other.startIndex;
                numFaces = other.numFaces;
            }

            public void ReadFromFile(BinaryReader reader)
            {
                startIndex = reader.ReadUInt16();
                numFaces = reader.ReadUInt16();
            }
            public void WriteToFile(BinaryWriter writer)
            {
                writer.Write(startIndex);
                writer.Write(numFaces);
            }
        }
    }
}
