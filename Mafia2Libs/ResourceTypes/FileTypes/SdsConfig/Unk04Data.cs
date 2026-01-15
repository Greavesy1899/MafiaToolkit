using System.Collections.Generic;
using System.IO;
using Utils.Helpers.Reflection;

namespace ResourceTypes.SDSConfig
{
    /// <summary>
    /// SDS configuration item within a virtual slot
    /// Based on reverse engineering of C_SlotManager::S_ConfigSDS::Open
    /// </summary>
    [PropertyClassAllowReflection]
    public class Unk04Data
    {
        [PropertyForceAsAttributeAttribute]
        public string Name { get; set; } = "";
        /// <summary>Total memory size (primary) in bytes</summary>
        public uint TotalSize1 { get; set; }
        /// <summary>Total memory size (secondary) in bytes</summary>
        public uint TotalSize2 { get; set; }
        /// <summary>Flags (0x01 = default configuration)</summary>
        public byte Flags { get; set; }
        /// <summary>SDS file references for this configuration item</summary>
        public Unk05Data[] SDSReferences { get; set; } = new Unk05Data[0];

        /// <summary>Returns true if this is the default configuration</summary>
        [PropertyIgnoreByReflector]
        public bool IsDefault => Flags == 0x01;

        [PropertyIgnoreByReflector]
        public List<string> Strings
        {
            get
            {
                List<string> s = new() { Name };

                foreach (var data in SDSReferences)
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

            TotalSize1 = br.ReadUInt32();
            TotalSize2 = br.ReadUInt32();
            Flags = br.ReadByte();

            short Count = br.ReadInt16();
            SDSReferences = new Unk05Data[Count];

            for (int i = 0; i < SDSReferences.Length; i++)
            {
                SDSReferences[i] = new(br, sdsConfig);
            }
        }

        public void Write(BinaryWriter bw, SdsConfigFile sdsConfig)
        {
            bw.Write((short)sdsConfig.StringTable.Offsets[Name]);
            bw.Write(TotalSize1);
            bw.Write(TotalSize2);
            bw.Write(Flags);
            bw.Write((short)SDSReferences.Length);

            foreach (var val in SDSReferences)
            {
                val.Write(bw, sdsConfig);
            }
        }
    }
}
