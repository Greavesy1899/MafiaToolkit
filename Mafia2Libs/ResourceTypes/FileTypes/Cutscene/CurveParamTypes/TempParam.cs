using System.ComponentModel;
using System.IO;

namespace ResourceTypes.Cutscene.CurveParams
{
    public class TempParam : ICurveParam
    {
        public int Type { get; set; }
        [Browsable(false)]
        public byte[] Data { get; set; }
        public TempParam()
        {

        }

        public TempParam(int _Type)
        {
            Type = _Type;
        }

        public TempParam(BinaryReader br, int _Type)
        {
            Type = _Type;
            Read(br);
        }

        public override void Read(BinaryReader br)
        {
            base.Read(br);
            Data = br.ReadBytes((int)(br.BaseStream.Length - br.BaseStream.Position));
        }

        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(Data);
        }

        public override int GetParamType()
        {
            return Type;
        }

        public override string ToString()
        {
            return GetType().Name + " " + Type;
        }
    }
}
