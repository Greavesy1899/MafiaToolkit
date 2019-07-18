using System;
using System.IO;
using SharpDX;

namespace Utils.SharpDXExtensions
{
    public static class Vector3Extenders
    {

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
}
