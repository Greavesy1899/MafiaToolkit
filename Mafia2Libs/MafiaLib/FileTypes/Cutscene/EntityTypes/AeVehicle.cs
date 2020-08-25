using System.IO;
using SharpDX;
using Utils.Extensions;
using Utils.SharpDXExtensions;

namespace ResourceTypes.Cutscene.AnimEntities
{
    public class AeVehicle : AeBase
    {
        public int Unk05 { get; set; }
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
}
