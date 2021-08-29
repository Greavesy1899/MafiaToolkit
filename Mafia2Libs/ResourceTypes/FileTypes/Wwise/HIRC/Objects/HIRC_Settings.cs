using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using ResourceTypes.Wwise.Helpers;

namespace ResourceTypes.Wwise.Objects
{
    public class Settings
    {
        [System.ComponentModel.Browsable(false)]
        public int type { get; set; }
        public uint id { get; set; }
        public List<Setting> settings { get; set; }
        public Settings(BinaryReader br, int iType)
        {
            type = iType;
            uint length = br.ReadUInt32();
            id = br.ReadUInt32();
            uint settingCount = br.ReadByte();
            settings = new List<Setting>();

            for (int i = 0; i < settingCount; i++)
            {
                settings.Add(new Setting(br.ReadByte()));
            }

            foreach (Setting set in settings)
            {
                set.value = br.ReadSingle();
            }
        }

        public Settings()
        {
            type = 0;
            id = 0;
            settings = new List<Setting>();
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)type);
            bw.Write(GetLength());
            bw.Write(id);
            bw.Write((byte)settings.Count);

            foreach (Setting set in settings)
            {
                bw.Write((byte)set.id);
            }

            foreach (Setting set in settings)
            {
                bw.Write(set.value);
            }
        }

        public int GetLength()
        {
            int length = 5 + settings.Count * 5;

            return length;
        }
    }
}
