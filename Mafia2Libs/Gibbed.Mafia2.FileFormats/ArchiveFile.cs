/* Copyright (c) 2017 Rick (rick 'at' gibbed 'dot' us)
 * 
 * This software is provided 'as-is', without any express or implied
 * warranty. In no event will the authors be held liable for any damages
 * arising from the use of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would
 *    be appreciated but is not required.
 * 
 * 2. Altered source versions must be plainly marked as such, and must not
 *    be misrepresented as being the original software.
 * 
 * 3. This notice may not be removed or altered from any source
 *    distribution.
 */

using Gibbed.Illusion.FileFormats;
using Gibbed.Illusion.FileFormats.Hashing;
using Gibbed.Mafia2.FileFormats.Archive;
using Gibbed.IO;
using Gibbed.Mafia2.ResourceFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using Utils.Language;
using Utils.Logging;
using Utils.Settings;

namespace Gibbed.Mafia2.FileFormats
{
    public partial class ArchiveFile
    {
        public const uint Signature = 0x53445300; // 'SDS\0'
        #region Fields
        private Endian _Endian;
        private uint _Version;
        private Archive.Platform _Platform;
        private uint _SlotRamRequired;
        private uint _SlotVramRequired;
        private uint _OtherRamRequired;
        private uint _OtherVramRequired;
        private byte[] _Unknown20;
        private List<Archive.ResourceType> _ResourceTypes;
        private string _ResourceInfoXml;
        private readonly List<Archive.ResourceEntry> _ResourceEntries;
        private readonly List<string> _ResourceNames;
        private Dictionary<ulong, string> _TextureNames;
        private Game ChosenGame;
        private GamesEnumerator ChosenGameType;
        private bool bHasSetGameType;
        #endregion
        #region Properties
        public Endian Endian {
            get { return this._Endian; }
            set { this._Endian = value; }
        }
        public uint Version {
            get { return this._Version; }
            set { this._Version = value; }
        }
        public Archive.Platform Platform {
            get { return this._Platform; }
            set { this._Platform = value; }
        }
        public uint SlotRamRequired {
            get { return this._SlotRamRequired; }
            set { this._SlotRamRequired = value; }
        }
        public uint SlotVramRequired {
            get { return this._SlotVramRequired; }
            set { this._SlotVramRequired = value; }
        }
        public uint OtherRamRequired {
            get { return this._OtherRamRequired; }
            set { this._OtherRamRequired = value; }
        }
        public uint OtherVramRequired {
            get { return this._OtherVramRequired; }
            set { this._OtherVramRequired = value; }
        }
        public byte[] Unknown20 {
            get { return this._Unknown20; }
            set { this._Unknown20 = value; }
        }
        public List<Archive.ResourceType> ResourceTypes {
            get { return this._ResourceTypes; }
        }
        public string ResourceInfoXml {
            get { return this._ResourceInfoXml; }
            set { this._ResourceInfoXml = value; }
        }
        public List<Archive.ResourceEntry> ResourceEntries {
            get { return this._ResourceEntries; }
        }
        public List<string> ResourceNames {
            get { return this._ResourceNames; }
        }
        #endregion
        #region Constructors
        public ArchiveFile()
        {
            this._ResourceTypes = new List<Archive.ResourceType>();
            this._ResourceEntries = new List<Archive.ResourceEntry>();
            this._ResourceNames = new List<string>();
            Unknown20 = new byte[16];

            // Get Chosen Game and Type.
            // Sanity check the GameStorage and
            // pull from it if possible.
            if (GameStorage.Instance != null)
            {
                ChosenGame = GameStorage.Instance.GetSelectedGame();

                if (ChosenGame != null)
                {
                    SetGameType(ChosenGame.GameType);
                }
            }
        }
        #endregion
        #region Functions
        public void Serialize(Stream output, ArchiveSerializeOptions options)
        {
            var compress = (options & ArchiveSerializeOptions.Compress) > 0;

            var basePosition = output.Position;
            var endian = this._Endian;

            using (var data = new MemoryStream(12))
            {
                data.WriteValueU32(Signature, Endian.Big);
                data.WriteValueU32(Version, endian);
                data.WriteValueU32((uint)this._Platform, Endian.Big);
                data.Flush();
                output.WriteFromMemoryStreamSafe(data, endian);
            }

            var headerPosition = output.Position;

            Archive.FileHeader fileHeader;
            output.Seek(56, SeekOrigin.Current);

            fileHeader.ResourceTypeTableOffset = (uint)(output.Position - basePosition);
            output.WriteValueS32(this._ResourceTypes.Count, endian);
            foreach (var resourceType in this._ResourceTypes)
            {
                resourceType.Write(output, endian);
            }
            uint stride = (uint)(Version == 20 ? 38 : 30);
            uint alignment = (uint)(Version == 20 ? 0x10000 : 0x4000);
            var blockAlignment = (options & ArchiveSerializeOptions.OneBlock) != ArchiveSerializeOptions.None ? (uint)this._ResourceEntries.Sum(re => stride + (re.Data == null ? 0 : re.Data.Length)) : alignment;
            fileHeader.BlockTableOffset = (uint)(output.Position - basePosition);
            fileHeader.ResourceCount = 0;
            var blockStream = BlockWriterStream.ToStream(output, blockAlignment, endian, compress, ChosenGameType == GamesEnumerator.MafiaI_DE);           
            foreach (var resourceEntry in this._ResourceEntries)
            {
                Archive.ResourceHeader resourceHeader;
                resourceHeader.TypeId = (uint)resourceEntry.TypeId;
                resourceHeader.Size = stride + (uint)(resourceEntry.Data == null ? 0 : resourceEntry.Data.Length);
                resourceHeader.Version = resourceEntry.Version;
                resourceHeader.SlotRamRequired = resourceEntry.SlotRamRequired;
                resourceHeader.SlotVramRequired = resourceEntry.SlotVramRequired;
                resourceHeader.OtherRamRequired = resourceEntry.OtherRamRequired;
                resourceHeader.OtherVramRequired = resourceEntry.OtherVramRequired;
                resourceHeader.Unk01 = 0;
                resourceHeader.Unk02 = 0;
                resourceHeader.Unk03 = 0;

                using (var data = new MemoryStream())
                {
                    resourceHeader.Write(data, endian, Version);
                    data.Flush();
                    blockStream.WriteFromMemoryStreamSafe(data, endian);
                }

                blockStream.WriteBytes(resourceEntry.Data);
                fileHeader.ResourceCount++;
            }

            blockStream.Flush();
            blockStream.Finish();

            fileHeader.XmlOffset = (uint)(output.Position - basePosition);
            if (string.IsNullOrEmpty(this._ResourceInfoXml) == false)
            {
                output.WriteString(this._ResourceInfoXml, Encoding.ASCII);
            }

            fileHeader.SlotRamRequired = this.SlotRamRequired;
            fileHeader.SlotVramRequired = this.SlotVramRequired;
            fileHeader.OtherRamRequired = this.OtherRamRequired;
            fileHeader.OtherVramRequired = this.OtherVramRequired;
            fileHeader.Flags = 1;
            fileHeader.Unknown20 = this._Unknown20 ?? new byte[16];

            output.Position = headerPosition;
            using (var data = new MemoryStream())
            {
                fileHeader.Write(data, endian);
                data.Flush();
                output.WriteFromMemoryStreamSafe(data, endian);
            }
        }
        public void Deserialize(Stream input)
        {
            // Read Texture Names before we start.
            // They are from an external file, taken from MTL.
            ReadTextureNames();

            if (IsGameType(GamesEnumerator.MafiaI_DE))
            {
                if (!File.Exists("libs/oo2core_8_win64.dll"))
                {
                    MessageBox.Show(Language.GetString("$M1DE_OODLEERROR"), "Toolkit");
                    return;
                }
            }

            var basePosition = input.Position;

            // Check Magic, should be SDS.
            var magic = input.ReadValueU32(Endian.Big);
            if (magic != Signature)
            {
                string FormatError = string.Format("Unsupported Archive Signature: {0}", magic);
                throw new FormatException(FormatError);
            }

            input.Seek(8, SeekOrigin.Begin);
            // Check Platform. There may be values for XboxOne and PS4, but that is unknown.
            var platform = (Platform)input.ReadValueU32(Endian.Big);
            if (platform != Platform.PC && platform != Platform.Xbox360 && platform != Platform.PS3)
            {
                string FormatError = string.Format("Unsupported Archive Platform: {0}", platform);
                throw new FormatException(FormatError);
            }

            var endian = platform == Archive.Platform.PC ? Endian.Little : Endian.Big;

            input.Seek(4, SeekOrigin.Begin);
            // Check Version, should be 19 (Mafia: II) or 20 (Mafia III).
            var version = input.ReadValueU32(endian);
            if (version != 19 && version != 20)
            {
                string FormatError = string.Format("Unsupported Archive Version: {0}", version);
                throw new FormatException(FormatError);
            }

            input.Seek(12, SeekOrigin.Begin);
            input.Position = basePosition;

            using (var data = input.ReadToMemoryStreamSafe(12, endian))
            {
                data.Position += 4; // skip magic
                _Version = data.ReadValueU32(endian);
                data.Position += 4; // skip platform
            }

            if (_Version != 19 && _Version != 20)
            {
                throw new FormatException("unsupported archive version");
            }

            Archive.FileHeader fileHeader;
            using (var data = input.ReadToMemoryStreamSafe(52, endian))
            {
                fileHeader = Archive.FileHeader.Read(data, endian);
            }

            input.Position = basePosition + fileHeader.ResourceTypeTableOffset;
            var resourceTypeCount = input.ReadValueU32(endian);
            var resourceTypes = new Archive.ResourceType[resourceTypeCount];
            for (uint i = 0; i < resourceTypeCount; i++)
            {
                resourceTypes[i] = Archive.ResourceType.Read(input, endian);
            }

            input.Position = basePosition + fileHeader.BlockTableOffset;
            var blockStream = BlockReaderStream.FromStream(input, endian);

            var resources = new Archive.ResourceEntry[fileHeader.ResourceCount];

            for (uint i = 0; i < fileHeader.ResourceCount; i++)
            {
                Archive.ResourceHeader resourceHeader;
                var size = (_Version == 20 ? 34 : 26);
                using (var data = blockStream.ReadToMemoryStreamSafe(size, endian))
                {
                    resourceHeader = Archive.ResourceHeader.Read(data, endian, _Version);

                }

                if (resourceHeader.Size < 30)
                {
                    throw new FormatException();
                }

                resources[i] = new Archive.ResourceEntry()
                {
                    TypeId = (int)resourceHeader.TypeId,
                    Version = resourceHeader.Version,
                    Data = blockStream.ReadBytes((int)resourceHeader.Size - (size + 4)),
                    SlotRamRequired = resourceHeader.SlotRamRequired,
                    SlotVramRequired = resourceHeader.SlotVramRequired,
                    OtherRamRequired = resourceHeader.OtherRamRequired,
                    OtherVramRequired = resourceHeader.OtherVramRequired,
                };
            }
            if (fileHeader.XmlOffset != 0)
            {
                input.Position = basePosition + fileHeader.XmlOffset;
                var xml = input.ReadString((int)(input.Length - input.Position), Encoding.ASCII);
                this._ResourceInfoXml = xml;
            }

            this._ResourceTypes.Clear();
            this._ResourceEntries.Clear();

            this._Endian = endian;
            this._Platform = platform;
            this._SlotRamRequired = fileHeader.SlotRamRequired;
            this._SlotVramRequired = fileHeader.SlotVramRequired;
            this._OtherRamRequired = fileHeader.OtherRamRequired;
            this._OtherVramRequired = fileHeader.OtherVramRequired;
            this._Unknown20 = (byte[])fileHeader.Unknown20.Clone();
            this._ResourceTypes.AddRange(resourceTypes);
            this._ResourceEntries.AddRange(resources);
        }

        public bool BuildResources(string folder)
        {
            string sdsFolder = folder;
            XmlDocument document = null;

            // Open a FileStream which contains the SDSContent data.
            string SDSContentPath = Path.Combine(sdsFolder, "SDSContent.xml");
            using (FileStream XMLStream = new FileStream(SDSContentPath, FileMode.Open))
            {
                try
                {
                    document = new XmlDocument();
                    document.Load(XMLStream);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Error while parsing SDSContent.XML. \n{0}", ex.Message));
                    return false;
                }
            }

            // Check if document is valid. If it is, then we know it has been found and loaded without problems.
            if(document == null)
            {
                MessageBox.Show(string.Format("Failed to open SDSContent.XML. \n{0}", SDSContentPath));
                return false;
            }

            // GoAhead and begin creating the document to save inside the SDSContent.
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement("xml");
            xmlDoc.AppendChild(rootNode);

            // Try and pack the resources found in SDSContent.
            bool bResult = false;

            if(_Version == 19)
            {
                bResult = BuildResourcesVersion19(document, xmlDoc, rootNode, sdsFolder);
            }
            else if(_Version == 20)
            {
                bResult = BuildResourcesVersion20(document, xmlDoc, rootNode, sdsFolder);
            }

            document = null;
            return bResult;
        }

        // Util function to set game type.
        // This is used for functionality where a selected game does not exist.
        public void SetGameType(GamesEnumerator SelectedGameType)
        {
            ChosenGameType = SelectedGameType;
            bHasSetGameType = true;
        }

        private bool IsGameType(GamesEnumerator TypeToCheck)
        {
            if(bHasSetGameType)
            {
                return ChosenGameType.Equals(TypeToCheck);
            }

            // TODO: Throw assert?
            return false;
        }

        public void ExtractPatch(FileInfo file)
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

            PatchFile patchFile = null;

            using (var input = File.OpenRead(file.FullName))
            {
                using (Stream data = ArchiveEncryption.Unwrap(input))
                {
                    patchFile = new PatchFile();
                    patchFile.file = new FileInfo(file.FullName + ".patch");
                    patchFile.Deserialize(data ?? input, Endian.Little);
                }
            }
            Dictionary<string, Dictionary<int, string>> sortedResources = new Dictionary<string, Dictionary<int, string>>();
            Dictionary<string, List<KeyValuePair<int, bool>>> resPatchAvailable = new Dictionary<string, List<KeyValuePair<int, bool>>>();

            for (int i = 0; i < ResourceTypes.Count; i++)
            {
                sortedResources.Add(ResourceTypes[i].Name, new Dictionary<int, string>());
                resPatchAvailable.Add(ResourceTypes[i].Name, new List<KeyValuePair<int, bool>>());
            }

            for (int i = 0; i < ResourceEntries.Count; i++)
            {
                var type = ResourceTypes[_ResourceEntries[i].TypeId].Name;
                var name = (type == "Mipmap" ? ResourceNames[i].Remove(0, 4) : ResourceNames[i]);

                if (sortedResources.ContainsKey(type))
                {
                    sortedResources[type].Add(i, name);

                    if (patchFile.UnkInts1.Contains(i))
                    {
                        resPatchAvailable[type].Add(new KeyValuePair<int, bool>(i, false));
                    }
                }
            }

            for (int i = 0; i < patchFile.resources.Length; i++)
            {
                var entry = patchFile.resources[i];

                if (entry.TypeId > ResourceTypes.Count)
                {
                    File.WriteAllBytes("Unk" + i + ".bin", entry.Data);
                    continue;
                }

                var type = ResourceTypes[entry.TypeId].Name;
                string name = string.Format("{0}_{1}", type, i);
                for (int z = 0; z < resPatchAvailable[type].Count; z++)
                {
                    var res = resPatchAvailable[type][z];
                    if (type == "Texture" || type == "Mipmap")
                    {
                        TextureResource tRes = new TextureResource();
                        tRes.Deserialize(entry.Version, new MemoryStream(entry.Data), Endian.Little);
                        var resName = sortedResources[type][res.Key];
                        var hash = FNV64.Hash(resName);
                        if (tRes.NameHash == hash)
                        {
                            Console.WriteLine("Detected possible candidate: {0}", resName);
                            name = resName;
                            break;
                        }
                    }
                    else
                    {
                        if (!res.Value)
                        {
                            name = sortedResources[type][res.Key];
                            resPatchAvailable[type][z] = new KeyValuePair<int, bool>(res.Key, true);
                            break;
                        }
                    }

                }
                var saveName = "";
                resourceXML.WriteStartElement("ResourceEntry");
                resourceXML.WriteElementString("Type", ResourceTypes[entry.TypeId].Name);
                switch (type)
                {
                    case "Texture":
                        var textureName = name + ".dds";
                        ReadTextureEntry(entry, resourceXML, name);
                        saveName = textureName;
                        break;
                    case "Mipmap":
                        var mipName = "MIP_" + name + ".dds";
                        ReadMipmapEntry(entry, resourceXML, name);
                        saveName = mipName;
                        break;
                    case "IndexBufferPool":
                        saveName = ReadBasicEntry(resourceXML, name);
                        break;
                    case "VertexBufferPool":
                        saveName = ReadBasicEntry(resourceXML, name);
                        break;
                    case "AnimalTrafficPaths":
                        saveName = ReadBasicEntry(resourceXML, name);
                        break;
                    case "FrameResource":
                        saveName = ReadBasicEntry(resourceXML, name);
                        break;
                    case "Translokator":
                        saveName = ReadBasicEntry(resourceXML, name);
                        break;
                    case "Effects":
                        saveName = ReadBasicEntry(resourceXML, name);
                        break;
                    case "FrameNameTable":
                        saveName = ReadBasicEntry(resourceXML, name);
                        break;
                    case "EntityDataStorage":
                        saveName = ReadBasicEntry(resourceXML, name);
                        break;
                    case "PREFAB":
                        saveName = ReadBasicEntry(resourceXML, name);
                        break;
                    case "ItemDesc":
                        saveName = ReadBasicEntry(resourceXML, name);
                        break;
                    case "Actors":
                        saveName = ReadBasicEntry(resourceXML, name);
                        break;
                    case "Collisions":
                        saveName = ReadBasicEntry(resourceXML, name);
                        break;
                    case "Animation2":
                        saveName = ReadBasicEntry(resourceXML, name);
                        break;
                    case "NAV_AIWORLD_DATA":
                        saveName = ReadBasicEntry(resourceXML, name);
                        break;
                    case "NAV_OBJ_DATA":
                        saveName = ReadBasicEntry(resourceXML, name);
                        break;
                    case "NAV_HPD_DATA":
                        saveName = ReadBasicEntry(resourceXML, name);
                        break;
                    case "FxAnimSet":
                        saveName = ReadBasicEntry(resourceXML, name);
                        break;
                    case "FxActor":
                        saveName = ReadBasicEntry(resourceXML, name);
                        break;
                    case "Sound":
                        ReadSoundEntry(entry, resourceXML, name, finalPath);
                        saveName = name + ".fsb";
                        break;
                    case "Script":
                        ReadScriptEntry(entry, resourceXML, finalPath);
                        continue;
                    case "AudioSectors":
                        ReadAudioSectorEntry(entry, resourceXML, name, finalPath);
                        saveName = name;
                        break;
                    default:
                        Console.WriteLine("Unhandled Resource Type {0}", type);
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

        /// <summary>
        /// Save resource data from given sds data.
        /// </summary>
        /// <param name="xml"></param>
        public void SaveResources(FileInfo file)
        {
            XPathDocument doc = null;

            if (string.IsNullOrEmpty(ResourceInfoXml) == false)
            {
                using (var reader = new StringReader(ResourceInfoXml))
                {
                    doc = new XPathDocument(reader);
                }
            }
            else
            {
                int type = -1;
                for(int i = 0; i != ResourceTypes.Count; i++)
                {
                    if (ResourceTypes[i].Name == "")
                    {
                        type = (int)ResourceTypes[i].Id;
                    }
                }

                if (type != -1)
                {
                    for (int i = 0; i < ResourceEntries.Count; i++)
                    {
                        if (ResourceEntries[i].TypeId == type)
                        {
                            using (MemoryStream stream = new MemoryStream(ResourceEntries[i].Data))
                            {
                                ushort authorLen = stream.ReadValueU16();
                                stream.ReadBytes(authorLen);
                                int fileSize = stream.ReadValueS32();
                                int password = stream.ReadValueS32();

                                using (var reader = new StringReader(Encoding.UTF8.GetString(stream.ReadBytes(fileSize))))
                                {
                                    doc = new XPathDocument(reader);
                                }
                            }
                            ResourceEntries.RemoveAt(i);
                            ResourceTypes.RemoveAt(type);
                        }
                    }
                }
            }

            if (doc != null)
            {
                var nav = doc.CreateNavigator();
                var nodes = nav.Select("/xml/ResourceInfo/SourceDataDescription");
                while (nodes.MoveNext() == true)
                {
                    _ResourceNames.Add(nodes.Current.Value);
                }
                Log.WriteLine("Found all items; count is " + nodes.Count);
            }


            if (_ResourceNames.Count == 0)
            {
                //Fix for friends for life SDS files.
                //MessageBox.Show("Detected SDS with no ResourceXML. I do not recommend repacking this SDS. It could cause crashes!", "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Log.WriteLine("Detected SDS with no ResourceXML. I do not recommend repacking this SDS. It could cause crashes!", LoggingTypes.WARNING);
                for (int i = 0; i != ResourceEntries.Count; i++)
                {
                    ResourceEntry Entry = ResourceEntries[i];
                    string Typename = _ResourceTypes[Entry.TypeId].Name;

                    // TODO: Find a new place for this.
                    string Extension = ".bin";
                    if(Typename == "Texture")
                    {
                        Extension = ".dds";
                    }
                    else if(Typename == "Generic")
                    {
                        Extension = ".genr";
                    }
                    else if (Typename == "Flash")
                    {
                        Extension = ".fla";
                    }
                    else if(Typename == "hkAnimation")
                    {
                        Extension = ".hkx";
                    }

                    string FileName = string.Format("File_{0}{1}", i, Extension);
                    _ResourceNames.Add(FileName);
                }
            }

            if(Version == 19)
            {
                SaveResourcesVersion19(file, _ResourceNames);
            } 
            else if(Version == 20)
            {
                SaveResourcesVersion20(file, _ResourceNames);
            }
        }

        private XmlNode AddRamElement(XmlDocument xmlDoc, string name, int num)
        {
            XmlNode node = xmlDoc.CreateElement(name);
            XmlAttribute attribute = xmlDoc.CreateAttribute("__type");
            attribute.Value = "Int";
            node.InnerText = num.ToString();
            node.Attributes.Append(attribute);

            return node;
        }

        private void ReadTextureNames()
        {
            string FileName = "";
            _TextureNames = new Dictionary<ulong, string>();

            var game = GameStorage.Instance.GetSelectedGame();

            if(game.GameType == GamesEnumerator.MafiaI_DE)
            {
                FileName = "/Resources/GameData/M1_Textures.txt";
            }

            if (!string.IsNullOrEmpty(FileName))
            {
                string[] Files = File.ReadAllLines(Application.StartupPath + "/" + FileName);

                foreach (var File in Files)
                {
                    ulong Hash = FNV64.Hash(File);
                    _TextureNames.Add(Hash, File);
                }
            }
        }
    }
    #endregion
}
