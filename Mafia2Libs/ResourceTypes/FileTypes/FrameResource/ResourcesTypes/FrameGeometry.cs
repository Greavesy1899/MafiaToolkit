using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Utils.Extensions;
using Utils.VorticeUtils;

namespace ResourceTypes.FrameResource
{
    public class FrameGeometry : FrameEntry
    {
        byte numLods;
        short unk01 = 0;
        Vector3 decompressionOffset;
        float decompressionFactor;
        FrameLOD[] lod;
        public int geometryHash;

        public byte NumLods {
            get { return numLods; }
            set { numLods = value; }
        }
        public short Unk01 {
            get { return unk01; }
            set { unk01 = value; }
        }
        public Vector3 DecompressionOffset {
            get { return decompressionOffset; }
            set { decompressionOffset = value; }
        }
        public float DecompressionFactor {
            get { return decompressionFactor; }
            set { decompressionFactor = value; }
        }
        public FrameLOD[] LOD {
            get { return lod; }
            set { lod = value; }
        }

        public FrameGeometry(FrameResource OwningResource) : base(OwningResource) { }

        public void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            numLods = reader.ReadByte8();
            unk01 = reader.ReadInt16(isBigEndian);

            decompressionOffset = Vector3Utils.ReadFromFile(reader, isBigEndian);
            decompressionFactor = reader.ReadSingle(isBigEndian);

            LOD = new FrameLOD[numLods];
            int combinedHash = 0;
            for (int i = 0; i < numLods; i++)
            {
                LOD[i] = new FrameLOD();
                LOD[i].ReadFromFile(reader, isBigEndian);
                int lodHash = HashCode.Combine(LOD[i].IndexBufferRef, LOD[i].VertexBufferRef);
                combinedHash = HashCode.Combine(combinedHash, lodHash);
            }

            geometryHash = combinedHash;
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(numLods);
            writer.Write(unk01);
            decompressionOffset.WriteToFile(writer);
            writer.Write(decompressionFactor);

            for(int i = 0; i != numLods; i++)
            {
                LOD[i].WriteToFile(writer);
            }

        }

        public override string ToString()
        {
            return $"Geometry Block";
        }
    }
}
