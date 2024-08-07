using System.IO;
using Utils.Extensions;
using Utils.Helpers.Reflection;

namespace ResourceTypes.Tyres
{
    [PropertyClassAllowReflection]
    public class TyreCondition
    {
        public string Name { get; set; }
        public TyreSettings[] Settings { get; set; } = new TyreSettings[0];
        public TyreCondition()
        {

        }

        public TyreCondition(MemoryStream stream)
        {
            Read(stream);
        }

        public void Read(MemoryStream stream)
        {
            Name = stream.ReadStringBuffer(8, true);
            int Count = stream.ReadInt32(false);
            Settings = new TyreSettings[Count];

            for (int i = 0; i < Settings.Length; i++)
            {
                Settings[i] = new(stream);
            }
        }

        public void Write(MemoryStream stream, bool isBigEndian)
        {
            stream.WriteStringBuffer(8, Name);
            stream.Write(Settings.Length, isBigEndian);

            foreach (var setting in Settings)
            {
                setting.Write(stream, isBigEndian);
            }
        }
    }
}
