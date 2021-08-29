using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.Collections.Generic;

namespace ResourceTypes.Wwise.Helpers
{
    public class MusicMarker
    {
        public uint id { get; set; }
        public double position { get; set; }
        public string markerName { get; set; }
        public MusicMarker(BinaryReader br)
        {
            id = br.ReadUInt32();
            position = br.ReadDouble();
            uint stringSize = br.ReadUInt32();
            byte[] stringBytes = br.ReadBytes((int)stringSize);
            markerName = Encoding.UTF8.GetString(stringBytes);
        }

        public MusicMarker()
        {
            id = 0;
            position = 0;
            markerName = "";
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write(id);
            bw.Write(position);
            bw.Write(markerName.Length);
            bw.Write(markerName.ToCharArray());
        }

        public int GetLength()
        {
            int length = 16 + markerName.Length;

            return length;
        }
    }
}
