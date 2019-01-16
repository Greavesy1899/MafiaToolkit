using System;
using System.ComponentModel;
using System.IO;
using SharpDX;

namespace Mafia2
{
    public static class Vector3Extenders
    {
        public static Vector3 ReadFromFile(BinaryReader reader)
        {
            Vector3 vec = new Vector3();
            vec.X = reader.ReadSingle();
            vec.Y = reader.ReadSingle();
            vec.Z = reader.ReadSingle();
            return vec;
        }

        public static void WriteToFile(this Vector3 vec, BinaryWriter writer)
        {
            writer.Write(vec.X);
            writer.Write(vec.Y);
            writer.Write(vec.Z);
        }
    }

    public static class Vector2Extenders
    {
        public static Vector2 ReadFromFile(BinaryReader reader)
        {
            Vector2 vec = new Vector2();
            vec.X = reader.ReadSingle();
            vec.Y = reader.ReadSingle();
            return vec;
        }

        public static void WriteToFile(this Vector2 vec, BinaryWriter writer)
        {
            writer.Write(vec.X);
            writer.Write(vec.Y);
        }
    }

    public static class Vector4Extenders
    {
        public static Vector4 ReadFromFile(BinaryReader reader)
        {
            Vector4 vec = new Vector4();
            vec.X = reader.ReadSingle();
            vec.Y = reader.ReadSingle();
            vec.Z = reader.ReadSingle();
            vec.W = reader.ReadSingle();
            return vec;
        }

        public static void WriteToFile(this Vector4 vec, BinaryWriter writer)
        {
            writer.Write(vec.X);
            writer.Write(vec.Y);
            writer.Write(vec.Z);
            writer.Write(vec.W);
        }
    }

    public static class HalfExtenders
    {
        public static byte[] GetBytes(this SharpDX.Half value)
        {
            return BitConverter.GetBytes(value.RawValue);
        }
    }
    public static class Half2Extenders
    {
        public static Half2 ReadFromFile(BinaryReader reader)
        {
            Half2 half = new Half2();
            half.X = reader.ReadSingle();
            half.Y = reader.ReadSingle();
            return half;
        }

        public static void WriteToFile(this Half2 half, BinaryWriter writer)
        {
            writer.Write(half.X);
            writer.Write(half.Y);
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

        /// <summary>
        /// Construct Short3 from file data.
        /// </summary>
        /// <param name="reader"></param>
        public Short3(BinaryReader reader)
        {
            S1 = reader.ReadUInt16();
            S2 = reader.ReadUInt16();
            S3 = reader.ReadUInt16();
        }

        /// <summary>
        /// Build Short3 from Int3
        /// </summary>
        /// <param name="ints"></param>
        public Short3(Int3 ints)
        {
            S1 = (ushort)ints.I1;
            S2 = (ushort)ints.I2;
            S3 = (ushort)ints.I3;
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

    public class Int3
    {
        public int I1 { get; set; }
        public int I2 { get; set; }
        public int I3 { get; set; }

        /// <summary>
        /// Construct Int3 from file data.
        /// </summary>
        /// <param name="reader"></param>
        public Int3(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        /// <summary>
        /// Build Int3 from Short3
        /// </summary>
        /// <param name="ints"></param>
        public Int3(Short3 s3)
        {
            I1 = (int)s3.S1;
            I2 = (int)s3.S2;
            I3 = (int)s3.S3;
        }

        /// <summary>
        /// read data from file.
        /// </summary>
        /// <param name="reader"></param>
        public void ReadFromFile(BinaryReader reader)
        {
            I1 = reader.ReadInt32();
            I2 = reader.ReadInt32();
            I3 = reader.ReadInt32();
        }

        /// <summary>
        /// write data to file
        /// </summary>
        /// <param name="writer"></param>
        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(I1);
            writer.Write(I2);
            writer.Write(I3);
        }

        public override string ToString()
        {
            return $"{I1} {I2} {I3}";
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Polygon
    {
        int numPoints;
        int firstVertIndex;
        int firstUnkIndex;
        Vector3 normal;
        float[] floats;

        public int NumPoints {
            get { return numPoints; }
            set { numPoints = value; }
        }
        public int FirstVertIndex {
            get { return firstVertIndex; }
            set { firstVertIndex = value; }
        }
        public int FirstUnkIndex {
            get { return firstUnkIndex; }
            set { firstUnkIndex = value; }
        }
        public Vector3 Normal {
            get { return normal; }
            set { normal = value; }
        }
        public float[] Floats {
            get { return floats; }
            set { floats = value; }
        }

        public Polygon(BinaryReader reader)
        {
            numPoints = reader.ReadInt32();
            firstVertIndex = reader.ReadInt32();
            firstUnkIndex = reader.ReadInt32();
            normal = Vector3Extenders.ReadFromFile(reader);
            floats = new float[]{ reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle() };
        }
    }
}
