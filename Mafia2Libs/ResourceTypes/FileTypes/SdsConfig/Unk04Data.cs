using System.Collections.Generic;
using System.IO;
using Utils.Helpers.Reflection;

namespace ResourceTypes.SDSConfig
{
    [PropertyClassAllowReflection]
    public class Unk04Data
    {
        [PropertyForceAsAttributeAttribute]
        public string Name { get; set; } = "";
        public int Unk01 { get; set; }
        public int Unk02 { get; set; }
        public byte Unk03 { get; set; }
        public Unk05Data[] Unk05Data { get; set; } = new Unk05Data[0];
        [PropertyIgnoreByReflector]
        public List<string> Strings
        {
            get
            {
                List<string> s = new() { Name };

                foreach (var data in Unk05Data)
                {
                    s.AddRange(data.Strings);
                }

                return s;
            }
        }
        public Unk04Data()
        {

        }

        public Unk04Data(BinaryReader br, SdsConfigFile sdsConfig)
        {
            Read(br, sdsConfig);
        }

        public void Read(BinaryReader br, SdsConfigFile sdsConfig)
        {
            short StringTableOffset = br.ReadInt16();
            Name = sdsConfig.StringTable.Strings[StringTableOffset];

            Unk01 = br.ReadInt32();
            Unk02 = br.ReadInt32();
            Unk03 = br.ReadByte();

            short Count = br.ReadInt16();
            Unk05Data = new Unk05Data[Count];

            for (int i = 0; i < Unk05Data.Length; i++)
            {
                Unk05Data[i] = new(br, sdsConfig);
            }
        }

        public void Write(BinaryWriter bw, SdsConfigFile sdsConfig)
        {
            bw.Write((short)sdsConfig.StringTable.Offsets[Name]);
            bw.Write(Unk01);
            bw.Write(Unk02);
            bw.Write(Unk03);
            bw.Write((short)Unk05Data.Length);

            foreach (var val in Unk05Data)
            {
                val.Write(bw, sdsConfig);
            }
        }
    }
}
