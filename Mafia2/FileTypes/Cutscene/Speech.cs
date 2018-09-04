using System;
using System.IO;

namespace Mafia2
{
    public class Speech
    {
        int fileversion = 2; //probably a reference to file version; xml is also 2;
        int fileversion2 = 2; //another one though?
        int numSpeechTypes; //num of first set of data.
        SpeechType[] speechTypes;
        int numSpeechItems; //num of second set of data.
        SpeechItem[] speechItems;

        public Speech(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public Speech(FileInfo info)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(info.FullName, FileMode.Open)))
            {
                Speech speech = new Speech(reader);
            }
        }

        public void ReadFromFile(BinaryReader reader)
        {
            if (reader.ReadInt32() != fileversion)
                throw new Exception("Error: Int 1 did not equal 2.");

            if (reader.ReadInt32() != fileversion2)
                throw new Exception("Error: Int 2 did not equal 2.");

            numSpeechTypes = reader.ReadInt32();
            speechTypes = new SpeechType[numSpeechTypes];
            numSpeechItems = reader.ReadInt32();
            speechItems = new SpeechItem[numSpeechItems];

            for (int i = 0; i != speechTypes.Length; i++)
                speechTypes[i] = new SpeechType(reader);
            for (int i = 0; i != speechItems.Length; i++)
                speechItems[i] = new SpeechItem(reader);
        }

        public class SpeechType
        {
            int unk0; //possible hash in int32.
            string entityType; //int32 for string size;
            string speechType; //int32 for string size;
            string folder; //int32 for string size;
            byte[] unkBytes; //20 bytes; sometimes all empty or sometimes used.

            public SpeechType(BinaryReader reader)
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

            public override string ToString()
            {
                return string.Format("{0}, {1}, {2}. {3}", unk0, entityType, speechType, folder);
            }
        }

        public class SpeechItem
        {
            int unk0; //another hash maybe; int32;
            long unk1; //8 bytes; possible padding or flags.
            int unk2; //1? in spvito;
            string itemName; //links to names of files;
            int unk3;
            byte[] unkBytes; //12 empty bytes in spvito;

            public SpeechItem(BinaryReader reader)
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

            public override string ToString()
            {
                return string.Format("{0}, {1}, {2}", unk0, itemName, unk3);
            }
        }
    }
}
