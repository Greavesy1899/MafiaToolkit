using Gibbed.Mafia2.FileFormats.Archive;
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
        public void SaveResourcesVersion19(FileInfo file, List<string> itemNames)
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

            int[] counts = new int[ResourceTypes.Count];

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
                string sdsToolName = ResourceTypes[entry.TypeId].Name + "_" + counts[entry.TypeId] + ".bin";
                switch (ResourceTypes[entry.TypeId].Name)
                {
                    case "Texture":
                        ReadTextureEntry(entry, resourceXML, itemNames[i]);
                        saveName = itemNames[i];
                        break;
                    case "Mipmap":
                        ReadMipmapEntry(entry, resourceXML, itemNames[i]);
                        saveName = "MIP_" + itemNames[i];
                        break;
                    case "IndexBufferPool":
                        saveName = ReadBasicEntry(resourceXML, ToolkitSettings.UseSDSToolFormat == false ? "IndexBufferPool_" + i + ".ibp" : sdsToolName);
                        break;
                    case "VertexBufferPool":
                        saveName = ReadBasicEntry(resourceXML, ToolkitSettings.UseSDSToolFormat == false ? "VertexBufferPool_" + i + ".vbp" : sdsToolName);
                        break;
                    case "AnimalTrafficPaths":
                        saveName = ReadBasicEntry(resourceXML, ToolkitSettings.UseSDSToolFormat == false ? "AnimalTrafficPaths" + i + ".atp" : sdsToolName);
                        break;
                    case "FrameResource":
                        saveName = ReadBasicEntry(resourceXML, ToolkitSettings.UseSDSToolFormat == false ? "FrameResource_" + i + ".fr" : sdsToolName);
                        break;
                    case "Effects":
                        saveName = ReadBasicEntry(resourceXML, ToolkitSettings.UseSDSToolFormat == false ? "Effects_" + i + ".eff" : sdsToolName);
                        break;
                    case "FrameNameTable":
                        saveName = ReadBasicEntry(resourceXML, ToolkitSettings.UseSDSToolFormat == false ? "FrameNameTable_" + i + ".fnt" : sdsToolName);
                        break;
                    case "EntityDataStorage":
                        saveName = ReadBasicEntry(resourceXML, ToolkitSettings.UseSDSToolFormat == false ? "EntityDataStorage_" + i + ".eds" : sdsToolName);
                        break;
                    case "PREFAB":
                        saveName = ReadBasicEntry(resourceXML, ToolkitSettings.UseSDSToolFormat == false ? "PREFAB_" + i + ".prf" : sdsToolName);
                        break;
                    case "ItemDesc":
                        saveName = ReadBasicEntry(resourceXML, ToolkitSettings.UseSDSToolFormat == false ? "ItemDesc_" + i + ".ids" : sdsToolName);
                        break;
                    case "Actors":
                        saveName = ReadBasicEntry(resourceXML, ToolkitSettings.UseSDSToolFormat == false ? "Actors_" + i + ".act" : sdsToolName);
                        break;
                    case "Collisions":
                        saveName = ReadBasicEntry(resourceXML, ToolkitSettings.UseSDSToolFormat == false ? "Collisions_" + i + ".col" : sdsToolName);
                        break;
                    case "AudioSectors":
                        ReadAudioSectorEntry(entry, resourceXML, itemNames[i], finalPath);
                        saveName = itemNames[i];
                        break;
                    case "SoundTable":
                        saveName = ReadBasicEntry(resourceXML, ToolkitSettings.UseSDSToolFormat == false ? "SoundTable_" + i + ".stbl" : sdsToolName);
                        break;
                    case "Speech":
                        saveName = ReadBasicEntry(resourceXML, ToolkitSettings.UseSDSToolFormat == false ? "Speech_" + i + ".spe" : sdsToolName);
                        break;
                    case "FxAnimSet":
                        saveName = ReadBasicEntry(resourceXML, ToolkitSettings.UseSDSToolFormat == false ? "FxAnimSet_" + i + ".fas" : sdsToolName);
                        break;
                    case "FxActor":
                        saveName = ReadBasicEntry(resourceXML, ToolkitSettings.UseSDSToolFormat == false ? "FxActor_" + i + ".fxa" : sdsToolName);
                        break;
                    case "Cutscene":
                        saveName = ReadBasicEntry(resourceXML, ToolkitSettings.UseSDSToolFormat == false ? "Cutscene_" + i + ".cut" : sdsToolName);
                        break;
                    case "Translokator":
                        saveName = ReadBasicEntry(resourceXML, ToolkitSettings.UseSDSToolFormat == false ? "Translokator_" + i + ".tra" : sdsToolName);
                        break;
                    case "Animation2":
                        saveName = ReadBasicEntry(resourceXML, itemNames[i] + ".an2");
                        break;
                    case "NAV_AIWORLD_DATA":
                        saveName = ReadBasicEntry(resourceXML, ToolkitSettings.UseSDSToolFormat == false ? "NAV_AIWORLD_DATA_" + i + ".nav" : sdsToolName);
                        break;
                    case "NAV_OBJ_DATA":
                        saveName = ReadBasicEntry(resourceXML, ToolkitSettings.UseSDSToolFormat == false ? "NAV_OBJ_DATA_" + i + ".nov" : sdsToolName);
                        break;
                    case "NAV_HPD_DATA":
                        saveName = ReadBasicEntry(resourceXML, ToolkitSettings.UseSDSToolFormat == false ? "NAV_HPD_DATA_" + i + ".nhv" : sdsToolName);
                        break;
                    case "Script":
                        ReadScriptEntry(entry, resourceXML, finalPath);
                        continue;
                    case "XML":
                        ReadXMLEntry(entry, resourceXML, itemNames[i], finalPath);
                        continue;
                    case "Sound":
                        ReadSoundEntry(entry, resourceXML, itemNames[i], finalPath);
                        saveName = itemNames[i] + ".fsb";
                        break;
                    case "MemFile":
                        ReadMemEntry(entry, resourceXML, itemNames[i], finalPath);
                        saveName = itemNames[i];
                        break;
                    case "Table":
                        ReadTableEntry(entry, resourceXML, "", finalPath);
                        counts[ResourceTypes[entry.TypeId].Id]++;
                        resourceXML.WriteElementString("Version", entry.Version.ToString());
                        resourceXML.WriteEndElement();
                        continue;
                    case "Animated Texture":
                        saveName = ReadBasicEntry(resourceXML, itemNames[i]);
                        break;
                    default:
                        MessageBox.Show("Found unknown type: " + ResourceTypes[entry.TypeId].Name);
                        break;
                }
                counts[ResourceTypes[entry.TypeId].Id]++;
                resourceXML.WriteElementString("Version", entry.Version.ToString());
                File.WriteAllBytes(finalPath + "/" + saveName, entry.Data);
                resourceXML.WriteEndElement();
            }

            resourceXML.WriteEndElement();
            resourceXML.Flush();
            resourceXML.Dispose();
        }
    }
}
