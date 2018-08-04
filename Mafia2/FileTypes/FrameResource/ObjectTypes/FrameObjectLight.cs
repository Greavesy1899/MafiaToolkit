using System.IO;

namespace Mafia2
{
    public class FrameObjectLight : FrameObjectJoint
    {
        int flags;
        float[] unkFloat1 = new float[7];
        int unk_int;
        float[] unkFloat2 = new float[5];
        byte unk_byte1;
        float[] unkFloat3 = new float[17];
        byte unk_byte2;
        float[] unkFloat4 = new float[5];
        Hash nameLight;
        int unk_int2;
        float[] unkFloat5 = new float[20];
        Hash[] names = new Hash[4];
        Vector3 unkVector1;
        Vector3 unkVector2;
        byte unk_byte3;
        Vector3 unkVector3;
        Vector3 unkVector4;
        Vector3 unkVector5;
        Vector3 unkVector6;

        public FrameObjectLight(BinaryReader reader) : base()
        {
            ReadFromFile(reader);
        }

        public override void ReadFromFile(BinaryReader reader)
        {
            base.ReadFromFile(reader);
            flags = reader.ReadInt32();

            for (int i = 0; i < 7; i++)
                unkFloat1[i] = reader.ReadSingle();

            unk_int = reader.ReadInt32();

            for (int i = 0; i < 5; i++)
                unkFloat2[i] = reader.ReadSingle();

            unk_byte1 = reader.ReadByte();

            for (int i = 0; i < 17; i++)
                unkFloat3[i] = reader.ReadSingle();

            unk_byte2 = reader.ReadByte();

            for (int i = 0; i < 5; i++)
                unkFloat4[i] = reader.ReadSingle();

            nameLight = new Hash(reader);

            unk_int2 = reader.ReadInt32();

            for (int i = 0; i < 20; i++)
                unkFloat5[i] = reader.ReadSingle();

            for (int i = 0; i < 4; i++)
                names[i] = new Hash(reader);

            unkVector1 = new Vector3(reader);
            unkVector2 = new Vector3(reader);
            unk_byte3 = reader.ReadByte();
            unkVector3 = new Vector3(reader);
            unkVector4 = new Vector3(reader);
            unkVector5 = new Vector3(reader);
            unkVector6 = new Vector3(reader);
        }
        public override void WriteToFile(BinaryWriter writer)
        {
            base.WriteToFile(writer);
            writer.Write(flags);

            for (int i = 0; i < 7; i++)
                writer.Write(unkFloat1[i]);

            writer.Write(unk_int);

            for (int i = 0; i < 5; i++)
                writer.Write(unkFloat2[i]);

            writer.Write(unk_byte1);

            for (int i = 0; i < 17; i++)
                writer.Write(unkFloat3[i]);

            writer.Write(unk_byte2);

            for (int i = 0; i < 5; i++)
                writer.Write(unkFloat4[i]);

            nameLight.WriteToFile(writer);

            writer.Write(unk_int2);

            for (int i = 0; i != 20; i++)
                writer.Write(unkFloat5[i]);

            for (int i = 0; i != 4; i++)
                names[i].WriteToFile(writer);

            unkVector1.WriteToFile(writer);
            unkVector2.WriteToFile(writer);
            writer.Write(unk_byte3);
            unkVector3.WriteToFile(writer);
            unkVector4.WriteToFile(writer);
            unkVector5.WriteToFile(writer);
            unkVector6.WriteToFile(writer);
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
