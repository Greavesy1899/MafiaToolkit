using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceTypes.PCKG
{
    public class PackageEntry
    {
        public ulong ID { get; set; }
        public long Offset { get; set; } = 16;
        public int UncompressedSize { get; set; }
        public int CompressedSize { get; set; }
        public ushort Height { get; set; }
        public ushort Width { get; set; }
        public int Const0 = 0;
        public byte[] Data { get; set; }
        public string Name
        {
            get
            {
                return ID.ToString("X") + ".dds";
            }
        }
        public PackageEntry()
        {

        }

        public PackageEntry(BinaryReader br)
        {
            Read(br);
        }

        public void Read(BinaryReader br)
        {
            ID = br.ReadUInt64();
            Offset = br.ReadInt64();
            UncompressedSize = br.ReadInt32();
            CompressedSize = br.ReadInt32();
            Height = br.ReadUInt16();
            Width = br.ReadUInt16();
            Const0 = br.ReadInt32();
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(ID);
            bw.Write(Offset);
            bw.Write(UncompressedSize);
            bw.Write(CompressedSize);
            bw.Write(Height);
            bw.Write(Width);
            bw.Write(Const0);
        }
    }
}
