using System.IO;
using Utils.StringHelpers;

namespace ResourceTypes.M3.XBin
{
    public static class XBinCoreUtils
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
            var data = StringHelpers.ReadStringEncoded(reader);
            reader.BaseStream.Seek(currentPosition, SeekOrigin.Begin);
            return data;
        }

        public static void GotoPtrWithOffset(BinaryReader reader)
        {
            uint offset = reader.ReadUInt32();
            GotoPtr(reader, offset);
        }

        public static void GotoPtr(BinaryReader reader, uint offset)
        {
            long currentPosition = reader.BaseStream.Position;
            reader.BaseStream.Seek((currentPosition - 4) + offset, SeekOrigin.Begin);
        }
    }
}
