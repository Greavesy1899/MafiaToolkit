using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mafia2
{
    public class FrameProps
    {
        public const int Signature = 1718775152;
        public int version;
        public int[] unks;
        public int[] unks2;
        public string[] unks3;

        public FrameProps(BinaryReader reader)
        {
            if (reader.ReadInt32() != Signature)
                return;

            version = reader.ReadInt32();
            unks = new int[5];
            for (int i = 0; i != 5; i++)
                unks[i] = reader.ReadInt32();

            unks2 = new int[unks[0]];
            unks3 = new string[unks[0]];

            for (int i = 0; i != unks2.Length; i++)
                unks2[i] = reader.ReadInt32();

            for (int i = 0; i != unks3.Length; i++)
                unks3[i] = new string(reader.ReadChars(unks2[i+1]));
        }
    }
}
