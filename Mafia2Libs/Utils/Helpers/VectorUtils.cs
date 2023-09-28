using System;
using System.IO;
using System.Numerics;
using Vortice.Mathematics;
using Utils.Extensions;
using Vortice.Mathematics.PackedVector;
using Toolkit.Mathematics;

namespace Utils.VorticeUtils
{
    public static class Vector3Utils
    {
        public static Vector3 TransformCoordinate(this Vector3 coordinate, Matrix4x4 transform)
        {
            Vector4 vector = new Vector4();
            vector.X = (coordinate.X * transform.M11) + (coordinate.Y * transform.M21) + (coordinate.Z * transform.M31) + transform.M41;
            vector.Y = (coordinate.X * transform.M12) + (coordinate.Y * transform.M22) + (coordinate.Z * transform.M32) + transform.M42;
            vector.Z = (coordinate.X * transform.M13) + (coordinate.Y * transform.M23) + (coordinate.Z * transform.M33) + transform.M43;
            vector.W = 1f / ((coordinate.X * transform.M14) + (coordinate.Y * transform.M24) + (coordinate.Z * transform.M34) + transform.M44);

            return new Vector3(vector.X * vector.W, vector.Y * vector.W, vector.Z * vector.W);
        }

        public static Vector3 FromVector4(Vector4 vector4)
        {
            Vector3 vec = new Vector3();
            vec.X = vector4.X;
            vec.Y = vector4.Y;
            vec.Z = vector4.Z;
            return vec;
        }

        public static Vector3 ReadFromFile(BinaryReader reader)
        {
            Vector3 vec = new Vector3();
            vec.X = reader.ReadSingle();
            vec.Y = reader.ReadSingle();
            vec.Z = reader.ReadSingle();
            return vec;
        }

        public static Vector3 ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            Vector3 vec = new Vector3();
            vec.X = stream.ReadSingle(isBigEndian);
            vec.Y = stream.ReadSingle(isBigEndian);
            vec.Z = stream.ReadSingle(isBigEndian);
            return vec;
        }

        public static void WriteToFile(this Vector3 vec, BinaryWriter writer)
        {
            writer.Write(vec.X);
            writer.Write(vec.Y);
            writer.Write(vec.Z);
        }

        public static void WriteToFile(this Vector3 vec, MemoryStream stream, bool isBigEndian)
        {
            stream.Write(vec.X, isBigEndian);
            stream.Write(vec.Y, isBigEndian);
            stream.Write(vec.Z, isBigEndian);
        }

        public static Vector3 Swap(this Vector3 pos)
        {
            float z = pos.Z;
            pos.Z = pos.X;
            pos.X = z;
            return pos;
        }

        public static bool IsNaN(this Vector3 vector)
        {
            return float.IsNaN(vector.X) || float.IsNaN(vector.Y) || float.IsNaN(vector.Z);
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

        public static Vector4 ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            Vector4 vec = new Vector4();
            vec.X = stream.ReadSingle(isBigEndian);
            vec.Y = stream.ReadSingle(isBigEndian);
            vec.Z = stream.ReadSingle(isBigEndian);
            vec.W = stream.ReadSingle(isBigEndian);
            return vec;
        }

        public static bool IsNaN(this Vector4 vector)
        {
            return float.IsNaN(vector.X) || float.IsNaN(vector.Y) || float.IsNaN(vector.Z) || float.IsNaN(vector.W);
        }

        public static void WriteToFile(this Vector4 vec, BinaryWriter writer)
        {
            writer.Write(vec.X);
            writer.Write(vec.Y);
            writer.Write(vec.Z);
            writer.Write(vec.W);
        }

        public static void WriteToFile(this Vector4 vec, MemoryStream stream, bool isBigEndian)
        {
            stream.Write(vec.X, isBigEndian);
            stream.Write(vec.Y, isBigEndian);
            stream.Write(vec.Z, isBigEndian);
            stream.Write(vec.W, isBigEndian);
        }
    }

    public static class HalfExtenders
    {
        public static byte[] GetBytes(this Toolkit.Mathematics.Half value) => BitConverter.GetBytes(value.value);
    }
    public static class Half2Extenders
    {
        public static Half2 ReadFromFile(BinaryReader reader)
        {
            float XComp = reader.ReadSingle();
            float YComp = reader.ReadSingle();

            Half2 half = new Half2(XComp, YComp);
            return half;
        }

        public static void WriteToFile(this Half2 half, BinaryWriter writer)
        {
            writer.Write(half.X);
            writer.Write(half.Y);
        }

    }
}
