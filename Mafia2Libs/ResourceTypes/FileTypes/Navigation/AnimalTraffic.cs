using System.ComponentModel;
using System.IO;
using System.Numerics;
using Utils.Types;
using Utils.VorticeUtils;
using Vortice.Mathematics;

namespace ResourceTypes.Navigation
{
    public class AnimalTrafficLoader
    {
        private FileInfo file;

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class PathVectors
        {
            //sometimes a path can have three vectors with one byte; this is just helpful i guess.
            Vector3 position;
            Vector3 rotation;
            byte unk0;

            public Vector3 Position {
                get { return position; }
                set { position = value; }
            }
            public Vector3 Rotation {
                get { return rotation; }
                set { rotation = value; }
            }
            public byte Unk0 {
                get { return unk0; }
                set { unk0 = value; }
            }

            public override string ToString()
            {
                return string.Format("{0} {1} {2}", Position, Rotation, unk0);
            }
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class AnimalTrafficPath
        {
            private byte numPaths;
            private byte[] data0;
            private byte[] data1;
            private byte[] data2;
            private BoundingBox bbox;
            private HashName hash;
            private float unk0;
            private float unk1;
            private byte unk2;
            private byte[] unk3;
            private PathVectors[] vectors;

            public byte[] Data0 {
                get { return data0; }
                set { data0 = value; }
            }
            public byte[] Data1 {
                get { return data1; }
                set { data1 = value; }
            }
            public byte[] Data2 {
                get { return data2; }
                set { data2 = value; }
            }
            public HashName Hash {
                get { return hash; }
                set { hash = value; }
            }
            public BoundingBox BoundingBox {
                get { return bbox; }
                set { bbox = value; }
            }
            public float Unk0 {
                get { return unk0; }
                set { unk0 = value; }
            }
            public float Unk1 {
                get { return unk1; }
                set { unk1 = value; }
            }
            public byte Unk2 {
                get { return unk2; }
                set { unk2 = value; }
            }
            public byte[] Unk3 {
                get { return unk3; }
                set { unk3 = value; }
            }
            public PathVectors[] Vectors {
                get { return vectors; }
                set { vectors = value; }
            }

            public override string ToString()
            {
                return string.Format("{0} {1} {2} {3} {4}, {5}", hash.ToString(), Unk0, Unk1, Unk2, Unk2, bbox);
            }
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class AnimalTrafficInstance
        {
            private HashName name;

            public HashName Name {
                get { return name; }
                set { name = value; }
            }

            public override string ToString()
            {
                return string.Format("{0}", Name);
            }
        }

        private AnimalTrafficInstance[] instances;
        private AnimalTrafficPath[] paths;

        public AnimalTrafficInstance[] Instances {
            get { return instances; }
            set { instances = value; }
        }
        public AnimalTrafficPath[] Paths {
            get { return paths; }
            set { paths = value; }
        }

        private const short Magic = 0x5441;
        private const int Version = 0x5F1B1EC9;
        public AnimalTrafficLoader(FileInfo info)
        {
            file = info;
            using (BinaryReader reader = new BinaryReader(File.Open(info.FullName, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
            //WriteToFile();
        }

        public void ReadFromFile(BinaryReader reader)
        {
            if (reader.ReadInt16() != Magic)
                return;

            ushort animalInsCount = reader.ReadUInt16();
            instances = new AnimalTrafficInstance[animalInsCount];

            if (reader.ReadInt32() != Version)
                return;

            for (int i = 0; i < animalInsCount; i++)
            {
                AnimalTrafficInstance instance = new AnimalTrafficInstance();
                instance.Name = new HashName();
                instance.Name.ReadFromFile(reader);
                instances[i] = instance;
            }

            ushort pathCount = reader.ReadUInt16();
            paths = new AnimalTrafficPath[pathCount];

            for (int i = 0; i < pathCount; i++)
            {
                AnimalTrafficPath path = new AnimalTrafficPath();
                byte pathSize = reader.ReadByte();
                byte count1 = reader.ReadByte();
                byte count2 = reader.ReadByte();
                byte count3 = reader.ReadByte();
                path.Data0 = reader.ReadBytes(count1);
                path.Data1 = reader.ReadBytes(count2);
                path.Data2 = reader.ReadBytes(count3);
                path.BoundingBox = BoundingBoxExtenders.ReadFromFile(reader);
                path.Hash = new HashName();
                path.Hash.ReadFromFile(reader); //decompiled exe says this is a hash but its always empty
                path.Unk0 = reader.ReadSingle(); //5
                path.Unk1 = reader.ReadSingle(); //15
                path.Unk2 = reader.ReadByte(); //1 257 or 513.
                path.Unk3 = reader.ReadBytes(path.Unk2);
                path.Vectors = new PathVectors[pathSize];

                for(int x = 0; x < pathSize; x++)
                {
                    PathVectors vector = new PathVectors();
                    vector.Position = Vector3Utils.ReadFromFile(reader); //Very large differences between these two
                    vector.Rotation = Vector3Utils.ReadFromFile(reader); //2nd one could be rotation, in radians.
                    vector.Unk0 = reader.ReadByte(); //7 or 4
                    path.Vectors[x] = vector;
                }
 

                paths[i] = path;
            }
        }

        public void WriteToFile()
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(file.FullName+"1", FileMode.Create)))
            {
                writer.Write(Magic); //magic
                writer.Write((ushort)instances.Length);
                writer.Write(Version);

                for(int i = 0; i < instances.Length; i++)
                    instances[i].Name.WriteToFile(writer);

                writer.Write((ushort)paths.Length);

                for (int i = 0; i < paths.Length; i++)
                {
                    AnimalTrafficPath path = paths[i];
                    writer.Write((byte)path.Vectors.Length);
                    writer.Write((byte)path.Data0.Length);
                    writer.Write((byte)path.Data1.Length);
                    writer.Write((byte)path.Data2.Length);
                    writer.Write(path.Data0);
                    writer.Write(path.Data1);
                    writer.Write(path.Data2);
                    BoundingBoxExtenders.WriteToFile(path.BoundingBox, writer);
                    path.Hash.WriteToFile(writer);
                    writer.Write(path.Unk0);
                    writer.Write(path.Unk1);
                    writer.Write(path.Unk2);
                    writer.Write(path.Unk3);

                    for (int x = 0; x < path.Vectors.Length; x++)
                    {
                        Vector3Utils.WriteToFile(path.Vectors[x].Position, writer);
                        Vector3Utils.WriteToFile(path.Vectors[x].Rotation, writer);
                        writer.Write(path.Vectors[x].Unk0);
                    }
                }
            }
        }
    }
}
