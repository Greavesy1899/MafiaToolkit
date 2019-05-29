using System;
using System.IO;

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
    public class PrefabLoader
    {
        int sizeOfFile; //-4 bytes
        int unk01; //possible count

        int sizeOfFile2; //-12 bytes

        PrefabStruct[] prefabs;


        public PrefabLoader(FileInfo file)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(file.FullName, FileMode.Open)))
                ReadFromFile(reader);
        }

        public PrefabLoader(string fileName)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
                ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            sizeOfFile = reader.ReadInt32();
            unk01 = reader.ReadInt32();
            sizeOfFile2 = reader.ReadInt32();
            prefabs = new PrefabStruct[unk01];

            for (int i = 0; i != unk01; i++)
            {
                prefabs[i] = new PrefabStruct();
                prefabs[i].ReadFromFile(reader);
            }
            if (reader.BaseStream.Position == reader.BaseStream.Length)
                Console.WriteLine("Reached EOF");
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", 1, 2);
        }

        public struct PrefabStruct
        {
            ulong hash;
            int type; //type?
            int unkSize; //maybe size
            int prefabSize; //another size?
            byte[] data;
            int unk4; //4?
            int unkHashCount;
            ulong[] unkHashes;

            public void ReadFromFile(BinaryReader reader)
            {
                hash = reader.ReadUInt64();
                type = reader.ReadInt32();
                unkSize = reader.ReadInt32();
                prefabSize = reader.ReadInt32();

                data = reader.ReadBytes(prefabSize);

                using (BinaryWriter writer = new BinaryWriter(File.Open("Prefabs/" + hash.ToString() + "Type_"+type+".prefab", FileMode.Create)))
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
        }
    }
}
