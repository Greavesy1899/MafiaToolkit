using System;
using System.IO;
using SharpDX;
using Utils.Extensions;
using Utils.SharpDXExtensions;


namespace ResourceTypes.Cutscene.AnimEntities
{
    //AeSpotLight
    public class AeSpotLight : AeBase
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
        public string Name3 { get; set; }
        public float[] Unk14 { get; set; }
        public string[] Unk15 { get; set; }
        public ulong Hash4 { get; set; }
        public ulong Hash5 { get; set; }
        public string Name4 { get; set; }
        public int Unk16 { get; set; }
        public int Unk17 { get; set; }
        public int Unk18 { get; set; }
        public byte Unk19 { get; set; }
        public int Unk20 { get; set; }
        public int Unk21 { get; set; }
        public Matrix Transform1 { get; set; }
        public ulong Unk22 { get; set; }

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
            Unk08 = new float[12];
            for (int i = 0; i < 12; i++)
            {
                Unk08[i] = stream.ReadSingle(isBigEndian);
            }
            Unk11 = stream.ReadInt32(isBigEndian);
            Unk12 = stream.ReadInt32(isBigEndian);
            Name3 = stream.ReadString16(isBigEndian);
            Unk14 = new float[20];
            for (int i = 0; i < 20; i++)
            {
                Unk14[i] = stream.ReadSingle(isBigEndian);
            }
            Unk15 = new string[3];
            for (int i = 0; i < 3; i++)
            {
                Unk15[i] = stream.ReadString16(isBigEndian);
            }
            Hash4 = stream.ReadUInt64(isBigEndian);
            Hash5 = stream.ReadUInt64(isBigEndian);
            Name4 = stream.ReadString16(isBigEndian);
            Unk16 = stream.ReadInt32(isBigEndian);
            Unk17 = stream.ReadInt32(isBigEndian);
            Unk18 = stream.ReadInt32(isBigEndian);
            Unk19 = stream.ReadByte8();
            Unk20 = stream.ReadInt32(isBigEndian);
            Unk21 = stream.ReadInt32(isBigEndian);
            Transform1 = MatrixExtensions.ReadFromFile(stream, isBigEndian);
            Unk22 = stream.ReadUInt64(isBigEndian);
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
            return 3;
        }

        public override int GetEntityDataType()
        {
            throw new NotImplementedException();
        }
    }
}
