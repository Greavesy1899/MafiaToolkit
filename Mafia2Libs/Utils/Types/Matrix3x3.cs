using SharpDX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Types
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

        public Vector3 Row1 {
            get { return new Vector3(m00, m01, m02); }
            set { m00 = value.X; m01 = value.Y; m02 = value.Z; }
        }
        public Vector3 Row2 {
            get { return new Vector3(m10, m11, m12); }
            set { m10 = value.X; m11 = value.Y; m12 = value.Z; }
        }
        public Vector3 Row3 {
            get { return new Vector3(m20, m21, m22); }
            set { m20 = value.X; m21 = value.Y; m22 = value.Z; }
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
            Row1 = new Vector3(m1.X, m2.X, m3.X);
            Row2 = new Vector3(m1.Y, m2.Y, m3.Y);
            Row3 = new Vector3(m1.Z, m2.Z, m3.Z);
        }

        public Matrix33()
        {
            Row1 = new Vector3(1, 0, 0);
            Row2 = new Vector3(0, 1, 0);
            Row3 = new Vector3(0, 0, 1);
        }

        public Matrix33(Matrix33 other)
        {
            Row1 = new Vector3(other.m00, other.m01, other.m02);
            Row2 = new Vector3(other.m10, other.m11, other.m12);
            Row3 = new Vector3(other.m20, other.m21, other.m22);
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

        public static Matrix33 operator +(Matrix33 matrix1, Matrix33 matrix2)
        {
            Matrix33 matrix = new Matrix33();
            matrix.m00 = matrix1.M00 + matrix2.m00;
            matrix.m01 = matrix1.M01 + matrix2.m01;
            matrix.m02 = matrix1.M02 + matrix2.m02;
            matrix.m10 = matrix1.M10 + matrix2.m10;
            matrix.m11 = matrix1.M11 + matrix2.m11;
            matrix.m12 = matrix1.M12 + matrix2.m12;
            matrix.m20 = matrix1.M20 + matrix2.m20;
            matrix.m21 = matrix1.M21 + matrix2.m21;
            matrix.m22 = matrix1.M22 + matrix2.m22;
            matrix.ToEuler();
            return matrix;
        }

        public static Matrix33 operator *(Matrix33 left, Matrix33 right)
        {
            Matrix33 temp = new Matrix33();
            temp.M00 = (left.M00 * right.M00) + (left.M01 * right.M10) + (left.M02 * right.M20);
            temp.M01 = (left.M00 * right.M01) + (left.M01 * right.M11) + (left.M02 * right.M21);
            temp.M02 = (left.M00 * right.M02) + (left.M01 * right.M12) + (left.M02 * right.M22);
            temp.M10 = (left.M10 * right.M00) + (left.M11 * right.M10) + (left.M12 * right.M20);
            temp.M11 = (left.M10 * right.M01) + (left.M11 * right.M11) + (left.M12 * right.M21);
            temp.M12 = (left.M10 * right.M02) + (left.M11 * right.M12) + (left.M12 * right.M22);
            temp.M20 = (left.M20 * right.M00) + (left.M21 * right.M10) + (left.M22 * right.M20);
            temp.M21 = (left.M20 * right.M01) + (left.M21 * right.M11) + (left.M22 * right.M21);
            temp.M22 = (left.M20 * right.M02) + (left.M21 * right.M12) + (left.M22 * right.M22);
            return temp;
        }

        public override string ToString()
        {
            return "Matrix";
        }

        // Returns a matrix with all elements set to zero (RO).
        public static Matrix33 Identity { get; } = new Matrix33(
           new Vector3(1, 0, 0),
           new Vector3(0, 1, 0),
           new Vector3(0, 0, 1));
    }
}
