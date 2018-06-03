using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Mafia2
{
    public class FrameObjectArea : FrameObjectBase
    {
        int unk01;
        int unk02;
        byte[] unkBytes;

        public FrameObjectArea(BinaryReader reader) : base()
        {
            ReadFromFile(reader);
        }

        public override void ReadFromFile(BinaryReader reader)
        {
            base.ReadFromFile(reader);
            unk01 = reader.ReadInt32();
            unk02 = reader.ReadInt32();
            //NEED TO DECODE THIS, JUST SKIPPING ALL THE BYTES
            if (unk02 == 1536)
                unkBytes = reader.ReadBytes(121);
            else if (unk02 == 1792)
                unkBytes = reader.ReadBytes(137);
            else if(unk02 == 2816)
                unkBytes = reader.ReadBytes(201);
            else if(unk02 == 2048)
                unkBytes = reader.ReadBytes(153);
            else if(unk02 == 1280)
                unkBytes = reader.ReadBytes(105);
            else if (unk02 == 2560)
                unkBytes = reader.ReadBytes(185);
            else if (unk02 == 2304)
                unkBytes = reader.ReadBytes(169);
            else
                throw new Exception("Error");
        }
    }
}
