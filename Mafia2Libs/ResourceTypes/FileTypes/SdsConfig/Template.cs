using System.Collections.Generic;
using System.IO;
using Utils.Helpers.Reflection;

namespace ResourceTypes.SDSConfig
{
    [PropertyClassAllowReflection]
    public class Template
    {
        [PropertyForceAsAttributeAttribute]
        public string Name { get; set; } = "";
        public Group[] Unk02Data { get; set; } = new Group[0];
        public Unk03Data[] Unk03Data { get; set; } = new Unk03Data[0];
        [PropertyIgnoreByReflector]
        public List<string> Strings
        {
            get
            {
                List<string> s = new() { Name };

                foreach (var data in Unk02Data)
                {
                    s.AddRange(data.Strings);
                }

                foreach (var data in Unk03Data)
                {
                    s.AddRange(data.Strings);
                }

                return s;
            }
        }
        public Template()
        {

        }

        public Template(BinaryReader br, SdsConfigFile sdsConfig)
        {
            Read(br, sdsConfig);
        }

        public void Read(BinaryReader br, SdsConfigFile sdsConfig)
        {
            short StringTableOffset = br.ReadInt16();
            Name = sdsConfig.StringTable.Strings[StringTableOffset];

            short Count = br.ReadInt16();
            Unk02Data = new Group[Count];

            for (int i = 0; i < Unk02Data.Length; i++)
            {
                Unk02Data[i] = new(br, sdsConfig);
            }

            Count = br.ReadInt16();
            Unk03Data = new Unk03Data[Count];

            for (int i = 0; i < Unk03Data.Length; i++)
            {
                Unk03Data[i] = new(br, sdsConfig);
            }
        }

        public void Write(BinaryWriter bw, SdsConfigFile sdsConfig)
        {
            bw.Write((short)sdsConfig.StringTable.Offsets[Name]);
            bw.Write((short)Unk02Data.Length);

            foreach (var val in Unk02Data)
            {
                val.Write(bw, sdsConfig);
            }

            bw.Write((short)Unk03Data.Length);

            foreach (var val in Unk03Data)
            {
                val.Write(bw, sdsConfig);
            }
        }
    }
}
