using System;

namespace Mafia2
{
    public struct Vector3
    {
        float x;
        float y;
        float z;

        public float X {
            get { return x; }
            set { x = value; }
        }
        public float Y {
            get { return y; }
            set { y = value; }
        }
        public float Z {
            get { return z; }
            set { z = value; }
        }

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public void Normalize()
        {
            float num1 = (float)(X * (double)X + Y * Y + Z * (double)Z);
            float num2 = num1 == 0.0 ? float.MaxValue : (float)(1.0 / Math.Sqrt(num1));
            X *= num2;
            Y *= num2;
            Z *= num2;
        }

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public override string ToString()
        {
            return string.Format("X: {0}, Y: {1}, Z: {2}", x, y, z);
        }

    }

    public struct Vector2
    {
        float x;
        float y;

        public float X {
            get { return x; }
            set { x = value; }
        }
        public float Y {
            get { return y; }
            set { y = value; }
        }

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        public override string ToString()
        {
            return string.Format("X: {0}, Y: {1}", x, y);
        }
    }

    public struct UVVector2
    {
        Half x;
        Half y;

        public Half X {
            get { return x; }
            set { x = value; }
        }
        public Half Y {
            get { return y; }
            set { y = value; }
        }

        public UVVector2(Half x, Half y)
        {
            this.x = x;
            this.y = y;
        }
        public override string ToString()
        {
            return string.Format("X: {0}, Y: {1}", x, y);
        }
    }
}
