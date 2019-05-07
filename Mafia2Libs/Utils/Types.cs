using Gibbed.Illusion.FileFormats.Hashing;
using SharpDX;
using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using Utils.Models;
using Utils.SharpDXExtensions;

namespace Utils.Types
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Hash
    {
        ulong hash;
        string _hex;
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
            get { return _hex; }
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
        public Hash(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        /// <summary>
        /// read name from file.
        /// </summary>
        /// <param name="reader"></param>
        public void ReadFromFile(BinaryReader reader)
        {
            hash = reader.ReadUInt64();
            size = reader.ReadInt16();
            _string = Encoding.ASCII.GetString(reader.ReadBytes(size));
            _hex = string.Format("{0:X8}", hash);
        }

        /// <summary>
        /// save hash to file.
        /// </summary>
        /// <param name="writer"></param>
        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(hash);
            writer.Write(size);
            writer.Write(_string.ToCharArray());
        }

        /// <summary>
        /// Sets hash name and updates hash automatically.
        /// </summary>
        /// <param name="name"></param>
        public void Set(string name)
        {
            _string = name;
            size = (short)name.Length;
            hash = FNV64.Hash(name);
            _hex = string.Format("{0:X}", hash);
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
        private Vector3 eulerRotation;

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

        [Browsable(false)]
        public Vector3 EulerRotation {
            get { return eulerRotation; }
            set { eulerRotation = value; }
        }

        public float RotationX {
            get { return eulerRotation.X; }
            set { eulerRotation.X = value; }
        }
        public float RotationY {
            get { return eulerRotation.X; }
            set { eulerRotation.X = value; }
        }
        public float RotationZ {
            get { return eulerRotation.X; }
            set { eulerRotation.X = value; }
        }

        /// <summary>
        /// Construct Matrix33 from three vectors.
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <param name="m3"></param>
        public Matrix33(Vector3 m1, Vector3 m2, Vector3 m3, bool rowMajor)
        {
            if (rowMajor)
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
            else
            {
                m00 = m1.X;
                m10 = m2.X;
                m20 = m3.X;
                m01 = m1.Y;
                m11 = m2.Y;
                m21 = m3.Y;
                m02 = m1.Z;
                m12 = m2.Z;
                m22 = m3.Z;
            }
            eulerRotation = ToEuler();
        }

        /// <summary>
        /// Constructs empty Matrix33.
        /// </summary>
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
            eulerRotation = ToEuler();
        }

        /// <summary>
        /// Write matrix to file.
        /// </summary>
        /// <param name="writer"></param>
        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(m00);
            writer.Write(m01);
            writer.Write(m02);
            writer.Write(m10);
            writer.Write(m11);
            writer.Write(m12);
            writer.Write(m20);
            writer.Write(m21);
            writer.Write(m22);
        }

        /// <summary>
        /// Convert matrix to euler.
        /// </summary>
        /// <returns></returns>
        public Vector3 ToEuler()
        {
            double x;
            double z;
            double y = Math.Asin(m02);

            if (Math.Abs(m02) < 0.99999)
            {
                x = Math.Atan2(m12, m22);
                z = Math.Atan2(m01, m00);
            }
            else
            {
                x = Math.Atan2(m21, m11);
                z = 0;
            }

            //BLENDER USES RADIANS, MAX USES DEGREES
            x = x * 180 / Math.PI;
            y = y * 180 / Math.PI;
            z = z * 180 / Math.PI;

            x = double.IsNaN(x) ? 0 : x;
            y = double.IsNaN(y) ? 0 : y;
            z = double.IsNaN(z) ? 0 : z;
            return new Vector3((float)x, (float)y, (float)z);
        }

        public void UpdateMatrixFromEuler()
        {
            //x == roll
            //y == pitch
            //z == yaw

            float roll = eulerRotation.X * (float)Math.PI / 180;
            float pitch = eulerRotation.Y * (float)Math.PI / 180;
            float yaw = eulerRotation.Z * (float)Math.PI / 180;

            float su = (float)Math.Sin(roll);
            float cu = (float)Math.Cos(roll);
            float sv = (float)Math.Sin(pitch);
            float cv = (float)Math.Cos(pitch);
            float sw = (float)Math.Sin(yaw);
            float cw = (float)Math.Cos(yaw);
            m00 = cv * cw;
            m10 = su * sv * cw - cu * sw;
            m20 = su * sw + cu * sv * cw;
            m01 = cv * sw;
            m11 = cu * cw + su * sv * sw;
            m21 = cu * sv * sw - su * cw;
            m02 = -sv;
            m12 = su * cv;
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

        public Matrix33 ChangeHandedness()
        {
            Matrix33 temp = this;
            m00 = temp.m00;
            m01 = temp.m10;
            m02 = temp.m20;
            m10 = temp.m01;
            m11 = temp.m11;
            m12 = temp.m21;
            m20 = temp.m02;
            m21 = temp.m12;
            m22 = temp.m22;

            return this;
        }

        public override string ToString()
        {
            return eulerRotation.ToString();
        }

        // Returns a matrix with all elements set to zero (RO).
        public static Matrix33 identity { get; } = new Matrix33(
           new Vector3(1, 0, 0),
           new Vector3(0, 1, 0),
           new Vector3(0, 0, 1),
           true);
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

        public Matrix33 Rotation { get; set; }

        [Browsable(false)]
        public Vector3 Position { get { return position; } set { position = value; } }

        public float PositionX {
            get { return position.X; }
            set { position.X = value; }
        }
        public float PositionY {
            get { return position.Y; }
            set { position.Y = value; }
        }
        public float PositionZ {
            get { return position.Z; }
            set { position.Z = value; }
        }

        /// <summary>
        /// Construct empty TransformMatrix.
        /// </summary>
        public TransformMatrix()
        {
            Position = new Vector3(0);
            Rotation = new Matrix33();
        }

        /// <summary>
        /// Construct TransformMatrix from parsed data.
        /// </summary>
        /// <param name="reader"></param>
        public TransformMatrix(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public TransformMatrix(TransformMatrix other)
        {
            Rotation = new Matrix33();
            Rotation.M00 = other.Rotation.M00;
            Rotation.M01 = other.Rotation.M01;
            Rotation.M02 = other.Rotation.M02;
            Rotation.M10 = other.Rotation.M10;
            Rotation.M11 = other.Rotation.M11;
            Rotation.M12 = other.Rotation.M12;
            Rotation.M20 = other.Rotation.M20;
            Rotation.M21 = other.Rotation.M21;
            Rotation.M22 = other.Rotation.M22;
            Rotation.EulerRotation = new Vector3(other.Rotation.EulerRotation.X, other.Rotation.EulerRotation.Y, other.Rotation.EulerRotation.Z);
            Position = new Vector3(other.Position.X, other.Position.Y, other.Position.Z);
        }

        /// <summary>
        /// Read TransformMatrix from the file.
        /// </summary>
        /// <param name="reader"></param>
        public void ReadFromFile(BinaryReader reader)
        {
            Vector3 m1 = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            float x = reader.ReadSingle();
            Vector3 m2 = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            float y = reader.ReadSingle();
            Vector3 m3 = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            float z = reader.ReadSingle();

            Rotation = new Matrix33(m1, m2, m3, true);
            Position = new Vector3(x, y, z);
        }

        /// <summary>
        /// Used for Max.
        /// </summary>
        /// <param name="writer"></param>
        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(Rotation.M00);
            writer.Write(Rotation.M01);
            writer.Write(Rotation.M02);
            writer.Write(Position.X);
            writer.Write(Rotation.M10);
            writer.Write(Rotation.M11);
            writer.Write(Rotation.M12);
            writer.Write(Position.Y);
            writer.Write(Rotation.M20);
            writer.Write(Rotation.M21);
            writer.Write(Rotation.M22);
            writer.Write(Position.Z);
        }

        /// <summary>
        /// Use this to write to the FrameResource.
        /// </summary>
        /// <param name="writer"></param>
        public void WriteToFrame(BinaryWriter writer)
        {
            writer.Write(Rotation.M00);
            writer.Write(Rotation.M10);
            writer.Write(Rotation.M20);
            writer.Write(Position.X);
            writer.Write(Rotation.M01);
            writer.Write(Rotation.M11);
            writer.Write(Rotation.M21);
            writer.Write(Position.Y);
            writer.Write(Rotation.M02);
            writer.Write(Rotation.M12);
            writer.Write(Rotation.M22);
            writer.Write(Position.Z);
        }

        public override string ToString()
        {
            return $"{Rotation} {Position}";
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
        /// Build Short3 from Int3
        /// </summary>
        /// <param name="ints"></param>
        public Short3(Int3 ints)
        {
            S1 = (ushort)ints.I1;
            S2 = (ushort)ints.I2;
            S3 = (ushort)ints.I3;
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

    public class Int3
    {
        public int I1 { get; set; }
        public int I2 { get; set; }
        public int I3 { get; set; }

        /// <summary>
        /// Construct Int3 from file data.
        /// </summary>
        /// <param name="reader"></param>
        public Int3(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        /// <summary>
        /// Build Int3 from Short3
        /// </summary>
        /// <param name="ints"></param>
        public Int3(Short3 s3)
        {
            I1 = (int)s3.S1;
            I2 = (int)s3.S2;
            I3 = (int)s3.S3;
        }

        /// <summary>
        /// read data from file.
        /// </summary>
        /// <param name="reader"></param>
        public void ReadFromFile(BinaryReader reader)
        {
            I1 = reader.ReadInt32();
            I2 = reader.ReadInt32();
            I3 = reader.ReadInt32();
        }

        /// <summary>
        /// write data to file
        /// </summary>
        /// <param name="writer"></param>
        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(I1);
            writer.Write(I2);
            writer.Write(I3);
        }

        public override string ToString()
        {
            return $"{I1} {I2} {I3}";
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Polygon
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
            normal = Vector3Extenders.ReadFromFile(reader);
            floats = new float[] { reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle() };
        }
    }
}
