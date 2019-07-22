using Gibbed.Illusion.FileFormats.Hashing;
using SharpDX;
using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using Utils.Extensions;
using Utils.Models;

namespace Utils.Types
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Hash
    {
        ulong hash;
        string _string;
        short size;

        [ReadOnly(true)]
        public ulong uHash {
            get { return hash; }
            set { hash = value; }
        }
        public string String {
            get { return _string; }
            set { Set(value); }
        }

        [ReadOnly(true)]
        public string Hex {
            get { return string.Format("{0:X}", hash); }
        }
        public Hash()
        {
            _string = "";
            hash = 0;
        }
        public Hash(string name)
        {
            Set(name);
        }
        public Hash(Hash other)
        {
            this.hash = other.hash;
            this._string = other._string;
        }
        public Hash(BinaryReader reader)
        {
            ReadFromFile(reader);
        }
        public Hash(MemoryStream stream, bool isBigEndian)
        {
            ReadFromFile(stream, isBigEndian);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            hash = reader.ReadUInt64();
            size = reader.ReadInt16();
            _string = Encoding.ASCII.GetString(reader.ReadBytes(size));
        }
        public void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            hash = stream.ReadUInt64(isBigEndian);
            size = stream.ReadInt16(isBigEndian);
            _string = Encoding.ASCII.GetString(stream.ReadBytes(size));
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(hash);
            writer.Write(size);
            writer.Write(_string.ToCharArray());
        }

        public void WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            writer.Write(hash, isBigEndian);
            writer.Write(size, isBigEndian);
            writer.Write(_string.ToCharArray());
        }

        public void Set(string name)
        {
            _string = name;
            size = (short)name.Length;

            if (_string == "")
                hash = 0;
            else
                hash = FNV64.Hash(name);
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(_string))
                return ((SkeletonBoneIDs)hash).ToString();
            else
                return _string;
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Matrix33
    {
        private float m00;
        private float m01;
        private float m02;
        private float m10;
        private float m11;
        private float m12;
        private float m20;
        private float m21;
        private float m22;

        public Vector3 Row1 {
            get { return new Vector3(m00, m01, m02); }
        }
        public Vector3 Row2 {
            get { return new Vector3(m10, m11, m12); }
        }
        public Vector3 Row3 {
            get { return new Vector3(m20, m21, m22); }
        }
        [Browsable(false)]
        public float M00 {
            get { return m00; }
            set { m00 = value; }
        }
        [Browsable(false)]
        public float M01 {
            get { return m01; }
            set { m01 = value; }
        }
        [Browsable(false)]
        public float M02 {
            get { return m02; }
            set { m02 = value; }
        }
        [Browsable(false)]
        public float M10 {
            get { return m10; }
            set { m10 = value; }
        }
        [Browsable(false)]
        public float M11 {
            get { return m11; }
            set { m11 = value; }
        }
        [Browsable(false)]
        public float M12 {
            get { return m12; }
            set { m12 = value; }
        }
        [Browsable(false)]
        public float M20 {
            get { return m20; }
            set { m20 = value; }
        }
        [Browsable(false)]
        public float M21 {
            get { return m21; }
            set { m21 = value; }
        }
        [Browsable(false)]
        public float M22 {
            get { return m22; }
            set { m22 = value; }
        }

        public Matrix33(Vector3 m1, Vector3 m2, Vector3 m3)
        {
            m00 = m1.X;
            m01 = m2.X;
            m02 = m3.X;
            m10 = m1.Y;
            m11 = m2.Y;
            m12 = m3.Y;
            m20 = m1.Z;
            m21 = m2.Z;
            m22 = m3.Z;
        }

        public Matrix33()
        {
            m00 = 1;
            m01 = 0;
            m02 = 0;
            m10 = 0;
            m11 = 1;
            m12 = 0;
            m20 = 0;
            m21 = 0;
            m22 = 1;
        }

        public Matrix33(Matrix33 other)
        {
            m00 = other.m00;
            m01 = other.m01;
            m02 = other.m02;
            m10 = other.m10;
            m11 = other.m11;
            m12 = other.m12;
            m20 = other.m20;
            m21 = other.m21;
            m22 = other.m22;
        }

        public void SetEuler(Vector3 vector)
        {
            UpdateMatrixFromEuler(vector);
        }

        public Vector3 ToEuler()
        {
            Vector3 rotation = new Vector3();
            rotation.Y = (float)-Math.Asin(m20);

            //Gymbal lock: pitch = -90
            if (m02 == 1)
            {
                rotation.Z = 0.0f;
                rotation.X = (float)Math.Atan2(-m01, -m02);
            }

            //Gymbal lock: pitch = 90
            else if (m20 == -1)
            {
                rotation.Z = 0.0f;
                rotation.X = (float)Math.Atan2(m01, m02);
            }
            //General solution
            else
            {
                rotation.Z = (float)Math.Atan2(m10, m00);
                rotation.X = (float)Math.Atan2(m21, m22);
            }
            rotation.X = MathUtil.RadiansToDegrees(rotation.X);
            rotation.Y = MathUtil.RadiansToDegrees(rotation.Y);
            rotation.Z = MathUtil.RadiansToDegrees(rotation.Z);
            rotation.X = float.IsNaN(rotation.X) ? 0.0f : rotation.X;
            rotation.Y = float.IsNaN(rotation.Y) ? 0.0f : rotation.Y;
            rotation.Z = float.IsNaN(rotation.Z) ? 0.0f : rotation.Z;

            return rotation;
        }

        private void UpdateMatrixFromEuler(Vector3 vector)
        {
            //x == roll
            //y == pitch
            //z == yaw

            float roll = MathUtil.DegreesToRadians(vector.X);
            float pitch = MathUtil.DegreesToRadians(vector.Y);
            float yaw = MathUtil.DegreesToRadians(vector.Z);

            float su = (float)Math.Sin(roll);
            float cu = (float)Math.Cos(roll);
            float sv = (float)Math.Sin(pitch);
            float cv = (float)Math.Cos(pitch);
            float sw = (float)Math.Sin(yaw);
            float cw = (float)Math.Cos(yaw);
            m00 = cv * cw;
            m01 = su * sv * cw - cu * sw;
            m02 = su * sw + cu * sv * cw;
            m10 = cv * sw;
            m11 = cu * cw + su * sv * sw;
            m12 = cu * sv * sw - su * cw;
            m20 = -sv;
            m21 = su * cv;
            m22 = cu * cv;

            //Remove exponents.
            if (m00 > -0.01f && m00 < 0.01f)
                m00 = 0.0f;

            if (m01 > -0.01f && m01 < 0.01f)
                m01 = 0.0f;

            if (m02 > -0.01f && m02 < 0.01f)
                m02 = 0.0f;

            if (m10 > -0.01f && m10 < 0.01f)
                m10 = 0.0f;

            if (m11 > -0.01f && m11 < 0.01f)
                m11 = 0.0f;

            if (m12 > -0.01f && m12 < 0.01f)
                m12 = 0.0f;

            if (m20 > -0.01f && m20 < 0.01f)
                m20 = 0.0f;

            if (m21 > -0.01f && m21 < 0.01f)
                m21 = 0.0f;

            if (m22 > -0.01f && m22 < 0.01f)
                m22 = 0.0f;
        }
        
        public static Matrix33 operator *(Matrix33 matrix1, Matrix33 matrix2)
        {
            Matrix33 matrix = new Matrix33();
            matrix.m00 = matrix1.m00 * matrix2.m00;
            matrix.m01 = matrix1.m01 * matrix2.m01;
            matrix.m02 = matrix1.m02 * matrix2.m02;
            matrix.m10 = matrix1.m10 * matrix2.m10;
            matrix.m11 = matrix1.m11 * matrix2.m11;
            matrix.m12 = matrix1.m12 * matrix2.m12;
            matrix.m20 = matrix1.m20 * matrix2.m20;
            matrix.m21 = matrix1.m21 * matrix2.m21;
            matrix.m22 = matrix1.m22 * matrix2.m22;
            return matrix;
        }

        public static Matrix33 operator +(Matrix33 matrix1, Matrix33 matrix2)
        {
            Matrix33 matrix = new Matrix33();
            matrix.m00 = 1.0f;
            matrix.m01 = matrix1.M01 + matrix2.m01;
            matrix.m02 = matrix1.M02 + matrix2.m02;
            matrix.m10 = matrix1.M10 + matrix2.m10;
            matrix.m11 = 1.0f;
            matrix.m12 = matrix1.M12 + matrix2.m12;
            matrix.m20 = matrix1.M20 + matrix2.m20;
            matrix.m21 = matrix1.M21 + matrix2.m21;
            matrix.m22 = 1.0f;
            matrix.ToEuler();
            return matrix;
        }

        public override string ToString()
        {
            return "Matrix";
        }

        // Returns a matrix with all elements set to zero (RO).
        public static Matrix33 identity { get; } = new Matrix33(
           new Vector3(1, 0, 0),
           new Vector3(0, 1, 0),
           new Vector3(0, 0, 1));
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ParentStruct
    {
        int index;
        string name;
        int refID;

        public int Index {
            get { return index; }
            set { index = value; }
        }

        public string Name {
            get { return name; }
            set { name = value; }
        }

        public int RefID {
            get { return refID; }
            set { refID = value; }
        }

        public ParentStruct(int index)
        {
            this.index = index;
        }

        public ParentStruct(ParentStruct other)
        {
            index = other.index;
            name = other.name;
            refID = other.refID;
        }

        public override string ToString()
        {
            if (index == -1)
                return string.Format("{0}, root", index);
            else
                return string.Format("{0}. {1}", index, name);
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class TransformMatrix
    {
        private Vector3 position;
        private Matrix33 scale;
        private Matrix33 rotation;
        private Matrix33 transformedMatrix;

        public Matrix33 Matrix { get { return GetTransformed(); }}

        public Vector3 Position { get { return position; } set { position = value; } }
        public Vector3 Rotation { get { return GetEulerRotation(); } set { SetRotationMatrix(value); } }
        public Vector3 Scale { get { return GetScale(); } set { SetScaleMatrix(value); } }
        

        public TransformMatrix()
        {
            Position = new Vector3(0);
            scale = new Matrix33();
            rotation = new Matrix33();
            SetTransformed();
        }

        public TransformMatrix(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public TransformMatrix(TransformMatrix other)
        {
            rotation = new Matrix33(other.rotation);
            scale = new Matrix33(other.scale);
            SetTransformed();
            Position = new Vector3(other.Position.X, other.Position.Y, other.Position.Z);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            Vector3 m1 = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            float x = reader.ReadSingle();
            Vector3 m2 = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            float y = reader.ReadSingle();
            Vector3 m3 = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            float z = reader.ReadSingle();
            transformedMatrix = new Matrix33(m1, m2, m3);
            scale = new Matrix33(new Vector3(m1.Length(), 0.0f, 0.0f), new Vector3(0.0f, m2.Length(), 0.0f), new Vector3(0.0f, 0.0f, m3.Length()));
            rotation = new Matrix33();
            transformedMatrix.M00 = transformedMatrix.M00 / scale.M00;
            transformedMatrix.M01 = transformedMatrix.M01 / scale.M00;
            transformedMatrix.M02 = transformedMatrix.M02 / scale.M00;
            transformedMatrix.M10 = transformedMatrix.M10 / scale.M11;
            transformedMatrix.M11 = transformedMatrix.M11 / scale.M11;
            transformedMatrix.M12 = transformedMatrix.M12 / scale.M11;
            transformedMatrix.M20 = transformedMatrix.M20 / scale.M22;
            transformedMatrix.M21 = transformedMatrix.M21 / scale.M22;
            transformedMatrix.M22 = transformedMatrix.M22 / scale.M22;
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

        public void SetScaleMatrix(Vector3 vector)
        {
            scale = new Matrix33(new Vector3(vector.X, 0.0f, 0.0f), new Vector3(0.0f, vector.Y, 0.0f), new Vector3(0.0f, 0.0f, vector.Z));
            SetTransformed();
        }

        private Vector3 GetScale()
        {
            return new Vector3(transformedMatrix.M00, transformedMatrix.M11, transformedMatrix.M22);
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
                transformedMatrix = new Matrix33(rotation);
                transformedMatrix.M00 *= scale.M00;
                transformedMatrix.M11 *= scale.M11;
                transformedMatrix.M22 *= scale.M22;
            }
        }

        private Matrix33 GetTransformed()
        {
            return transformedMatrix;
        }

        public static TransformMatrix operator +(TransformMatrix matrix1, TransformMatrix matrix2)
        {
            TransformMatrix matrix = new TransformMatrix();
            matrix.position = matrix1.position + matrix2.position;
            matrix.scale = matrix1.scale + matrix2.scale;
            matrix.rotation = matrix1.rotation + matrix2.rotation;
            matrix.transformedMatrix = matrix1.transformedMatrix + matrix2.transformedMatrix;
            return matrix;
        }

        public override string ToString()
        {
            return $"{Matrix} {Position}";
        }
    }

    public class Short3
    {
        public ushort S1 { get; set; }
        public ushort S2 { get; set; }
        public ushort S3 { get; set; }

        /// <summary>
        /// SET all values to -100
        /// </summary>
        public Short3()
        {
            S1 = ushort.MaxValue;
            S2 = ushort.MaxValue;
            S3 = ushort.MaxValue;
        }

        public Short3(Short3 other)
        {
            S1 = other.S1;
            S2 = other.S2;
            S3 = other.S3;
        }

        /// <summary>
        /// Construct Short3 from file data.
        /// </summary>
        /// <param name="reader"></param>
        public Short3(BinaryReader reader)
        {
            S1 = reader.ReadUInt16();
            S2 = reader.ReadUInt16();
            S3 = reader.ReadUInt16();
        }

        /// <summary>
        /// Construct Short3 from three integers.
        /// </summary>
        /// <param name="i1"></param>
        /// <param name="i2"></param>
        /// <param name="i3"></param>
        public Short3(int i1, int i2, int i3)
        {
            S1 = (ushort)i1;
            S2 = (ushort)i2;
            S3 = (ushort)i3;
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
}
