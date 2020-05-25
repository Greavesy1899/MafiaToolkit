using Gibbed.Mafia2.FileFormats.Archive;
using Gibbed.Mafia2.ResourceFormats;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;
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
                Directory.CreateDirectory(extractedPath);

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
                    case "MemFile":
                    case "RoadMap":
                        ConstructPath(finalPath, itemNames[i]);
                        ReadBasicEntry(resourceXML, itemNames[i]);
                        saveName = itemNames[i];
                        break;
                    case "SystemObjectDatabase":
                        saveName = string.Format("SystemObjectDatabase_{0}.bin", i);
                        break;
                    case "XML":
                        ReadXMLEntry(entry, resourceXML, itemNames[i], finalPath);
                        continue;
                    case "Flash":
                        ReadFlashEntry(entry, resourceXML, itemNames[i], finalPath);
                        saveName = itemNames[i];
                        break;
                    default:
                        MessageBox.Show("Found unknown type: " + ResourceTypes[entry.TypeId].Name);
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
            using(var stream = new MemoryStream(entry.Data))
            {
                resource.Deserialize(entry.Version, stream, Endian);
            }

            newPath += "/" + dirs[dirs.Length - 1];
            resourceXML.WriteElementString("File", name);
            resourceXML.WriteElementString("Name", resource.Name);
        }
    }
}
