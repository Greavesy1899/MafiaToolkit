using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using ThirdParty.OPCODE;
using Utils.Extensions;
using Utils.Logging;
using Utils.Models;
using Utils.Types;

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
        [Editor(typeof(FlagEnumUIEditor), typeof(UITypeEditor))]
        public VertexFlags VertexDeclaration {
            get { return vertexDeclaration; }
            set { vertexDeclaration = value; }
        }
        public uint OpcodeType { get; private set; }
        public uint MemRequireA { get; private set; }
        public uint MemRequireB { get; private set; }
        public HybridModel OpcodeModel { get; private set; }
        public MaterialSplitInfo SplitInfo {
            get { return splitInfo; }
            set { splitInfo = value; }
        }

        public void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            distance = reader.ReadSingle(isBigEndian);
            indexBufferRef = new HashName(reader, isBigEndian);
            vertexDeclaration = (VertexFlags)reader.ReadUInt32(isBigEndian);
            vertexBufferRef = new HashName(reader, isBigEndian);
            numVerts = reader.ReadInt32(isBigEndian);
            nZero1 = reader.ReadInt32(isBigEndian);

            // If present, read OPCODE data of this mesh
            OpcodeType = reader.ReadUInt32(isBigEndian);
            if (OpcodeType != 0)
            {
                MemRequireA = reader.ReadUInt32(isBigEndian);
                MemRequireB = reader.ReadUInt32(isBigEndian);
                ToolkitAssert.Ensure(MemRequireA == MemRequireB, "MemRequireA [{0}] and MemRequireB [{1}] should be the same.", MemRequireA, MemRequireB);

                OpcodeModel = new HybridModel();
                OpcodeModel.Load_FrameRes(reader, isBigEndian ? Endian.Big : Endian.Little);
            }

            // Read C_BPASubDiv object
            matSplitType = reader.ReadInt32(isBigEndian);
            if (matSplitType != 0)
            {
                splitInfo.ReadFromFile(reader, isBigEndian, matSplitType);
            }

            zeroTail = reader.ReadInt32(isBigEndian);
            ToolkitAssert.Ensure(zeroTail == 0, "This object should end with zero. It has ended with: [{0}]", zeroTail);
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(distance);
            indexBufferRef.WriteToFile(writer);
            writer.Write((uint)vertexDeclaration);
            vertexBufferRef.WriteToFile(writer);
            writer.Write(numVerts);
            writer.Write(nZero1);

            // Write Opcode data (if present) for this mesh
            writer.Write(OpcodeType);
            if(OpcodeType != 0)
            {
                // TODO: Figure out MemRequreA + MemRequireB
                // see if we can compute from HybridModel.
                writer.Write(MemRequireA);
                writer.Write(MemRequireB);

                // TODO: When FrameResource saves as big endian, 
                // make sure to pass endianess into this function.
                using (MemoryStream OpcodeStream = new MemoryStream())
                {
                    OpcodeModel.Save_FrameRes(OpcodeStream);
                    writer.Write(OpcodeStream.ToArray());
                }
            }

            writer.Write(matSplitType);

            if (matSplitType != 0)
            {
                splitInfo.WriteToFile(writer, matSplitType);
            }

            writer.Write(zeroTail);
        }

        public void BuildNewPartition()
        {
            // TODO: When saving an M2DE mesh, should this be 96?
            // I think MemRequireA and MemRequireB is affected by
            // whether the game is 32 or 64 bit.
            OpcodeType = 1;
            MemRequireA = 0x44; // 68
            MemRequireB = 0x44; // 68

            OpcodeModel = new HybridModel();
            OpcodeModel.BuildDefault();
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