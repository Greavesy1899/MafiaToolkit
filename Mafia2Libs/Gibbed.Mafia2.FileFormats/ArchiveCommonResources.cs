using Gibbed.Illusion.FileFormats.Hashing;
using Gibbed.Mafia2.FileFormats.Archive;
using Gibbed.Mafia2.ResourceFormats;
using Gibbed.IO;
using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using Utils.Settings;
using Utils.Lua;

namespace Gibbed.Mafia2.FileFormats
{
    public partial class ArchiveFile
    {
        public string ReadTextureEntry(ResourceEntry entry, XmlWriter resourceXML, string name)
        {
            TextureResource resource = new TextureResource();

            using(MemoryStream stream = new MemoryStream(entry.Data))
            {
                resource.Deserialize(entry.Version, stream, Endian);
            }

            string FetchedName = "";
            if (_TextureNames.ContainsKey(resource.NameHash))
            {
                FetchedName = _TextureNames[resource.NameHash];

                if (!string.IsNullOrEmpty(FetchedName))
                {
                    name = FetchedName;
                }
            }

            resourceXML.WriteElementString("File", name);

            // We lack the file hash in M3 and M1: DE. So we have to add it to the file.
            if(IsGameType(GamesEnumerator.MafiaI_DE) || IsGameType(GamesEnumerator.MafiaIII))
            {
                resourceXML.WriteElementString("FileHash", resource.NameHash.ToString());
            }

            resourceXML.WriteElementString("HasMIP", resource.HasMIP.ToString());
            entry.Data = resource.Data;
            return name;
        }
        public ResourceEntry WriteTextureEntry(ResourceEntry entry, XPathNodeIterator nodes, string sdsFolder, XmlNode descNode)
        {
            // We lack the file hash in M3 and M1: DE. So we have to get it from the file.
            bool bIsMaf3 = false;
            if (IsGameType(GamesEnumerator.MafiaI_DE) || IsGameType(GamesEnumerator.MafiaIII))
            {
                bIsMaf3 = true;
            }

            //read from xml.
            nodes.Current.MoveToNext();
            string file = nodes.Current.Value;
            nodes.Current.MoveToNext();

            ulong hash = 0;
            if (bIsMaf3)
            {
                string hashString = nodes.Current.ToString();
                hash = Convert.ToUInt64(hashString);
                nodes.Current.MoveToNext();
            }
            byte hasMIP = Convert.ToByte(nodes.Current.Value);
            nodes.Current.MoveToNext();

            // Setup ResourceEntry MetaInfo.
            entry.Version = Convert.ToUInt16(nodes.Current.Value);
            descNode.InnerText = file;

            // Begin serialising the Texture Resource.
            TextureResource resource = new TextureResource();

            // Create stream.
            using (MemoryStream stream = new MemoryStream())
            {
                // Read the Texture file (.DDS).
                byte[] texData = File.ReadAllBytes(sdsFolder + "/" + file);
                resource = new TextureResource(FNV64.Hash(file), hasMIP, texData);

                // Do Version specific handling;
                if (bIsMaf3)
                {
                    resource.NameHash = hash;
                }

                // Serialise to the TextureResource and pack it into the ResourceEntry.
                resource.Serialize(entry.Version, stream, _Endian);
                entry.Data = stream.ToArray();

                // Configure VRAM information for the SDS.
                if (IsGameType(GamesEnumerator.MafiaII_DE) || IsGameType(GamesEnumerator.MafiaII))
                {
                    entry.SlotVramRequired = (uint)(texData.Length - 128);
                    //if (hasMIP == 1)
                    //{
                    //    using (BinaryReader reader = new BinaryReader(File.Open(sdsFolder + "/MIP_" + file, FileMode.Open)))
                    //        entry.SlotVramRequired += (uint)(reader.BaseStream.Length - 128);
                    //}
                }

                if (IsGameType(GamesEnumerator.MafiaI_DE))
                {
                    var size = (resource.bIsDX10 ? 157 : 137);
                    entry.SlotVramRequired = (uint)(stream.Length - size);
                }
            }
            return entry;
        }
        public ResourceEntry ReadMipmapEntry(ResourceEntry entry, XmlWriter resourceXML, string name)
        {
            TextureResource resource = new TextureResource();
            resource.DeserializeMIP(entry.Version, new MemoryStream(entry.Data), Endian);
            resourceXML.WriteElementString("File", "MIP_" + name);
            entry.Data = resource.Data;
            return entry;
        }
        public ResourceEntry WriteMipmapEntry(ResourceEntry entry, XPathNodeIterator nodes, string sdsFolder, XmlNode descNode)
        {
            //texture data storage.
            MemoryStream data = new MemoryStream();
            TextureResource resource;
            byte[] texData;

            //get xml stuff
            nodes.Current.MoveToNext();
            string file = nodes.Current.Value;
            nodes.Current.MoveToNext();
            entry.Version = Convert.ToUInt16(nodes.Current.Value);
            texData = File.ReadAllBytes(sdsFolder + "/" + file);
            resource = new TextureResource(FNV64.Hash(file.Remove(0, 4)), 0, texData);
            resource.SerializeMIP(entry.Version, data, Endian.Little);

            //finish.
            descNode.InnerText = file.Remove(0, 4);
            entry.Data = data.ToArray();
            return entry;
        }
        public string ReadBasicEntry(XmlWriter resourceXML, string name)
        {
            resourceXML.WriteElementString("File", name);
            return name;
        }
        public ResourceEntry WriteBasicEntry(ResourceEntry entry, XPathNodeIterator nodes, string sdsFolder, XmlNode descNode)
        {
            //get data from xml
            nodes.Current.MoveToNext();
            string file = nodes.Current.Value;
            nodes.Current.MoveToNext();
            entry.Version = Convert.ToUInt16(nodes.Current.Value);

            //finish
            entry.Data = File.ReadAllBytes(sdsFolder + "/" + file);
            entry.SlotRamRequired = (uint)entry.Data.Length;
            descNode.InnerText = file;
            return entry;
        }
        public ResourceEntry WriteAnimatedTextureEntry(ResourceEntry entry, XPathNodeIterator nodes, string sdsFolder, XmlNode descNode)
        {
            //get data from xml
            nodes.Current.MoveToNext();
            string file = nodes.Current.Value;
            nodes.Current.MoveToNext();
            entry.Version = Convert.ToUInt16(nodes.Current.Value);

            //finish
            entry.Data = File.ReadAllBytes(sdsFolder + "/" + file);
            descNode.InnerText = file;
            return entry;
        }
        public ResourceEntry WriteCollisionEntry(ResourceEntry entry, XPathNodeIterator nodes, string sdsFolder, XmlNode descNode)
        {
            //get data from xml
            nodes.Current.MoveToNext();
            string file = nodes.Current.Value;
            nodes.Current.MoveToNext();
            entry.Version = Convert.ToUInt16(nodes.Current.Value);
            entry.Data = File.ReadAllBytes(sdsFolder + "/" + file);

            //finish
            entry.SlotRamRequired = (uint)entry.Data.Length + 1;
            descNode.InnerText = "not available";
            return entry;
        }
        public void ReadScriptEntry(ResourceEntry entry, XmlWriter resourceXML, string scriptDir)
        {
            ScriptResource resource = new ScriptResource();
            resource.Deserialize(entry.Version, new MemoryStream(entry.Data), _Endian);
            resourceXML.WriteElementString("File", resource.Path);
            resourceXML.WriteElementString("ScriptNum", resource.Scripts.Count.ToString());

            for (int x = 0; x != resource.Scripts.Count; x++)
            {
                // Get the script resource.
                ScriptData ScriptItem = resource.Scripts[x];

                // Get directory and Script name.
                string ScriptDirectory = Path.GetDirectoryName(ScriptItem.Name);
                string ScriptName = Path.GetFileName(ScriptItem.Name);

                // Create the new directory.
                string NewDirectory = scriptDir + ScriptDirectory;
                Directory.CreateDirectory(NewDirectory);

                // Write the script data to the designated file.
                string ScriptPath = Path.Combine(NewDirectory, ScriptName);
                File.WriteAllBytes(ScriptPath, ScriptItem.Data);

                // If user requests, decompile the Lua file.
                if (ToolkitSettings.DecompileLUA)
                {
                    FileInfo Info = new FileInfo(ScriptPath);
                    LuaHelper.ReadFile(Info);
                }

                resourceXML.WriteElementString("Name", ScriptItem.Name);
            }
            resourceXML.WriteElementString("Version", entry.Version.ToString());
            resourceXML.WriteEndElement(); // We finish early with scripts, as this has an alternate layout.
        }
        public ResourceEntry WriteScriptEntry(ResourceEntry entry, XPathNodeIterator nodes, string sdsFolder, XmlNode descNode)
        {
            // Get data from Xml.
            nodes.Current.MoveToNext();
            string path = nodes.Current.Value;
            nodes.Current.MoveToNext();
            int numScripts = Convert.ToInt32(nodes.Current.Value);

            // Create the new resource, add path.
            ScriptResource resource = new ScriptResource();
            resource.Path = path;

            // Iterate through scripts, reading each one and pushing them into the list.
            for (int i = 0; i < numScripts; i++)
            {
                ScriptData data = new ScriptData();
                nodes.Current.MoveToNext();
                data.Name = nodes.Current.Value;
                data.Data = File.ReadAllBytes(sdsFolder + data.Name);
                resource.Scripts.Add(data);
            }

            // Finish reading the Xml by getting the version.
            nodes.Current.MoveToNext();
            ushort version = Convert.ToUInt16(nodes.Current.Value);

            // Create the stream and serialize the resource package into said stream.
            using(MemoryStream stream = new MemoryStream())
            {
                resource.Serialize(version, stream, Endian.Little);
                entry.Data = stream.ToArray();
                entry.OtherRamRequired = (uint)resource.GetRawBytes();
            }

            // Set the entry version and setup the data for the meta info.
            entry.Version = version;        
            descNode.InnerText = path;
            return entry;
        }
        public string ReadXMLEntry(ResourceEntry entry, XmlWriter resourceXML, string name, string xmlDir)
        {
            XmlResource resource = new XmlResource();

            using (MemoryStream stream = new MemoryStream(entry.Data))
            {
                // Unpack our XML Resource.
                resource = new XmlResource();
                resource.Deserialize(entry.Version, stream, Endian);
                name = resource.Name;

                // Create the directories.
                string[] dirs = name.Split(new char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
                string xmldir = xmlDir;
                for (int z = 0; z != dirs.Length - 1; z++)
                {

                    xmldir = Path.Combine(xmldir, dirs[z]);
                    Directory.CreateDirectory(xmldir);
                }

                // Set the filename we want to save the file too.
                string FileName = Path.Combine(xmldir, Path.GetFileName(name) + ".xml");

                if (resource.bFailedToDecompile)
                {
                    byte[] data = stream.ReadBytes((int)(stream.Length - stream.Position));
                    File.WriteAllBytes(FileName, data);
                }
                else
                {
                    // 08/08/2020. Originally was File.WriteAllText, but caused problems with some XML documents.
                    using (StreamWriter writer = new StreamWriter(File.Open(FileName, FileMode.Create)))
                    {
                        writer.WriteLine(resource.Content);
                    }
                }
            }

            resourceXML.WriteElementString("File", name);
            resourceXML.WriteElementString("XMLTag", resource.Tag);
            resourceXML.WriteElementString("Unk1", Convert.ToByte(resource.Unk1).ToString());
            resourceXML.WriteElementString("Unk3", Convert.ToByte(resource.Unk3).ToString());
            resourceXML.WriteElementString("FailedToDecompile", Convert.ToByte(resource.bFailedToDecompile).ToString());
            resourceXML.WriteElementString("Version", entry.Version.ToString());
            resourceXML.WriteEndElement(); //finish early.
            return name;
        }
        public ResourceEntry WriteXMLEntry(ResourceEntry entry, XPathNodeIterator nodes, string sdsFolder, XmlNode descNode)
        {
            nodes.Current.MoveToNext();
            string file = nodes.Current.Value;
            descNode.InnerText = file;

            nodes.Current.MoveToNext();
            string tag = nodes.Current.Value;

            nodes.Current.MoveToNext();
            bool unk1 = nodes.Current.ValueAsBoolean;

            nodes.Current.MoveToNext();
            bool unk3 = nodes.Current.ValueAsBoolean;

            nodes.Current.MoveToNext();
            bool failedToDecompile = nodes.Current.ValueAsBoolean;

            //need to do version early.
            nodes.Current.MoveToNext();
            entry.Version = Convert.ToUInt16(nodes.Current.Value);

            MemoryStream stream = new MemoryStream();

            XmlResource resource = new XmlResource
            {
                Name = file,
                Content = sdsFolder + "/" + file + ".xml",
                Tag = tag,
                Unk1 = unk1,
                Unk3 = unk3,
                bFailedToDecompile = failedToDecompile
            };

            resource.Serialize(entry.Version, stream, Endian.Little);

            if (resource.Unk3)
            {
                entry.Data = stream.ToArray();
            }
            else
            {
                entry.Data = stream.ToArray();
            }

            return entry;
        }
        public void ReadAudioSectorEntry(ResourceEntry entry, XmlWriter resourceXML, string name, string soundDir)
        {
            string[] dirs = name.Split('/');

            string sounddir = soundDir;
            for (int z = 0; z != dirs.Length - 1; z++)
            {
                sounddir += "/" + dirs[z];
                Directory.CreateDirectory(sounddir);
            }
            sounddir += "/" + dirs[dirs.Length - 1];
            File.WriteAllBytes(sounddir, entry.Data);
            resourceXML.WriteElementString("File", name);
        }
        public ResourceEntry WriteAudioSectorEntry(ResourceEntry entry, XPathNodeIterator nodes, string sdsFolder, XmlNode descNode)
        {
            string file;
            nodes.Current.MoveToNext();
            file = nodes.Current.Value;
            entry.Data = File.ReadAllBytes(sdsFolder + "/" + file);
            nodes.Current.MoveToNext();
            entry.Version = Convert.ToUInt16(nodes.Current.Value);
            descNode.InnerText = file;
            return entry;
        }
        public void ReadSoundEntry(ResourceEntry entry, XmlWriter resourceXML, string name, string soundDir)
        {
            // Create and deserialize the data.
            SoundResource resource = new SoundResource();

            using (MemoryStream stream = new MemoryStream(entry.Data))
            {
                resource.Deserialize(entry.Version, stream, _Endian);
            }

            entry.Data = resource.Data;

            // Create directories and then write the XML to finish it off.
            string fileName = name + ".fsb";
            string[] dirs = name.Split(new char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);

            string tempDir = soundDir;
            for (int z = 0; z < dirs.Length - 1; z++)
            {
                tempDir = Path.Combine(tempDir, dirs[z]);
                Directory.CreateDirectory(tempDir);
            }

            resourceXML.WriteElementString("File", fileName);
        }
        public ResourceEntry WriteSoundEntry(ResourceEntry entry, XPathNodeIterator nodes, string sdsFolder, XmlNode descNode)
        {
            nodes.Current.MoveToNext();
            string file = nodes.Current.Value.Remove(nodes.Current.Value.Length - 4, 4);

            // Combine path and add extension.
            string path = Path.Combine(sdsFolder, file);
            path += ".fsb";

            // Get the Version and set the inner text (meta XML).
            nodes.Current.MoveToNext();
            entry.Version = Convert.ToUInt16(nodes.Current.Value);
            descNode.InnerText = file;

            using (MemoryStream stream = new MemoryStream())
            {
                byte[] fileData = File.ReadAllBytes(path);
                SoundResource resource = new SoundResource
                {
                    Name = file,
                    Data = fileData,
                    FileSize = fileData.Length
                };

                resource.Serialize(entry.Version, stream, _Endian);

                // Fill the remaining data for the entry.
                entry.SlotRamRequired = 40;
                entry.SlotVramRequired = (uint)resource.FileSize;
                entry.Data = stream.ToArray();
            }
            return entry;
        }
        public string ReadMemEntry(ResourceEntry entry, XmlWriter resourceXML, string name, string memDIR)
        {
            MemFileResource resource = new MemFileResource();
            using (var stream = new MemoryStream(entry.Data))
            {
                resource.Deserialize(entry.Version, stream, _Endian);
                entry.Data = resource.Data;
            }

            if (string.IsNullOrEmpty(name))
            {
                name = resource.Name;
            }

            string[] dirs = name.Split('/');

            string memdir = memDIR;
            for (int z = 0; z != dirs.Length - 1; z++)
            {
                memdir += "/" + dirs[z];
                Directory.CreateDirectory(memdir);
            }
            resourceXML.WriteElementString("File", name);
            resourceXML.WriteElementString("Unk2_V4", resource.Unk2_V4.ToString());
            return name;
        }
        public ResourceEntry WriteMemFileEntry(ResourceEntry entry, XPathNodeIterator nodes, string sdsFolder, XmlNode descNode)
        {
            //get file name from XML.
            nodes.Current.MoveToNext();
            string file = nodes.Current.Value;
            nodes.Current.MoveToNext();
            uint unk2 = Convert.ToUInt32(nodes.Current.Value);
            nodes.Current.MoveToNext();
            entry.Version = Convert.ToUInt16(nodes.Current.Value);

            //construct MemResource.
            MemFileResource resource = new MemFileResource
            {
                Name = file,
                Unk1 = 1,
                Unk2_V4 = unk2
            };

            // Read all the data, then allocate memory required
            resource.Data = File.ReadAllBytes(sdsFolder + "/" + file);
            entry.SlotRamRequired = (uint)resource.Data.Length;

            //serialize.
            using (MemoryStream stream = new MemoryStream())
            {
                resource.Serialize(entry.Version, stream, Endian.Little);
                entry.Data = stream.ToArray();
            }

            descNode.InnerText = file;
            return entry;
        }
        public ResourceEntry ReadTableEntry(ResourceEntry entry, XmlWriter resourceXML, string name, string tableDIR)
        {
            TableResource resource = new TableResource();
            resource.Deserialize(entry.Version, new MemoryStream(entry.Data), Endian);
            if (!Directory.Exists(tableDIR + "/tables"))
            {
                Directory.CreateDirectory(tableDIR + "/tables");
            }

            resourceXML.WriteElementString("NumTables", resource.Tables.Count.ToString());

            foreach (TableData data in resource.Tables)
            {
                //maybe we can get away with saving to version 1, and then converting to version 2 when packing?
                using (MemoryStream stream = new MemoryStream())
                {
                    data.Serialize(1, stream, Endian.Little);
                    File.WriteAllBytes(tableDIR + data.Name, stream.ToArray());
                }

                resourceXML.WriteElementString("Table", data.Name);
            }

            return entry;
        }
        public ResourceEntry WriteTableEntry(ResourceEntry entry, XPathNodeIterator nodes, string sdsFolder, XmlNode descNode)
        {
            TableResource resource = new TableResource();

            //number of tables
            nodes.Current.MoveToNext();
            int count = nodes.Current.ValueAsInt;

            //read tables and add to resource.
            for (int i = 0; i != count; i++)
            {
                //goto next and read file name.
                nodes.Current.MoveToNext();
                string file = nodes.Current.Value;

                //create file data.
                TableData data = new TableData();

                //now read..
                using (BinaryReader reader = new BinaryReader(File.Open(sdsFolder + file, FileMode.Open)))
                {
                    data.Deserialize(1, reader.BaseStream, Endian);
                    data.Name = file;
                    data.NameHash = FNV64.Hash(data.Name);
                }

                resource.Tables.Add(data);
            }

            //get version, always 1 Mafia II (2010) is 1, Mafia: DE (2020) is 2.
            nodes.Current.MoveToNext();
            entry.Version = Convert.ToUInt16(nodes.Current.Value);

            //create a temporary memory stream, merge all data and then fill entry data.
            using (MemoryStream stream = new MemoryStream())
            {
                resource.Serialize(entry.Version, stream, Endian.Little);
                entry.Data = stream.ToArray();
                entry.SlotRamRequired = (uint)entry.Data.Length + 128;
            }

            //fin.
            return entry;
        }
        public ResourceEntry WriteBufferEntry(ResourceEntry entry, XPathNodeIterator nodes, string sdsFolder, XmlNode descNode)
        {
            //get data from xml:
            nodes.Current.MoveToNext();
            string file = nodes.Current.Value;
            nodes.Current.MoveToNext();
            entry.Version = Convert.ToUInt16(nodes.Current.Value);

            //load buffers.
            entry.Data = File.ReadAllBytes(sdsFolder + "/" + file);
            entry.SlotVramRequired = BitConverter.ToUInt32(entry.Data, 5);

            //finish
            descNode.InnerText = "not available";
            return entry;
        }
        public ResourceEntry WriteEntityDataEntry(ResourceEntry entry, XPathNodeIterator nodes, string sdsFolder, XmlNode descNode)
        {
            //get data from XML
            nodes.Current.MoveToNext();
            string file = nodes.Current.Value;
            nodes.Current.MoveToNext();
            entry.Version = Convert.ToUInt16(nodes.Current.Value);
            entry.Data = File.ReadAllBytes(sdsFolder + "/" + file);
            entry.SlotRamRequired = (uint)(entry.Data.Length);

            //finish
            descNode.InnerText = "not available";
            return entry;
        }
        public ResourceEntry WriteAnimationEntry(ResourceEntry entry, XPathNodeIterator nodes, string sdsFolder, XmlNode descNode)
        {
            //get data from xml:
            nodes.Current.MoveToNext();
            string file = nodes.Current.Value;
            nodes.Current.MoveToNext();
            entry.Version = Convert.ToUInt16(nodes.Current.Value);
            entry.Data = File.ReadAllBytes(sdsFolder + "/" + file);
            //finish
            descNode.InnerText = file.Remove(file.Length - 4, 4);
            return entry;
        }
    }
}
