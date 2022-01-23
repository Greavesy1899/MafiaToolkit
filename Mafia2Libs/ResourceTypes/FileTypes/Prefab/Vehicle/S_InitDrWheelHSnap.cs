using BitStreams;

namespace ResourceTypes.Prefab.Vehicle
{
    public class S_InitDrWheelHSnap
    {
        public ulong DrWheelFrameName { get; set; }
        public ulong LeftSnapFrameName { get; set; }
        public ulong RightSnapFrameName { get; set; }

        public void Load(BitStream MemStream)
        {
            DrWheelFrameName = MemStream.ReadUInt64();
            LeftSnapFrameName = MemStream.ReadUInt64();
            RightSnapFrameName = MemStream.ReadUInt64();
        }

        public void Save(BitStream MemStream)
        {
            MemStream.WriteUInt64(DrWheelFrameName);
            MemStream.WriteUInt64(LeftSnapFrameName);
            MemStream.WriteUInt64(RightSnapFrameName);
        }
    }
}
