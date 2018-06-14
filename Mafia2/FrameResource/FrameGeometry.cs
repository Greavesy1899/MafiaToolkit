using System;
using System.IO;

namespace Mafia2
{
    public class FrameGeometry
    {
        byte numLods;
        short unk01_short = 0;
        Vector3 positionOffset;
        float positionFactor;
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
        public Vector3 PositionOffset {
            get { return positionOffset; }
            set { positionOffset = value; }
        }
        public float PositionFactor {
            get { return positionFactor; }
            set { positionFactor = value; }
        }
        public FrameLOD[] LOD {
            get { return lod; }
            set { lod = value; }
        }
        public int Unk02 {
            get { return unk02_int; }
            set { unk02_int = value; }
        }

        public FrameGeometry(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            numLods = reader.ReadByte();
            unk01_short = reader.ReadInt16();

            positionOffset = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            positionFactor = reader.ReadSingle();

            LOD = new FrameLOD[numLods];

            for (int i = 0; i < numLods; i++)
            {
                LOD[i] = new FrameLOD();
                LOD[i].ReadFromFile(reader);
            }
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(numLods);
            writer.Write(unk01_short);
            positionOffset.WriteToFile(writer);
            writer.Write(positionFactor);

        }

        public override string ToString()
        {
            return string.Format("Geometry Block");
        }
    }
}
