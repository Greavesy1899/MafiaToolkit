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

        public static Matrix RowEchelon(Matrix other)
        {
            Matrix matrix = new Matrix();
            matrix.Row1 = other.Column1;
            matrix.Row2 = other.Column2;
            matrix.Row3 = other.Column3;
            matrix.Row4 = other.Row4;
            matrix.Column4 = Vector4.Zero;
            return matrix;
        }

        public static Vector3 RotatePoint(this Matrix matrix, Vector3 position)
        {
            Vector3 vector = new Vector3();
            vector.X = matrix.M11 * position.X + matrix.M12 * position.Y + matrix.M13 * position.Z;
            vector.Y = matrix.M21 * position.X + matrix.M22 * position.Y + matrix.M23 * position.Z;
            vector.Z = matrix.M31 * position.X + matrix.M32 * position.Y + matrix.M33 * position.Z;
            return vector; //Output coords in order (x,y,z)
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
            var qw = quat.W;
            var qx = quat.X;
            var qy = quat.Y;
            var qz = quat.Z;
            var eX = Math.Atan2(-2 * ((qy * qz) - (qw * qx)), (qw * qw) - (qx * qx) - (qy * qy) + (qz * qz));
            var eY = Math.Asin(2 * ((qx * qz) + (qw * qy)));
            var eZ = Math.Atan2(-2 * ((qx * qy) - (qw * qz)), (qw * qw) + (qx * qx) - (qy * qy) - (qz * qz));
            euler.Z = (float)Math.Round(eZ * 180 / Math.PI);
            euler.Y = (float)Math.Round(eY * 180 / Math.PI);
            euler.X = (float)Math.Round(eX * 180 / Math.PI);

            //temp
            if (float.IsNaN(euler.X))
            {
                euler.X = 0.0f;
            }
            if (float.IsNaN(euler.Y))
            {
                euler.Y = 0.0f;
            }
            if (float.IsNaN(euler.Z))
            {
                euler.Z = 0.0f;
            }
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
