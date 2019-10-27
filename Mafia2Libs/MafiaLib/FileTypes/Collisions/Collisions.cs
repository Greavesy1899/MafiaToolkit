using System;
using System.Collections.Generic;
using System.IO;
using ResourceTypes.Collisions.Opcode;
using SharpDX;
using Utils.SharpDXExtensions;

namespace ResourceTypes.Collisions
{
    public class Collision
    {
        private const int Version = 0x11;
        // TODO: Most likely it's platform (== 0 on PC/Mac, == 1 on XBox360, == 2 on PS3)
        private const int Unk0 = 0;

        public string Name { get; set; }

        public List<Placement> Placements { get; private set; }  = new List<Placement>(); 
        public Dictionary<ulong, CollisionModel> Models { get; private set; } = new Dictionary<ulong,CollisionModel>();


        public Collision()
        {
        }

        public Collision(string fileName)
        {
            this.Name = fileName;
            using (BinaryReader reader = new BinaryReader(File.Open(this.Name, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
        }

        public void ReadFromFile(BinaryReader reader)
        {
            if (reader.ReadInt32() != Version)
            { 
                throw new Exception("Unknown collision version");
            }

            if (reader.ReadInt32() != Unk0)
            { 
                throw new Exception("Unknown collision header");
            }

            int numPlacements = reader.ReadInt32();
            Placements = new List<Placement>(numPlacements);
            for (int i = 0; i < numPlacements; i++)
                Placements.Add(new Placement(reader));

            int numModels = reader.ReadInt32();
            Models = new Dictionary<ulong, CollisionModel>();
            for (int i = 0; i < numModels; i++)
            {
                CollisionModel model = new CollisionModel(reader);
                Models.Add(model.Hash, model);
            }
        }

        public void WriteToFile()
        {
            if (string.IsNullOrEmpty(Name))
            {
                throw new Exception("Name is null or empty");
            }

            using (BinaryWriter writer = new BinaryWriter(File.Open(Name, FileMode.Create)))
            {
                writer.Write(Version);
                writer.Write(Unk0);

                writer.Write(Placements.Count);
                foreach (Placement placement in Placements)
                {
                    placement.WriteToFile(writer);
                }

                writer.Write(Models.Count);
                // NOTE: Models should be sorted by hash (ascending)
                // TODO rtfm and maybe migrate to System.Collections.Generic.SortedList<TKey,TValue>
                List<ulong> keys = new List<ulong>(Models.Keys);
                keys.Sort();
                foreach (var key in keys)
                {
                    Models[key].WriteToFile(writer);
                }
            }
        }

        public void WriteToFile(string name)
        {
            this.Name = name;
            WriteToFile();
        }

        public class Placement
        {
            public Vector3 Position { get; set; }

            private Vector3 rotation;
            public Vector3 Rotation
            {
                get { return rotation; }
                set { rotation = value; }
            }

            public ulong Hash { get; set; }
            public int Unk4 { get; set; }
            public byte Unk5 { get; set; }

            public Placement(BinaryReader reader)
            {
                ReadFromFile(reader);
            }

            public Placement()
            {
            }

            public Placement(Placement other)
            {
                Position = other.Position;
                rotation = other.rotation;
                Hash = other.Hash;
                Unk4 = other.Unk4;
                Unk5 = other.Unk5;
            }

            public void ReadFromFile(BinaryReader reader)
            {
                Position = Vector3Extenders.ReadFromFile(reader);
                rotation = Vector3Extenders.ReadFromFile(reader);
                Vector3 rot = new Vector3();
                rot.X = MathUtil.RadiansToDegrees(rotation.X);
                rot.Y = MathUtil.RadiansToDegrees(rotation.Y);
                rot.Z = -MathUtil.RadiansToDegrees(rotation.Z);
                rotation = rot;
                Hash = reader.ReadUInt64();
                Unk4 = reader.ReadInt32();
                Unk5 = reader.ReadByte();
            }

            public void WriteToFile(BinaryWriter writer)
            {
                Position.WriteToFile(writer);
                Vector3 rot = new Vector3();
                rot.X = MathUtil.DegreesToRadians(rotation.X);
                rot.Y = MathUtil.DegreesToRadians(rotation.Y);
                rot.Z = -MathUtil.DegreesToRadians(rotation.Z);
                rot.WriteToFile(writer);
                writer.Write(Hash);
                writer.Write(Unk4);
                writer.Write(Unk5);
            }

            public override string ToString()
            {
                return string.Format("{0}, {1}, {2}", Hash, Unk4, Unk5);
            }
        }

        public class CollisionModel
        {
            public ulong Hash { get; set; }
            public TriangleMesh Mesh { get; set; }
            
            protected Section[] sections;
            public Section[] Sections
            {
                get { return sections; }
                set { sections = value; }
            }

            public CollisionModel(BinaryReader reader)
            {
                ReadFromFile(reader);
            }

            public CollisionModel()
            {
                Hash = 0;
                Mesh = new TriangleMesh();;
                sections = new Section[1];
            }

            public void ReadFromFile(BinaryReader reader)
            {
                Hash = reader.ReadUInt64();

                int dataSize = reader.ReadInt32();
                Mesh = new TriangleMesh();
                Mesh.Load(reader);

                int length = reader.ReadInt32();
                sections = new Section[length];
                for (int i = 0; i != sections.Length; i++)
                    sections[i] = new Section(reader);
            }

            public void WriteToFile(BinaryWriter writer)
            {
                writer.Write(Hash);

                writer.Write(Mesh.GetUsedBytes());
                Mesh.Save(writer);

                writer.Write(sections.Length);
                for (int i = 0; i != sections.Length; i++)
                    sections[i].WriteToFile(writer);
            }
        }

        public class Section
        {
            public int Start { get; set; }

            public int NumEdges { get; set; }

            /// <summary>
            /// Actually it's materialIndex-2 (that's strange, isn't it?)
            /// </summary>
            public int Material { get; set; }

            /// <summary>
            /// Always == 0 (at least on PC)
            /// </summary>
            public int Unk2 { get; set; }

            public Section(BinaryReader reader)
            {
                Start = reader.ReadInt32();
                NumEdges = reader.ReadInt32();
                Material = reader.ReadInt32();
                Unk2 = reader.ReadInt32();
            }

            public Section()
            {
            }

            public void WriteToFile(BinaryWriter writer)
            {
                writer.Write(Start);
                writer.Write(NumEdges);
                writer.Write(Material);
                writer.Write(Unk2);
            }
        }
    }
}
