using System.IO;
using Utils.Helpers.Reflection;
using Utils.StringHelpers;

namespace ResourceTypes.M3.XBin.PaintCombinations
{
    public class PaintCombinationsTableItem_Elm_MDE : IPaintCombinationsTableItem_Elm
    {
        [PropertyForceAsAttribute]
        public int ColorIndex { get; set; }
        [PropertyForceAsAttribute]
        public int WheelIndex { get; set; }        // Mafia 1 DE Only
        [PropertyForceAsAttribute]
        public int Weight { get; set; }        // Mafia 1 DE Only
        [PropertyForceAsAttribute]
        public string Painting { get; set; }        // Mafia 1 DE Only

        public PaintCombinationsTableItem_Elm_MDE()
        {
            Painting = string.Empty;
        }

        public void ReadEntry(BinaryReader Reader)
        {
            ColorIndex = Reader.ReadInt32();
            WheelIndex = Reader.ReadInt32();
            Painting = StringHelpers.ReadStringBuffer(Reader, 32).Trim('\0');
            Weight = Reader.ReadInt32();
        }

        public void WriteEntry(XBinWriter Writer)
        {
            Writer.Write(ColorIndex);
            Writer.Write(WheelIndex);
            StringHelpers.WriteStringBuffer(Writer, 32, Painting);
            Writer.Write(Weight);
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3}", ColorIndex, WheelIndex, Weight, Painting);
        }
    }
}
