using System;
using System.Globalization;
using System.IO;
using System.Numerics;
using Utils.Extensions;
using Utils.Logging;
using Vortice.Mathematics;

namespace Utils.VorticeUtils
{
    public static class MatrixUtils
    {
        public static void SetColumn(ref this Matrix4x4 matrix, int Index, Vector4 NewColumn)
        {          
            if (Index == 0) { matrix.M11 = NewColumn.X; matrix.M21 = NewColumn.Y; matrix.M31 = NewColumn.Z; matrix.M41 = NewColumn.W; }
            else if (Index == 1) { matrix.M12 = NewColumn.X; matrix.M22 = NewColumn.Y; matrix.M32 = NewColumn.Z; matrix.M42 = NewColumn.W; }
            else if (Index == 2) { matrix.M13 = NewColumn.X; matrix.M23 = NewColumn.Y; matrix.M33 = NewColumn.Z; matrix.M43 = NewColumn.W; }
            else if (Index == 3) { matrix.M14 = NewColumn.X; matrix.M24 = NewColumn.Y; matrix.M34 = NewColumn.Z; matrix.M44 = NewColumn.W; }
            else { ToolkitAssert.Ensure(false, "Invalid Index passed into Matrix4x4.GetColumn"); }
        }

        public static Vector4 GetColumn(this Matrix4x4 matrix, int Index)
        {
            if(Index == 0) { return new Vector4(matrix.M11, matrix.M21, matrix.M31, matrix.M41); }
            else if (Index == 1) { return new Vector4(matrix.M12, matrix.M22, matrix.M32, matrix.M42); }
            else if (Index == 2) { return new Vector4(matrix.M13, matrix.M23, matrix.M33, matrix.M43); }
            else if (Index == 3) { return new Vector4(matrix.M14, matrix.M24, matrix.M34, matrix.M44); }
            else { ToolkitAssert.Ensure(false, "Invalid Index passed into Matrix4x4.GetColumn"); }

            return Vector4.Zero;
        }

        public static void SetRow(ref this Matrix4x4 matrix, int Index, Vector4 NewRow)
        {
            if (Index == 0) { matrix.M11 = NewRow.X; matrix.M12 = NewRow.Y; matrix.M13 = NewRow.Z; matrix.M14 = NewRow.W; }
            else if (Index == 1) { matrix.M21 = NewRow.X; matrix.M22 = NewRow.Y; matrix.M23 = NewRow.Z; matrix.M24 = NewRow.W; }
            else if (Index == 2) { matrix.M31 = NewRow.X; matrix.M32 = NewRow.Y; matrix.M33 = NewRow.Z; matrix.M34 = NewRow.W; }
            else if (Index == 3) { matrix.M41 = NewRow.X; matrix.M42 = NewRow.Y; matrix.M43 = NewRow.Z; matrix.M44 = NewRow.W; }
            else { ToolkitAssert.Ensure(false, "Invalid Index passed into Matrix4x4.GetColumn"); }
        }

        public static Vector4 GetRow(this Matrix4x4 matrix, int Index)
        {
            if (Index == 0) { return new Vector4(matrix.M11, matrix.M12, matrix.M13, matrix.M14); }
            else if (Index == 1) { return new Vector4(matrix.M21, matrix.M22, matrix.M23, matrix.M24); }
            else if (Index == 2) { return new Vector4(matrix.M31, matrix.M32, matrix.M33, matrix.M34); }
            else if (Index == 3) { return new Vector4(matrix.M41, matrix.M42, matrix.M43, matrix.M44); }
            else { ToolkitAssert.Ensure(false, "Invalid Index passed into Matrix4x4.GetRow"); }

            return Vector4.Zero;
        }

        public static Matrix4x4 CopyFrom(this Matrix4x4 Other)
        {
            Matrix4x4 NewTransform = new Matrix4x4(
                Other.M11, Other.M12, Other.M13, Other.M14,
                Other.M21, Other.M22, Other.M23, Other.M24,
                Other.M31, Other.M32, Other.M33, Other.M34,
                Other.M41, Other.M42, Other.M43, Other.M44);

            return NewTransform;
        }

        public static bool IsNaN(this Matrix4x4 matrix)
        {
            return matrix.GetColumn(0).IsNaN() || matrix.GetColumn(1).IsNaN() || matrix.GetColumn(2).IsNaN() || matrix.GetColumn(3).IsNaN();
        }

        public static Matrix4x4 ReadFromFile(BinaryReader reader)
        {
            Matrix4x4 matrix = new Matrix4x4();
            matrix.SetColumn(0, Vector4Extenders.ReadFromFile(reader));
            matrix.SetColumn(1, Vector4Extenders.ReadFromFile(reader));
            matrix.SetColumn(2, Vector4Extenders.ReadFromFile(reader));

            return matrix;
        }

        public static Matrix4x4 ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            Matrix4x4 matrix = new Matrix4x4();
            matrix.SetColumn(0, Vector4Extenders.ReadFromFile(stream, isBigEndian));
            matrix.SetColumn(1, Vector4Extenders.ReadFromFile(stream, isBigEndian));
            matrix.SetColumn(2, Vector4Extenders.ReadFromFile(stream, isBigEndian));

            if (matrix.IsNaN())
            {
                ToolkitAssert.Ensure(matrix.IsNaN(), "Matrix.IsNan() during ReadFromFile");
                matrix = Matrix4x4.Identity;
            }

            return matrix;
        }

        public static void WriteToFile(this Matrix4x4 matrix, BinaryWriter writer)
        {
            Vector4Extenders.WriteToFile(matrix.GetColumn(0), writer);
            Vector4Extenders.WriteToFile(matrix.GetColumn(1), writer);
            Vector4Extenders.WriteToFile(matrix.GetColumn(2), writer);
        }

        public static void WriteToFile(this Matrix4x4 matrix, MemoryStream stream, bool isBigEndian)
        {
            Vector4Extenders.WriteToFile(matrix.GetColumn(0), stream, isBigEndian);
            Vector4Extenders.WriteToFile(matrix.GetColumn(1), stream, isBigEndian);
            Vector4Extenders.WriteToFile(matrix.GetColumn(2), stream, isBigEndian);
        }

        public static Matrix4x4 SetMatrix(Quaternion rotation, Vector3 scale, Vector3 position)
        {
            // Doing the normal T * R * S does not work; I have to manually push in the vector into the final row.
            Matrix4x4 r = Matrix4x4.CreateFromQuaternion(rotation);
            Matrix4x4 s = Matrix4x4.CreateScale(scale);
            Matrix4x4 final = r * s;
            final.Translation = position;

            return final;
        }

        public static Matrix4x4 SetTranslationVector(Matrix4x4 other, Vector3 position)
        {
            Matrix4x4 matrix = other;
            other.Translation = position;

            return matrix;
        }

        public static Matrix4x4 CreateFromDirection(Vector3 Direction)
        {
            Vector3 UpDirection = new Vector3(0.0f, 0.0f, 1.0f);

            Matrix4x4 NewMatrix = Matrix4x4.Identity;

            Vector3 XAxis = Vector3.Normalize(Vector3.Cross(UpDirection, Direction));
            Vector3 YAxis = Vector3.Normalize(Vector3.Cross(Direction, XAxis));

            NewMatrix.SetColumn(0, new Vector4(XAxis.X, YAxis.X, Direction.X, 1.0f));
            NewMatrix.SetColumn(1, new Vector4(XAxis.Y, YAxis.Y, Direction.Y, 1.0f));
            NewMatrix.SetColumn(2, new Vector4(XAxis.Z, YAxis.Z, Direction.Z, 1.0f));
            return NewMatrix;
        }

        public static Matrix4x4 SetMatrix(Vector3 rotation, Vector3 scale, Vector3 position)
        {
            /*float radX, radY, radZ;
            radX = -MathUtil.DegreesToRadians(rotation.X);
            radY = -MathUtil.DegreesToRadians(rotation.Y);
            radZ = -MathUtil.DegreesToRadians(rotation.Z);

            Matrix x = Matrix.RotationX(radX);
            Matrix y = Matrix.RotationY(radY);
            Matrix z = Matrix.RotationZ(radZ);

            Matrix result = x * y * z;

            Matrix fixedRotation = new Matrix();
            fixedRotation.Column1 = result.Row1;
            fixedRotation.Column2 = result.Row2;
            fixedRotation.Column3 = result.Row3;*/

            float X = MathHelper.ToRadians(rotation.X);
            float Y = MathHelper.ToRadians(rotation.Y);
            float Z = MathHelper.ToRadians(rotation.Z);

            Quaternion rotation1 = Quaternion.CreateFromYawPitchRoll(Y, X, Z);
            return SetMatrix(rotation1, scale, position);
        }
    }

    public static class QuaternionExtensions
    {
        public static Vector3 ToEuler(this Quaternion quat)
        {
            float X = quat.X;
            float Y = quat.Y;
            float Z = quat.Z;
            float W = quat.W;
            float X2 = X * 2.0f;
            float Y2 = Y * 2.0f;
            float Z2 = Z * 2.0f;
            float XX2 = X * X2;
            float XY2 = X * Y2;
            float XZ2 = X * Z2;
            float YX2 = Y * X2;
            float YY2 = Y * Y2;
            float YZ2 = Y * Z2;
            float ZX2 = Z * X2;
            float ZY2 = Z * Y2;
            float ZZ2 = Z * Z2;
            float WX2 = W * X2;
            float WY2 = W * Y2;
            float WZ2 = W * Z2;

            Vector3 AxisX, AxisY, AxisZ;
            AxisX.X = (1.0f - (YY2 + ZZ2));
            AxisY.X = (XY2 + WZ2);
            AxisZ.X = (XZ2 - WY2);
            AxisX.Y = (XY2 - WZ2);
            AxisY.Y = (1.0f - (XX2 + ZZ2));
            AxisZ.Y = (YZ2 + WX2);
            AxisX.Z = (XZ2 + WY2);
            AxisY.Z = (YZ2 - WX2);
            AxisZ.Z = (1.0f - (XX2 + YY2));

            double SmallNumber = double.Parse("1E-08", NumberStyles.Float);
            Vector3 ResultVector = new Vector3();

            ResultVector.Y = (float)Math.Asin(-MathHelper.Clamp(AxisZ.X, -1.0f, 1.0f));

            if(Math.Abs(AxisZ.X) < 1.0f - SmallNumber)
            {
                ResultVector.X = (float)Math.Atan2(AxisZ.Y, AxisZ.Z);
                ResultVector.Z = (float)Math.Atan2(AxisY.X, AxisX.X);
            }
            else
            {
                ResultVector.X = 0.0f;
                ResultVector.Z = (float)Math.Atan2(-AxisX.Y, AxisY.Y);
            }

            ResultVector.Z = MathHelper.ToDegrees(ResultVector.Z);
            ResultVector.Y = MathHelper.ToDegrees(ResultVector.Y);
            ResultVector.X = MathHelper.ToDegrees(ResultVector.X);
            return ResultVector;
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
