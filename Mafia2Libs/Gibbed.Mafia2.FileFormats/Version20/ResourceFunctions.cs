using Gibbed.Illusion.ResourceFormats;
using Gibbed.Mafia2.FileFormats.Archive;
using Gibbed.Mafia2.ResourceFormats;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using Utils.Logging;

namespace Gibbed.Mafia2.FileFormats
{
    public partial class ArchiveFile
    {
        private bool bDumpFileNames = false;
        private Dictionary<ulong, string> FileNamesAndHash;

        private string ConstructPath(string root, string file)
        {
            var dirs = file.Split('/');
            var newPath = root;
            for (int z = 0; z != dirs.Length - 1; z++)
            {
                newPath += "/" + dirs[z];
                Directory.CreateDirectory(newPath);
            }
            return newPath;
        }

        public void SaveResourcesVersion20(FileInfo file, List<string> itemNames)
        {
            FileNamesAndHash = ReadFileNameDB("Resources/GameData/M3DE_ResourceNameDatabase.txt");

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = ("\t");
            settings.OmitXmlDeclaration = true;

            string extractedPath = Path.Combine(file.Directory.FullName, "extracted");

            if (!Directory.Exists(extractedPath))
            {
                Directory.CreateDirectory(extractedPath);
            }

            string finalPath = Path.Combine(extractedPath, file.Name);
            Directory.CreateDirectory(finalPath);

            Log.WriteLine("Begin unpacking and saving files..");

            XmlWriter resourceXML = XmlWriter.Create(finalPath + "/SDSContent.xml", settings);
            resourceXML.WriteStartElement("SDSResource");

            //TODO Cleanup this code. It's awful. (V2 26/08/18, improved to use switch)
            for (int i = 0; i != ResourceEntries.Count; i++)
            {
                ResourceEntry entry = ResourceEntries[i];
                if (entry.TypeId == -1)
                {
                    MessageBox.Show(string.Format("Detected unknown type, skipping. Size: {0}", entry.Data.Length), "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    continue;
                }

                // DEBUG ONLY - Get entry name and guid to store in dictionary.
                if (Debugger.IsAttached && bDumpFileNames)
                {
                    var pair = GetFileName(entry, itemNames[i]);
                    if (pair.Key != 0 && pair.Value != "" && !pair.Value.Contains("File_"))
                    {
                        if (!FileNamesAndHash.ContainsKey(pair.Key))
                        {
                            FileNamesAndHash.Add(pair.Key, pair.Value);
                        }
                    }
                }

                string FileName = HasFilename(FileNamesAndHash, entry);
                if (!string.IsNullOrEmpty(FileName))
                {
                    //Console.WriteLine(string.Format("{0}", FileName));
                    itemNames[i] = FileName;
                }

                resourceXML.WriteStartElement("ResourceEntry");
                resourceXML.WriteAttributeString("SlotVram", entry.SlotVramRequired.ToString());
                resourceXML.WriteAttributeString("SlotRam", entry.SlotRamRequired.ToString());
                resourceXML.WriteElementString("Type", ResourceTypes[entry.TypeId].Name);
                string saveName = "";
                string NameToPass = "";
                //Log.WriteLine("Resource: " + i + ", name: " + itemNames[i] + ", type: " + entry.TypeId);
                switch (ResourceTypes[entry.TypeId].Name)
                {
                    case "Texture":
                        NameToPass = itemNames[i];
                        saveName = ReadTextureEntry(entry, resourceXML, NameToPass);
                        break;
                    case "hkAnimation":
                        NameToPass = itemNames[i];
                        saveName = ReadHavokEntry(entry, resourceXML, NameToPass);
                        ConstructPath(finalPath, saveName);
                        break;
                    case "NAV_PATH_DATA":
                    case "NAV_AIWORLD_DATA":
                    case "RoadMap":
                    case "EnlightenResource":
                        ConstructPath(finalPath, itemNames[i]);
                        ReadBasicEntry(resourceXML, itemNames[i]);
                        saveName = itemNames[i];
                        break;
                    case "Generic":
                        NameToPass = itemNames[i];
                        saveName = ReadGenericEntry(entry, resourceXML, NameToPass, finalPath);
                        continue;
                    case "MemFile":
                        NameToPass = (ResourceInfoXml == null ? "" : itemNames[i]);
                        saveName = ReadMemEntry(entry, resourceXML, NameToPass, finalPath);
                        break;
                    case "SystemObjectDatabase":
                        string name = ReadXBinEntry(entry, resourceXML, itemNames[i], finalPath);
                        name = itemNames[i];
                        continue;
                    case "XML":
                        NameToPass = (ResourceInfoXml == null ? "" : itemNames[i]);
                        name = ReadXMLEntry(entry, resourceXML, NameToPass, finalPath);
                        continue;
                    case "Flash":
                        NameToPass = (ResourceInfoXml == null ? "" : itemNames[i]);
                        saveName = ReadFlashEntry(entry, resourceXML, NameToPass, finalPath);
                        break;
                    case "Script":
                        ReadScriptEntry(entry, resourceXML, finalPath);
                        continue;
                    case "Cutscene":
                        ReadCutsceneEntry(entry, resourceXML, finalPath);
                        continue;
                    default:
                        string TypeName = ResourceTypes[entry.TypeId].Name;
                        string FormatError = string.Format("Found Unknown Type: {0}\nClicking continue will proceed with unpacking the SDS.", TypeName);
                        MessageBox.Show(FormatError, "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        saveName = string.Format("{0}_{1}.bin", TypeName, i);
                        break;
                }
                resourceXML.WriteElementString("Version", entry.Version.ToString());
                File.WriteAllBytes(finalPath + "/" + saveName, entry.Data);
                resourceXML.WriteEndElement();
            }

            resourceXML.WriteEndElement();
            resourceXML.Close();
            resourceXML.Flush();
            resourceXML.Dispose();

            if (Debugger.IsAttached && bDumpFileNames)
            {
                WriteFileNameDB("Resources/ResourceNameDatabase.txt", FileNamesAndHash);
            }
        }

        public bool BuildResourcesVersion20(XmlDocument document, XmlDocument xmlDoc, XmlNode rootNode, string sdsFolder)
        {
            XPathNavigator nav = document.CreateNavigator();
            var nodes = nav.Select("/SDSResource/ResourceEntry");
            Dictionary<string, List<ResourceEntry>> entries = new Dictionary<string, List<ResourceEntry>>();
            int index = 0;
            while (nodes.MoveNext() == true)
            {
                nodes.Current.MoveToFirstAttribute();
                uint SlotVramRequired = uint.Parse(nodes.Current.Value);
                nodes.Current.MoveToNextAttribute();
                uint SlotRamRequired = uint.Parse(nodes.Current.Value);
                nodes.Current.MoveToParent();
                nodes.Current.MoveToFirstChild();
                string resourceType = nodes.Current.Value;

                if (!entries.ContainsKey(resourceType))
                {
                    ResourceType resource = new ResourceType();
                    resource.Name = nodes.Current.Value;
                    resource.Id = (uint)entries.Count;

                    //TODO
                    if (resource.Name == "NAV_PATH_DATA")
                        resource.Parent = 1;

                    ResourceTypes.Add(resource);
                    entries.Add(resourceType, new List<ResourceEntry>());
                }
                XmlNode resourceNode = xmlDoc.CreateElement("ResourceInfo");
                XmlNode typeNameNode = xmlDoc.CreateElement("TypeName");
                typeNameNode.InnerText = resourceType;
                XmlNode sddescNode = xmlDoc.CreateElement("SourceDataDescription");

                ResourceEntry resourceEntry = new ResourceEntry() {
                    SlotRamRequired = SlotRamRequired,
                    SlotVramRequired = SlotVramRequired,
                };

                if (index == 36)
                {
                    Console.WriteLine("");
                }

                switch (resourceType)
                {
                    case "Texture":
                        resourceEntry = WriteTextureEntry(resourceEntry, nodes, sdsFolder, sddescNode);
                        break;
                    case "hkAnimation":
                        resourceEntry = WriteHavokEntry(resourceEntry, nodes, sdsFolder, sddescNode);
                        break;
                    case "Generic":
                        resourceEntry = WriteGenericEntry(resourceEntry, nodes, sdsFolder, sddescNode);
                        resourceEntry.SlotRamRequired = SlotRamRequired;
                        resourceEntry.SlotVramRequired = SlotVramRequired;
                        break;
                    case "NAV_PATH_DATA":
                    case "NAV_AIWORLD_DATA":
                    case "RoadMap":
                    case "EnlightenResource":
                        resourceEntry = WriteBasicEntry(resourceEntry, nodes, sdsFolder, sddescNode);
                        resourceEntry.OtherRamRequired += (uint)(resourceEntry.Data.Length);
                        break;
                    case "MemFile":
                        resourceEntry = WriteMemFileEntry(resourceEntry, nodes, sdsFolder, sddescNode);
                        break;
                    case "SystemObjectDatabase":
                        resourceEntry = WriteXBinEntry(resourceEntry, nodes, sdsFolder, sddescNode);
                        break;
                    case "XML":
                        resourceEntry = WriteXMLEntry(resourceEntry, nodes, sdsFolder, sddescNode);
                        break;
                    case "Script":
                        resourceEntry = WriteScriptEntry(resourceEntry, nodes, sdsFolder, sddescNode);
                        break;
                    case "Flash":
                        resourceEntry = WriteFlashEntry(resourceEntry, nodes, sdsFolder, sddescNode);
                        break;
                    case "Cutscene":
                        resourceEntry = WriteCutsceneEntry(resourceEntry, nodes, sdsFolder, sddescNode);
                        break;
                    default:
                        MessageBox.Show("Did not pack type: " + resourceType, "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                }
                resourceNode.AppendChild(typeNameNode);
                resourceNode.AppendChild(sddescNode);
                resourceNode.AppendChild(AddRamElement(xmlDoc, "SlotRamRequired", (int)resourceEntry.SlotRamRequired));
                resourceNode.AppendChild(AddRamElement(xmlDoc, "SlotVRamRequired", (int)resourceEntry.SlotVramRequired));
                resourceNode.AppendChild(AddRamElement(xmlDoc, "OtherRamRequired", (int)resourceEntry.OtherRamRequired));
                resourceNode.AppendChild(AddRamElement(xmlDoc, "OtherVramRequired", (int)resourceEntry.OtherVramRequired));
                rootNode.AppendChild(resourceNode);

                this.SlotVramRequired += resourceEntry.OtherVramRequired;
                this.SlotRamRequired += resourceEntry.OtherRamRequired;
                resourceEntry.TypeId = (int)ResourceTypes.Find(s => s.Name.Equals(resourceType)).Id;
                entries[resourceType].Add(resourceEntry);

                index++;
            }
            _ResourceTypes.Reverse();

            for (int i = 0; i < _ResourceTypes.Count; i++)
            {
                var entry = _ResourceTypes[i];
                entry.Id = (uint)i;
                _ResourceTypes[i] = entry;
            }
            foreach (var collection in entries)
            {
                var key = collection.Key;
                foreach(var entry in collection.Value)
                {
                    entry.TypeId = (int)ResourceTypes.Find(s => s.Name.Equals(key)).Id;
                }
            }

            foreach (var pair in entries)
            {
                _ResourceEntries.AddRange(pair.Value);
            }

            ResourceInfoXml = xmlDoc.OuterXml;
            return true;
        }
        private ResourceEntry WriteFlashEntry(ResourceEntry entry, XPathNodeIterator nodes, string sdsFolder, XmlNode descNode)
        {
            FlashResource resource = new FlashResource();

            //read contents from XML entry
            nodes.Current.MoveToNext();
            resource.FileName = nodes.Current.Value;
            nodes.Current.MoveToNext();
            resource.Name = nodes.Current.Value;
            nodes.Current.MoveToNext();
            entry.Version = (ushort)nodes.Current.ValueAsInt;

            resource.Data = File.ReadAllBytes(sdsFolder + "/" + resource.FileName);

            using (var stream = new MemoryStream())
            {
                resource.Serialize(entry.Version, stream, _Endian);
                entry.Data = stream.ToArray();
            }

            descNode.InnerText = resource.FileName;
            return entry;
        }
        private string ReadFlashEntry(ResourceEntry entry, XmlWriter resourceXML, string name, string flashDir)
        {
            // Read the resource first; we have the nicety of having the flash name stored 
            // in the meta info.
            FlashResource resource = new FlashResource();
            using (var stream = new MemoryStream(entry.Data))
            {
                resource.Deserialize(entry.Version, stream, Endian);
                entry.Data = resource.Data;
            }

            // Since we know that flash will have the filename, 
            // we collect it from here instead.
            if(string.IsNullOrEmpty(name))
            {
                name = resource.FileName;
            }

            string[] dirs = name.Split('/');

            string newPath = flashDir;
            for (int z = 0; z != dirs.Length - 1; z++)
            {
                newPath += "/" + dirs[z];
                Directory.CreateDirectory(newPath);
            }

            newPath += "/" + dirs[dirs.Length - 1];
            resourceXML.WriteElementString("File", name);
            resourceXML.WriteElementString("Name", resource.Name);

            // In this case this is valid; we will no doubt get a name.
            return name;
        }

        public string ReadGenericEntry(ResourceEntry entry, XmlWriter resourceXML, string name, string genericDir)
        {
            GenericResource resource = new GenericResource();

            // Read generic resource
            using(MemoryStream stream = new MemoryStream(entry.Data))
            {
                resource.Deserialize(entry.Version, stream, Endian);
                name = resource.DetermineName(name);
            }

            // Construct the path and attempt to save the data.
            ConstructPath(genericDir, name);
            File.WriteAllBytes(genericDir + "/" + name, resource.Data);

            // Write to SDSContent.
            resourceXML.WriteElementString("File", name);
            resourceXML.WriteElementString("Generic_Unk01", resource.Unk0.ToString());
            resourceXML.WriteElementString("Version", entry.Version.ToString());
            resourceXML.WriteEndElement();
            return name;
        }

        public ResourceEntry WriteGenericEntry(ResourceEntry entry, XPathNodeIterator nodes, string sdsFolder, XmlNode descNode)
        {
            // Create new resource
            GenericResource resource = new GenericResource();

            // Fetch data from XML
            nodes.Current.MoveToNext();
            resource.DebugName = nodes.Current.Value;
            nodes.Current.MoveToNext();
            resource.Unk0 = (ushort)nodes.Current.ValueAsInt;
            nodes.Current.MoveToNext();
            entry.Version = (ushort)nodes.Current.ValueAsInt;

            // Read data and serialize into the resource format.
            resource.Data = File.ReadAllBytes(sdsFolder + "/" + resource.DebugName);
            using(MemoryStream stream = new MemoryStream())
            {
                resource.Serialize(entry.Version, stream, _Endian);
                entry.Data = stream.ToArray();
            }

            // Fill out the entry and XML entry.
            entry.OtherRamRequired = 0;
            entry.OtherVramRequired = 0;

            int extensionStart = resource.DebugName.IndexOf(".");
            string filename = resource.DebugName.Remove(extensionStart);
            descNode.InnerText = filename;

            return entry;
        }

        public ResourceEntry WriteXBinEntry(ResourceEntry entry, XPathNodeIterator nodes, string sdsFolder, XmlNode descNode)
        {
            XBinResource resource = new XBinResource();

            //read contents from XML entry
            nodes.Current.MoveToNext();
            resource.Name = nodes.Current.Value;
            nodes.Current.MoveToNext();
            resource.Unk01 = Convert.ToUInt64(nodes.Current.Value);
            nodes.Current.MoveToNext();
            resource.Unk02 = Convert.ToUInt32(nodes.Current.Value);
            nodes.Current.MoveToNext();
            resource.Unk03 = Convert.ToUInt64(nodes.Current.Value);
            nodes.Current.MoveToNext();
            resource.Unk04 = nodes.Current.Value;
            nodes.Current.MoveToNext();
            resource.Hash = Convert.ToUInt64(nodes.Current.Value);
            nodes.Current.MoveToNext();
            entry.Version = Convert.ToUInt16(nodes.Current.Value);

            //finish
            resource.Data = File.ReadAllBytes(sdsFolder + "/" + resource.Name);
            resource.Size = resource.Data.Length;
            descNode.InnerText = resource.Name;
            entry.OtherRamRequired = (uint)(resource.Size);
            
            using(var stream = new MemoryStream())
            {
                resource.Serialize(entry.Version, stream, _Endian);
                entry.Data = stream.ToArray();
            }
            return entry;
        }

        private string ReadXBinEntry(ResourceEntry entry, XmlWriter resourceXML, string name, string xbinDir)
        {
            XBinResource resource = new XBinResource();
            using (var stream = new MemoryStream(entry.Data))
            {
                resource.Deserialize(entry.Version, stream, _Endian);
            }

            string[] dirs = resource.Name.Split('/');

            string newPath = xbinDir;
            for (int z = 0; z != dirs.Length - 1; z++)
            {
                newPath += "/" + dirs[z];
                Directory.CreateDirectory(newPath);
            }

            resourceXML.WriteElementString("File", resource.Name);
            resourceXML.WriteElementString("Unk01", resource.Unk01.ToString());
            resourceXML.WriteElementString("Unk02", resource.Unk02.ToString());
            resourceXML.WriteElementString("Unk03", resource.Unk03.ToString());
            resourceXML.WriteElementString("Unk04", resource.Unk04);
            resourceXML.WriteElementString("Hash", resource.Hash.ToString());
            resourceXML.WriteElementString("Version", entry.Version.ToString());
            File.WriteAllBytes(xbinDir + "/" + resource.Name, resource.Data);

            if (resource.XMLData != null)
            {
                File.WriteAllBytes(xbinDir + "/" + resource.Name + ".xml", resource.XMLData);
            }

            resourceXML.WriteEndElement();
            return resource.Name;
        }

        private ResourceEntry WriteHavokEntry(ResourceEntry entry, XPathNodeIterator nodes, string sdsFolder, XmlNode descNode)
        {
            HavokResource resource = new HavokResource();

            // Read contents from SDSContent.xml
            nodes.Current.MoveToNext();
            string name = nodes.Current.Value;
            nodes.Current.MoveToNext();
            uint unk01 = uint.Parse(nodes.Current.Value);
            nodes.Current.MoveToNext();
            ulong filehash = ulong.Parse(nodes.Current.Value);
            nodes.Current.MoveToNext();
            uint unk02 = uint.Parse(nodes.Current.Value);
            nodes.Current.MoveToNext();
            ushort version = ushort.Parse(nodes.Current.Value);

            // Serialize into separate stream
            using (MemoryStream stream = new MemoryStream())
            {
                resource.Unk01 = unk01;
                resource.FileHash = filehash;
                resource.Unk02 = unk02;
                resource.Data = File.ReadAllBytes(sdsFolder + "/" + name);
                resource.Serialize(entry.Version, stream, Endian);

                // Set entry information and this is it.
                entry.Data = stream.ToArray();
                entry.Version = version;
            }

            descNode.InnerText = name;

            return entry;
        }

        private string ReadHavokEntry(ResourceEntry entry, XmlWriter resourceXML, string name)
        {
            HavokResource resource = new HavokResource();

            using(MemoryStream stream = new MemoryStream(entry.Data))
            {
                resource.Deserialize(entry.Version, stream, _Endian);
                entry.Data = resource.Data;
            }

            if(FileNamesAndHash.ContainsKey(resource.FileHash) && name.Contains("File_"))
            {
                name = FileNamesAndHash[resource.FileHash];
            }

            resourceXML.WriteElementString("File", name);
            resourceXML.WriteElementString("Unk01", resource.Unk01.ToString());
            resourceXML.WriteElementString("FileHash", resource.FileHash.ToString());
            resourceXML.WriteElementString("Unk02", resource.Unk02.ToString());
            return name;
        }

        private ResourceEntry WriteCutsceneEntry(ResourceEntry entry, XPathNodeIterator nodes, string SDSFolder, XmlNode descNode)
        {
            // Read contents from SDSContent.xml
            nodes.Current.MoveToNext();
            int NumGCRs = nodes.Current.ValueAsInt;
            nodes.Current.MoveToNext();

            // construct new resource
            CutsceneResource Resource = new CutsceneResource();
            Resource.GCREntityRecords = new CutsceneResource.GCRResource[NumGCRs];

            for(int i = 0; i < NumGCRs; i++)
            {
                string Name = nodes.Current.Value;
                CutsceneResource.GCRResource Record = new CutsceneResource.GCRResource();

                string CombinedPath = Path.Combine(SDSFolder, Name);
                Record.Name = Name;
                Record.Content = File.ReadAllBytes(CombinedPath);

                nodes.Current.MoveToNext();

                Resource.GCREntityRecords[i] = Record;
            }

            ushort version = ushort.Parse(nodes.Current.Value);

            using (MemoryStream stream = new MemoryStream())
            {
                Resource.Serialize(entry.Version, stream, _Endian);
                entry.Data = stream.ToArray();
            }

            entry.Version = version;

            return entry;
        }

        private string ReadCutsceneEntry(ResourceEntry entry, XmlWriter writer, string SDSFolder)
        {
            CutsceneResource Resource = new CutsceneResource();

            using (MemoryStream stream = new MemoryStream(entry.Data))
            {
                Resource.Deserialize(entry.Version, stream, _Endian);
            }

            // Write all EntityRecords to individual files
            writer.WriteElementString("GCRNum", Resource.GCREntityRecords.Length.ToString());
            foreach(var Record in Resource.GCREntityRecords)
            {
                File.WriteAllBytes(Path.Combine(SDSFolder, Record.Name), Record.Content);
                writer.WriteElementString("Name", Record.Name);
            }

            writer.WriteElementString("Version", entry.Version.ToString());
            writer.WriteEndElement();

            return SDSFolder;
        }
    }
}
