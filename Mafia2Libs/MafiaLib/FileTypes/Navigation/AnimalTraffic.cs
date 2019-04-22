using SharpDX;
using System.IO;
using Utils.SharpDXExtensions;
using Utils.Types;

namespace ResourceTypes.Navigation
{
    public class AnimalTrafficLoader
    {
        private FileInfo file;

        public struct PathVectors
        {
            //sometimes a path can have three vectors with one byte; this is just helpful i guess.
            public Vector3[] vectors;
            public byte unk0;

            public override string ToString()
            {
                return string.Format("{0} {1} {2}", vectors[0], vectors[1], unk0);
            }
        }
        public struct AnimalTrafficPath
        {
            public byte numPaths;
            public byte[] unkSet0;
            public byte[] unkSet1;
            public byte[] unkSet2;
            public BoundingBox bbox;
            public Hash unkHash;
            public float unk0;
            public float unk1;
            public short unk2;
            public PathVectors[] vectors;
            public byte unk3;


            public override string ToString()
            {
                return string.Format("{0} {1} {2} {3} {4}, {5}", unkHash.ToString(), unk0, unk1, unk2, unk3, bbox);
            }
        }
        public struct AnimalTrafficInstance
        {
            public Hash Name;

            public override string ToString()
            {
                return string.Format("{0}", Name);
            }
        }

        AnimalTrafficInstance[] instances;
        public AnimalTrafficPath[] paths;

        public AnimalTrafficLoader(FileInfo info)
        {
            file = info;
            using (BinaryReader reader = new BinaryReader(File.Open(info.FullName, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
            WriteToFile();
        }

        public void ReadFromFile(BinaryReader reader)
        {
            if (reader.ReadInt16() != 21569)
                return;

            ushort animalInsCount = reader.ReadUInt16();
            instances = new AnimalTrafficInstance[animalInsCount];

            if (reader.ReadInt32() != 1595612873)
                return;

            for (int i = 0; i < animalInsCount; i++)
            {
                AnimalTrafficInstance instance = new AnimalTrafficInstance();
                instance.Name = new Hash();
                instance.Name.ReadFromFile(reader);
                instances[i] = instance;
            }

            ushort pathCount = reader.ReadUInt16();
            paths = new AnimalTrafficPath[pathCount];

            for (int i = 0; i < pathCount; i++)
            {
                AnimalTrafficPath path = new AnimalTrafficPath();
                path.numPaths = reader.ReadByte();
                byte count1 = reader.ReadByte();
                byte count2 = reader.ReadByte();
                byte count3 = reader.ReadByte();
                path.unkSet0 = reader.ReadBytes(count1);
                path.unkSet1 = reader.ReadBytes(count2);
                path.unkSet2 = reader.ReadBytes(count3);
                path.bbox = BoundingBoxExtenders.ReadFromFile(reader);
                path.unkHash = new Hash();
                path.unkHash.ReadFromFile(reader); //decompiled exe says this is a hash but its always empty
                path.unk0 = reader.ReadSingle(); //5
                path.unk1 = reader.ReadSingle(); //15
                path.unk2 = reader.ReadInt16(); //1 257 or 513.
                path.vectors = new PathVectors[path.numPaths];

                for(int x = 0; x < path.numPaths; x++)
                {
                    PathVectors vector = new PathVectors();
                    vector.vectors = new Vector3[2];
                    vector.vectors[0] = Vector3Extenders.ReadFromFile(reader); //Very large differences between these two
                    vector.vectors[1] = Vector3Extenders.ReadFromFile(reader); //2nd one could be rotation, in radians.
                    vector.unk0 = reader.ReadByte(); //7 or 4
                    path.vectors[x] = vector;
                }
                paths[i] = path;
            }
        }

        public void WriteToFile()
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(file.FullName+"1", FileMode.Create)))
            {
                writer.Write((ushort)21569); //magic
                writer.Write((ushort)instances.Length);
                writer.Write(1595612873);

                for(int i = 0; i < instances.Length; i++)
                    instances[i].Name.WriteToFile(writer);

                writer.Write((ushort)paths.Length);

                for (int i = 0; i < paths.Length; i++)
                {
                    AnimalTrafficPath path = paths[i];
                    writer.Write(path.numPaths);
                    writer.Write((byte)path.unkSet0.Length);
                    writer.Write((byte)path.unkSet1.Length);
                    writer.Write((byte)path.unkSet2.Length);
                    writer.Write(path.unkSet0);
                    writer.Write(path.unkSet1);
                    writer.Write(path.unkSet2);
                    BoundingBoxExtenders.WriteToFile(path.bbox, writer);
                    path.unkHash.WriteToFile(writer);
                    writer.Write(path.unk0);
                    writer.Write(path.unk1);
                    writer.Write(path.unk2);

                    for (int x = 0; x < path.numPaths; x++)
                    {
                        Vector3Extenders.WriteToFile(path.vectors[x].vectors[0], writer);
                        Vector3Extenders.WriteToFile(path.vectors[x].vectors[1], writer);
                        writer.Write(path.vectors[x].unk0);
                    }
                }
            }
        }
    }
}
