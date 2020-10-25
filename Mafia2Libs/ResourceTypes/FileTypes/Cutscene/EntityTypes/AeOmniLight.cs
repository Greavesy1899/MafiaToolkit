using System;
using System.Diagnostics;
using System.IO;
using SharpDX;
using Utils.Extensions;
using Utils.SharpDXExtensions;

namespace ResourceTypes.Cutscene.AnimEntities
{
    //AeOmniLight
    public class AeOmniLight : AeBase
    {
        public byte Unk05 { get; set; }
        public int Unk06 { get; set; }
        public int Unk07 { get; set; }
        public Matrix Transform { get; set; }
        public int Unk09 { get; set; }
        public int Unk10 { get; set; }
        public float[] Unk08 { get; set; }
        public int Unk11 { get; set; }
        public int Unk12 { get; set; }
        public string ProjectorTexture { get; set; }

        // Only available if Unk10 is above 0
        public int Unk10_1_Int { get; set; } // Usually == 74
        public float[] Unk10_1_Floats { get; set; }
        public string[] Unk10_1_Strings { get; set; }

        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            Unk05 = stream.ReadByte8();
            Unk06 = stream.ReadInt32(isBigEndian);
            Unk07 = stream.ReadInt32(isBigEndian);
            Transform = MatrixExtensions.ReadFromFile(stream, isBigEndian);
            Unk09 = stream.ReadInt32(isBigEndian);
            Unk10 = stream.ReadInt32(isBigEndian);

            if (Unk10 > 0)
            {
                Unk10_1_Int = stream.ReadInt32(isBigEndian);

                Unk10_1_Floats = new float[20];
                for (int i = 0; i < 20; i++)
                {
                    Unk10_1_Floats[i] = stream.ReadSingle(isBigEndian);
                }
                Unk10_1_Strings = new string[3];
                for (int i = 0; i < 3; i++)
                {
                    Unk10_1_Strings[i] = stream.ReadString16(isBigEndian);
                }
            }
            else
            {
                Unk08 = new float[10];
                for (int i = 0; i < 10; i++)
                {
                    Unk08[i] = stream.ReadSingle(isBigEndian);
                }
                Unk11 = stream.ReadInt32(isBigEndian);
                Unk12 = stream.ReadInt32(isBigEndian);
                ProjectorTexture = stream.ReadString16(isBigEndian);
            }
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);

            stream.WriteByte(Unk05);
            stream.Write(Unk06, isBigEndian);
            stream.Write(Unk07, isBigEndian);
            Transform.WriteToFile(stream, isBigEndian);
            stream.Write(Unk09, isBigEndian);
            stream.Write(Unk10, isBigEndian);

            if (Unk10 > 0)
            {
                stream.Write(Unk10_1_Int, isBigEndian);

                foreach (var Value in Unk10_1_Floats)
                {
                    stream.Write(Value, isBigEndian);
                }
                foreach (var Value in Unk10_1_Strings)
                {
                    stream.WriteString16(Value, isBigEndian);
                }
            }
            else
            {
                foreach (float Value in Unk08)
                {
                    stream.Write(Value, isBigEndian);
                }

                stream.Write(Unk11, isBigEndian);
                stream.Write(Unk12, isBigEndian);
                stream.WriteString16(ProjectorTexture, isBigEndian);
            }
        }
        public override AnimEntityTypes GetEntityType()
        {
            return AnimEntityTypes.AeOmniLight;
        }
    }

    public class AeOmniLightData : AeBaseData
    {
        public byte Unk02 { get; set; }
        public int Unk03 { get; set; }
        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            Debug.Assert(stream.Position != stream.Length, "I've read the parent class data, although i've hit the eof!");

            Unk02 = stream.ReadByte8();

            // Could be an array of integers
            if (Unk02 == 1)
            {
                Unk03 = stream.ReadInt32(isBigEndian);
            }
            else if(Unk02 != 0)
            {
                Console.WriteLine("oof");
            }
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);
            stream.WriteByte(Unk02);

            // Could be an array of integers
            if(Unk02 == 1)
            {
                stream.Write(Unk03, isBigEndian);
            }
            else if (Unk02 != 0)
            {
                Console.WriteLine("oof");
            }
        }
    }
}
