using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Windows;
using ResourceTypes.Wwise;
using ResourceTypes.Wwise.Helpers;

namespace ResourceTypes.Wwise.Objects
{
    public class SoundSFX
    {
        [System.ComponentModel.Browsable(false)]
        public int Type { get; set; }
        public uint ID { get; set; }
        public uint PluginID { get; set; }
        public int PluginType { get; set; }
        public int PluginCompany { get; set; }
        public byte StreamType { get; set; } //0x00 = Data/Bnk, 0x02 = Streaming
        public uint SourceID { get; set; }
        public int InMemoryMediaSize { get; set; }
        public byte SourceBits { get; set; } //bit0 = "bIsLanguageSpecific", bit1 = "bPrefetch", bit3 = "bNonCachable", bit7 = "bHasSource"
        public int SoundInitialSize { get; set; }
        public NodeBase NodeBase { get; set; }
        public SoundSFX(HIRCObject ParentObject, BinaryReader br, int iType)
        {
            Type = iType;
            uint Length = br.ReadUInt32();
            ID = br.ReadUInt32();
            PluginType = br.ReadByte();
            PluginCompany = br.ReadByte();
            br.BaseStream.Seek((int)br.BaseStream.Position - 2, SeekOrigin.Begin);
            PluginID = br.ReadUInt32();
            StreamType = br.ReadByte();
            SourceID = br.ReadUInt32();
            InMemoryMediaSize = br.ReadInt32();
            SourceBits = br.ReadByte();

            switch(PluginID)
            {
                case 13107202:
                case 7798786:
                case 7864322:
                case 9699330:
                case 6619138:
                case 6684674:
                case 6553602:
                    SoundInitialSize = br.ReadInt32();
                    break;

                default:
                    break;
            }

            NodeBase = new NodeBase(br, ParentObject);
        }

        public SoundSFX(HIRCObject Parent)
        {
            Type = 0;
            ID = 0;
            PluginType = 0;
            PluginCompany = 0;
            PluginID = 0;
            StreamType = 0;
            SourceID = 0;
            InMemoryMediaSize = 0;
            SourceBits = 0;
            SoundInitialSize = 0;

            NodeBase = new NodeBase(Parent);
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)Type);
            bw.Write(GetLength());
            bw.Write(ID);
            bw.Write(PluginID);
            bw.Write(StreamType);
            bw.Write(SourceID);
            bw.Write(InMemoryMediaSize);
            bw.Write(SourceBits);

            switch (PluginID)
            {
                case 13107202:
                case 7798786:
                case 7864322:
                case 9699330:
                case 6619138:
                case 6684674:
                case 6553602:
                    bw.Write(SoundInitialSize);
                    break;

                default:
                    break;
            }

            NodeBase.WriteToFile(bw);
        }

        public int GetLength()
        {
            int Length = 18;

            switch (PluginID)
            {
                case 13107202:
                case 7798786:
                case 7864322:
                case 9699330:
                case 6619138:
                case 6684674:
                case 6553602:
                    Length += 4;
                    break;

                default:
                    break;
            }

            Length += NodeBase.GetLength();

            return Length;
        }
    }
}
