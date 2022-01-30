using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Numerics;
using Utils.Extensions;
using Utils.Models;
using Utils.Types;
using Utils.VorticeUtils;

namespace ResourceTypes.FrameResource
{
    public class FrameLOD
    {

        public static VertexFlags[] VertexFlagOrder = new VertexFlags[14] {
            VertexFlags.Position,
            VertexFlags.Position2D,
            VertexFlags.Normals,
            VertexFlags.Skin,
            VertexFlags.Color,
            VertexFlags.Color1,
            VertexFlags.DamageGroup,
            VertexFlags.TexCoords0,
            VertexFlags.TexCoords1,
            VertexFlags.TexCoords2,
            VertexFlags.Unk05,
            VertexFlags.ShadowTexture,
            VertexFlags.BBCoeffs,
            VertexFlags.Tangent
        };

        float distance = 0;
        HashName indexBufferRef;
        VertexFlags vertexDeclaration;
        HashName vertexBufferRef;
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
        public HashName IndexBufferRef {
            get { return indexBufferRef; }
            set { indexBufferRef = value; }
        }
        public HashName VertexBufferRef {
            get { return vertexBufferRef; }
            set { vertexBufferRef = value; }
        }
        public int NumVerts {
            get { return numVerts; }
            set { numVerts = value; }
        }
        [Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
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

        public void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            distance = reader.ReadSingle(isBigEndian);
            indexBufferRef = new HashName(reader, isBigEndian);
            vertexDeclaration = (VertexFlags)reader.ReadUInt32(isBigEndian);
            vertexBufferRef = new HashName(reader, isBigEndian);
            numVerts = reader.ReadInt32(isBigEndian);
            nZero1 = reader.ReadInt32(isBigEndian);

            nPartition = reader.ReadInt32(isBigEndian);
            if (nPartition != 0)
                partitionInfo.ReadFromFile(reader, isBigEndian);

            matSplitType = reader.ReadInt32(isBigEndian);

            if (matSplitType != 0)
                splitInfo.ReadFromFile(reader, isBigEndian, matSplitType);

            zeroTail = reader.ReadInt32(isBigEndian);
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

        public void BuildNewPartition()
        {
            nPartition = 1;
            partitionInfo.BuildBlankPartition();
        }
        public void BuildNewMaterialSplit()
        {
            matSplitType = 0xC;
            splitInfo.BuildMaterialSplits();
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

            public MaterialSplit(MemoryStream reader, bool isBigEndian)
            {
                firstBurst = reader.ReadInt32(isBigEndian);
                numBurst = reader.ReadInt32(isBigEndian);
                baseIndex = reader.ReadInt32(isBigEndian);
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

            public MaterialBurst(MemoryStream reader, bool isBigEndian)
            {
                bounds = new short[6];

                for (int i = 0; i != 6; i++)
                    bounds[i] = reader.ReadInt16(isBigEndian);

                firstIndex = reader.ReadUInt16(isBigEndian);
                secondIndex = reader.ReadUInt16(isBigEndian);
                nleftIndex = reader.ReadInt16(isBigEndian);
                nrightIndex = reader.ReadInt16(isBigEndian);
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
            uint mPosData;
            uint mNegData;
            short[] mCenter;
            ushort[] mExtents;

            public uint PosData {
                get { return mPosData; }
                set { mPosData = value; }
            }
            public uint NegData {
                get { return mNegData; }
                set { mNegData = value; }
            }
            public uint PosLeafIndex { get { return PosData / 20; } }
            public uint NegLeafIndex { get { return NegData / 20; } }
            public short[] QuantizedCenter {
                get { return mCenter; }
                set { mCenter = value; }
            }
            public ushort[] QuantizedExtents {
                get { return mExtents; }
                set { mExtents = value; }
            }

            public void ReadFromFile(MemoryStream reader, bool isBigEndian)
            {
                mPosData = reader.ReadUInt32(isBigEndian);
                mNegData = reader.ReadUInt32(isBigEndian);
                mCenter = new short[3] { reader.ReadInt16(isBigEndian), reader.ReadInt16(isBigEndian), reader.ReadInt16(isBigEndian) }; //packed
                mExtents = new ushort[3] { reader.ReadUInt16(isBigEndian), reader.ReadUInt16(isBigEndian), reader.ReadUInt16(isBigEndian) }; //packed
            }

            public void WriteToFile(BinaryWriter writer)
            {
                writer.Write(mPosData);
                for (int i = 0; i != mCenter.Length; i++)
                    writer.Write(mCenter[i]);
                for (int i = 0; i != mExtents.Length; i++)
                    writer.Write(mExtents[i]);
                writer.Write(mNegData);
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
            Descriptor[] numDesc1;
            bool isAvailB;
            int numLongs1Length;
            uint[] numLongs1;
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
            public int numLong1 {
                get { return numLongs1Length; }
                set { numLongs1Length = value; }
            }
            public uint[] Longs1 {
                get { return numLongs1; }
                set {
                    numLongs1Length = value.Length;
                    numLongs1 = value;
                }
            }
            public int numLong2 {
                get { return numLongs2Length; }
                set { numLongs2Length = value; }
            }
            public int[] Longs2 {
                get { return numLongs2; }
                set {
                    numLongs2Length = value.Length;
                    numLongs2 = value;
                }
            }
            public int NumDesc1Length {
                get { return numDesc1Length; }
                set { numDesc1Length = value; }
            }
            public Descriptor[] Descriptors {
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

            public void ReadFromFile(MemoryStream reader, bool isBigEndian)
            {
                memRequireA = reader.ReadInt32(isBigEndian);
                memRequireB = reader.ReadInt32(isBigEndian);
                partitionType = reader.ReadInt32(isBigEndian);
                isBone = reader.ReadBoolean();

                if (isBone)
                {
                    offsetVector = new Vector3(reader.ReadSingle(isBigEndian), reader.ReadSingle(isBigEndian), reader.ReadSingle(isBigEndian));
                    scaleVector = new Vector3(reader.ReadSingle(isBigEndian), reader.ReadSingle(isBigEndian), reader.ReadSingle(isBigEndian));

                    numDesc1Length = reader.ReadInt32(isBigEndian);
                    numDesc1 = new Descriptor[numDesc1Length];
                    for (int i = 0; i != numDesc1.Length; i++)
                    {
                        numDesc1[i] = new Descriptor();
                        numDesc1[i].ReadFromFile(reader, isBigEndian);
                        //numDesc1[i].Unpack(offsetVector, scaleVector);
                    }
                }
                numLongs1Length = reader.ReadInt32(isBigEndian);
                isAvailB = reader.ReadBoolean();
                if (isAvailB)
                {

                    numLongs1 = new uint[numLongs1Length];
                    for (int i = 0; i != numLongs1.Length; i++)
                    {
                        numLongs1[i] = reader.ReadUInt32(isBigEndian);
                        uint NbTriangles = (numLongs1[i] & 15) + 1;
                        uint TriangleIndex = (numLongs1[i] >> 4);
                    }
                }
                numLongs2Length = reader.ReadInt32(isBigEndian);
                isAvailC = reader.ReadBoolean();
                if(isAvailC)
                { 
                    numLongs2 = new int[numLongs2Length];
                    for (int i = 0; i != numLongs2.Length; i++)
                    {
                        numLongs2[i] = reader.ReadInt32(isBigEndian);
                    }
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
                    for (int i = 0; i != numDesc1.Length; i++)
                    {
                        numDesc1[i].WriteToFile(writer);
                    }
                }
                writer.Write(numLongs1Length);
                writer.Write(isAvailB);
                if (isAvailB)
                {
                    for (int i = 0; i != numLongs1.Length; i++)
                    {
                        writer.Write(numLongs1[i]);
                    }

                }
                writer.Write(numLongs2Length);
                writer.Write(isAvailC);
                if (isAvailC)
                {
                    for (int i = 0; i != numLongs2.Length; i++)
                    {
                        writer.Write(numLongs2[i]);
                    }
                }
            }

            public void BuildBlankPartition()
            {
                isBone = false;
                isAvailB = false;
                isAvailC = false;
                memRequireA = 0x44;
                memRequireB = 0x44;
                partitionType = 4;
                Longs1 = new uint[0];
                Longs2 = new int[0];
                Descriptors = new Descriptor[0];
                numDesc1Length = 0;
                numLongs1Length = numDesc1Length + 1;
                numLongs2Length = 0;
                offsetVector = new Vector3(0.0f, 0.0f, 0.0f);
                scaleVector = new Vector3(1.0f, 1.0f, 1.0f);
            }
        }
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class MaterialSplitInfo
        {
            int indexStride = 0;
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
            ulong hash;
            MaterialBurst[] materialBursts;
            MaterialSplit[] materialSplits;

            public bool AvailD {
                get { return availD; }
                set { availD = value; }
            }
            public int IndexStride {
                get { return indexStride; }
                set { indexStride = value; }
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
            public ulong Hash {
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

            public void ReadFromFile(MemoryStream reader, bool isBigEndian, int type)
            {
                if (type == 12)
                {
                    indexStride = reader.ReadInt32(isBigEndian); //2
                    numVerts = reader.ReadInt32(isBigEndian);
                    numFaces = reader.ReadInt32(isBigEndian);
                    unk20 = reader.ReadInt32(isBigEndian); //0
                    unk21 = reader.ReadInt32(isBigEndian); //12
                    numSplitGroup = reader.ReadInt32(isBigEndian); //1

                    var result = indexStride + unk20 + unk21 + nSizeOfMatBurstEntries + nSizeOfMatSplitEntries;
                    if (result != 0xE && result != 0x10)
                    {
                        throw new Exception("does not equal 14 or 16");
                    }
                }

                if(type == 1)
                {
                    numSplitGroup = 1;
                }

                if (numSplitGroup == 1)
                {
                    availD = reader.ReadBoolean();
                    unk24 = reader.ReadInt32(isBigEndian);
                    nSizeOfMatBurstEntries = reader.ReadInt32(isBigEndian);
                    nSizeOfMatSplitEntries = reader.ReadInt32(isBigEndian);
                    numMatBurst = reader.ReadInt32(isBigEndian);
                    numMatSplit = reader.ReadInt32(isBigEndian);
                    hash = reader.ReadUInt64(isBigEndian);
                    materialBursts = new MaterialBurst[numMatBurst];
                    materialSplits = new MaterialSplit[numMatSplit];


                    for (int i = 0; i != materialBursts.Length; i++)
                    {
                        materialBursts[i] = new MaterialBurst(reader, isBigEndian);
                    }
                    for (int i = 0; i != materialSplits.Length; i++)
                    {
                        materialSplits[i] = new MaterialSplit(reader, isBigEndian);
                    }
                }
            }

            public void WriteToFile(BinaryWriter writer, int type)
            {
                if (type == 12)
                {
                    writer.Write(indexStride);
                    writer.Write(numVerts);
                    writer.Write(numFaces);
                    writer.Write(unk20);
                    writer.Write(unk21);
                    writer.Write(numSplitGroup);
                }

                if (numSplitGroup == 1)
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

            public void BuildMaterialSplits()
            {
                indexStride = 2;
                unk20 = 0;
                unk21 = 0xC;
                numSplitGroup = 1;
                availD = true;
                unk24 = 1;
                nSizeOfMatBurstEntries = 0x14;
                nSizeOfMatSplitEntries = 0xC;
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
                case VertexFlags.Skin:
                    return 8;
                case VertexFlags.Normals:
                case VertexFlags.Color:
                case VertexFlags.TexCoords0:
                case VertexFlags.TexCoords1:
                case VertexFlags.TexCoords2:
                case VertexFlags.Unk05:
                case VertexFlags.ShadowTexture:
                case VertexFlags.Color1:
                case VertexFlags.DamageGroup:
                    return 4;
                case VertexFlags.Tangent:
                    return 0;
                case VertexFlags.BBCoeffs:
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