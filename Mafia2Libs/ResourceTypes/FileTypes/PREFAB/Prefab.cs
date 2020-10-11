using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;

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
                prefab.WriteToFile(writer);
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
            [ReadOnly(true)]
            public int Unk0 {get;set;}
            [ReadOnly(true)]
            public int PrefabSize { get; set; }

            byte[] data;

            public void ReadFromFile(BinaryReader reader)
            {
                Hash = reader.ReadUInt64();
                PrefabType = reader.ReadInt32();
                Unk0 = reader.ReadInt32();
                PrefabSize = reader.ReadInt32();
                data = reader.ReadBytes(PrefabSize);

                //BitStreams.BitStream stream = new BitStreams.BitStream(reader.BaseStream);
                //int globalInitVer = stream.ReadInt32();

                //byte[] data1 = new byte[8];
                //for (int i = 0; i < 8; i++)
                //    data1[i] = stream.ReadByte();
                //ulong int11 = (ulong)((data1[3] << 24) | (data1[2] << 16) | (data1[1] << 8) | data1[0]);
                //ulong int12 = (ulong)((data1[7] << 24) | (data1[6] << 16) | (data1[5] << 8) | data1[4]);
                //byte[] data2 = new byte[8];
                //for (int i = 0; i < 8; i++)
                //    data2[i] = stream.ReadByte();
                //ulong int21 = (ulong)((data1[3] << 24) | (data2[2] << 16) | (data2[1] << 8) | data2[0]);
                //ulong int22 = (ulong)((data1[7] << 24) | (data2[6] << 16) | (data2[5] << 8) | data2[4]);
                //byte[] data3 = new byte[8];
                //for (int i = 0; i < 8; i++)
                //    data3[i] = stream.ReadByte();
                //ulong int31 = (ulong)((data1[3] << 24) | (data3[2] << 16) | (data3[1] << 8) | data3[0]);
                //ulong int32 = (ulong)((data1[7] << 24) | (data3[6] << 16) | (data3[5] << 8) | data3[4]);
                //data = reader.ReadBytes(prefabSize-28);

                //stream.ReadBits(464)
                using (BinaryWriter writer = new BinaryWriter(File.Open("Prefabs/" + Hash.ToString() + "Type_" + PrefabType + ".prefab", FileMode.Create)))
                {
                    writer.Write(data);
                }


                //unk4 = reader.ReadInt32();
                //unkHashCount = reader.ReadInt32();
                //unkHashes = new ulong[unkHashCount];

                //for (int i = 0; i != unkHashes.Length; i++)
                //    unkHashes[i] = reader.ReadUInt64();

                //if(reader.ReadInt32() != 0) //should be zero?
                //    Console.WriteLine("Wasn't zero.");
            }

            public void WriteToFile(BinaryWriter writer)
            {
                writer.Write(Hash);
                writer.Write(PrefabType);
                writer.Write(Unk0);
                writer.Write(PrefabSize);
                writer.Write(data);
            }

            public int GetSize()
            {
                return PrefabSize + 20;
            }
        }
    }
}
