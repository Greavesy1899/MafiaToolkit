using System.ComponentModel;
using System.IO;

namespace ResourceTypes.Wwise.Helpers
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class FeedbackSource
    {
        public uint CompanyID { get; set; }
        public uint DeviceID { get; set; }
        public float VolumeOffset { get; set; }
        public uint PluginID { get; set; }
        public byte StreamType { get; set; }
        public uint SourceID { get; set; }
        public int InMemoryMediaSize { get; set; }
        public byte SourceBits { get; set; }
        public int Size { get; set; }
        public FeedbackSource(BinaryReader br)
        {
            CompanyID = br.ReadUInt16();
            DeviceID = br.ReadUInt16();
            VolumeOffset = br.ReadSingle();
            PluginID = br.ReadUInt32();
            StreamType = br.ReadByte();
            SourceID = br.ReadUInt32();
            InMemoryMediaSize = br.ReadInt32();
            SourceBits = br.ReadByte();
            Size = br.ReadInt32();
        }

        public FeedbackSource()
        {
            CompanyID = 0;
            DeviceID = 0;
            VolumeOffset = 0;
            PluginID = 0;
            StreamType = 0;
            SourceID = 0;
            InMemoryMediaSize = 0;
            SourceBits = 0;
            Size = 0;
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((short)CompanyID);
            bw.Write((short)DeviceID);
            bw.Write(VolumeOffset);
            bw.Write(PluginID);
            bw.Write(StreamType);
            bw.Write(SourceID);
            bw.Write(InMemoryMediaSize);
            bw.Write(SourceBits);
            bw.Write(Size);
        }
    }
}
