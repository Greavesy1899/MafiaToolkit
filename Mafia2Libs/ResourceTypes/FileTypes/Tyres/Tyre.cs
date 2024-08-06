using System.IO;
using Utils.Extensions;
using Utils.Helpers.Reflection;

namespace ResourceTypes.Tyres
{
    [PropertyClassAllowReflection]
    public class Tyre
    {
        public string Name { get; set; }
        public TyreCondition[] Conditions { get; set; } = new TyreCondition[0];
        public Tyre()
        {

        }

        public Tyre(MemoryStream stream)
        {
            Read(stream);
        }

        public void Read(MemoryStream stream)
        {
            Name = stream.ReadStringBuffer(4, true);
            int Count = stream.ReadInt32(false);
            Conditions = new TyreCondition[Count];

            for (int i = 0; i < Conditions.Length; i++)
            {
                Conditions[i] = new(stream);
            }
        }

        public void Write(MemoryStream stream, bool isBigEndian)
        {
            stream.WriteStringBuffer(4, Name);
            stream.Write(Conditions.Length, isBigEndian);

            foreach (var condition in Conditions)
            {
                condition.Write(stream, isBigEndian);
            }
        }
    }
}
