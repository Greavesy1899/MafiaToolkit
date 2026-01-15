using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Gibbed.Illusion.FileFormats.Hashing;
using Utils.Helpers.Reflection;
using Utils.Logging;
using Utils.StringHelpers;

namespace ResourceTypes.FrameProps
{
    /// <summary>
    /// Parser for FrameProps.bin - Mafia 2 frame properties configuration file
    /// Based on reverse engineering of C_FrameProps::LoadFromSharedMemory
    ///
    /// File Structure:
    /// - Header: magic (0x66726d70 'pmrf'), version (3), stringOffsetCount, stringDataSize, entryCount, reserved1, reserved2
    /// - StringOffsetTable: uint32[] offsets into string data
    /// - StringData: null-terminated strings
    /// - FrameHashTable: uint64[] sorted frame name hashes for binary search
    /// - FrameDataOffsets: uint32[] offsets into PropertyData for each frame
    /// - PropertyData: variable-length property entries
    /// </summary>
    public class FramePropsFile
    {
        public const uint Magic = 0x66726d70; // 'pmrf' in little-endian
        public const int Version = 3;

        [PropertyIgnoreByReflector]
        public List<string> StringTable { get; set; } = new();

        public FramePropsEntry[] Entries { get; set; } = Array.Empty<FramePropsEntry>();

        public FramePropsFile()
        {
        }

        public FramePropsFile(FileInfo file)
        {
            ReadFromFile(file);
        }

        public FramePropsFile(string fileName)
        {
            ReadFromFile(fileName);
        }

        public void ReadFromFile(FileInfo file)
        {
            ReadFromFile(file.FullName);
        }

        public void ReadFromFile(string fileName)
        {
            using (MemoryStream ms = new(File.ReadAllBytes(fileName)))
            {
                Read(ms);
            }
        }

        public void Read(Stream s)
        {
            using (BinaryReader br = new(s))
            {
                Read(br);
            }
        }

        public void Read(BinaryReader br)
        {
            uint magic = br.ReadUInt32();
            int version = br.ReadInt32();

            if (magic != Magic)
                throw new FormatException($"Invalid FrameProps magic: expected 0x{Magic:X8}, got 0x{magic:X8}");
            if (version != Version)
                throw new FormatException($"Unsupported FrameProps version: expected {Version}, got {version}");

            // Read header fields
            int stringOffsetCount = br.ReadInt32();
            int stringDataSize = br.ReadInt32();
            int entryCount = br.ReadInt32();
            int reserved1 = br.ReadInt32();
            int reserved2 = br.ReadInt32();

            // Read string offset table
            uint[] stringOffsets = new uint[stringOffsetCount];
            for (int i = 0; i < stringOffsetCount; i++)
            {
                stringOffsets[i] = br.ReadUInt32();
            }

            // Read string data section
            long stringDataStart = br.BaseStream.Position;
            byte[] stringData = br.ReadBytes(stringDataSize);

            // Parse strings from offsets
            StringTable = new List<string>(stringOffsetCount);
            for (int i = 0; i < stringOffsetCount; i++)
            {
                int offset = (int)stringOffsets[i];
                StringBuilder sb = new StringBuilder();
                while (offset < stringData.Length && stringData[offset] != 0)
                {
                    sb.Append((char)stringData[offset++]);
                }
                StringTable.Add(sb.ToString());
            }

            // Read frame hash table (sorted for binary search)
            ulong[] frameHashes = new ulong[entryCount];
            for (int i = 0; i < entryCount; i++)
            {
                frameHashes[i] = br.ReadUInt64();
            }

            // Read frame data offsets
            uint[] frameDataOffsets = new uint[entryCount];
            for (int i = 0; i < entryCount; i++)
            {
                frameDataOffsets[i] = br.ReadUInt32();
            }

            // Read property data for each frame
            long propertyDataStart = br.BaseStream.Position;
            Entries = new FramePropsEntry[entryCount];

            for (int i = 0; i < entryCount; i++)
            {
                br.BaseStream.Position = propertyDataStart + frameDataOffsets[i];
                Entries[i] = ReadFramePropsEntry(br, frameHashes[i]);
            }

            // Sort entries by hash for consistency
            Array.Sort(Entries, (a, b) => a.FrameNameHash.CompareTo(b.FrameNameHash));
        }

        private FramePropsEntry ReadFramePropsEntry(BinaryReader br, ulong frameHash)
        {
            FramePropsEntry entry = new FramePropsEntry();
            entry.FrameNameHash = frameHash;

            ushort propertyCount = br.ReadUInt16();

            // First pass: read property headers (hash + string count)
            var propertyHeaders = new List<(ulong hash, byte stringCount)>(propertyCount);
            for (int i = 0; i < propertyCount; i++)
            {
                ulong propHash = br.ReadUInt64();
                byte stringCount = br.ReadByte();
                propertyHeaders.Add((propHash, stringCount));
            }

            // Second pass: read string indices and build property values
            entry.Properties = new FrameProperty[propertyCount];
            for (int i = 0; i < propertyCount; i++)
            {
                var header = propertyHeaders[i];
                FrameProperty prop = new FrameProperty();
                prop.PropertyNameHash = header.hash;

                // Read string indices and concatenate values
                StringBuilder valueBuilder = new StringBuilder();
                for (int j = 0; j < header.stringCount; j++)
                {
                    ushort stringIndex = br.ReadUInt16();
                    if (stringIndex < StringTable.Count)
                    {
                        if (valueBuilder.Length > 0)
                            valueBuilder.Append(';');
                        valueBuilder.Append(StringTable[stringIndex]);
                    }
                }
                prop.Value = valueBuilder.ToString();

                entry.Properties[i] = prop;
            }

            return entry;
        }

        public void WriteToFile(FileInfo file)
        {
            WriteToFile(file.FullName);
        }

        public void WriteToFile(string fileName)
        {
            using (MemoryStream ms = new())
            {
                Write(ms);
                File.WriteAllBytes(fileName, ms.ToArray());
            }
        }

        public void Write(Stream s)
        {
            using (BinaryWriter bw = new(s))
            {
                Write(bw);
            }
        }

        public void Write(BinaryWriter bw)
        {
            // Build string table from all property values
            Dictionary<string, int> stringToIndex = new Dictionary<string, int>();
            List<string> newStringTable = new List<string>();

            foreach (var entry in Entries)
            {
                foreach (var prop in entry.Properties)
                {
                    // Split value by semicolon to get individual strings
                    string[] parts = prop.Value.Split(';');
                    foreach (var part in parts)
                    {
                        if (!stringToIndex.ContainsKey(part))
                        {
                            stringToIndex[part] = newStringTable.Count;
                            newStringTable.Add(part);
                        }
                    }
                }
            }

            // Build string data section
            using (MemoryStream stringDataMs = new())
            {
                uint[] stringOffsets = new uint[newStringTable.Count];
                for (int i = 0; i < newStringTable.Count; i++)
                {
                    stringOffsets[i] = (uint)stringDataMs.Position;
                    byte[] strBytes = Encoding.ASCII.GetBytes(newStringTable[i]);
                    stringDataMs.Write(strBytes, 0, strBytes.Length);
                    stringDataMs.WriteByte(0); // null terminator
                }
                byte[] stringData = stringDataMs.ToArray();

                // Build property data section
                using (MemoryStream propertyDataMs = new())
                {
                    uint[] frameDataOffsets = new uint[Entries.Length];

                    // Sort entries by hash for binary search compatibility
                    var sortedEntries = Entries.OrderBy(e => e.FrameNameHash).ToArray();

                    for (int i = 0; i < sortedEntries.Length; i++)
                    {
                        frameDataOffsets[i] = (uint)propertyDataMs.Position;
                        WriteFramePropsEntry(propertyDataMs, sortedEntries[i], stringToIndex);
                    }

                    byte[] propertyData = propertyDataMs.ToArray();

                    // Write header
                    bw.Write(Magic);
                    bw.Write(Version);
                    bw.Write(newStringTable.Count); // stringOffsetCount
                    bw.Write(stringData.Length);     // stringDataSize
                    bw.Write(Entries.Length);        // entryCount
                    bw.Write(0);                     // reserved1
                    bw.Write(0);                     // reserved2

                    // Write string offset table
                    foreach (var offset in stringOffsets)
                    {
                        bw.Write(offset);
                    }

                    // Write string data
                    bw.Write(stringData);

                    // Write frame hash table (sorted)
                    foreach (var entry in sortedEntries)
                    {
                        bw.Write(entry.FrameNameHash);
                    }

                    // Write frame data offsets
                    foreach (var offset in frameDataOffsets)
                    {
                        bw.Write(offset);
                    }

                    // Write property data
                    bw.Write(propertyData);
                }
            }
        }

        private void WriteFramePropsEntry(MemoryStream ms, FramePropsEntry entry, Dictionary<string, int> stringToIndex)
        {
            using (BinaryWriter bw = new(ms, Encoding.ASCII, true))
            {
                bw.Write((ushort)entry.Properties.Length);

                // Build string indices for each property
                var propertyStringIndices = new List<ushort[]>();
                foreach (var prop in entry.Properties)
                {
                    string[] parts = prop.Value.Split(';');
                    ushort[] indices = parts.Select(p => (ushort)stringToIndex[p]).ToArray();
                    propertyStringIndices.Add(indices);
                }

                // Write property headers
                for (int i = 0; i < entry.Properties.Length; i++)
                {
                    bw.Write(entry.Properties[i].PropertyNameHash);
                    bw.Write((byte)propertyStringIndices[i].Length);
                }

                // Write string indices
                foreach (var indices in propertyStringIndices)
                {
                    foreach (var index in indices)
                    {
                        bw.Write(index);
                    }
                }
            }
        }

        public void ConvertToXML(string filename)
        {
            XElement root = ReflectionHelpers.ConvertPropertyToXML(this);
            root.Save(filename);
        }

        public void ConvertFromXML(string filename)
        {
            XElement loadedDoc = XElement.Load(filename);
            FramePropsFile fileContents = ReflectionHelpers.ConvertToPropertyFromXML<FramePropsFile>(loadedDoc);

            // Copy data from loaded XML
            Entries = fileContents.Entries;
        }

        /// <summary>
        /// Lookup frame properties by frame name hash
        /// </summary>
        public FramePropsEntry GetFramePropsEntry(ulong frameNameHash)
        {
            return Entries.FirstOrDefault(e => e.FrameNameHash == frameNameHash);
        }

        /// <summary>
        /// Lookup frame properties by frame name (computes FNV64 hash)
        /// </summary>
        public FramePropsEntry GetFramePropsEntry(string frameName)
        {
            ulong hash = FNV64.Hash(frameName);
            return GetFramePropsEntry(hash);
        }
    }
}
