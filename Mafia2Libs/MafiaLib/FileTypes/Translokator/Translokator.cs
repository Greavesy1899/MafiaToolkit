using SharpDX;
using System;
using System.IO;
using Utils.SharpDXExtensions;
using Utils.StringHelpers;

namespace ResourceTypes.Translokator
{
    public struct structGrid
    {
        public short Key;
        public Vector3 Origin;
        public Vector2 CellSize;
        public int Width;
        public int Height;
        public short[] data;
    }

    public struct structInstance
    {
        public Vector3 Position;
        public short PositionX;
        public short PositionY;
        public short PositionZ;
        public int Rotation2;
        public short Unk01;
        public int Rotation1;
    }

    public struct structObject
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

    public struct structObjectGroup
    {
        public short Unk01;
        public int NumObjects;
        public structObject[] Objects;
    }

    public class TranslokatorLoader
    {
        public structGrid[] Grids;
        public structObjectGroup[] ObjectGroups;

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
            Vector3 euler = new Vector3(v6, v7, v8);
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
            Console.WriteLine("ACTUAL ROTATION: {0}", quat);
        }

        private void CompressRotation(Quaternion quat)
        {
            //float heading, attitude, bank;
            //double sqw = quat.W * quat.W;
            //double sqx = quat.X * quat.X;
            //double sqy = quat.Y * quat.Y;
            //double sqz = quat.Z * quat.Z;
            //double unit = sqx + sqy + sqz + sqw; // if normalised is one, otherwise is correction factor
            //double test = quat.X * quat.Y + quat.Z * quat.W;
            //if (test > 0.499 * unit)
            //{ // singularity at north pole
            //    heading = (float)(2 * Math.Atan2(quat.X, quat.W));
            //    attitude = (float)(Math.PI / 2);
            //    bank = 0;
            //    return;
            //}
            //if (test < -0.499 * unit)
            //{ // singularity at south pole
            //    heading = -(float)(2 * Math.Atan2(quat.X, quat.W));
            //    attitude = -(float)(Math.PI / 2);
            //    bank = 0;
            //    return;
            //}
            //heading = (float)Math.Atan2(2 * quat.Y * quat.W - 2 * quat.X * quat.Z, sqx - sqy - sqz + sqw);
            //attitude = (float)Math.Asin(2 * test / unit);
            //bank = (float)Math.Atan2(2 * quat.X * quat.W - 2 * quat.Y * quat.Z, -sqx + sqy - sqz + sqw);
        }

        private Vector3 DecompressPosition(byte[] transform, Vector3 tmin, Vector3 tmax)
        {
            Vector3 position = new Vector3();
            var _1_65535 = 1.0f / 65535.0f;
            ushort x = BitConverter.ToUInt16(transform, 0);
            ushort y = BitConverter.ToUInt16(transform, 2);
            ushort z = BitConverter.ToUInt16(transform, 4);
            var xFrac = x * _1_65535;
            var yFrac = y * _1_65535;
            var zFrac = z * _1_65535;
            var xFull = ((transform[9] >> 6) & 3);
            var yfull = (float)(transform[12] & 3);
            var zfull = (float)((transform[12] >> 2) & 3);
            var xpositive = (transform[9] & 0x08) == 0;
            var ypositive = (transform[9] & 0x10) == 0;
            var zpositive = (transform[9] & 0x20) == 0;
            Vector3 tref = new Vector3((xpositive ? tmax.X : -tmin.X) * 0.25f, (ypositive ? tmax.Y : -tmin.Y) * 0.25f, (zpositive ? tmax.Z : -tmin.Z) * 0.25f);
            Vector3 final = new Vector3((xFull + xFrac) * tref.X, (yfull + yFrac) * tref.Y, (zfull + zFrac) * tref.Z);
            position = new Vector3(xpositive ? final.X : -final.X, ypositive ? final.Y : -final.Y, zpositive ? final.Z : -final.Z);
            //CompressPosition(position, tmin, tmax);
            //Console.WriteLine("ACTUAL POSITION: {0}", position);
            return position;
        }

        private void CompressPosition(Vector3 vector, Vector3 tmin, Vector3 tmax)
        {
            var xPositive = vector.X >= 0.0f ? true : false;
            var yPositive = vector.Y >= 0.0f ? true : false;
            var zPositive = vector.Z >= 0.0f ? true : false;
            Vector3 tref = new Vector3((xPositive ? tmax.X : -tmin.X) * 0.25f, (yPositive ? tmax.Y : -tmin.Y) * 0.25f, (zPositive ? tmax.Z : -tmin.Z) * 0.25f);
            Vector3 preFinal = new Vector3(vector.X / tref.X, vector.Y / tref.Y, vector.Z / tref.Z);
            var xFrac = preFinal.X > 1.0f ? preFinal.X - Math.Floor(preFinal.X) : preFinal.X;
            var yFrac = preFinal.Y > 1.0f ? preFinal.X - Math.Floor(preFinal.Y) : preFinal.Y;
            var zFrac = preFinal.Z > 1.0f ? preFinal.X - Math.Floor(preFinal.Z) : preFinal.Z;
            var preX = xFrac * 65535.0f;
            var preY = yFrac * 65535.0f;
            var preZ = zFrac * 65535.0f;
            ushort x = Convert.ToUInt16(preX);
            ushort y = Convert.ToUInt16(preY);
            ushort z = Convert.ToUInt16(preZ);

        }

        public void ReadFromFile(BinaryReader reader)
        {
            int Version = reader.ReadInt32();
            int unk1 = reader.ReadInt32();
            short unk2 = reader.ReadInt16();

            BoundingBox bounds = BoundingBoxExtenders.ReadFromFile(reader);

            int unk3 = reader.ReadInt32();
            Grids = new structGrid[unk3];

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

                Grids[i] = grid;
            }

            int unk3Count = reader.ReadInt32();
            short[] unks3 = new short[unk3Count * 2];

            for (int i = 0; i != unk3Count * 2; i++)
            {
                unks3[i] = reader.ReadInt16();
            }

            int numObjectGroups = reader.ReadInt32();
            ObjectGroups = new structObjectGroup[numObjectGroups];
            //Console.WriteLine("Num Object Groups: {0}", numObjectGroups);

            for (int i = 0; i != numObjectGroups; i++)
            {
                //Console.WriteLine("Object Groups: {0}", i);
                structObjectGroup objectGroup = new structObjectGroup();
                objectGroup.Unk01 = reader.ReadInt16();
                objectGroup.NumObjects = reader.ReadInt32();
                objectGroup.Objects = new structObject[objectGroup.NumObjects];
               // Console.WriteLine("Num Objects: {0}", objectGroup.NumObjects);

                for (int x = 0; x != objectGroup.NumObjects; x++)
                {
                    //Console.WriteLine("Object: {0}", x);
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
                    //Console.WriteLine("Name: {0}", obj.Name);
                    //Console.WriteLine("Num Instances: {0}", obj.NumInstances);

                    for (int y = 0; y != obj.NumInstances; y++)
                    {
                        //Console.WriteLine("Instance: {0}", y);
                        byte[] packed = reader.ReadBytes(14);
                        structInstance instance = new structInstance();
                        instance.PositionX = BitConverter.ToInt16(packed, 0);
                        instance.PositionY = BitConverter.ToInt16(packed, 2);
                        instance.PositionZ = BitConverter.ToInt16(packed, 4);
                        instance.Rotation2 = BitConverter.ToInt32(packed, 6);
                        instance.Unk01 = BitConverter.ToInt16(packed, 10);
                        
                        instance.Rotation1 = BitConverter.ToInt16(packed, 12);

                        //Console.WriteLine("{0:X4}", instance.PositionX);
                        //Console.WriteLine("{0:X4}", instance.PositionY);
                        //Console.WriteLine("{0:X4}", instance.PositionZ);
                        //Console.WriteLine("{0:X8}", instance.Rotation2);
                        //Console.WriteLine("{0:X4}", instance.Unk01);
                        //Console.WriteLine("{0:X4}", instance.Rotation1);

                        //DecompressRotation(instance);                    
                        instance.Position = DecompressPosition(packed, bounds.Minimum, bounds.Maximum);
                        obj.Instances[y] = instance;


                    }
                    objectGroup.Objects[x] = obj;
                }
                ObjectGroups[i] = objectGroup;
            }
        }
    }
}