using Gibbed.Illusion.FileFormats.Hashing;
using ResourceTypes.FrameResource;
using System.ComponentModel;
using System.IO;
using Utils.Extensions;
using Utils.StringHelpers;
using Utils.Models;
using System;

namespace Utils.Types
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class HashName
    {
        ulong hash;
        string name;

        public ulong Hash {
            get { return hash; }
            set { hash = value; }
        }
        public string String {
            get { return name; }
            set { Set(value); }
        }

        [ReadOnly(true)]
        public string Hex {
            get { return string.Format("{0:X}", hash); }
        }
        public HashName()
        {
            name = "";
            hash = 0;
        }
        public HashName(string name)
        {
            Set(name);
        }
        public HashName(HashName other)
        {
            this.hash = other.hash;
            this.name = other.name;
        }
        public HashName(BinaryReader reader)
        {
            ReadFromFile(reader);
        }
        public HashName(MemoryStream stream, bool isBigEndian)
        {
            ReadFromFile(stream, isBigEndian);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            hash = reader.ReadUInt64();
            name = reader.ReadString16();
        }
        public void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            hash = stream.ReadUInt64(isBigEndian);
            name = stream.ReadString16(isBigEndian);
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(hash);
            writer.WriteString16(name);
        }

        public void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            stream.Write(hash, isBigEndian);
            stream.WriteString16(name, isBigEndian);
        }

        public string ConstructGUID()
        {
            byte[] GuidBytes = BitConverter.GetBytes(hash);
            byte[] LeftHand = new byte[4];
            Array.Copy(GuidBytes, LeftHand, 4);

            byte[] RightHand = new byte[4];
            Array.Copy(GuidBytes, 4, RightHand, 0, 4);

            string SLeftHand = BitConverter.ToString(LeftHand).Replace("-", "");
            string SRightHand = BitConverter.ToString(RightHand).Replace("-", "");
            string FormattedGUID = string.Format("0x{0:x8}, 0x{1:x8}", SLeftHand, SRightHand);
            return FormattedGUID;
        }

        public void Set(string value)
        {
            name = value;

            // Cannot check string.IsNullOrWhitespace
            if(name != "")
            {
                hash = FNV64.Hash(name);
            }
        }

        public int CalculateSize()
        {
            int size = 10;
            size += (name.Length);
            return size;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(name))
            {
                return ((SkeletonBoneIDs)hash).ToString();
            }

            return name;
        }
    }

    public class Short3
    {
        public ushort S1 { get; set; }
        public ushort S2 { get; set; }
        public ushort S3 { get; set; }

        public Short3(Short3 other)
        {
            S1 = other.S1;
            S2 = other.S2;
            S3 = other.S3;
        }

        public Short3(MemoryStream reader, bool isBigEndian)
        {
            S1 = reader.ReadUInt16(isBigEndian);
            S2 = reader.ReadUInt16(isBigEndian);
            S3 = reader.ReadUInt16(isBigEndian);
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(S1);
            writer.Write(S2);
            writer.Write(S3);
        }

        public override string ToString()
        {
            return $"{S1} {S2} {S3}";
        }
    }
}
