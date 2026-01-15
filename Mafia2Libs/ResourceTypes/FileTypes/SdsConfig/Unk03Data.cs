using System.Collections.Generic;
using System.IO;
using Utils.Helpers.Reflection;

namespace ResourceTypes.SDSConfig
{
    /// <summary>
    /// S_VirtualSlot structure - container for SDS configuration items
    /// Based on reverse engineering of C_SlotManager::S_ConfigSDS::Open
    /// </summary>
    [PropertyClassAllowReflection]
    public class Unk03Data
    {
        [PropertyForceAsAttributeAttribute]
        public string Name { get; set; } = "";
        /// <summary>Memory budget (primary) in bytes</summary>
        public uint MemoryBudget1 { get; set; }
        /// <summary>Memory budget (secondary) in bytes</summary>
        public uint MemoryBudget2 { get; set; }
        /// <summary>SDS configuration items within this virtual slot</summary>
        public Unk04Data[] SDSItems { get; set; } = new Unk04Data[0];
        [PropertyIgnoreByReflector]
        public List<string> Strings
        {
            get
            {
                List<string> s = new() { Name };

                foreach (var data in SDSItems)
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

            MemoryBudget1 = br.ReadUInt32();
            MemoryBudget2 = br.ReadUInt32();

            short Count = br.ReadInt16();
            SDSItems = new Unk04Data[Count];

            for (int i = 0; i < SDSItems.Length; i++)
            {
                SDSItems[i] = new(br, sdsConfig);
            }
        }

        public void Write(BinaryWriter bw, SdsConfigFile sdsConfig)
        {
            bw.Write((short)sdsConfig.StringTable.Offsets[Name]);
            bw.Write(MemoryBudget1);
            bw.Write(MemoryBudget2);
            bw.Write((short)SDSItems.Length);

            foreach (var val in SDSItems)
            {
                val.Write(bw, sdsConfig);
            }
        }
    }
}
