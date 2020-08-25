using System;
using System.IO;
using SharpDX;
using Utils.Extensions;
using Utils.SharpDXExtensions;

namespace ResourceTypes.Cutscene.AnimEntities
{
    public class AeFrame : AeBase
    {
        public byte Unk06 { get; set; }
        public ulong Hash2 { get; set; }
        public Matrix Transform { get; set; }
        public float Unk07 { get; set; }
        public float Unk08 { get; set; }
        public Matrix Transform1 { get; set; }
        public ulong Hash3 { get; set; }

        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            File.WriteAllBytes("frame.bin", stream.ToArray());
            base.ReadFromFile(stream, isBigEndian);
            Unk06 = stream.ReadByte8();
            Hash2 = stream.ReadUInt64(isBigEndian);
            Hash3 = stream.ReadUInt64(isBigEndian);
            Transform = MatrixExtensions.ReadFromFile(stream, isBigEndian);

            //if(Unk05 == 121)
            //{

            //}
            //Transform = MatrixExtensions.ReadFromFile(stream, isBigEndian);
            //Unk07 = stream.ReadSingle(isBigEndian);
            //Unk08 = stream.ReadSingle(isBigEndian);
            //Transform1 = MatrixExtensions.ReadFromFile(stream, isBigEndian);
        }
    }

    public class AeFrameData : AeBaseData
    {
        public int Unk02 { get; set; }
        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            Unk02 = stream.ReadInt32(isBigEndian);
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);
            stream.Write(Unk02, isBigEndian);
        }
    }
}
