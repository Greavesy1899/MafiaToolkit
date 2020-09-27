using System;
using System.IO;
using Utils.StringHelpers;

namespace FileTypes.XBin.StreamMap
{
    public static class StreamMapCoreUtils
    {
        public static string ReadStringPtrWithOffset(BinaryReader reader)
        {
            uint offset = reader.ReadUInt32();
            return ReadStringPtr(reader, offset);
        }

        public static string ReadStringPtr(BinaryReader reader, uint offset)
        {
            long currentPosition = reader.BaseStream.Position;
            reader.BaseStream.Seek((currentPosition - 4) + offset, SeekOrigin.Begin);
            var data = StringHelpers.ReadString(reader);
            reader.BaseStream.Seek(currentPosition, SeekOrigin.Begin);
            return data;
        }

        // Attempt to auto-align. Some Commands are actually 1 byte.
        public static void AutoAlign(this BinaryReader reader, int alignment)
        {
            int remainder = 0;
            Math.DivRem((int)reader.BaseStream.Position, alignment, out remainder);

            // What is this?
            if (remainder > 0)
            {
                Console.WriteLine("Attempting to Auto-Align. {0} -> {1}", reader.BaseStream.Position, reader.BaseStream.Position + remainder);
                reader.ReadBytes(remainder);
            }
        }
    }
}
