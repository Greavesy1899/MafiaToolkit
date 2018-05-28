using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Mafia2
{
    public class FrameObjectCamera : FrameObjectBase
    {
        byte[] unkBytes;

        public byte[] UnkBytes {
            get { return unkBytes; }
            set { unkBytes = value; }
        }
        
        public FrameObjectCamera(BinaryReader reader) : base()
        {
            ReadFromFile(reader);
        }
        public override void ReadFromFile(BinaryReader reader)
        {
            base.ReadFromFile(reader);
            unkBytes = reader.ReadBytes(5);
        }

        public override string ToString()
        {
            return string.Format("Camera Block");
        }
    }
}
