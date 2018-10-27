using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace Mafia2
{
    public class Collision
    {
        int version;
        int unk0;

        int count1;
       List<Placement> placementData;

        int count2;
        Dictionary<ulong, NXSStruct> nxsData;

        public string name;

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

        public Collision(string fileName)
        {
            name = fileName;
            using (BinaryReader reader = new BinaryReader(File.Open(name, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
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

            /// <summary>
            /// Read data from file.
            /// </summary>
            /// <param name="reader">stream</param>
            public void ReadFromFile(BinaryReader reader)
            {
                position = new Vector3(reader);
                rotation = new Vector3(reader);
                rotation.ConvertToDegrees();
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
                rotation.ConvertToRadians();
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
                bytes = new byte[0];
                data = new MeshData();
                sections = new Section[1];
        }

            public void ReadFromFile(BinaryReader reader)
            {
                hash = reader.ReadUInt64();

                dataSize = reader.ReadInt32();
                long pos = reader.BaseStream.Position;

                bytes = reader.ReadBytes(dataSize);

                int length = reader.ReadInt32();
                sections = new Section[length];
                for (int i = 0; i != sections.Length; i++)
                    sections[i] = new Section(reader);

                long pos2 = reader.BaseStream.Position;

                reader.BaseStream.Position = pos;
                data = new MeshData(reader, sections);
                reader.BaseStream.Position = pos2;
                Console.WriteLine("Passed Collision: {0}", hash);
            }

            public void WriteToFile(BinaryWriter writer)
            {
                Console.WriteLine("saving :" + hash);
                //quick check for bugs
                if (data.Num2 == 3)
                {
                    data.Num5 = data.Triangles.Length - 1;
                    if (data.Triangles.Length <= 256)
                    {
                        data.UnkData = new byte[data.Triangles.Length];
                    }
                    else
                    {
                        data.UnkBytes = new short[data.Triangles.Length];
                    }
                }

                writer.Write(hash);
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
            int num1;
            int num2;
            float unkSmall;
            int num3;
            int num4;

            int nPoints;
            int nTriangles;

            protected Vector3[] points;
            protected Int3[] triangles; //or some linking thing
            protected CollisionMaterials[] unkShorts; //COULD be materialIDs

            int num5;

            short[] unkBytes;

            int unk0;
            byte[] unkData;

            int unk1;

            OPCDataClass opcData;
            HBMDataClass hbmData;

            private int unkSize;
            private byte[] unkSizeData;

            private short[] unkShortSectorData;
            private byte[] unkByteSectorData;

            public Section[] sections;

            public float UnkSmall {
                get { return unkSmall; }
                set { unkSmall = value; }
            }
            public int Num1 {
                get { return num1; }
                set { num1 = value; }
            }
            public int Num2 {
                get { return num2; }
                set { num2 = value; }
            }
            public int Num3 {
                get { return num3; }
                set { num3 = value; }
            }
            public int Num4 {
                get { return num4; }
                set { num4 = value; }
            }
            public Vector3[] Vertices {
                get { return points; }
                set { points = value; }
            }
            public Int3[] Triangles {
                get { return triangles; }
                set { triangles = value; }
            }
            public CollisionMaterials[] Materials {
                get { return unkShorts; }
                set { unkShorts = value; }
            }

            //TEST
            public int Num5 {
                get { return num5; }
                set { num5 = value; }
            }
            public byte[] UnkData {
                get { return unkData; }
                set { unkData = value; }
            }
            public short[] UnkBytes {
                get { return unkBytes; }
                set { unkBytes = value; }
            }
            public OPCDataClass OPCData {
                get { return opcData; }
                set { opcData = value; }
            }
            public HBMDataClass HBMData {
                get { return hbmData; }
                set { hbmData = value; }
            }
            public int UnkSize {
                get { return unkSize; }
                set { unkSize = value; }
            }
            public byte[] UnkSizeData {
                get { return unkSizeData; }
                set { unkSizeData = value; }
            }
            public short[] UnkShortSectorData {
                get { return unkShortSectorData; }
                set { unkShortSectorData = value; }
            }
            public byte[] UnkByteSectorData {
                get { return unkByteSectorData; }
                set { unkByteSectorData = value; }
            }
            public int Unk0 {
                get { return unk0; }
                set { unk0 = value; }
            }
            public int Unk1 {
                get { return unk1; }
                set { unk1 = value; }
            }

            public MeshData(BinaryReader reader, Section[] sections)
            {
                this.sections = sections;
                ReadFromFile(reader);
            }
            public MeshData()
            {

            }

            public void ReadFromFile(BinaryReader reader)
            {
                nxs = new string(reader.ReadChars(4));
                mesh = new string(reader.ReadChars(4));
                num1 = reader.ReadInt32();
                num2 = reader.ReadInt32();
                unkSmall = reader.ReadSingle();
                num3 = reader.ReadInt32();
                num4 = reader.ReadInt32();

                nPoints = reader.ReadInt32();
                nTriangles = reader.ReadInt32();

                points = new Vector3[nPoints];
                triangles = new Int3[nTriangles];
                unkShorts = new CollisionMaterials[nTriangles];

                for (int i = 0; i != points.Length; i++)
                {
                    points[i] = new Vector3(reader);
                }

                for (int i = 0; i != triangles.Length; i++)
                {
                    triangles[i] = new Int3(reader);
                }

                for (int i = 0; i != unkShorts.Length; i++)
                {
                    unkShorts[i] = (CollisionMaterials)reader.ReadInt16();
                }

                //bool overTri1 = false;

                if (num2 == 3)
                {
                    num5 = reader.ReadInt32();

                    if (nTriangles <= 256)
                    {
                        unkData = new byte[nTriangles];
                        for (int i = 0; i != nTriangles; i++)
                            unkData[i] = reader.ReadByte();
                    }
                    else
                    {
                        unkBytes = new short[nTriangles];
                        for (int i = 0; i != nTriangles; i++)
                            unkBytes[i] = reader.ReadInt16();
                    }

                    //OLD METHOD::
                    //if (num5 != nTriangles - 1)
                    //{
                    //    overTri1 = true;
                    //}
                }


                unk0 = reader.ReadInt32();
                unk1 = reader.ReadInt32();
                long curPosition = reader.BaseStream.Position;
                unkShortSectorData = new short[nTriangles];

                for (int i = 0; i != nTriangles; i++)
                    unkShortSectorData[i] = reader.ReadInt16();

                unkByteSectorData = reader.ReadBytes(nTriangles);
                long finalPosition = reader.BaseStream.Position;

                int total = 0;

                for(int i = 0; i != sections.Length; i++)
                {
                    total += sections[i].NumEdges;
                }

                if (total != nTriangles * 3)
                {
                    Console.WriteLine("ERROR! total = {0} nTriangles*3 = {1}", total, nTriangles*3);

                    if (total == num5 * 3)
                        Console.WriteLine("But num5 would: total = {0} num5*3 = {1}", total, num5 * 3);
                    else
                        Console.WriteLine("ERROR! total = {0} num5*3 = {1}", total, num5 * 3);
                }

                //OLD METHOD:
                //if (overTri1)
                //{
                //    unkShortSectorData = new short[nTriangles];

                //    for (int i = 0; i != nTriangles; i++)
                //        unkShortSectorData[i] = reader.ReadInt16();

                //    unkByteSectorData = reader.ReadBytes(nTriangles);
                //}
                //else
                //{
                //    for (int i = 0; i != sections.Length; i++)
                //        sections[i].EdgeData = reader.ReadBytes(sections[i].NumEdges);
                //}

                opcData = new OPCDataClass(reader);
                hbmData = new HBMDataClass(reader);
 
                unkSize = reader.ReadInt32();
                unkSizeData = reader.ReadBytes(unkSize);

                if (unkSize != nTriangles)
                    throw new FormatException("UnkSize does not equal nTriangles:");

                this.sections = sections;
            }
            public void WriteToFile(BinaryWriter writer)
            {
                writer.Write(22239310);
                writer.Write(1213416781);
                writer.Write(num1);
                writer.Write(num2);
                writer.Write(unkSmall);
                writer.Write(num3);
                writer.Write(num4);

                writer.Write(nPoints);
                writer.Write(nTriangles);

                for (int i = 0; i != points.Length; i++)
                    points[i].WriteToFile(writer);

                for (int i = 0; i != triangles.Length; i++)
                    triangles[i].WriteToFile(writer);

                for (int i = 0; i != unkShorts.Length; i++)
                    writer.Write((short)unkShorts[i]);

                //bool overTri1 = false;

                if (num2 == 3)
                {
                    writer.Write(num5);
                    if(nTriangles <= 256)
                    {
                        writer.Write(unkData);
                    }
                    else
                    {
                        foreach (short s in unkBytes)
                            writer.Write(s);
                    }

                    //OLD METHOD::
                    //if (num5 != nTriangles - 1)
                    //{
                    //    overTri1 = true;
                    //}
                }

                writer.Write(unk0);
                writer.Write(unk1);

                for (int i = 0; i != nTriangles; i++)
                    writer.Write(unkShortSectorData[i]);

                writer.Write(UnkByteSectorData);

                //OLD METHOD::
                //if (overTri1)
                //{
                //    for (int i = 0; i != nTriangles; i++)
                //        writer.Write(unkShortSectorData[i]);

                //    writer.Write(UnkByteSectorData);
                //}
                //else
                //{
                //    //sections for 2 is 186811
                //    for (int i = 0; i != sections.Length; i++)
                //        writer.Write(sections[i].EdgeData);
                //}

                opcData.WriteToFile(writer);
                hbmData.WriteToFile(writer);

                writer.Write(unkSize);
                writer.Write(unkSizeData);
            }
            public int GetMeshSize()
            {
                int size = 0;

                //header data is 36 bytes long;
                size += 36;

                for (int i = 0; i != points.Length; i++)
                    size += 12;

                for (int i = 0; i != triangles.Length; i++)
                    size += 12;

                for (int i = 0; i != unkShorts.Length; i++)
                    size += 2;

                bool overTri1 = false;

                if (num2 == 3)
                {
                    size += 4;
                    if (nTriangles <= 256)
                    {
                        size += unkData.Length;
                    }
                    else
                    {
                        size += (unkBytes.Length * 2); //unkbytes is 'short' array;
                    }

                    if (num5 != nTriangles - 1)
                    {
                        overTri1 = true;
                    }
                }

                //unk0 and unk1;
                size += 8;

                size += (nTriangles * 3);

                //OLD
                //if (overTri1)
                //{
                //    size += (nTriangles * 3);
                //}
                //else
                //{
                //    for (int i = 0; i != sections.Length; i++)
                //        size += sections[i].NumEdges;
                //}

                opcData.GetSize(ref size);
                hbmData.GetSize(ref size);

                size += 4;
                size += unkSize;
                return size;
            }
            public void BuildBasicCollision(Lod model)
            {
                nxs = Convert.ToString(22239310);
                mesh = Convert.ToString(1213416781);


                num1 = 1;
                num2 = 1;
                unkSmall = 0.001f;
                num3 = 255;
                num4 = 0;

                List<Int3> ltriangles = new List<Int3>();
                List<CollisionMaterials> lmatTypes = new List<CollisionMaterials>();

                for(int i = 0; i != model.Parts.Length; i++)
                {
                    for (int x = 0; x != model.Parts[i].Indices.Length; x++)
                    {
                        ltriangles.Add(new Int3(model.Parts[i].Indices[x]));
                        lmatTypes.Add(ConvertCollisionMats(model.Parts[i].Material));
                    }
                }

                nPoints = model.Vertices.Length;
                nTriangles = ltriangles.Count;

                points = new Vector3[nPoints];

                for (int i = 0; i != points.Length; i++)
                    points[i] = model.Vertices[i].Position;

                triangles = ltriangles.ToArray();
                unkShorts = lmatTypes.ToArray();

                unk0 = 1;
                unk1 = 1;

                unkShortSectorData = new short[nTriangles];
                unkByteSectorData = new byte[nTriangles];

                opcData = new OPCDataClass();
                opcData.BuildBasicOPC();
                hbmData = new HBMDataClass();
                hbmData.BuildBasicHBM();

                //sort out bounding box/sphere
                List<Vertex[]> data = new List<Vertex[]>();
                data.Add(model.Vertices);
                hbmData.BoundingBox = new BoundingBox();
                hbmData.BoundingSphere = new BoundingSphere(new Vector3(0), 0);
                hbmData.BoundingBox.CalculateBounds(data);
                hbmData.UpdateSphereBounds();

                unkSize = nTriangles;
                unkSizeData = new byte[unkSize];
                for (int i = 0; i != unkSizeData.Length; i++)
                    unkSizeData[i] = 26;
            }
            private CollisionMaterials ConvertCollisionMats(string name)
            {
                name = name.ToLower();

                switch (name)
                {
                    case "rock":
                        return CollisionMaterials.OBSOLETE_Carpet;
                    case "grass":
                        return CollisionMaterials.GrassAndSnow;
                    case "invis_col":
                        return CollisionMaterials.PlayerCollision;
                    case "road":
                        return CollisionMaterials.Tarmac;
                    case "soil":
                    case "gravel":
                    case "dirt":
                        return CollisionMaterials.Water;
                    default:
                        Console.WriteLine("ERROR! Unknown col type: {0}", name);
                        return CollisionMaterials.OBSOLETE_Plaster;
                }
            }

            [TypeConverter(typeof(ExpandableObjectConverter))]
            public class OPCDataClass
            {
                private int opcSize; //possible.
                private int opcVersion; //1
                private int opcType; //if this is 4; there is no data. If 3, data is here.
                private int opcCount; //only available if type is 3.
                private UnkOPCData[] opcData; //8 halfs, with a long value.
                private float[] opcFloats; //6 floats.

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

                public OPCDataClass(BinaryReader reader)
                {
                    ReadFromFile(reader);
                }

                public OPCDataClass()
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
                        for (int i = 0; i != opcData.Length; i++)
                            opcData[i] = new UnkOPCData(reader);

                        opcFloats = new float[6];
                        for (int i = 0; i != 6; i++)
                            opcFloats[i] = reader.ReadSingle();
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
                        for (int i = 0; i != opcData.Length; i++)
                            opcData[i].WriteToFile(writer);

                        for (int i = 0; i != 6; i++)
                            writer.Write(opcFloats[i]);
                    }
                }
                public void BuildBasicOPC()
                {
                    opcSize = 28;

                    //BEGIN OPC/HBM SECTION.
                    string opc = Convert.ToString(21188687);

                    opcVersion = 1;
                    opcType = 4;
                }
                public void GetSize(ref int size)
                {
                    //size, opc header, opc version and type.
                    size += 16;

                    if (opcType == 3)
                    {
                        size += 4;

                        for (int i = 0; i != opcData.Length; i++)
                            size += 20;

                        for (int i = 0; i != 6; i++)
                            size += 4;
                    }
                }
                public override string ToString()
                {
                    return "OPC Data";
                }

                public struct UnkOPCData
                {
                    private short[] unkHalfs;
                    private float[] unkFloats1;
                    private float[] unkFloats2;
                    private int unkInt;

                    public short[] UnkHalfs {
                        get { return unkHalfs; }
                        set { unkHalfs = value; }
                    }
                    public float[] UnkFloats1 {
                        get { return unkFloats1; }
                        set { unkFloats1 = value; }
                    }
                    public float[] UnkFloats2 {
                        get { return unkFloats2; }
                        set { unkFloats2 = value; }
                    }
                    public int UnkInt {
                        get { return unkInt; }
                        set { unkInt = value; }
                    }

                    public UnkOPCData(BinaryReader reader)
                    {
                        unkHalfs = new short[8];
                        unkFloats1 = new float[8];
                        unkFloats2 = new float[8];

                        for (int i = 0; i != unkHalfs.Length; i++)
                        {
                            unkHalfs[i] = reader.ReadInt16();
                        }

                        unkInt = reader.ReadInt32();

                        for (int i = 0; i != unkHalfs.Length; i++)
                        {
                            if (unkInt != 0 && unkHalfs[i] != 0)
                            {
                                unkFloats1[i] = unkHalfs[i] / unkInt;
                                unkFloats2[i] = unkInt / unkHalfs[i];
                            }
                        }
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

            [TypeConverter(typeof(ExpandableObjectConverter))]
            public class HBMDataClass
            {
                private int hbmVersion; //0
                private int hbmOffset; //potentially opcCount+1;
                private int hbmMaxOffset;
                private byte[] hbmOffsetData;
                private int hbmNumRefs;
                private int hbmMaxRefValue;
                private short[] hbmRefs;
                private BoundingBox boundingBox;
                private BoundingSphere boundingSphere;
                private float[] hbmUnkFloats;

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
                public float[] HBMUnkFloats {
                    get { return hbmUnkFloats; }
                    set { hbmUnkFloats = value; }
                }
                public BoundingBox BoundingBox {
                    get { return boundingBox; }
                    set { boundingBox = value; }
                }
                public BoundingSphere BoundingSphere {
                    get { return boundingSphere; }
                    set { boundingSphere = value; }
                }

                public HBMDataClass(BinaryReader reader)
                {
                    ReadFromFile(reader);
                }

                public HBMDataClass()
                {
                }

                public void ReadFromFile(BinaryReader reader)
                {
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

                    hbmUnkFloats = new float[14];
                    hbmUnkFloats[0] = reader.ReadSingle();

                    boundingSphere = new BoundingSphere();
                    boundingSphere.ReadToFile(reader);

                    boundingBox = new BoundingBox();
                    boundingBox.ReadToFile(reader);

                    for (int i = 1; i != hbmUnkFloats.Length; i++)
                        hbmUnkFloats[i] = reader.ReadSingle();
                }
                public void WriteToFile(BinaryWriter writer)
                {
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

                    writer.Write(hbmUnkFloats[0]);

                    boundingSphere.WriteToFile(writer);
                    boundingBox.WriteToFile(writer);

                    for (int i = 1; i != hbmUnkFloats.Length; i++)
                        writer.Write(hbmUnkFloats[i]);
                }
                public void BuildBasicHBM()
                {
                    string hbm = Convert.ToString(21840456);

                    hbmVersion = 0;
                    hbmOffset = 1;
                    hbmMaxOffset = 0;

                    boundingBox = new BoundingBox();

                    hbmUnkFloats = new float[14];
                }
                public void UpdateSphereBounds()
                {
                    boundingSphere.CreateFromBoundingBox(boundingBox);
                }
                public void GetSize(ref int size)
                {
                    //HBM header, version, offset and maxoffset;
                    size += 16;

                    if (hbmOffset > 1)
                    {
                        size += hbmOffsetData.Length;

                        size += 4;
                        if (hbmNumRefs != 0)
                            throw new NotImplementedException();
                    }

                    for (int i = 0; i != 24; i++)
                        size += 4;
                }
                public override string ToString()
                {
                    return "HBM Data";
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