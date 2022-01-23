using BitStreams;

namespace ResourceTypes.Prefab.Vehicle
{
    public class S_CarInitData : S_VehicleInitData
    {
        public ulong Hash0 { get; set; }
        public ulong Hash1 { get; set; }
        public S_InitSeat[] Seats { get; set; }
        public ulong[] BusSeatsFrameName { get; set; }
        public ulong[] EnterBusFrameName { get; set; }
        public ulong[] Hashes4 { get; set; }
        public ulong[] Hashes5 { get; set; }
        public ulong[] WipersFrameName { get; set; }
        public S_InitDrWheelHSnap[] DrWheelSnap { get; set; }
        public ulong[] Hashes8 { get; set; }
        public ulong[] Hashes9 { get; set; }
        public S_InitDoorInterestingPoints[] DoorInterestingPoints { get; set; }
        public S_InitClimbBox[] ClimbBoxes { get; set; }

        public override void Load(BitStream MemStream)
        {
            base.Load(MemStream);

            Hash0 = MemStream.ReadUInt64();
            Hash1 = MemStream.ReadUInt64();

            // Read Seats
            uint NumSeats = MemStream.ReadUInt32();
            Seats = new S_InitSeat[NumSeats];
            for (uint i = 0; i < NumSeats; i++)
            {
                S_InitSeat SeatData = new S_InitSeat();
                SeatData.Load(MemStream);
                Seats[i] = SeatData;
            }
        
            BusSeatsFrameName = PrefabUtils.ReadHashArray(MemStream);
            EnterBusFrameName = PrefabUtils.ReadHashArray(MemStream);
            Hashes4 = PrefabUtils.ReadHashArray(MemStream); // TODO: What are these?
            Hashes5 = PrefabUtils.ReadHashArray(MemStream); // TODO: What are these?
            WipersFrameName = PrefabUtils.ReadHashArray(MemStream);

            // Read DrWheelHSnap
            uint NumDrWheelHSnap = MemStream.ReadUInt32();
            DrWheelSnap = new S_InitDrWheelHSnap[NumDrWheelHSnap];
            for (uint i = 0; i < NumDrWheelHSnap; i++)
            {
                S_InitDrWheelHSnap WheelSnap = new S_InitDrWheelHSnap();
                WheelSnap.Load(MemStream);
                DrWheelSnap[i] = WheelSnap;
            }

            // TODO: What are these?
            Hashes8 = PrefabUtils.ReadHashArray(MemStream); // TODO: What are these?
            Hashes9 = PrefabUtils.ReadHashArray(MemStream); // TODO: What are these?

            // Read DoorInterestingPoints
            uint NumInterestingPoints = MemStream.ReadUInt32();
            DoorInterestingPoints = new S_InitDoorInterestingPoints[NumInterestingPoints];
            for (uint i = 0; i < NumInterestingPoints; i++)
            {
                S_InitDoorInterestingPoints DoorPoint = new S_InitDoorInterestingPoints();
                DoorPoint.Load(MemStream);
                DoorInterestingPoints[i] = DoorPoint;
            }

            // Read ClimbBoxes
            uint NumClimbBoxes = MemStream.ReadUInt32();
            ClimbBoxes = new S_InitClimbBox[NumClimbBoxes];
            for (uint i = 0; i < NumClimbBoxes; i++)
            {
                S_InitClimbBox ClimbBox = new S_InitClimbBox();
                ClimbBox.Load(MemStream);
                ClimbBoxes[i] = ClimbBox;
            }
        }

        public override void Save(BitStream MemStream)
        {
            base.Save(MemStream);

            MemStream.WriteUInt64(Hash0);
            MemStream.WriteUInt64(Hash1);

            // Write Seats
            MemStream.WriteUInt32((uint)Seats.Length);
            foreach(S_InitSeat Seat in Seats)
            {
                Seat.Save(MemStream);
            }

            PrefabUtils.WriteHashArray(MemStream, BusSeatsFrameName);
            PrefabUtils.WriteHashArray(MemStream, EnterBusFrameName);
            PrefabUtils.WriteHashArray(MemStream, Hashes4);
            PrefabUtils.WriteHashArray(MemStream, Hashes5);
            PrefabUtils.WriteHashArray(MemStream, WipersFrameName);

            // Write DrWheelHSnap
            MemStream.WriteUInt32((uint)DrWheelSnap.Length);
            foreach (S_InitDrWheelHSnap WheelSnap in DrWheelSnap)
            {
                WheelSnap.Save(MemStream);
            }

            PrefabUtils.WriteHashArray(MemStream, Hashes8);
            PrefabUtils.WriteHashArray(MemStream, Hashes9);

            // Write DoorInterestingPoints
            MemStream.WriteUInt32((uint)DoorInterestingPoints.Length);
            foreach (S_InitDoorInterestingPoints DoorPoint in DoorInterestingPoints)
            {
                DoorPoint.Save(MemStream);
            }

            // Write ClimbBoxes
            MemStream.WriteUInt32((uint)ClimbBoxes.Length);
            foreach (S_InitClimbBox ClimbBox in ClimbBoxes)
            {
                ClimbBox.Save(MemStream);
            }
        }
    }
}
