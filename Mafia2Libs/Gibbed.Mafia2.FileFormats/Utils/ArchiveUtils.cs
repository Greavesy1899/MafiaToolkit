using Gibbed.IO;
using Gibbed.Mafia2.FileFormats.Archive;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.XPath;
using Utils.Settings;

namespace Gibbed.Mafia2.FileFormats
{
    // Util class for dumping file names and possible IDs
    public partial class ArchiveFile
    {
        // TODO: Make one for fusion games (M3 and M1DE)
        // For M2 and M2DE.
        private Dictionary<string, string> FileExtensionLookup = new Dictionary<string, string>
        {
            { "Texture", ".dds" },
            { "Mipmap", ".dds" },
            { "IndexBufferPool", ".ibp" },
            { "VertexBufferPool", ".vbp" },
            { "AnimalTrafficPaths", ".atp" },
            { "FrameResource", ".fr" },
            { "Effects", ".eff" },
            { "FrameNameTable", ".fnt" },
            { "EntityDataStorage", ".eds" },
            { "PREFAB", ".prf" },
            { "ItemDesc", ".ids" },
            { "Actors", ".act" },
            { "Collisions", ".col" },
            { "SoundTable", ".stbl" },
            { "Speech", ".spe" },
            { "FxAnimSet", ".fas" },
            { "FxActor", ".fxa" },
            { "Cutscene", ".cut" },
            { "Translokator", ".tra" },
            { "Animation2", ".an2" },
            { "NAV_AIWORLD_DATA", ".nav" },
            { "NAV_OBJ_DATA", ".nov" },
            { "NAV_HPD_DATA", ".nhv" },
            { "AudioSectors", ".auds" },
            { "Script", ".luapack" },
            { "Table", ".tblpack" },
            { "Sound", ".fsb" },
            { "MemFile", ".txt" },
            { "XML", ".xml" },
            { "Animated Texture", ".ifl" },
        };

        private Dictionary<string, string> FileExtensionLookupFusion = new Dictionary<string, string>
        {
            { "Texture", ".dds" },
            { "Generic", ".genr" },
            { "Flash", ".fla" },
            { "hkAnimation", ".hkx" },
            { "NAV_PATH_DATA", ".hkt" },
            { "EnlightenResource", ".enl" },
            { "RoadMap", ".gsd" },
            { "NAV_AIWORLD_DATA", ".nav" },
            { "MemFile", ".txt" },
            { "XML", ".xml" },
            { "Script", ".luapack" },
            { "SystemObjectDatabase", ".xbin" },
            { "Cutscene", ".gcs" }
        };

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

            if (ChosenGameType == GamesEnumerator.MafiaII || ChosenGameType == GamesEnumerator.MafiaII_DE)
            {
                Extension = FileExtensionLookup[Typename];
                return Extension;
            }
            else
            {
                Extension = FileExtensionLookupFusion[Typename];
                return Extension;
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
                            if (XMLDoc != null)
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
            if (!File.Exists(database))
            {
                return new Dictionary<ulong, string>();
            }

            string[] DatabaseLines = File.ReadAllLines(database);

            Dictionary<ulong, string> DictionaryDB = new Dictionary<ulong, string>();

            foreach (var Line in DatabaseLines)
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
            foreach (var pair in dictionary)
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
