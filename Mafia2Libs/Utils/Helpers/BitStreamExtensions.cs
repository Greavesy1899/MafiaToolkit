using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BitStreams;

namespace Mafia2Tool.Utils.Helpers
{
    internal static class BitStreamExtensions
    {
        public static float ReadSingleUnorm(this BitStream stream, int size)
        {
            System.Diagnostics.Debug.Assert(size > 0 && size <= 32);
            var maxValue = (float)((1u << size) - 1);
            var bytes = stream.ReadBytes(size);
            var singlei = BitConverter.ToInt32(bytes);
            return singlei / maxValue;
        }
    }
}
