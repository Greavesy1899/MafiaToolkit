using SharpDX;
using System;
using System.Collections.Generic;
using System.IO;
using Utils.Extensions;

namespace Utils.SharpDXExtensions
{
    public static class MatrixExtensions
    {
        public static Matrix ReadFromFile(BinaryReader reader)
        {
            Matrix matrix = new Matrix();
            Vector3 m1 = Vector3Extenders.ReadFromFile(reader);
            float x = reader.ReadSingle();
            Vector3 m2 = Vector3Extenders.ReadFromFile(reader);
            float y = reader.ReadSingle();
            Vector3 m3 = Vector3Extenders.ReadFromFile(reader);
            float z = reader.ReadSingle();
            matrix.Column1 = new Vector4(m1, 0.0f);
            matrix.Column2 = new Vector4(m2, 0.0f);
            matrix.Column3 = new Vector4(m3, 0.0f);
            matrix.Row4 = new Vector4(x, y, z, 1.0f);
            return matrix;
        }

        public static Matrix ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            Matrix matrix = new Matrix();
            Vector3 m1 = Vector3Extenders.ReadFromFile(stream, isBigEndian);
            float x = stream.ReadSingle(isBigEndian);
            Vector3 m2 = Vector3Extenders.ReadFromFile(stream, isBigEndian);
            float y = stream.ReadSingle(isBigEndian);
            Vector3 m3 = Vector3Extenders.ReadFromFile(stream, isBigEndian);
            float z = stream.ReadSingle(isBigEndian);
            matrix.Column1 = new Vector4(m1, 0.0f);
            matrix.Column2 = new Vector4(m2, 0.0f);
            matrix.Column3 = new Vector4(m3, 0.0f);
            matrix.Row4 = new Vector4(x, y, z, 1.0f);
            return matrix;
        }

        public static void WriteToFile(this Matrix matrix, BinaryWriter writer)
        {
            Vector3Extenders.WriteToFile(Vector3Extenders.FromVector4(matrix.Column1), writer);
            writer.Write(matrix.Row4.X);
            Vector3Extenders.WriteToFile(Vector3Extenders.FromVector4(matrix.Column2), writer);
            writer.Write(matrix.Row4.Y);
            Vector3Extenders.WriteToFile(Vector3Extenders.FromVector4(matrix.Column3), writer);
            writer.Write(matrix.Row4.Z);
        }

        public static void WriteToFile(this Matrix matrix, MemoryStream stream, bool isBigEndian)
        {
            Vector3Extenders.WriteToFile(Vector3Extenders.FromVector4(matrix.Column1), stream, isBigEndian);
            stream.Write(matrix.Row4.X, isBigEndian);
            Vector3Extenders.WriteToFile(Vector3Extenders.FromVector4(matrix.Column2), stream, isBigEndian);
            stream.Write(matrix.Row4.Y, isBigEndian);
            Vector3Extenders.WriteToFile(Vector3Extenders.FromVector4(matrix.Column3), stream, isBigEndian);
            stream.Write(matrix.Row4.Z, isBigEndian);
        }

        public static Vector3 ToEuler(this Matrix matrix)
        {
            Vector3 rotation = new Vector3();
            rotation.Y = (float)-Math.Asin(matrix.M31);

            //Gymbal lock: pitch = -90
            if (matrix.M31 == 1)
            {
                rotation.Z = 0.0f;
                rotation.X = (float)Math.Atan2(-matrix.M12, -matrix.M13);
            }

            //Gymbal lock: pitch = 90
            else if (matrix.M31 == -1)
            {
                rotation.Z = 0.0f;
                rotation.X = (float)Math.Atan2(matrix.M12, matrix.M13);
            }
            //General solution
            else
            {
                rotation.Z = (float)Math.Atan2(matrix.M21, matrix.M11);
                rotation.X = (float)Math.Atan2(matrix.M32, matrix.M33);
            }
            rotation.X = MathUtil.RadiansToDegrees(rotation.X);
            rotation.Y = MathUtil.RadiansToDegrees(rotation.Y);
            rotation.Z = MathUtil.RadiansToDegrees(rotation.Z);
            rotation.X = float.IsNaN(rotation.X) ? 0.0f : rotation.X;
            rotation.Y = float.IsNaN(rotation.Y) ? 0.0f : rotation.Y;
            rotation.Z = float.IsNaN(rotation.Z) ? 0.0f : rotation.Z;

            return rotation;
        }

        public static void FromEuler(this Matrix transform, Vector3 rotation)
        {
            float roll = MathUtil.DegreesToRadians(rotation.X);
            float pitch = MathUtil.DegreesToRadians(rotation.Y);
            float yaw = MathUtil.DegreesToRadians(rotation.Z);

            float su = (float)Math.Sin(roll);
            float cu = (float)Math.Cos(roll);
            float sv = (float)Math.Sin(pitch);
            float cv = (float)Math.Cos(pitch);
            float sw = (float)Math.Sin(yaw);
            float cw = (float)Math.Cos(yaw);
            transform.M11 = cv * cw;
            transform.M12 = su * sv * cw - cu * sw;
            transform.M13 = su * sw + cu * sv * cw;
            transform.M21 = cv * sw;
            transform.M22 = cu * cw + su * sv * sw;
            transform.M23 = cu * sv * sw - su * cw;
            transform.M31 = -sv;
            transform.M32 = su * cv;
            transform.M33 = cu * cv;
        }

        public static Matrix SetMatrix(this Matrix transform, Vector3 rotation, Vector3 scale, Vector3 position)
        {
            Matrix r = Matrix.RotationYawPitchRoll(MathUtil.DegreesToRadians(rotation.Z), MathUtil.DegreesToRadians(rotation.Y), MathUtil.DegreesToRadians(rotation.X));
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
            return euler;
        }
    }
}
