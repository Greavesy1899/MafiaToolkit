using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        int nZero1;
        int zeroTail;
        int nPartition;
        PartitionInfo partitionInfo = new PartitionInfo();
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
        public MaterialSplitInfo SplitInfo {
            get { return splitInfo; }
            set { splitInfo = value; }
        }
        public PartitionInfo Partition {
            get { return partitionInfo; }
            set { partitionInfo = value; }
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
                partitionInfo.ReadFromFile(reader);

            matSplitType = reader.ReadInt32();

            if (matSplitType != 0)
                splitInfo.ReadFromFile(reader, matSplitType);

            zeroTail = reader.ReadInt32();
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(distance);
            indexBufferRef.WriteToFile(writer);
            writer.Write((uint)vertexDeclaration);
            vertexBufferRef.WriteToFile(writer);
            writer.Write(numVerts);
            writer.Write(nZero1);

            writer.Write(nPartition);
            if (nPartition != 0)
                partitionInfo.WriteToFile(writer);

            writer.Write(matSplitType);

            if (matSplitType != 0)
                splitInfo.WriteToFile(writer, matSplitType);

            writer.Write(zeroTail);
        }

        public struct MaterialSplit
        {
            int firstBurst;
            int numBurst;
            int baseIndex;

            public int FirstBurst {
                get { return firstBurst; }
                set { firstBurst = value; }
            }
            public int NumBurst {
                get { return numBurst; }
                set { numBurst = value; }
            }
            public int BaseIndex {
                get { return baseIndex; }
                set { baseIndex = value; }
            }

            public MaterialSplit(BinaryReader reader)
            {
                firstBurst = reader.ReadInt32();
                numBurst = reader.ReadInt32();
                baseIndex = reader.ReadInt32();
            }

            public void WriteToFile(BinaryWriter writer)
            {
                writer.Write(firstBurst);
                writer.Write(numBurst);
                writer.Write(baseIndex);
            }
        }
        public struct MaterialBurst
        {
            short[] bounds;
            ushort firstIndex;
            ushort secondIndex;
            short nleftIndex;
            short nrightIndex;

            public short[] Bounds {
                get { return bounds; }
                set { bounds = value; }
            }
            public ushort FirstIndex {
                get { return firstIndex; }
                set { firstIndex = value; }
            }
            public ushort SecondIndex {
                get { return secondIndex; }
                set { secondIndex = value; }
            }
            public short LeftIndex {
                get { return nleftIndex; }
                set { nleftIndex = value; }
            }
            public short RightIndex {
                get { return nrightIndex; }
                set { nrightIndex = value; }
            }

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

            public void WriteToFile(BinaryWriter writer)
            {
                for (int i = 0; i != 6; i++)
                    writer.Write(bounds[i]);

                writer.Write(firstIndex);
                writer.Write(secondIndex);
                writer.Write(nleftIndex);
                writer.Write(nrightIndex);
            }
        }
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public struct Descriptor
        {
            int num1;
            int num2;
            short[] p0;
            short[] p1;

            public int Num1 {
                get { return num1; }
                set { num1 = value; }
            }
            public int Num2 {
                get { return num2; }
                set { num2 = value; }
            }
            public short[] P0 {
                get { return p0; }
                set { p0 = value; }
            }
            public short[] P1 {
                get { return p1; }
                set { p1 = value; }
            }

            public Descriptor(BinaryReader reader)
            {
                num1 = reader.ReadInt32();
                p0 = new short[3] { reader.ReadInt16(), reader.ReadInt16(), reader.ReadInt16()}; //packed
                p1 = new short[3] { reader.ReadInt16(), reader.ReadInt16(), reader.ReadInt16()}; //packed
                num2 = reader.ReadInt32();
            }

            public void WriteToFile(BinaryWriter writer)
            {
                writer.Write(num1);
                for (int i = 0; i != p0.Length; i++)
                    writer.Write(p0[i]);
                for (int i = 0; i != p1.Length; i++)
                    writer.Write(p1[i]);
                writer.Write(num2);
            }
        }
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class PartitionInfo
        {
            int memRequireA;
            int memRequireB;
            int partitionType;
            Vector3 offsetVector;
            Vector3 scaleVector;
            int numDesc1Length;
            int unk1;
            Descriptor[] numDesc1;
            int unk2;
            bool isAvailB;
            int[] numLongs1;
            int numLongs2Length;
            bool isAvailC;
            int[] numLongs2;
            bool isBone;

            public bool IsBone {
                get { return isBone; }
                set { isBone = value; }
            }
            public bool IsAvailB {
                get { return isAvailB; }
                set { isAvailB = value; }
            }
            public bool IsAvailC {
                get { return isAvailC; }
                set { isAvailC = value; }
            }
            public int MemRequiredA {
                get { return memRequireA; }
                set { memRequireA = value; }
            }
            public int MemRequiredB {
                get { return memRequireB; }
                set { memRequireB = value; }
            }
            public int Type {
                get { return partitionType; }
                set { partitionType = value; }
            }
            public int Unk1 {
                get { return unk1; }
                set { unk1 = value; }
            }
            public int Unk2 {
                get { return unk2; }
                set { unk2 = value; }
            }
            public int[] Longs1 {
                get { return numLongs1; }
                set { numLongs1 = value; }
            }
            public int[] Longs2 {
                get { return numLongs2; }
                set { numLongs2 = value; }
            }
            public Descriptor[] descriptors {
                get { return numDesc1; }
                set { numDesc1 = value; }
            }
            public Vector3 OffsetVector {
                get { return offsetVector; }
                set { offsetVector = value; }
            }
            public Vector3 ScaleVector {
                get { return scaleVector; }
                set { scaleVector = value; }
            }

            public void ReadFromFile(BinaryReader reader)
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
                    numDesc1 = new Descriptor[numDesc1Length];
                    for (int i = 0; i != numDesc1.Length; i++)
                    {
                        numDesc1[i] = new Descriptor(reader);
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

            public void WriteToFile(BinaryWriter writer)
            {
                writer.Write(memRequireA);
                writer.Write(memRequireB);
                writer.Write(partitionType);
                writer.Write(isBone);

                if (isBone)
                {
                    offsetVector.WriteToFile(writer);
                    scaleVector.WriteToFile(writer);

                    writer.Write(numDesc1Length);
                    writer.Write(unk1);
                    for (int i = 0; i != numDesc1.Length; i++)
                    {
                        numDesc1[i].WriteToFile(writer);
                    }
                    writer.Write(isAvailB);
                    writer.Write(unk2);
                    for (int i = 0; i != numLongs1.Length; i++)
                    {
                        writer.Write(numLongs1[i]);
                    }
                    writer.Write(numLongs2Length);
                    writer.Write(isAvailC);
                    for (int i = 0; i != numLongs2.Length; i++)
                    {
                        writer.Write(numLongs2[i]);
                    }
                }
                else
                {
                    writer.Write((byte)1);
                    for (int i = 0; i != 9; i++)
                        writer.Write((byte)0);
                }
            }
        }
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class MaterialSplitInfo
        {
            int unk18 = 0;
            int unk20 = 0;
            int unk21 = 0;
            int unk24 = 0;
            int numVerts = 0;
            int numFaces = 0;
            int numSplitGroup = 0;
            bool availD = false;
            int unk26 = 0;
            int nSizeOfMatBurstEntries = 0;
            int nSizeOfMatSplitEntries = 0;
            int numMatBurst = 0;
            int numMatSplit = 0;
            long hash;
            MaterialBurst[] materialBursts;
            MaterialSplit[] materialSplits;

            public bool AvailD {
                get { return availD; }
                set { availD = value; }
            }
            public int Unk18 {
                get { return unk18; }
                set { unk18 = value; }
            }
            public int Unk20 {
                get { return unk20; }
                set { unk20 = value; }
            }
            public int Unk21 {
                get { return unk21; }
                set { unk21 = value; }
            }
            public int Unk24 {
                get { return unk24; }
                set { unk24 = value; }
            }
            public int Unk26 {
                get { return unk26; }
                set { unk26 = value; }
            }
            public int NSizeOfMatBurstEntries {
                get { return nSizeOfMatBurstEntries; }
                set { nSizeOfMatBurstEntries = value; }
            }
            public int NSizeOfMatSplitEntries {
                get { return nSizeOfMatSplitEntries; }
                set { nSizeOfMatSplitEntries = value; }
            }
            public int NumMatBurst {
                get { return numMatBurst; }
                set { numMatBurst = value; }
            }
            public int NumMatSplit {
                get { return numMatSplit; }
                set { numMatSplit = value; }
            }
            public int NumVerts {
                get { return numVerts; }
                set { numVerts = value; }
            }
            public int NumFaces {
                get { return numFaces; }
                set { numFaces = value; }
            }
            public int NumSplitGroup {
                get { return numSplitGroup; }
                set { numSplitGroup = value; }
            }
            public long Hash {
                get { return hash; }
                set { hash = value; }
            }
            public MaterialBurst[] MaterialBursts {
                get { return materialBursts; }
                set { materialBursts = value; }
            }
            public MaterialSplit[] MaterialSplits {
                get { return materialSplits; }
                set { materialSplits = value; }
            }

            public void ReadFromFile(BinaryReader reader, int type)
            {
                if (type != 1)
                {
                    unk18 = reader.ReadInt32();
                    numVerts = reader.ReadInt32();
                    numFaces = reader.ReadInt32();
                    unk20 = reader.ReadInt32();
                    unk21 = reader.ReadInt32();
                    numSplitGroup = reader.ReadInt32();
                }
                
                if(type == 1)
                {
                    availD = reader.ReadBoolean();
                    unk24 = reader.ReadInt32();
                    nSizeOfMatBurstEntries = reader.ReadInt32();
                    nSizeOfMatSplitEntries = reader.ReadInt32();
                    numMatBurst = reader.ReadInt32();
                    numMatSplit = reader.ReadInt32();

                    hash = reader.ReadInt64();

                    materialBursts = new MaterialBurst[numMatBurst];
                    materialSplits = new MaterialSplit[numMatSplit];


                    for (int i = 0; i != materialBursts.Length; i++)
                    {
                        materialBursts[i] = new MaterialBurst(reader);
                    }
                    for (int i = 0; i != materialSplits.Length; i++)
                    {
                        materialSplits[i] = new MaterialSplit(reader);
                    }
                }

                if (numSplitGroup != 0)
                {
                    availD = reader.ReadBoolean();
                    unk24 = reader.ReadInt32();
                    nSizeOfMatBurstEntries = reader.ReadInt32();
                    nSizeOfMatSplitEntries = reader.ReadInt32();
                    numMatBurst = reader.ReadInt32();
                    numMatSplit = reader.ReadInt32();

                    hash = reader.ReadInt64();

                    materialBursts = new MaterialBurst[numMatBurst];
                    materialSplits = new MaterialSplit[numMatSplit];


                    for (int i = 0; i != materialBursts.Length; i++)
                    {
                        materialBursts[i] = new MaterialBurst(reader);
                    }
                    for (int i = 0; i != materialSplits.Length; i++)
                    {
                        materialSplits[i] = new MaterialSplit(reader);
                    }
                }

            }

            public void WriteToFile(BinaryWriter writer, int type)
            {
                if (type != 1)
                {
                    writer.Write(unk18);
                    writer.Write(numVerts);
                    writer.Write(numFaces);
                    writer.Write(unk20);
                    writer.Write(unk21);
                    writer.Write(numSplitGroup);
                }

                if (type == 1)
                {
                    writer.Write(availD);
                    writer.Write(unk24);
                    writer.Write(nSizeOfMatBurstEntries);
                    writer.Write(nSizeOfMatSplitEntries);
                    writer.Write(numMatBurst);
                    writer.Write(numMatSplit);
                    writer.Write(hash);

                    for (int i = 0; i != materialBursts.Length; i++)
                    {
                        materialBursts[i].WriteToFile(writer);
                    }
                    for (int i = 0; i != materialSplits.Length; i++)
                    {
                        materialSplits[i].WriteToFile(writer);
                    }
                }

                if (numSplitGroup != 0)
                {
                    writer.Write(availD);
                    writer.Write(unk24);
                    writer.Write(nSizeOfMatBurstEntries);
                    writer.Write(nSizeOfMatSplitEntries);
                    writer.Write(numMatBurst);
                    writer.Write(numMatSplit);
                    writer.Write(hash);

                    for (int i = 0; i != materialBursts.Length; i++)
                    {
                        materialBursts[i].WriteToFile(writer);
                    }
                    for (int i = 0; i != materialSplits.Length; i++)
                    {
                        materialSplits[i].WriteToFile(writer);
                    }
                }

            }

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