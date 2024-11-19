using System.IO;

namespace ResourceTypes.M3.XBin.PaintCombinations
{
    public interface IPaintCombinationsTableItem_Elm
    {
        void ReadEntry(BinaryReader Reader);
        void WriteEntry(XBinWriter Writer);
    }
}
