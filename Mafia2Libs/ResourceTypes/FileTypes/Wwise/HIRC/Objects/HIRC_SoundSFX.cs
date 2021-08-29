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
        public int type { get; set; }
        public uint id { get; set; }
        public uint pluginId { get; set; }
        public int pluginType { get; set; }
        public int pluginCompany { get; set; }
        public byte streamType { get; set; } //0x00 = Data/Bnk, 0x02 = Streaming
        public uint sourceId { get; set; }
        public int inMemoryMediaSize { get; set; }
        public byte sourceBits { get; set; } //bit0 = "bIsLanguageSpecific", bit1 = "bPrefetch", bit3 = "bNonCachable", bit7 = "bHasSource"
        public int soundInitialSize { get; set; }
        public NodeBase nodeBase { get; set; }
        public SoundSFX(HIRCObject parentObject, BinaryReader br, int iType)
        {
            type = iType;
            uint length = br.ReadUInt32();
            id = br.ReadUInt32();
            pluginType = br.ReadByte();
            pluginCompany = br.ReadByte();
            br.BaseStream.Seek((int)br.BaseStream.Position - 2, SeekOrigin.Begin);
            pluginId = br.ReadUInt32();
            streamType = br.ReadByte();
            sourceId = br.ReadUInt32();
            inMemoryMediaSize = br.ReadInt32();
            sourceBits = br.ReadByte();

            switch(pluginId)
            {
                case 13107202:
                case 7798786:
                case 7864322:
                case 9699330:
                case 6619138:
                case 6684674:
                case 6553602:
                    soundInitialSize = br.ReadInt32();
                    break;

                default:
                    break;
            }

            nodeBase = new NodeBase(br, parentObject);
        }

        public SoundSFX(HIRCObject parent)
        {
            type = 0;
            id = 0;
            pluginType = 0;
            pluginCompany = 0;
            pluginId = 0;
            streamType = 0;
            sourceId = 0;
            inMemoryMediaSize = 0;
            sourceBits = 0;
            soundInitialSize = 0;

            nodeBase = new NodeBase(parent);
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)type);
            bw.Write(GetLength());
            bw.Write(id);
            bw.Write(pluginId);
            bw.Write(streamType);
            bw.Write(sourceId);
            bw.Write(inMemoryMediaSize);
            bw.Write(sourceBits);

            switch (pluginId)
            {
                case 13107202:
                case 7798786:
                case 7864322:
                case 9699330:
                case 6619138:
                case 6684674:
                case 6553602:
                    bw.Write(soundInitialSize);
                    break;

                default:
                    break;
            }

            nodeBase.WriteToFile(bw);
        }

        public int GetLength()
        {
            int length = 18;

            switch (pluginId)
            {
                case 13107202:
                case 7798786:
                case 7864322:
                case 9699330:
                case 6619138:
                case 6684674:
                case 6553602:
                    length += 4;
                    break;

                default:
                    break;
            }

            length += nodeBase.GetLength();

            return length;
        }
    }
}
