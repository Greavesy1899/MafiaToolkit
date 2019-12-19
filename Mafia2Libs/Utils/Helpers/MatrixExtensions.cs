using SharpDX;
using System;
using System.IO;

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
        public static Matrix SetMatrix(this Matrix transform, Vector3 rotation, Vector3 scale, Vector3 position)
        {
            Quaternion quaternion = Quaternion.RotationYawPitchRoll(MathUtil.DegreesToRadians(rotation.X), MathUtil.DegreesToRadians(rotation.Y), MathUtil.DegreesToRadians(rotation.Z));
            Matrix r = Matrix.RotationQuaternion(quaternion);
            Matrix s = Matrix.Scaling(scale);
            Matrix t = Matrix.Translation(position);
            return r * s * t;
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
    }
}
