using System;
using System.IO;
using System.Text;

namespace Utils.Extensions
{
    //basically BinaryReader but i can use for memory streams, this will soon be used for the entire toolkit, so big endian can be supported.
    //Some may think (why not use gibbed?) my answer is: idk.
    public static class StreamHelpers
    {
        public static char[] ReadChars(this Stream stream, int size)
        {
            char[] data = new char[size];
            for (int i = 0; i < size; i++)
                data[i] = stream.ReadChar();
            return data;
        }
        public static char ReadChar(this Stream stream)
        {
            char c = (char)stream.ReadByte();
            return c;
        }
        public static string ReadStringBuffer(this Stream stream, int size)
        {
            return new string(stream.ReadChars(size));
        }
        public static byte[] ReadBytes(this Stream stream, int num)
        {
            byte[] data = new byte[num];
            for (int i = 0; i < num; i++)
                data[i] = (byte)stream.ReadByte();
            return data;
        }
        public static byte ReadByte8(this Stream stream)
        {
            return (byte)stream.ReadByte();
        }
        public static float ReadSingle(this Stream stream, bool bigEndian)
        {
            byte[] data = new byte[4];
            stream.Read(data, 0, 4);
            if (bigEndian) Array.Reverse(data);
            return BitConverter.ToSingle(data, 0);
        }
        public static double ReadDouble(this Stream stream, bool bigEndian)
        {
            byte[] data = new byte[8];
            stream.Read(data, 0, 8);
            if (bigEndian) Array.Reverse(data);
            return BitConverter.ToDouble(data, 0);
        }
        public static int ReadInt32(this Stream stream, bool bigEndian)
        {
            byte[] data = new byte[sizeof(int)];
            stream.Read(data, 0, 4);
            if (bigEndian) Array.Reverse(data);
            return BitConverter.ToInt32(data, 0);
        }
        public static uint ReadUInt32(this Stream stream, bool bigEndian)
        {
            byte[] data = new byte[sizeof(int)];
            stream.Read(data, 0, 4);
            if (bigEndian) Array.Reverse(data);
            return BitConverter.ToUInt32(data, 0);
        }
        public static short ReadInt16(this Stream stream, bool bigEndian)
        {
            byte[] data = new byte[sizeof(short)];
            stream.Read(data, 0, 2);
            if (bigEndian) Array.Reverse(data);
            return BitConverter.ToInt16(data, 0);
        }
        public static ushort ReadUInt16(this Stream stream, bool bigEndian)
        {
            byte[] data = new byte[sizeof(short)];
            stream.Read(data, 0, 2);
            if (bigEndian) Array.Reverse(data);
            return BitConverter.ToUInt16(data, 0);
        }
        public static ulong ReadUInt64(this Stream stream, bool bigEndian)
        {
            byte[] data = new byte[sizeof(long)];
            stream.Read(data, 0, 8);
            if (bigEndian) Array.Reverse(data);
            return BitConverter.ToUInt64(data, 0);
        }
        public static long ReadInt64(this Stream stream, bool bigEndian)
        {
            byte[] data = new byte[sizeof(long)];
            stream.Read(data, 0, 8);
            if (bigEndian) Array.Reverse(data);
            return BitConverter.ToInt64(data, 0);
        }

        public static void Write(this Stream stream, byte[] data)
        {
            stream.Write(data, 0, data.Length);
        }
        public static void Write(this Stream stream, char[] data)
        {
            for (int i = 0; i < data.Length; i++)
                stream.Write(data[i]);
        }
        public static void Write(this Stream stream, char value)
        {
            stream.Write(BitConverter.GetBytes(value), 0, 1);
        }
        public static void WriteStringBuffer(this Stream writer, int size, string text, char trim = ' ', Encoding encoding = null)
        {
            bool addTrim = (trim == ' ' ? false : true);
            int padding = size - text.Length;
            var data = encoding == null ? Encoding.ASCII.GetBytes(text) : encoding.GetBytes(text);
            writer.Write(data);

            if (addTrim && padding > 0)
            {
                writer.Write('\0');
                padding -= 1;
            }

            writer.Write(new byte[padding]);
        }
        public static void Write(this Stream stream, float value, bool bigEndian)
        {
            byte[] data = BitConverter.GetBytes(value);
            if (bigEndian) Array.Reverse(data);
            stream.Write(data);
        }
        public static void Write(this Stream stream, double value, bool bigEndian)
        {
            byte[] data = BitConverter.GetBytes(value);
            if (bigEndian) Array.Reverse(data);
            stream.Write(data);
        }
        public static void Write(this Stream stream, int value, bool bigEndian)
        {
            byte[] data = BitConverter.GetBytes(value);
            if (bigEndian) Array.Reverse(data);
            stream.Write(data);
        }
        public static void Write(this Stream stream, uint value, bool bigEndian)
        {
            byte[] data = BitConverter.GetBytes(value);
            if (bigEndian) Array.Reverse(data);
            stream.Write(data);
        }
        public static void Write(this Stream stream, short value, bool bigEndian)
        {
            byte[] data = BitConverter.GetBytes(value);
            if (bigEndian) Array.Reverse(data);
            stream.Write(data);
        }
        public static void Write(this Stream stream, ushort value, bool bigEndian)
        {
            byte[] data = BitConverter.GetBytes(value);
            if (bigEndian) Array.Reverse(data);
            stream.Write(data);
        }
        public static void Write(this Stream stream, long value, bool bigEndian)
        {
            byte[] data = BitConverter.GetBytes(value);
            if (bigEndian) Array.Reverse(data);
            stream.Write(data);
        }
        public static void Write(this Stream stream, ulong value, bool bigEndian)
        {
            byte[] data = BitConverter.GetBytes(value);
            if (bigEndian) Array.Reverse(data);
            stream.Write(data);
        }
    }
}
