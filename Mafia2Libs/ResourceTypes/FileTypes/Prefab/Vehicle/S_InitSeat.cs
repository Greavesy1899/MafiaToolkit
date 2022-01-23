using BitStreams;

namespace ResourceTypes.Prefab.Vehicle
{
    public class S_InitSeat
    {
        public uint Flags { get; set; }
        public ulong DoorIndexFrameName { get; set; }
        public C_Vector3 TargetAim { get; set; }
        public C_Vector3 TargetSeat { get; set; }
        public C_Vector3 LockPos { get; set; }
        public C_Vector3 Direction { get; set; }
        public C_Vector3 Position { get; set; }
        public uint SeatType { get; set; }
        public uint SeatIndex { get; set; }
        public uint SeatGroup { get; set; }
        public ulong FrameName { get; set; }

        public S_InitSeat()
        {
            TargetAim = new C_Vector3();
            TargetSeat = new C_Vector3();
            LockPos = new C_Vector3();
            Direction = new C_Vector3();
            Position = new C_Vector3();
        }

        public void Load(BitStream MemStream)
        {
            Flags = MemStream.ReadUInt32();
            DoorIndexFrameName = MemStream.ReadUInt64();
            TargetAim = C_Vector3.Construct(MemStream);
            TargetSeat = C_Vector3.Construct(MemStream);
            LockPos = C_Vector3.Construct(MemStream);
            Direction = C_Vector3.Construct(MemStream);
            Position = C_Vector3.Construct(MemStream);
            SeatType = MemStream.ReadUInt32();
            SeatIndex = MemStream.ReadUInt32();
            SeatGroup = MemStream.ReadUInt32();
            FrameName = MemStream.ReadUInt64();
        }

        public void Save(BitStream MemStream)
        {
            MemStream.WriteUInt32(Flags);
            MemStream.WriteUInt64(DoorIndexFrameName);
            TargetAim.Save(MemStream);
            TargetSeat.Save(MemStream);
            LockPos.Save(MemStream);
            Direction.Save(MemStream);
            Position.Save(MemStream);
            MemStream.WriteUInt32(SeatType);
            MemStream.WriteUInt32(SeatIndex);
            MemStream.WriteUInt32(SeatGroup);
            MemStream.WriteUInt64(FrameName);
        }
    }
}
