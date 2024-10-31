using System.Collections.Generic;
using System.IO;
using Utils.Helpers.Reflection;

namespace ResourceTypes.SDSConfig
{
    [PropertyClassAllowReflection]
    public class Unk03Data
    {
        [PropertyForceAsAttributeAttribute]
        public string Name { get; set; } = "";
        public int Unk01 { get; set; }
        public int Unk02 { get; set; }
        public Unk04Data[] Unk04Data { get; set; } = new Unk04Data[0];
        [PropertyIgnoreByReflector]
        public List<string> Strings
        {
            get
            {
                List<string> s = new() { Name };

                foreach (var data in Unk04Data)
                {
                    s.AddRange(data.Strings);
                }

                return s;
            }
        }
        public Unk03Data()
        {

        }

        public Unk03Data(BinaryReader br, SdsConfigFile sdsConfig)
        {
            Read(br, sdsConfig);
        }

        public void Read(BinaryReader br, SdsConfigFile sdsConfig)
        {
            short StringTableOffset = br.ReadInt16();
            Name = sdsConfig.StringTable.Strings[StringTableOffset];

            Unk01 = br.ReadInt32();
            Unk02 = br.ReadInt32();

            short Count = br.ReadInt16();
            Unk04Data = new Unk04Data[Count];

            for (int i = 0; i < Unk04Data.Length; i++)
            {
                Unk04Data[i] = new(br, sdsConfig);
            }
        }

        public void Write(BinaryWriter bw, SdsConfigFile sdsConfig)
        {
            bw.Write((short)sdsConfig.StringTable.Offsets[Name]);
            bw.Write(Unk01);
            bw.Write(Unk02);
            bw.Write((short)Unk04Data.Length);

            foreach (var val in Unk04Data)
            {
                val.Write(bw, sdsConfig);
            }
        }
    }
}
