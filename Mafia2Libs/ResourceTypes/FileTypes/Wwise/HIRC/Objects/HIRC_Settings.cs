using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using ResourceTypes.Wwise.Helpers;

namespace ResourceTypes.Wwise.Objects
{
    public class Settings
    {
        [Browsable(false)]
        public int Type { get; set; }
        public uint ID { get; set; }
        public List<Setting> SettingsList { get; set; }
        public Settings(BinaryReader br, int iType)
        {
            Type = iType;
            uint Length = br.ReadUInt32();
            ID = br.ReadUInt32();
            uint settingCount = br.ReadByte();
            SettingsList = new List<Setting>();

            for (int i = 0; i < settingCount; i++)
            {
                SettingsList.Add(new Setting(br.ReadByte()));
            }

            foreach (Setting set in SettingsList)
            {
                set.Value = br.ReadSingle();
            }
        }

        public Settings()
        {
            Type = 0;
            ID = 0;
            SettingsList = new List<Setting>();
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)Type);
            bw.Write(GetLength());
            bw.Write(ID);
            bw.Write((byte)SettingsList.Count);

            foreach (Setting set in SettingsList)
            {
                bw.Write((byte)set.ID);
            }

            foreach (Setting set in SettingsList)
            {
                bw.Write(set.Value);
            }
        }

        public int GetLength()
        {
            int Length = 5 + SettingsList.Count * 5;

            return Length;
        }
    }
}
