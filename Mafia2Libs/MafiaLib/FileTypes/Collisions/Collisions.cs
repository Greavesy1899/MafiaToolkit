using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using SharpDX;
using Utils.Models;
using Utils.SharpDXExtensions;
using static Utils.Models.M2TStructure;

namespace ResourceTypes.Collisions
{
    public class Collision
    {
        int version;
        int unk0;

        int count1;
       List<Placement> placementData;

        int count2;
        Dictionary<ulong, NXSStruct> nxsData;

        string name;

        public Dictionary<ulong, NXSStruct> NXSData
        {
            get { return nxsData; }
            set { nxsData = value; }
        }

        public List<Placement> Placements
        {
            get { return placementData; }
            set { placementData = value; }
        }

        public string Name {
            get { return name; }
            set { name = value; }
        }

        public Collision(string fileName)
        {
            name = fileName;
            using (BinaryReader reader = new BinaryReader(File.Open(name, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
        }

        public Collision()
        {
            version = 17;
            unk0 = 0;
            nxsData = new Dictionary<ulong, NXSStruct>();
            placementData = new List<Placement>();
        }

        public void ReadFromFile(BinaryReader reader)
        {
            version = reader.ReadInt32();

            if (version != 17)
                throw new Exception("Unknown collision version");

            unk0 = reader.ReadInt32();
            count1 = reader.ReadInt32();

            placementData = new List<Placement>(count1);
            for (int i = 0; i != count1; i++)
            {
                placementData.Add(new Placement(reader));
            }

            count2 = reader.ReadInt32();
            nxsData = new Dictionary<ulong, NXSStruct>();

            for (int i = 0; i != count2; i++)
            {
                NXSStruct data = new NXSStruct(reader);
                nxsData.Add(data.Hash, data);
            }

        }

        public void WriteToFile()
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(name, FileMode.Create)))
                WriteToFile(writer);
        }
        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(version);
            writer.Write(unk0);
            writer.Write(placementData.Count);
            for (int i = 0; i != placementData.Count; i++)
                placementData[i].WriteToFile(writer);

            writer.Write(nxsData.Count);
            for (int i = 0; i != nxsData.Count; i++)
                nxsData.ElementAt(i).Value.WriteToFile(writer);
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", 1, 2);
        }

        public class Placement
        {
            private Vector3 position;
            private Vector3 rotation;
            private ulong hash;
            private int unk4;
            private byte unk5;

            public Vector3 Position
            {
                get { return position; }
                set { position = value; }
            }

            public Vector3 Rotation
            {
                get { return rotation; }
                set { rotation = value; }
            }

            public ulong Hash
            {
                get { return hash; }
                set { hash = value; }
            }

            public int Unk4
            {
                get { return unk4; }
                set { unk4 = value; }
            }

            public byte Unk5
            {
                get { return unk5; }
                set { unk5 = value; }
            }

            public Placement(BinaryReader reader)
            {
                ReadFromFile(reader);
            }

            public Placement()
            {

            }

            public Placement(Placement other)
            {
                position = other.position;
                rotation = other.rotation;
                hash = other.hash;
                unk4 = other.unk4;
                unk5 = other.unk5;
            }

            /// <summary>
            /// Read data from file.
            /// </summary>
            /// <param name="reader">stream</param>
            public void ReadFromFile(BinaryReader reader)
            {
                position = Vector3Extenders.ReadFromFile(reader);
                rotation = Vector3Extenders.ReadFromFile(reader);
                Vector3 rot = new Vector3();
                rot.X = MathUtil.RadiansToDegrees(rotation.X);
                rot.Y = MathUtil.RadiansToDegrees(rotation.Y);
                rot.Z = -MathUtil.RadiansToDegrees(rotation.Z);
                rotation = rot;
                hash = reader.ReadUInt64();
                unk4 = reader.ReadInt32();
                unk5 = reader.ReadByte();
                Console.WriteLine(string.Format("hash {0}, unk4 {1}, unk5 {2}", hash, unk4, unk5));
            }

            /// <summary>
            /// Write Data to file
            /// </summary>
            /// <param name="writer">stream</param>
            public void WriteToFile(BinaryWriter writer)
            {
                position.WriteToFile(writer);
                Vector3 rot = new Vector3();
                rot.X = 0.0f;//MathUtil.DegreesToRadians(rotation.X);
                rot.Y = 0.0f;//MathUtil.DegreesToRadians(rotation.Y);
                rot.Z = -MathUtil.DegreesToRadians(rotation.Z);
                rotation = rot;
                rotation.WriteToFile(writer);
                writer.Write(hash);
                writer.Write(unk4);
                writer.Write(unk5);
            }

            public override string ToString()
            {
                return string.Format("{0}, {1}, {2}", hash, unk4, unk5);
            }
        }

        public class NXSStruct
        {
            ulong hash;
            private int dataSize;
            private byte[] bytes;
            protected MeshData data;
            protected Section[] sections;

            public ulong Hash
            {
                get { return hash; }
                set { hash = value; }
            }

            public MeshData Data
            {
                get { return data; }
                set { data = value; }
            }

            public Section[] Sections
            {
                get { return sections; }
                set { sections = value; }
            }

            public NXSStruct(BinaryReader reader)
            {
                ReadFromFile(reader);
            }
            public NXSStruct()
            {
                hash = 0;
                dataSize = 0;
                data = new MeshData();
                sections = new Section[1];
        }

            public void ReadFromFile(BinaryReader reader)
            {
                hash = reader.ReadUInt64();

                dataSize = reader.ReadInt32();
                data = new MeshData(reader, sections);

                int length = reader.ReadInt32();
                sections = new Section[length];
                for (int i = 0; i != sections.Length; i++)
                    sections[i] = new Section(reader);

                Console.WriteLine("Passed Collision: {0}", hash);
            }

            public void WriteToFile(BinaryWriter writer)
            {
                Console.WriteLine("saving :" + hash);
                writer.Write(hash);
                Console.WriteLine("{0} {1} {2}", dataSize, data.GetMeshSize(), (int)data.Flags);
                writer.Write(data.GetMeshSize());
                data.WriteToFile(writer);
                writer.Write(sections.Length);

                for (int i = 0; i != sections.Length; i++)
                    sections[i].WriteToFile(writer);
            }
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class MeshData
        {
            string nxs;
            string mesh;
            int ver;
            MeshSerialFlags flags;
            float convexEdgeThreshold;
            int maxVertices;
            int num4;

            int nPoints;
            int nTriangles;

            protected Vector3[] points;
            protected uint[] indices; //or some linking thing
            protected CollisionMaterials[] materials; //COULD be materialIDs

            int max;
            int[] remapData;

            int numConvexParts;
            int numFlatParts;
            private short[] convexParts;
            private short[] flatParts;

            HBMOPCDataClass opcHbmData;
            private BoundingBox boundingBox;
            private BoundingSphere boundingSphere;
            private float[] unkFloats;
            private int unkSize;
            private byte[] unkSizeData;

            public float ConvexEdgeThreshold {
                get { return convexEdgeThreshold; }
                set { convexEdgeThreshold = value; }
            }
            public int Version {
                get { return ver; }
                set { ver = value; }
            }
            public MeshSerialFlags Flags {
                get { return flags; }
                set { flags = value; }
            }
            public int MaxVertices {
                get { return maxVertices; }
                set { maxVertices = value; }
            }
            public int Num4 {
                get { return num4; }
                set { num4 = value; }
            }
            public Vector3[] Vertices {
                get { return points; }
                set { points = value; }
            }
            public uint[] Indices {
                get { return indices; }
                set { indices = value; }
            }
            public CollisionMaterials[] Materials {
                get { return materials; }
                set { materials = value; }
            }
            public int RemapMax {
                get { return max; }
                set { max = value; }
            }
            public int[] RemapData {
                get { return remapData; }
                set { remapData = value; }
            }
            public HBMOPCDataClass OPCHBMData {
                get { return opcHbmData; }
                set { opcHbmData = value; }
            }
            public float[] UnkFloats {
                get { return unkFloats; }
                set { unkFloats = value; }
            }
            public BoundingBox BoundingBox {
                get { return boundingBox; }
                set { boundingBox = value; }
            }
            public BoundingSphere BoundingSphere {
                get { return boundingSphere; }
                set { boundingSphere = value; }
            }
            public int UnkSize {
                get { return unkSize; }
                set { unkSize = value; }
            }
            public byte[] UnkSizeData {
                get { return unkSizeData; }
                set { unkSizeData = value; }
            }
            public short[] ConvexParts {
                get { return convexParts; }
                set { convexParts = value; }
            }
            public short[] FlatParts {
                get { return flatParts; }
                set { flatParts = value; }
            }
            public int NumConvexParts {
                get { return numConvexParts; }
                set { numConvexParts = value; }
            }
            public int NumFlatParts {
                get { return numFlatParts; }
                set { numFlatParts = value; }
            }

            public MeshData(BinaryReader reader, Section[] sections)
            {
                ReadFromFile(reader);
            }
            public MeshData()
            {

            }

            public void ReadFromFile(BinaryReader reader)
            {
                nxs = new string(reader.ReadChars(4));
                mesh = new string(reader.ReadChars(4));
                ver = reader.ReadInt32();
                flags = (MeshSerialFlags)reader.ReadInt32();
                convexEdgeThreshold = reader.ReadSingle();
                maxVertices = reader.ReadInt32();
                num4 = reader.ReadInt32();

                nPoints = reader.ReadInt32();
                nTriangles = reader.ReadInt32();

                points = new Vector3[nPoints];
                indices = new uint[nTriangles*3];
                materials = new CollisionMaterials[nTriangles];

                for (int i = 0; i != points.Length; i++)
                    points[i] = Vector3Extenders.ReadFromFile(reader);

                if (flags.HasFlag(MeshSerialFlags.MSF_8BIT_INDICES) || flags.HasFlag(MeshSerialFlags.MSF_16BIT_INDICES))
                    throw new Exception("Unsupported! 8bit or 16bit indices");

                for (int i = 0; i != indices.Length; i++)
                    indices[i] = reader.ReadUInt32();

                if (flags.HasFlag(MeshSerialFlags.MSF_MATERIALS))
                {
                    for (int i = 0; i != materials.Length; i++)
                        materials[i] = (CollisionMaterials)reader.ReadInt16();
                }

                if (flags.HasFlag(MeshSerialFlags.MSF_FACE_REMAP))
                {
                    max = reader.ReadInt32();
                    remapData = new int[nTriangles];

                    for (int i = 0; i < nTriangles; i++)
                    {
                        if (max > 0xFFFF)
                            remapData[i] = reader.ReadInt32();
                        else if (max > 0xFF)
                            remapData[i] = reader.ReadInt16();
                        else
                            remapData[i] = reader.ReadByte();
                    }
                }

                numConvexParts = reader.ReadInt32();
                numFlatParts = reader.ReadInt32();

                if (numConvexParts > 0)
                {
                    convexParts = new short[nTriangles];
                    for (int i = 0; i != nTriangles; i++)
                        convexParts[i] = reader.ReadInt16();
                }
                if (numFlatParts > 0)
                {
                    if (numFlatParts < 256)
                    {
                        flatParts = new short[nTriangles];
                        for (int i = 0; i != nTriangles; i++)
                            flatParts[i] = reader.ReadByte();
                    }
                    else
                    {
                        flatParts = new short[nTriangles];
                        for (int i = 0; i != nTriangles; i++)
                            flatParts[i] = reader.ReadInt16();
                    }
                }

               opcHbmData = new HBMOPCDataClass(reader);

                unkFloats = new float[14];
                unkFloats[0] = reader.ReadSingle();

                boundingSphere = BoundingSphereExtenders.ReadFromFile(reader);
                boundingBox = BoundingBoxExtenders.ReadFromFile(reader);

                for (int i = 1; i != unkFloats.Length; i++)
                    unkFloats[i] = reader.ReadSingle();

                unkSize = reader.ReadInt32();
                unkSizeData = reader.ReadBytes(unkSize);

                if (unkSize != nTriangles)
                    throw new FormatException("UnkSize does not equal nTriangles:");
            }
            public void WriteToFile(BinaryWriter writer)
            {
                writer.Write(22239310);
                writer.Write(1213416781);
                writer.Write(ver);
                writer.Write((int)flags);
                writer.Write(convexEdgeThreshold);
                writer.Write(maxVertices);
                writer.Write(num4);

                writer.Write(nPoints);
                writer.Write(nTriangles);

                for (int i = 0; i != points.Length; i++)
                    points[i].WriteToFile(writer);

                for (int i = 0; i != indices.Length; i++)
                    writer.Write(indices[i]);

                if (flags.HasFlag(MeshSerialFlags.MSF_MATERIALS))
                {
                    for (int i = 0; i != materials.Length; i++)
                        writer.Write((short)materials[i]);
                }

                if (flags.HasFlag(MeshSerialFlags.MSF_FACE_REMAP))
                {
                    writer.Write(max);
                    for (int i = 0; i < nTriangles; i++)
                    {
                        if (max > 0xFFFF)
                            writer.Write(remapData[i]);
                        else if (max > 0xFF)
                            writer.Write((short)remapData[i]);
                        else
                            writer.Write((byte)remapData[i]);
                    }
                }

                writer.Write(numConvexParts);
                writer.Write(numFlatParts);

                if (numConvexParts > 0)
                {
                    for (int i = 0; i != nTriangles; i++)
                        writer.Write(convexParts[i]);
                }
                if (numFlatParts > 0)
                {
                    if (numFlatParts < 256)
                    {
                        for (int i = 0; i != nTriangles; i++)
                            writer.Write((byte)flatParts[i]);
                    }
                    else
                    {
                        for (int i = 0; i != nTriangles; i++)
                            writer.Write(flatParts[i]);
                    }
                }

                opcHbmData.WriteToFile(writer);

                writer.Write(unkFloats[0]);

                boundingSphere.WriteToFile(writer);
                boundingBox.WriteToFile(writer);

                for (int i = 1; i != unkFloats.Length; i++)
                    writer.Write(unkFloats[i]);

                writer.Write(unkSize);
                writer.Write(unkSizeData);
            }
            public int GetMeshSize()
            {
                int size = 0;
                size += 36;
                size += (points.Length * 12);

                if (flags.HasFlag(MeshSerialFlags.MSF_8BIT_INDICES) || flags.HasFlag(MeshSerialFlags.MSF_16BIT_INDICES))
                    throw new Exception("Unsupported! 8bit or 16bit indices");

                size += (indices.Length * 4);

                if (flags.HasFlag(MeshSerialFlags.MSF_MATERIALS))
                    size += (materials.Length * 2);

                if (flags.HasFlag(MeshSerialFlags.MSF_FACE_REMAP))
                {
                    size += 4;

                    if (max > 0xFFFF)
                        size += (4 * nTriangles);
                    else if (max > 0xFF)
                        size += (2 * nTriangles);
                    else
                        size += (nTriangles);
                }

                size += 8;
                if (numConvexParts > 0)
                {
                    size += (nTriangles * 2);
                }
                if (numFlatParts > 0)
                {
                    if (numFlatParts < 256)
                    {
                        size += (nTriangles);
                    }
                    else
                    {
                        size += (nTriangles * 2);
                    }
                }

                opcHbmData.GetSize(ref size);

                size += 100;
                size += 4;
                size += unkSize;
                return size;
            }
            public void BuildBasicCollision(Lod model)
            {
                nxs = Convert.ToString(22239310);
                mesh = Convert.ToString(1213416781);

                ver = 1;
                flags = MeshSerialFlags.MSF_MATERIALS;
                convexEdgeThreshold = 0.001f;
                maxVertices = 255;
                num4 = 0;
                nPoints = model.Vertices.Length;
                nTriangles = model.Indices.Length/3;
                points = new Vector3[nPoints];
                indices = new uint[model.Indices.Length];
                materials = new CollisionMaterials[model.Indices.Length];

                int idx = 0;
                for (int i = 0; i != model.Parts.Length; i++)
                {
                    ModelPart part = model.Parts[i];
                    for (int x = (int)part.StartIndex; x < part.StartIndex+part.NumFaces*3; x++)
                    {
                        indices[x] = model.Indices[x];
                        materials[x] = (CollisionMaterials)Enum.Parse(typeof(CollisionMaterials), model.Parts[i].Material);
                    }
                }

                for (int i = 0; i != points.Length; i++)
                    points[i] = model.Vertices[i].Position;

                numConvexParts = 0;
                numFlatParts = 0;

                convexParts = new short[nTriangles];
                flatParts = new short[nTriangles];

                opcHbmData = new HBMOPCDataClass();
                opcHbmData.BuildBasicOPCHBM();

                boundingBox = new BoundingBox();
                unkFloats = new float[14];

                //sort out bounding box/sphere
                List<Vertex[]> data = new List<Vertex[]>();
                data.Add(model.Vertices);
                boundingBox = new BoundingBox();
                boundingSphere = new BoundingSphere();
                boundingBox = BoundingBoxExtenders.CalculateBounds(data);
                boundingSphere = BoundingSphereExtenders.CalculateFromBBox(boundingBox);

                unkSize = nTriangles;
                unkSizeData = new byte[unkSize];
                for (int i = 0; i != unkSizeData.Length; i++)
                    unkSizeData[i] = 0;
            }

            [TypeConverter(typeof(ExpandableObjectConverter))]
            public class HBMOPCDataClass
            {
                private int opcSize; //possible.
                private int opcVersion; //1
                private int opcType; //if this is 4; there is no data. If 3, data is here.
                private int opcCount; //only available if type is 3.
                private UnkOPCData[] opcData; //8 halfs, with a long value.
                private float[] opcFloats; //6 floats.

                private int hbmVersion; //0
                private int hbmOffset; //potentially opcCount+1;
                private int hbmMaxOffset;
                private byte[] hbmOffsetData;
                private int hbmNumRefs;
                private int hbmMaxRefValue;
                private short[] hbmRefs;

                public int OPCSize {
                    get { return opcSize; }
                    set { opcSize = value; }
                }
                public int OPCVersion {
                    get { return opcVersion; }
                    set { opcVersion = value; }
                }
                public int OPCType {
                    get { return opcType; }
                    set { opcType = value; }
                }
                public int OPCCount {
                    get { return opcCount; }
                    set { opcCount = value; }
                }
                public UnkOPCData[] unkOPCData {
                    get { return opcData; }
                    set { opcData = value; }
                }
                public float[] OPCFloats {
                    get { return opcFloats; }
                    set { opcFloats = value; }
                }
                public int HBMVersion {
                    get { return hbmVersion; }
                    set { hbmVersion = value; }
                }
                public int HBMOffset {
                    get { return hbmOffset; }
                    set { hbmOffset = value; }
                }
                public int HBMMaxOffset {
                    get { return hbmMaxOffset; }
                    set { hbmMaxOffset = value; }
                }
                public byte[] HBMOffsetData {
                    get { return hbmOffsetData; }
                    set { hbmOffsetData = value; }
                }
                public int HBMNumRefs {
                    get { return hbmNumRefs; }
                    set { hbmNumRefs = value; }
                }
                public int HBMMaxRefValue {
                    get { return hbmMaxRefValue; }
                    set { hbmMaxRefValue = value; }
                }
                public short[] HBMRefs {
                    get { return hbmRefs; }
                    set { hbmRefs = value; }
                }

                public HBMOPCDataClass(BinaryReader reader)
                {
                    ReadFromFile(reader);
                }

                public HBMOPCDataClass()
                {
                }

                public void ReadFromFile(BinaryReader reader)
                {
                    opcSize = reader.ReadInt32();

                    //BEGIN OPC/HBM SECTION.
                    int opc = reader.ReadInt32();
                    if (opc != 21188687)
                        throw new FormatException("Did not reach OPC correctly");

                    opcVersion = reader.ReadInt32();
                    opcType = reader.ReadInt32();

                    if (opcType == 3)
                    {
                        opcCount = reader.ReadInt32();
                        opcData = new UnkOPCData[opcCount];
                        for (int i = 0; i != opcCount; i++)
                            opcData[i] = new UnkOPCData(reader);

                        opcFloats = new float[6];
                        for (int i = 0; i != 6; i++)
                            opcFloats[i] = reader.ReadSingle();
                    }

                    int hbm = reader.ReadInt32();
                    if (hbm != 21840456)
                        throw new FormatException("Did not reach HBM correctly");

                    hbmVersion = reader.ReadInt32();
                    hbmOffset = reader.ReadInt32();
                    hbmMaxOffset = reader.ReadInt32(); //max num in offset;

                    if (hbmOffset > 1)
                    {
                        if (hbmMaxOffset > byte.MaxValue && hbmMaxOffset < ushort.MaxValue)
                            hbmOffsetData = reader.ReadBytes(hbmOffset * 2);
                        else if (hbmMaxOffset > ushort.MaxValue)
                            hbmOffsetData = reader.ReadBytes(hbmOffset * 4);
                        else
                            hbmOffsetData = reader.ReadBytes(hbmOffset);

                        hbmNumRefs = reader.ReadInt32();
                    }
                }
                public void WriteToFile(BinaryWriter writer)
                {
                    writer.Write(opcSize);

                    //BEGIN OPC/HBM SECTION.
                    writer.Write(21188687);

                    writer.Write(opcVersion);
                    writer.Write(opcType);

                    if (opcType == 3)
                    {
                        writer.Write(opcCount);
                        for (int i = 0; i != opcCount; i++)
                            opcData[i].WriteToFile(writer);

                        for (int i = 0; i != 6; i++)
                            writer.Write(opcFloats[i]);
                    }

                    writer.Write(21840456);

                    writer.Write(hbmVersion);
                    writer.Write(hbmOffset);
                    writer.Write(hbmMaxOffset);

                    if (hbmOffset > 1)
                    {
                        writer.Write(hbmOffsetData);
                        writer.Write(hbmNumRefs);

                        if (hbmNumRefs != 0)
                            throw new NotImplementedException();
                    }
                }
                public void BuildBasicOPCHBM()
                {
                    opcSize = 28;

                    //BEGIN OPC/HBM SECTION.
                    string opc = Convert.ToString(21188687);

                    opcVersion = 1;
                    opcType = 4;

                    string hbm = Convert.ToString(21840456);

                    hbmVersion = 0;
                    hbmOffset = 1;
                    hbmMaxOffset = 0;
                }
                public void GetSize(ref int size)
                {
                    int curSize = 0;
                    //size, opc header, opc version and type.
                    curSize += 12;

                    if (opcType == 3)
                    {
                        curSize += 4;

                        for (int i = 0; i != opcCount; i++)
                            curSize += 20;

                        for (int i = 0; i != 6; i++)
                            curSize += 4;
                    }

                    //HBM header, version, offset and maxoffset;
                    curSize += 16;

                    if (hbmOffset > 1)
                    {
                        curSize += hbmOffsetData.Length;

                        curSize += 4;
                        if (hbmNumRefs != 0)
                            throw new NotImplementedException();
                    }

                    opcSize = curSize;
                    size += curSize;
                }
                public override string ToString()
                {
                    return "OPC/HBM Data";
                }

                public struct UnkOPCData
                {
                    private short[] unkHalfs;
                    private int unkInt;

                    public short[] UnkHalfs {
                        get { return unkHalfs; }
                        set { unkHalfs = value; }
                    }
                    public int UnkInt {
                        get { return unkInt; }
                        set { unkInt = value; }
                    }

                    public UnkOPCData(BinaryReader reader)
                    {
                        unkHalfs = new short[8];

                        for (int i = 0; i != unkHalfs.Length; i++)
                        {
                            unkHalfs[i] = reader.ReadInt16();
                        }

                        unkInt = reader.ReadInt32();
                    }
                    public void WriteToFile(BinaryWriter writer)
                    {
                        for (int i = 0; i != unkHalfs.Length; i++)
                            writer.Write(unkHalfs[i]);

                        writer.Write(unkInt);
                    }
                    public override string ToString()
                    {
                        return "UnkOPCData";
                    }
                }
            }
        }

        public class Section
        {
            int start;
            int numEdges;
            int unk1;
            int unk2;
            byte[] edgeData;

            public int Start
            {
                get { return start; }
                set { start = value; }
            }

            public int NumEdges
            {
                get { return numEdges; }
                set { numEdges = value; }
            }

            public int Unk1
            {
                get { return unk1; }
                set { unk1 = value; }
            }

            public int Unk2
            {
                get { return unk2; }
                set { unk2 = value; }
            }

            public byte[] EdgeData
            {
                get { return edgeData; }
                set { edgeData = value; }
            }

            public Section(BinaryReader reader)
            {
                start = reader.ReadInt32();
                numEdges = reader.ReadInt32();
                unk1 = reader.ReadInt32();
                unk2 = reader.ReadInt32();
            }

            public Section()
            {

            }

            public void WriteToFile(BinaryWriter writer)
            {
                writer.Write(start);
                writer.Write(numEdges);
                writer.Write(unk1);
                writer.Write(unk2);

            }
        }
    }
}