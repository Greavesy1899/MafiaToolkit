using System;
using System.Collections.Generic;
using System.IO;

namespace Mafia2
{
    public class FrameLOD
    {

        public static VertexFlags[] VertexFlagOrder = new VertexFlags[12] {
            VertexFlags.Position,
            VertexFlags.Normals,
            VertexFlags.BlendData,
            VertexFlags.flag_0x80,
            VertexFlags.flag_0x20000,
            VertexFlags.flag_0x100000,
            VertexFlags.TexCoords0,
            VertexFlags.TexCoords1,
            VertexFlags.TexCoords2,
            VertexFlags.TexCoords7,
            VertexFlags.flag_0x40000,
            VertexFlags.Tangent
        };

        float distance = 0;
        Hash indexBufferRef;
        VertexFlags vertexDeclaration;
        Hash vertexBufferRef;
        int numVerts;
        bool isBone;
        int count;
        int count2;
        bool flags2;
        int NumVerts;
        int NumFaces;
        int count__1;
        int count__2;

        public float Distance {
            get { return distance; }
            set { distance = value; }
        }
        public Hash IndexBufferRef {
            get { return indexBufferRef; }
            set { indexBufferRef = value; }
        }
        public Hash VertexBufferRef {
            get { return vertexBufferRef; }
            set { vertexBufferRef = value; }
        }
        public int NumVertsPr {
            get { return numVerts; }
            set { numVerts = value; }
        }
        public VertexFlags VertexDeclaration {
            get { return vertexDeclaration; }
            set { vertexDeclaration = value; }
        }
        public bool IsBone {
            get { return isBone; }
            set { isBone = value; }
        }

        //1st set of unknowns. This needs to be seperated into structs.
        int u_01_int;
        int c_01;
        int u_00_size;
        int u_01_size;
        int u_02_int;
        Vector3 u_00_vector;
        Vector3 u_01_vector;
        int u_00_int;
        int[] P0;
        int[] P1;
        int u_07_short;
        int u_04_int;
        int unknown_05_ints;
        bool u_06_bool;
        int[] int_arr;
        string u_b_arr;
        int u_04_i_2;
        int u_05_i_5;
        int u_06_i;
        int u_07_2;
        int c_1;
        string[] USELR;

        public void ReadFromFile(BinaryReader reader)
        {

            distance = reader.ReadSingle();
            indexBufferRef = new Hash(reader);
            vertexDeclaration = (VertexFlags)reader.ReadUInt32();
            vertexBufferRef = new Hash(reader);
            numVerts = reader.ReadInt32();

            u_01_int = reader.ReadInt32();
            c_01 = reader.ReadInt32();

            if (c_01 != 0)
            {

                u_00_size = reader.ReadInt32();
                u_01_size = reader.ReadInt32();
                u_02_int = reader.ReadInt32();
                isBone = reader.ReadBoolean();

                if (isBone)
                {
                    u_00_vector = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    u_01_vector = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

                    count = reader.ReadInt32();
                    u_01_int = reader.ReadInt32();

                    for (int i = 0; i != count; i++)
                    {
                        u_00_int = reader.ReadInt32();
                        P0 = new int[3] { reader.ReadInt16(), reader.ReadInt16(), reader.ReadInt16() };
                        P1 = new int[3] { reader.ReadInt16(), reader.ReadInt16(), reader.ReadInt16() };
                        u_07_short = reader.ReadInt32();
                    }

                    flags2 = reader.ReadBoolean();
                    u_04_int = reader.ReadInt32();

                    for (int i = 0; i != count; i++)
                    {
                        unknown_05_ints = reader.ReadInt32();
                    }

                    count2 = reader.ReadInt32();
                    u_06_bool = reader.ReadBoolean();

                    int_arr = new int[count2];
                    for (int i = 0; i != count2; i++)
                    {
                        int_arr[i] = reader.ReadInt32();
                    }
                }
                else
                {
                    u_b_arr = BitConverter.ToString(reader.ReadBytes(10)).Replace("-", " ");
                }
                u_04_i_2 = reader.ReadInt32();
                u_05_i_5 = reader.ReadInt32();
                NumVerts = reader.ReadInt32();
                NumFaces = reader.ReadInt32();
                u_06_i = reader.ReadInt32();
                u_07_2 = reader.ReadInt32();
            }
            c_1 = reader.ReadInt32();

            if (c_1 != 0)
            {
                //need to assign these; its 07:49am.
                reader.ReadBoolean();
                reader.ReadInt32();
                reader.ReadInt32();
                reader.ReadInt32();
                count__1 = reader.ReadInt32();
                count__2 = reader.ReadInt32();
                reader.ReadInt64();
                USELR = new string[count__1];
                for (int i = 0; i != count__1; i++)
                {
                    USELR[i] = string.Format("{0,-3} {1,-3} {2,-3} {3,-3} {4,-3} {5,-3} {6,-5} {7,-5} {8,-5} {9}", reader.ReadInt16(), reader.ReadInt16(), reader.ReadInt16(), reader.ReadInt16(), reader.ReadInt16(), reader.ReadInt16(), reader.ReadInt16(), reader.ReadInt16(), reader.ReadInt16(), reader.ReadInt16());
                }

                for (int i = 0; i < count__2; i++)
                {
                    reader.ReadInt32();
                    reader.ReadInt32();
                    reader.ReadInt32();
                }
            }
            reader.ReadInt32();
        }

        private struct Unk1_struct
        {
        }

        public struct VertexOffset
        {
            int offset;
            int length;

            public int Offset {
                get { return offset; }
                set { offset = value; }
            }

            public int Length {
                get { return length; }
                set { length = value; }
            }
        }

        public Dictionary<VertexFlags, VertexOffset> GetVertexOffsets(out int stride)
        {
            Dictionary<VertexFlags, VertexOffset> dictionary = new Dictionary<VertexFlags, VertexOffset>();
            int num = 0;
            foreach (VertexFlags vertexFlags in VertexFlagOrder)
            {
                if (vertexDeclaration.HasFlag(vertexFlags))
                {
                    int vertexComponentLength = GetVertexComponentLength(vertexFlags);
                    if (vertexComponentLength > 0)
                    {
                        VertexOffset vertexOffset = new VertexOffset()
                        {
                            Offset = num,
                            Length = vertexComponentLength
                        };
                        dictionary.Add(vertexFlags, vertexOffset);
                        num += vertexComponentLength;
                    }
                }
            }
            stride = num;
            return dictionary;
        }
        private static int GetVertexComponentLength(VertexFlags flags)
        {
            switch (flags)
            {
                case VertexFlags.Position:
                case VertexFlags.BlendData:
                    return 8;
                case VertexFlags.Normals:
                case VertexFlags.flag_0x80:
                case VertexFlags.TexCoords0:
                case VertexFlags.TexCoords1:
                case VertexFlags.TexCoords2:
                case VertexFlags.TexCoords7:
                case VertexFlags.flag_0x20000:
                case VertexFlags.flag_0x100000:
                    return 4;
                case VertexFlags.Tangent:
                    return 0;
                case VertexFlags.flag_0x40000:
                    return 12;
                default:
                    return -1;
            }
        }

        public override string ToString()
        {
            return string.Format("LOD Block");
        }

    }
}