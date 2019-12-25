using SharpDX;
using System;
using System.IO;
using Utils.SharpDXExtensions;
using Utils.Extensions;
using Utils.Types;

namespace ResourceTypes.Cutscene
{
    public interface IAeEntity
    {
        bool ReadFromFile(MemoryStream stream, bool isBigEndian);
        bool WriteToFile(MemoryStream writer, bool isBigEndian);
    }

    //AeOmniLight
    public class AeOmniLight : IAeEntity
    {
        public const int Type = 2;

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
        public short Unk13 { get; set; }
        public bool ReadFromFile(MemoryStream stream, bool isBigEndian)
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
            Unk08 = new float[12];
            for(int i = 0; i < 12; i++)
            {
                Unk08[i] = stream.ReadSingle(isBigEndian);
            }
            Unk11 = stream.ReadInt32(isBigEndian);
            Unk12 = stream.ReadInt32(isBigEndian);
            Unk13 = stream.ReadInt16(isBigEndian);
            return true;
        }

        public bool WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            throw new NotImplementedException();
        }
    }

    //AeSpotLight
    public class AeSpotLight : IAeEntity
    {
        public const int Type = 3;

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
        public short Unk13 { get; set; }
        public bool ReadFromFile(MemoryStream stream, bool isBigEndian)
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
            Unk08 = new float[12];
            for (int i = 0; i < 12; i++)
            {
                Unk08[i] = stream.ReadSingle(isBigEndian);
            }
            Unk11 = stream.ReadInt32(isBigEndian);
            Unk12 = stream.ReadInt32(isBigEndian);
            Unk13 = stream.ReadInt16(isBigEndian);
            return true;
        }

        public bool WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            throw new NotImplementedException();
        }
    }
    //AeCameraLink
    public class AeUnk4 : IAeEntity
    {
        public const int Type = 4;

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

        public bool ReadFromFile(MemoryStream stream, bool isBigEndian)
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

        public bool WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            throw new NotImplementedException();
        }
    }

    public class AeTargetCamera : IAeEntity
    {
        public const int Type = 5;

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
        public ulong Hash3 { get; set; }
        public float Unk08 { get; set; }
        public ulong Hash4 { get; set; }
        public ulong Hash5 { get; set; }
        public string Name3 { get; set; }
        public int Unk09 { get; set; }
        public int Unk10 { get; set; }
        public int Unk11 { get; set; }
        public byte Unk12 { get; set; }
        public int Unk13 { get; set; }
        public int Unk14 { get; set; }
        public Matrix Transform1 { get; set; }
        public ulong Hash6 { get; set; }
        public bool ReadFromFile(MemoryStream stream, bool isBigEndian)
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
            Hash3 = stream.ReadUInt64(isBigEndian);
            Unk08 = stream.ReadSingle(isBigEndian);
            Hash4 = stream.ReadUInt64(isBigEndian);
            Hash5 = stream.ReadUInt64(isBigEndian);
            Name3 = stream.ReadString16(isBigEndian);
            Unk09 = stream.ReadInt32(isBigEndian);
            Unk10 = stream.ReadInt32(isBigEndian);
            Unk11 = stream.ReadInt32(isBigEndian);
            Unk12 = stream.ReadByte8();
            Unk13 = stream.ReadInt32(isBigEndian);
            Unk14 = stream.ReadInt32(isBigEndian);
            Transform1 = MatrixExtensions.ReadFromFile(stream, isBigEndian);
            Hash6 = stream.ReadUInt64(isBigEndian);
            return true;
        }

        public bool WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            throw new NotImplementedException();
        }
    }

    public class AeModel : IAeEntity
    {
        public const int Type = 6;

        public int Unk01 { get; set; }
        public string Name1 { get; set; }
        public ulong Hash0 { get; set; }
        public ulong Hash1 { get; set; }
        public string Name2 { get; set; }
        public int Unk02 { get; set; }
        public int Unk03 { get; set; }
        public int Unk04 { get; set; }
        public byte Unk05 { get; set; }
        public int Unk06 { get; set; }
        public int Unk07 { get; set; }
        public Matrix Transform { get; set; }
        public string Name3 { get; set; }

        public AeModel()
        {

        }

        public bool ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            Unk01 = stream.ReadInt32(isBigEndian);
            Name1 = stream.ReadString16(isBigEndian);
            Hash0 = stream.ReadUInt64(isBigEndian);
            Hash1 = stream.ReadUInt64(isBigEndian);
            Name2 = stream.ReadString16(isBigEndian);
            Unk02 = stream.ReadInt32(isBigEndian);
            Unk03 = stream.ReadInt32(isBigEndian);
            Unk04 = stream.ReadInt32(isBigEndian);
            Unk05 = stream.ReadByte8();
            Unk06 = stream.ReadInt32(isBigEndian);
            Unk07 = stream.ReadInt32(isBigEndian);
            Transform = MatrixExtensions.ReadFromFile(stream, isBigEndian);
            Name3 = stream.ReadString16(isBigEndian);
            return true;
        }

        public bool WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            throw new NotImplementedException();
            return true;
        }
    }

    public class AeUnk10 : IAeEntity
    {
        public const int Type = 10;

        public short isValid {get;set;}

        public bool ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            isValid = stream.ReadInt16(isBigEndian);
            return true;
        }

        public bool WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            throw new NotImplementedException();
        }
    }

    public class AeUnk12 : IAeEntity
    {
        public const int Type = 12;

        public short isValid { get; set; }

        public bool ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            isValid = stream.ReadInt16(isBigEndian);
            return true;
        }

        public bool WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            throw new NotImplementedException();
        }
    }

    public class AeUnk13 : IAeEntity
    {
        public const int Type = 13;

        public short isValid { get; set; }

        public bool ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            isValid = stream.ReadInt16(isBigEndian);
            return true;
        }

        public bool WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            throw new NotImplementedException();
        }
    }

    public class AeUnk18 : IAeEntity
    {
        public const int Type = 18;

        public short isValid { get; set; }

        public bool ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            isValid = stream.ReadInt16(isBigEndian);
            return true;
        }

        public bool WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            throw new NotImplementedException();
        }
    }

    public class AeFrame : IAeEntity
    {
        public const int Type = 22;

        public short Unk01 { get; set; }
        public string Name1 { get; set; }
        public byte Unk02 { get; set; }
        public ulong Hash0 { get; set; }
        public ulong Hash1 { get; set; }
        public string Name2 { get; set; }
        public int Unk03 { get; set; }
        public int Unk04 { get; set; }
        public int Unk05 { get; set; }
        public byte Unk06 { get; set; }
        public int Unk07 { get; set; }
        public int Unk08 { get; set; }
        public Matrix Transform { get; set; }
        public float Unk09 { get; set; }
        public float Unk10 { get; set; }
        public Matrix Transform1 { get; set; }
        public AeFrame()
        {

        }

        public bool ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            Unk01 = stream.ReadInt16(isBigEndian);
            Name1 = stream.ReadString16(isBigEndian);
            Unk02 = stream.ReadByte8();
            Hash0 = stream.ReadUInt64(isBigEndian);
            Hash1 = stream.ReadUInt64(isBigEndian);
            Name2 = stream.ReadString16(isBigEndian);
            Unk03 = stream.ReadInt32(isBigEndian);
            Unk04 = stream.ReadInt32(isBigEndian);
            Unk05 = stream.ReadInt32(isBigEndian);
            Unk06 = stream.ReadByte8();
            Unk07 = stream.ReadInt32(isBigEndian);
            Unk08 = stream.ReadInt32(isBigEndian);
            Transform = MatrixExtensions.ReadFromFile(stream, isBigEndian);
            Unk09 = stream.ReadSingle(isBigEndian);
            Unk10 = stream.ReadSingle(isBigEndian);
            Transform1 = MatrixExtensions.ReadFromFile(stream, isBigEndian);
            return true;
        }

        public bool WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            throw new NotImplementedException();
            return true;
        }
    }

    public class AeUnk23 : IAeEntity
    {
        public const int Type = 23;

        public short Unk01 { get; set; }
        public string Name1 { get; set; }
        public byte Unk02 { get; set; }
        public ulong Hash0 { get; set; }
        public ulong Hash1 { get; set; }
        public string Name2 { get; set; }
        public int Unk03 { get; set; }
        public int Unk04 { get; set; }
        public int Unk05 { get; set; }
        public byte Unk06 { get; set; }
        public int Unk07 { get; set; }
        public int Unk08 { get; set; }
        public Matrix Transform { get; set; }
        public string Name3 { get; set; }
        public AeUnk23()
        {

        }

        public bool ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            Unk01 = stream.ReadInt16(isBigEndian);
            Name1 = stream.ReadString16(isBigEndian);
            Name2 = stream.ReadString16(isBigEndian);
            Unk02 = stream.ReadByte8();
            Hash0 = stream.ReadUInt64(isBigEndian);
            Hash1 = stream.ReadUInt64(isBigEndian);
            Unk03 = stream.ReadInt32(isBigEndian);
            Unk04 = stream.ReadInt32(isBigEndian);
            Unk05 = stream.ReadInt32(isBigEndian);
            Unk06 = stream.ReadByte8();
            Unk07 = stream.ReadInt32(isBigEndian);
            Unk08 = stream.ReadInt32(isBigEndian);
            Transform = MatrixExtensions.ReadFromFile(stream, isBigEndian);
            Name3 = stream.ReadString16(isBigEndian); 
            return true;
        }

        public bool WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            throw new NotImplementedException();
            return true;
        }
    }
}
