using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Xml.Linq;
using Utils.Helpers.Reflection;

namespace ResourceTypes.GameParams
{
    /// <summary>
    /// Parser for gameparams.bin - Mafia 2 game parameters configuration file
    /// Based on reverse engineering of sub_11C9F20 and sub_11CBE80
    ///
    /// File Structure:
    /// - BitStream format with variable-width fields
    /// - Header: flags (32 bits), name (32 bytes null-terminated string)
    /// - Entries: type ID (4 bits) followed by type-specific serialized param data
    /// - Terminator: type ID 8 signals end of params
    ///
    /// The file stores game settings like:
    /// - gameParams.Options.GameSettings.*
    /// - gameParams.Options.VideoSettings.*
    /// - gameParams.Options.AudioSettings.*
    /// - gameParams.Options.ControlsSettings.*
    /// - gameParams.Controls.*
    /// - gameParams.DLC.*
    /// </summary>
    public class GameParamsFile
    {
        public const int TerminatorType = 8;
        public const int TypeIdBits = 4; // Calculated from game code

        [Category("Header")]
        public uint Flags { get; set; }

        [Category("Header")]
        public string Name { get; set; } = "";

        [Category("Data")]
        public List<GameParamEntry> Entries { get; set; } = new();

        // Raw file data for round-trip preservation
        [PropertyIgnoreByReflector]
        [Browsable(false)]
        public byte[] RawFileData { get; set; } = Array.Empty<byte>();

        public GameParamsFile()
        {
        }

        public GameParamsFile(FileInfo file)
        {
            ReadFromFile(file);
        }

        public GameParamsFile(string fileName)
        {
            ReadFromFile(fileName);
        }

        public void ReadFromFile(FileInfo file)
        {
            ReadFromFile(file.FullName);
        }

        public void ReadFromFile(string fileName)
        {
            RawFileData = File.ReadAllBytes(fileName);

            using (MemoryStream ms = new(RawFileData))
            {
                Read(ms);
            }
        }

        public void Read(Stream s)
        {
            using (GameParamsBitReader br = new(s))
            {
                // File starts with 4-bit type header (type 5 = C_GameParams)
                uint fileType = br.ReadBits(4);

                // 32 bits: flags field
                Flags = br.ReadBits(32);

                // 32 bytes: name string (null-terminated)
                Name = br.ReadString(32);

                // Parse entries
                ParseEntriesFromBitReader(br);
            }
        }

        /// <summary>
        /// Scan forward looking for a valid entry start (byte-aligned Container with ASCII name)
        /// </summary>
        private bool ScanForNextValidEntry(GameParamsBitReader br, out int foundTypeId)
        {
            foundTypeId = 0;
            int maxScanBytes = 50000; // Scan up to 50KB for next valid entry (file is ~66KB)
            int startByte = br.CurrentBytePosition;

            for (int i = 0; i < maxScanBytes && br.CurrentBytePosition < br.TotalBytes - 40; i++)
            {
                // Align to next byte boundary
                br.RestorePosition((startByte + i, 0));

                var savedPos = br.SavePosition();

                // Try to read as a Container (type 5) or Array (type 6) - most top-level entries are these
                int typeId = (int)br.ReadBits(TypeIdBits);

                // Only look for Containers/Arrays - they're the main structural elements
                if (typeId == 5 || typeId == 6)
                {
                    // Try to read flags + name
                    uint flags = br.ReadBits(32);
                    string name = br.ReadString(32);

                    // Check if name looks valid:
                    // - Must be at least 3 chars (avoid false positives like "ew")
                    // - Must start with ASCII uppercase letter A-Z (game uses PascalCase)
                    // - All chars must be ASCII alphanumeric or underscore
                    if (name.Length >= 3 && name.Length <= 30 && name[0] >= 'A' && name[0] <= 'Z')
                    {
                        bool validName = true;
                        foreach (char c in name)
                        {
                            // Only allow ASCII: A-Z, a-z, 0-9, _
                            bool isAsciiLetter = (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');
                            bool isAsciiDigit = c >= '0' && c <= '9';
                            if (!isAsciiLetter && !isAsciiDigit && c != '_')
                            {
                                validName = false;
                                break;
                            }
                        }

                        if (validName)
                        {
                            // Found a valid-looking entry! Restore to just after typeId
                            br.RestorePosition(savedPos);
                            br.ReadBits(TypeIdBits); // Re-read typeId to position correctly
                            foundTypeId = typeId;
                            return true;
                        }
                    }
                }

                // Also try other known types if they happen to have valid names
                // This catches things like standalone Bool, Int, Float, String params at top level
                else if (typeId >= 0 && typeId <= 4 || typeId == 7)
                {
                    // Read flags + name (we already read typeId above)
                    uint flags = br.ReadBits(32);
                    string name = br.ReadString(32);

                    // Same validation as containers - ASCII only
                    if (name.Length >= 3 && name.Length <= 30 && name[0] >= 'A' && name[0] <= 'Z')
                    {
                        bool validName = true;
                        foreach (char c in name)
                        {
                            bool isAsciiLetter = (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');
                            bool isAsciiDigit = c >= '0' && c <= '9';
                            if (!isAsciiLetter && !isAsciiDigit && c != '_')
                            {
                                validName = false;
                                break;
                            }
                        }

                        if (validName)
                        {
                            br.RestorePosition(savedPos);
                            br.ReadBits(TypeIdBits);
                            foundTypeId = typeId;
                            return true;
                        }
                    }
                }

                // Not valid, restore and try next byte
                br.RestorePosition(savedPos);
            }

            return false;
        }

        private void ParseEntriesFromBitReader(GameParamsBitReader br)
        {
            Entries.Clear();
            int entryIndex = 0;

            while (!br.IsAtEnd)
            {
                int posBeforeType = br.CurrentBytePosition;
                int bitBeforeType = br.CurrentBitPosition;

                // Read 4-bit type ID
                int typeId = (int)br.ReadBits(TypeIdBits);

                if (typeId == TerminatorType)
                {
                    // Continue scanning for more data after terminator
                    if (ScanForNextValidEntry(br, out int foundTypeId))
                    {
                        typeId = foundTypeId;
                        // Fall through to process this entry
                    }
                    else
                    {
                        break;
                    }
                }

                try
                {
                    GameParamEntry entry = GameParamEntry.CreateForType(typeId);
                    entry.TypeId = typeId;

                    // Try to read - if we hit padding/invalid data, try to scan forward
                    if (!entry.TryRead(br))
                    {
                        // Try to find next valid entry by scanning forward
                        if (ScanForNextValidEntry(br, out int foundTypeId))
                        {
                            // Read the found entry
                            entry = GameParamEntry.CreateForType(foundTypeId);
                            entry.TypeId = foundTypeId;
                            if (entry.TryRead(br))
                            {
                                Entries.Add(entry);
                                entryIndex++;
                                continue;
                            }
                        }

                        break;
                    }

                    Entries.Add(entry);
                    entryIndex++;
                }
                catch (Exception ex)
                {
                    throw new InvalidDataException(
                        $"Failed to parse entry {entryIndex} (typeId={typeId}) at byte {posBeforeType}, bit {bitBeforeType}: {ex.Message}", ex);
                }
            }
        }

        public void WriteToFile(FileInfo file)
        {
            // Writing disabled - file format is read-only for now
            throw new NotSupportedException("GameParams writing is not supported yet.");
        }

        public void WriteToFile(string fileName)
        {
            // Writing disabled - file format is read-only for now
            throw new NotSupportedException("GameParams writing is not supported yet.");
        }

        public void Write(Stream s)
        {
            using (GameParamsBitWriter bw = new(s))
            {
                // Write 4-bit file type header (type 5 = C_GameParams container)
                bw.WriteBits(5, 4);

                // Write C_GameParams header
                bw.WriteBits(Flags, 32);
                bw.WriteString(Name, 32);

                // Write entries
                foreach (var entry in Entries)
                {
                    bw.WriteBits((uint)entry.TypeId, TypeIdBits);
                    entry.Write(bw);
                }

                // Write terminator
                bw.WriteBits(TerminatorType, TypeIdBits);
            }
        }

        public void ConvertToXML(string filename)
        {
            // XML export disabled - file format is read-only for now
            throw new NotSupportedException("GameParams XML export is not supported yet.");
        }

        public void ConvertFromXML(string filename)
        {
            // XML import disabled - file format is read-only for now
            throw new NotSupportedException("GameParams XML import is not supported yet.");
        }
    }

    /// <summary>
    /// Base class for game param entries
    /// </summary>
    public class GameParamEntry
    {
        [Category("Entry")]
        public int TypeId { get; set; }

        [Category("Entry")]
        public uint EntryFlags { get; set; }

        [Category("Entry")]
        public string ParamName { get; set; } = "";

        // Raw data for types we can't fully parse
        [PropertyIgnoreByReflector]
        [Browsable(false)]
        public byte[] RawEntryData { get; set; } = Array.Empty<byte>();

        /// <summary>
        /// Creates the appropriate entry type based on type ID.
        /// Type mappings from IDA analysis of Mafia 2 executable:
        /// - Type 0: Base param (flags + name only, no value data)
        /// - Type 1: IntParam (GetType at 0x11BBCC0 returns 1, vtable 0x190BEF8)
        /// - Type 2: FloatParam (GetType at 0x11BC030 returns 2, vtable 0x190BF68)
        /// - Type 3: StringParam (GetType at 0x11BC150 returns 3)
        /// - Type 4: IntParam variant (GetType at 0x11BC370 returns 4, vtable 0x190BFD8)
        /// - Type 5: C_GameParams Container (GetType at 0x11C54B0 returns 5, vtable 0x190C5B4)
        /// - Type 6: C_GameParamArray Container (GetType at 0x11C95F0 returns 6, vtable 0x190C610)
        /// - Type 7: BoolParam (GetType at 0x11BBF10 returns 7)
        /// - Type 8: Terminator
        /// </summary>
        public static GameParamEntry CreateForType(int typeId)
        {
            return typeId switch
            {
                0 => new GameParamBoolEntry(),       // BoolParam variant (same as Type 7)
                1 => new GameParamIntEntry(),        // IntParam
                2 => new GameParamFloatEntry(),      // FloatParam
                3 => new GameParamStringEntry(),     // StringParam
                4 => new GameParamIntEntry(),        // IntParam variant
                5 => new GameParamContainerEntry(),  // C_GameParams - Container
                6 => new GameParamArrayEntry(),      // C_GameParamArray - Array container (same Parse as type 5)
                7 => new GameParamBoolEntry(),       // BoolParam
                9 => new GameParamUnknown9Entry(),   // Unknown type 9 - needs investigation
                _ => throw new InvalidDataException($"Unknown GameParam type ID: {typeId}. This would cause bit alignment corruption.")
            };
        }

        public virtual void Read(GameParamsBitReader br)
        {
            // Base read: flags and name common to all types
            EntryFlags = br.ReadBits(32);
            ParamName = br.ReadString(32);

            // Validate name - if it contains non-printable chars, we have alignment issues
            // Allow digits at start (for indexed entries like "00", "01")
            foreach (char c in ParamName)
            {
                if (c < 0x20 || c > 0x7E)
                {
                    throw new InvalidDataException(
                        $"Invalid character 0x{(int)c:X2} in param name '{ParamName}' at byte {br.CurrentBytePosition}. This indicates bit alignment corruption.");
                }
            }
        }

        /// <summary>
        /// Try to read the entry, returns false if name contains invalid characters
        /// (indicating we've hit padding/uninitialized data)
        /// </summary>
        public virtual bool TryRead(GameParamsBitReader br)
        {
            try
            {
                Read(br);
                return true;
            }
            catch (InvalidDataException)
            {
                return false; // Hit padding/garbage data
            }
        }

        public virtual void Write(GameParamsBitWriter bw)
        {
            bw.WriteBits(EntryFlags, 32);
            bw.WriteString(ParamName, 32);
        }

        public override string ToString()
        {
            return $"[Type {TypeId}] {ParamName}";
        }
    }

    /// <summary>
    /// Integer parameter with min/max range
    /// </summary>
    public class GameParamIntEntry : GameParamEntry
    {
        [Category("Value")]
        public int MinValue { get; set; }

        [Category("Value")]
        public int MaxValue { get; set; }

        [Category("Value")]
        public int CurrentValue { get; set; }

        public override void Read(GameParamsBitReader br)
        {
            base.Read(br);

            MinValue = (int)br.ReadBits(32);
            MaxValue = (int)br.ReadBits(32);

            // Calculate bits needed for range
            uint range = (uint)(MaxValue - MinValue);
            int bits = CalculateBitsForRange(range);

            CurrentValue = MinValue + (int)br.ReadBits(bits);
        }

        public override void Write(GameParamsBitWriter bw)
        {
            base.Write(bw);

            bw.WriteBits((uint)MinValue, 32);
            bw.WriteBits((uint)MaxValue, 32);

            uint range = (uint)(MaxValue - MinValue);
            int bits = CalculateBitsForRange(range);
            bw.WriteBits((uint)(CurrentValue - MinValue), bits);
        }

        private static int CalculateBitsForRange(uint range)
        {
            if (range == 0) return 1;
            int bits = 1;
            while ((1u << bits) - 1 < range)
            {
                bits++;
                if (bits > 32) break;
            }
            return bits;
        }

        public override string ToString()
        {
            return $"[Int] {ParamName} = {CurrentValue} ({MinValue}-{MaxValue})";
        }
    }

    /// <summary>
    /// Float parameter with min/max range and step
    /// </summary>
    public class GameParamFloatEntry : GameParamEntry
    {
        [Category("Value")]
        public float MinValue { get; set; }

        [Category("Value")]
        public float MaxValue { get; set; }

        [Category("Value")]
        public float Step { get; set; }

        [Category("Value")]
        public float CurrentValue { get; set; }

        public override void Read(GameParamsBitReader br)
        {
            base.Read(br);

            MinValue = br.ReadFloat();
            MaxValue = br.ReadFloat();
            Step = br.ReadFloat();

            // Calculate bits needed for normalized range
            // When min/max span entire float range or range is too large, store raw 32-bit float
            int bits = GetBitsForValue(out bool useRawFloat);

            if (useRawFloat)
            {
                // Range too large - read as raw 32-bit float
                CurrentValue = br.ReadFloat();
            }
            else
            {
                uint normalized = br.ReadBits(bits);
                CurrentValue = MinValue + (float)(normalized * Step);
            }
        }

        public override void Write(GameParamsBitWriter bw)
        {
            base.Write(bw);

            bw.WriteFloat(MinValue);
            bw.WriteFloat(MaxValue);
            bw.WriteFloat(Step);

            int bits = GetBitsForValue(out bool useRawFloat);

            if (useRawFloat)
            {
                bw.WriteFloat(CurrentValue);
            }
            else
            {
                uint normalized = Step != 0 ? (uint)((CurrentValue - MinValue) / Step) : 0;
                bw.WriteBits(normalized, bits);
            }
        }

        private int GetBitsForValue(out bool useRawFloat)
        {
            useRawFloat = false;

            // Check for full float range or invalid step
            if (Step == 0 || float.IsNaN(Step) || float.IsInfinity(Step) ||
                float.IsInfinity(MinValue) || float.IsInfinity(MaxValue) ||
                MinValue <= float.MinValue / 2 || MaxValue >= float.MaxValue / 2)
            {
                useRawFloat = true;
                return 32;
            }

            double range = (MaxValue - MinValue) / Step;
            if (range <= 0 || range > uint.MaxValue || double.IsNaN(range) || double.IsInfinity(range))
            {
                useRawFloat = true;
                return 32;
            }

            return CalculateBitsForRange((uint)range);
        }

        private static int CalculateBitsForRange(uint range)
        {
            if (range == 0) return 1;
            int bits = 1;
            while ((1u << bits) - 1 < range)
            {
                bits++;
                if (bits > 32) break;
            }
            return bits;
        }

        public override string ToString()
        {
            return $"[Float] {ParamName} = {CurrentValue} ({MinValue}-{MaxValue}, step {Step})";
        }
    }

    /// <summary>
    /// String parameter
    /// IDA: sub_11BF7E0 reads 32-bit flags + 32-byte name + 32-bit unknown + 255-byte string
    /// </summary>
    public class GameParamStringEntry : GameParamEntry
    {
        [Category("Value")]
        public uint StringFlags { get; set; }

        [Category("Value")]
        public string Value { get; set; } = "";

        public override void Read(GameParamsBitReader br)
        {
            base.Read(br);

            // 32-bit unknown field (stored at offset 44 in game)
            StringFlags = br.ReadBits(32);

            // 255-byte null-terminated string
            Value = br.ReadString(255);
        }

        public override void Write(GameParamsBitWriter bw)
        {
            base.Write(bw);

            bw.WriteBits(StringFlags, 32);
            bw.WriteString(Value, 255);
        }

        public override string ToString()
        {
            return $"[String] {ParamName} = \"{Value}\"";
        }
    }

    /// <summary>
    /// Boolean parameter
    /// </summary>
    public class GameParamBoolEntry : GameParamEntry
    {
        [Category("Value")]
        public bool Value { get; set; }

        public override void Read(GameParamsBitReader br)
        {
            base.Read(br);
            Value = br.ReadBits(1) != 0;
        }

        public override void Write(GameParamsBitWriter bw)
        {
            base.Write(bw);
            bw.WriteBits(Value ? 1u : 0u, 1);
        }

        public override string ToString()
        {
            return $"[Bool] {ParamName} = {Value}";
        }
    }

    /// <summary>
    /// Container parameter that holds child params (C_GameParams - Type 5)
    /// </summary>
    public class GameParamContainerEntry : GameParamEntry
    {
        [Category("Children")]
        public List<GameParamEntry> Children { get; set; } = new();

        public override void Read(GameParamsBitReader br)
        {
            base.Read(br);

            // Read child entries until terminator
            Children.Clear();
            int childIndex = 0;
            while (!br.IsAtEnd)
            {
                int posBeforeType = br.CurrentBytePosition;
                int bitBeforeType = br.CurrentBitPosition;

                int typeId = (int)br.ReadBits(GameParamsFile.TypeIdBits);

                if (typeId == GameParamsFile.TerminatorType)
                {
                    break;
                }

                try
                {
                    GameParamEntry child = CreateForType(typeId);
                    child.TypeId = typeId;

                    // Try to read - if we hit invalid data (padding), treat as end of children
                    if (!child.TryRead(br))
                    {
                        br.RestorePosition((posBeforeType, bitBeforeType)); // Restore to before typeId
                        break;
                    }

                    Children.Add(child);
                    childIndex++;
                }
                catch (Exception ex)
                {
                    throw new InvalidDataException(
                        $"Failed to parse child {childIndex} of '{ParamName}' (typeId={typeId}) at byte {posBeforeType}, bit {bitBeforeType}: {ex.Message}", ex);
                }
            }
        }

        public override void Write(GameParamsBitWriter bw)
        {
            base.Write(bw);

            foreach (var child in Children)
            {
                bw.WriteBits((uint)child.TypeId, GameParamsFile.TypeIdBits);
                child.Write(bw);
            }

            bw.WriteBits(GameParamsFile.TerminatorType, GameParamsFile.TypeIdBits);
        }

        public override string ToString()
        {
            return $"[Container] {ParamName} ({Children.Count} children)";
        }
    }

    /// <summary>
    /// Array container parameter (C_GameParamArray - Type 6)
    /// Uses identical serialization as C_GameParams (Type 5) - flags, name, child entries
    /// IDA: GetType at 0x11C95F0 returns 6, vtable 0x190C610, Parse uses sub_11CBE80
    /// </summary>
    public class GameParamArrayEntry : GameParamContainerEntry
    {
        public override string ToString()
        {
            return $"[Array] {ParamName} ({Children.Count} items)";
        }
    }

    /// <summary>
    /// Type 9: Unknown entry type - just reads base data for debugging
    /// </summary>
    public class GameParamUnknown9Entry : GameParamEntry
    {
        public override void Read(GameParamsBitReader br)
        {
            // Just read base data (flags + name)
            base.Read(br);
        }

        public override string ToString()
        {
            return $"[Type9] {ParamName}";
        }
    }

    /// <summary>
    /// BitStream reader for gameparams.bin format
    /// Uses DWORD-based bit reading like the game engine
    /// </summary>
    public class GameParamsBitReader : IDisposable
    {
        private readonly byte[] data;
        private int bytePosition;
        private int bitPosition;

        public bool IsAtEnd => bytePosition >= data.Length;
        public int CurrentBytePosition => bytePosition;
        public int CurrentBitPosition => bitPosition;
        public int TotalBytes => data.Length;

        public (int bytePos, int bitPos) SavePosition() => (bytePosition, bitPosition);

        public void RestorePosition((int bytePos, int bitPos) pos)
        {
            bytePosition = pos.bytePos;
            bitPosition = pos.bitPos;
        }

        public GameParamsBitReader(Stream s)
        {
            using (MemoryStream ms = new())
            {
                s.CopyTo(ms);
                data = ms.ToArray();
            }
            bytePosition = 0;
            bitPosition = 0;
        }

        public uint ReadBits(int count)
        {
            uint result = 0;
            for (int i = 0; i < count; i++)
            {
                if (bytePosition >= data.Length)
                    break;

                if ((data[bytePosition] & (1 << bitPosition)) != 0)
                {
                    result |= (1u << i);
                }

                bitPosition++;
                if (bitPosition >= 8)
                {
                    bitPosition = 0;
                    bytePosition++;
                }
            }
            return result;
        }

        public byte ReadByte()
        {
            return (byte)ReadBits(8);
        }

        public float ReadFloat()
        {
            uint bits = ReadBits(32);
            byte[] bytes = BitConverter.GetBytes(bits);
            return BitConverter.ToSingle(bytes, 0);
        }

        /// <summary>
        /// Read a null-terminated string, consuming up to maxLength bytes.
        /// IMPORTANT: The game's GetString stops reading at null, not consuming remaining bytes!
        /// </summary>
        public string ReadString(int maxLength)
        {
            StringBuilder sb = new();
            for (int i = 0; i < maxLength; i++)
            {
                byte b = ReadByte();
                if (b == 0) break;
                sb.Append((char)b);
            }
            return sb.ToString();
        }

        public void Dispose()
        {
        }
    }

    /// <summary>
    /// BitStream writer for gameparams.bin format
    /// </summary>
    public class GameParamsBitWriter : IDisposable
    {
        private readonly Stream stream;
        private readonly List<byte> buffer = new();
        private byte currentByte;
        private int bitPosition;

        public GameParamsBitWriter(Stream s)
        {
            stream = s;
        }

        public void WriteBits(uint value, int count)
        {
            for (int i = 0; i < count; i++)
            {
                if ((value & (1u << i)) != 0)
                {
                    currentByte |= (byte)(1 << bitPosition);
                }

                bitPosition++;
                if (bitPosition >= 8)
                {
                    buffer.Add(currentByte);
                    currentByte = 0;
                    bitPosition = 0;
                }
            }
        }

        public void WriteByte(byte value)
        {
            WriteBits(value, 8);
        }

        public void WriteFloat(float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            uint bits = BitConverter.ToUInt32(bytes, 0);
            WriteBits(bits, 32);
        }

        /// <summary>
        /// Write a null-terminated string (matching game's GetString format).
        /// Writes string bytes + null terminator only, no padding.
        /// </summary>
        public void WriteString(string value, int maxLength)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(value ?? "");
            int writeLen = Math.Min(bytes.Length, maxLength - 1); // Leave room for null
            for (int i = 0; i < writeLen; i++)
            {
                WriteByte(bytes[i]);
            }
            WriteByte(0); // Null terminator
        }

        public void Flush()
        {
            if (bitPosition > 0)
            {
                buffer.Add(currentByte);
                currentByte = 0;
                bitPosition = 0;
            }
            stream.Write(buffer.ToArray(), 0, buffer.Count);
            buffer.Clear();
        }

        public void Dispose()
        {
            Flush();
        }
    }
}
