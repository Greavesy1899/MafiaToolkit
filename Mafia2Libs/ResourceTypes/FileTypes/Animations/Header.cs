using System;
using System.IO;
using System.Numerics;
using Utils.VorticeUtils;

namespace ResourceTypes.Animation2
{
    public class Header
    {
        public int SkeletonID { get; set; } = -1;
        public byte Version { get; set; } = 2;
        public static readonly uint Magic = 0xFA5612BC;
        public short NumPrimaryEvents { get; set; } = 0;
        public short NumSecondaryEvents { get; set; } = 0;
        public Vector4 Unk01 { get; set; } = new(0,0,0,1);
        public uint Unk02 { get; set; } = 0;
        public uint Unk03 { get; set; } = 0;
        public uint Unk04 { get; set; } = 0;
        public uint Unk05 { get; set; } = 0xFF7FFF8B;
        public uint Unk06 { get; set; } = 0;
        public uint Unk07 { get; set; } = 0;
        public ulong Hash { get; set; } = 0;
        public byte Unk08 { get; set; } = 0;
        public float Duration { get; set; } = 0.0f;
        public short Count { get; set; } = 0;
        public byte Unk09 { get; set; } = 3;
        public ulong RootBoneID { get; set; } = 0;
        public Header()
        {

        }

        public Header(Stream s)
        {
            Read(s);
        }

        public Header(BinaryReader br)
        {
            Read(br);
        }

        public void Read(Stream s)
        {
            using (BinaryReader br = new(s))
            {
                Read(br);
            }
        }

        public void Read(BinaryReader br)
        {
            SkeletonID = br.ReadInt32();
            Version = br.ReadByte();

            uint _Magic = br.ReadUInt32();
            if (_Magic != Magic)
            {
                throw new Exception("Not an Animation2 file.");
            }

            NumPrimaryEvents = br.ReadInt16();
            NumSecondaryEvents = br.ReadInt16();
            Unk01 = Vector4Extenders.ReadFromFile(br);
            Unk02 = br.ReadUInt32();
            Unk03 = br.ReadUInt32();
            Unk04 = br.ReadUInt32();
            Unk05 = br.ReadUInt32();
            Unk06 = br.ReadUInt32();
            Unk07 = br.ReadUInt32();
            Hash = br.ReadUInt64();
            Unk08 = br.ReadByte();
            Duration = br.ReadSingle();
            Count = br.ReadInt16();
            Unk09 = br.ReadByte();
            RootBoneID = br.ReadUInt64();
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(SkeletonID);
            bw.Write(Version);
            bw.Write(Magic);
            bw.Write(NumPrimaryEvents);
            bw.Write(NumSecondaryEvents);
            Unk01.WriteToFile(bw);
            bw.Write(Unk02);
            bw.Write(Unk03);
            bw.Write(Unk04);
            bw.Write(Unk05);
            bw.Write(Unk06);
            bw.Write(Unk07);
            bw.Write(Hash);
            bw.Write(Unk08);
            bw.Write(Duration);
            bw.Write(Count);
            bw.Write(Unk09);
            bw.Write(RootBoneID);
        }
    }
}
