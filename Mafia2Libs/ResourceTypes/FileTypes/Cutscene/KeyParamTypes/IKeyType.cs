using System.ComponentModel;
using System.IO;

namespace ResourceTypes.Cutscene.KeyParams
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public abstract class IKeyType
    {
        [Browsable(true)]
        public int Size { get; set; }
        [Browsable(false)]
        public int KeyType { get; set; }

        public virtual void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {

        }

        public virtual void WriteToFile(MemoryStream stream, bool isBigEndian)
        {

        }
    }
}
