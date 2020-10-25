using System;
using System.IO;
using Utils.Extensions;

namespace ResourceTypes.Cutscene.KeyParams
{
    public class KeyType_40 : IKeyType
    {
        public int Unk01 { get; set; }
        public ushort Unk02 { get; set; }

        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            Unk01 = stream.ReadInt32(isBigEndian);
            Unk02 = stream.ReadUInt16(isBigEndian);

            if (Unk01 != 0 || Unk02 != 1)
            {
                Console.WriteLine("stop here");
            }
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);
            stream.Write(Unk01, isBigEndian);
            stream.Write(Unk02, isBigEndian);
        }
    }
}
