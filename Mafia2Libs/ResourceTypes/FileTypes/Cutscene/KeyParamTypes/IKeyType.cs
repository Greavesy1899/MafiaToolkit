using System.ComponentModel;
using System.IO;

namespace ResourceTypes.Cutscene.KeyParams
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public abstract class IKeyType
    {
        [Browsable(false)]
        public int KeyType { get; set; }

        public virtual void ReadFromFile(BinaryReader br)
        {

        }

        public virtual void WriteToFile(BinaryWriter bw)
        {
            bw.Write(KeyType);
        }
    }
}
