using System;
using System.Collections.Generic;
using System.IO;
using Utils.StringHelpers;

namespace ResourceTypes.Misc
{
    public class CGameData
    {
        private FileInfo info;

        private const int Magic = 0x676D7072;
        private const int version = 6;

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
            this.info = info;
            using (BinaryReader reader = new BinaryReader(File.Open(info.FullName, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
            using (BinaryWriter writer = new BinaryWriter(File.Open(info.FullName+"2", FileMode.Create)))
            {
                WriteToFile(writer);
            }
        }

        public void ReadFromFile(BinaryReader reader)
        {
            //see C_Game::ParseData for source code.
            if (reader.ReadInt32() != Magic)
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

            DumpToText();
        }

        public void WriteToFile(BinaryWriter writer)
        {
            //see C_Game::ParseData for source code.
            writer.Write(Magic);
            writer.Write(version);

            writer.Write(57345);

            int size = 0;
            foreach (string slot in slots)
                size += (slot.Length + 1);

            writer.Write(size);

            foreach (string slot in slots)
                StringHelpers.WriteString(writer, slot);

            writer.Write(57346);
            writer.Write(4);
            writer.Write(unk0);

            writer.Write(57347);
            writer.Write(1 + (unkSkyString.Length+1) + 8);
            writer.Write(useSkyString);
            StringHelpers.WriteString(writer, unkSkyString);
            writer.Write(unkSky1);
            writer.Write(unkSky2);

            writer.Write(57348);
            writer.Write(2);
            writer.Write(unk1);

            writer.Write(57349);
            writer.Write((name.Length+1) + (actorBin.Length+1));
            StringHelpers.WriteString(writer, name);
            StringHelpers.WriteString(writer, actorBin);

            writer.Write(65195);
            writer.Write(0);
        }

        private void DumpToText()
        {
            List<string> file = new List<string>();
            file.Add("CGame Data: " + info.Name);
            file.AddRange(slots);
            file.Add("");
            file.Add("Unk0: " + unk0);
            file.Add("");
            file.Add("Use Sky? " + useSkyString);
            file.Add("Sky String: " + unkSkyString);
            file.Add("Sky Unk0: " + unkSky1);
            file.Add("Sky Unk1: " + unkSky2);
            file.Add("");
            file.Add("Unk1: " + unk1);
            file.Add("");
            file.Add("Name: " + name);
            file.Add("ActorBin: " + actorBin);
            File.WriteAllLines(info.Name + ".txt", file.ToArray());
        }
    }
}
