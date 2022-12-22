using BitStreams;

namespace ResourceTypes.Prefab.CrashObject
{
    public class S_InitHumanSupport
    {
        public ulong Hash { get; set; }
        public float[] Unk0 { get; set; }
        public uint Unk1 { get; set; }
        public uint Unk2 { get; set; }
        public byte Unk3 { get; set; }

        public S_InitHumanSupport()
        {
            Unk0 = new float[0];
        }

        public void Load(BitStream MemStream)
        {
            Hash = MemStream.ReadUInt64();

            Unk0 = new float[12];
            for(int i = 0; i < Unk0.Length; i++)
            {
                Unk0[i] = MemStream.ReadSingle();
            }

            Unk1 = MemStream.ReadUInt32();
            Unk2 = MemStream.ReadUInt32();
            Unk3 = MemStream.ReadBit();
        }

        public void Save(BitStream MemStream)
        {
            MemStream.WriteUInt64(Hash);

            for (int i = 0; i < 12; i++)
            {
                MemStream.WriteSingle(Unk0[i]);
            }

            MemStream.WriteUInt32(Unk1);
            MemStream.WriteUInt32(Unk2);
            MemStream.WriteBit(Unk3);
        }
    }
}
