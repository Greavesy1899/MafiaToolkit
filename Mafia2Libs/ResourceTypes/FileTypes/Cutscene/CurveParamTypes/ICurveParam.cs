using System.ComponentModel;
using System.IO;

namespace ResourceTypes.Cutscene.CurveParams
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public abstract class ICurveParam
    {

        public virtual void Read(BinaryReader br)
        {

        }

        public virtual void Write(BinaryWriter bw)
        {
            bw.Write(GetParamType());
        }

        public virtual int GetParamType()
        {
            return 0;
        }

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}
