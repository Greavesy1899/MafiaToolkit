using System.IO;
using Utils.Extensions;

namespace ResourceTypes.Actors
{
    public class ActorTrain : IActorExtraDataInterface
    {
        public class TrainSnd
        {
            public uint SndCategory { get; set; }
            public uint SndId { get; set; }
            public float Volume { get; set; }
            public float VolumeBreak { get; set; }
            public float FrqLo { get; set; }
            public float FrqHi { get; set; }
            public float RotLo { get; set; }
            public float RotLoVol { get; set; }
            public float RotHiVol { get; set; }
            public float RotHi { get; set; }
            public uint Unk01 { get; set; }
            public uint Unk02 { get; set; }
            public uint Unk03 { get; set; }

            public void ReadFromFile(MemoryStream Reader, bool bIsBigEndian)
            {
                SndCategory = Reader.ReadUInt32(bIsBigEndian);
                SndId = Reader.ReadUInt32(bIsBigEndian);
                Volume = Reader.ReadSingle(bIsBigEndian);
                VolumeBreak = Reader.ReadSingle(bIsBigEndian);
                FrqLo = Reader.ReadSingle(bIsBigEndian);
                FrqHi = Reader.ReadSingle(bIsBigEndian);
                RotLo = Reader.ReadSingle(bIsBigEndian);
                RotLoVol = Reader.ReadSingle(bIsBigEndian);
                RotHiVol = Reader.ReadSingle(bIsBigEndian);
                RotHi = Reader.ReadSingle(bIsBigEndian);
                Unk01 = Reader.ReadUInt32(bIsBigEndian);
                Unk02 = Reader.ReadUInt32(bIsBigEndian);
                Unk03 = Reader.ReadUInt32(bIsBigEndian);
            }

            public void WriteToFile(MemoryStream Writer, bool bIsBigEndian)
            {
                Writer.Write(SndCategory, bIsBigEndian);
                Writer.Write(SndId, bIsBigEndian);
                Writer.Write(Volume, bIsBigEndian);
                Writer.Write(VolumeBreak, bIsBigEndian);
                Writer.Write(FrqLo, bIsBigEndian);
                Writer.Write(FrqHi, bIsBigEndian);
                Writer.Write(RotLo, bIsBigEndian);
                Writer.Write(RotLoVol, bIsBigEndian);
                Writer.Write(RotHiVol, bIsBigEndian);
                Writer.Write(RotHi, bIsBigEndian);
                Writer.Write(Unk01, bIsBigEndian);
                Writer.Write(Unk02, bIsBigEndian);
                Writer.Write(Unk03, bIsBigEndian);
            }
        }

        public float MaxAcceleration { get; set; }
        public float MaxDeacceleration { get; set; }
        public float MaxSpeed { get; set; } // Divide by 3.6 when loading
        public uint MinPasazeru { get; set; }
        public uint MaxPasazeru { get; set; }
        public TrainSnd[] EngineSounds { get; set; }
        public TrainSnd[] DoorSounds { get; set; }
        public TrainSnd[] Engine2Sounds { get; set; }
        public TrainSnd[] OtherSounds { get; set; }
        public TrainSnd[] Door2Sounds { get; set; }

        // Class constants
        private const uint NUM_ENGINE_SOUNDS = 4;
        private const uint NUM_DOOR_SOUNDS = 2;
        private const uint NUM_ENGINE2_SOUNDS = 5;
        private const uint NUM_OTHER_SOUNDS = 2;
        private const uint NUM_DOOR2_SOUNDS = 2;

        public ActorTrain()
        {
            // Initialise sounds arrays
            EngineSounds = SetupArray(NUM_ENGINE_SOUNDS);
            DoorSounds = SetupArray(NUM_DOOR_SOUNDS);
            Engine2Sounds = SetupArray(NUM_ENGINE2_SOUNDS);
            OtherSounds = SetupArray(NUM_OTHER_SOUNDS);
            Door2Sounds = SetupArray(NUM_DOOR2_SOUNDS);
        }

        public void ReadFromFile(MemoryStream Reader, bool bIsBigEndian)
        {
            MaxAcceleration = Reader.ReadSingle(bIsBigEndian);
            MaxDeacceleration = Reader.ReadSingle(bIsBigEndian);
            MaxSpeed = Reader.ReadSingle(bIsBigEndian);
            MinPasazeru = Reader.ReadUInt32(bIsBigEndian);
            MaxPasazeru = Reader.ReadUInt32(bIsBigEndian);

            // Read all fixed size arrays
            ReadArray(EngineSounds, Reader, bIsBigEndian);
            ReadArray(DoorSounds, Reader, bIsBigEndian);
            ReadArray(Engine2Sounds, Reader, bIsBigEndian);
            ReadArray(OtherSounds, Reader, bIsBigEndian);
            ReadArray(Door2Sounds, Reader, bIsBigEndian);
        }

        public void WriteToFile(MemoryStream Writer, bool bIsBigEndian)
        {
            Writer.Write(MaxAcceleration, bIsBigEndian);
            Writer.Write(MaxDeacceleration, bIsBigEndian);
            Writer.Write(MaxSpeed, bIsBigEndian);
            Writer.Write(MinPasazeru, bIsBigEndian);
            Writer.Write(MaxPasazeru, bIsBigEndian);

            // Write all fixed size arrays
            WriteArray(EngineSounds, Writer, bIsBigEndian);
            WriteArray(DoorSounds, Writer, bIsBigEndian);
            WriteArray(Engine2Sounds, Writer, bIsBigEndian);
            WriteArray(OtherSounds, Writer, bIsBigEndian);
            WriteArray(Door2Sounds, Writer, bIsBigEndian);
        }

        public int GetSize()
        {
            return 800;
        }

        //~ Utility functions, mainly with fixed-sized arrays.
        private TrainSnd[] SetupArray(uint NumItems)
        {
            TrainSnd[] OutArray = new TrainSnd[NumItems];

            OutArray = new TrainSnd[NumItems];
            for (int i = 0; i < NumItems; i++)
            {
                OutArray[i] = new TrainSnd();
            }

            return OutArray;
        }

        private void ReadArray(TrainSnd[] Array, MemoryStream ReaderStream, bool bIsBigEndian)
        {
            for (int i = 0; i < Array.Length; i++)
            {
                Array[i].ReadFromFile(ReaderStream, bIsBigEndian);
            }
        }

        private void WriteArray(TrainSnd[] Array, MemoryStream ReaderStream, bool bIsBigEndian)
        {
            for (int i = 0; i < Array.Length; i++)
            {
                Array[i].WriteToFile(ReaderStream, bIsBigEndian);
            }
        }
    }
}
