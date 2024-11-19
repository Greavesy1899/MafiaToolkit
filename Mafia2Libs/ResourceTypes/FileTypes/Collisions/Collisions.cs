﻿using ResourceTypes.Collisions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using Utils.Helpers;
using Utils.Logging;
using Utils.VorticeUtils;
using Vortice.Mathematics;

namespace ResourceTypes.Collisions
{
    public class Collision
    {
        private const int Version = 0x11; // 17
 
        public string Name { get; set; }
        /// <summary>
        /// Platform (== 0 on PC/Mac, == 1 on XBox360, == 2 on PS3)
        /// </summary>
        /// <remarks>Could be <c>NxPlatform</c> type, enum values are match</remarks>
        public uint Platform { get; set; } = 0;
        public List<Placement> Placements { get; private set; }  = new List<Placement>(); 
        public SortedDictionary<ulong, CollisionModel> Models { get; private set; } = new SortedDictionary<ulong, CollisionModel>();


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

            Platform = reader.ReadUInt32();
            if (Platform > 2)
            { 
                throw new Exception($"Unknown platform {Platform}");
            }

            int numPlacements = reader.ReadInt32();
            Placements = new List<Placement>(numPlacements);
            for (int i = 0; i < numPlacements; i++)
                Placements.Add(new Placement(reader));

            int numModels = reader.ReadInt32();

            Models = new SortedDictionary<ulong, CollisionModel>();
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

            //force cook collisions
            if (Utils.Settings.ToolkitSettings.CookCollisions)
            {
                using(BinaryWriter writer = new BinaryWriter(File.Open("MeshBundle.bin", FileMode.Create)))
                {
                    writer.Write(Models.Count);

                    TriangleCooking cooker = new TriangleCooking();

                    foreach (var collisionModel in Models)
                    {
                        cooker.WriteRawFormatToFile(writer, collisionModel.Value.Mesh);
                    }
                }

                PhysXHelper.MultiCookTriangleCollision("MeshBundle.bin", "CookedBundle.bin");

                using(BinaryReader reader = new BinaryReader(File.Open("CookedBundle.bin", FileMode.Open)))
                {
                    uint NumModels = reader.ReadUInt32();
                    for(int i = 0; i < NumModels; i++)
                    {
                        TriangleMesh CookedMesh = new TriangleMesh();
                        CookedMesh.Load(reader);
                        CookedMesh.Force32BitIndices();
                        Models.ElementAt(i).Value.Mesh = CookedMesh;
                    }
                }

                if (File.Exists("MeshBundle.bin"))
                {
                    File.Delete("MeshBundle.bin");
                }

                if (File.Exists("CookedBundle.bin"))
                {
                    File.Delete("CookedBundle.bin");
                }
            }


            using (BinaryWriter writer = new BinaryWriter(File.Open(Name, FileMode.Create)))
            {
                WriteToFile(writer);
            }
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(Version);
            writer.Write(Platform);

            writer.Write(Placements.Count);
            foreach (var placement in Placements)
            {
                placement.WriteToFile(writer);
            }

            writer.Write(Models.Count);

            // NOTE: Models should be sorted by hash (ascending)
            // that's why SortedDictionary is used
            foreach (var collisionModel in Models)
            {
                collisionModel.Value.WriteToFile(writer);
            }
        }

        public void WriteToFile(string name)
        {
            this.Name = name;
            WriteToFile();
        }

        /** Util to remove model from Collisions file. */
        public void RemoveModel(ulong Hash)
        {
            if(Models.ContainsKey(Hash))
            {
                Models.Remove(Hash);
            }

            RemoveAllPlacementsForModels(Hash);
        }

        /** Util to remove model from Collisions file */
        public void RemoveModel(CollisionModel Model)
        {
            RemoveModel(Model.Hash);
        }

        /** Remove Placements for given Collision Object. */
        private void RemoveAllPlacementsForModels(ulong CollisionHash)
        {
            // Check if the placement has the collision hash, 
            // If so, delete from list.
            for (int i = Placements.Count - 1; i >= 0; i--)
            {
                Placement Instance = Placements[i];
                if(Instance.Hash == CollisionHash)
                {
                    Placements.RemoveAt(i);
                }
            }
        }

        public class Placement
        {
            public Vector3 Position { get; set; }

            public Vector3 Rotation { get; set; }

            public ulong Hash { get; set; }
            public int Unk4 { get; set; }
            public byte Unk5 { get; set; }

            /// <summary>
            /// Helper property to get/set rotation in degrees (with Z axes adopted to the Toolkit render coordinate system)
            /// instead of original rotation which is stored in radians
            /// </summary>
            public Vector3 RotationDegrees {
                get {
                    Vector3 vec = new Vector3();
                    vec.X = MathHelper.ToDegrees(Rotation.X);
                    vec.Y = MathHelper.ToDegrees(Rotation.Y);
                    //vec.X = ((vec.X + vec.Y) * -1);
                    //vec.X = ((vec.X + vec.Y) * -1);
                    vec.Y = ((vec.X + vec.Y) * -1);
                    vec.X = 0.0f;
                    vec.Z = /*Unk5 != 128 ? MathUtil.RadiansToDegrees(Rotation.Z) : */-MathHelper.ToDegrees(Rotation.Z);
                    return vec;
                }
                set {
                    Vector3 vec = new Vector3();
                    vec.X = MathHelper.ToRadians(value.X);
                    vec.Y = MathHelper.ToRadians(value.Y);
                    //vec.X = ((vec.X + vec.Y) * -1);
                    //vec.Y = ((vec.X + vec.Y) * -1);
                    vec.Z = /*Unk5 != 128 ? MathUtil.DegreesToRadians(value.Z) : */-MathHelper.ToRadians(value.Z);
                    Rotation = vec;
                }
            }

            /// <summary>
            /// Helper property to easily build the transform for the Toolkit render system.
            /// </summary>
            public Matrix4x4 Transform { 
                get {

                    Matrix4x4 transform = MatrixUtils.SetMatrix(RotationDegrees, Vector3.One, Position);
                    ToolkitAssert.Ensure(!transform.IsNaN(), "Transform is NaN");
                    transform.Translation = Position;
                    return transform;
                }
            }


            public Placement(BinaryReader reader)
            {
                ReadFromFile(reader);
            }

            public Placement()
            {
                Unk5 = 128;
                Unk4 = -1;
                Position = new Vector3(0, 0, 0);
                Rotation = new Vector3(0);
            }

            public Placement(Placement other)
            {
                Position = other.Position;
                Rotation = other.Rotation;
                Hash = other.Hash;
                Unk4 = other.Unk4;
                Unk5 = other.Unk5;
            }

            public void ReadFromFile(BinaryReader reader)
            {
                Position = Vector3Utils.ReadFromFile(reader);
                Rotation = Vector3Utils.ReadFromFile(reader);
                Hash = reader.ReadUInt64();
                Unk4 = reader.ReadInt32();
                Unk5 = reader.ReadByte();
            }

            public void WriteToFile(BinaryWriter writer)
            {
                Position.WriteToFile(writer);
                Rotation.WriteToFile(writer);
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
            public IList<Section> Sections { get; set; } 

            public CollisionModel(BinaryReader reader)
            {
                ReadFromFile(reader);
            }

            public CollisionModel()
            {
                Hash = 0;
                Mesh = new TriangleMesh();
                Sections = new List<Section>();
            }

            public void ReadFromFile(BinaryReader reader)
            {
                Hash = reader.ReadUInt64();

                int dataSize = reader.ReadInt32();
                Mesh = new TriangleMesh();
                Mesh.Load(reader);

                int numSections = reader.ReadInt32();
                Sections = new List<Section>();
                for (int i = 0; i < numSections; i++)
                {
                    Sections.Add(new Section(reader));
                }
            }

            public void WriteToFile(BinaryWriter writer)
            {
                writer.Write(Hash);

                writer.Write(Mesh.GetUsedBytes());
                Mesh.Save(writer);

                writer.Write(Sections.Count);
                foreach (var section in Sections)
                {
                    section.WriteToFile(writer);
                }
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

            public Section()
            {
            }

            public Section(BinaryReader reader)
            {
                Start = reader.ReadInt32();
                NumEdges = reader.ReadInt32();
                Material = reader.ReadInt32();
                Unk2 = reader.ReadInt32();
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
