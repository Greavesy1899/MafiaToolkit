using Gibbed.Illusion.FileFormats.Hashing;
using SharpDX;
using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using Utils.Extensions;
using Utils.Models;
using Utils.SharpDXExtensions;

namespace Utils.Types
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Hash
    {
        ulong hash;
        string _string;
        short size;

        //[ReadOnly(true)]
        public ulong uHash {
            get { return hash; }
            set { hash = value; }
        }
        public string String {
            get { return _string; }
            set { Set(value); }
        }

        [ReadOnly(true)]
        public string Hex {
            get { return string.Format("{0:X}", hash); }
        }
        public Hash()
        {
            _string = "";
            hash = 0;
        }
        public Hash(string name)
        {
            Set(name);
        }
        public Hash(Hash other)
        {
            this.hash = other.hash;
            this._string = other._string;
        }
        public Hash(BinaryReader reader)
        {
            ReadFromFile(reader);
        }
        public Hash(MemoryStream stream, bool isBigEndian)
        {
            ReadFromFile(stream, isBigEndian);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            hash = reader.ReadUInt64();
            size = reader.ReadInt16();
            _string = Encoding.ASCII.GetString(reader.ReadBytes(size));
        }
        public void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            hash = stream.ReadUInt64(isBigEndian);
            size = stream.ReadInt16(isBigEndian);
            _string = Encoding.ASCII.GetString(stream.ReadBytes(size));
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(hash);
            writer.Write(size);
            writer.Write(_string.ToCharArray());
        }

        public void WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            writer.Write(hash, isBigEndian);
            writer.Write(size, isBigEndian);
            writer.Write(_string.ToCharArray());
        }

        public void Set(string name)
        {
            _string = name;
            size = (short)name.Length;

            if (_string == "")
                hash = 0;
            else
                hash = FNV64.Hash(name);
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(_string))
                return ((SkeletonBoneIDs)hash).ToString();
            else
                return _string;
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ParentStruct
    {
        int index;
        string name;
        int refID;

        public int Index {
            get { return index; }
            set { index = value; }
        }

        public string Name {
            get { return name; }
            set { name = value; }
        }

        public int RefID {
            get { return refID; }
            set { refID = value; }
        }

        public ParentStruct(int index)
        {
            this.index = index;
        }

        public ParentStruct(ParentStruct other)
        {
            index = other.index;
            name = other.name;
            refID = other.refID;
        }

        public override string ToString()
        {
            if (index == -1)
                return string.Format("{0}, root", index);
            else
                return string.Format("{0}. {1}", index, name);
        }
    }

    public class Short3
    {
        public ushort S1 { get; set; }
        public ushort S2 { get; set; }
        public ushort S3 { get; set; }

        /// <summary>
        /// SET all values to -100
        /// </summary>
        public Short3()
        {
            S1 = ushort.MaxValue;
            S2 = ushort.MaxValue;
            S3 = ushort.MaxValue;
        }

        public Short3(Short3 other)
        {
            S1 = other.S1;
            S2 = other.S2;
            S3 = other.S3;
        }

        /// <summary>
        /// Construct Short3 from file data.
        /// </summary>
        /// <param name="reader"></param>
        public Short3(MemoryStream reader, bool isBigEndian)
        {
            S1 = reader.ReadUInt16(isBigEndian);
            S2 = reader.ReadUInt16(isBigEndian);
            S3 = reader.ReadUInt16(isBigEndian);
        }

        public Short3(BinaryReader reader)
        {
            S1 = reader.ReadUInt16();
            S2 = reader.ReadUInt16();
            S3 = reader.ReadUInt16();
        }

        /// <summary>
        /// Construct Short3 from three integers.
        /// </summary>
        /// <param name="i1"></param>
        /// <param name="i2"></param>
        /// <param name="i3"></param>
        public Short3(int i1, int i2, int i3)
        {
            S1 = (ushort)i1;
            S2 = (ushort)i2;
            S3 = (ushort)i3;
        }

        /// <summary>
        /// Write Short3 data to file.
        /// </summary>
        /// <param name="writer"></param>
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
