using System;
using System.Collections.Generic;
using System.IO;
using Utils.StringHelpers;

namespace ResourceTypes.Misc
{
    public class CGameData
    {
        private readonly int magic = 0x676D7072;
        private readonly int version = 6;

        private string[] slots;
        private string name;
        private string actorBin;
        private byte useSkyString;
        private string unkSkyString;
        private float unkSky1;
        private float unkSky2;
        private int unk0;
        private short unk1;


        public CGameData(FileInfo info)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(info.FullName, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
        }

        public void ReadFromFile(BinaryReader reader)
        {
            //see C_Game::ParseData for source code.
            if (reader.ReadInt32() != magic)
                return;

            if (reader.ReadInt32() != version)
                return;

            if (reader.ReadInt32() == 57345)
            {
                int poolSize = reader.ReadInt32();
                int pos = 0;
                List<string> slots = new List<string>();

                while (pos != poolSize)
                {
                    string slot = StringHelpers.ReadString(reader);
                    slots.Add(slot);
                    pos += (slot.Length + 1);
                }
                this.slots = slots.ToArray();
            }
            if(reader.ReadInt32() == 57346)
            {
                int size = reader.ReadInt32();
                unk0 = reader.ReadInt32();
            }
            if(reader.ReadInt32() == 57347)
            {
                int size = reader.ReadInt32();
                useSkyString = reader.ReadByte();
                unkSkyString = StringHelpers.ReadString(reader);
                unkSky1 = reader.ReadSingle();
                unkSky2 = reader.ReadSingle();
            }
            if(reader.ReadInt32() == 57348)
            {
                int size = reader.ReadInt32();
                unk1 = reader.ReadInt16();
            }
            if(reader.ReadInt32() == 57349)
            {
                int size = reader.ReadInt32();
                name = StringHelpers.ReadString(reader);
                actorBin = StringHelpers.ReadString(reader);
            }

            if (reader.ReadInt32() == 65195)
                Console.WriteLine("EOF reached!");

        }
    }
}
