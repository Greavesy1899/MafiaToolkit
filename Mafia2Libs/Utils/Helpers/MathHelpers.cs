using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.MathHelpers
{
    public static class MathHelpers
    {
        public static int RoundUp16(int x, bool offset)
        {
            int roundUp = ((x + 15) & (-16));

            if ((roundUp == x) && offset)
            {
                roundUp += 16;
            }

            return roundUp;
        }

        public static bool GetBit(this byte b, int bitNumber)
        {
            return (b & (1 << bitNumber)) != 0;
        }
    }
}
