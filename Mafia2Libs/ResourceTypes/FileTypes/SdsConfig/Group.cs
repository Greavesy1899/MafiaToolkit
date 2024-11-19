using System.Collections.Generic;
using System.IO;
using Utils.Helpers.Reflection;

namespace ResourceTypes.SDSConfig
{
    [PropertyClassAllowReflection]
    public class Group
    {
        [PropertyForceAsAttributeAttribute]
        public string Name { get; set; } = "";
        public short Size { get; set; }
        public short Type { get; set; }
        public short Unk03 { get; set; }
        public int Unk04 { get; set; }
        public int Unk05 { get; set; }
        [PropertyIgnoreByReflector]
        public List<string> Strings
        {
            get
            {
                List<string> s = new() { Name };
                return s;
            }
        }
        public Group()
        {

        }

        public Group(BinaryReader br, SdsConfigFile sdsConfig)
        {
            Read(br, sdsConfig);
        }

        public void Read(BinaryReader br, SdsConfigFile sdsConfig)
        {
            short StringTableOffset = br.ReadInt16();
            Name = sdsConfig.StringTable.Strings[StringTableOffset];

            Size = br.ReadInt16();
            Type = br.ReadInt16();
            Unk03 = br.ReadInt16();
            Unk04 = br.ReadInt32();
            Unk05 = br.ReadInt32();
        }

        public void Write(BinaryWriter bw, SdsConfigFile sdsConfig)
        {
            bw.Write((short)sdsConfig.StringTable.Offsets[Name]);
            bw.Write(Size);
            bw.Write(Type);
            bw.Write(Unk03);
            bw.Write(Unk04);
            bw.Write(Unk05);
        }
    }
}
