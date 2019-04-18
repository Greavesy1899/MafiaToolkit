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

        private void DecompressRotation(structInstance instance)
        {
            Vector4 quat = new Vector4();
            float v6 = 0.0f;
            float v7 = 0.0f;
            float v8 = 0.0f;
            float v9 = 0.0f;

            if ((instance.Rotation2 & 0x10) != 0)
            {
                int val = instance.Rotation2 & 0x1FF;
                v7 = 0.0019569471f;
                v6 = val * 0.0019569471f;
                v8 = ((instance.Rotation2 >> 9) & 0x1FF) * v7;
                v9 = (instance.Rotation2 >> 18) & 0x1FF;
            }
            else
            {
                int val = ((instance.Rotation1 & 0xE0) << 4) | (instance.Rotation2 & 0x1FF);
                v6 = val * 0.00024420026f;
                v7 = 0.00012208521f;
                v8 = Convert.ToSingle((((instance.Rotation2 >> 9) & 0x1FF) + 2 * (instance.Rotation1 & 0xF00)) * 0.00012208521);
                v9 = ((instance.Rotation2 >> 18) & 0x1FF | (instance.Rotation1 >> 3) & 0x1E00);
            }
            double v11 = (v6 * Math.PI) * 2;
            double v12 = Math.Sin(v11 + -Math.PI) * 0.5f;
            double v13 = Math.Cos(v11 + -Math.PI) * 0.5f;
            double v14 = (((v8 * Math.PI) + (v8 * Math.PI)) + -Math.PI) * 0.5f;
            double v15 = Math.Sin(v14);
            double v16 = Math.Cos(v14);
            double v17 = ((((v9 * v7) * Math.PI) + ((v9 * v7) * Math.PI)) + -Math.PI) * 0.5;
            double v18 = Math.Sin(v17);
            double v19 = Math.Cos(v17);
            quat.X = Convert.ToSingle((v13 * (v15 * v18)) + (v12 * (v16 * v19)));
            quat.Y = Convert.ToSingle((v13 * (v15 * v19)) + (v12 * (v16 * v18)));
            quat.Z = Convert.ToSingle(((v16 * v18) * v13) - ((v15 * v19) * v12));
            quat.W = Convert.ToSingle(((v16 * v19) * v13) - ((v15 * v18) * v12));
        }

        private void DecompressPosition(structInstance instance)
        {
            Vector3 position = new Vector3();
            position = new Vector3(instance.PositionX / 65535.0f, instance.PositionY / 65535.0f, instance.PositionZ / 65535.0f);
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
                        DecompressRotation(instance);
                        DecompressPosition(instance);
                        obj.Instances[y] = instance;

                        Console.WriteLine("{0:X4}", instance.PositionX);
                        Console.WriteLine("{0:X4}", instance.PositionY);
                        Console.WriteLine("{0:X4}", instance.PositionZ);
                        Console.WriteLine("{0:X8}", instance.Rotation2);
                        Console.WriteLine("{0}", instance.Unk01);
                        Console.WriteLine("{0:X4}", instance.Rotation1);
                    }
                    objectGroup.Objects[x] = obj;
                }
                objectGroups[i] = objectGroup;
            }
        }
    }
}