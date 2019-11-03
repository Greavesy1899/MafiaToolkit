using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Gibbed.IO;

namespace ResourceTypes.Collisions.Opcode
{
    // NOTE: Could be migrated to Gibbed.IO.Endian to reduce
    // the number of semantically identical types
    public enum Endian
    {
        Little,
        Big
    }

    public interface IOpcodeSerializable
    {
        void Load(BinaryReader reader);
        void Save(BinaryWriter writer, Endian endian = Endian.Little);
        uint GetUsedBytes();
    }

    /// <summary>
    /// In general class contains some kind of reflection to the utility methods
    /// located in the <c>Serialize.h</c> and <c>Serialize.cpp</c> of the original SDK 
    /// </summary>
    static class SerializationUtils
    {
        public static uint ReadDword(BinaryReader reader, bool endianMismatch = false)
        {
            uint value = reader.ReadUInt32();
            if (endianMismatch)
            {
                value = value.Swap();
            }
            return value;
        }

        public static void WriteDword(uint value, BinaryWriter writer, bool endianMismatch = false)
        {
            uint valueToWrite = value;
            if (endianMismatch)
            {
                valueToWrite = valueToWrite.Swap();
            }
            writer.Write(valueToWrite);
        }

        public static ushort ReadWord(BinaryReader reader, bool endianMismatch = false)
        {
            ushort value = reader.ReadUInt16();
            if (endianMismatch)
            {
                value = value.Swap();
            }
            return value;
        }

        public static void WriteWord(ushort value, BinaryWriter writer, bool endianMismatch = false)
        {
            ushort valueToWrite = value;
            if (endianMismatch)
            {
                valueToWrite = valueToWrite.Swap();
            }
            writer.Write(valueToWrite);
        }

        public static short ReadShort(BinaryReader reader, bool endianMismatch = false)
        {
            short value = reader.ReadInt16();
            if (endianMismatch)
            {
                value = value.Swap();
            }
            return value;
        }

        public static void WriteShort(short value, BinaryWriter writer, bool endianMismatch = false)
        {
            short valueToWrite = value;
            if (endianMismatch)
            {
                valueToWrite = valueToWrite.Swap();
            }
            writer.Write(valueToWrite);
        }

        public static float ReadFloat(BinaryReader reader, bool endianMismatch = false)
        {
            float value = reader.ReadSingle();
            if (endianMismatch)
            {
                value = value.Swap();
            }
            return value;
        }

        public static void WriteFloat(float value, BinaryWriter writer, bool endianMismatch = false)
        {
            float valueToWrite = value;
            if (endianMismatch)
            {
                valueToWrite = valueToWrite.Swap();
            }
            writer.Write(valueToWrite);
        }

        public static bool ReadHeader(char hdr1, char hdr2, char hdr3, out char a, out char b, out char c, out char d, out uint version, out bool isLittleEndian, BinaryReader reader)
        {
            char h1, h2, h3;
            ReadChunk(out h1, out h2, out h3, out isLittleEndian, reader);
            if (h1 != hdr1 || h2 != hdr2 || h3 != hdr3)
            {
                a = b = c = d = '?';
                version = 0;
                return false;
            }

            ReadChunk(out a, out b, out c, out d, reader);
            version = ReadDword(reader, !isLittleEndian);
            return true;
        }

        public static void WriteHeader(char a, char b, char c, char d, uint version, bool isLittleEndian, BinaryWriter writer)
        {
            WriteChunk('N', 'X', 'S', isLittleEndian, writer); // "Novodex stream" identifier
            WriteChunk(a, b, c, d, writer);
            WriteDword(version, writer, !isLittleEndian);
        }

        public static void ReadChunk(out char a, out char b, out char c, out char d, BinaryReader reader)
        {
            a = Convert.ToChar(reader.ReadByte());
            b = Convert.ToChar(reader.ReadByte());
            c = Convert.ToChar(reader.ReadByte());
            d = Convert.ToChar(reader.ReadByte());
        }

        public static void ReadChunk(out char a, out char b, out char c, out bool d, BinaryReader reader)
        {
            a = Convert.ToChar(reader.ReadByte());
            b = Convert.ToChar(reader.ReadByte());
            c = Convert.ToChar(reader.ReadByte());
            d = Convert.ToBoolean(reader.ReadByte());
        }

        public static void WriteChunk(char a, char b, char c, char d, BinaryWriter writer)
        {
            writer.Write(Convert.ToByte(a));
            writer.Write(Convert.ToByte(b));
            writer.Write(Convert.ToByte(c));
            writer.Write(Convert.ToByte(d));
        }

        public static void WriteChunk(char a, char b, char c, bool d, BinaryWriter writer)
        {
            writer.Write(Convert.ToByte(a));
            writer.Write(Convert.ToByte(b));
            writer.Write(Convert.ToByte(c));
            writer.Write(Convert.ToByte(d));
        }

        public static IList<uint> ReadIndices(uint maxIndex, uint numIndices,  BinaryReader reader, bool endianMismatch = false)
        {
            List<uint> indices = new List<uint>((int)numIndices);
            
            if (maxIndex <= 0xff)
            {
                byte[] bytes = reader.ReadBytes((int)numIndices);
                indices.AddRange(Array.ConvertAll(bytes, val => (uint)val));
            }
            else if (maxIndex <= 0xffff)
            {
                for (int i = 0; i < numIndices; i++)
                {
                    uint indexValue = ReadWord(reader, endianMismatch);
                    indices.Add(indexValue);
                }
            }
            else
            {
                for (int i = 0; i < numIndices; i++)
                {
                    uint indexValue = ReadDword(reader, endianMismatch);
                    indices.Add(indexValue);
                }
            }

            return indices;
        }

        public static void WriteIndices(IList<uint> indices, BinaryWriter writer, bool endianMismatch = false)
        {
            uint maxIndex = indices.Max();
            WriteDword(maxIndex, writer, endianMismatch);

            if (maxIndex <= 0xff)
            {
                writer.Write(indices.Select(index => (byte)index).ToArray());
            }
            else if (maxIndex <= 0xffff)
            {
                foreach(uint index in indices)
                {
                    WriteWord((ushort)index, writer, endianMismatch);
                }
            }
            else
            {
                foreach (uint index in indices)
                {
                    WriteDword(index, writer, endianMismatch);
                }
            }
        }

        public static uint GetUsedBytesByIndices(IList<uint> indices)
        {
            if (indices.Count == 0)
            {
                return 0;
            }

            uint maxIndex = indices.Max();
            uint indexStride;
            if (maxIndex <= 0xff)
            {
                indexStride = sizeof(byte);
            }
            else if (maxIndex <= 0xffff)
            {
                indexStride = sizeof(ushort);
            }
            else
            {
                indexStride = sizeof(uint);
            }

            return 4 // maxIndex
                + indexStride * (uint)indices.Count;
        }

    }
}
