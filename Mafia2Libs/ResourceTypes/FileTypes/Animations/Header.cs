using System;
using System.IO;
using System.Numerics;
using Utils.VorticeUtils;

namespace ResourceTypes.Animation2
{
    public class Header
    {
        public int SkeletonID { get; set; }
        public byte Version { get; set; }
        public static readonly uint Magic = 0xFA5612BC;
        public short NumEvents { get; set; }
        public ushort Unk00 { get; set; }
        public Vector4 Unk01 { get; set; } = new(0,0,0,1);
        public uint Unk02 { get; set; }
        public uint Unk03 { get; set; }
        public uint Unk04 { get; set; }
        public uint Unk05 { get; set; }
        public uint Unk06 { get; set; }
        public uint Unk07 { get; set; }
        public ulong Hash { get; set; }
        public byte Unk08 { get; set; }
        public float Duration { get; set; }
        public short Count { get; set; }
        public byte Unk09 { get; set; }
        public ulong RootBoneID { get; set; }
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

            NumEvents = br.ReadInt16();
            Unk00 = br.ReadUInt16();
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
    }
}
