using System.IO;
using Utils.Extensions;

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

        public int NumWeathers { get; set; }
        public WeatherParam[] Weathers { get; set; }
        public ushort Unk05 { get; set; }

        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);

            NumWeathers = stream.ReadInt32(isBigEndian);
            Weathers = new WeatherParam[NumWeathers];

            for (int i = 0; i < NumWeathers; i++)
            {
                WeatherParam param = new WeatherParam();
                param.KeyFrameStart = stream.ReadInt32(isBigEndian);
                param.KeyFrameEnd = stream.ReadInt32(isBigEndian);
                param.Unk03 = stream.ReadByte8();
                param.WeatherName = stream.ReadString16(isBigEndian);
                Weathers[i] = param;
            }

            Unk05 = stream.ReadUInt16(isBigEndian);
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);

            stream.Write(Weathers.Length, isBigEndian);

            foreach(var Weather in Weathers)
            {
                stream.Write(Weather.KeyFrameStart, isBigEndian);
                stream.Write(Weather.KeyFrameEnd, isBigEndian);
                stream.WriteByte(Weather.Unk03);
                stream.WriteString16(Weather.WeatherName, isBigEndian);
            }

            stream.Write(Unk05, isBigEndian);
        }

        public override string ToString()
        {
            return string.Format("NumWeathers: {0}", Weathers.Length);
        }
    }
}
