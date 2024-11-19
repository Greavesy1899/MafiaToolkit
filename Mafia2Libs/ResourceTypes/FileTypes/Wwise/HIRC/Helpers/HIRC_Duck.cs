using System.ComponentModel;
using System.IO;

namespace ResourceTypes.Wwise.Helpers
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Duck
    {
        public uint BusID { get; set; }
        public float Volume { get; set; }
        public int FadeOutTime { get; set; }
        public int FadeInTime { get; set; }
        public int FadeCurve { get; set; }
        public int TargetProp { get; set; }

        public Duck(BinaryReader br)
        {
            BusID = br.ReadUInt32();
            Volume = br.ReadSingle();
            FadeOutTime = br.ReadInt32();
            FadeInTime = br.ReadInt32();
            FadeCurve = br.ReadByte();
            TargetProp = br.ReadByte();
        }

        public Duck()
        {
            BusID = 0;
            Volume = 0;
            FadeOutTime = 0;
            FadeInTime = 0;
            FadeCurve = 0;
            TargetProp = 0;
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write(BusID);
            bw.Write(Volume);
            bw.Write(FadeOutTime);
            bw.Write(FadeInTime);
            bw.Write((byte)FadeCurve);
            bw.Write((byte)TargetProp);
        }
    }
}
