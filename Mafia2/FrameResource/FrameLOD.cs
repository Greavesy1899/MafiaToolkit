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
            VertexFlags.DamageGroup,
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
        int nZero1;
        int zeroTail;
        int nPartition;

        int memRequireA;
        int memRequireB;
        int partitionType;
        Vector3 offsetVector;
        Vector3 scaleVector;
        int numDesc1Length;
        int unk1;
        Unk1_struct[] numDesc1;
        int unk2;
        bool isAvailB;
        int[] numLongs1;
        int numLongs2Length;
        bool isAvailC;
        int[] numLongs2;

        int matSplitType;
        MaterialSplitInfo splitInfo = new MaterialSplitInfo();

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

        public void ReadFromFile(BinaryReader reader)
        {
            distance = reader.ReadSingle();
            indexBufferRef = new Hash(reader);
            vertexDeclaration = (VertexFlags)reader.ReadUInt32();
            vertexBufferRef = new Hash(reader);
            numVerts = reader.ReadInt32();

            nZero1 = reader.ReadInt32();
            nPartition = reader.ReadInt32();

            if (nPartition != 0)
            {
                memRequireA = reader.ReadInt32();
                memRequireB = reader.ReadInt32();
                partitionType = reader.ReadInt32();
                isBone = reader.ReadBoolean();

                if (isBone)
                {
                    offsetVector = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    scaleVector = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

                    numDesc1Length = reader.ReadInt32();
                    unk1 = reader.ReadInt32();
                    numDesc1 = new Unk1_struct[numDesc1Length];
                    for (int i = 0; i != numDesc1.Length; i++)
                    {
                        numDesc1[i] = new Unk1_struct(reader);
                    }
                    isAvailB = reader.ReadBoolean();
                    unk2 = reader.ReadInt32();
                    numLongs1 = new int[numDesc1Length];
                    for (int i = 0; i != numLongs1.Length; i++)
                    {
                        numLongs1[i] = reader.ReadInt32();
                    }
                    numLongs2Length = reader.ReadInt32();
                    isAvailC = reader.ReadBoolean();
                    numLongs2 = new int[numLongs2Length];
                    for (int i = 0; i != numLongs2.Length; i++)
                    {
                        numLongs2[i] = reader.ReadInt32();
                    }
                }
                else
                {
                    reader.ReadBytes(10);
                }
            }

            matSplitType = reader.ReadInt32();

            if (matSplitType != 0)
            {
                splitInfo.unk18 = reader.ReadInt32();
                splitInfo.numVerts = reader.ReadInt32();
                splitInfo.numFaces = reader.ReadInt32();
                splitInfo.unk20 = reader.ReadInt32();
                splitInfo.unk21 = reader.ReadInt32();
                splitInfo.numSplitGroup = reader.ReadInt32();
                splitInfo.availD = reader.ReadBoolean();
                splitInfo.unk24 = reader.ReadInt32();
                splitInfo.nSizeOfMatBurstEntries = reader.ReadInt32();
                splitInfo.nSizeOfMatSplitEntries = reader.ReadInt32();
                splitInfo.numMatBurst = reader.ReadInt32();
                splitInfo.numMatSplit = reader.ReadInt32();
                splitInfo.hash = reader.ReadInt64();

                splitInfo.materialBursts = new MaterialBurst[splitInfo.numMatBurst];
                splitInfo.materialSplits = new MaterialSplit[splitInfo.numMatSplit];

                for(int i = 0; i != splitInfo.materialBursts.Length; i++)
                {
                    splitInfo.materialBursts[i] = new MaterialBurst(reader);
                }
                for (int i = 0; i != splitInfo.materialSplits.Length; i++)
                {
                    splitInfo.materialSplits[i] = new MaterialSplit(reader);
                }
            }
            zeroTail = reader.ReadInt32();
        }
        private struct Unk1_struct
        {
            int num1;
            int num2;
            short[] P0;
            short[] P1;

            public Unk1_struct(BinaryReader reader)
            {
                num1 = reader.ReadInt32();
                P0 = new short[3] { reader.ReadInt16(), reader.ReadInt16(), reader.ReadInt16()}; //packed
                P1 = new short[3] { reader.ReadInt16(), reader.ReadInt16(), reader.ReadInt16()}; //packed
                num2 = reader.ReadInt32();
            }
        }

        private struct MaterialSplit
        {
            int firstBurst;
            int numBurst;
            int baseIndex;

            public MaterialSplit(BinaryReader reader)
            {
                firstBurst = reader.ReadInt32();
                numBurst = reader.ReadInt32();
                baseIndex = reader.ReadInt32();
            }
        }

        private struct MaterialBurst
        {
            short[] bounds;
            ushort firstIndex;
            ushort secondIndex;
            short nleftIndex;
            short nrightIndex;

            public MaterialBurst(BinaryReader reader)
            {
                bounds = new short[6];

                for (int i = 0; i != 6; i++)
                    bounds[i] = reader.ReadInt16();

                firstIndex = reader.ReadUInt16();
                secondIndex = reader.ReadUInt16();
                nleftIndex = reader.ReadInt16();
                nrightIndex = reader.ReadInt16();
            }
        }

        private class MaterialSplitInfo
        {
            public int unk18 = 0;
            public int unk20 = 0;
            public int unk21 = 0;
            public int unk24 = 0;
            public int numVerts = 0;
            public int numFaces = 0;
            public int numSplitGroup = 0;
            public bool availD = false;
            public int unk26 = 0;
            public int nSizeOfMatBurstEntries = 0;
            public int nSizeOfMatSplitEntries = 0;
            public int numMatBurst = 0;
            public int numMatSplit = 0;
            public long hash;
            public MaterialBurst[] materialBursts;
            public MaterialSplit[] materialSplits;

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
                case VertexFlags.DamageGroup:
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