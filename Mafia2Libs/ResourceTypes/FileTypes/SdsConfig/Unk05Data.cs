using System.Collections.Generic;
using System.IO;
using Utils.Helpers.Reflection;

namespace ResourceTypes.SDSConfig
{
    /// <summary>
    /// 16-byte SDS reference structure within SDSItem
    /// Based on reverse engineering of C_SlotManager::S_ConfigSDS::Open
    /// </summary>
    [PropertyClassAllowReflection]
    public class Unk05Data
    {
        [PropertyForceAsAttributeAttribute]
        public string Name { get; set; } = "";
        /// <summary>Instance count/multiplier</summary>
        public ushort Count { get; set; }
        /// <summary>Streaming type category</summary>
        public ushort TypeId { get; set; }
        /// <summary>Loading priority (higher = more important)</summary>
        public ushort Priority { get; set; }
        /// <summary>Memory budget in bytes</summary>
        public uint MemorySize { get; set; }
        /// <summary>Auxiliary/additional size in bytes</summary>
        public uint AuxSize { get; set; }
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

            Count = br.ReadUInt16();
            TypeId = br.ReadUInt16();
            Priority = br.ReadUInt16();
            MemorySize = br.ReadUInt32();
            AuxSize = br.ReadUInt32();
        }

        public void Write(BinaryWriter bw, SdsConfigFile sdsConfig)
        {
            bw.Write((short)sdsConfig.StringTable.Offsets[Name]);
            bw.Write(Count);
            bw.Write(TypeId);
            bw.Write(Priority);
            bw.Write(MemorySize);
            bw.Write(AuxSize);
        }
    }
}
