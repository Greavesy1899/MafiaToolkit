using System;
using System.IO;
using SharpDX;
using Utils.Extensions;
using Utils.SharpDXExtensions;

namespace ResourceTypes.Cutscene.AnimEntities
{
    //AeOmniLight
    public class AeOmniLight : AeBase
    {
        public short Unk0 { get; set; }
        public string Name0 { get; set; }
        public string Name1 { get; set; }
        public byte Unk01 { get; set; }
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
        public int Unk09 { get; set; }
        public int Unk10 { get; set; }
        public float[] Unk08 { get; set; }
        public int Unk11 { get; set; }
        public int Unk12 { get; set; }
        public short Unk13 { get; set; }
        public override bool ReadDefinitionFromFile(MemoryStream stream, bool isBigEndian)
        {
            Unk0 = stream.ReadInt16(isBigEndian);
            Name0 = stream.ReadString16(isBigEndian);
            Name1 = stream.ReadString16(isBigEndian);
            if (!string.IsNullOrEmpty(Name1))
            {
                Unk01 = stream.ReadByte8();
            }
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
            Unk09 = stream.ReadInt32(isBigEndian);
            Unk10 = stream.ReadInt32(isBigEndian);
            Unk08 = new float[10];
            for (int i = 0; i < 10; i++)
            {
                Unk08[i] = stream.ReadSingle(isBigEndian);
            }
            Unk11 = stream.ReadInt32(isBigEndian);
            Unk12 = stream.ReadInt32(isBigEndian);
            Unk13 = stream.ReadInt16(isBigEndian);
            return true;
        }

        public override bool WriteDefinitionToFile(MemoryStream writer, bool isBigEndian)
        {
            throw new NotImplementedException();
        }

        public override bool ReadDataFromFile(MemoryStream stream, bool isBigEndian)
        {
            EntityData = new AeOmniLightData();
            EntityData.ReadFromFile(stream, isBigEndian);
            return true;
        }

        public override bool WriteDataFromFile(MemoryStream stream, bool isBigEndian)
        {
            throw new NotImplementedException();
        }

        public override int GetEntityDefinitionType()
        {
            return 2;
        }

        public override int GetEntityDataType()
        {
            throw new NotImplementedException();
        }
    }

    public class AeOmniLightData : AeBaseData
    {
        public byte Unk02 { get; set; }
        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            Unk02 = stream.ReadByte8();
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);
            stream.Write(Unk02, isBigEndian);
        }
    }
}
