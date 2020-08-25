using System;
using System.IO;
using SharpDX;
using Utils.Extensions;
using Utils.SharpDXExtensions;

namespace ResourceTypes.Cutscene.AnimEntities
{
    public class AeTargetCamera : AeBase
    {
        public byte Unk05 { get; set; }
        public int Unk06 { get; set; }
        public int Unk07 { get; set; }
        public Matrix Transform { get; set; }
        public float Unk08 { get; set; }
        public float Unk09 { get; set; }
        public float Unk10 { get; set; }
        public ulong Hash4 { get; set; }
        public ulong Hash5 { get; set; }
        public string Name33 { get; set; }
        public int Unk11 { get; set; }
        public int Unk12 { get; set; }
        public int Unk13 { get; set; }
        public byte Unk14 { get; set; }
        public int Unk15 { get; set; }
        public int Unk16 { get; set; }
        public ulong Hash6 { get; set; }
        public ulong Hash7 { get; set; }
        public Matrix Transform1 { get; set; }

        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            Unk05 = stream.ReadByte8();
            Unk06 = stream.ReadInt32(isBigEndian);
            Unk07 = stream.ReadInt32(isBigEndian);
            Transform = MatrixExtensions.ReadFromFile(stream, isBigEndian);
            Unk08 = stream.ReadSingle(isBigEndian);
            Unk09 = stream.ReadSingle(isBigEndian);
            Unk10 = stream.ReadSingle(isBigEndian);
            Hash4 = stream.ReadUInt64(isBigEndian);
            Hash5 = stream.ReadUInt64(isBigEndian);
            Name33 = stream.ReadString16(isBigEndian);
            Unk11 = stream.ReadInt32(isBigEndian);
            Unk12 = stream.ReadInt32(isBigEndian);
            Unk13 = stream.ReadInt32(isBigEndian);
            Unk14 = stream.ReadByte8();
            Hash6 = stream.ReadUInt64(isBigEndian);

            if (Hash6 != 0)
            {
                Hash7 = stream.ReadUInt64(isBigEndian);
                Transform1 = MatrixExtensions.ReadFromFile(stream, isBigEndian);
            }
            else
            {
                Transform1 = MatrixExtensions.ReadFromFile(stream, isBigEndian);
                Hash7 = stream.ReadUInt64(isBigEndian);
            }
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            throw new NotImplementedException();
        }
    }

    public class AeTargetCameraData : AeBaseData
    {
        public int Unk02 { get; set; }
        public byte Unk03 { get; set; }
        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            Unk02 = stream.ReadInt32(isBigEndian);
            Unk03 = stream.ReadByte8();
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);
            stream.Write(Unk02, isBigEndian);
            stream.Write(Unk03, isBigEndian);
        }
    }
}
