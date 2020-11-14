using System.IO;
using Utils.Extensions;

namespace ResourceTypes.Cutscene.AnimEntities.LightTypes
{
    public class AeLightType0 : IAeLightType
    {
        public float[] Unk08 { get; set; }
        public int Unk11 { get; set; }
        public int Unk12 { get; set; }
        public string ProjectorTexture { get; set; }

        private int NumFloats = 0;

        // TODO: Why does Omnilight have 10 and SpotLight have 12? 
        // We need to find this out.
        public void SetNumFloats(int NumFloats)
        {
            this.NumFloats = NumFloats;
        }

        public void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            Unk08 = new float[NumFloats];
            for (int i = 0; i < NumFloats; i++)
            {
                Unk08[i] = stream.ReadSingle(isBigEndian);
            }
            Unk11 = stream.ReadInt32(isBigEndian);
            Unk12 = stream.ReadInt32(isBigEndian);
            ProjectorTexture = stream.ReadString16(isBigEndian);
        }

        public void WriteToFile(MemoryStream stream, bool isBigEndian)
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
}
