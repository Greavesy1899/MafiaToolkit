using System.Collections.Generic;
using System.IO;
using Utils.Helpers.Reflection;

namespace ResourceTypes.SDSConfig
{
    /// <summary>
    /// S_SetSDS structure - top-level streaming configuration entry
    /// Based on reverse engineering of C_SlotManager::S_ConfigSDS::Open
    /// </summary>
    [PropertyClassAllowReflection]
    public class Template
    {
        [PropertyForceAsAttributeAttribute]
        public string Name { get; set; } = "";
        /// <summary>Base SDS file references for this set</summary>
        public Group[] BaseSDSReferences { get; set; } = new Group[0];
        /// <summary>Virtual slots containing configuration variants</summary>
        public Unk03Data[] VirtualSlots { get; set; } = new Unk03Data[0];
        [PropertyIgnoreByReflector]
        public List<string> Strings
        {
            get
            {
                List<string> s = new() { Name };

                foreach (var data in BaseSDSReferences)
                {
                    s.AddRange(data.Strings);
                }

                foreach (var data in VirtualSlots)
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
            BaseSDSReferences = new Group[Count];

            for (int i = 0; i < BaseSDSReferences.Length; i++)
            {
                BaseSDSReferences[i] = new(br, sdsConfig);
            }

            Count = br.ReadInt16();
            VirtualSlots = new Unk03Data[Count];

            for (int i = 0; i < VirtualSlots.Length; i++)
            {
                VirtualSlots[i] = new(br, sdsConfig);
            }
        }

        public void Write(BinaryWriter bw, SdsConfigFile sdsConfig)
        {
            bw.Write((short)sdsConfig.StringTable.Offsets[Name]);
            bw.Write((short)BaseSDSReferences.Length);

            foreach (var val in BaseSDSReferences)
            {
                val.Write(bw, sdsConfig);
            }

            bw.Write((short)VirtualSlots.Length);

            foreach (var val in VirtualSlots)
            {
                val.Write(bw, sdsConfig);
            }
        }
    }
}
