using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Types;

namespace ResourceTypes.Cutscene
{
    public class GCSCamera
    {
        public string Name;
        public int Unk0;
        public int Unk1;
        public int UnkType;
        public byte[] Padding;
        public Matrix Matrix;
        public float Unk2;
        public float Unk3;
        public float Unk4;
        //public int Unk0;
        //public int Unk1;
        //public int UnkType;
        //public byte[] Padding;
        //public TransformMatrix Matrix;
    }
}
