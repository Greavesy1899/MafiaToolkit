using System;
using System.IO;

namespace Mafia2
{
    public class Speech
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

        public Speech(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public Speech(FileInfo info)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(info.FullName, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
        }

        public void ReadFromFile(BinaryReader reader)
        {
            if (reader.ReadInt32() != fileversion)
                throw new Exception("Error: Int 1 did not equal 2.");

            if (reader.ReadInt32() != fileversion2)
                throw new Exception("Error: Int 2 did not equal 2.");

            numSpeechTypes = reader.ReadInt32();
            speechTypes = new SpeechTypeData[numSpeechTypes];
            numSpeechItems = reader.ReadInt32();
            speechItems = new SpeechItemData[numSpeechItems];

            for (int i = 0; i != speechTypes.Length; i++)
                speechTypes[i] = new SpeechTypeData(reader);
            for (int i = 0; i != speechItems.Length; i++)
                speechItems[i] = new SpeechItemData(reader);
        }

        public class SpeechTypeData
        {
            int unk0; //possible hash in int32.
            string entityType; //int32 for string size;
            string speechType; //int32 for string size;
            string folder; //int32 for string size;
            byte[] unkBytes; //20 bytes; sometimes all empty or sometimes used.

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
            public byte[] UnkBytes {
                get { return unkBytes; }
                set { unkBytes = value; }
            }

            public SpeechTypeData(BinaryReader reader)
            {
                ReadFromFile(reader);
            }

            public void ReadFromFile(BinaryReader reader)
            {
                unk0 = reader.ReadInt32();
                entityType = Functions.ReadString32(reader);
                speechType = Functions.ReadString32(reader);
                folder = Functions.ReadString32(reader);
                unkBytes = reader.ReadBytes(20);
            }

            public void WriteToFile(BinaryWriter writer)
            {
                writer.Write(unk0);
                Functions.WriteStirng32(writer, entityType);
                Functions.WriteStirng32(writer, speechType);
                Functions.WriteStirng32(writer, folder);
                writer.Write(unkBytes);
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
            byte[] unkBytes; //12 empty bytes in spvito;

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
            public byte[] UnkBytes {
                get { return unkBytes; }
                set { unkBytes = value; }
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
                itemName = Functions.ReadString32(reader);
                unk3 = reader.ReadInt32();
                unkBytes = reader.ReadBytes(12);
            }

            public void WriteToFile(BinaryWriter writer)
            {
                writer.Write(unk0);
                writer.Write(unk1);
                writer.Write(unk2);
                Functions.WriteStirng32(writer, itemName);
                writer.Write(unk3);
                writer.Write(unkBytes);
            }

            public override string ToString()
            {
                return string.Format("{0}, {1}, {2}", unk0, itemName, unk3);
            }
        }
    }
}
