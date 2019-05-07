using SharpDX;
using System.IO;
using Utils.SharpDXExtensions;

namespace ResourceTypes.FrameResource
{
    public class FrameGeometry : FrameEntry
    {
        byte numLods;
        short unk01_short = 0;
        Vector3 decompressionOffset;
        float decompressionFactor;
        FrameLOD[] lod;
        int unk02_int;

        public byte NumLods {
            get { return numLods; }
            set { numLods = value; }
        }
        public short Unk01 {
            get { return unk01_short; }
            set { unk01_short = value; }
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
        public int Unk02 {
            get { return unk02_int; }
            set { unk02_int = value; }
        }

        /// <summary>
        /// Construct FrameGeometry from stream data.
        /// </summary>
        /// <param name="reader"></param>
        public FrameGeometry(BinaryReader reader) : base()
        {
            ReadFromFile(reader);
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
        public void ReadFromFile(BinaryReader reader)
        {
            numLods = reader.ReadByte();
            unk01_short = reader.ReadInt16();

            decompressionOffset = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            decompressionFactor = reader.ReadSingle();

            LOD = new FrameLOD[numLods];

            for (int i = 0; i < numLods; i++)
            {
                LOD[i] = new FrameLOD();
                LOD[i].ReadFromFile(reader);
            }
        }


        /// <summary>
        /// Write data to stream.
        /// </summary>
        /// <param name="writer"></param>
        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(numLods);
            writer.Write(unk01_short);
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
