using System;
using System.IO;

namespace ResourceTypes.Cutscene.KeyParams
{
    public class KeyType_40 : IKeyType
    {
        public int Unk01 { get; set; }
        public ushort Unk02 { get; set; }

        public override void ReadFromFile(BinaryReader br)
        {
            base.ReadFromFile(br);
            Unk01 = br.ReadInt32();
            Unk02 = br.ReadUInt16();

            if (Unk01 != 0 || Unk02 != 1)
            {
                Console.WriteLine("stop here");
            }
        }

        public override void WriteToFile(BinaryWriter bw)
        {
            base.WriteToFile(bw);
            bw.Write(Unk01);
            bw.Write(Unk02);
        }

        public override string ToString()
        {
            return "Type: 40";
        }
    }
}
