using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Helpers.Reflection;

namespace ResourceTypes.M3.XBin.PaintCombinations
{
    public interface IPaintCombinationsTableItem_Elm
    {
        void ReadEntry(BinaryReader Reader);
        void WriteEntry(XBinWriter Writer);
    }
}
