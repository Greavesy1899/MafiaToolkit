using System.ComponentModel;
using System.IO;

namespace ResourceTypes.Wwise.Helpers
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SwitchParam
    {
        public uint ID { get; set; }
        public byte ParamBitVector { get; set; }
        public byte ParamBitVector2 { get; set; }
        public float FadeOutTime { get; set; }
        public float FadeInTime { get; set; }
        public SwitchParam(BinaryReader br)
        {
            ID = br.ReadUInt32();
            ParamBitVector = br.ReadByte();
            ParamBitVector2 = br.ReadByte();
            FadeOutTime = br.ReadSingle();
            FadeInTime = br.ReadSingle();
        }

        public SwitchParam()
        {
            ID = 0;
            ParamBitVector = 0;
            ParamBitVector2 = 0;
            FadeOutTime = 0;
            FadeInTime = 0;
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write(ID);
            bw.Write(ParamBitVector);
            bw.Write(ParamBitVector2);
            bw.Write(FadeOutTime);
            bw.Write(FadeInTime);
        }
    }
}
