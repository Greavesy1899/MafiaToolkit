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

        private void ParseEntriesFromBitReader(GameParamsBitReader br)
        {
            Entries.Clear();

            while (!br.IsAtEnd)
            {
                // Read 4-bit type ID
                int typeId = (int)br.ReadBits(TypeIdBits);

                if (typeId == TerminatorType)
                {
                    break;
                }

                GameParamEntry entry = GameParamEntry.CreateForType(typeId);
                entry.TypeId = typeId;
                entry.Read(br);
                Entries.Add(entry);
            }
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
            XElement root = ReflectionHelpers.ConvertPropertyToXML(this);
            root.Save(filename);
        }

        public void ConvertFromXML(string filename)
        {
            XElement loadedDoc = XElement.Load(filename);
            GameParamsFile fileContents = ReflectionHelpers.ConvertToPropertyFromXML<GameParamsFile>(loadedDoc);

            Flags = fileContents.Flags;
            Name = fileContents.Name;
            Entries = fileContents.Entries;
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
                1 => new GameParamIntEntry(),        // IntParam
                2 => new GameParamFloatEntry(),      // FloatParam
                3 => new GameParamStringEntry(),     // StringParam
                4 => new GameParamIntEntry(),        // IntParam variant
                5 => new GameParamContainerEntry(),  // C_GameParams - Container
                6 => new GameParamArrayEntry(),      // C_GameParamArray - Array container (same Parse as type 5)
                7 => new GameParamBoolEntry(),       // BoolParam
                _ => new GameParamEntry()            // Unknown type - read base only
            };
        }

        public virtual void Read(GameParamsBitReader br)
        {
            // Base read: flags and name common to all types
            EntryFlags = br.ReadBits(32);
            ParamName = br.ReadString(32);
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
            double range = (MaxValue - MinValue) / Step;
            int bits = CalculateBitsForRange((uint)range);

            uint normalized = br.ReadBits(bits);
            CurrentValue = MinValue + (float)(normalized * Step);
        }

        public override void Write(GameParamsBitWriter bw)
        {
            base.Write(bw);

            bw.WriteFloat(MinValue);
            bw.WriteFloat(MaxValue);
            bw.WriteFloat(Step);

            double range = (MaxValue - MinValue) / Step;
            int bits = CalculateBitsForRange((uint)range);
            uint normalized = (uint)((CurrentValue - MinValue) / Step);
            bw.WriteBits(normalized, bits);
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
            while (!br.IsAtEnd)
            {
                int typeId = (int)br.ReadBits(GameParamsFile.TypeIdBits);

                if (typeId == GameParamsFile.TerminatorType)
                {
                    break;
                }

                GameParamEntry child = CreateForType(typeId);
                child.TypeId = typeId;
                child.Read(br);
                Children.Add(child);
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

        public string GetHexDump(int startByte, int count)
        {
            StringBuilder sb = new();
            int end = Math.Min(startByte + count, data.Length);
            for (int i = startByte; i < end; i++)
            {
                sb.Append($"{data[i]:X2} ");
                if ((i - startByte + 1) % 16 == 0) sb.AppendLine();
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
