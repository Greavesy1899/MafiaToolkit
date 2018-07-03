using System;
using System.ComponentModel;
using System.IO;

namespace Mafia2
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Matrix33
    {
        Vector3 row1;
        Vector3 row2;
        Vector3 row3;

        Vector3 euler;

        public Vector3 Row1 {
            get { return row1; }
            set { row1 = value; }
        }
        public Vector3 Row2 {
            get { return row2; }
            set { row2 = value; }
        }
        public Vector3 Row3 {
            get { return row3; }
            set { row3 = value; }
        }
        public Vector3 Euler {
            get { return euler; }
            set { euler = value; }
        }

        public Matrix33(Vector3 m1, Vector3 m2, Vector3 m3)
        {
            this.row1 = m1;
            this.row2 = m2;
            this.row3 = m3;
            euler = ConvertToAngle();
        }

        public void WriteToFile(BinaryWriter writer)
        {
            row1.WriteToFile(writer);
            row1.WriteToFile(writer);
            row1.WriteToFile(writer);
        }

        public Vector3 ConvertToAngle()
        {
            double x;
            double z;

            double y = Math.Asin(row1.Z);

            if(Math.Abs(row1.Z) < 0.99999)
            {
                x = Math.Atan2(-row2.Z, row3.Z);
                z = Math.Atan2(-row1.Y, row1.X);
            }
            else
            {
                x = Math.Atan2(row3.Y, row2.Y);
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
            return string.Format("{0},", Euler);
        }


    }
}
