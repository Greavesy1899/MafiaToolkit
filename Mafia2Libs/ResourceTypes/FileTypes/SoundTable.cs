using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Extensions;

namespace ResourceTypes.FileTypes.SoundTable
{
    public class Entry0
    {
        public byte Unk0 { get; set; }
        public byte Unk1 { get; set; }
        public float[] Unk3 { get; set; }

        public void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            Unk0 = reader.ReadByte8();
            Unk1 = reader.ReadByte8();

            Unk3 = new float[(Unk1 * 8) / 2];
            for(int i = 0; i < Unk3.Length; i++)
            {
                Unk3[i] = reader.ReadSingle(isBigEndian);
            }
        }
    }

    public class Entry1
    {
        public byte Unk0 { get; set; }
        public float[] Unk1 { get; set; }

    }

    public class SoundTable
    {
        public void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            uint NumEntries = reader.ReadUInt32(isBigEndian);
            for(int i = 0; i < NumEntries; i++)
            {
                Entry0 NewEntry = new Entry0();
                NewEntry.ReadFromFile(reader, isBigEndian);
            }

            NumEntries = reader.ReadUInt32(isBigEndian);

        }

        public void WriteToFile(MemoryStream writer, bool isBigEndian)
        {

        }
    }
}
