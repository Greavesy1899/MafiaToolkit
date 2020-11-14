using System.IO;
using Utils.Extensions;

namespace ResourceTypes.Cutscene.AnimEntities.LightTypes
{
    public interface IAeLightType
    {
        void ReadFromFile(MemoryStream stream, bool IsBigEndian);
        void WriteToFile(MemoryStream stream, bool IsBigEndian);
    }
    public class AeLightType2 : IAeLightType
    {
        public float[] Unk08 { get; set; }
        public int Unk11 { get; set; }
        public int Unk12 { get; set; }
        public float[] Unk10_1_Floats_1 { get; set; }
        public string[] Unk10_1_Strings { get; set; }
        public string ProjectorTexture { get; set; }

        public void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            Unk08 = new float[12];
            for (int i = 0; i < 12; i++)
            {
                Unk08[i] = stream.ReadSingle(isBigEndian);
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

        public void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            foreach (var Value in Unk08)
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
    }
}
