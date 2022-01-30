using BitStreams;
using System;
using System.ComponentModel;
using System.Diagnostics;
using Utils.Helpers.Reflection;

namespace ResourceTypes.Prefab.CrashObject
{
    public class S_InitDeformPartCommon_Unk4
    {
        public int Unk0 { get; set; }
        public int Unk1 { get; set; }
    }

    [TypeConverter(typeof(ExpandableObjectConverter)), PropertyClassAllowReflection]
    public class S_InitDeformPartCommon
    {
        public float SpeedMin { get; set; } // [Value * 3.6f]
        public float SpeedMax { get; set; } // [Value * 3.6f]
        public float Resistance { get; set; } // [Value * 100.0f]
        public float Mass { get; set; } // [Value]
        public float EnergyStart { get; set; } // [Value / 0.01f]
        public float EnergyDrop { get; set; } // [Value / 0.01f]
        public int[] Unk2 { get; set; } // Dynamic array of floats
        public byte Unk3 { get; set; } // flag to acknowledge extra data
        public C_Transform Unk3_Transform { get; set; }
        public S_InitDeformPartCommon_Unk4[] Unk4 { get; set; }
        public byte Unk5 { get; set; } // flag to acknowledge extra data
        public S_InitCollVolume_Nested Unk5_Data { get; set; } // NOT ACTUALLY COLLISION DATA..
        public byte Unk6 { get; set; } // flag to acknowledge extra data
        public string Unk6_Value { get; set; }
        public int Unk7 { get; set; } // float
        public uint[] Unk8 { get; set; } // unknown data
        public S_InitDeformPartEffects PartEffects { get; set; }

        public S_InitDeformPartCommon()
        {
            Unk2 = new int[0];
            Unk3_Transform = new C_Transform();
            Unk4 = new S_InitDeformPartCommon_Unk4[0];
            Unk6_Value = String.Empty;
            Unk8 = new uint[0];
            PartEffects = new S_InitDeformPartEffects();
        }

        public void Load(BitStream MemStream)
        {
            SpeedMin = MemStream.ReadSingle() * 3.6f;
            SpeedMax = MemStream.ReadSingle() * 3.6f;
            Resistance = MemStream.ReadSingle() * 100.0f;
            Mass = MemStream.ReadSingle();
            EnergyStart = MemStream.ReadSingle() / 0.01f;
            EnergyDrop = MemStream.ReadSingle() / 0.01f;

            // Count - list of floats
            uint Unk2Count = MemStream.ReadUInt32();
            Unk2 = new int[Unk2Count];
            for (int i = 0; i < Unk2.Length; i++)
            {
                Unk2[i] = MemStream.ReadInt32();
            }

            Unk3 = MemStream.ReadBit(); // flag for extra data
            if(Unk3 == 1)
            {
                Unk3_Transform = new C_Transform();
                Unk3_Transform.Load(MemStream);
            }

            uint Unk4_Count = MemStream.ReadUInt32(); // count of unknown data
            Unk4 = new S_InitDeformPartCommon_Unk4[Unk4_Count];
            for(int i = 0; i < Unk4.Length; i++)
            {
                S_InitDeformPartCommon_Unk4 NewUnknown = new S_InitDeformPartCommon_Unk4();
                NewUnknown.Unk0 = MemStream.ReadInt32();
                NewUnknown.Unk1 = MemStream.ReadInt32();
                Unk4[i] = NewUnknown;
            }

            Unk5 = MemStream.ReadBit(); // flag for extra data
            if(Unk5 == 1)
            {
                // in M2DE exe - sub_1402D1CF0
                // in logs - INITCOLLVOLUMENESTED
                Unk5_Data = new S_InitCollVolume_Nested();
                Unk5_Data.Load(MemStream);
            }

            Unk6 = MemStream.ReadBit(); // flag for extra data
            if(Unk6 == 1)
            {
                Unk6_Value = MemStream.ReadString32();
            }

            Unk7 = MemStream.ReadInt32();

            // Unknown data. Could be indexes?
            uint NumUnk8 = MemStream.ReadUInt32();
            Unk8 = new uint[NumUnk8];
            for(uint i = 0; i < NumUnk8; i++)
            {
                Unk8[i] = MemStream.ReadUInt32();
            }

            PartEffects = new S_InitDeformPartEffects();
            PartEffects.Load(MemStream);
        }

        public void Save(BitStream MemStream)
        {
            MemStream.WriteSingle(SpeedMin / 3.6f);
            MemStream.WriteSingle(SpeedMax / 3.6f);
            MemStream.WriteSingle(Resistance / 100.0f);
            MemStream.WriteSingle(Mass);
            MemStream.WriteSingle(EnergyStart * 0.01f);
            MemStream.WriteSingle(EnergyDrop * 0.01f);

            MemStream.WriteInt32(Unk2.Length);
            foreach (int Value in Unk2)
            {
                MemStream.WriteInt32(Value);
            }

            // Unknown transform
            MemStream.WriteBit(Unk3);
            if(Unk3 == 1)
            {
                Unk3_Transform.Save(MemStream);
            }

            MemStream.WriteInt32(Unk4.Length);
            foreach(S_InitDeformPartCommon_Unk4 Unk4Data in Unk4)
            {
                MemStream.WriteInt32(Unk4Data.Unk0);
                MemStream.WriteInt32(Unk4Data.Unk1);
            }

            // Unknown data
            MemStream.WriteBit(Unk5);
            if(Unk5 == 1)
            {
                Unk5_Data.Save(MemStream);
            }

            MemStream.WriteBit(Unk6);
            if(Unk6 == 1)
            {
                MemStream.WriteString32(Unk6_Value);
            }

            MemStream.WriteInt32(Unk7);

            // Unknown data. Could be indexes?
            MemStream.WriteInt32(Unk8.Length);
            foreach(uint Value in Unk8)
            {
                MemStream.WriteUInt32(Value);
            }

            PartEffects.Save(MemStream);
        }
    }
}
