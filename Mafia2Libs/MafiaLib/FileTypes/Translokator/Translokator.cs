using SharpDX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Utils.Extensions;
using Utils.SharpDXExtensions;
using Utils.StringHelpers;

namespace ResourceTypes.Translokator
{
    public class Grid
    {
        short key;
        Vector3 origin;
        Vector2 cellSize;
        int width;
        int height;
        short[] data;

        public short Key {
            get { return key; }
            set { key = value; }
        }
        [TypeConverter(typeof(Vector3Converter))]
        public Vector3 Origin {
            get { return origin; }
            set { origin = value; }
        }
        [TypeConverter(typeof(Vector2Converter))]
        public Vector2 CellSize {
            get { return cellSize; }
            set { cellSize = value; }
        }
        public int Width {
            get { return width; }
            set { width = value; }
        }
        public int Height {
            get { return height; }
            set { height = value; }
        }
        public short[] Data {
            get { return data; }
            set { data = value; }
        }
    }

    public class Instance
    {
        Vector3 position;
        Vector4 rotation;
        float scale;
        public ushort PositionX;
        public ushort PositionY;
        public ushort PositionZ;
        int rotation2;
        ushort unk01;
        ushort rotation1;

        [TypeConverter(typeof(Vector3Converter))]
        public Vector3 Position {
            get { return position; }
            set { position = value; }
        }

        public Vector4 Rotation {
            get { return rotation; }
            set { rotation = value; }
        }

        public float Scale {
            get { return scale; }
            set { scale = value; }
        }

        public ushort Unk01 {
            get { return unk01; }
            set { unk01 = value; }
        }

        public ushort Rotation1 {
            get { return rotation1; }
            set { rotation1 = value; }
        }

        public int Rotation2 {
            get { return rotation2; }
            set { rotation2 = value; }
        }
    }

    public class Object
    {
        short unk01;
        short unk02;
        ulong hash;
        string name;
        byte[] unkBytes1;
        float unk03;
        float unk04;
        int numInstances;
        Instance[] instances;

        public short Unk01 {
            get { return unk01; }
            set { unk01 = value; }
        }
        public short Unk02 {
            get { return unk02; }
            set { unk02 = value; }
        }
        public ulong Hash {
            get { return hash; }
            set { hash = value; }
        }
        public string Name {
            get { return name; }
            set { name = value; }
        }
        public byte[] UnkBytes1 {
            get { return unkBytes1; }
            set { unkBytes1 = value; }
        }
        public float Unk03 {
            get { return unk03; }
            set { unk03 = value; }
        }
        public float Unk04 {
            get { return unk04; }
            set { unk04 = value; }
        }
        [Browsable(false)]
        public int NumInstances {
            get { return numInstances; }
            set { numInstances = value; }
        }
        [Browsable(false)]
        public Instance[] Instances {
            get { return instances; }
            set { instances = value; }
        }
    }

    public class ObjectGroup
    {
        short unk01;
        int numObjects;
        Object[] objects;

        public short Unk01 {
            get { return unk01; }
            set { unk01 = value; }
        }
        [Browsable(false)]
        public int NumObjects {
            get { return numObjects; }
            set { numObjects = value; }
        }
        [Browsable(false)]
        public Object[] Objects {
            get { return objects; }
            set { objects = value; }
        }
    }

    public class TranslokatorLoader
    {
        public Grid[] Grids;
        public ObjectGroup[] ObjectGroups;
        List<ushort> IDs = new List<ushort>();

        int version;
        int unk1;
        short unk2;
        BoundingBox bounds;
        int numGrids;
        int numUnk3;
        short[] unk3;
        int numObjectGroups;

        public int Version {
            get { return version; }
            set { version = value; }
        }
        public int Unk1 {
            get { return unk1; }
            set { unk1 = value; }
        }
        public short Unk2 {
            get { return unk2; }
            set { unk2 = value; }
        }
        public BoundingBox Bounds {
            get { return bounds; }
            set { bounds = value; }
        }
        public short[] Unk3 {
            get { return unk3; }
            set { unk3 = value; }
        }

        public TranslokatorLoader(FileInfo info)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(info.FullName, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
        }

        private void DecompressRotation(Instance instance)
        {
            Vector4 quat = new Vector4();
            var v6 = 0.0f;
            var v7 = 0.0;
            var v8 = 3.141592741012573;
            var Y = 0.0;
            var X = 0.0;

            if ((instance.Rotation1 & 0x10) != 0)
            {
                var v4 = instance.Rotation2 >> 9;
                var v5 = v4 & 0x1FF;
                X = (instance.Rotation2 & 0x1FF) * 0.001956947147846222 * v8 * 2.0 - v8;
                Y = v5 * 0.001956947147846222 * v8 * 2.0 - v8; //0.001956947147846222 == 1.0f/511.0f
                v7 = 0.001956947147846222 * ((v4 >> 9) & 0x1FF) * v8;
            }
            else
            {
                var v9 = instance.Rotation2 & 0x1FF | 16 * (instance.Rotation1 & 0xE0);
                var v10 = instance.Rotation2 >> 9;
                var v5 = v10 & 0x1FF | 2 * (instance.Rotation1 & 0xF00);
                X = v9 * 0.0002442002587486058 * v8 * 2.0 - v8; //0.0002442002587486058 == 1.0f/8191.0f
                Y = v5 * 0.0001220852136611938 * v8 * 2.0 - v8; //0.0001220852136611938 == 1.0f/4095.0f
                v7 = 0.0001220852136611938 * ((instance.Rotation1 & 0xF000 | (v10 >> 6) & 0xFF8) >> 3) * v8;
            }

            var Z = 2.0f * v7 - v8;
            CompressRotation(new Vector3((float)X, (float)Y, (float)Z), instance);
        }
        private void CompressRotation(Vector3 rotation, Instance instance)
        {
            double x;
            double y;
            double z;

            if (instance.Scale == 1)
            {
                x = (rotation.X / Math.PI + 1.0f) / 2.0f * 511.0f;
                y = (rotation.Y / Math.PI + 1.0f) / 2.0f * 511.0f;
                z = (rotation.Z / Math.PI + 1.0f) / 2.0f * 511.0f;
            }
            else
            {
                x = (rotation.X / Math.PI + 1.0f) / 2.0f * 8191.0f;
                y = (rotation.Y / Math.PI + 1.0f) / 2.0f * 4095.0f;
                z = (rotation.Z / Math.PI + 1.0f) / 2.0f * 4095.0f;
            }
        }

        public Vector3 DecompressPosition(byte[] transform, Instance instance, Vector3 tmin, Vector3 tmax, bool debug = false)
        {
            if(debug)
                Console.WriteLine("DECOMPRESSING:");

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

            if (debug)
            {
                Console.WriteLine("Instance; {0} {1} {2}", x, y, z);
                Console.WriteLine("Instance; {0} {1} {2}", xFrac, yFrac, zFrac);
                Console.WriteLine("Final Position: {0} {1} {2}", position.X, position.Y, position.Z);
            }
            return position;
        }
        public void CompressPosition(Instance instance, Vector3 tmin, Vector3 tmax, bool debug = false)
        {
            if(debug)
                Console.WriteLine("COMPRESSING:");
            var vector = instance.Position;
            var xPositive = vector.X >= 0.0f ? true : false;
            var yPositive = vector.Y >= 0.0f ? true : false;
            var zPositive = vector.Z >= 0.0f ? true : false;
            Vector3 tref = new Vector3((xPositive ? tmax.X : tmin.X) * 0.25f, (yPositive ? tmax.Y : tmin.Y) * 0.25f, (zPositive ? tmax.Z : tmin.Z) * 0.25f);
            Vector3 preFinal = new Vector3(vector.X / tref.X, vector.Y / tref.Y, vector.Z / tref.Z);
            preFinal.X = Math.Abs(preFinal.X);
            preFinal.Y = Math.Abs(preFinal.Y);
            preFinal.Z = Math.Abs(preFinal.Z);
            var xFrac = preFinal.X > 1.0f ? preFinal.X - Math.Floor(preFinal.X) : preFinal.X;
            var yFrac = preFinal.Y > 1.0f ? preFinal.Y - Math.Floor(preFinal.Y) : preFinal.Y;
            var zFrac = preFinal.Z > 1.0f ? preFinal.Z - Math.Floor(preFinal.Z) : preFinal.Z;
            var preX = xFrac * 65535.0f;
            var preY = yFrac * 65535.0f;
            var preZ = zFrac * 65535.0f;
            instance.PositionX = Convert.ToUInt16(preX);
            instance.PositionY = Convert.ToUInt16(preY);
            instance.PositionZ = Convert.ToUInt16(preZ);

            if (debug)
            {
                Console.WriteLine("Frac: {0} {1} {2}", xFrac, yFrac, zFrac);
                Console.WriteLine("Final: {0} {1} {2}", instance.Position.X, instance.Position.Y, instance.Position.Z);
                Console.WriteLine("Final Instance: {0} {1} {2}", instance.PositionX, instance.PositionY, instance.PositionZ);
            }
        }
        public void DecompressScale(Instance transform)
        {
            var scale = 1.0f;

            if ((transform.Rotation1 & 0x10) != 0)
            {
                var s = transform.Rotation1 & 0xFFE0;
                var e = (s >> 10) & 0x1F;
                if (e != 0)
                {
                    var sign = (s << 16) & 0x80000000;
                    var exponent = ((e + 127 - 15) << 23);
                    var mantissa = (s << 13) & 0x7C0000;
                    var t = sign | exponent | mantissa;
                    scale = Convert.ToSingle(t);
                }
            }
            Console.WriteLine(scale);
            transform.Scale = scale;
        }

        private void CompressInstances()
        {
            ushort numInstance = 0;
            for (int i = 0; i != ObjectGroups.Length; i++)
            {
                ObjectGroup objectGroup = ObjectGroups[i];

                for (int x = 0; x != objectGroup.NumObjects; x++)
                {
                    Object obj = objectGroup.Objects[x];

                    for (int y = 0; y != obj.NumInstances; y++)
                    {
                        Instance instance = obj.Instances[y];
                        instance.Unk01 = numInstance;
                        CompressPosition(instance, bounds.Minimum, bounds.Maximum);
                        numInstance++;
                    }
                }
            }
        }

        public void ReadFromFile(BinaryReader reader)
        {
            version = reader.ReadInt32();
            unk1 = reader.ReadInt32();
            unk2 = reader.ReadInt16();
            bounds = BoundingBoxExtenders.ReadFromFile(reader);
            numGrids = reader.ReadInt32();
            
            Grids = new Grid[numGrids];

            for (int i = 0; i < numGrids; i++)
            {
                Grid grid = new Grid();
                grid.Key = reader.ReadInt16();
                grid.Origin = Vector3Extenders.ReadFromFile(reader);
                grid.CellSize = Vector2Extenders.ReadFromFile(reader);
                grid.Width = reader.ReadInt32();
                grid.Height = reader.ReadInt32();
                grid.Data = new short[grid.Width * grid.Height];

                for (int y = 0; y != grid.Data.Length; y++)
                    grid.Data[y] = reader.ReadInt16();

                Grids[i] = grid;
            }

            numUnk3 = reader.ReadInt32();
            unk3 = new short[numUnk3 * 2];

            for (int i = 0; i < numUnk3 * 2; i++)
                unk3[i] = reader.ReadInt16();

            numObjectGroups = reader.ReadInt32();
            ObjectGroups = new ObjectGroup[numObjectGroups];
            //Console.WriteLine("Num Object Groups: {0}", numObjectGroups);

            for (int i = 0; i != numObjectGroups; i++)
            {
                //Console.WriteLine("Object Groups: {0}", i);
                ObjectGroup objectGroup = new ObjectGroup();
                objectGroup.Unk01 = reader.ReadInt16();
                objectGroup.NumObjects = reader.ReadInt32();
                objectGroup.Objects = new Object[objectGroup.NumObjects];
                // Console.WriteLine("Num Objects: {0}", objectGroup.NumObjects);

                for (int x = 0; x != objectGroup.NumObjects; x++)
                {
                    //Console.WriteLine("Object: {0}", x);
                    Object obj = new Object();
                    obj.Unk01 = reader.ReadInt16();
                    obj.Unk02 = reader.ReadInt16();
                    obj.Hash = reader.ReadUInt64();
                    obj.Name = StringHelpers.ReadString(reader);
                    obj.UnkBytes1 = reader.ReadBytes(31 - obj.Name.Length);
                    obj.Unk03 = reader.ReadSingle();
                    obj.Unk04 = reader.ReadSingle();
                    obj.NumInstances = reader.ReadInt32();
                    obj.Instances = new Instance[obj.NumInstances];
                    Console.WriteLine("Name: {0}", obj.Name);
                    //Console.WriteLine("Num Instances: {0}", obj.NumInstances);

                    for (int y = 0; y != obj.NumInstances; y++)
                    {
                        //Console.WriteLine("Instance: {0}", y);
                        byte[] packed = reader.ReadBytes(14);
                        Instance instance = new Instance();
                        instance.PositionX = BitConverter.ToUInt16(packed, 0);
                        instance.PositionY = BitConverter.ToUInt16(packed, 2);
                        instance.PositionZ = BitConverter.ToUInt16(packed, 4);
                        instance.Rotation2 = BitConverter.ToInt32(packed, 6);
                        instance.Unk01 = BitConverter.ToUInt16(packed, 10);
                        instance.Rotation1 = BitConverter.ToUInt16(packed, 12);

                        if (!IDs.Contains(instance.Unk01))
                            IDs.Add(instance.Unk01);
                        else
                            Console.WriteLine("Duplication!! {0} {1}", obj.Name, instance.Unk01);

                        //Console.WriteLine("{0:X4}", instance.PositionX);
                        //Console.WriteLine("{0:X4}", instance.PositionY);
                        //Console.WriteLine("{0:X4}", instance.PositionZ);
                        //Console.WriteLine("{0:X8}", instance.Rotation2);
                        //Console.WriteLine("{0:X4}", instance.Unk01);
                        //Console.WriteLine("{0:X4}", instance.Rotation1);

                        DecompressScale(instance);
                        DecompressRotation(instance);                    
                        instance.Position = DecompressPosition(packed, instance, bounds.Minimum, bounds.Maximum);
                        //Console.WriteLine(instance.Position);
                        obj.Instances[y] = instance;


                    }
                    objectGroup.Objects[x] = obj;
                }
                ObjectGroups[i] = objectGroup;
            }
            IDs.Sort();
        }

        public void WriteToFile(FileInfo info)
        {
            CompressInstances();

            using (BinaryWriter writer = new BinaryWriter(File.Open(info.FullName, FileMode.Create)))
            {
                InternalWriteToFile(writer);
            }
        }
        private void InternalWriteToFile(BinaryWriter writer)
        {

            writer.Write(version);
            writer.Write(unk1);
            writer.Write(unk2);
            BoundingBoxExtenders.WriteToFile(bounds, writer);
            writer.Write(Grids.Length);

            for (int i = 0; i < Grids.Length; i++)
            {
                Grid grid = Grids[i];
                writer.Write(grid.Key);
                Vector3Extenders.WriteToFile(grid.Origin, writer);
                Vector2Extenders.WriteToFile(grid.CellSize, writer);
                writer.Write(grid.Width);
                writer.Write(grid.Height);

                for (int y = 0; y != grid.Data.Length; y++)
                    writer.Write(grid.Data[y]);
            }

            writer.Write(unk3.Length/2);
            for (int i = 0; i < unk3.Length; i++)
                writer.Write(unk3[i]);

            writer.Write(ObjectGroups.Length);
            for (int i = 0; i != ObjectGroups.Length; i++)
            {
                ObjectGroup objectGroup = ObjectGroups[i];
                writer.Write(objectGroup.Unk01);
                writer.Write(objectGroup.Objects.Length);

                for (int x = 0; x != objectGroup.NumObjects; x++)
                {
                    Object obj = objectGroup.Objects[x];
                    writer.Write(obj.Unk01);
                    writer.Write(obj.Unk02);
                    writer.Write(obj.Hash);
                    StringHelpers.WriteString(writer, obj.Name);
                    writer.Write(obj.UnkBytes1);
                    writer.Write(obj.Unk03);
                    writer.Write(obj.Unk04);
                    writer.Write(obj.NumInstances);

                    for (int y = 0; y != obj.NumInstances; y++)
                    {
                        Instance instance = obj.Instances[y];
                        writer.Write(instance.PositionX);
                        writer.Write(instance.PositionY);
                        writer.Write(instance.PositionZ);
                        writer.Write(instance.Rotation2);
                        writer.Write(instance.Unk01);
                        writer.Write(instance.Rotation1);
                    }
                    objectGroup.Objects[x] = obj;
                }
                ObjectGroups[i] = objectGroup;
            }
        }
    }
}