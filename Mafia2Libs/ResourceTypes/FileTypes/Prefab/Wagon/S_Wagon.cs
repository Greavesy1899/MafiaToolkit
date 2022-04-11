using BitStreams;
using ResourceTypes.Prefab.CrashObject;

namespace ResourceTypes.Prefab.Wagon
{
    public class S_WagonInitData_Unk0
    {
        public ulong Hash0 { get; set; }
        public int Unk0 { get; set; }
        public int Unk1 { get; set; }
        public int Unk2 { get; set; }
        public int Unk3 { get; set; }
        public int Unk4 { get; set; }
        public int Unk5 { get; set; }
        public int Unk6 { get; set; }

        public void Load(BitStream MemStream)
        {
            Hash0 = MemStream.ReadUInt64();
            Unk0 = MemStream.ReadInt32();
            Unk1 = MemStream.ReadInt32();
            Unk2 = MemStream.ReadInt32();
            Unk3 = MemStream.ReadInt32();
            Unk4 = MemStream.ReadInt32();
            Unk5 = MemStream.ReadInt32();
            Unk6 = MemStream.ReadInt32();
        }

        public void Save(BitStream MemStream)
        {
            MemStream.WriteUInt64(Hash0);
            MemStream.WriteInt32(Unk0);
            MemStream.WriteInt32(Unk1);
            MemStream.WriteInt32(Unk2);
            MemStream.WriteInt32(Unk3);
            MemStream.WriteInt32(Unk4);
            MemStream.WriteInt32(Unk5); 
            MemStream.WriteInt32(Unk6);
        }
    }

    public class S_WagonInitData : S_DeformationInitData
    {
        public S_WagonInitData_Unk0[] WagonData { get; set; }

        public S_WagonInitData()
        {
            WagonData = new S_WagonInitData_Unk0[0];
        }

        public override void Load(BitStream MemStream)
        {
            base.Load(MemStream);

            // Not present in any game
            uint NumData = MemStream.ReadUInt32();
            for(int i = 0; i < NumData; i++)
            {
                S_WagonInitData_Unk0 InitData_Unk0 = new S_WagonInitData_Unk0();
                InitData_Unk0.Load(MemStream);
                WagonData[i] = InitData_Unk0;
            }
        }

        public override void Save(BitStream MemStream)
        {
            base.Save(MemStream);

            // Not present in any game
            MemStream.WriteUInt32((uint)WagonData.Length);
            foreach (S_WagonInitData_Unk0 Entry in WagonData)
            {
                Entry.Save(MemStream);
            }
        }
    }
}
