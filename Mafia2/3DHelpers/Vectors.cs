using System;
using System.ComponentModel;
using System.IO;

namespace Mafia2
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Vector3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        /// <summary>
        /// Construct a Vector3 from three floats.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Construct a Vector3 and set all three values to the passed value.
        /// </summary>
        /// <param name="value"></param>
        public Vector3(float value)
        {
            X = value;
            Y = value;
            Z = value;
        }

        /// <summary>
        /// Construct Vector3 from file data.
        /// </summary>
        /// <param name="reader"></param>
        public Vector3(BinaryReader reader)
        {
            ReadfromFile(reader);
        }

        /// <summary>
        /// Write vector3 to file.
        /// </summary>
        /// <param name="writer"></param>
        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Z);
        }

        /// <summary>
        /// Read Vector3 data from file.
        /// </summary>
        /// <param name="reader"></param>
        public void ReadfromFile(BinaryReader reader)
        {
            X = reader.ReadSingle();
            Y = reader.ReadSingle();
            Z = reader.ReadSingle();
        }

        /// <summary>
        /// Convert vector3 radians to degrees.
        /// </summary>
        public void ConvertToDegrees()
        {
            X = -(float)(X * 180 / Math.PI);
            Y = -(float)(Y * 180 / Math.PI);
            Z = -(float)(Z * 180 / Math.PI);
        }

        /// <summary>
        /// Find the cross product between two vectors.
        /// </summary>
        /// <param name="vector2"></param>
        public void CrossProduct(Vector3 vector2)
        {
            X = Y * vector2.Z - Z * vector2.Y;
            Y = Z * vector2.X - X * vector2.Z;
            Z = X * vector2.Y - Y * vector2.X;
        }

        /// <summary>
        /// Normalize the vector3.
        /// </summary>
        public void Normalize()
        {
            float num1 = (float)(X * (double)X + Y * Y + Z * (double)Z);
            float num2 = num1 == 0.0 ? float.MaxValue : (float)(1.0 / Math.Sqrt(num1));
            X *= num2;
            Y *= num2;
            Z *= num2;
        }

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }
        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }
        public static Vector3 operator *(Vector3 a, float scale)
        {
            return new Vector3(a.X * scale, a.Y * scale, a.Z * scale);
        }
        public static Vector3 operator /(Vector3 a, float scale)
        {
            return new Vector3(a.X / scale, a.Y / scale, a.Z / scale);
        }

        public override string ToString()
        {
            return $"X: {X}, Y: {Y}, Z: {Z}";
        }

    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Vector2
    {
        public float X { get; set; }
        public float Y { get; set; }

        /// <summary>
        /// Construct Vector2 from two floats.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Construct Vector2 from file data.
        /// </summary>
        /// <param name="reader"></param>
        public Vector2(BinaryReader reader)
        {
            X = reader.ReadSingle();
            Y = reader.ReadSingle();
        }

        public override string ToString()
        {
            return $"X: {X}, Y: {Y}";
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Float4
    {
        public float[] Data { get; set; }

        /// <summary>
        /// Construct Float4 from parsed data.
        /// </summary>
        /// <param name="reader"></param>
        public Float4(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        /// <summary>
        /// Read data from file.
        /// </summary>
        /// <param name="reader"></param>
        public void ReadFromFile(BinaryReader reader)
        {
            Data = new float[4];

            for (int i = 0; i != 4; i++)
                Data[i] = reader.ReadSingle();
        }

        /// <summary>
        /// Write data to file.
        /// </summary>
        /// <param name="writer"></param>
        public void WriteToFile(BinaryWriter writer)
        {
            for (int i = 0; i != 4; i++)
                writer.Write(Data[i]);
        }

        public override string ToString()
        {
            return $"{Data[0]}, {Data[1]}, {Data[2]}, {Data[3]}";
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class UVVector2
    {
        public Half X { get; set; }
        public Half Y { get; set; }

        /// <summary>
        /// Construct UV Vector from two halfs.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public UVVector2(Half x, Half y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Construct empty vector
        /// </summary>
        public UVVector2()
        {

        }

        /// <summary>
        /// Write UVVector2 as floats.
        /// </summary>
        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(X);
            writer.Write(1f - Y);
        }

        /// <summary>
        /// Read UVVector2 from file as floats.
        /// </summary>
        /// <param name="reader"></param>
        public void ReadFromFile(BinaryReader reader)
        {
            X = HalfHelper.SingleToHalf(reader.ReadSingle());
            Y = HalfHelper.SingleToHalf(reader.ReadSingle());
            Y -= (Half)1.0f;
        }

        public override string ToString()
        {
            return $"X: {X}, Y: {Y}";
        }
    }

    public class Short3
    {
        public short S1 { get; set; }
        public short S2 { get; set; }
        public short S3 { get; set; }

        /// <summary>
        /// SET all values to -100
        /// </summary>
        public Short3()
        {
            S1 = -100;
            S2 = -100;
            S3 = -100;
        }

        /// <summary>
        /// Construct Short3 from file data.
        /// </summary>
        /// <param name="reader"></param>
        public Short3(BinaryReader reader)
        {
            S1 = reader.ReadInt16();
            S2 = reader.ReadInt16();
            S3 = reader.ReadInt16();
        }

        /// <summary>
        /// Construct Short3 from three integers.
        /// </summary>
        /// <param name="i1"></param>
        /// <param name="i2"></param>
        /// <param name="i3"></param>
        public Short3(int i1, int i2, int i3)
        {
            S1 = (short)i1;
            S2 = (short)i2;
            S3 = (short)i3;
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
            I1 = reader.ReadInt32();
            I2 = reader.ReadInt32();
            I3 = reader.ReadInt32();
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
            normal = new Vector3(reader);
            floats = new float[]{ reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle() };
        }
    }
}
