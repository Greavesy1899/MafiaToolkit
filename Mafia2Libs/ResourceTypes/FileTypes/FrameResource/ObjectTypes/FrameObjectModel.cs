using SharpDX;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Utils.Extensions;
using Utils.SharpDXExtensions;
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
        Matrix[] restTransform;
        Matrix unkTransform;
        AttachmentReference[] attachmentReferences;
        uint unkFlags;
        int physSplitSize;
        int hitBoxSize;
        short nPhysSplits;
        WeightedByMeshSplit[] blendMeshSplits;
        HitBoxInfo[] hitBoxInfo;

        [ReadOnly(true)]
        public int BlendInfoIndex {
            get { return blendInfoIndex; }
            set { blendInfoIndex = value; }
        }
        [ReadOnly(true)]
        public int SkeletonIndex {
            get { return skeletonIndex; }
            set { skeletonIndex = value; }
        }
        [ReadOnly(true)]
        public int SkeletonHierachyIndex {
            get { return skeletonHierachyIndex; }
            set { skeletonHierachyIndex = value; }
        }
        public WeightedByMeshSplit[] BlendMeshSplits {
            get { return blendMeshSplits; }
            set { blendMeshSplits = value; }
        }
        public Matrix[] RestTransform {
            get { return restTransform; }
            set { restTransform = value; }
        }
        public Matrix UnkTransform {
            get { return unkTransform; }
            set { unkTransform = value; }
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
        [Category("Linked Blocks")]
        public FrameSkeleton Skeleton {
            get { return skeleton; }
            set { skeleton = value; }
        }
        [Category("Linked Blocks")]
        public FrameBlendInfo BlendInfo {
            get { return blendInfo; }
            set { blendInfo = value; }
        }

        [Category("Linked Blocks"), TypeConverter(typeof(ExpandableObjectConverter))]
        public FrameSkeletonHierachy SkeletonHierarchy {
            get { return hierachy; }
            set { hierachy = value; }
        }

        public FrameObjectModel() : base()
        {


        }
        public FrameObjectModel (MemoryStream reader, bool isBigEndian)
        {
        }

        public FrameObjectModel(FrameObjectSingleMesh other) : base(other)
        {
            restTransform = new Matrix[0];
            attachmentReferences = new AttachmentReference[0];
            blendMeshSplits = new WeightedByMeshSplit[0];
            hitBoxInfo = new HitBoxInfo[0];
        }

        public FrameObjectModel(FrameObjectModel other) : base(other)
        {
            blendInfoIndex = other.blendInfoIndex;
            skeletonIndex = other.skeletonIndex;
            skeletonHierachyIndex = other.skeletonHierachyIndex;
            skeleton = other.skeleton;
            blendInfo = other.blendInfo;

            restTransform = new Matrix[skeleton.NumBones[0]];
            for (int i = 0; i != restTransform.Length; i++)
            {
                restTransform[i] = new Matrix(other.restTransform[i].ToArray());
            }

            unkTransform = new Matrix(other.unkTransform.ToArray());

            attachmentReferences = new AttachmentReference[other.attachmentReferences.Length];
            for (int i = 0; i != attachmentReferences.Length; i++)
            {
                attachmentReferences[i] = new AttachmentReference(other.attachmentReferences[i]);
            }

            unkFlags = other.unkFlags;
            physSplitSize = other.physSplitSize;
            hitBoxSize = other.hitBoxSize;
            nPhysSplits = other.nPhysSplits;

            blendMeshSplits = new WeightedByMeshSplit[nPhysSplits];
            for (int i = 0; i != blendMeshSplits.Length; i++)
            {
                blendMeshSplits[i] = new WeightedByMeshSplit(other.blendMeshSplits[i]);
            }

            hitBoxInfo = new HitBoxInfo[other.hitBoxInfo.Length];
            for (int i = 0; i != hitBoxInfo.Length; i++)
            {
                hitBoxInfo[i] = new HitBoxInfo(hitBoxInfo[i]);
            }
        }

        public override void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            base.ReadFromFile(reader, isBigEndian);

            blendInfoIndex = reader.ReadInt32(isBigEndian);
            skeletonIndex = reader.ReadInt32(isBigEndian);
            skeletonHierachyIndex = reader.ReadInt32(isBigEndian);
        }

        public void ReadFromFilePart2(MemoryStream stream, bool isBigEndian)
        {
            //do rest matrices.
            restTransform = new Matrix[skeleton.NumBones[0]];
            for (int i = 0; i < restTransform.Length; i++)
            {
                restTransform[i] = MatrixExtensions.ReadFromFile(stream, isBigEndian);
            }

            //unknown transform.
            unkTransform = MatrixExtensions.ReadFromFile(stream, isBigEndian);

            //attachments.
            int length1 = stream.ReadInt32(isBigEndian);
            attachmentReferences = new AttachmentReference[length1];
            for (int i = 0; i < length1; i++)
            {
                attachmentReferences[i] = new AttachmentReference(stream, isBigEndian);
                attachmentReferences[i].JointName = skeleton.BoneNames[attachmentReferences[i].JointIndex].ToString();
            }

            //unknwon.
            unkFlags = stream.ReadUInt32(isBigEndian);
            physSplitSize = stream.ReadInt32(isBigEndian);
            hitBoxSize = stream.ReadInt32(isBigEndian);

            nPhysSplits = (physSplitSize > 0) ? stream.ReadInt16(isBigEndian) : (short)0;

            int totalSplits = 0;
            blendMeshSplits = new WeightedByMeshSplit[nPhysSplits];
            for (int i = 0; i != nPhysSplits; i++)
            {
                blendMeshSplits[i] = new WeightedByMeshSplit(stream, isBigEndian);
                totalSplits += blendMeshSplits[i].Data.Length;
            }

            hitBoxInfo = new HitBoxInfo[totalSplits];
            for (int i = 0; i != hitBoxInfo.Length; i++)
            {
                hitBoxInfo[i] = new HitBoxInfo(stream, isBigEndian);
            }
        }

        public override void WriteToFile(BinaryWriter writer)
        {
            // Part 1 include base class WriteToFile call.
            WriteToFilePart1(writer);
            WriteToFilePart2(writer);
        }

        public void WriteToFilePart1(BinaryWriter writer)
        {
            base.WriteToFile(writer);

            writer.Write(blendInfoIndex);
            writer.Write(skeletonIndex);
            writer.Write(skeletonHierachyIndex);
        }

        public void WriteToFilePart2(BinaryWriter writer)
        {
            //do rest matrices.
            for (int i = 0; i < restTransform.Length; i++)
            {
                MatrixExtensions.WriteToFile(restTransform[i], writer);
            }

            //unknown transform.
            MatrixExtensions.WriteToFile(unkTransform, writer);

            //attachments.
            writer.Write(attachmentReferences.Length);
            for (int i = 0; i < attachmentReferences.Length; i++)
            {
                attachmentReferences[i].WriteToFile(writer);
            }

            //unknwon.
            writer.Write(unkFlags);
            writer.Write(physSplitSize);
            writer.Write(hitBoxSize);

            if (physSplitSize > 0)
            {
                writer.Write(nPhysSplits);
            }

            for (int i = 0; i < nPhysSplits; i++)
            {
                blendMeshSplits[i].WriteToFile(writer);
            }

            for (int i = 0; i < hitBoxInfo.Length; i++)
            {
                hitBoxInfo[i].WriteToFile(writer);
            }
        }

        public override string ToString()
        {
            return string.Format("{0}", Name.ToString());
        }

        public class AttachmentReference
        {
            int attachmentIndex;
            byte jointIndex;

            //not saved
            string jointName;
            FrameObjectBase attachment;

            public int AttachmentIndex {
                get { return attachmentIndex; }
                set { attachmentIndex = value; }
            }
            public byte JointIndex {
                get { return jointIndex; }
                set { jointIndex = value; }
            }
            public string JointName {
                get { return jointName; }
                set { jointName = value; }
            }
            public FrameObjectBase Attachment {
                get { return attachment; }
                set { attachment = value; }
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
            string jointName;

            public ushort BlendIndex {
                get { return blendIndex; }
                set { blendIndex = value; }
            }
            public BlendMeshSplitInfo[] Data {
                get { return data; }
                set { data = value; }
            }
            public string JointName {
                get { return jointName; }
                set { jointName = value; }
            }

            public WeightedByMeshSplit(MemoryStream reader, bool isBigEndian)
            {
                ReadFromFile(reader, isBigEndian);
                jointName = "";
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
                if (!string.IsNullOrEmpty(jointName))
                {
                    return jointName.ToString();
                }

                return "";
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
