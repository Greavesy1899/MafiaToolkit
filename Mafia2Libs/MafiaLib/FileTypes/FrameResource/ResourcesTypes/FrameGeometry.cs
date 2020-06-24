using SharpDX;
using System.IO;
using Utils.Extensions;
using Utils.SharpDXExtensions;

namespace ResourceTypes.FrameResource
{
    public class FrameGeometry : FrameEntry
    {
        byte numLods;
        short unk01 = 0;
        Vector3 decompressionOffset;
        float decompressionFactor;
        FrameLOD[] lod;

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

        /// <summary>
        /// Construct FrameGeometry from stream data.
        /// </summary>
        /// <param name="reader"></param>
        public FrameGeometry(MemoryStream reader, bool isBigEndian) : base()
        {
            ReadFromFile(reader, isBigEndian);
        }

        /// <summary>
        /// Construct FrameGeometry with default data.
        /// </summary>
        public FrameGeometry() : base()
        {

        }

        /// <summary>
        /// Read data from stream.
        /// </summary>
        /// <param name="reader"></param>
        public void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            numLods = reader.ReadByte8();
            unk01 = reader.ReadInt16(isBigEndian);

            decompressionOffset = Vector3Extenders.ReadFromFile(reader, isBigEndian);
            decompressionFactor = reader.ReadSingle(isBigEndian);

            LOD = new FrameLOD[numLods];

            for (int i = 0; i < numLods; i++)
            {
                LOD[i] = new FrameLOD();
                LOD[i].ReadFromFile(reader, isBigEndian);
            }
        }


        /// <summary>
        /// Write data to stream.
        /// </summary>
        /// <param name="writer"></param>
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
