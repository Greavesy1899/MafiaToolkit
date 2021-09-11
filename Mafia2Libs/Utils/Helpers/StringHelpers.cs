using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Utils.StringHelpers
{
    public static class StringHelpers
    {
        public static string ReadString8(this BinaryReader reader)
        {
            byte size = reader.ReadByte();
            return new string(reader.ReadChars(size));
        }
        public static string ReadString16(this BinaryReader reader)
        {
            short size = reader.ReadInt16();
            return new string(reader.ReadChars(size));
        }
        public static string ReadString32(BinaryReader reader)
        {
            int size = reader.ReadInt32();
            return new string(reader.ReadChars(size));
        }
        public static string ReadString64(this BinaryReader reader)
        {
            long size = reader.ReadInt64();
            return new string(reader.ReadChars((int)size));
        }
        public static string ReadString(BinaryReader reader)
        {
            string newString = "";

            while (reader.PeekChar() != '\0')
            {
                newString += reader.ReadChar();
            }
            reader.ReadByte();
            return newString;
        }

        public static string ReadStringEncoded(BinaryReader reader)
        {
            List<byte> StringBytes = new List<byte>();

            while (reader.PeekChar() != '\0')
            {
                StringBytes.Add(reader.ReadByte());
            }
            reader.ReadByte();
            return Encoding.UTF8.GetString(StringBytes.ToArray());
        }

        public static string ReadStringBuffer(BinaryReader reader, int size)
        {
            string Result = new string(reader.ReadChars(size));
            return Result.Trim('\0');
        }

        public static string ReadUniNullTerminatedString(BinaryReader br)
        {
            List<byte> stringC = new List<byte>();
            byte newByte = br.ReadByte();
            byte newByteNull = br.ReadByte();
            while (newByte != 0)
            {
                stringC.Add(newByte);
                newByte = br.ReadByte();
                newByteNull = br.ReadByte();
            }
            return Encoding.ASCII.GetString(stringC.ToArray());
        }

        public static void WriteUniNullTerminatedString(BinaryWriter bw, string str)
        {
            bw.Write(Encoding.Unicode.GetBytes(str.ToCharArray()));
            bw.Write((short)0);
        }

        public static void WriteStringBuffer(BinaryWriter writer, int size, string text, char trim = ' ', Encoding encoding = null)
        {
            bool addTrim = (trim == ' ' ? false : true);
            int padding = size - text.Length;
            var data = encoding == null ? Encoding.ASCII.GetBytes(text) : encoding.GetBytes(text);
            writer.Write(data);

            if (addTrim)
            {
                writer.Write('\0');
                padding -= 1;
            }

            writer.Write(new byte[padding]);
        }
        public static void WriteString(BinaryWriter writer, string text, bool trail = true)
        {
            writer.Write(text.ToCharArray());

            if(trail)
                writer.Write('\0');
        }
        public static void WriteString8(this BinaryWriter writer, string text)
        {
            writer.Write((byte)text.Length);
            writer.Write(text.ToCharArray());
        }
        public static void WriteString16(this BinaryWriter writer, string text)
        {
            writer.Write((ushort)text.Length);
            writer.Write(text.ToCharArray());
        }
        public static void WriteString32(this BinaryWriter writer, string text)
        {
            writer.Write(text.Length);
            writer.Write(text.ToCharArray());
        }
    }

}
