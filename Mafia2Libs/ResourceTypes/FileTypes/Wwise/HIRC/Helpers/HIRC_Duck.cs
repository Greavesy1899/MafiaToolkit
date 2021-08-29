using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace ResourceTypes.Wwise.Helpers
{
    public class Duck
    {
        public uint busId { get; set; }
        public float duckVolume { get; set; }
        public int fadeOutTime { get; set; }
        public int fadeInTime { get; set; }
        public int fadeCurve { get; set; }
        public int targetProp { get; set; }

        public Duck(BinaryReader br)
        {
            busId = br.ReadUInt32();
            duckVolume = br.ReadSingle();
            fadeOutTime = br.ReadInt32();
            fadeInTime = br.ReadInt32();
            fadeCurve = br.ReadByte();
            targetProp = br.ReadByte();
        }

        public Duck()
        {
            busId = 0;
            duckVolume = 0;
            fadeOutTime = 0;
            fadeInTime = 0;
            fadeCurve = 0;
            targetProp = 0;
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write(busId);
            bw.Write(duckVolume);
            bw.Write(fadeOutTime);
            bw.Write(fadeInTime);
            bw.Write((byte)fadeCurve);
            bw.Write((byte)targetProp);
        }
    }
}
