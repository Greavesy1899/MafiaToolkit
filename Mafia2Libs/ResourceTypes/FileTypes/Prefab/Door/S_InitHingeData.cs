using BitStreams;

namespace ResourceTypes.Prefab.Door
{
    public class S_InitHingeData
    {
        public ulong DoorFrameName { get; set; }
        public bool Unk0 { get; set; } // m_IsReverse?
        public float[] Unk1 { get; set; } // KickMotorSpeed?
        public float[] Unk2 { get; set; } // KickMotorPower?

        public S_InitHingeData()
        {
            Unk1 = new float[6];
            Unk2 = new float[6];
        }

        public void Load(BitStream MemStream)
        {
            DoorFrameName = MemStream.ReadUInt64();
            Unk0 = MemStream.ReadBit();

            // Read float arrays.
            // Should be fixed to 6 floats.
            for(uint i = 0; i < 6; i++)
            {
                Unk1[i] = MemStream.ReadSingle();
            }

            for (uint i = 0; i < 6; i++)
            {
                Unk2[i] = MemStream.ReadSingle();
            }
        }

        public void Save(BitStream MemStream)
        {
            MemStream.WriteUInt64(DoorFrameName);
            MemStream.WriteBit(Unk0);

            // Read float arrays.
            // Should be fixed to 6 floats.
            for (uint i = 0; i < 6; i++)
            {
                MemStream.WriteSingle(Unk1[i]);
            }

            for (uint i = 0; i < 6; i++)
            {
                MemStream.WriteSingle(Unk2[i]);
            }
        }
    }
}
