using Mafia2;
using System;
using System.ComponentModel;

namespace Mafia2
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Matrix33
    {
        Vector3 m1;
        Vector3 m2;
        Vector3 m3;

        Vector3 angle;

        public Vector3 M1 {
            get { return m1; }
            set { m1 = value; }
        }
        public Vector3 M2 {
            get { return m2; }
            set { m2 = value; }
        }
        public Vector3 M3 {
            get { return m3; }
            set { m3 = value; }
        }
        public Vector3 Vector {
            get { return angle; }
            set { angle = value; }
        }

        public Matrix33(Vector3 m1, Vector3 m2, Vector3 m3)
        {
            this.m1 = m1;
            this.m2 = m2;
            this.m3 = m3;
            angle = ConvertToAngle();
        }

        public Vector3 ConvertToAngle()
        {
            double x;
            double z;

            double y = Math.Asin(m1.Z);

            if(Math.Abs(m1.Z) < 0.99999)
            {
                x = Math.Atan2(-m2.Z, m3.Z);
                z = Math.Atan2(-m1.Y, m1.X);
            }
            else
            {
                x = Math.Atan2(m3.Y, m2.Y);
                z = 0;
            }

            //BLENDER USES RADIANS, MAX USES DEGREES
            x = x * 180 / Math.PI;
            y = y * 180 / Math.PI;
            z = z * 180 / Math.PI;

            return new Vector3((float)x, (float)y, (float)z);
        }

        public override string ToString()
        {
            return string.Format("{0},", Vector);
        }


    }
}
