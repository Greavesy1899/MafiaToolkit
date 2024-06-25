using System;
using System.IO;
using Utils.Helpers.Reflection;
using Utils.StringHelpers;

namespace ResourceTypes.Speech
{
    public class SpeechFile
    {
        private int fileversion = 2; //probably a reference to file version; xml is also 2;
        private int fileversion2 = 2; //another one though?

        public SpeechTypeInfo[] SpeechTypes { get; private set; }
        public SpeechItemInfo[] SpeechItems { get; private set; }

        public SpeechFile()
        {
            SpeechTypes = new SpeechTypeInfo[0];
            SpeechItems = new SpeechItemInfo[0];
        }

        public SpeechFile(FileInfo info)
        {
            SpeechTypes = new SpeechTypeInfo[0];
            SpeechItems = new SpeechItemInfo[0];

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

            uint NumSpeechTypes = reader.ReadUInt32();
            uint numSpeechItems = reader.ReadUInt32();
            SpeechTypes = new SpeechTypeInfo[NumSpeechTypes];
            SpeechItems = new SpeechItemInfo[numSpeechItems];
            for (int i = 0; i < NumSpeechTypes; i++)
            {
                SpeechTypeInfo NewType = new SpeechTypeInfo();
                NewType.ReadFromFile(reader);

                SpeechTypes[i] = NewType;
            }

            for (int i = 0; i < numSpeechItems; i++)
            {
                SpeechItemInfo NewItem = new SpeechItemInfo();
                NewItem.ReadFromFile(reader);

                SpeechItems[i] = NewItem;
            }
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(fileversion);
            writer.Write(fileversion2);
            writer.Write((int)SpeechTypes.Length);
            writer.Write((int)SpeechItems.Length);

            foreach (SpeechTypeInfo type in SpeechTypes)
            {
                type.WriteToFile(writer);
            }

            foreach (SpeechItemInfo item in SpeechItems)
            {
                item.WriteToFile(writer);
            }

        }

        [PropertyClassAllowReflection]
        public class SpeechTypeInfo
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

            public void SpeechItemInfo()
            {
                EntityType = string.Empty;
                speechType = string.Empty;
                Folder = string.Empty;
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


        [PropertyClassAllowReflection]
        public class SpeechItemInfo
        {
            int unk0; //another hash maybe; int32;
            long unk1; //8 bytes; possible padding or flags.
            int unk2; //1? in spvito;
            string itemName; //links to names of files;
            int unk3;
            byte[] unk4; // Array of bytes?
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
            public byte[] Unk4 {
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

            public SpeechItemInfo()
            {
                ItemName = string.Empty;
                unk4 = new byte[0];
            }

            public void ReadFromFile(BinaryReader reader)
            {
                unk0 = reader.ReadInt32();
                unk1 = reader.ReadInt64();
                unk2 = reader.ReadInt32();
                itemName = StringHelpers.ReadString32(reader);
                unk3 = reader.ReadInt32();

                int NumBytes = reader.ReadInt32();
                unk4 = reader.ReadBytes(NumBytes);
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
                writer.Write((int)unk4.Length);
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
