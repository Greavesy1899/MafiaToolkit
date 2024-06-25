using System.IO;
using Utils.Extensions;
using Utils.StringHelpers;

namespace ResourceTypes.Cutscene.KeyParams
{
    public class KeyWeatherParam : IKeyType
    {
        public class WeatherParam
        {
            public int KeyFrameStart { get; set; } // Key Frame Start?
            public int KeyFrameEnd { get; set; } // Key Frame End?
            public byte Unk03 { get; set; } // Is Available?
            public string WeatherName { get; set; } // Weather Name i think

            public override string ToString()
            {
                return string.Format("{0} Start: {1} End: {2}", WeatherName, KeyFrameStart, KeyFrameEnd);
            }
        }

        public WeatherParam[] Weathers { get; set; }
        public ushort Unk05 { get; set; }

        public override void ReadFromFile(BinaryReader br)
        {
            base.ReadFromFile(br);

            int NumWeathers = br.ReadInt32();
            Weathers = new WeatherParam[NumWeathers];

            for (int i = 0; i < NumWeathers; i++)
            {
                WeatherParam param = new WeatherParam();
                param.KeyFrameStart = br.ReadInt32();
                param.KeyFrameEnd = br.ReadInt32();
                param.Unk03 = br.ReadByte();
                param.WeatherName = br.ReadString16();
                Weathers[i] = param;
            }

            Unk05 = br.ReadUInt16();
        }

        public override void WriteToFile(BinaryWriter bw)
        {
            base.WriteToFile(bw);

            bw.Write(Weathers.Length);

            foreach(var Weather in Weathers)
            {
                bw.Write(Weather.KeyFrameStart);
                bw.Write(Weather.KeyFrameEnd);
                bw.Write(Weather.Unk03);
                bw.WriteString16(Weather.WeatherName);
            }

            bw.Write(Unk05);
        }

        public override string ToString()
        {
            return string.Format("NumWeathers: {0}", Weathers.Length);
        }
    }
}
