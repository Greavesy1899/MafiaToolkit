using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gibbed.IO;

namespace ResourceTypes.Collisions.Opcode
{
    class SerializationUtils
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

        public static ushort ReadWord(BinaryReader reader, bool endianMismatch = false)
        {
            ushort value = reader.ReadUInt16();
            if (endianMismatch)
            {
                value = value.Swap();
            }
            return value;
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

        public static float ReadFloat(BinaryReader reader, bool endianMismatch = false)
        {
            float value = reader.ReadSingle();
            if (endianMismatch)
            {
                value = value.Swap();
            }
            return value;
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


    }
}
