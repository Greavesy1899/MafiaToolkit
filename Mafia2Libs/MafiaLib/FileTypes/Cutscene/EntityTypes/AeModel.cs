using System.Diagnostics;
using System.IO;
using SharpDX;
using Utils.Extensions;
using Utils.SharpDXExtensions;

namespace ResourceTypes.Cutscene.AnimEntities
{
    public class AeModel : AeBase
    {
        public byte Unk06 { get; set; }
        public int Unk07 { get; set; }
        public int Unk08 { get; set; }
        public Matrix Transform { get; set; }
        public string Name4 { get; set; }

        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            Unk06 = stream.ReadByte8();
            Unk07 = stream.ReadInt32(isBigEndian);
            Unk08 = stream.ReadInt32(isBigEndian);
            Transform = MatrixExtensions.ReadFromFile(stream, isBigEndian);
            Name4 = stream.ReadString16(isBigEndian);
        }
    }

    public class AeModelData : AeBaseData
    {
        public int Unk02 { get; set; } // 0
        public Vector3 Unk03 { get; set; } // 0.2f, 0.2f, 0.2f
        public float Unk04 { get; set; } // 7.0f
        public byte[] Unk05 { get; set; } // For me, this was all empty
        public float Unk06 { get; set; } // 1.0f;
        public int Unk07 { get; set; }
        public byte Unk08 { get; set; }
        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            Debug.Assert(stream.Position != stream.Length, "I've read the parent class data, although i've hit the eof!");

            Unk02 = stream.ReadInt32(isBigEndian);
            Unk03 = Vector3Extenders.ReadFromFile(stream, isBigEndian);
            Unk04 = stream.ReadSingle(isBigEndian);
            Unk05 = stream.ReadBytes(24);
            Unk06 = stream.ReadSingle(isBigEndian);
            Unk07 = stream.ReadInt32(isBigEndian);
            Unk08 = stream.ReadByte8();
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);
            stream.Write(Unk02, isBigEndian);
            Unk03.WriteToFile(stream, isBigEndian);
            stream.Write(Unk04, isBigEndian);
            stream.Write(Unk05);
            stream.Write(Unk06, isBigEndian);
            stream.Write(Unk07, isBigEndian);
            stream.Write(Unk07, isBigEndian);
        }
    }
}
