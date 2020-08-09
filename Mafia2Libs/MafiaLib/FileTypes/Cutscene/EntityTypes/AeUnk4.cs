using System;
using System.IO;
using SharpDX;
using Utils.Extensions;
using Utils.SharpDXExtensions;

namespace ResourceTypes.Cutscene.AnimEntities
{
    //AeCameraLink
    public class AeUnk4 : AeBase
    {
        public int Unk01 { get; set; }
        public string Name1 { get; set; }
        public ulong Hash1 { get; set; }
        public ulong Hash2 { get; set; }
        public string Name2 { get; set; }
        public int Unk02 { get; set; }
        public int Unk03 { get; set; }
        public int Unk04 { get; set; }
        public byte Unk05 { get; set; }
        public int Unk06 { get; set; }
        public int Unk07 { get; set; }
        public Matrix Transform { get; set; }
        public float Unk08 { get; set; }
        public float Unk09 { get; set; }
        public float Unk10 { get; set; }

        public override bool ReadDefinitionFromFile(MemoryStream stream, bool isBigEndian)
        {
            Unk01 = stream.ReadInt32(isBigEndian);
            Name1 = stream.ReadString16(isBigEndian);
            Hash1 = stream.ReadUInt64(isBigEndian);
            Hash2 = stream.ReadUInt64(isBigEndian);
            Name2 = stream.ReadString16(isBigEndian);
            Unk02 = stream.ReadInt32(isBigEndian);
            Unk03 = stream.ReadInt32(isBigEndian);
            Unk04 = stream.ReadInt32(isBigEndian);
            Unk05 = stream.ReadByte8();
            Unk06 = stream.ReadInt32(isBigEndian);
            Unk07 = stream.ReadInt32(isBigEndian);
            Transform = MatrixExtensions.ReadFromFile(stream, isBigEndian);
            Unk08 = stream.ReadSingle(isBigEndian);
            Unk09 = stream.ReadSingle(isBigEndian);
            Unk10 = stream.ReadSingle(isBigEndian);
            return true;
        }

        public override bool WriteDefinitionToFile(MemoryStream writer, bool isBigEndian)
        {
            throw new NotImplementedException();
        }

        public override bool ReadDataFromFile(MemoryStream stream, bool isBigEndian)
        {
            EntityData = new AeBaseData();
            EntityData.ReadFromFile(stream, isBigEndian);
            return true;
        }

        public override bool WriteDataFromFile(MemoryStream stream, bool isBigEndian)
        {
            throw new NotImplementedException();
        }

        public override int GetEntityDefinitionType()
        {
            return 4;
        }

        public override int GetEntityDataType()
        {
            throw new NotImplementedException();
        }
    }
}
