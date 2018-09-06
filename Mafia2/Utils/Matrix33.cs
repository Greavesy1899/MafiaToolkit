using System;
using System.ComponentModel;
using System.IO;

namespace Mafia2
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Matrix33
    {
        public float[,] Rows { get; set; } = new float[3, 3];
        public Vector3 Euler { get; set; }

        /// <summary>
        /// Construct Matrix33 from three vectors.
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <param name="m3"></param>
        public Matrix33(Vector3 m1, Vector3 m2, Vector3 m3)
        {
            Rows[0, 0] = m1.X;
            Rows[0, 1] = m2.X;
            Rows[0, 2] = m3.X;
            Rows[1, 0] = m1.Y;
            Rows[1, 1] = m2.Y;
            Rows[1, 2] = m3.Y;
            Rows[2, 0] = m1.Z;
            Rows[2, 1] = m2.Z;
            Rows[2, 2] = m3.Z;
            Euler = ToEuler();
        }

        /// <summary>
        /// Constructs empty Matrix33.
        /// </summary>
        public Matrix33()
        {
            Rows[0, 0] = 1;
            Rows[0, 1] = 0;
            Rows[0, 2] = 0;
            Rows[1, 0] = 0;
            Rows[1, 1] = 1;
            Rows[1, 2] = 0;
            Rows[2, 0] = 0;
            Rows[2, 1] = 0;
            Rows[2, 2] = 1;
        }

        /// <summary>
        /// Write matrix to file.
        /// </summary>
        /// <param name="writer"></param>
        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(Rows[0, 0]);
            writer.Write(Rows[0, 1]);
            writer.Write(Rows[0, 2]);
            writer.Write(Rows[1, 0]);
            writer.Write(Rows[1, 1]);
            writer.Write(Rows[1, 2]);
            writer.Write(Rows[2, 0]);
            writer.Write(Rows[2, 1]);
            writer.Write(Rows[2, 2]);
        }

        /// <summary>
        /// Convert matrix to euler.
        /// </summary>
        /// <returns></returns>
        public Vector3 ToEuler()
        {
            double x;
            double z;
            double y = Math.Asin(Rows[0,2]);

            if(Math.Abs(Rows[0,2]) < 0.99999)
            {
                x = Math.Atan2(Rows[1, 1], Rows[2, 2]);
                z = Math.Atan2(Rows[0, 1], Rows[0, 2]);
            }
            else
            {
                x = Math.Atan2(Rows[2, 1], Rows[2, 1]);
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
            return $"{Euler}";
        }


    }
}
