using Gibbed.Mafia2.FileFormats.Archive;
using System;
using System.Collections.Generic;
using System.IO;

namespace Gibbed.Mafia2.FileFormats
{
    // Util class for dumping file names and possible IDs
    public partial class ArchiveFile
    {
        private KeyValuePair<ulong, string> GetFileName(ResourceEntry entry, string item)
        {
            if (entry.SlotRamRequired != 0 && entry.SlotVramRequired != 0 && item != "not available")
            {
                byte[] Part1 = BitConverter.GetBytes(entry.SlotVramRequired);
                byte[] Part2 = BitConverter.GetBytes(entry.SlotRamRequired);
                byte[] Merged = new byte[8];

                Array.Copy(Part1, 0, Merged, 0, 4);
                Array.Copy(Part2, 0, Merged, 4, 4);
                ulong NameHash = BitConverter.ToUInt64(Merged, 0);

                return new KeyValuePair<ulong, string>(NameHash, item);
            }

            return new KeyValuePair<ulong, string>(0, "");
        }

        private string HasFilename(Dictionary<ulong, string> dictionaryDB, ResourceEntry entry)
        {
            if (entry.SlotRamRequired != 0 && entry.SlotVramRequired != 0)
            {
                byte[] Part1 = BitConverter.GetBytes(entry.SlotVramRequired);
                byte[] Part2 = BitConverter.GetBytes(entry.SlotRamRequired);
                byte[] Merged = new byte[8];

                Array.Copy(Part1, 0, Merged, 0, 4);
                Array.Copy(Part2, 0, Merged, 4, 4);
                ulong NameHash = BitConverter.ToUInt64(Merged, 0);

                if(dictionaryDB.ContainsKey(NameHash))
                {
                    return dictionaryDB[NameHash];
                }
            }

            return "";
        }

        private Dictionary<ulong, string> ReadFileNameDB(string database)
        {
            // Check if the file exists; if not, we send back an empty dictionary.
            if(!File.Exists(database))
            {
                return new Dictionary<ulong, string>();
            }

            string[] DatabaseLines = File.ReadAllLines(database);

            Dictionary<ulong, string> DictionaryDB = new Dictionary<ulong, string>();

            foreach(var Line in DatabaseLines)
            {
                string[] SplitLine = Line.Split(' ');
                ulong NameHash = ulong.Parse(SplitLine[0]);
                string Name = SplitLine[1];
                DictionaryDB.Add(NameHash, Name);
            }

            return DictionaryDB;
        }

        private void WriteFileNameDB(string database, Dictionary<ulong, string> dictionary)
        {
            string[] DatabaseLines = new string[dictionary.Count];

            int index = 0;
            foreach(var pair in dictionary)
            {
                string line = "";
                line += pair.Key.ToString();
                line += " ";
                line += pair.Value;

                DatabaseLines[index] = line;
                index++;
            }

            File.WriteAllLines(database, DatabaseLines);
        }

    }
}
