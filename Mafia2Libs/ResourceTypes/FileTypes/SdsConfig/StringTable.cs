using System.Collections.Generic;
using System.IO;
using Utils.Extensions;
using Utils.Helpers.Reflection;

namespace ResourceTypes.SDSConfig
{
    [PropertyClassAllowReflection]
    public class StringTable
    {
        public Dictionary<int, string> Strings { get; set; } = new();
        public Dictionary<string, int> Offsets { get; set; } = new();
        public StringTable()
        {

        }

        public StringTable(BinaryReader br)
        {
            Read(br);
        }

        public void Read(BinaryReader br)
        {
            Strings = new();

            int Length = br.ReadInt32();
            var _data = br.ReadBytes(Length);
            var Data = new byte[Length];

            for (int i = 0; i < _data.Length; i++)
            {
                Data[i] = (byte)~_data[i];
            }

            using (MemoryStream ms = new(Data))
            {
                while (ms.Position != ms.Length)
                {
                    Strings.Add((int)ms.Position, ms.ReadString());
                }
            }
        }

        public byte[] BuildFromStrings(List<string> strings)
        {
            Offsets = new();
            byte[] _data;

            using (MemoryStream ms = new())
            {
                foreach (var s in strings)
                {
                    if (!Offsets.ContainsKey(s))
                    {
                        Offsets.Add(s, (int)ms.Position);
                        ms.WriteString(s);
                    }
                }

                _data = ms.ToArray();
            }

            byte[] Data = new byte[_data.Length];

            for (int i = 0; i < _data.Length; i++)
            {
                Data[i] = (byte)~_data[i];
            }

            return Data;
        }
    }
}
