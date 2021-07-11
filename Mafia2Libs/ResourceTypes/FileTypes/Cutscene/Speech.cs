using System;
using System.IO;
using Utils.StringHelpers;

namespace ResourceTypes.Speech
{
    public class SpeechLoader
    {
        int fileversion = 2; //probably a reference to file version; xml is also 2;
        int fileversion2 = 2; //another one though?
        int numSpeechTypes; //num of first set of data.
        SpeechTypeData[] speechTypes;
        int numSpeechItems; //num of second set of data.
        SpeechItemData[] speechItems;

        public SpeechTypeData[] SpeechTypes {
            get { return speechTypes; }
            set { speechTypes = value; }
        }
        public SpeechItemData[] SpeechItems {
            get { return speechItems; }
            set { speechItems = value; }
        }

        public SpeechLoader(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public SpeechLoader(FileInfo info)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(info.FullName, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
        }

        public void ReadFromFile(BinaryReader reader)
        {
            if (reader.ReadInt32() != fileversion)
            {
                throw new Exception("Error: Int 1 did not equal 2.");
            }

            if (reader.ReadInt32() != fileversion2)
            {
                throw new Exception("Error: Int 2 did not equal 2.");
            }

            numSpeechTypes = reader.ReadInt32();
            speechTypes = new SpeechTypeData[numSpeechTypes];
            numSpeechItems = reader.ReadInt32();
            speechItems = new SpeechItemData[numSpeechItems];

            for (int i = 0; i != speechTypes.Length; i++)
            {
                speechTypes[i] = new SpeechTypeData(reader);
            }
            for (int i = 0; i != speechItems.Length; i++)
            {
                speechItems[i] = new SpeechItemData(reader);
            }
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(fileversion);
            writer.Write(fileversion2);
            writer.Write(numSpeechTypes);
            writer.Write(numSpeechItems);

            foreach (SpeechTypeData type in speechTypes)
            {
                type.WriteToFile(writer);
            }

            foreach (SpeechItemData item in speechItems)
            {
                item.WriteToFile(writer);
            }

        }

        public class SpeechTypeData
        {
            int unk0; //possible hash in int32.
            string entityType; //int32 for string size;
            string speechType; //int32 for string size;
            string folder; //int32 for string size;
            int unk1;
            int unk2;
            int unk3;
            int unk4;
            int unk5;

            public int Unk0 {
                get { return unk0; }
                set { unk0 = value; }
            }
            public string EntityType {
                get { return entityType; }
                set { entityType = value; }
            }
            public string SpeechType {
                get { return speechType; }
                set { speechType = value; }
            }
            public string Folder {
                get { return folder; }
                set { folder = value; }
            }
            public int Unk1 {
                get { return unk1; }
                set { unk1 = value; }
            }
            public int Unk2 {
                get { return unk2; }
                set { unk2 = value; }
            }
            public int Unk3 {
                get { return unk3; }
                set { unk3 = value; }
            }
            public int Unk4 {
                get { return unk4; }
                set { unk4 = value; }
            }
            public int Unk5 {
                get { return unk5; }
                set { unk5 = value; }
            }

            public SpeechTypeData(BinaryReader reader)
            {
                ReadFromFile(reader);
            }

            public void ReadFromFile(BinaryReader reader)
            {
                unk0 = reader.ReadInt32();
                entityType = StringHelpers.ReadString32(reader);
                speechType = StringHelpers.ReadString32(reader);
                folder = StringHelpers.ReadString32(reader);
                unk1 = reader.ReadInt32();
                unk2 = reader.ReadInt32();
                unk3 = reader.ReadInt32();
                unk4 = reader.ReadInt32();
                unk5 = reader.ReadInt32();
            }

            public void WriteToFile(BinaryWriter writer)
            {
                writer.Write(unk0);
                StringHelpers.WriteString32(writer, entityType);
                StringHelpers.WriteString32(writer, speechType);
                StringHelpers.WriteString32(writer, folder);
                writer.Write(unk1);
                writer.Write(unk2);
                writer.Write(unk3);
                writer.Write(unk4);
                writer.Write(unk5);
            }

            public override string ToString()
            {
                return string.Format("{0}, {1}, {2}. {3}", unk0, entityType, speechType, folder);
            }
        }

        public class SpeechItemData
        {
            int unk0; //another hash maybe; int32;
            long unk1; //8 bytes; possible padding or flags.
            int unk2; //1? in spvito;
            string itemName; //links to names of files;
            int unk3;
            int unk4; //0 in cloth_gossip.
            int unk5; //8000 in cloth_gossip
            int unk6; //16000 in cloth_gossip

            public int Unk0 {
                get { return unk0; }
                set { unk0 = value; }
            }
            public long Unk1 {
                get { return unk1; }
                set { unk1 = value; }
            }
            public int Unk2 {
                get { return unk2; }
                set { unk2 = value; }
            }
            public string ItemName {
                get { return itemName; }
                set { itemName = value; }
            }
            public int Unk3 {
                get { return unk3; }
                set { unk3 = value; }
            }
            public int Unk4 {
                get { return unk4; }
                set { unk4 = value; }
            }
            public int Unk5 {
                get { return unk5; }
                set { unk5 = value; }
            }
            public int Unk6 {
                get { return unk6; }
                set { unk6 = value; }
            }

            public SpeechItemData(BinaryReader reader)
            {
                ReadFromFile(reader);
            }

            public void ReadFromFile(BinaryReader reader)
            {
                unk0 = reader.ReadInt32();
                unk1 = reader.ReadInt64();
                unk2 = reader.ReadInt32();
                itemName = StringHelpers.ReadString32(reader);
                unk3 = reader.ReadInt32();
                unk4 = reader.ReadInt32();
                unk5 = reader.ReadInt32();
                unk6 = reader.ReadInt32();
            }

            public void WriteToFile(BinaryWriter writer)
            {
                writer.Write(unk0);
                writer.Write(unk1);
                writer.Write(unk2);
                StringHelpers.WriteString32(writer, itemName);
                writer.Write(unk3);
                writer.Write(unk4);
                writer.Write(unk5);
                writer.Write(unk6);
            }

            public override string ToString()
            {
                return string.Format("{0}, {1}, {2}", unk0, itemName, unk3);
            }
        }
    }
}
