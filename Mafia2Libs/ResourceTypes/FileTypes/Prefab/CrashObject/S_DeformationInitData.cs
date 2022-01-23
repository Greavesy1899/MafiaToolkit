using BitStreams;
using System;
using System.Diagnostics;

namespace ResourceTypes.Prefab.CrashObject
{
    public class S_DeformationInitData : S_GlobalInitData
    {
        public ulong HashCRC { get; set; }
        public ulong ScaleBoneFrameName { get; set; }
        public ulong RootFrameName { get; set; }
        public S_InitDeformPart[] DeformParts { get; set; }
        public S_InitJoint[] InitJoints { get; set; }
        public ulong[] Unk1_Hashes { get; set; }
        public ushort[] Unk1_Indexes { get; set; }
        public S_InitOwnerDeform[] OwnerDeforms { get; set; }
        public byte Unk2 { get; set; }
        public byte Unk3 { get; set; }

        public S_DeformationInitData() : base()
        {
            DeformParts = new S_InitDeformPart[0];
            InitJoints = new S_InitJoint[0];
            Unk1_Hashes = new ulong[0];
            Unk1_Indexes = new ushort[0];
            OwnerDeforms = new S_InitOwnerDeform[0];
        }

        public override void Load(BitStream MemStream)
        {
            base.Load(MemStream);

            HashCRC = MemStream.ReadUInt64();
            ScaleBoneFrameName = MemStream.ReadUInt64();
            RootFrameName = MemStream.ReadUInt64();

            uint NumDeformParts = MemStream.ReadUInt32();
            DeformParts = new S_InitDeformPart[NumDeformParts];
            for (int i = 0; i < NumDeformParts; i++)
            {
                S_InitDeformPart DeformPart = new S_InitDeformPart();
                DeformPart.Load(MemStream);
                DeformParts[i] = DeformPart;
            }

            uint NumJoints = MemStream.ReadUInt32();
            InitJoints = new S_InitJoint[NumJoints];
            for (int i = 0; i < InitJoints.Length; i++)
            {
                S_InitJoint NewJoint = new S_InitJoint();
                NewJoint.Load(MemStream);
                InitJoints[i] = NewJoint;
            }

            // NB: Could be S_InitOwnerGroup?
            uint NumHashes = MemStream.ReadUInt32();
            Unk1_Hashes = new ulong[NumHashes];
            Unk1_Indexes = new ushort[NumHashes];
            for (int i = 0; i < Unk1_Hashes.Length; i++)
            {
                Unk1_Hashes[i] = MemStream.ReadUInt64();
                Unk1_Indexes[i] = MemStream.ReadUInt16();
            }

            uint NumOwnerDeforms = MemStream.ReadUInt32();
            OwnerDeforms = new S_InitOwnerDeform[NumOwnerDeforms];
            for (int i = 0; i < OwnerDeforms.Length; i++)
            {
                S_InitOwnerDeform OwnerDeform = new S_InitOwnerDeform();
                OwnerDeform.Load(MemStream);
                OwnerDeforms[i] = OwnerDeform;
            }

            Unk2 = MemStream.ReadBit();
            Unk3 = MemStream.ReadBit();
        }

        public override void Save(BitStream MemStream)
        {
            base.Save(MemStream);

            MemStream.WriteUInt64(HashCRC);
            MemStream.WriteUInt64(ScaleBoneFrameName);
            MemStream.WriteUInt64(RootFrameName);

            MemStream.WriteUInt32((uint)DeformParts.Length);
            foreach(S_InitDeformPart DeformPart in DeformParts)
            {
                DeformPart.Save(MemStream);
            }

            MemStream.WriteUInt32((uint)InitJoints.Length);
            foreach (S_InitJoint InitJoint in InitJoints)
            {
                InitJoint.Save(MemStream);
            }

            // NB: Could be S_InitOwnerGroup?
            MemStream.WriteUInt32((uint)Unk1_Hashes.Length);
            for (int i = 0; i < Unk1_Hashes.Length; i++)
            {
                MemStream.WriteUInt64(Unk1_Hashes[i]);
                MemStream.WriteUInt16(Unk1_Indexes[i]);
            }

            MemStream.WriteUInt32((uint)OwnerDeforms.Length);
            foreach (S_InitOwnerDeform OwnerDeform in OwnerDeforms)
            {
                OwnerDeform.Save(MemStream);
            }

            MemStream.WriteBit(Unk2);
            MemStream.WriteBit(Unk3);
        }
    }
}
