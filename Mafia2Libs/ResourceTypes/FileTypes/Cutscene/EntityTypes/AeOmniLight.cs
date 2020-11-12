using SharpDX;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using Utils.Extensions;
using Utils.SharpDXExtensions;

namespace ResourceTypes.Cutscene.AnimEntities
{
    public class AeOmniLightWrapper : AnimEntityWrapper
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public AeOmniLight OmniLightEntity { get; set; }

        public AeOmniLightWrapper() : base()
        {
            OmniLightEntity = new AeOmniLight();
            AnimEntityData = new AeOmniLightData();
        }

        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            OmniLightEntity.ReadFromFile(stream, isBigEndian);
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);
            OmniLightEntity.WriteToFile(stream, isBigEndian);
        }

        public override AnimEntityTypes GetEntityType()
        {
            return AnimEntityTypes.AeOmniLight;
        }
    }

    public class AeOmniLight : AnimEntity
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
        public float[] Unk10_1_Floats_1 { get; set; }
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

            if (Unk10 == 0)
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
            else if (Unk10 == 1)
            {
                Unk10_1_Int = stream.ReadInt32(isBigEndian);

                Unk10_1_Floats_1 = new float[20];
                for (int i = 0; i < 20; i++)
                {
                    Unk10_1_Floats_1[i] = stream.ReadSingle(isBigEndian);
                }
                Unk10_1_Strings = new string[3];
                for (int i = 0; i < 3; i++)
                {
                    Unk10_1_Strings[i] = stream.ReadString16(isBigEndian);
                }
            }
            else if (Unk10 == 2)
            {
                Unk10_1_Floats = new float[10];
                for (int i = 0; i < 10; i++)
                {
                    Unk10_1_Floats[i] = stream.ReadSingle(isBigEndian);
                }

                Unk11 = stream.ReadInt32(isBigEndian);
                Unk12 = stream.ReadInt32(isBigEndian);
                ProjectorTexture = stream.ReadString16(isBigEndian);

                Unk10_1_Floats_1 = new float[20];
                for (int i = 0; i < 20; i++)
                {
                    Unk10_1_Floats_1[i] = stream.ReadSingle(isBigEndian);
                }
                Unk10_1_Strings = new string[3];
                for (int i = 0; i < 3; i++)
                {
                    Unk10_1_Strings[i] = stream.ReadString16(isBigEndian);
                }
            }
            else
            {
                throw new FileFormatException();
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

            if(Unk10 == 0)
            {
                foreach (float Value in Unk08)
                {
                    stream.Write(Value, isBigEndian);
                }

                stream.Write(Unk11, isBigEndian);
                stream.Write(Unk12, isBigEndian);
                stream.WriteString16(ProjectorTexture, isBigEndian);
            }
            else if(Unk10 == 1)
            {
                stream.Write(Unk10_1_Int, isBigEndian);
                foreach (var Value in Unk10_1_Floats_1)
                {
                    stream.Write(Value, isBigEndian);
                }
                foreach (var Value in Unk10_1_Strings)
                {
                    stream.WriteString16(Value, isBigEndian);
                }
            }
            else if(Unk10 == 2)
            {
                foreach (var Value in Unk10_1_Floats)
                {
                    stream.Write(Value, isBigEndian);
                }

                stream.Write(Unk11, isBigEndian);
                stream.Write(Unk12, isBigEndian);
                stream.WriteString16(ProjectorTexture, isBigEndian);

                foreach (var Value in Unk10_1_Floats_1)
                {
                    stream.Write(Value, isBigEndian);
                }
                foreach (var Value in Unk10_1_Strings)
                {
                    stream.WriteString16(Value, isBigEndian);
                }
            }

            UpdateSize(stream, isBigEndian);
        }
    }

    public class AeOmniLightData : AeBaseData
    {
        public byte Unk0 { get; set; }

        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            Debug.Assert(stream.Position != stream.Length, "I've read the parent class data, although i've hit the eof!");

            Unk0 = stream.ReadByte8();
            Debug.Assert(Unk0 == 0, "Unk0 did not equal zero");
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);
            stream.WriteByte(Unk0);
        }
    }

}
