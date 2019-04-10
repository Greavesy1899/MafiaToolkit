using SharpDX;
using System;
using System.IO;
using Utils.SharpDXExtensions;
using Utils.StringHelpers;

namespace ResourceTypes.Translokator
{
    struct structGrid
    {
        public short Key;
        public Vector3 Origin;
        public Vector2 CellSize;
        public int Width;
        public int Height;
        public short[] data;
    }

    struct structInstance
    {
        public short PositionX;
        public short PositionY;
        public short PositionZ;
        public int Rotation2;
        public short Unk01;
        public int Rotation1;
    }

    struct structObject
    {
        public short Unk01;
        public short Unk02;
        public ulong Hash;
        public string Name;
        public byte[] UnkBytes1;
        public float Unk03;
        public float Unk04;
        public int NumInstances;
        public structInstance[] Instances;
    }

    struct structObjectGroup
    {
        public short Unk01;
        public int NumObjects;
        public structObject[] Objects;
    }

    public class TranslokatorLoader
    {
        public TranslokatorLoader(FileInfo info)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(info.FullName, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
        }

        public void ReadFromFile(BinaryReader reader)
        {
            int Version = reader.ReadInt32();
            int unk1 = reader.ReadInt32();
            short unk2 = reader.ReadInt16();

            BoundingBox bounds = BoundingBoxExtenders.ReadFromFile(reader);

            int unk3 = reader.ReadInt32();
            structGrid[] grids = new structGrid[unk3];

            for (int i = 0; i != unk3; i++)
            {
                structGrid grid = new structGrid();
                grid.Key = reader.ReadInt16();
                grid.Origin = Vector3Extenders.ReadFromFile(reader);
                grid.CellSize = Vector2Extenders.ReadFromFile(reader);
                grid.Width = reader.ReadInt32();
                grid.Height = reader.ReadInt32();
                grid.data = new short[grid.Width * grid.Height];

                for (int y = 0; y != grid.data.Length; y++)
                    grid.data[y] = reader.ReadInt16();

                grids[i] = grid;
            }

            int unk3Count = reader.ReadInt32();
            short[] unks3 = new short[unk3Count * 2];

            for (int i = 0; i != unk3Count * 2; i++)
            {
                unks3[i] = reader.ReadInt16();
            }

            int numObjectGroups = reader.ReadInt32();
            structObjectGroup[] objectGroups = new structObjectGroup[numObjectGroups];
            Console.WriteLine("Num Object Groups: {0}", numObjectGroups);

            for (int i = 0; i != numObjectGroups; i++)
            {
                Console.WriteLine("Object Groups: {0}", i);
                structObjectGroup objectGroup = new structObjectGroup();
                objectGroup.Unk01 = reader.ReadInt16();
                objectGroup.NumObjects = reader.ReadInt32();
                objectGroup.Objects = new structObject[objectGroup.NumObjects];
                Console.WriteLine("Num Objects: {0}", objectGroup.NumObjects);

                for (int x = 0; x != objectGroup.NumObjects; x++)
                {
                    Console.WriteLine("Object: {0}", x);
                    structObject obj = new structObject();
                    obj.Unk01 = reader.ReadInt16();
                    obj.Unk02 = reader.ReadInt16();
                    obj.Hash = reader.ReadUInt64();
                    obj.Name = StringHelpers.ReadString(reader);
                    obj.UnkBytes1 = reader.ReadBytes(31 - obj.Name.Length);
                    obj.Unk03 = reader.ReadSingle();
                    obj.Unk04 = reader.ReadSingle();
                    obj.NumInstances = reader.ReadInt32();
                    obj.Instances = new structInstance[obj.NumInstances];
                    Console.WriteLine("Name: {0}", obj.Name);
                    Console.WriteLine("Num Instances: {0}", obj.NumInstances);

                    for (int y = 0; y != obj.NumInstances; y++)
                    {
                        Console.WriteLine("Instance: {0}", y);
                        structInstance instance = new structInstance();
                        instance.PositionX = reader.ReadInt16();
                        instance.PositionY = reader.ReadInt16();
                        instance.PositionZ = reader.ReadInt16();
                        instance.Rotation2 = reader.ReadInt32();
                        instance.Unk01 = reader.ReadInt16();
                        instance.Rotation1 = reader.ReadInt16();
                        obj.Instances[y] = instance;

                        Console.WriteLine("{0:X4}", instance.PositionX);
                        Console.WriteLine("{0:X4}", instance.PositionY);
                        Console.WriteLine("{0:X4}", instance.PositionZ);
                        Console.WriteLine("{0:X8}", instance.Rotation2);
                        Console.WriteLine("{0:X4}", instance.Unk01);
                        Console.WriteLine("{0:X4}", instance.Rotation1);
                    }
                    objectGroup.Objects[x] = obj;
                }
                objectGroups[i] = objectGroup;
            }
        }
    }
}