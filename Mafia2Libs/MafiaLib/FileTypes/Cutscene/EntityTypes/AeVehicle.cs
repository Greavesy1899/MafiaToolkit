using System;
using System.IO;
using SharpDX;
using Utils.Extensions;
using Utils.SharpDXExtensions;

namespace ResourceTypes.Cutscene.AnimEntities
{
    public class AeVehicle : AeBase
    {
        public short Unk01 { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public byte Unk02 { get; set; }
        public ulong Hash0 { get; set; }
        public ulong Hash1 { get; set; }
        public string Name3 { get; set; }
        public int Unk03 { get; set; }
        public int Unk04 { get; set; }
        public int Unk05 { get; set; }
        public byte Unk06 { get; set; }
        public int Unk07 { get; set; }
        public int Unk08 { get; set; }
        public Matrix Transform { get; set; }
        public string Name4 { get; set; }

        public override bool ReadDefinitionFromFile(MemoryStream stream, bool isBigEndian)
        {
            Unk01 = stream.ReadInt16(isBigEndian);
            Name1 = stream.ReadString16(isBigEndian);
            Name2 = stream.ReadString16(isBigEndian);
            Unk02 = stream.ReadByte8();
            Hash0 = stream.ReadUInt64(isBigEndian);
            Hash1 = stream.ReadUInt64(isBigEndian);
            Name3 = stream.ReadString16(isBigEndian);
            Unk03 = stream.ReadInt32(isBigEndian);
            Unk04 = stream.ReadInt32(isBigEndian);
            Unk05 = stream.ReadInt32(isBigEndian);
            Unk06 = stream.ReadByte8();
            Unk07 = stream.ReadInt32(isBigEndian);
            Unk08 = stream.ReadInt32(isBigEndian);
            Transform = MatrixExtensions.ReadFromFile(stream, isBigEndian);
            Name4 = stream.ReadString16(isBigEndian);
            return true;
        }

        public override bool WriteDefinitionToFile(MemoryStream stream, bool isBigEndian)
        {
            throw new NotImplementedException();
            return true;
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
            return 14;
        }

        public override int GetEntityDataType()
        {
            throw new NotImplementedException();
        }
    }
}
