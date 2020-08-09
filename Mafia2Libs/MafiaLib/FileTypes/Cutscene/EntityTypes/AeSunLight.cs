using System;
using System.IO;
using SharpDX;
using Utils.Extensions;
using Utils.SharpDXExtensions;

namespace ResourceTypes.Cutscene.AnimEntities
{
    //AeSunLight
    public class AeSunLight : AeBase
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
        public int Unk09 { get; set; }
        public int Unk10 { get; set; }
        public float[] Unk08 { get; set; }
        public int Unk11 { get; set; }
        public int Unk12 { get; set; }
        public string Name3 { get; set; }
        public float[] Unk14 { get; set; }
        public byte Unk15 { get; set; }
        public float[] Unk16 { get; set; }

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
            Unk09 = stream.ReadInt32(isBigEndian);
            Unk10 = stream.ReadInt32(isBigEndian);
            Unk08 = new float[10];
            for (int i = 0; i < 10; i++)
            {
                Unk08[i] = stream.ReadSingle(isBigEndian);
            }
            Unk11 = stream.ReadInt32(isBigEndian);
            Unk12 = stream.ReadInt32(isBigEndian);
            Name3 = stream.ReadString16(isBigEndian);
            Unk14 = new float[9];
            for (int i = 0; i < 9; i++)
            {
                Unk14[i] = stream.ReadSingle(isBigEndian);
            }
            Unk15 = stream.ReadByte8();
            Unk16 = new float[12];
            for (int i = 0; i < 12; i++)
            {
                Unk16[i] = stream.ReadSingle(isBigEndian);
            }
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
            return 29;
        }

        public override int GetEntityDataType()
        {
            throw new NotImplementedException();
        }
    }
}
