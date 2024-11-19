using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Numerics;
using Utils.Extensions;
using Utils.Helpers;
using Utils.Logging;
using Utils.Types;
using Utils.VorticeUtils;
using Vortice.Mathematics;

namespace ResourceTypes.Navigation
{
    public class AnimalTrafficLoader
    {
        private FileInfo file;

        // hardcoded
        private const short Magic = 0x5441;
        private const int Version = 0x5F1B1EC9;

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class PathPoint
        {
            // Flags for the point
            [Flags]
            public enum PathPointFlags
            {
                None = 0,
                CanSpawn = 1,
                CanDespawn = 2,
                CanIdle = 4,
            }

            // Worldspace position of the point
            [LocalisedDescription("The Position of this point along the path.")]
            public Vector3 Position { get; set; }

            // Could be direction of point?
            [LocalisedDescription("The rotation (of what?) of the point. STORED IN RADIANS.")]
            public Vector3 Rotation { get; set; }

            // 7 for dove, 5 for idle?, 2 for despawn?, 0 for transition?
            [LocalisedDescription("The flags for this point in the path."), Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
            public PathPointFlags Flags { get; set; } 

            public override string ToString()
            {
                return string.Format("{0} {1} {2}", Position, Rotation, Flags);
            }
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class AnimalTrafficPath
        {
            // Points where the animal can despawn
            [Browsable(false), LocalisedDescription("Points in the path where the animal can despawn at. They will travel to one of these points to DESPAWN.")]
            public byte[] SpawnPoints { get; set; }

            // Points where the animal can spawn at
            [Browsable(false), LocalisedDescription("Points in the path where the animal can SPAWN at. They will SPAWN at one of these points.")]
            public byte[] DespawnPoints { get; set; }

            // Points where the animal can idle at
            [Browsable(false), LocalisedDescription("Points in the path where the animal can IDLE. They will move to this point (and from) from spawn and to despawn.")]
            public byte[] IdlePoints { get; set; }

            // Always empty might as well remove from view
            [Browsable(false)] 
            public HashName Hash0 { get; set; }

            // Automatically calculated by program, remove from view
            [Browsable(false)] 
            public BoundingBox BoundingBox0 { get; set; }

            // Unknown
            [LocalisedDescription("Unknown. Defaults to 5.0f")]
            public float Unk00 { get; set; }

            // Unknown
            [LocalisedDescription("Unknown. Defaults to 15.0f")]
            public float Unk10 { get; set; }
            
            // Has an "Easy to use" property, remove from view
            [LocalisedDescription("Unknown; Unk30[0] appears to be animal type, so maybe the rest can be other animal types too?")]
            public byte[] Unk30 { get; set; } // direct index to animal types array

            // A list of points on the AnimalTraffic path.
            [LocalisedDescription("Points which form this path. Needs at least ONE point.")]
            public PathPoint[] Points { get; set; }

            // "Easy to use" properties
            [LocalisedDescription("The Animal type this path should use. Use the 'AnimalTypes' in the AnimalTraffic file for a list.")]
            public byte AnimalTypeIdx
            {
                get { return Unk30[0]; }
                set { Unk30[0] = value; }
            }

            public AnimalTrafficPath()
            {
                Hash0 = new HashName();

                // Always appears to have ATLEAST one element
                SpawnPoints = new byte[1];
                DespawnPoints = new byte[1];
                IdlePoints = new byte[1];

                Unk30 = new byte[1]; // One because of the AnimalTypeIdx
                Points = new PathPoint[1]; // Probably need ATLEAST one to be valid...
                Points[0] = new PathPoint();

                // Appear to be default
                Unk00 = 5.0f;
                Unk10 = 15.0f;
            }

            public void PreWriteFixup()
            {
                // Compute from points, then add do a stretch on the extents of the BBox.
                BoundingBox NewBBox = BoundingBox.CreateFromPoints(GetPoints());
                NewBBox.SetMinimum(NewBBox.Min - new Vector3(0.01f));
                NewBBox.SetMaximum(NewBBox.Max + new Vector3(0.01f));
                BoundingBox0 = NewBBox;

                // Generate lookups for points - use flags to determine list
                List<byte> SpawnPointsList = new List<byte>();
                List<byte> IdlePointsList = new List<byte>();
                List<byte> DespawnPointsList = new List<byte>();

                // Iterate through all points
                for(int i = 0; i < Points.Length; i++)
                {
                    PathPoint PPoint = Points[i];
                    byte IndexAsByte = (byte)i;

                    // Add to correct lists
                    if(PPoint.Flags.HasFlag(PathPoint.PathPointFlags.CanSpawn))
                    {
                        SpawnPointsList.Add(IndexAsByte);
                    }

                    if (PPoint.Flags.HasFlag(PathPoint.PathPointFlags.CanDespawn))
                    {
                        DespawnPointsList.Add(IndexAsByte);
                    }

                    if (PPoint.Flags.HasFlag(PathPoint.PathPointFlags.CanIdle))
                    {
                        IdlePointsList.Add(IndexAsByte);
                    }
                }

                // Then apply new lists
                SpawnPoints = SpawnPointsList.ToArray();
                DespawnPoints = DespawnPointsList.ToArray();
                IdlePoints = IdlePointsList.ToArray();
            }

            public Vector3[] GetPoints()
            {
                // TODO: Probably could do Array.ForEach<T> here.
                Vector3[] OutPoints = new Vector3[Points.Length];
                for(int i = 0; i < OutPoints.Length; i++)
                {
                    OutPoints[i] = Points[i].Position;
                }

                return OutPoints;
            }

            public override string ToString()
            {
                return string.Format("{0} {1} {2} {3}", Hash0.ToString(), Unk00, Unk10, AnimalTypeIdx);
            }
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class AnimalTrafficType
        {
            private HashName name;

            [LocalisedDescription("The Hash found in AnimalTraffic.xml.")]
            public HashName Name {
                get { return name; }
                set { name = value; }
            }
            public AnimalTrafficType()
            {
                Name = new HashName();
            }

            public override string ToString()
            {
                return string.Format("{0}", Name);
            }
        }

        // The type of animals this AnimalTrafficPath file can use
        public AnimalTrafficType[] AnimalTypes { get; set; }

        // The list of paths to load (and can be chosen) at runtime
        public AnimalTrafficPath[] Paths { get; set; }

        public AnimalTrafficLoader(FileInfo info)
        {
            file = info;
            using (BinaryReader reader = new BinaryReader(File.Open(info.FullName, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
        }

        public void ReadFromFile(BinaryReader reader)
        {
            if (reader.ReadInt16() != Magic)
            {
                return;
            }

            ushort AnimalTypeCount = reader.ReadUInt16();
            AnimalTypes = new AnimalTrafficType[AnimalTypeCount];

            if (reader.ReadInt32() != Version)
            {
                return;
            }

            for (int i = 0; i < AnimalTypeCount; i++)
            {
                AnimalTrafficType NewAnimalType = new AnimalTrafficType();
                NewAnimalType.Name = new HashName();
                NewAnimalType.Name.ReadFromFile(reader);
                AnimalTypes[i] = NewAnimalType;
            }

            ushort pathCount = reader.ReadUInt16();
            Paths = new AnimalTrafficPath[pathCount];

            for (int i = 0; i < pathCount; i++)
            {
                AnimalTrafficPath path = new AnimalTrafficPath();
                byte pathSize = reader.ReadByte();
                byte count1 = reader.ReadByte();
                byte count2 = reader.ReadByte();
                byte count3 = reader.ReadByte();
                path.SpawnPoints = reader.ReadBytes(count1);
                path.DespawnPoints = reader.ReadBytes(count2); // Data[0] The point where the animal should despawn
                path.IdlePoints = reader.ReadBytes(count3);
                path.BoundingBox0 = BoundingBoxExtenders.ReadFromFile(reader);
                path.Hash0 = new HashName();
                path.Hash0.ReadFromFile(reader); //decompiled exe says this is a hashbut its always empty
                path.Unk00 = reader.ReadSingle(); //5
                path.Unk10 = reader.ReadSingle(); //15
                byte count4 = reader.ReadByte();
                path.Unk30 = reader.ReadBytes(count4);

                // NB: Unk30 can have a size greater than 1.
                // I believe the first one is animal type, but the rest im uncertain of.
                // Could it be referencing some data perhaps? Need to check if it causes problems.

                // Load Path points
                path.Points = new PathPoint[pathSize];
                for(int x = 0; x < pathSize; x++)
                {
                    PathPoint vector = new PathPoint();
                    vector.Position = Vector3Utils.ReadFromFile(reader); //Very large differences between these two
                    vector.Rotation = Vector3Utils.ReadFromFile(reader); //2nd one could be rotation, in radians.
                    vector.Flags = (PathPoint.PathPointFlags)reader.ReadByte();
                    path.Points[x] = vector;
                }

                Paths[i] = path;
            }
        }

        public void WriteToFile()
        {
            // Make sure the paths have a chance of updating data
            HandlePreSave();

            using (BinaryWriter writer = new BinaryWriter(File.Open(file.FullName, FileMode.Create)))
            {
                writer.Write(Magic); //magic
                writer.Write((ushort)AnimalTypes.Length);
                writer.Write(Version);

                foreach(AnimalTrafficType Instance in AnimalTypes)
                {
                    Instance.Name.WriteToFile(writer);
                }

                // Write paths
                writer.Write((ushort)Paths.Length);
                foreach(AnimalTrafficPath path in Paths)
                {
                    writer.Write((byte)path.Points.Length);
                    writer.Write((byte)path.SpawnPoints.Length);
                    writer.Write((byte)path.DespawnPoints.Length);
                    writer.Write((byte)path.IdlePoints.Length);
                    writer.Write(path.SpawnPoints);
                    writer.Write(path.DespawnPoints);
                    writer.Write(path.IdlePoints);
                    BoundingBoxExtenders.WriteToFile(path.BoundingBox0, writer);
                    path.Hash0.WriteToFile(writer);
                    writer.Write(path.Unk00);
                    writer.Write(path.Unk10);
                    writer.Write((byte)path.Unk30.Length);
                    writer.Write(path.Unk30);

                    // Write Path Points
                    foreach(PathPoint PathVect in path.Points)
                    {
                        Vector3Utils.WriteToFile(PathVect.Position, writer);
                        Vector3Utils.WriteToFile(PathVect.Rotation, writer);
                        writer.Write((byte)PathVect.Flags);
                    }
                }
            }
        }

        private void HandlePreSave()
        {
            foreach (AnimalTrafficPath Path in Paths)
            {
                Path.PreWriteFixup();
            }
        }
    }
}
