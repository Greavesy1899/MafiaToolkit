using System.IO;
using Utils.Extensions;

namespace ResourceTypes.Cutscene.AnimEntities.LightTypes
{
    public class AeLightType1 : IAeLightType
    {
        public int Unk10_1_Int { get; set; } // Usually == 74
        public float[] Unk10_1_Floats_1 { get; set; }
        public string[] Unk10_1_Strings { get; set; }

        public void ReadFromFile(MemoryStream stream, bool isBigEndian)
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

        public void WriteToFile(MemoryStream stream, bool isBigEndian)
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
    }
}
