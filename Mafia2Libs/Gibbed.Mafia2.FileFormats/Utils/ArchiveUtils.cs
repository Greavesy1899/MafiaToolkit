using Gibbed.IO;
using Gibbed.Mafia2.FileFormats.Archive;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.XPath;

namespace Gibbed.Mafia2.FileFormats
{
    // Util class for dumping file names and possible IDs
    public partial class ArchiveFile
    {
        private KeyValuePair<ulong, string> GetFileName(ResourceEntry entry, string item)
        {
            if (item != "not available")
            {
                return new KeyValuePair<ulong, string>(entry.FileHash, item);
            }

            return new KeyValuePair<ulong, string>(0, "");
        }

        private string HasFilename(Dictionary<ulong, string> dictionaryDB, ResourceEntry entry)
        {
            if (dictionaryDB.ContainsKey(entry.FileHash))
            {
                return dictionaryDB[entry.FileHash];
            }

            return "";
        }

        // TODO: Only really applicable for Fusion games, need a better solution than this.
        // I was thinking a lookup dictionary, although it already exists for SDSContent.xml.
        // Ideally, I need to unify this setup.
        private string DetermineFileExtension(string Typename)
        {
            // TODO: Find a new place for this.
            string Extension = ".bin";
            if (Typename == "Texture")
            {
                Extension = ".dds";
            }
            else if (Typename == "Generic")
            {
                Extension = ".genr";
            }
            else if (Typename == "Flash")
            {
                Extension = ".fla";
            }
            else if (Typename == "hkAnimation")
            {
                Extension = ".hkx";
            }
            else if (Typename == "NAV_PATH_DATA")
            {
                Extension = ".hkt";
            }
            else if (Typename == "EnlightenResource")
            {
                Extension = ".enl";
            }
            else if (Typename == "RoadMap")
            {
                Extension = ".gsd";
            }

            return Extension;
        }

        private XPathDocument CheckForCrySDS()
        {
            int CrySDSType = -1;
            for (int i = 0; i != ResourceTypes.Count; i++)
            {
                // check if resource type has empty name
                if (ResourceTypes[i].Name == "")
                {
                    CrySDSType = (int)ResourceTypes[i].Id;
                }
            }

            // iterate entries and try to find the XML
            if (CrySDSType != -1)
            {
                for (int i = 0; i < ResourceEntries.Count; i++)
                {
                    if (ResourceEntries[i].TypeId == CrySDSType)
                    {
                        // Fix for CrySDS archives
                        using (MemoryStream stream = new MemoryStream(ResourceEntries[i].Data))
                        {
                            // Skip passwords
                            ushort authorLen = stream.ReadValueU16();
                            stream.ReadBytes(authorLen);
                            int fileSize = stream.ReadValueS32();
                            int password = stream.ReadValueS32();

                            // pull XML and create a new document
                            XPathDocument XMLDoc = null;
                            using (var reader = new StringReader(Encoding.UTF8.GetString(stream.ReadBytes(fileSize))))
                            {
                                XMLDoc = new XPathDocument(reader);
                            }

                            // Remove CrySDS lock
                            ResourceEntries.RemoveAt(i);
                            ResourceTypes.RemoveAt(CrySDSType);

                            // Return document
                            if(XMLDoc != null)
                            {
                                return XMLDoc;
                            }
                        }
                    }
                }
            }

            return null;
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
