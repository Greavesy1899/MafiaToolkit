using System.IO;
using Toolkit.Mathematics;
using Utils.Extensions;

namespace ResourceTypes.Cutscene.AnimEntities
{
    public class AeSpotLightTarget : AnimEntity
    {
        public byte Unk19 { get; set; }
        public ulong Unk20 { get; set; }
        public ulong Unk21 { get; set; }
        public Matrix44 Transform { get; set; } = new();
        public ulong Unk22 { get; set; }

        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            Unk19 = stream.ReadByte8();
            Unk20 = stream.ReadUInt64(isBigEndian);

            if (Unk20 != 0)
            {
                Unk21 = stream.ReadUInt64(isBigEndian);
            }

            Transform.ReadFromFile(stream, isBigEndian);
            Unk22 = stream.ReadUInt64(isBigEndian);
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);
            stream.WriteByte(Unk19);
            stream.Write(Unk20, isBigEndian);

            if (Unk20 != 0)
            {
                stream.Write(Unk21, isBigEndian);
            }
            
            Transform.WriteToFile(stream, isBigEndian);
            stream.Write(Unk22, isBigEndian);
            UpdateSize(stream, isBigEndian);
        }
    }
}
