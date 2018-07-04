using System;
using System.ComponentModel;
using System.IO;

namespace Mafia2
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Matrix33
    {
        float[,] rows = new float[3, 3];

        Vector3 euler;

        public float[,] Rows {
            get { return rows; }
            set { rows = value; }
        }
        public Vector3 Euler {
            get { return euler; }
            set { euler = value; }
        }

        public Matrix33(Vector3 m1, Vector3 m2, Vector3 m3)
        {
            rows[0, 0] = m1.X;
            rows[0, 1] = m2.X;
            rows[0, 2] = m3.X;
            rows[1, 0] = m1.Y;
            rows[1, 1] = m2.Y;
            rows[1, 2] = m3.Y;
            rows[2, 0] = m1.Z;
            rows[2, 1] = m2.Z;
            rows[2, 2] = m3.Z;
            euler = ConvertToAngle();
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(rows[0, 0]);
            writer.Write(rows[0, 1]);
            writer.Write(rows[0, 2]);
            writer.Write(rows[1, 0]);
            writer.Write(rows[1, 1]);
            writer.Write(rows[1, 2]);
            writer.Write(rows[2, 0]);
            writer.Write(rows[2, 1]);
            writer.Write(rows[2, 2]);
        }

        public Vector3 ConvertToAngle()
        {
            double x;
            double z;

            double y = Math.Asin(rows[0,2]);

            if(Math.Abs(rows[0,2]) < 0.99999)
            {
                x = Math.Atan2(rows[1, 1], rows[2, 2]);
                z = Math.Atan2(rows[0, 1], rows[0, 2]);
            }
            else
            {
                x = Math.Atan2(rows[2, 1], rows[2, 1]);
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
