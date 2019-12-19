using SharpDX;
using System;
using System.IO;
using Utils.Extensions;

namespace Utils.SharpDXExtensions
{
    public static class MatrixExtensions
    {
        public static Matrix ReadFromFile(BinaryReader reader)
        {
            Matrix matrix = new Matrix();
            matrix.Column1 = Vector4Extenders.ReadFromFile(reader);
            matrix.Column2 = Vector4Extenders.ReadFromFile(reader);
            matrix.Column3 = Vector4Extenders.ReadFromFile(reader);
            return matrix;
        }

        public static Matrix ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            Matrix matrix = new Matrix();
            matrix.Column1 = Vector4Extenders.ReadFromFile(stream, isBigEndian);
            matrix.Column2 = Vector4Extenders.ReadFromFile(stream, isBigEndian);
            matrix.Column3 = Vector4Extenders.ReadFromFile(stream, isBigEndian);
            return matrix;
        }

        public static void WriteToFile(this Matrix matrix, BinaryWriter writer)
        {
            Vector4Extenders.WriteToFile(matrix.Column1, writer);
            Vector4Extenders.WriteToFile(matrix.Column2, writer);
            Vector4Extenders.WriteToFile(matrix.Column3, writer);
        }

        public static void WriteToFile(this Matrix matrix, MemoryStream stream, bool isBigEndian)
        {
            Vector4Extenders.WriteToFile(matrix.Column1, stream, isBigEndian);
            Vector4Extenders.WriteToFile(matrix.Column2, stream, isBigEndian);
            Vector4Extenders.WriteToFile(matrix.Column3, stream, isBigEndian);
        }

        public static Matrix SetMatrix(Quaternion rotation, Vector3 scale, Vector3 position)
        {
            Matrix r = Matrix.RotationQuaternion(rotation);
            Matrix s = Matrix.Scaling(scale);
            Matrix t = Matrix.Translation(position);
            return r * s * t;
        }
        public static Matrix SetMatrix(Vector3 rotation, Vector3 scale, Vector3 position)
        {
            Quaternion quaternion = Quaternion.RotationYawPitchRoll(MathUtil.DegreesToRadians(rotation.X), MathUtil.DegreesToRadians(rotation.Y), MathUtil.DegreesToRadians(rotation.Z));
            Matrix matrix = SetMatrix(quaternion, scale, position);
            return matrix;
        }
    }

    public static class QuaternionExtensions
    {
        public static Vector3 ToEuler(this Quaternion quat)
        {
            Vector3 euler = new Vector3();
            double test = quat.X * quat.Y + quat.Z * quat.W;
            if (test > 0.499)
            { // singularity at north pole
                euler.X = MathUtil.RadiansToDegrees((float)(2 * Math.Atan2(quat.X, quat.W)));
                euler.Z = MathUtil.RadiansToDegrees((float)(Math.PI / 2));
                euler.Y = MathUtil.RadiansToDegrees(0);
                return euler;
            }
            if (test < -0.499)
            { // singularity at south pole
                euler.X = MathUtil.RadiansToDegrees((float)(-2 * Math.Atan2(quat.X, quat.W)));
                euler.Z = MathUtil.RadiansToDegrees((float)(-Math.PI / 2));
                euler.Y = MathUtil.RadiansToDegrees(0);
                return euler;
            }
            double sqx = quat.X * quat.X;
            double sqy = quat.Y * quat.Y;
            double sqz = quat.Z * quat.Z;
            euler.X = -MathUtil.RadiansToDegrees((float)Math.Atan2(2 * quat.Y * quat.W - 2 * quat.X * quat.Z, 1 - 2 * sqy - 2 * sqz));
            euler.Z = -MathUtil.RadiansToDegrees((float)Math.Asin(2 * test));
            euler.Y = -MathUtil.RadiansToDegrees((float)Math.Atan2(2 * quat.X * quat.W - 2 * quat.Y * quat.Z, 1 - 2 * sqx - 2 * sqz));
            return euler;
        }

        public static Quaternion ReadFromFile(BinaryReader reader)
        {
            Quaternion quaternion = new Quaternion();
            quaternion.X = reader.ReadSingle();
            quaternion.Y = reader.ReadSingle();
            quaternion.Z = reader.ReadSingle();
            quaternion.W = reader.ReadSingle();
            return quaternion;
        }

        public static Quaternion ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            Quaternion quaternion = new Quaternion();
            quaternion.X = stream.ReadSingle(isBigEndian);
            quaternion.Y = stream.ReadSingle(isBigEndian);
            quaternion.Z = stream.ReadSingle(isBigEndian);
            quaternion.W = stream.ReadSingle(isBigEndian);
            return quaternion;
        }

        public static void WriteToFile(this Quaternion quaternion, BinaryWriter writer)
        {
            writer.Write(quaternion.X);
            writer.Write(quaternion.Y);
            writer.Write(quaternion.Z);
            writer.Write(quaternion.W);
        }

        public static void WriteToFile(this Quaternion quaternion, MemoryStream stream, bool isBigEndian)
        {
            stream.Write(quaternion.X, isBigEndian);
            stream.Write(quaternion.Y, isBigEndian);
            stream.Write(quaternion.Z, isBigEndian);
            stream.Write(quaternion.W, isBigEndian);
        }
    }
}
