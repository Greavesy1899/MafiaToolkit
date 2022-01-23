using BitStreams;

namespace ResourceTypes.Prefab.Vehicle
{
    public class S_InitDoorInterestingPoints
    {
        public C_Vector3 CarDoorHandlePos { get; set; }
        public C_Vector3 CarDoorLockPos { get; set; }
        public ulong DoorFrameName { get; set; }

        public S_InitDoorInterestingPoints()
        {
            CarDoorHandlePos = new C_Vector3();
            CarDoorLockPos = new C_Vector3();
        }

        public void Load(BitStream MemStream)
        {
            CarDoorHandlePos = C_Vector3.Construct(MemStream);
            CarDoorLockPos = C_Vector3.Construct(MemStream);
            DoorFrameName = MemStream.ReadUInt64();
        }

        public void Save(BitStream MemStream)
        {
            CarDoorHandlePos.Save(MemStream);
            CarDoorLockPos.Save(MemStream);
            MemStream.WriteUInt64(DoorFrameName);
        }
    }
}
