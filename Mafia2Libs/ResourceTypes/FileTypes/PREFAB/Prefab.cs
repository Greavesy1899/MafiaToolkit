using BitStreams;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using ResourceTypes.Prefab.CrashObject;
using ResourceTypes.Prefab.Vehicle;
using ResourceTypes.Prefab.Door;
using ResourceTypes.Prefab.Wagon;
using System.Xml.Linq;

namespace ResourceTypes.Prefab
{
//S_DeformationInitData
//S_VehicleInitData
//S_ActorDeformData
//S_WheelInitData
//S_PhThingActorInitData
//S_BoatInitData
//S_WagonInitData
//S_BrainInitData
//2: CarInitData
//1: VehicleInitData
//0: DeformationInitData
//3: COInitData(CrashObjectInitData?)
//4: ActorDeformInitData
//5: WheelInitData
//6: PhThingActorBaseInitData
//7: DoorInitData
//9: BoatInitData
//10: WagonInitData
//11: BrainInitData
//8: LifeInitData

    public class PrefabLoader
    {
        int sizeOfFile; //-4 bytes
        int sizeOfFile2; //-12 bytes

        public PrefabStruct[] Prefabs { get; set; }


        public PrefabLoader(FileInfo file)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(file.FullName, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
        }

        public void WriteToFile(FileInfo file)
        {
            UpdatePrefabMetaInfo();

            using(BinaryWriter writer = new BinaryWriter(File.Open(file.FullName, FileMode.Create)))
            {
                WriteToFile(writer);
            }
        }

        public void ReadFromFile(BinaryReader reader)
        {
            if (!Directory.Exists("Prefabs"))
                Directory.CreateDirectory("Prefabs");

            sizeOfFile = reader.ReadInt32();
            int numPrefabs = reader.ReadInt32();
            sizeOfFile2 = reader.ReadInt32();
            Prefabs = new PrefabStruct[numPrefabs];

            for (int i = 0; i < numPrefabs; i++)
            {
                Prefabs[i] = new PrefabStruct();
                Prefabs[i].ReadFromFile(reader);
            }

            Debug.Assert(reader.BaseStream.Position == reader.BaseStream.Position, "We did not reach the end of the prefab file!");
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(sizeOfFile);
            writer.Write(Prefabs.Length);
            writer.Write(sizeOfFile2);

            foreach(var prefab in Prefabs)
            {
                bool bWasTheSameSize = prefab.WriteToFile(writer, true);
                if (Debugger.IsAttached && bWasTheSameSize)
                {
                    //break;
                }
            }
        }

        private void UpdatePrefabMetaInfo()
        {
            // Calculate size of file
            int size = 8;

            foreach (var prefab in Prefabs)
            {
                size += prefab.GetSize();
            }

            sizeOfFile = size;
            sizeOfFile2 = size - 8;
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", 1, 2);
        }

        public class PrefabStruct
        {
            public ulong Hash { get; set; }
            public string AssignedName { get; set; }
            public int PrefabType { get; set; }
            public S_GlobalInitData InitData { get; set; }
            [ReadOnly(true)]
            public int Unk0 {get;set;}
            [ReadOnly(true)]
            public int PrefabSize { get; set; }

            byte[] data;

            public PrefabStruct()
            {
                AssignedName = "";
            }

            public void ReadFromFile(BinaryReader reader)
            {
                Hash = reader.ReadUInt64();
                PrefabType = reader.ReadInt32();
                Unk0 = reader.ReadInt32();
                PrefabSize = reader.ReadInt32();

                long CurrentPosition = reader.BaseStream.Position;
                data = reader.ReadBytes(PrefabSize);

                BitStream MemStream = new BitStream(data);

                if (PrefabType == 2)
                {
                    InitData = new S_CarInitData();
                    InitData.Load(MemStream);
                }
                else if (PrefabType == 3)
                {
                    InitData = new S_COInitData();
                    InitData.Load(MemStream);
                }
                else if(PrefabType == 4)
                {
                    InitData = new S_ActorDeformInitData();
                    InitData.Load(MemStream);
                }
                else if (PrefabType == 5)
                {
                    InitData = new S_WheelInitData();
                    InitData.Load(MemStream);
                }
                else if (PrefabType == 6)
                {
                    InitData = new S_PhThingActorBaseInitData();
                    InitData.Load(MemStream);
                }
                else if (PrefabType == 7)
                {
                    InitData = new S_DoorInitData();
                    InitData.Load(MemStream);
                }
                else if (PrefabType == 8)
                {
                    InitData = new S_LiftInitData();
                    InitData.Load(MemStream);
                }
                else if(PrefabType == 10)
                {
                    InitData = new S_WagonInitData();
                    InitData.Load(MemStream);
                }
                else if(PrefabType == 9)
                {
                    InitData = new S_BoatInitData();
                    InitData.Load(MemStream);
                }

                Debug.Assert(MemStream.Length == PrefabSize, "Didn't read everthing when loading");
            }

            public bool WriteToFile(BinaryWriter writer, bool bMakeNew = false)
            {
                bool bIsLengthTheSame = true;

                if (bMakeNew)
                {
                    bIsLengthTheSame = false;
                    data = GetLatestData(out bIsLengthTheSame);
                }
                
                writer.Write(Hash);
                writer.Write(PrefabType);
                writer.Write(Unk0);
                writer.Write(PrefabSize);
                writer.Write(data);

                return bIsLengthTheSame;
            }

            public int GetSize()
            {
                return PrefabSize + 20;
            }

            private byte[] GetLatestData(out bool bIsLengthTheSame)
            {
                // Write prefab data to stream
                byte[] Storage = new byte[65536];
                BitStream OutStream = new BitStream(Storage);
                InitData.Save(OutStream);

                OutStream.EndByte();
                OutStream.ChangeLength(OutStream.GetStream().Position);
                byte[] NewData = OutStream.GetStreamData();

                // Sanity check size
                // (Debugger only)
                if (Debugger.IsAttached)
                {
                    Debug.Assert(OutStream.Length == PrefabSize, "Incorrect Size when doing the save test");
                }

                bIsLengthTheSame = OutStream.Length == PrefabSize;
                PrefabSize = (int)OutStream.Length;

                return NewData;
            }
        }
    }
}
