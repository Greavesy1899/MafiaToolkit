using Gibbed.Mafia2.FileFormats.Archive;
using Gibbed.Mafia2.ResourceFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using Utils.Logging;
using Utils.Settings;

namespace Gibbed.Mafia2.FileFormats
{
    public partial class ArchiveFile
    {
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

                resourceXML.WriteStartElement("ResourceEntry");
                resourceXML.WriteAttributeString("SlotVram", entry.SlotVramRequired.ToString());
                resourceXML.WriteAttributeString("SlotRam", entry.SlotRamRequired.ToString());
                resourceXML.WriteElementString("Type", ResourceTypes[entry.TypeId].Name);
                string saveName = "";
                Log.WriteLine("Resource: " + i + ", name: " + itemNames[i] + ", type: " + entry.TypeId);
                switch (ResourceTypes[entry.TypeId].Name)
                {
                    case "Texture":
                        ReadTextureEntry(entry, resourceXML, itemNames[i]);
                        saveName = itemNames[i];
                        break;
                    case "hkAnimation":
                    case "Generic":
                    case "NAV_PATH_DATA":
                    case "NAV_AIWORLD_DATA":
                    case "RoadMap":
                        ConstructPath(finalPath, itemNames[i]);
                        ReadBasicEntry(resourceXML, itemNames[i]);
                        saveName = itemNames[i];
                        break;
                    case "MemFile":
                        ReadMemEntry(entry, resourceXML, itemNames[i], finalPath);
                        saveName = itemNames[i];
                        break;
                    case "SystemObjectDatabase":
                        string name = ReadXBinEntry(entry, resourceXML, itemNames[i], finalPath);
                        name = itemNames[i];
                        continue;
                    case "XML":
                        ReadXMLEntry(entry, resourceXML, itemNames[i], finalPath);
                        continue;
                    case "Flash":
                        ReadFlashEntry(entry, resourceXML, itemNames[i], finalPath);
                        saveName = itemNames[i];
                        break;
                    case "Script":
                        ReadScriptEntry(entry, resourceXML, finalPath);
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
            resourceXML.Flush();
            resourceXML.Dispose();
        }

        public bool BuildResourcesVersion20(XmlDocument document, XmlDocument xmlDoc, XmlNode rootNode, string sdsFolder)
        {
            XPathNavigator nav = document.CreateNavigator();
            var nodes = nav.Select("/SDSResource/ResourceEntry");
            Dictionary<string, List<ResourceEntry>> entries = new Dictionary<string, List<ResourceEntry>>();
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
                        resource.Parent = 0;

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

                switch (resourceType)
                {
                    case "Texture":
                        resourceEntry = WriteTextureEntry(resourceEntry, nodes, sdsFolder, sddescNode);
                        //this.SlotVramRequired += (uint)(resourceEntry.Data.Length - 128);
                        break;
                    case "hkAnimation":
                        resourceEntry = WriteBasicEntry(resourceEntry, nodes, sdsFolder, sddescNode);
                        break;
                    case "Generic":
                        resourceEntry = WriteBasicEntry(resourceEntry, nodes, sdsFolder, sddescNode);
                        resourceEntry.SlotRamRequired = SlotRamRequired;
                        resourceEntry.SlotVramRequired = SlotVramRequired;
                        this.OtherRamRequired += (uint)resourceEntry.Data.Length;
                        break;
                    case "NAV_PATH_DATA":
                    case "NAV_AIWORLD_DATA":
                    case "RoadMap":
                        resourceEntry = WriteBasicEntry(resourceEntry, nodes, sdsFolder, sddescNode);
                        resourceEntry.OtherRamRequired += (uint)(resourceEntry.Data.Length);
                        break;
                    case "MemFile":
                        resourceEntry = WriteMemFileEntry(resourceEntry, nodes, sdsFolder, sddescNode);
                        break;
                    case "SystemObjectDatabase":
                        resourceEntry = WriteXBinEntry(resourceEntry, nodes, sdsFolder, sddescNode);
                        //this.SlotRamRequired += (uint)(resourceEntry.Data.Length);
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
        private void ReadFlashEntry(ResourceEntry entry, XmlWriter resourceXML, string name, string flashDir)
        {
            string[] dirs = name.Split('/');

            string newPath = flashDir;
            for (int z = 0; z != dirs.Length - 1; z++)
            {
                newPath += "/" + dirs[z];
                Directory.CreateDirectory(newPath);
            }

            FlashResource resource = new FlashResource();
            using (var stream = new MemoryStream(entry.Data))
            {
                resource.Deserialize(entry.Version, stream, Endian);
                entry.Data = resource.Data;
            }
            newPath += "/" + dirs[dirs.Length - 1];
            resourceXML.WriteElementString("File", name);
            resourceXML.WriteElementString("Name", resource.Name);
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
            //resourceXML.WriteElementString("Entry_0", entry.SlotRamRequired.ToString());
            //resourceXML.WriteElementString("Entry_1", entry.SlotVramRequired.ToString());
            resourceXML.WriteElementString("Version", entry.Version.ToString());
            File.WriteAllBytes(xbinDir + "/" + resource.Name, resource.Data);

            if (resource.XMLData != null)
            {
                File.WriteAllBytes(xbinDir + "/" + resource.Name + ".xml", resource.XMLData);
            }

            resourceXML.WriteEndElement();
            return resource.Name;
        }
    }
}
