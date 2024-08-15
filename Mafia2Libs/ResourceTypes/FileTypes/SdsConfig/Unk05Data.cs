using System.Collections.Generic;
using System.IO;
using Utils.Helpers.Reflection;

namespace ResourceTypes.SDSConfig
{
    [PropertyClassAllowReflection]
    public class Unk05Data
    {
        [PropertyForceAsAttributeAttribute]
        public string Name { get; set; } = "";
        public short Size { get; set; }
        public short Type { get; set; }
        public short Unk03 { get; set; }
        public ushort Unk04 { get; set; }
        public short Unk05 { get; set; }
        public short Unk06 { get; set; }
        public short Unk07 { get; set; }
        [PropertyIgnoreByReflector]
        public List<string> Strings
        {
            get
            {
                List<string> s = new() { Name };
                return s;
            }
        }
        public Unk05Data()
        {

        }

        public Unk05Data(BinaryReader br, SdsConfigFile sdsConfig)
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
            Unk04 = br.ReadUInt16();
            Unk05 = br.ReadInt16();
            Unk06 = br.ReadInt16();
            Unk07 = br.ReadInt16();
        }

        public void Write(BinaryWriter bw, SdsConfigFile sdsConfig)
        {
            bw.Write((short)sdsConfig.StringTable.Offsets[Name]);
            bw.Write(Size);
            bw.Write(Type);
            bw.Write(Unk03);
            bw.Write(Unk04);
            bw.Write(Unk05);
            bw.Write(Unk06);
            bw.Write(Unk07);
        }
    }
}
