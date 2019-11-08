using SharpDX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Extensions;
using Utils.SharpDXExtensions;

namespace Utils.Types
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class TransformMatrix
    {
        private Vector3 position;
        private Vector3 scale;
        private Matrix33 rotation;
        private Matrix33 transformedMatrix;

        public Matrix33 Matrix { get { return GetTransformed(); } }

        [TypeConverter(typeof(Vector3Converter))]
        public Vector3 Position { get { return position; } set { position = value; } }
        [TypeConverter(typeof(Vector3Converter))]
        public Vector3 Rotation { get { return GetEulerRotation(); } set { SetRotationMatrix(value); } }
        [TypeConverter(typeof(Vector3Converter))]
        public Vector3 Scale { get { return scale; } set { scale = value; SetTransformed(); } }


        public TransformMatrix()
        {
            Position = new Vector3(0);
            rotation = new Matrix33();
            SetTransformed();
        }

        public TransformMatrix(BinaryReader reader)
        {
            ReadFromFile(reader);
            SetTransformed();
        }

        public TransformMatrix(MemoryStream reader, bool isBigEndian)
        {
            ReadFromFile(reader, isBigEndian);
            SetTransformed();
        }

        public TransformMatrix(TransformMatrix other)
        {
            rotation = new Matrix33(other.rotation);
            scale = other.scale;
            SetTransformed();
            Position = new Vector3(other.Position.X, other.Position.Y, other.Position.Z);
        }

        public void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            Vector3 m1 = Vector3Extenders.ReadFromFile(reader, isBigEndian);
            float x = reader.ReadSingle(isBigEndian);
            Vector3 m2 = Vector3Extenders.ReadFromFile(reader, isBigEndian);
            float y = reader.ReadSingle(isBigEndian);
            Vector3 m3 = Vector3Extenders.ReadFromFile(reader, isBigEndian);
            float z = reader.ReadSingle(isBigEndian);
            transformedMatrix = new Matrix33(m1, m2, m3);
            Decompose();
            Position = new Vector3(x, y, z);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            Vector3 m1 = Vector3Extenders.ReadFromFile(reader);
            float x = reader.ReadSingle();
            Vector3 m2 = Vector3Extenders.ReadFromFile(reader);
            float y = reader.ReadSingle();
            Vector3 m3 = Vector3Extenders.ReadFromFile(reader);
            float z = reader.ReadSingle();
            transformedMatrix = new Matrix33(m1, m2, m3);
            Decompose();
            Position = new Vector3(x, y, z);
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(Matrix.M00);
            writer.Write(Matrix.M10);
            writer.Write(Matrix.M20);
            writer.Write(Position.X);
            writer.Write(Matrix.M01);
            writer.Write(Matrix.M11);
            writer.Write(Matrix.M21);
            writer.Write(Position.Y);
            writer.Write(Matrix.M02);
            writer.Write(Matrix.M12);
            writer.Write(Matrix.M22);
            writer.Write(Position.Z);
        }

        public void WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            writer.Write(Matrix.M00, isBigEndian);
            writer.Write(Matrix.M10, isBigEndian);
            writer.Write(Matrix.M20, isBigEndian);
            writer.Write(Position.X, isBigEndian);
            writer.Write(Matrix.M01, isBigEndian);
            writer.Write(Matrix.M11, isBigEndian);
            writer.Write(Matrix.M21, isBigEndian);
            writer.Write(Position.Y, isBigEndian);
            writer.Write(Matrix.M02, isBigEndian);
            writer.Write(Matrix.M12, isBigEndian);
            writer.Write(Matrix.M22, isBigEndian);
            writer.Write(Position.Z, isBigEndian);
        }

        public void SetRotationMatrix(Vector3 vector)
        {
            rotation = new Matrix33();
            rotation.SetEuler(vector);
            SetTransformed();
        }

        private Vector3 GetEulerRotation()
        {
            return transformedMatrix.ToEuler();
        }

        private void SetTransformed()
        {
            if (rotation != null && scale != null)
            {
                transformedMatrix = new Matrix33();
                transformedMatrix = rotation;
                transformedMatrix.M00 *= scale.X;
                transformedMatrix.M11 *= scale.Y;
                transformedMatrix.M22 *= scale.Z;
            }
        }

        private Matrix33 GetTransformed()
        {
            return transformedMatrix;
        }

        private void Decompose()
        {
            //retrieve scale first.
            scale = new Vector3();
            scale.X = (float)Math.Sqrt((Matrix.M00 * Matrix.M00) + (Matrix.M01 * Matrix.M01) + (Matrix.M02 * Matrix.M02));
            scale.Y = (float)Math.Sqrt((Matrix.M10 * Matrix.M10) + (Matrix.M11 * Matrix.M11) + (Matrix.M12 * Matrix.M12));
            scale.Z = (float)Math.Sqrt((Matrix.M20 * Matrix.M20) + (Matrix.M21 * Matrix.M21) + (Matrix.M22 * Matrix.M22));

            //when we divide out the scale we just have the rotation left.
            Matrix33 rotation = new Matrix33();
            rotation.M00 = Matrix.M00 / scale.X;
            rotation.M01 = Matrix.M01 / scale.X;
            rotation.M02 = Matrix.M02 / scale.X;
            rotation.M10 = Matrix.M10 / scale.Y;
            rotation.M11 = Matrix.M11 / scale.Y;
            rotation.M12 = Matrix.M12 / scale.Y;
            rotation.M20 = Matrix.M20 / scale.Z;
            rotation.M21 = Matrix.M21 / scale.Z;
            rotation.M22 = Matrix.M22 / scale.Z;
            this.rotation = rotation;
        }

        public static TransformMatrix operator +(TransformMatrix left, TransformMatrix right)
        {
            TransformMatrix tm = new TransformMatrix();
            tm.rotation = left.rotation + right.rotation;
            tm.position = left.position + right.position;
            tm.scale = left.scale * right.scale;
            tm.SetTransformed();
            return tm;
        }

        public static TransformMatrix operator *(TransformMatrix left, TransformMatrix right)
        {
            TransformMatrix tm = new TransformMatrix();
            tm.rotation = left.rotation * right.rotation;
            tm.position = left.position + right.position;
            tm.scale = left.scale * right.scale;
            tm.SetTransformed();
            return tm;
        }

        public override string ToString()
        {
            return "Transformation";
        }
    }
}
