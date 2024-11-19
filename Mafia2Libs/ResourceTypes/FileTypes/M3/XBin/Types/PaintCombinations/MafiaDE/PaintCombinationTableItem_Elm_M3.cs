using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Helpers.Reflection;
using Utils.StringHelpers;

namespace ResourceTypes.M3.XBin.PaintCombinations
{
    public class PaintCombinationsTableItem_Elm_M3 : IPaintCombinationsTableItem_Elm
    {
        [PropertyForceAsAttribute]
        public int ColorIndex { get; set; }

        public void ReadEntry(BinaryReader Reader)
        {
            ColorIndex = Reader.ReadInt32();
        }

        public void WriteEntry(XBinWriter Writer)
        {
            Writer.Write(ColorIndex);
        }

        public override string ToString()
        {
            return string.Format("{0}", ColorIndex);
        }
    }
}
