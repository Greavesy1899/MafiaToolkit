using System;
using System.IO;

namespace Mafia2
{
    public struct Vector3
    {
        float x;
        float y;
        float z;

        public float X {
            get { return x; }
            set { x = value; }
        }
        public float Y {
            get { return y; }
            set { y = value; }
        }
        public float Z {
            get { return z; }
            set { z = value; }
        }

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3(BinaryReader reader)
        {
            x = reader.ReadSingle();
            y = reader.ReadSingle();
            z = reader.ReadSingle();
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(x);
            writer.Write(y);
            writer.Write(z);
        }

        public void ConvertToDegrees()
        {
            double x = this.x * 180 / Math.PI;
            double y = this.y * 180 / Math.PI;
            double z = this.z * 180 / Math.PI;

            this.x = -(float)x;
            this.y = -(float)y;
            this.z = -(float)z;
        }
    
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
            return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public override string ToString()
        {
            return string.Format("X: {0}, Y: {1}, Z: {2}", x, y, z);
        }

    }

    public struct Vector2
    {
        float x;
        float y;

        public float X {
            get { return x; }
            set { x = value; }
        }
        public float Y {
            get { return y; }
            set { y = value; }
        }

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        public Vector2(BinaryReader reader)
        {
            this.x = reader.ReadSingle();
            this.y = reader.ReadSingle();
        }
        public override string ToString()
        {
            return string.Format("X: {0}, Y: {1}", x, y);
        }
    }

    public struct UVVector2
    {
        Half x;
        Half y;

        public Half X {
            get { return x; }
            set { x = value; }
        }
        public Half Y {
            get { return y; }
            set { y = value; }
        }

        public UVVector2(Half x, Half y)
        {
            this.x = x;
            this.y = y;
        }
        public override string ToString()
        {
            return string.Format("X: {0}, Y: {1}", x, y);
        }
    }

    public class Short3
    {
        public short s1;
        public short s2;
        public short s3;

        /// <summary>
        /// SET TO -100
        /// </summary>
        public Short3()
        {
            s1 = -100;
            s2 = -100;
            s3 = -100;
        }

        public Short3(BinaryReader reader)
        {
            s1 = reader.ReadInt16();
            s2 = reader.ReadInt16();
            s3 = reader.ReadInt16();
        }

        public Short3(int i1, int i2, int i3)
        {
            s1 = (short)i1;
            s2 = (short)i2;
            s3 = (short)i3;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", s1, s2, s3);
        }
    }

    public class Int3
    {
        public int i1;
        public int i2;
        public int i3;

        public Int3(BinaryReader reader)
        {
            i1 = reader.ReadInt32();
            i2 = reader.ReadInt32();
            i3 = reader.ReadInt32();
        }


        public override string ToString()
        {
            return string.Format("{0} {1} {2}", i1, i2, i3);
        }
    }

    public struct Polygon
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
