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
            var data = StringHelpers.ReadString(reader);
            reader.BaseStream.Seek(currentPosition, SeekOrigin.Begin);
            return data;
        }
    }
}
