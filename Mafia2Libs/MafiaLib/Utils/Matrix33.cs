using System;
using System.ComponentModel;
using System.IO;

namespace Mafia2
{
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

        public float M00 {
            get { return m00; }
            set { m00 = value; }
        }
        public float M01 {
            get { return m01; }
            set { m01 = value; }
        }
        public float M02 {
            get { return m02; }
            set { m02 = value; }
        }
        public float M10 {
            get { return m10; }
            set { m10 = value; }
        }
        public float M11 {
            get { return m11; }
            set { m11 = value; }
        }
        public float M12 {
            get { return m12; }
            set { m12 = value; }
        }
        public float M20 {
            get { return m20; }
            set { m20 = value; }
        }
        public float M21 {
            get { return m21; }
            set { m21 = value; }
        }
        public float M22 {
            get { return m22; }
            set { m22 = value; }
        }
        public Vector3 EulerRotation {
            get { return eulerRotation; }
            set { eulerRotation = value; }
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

            return new Vector3((float)x, (float)y, (float)z);
        }

        public void UpdateMatrixFromEuler()
        {
            float x = eulerRotation.X * (float)Math.PI / 180;
            float y = eulerRotation.Y * (float)Math.PI / 180;
            float z = eulerRotation.Z * (float)Math.PI / 180;

            float ch = (float)Math.Cos(z);
            float sh = (float)Math.Sin(z);
            float ca = (float)Math.Cos(y);
            float sa = (float)Math.Sin(y);
            float cb = (float)Math.Cos(x);
            float sb = (float)Math.Sin(x);
            m00 = ch * ca;
            m01 = sh * sb - ch * sa * cb;
            m02 = ch * sa * sb + sh * cb;
            m10 = sa;
            m11 = ca * cb;
            m12 = -ca * sb;
            m20 = -sh * ca;
            m21 = sh * sa * cb + ch * sb;
            m22 = -sh * sa * sb + ch * cb;

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
            return $"Matrix";
        }

        // Returns a matrix with all elements set to zero (RO).
        public static Matrix33 identity { get; } = new Matrix33(
           new Vector3(1, 0, 0),
           new Vector3(0, 1, 0),
           new Vector3(0, 0, 1),
           true);
    }
}
