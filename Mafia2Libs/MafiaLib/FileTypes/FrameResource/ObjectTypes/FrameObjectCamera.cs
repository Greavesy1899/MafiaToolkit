using System.IO;
using Utils.Extensions;
using Utils.Types;

namespace ResourceTypes.FrameResource
{
    public class FrameObjectCamera : FrameObjectJoint
    {
        int unk01;
        unkStruct[] unkData;

        public int Unk01 {
            get { return unk01; }
            set { unk01 = value; }
        }
        public unkStruct[] UnkData {
            get { return unkData; }
            set { unkData = value; }
        }

        public FrameObjectCamera() : base()
        {
            unk01 = 0;
        }

        public FrameObjectCamera(MemoryStream reader, bool isBigEndian) : base()
        {
            ReadFromFile(reader, isBigEndian);
        }

        public FrameObjectCamera(FrameObjectCamera other) : base(other)
        {
            unk01 = other.unk01;
            unkData = other.unkData;
        }

        public override void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            base.ReadFromFile(reader, isBigEndian);
            unk01 = reader.ReadInt32(isBigEndian);

            unkData = new unkStruct[unk01];

            for (int i = 0; i != unk01; i++)
                unkData[i] = new unkStruct(reader, isBigEndian);
        }

        public override void WriteToFile(BinaryWriter writer)
        {
            base.WriteToFile(writer);
            writer.Write(unk01);

            if (unk01 <= 0)
                return;

            unkData = new unkStruct[unk01];

            for (int i = 0; i != unk01; i++)
                unkData[i].WriteToFile(writer);
        }

        public override string ToString()
        {
            return string.Format("Camera Block");
        }

        public struct unkStruct
        {
            float[] unkFloats;
            Hash unkHash;

            public unkStruct(MemoryStream reader, bool isBigEndian)
            {
                unkFloats = new float[5];

                for (int i = 0; i != 5; i++)
                    unkFloats[i] = reader.ReadSingle(isBigEndian);

                unkHash = new Hash(reader, isBigEndian);
            }

            public void WriteToFile(BinaryWriter writer)
            {
                for (int i = 0; i != 5; i++)
                    writer.Write(unkFloats[i]);

                unkHash.WriteToFile(writer);
            }
        }
    }
}
