using System.IO;

namespace Mafia2
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

        }
        public FrameObjectCamera(BinaryReader reader) : base()
        {
            ReadFromFile(reader);
        }
        public override void ReadFromFile(BinaryReader reader)
        {
            base.ReadFromFile(reader);
            unk01 = reader.ReadInt32();

            if (unk01 <= 0)
                return;

            unkData = new unkStruct[unk01];

            for (int i = 0; i != unk01; i++)
                unkData[i] = new unkStruct(reader);
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

            public unkStruct(BinaryReader reader)
            {
                unkFloats = new float[5];

                for (int i = 0; i != 5; i++)
                    unkFloats[i] = reader.ReadSingle();

                unkHash = new Hash(reader);
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
